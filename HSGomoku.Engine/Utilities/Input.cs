using System;

using Microsoft.Xna.Framework.Input;

namespace HSGomoku.Engine.Utilities
{
    public static class Input //Static classes can easily be accessed anywhere in our codebase. They always stay in memory so you should only do it for universal things like input.
    {
        private static KeyboardState _keyboardState = Keyboard.GetState();
        private static KeyboardState _lastKeyboardState;

        private static MouseState _mouseState;
        private static MouseState _lastMouseState;

        public static void Update()
        {
            _lastKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            _lastMouseState = _mouseState;
            _mouseState = Mouse.GetState();
        }

        #region Keyboard State

        /// <summary>
        /// Checks if key is currently pressed.
        /// </summary>
        public static Boolean IsKeyDown(Keys input)
        {
            return _keyboardState.IsKeyDown(input);
        }

        /// <summary>
        /// Checks if key is currently up.
        /// </summary>
        public static Boolean IsKeyUp(Keys input)
        {
            return _keyboardState.IsKeyUp(input);
        }

        #endregion Keyboard State

        #region Key Presses

        /// <summary>
        /// Checks if key was just pressed.
        /// </summary>
        public static Boolean KeyPressed(Keys input)
        {
            if (_keyboardState.IsKeyDown(input) == true && _lastKeyboardState.IsKeyDown(input) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Key Presses

        #region Mouse State

        /// <summary>
        /// Returns whether or not the left mouse button is being released
        /// </summary>
        public static Boolean MouseLeftUp()
        {
            return _mouseState.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// Returns whether or not the left mouse button is being pressed.
        /// </summary>
        public static Boolean MouseLeftDown()
        {
            return _mouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Returns whether or not the right mouse button is being released
        /// </summary>
        public static Boolean MouseRightUp()
        {
            return _mouseState.RightButton == ButtonState.Released;
        }

        /// <summary>
        /// Returns whether or not the right mouse button is being pressed.
        /// </summary>
        public static Boolean MouseRightDown()
        {
            return _mouseState.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Returns whether or not the middle mouse button is being released
        /// </summary>
        public static Boolean MouseMiddleUp()
        {
            return _mouseState.MiddleButton == ButtonState.Released;
        }

        /// <summary>
        /// Returns whether or not the middle mouse button is being pressed.
        /// </summary>
        public static Boolean MouseMiddleDown()
        {
            return _mouseState.MiddleButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Checks if the left mouse button was clicked.
        /// </summary>
        public static Boolean MouseLeftClicked()
        {
            if (_mouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the right mouse button was clicked.
        /// </summary>
        public static Boolean MouseRightClicked()
        {
            if (_mouseState.RightButton == ButtonState.Pressed && _lastMouseState.RightButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the middle mouse button was clicked.
        /// </summary>
        public static Boolean MouseMiddleClicked()
        {
            if (_mouseState.MiddleButton == ButtonState.Pressed && _lastMouseState.MiddleButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Mouse State
    }
}