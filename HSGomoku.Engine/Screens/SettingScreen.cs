using System;
using System.Linq;

using HSGomoku.Engine.ScreenManage;
using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HSGomoku.Engine.Screens
{
    internal class SettingScreen : Screen
    {
        public SettingScreen(GraphicsDevice device, ContentManager content, GraphicsDeviceManager graphics)
            : base(device, content, graphics)
        {
            this.name = "SettingScreen";
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
            if (Input.KeyPressed(Keys.A))
            {
                ScreenManager.GotoScreen(nameof(StartScreen));
            }

            if (Input.KeyPressed(Keys.C))
            {
                ChangeResolution();
            }

            if (Input.KeyPressed(Keys.F))
            {
                ToggleFullScreen();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.device.Clear(new Color(233, 203, 166));
            //Resolution.BeginDraw();

            base.Draw(gameTime);
        }

        private Boolean is720 = true;
        private Boolean isFullScreen = false;

        private void ChangeResolution()
        {
            if (this.is720)
            {
                Resolution.SetResolution(1920, 1080, this.isFullScreen);
                this.is720 = false;
            }
            else
            {
                Resolution.SetResolution(960, 720, this.isFullScreen);
                this.is720 = true;
            }
        }

        private void ToggleFullScreen()
        {
            if (this.isFullScreen)
            {
                Resolution.SetResolution(1920, 1440, false);
                this.isFullScreen = false;
            }
            else
            {
                var a = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;
                Resolution.SetResolution(a.Last().Width, a.Last().Height, true);
                this.isFullScreen = true;
            }
        }
    }
}