using System;
using Apos.Gui;
using Apos.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

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

            if (Button.Put("Resume Game").Clicked) {

            }
            if (Button.Put("Settings").Clicked) {
                _screen = MenuScreens.Settings;
            }
            if (Button.Put("Debug").Clicked) {
                _screen = MenuScreens.Debug;
            }
            if (Button.Put("Quit").Clicked) {
                _screen = MenuScreens.Quit;
            }
        }
        private void SetupSettingsMenu() {
            Label.Put("Settings", 50);

            Label.Put($"[Current UI scale: {GuiHelper.Scale}x]");

            if (Button.Put($"FullScreen: {(Utility.Settings.IsFullscreen ? " true" : "false")}").Clicked) {
                Utility.ToggleFullscreen();
            }
            if (Button.Put($"Borderless: {(Utility.Settings.IsBorderless ? " true" : "false")}").Clicked) {
                Utility.ToggleBorderless();
            }
            if (Button.Put("UI Scale 1x").Clicked) {
                QueueScale(1f);
            }
            if (Button.Put("UI Scale 2x").Clicked) {
                QueueScale(2f);
            }
            if (Button.Put("UI Scale 3x").Clicked) {
                QueueScale(3f);
            }
            if (Button.Put("UI Scale 4x").Clicked) {
                QueueScale(4f);
            }
            if (Button.Put("Back").Clicked) {
                _screen = MenuScreens.Main;
            }
        }
        private void SetupDebugMenu() {
            Label.Put("Debug", 50);

            if (Button.Put($"Show path line: {(Utility.ShowLine ? " true" : "false")}").Clicked) {
                Utility.ShowLine = !Utility.ShowLine;
            }
            if (Button.Put("Back").Clicked) {
                _screen = MenuScreens.Main;
            }
        }
        private void SetupQuitConfirm() {
            Label.Put("Do you really want to quit?", 50);

            if (Button.Put("Yes").Clicked) {
                InputHelper.Game.Exit();
            }
            if (Button.Put("No").Clicked) {
                _screen = MenuScreens.Main;
            }
        }

        private void QueueScale(float scale) {
            GuiHelper.CurrentIMGUI.QueueNextTick(() => {
                GuiHelper.Scale = scale;
            });
        }

        private class MenuUI : MenuPanel {
            public MenuUI(int id) : base(id) { }

            public override void Draw(GameTime gameTime) {
                GuiHelper.PushScissor(Clip);
                GuiHelper.SpriteBatch.FillRectangle(Bounds, Color.Black * 0.6f);
                GuiHelper.SpriteBatch.DrawRectangle(Bounds, Color.Black, 2f);
                GuiHelper.PopScissor();

                base.Draw(gameTime);
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
