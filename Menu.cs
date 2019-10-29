using System;
using Apos.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Optional;

namespace AposGameStarter {
    /// <summary>
    /// Interface to modify the game settings.
    /// </summary>
    class Menu {
        public Menu() {
            grabFocus = (Component b) => {
                menuFocus.Focus = b;
            };

            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();
            menuSwitch = new Switcher<MenuScreens>();

            menuSwitch.Add(MenuScreens.Main, setupMainMenu());
            menuSwitch.Add(MenuScreens.Settings, setupSettingsMenu());
            menuSwitch.Add(MenuScreens.Debug, setupDebugMenu());
            menuSwitch.Add(MenuScreens.Quit, setupQuitConfirm());

            mp.Add(menuSwitch);

            menuFocus = new ComponentFocus(mp, Default.ConditionPreviousFocus, Default.ConditionNextFocus);

            selectMenu(MenuScreens.Main);
        }
        enum MenuScreens {
            Main,
            Settings,
            Debug,
            Quit
        }
        ComponentFocus menuFocus;
        Switcher<MenuScreens> menuSwitch;

        Action<Component> grabFocus;

        private Component setupMainMenu() {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionHoverMouse);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("AposGameStarter"));

            p.Add(Default.CreateButton("Resume Game", (Component b) => {
                return true;
            }, grabFocus));
            p.Add(Default.CreateButton("Settings", (Component b) => {
                selectMenu(MenuScreens.Settings);
                return true;
            }, grabFocus));
            p.Add(Default.CreateButton("Debug", (Component b) => {
                selectMenu(MenuScreens.Debug);
                return true;
            }, grabFocus));
            p.Add(Default.CreateButton("Quit", (Component b) => {
                selectMenu(MenuScreens.Quit);
                return true;
            }, grabFocus));

            return p;
        }
        private Component setupSettingsMenu() {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionHoverMouse);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Settings"));
            p.Add(createLabelDynamic(() => {
                return "[Current UI scale: " + GuiHelper.Scale + "x]";
            }));
            p.Add(Default.CreateButton("UI Scale 1x", (Component b) => {
                GuiHelper.NextLoopActions.Add(() => { GuiHelper.Scale = 1f; });
                return true;
            }, grabFocus));
            p.Add(Default.CreateButton("UI Scale 2x", (Component b) => {
                GuiHelper.NextLoopActions.Add(() => { GuiHelper.Scale = 2f; });
                return true;
            }, grabFocus));
            p.Add(Default.CreateButton("UI Scale 3x", (Component b) => {
                GuiHelper.NextLoopActions.Add(() => { GuiHelper.Scale = 3f; });
                return true;
            }, grabFocus));
            p.Add(Default.CreateButton("UI Scale 4x", (Component b) => {
                GuiHelper.NextLoopActions.Add(() => { GuiHelper.Scale = 4f; });
                return true;
            }, grabFocus));
            p.Add(Default.CreateButton("Back", (Component b) => {
                selectMenu(MenuScreens.Main);
                return true;
            }, grabFocus));

            return p;
        }
        private Component setupDebugMenu() {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionHoverMouse);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Debug"));
            p.Add(Default.CreateButton(() => {
                return "Show path line: " + (Utility.ShowLine ? " true" : "false");
            }, (Component b) => {
                Utility.ShowLine = !Utility.ShowLine;
                return true;
            }, grabFocus));
            p.Add(Default.CreateButton("Back", (Component b) => {
                selectMenu(MenuScreens.Main);
                return true;
            }, grabFocus));

            return p;
        }
        private Component setupQuitConfirm() {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionHoverMouse);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Do you really want to quit?"));
            p.Add(Default.CreateButton("Yes", (Component b) => {
                Utility.Game.Exit();
                return true;
            }, grabFocus));
            p.Add(Default.CreateButton("No", (Component b) => {
                selectMenu(MenuScreens.Main);
                return true;
            }, grabFocus));

            return p;
        }
        private void selectMenu(MenuScreens key) {
            GuiHelper.NextLoopActions.Add(() => {
                menuSwitch.Key = Option.Some(key);
                menuFocus.Focus = menuSwitch;
            });
        }

        public void UpdateSetup() {
            menuFocus.UpdateSetup();
        }
        public void UpdateInput() {
            bool usedInput = false;

            if (Default.ConditionBackFocus()) {
                if (menuSwitch.Key == Option.Some(MenuScreens.Main)) {
                    selectMenu(MenuScreens.Quit);
                } else {
                    selectMenu(MenuScreens.Main);
                }
                usedInput = true;
            }

            if (!usedInput) {
                usedInput = menuFocus.UpdateInput();
            }
        }
        public void Update() {
            menuFocus.Update();
        }
        public void DrawUI(SpriteBatch s) {
            menuFocus.Draw(s);
        }
        private Component createTitle(string text) {
            Label l = new Label(text);
            Border border = new Border(l, 20, 20, 20, 50);

            return border;
        }
        private Component createLabelDynamic(Func<string> text) {
            LabelDynamic ld = new LabelDynamic(text);
            ld.ActiveColor = Color.White;
            ld.NormalColor = new Color(150, 150, 150);
            Border border = new Border(ld, 20, 20, 20, 20);

            return border;
        }

        private class MenuPanel : ScreenPanel {
            public MenuPanel() { }

            public override void Draw(SpriteBatch s) {
                SetScissor(s);
                s.FillRectangle(BoundingRect, Color.Black * 0.6f);

                s.DrawLine(Left, Top, Right, Top, Color.Black, 2);
                s.DrawLine(Right, Top, Right, Bottom, Color.Black, 2);
                s.DrawLine(Left, Bottom, Right, Bottom, Color.Black, 2);
                s.DrawLine(Left, Top, Left, Bottom, Color.Black, 2);

                base.Draw(s);
                ResetScissor(s);
            }
        }
    }
}