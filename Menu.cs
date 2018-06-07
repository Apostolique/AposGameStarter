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
            currentMenu = MenuScreens.Main;

            menus = new Dictionary<MenuScreens, Panel>();

            menus.Add(MenuScreens.Main, setupMainMenu());
            menus.Add(MenuScreens.Settings, setupSettingsMenu());
            menus.Add(MenuScreens.Debug, setupDebugMenu());
            menus.Add(MenuScreens.Quit, setupQuitConfirm());
        }
        enum MenuScreens {
            Main,
            Settings,
            Debug,
            Quit
        }
        Dictionary<MenuScreens, Panel> menus;
        MenuScreens currentMenu;

        Func<bool> leftClick = () => Utility.Input.OldMouse.LeftButton == ButtonState.Released && Utility.Input.NewMouse.LeftButton == ButtonState.Pressed;
        private Panel setupMainMenu() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();

            Label l1 = new Label("AposGameCheatSheet");

            mp.Add(l1);


            mp.Add(createButtonLabel("Resume Game", leftClick, delegate(Button b) {
            }));
            mp.Add(createButtonLabel("Settings", leftClick, delegate(Button b) {
                currentMenu = MenuScreens.Settings;
            }));
            mp.Add(createButtonLabel("Debug", leftClick, delegate(Button b) {
                currentMenu = MenuScreens.Debug;
            }));
            mp.Add(createButtonLabel("Quit", leftClick, delegate(Button b) {
                currentMenu = MenuScreens.Quit;
            }));
            
            return mp;
        }
        private Panel setupSettingsMenu() {
            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();

            Label l1 = new Label("Settings");

            mp.Add(l1);
            mp.Add(createButtonLabel("Back", leftClick, delegate(Button b) {
                currentMenu = MenuScreens.Main;
            }));

            return mp;
        }
        private Panel setupDebugMenu() {
            Panel debugMenu = new ScreenPanel();

            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();

            debugMenu.Add(mp);

            Label l1 = new Label("Debug");

            mp.Add(l1);
            mp.Add(createButtonLabelDynamic(delegate() {
                return "Show path line: " + (Utility.showLine ? "true" : "false");
            }, leftClick, delegate(Button b) {
                Utility.showLine = !Utility.showLine;
            }));
            mp.Add(createButtonLabel("Back", leftClick, delegate(Button b) {
                currentMenu = MenuScreens.Main;
            }));
            
            return debugMenu;
        }
        private Panel setupQuitConfirm() {
            Panel quitMenu = new ScreenPanel();

            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();

            quitMenu.Add(mp);

            Label l1 = new Label("Do you really want to quit?");

            mp.Add(l1);
            mp.Add(createButtonLabel("Yes", leftClick, delegate(Button b) {
                Utility.game.Exit();
            }));
            mp.Add(createButtonLabel("No", leftClick, delegate(Button b) {
                currentMenu = MenuScreens.Main;
            }));

            return quitMenu;
        }
        public void UpdateSetup() {
            Panel currentPanel = menus[currentMenu];
            currentPanel.UpdateSetup();
        }
        public void UpdateInput() {
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
        private Button createButtonLabel(string text, Func<bool> c, Action<Button> a) {
            Button b = new ButtonLabel(text);
            b.setBox(false);
            b.AddAction(c, a);
            return b;
        }
        private Button createButtonLabel(string text) {
            Button b = new ButtonLabel(text);
            b.setBox(false);
            return b;
        }
        private Button createButtonLabelDynamic(Func<string> text, Func<bool> c,  Action<Button> a) {
            LabelDynamic ld = new LabelDynamic(text);
            Button b = new ButtonLabel(ld);
            b.setBox(false);
            b.AddAction(c, a);
            return b;
        }
        private Button createButtonLabelDynamic(Func<string> text) {
            LabelDynamic ld = new LabelDynamic(text);
            Button b = new ButtonLabel(ld);
            b.setBox(false);
            return b;
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
