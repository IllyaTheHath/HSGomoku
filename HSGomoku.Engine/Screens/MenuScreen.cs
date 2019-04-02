using HSGomoku.Engine.Components;
using HSGomoku.Engine.ScreenManage;
using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HSGomoku.Engine.Screens
{
    internal class MenuScreen : Screen
    {
        private Texture2D _background;
        private MenuButton _btnGame;
        private MenuButton _btnMultiGame;
        private MenuButton _btnSetting;
        private MenuButton _btnHelp;
        private MenuButton _btnExit;

        public MenuScreen(Game game) : base(game)
        {
        }

        public override void Init()
        {
            this._btnGame = new MenuButton(
                this._content.Load<Texture2D>("img\\menubutton_game"),
                new Vector2(40, 400),
                new Vector2(300, 150),
                nameof(SingleGameScreen));
            this._btnMultiGame = new MenuButton(
                this._content.Load<Texture2D>("img\\menubutton_multi_game"),
                new Vector2(100, 600),
                new Vector2(300, 150),
                nameof(MultiGameScreen));
            this._btnSetting = new MenuButton(
                this._content.Load<Texture2D>("img\\menubutton_setting"),
                new Vector2(160, 800),
                new Vector2(300, 150),
                nameof(SettingScreen));
            this._btnHelp = new MenuButton(
                this._content.Load<Texture2D>("img\\menubutton_help"),
                new Vector2(220, 1000),
                new Vector2(300, 150),
                nameof(HelpScreen));
            this._btnExit = new MenuButton(
                this._content.Load<Texture2D>("img\\menubutton_exit"),
                new Vector2(280, 1200),
                new Vector2(300, 150));
            this._btnExit.Click += (s, e) =>
            {
                Game.Exit();
            };

            this._btnGame.Init();
            this._btnMultiGame.Init();
            this._btnSetting.Init();
            this._btnHelp.Init();
            this._btnExit.Init();

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
            this._btnGame = null;
            this._btnMultiGame = null;
            this._btnSetting = null;
            this._btnHelp = null;
            this._btnExit = null;

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

            this._btnGame?.Update(gameTime);

            this._btnMultiGame?.Update(gameTime);

            this._btnSetting?.Update(gameTime);

            this._btnHelp?.Update(gameTime);

            this._btnExit?.Update(gameTime);

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

            this._spriteBatch?.Draw(this._background, Vector2.Zero, Color.White);

            this._btnGame?.Draw(this._spriteBatch, gameTime);

            this._btnMultiGame?.Draw(this._spriteBatch, gameTime);

            this._btnSetting?.Draw(this._spriteBatch, gameTime);

            this._btnHelp?.Draw(this._spriteBatch, gameTime);

            this._btnExit?.Draw(this._spriteBatch, gameTime);

            this._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}