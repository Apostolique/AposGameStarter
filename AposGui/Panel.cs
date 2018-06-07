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
    /// Goal: Container that can hold Components.
    /// </summary>
    class Panel : Component
    {
        public Panel() {
            children = new List<Component>();
            Layout = new Layout();
            Offset = new Point(0, 0);
            Size = new Size2(0, 0);
        }
        List<Component> children;
        public Point Offset {
            get; set;
        }
        public Size2 Size {
            get; set;
        }
        private Layout _layout;
        public Layout Layout {
            get => _layout;
            set {
                _layout = value;
                _layout.Panel = this;
            }
        }

        public virtual void Add(Component e) {
            children.Add(e);
        }
        public virtual void Remove(Component e) {
            children.Remove(e);
        }
        public override void UpdateSetup() {
            if (Layout != null) {
                Layout.RecomputeChildren(children);
            }
            foreach (Component e in children) {
                e.UpdateSetup();
            }
        }
        public override bool UpdateInput() {
            bool usedInput = base.UpdateInput();
            foreach (Component e in children) {
                usedInput = e.UpdateInput() || usedInput;
            }
            return usedInput;
        }
        public override void Update() {
            base.Update();
            foreach (Component e in children) {
                e.Update();
            }
        }
        public override void Draw(SpriteBatch s, Rectangle clipRect) {
            clipRect = ClipRectangle(clipRect);
            foreach (Component e in children) {
                e.Draw(s, clipRect);
            }
        }
    }
}
