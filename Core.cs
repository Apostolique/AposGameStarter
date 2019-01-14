﻿using System;
using AposGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AposGameStarter
{
    public class Core : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch s;

        Menu menu;

        public Core()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Utility.game = this;
            Utility.Window = Window;
            Input.Setup();
            GuiHelper.Window = Window;
            GuiHelper.Scale = 1f;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += WindowClientChanged;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            s = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Assets.LoadAssets(Content);
            menu = new Menu();
        }

        private void WindowClientChanged(object sender, EventArgs e) {
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            // TODO: Add your update logic here
            menu.UpdateSetup();
            menu.UpdateInput();
            menu.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            menu.DrawUI(s);

            base.Draw(gameTime);
        }
    }
}
