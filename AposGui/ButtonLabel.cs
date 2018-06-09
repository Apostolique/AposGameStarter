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
        public override void Draw(SpriteBatch s, Rectangle clipRect) {
            clipRect = ClipRectangle(clipRect);

            if (showBox) {
                if (isHovered || HasFocus) {
                    s.FillRectangle(new RectangleF(Left, Top, Width, Height), new Color(20, 20, 20));
                } else {
                    s.FillRectangle(new RectangleF(Left, Top, Width, Height), Color.Black);
                }
            }

            if (showBox || isHovered || HasFocus) {
                Item.Draw(s, clipRect);
            } else {
                ((Label)Item).Draw(s, clipRect, new Color(150, 150, 150));
            }

            if (showBox && (isHovered || HasFocus)) {
                s.DrawLine(Left, Top, Left, Bottom, Color.White, 2);
                s.DrawLine(Right, Top, Right, Bottom, Color.White, 2);
                s.DrawLine(Left, Top, Right, Top, Color.White, 2);
                s.DrawLine(Left, Bottom, Right, Bottom, Color.White, 2);
            }
        }
    }
}
