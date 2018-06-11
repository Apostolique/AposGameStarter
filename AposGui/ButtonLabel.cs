using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AposGameCheatSheet.AposGui
{
    /// <summary>
    /// Goal: A button that contains some text.
    /// </summary>
    class ButtonLabel : Button
    {
        public ButtonLabel() {
            Item = new Label();
        }
        public ButtonLabel(string text) {
            Item = new Label(text);
        }
        public ButtonLabel(Label text) {
            Item = text;
        }
        public override void Draw(SpriteBatch s) {
            if (ShowBox) {
                if (IsHovered || HasFocus) {
                    s.FillRectangle(new RectangleF(Left, Top, Width, Height), new Color(20, 20, 20));
                } else {
                    s.FillRectangle(new RectangleF(Left, Top, Width, Height), Color.Black);
                }
            }

            if (ShowBox || IsHovered || HasFocus) {
                Item.Draw(s);
            } else {
                ((Label)Item).Draw(s, new Color(150, 150, 150));
            }

            if (ShowBox && (IsHovered || HasFocus)) {
                s.DrawLine(Left, Top, Left, Bottom, Color.White, 2);
                s.DrawLine(Right, Top, Right, Bottom, Color.White, 2);
                s.DrawLine(Left, Top, Right, Top, Color.White, 2);
                s.DrawLine(Left, Bottom, Right, Bottom, Color.White, 2);
            }
        }
    }
}
