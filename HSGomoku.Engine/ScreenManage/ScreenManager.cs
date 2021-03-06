﻿/************************************************************************/
/* Author : David Amador
 * Web:      http://www.david-amador.com
 * Twitter : http://www.twitter.com/DJ_Link
 *
 * You can use this for whatever you want. If you want to give me some credit for it that's cool but not mandatory
/************************************************************************/

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace HSGomoku.Engine.ScreenManage
{
    /// <summary>
    /// Screen Manager Keeps a list of available screens so you can switch between them, ie. jumping
    /// from the start screen to the game screen
    /// </summary>
    internal static class ScreenManager
    {
        // Protected Members
        private static List<Screen> _screens = new List<Screen>();

        private static Boolean _started = false;
        private static Screen _previous = null;

        public static Screen ActiveScreen { get; set; } = null;

        /// <summary>
        /// Add new Screen
        /// </summary>
        /// <param name="screen">New screen, name must be unique</param>
        public static void AddScreen(Screen screen)
        {
            foreach (Screen scr in _screens)
            {
                if (scr.Name == screen.Name)
                {
                    return;
                }
            }
            _screens.Add(screen);
        }

        public static Int32 GetScreenNumber()
        {
            return _screens.Count;
        }

        public static Screen GetScreen(Int32 idx)
        {
            return _screens[idx];
        }

        /// <summary>
        /// Go to screen
        /// </summary>
        /// <param name="name">Screen name</param>
        public static void GotoScreen(String name)
        {
            if (ActiveScreen != null && ActiveScreen.Name == name)
            {
                return;
            }
            foreach (Screen screen in _screens)
            {
                if (screen.Name == name)
                {
                    // Shutsdown Previous Screen
                    _previous = ActiveScreen;
                    if (ActiveScreen != null)
                    {
                        ActiveScreen.Shutdown();
                    }
                    // Inits New Screen
                    ActiveScreen = screen;
                    if (_started)
                    {
                        ActiveScreen.Init();
                        ActiveScreen.LoadContent();
                    }

                    return;
                }
            }
        }

        /// <summary>
        /// Init Screen manager Only at this point is screen manager going to init the selected screen
        /// </summary>
        public static void Init()
        {
            _started = true;
            if (ActiveScreen != null)
            {
                ActiveScreen.Init();
            }
        }

        /// <summary>
        /// Falls back to previous selected screen if any
        /// </summary>
        public static void GoBack()
        {
            if (_previous != null)
            {
                GotoScreen(_previous.Name);
                return;
            }
        }

        public static void LoadContent()
        {
            if (_started == false)
            {
                return;
            }

            if (ActiveScreen != null)
            {
                ActiveScreen.LoadContent();
            }
        }

        /// <summary>
        /// Updates Active Screen
        /// </summary>
        /// <param name="elapsed">GameTime</param>
        public static void Update(GameTime gameTime)
        {
            if (_started == false)
            {
                return;
            }

            if (ActiveScreen != null)
            {
                ActiveScreen.Update(gameTime);
            }
        }

        public static void Draw(GameTime gameTime)
        {
            if (_started == false)
            {
                return;
            }

            if (ActiveScreen != null)
            {
                ActiveScreen.Draw(gameTime);
            }
        }
    }
}