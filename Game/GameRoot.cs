using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Apos.Gui;
using System;

namespace GameProject {
    public class GameRoot : Game {
        public GameRoot() {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            Utility.Settings = Utility.EnsureJson<Settings>("Settings.json");
            Utility.Graphics = _graphics;
            Utility.Window = Window;

            IsFixedTimeStep = Utility.Settings.IsFixedTimeStep;
            _graphics.SynchronizeWithVerticalRetrace = Utility.Settings.IsVSync;

            Utility.Settings.IsFullscreen = Utility.Settings.IsFullscreen || Utility.Settings.IsBorderless;

            Utility.RestoreWindow();
            if (Utility.Settings.IsFullscreen) {
                Utility.ApplyFullscreenChange(false);
            }

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += WindowClientChanged;

            base.Initialize();
        }

        protected override void LoadContent() {
            _s = new SpriteBatch(GraphicsDevice);

            Assets.LoadAssets(Content, GraphicsDevice);
            GuiHelper.Setup(this, Assets.FontSystem);

            _ui = new IMGUI();
            _menu = new Menu();
        }

        protected override void UnloadContent() {
            Utility.SaveJson<Settings>("Settings.json", Utility.Settings);

            base.UnloadContent();
        }

        private void WindowClientChanged(object sender, EventArgs e) {
            if (!Utility.Settings.IsFullscreen) {
                Utility.SaveWindow();
            }
        }

        protected override void Update(GameTime gameTime) {
            GuiHelper.UpdateSetup(gameTime);
            _ui.UpdateAll(gameTime);

            _menu.CreateMenu();

            if (Triggers.ToggleFullscreen.Pressed()) {
                Utility.ToggleFullscreen();
            }
            if (Triggers.ToggleBorderless.Pressed()) {
                Utility.ToggleBorderless();
            }

            _menu.UpdateInput();

            GuiHelper.UpdateCleanup();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            _ui.Draw(gameTime);

            base.Draw(gameTime);
        }

        GraphicsDeviceManager _graphics;
        SpriteBatch _s;

        IMGUI _ui;
        Menu _menu;
    }
}
