using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace AposGameStarter.AposGui
{
    /// <summary>
    /// Goal: A checkbox component that handles actions.
    /// </summary>
    class CheckBox : Component
    {
        public CheckBox() : this(new Component()) {
        }
        public CheckBox(Component iItem) {
            Item = iItem;
            checkBoxActions = new List<CheckBoxConditionAction>();
            IsFocusable = true;
            OldIsHovered = false;
            IsHovered = false;
            ShowBox = true;
        }
        protected struct CheckBoxConditionAction {
            public Func<CheckBox, bool> condition;
            public Action<CheckBox> checkBoxAction;
            public CheckBoxConditionAction(Func<CheckBox, bool> iCondition, Action<CheckBox> iCheckBoxAction) {
                condition = iCondition;
                checkBoxAction = iCheckBoxAction;
            }
        }
        public virtual Component Item {
            get; set;
        }
        public virtual bool OldIsHovered {
            get; set;
        }
        public virtual bool IsHovered {
            get; set;
        }
        public virtual bool ShowBox {
            get; set;
        }
        protected List<CheckBoxConditionAction> checkBoxActions;

        public override Point Position {
            get => base.Position;
            set {
                base.Position = value;
                if (Item != null) {
                    Item.Position = base.Position;
                }
            }
        }
        public override int Width {
            get => base.Width;
            set {
                base.Width = value;
                if (Item != null) {
                    Item.Width = base.Width;
                }
            }
        }
        public override int Height {
            get => base.Height;
            set {
                base.Height = value;
                if (Item != null) {
                    Item.Height = base.Height;
                }
            }
        }
        public override Rectangle ClippingRect {
            get {
                return base.ClippingRect;
            }
            set {
                base.ClippingRect = value;

                if (Item != null) {
                    Item.ClippingRect = base.ClippingRect;
                }
            }
        }
        public void AddAction(Func<CheckBox, bool> condition, Action<CheckBox> bAction) {
            checkBoxActions.Add(new CheckBoxConditionAction(condition, bAction));
        }
        public override bool UpdateInput() {
            OldIsHovered = IsHovered;
            IsHovered = IsInsideClip(new Point(Input.NewMouse.X, Input.NewMouse.Y));

            bool isUsed = false;

            foreach (CheckBoxConditionAction bca in checkBoxActions) {
                if (bca.condition(this)) {
                    bca.checkBoxAction(this);
                    isUsed = true;
                }
            }

            return isUsed;
        }
        public override void Draw(SpriteBatch s) {
            if (ShowBox) {
                if (IsHovered || HasFocus) {
                    s.FillRectangle(new RectangleF(Left, Top, Width, Height), new Color(20, 20, 20));
                } else {
                    s.FillRectangle(new RectangleF(Left, Top, Width, Height), Color.Black);
                }
            }

            Item.Draw(s);

            if (ShowBox && (IsHovered || HasFocus)) {
                s.DrawLine(Left, Top, Left, Bottom, Color.White, 2);
                s.DrawLine(Right, Top, Right, Bottom, Color.White, 2);
                s.DrawLine(Left, Top, Right, Top, Color.White, 2);
                s.DrawLine(Left, Bottom, Right, Bottom, Color.White, 2);
            }
        }
        public override int PrefWidth => Item.PrefWidth;
        public override int PrefHeight => Item.PrefHeight;
    }
}
