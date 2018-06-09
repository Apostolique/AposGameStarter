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
            menus = new Dictionary<MenuScreens, Panel>();
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
        Dictionary<MenuScreens, Panel> menus;
        MenuScreens currentMenu;
        Component focus;

        Func<Button, bool> leftClick = (Button b) => b.isHovered && Utility.Input.OldMouse.LeftButton == ButtonState.Released && Utility.Input.NewMouse.LeftButton == ButtonState.Pressed;
        Func<Button, bool> gamePadAClick = (Button b) => b.HasFocus && Utility.Input.Capabilities.IsConnected && Utility.Input.Capabilities.HasAButton && Utility.Input.OldGamePad.Buttons.A == ButtonState.Released && Utility.Input.NewGamePad.Buttons.A == ButtonState.Pressed;
        Func<bool> previousFocusAction = () => Utility.Input.Capabilities.IsConnected && Utility.Input.Capabilities.HasLeftStickButton && Utility.Input.OldGamePad.ThumbSticks.Left.Y <= 0 && Utility.Input.NewGamePad.ThumbSticks.Left.Y > 0;
        Func<bool> nextFocusAction = () => Utility.Input.Capabilities.IsConnected && Utility.Input.Capabilities.HasLeftStickButton && Utility.Input.OldGamePad.ThumbSticks.Left.Y >= 0 && Utility.Input.NewGamePad.ThumbSticks.Left.Y < 0;
        Func<bool> gamePadBClick = () => Utility.Input.Capabilities.IsConnected && Utility.Input.Capabilities.HasBButton && Utility.Input.OldGamePad.Buttons.B == ButtonState.Released && Utility.Input.NewGamePad.Buttons.B == ButtonState.Pressed;

        private Panel setupMainMenu() {
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
            
            return mp;
        }
        private Panel setupSettingsMenu() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();

            Label l1 = new Label("Settings");
            Border l1Border = new Border(l1, 30, 30, 30, 50);
            mp.Add(l1Border);
            mp.Add(createButtonLabel("Back", delegate(Button b) {
                selectMenu(MenuScreens.Main);
            }));

            return mp;
        }
        private Panel setupDebugMenu() {
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
            
            return mp;
        }
        private Panel setupQuitConfirm() {
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

            return quitMenu;
        }
        private void selectMenu(MenuScreens ms) {
            if (focus != null) {
                focus.HasFocus = false;
            }

            currentMenu = ms;
            Component possibleFocus = findFinal(menus[ms]);
            if (possibleFocus.CanFocus) {
                focus = possibleFocus;
                focus.HasFocus = true;
            } else {
                focus = findNext(possibleFocus);
            }
        }
        private Component findPrevious(Component c) {
            if (c != null) {
                Component currentFocus = c;
                currentFocus.HasFocus = false;

                do {
                    currentFocus = currentFocus.GetPrevious();
                    currentFocus = findFinal(currentFocus);
                } while (!currentFocus.CanFocus && currentFocus != c);

                if (currentFocus.CanFocus) {
                    currentFocus.HasFocus = true;
                    return currentFocus;
                }
            }
            return null;
        }
        private Component findNext(Component c) {
            if (c != null) {
                Component currentFocus = c;
                currentFocus.HasFocus = false;

                do {
                    currentFocus = currentFocus.GetNext();
                    currentFocus = findFinal(currentFocus);
                } while (!currentFocus.CanFocus && currentFocus != c);

                if (currentFocus.CanFocus) {
                    currentFocus.HasFocus = true;
                    return currentFocus;
                }
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
        public void UpdateSetup() {
            foreach (KeyValuePair<MenuScreens, Panel> kvp in menus) {
                kvp.Value.UpdateSetup();
            }
        }
        public void UpdateInput() {
            if (nextFocusAction()) {
                focus = findNext(focus);
            }
            if (previousFocusAction()) {
                focus = findPrevious(focus);
            }
            if (gamePadBClick()) {
                selectMenu(MenuScreens.Main);
            }

            Panel currentPanel = menus[currentMenu];
            bool usedInput = currentPanel.UpdateInput();
        }
        public void Update() {
            Panel currentPanel = menus[currentMenu];
            currentPanel.Update();
        }
        public void DrawUI(SpriteBatch s) {
            Panel currentPanel = menus[currentMenu];
            currentPanel.Draw(s, new Rectangle(0, 0, Utility.WindowWidth, Utility.WindowHeight));
        }
        private Component createButtonLabel(string text, Action<Button> a) {
            Button b = new ButtonLabel(text);
            b.setBox(false);
            b.AddAction(leftClick, a);
            b.AddAction(gamePadAClick, a);

            Border border = new Border(b, 20, 20, 20, 20);
            return border;
        }
        private Component createButtonLabel(string text) {
            Button b = new ButtonLabel(text);
            b.setBox(false);

            Border border = new Border(b, 20, 20, 20, 20);
            return border;
        }
        private Component createButtonLabelDynamic(Func<string> text, Action<Button> a) {
            LabelDynamic ld = new LabelDynamic(text);
            Button b = new ButtonLabel(ld);
            b.setBox(false);
            b.AddAction(leftClick, a);
            b.AddAction(gamePadAClick, a);

            Border border = new Border(b, 20, 20, 20, 20);
            return border;
        }
        private Component createButtonLabelDynamic(Func<string> text) {
            LabelDynamic ld = new LabelDynamic(text);
            Button b = new ButtonLabel(ld);
            b.setBox(false);

            Border border = new Border(b, 20, 20, 20, 20);
            return border;
        }
        private class MenuPanel : PanelVerticalScroll {
            public MenuPanel() {
            }
            public override bool UpdateInput() {
                bool used = base.UpdateInput();
                return used || IsInside(new Point(Utility.Input.NewMouse.X, Utility.Input.NewMouse.Y));
            }

            public override void Draw(SpriteBatch s, Rectangle clipRect) {
                clipRect = ClipRectangle(clipRect);

                s.FillRectangle(clipRect, Color.Black * 0.6f);
                //s.FillRectangle(new Rectangle(0, 0, 100, 100), Color.Black * 0.6f);

                s.DrawLine(Left, Top, Right, Top, Color.Black, 2);
                s.DrawLine(Right, Top, Right, Bottom, Color.Black, 2);
                s.DrawLine(Left, Bottom, Right, Bottom, Color.Black, 2);
                s.DrawLine(Left, Top, Left, Bottom, Color.Black, 2);

                base.Draw(s, clipRect);
            }
            public override int Width => Utility.WindowWidth;
            public override int Height => Utility.WindowHeight;
        }
    }
}
