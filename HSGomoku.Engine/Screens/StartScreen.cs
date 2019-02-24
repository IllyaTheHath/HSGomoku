using HSGomoku.Engine.ScreenManage;
using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HSGomoku.Engine.Screens
{
    internal class StartScreen : Screen
    {
        public StartScreen(GraphicsDevice device, ContentManager content, GraphicsDeviceManager graphics)
            : base(device, content, graphics)
        {
            this.name = "StartScreen";
        }

        public override void Init()
        {
            base.Init();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.KeyPressed(Keys.S))
            {
                ScreenManager.GotoScreen(nameof(GameScreen));
            }
            if (Input.KeyPressed(Keys.D))
            {
                ScreenManager.GotoScreen(nameof(SettingScreen));
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //this.device.Clear(Color.CornflowerBlue);
            //Resolution.BeginDraw();

            base.Draw(gameTime);
        }
    }
}