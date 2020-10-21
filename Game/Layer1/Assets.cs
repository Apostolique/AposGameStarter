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
            using MemoryStream ms = new MemoryStream();
            TitleContainer.OpenStream($"{Content.RootDirectory}/Fonts/SourceCodePro-Medium.ttf").CopyTo(ms);
            byte[] fontBytes = ms.ToArray();

            FontSystem = FontSystemFactory.Create(graphicsDevice, fontBytes, 2048, 2048);
        }
        public static FontSystem FontSystem;
    }
}
