using Microsoft.Xna.Framework;

namespace AposGameStarter
{
    static class Utility
    {
        public static Core game;
        public static GameWindow Window;

        public static int WindowWidth => Window.ClientBounds.Width;
        public static int WindowHeight => Window.ClientBounds.Height;

        public static bool showLine = false;
    }
}