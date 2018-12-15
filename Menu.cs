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
            hoverFocus = delegate(Button b) {
                menus[currentMenu].Focus = b;
            };
            hoverAction = (Button b) =>
                !b.OldIsHovered && b.IsHovered ||
                !b.HasFocus && b.IsHovered && Input.OldMouse.Position != Input.NewMouse.Position;
            selectAction = (Button b) =>
                b.HasFocus &&
                (Input.OldGamePad[0].Buttons.A == ButtonState.Released && Input.NewGamePad[0].Buttons.A == ButtonState.Pressed ||
                Input.OldKeyboard.IsKeyUp(Keys.Space) && Input.NewKeyboard.IsKeyDown(Keys.Space) ||
                Input.OldKeyboard.IsKeyUp(Keys.Enter) && Input.NewKeyboard.IsKeyDown(Keys.Enter) ||
                b.IsHovered && Input.OldMouse.LeftButton == ButtonState.Pressed && Input.NewMouse.LeftButton == ButtonState.Released);
            previousFocusAction = () =>
                Input.Capabilities.IsConnected && Input.Capabilities.HasLeftStickButton &&
                Input.OldGamePad[0].ThumbSticks.Left.Y <= 0 && Input.NewGamePad[0].ThumbSticks.Left.Y > 0 ||
                Input.OldKeyboard.IsKeyUp(Keys.Up) && Input.NewKeyboard.IsKeyDown(Keys.Up);
            nextFocusAction = () =>
                Input.Capabilities.IsConnected && Input.Capabilities.HasLeftStickButton &&
                Input.OldGamePad[0].ThumbSticks.Left.Y >= 0 && Input.NewGamePad[0].ThumbSticks.Left.Y < 0 ||
                Input.OldKeyboard.IsKeyUp(Keys.Down) && Input.NewKeyboard.IsKeyDown(Keys.Down);
            backAction = () =>
                Input.Capabilities.IsConnected && Input.Capabilities.HasBButton &&
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

        Func<Button, bool> hoverAction;
        Action<Button> hoverFocus;

        Func<Button, bool> selectAction;
        Func<bool> previousFocusAction;
        Func<bool> nextFocusAction;
        Func<bool> backAction;

        private ComponentFocus setupMainMenu() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();

            Label l1 = new Label(Assets.bitFont, "AposGameStarter");
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

            return new ComponentFocus(mp, previousFocusAction, nextFocusAction);
        }
        private ComponentFocus setupSettingsMenu() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();

            Label l1 = new Label(Assets.bitFont, "Settings");
            Border l1Border = new Border(l1, 30, 30, 30, 50);
            mp.Add(l1Border);
            mp.Add(createButtonLabel("Back", delegate(Button b) {
                selectMenu(MenuScreens.Main);
            }));

            return new ComponentFocus(mp, previousFocusAction, nextFocusAction);
        }
        private ComponentFocus setupDebugMenu() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();

            Label l1 = new Label(Assets.bitFont, "Debug");
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
            
            return new ComponentFocus(mp, previousFocusAction, nextFocusAction);
        }
        private ComponentFocus setupQuitConfirm() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();

            Label l1 = new Label(Assets.bitFont, "Do you really want to quit?");
            Border l1Border = new Border(l1, 30, 30, 30, 50);
            mp.Add(l1Border);
            mp.Add(createButtonLabel("Yes", delegate(Button b) {
                Utility.game.Exit();
            }));
            mp.Add(createButtonLabel("No", delegate(Button b) {
                selectMenu(MenuScreens.Main);
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
            currentPanel.Draw(s);
        }
        private Component createButtonLabel(string text, Action<Button> a) {
            Label l = new Label(Assets.bitFont, text);
            l.ActiveColor = Color.White;
            l.NormalColor = new Color(150, 150, 150);
            Border border = new Border(l, 20, 20, 20, 20);
            Button b = new Button(border);
            b.ShowBox = false;
            b.AddAction(selectAction, a);
            b.AddAction(hoverAction, hoverFocus);

            return b;
        }
        private Component createButtonLabel(string text) {
            Label l = new Label(Assets.bitFont, text);
            l.ActiveColor = Color.White;
            l.NormalColor = new Color(150, 150, 150);
            Border border = new Border(l, 20, 20, 20, 20);
            Button b = new Button(border);
            b.ShowBox = false;

            return b;
        }
        private Component createButtonLabelDynamic(Func<string> text, Action<Button> a) {
            LabelDynamic ld = new LabelDynamic(Assets.bitFont, text);
            ld.ActiveColor = Color.White;
            ld.NormalColor = new Color(150, 150, 150);
            Border border = new Border(ld, 20, 20, 20, 20);
            Button b = new Button(border);
            b.ShowBox = false;
            b.AddAction(selectAction, a);
            b.AddAction(hoverAction, hoverFocus);

            return b;
        }
        private Component createButtonLabelDynamic(Func<string> text) {
            LabelDynamic ld = new LabelDynamic(Assets.bitFont, text);
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
        public class ComponentFocus {
            public ComponentFocus(Component c, Func<bool> previousFocusAction, Func<bool> nextFocusAction) {
                RootComponent = c;

                Focus = findNext(RootComponent);

                _previousFocusAction = previousFocusAction;
                _nextFocusAction = nextFocusAction;
            }
            public Component RootComponent {
                get;
                set;
            }
            public Component Focus {
                get => _focus;
                set {
                    if (_focus != null) {
                        _focus.HasFocus = false;
                    }
                    _focus = value;
                    if (_focus != null) {
                        _focus.HasFocus = true;
                    }
                }
            }

            private Component _focus;
            private Func<bool> _previousFocusAction;
            private Func<bool> _nextFocusAction;

            public bool UpdateInput() {
                bool usedInput = false;
                if (_nextFocusAction()) {
                    FocusNext();
                    usedInput = true;
                }
                if (_previousFocusAction()) {
                    FocusPrevious();
                    usedInput = true;
                }

                return usedInput;
            }

            public void FocusPrevious() {
                Focus = findPrevious(Focus);
            }
            public void FocusNext() {
                Focus = findNext(Focus);
            }
            private Component findPrevious(Component c) {
                if (c == null) {
                    c = RootComponent;
                }
                Component currentFocus = c;
                currentFocus.HasFocus = false;

                do {
                    currentFocus = currentFocus.GetPrevious();
                    currentFocus = findFinalInverse(currentFocus);
                } while (!currentFocus.IsFocusable && currentFocus != c);

                if (currentFocus.IsFocusable) {
                    currentFocus.HasFocus = true;
                    return currentFocus;
                }
                return null;
            }
            private Component findNext(Component c) {
                if (c == null) {
                    c = RootComponent;
                }
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
                return null;
            }
            private Component findFinal(Component c) {
                Component previousFinal;
                Component currentFinal = c;
                do {
                    previousFinal = currentFinal;
                    currentFinal = previousFinal.GetFinal();
                } while (currentFinal != previousFinal && currentFinal != c);

                return currentFinal;
            }
            private Component findFinalInverse(Component c) {
                Component previousFinal;
                Component currentFinal = c;
                do {
                    previousFinal = currentFinal;
                    currentFinal = previousFinal.GetFinalInverse();
                } while (currentFinal != previousFinal && currentFinal != c);

                return currentFinal;
            }
        }
    }
}
