using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace AposGameCheatSheet.AposGui
{
    /// <summary>
    /// Goal: A text component.
    /// </summary>
    class Label : Component
    {
        string text;
        Size2 textSize;
        public Label() : this("Text Missing") {
        }
        public Label(string iText) {
            text = iText;
            textSize = Assets.bitFont.MeasureString(text);
            Width = PrefWidth;
            Height = PrefHeight;
        }
        public override void Draw(SpriteBatch s) {
            Draw(s, Color.White);
        }
        public virtual void Draw(SpriteBatch s, Color c) {
            int halfWidth = Width / 2;
            int textHalfWidth = PrefWidth / 2;

            int halfHeight = Height / 2;
            int textHalfHeight = PrefHeight / 2;

            s.DrawString(Assets.bitFont, text, new Vector2(Left + halfWidth - textHalfWidth, Top + halfHeight - textHalfHeight), c, ClippingRect);
        }
        public override int PrefWidth => (int)textSize.Width;
        public override int PrefHeight => (int)textSize.Height;
    }
}
