using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace AposGameCheatSheet.AposGui
{
    /// <summary>
    /// Goal: Stacks components on top of each others and centers them inside the panel.
    /// </summary>
    class LayoutVerticalCenter : Layout
    {
        public LayoutVerticalCenter() {
        }
        public override void RecomputeChildren(List<Component> children) {
            //Tell each children their position and size.
            Point position = Panel.Position;
            int width = Panel.Width;
            int height = Panel.Height;
            int halfWidth = width / 2;
            int halfHeight = height / 2;

            int canvasHeight = 0;
            foreach (Component c in children) {
                int cMarginHeight = c.PrefHeight;
                canvasHeight += cMarginHeight;
            }

            int canvasOffsetY = halfHeight - canvasHeight / 2;
            int offsetY = position.Y;
            foreach (Component c in children) {
                int cWidth = c.PrefWidth;
                int componentHalfWidth = cWidth / 2;
                int cHeight = c.PrefHeight;
                c.Width = cWidth;
                c.Height = cHeight;
                if (canvasHeight < height) {
                    c.Position = new Point(position.X + halfWidth - componentHalfWidth, offsetY + canvasOffsetY) + Panel.Offset;
                } else {
                    c.Position = new Point(position.X + halfWidth - componentHalfWidth, offsetY) + Panel.Offset;
                }
                offsetY += cHeight;
            }
            Panel.Size = new Size2(width, offsetY);
        }
    }
}
