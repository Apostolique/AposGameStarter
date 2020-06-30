using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SpriteFontPlus;

namespace GameProject {
    static class Assets {
        public static void LoadAssets(ContentManager Content) {
            LoadFonts(Content);
        }
        public static void LoadFonts(ContentManager Content) {
            using MemoryStream ms = new MemoryStream();
            TitleContainer.OpenStream($"{Content.RootDirectory}/Fonts/SourceCodePro-Medium.ttf").CopyTo(ms);
            byte[] fontBytes = ms.ToArray();

            Font = DynamicSpriteFont.FromTtf(fontBytes, 30);
        }
        public static DynamicSpriteFont Font;
    }
}
