//////////////////////////////////////////////////////////////////////////
////License:  The MIT License (MIT)
////Copyright (c) 2010 David Amador (http://www.david-amador.com)
////
////Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
////
////The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
////
////THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//////////////////////////////////////////////////////////////////////////

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HSGomoku.Engine.Utilities
{
    internal static class Resolution
    {
        private static GraphicsDeviceManager _Device = null;

        private static Int32 _Width = 1024;
        private static Int32 _Height = 768;
        private static Int32 _VWidth = 1024;
        private static Int32 _VHeight = 768;
        private static Matrix _ScaleMatrix;
        private static Boolean _FullScreen = false;
        private static Boolean _dirtyMatrix = true;

        public static void Init(ref GraphicsDeviceManager device)
        {
            _Width = device.PreferredBackBufferWidth;
            _Height = device.PreferredBackBufferHeight;
            _Device = device;
            _dirtyMatrix = true;
            ApplyResolutionSettings();
        }

        public static Matrix GetTransformationMatrix()
        {
            if (_dirtyMatrix)
            {
                RecreateScaleMatrix();
            }

            return _ScaleMatrix;
        }

        public static void SetResolution(Int32 Width, Int32 Height, Boolean FullScreen)
        {
            _Width = Width;
            _Height = Height;

            _FullScreen = FullScreen;

            ApplyResolutionSettings();
        }

        public static void SetVirtualResolution(Int32 Width, Int32 Height)
        {
            _VWidth = Width;
            _VHeight = Height;

            _dirtyMatrix = true;
        }

        private static void ApplyResolutionSettings()
        {
#if XBOX360
           _FullScreen = true;
#endif

            // If we aren't using a full screen mode, the height and width of the window can be set
            // to anything equal to or smaller than the actual screen size.
            if (_FullScreen == false)
            {
                if ((_Width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    && (_Height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    _Device.PreferredBackBufferWidth = _Width;
                    _Device.PreferredBackBufferHeight = _Height;
                    _Device.IsFullScreen = _FullScreen;
                    _Device.ApplyChanges();
                }
            }
            else
            {
                // If we are using full screen mode, we should check to make sure that the display
                // adapter can handle the video mode we are trying to set. To do this, we will
                // iterate through the display modes supported by the adapter and check them against
                // the mode we want to set.
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    // Check the width and height of each mode against the passed values
                    if ((dm.Width == _Width) && (dm.Height == _Height))
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        _Device.PreferredBackBufferWidth = _Width;
                        _Device.PreferredBackBufferHeight = _Height;
                        _Device.IsFullScreen = _FullScreen;
                        _Device.ApplyChanges();
                    }
                }
            }

            _dirtyMatrix = true;

            _Width = _Device.PreferredBackBufferWidth;
            _Height = _Device.PreferredBackBufferHeight;
        }

        /// <summary>
        /// Sets the device to use the draw pump Sets correct aspect ratio
        /// </summary>
        public static void BeginDraw()
        {
            // Start by reseting viewport to (0,0,1,1)
            FullViewport();
            // Clear to Black
            _Device.GraphicsDevice.Clear(Color.Black);
            // Calculate Proper Viewport according to Aspect Ratio
            ResetViewport();
            // and clear that This way we are gonna have black bars if aspect ratio requires it and
            // the clear color on the rest
            _Device.GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        private static void RecreateScaleMatrix()
        {
            _dirtyMatrix = false;
            _ScaleMatrix = Matrix.CreateScale(
                           (Single)_Device.GraphicsDevice.Viewport.Width / _VWidth,
                           (Single)_Device.GraphicsDevice.Viewport.Width / _VWidth,
                           1f);
        }

        public static void FullViewport()
        {
            Viewport vp = new Viewport();
            vp.X = vp.Y = 0;
            vp.Width = _Width;
            vp.Height = _Height;
            _Device.GraphicsDevice.Viewport = vp;
        }

        /// <summary>
        /// Get virtual Mode Aspect Ratio
        /// </summary>
        /// <returns>aspect ratio</returns>
        public static Single GetVirtualAspectRatio()
        {
            return _VWidth / (Single)_VHeight;
        }

        public static void ResetViewport()
        {
            Single targetAspectRatio = GetVirtualAspectRatio();
            // figure out the largest area that fits in this resolution at the desired aspect ratio
            Int32 width = _Device.PreferredBackBufferWidth;
            Int32 height = (Int32)(width / targetAspectRatio + .5f);
            Boolean changed = false;

            if (height > _Device.PreferredBackBufferHeight)
            {
                height = _Device.PreferredBackBufferHeight;
                // PillarBox
                width = (Int32)(height * targetAspectRatio + .5f);
                changed = true;
            }

            // set up the new viewport centered in the backbuffer
            Viewport viewport = new Viewport();

            viewport.X = (_Device.PreferredBackBufferWidth / 2) - (width / 2);
            viewport.Y = (_Device.PreferredBackBufferHeight / 2) - (height / 2);
            viewport.Width = width;
            viewport.Height = height;
            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            if (changed)
            {
                _dirtyMatrix = true;
            }

            _Device.GraphicsDevice.Viewport = viewport;
        }
    }
}