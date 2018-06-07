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
    /// Goal: The ScreenPanel is always the same size as the Window.
    /// </summary>
    class ScreenPanel : Panel
    {
        public ScreenPanel() {
            Position = new Point(0, 0);
        }
        public override int Width => Utility.WindowWidth;
        public override int Height => Utility.WindowHeight;
    }
}
