using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.BitmapFonts;

namespace AposGameCheatSheet
{
    static class Assets
    {
        public static void LoadAssets(ContentManager Content) {
            LoadFonts(Content);
        }
        public static void LoadFonts(ContentManager Content) {
            bitFont = Content.Load<BitmapFont>("Fonts/SourceCodeProMedium");
        }
        public static BitmapFont bitFont;
    }
}