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
    internal class SingleGameScreen : Screen, IGameScreen
    {
        private Texture2D _board;
        private Button btnSurrender;
        private Button btnBack;
        private GameHUD _gameHUD;

        private GameBoard _gameboard;
        private AI _ai;

        public SingleGameScreen(Game game) : base(game)
        {
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

            this._gameHUD = new GameHUD();

            // GameBoard
            this._gameboard = new GameBoard(this._content, this);

            // AI
            this._ai = new AI();

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

            base.LoadContent();
        }

        public override void Shutdown()
        {
            this._board = null;
            this.btnSurrender = null;
            this.btnBack = null;
            this._gameHUD = null;

            this._gameboard = null;
            this._ai = null;

            base.Shutdown();
        }

        public override void Update(GameTime gameTime)
        {
            this.btnSurrender?.Update(gameTime);
            this.btnBack?.Update(gameTime);
            this._gameboard?.Update(gameTime);

            if (CurrentPlayerState == PlayerState.White)
            {
                if (LastChessPosition.X == -1 || LastChessPosition.Y == -1)
                {
                    return;
                }
                this._ai.ComputerDo((Int32)LastChessPosition.X, (Int32)LastChessPosition.Y, out Int32 x, out Int32 y);
                PlaceChess(x, y);
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

            this._spriteBatch?.Draw(this._board, Vector2.Zero, Color.White);

            this.btnSurrender?.Draw(this._spriteBatch, gameTime);

            this.btnBack?.Draw(this._spriteBatch, gameTime);

            this._spriteBatch.End();

            // HUD
            this._gameHUD?.Draw(this._spriteBatch);

            this._gameboard?.Draw(this._spriteBatch, gameTime);

            base.Draw(gameTime);
        }

        public void RaiseWinningEvent(PlayerState p)
        {
            //new System.Threading.Tasks.Task(() =>
            //{
            CurrentPlayerState = PlayerState.None;
            SDL2.SDL.SDL_ShowSimpleMessageBox(
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
            SDL2.SDL.SDL_ShowSimpleMessageBox(
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

        public void PlaceChess(Int32 x, Int32 y)
        {
            if (CurrentPlayerState == PlayerState.None)
            {
                return;
            }
            if (!this._gameboard.CanPlace(x, y))
            {
                return;
            }
            var chessButton = this._gameboard.GetChessButton(x, y);
            if (chessButton == null)
            {
                return;
            }

            chessButton.HasChess = true;
            LastChessPosition = chessButton.BoardPosition;
            if (CurrentPlayerState == PlayerState.White)
            {
                chessButton.IsBlack = false;
            }
            else if (CurrentPlayerState == PlayerState.Black)
            {
                chessButton.IsBlack = true;
            }
            this._gameboard._chessNumber++;

            // 检测是否有玩家胜利
            var ctype = chessButton.IsBlack ? ChessType.Black : ChessType.White;

            var linkCount = this._gameboard.CheckLink(x, y, ctype);

            if (linkCount >= GameBoard.winChessCount)
            {
                RaiseWinningEvent(CurrentPlayerState);
            }
            else
            {
                // 检测是否平局
                if (this._gameboard._chessNumber == GameBoard.crossCount * GameBoard.crossCount)
                {
                    RaiseDrawEvent(this, new EventArgs());
                }
                else
                {
                    CurrentPlayerState = CurrentPlayerState == PlayerState.Black ? PlayerState.White : PlayerState.Black;
                }
            }
        }
    }
}