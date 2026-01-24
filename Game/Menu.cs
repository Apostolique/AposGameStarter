using System.Runtime.CompilerServices;
using Apos.Gui;
using Apos.Input;
using Microsoft.Xna.Framework;

namespace GameProject {
    /// <summary>
    /// Interface to modify the game settings.
    /// </summary>
    class Menu {
        public Menu() { }

        public void CreateMenu() {
            MenuUI.Push();

            if (_screen == MenuScreens.Main) {
                SetupMainMenu();
            } else if (_screen == MenuScreens.Settings) {
                SetupSettingsMenu();
            } else if (_screen == MenuScreens.Debug) {
                SetupDebugMenu();
            } else {
                SetupQuitConfirm();
            }

            MenuUI.Pop();
        }

        public void UpdateInput() {
            if (Default.Back.Pressed()) {
                if (_screen == MenuScreens.Main) {
                    _screen = MenuScreens.Quit;
                } else {
                    _screen = MenuScreens.Main;
                }
            }
        }

        private void SetupMainMenu() {
            Label.Put("AposGameStarter", 50);

            if (ButtonLabel.Put("Resume Game").Clicked) {

            }
            if (ButtonLabel.Put("Settings").Clicked) {
                _screen = MenuScreens.Settings;
            }
            if (ButtonLabel.Put("Debug").Clicked) {
                _screen = MenuScreens.Debug;
            }
            if (ButtonLabel.Put("Quit").Clicked) {
                _screen = MenuScreens.Quit;
            }
        }
        private void SetupSettingsMenu() {
            Label.Put("Settings", 50);

            Label.Put($"[Current UI scale: {GuiHelper.Scale}x]");

            if (ButtonLabel.Put($"FullScreen: {(Utility.Settings.IsFullscreen ? " true" : "false")}").Clicked) {
                Utility.ToggleFullscreen();
            }
            if (ButtonLabel.Put($"Borderless: {(Utility.Settings.IsBorderless ? " true" : "false")}").Clicked) {
                Utility.ToggleBorderless();
            }
            if (ButtonLabel.Put("UI Scale 1x").Clicked) {
                GuiHelper.Scale = 1f;
            }
            if (ButtonLabel.Put("UI Scale 2x").Clicked) {
                GuiHelper.Scale = 2f;
            }
            if (ButtonLabel.Put("UI Scale 3x").Clicked) {
                GuiHelper.Scale = 3f;
            }
            if (ButtonLabel.Put("UI Scale 4x").Clicked) {
                GuiHelper.Scale = 4f;
            }
            if (ButtonLabel.Put("Back").Clicked) {
                _screen = MenuScreens.Main;
            }
        }
        private void SetupDebugMenu() {
            Label.Put("Debug", 50);

            if (ButtonLabel.Put($"Show path line: {(Utility.ShowLine ? " true" : "false")}").Clicked) {
                Utility.ShowLine = !Utility.ShowLine;
            }
            if (ButtonLabel.Put("Back").Clicked) {
                _screen = MenuScreens.Main;
            }
        }
        private void SetupQuitConfirm() {
            Label.Put("Do you really want to quit?", 50);

            if (ButtonLabel.Put("Yes").Clicked) {
                InputHelper.Game.Exit();
            }
            if (ButtonLabel.Put("No").Clicked) {
                _screen = MenuScreens.Main;
            }
        }

        private class MenuUI(int id) : MenuPanel(id) {
            public override void Draw(GameTime gameTime) {
                GuiHelper.PushScissor(Clip);
                GuiHelper.ShapeBatch.FillRectangle(Bounds.Position, Bounds.Size, Color.Black * 0.6f);
                GuiHelper.ShapeBatch.BorderRectangle(Bounds.Position, Bounds.Size, Color.Black, 2f);
                GuiHelper.PopScissor();

                base.Draw(gameTime);
            }

            public static new MenuUI Push([CallerLineNumber] int id = 0, bool isAbsoluteId = false) {
                // 1. Check if ScreenPanel with id already exists.
                //      a. If already exists. Get it.
                //      b  If not, create it.
                // 3. Push it on the stack.
                // 4. Ping it.
                id = GuiHelper.CurrentIMGUI.CreateId(id, isAbsoluteId);
                GuiHelper.CurrentIMGUI.TryGetValue(id, out IComponent c);

                MenuUI a;
                if (c is MenuUI d) {
                    a = d;
                } else {
                    a = new MenuUI(id);
                }

                GuiHelper.CurrentIMGUI.GrabParent(a);

                GuiHelper.CurrentIMGUI.Push(a);

                return a;
            }
        }

        private enum MenuScreens {
            Main,
            Settings,
            Debug,
            Quit
        }

        private MenuScreens _screen = MenuScreens.Main;
    }
}
