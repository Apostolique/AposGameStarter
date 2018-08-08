using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AposGameStarter.AposGui
{
    /// <summary>
    /// Goal: Container that can hold Components.
    /// </summary>
    class PanelVerticalScroll : Panel
    {
        public PanelVerticalScroll() {
        }
        public override bool UpdateInput() {
            bool used = base.UpdateInput();
            bool isHovered = IsInsideClip(new Point(Input.NewMouse.X, Input.NewMouse.Y));
            
            if (!used && isHovered) {
                if (Input.NewMouse.ScrollWheelValue != Input.OldMouse.ScrollWheelValue) {
                    Offset = new Point(Offset.X, (int)Math.Min(Math.Max(Offset.Y + Input.NewMouse.ScrollWheelValue - Input.OldMouse.ScrollWheelValue, Height - Size.Height), 0));
                }
            }

            return isHovered || used;
        }
    }
}
