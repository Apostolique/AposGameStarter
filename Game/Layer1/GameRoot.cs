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
            Utility.Settings = new Settings();
            Utility.Game = this;
            Utility.Window = Window;
            Utility.Graphics = _graphics;

            _graphics.PreferredBackBufferWidth = Utility.Settings.Width;
            _graphics.PreferredBackBufferHeight = Utility.Settings.Height;
            _graphics.ApplyChanges();

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += WindowClientChanged;

            base.Initialize();
        }

        protected override void LoadContent() {
            _s = new SpriteBatch(GraphicsDevice);

            Assets.LoadAssets(Content, GraphicsDevice);
            GuiHelper.Setup(this, Assets.FontSystem);
            _menu = new Menu();
        }

        private void WindowClientChanged(object sender, EventArgs e) { }

        protected override void Update(GameTime gameTime) {
            GuiHelper.UpdateSetup();

            _menu.UpdateSetup();
            _menu.UpdateInput();
            _menu.Update();

            GuiHelper.UpdateCleanup();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            _menu.DrawUI();

            base.Draw(gameTime);
        }

        GraphicsDeviceManager _graphics;
        SpriteBatch _s;

        Menu _menu;
    }
}
