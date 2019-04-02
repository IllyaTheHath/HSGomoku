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
    public sealed class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        public GraphicsDeviceManager GraphicsDeviceManager { get { return this._graphics; } }

        private readonly SpriteBatch _spriteBatch;
        public SpriteBatch SpriteBatch { get { return this._spriteBatch; } }

        private readonly FpsCounter _fpsCounter = new FpsCounter();

        public String Title { get; set; } = "Anaki ♂ Gomoku";

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

            IsMouseVisible = true;
            Window.Title = Title;

            // 屏幕管理
            ScreenManager.AddScreen(new SplashScreen(this));
            ScreenManager.AddScreen(new MenuScreen(this));
            ScreenManager.AddScreen(new SingleGameScreen(this));
            ScreenManager.AddScreen(new MultiGameScreen(this));
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

            // FPS计数器
            this._fpsCounter.Load(Content, this._graphics);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
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
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (IsActive)
            {
                // 清理
                GraphicsDevice.Clear(Color.CornflowerBlue);
                Resolution.BeginDraw();

                ScreenManager.Draw(gameTime);

                // FPS计数器
                this._fpsCounter.Draw(this._spriteBatch);
            }

            base.Draw(gameTime);
        }

        protected override void Dispose(Boolean disposing)
        {
            base.Dispose(disposing);
            for (Int32 i = 0; i < ScreenManager.GetScreenNumber(); i++)
            {
                ScreenManager.GetScreen(i)?.Shutdown();
            }
        }
    }
}