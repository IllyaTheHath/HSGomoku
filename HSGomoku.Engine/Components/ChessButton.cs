using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HSGomoku.Engine.Components
{
    internal class ChessButton : ClickableControl
    {
        private readonly Texture2D _blackTexture;
        private readonly Texture2D _whiteTexture;

        public Boolean HasChess { get; set; }
        public Boolean IsBlack { get; set; }
        public Vector2 BoardPosition { get; }

        private readonly GameBoard gameBoard;

        private ChessButton() : base()
        {
            HasChess = false;
            IsBlack = false;
            this.Click += ChessButton_Click;
        }

        public ChessButton(GameBoard gameBoard, Vector2 position, Vector2 size, ContentManager content, Vector2 boardPosition) : this()
        {
            this.gameBoard = gameBoard;
            this._blackTexture = content.Load<Texture2D>("img\\chess_black");
            this._whiteTexture = content.Load<Texture2D>("img\\chess_white");
            this.size = size;
            this.position = position - this.size / 2;
            BoardPosition = boardPosition;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (HasChess)
            {
                this._texture = IsBlack ? this._blackTexture : this._whiteTexture;

                base.Draw(spriteBatch, gameTime);
            }
        }

        private void ChessButton_Click(Object sender, EventArgs e)
        {
            this.gameBoard.GameScreen.PlaceChess((Int32)BoardPosition.X, (Int32)BoardPosition.Y);
            //this.gameBoard.PlaceChess((Int32)BoardPosition.X, (Int32)BoardPosition.Y);
            //if (CurrentPlayerState == PlayerState.None)
            //{
            //    return;
            //}
            //if (HasChess)
            //{
            //    return;
            //}

            //HasChess = true;
            //LastChessPosition = BoardPosition;
            //if (CurrentPlayerState == PlayerState.White)
            //{
            //    IsBlack = false;
            //    CurrentPlayerState = PlayerState.Black;
            //}
            //else if (CurrentPlayerState == PlayerState.Black)
            //{
            //    IsBlack = true;
            //    CurrentPlayerState = PlayerState.White;
            //}
        }
    }
}