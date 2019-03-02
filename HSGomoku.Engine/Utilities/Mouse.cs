using System;

using Microsoft.Xna.Framework.Input;

namespace HSGomoku.Engine.Utilities
{
    internal static class Mouse
    {
        public static MouseState GetState()
        {
            var originMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            var calcMouseState = new MouseState(
                (Int32)(originMouseState.X / Resolution.ScreenScale.X),
                (Int32)(originMouseState.Y / Resolution.ScreenScale.Y),
                originMouseState.ScrollWheelValue,
                originMouseState.LeftButton,
                originMouseState.MiddleButton,
                originMouseState.RightButton,
                originMouseState.XButton1,
                originMouseState.XButton2);

            return calcMouseState;
        }
    }
}