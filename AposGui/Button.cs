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
    /// Goal: A button component that handles actions.
    /// </summary>
    class Button : Component
    {
        public Button() : this(new Component()) {
        }
        public Button(Component iItem) {
            Item = iItem;
            buttonActions = new List<ButtonConditionAction>();
        }
        struct ButtonConditionAction {
            public Func<bool> condition;
            public Action<Button> buttonAction;
            public ButtonConditionAction(Func<bool> iCondition, Action<Button> iButtonAction) {
                condition = iCondition;
                buttonAction = iButtonAction;
            }
        }
        public Component Item {
            get; set;
        }
        public bool isHovered = false;
        public bool showBox = true;
        private List<ButtonConditionAction> buttonActions;

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
        public void AddAction(Func<bool> condition, Action<Button> bAction) {
            buttonActions.Add(new ButtonConditionAction(condition, bAction));
        }
        public override bool UpdateInput() {
            isHovered = IsInside(new Point(Utility.Input.NewMouse.X, Utility.Input.NewMouse.Y));

            bool isUsed = false;

            foreach (ButtonConditionAction bca in buttonActions) {
                if (isHovered && bca.condition()) {
                    bca.buttonAction(this);
                    isUsed = true;
                }
            }

            return isUsed;
        }
        public override void Draw(SpriteBatch s, Rectangle clipRect) {
            clipRect = ClipRectangle(clipRect);

            if (showBox) {
                if (isHovered) {
                    s.FillRectangle(new RectangleF(Left, Top, Width, Height), new Color(20, 20, 20));
                } else {
                    s.FillRectangle(new RectangleF(Left, Top, Width, Height), Color.Black);
                }
            }

            Item.Draw(s, clipRect);

            if (showBox && isHovered) {
                s.DrawLine(Left, Top, Left, Bottom, Color.White, 2);
                s.DrawLine(Right, Top, Right, Bottom, Color.White, 2);
                s.DrawLine(Left, Top, Right, Top, Color.White, 2);
                s.DrawLine(Left, Bottom, Right, Bottom, Color.White, 2);
            }
        }
        public override int PrefWidth => Item.PrefWidth;
        public override int PrefHeight => Item.PrefHeight;

        public void setBox(bool iBox) {
            showBox = iBox;
        }
    }
}
