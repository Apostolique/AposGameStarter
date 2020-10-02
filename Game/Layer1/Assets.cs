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
            Font = DynamicSpriteFont.FromTtf(TitleContainer.OpenStream($"{Content.RootDirectory}/Fonts/SourceCodePro-Medium.ttf"), 30);
        }
        public static DynamicSpriteFont Font;
    }
}
