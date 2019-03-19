using System;

using HSGomoku.Engine.Components;
using HSGomoku.Engine.UI;
using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Screen = HSGomoku.Engine.ScreenManage.Screen;
using ScreenManager = HSGomoku.Engine.ScreenManage.ScreenManager;

namespace HSGomoku.Engine.Screens
{
    internal class GameScreen : Screen
    {
        private Texture2D _board;
        private GameBoard _gameboard;
        private Button btnSurrender;
        private Button btnBack;

        private readonly GameHUD _gameHUD = new GameHUD();
        //private readonly FpsCounter _fpsCounter = new FpsCounter();

        public GameScreen(Game game) : base(game)
        {
            //Name = "GameScreen";
            //this.btnBack = new Button(
            //    this.content.Load<Texture2D>("img\\button_back"),
            //    new Vector2(800, 400),
            //    new Vector2(144, 72));
            //this.btnBack.Click += (s, e) =>
            //{
            //    ScreenManager.GoBack();
            //};
        }

        public override void Init()
        {
            this.btnSurrender = new Button(
                this._content.Load<Texture2D>("img\\button_surrender"),
                new Vector2(1440, 1290),
                new Vector2(144, 72));
            this.btnBack = new Button(
                this._content.Load<Texture2D>("img\\button_back"),
                new Vector2(1726, 1290),
                new Vector2(144, 72));
            this.btnBack.Click += (s, e) =>
            {
                ScreenManager.GoBack();
            };

            this.btnSurrender.Init();
            this.btnBack.Init();

            base.Init();
        }

        public override void LoadContent()
        {
            // 棋盘
            this._board = this._content.Load<Texture2D>("img\\board");

            // HUD
            this._gameHUD.Load(this._content, this._graphics);

            // GameBoard
            this._gameboard = new GameBoard(this._content);
            this._gameboard.WinningEvent += RaiseWinningEvent;

            //// FPS计数器
            //this._fpsCounter.Load(this.content, this.graphics);

            base.LoadContent();
        }

        public override void Shutdown()
        {
            this.btnSurrender = null;
            this.btnBack = null;
            base.Shutdown();
        }

        public override void Update(GameTime gameTime)
        {
            //this._fpsCounter.Update(gameTime);

            //if (Input.KeyPressed(Keys.W))
            //{
            //    ScreenManager.GotoScreen(nameof(StartScreen));
            //}
            if (this.btnSurrender != null)
            {
                this.btnSurrender.Update(gameTime);
            }
            if (this.btnBack != null)
            {
                this.btnBack.Update(gameTime);
            }
            this._gameboard.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //// 清理
            this._device.Clear(new Color(233, 203, 166));
            //Resolution.BeginDraw();

            // 背景棋盘
            //this._spriteBatch.Begin();
            this._spriteBatch.Begin(SpriteSortMode.BackToFront,
                                BlendState.AlphaBlend,
                                SamplerState.LinearClamp,
                                DepthStencilState.Default,
                                RasterizerState.CullNone,
                                null,
                                Resolution.GetTransformationMatrix());

            if (this._board != null)
            {
                this._spriteBatch.Draw(this._board, Vector2.Zero, Color.White);
            }
            if (this.btnSurrender != null)
            {
                this.btnSurrender.Draw(this._spriteBatch, gameTime);
            }
            if (this.btnBack != null)
            {
                this.btnBack.Draw(this._spriteBatch, gameTime);
            }

            this._spriteBatch.End();

            // HUD
            this._gameHUD.Draw(this._spriteBatch);

            this._gameboard.Draw(this._spriteBatch, gameTime);

            //// FPS计数器
            //this._fpsCounter.Draw(this.spriteBatch);

            base.Draw(gameTime);
        }

        public void RaiseWinningEvent(Object sender, EventArgs e)
        {
            new System.Threading.Tasks.Task(() =>
            {
                SDL2.SDL.SDL_ShowSimpleMessageBox(
                                SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                                "胜利",
                                "有一个玩家胜利了, 但是我还没写是谁, 而且我也没写以后的判断",
                                Game.Window.Handle);
            }).Start();
        }
    }
}