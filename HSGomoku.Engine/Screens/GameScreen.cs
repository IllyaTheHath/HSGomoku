using System;

using HSGomoku.Engine.Components;
using HSGomoku.Engine.Model;
using HSGomoku.Engine.ScreenManage;
using HSGomoku.Engine.UI;
using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static HSGomoku.Engine.Utilities.Statistics;

namespace HSGomoku.Engine.Screens
{
    internal class GameScreen : Screen
    {
        private Texture2D _board;
        private Button btnSurrender;
        private Button btnBack;

        private GameBoard _gameboard;
        private AI _ai;

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
            this.btnSurrender.Click += Surrender;
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

        private void Surrender(Object sender, EventArgs e)
        {
            CurrentPlayerState = PlayerState.None;
            var result = SDL2.SDL.SDL_ShowSimpleMessageBox(
                            SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                            "游戏结束",
                            "你输了",
                            Game.Window.Handle);
            Reset();
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
            this._gameboard.DrawEvent += RaiseDrawEvent;

            // AI
            this._ai = new AI();

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

            if (CurrentPlayerState == PlayerState.White)
            {
                if (LastChessPosition.X == -1 || LastChessPosition.Y == -1)
                {
                    return;
                }
                this._ai.ComputerDo((Int32)LastChessPosition.X, (Int32)LastChessPosition.Y, out Int32 x, out Int32 y);
                this._gameboard.PlaceChess(x, y);
            }

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

        public void RaiseWinningEvent(PlayerState p)
        {
            //new System.Threading.Tasks.Task(() =>
            //{
            CurrentPlayerState = PlayerState.None;
            var result = SDL2.SDL.SDL_ShowSimpleMessageBox(
                            SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                            "游戏结束",
                            $"{(p == PlayerState.Black ? "黑棋" : "白旗")}胜利了",
                            Game.Window.Handle);
            Reset();
            //}).Start();
        }

        private void RaiseDrawEvent(Object sender, EventArgs e)
        {
            CurrentPlayerState = PlayerState.None;
            var result = SDL2.SDL.SDL_ShowSimpleMessageBox(
                            SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                            "游戏结束",
                            "平局",
                            Game.Window.Handle);
            Reset();
        }

        public void Reset()
        {
            this._ai = new AI();
            this._gameboard.Reset();

            LastChessPosition = new Vector2(-1, -1);
            CurrentPlayerState = PlayerState.Black;
        }
    }
}