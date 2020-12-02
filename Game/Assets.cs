using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject {
    static class Assets {
        public static void LoadAssets(ContentManager Content, GraphicsDevice graphicsDevice) {
            LoadFonts(Content, graphicsDevice);
        }
        public static void LoadFonts(ContentManager Content, GraphicsDevice graphicsDevice) {
            FontSystem = FontSystemFactory.Create(graphicsDevice, 2048, 2048);
            FontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/Fonts/SourceCodePro-Medium.ttf"));
        }
        public static FontSystem FontSystem;
    }
}
