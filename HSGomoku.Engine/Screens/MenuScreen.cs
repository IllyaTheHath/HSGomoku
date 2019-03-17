using HSGomoku.Engine.Components;
using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Screen = HSGomoku.Engine.ScreenManage.Screen;

namespace HSGomoku.Engine.Screens
{
    internal class MenuScreen : Screen
    {
        private Texture2D _background;
        private MenuButton btnGame;
        private MenuButton btnSetting;
        private MenuButton btnHelp;
        private MenuButton btnExit;

        public MenuScreen(Game game) : base(game)
        {
            //Name = "StartScreen";
            //this.btnGame = new MenuButton(
            //    this.content.Load<Texture2D>("img\\menubutton_game"),
            //    new Vector2(150, 720),
            //    new Vector2(300, 150),
            //    nameof(GameScreen));
            //this.btnSetting = new MenuButton(
            //    this.content.Load<Texture2D>("img\\menubutton_setting"),
            //    new Vector2(225, 950),
            //    new Vector2(300, 150),
            //    nameof(SettingScreen));
            //this.btnExit = new MenuButton(
            //    this.content.Load<Texture2D>("img\\menubutton_exit"),
            //    new Vector2(300, 1180),
            //    new Vector2(300, 150));
            //this.btnExit.Click += (s, e) =>
            //{
            //    game.Exit();
            //};
        }

        public override void Init()
        {
            this.btnGame = new MenuButton(
                this._content.Load<Texture2D>("img\\menubutton_game"),
                new Vector2(100, 600),
                new Vector2(300, 150),
                nameof(GameScreen));
            this.btnSetting = new MenuButton(
                this._content.Load<Texture2D>("img\\menubutton_setting"),
                new Vector2(160, 800),
                new Vector2(300, 150),
                nameof(SettingScreen));
            this.btnHelp = new MenuButton(
                this._content.Load<Texture2D>("img\\menubutton_help"),
                new Vector2(220, 1000),
                new Vector2(300, 150),
                nameof(HelpScreen));
            this.btnExit = new MenuButton(
                this._content.Load<Texture2D>("img\\menubutton_exit"),
                new Vector2(280, 1200),
                new Vector2(300, 150));
            this.btnExit.Click += (s, e) =>
            {
                Game.Exit();
            };

            this.btnGame.Init();
            this.btnSetting.Init();
            this.btnHelp.Init();
            this.btnExit.Init();

            base.Init();
        }

        public override void LoadContent()
        {
            // 背景图片
            this._background = this._content.Load<Texture2D>("img\\main_background");

            base.LoadContent();
        }

        public override void Shutdown()
        {
            this.btnGame = null;
            this.btnSetting = null;
            this.btnHelp = null;
            this.btnExit = null;

            base.Shutdown();
        }

        public override void Update(GameTime gameTime)
        {
            //if (Input.KeyPressed(Keys.S))
            //{
            //    ScreenManager.GotoScreen(nameof(GameScreen));
            //}
            //if (Input.KeyPressed(Keys.D))
            //{
            //    ScreenManager.GotoScreen(nameof(SettingScreen));
            //}

            if (this.btnGame != null)
            {
                this.btnGame.Update(gameTime);
            }
            if (this.btnSetting != null)
            {
                this.btnSetting.Update(gameTime);
            }
            if (this.btnHelp != null)
            {
                this.btnHelp.Update(gameTime);
            }
            if (this.btnExit != null)
            {
                this.btnExit.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this._device.Clear(Color.CornflowerBlue);
            //Resolution.BeginDraw();

            this._spriteBatch.Begin(SpriteSortMode.BackToFront,
                                BlendState.AlphaBlend,
                                SamplerState.LinearClamp,
                                DepthStencilState.Default,
                                RasterizerState.CullNone,
                                null,
                                Resolution.GetTransformationMatrix());
            if (this._background != null)
            {
                this._spriteBatch.Draw(this._background, Vector2.Zero, Color.White);
            }

            if (this.btnGame != null)
            {
                this.btnGame.Draw(this._spriteBatch, gameTime);
            }
            if (this.btnSetting != null)
            {
                this.btnSetting.Draw(this._spriteBatch, gameTime);
            }
            if (this.btnHelp != null)
            {
                this.btnHelp.Draw(this._spriteBatch, gameTime);
            }
            if (this.btnExit != null)
            {
                this.btnExit.Draw(this._spriteBatch, gameTime);
            }

            this._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}