using System;

using HSGomoku.Engine.Components;
using HSGomoku.Engine.ScreenManage;
using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static HSGomoku.Engine.Utilities.Statistics;

namespace HSGomoku.Engine.Screens
{
    internal class SettingScreen : Screen
    {
        private Button _btnRes;
        private Button _btnFrame;
        private Button _btnBack;

        public SettingScreen(Game game) : base(game)
        {
        }

        public override void Init()
        {
            this._btnRes = new Button(
                this._content.Load<Texture2D>("img\\button_res"),
                new Vector2(400, 400),
                new Vector2(144, 72));
            this._btnRes.Click += (s, e) =>
            {
                ChangeResolution();
            };
            this._btnFrame = new Button(
                this._content.Load<Texture2D>("img\\button_framerate"),
                new Vector2(400, 600),
                new Vector2(144, 72));
            this._btnFrame.Click += (s, e) =>
            {
                ChangeFrameRate();
            };
            this._btnBack = new Button(
                this._content.Load<Texture2D>("img\\button_back"),
                new Vector2(400, 800),
                new Vector2(144, 72));
            this._btnBack.Click += (s, e) =>
            {
                ScreenManager.GoBack();
            };

            this._btnRes.Init();
            this._btnFrame.Init();
            this._btnBack.Init();

            base.Init();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Shutdown()
        {
            this._btnRes = null;
            this._btnFrame = null;
            this._btnBack = null;

            base.Shutdown();
        }

        public override void Update(GameTime gameTime)
        {
            //if (Input.KeyPressed(Keys.A))
            //{
            //    ScreenManager.GotoScreen(nameof(StartScreen));
            //}

            //if (Input.KeyPressed(Keys.C))
            //{
            //    ChangeResolution();
            //}

            //if (Input.KeyPressed(Keys.F))
            //{
            //    ToggleFullScreen();
            //}

            this._btnRes?.Update(gameTime);

            this._btnFrame?.Update(gameTime);

            this._btnBack?.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this._device.Clear(new Color(233, 203, 166));
            //Resolution.BeginDraw();

            this._spriteBatch.Begin(SpriteSortMode.BackToFront,
                                BlendState.AlphaBlend,
                                SamplerState.LinearClamp,
                                DepthStencilState.Default,
                                RasterizerState.CullNone,
                                null,
                                Resolution.GetTransformationMatrix());

            this._btnRes?.Draw(this._spriteBatch, gameTime);

            this._btnFrame?.Draw(this._spriteBatch, gameTime);

            this._btnBack?.Draw(this._spriteBatch, gameTime);

            this._spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ChangeResolution()
        {
            if (CurrentResolution == SupportResolution.P1440)
            {
                Resolution.SetResolution(SupportResolution.P1080, false);
            }
            else
            {
                Resolution.SetResolution(SupportResolution.P1440, false);
            }
        }

        private void ChangeFrameRate()
        {
            if (CurrentGameFrameRate == GameFrameRate.F30)
            {
                CurrentGameFrameRate = GameFrameRate.F60;
            }
            else if (CurrentGameFrameRate == GameFrameRate.F60)
            {
                CurrentGameFrameRate = GameFrameRate.F120;
            }
            else if (CurrentGameFrameRate == GameFrameRate.F120)
            {
                CurrentGameFrameRate = GameFrameRate.F30;
            }
            Game.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / (Single)CurrentGameFrameRate);
        }

        //private void ToggleFullScreen()
        //{
        //    if (this.isFullScreen)
        //    {
        //        Resolution.SetResolution(1920, 1440, false);
        //        this.isFullScreen = false;
        //    }
        //    else
        //    {
        //        var a = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;
        //        Resolution.SetResolution(1920, 1440, true);
        //        this.isFullScreen = true;
        //    }
        //}
    }
}