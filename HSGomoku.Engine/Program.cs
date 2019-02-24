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

            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
}