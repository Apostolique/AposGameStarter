using System;
using System.Collections.Generic;
using System.Text;
using AposGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace AposGameStarter
{
    /// <summary>
    /// Goal: Interface to modify the game settings.
    /// </summary>
    class Menu
    {
        public Menu() {
            grabFocus = (Component b) => {
                menus[currentMenu].Focus = b;
            };
            hoverCondition = (Component b) =>
                b.IsInsideClip(GuiHelper.MouseToUI());
            hoverFocus = (Component b) =>
                !b.OldIsHovered && b.IsHovered;
            selectCondition = (Component b) =>
                b.HasFocus &&
                (Input.OldGamePad[0].Buttons.A == ButtonState.Released && Input.NewGamePad[0].Buttons.A == ButtonState.Pressed ||
                Input.OldKeyboard.IsKeyUp(Keys.Space) && Input.NewKeyboard.IsKeyDown(Keys.Space) ||
                Input.OldKeyboard.IsKeyUp(Keys.Enter) && Input.NewKeyboard.IsKeyDown(Keys.Enter) ||
                b.IsHovered && Input.OldMouse.LeftButton == ButtonState.Pressed && Input.NewMouse.LeftButton == ButtonState.Released);
            previousFocusAction = () =>
                Input.OldGamePad[0].ThumbSticks.Left.Y <= 0 && Input.NewGamePad[0].ThumbSticks.Left.Y > 0 ||
                Input.OldKeyboard.IsKeyUp(Keys.Up) && Input.NewKeyboard.IsKeyDown(Keys.Up);
            nextFocusAction = () =>
                Input.OldGamePad[0].ThumbSticks.Left.Y >= 0 && Input.NewGamePad[0].ThumbSticks.Left.Y < 0 ||
                Input.OldKeyboard.IsKeyUp(Keys.Down) && Input.NewKeyboard.IsKeyDown(Keys.Down);
            backAction = () =>
                Input.OldGamePad[0].Buttons.B == ButtonState.Released && Input.NewGamePad[0].Buttons.B == ButtonState.Pressed ||
                Input.OldKeyboard.IsKeyUp(Keys.Escape) && Input.NewKeyboard.IsKeyDown(Keys.Escape);

            menus = new Dictionary<MenuScreens, ComponentFocus>();
            menus.Add(MenuScreens.Main, setupMainMenu());
            menus.Add(MenuScreens.Settings, setupSettingsMenu());
            menus.Add(MenuScreens.Debug, setupDebugMenu());
            menus.Add(MenuScreens.Quit, setupQuitConfirm());

            selectMenu(MenuScreens.Main);
        }
        enum MenuScreens {
            Main,
            Settings,
            Debug,
            Quit
        }
        Dictionary<MenuScreens, ComponentFocus> menus;
        MenuScreens currentMenu;

        Func<Component, bool> hoverCondition;
        Func<Component, bool> hoverFocus;
        Action<Component> grabFocus;

        Func<Component, bool> selectCondition;
        Func<bool> previousFocusAction;
        Func<bool> nextFocusAction;
        Func<bool> backAction;

        private ComponentFocus setupMainMenu() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();
            mp.AddHoverCondition(hoverCondition);

            Label l1 = new Label("AposGameStarter");
            Border l1Border = new Border(l1, 30, 30, 30, 50);
            mp.Add(l1Border);

            mp.Add(createButtonLabel("Resume Game", (Component b) => {
                return true;
            }));
            mp.Add(createButtonLabel("Settings", (Component b) => {
                selectMenu(MenuScreens.Settings);
                return true;
            }));
            mp.Add(createButtonLabel("Debug", (Component b) => {
                selectMenu(MenuScreens.Debug);
                return true;
            }));
            mp.Add(createButtonLabel("Quit", (Component b) => {
                selectMenu(MenuScreens.Quit);
                return true;
            }));

            return new ComponentFocus(mp, previousFocusAction, nextFocusAction);
        }
        private ComponentFocus setupSettingsMenu() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();
            mp.AddHoverCondition(hoverCondition);

            Label l1 = new Label("Settings");
            Border l1Border = new Border(l1, 30, 30, 30, 50);
            mp.Add(l1Border);
            mp.Add(createLabelDynamic(() => {
                return "[Current UI scale: " + GuiHelper.Scale + "x]";
            }));
            mp.Add(createButtonLabel("UI Scale 1x", (Component b) => {
                GuiHelper.Scale = 1f;
                return true;
            }));
            mp.Add(createButtonLabel("UI Scale 2x", (Component b) => {
                GuiHelper.Scale = 2f;
                return true;
            }));
            mp.Add(createButtonLabel("UI Scale 3x", (Component b) => {
                GuiHelper.Scale = 3f;
                return true;
            }));
            mp.Add(createButtonLabel("UI Scale 4x", (Component b) => {
                GuiHelper.Scale = 4f;
                return true;
            }));
            mp.Add(createButtonLabel("Back", (Component b) => {
                selectMenu(MenuScreens.Main);
                return true;
            }));

            return new ComponentFocus(mp, previousFocusAction, nextFocusAction);
        }
        private ComponentFocus setupDebugMenu() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();
            mp.AddHoverCondition(hoverCondition);

            Label l1 = new Label("Debug");
            Border l1Border = new Border(l1, 30, 30, 30, 50);
            mp.Add(l1Border);
            mp.Add(createButtonLabelDynamic(() => {
                return "Show path line: " + (Utility.showLine ? " true" : "false");
            }, (Component b) => {
                Utility.showLine = !Utility.showLine;
                return true;
            }));
            mp.Add(createButtonLabel("Back", (Component b) => {
                selectMenu(MenuScreens.Main);
                return true;
            }));
            
            return new ComponentFocus(mp, previousFocusAction, nextFocusAction);
        }
        private ComponentFocus setupQuitConfirm() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();
            mp.AddHoverCondition(hoverCondition);

            Label l1 = new Label("Do you really want to quit?");
            Border l1Border = new Border(l1, 30, 30, 30, 50);
            mp.Add(l1Border);
            mp.Add(createButtonLabel("Yes", (Component b) => {
                Utility.game.Exit();
                return true;
            }));
            mp.Add(createButtonLabel("No", (Component b) => {
                selectMenu(MenuScreens.Main);
                return true;
            }));

            return new ComponentFocus(mp, previousFocusAction, nextFocusAction);
        }
        private void selectMenu(MenuScreens ms) {
            currentMenu = ms;
        }

        public void UpdateSetup() {
            foreach (KeyValuePair<MenuScreens, ComponentFocus> kvp in menus) {
                kvp.Value.RootComponent.UpdateSetup();
            }
        }
        public void UpdateInput() {
            ComponentFocus currentPanel = menus[currentMenu];

            bool usedInput = currentPanel.UpdateInput();

            if (backAction()) {
                if (currentMenu == MenuScreens.Main) {
                    selectMenu(MenuScreens.Quit);
                } else {
                    selectMenu(MenuScreens.Main);
                }
                usedInput = true;
            }

            if (!usedInput) {
                usedInput = currentPanel.RootComponent.UpdateInput();
            }
        }
        public void Update() {
            Component currentPanel = menus[currentMenu].RootComponent;
            currentPanel.Update();
        }
        public void DrawUI(SpriteBatch s) {
            Component currentPanel = menus[currentMenu].RootComponent;
            GuiHelper.DrawGui(s, currentPanel);
        }
        private Component createLabel(string text) {
            Label l = new Label(text);
            l.ActiveColor = Color.White;
            l.NormalColor = new Color(150, 150, 150);
            Border border = new Border(l, 20, 20, 20, 20);

            return border;
        }
        private Component createLabelDynamic(Func<string> text) {
            LabelDynamic ld = new LabelDynamic(text);
            ld.ActiveColor = Color.White;
            ld.NormalColor = new Color(150, 150, 150);
            Border border = new Border(ld, 20, 20, 20, 20);

            return border;
        }
        private Component createButtonLabel(string text, Func<Component, bool> a) {
            Label l = new Label(text);
            l.ActiveColor = Color.White;
            l.NormalColor = new Color(150, 150, 150);
            Border border = new Border(l, 20, 20, 20, 20);

            return createButton(border, a);
        }
        private Component createButtonLabelDynamic(Func<string> text, Func<Component, bool> a) {
            LabelDynamic ld = new LabelDynamic(text);
            ld.ActiveColor = Color.White;
            ld.NormalColor = new Color(150, 150, 150);
            Border border = new Border(ld, 20, 20, 20, 20);

            return createButton(border, a);
        }
        private Component createButton(Component c, Func<Component, bool> a) {
            Button b = new Button(c);
            b.ShowBox = false;
            b.GrabFocus = grabFocus;
            b.AddHoverCondition(hoverCondition);
            b.AddAction(selectCondition, a);
            b.AddAction(hoverFocus, (Component component) => true);

            return b;
        }
        private class MenuPanel : PanelVerticalScroll {
            public MenuPanel() {
            }
            public override bool UpdateInput() {
                bool used = base.UpdateInput();
                return used || IsInsideClip(GuiHelper.MouseToUI());
            }

            public override void Draw(SpriteBatch s) {
                SetScissor(s);
                s.FillRectangle(BoundingRect, Color.Black * 0.6f);
                //s.FillRectangle(new Rectangle(0, 0, 100, 100), Color.Black * 0.6f);

                s.DrawLine(Left, Top, Right, Top, Color.Black, 2);
                s.DrawLine(Right, Top, Right, Bottom, Color.Black, 2);
                s.DrawLine(Left, Bottom, Right, Bottom, Color.Black, 2);
                s.DrawLine(Left, Top, Left, Bottom, Color.Black, 2);

                base.Draw(s);
                ResetScissor(s);
            }
            public override int Width => GuiHelper.WindowWidth;
            public override int Height => GuiHelper.WindowHeight;
        }
    }
}
