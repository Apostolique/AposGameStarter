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
    }
}
