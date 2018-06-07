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
    /// Goal: A button that displays itself horizontally.
    /// </summary>
    class Button : Component
    {
        struct ButtonConditionAction {
            public Func<bool> condition;
            public Action<Button> buttonAction;
            public ButtonConditionAction(Func<bool> iCondition, Action<Button> iButtonAction) {
                condition = iCondition;
                buttonAction = iButtonAction;
            }
        }

        public Component item;
        public bool isHovered = false;
        public bool showBox = true;
        private List<ButtonConditionAction> buttonActions;
        public Button() {
            item = new Component();
            buttonActions = new List<ButtonConditionAction>();
        }
        public Button(Component iItem) {
            item = iItem;
            buttonActions = new List<ButtonConditionAction>();
        }
        public void AddAction(Func<bool> condition, Action<Button> bAction) {
            buttonActions.Add(new ButtonConditionAction(condition, bAction));
        }
        public void setItem(Component iItem) {
            item = iItem;
        }
        public override void UpdateSetup() {
            item.Position = Position;
            item.Width = Width;
            item.Height = Height;
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

            item.Draw(s, clipRect);

            if (showBox && isHovered) {
                s.DrawLine(Left, Top, Left, Bottom, Color.White, 2);
                s.DrawLine(Right, Top, Right, Bottom, Color.White, 2);
                s.DrawLine(Left, Top, Right, Top, Color.White, 2);
                s.DrawLine(Left, Bottom, Right, Bottom, Color.White, 2);
            }
        }
        public override int PrefWidth => item.PrefWidth;
        public override int PrefHeight => item.PrefHeight;

        public void setBox(bool iBox) {
            showBox = iBox;
        }
    }
}
