using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject {
    static class Assets {
        public static void LoadAssets(ContentManager Content, GraphicsDevice graphicsDevice) {
            LoadFonts(Content);
        }
        public static void LoadFonts(ContentManager Content) {
            FontSystem = new();
            FontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/Fonts/source-code-pro-medium.ttf"));
        }
        public static FontSystem FontSystem;
    }
}
