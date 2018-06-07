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
    /// Goal: Stacks components on top of each others.
    /// </summary>
    class LayoutVertical : Layout
    {
        public LayoutVertical() {
        }
        public override void RecomputeChildren(List<Component> childs) {
            //Tell each children their position and size.
            Point position = Panel.Position;
            int width = Panel.Width;
            int height = Panel.Height;

            int offsetY = position.Y;
            foreach (Component c in childs) {
                int cHeight = c.PrefHeight;
                c.Width = width;
                c.Height = cHeight;
                c.Position = new Point(position.X, offsetY) + Panel.Offset;
                offsetY += cHeight;
            }
            Panel.Size = new Size2(width, offsetY);
        }
    }
}
