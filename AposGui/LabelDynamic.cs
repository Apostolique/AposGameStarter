using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace AposGameCheatSheet.AposGui
{
    class LabelDynamic : Label
    {
        public LabelDynamic() {
            text = delegate() {
                return "Text Missing";
            };
        }
        public LabelDynamic(Func<string> iText) {
            text = iText;
        }
        Func<string> text;
        Size2 textSize => Assets.bitFont.MeasureString(text());
        public override void Draw(SpriteBatch s, Rectangle clipRect) {
            Draw(s, clipRect, Color.White);
        }
        public override void Draw(SpriteBatch s, Rectangle clipRect, Color c) {
            clipRect = ClipRectangle(clipRect);

            int halfWidth = Width / 2;
            int textHalfWidth = PrefWidth / 2;

            int halfHeight = Height / 2;
            int textHalfHeight = PrefHeight / 2;

            s.DrawString(Assets.bitFont, text(), new Vector2(Left + halfWidth - textHalfWidth, Top + halfHeight - textHalfHeight), c, clipRect);
        }
        public override int PrefWidth => (int)textSize.Width;
        public override int PrefHeight => (int)textSize.Height;
    }
}
