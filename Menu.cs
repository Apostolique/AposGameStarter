using System;
using System.Collections.Generic;
using System.Text;
using AposGameCheatSheet.AposGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace AposGameCheatSheet
{
    /// <summary>
    /// Goal: Interface to modify the game settings.
    /// </summary>
    class Menu
    {
        public Menu() {
            hoverAction = delegate(Button b) {
                ComponentFocus currentPanel = menus[currentMenu];
                currentPanel.focus.HasFocus = false;
                currentPanel.focus = b;
                currentPanel.focus.HasFocus = true;
            };
            hoverFocus = (Button b) =>
                !b.OldIsHovered && b.IsHovered ||
                !b.HasFocus && b.IsHovered && Input.OldMouse.Position != Input.NewMouse.Position;
            leftClick = (Button b) =>
                b.IsHovered && b.HasFocus &&
                Input.OldMouse.LeftButton == ButtonState.Pressed && Input.NewMouse.LeftButton == ButtonState.Released;
            keyboardClick = (Button b) =>
                b.HasFocus &&
                (Input.OldKeyboard.IsKeyUp(Keys.Space) && Input.NewKeyboard.IsKeyDown(Keys.Space) ||
                Input.OldKeyboard.IsKeyUp(Keys.Enter) && Input.NewKeyboard.IsKeyDown(Keys.Enter));
            gamePadAClick = (Button b) =>
                b.HasFocus && Input.Capabilities.IsConnected && Input.Capabilities.HasAButton && Input.OldGamePad.Buttons.A == ButtonState.Released && Input.NewGamePad.Buttons.A == ButtonState.Pressed;
            previousFocusAction = () =>
                Input.Capabilities.IsConnected && Input.Capabilities.HasLeftStickButton &&
                Input.OldGamePad.ThumbSticks.Left.Y <= 0 && Input.NewGamePad.ThumbSticks.Left.Y > 0 ||
                Input.OldKeyboard.IsKeyUp(Keys.Up) && Input.NewKeyboard.IsKeyDown(Keys.Up);
            nextFocusAction = () =>
                Input.Capabilities.IsConnected && Input.Capabilities.HasLeftStickButton &&
                Input.OldGamePad.ThumbSticks.Left.Y >= 0 && Input.NewGamePad.ThumbSticks.Left.Y < 0 ||
                Input.OldKeyboard.IsKeyUp(Keys.Down) && Input.NewKeyboard.IsKeyDown(Keys.Down);
            gamePadBClick = () =>
                Input.Capabilities.IsConnected && Input.Capabilities.HasBButton &&
                Input.OldGamePad.Buttons.B == ButtonState.Released && Input.NewGamePad.Buttons.B == ButtonState.Pressed;

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

        Func<Button, bool> hoverFocus;
        Action<Button> hoverAction;

        Func<Button, bool> leftClick;
        Func<Button, bool> keyboardClick;
        Func<Button, bool> gamePadAClick;
        Func<bool> previousFocusAction;
        Func<bool> nextFocusAction;
        Func<bool> gamePadBClick;

        private ComponentFocus setupMainMenu() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();

            Label l1 = new Label("AposGameCheatSheet");
            Border l1Border = new Border(l1, 30, 30, 30, 50);
            mp.Add(l1Border);

            mp.Add(createButtonLabel("Resume Game", delegate(Button b) {
            }));
            mp.Add(createButtonLabel("Settings", delegate(Button b) {
                selectMenu(MenuScreens.Settings);
            }));
            mp.Add(createButtonLabel("Debug", delegate(Button b) {
                selectMenu(MenuScreens.Debug);
            }));
            mp.Add(createButtonLabel("Quit", delegate(Button b) {
                selectMenu(MenuScreens.Quit);
            }));

            return new ComponentFocus(mp);
        }
        private ComponentFocus setupSettingsMenu() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();

            Label l1 = new Label("Settings");
            Border l1Border = new Border(l1, 30, 30, 30, 50);
            mp.Add(l1Border);
            mp.Add(createButtonLabel("Back", delegate(Button b) {
                selectMenu(MenuScreens.Main);
            }));

            return new ComponentFocus(mp);
        }
        private ComponentFocus setupDebugMenu() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();

            Label l1 = new Label("Debug");
            Border l1Border = new Border(l1, 30, 30, 30, 50);
            mp.Add(l1Border);
            mp.Add(createButtonLabelDynamic(delegate() {
                return "Show path line: " + (Utility.showLine ? "true" : "false");
            }, delegate(Button b) {
                Utility.showLine = !Utility.showLine;
            }));
            mp.Add(createButtonLabel("Back", delegate(Button b) {
                selectMenu(MenuScreens.Main);
            }));
            
            return new ComponentFocus(mp);
        }
        private ComponentFocus setupQuitConfirm() {
            Panel quitMenu = new ScreenPanel();

            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();

            quitMenu.Add(mp);

            Label l1 = new Label("Do you really want to quit?");
            Border l1Border = new Border(l1, 30, 30, 30, 50);
            mp.Add(l1Border);
            mp.Add(createButtonLabel("Yes", delegate(Button b) {
                Utility.game.Exit();
            }));
            mp.Add(createButtonLabel("No", delegate(Button b) {
                selectMenu(MenuScreens.Main);
            }));

            return new ComponentFocus(quitMenu);
        }
        private void selectMenu(MenuScreens ms) {
            currentMenu = ms;
        }

        public void UpdateSetup() {
            foreach (KeyValuePair<MenuScreens, ComponentFocus> kvp in menus) {
                kvp.Value.component.UpdateSetup();
            }
        }
        public void UpdateInput() {
            ComponentFocus currentPanel = menus[currentMenu];

            if (nextFocusAction()) {
                currentPanel.focus = currentPanel.findNext(currentPanel.focus);
            }
            if (previousFocusAction()) {
                currentPanel.focus = currentPanel.findPrevious(currentPanel.focus);
            }
            if (gamePadBClick()) {
                if (currentMenu == MenuScreens.Main) {
                    selectMenu(MenuScreens.Quit);
                } else {
                    selectMenu(MenuScreens.Main);
                }
            }

            bool usedInput = currentPanel.component.UpdateInput();
        }
        public void Update() {
            Component currentPanel = menus[currentMenu].component;
            currentPanel.Update();
        }
        public void DrawUI(SpriteBatch s) {
            Component currentPanel = menus[currentMenu].component;
            currentPanel.Draw(s);
        }
        private Component createButtonLabel(string text, Action<Button> a) {
            Label l = new Label(text);
            l.ActiveColor = Color.White;
            l.NormalColor = new Color(150, 150, 150);
            Border border = new Border(l, 20, 20, 20, 20);
            Button b = new Button(border);
            b.ShowBox = false;
            b.AddAction(leftClick, a);
            b.AddAction(keyboardClick, a);
            b.AddAction(gamePadAClick, a);
            b.AddAction(hoverFocus, hoverAction);

            return b;
        }
        private Component createButtonLabel(string text) {
            Label l = new Label(text);
            l.ActiveColor = Color.White;
            l.NormalColor = new Color(150, 150, 150);
            Border border = new Border(l, 20, 20, 20, 20);
            Button b = new Button(border);
            b.ShowBox = false;

            return b;
        }
        private Component createButtonLabelDynamic(Func<string> text, Action<Button> a) {
            LabelDynamic ld = new LabelDynamic(text);
            ld.ActiveColor = Color.White;
            ld.NormalColor = new Color(150, 150, 150);
            Border border = new Border(ld, 20, 20, 20, 20);
            Button b = new Button(border);
            b.ShowBox = false;
            b.AddAction(leftClick, a);
            b.AddAction(keyboardClick, a);
            b.AddAction(gamePadAClick, a);
            b.AddAction(hoverFocus, hoverAction);

            return b;
        }
        private Component createButtonLabelDynamic(Func<string> text) {
            LabelDynamic ld = new LabelDynamic(text);
            ld.ActiveColor = Color.White;
            ld.NormalColor = new Color(150, 150, 150);
            Border border = new Border(ld, 20, 20, 20, 20);
            Button b = new Button(border);
            b.ShowBox = false;

            return b;
        }
        private class MenuPanel : PanelVerticalScroll {
            public MenuPanel() {
            }
            public override bool UpdateInput() {
                bool used = base.UpdateInput();
                return used || IsInsideClip(new Point(Input.NewMouse.X, Input.NewMouse.Y));
            }

            public override void Draw(SpriteBatch s) {
                s.FillRectangle(ClippingRect, Color.Black * 0.6f);
                //s.FillRectangle(new Rectangle(0, 0, 100, 100), Color.Black * 0.6f);

                s.DrawLine(Left, Top, Right, Top, Color.Black, 2);
                s.DrawLine(Right, Top, Right, Bottom, Color.Black, 2);
                s.DrawLine(Left, Bottom, Right, Bottom, Color.Black, 2);
                s.DrawLine(Left, Top, Left, Bottom, Color.Black, 2);

                base.Draw(s);
            }
            public override int Width => Utility.WindowWidth;
            public override int Height => Utility.WindowHeight;
        }
        private class ComponentFocus {
            public ComponentFocus(Component c) {
                component = c;

                if (focus != null) {
                    focus.HasFocus = false;
                }

                Component possibleFocus = findFinal(component);
                if (possibleFocus.IsFocusable) {
                    focus = possibleFocus;
                    focus.HasFocus = true;
                } else {
                    focus = findNext(possibleFocus);
                }
            }
            public ComponentFocus(Component c, Component f) {
                component = c;
                focus = f;
            }
            public Component component;
            public Component focus;

            public Component findPrevious(Component c) {
                if (c != null) {
                    Component currentFocus = c;
                    currentFocus.HasFocus = false;

                    do {
                        currentFocus = currentFocus.GetPrevious();
                        currentFocus = findFinal(currentFocus);
                    } while (!currentFocus.IsFocusable && currentFocus != c);

                    if (currentFocus.IsFocusable) {
                        currentFocus.HasFocus = true;
                        return currentFocus;
                    }
                }
                return null;
            }
            public Component findNext(Component c) {
                if (c != null) {
                    Component currentFocus = c;
                    currentFocus.HasFocus = false;

                    do {
                        currentFocus = currentFocus.GetNext();
                        currentFocus = findFinal(currentFocus);
                    } while (!currentFocus.IsFocusable && currentFocus != c);

                    if (currentFocus.IsFocusable) {
                        currentFocus.HasFocus = true;
                        return currentFocus;
                    }
                }
                return null;
            }
            public Component findFinal(Component c) {
                Component previousFinal;
                Component currentFinal = c;
                do {
                    previousFinal = currentFinal;
                    currentFinal = previousFinal.GetFinal();
                } while (currentFinal != previousFinal && currentFinal != c);

                return currentFinal;
            }
        }
    }
}
