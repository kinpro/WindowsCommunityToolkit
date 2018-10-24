﻿// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class CompositionGeometricClip : CompositionClip
    {
        internal CompositionGeometricClip() { }

        public CompositionGeometry Geometry { get; set; }

        public override CompositionObjectType Type => CompositionObjectType.CompositionGeometricClip;
    }
}
