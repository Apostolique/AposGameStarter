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
    /// Goal: Handles how components are positioned inside a panel.
    /// </summary>
    class Layout
    {
        public Layout() {
        }
        public virtual Panel Panel {
            get; set;
        }
        public virtual void RecomputeChildren(List<Component> children) {
            //Tell each children their position and size.
        }
    }
}
