using System;

using Microsoft.Xna.Framework;

namespace HSGomoku.Engine.Utilities
{
    internal sealed class Statistics
    {
        #region Game State

        public enum GameState
        {
            Menu,
            Loading,
            Playing,
        }

        public static GameState CurrentGameState { get; set; }

        public enum PlayerState
        {
            None,
            Black,
            White
        }

        public static PlayerState CurrentPlayerState { get; set; } = PlayerState.None;

        public static Vector2 LastChessPosition { get; set; }

        #endregion Game State

        #region Frame Rate

        public enum GameFrameRate
        {
            F30 = 30,
            F60 = 60,
            F120 = 120
        }

        public static GameFrameRate CurrentGameFrameRate { get; set; }

        public static Single SpeedMultiply
        {
            get
            {
                switch (CurrentGameFrameRate)
                {
                    case GameFrameRate.F60:
                        return 1.0f;

                    case GameFrameRate.F120:
                        return 0.5f;

                    case GameFrameRate.F30:
                        return 2f;

                    default:
                        return 1.0f;
                }
            }
        }

        #endregion Frame Rate

        #region Resolution

        public struct SupportResolution
        {
            public static Vector2 P1440
            {
                get
                {
                    return new Vector2(1920, 1440);
                }
            }

            public static Vector2 P1080
            {
                get
                {
                    return new Vector2(1440, 1080);
                }
            }

            public static Vector2 P960
            {
                get
                {
                    return new Vector2(1280, 960);
                }
            }

            public static Vector2 P720
            {
                get
                {
                    return new Vector2(960, 720);
                }
            }

            public static Vector2 P600
            {
                get
                {
                    return new Vector2(800, 600);
                }
            }
        }

        public static Vector2 CurrentResolution { get; set; }

        #endregion Resolution
    }
}