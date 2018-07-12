// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

#if DEBUG
// Uncomment this to slow down async awaits for testing.
//#define SlowAwaits
#endif

// Comment this out to use the old Windows.Data.Json parser.
//#define USE_NEWTONSOFT_PARSING

using LottieData;
using LottieData.Serialization;
using LottieToWinComp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Xaml;
using System.Numerics;
using Windows.UI.Composition;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft_UI_Xaml_Controls;

namespace Lottie
{
    /// <summary>
    /// A <see cref="CompositionSource"/> for a Lottie composition. This allows
    /// a Lottie to be specified as the source of a <see cref="Composition"/>.
    /// </summary>
    public sealed class LottieCompositionSource : IDynamicCompositionSource
    {
        readonly StorageFile _storageFile;
        EventRegistrationTokenTable<DynamicCompositionSourceEventHandler> _compositionInvalidatedEventTokenTable;
        int _loadVersion;
        Uri _uriSource;
        ContentFactory _contentFactory;

        /// <summary>
        /// Constructor to allow a <see cref="LottieCompositionSource"/> to be used in markup.
        /// </summary>
        public LottieCompositionSource() { }

        /// <summary>
        /// Creates a <see cref="CompositionSource"/> from a <see cref="StorageFile"/>.
        /// </summary>
        public LottieCompositionSource(StorageFile storageFile)
        {
            _storageFile = storageFile;
        }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of the JSON source file for this <see cref="LottieCompositionSource"/>.
        /// </summary>
        public Uri UriSource
        {
            get => _uriSource;
            set
            {
                if (_uriSource == value)
                {
                    return;
                }
                _uriSource = value;
                StartLoading();
            }
        }

        public LottieCompositionOptions Options { get; set; }

        /// <summary>
        /// Called by XAML to convert a string to an <see cref="ICompositionSource"/>.
        /// </summary>
        public static LottieCompositionSource CreateFromString(string uri)
        {
            var uriUri = StringToUri(uri);
            if (uriUri == null)
            {
                // TODO - throw?
                return null;
            }
            return new LottieCompositionSource { UriSource = uriUri };
        }

        // TODO: accept IRandomAccessStream
        [DefaultOverload]
        public IAsyncAction SetSourceAsync(StorageFile file)
        {
            _uriSource = null;
            return LoadAsync(file == null ? null : new Loader(file)).AsAsyncAction();
        }

        public IAsyncAction SetSourceAsync(Uri sourceUri)
        {
            _uriSource = sourceUri;
            return LoadAsync(sourceUri == null ? null : new Loader(sourceUri)).AsAsyncAction();
        }

        event DynamicCompositionSourceEventHandler IDynamicCompositionSource.CompositionInvalidated
        {
            add
            {
                return EventRegistrationTokenTable<DynamicCompositionSourceEventHandler>
                   .GetOrCreateEventRegistrationTokenTable(ref _compositionInvalidatedEventTokenTable)
                   .AddEventHandler(value);
            }

            remove
            {
                EventRegistrationTokenTable<DynamicCompositionSourceEventHandler>
                   .GetOrCreateEventRegistrationTokenTable(ref _compositionInvalidatedEventTokenTable)
                    .RemoveEventHandler(value);
            }
        }

        bool ICompositionSource.TryCreateInstance(
            Compositor compositor,
            out Visual rootVisual,
            out Vector2 size,
            out TimeSpan duration,
            out object diagnostics)
        {
            if (_contentFactory == null)
            {
                rootVisual = null;
                size = default(Vector2);
                duration = default(TimeSpan);
                diagnostics = null;
                return false;
            }
            else
            {
                return _contentFactory.TryCreateInstance(
                    compositor,
                    out rootVisual,
                    out size,
                    out duration,
                    out diagnostics);
            }
        }

        void NotifyListenersThatCompositionChanged()
        {
            EventRegistrationTokenTable<DynamicCompositionSourceEventHandler>
                .GetOrCreateEventRegistrationTokenTable(ref _compositionInvalidatedEventTokenTable)
                .InvocationList?.Invoke(this);
        }

        // Starts a LoadAsync and returns immediately.
        async void StartLoading() => await LoadAsync(new Loader(UriSource));

        // Starts loading. Completes the returned task when the load completes or is replaced by another
        // load.
        async Task LoadAsync(Loader loader)
        {
            var loadVersion = ++_loadVersion;

            var oldContentFactory = _contentFactory;
            _contentFactory = null;

            if (oldContentFactory != null)
            {
                // Notify all listeners that their existing content is no longer valid.
                // They should stop showing the content. We will notify them again if the load
                // succeeds.
                NotifyListenersThatCompositionChanged();
            }

            if (loader == null)
            {
                return;
            }

            var contentFactory = await loader.Load(Options);
            if (loadVersion != _loadVersion)
            {
                // Another load request came in before this one completed.
                return;
            }

            if (contentFactory == null)
            {
                // Load didn't produce anything.
                return;
            }

            // We are the the most recent load. Save the result.
            _contentFactory = contentFactory;

            if (contentFactory.CanInstantiate)
            {
                // Notify all listeners that they should create their instance of the content again.
                NotifyListenersThatCompositionChanged();
            }
            else
            {
                // The load failed. Throw an exception so the caller knows.
                throw new ArgumentException("Failed to load composition.");
            }
        }

        static Issue[] ToIssues(IEnumerable<(string Code, string Description)> issues)
            => issues.Select(issue => new Issue { Code = issue.Code, Description = issue.Description }).ToArray();

        // Handles loading a composition from a Lottie file.
        sealed class Loader
        {
            readonly Uri _uri;
            readonly StorageFile _storageFile;

            internal Loader(Uri uri) { _uri = uri; }

            internal Loader(StorageFile storageFile) { _storageFile = storageFile; }

            // Null loader.
            internal Loader() { }

            // Asynchronously loads WinCompData from a Lottie file.
            public async Task<ContentFactory> Load(LottieCompositionOptions Options)
            {
                if (_uri == null && _storageFile == null)
                {
                    // Request to load null. Return a null ContentFactory.
                    return null;
                }

                LottieCompositionDiagnostics diagnostics = null;
                Stopwatch sw = null;
                if (Options.HasFlag(LottieCompositionOptions.IncludeDiagnostics))
                {
                    sw = Stopwatch.StartNew();
                    diagnostics = new LottieCompositionDiagnostics
                    {
                        Options = Options
                    };
                }

                var result = new ContentFactory(diagnostics);

                // Get the file name and contents.
                (var fileName, var jsonString) = await ReadFileAsync();

                if (diagnostics != null)
                {
                    diagnostics.FileName = fileName;
                    diagnostics.ReadTime = sw.Elapsed;
                    sw.Restart();
                }

                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    // Failed to load ...
                    return result;
                }


                // Parsing large Lottie files can take significant time. Do it on
                // another thread.
                LottieData.LottieComposition lottieComposition = null;
                await CheckedAwait(Task.Run(() =>
                {
                    lottieComposition =
#if USE_NEWTONSOFT_PARSING
                        LottieData.Serialization.Net.LottieCompositionReader.ReadLottieCompositionFromJsonString(
                            jsonString,
                            LottieData.Serialization.Net.LottieCompositionReader.Options.IgnoreMatchNames,
#else
                        LottieCompositionJsonReader.ReadLottieCompositionFromJsonString(
                            jsonString,
                            LottieCompositionJsonReader.Options.IgnoreMatchNames,
#endif
                            out var readerIssues);

                    if (diagnostics != null)
                    {
                        diagnostics.JsonParsingIssues = ToIssues(readerIssues);
                    }
                }));

                if (diagnostics != null)
                {
                    diagnostics.ParseTime = sw.Elapsed;
                    sw.Restart();
                }

                if (lottieComposition == null)
                {
                    // Failed to load...
                    return result;
                }

                if (diagnostics != null)
                {
                    // Save the LottieComposition in the diagnostics so that the xml and codegen
                    // code can be derived from it.
                    diagnostics.LottieComposition = lottieComposition;

                    // For each marker, normalize to a progress value by subtracting the InPoint (so it is relative to the start of the animation)
                    // and dividing by OutPoint - InPoint
                    diagnostics.Markers = lottieComposition.Markers.Select(m =>
                    {
                        return new KeyValuePair<string, double>(m.Name, (m.Progress * lottieComposition.FramesPerSecond) / lottieComposition.Duration.TotalSeconds);
                    }).ToArray();

                    // Validate the composition and report if issues are found.
                    diagnostics.LottieValidationIssues = ToIssues(LottieCompositionValidator.Validate(lottieComposition));
                    diagnostics.ValidationTime = sw.Elapsed;
                    sw.Restart();
                }

                result.SetDimensions(width: lottieComposition.Width,
                                     height: lottieComposition.Height,
                                     duration: lottieComposition.Duration);


                // Translating large Lotties can take significant time. Do it on another thread.
                bool translateSucceeded = false;
                WinCompData.Visual wincompDataRootVisual = null;
                await CheckedAwait(Task.Run(() =>
                {
                    translateSucceeded = LottieToWinCompTranslator.TryTranslateLottieComposition(
                        lottieComposition,
                        false, // strictTranslation
                        true,  // Add descriptions for codegen comments
                        out wincompDataRootVisual,
                        out var translationIssues);

                    if (diagnostics != null)
                    {
                        diagnostics.TranslationIssues = ToIssues(translationIssues);
                    }
                }));

                if (diagnostics != null)
                {
                    diagnostics.TranslationTime = sw.Elapsed;
                    sw.Restart();
                }

                if (!translateSucceeded)
                {
                    // Failed.
                    return result;
                }
                else
                {
                    if (diagnostics != null)
                    {
                        // Save the root visual so diagnostics can generate XML and codegen.
                        diagnostics.RootVisual = wincompDataRootVisual;
                    }
                    result.SetRootVisual(wincompDataRootVisual);
                    return result;
                }
            }

            Task<(string, string)> ReadFileAsync()
                    => _storageFile != null
                        ? ReadStorageFileAsync(_storageFile)
                        : ReadUriAsync(_uri);

            async Task<(string, string)> ReadUriAsync(Uri uri)
            {
                var absoluteUri = GetAbsoluteUri(uri);
                if (absoluteUri != null)
                {
                    if (absoluteUri.Scheme.StartsWith("ms-"))
                    {
                        return await ReadStorageFileAsync(await StorageFile.GetFileFromApplicationUriAsync(absoluteUri));
                    }
                    else
                    {
                        var winrtClient = new Windows.Web.Http.HttpClient();
                        var response = await winrtClient.GetAsync(absoluteUri);
                        var result = await response.Content.ReadAsStringAsync();
                        return (absoluteUri.LocalPath, result);
                    }
                }
                return (null, null);
            }

            async Task<(string, string)> ReadStorageFileAsync(StorageFile storageFile)
            {
                Debug.Assert(storageFile != null);
                var result = await FileIO.ReadTextAsync(storageFile);
                return (storageFile.Name, result);
            }
        }

        // Parses a string into an absolute URI, or null if the string is malformed.
        static Uri StringToUri(string uri)
        {
            if (!Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute))
            {
                return null;
            }

            return GetAbsoluteUri(new Uri(uri, UriKind.RelativeOrAbsolute));
        }

        // Returns an absolute URI. Relative URIs are made relative to ms-appx:///
        static Uri GetAbsoluteUri(Uri uri)
        {
            if (uri == null)
            {
                return null;
            }

            if (uri.IsAbsoluteUri)
            {
                return uri;
            }

            return new Uri($"ms-appx:///{uri}", UriKind.Absolute);
        }

        // Information from which a composition's content can be instantiated. Contains the WinCompData
        // translation of a composition and some metadata.
        sealed class ContentFactory : ICompositionSource
        {
            readonly LottieCompositionDiagnostics _diagnostics;
            WinCompData.Visual _wincompDataRootVisual;
            double _width;
            double _height;
            TimeSpan _duration;

            internal ContentFactory(LottieCompositionDiagnostics diagnostics)
            {
                _diagnostics = diagnostics;
            }

            internal void SetDimensions(double width, double height, TimeSpan duration)
            {
                _width = width;
                _height = height;
                _duration = duration;
            }

            internal void SetRootVisual(WinCompData.Visual rootVisual)
            {
                _wincompDataRootVisual = rootVisual;
            }

            internal bool CanInstantiate => _wincompDataRootVisual != null;

            public bool TryCreateInstance(
                Compositor compositor,
                out Visual rootVisual,
                out Vector2 size,
                out TimeSpan duration,
                out object diagnostics)
            {
                LottieCompositionDiagnostics diags = _diagnostics != null ? _diagnostics.Clone() : null;
                diagnostics = diags;

                if (!CanInstantiate)
                {
                    rootVisual = null;
                    size = default(Vector2);
                    duration = default(TimeSpan);
                    return false;
                }
                else
                {
                    var sw = Stopwatch.StartNew();

                    rootVisual = Instantiator.CreateVisual(compositor, _wincompDataRootVisual);
                    size = new Vector2((float)_width, (float)_height);
                    duration = _duration;

                    if (diags != null)
                    {
                        diags.InstantiationTime = sw.Elapsed;
                    }
                    return true;
                }
            }
        }

        public override string ToString()
        {
            // TODO - if there's a _contentFactory, it should store the identity and report here
            var identity = (_storageFile != null) ? _storageFile.Name : _uriSource?.ToString() ?? "";
            return $"LottieCompositionSource({identity})";
        }

#region DEBUG
        // For testing purposes, slows down a task.
#if SlowAwaits
        const int _checkedDelayMs = 5;
        async
#endif
        static Task CheckedAwait(Task task)
        {
#if SlowAwaits
            await Task.Delay(_checkedDelayMs);
            await task;
            await Task.Delay(_checkedDelayMs);
#else
            return task;
#endif
        }

#endregion DEBUG
    }
}

