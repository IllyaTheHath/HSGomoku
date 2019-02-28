using System;

using HSGomoku.Engine.ScreenManage;
using HSGomoku.Engine.Screens;
using HSGomoku.Engine.UI;
using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using static HSGomoku.Engine.Utilities.Statistics;

namespace HSGomoku.Engine
{
    internal sealed class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        public GraphicsDeviceManager GraphicsDeviceManager { get { return this._graphics; } }

        private readonly SpriteBatch _spriteBatch;
        public SpriteBatch SpriteBatch { get { return this._spriteBatch; } }

        //private Texture2D _board;

        //private readonly GameHUD _gameHUD = new GameHUD();
        private readonly FpsCounter _fpsCounter = new FpsCounter();

        public Game()
        {
            this._graphics = new GraphicsDeviceManager(this);
            this._spriteBatch = new SpriteBatch(GraphicsDevice);

            // 帧率
            CurrentGameFrameRate = GameFrameRate.F120;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / (Single)CurrentGameFrameRate);
            // 垂直同步
            this._graphics.SynchronizeWithVerticalRetrace = false;

            // 初始化字体
            FNAFont.InitFont();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // 分辨率
            Resolution.Init(ref this._graphics);
            Resolution.SetVirtualResolution(SupportResolution.P1440);
            Resolution.SetResolution(SupportResolution.P960, false);
            //Resolution.SetResolution(1024, 768, false);

            //Window.AllowUserResizing = true;
            IsMouseVisible = true;
            Window.Title = "Gomoku";

            // 屏幕管理
            ScreenManager.AddScreen(new SplashScreen(this));
            ScreenManager.AddScreen(new MenuScreen(this));
            ScreenManager.AddScreen(new GameScreen(this));
            ScreenManager.AddScreen(new SettingScreen(this));
            ScreenManager.AddScreen(new HelpScreen(this));

            ScreenManager.GotoScreen(nameof(MenuScreen));
            ScreenManager.Init();

            // 游戏状态
            CurrentGameState = GameState.Loading;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            ScreenManager.LoadContent();
            //this._board = Content.Load<Texture2D>("img\\board");

            //// HUD
            //this._gameHUD.Load(Content, this._graphics);

            // FPS计数器
            this._fpsCounter.Load(Content, this._graphics);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

#if DEBUG
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
#endif
            ScreenManager.Update(gameTime);

            this._fpsCounter.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // 清理
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Resolution.BeginDraw();

            ScreenManager.Draw(gameTime);

            ////this._spriteBatch.Begin();
            //this._spriteBatch.Begin(SpriteSortMode.BackToFront,
            //                        BlendState.AlphaBlend,
            //                        SamplerState.LinearClamp,
            //                        DepthStencilState.Default,
            //                        RasterizerState.CullNone,
            //                        null,
            //                        Resolution.GetTransformationMatrix());
            //// 背景棋盘
            //this._spriteBatch.Draw(this._board, Vector2.Zero, Color.White);
            //this._spriteBatch.End();

            //// HUD
            //this._gameHUD.Draw(this._spriteBatch);

            // FPS计数器
            this._fpsCounter.Draw(this._spriteBatch);

            base.Draw(gameTime);
        }
    }
}