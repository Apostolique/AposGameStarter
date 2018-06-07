using AposGameCheatSheet.AposGui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AposGameCheatSheet
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
            Utility.Input = new Input();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            s = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Assets.LoadAssets(Content);
            menu = new Menu();
        }

        protected override void Update(GameTime gameTime)
        {
            Utility.Input.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
            s.Begin();
            menu.DrawUI(s);
            s.End();

            base.Draw(gameTime);
        }
    }
}
