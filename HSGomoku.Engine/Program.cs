using System;

namespace HSGomoku.Engine
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(String[] args)
        {
            // Enable HIDPI
            Environment.SetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI", "1");

            // Enable DirectX
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Environment.SetEnvironmentVariable("FNA_OPENGL_FORCE_ES3", "1");
                Environment.SetEnvironmentVariable("SDL_OPENGL_ES_DRIVER", "1");
            }

            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
}