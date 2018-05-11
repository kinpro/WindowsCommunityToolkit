﻿using Compositions;
using Lottie;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace LottieViewer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (!e.PrelaunchActivated)
            {
                // Ensure the current window is active
                Window.Current.Activate();

                // Run the splash screen animation.
                await StartAnimatedSplashScreenAsync();

                // Start navigation to the first page.
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (titleBar != null)
            {
                var backgroundColor = (SolidColorBrush)Current.Resources["BackgroundBrush"];
                var foregroundColor = (SolidColorBrush)Current.Resources["ForegroundBrush"];
                var inactiveBackgroundColor = (SolidColorBrush)Current.Resources["ToolsBackgroundBrush"];
                titleBar.ButtonBackgroundColor = backgroundColor.Color;
                titleBar.ButtonForegroundColor = foregroundColor.Color;
                titleBar.BackgroundColor = backgroundColor.Color;
                titleBar.ForegroundColor = foregroundColor.Color;

                titleBar.InactiveBackgroundColor = backgroundColor.Color;
                titleBar.InactiveForegroundColor = foregroundColor.Color;

                titleBar.ButtonInactiveBackgroundColor = backgroundColor.Color;
                titleBar.ButtonInactiveForegroundColor = foregroundColor.Color;
            }
            base.OnWindowCreated(args);
        }


        // Starts the animated splash screen as content for the current window. The
        // returned Task completes when the animation finishes.
        async Task StartAnimatedSplashScreenAsync()
        {
            // Insert splashGrid above the current window content.
            var originalWindowContent = Window.Current.Content;
            var splashGrid = new Grid();
            Window.Current.Content = splashGrid;

            var compositionPlayer = new CompositionPlayer
            {
                Stretch = Stretch.UniformToFill,
                AutoPlay = false,
                IsLoopingEnabled = false,
                FromProgress = 0,
                ToProgress = 0.595,
                Source = new LottieLogo()
            };

            splashGrid.Children.Add(originalWindowContent);
            splashGrid.Children.Add(compositionPlayer);

            // Start playing.
            await compositionPlayer.PlayAsync();

            // Fade out the splash screen
            var storyboard = new Storyboard();
            storyboard.Children.Add(new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.15),
            });
            Storyboard.SetTargetProperty(storyboard, "Opacity");
            Storyboard.SetTarget(storyboard, compositionPlayer);
            storyboard.Begin();
            storyboard.Completed += (sender, e)
                =>
            {
                // Restore the original content.

                // TODO  - simply moving the original content back causes
                //         the CompositionPlayer on the Page to disappear. Why?
                //splashGrid.Children.Clear();
                //Window.Current.Content = originalWindowContent;

                splashGrid.Children.Remove(compositionPlayer);
            };
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
