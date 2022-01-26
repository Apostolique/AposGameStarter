using System;
using Apos.Gui;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GameProject {
    /// <summary>
    /// Interface to modify the game settings.
    /// </summary>
    class Menu {
        public Menu() {
            menuFocus = new ComponentFocus(Default.ConditionPrevFocus, Default.ConditionNextFocus);

            MenuPanel mp = new MenuPanel();
            mp.Layout = new LayoutVerticalCenter();
            menuSwitch = new Switcher<MenuScreens>();

            menuSwitch.Add(MenuScreens.Main, setupMainMenu());
            menuSwitch.Add(MenuScreens.Settings, setupSettingsMenu());
            menuSwitch.Add(MenuScreens.Debug, setupDebugMenu());
            menuSwitch.Add(MenuScreens.Quit, setupQuitConfirm());

            mp.Add(menuSwitch);

            menuFocus.Root = mp;

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

        private Component setupMainMenu() {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("AposGameStarter"));

            p.Add(Default.CreateButton("Resume Game", c => { }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Settings", c => {
                selectMenu(MenuScreens.Settings);
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Debug", c => {
                selectMenu(MenuScreens.Debug);
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Quit", c => {
                selectMenu(MenuScreens.Quit);
            }, menuFocus.GrabFocus));

            return p;
        }
        private Component setupSettingsMenu() {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Settings"));
            p.Add(createLabelDynamic(() => {
                return "[Current UI scale: " + GuiHelper.Scale + "x]";
            }));
            p.Add(Default.CreateButton(() => {
                return $"FullScreen: {(Utility.Settings.IsFullscreen ? " true" : "false")}";
            }, c => {
                GuiHelper.NextLoopActions.Add(() => { Utility.ToggleFullscreen(); });
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton(() => {
                return $"Borderless: {(Utility.Settings.IsBorderless ? " true" : "false")}";
            }, c => {
                GuiHelper.NextLoopActions.Add(() => {
                    Utility.ToggleBorderless();
                });
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("UI Scale 1x", c => {
                GuiHelper.NextLoopActions.Add(() => { GuiHelper.Scale = 1f; });
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("UI Scale 2x", c => {
                GuiHelper.NextLoopActions.Add(() => { GuiHelper.Scale = 2f; });
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("UI Scale 3x", c => {
                GuiHelper.NextLoopActions.Add(() => { GuiHelper.Scale = 3f; });
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("UI Scale 4x", c => {
                GuiHelper.NextLoopActions.Add(() => { GuiHelper.Scale = 4f; });
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Back", c => {
                selectMenu(MenuScreens.Main);
            }, menuFocus.GrabFocus));

            return p;
        }
        private Component setupDebugMenu() {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Debug"));
            p.Add(Default.CreateButton(() => {
                return "Show path line: " + (Utility.ShowLine ? " true" : "false");
            }, c => {
                Utility.ShowLine = !Utility.ShowLine;
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("Back", c => {
                selectMenu(MenuScreens.Main);
            }, menuFocus.GrabFocus));

            return p;
        }
        private Component setupQuitConfirm() {
            Panel p = new Panel();
            p.Layout = new LayoutVerticalCenter();
            p.AddHoverCondition(Default.ConditionMouseHover);
            p.AddAction(Default.IsScrolled, Default.ScrollVertically);

            p.Add(createTitle("Do you really want to quit?"));
            p.Add(Default.CreateButton("Yes", c => {
                Utility.Game.Exit();
            }, menuFocus.GrabFocus));
            p.Add(Default.CreateButton("No", c => {
                selectMenu(MenuScreens.Main);
            }, menuFocus.GrabFocus));

            return p;
        }
        private void selectMenu(MenuScreens key) {
            GuiHelper.NextLoopActions.Add(() => {
                menuSwitch.Key = Optional.Option.Some(key);
                menuFocus.Focus = menuSwitch;
            });
        }

        public void UpdateSetup() {
            menuFocus.UpdateSetup();
        }
        public void UpdateInput() {
            if (Default.ConditionBackFocus()) {
                if (menuSwitch.Key == Optional.Option.Some(MenuScreens.Main)) {
                    selectMenu(MenuScreens.Quit);
                } else {
                    selectMenu(MenuScreens.Main);
                }
            }

            menuFocus.UpdateInput();
        }
        public void Update() {
            menuFocus.Update();
        }
        public void DrawUI() {
            menuFocus.Draw();
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

            public override void Draw() {
                SetScissor();
                _s.FillRectangle(BoundingRect, Color.Black * 0.6f);

                _s.DrawLine(Left, Top, Right, Top, Color.Black, 2);
                _s.DrawLine(Right, Top, Right, Bottom, Color.Black, 2);
                _s.DrawLine(Left, Bottom, Right, Bottom, Color.Black, 2);
                _s.DrawLine(Left, Top, Left, Bottom, Color.Black, 2);

                base.Draw();
                ResetScissor();
            }
        }
    }
}
