using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using static HSGomoku.Engine.Utilities.Statistics;

namespace HSGomoku.Engine.Components
{
    internal sealed class GameBoard
    {
        // 15个交叉
        public static readonly Int32 crossCount = 15;

        // 棋盘大小
        public static readonly Int32 size = 1280;

        public static readonly Int32 halfSize = 640;

        // 交叉点大小
        public static readonly Single crossSize = size / (crossCount - 1);

        // 棋盘坐标系原点
        private static readonly Vector2 origin = new Vector2(80, 80);

        private readonly Int32[][] _map = new Int32[crossCount][];
        private readonly Dictionary<Vector2, ChessButton> _buttonMap = new Dictionary<Vector2, ChessButton>();
        private static Vector2 lastPlay = new Vector2(-1, -1);

        public GameBoard(ContentManager content)
        {
            for (Int32 x = 0; x < crossCount; ++x)
            {
                this._map[x] = new Int32[crossCount];
                for (Int32 y = 0; y < crossCount; ++y)
                {
                    // 0 for none, 1 for black, 2 for white
                    this._map[x][y] = 0;
                    //this._buttonMap.Clear();
                    Vector2 position = origin + new Vector2(crossSize * x, crossSize * y);
                    Vector2 size = new Vector2(crossSize, crossSize);
                    Vector2 boardPosition = new Vector2(x, y);
                    ChessButton button = new ChessButton(position, size, content, boardPosition);
                    button.Init();
                    this._buttonMap.Add(boardPosition, button);
                }
            }
            CurrentPlayerState = PlayerState.Black;
        }

        public void Reset()
        {
            foreach (var button in this._buttonMap)
            {
                button.Value.HasChess = false;
                button.Value.IsBlack = true;
            }
            lastPlay = new Vector2(-1, -1);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var button in this._buttonMap)
            {
                button.Value.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var button in this._buttonMap)
            {
                button.Value.Draw(spriteBatch, gameTime);
            }
        }
    }
}