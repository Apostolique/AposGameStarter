using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SpriteFontPlus;

namespace AposGameStarter
{
    static class Assets
    {
        public static void LoadAssets(ContentManager Content) {
            LoadFonts(Content);
        }
        public static void LoadFonts(ContentManager Content) {
            Font = DynamicSpriteFont.FromTtf(File.ReadAllBytes(Content.RootDirectory + "/Fonts/SourceCodePro-Medium.ttf"), 30);
            Font2 = Content.Load<SpriteFont>("Fonts/SourceCodePro");
        }
        public static DynamicSpriteFont Font;
        public static SpriteFont Font2;
    }
}