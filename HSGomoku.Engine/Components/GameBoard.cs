using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using static HSGomoku.Engine.Utilities.Statistics;

namespace HSGomoku.Engine.Components
{
    internal enum ChessType
    {
        None = 0,
        Black = 1,
        White = 2,
    }

    internal sealed class GameBoard
    {
        #region define

        // 15个交叉
        public static readonly Int32 crossCount = 15;

        // 棋盘大小
        public static readonly Int32 size = 1280;

        public static readonly Int32 halfSize = 640;

        // 交叉点大小
        public static readonly Single crossSize = size / (crossCount - 1);

        // 棋盘坐标系原点
        private static readonly Vector2 origin = new Vector2(80, 80);

        // 连5个子可以赢
        public static readonly Int32 winChessCount = 5;

        #endregion define

        //private readonly Int32[][] _map = new Int32[crossCount][];
        private readonly Dictionary<Vector2, ChessButton> _buttonMap = new Dictionary<Vector2, ChessButton>();

        //public delegate void winDele(PlayerState p);
        public event Action<PlayerState> WinningEvent;
        public event EventHandler DrawEvent;
        public Int32 _chessNumber;

        public GameBoard(ContentManager content)
        {
            for (Int32 x = 0; x < crossCount; ++x)
            {
                //this._map[x] = new Int32[crossCount];
                for (Int32 y = 0; y < crossCount; ++y)
                {
                    // 0 for none, 1 for black, 2 for white
                    //this._map[x][y] = 0;
                    //this._buttonMap.Clear();
                    Vector2 position = origin + new Vector2(crossSize * x, crossSize * y);
                    Vector2 size = new Vector2(crossSize, crossSize);
                    Vector2 boardPosition = new Vector2(x, y);
                    ChessButton button = new ChessButton(this, position, size, content, boardPosition);
                    button.Init();
                    this._buttonMap.Add(boardPosition, button);
                }
            }
            CurrentPlayerState = PlayerState.Black;
            this._chessNumber = 0;
        }

        public void Reset()
        {
            foreach (var button in this._buttonMap)
            {
                button.Value.HasChess = false;
                button.Value.IsBlack = true;
            }
            this._chessNumber = 0;
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

        public void PlaceChess(Int32 x, Int32 y)
        {
            if (CurrentPlayerState == PlayerState.None)
            {
                return;
            }
            if (!CanPlace(x, y))
            {
                return;
            }
            var chessButton = GetChessButton(x, y);
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
            this._chessNumber++;

            // 检测是否有玩家胜利
            var ctype = chessButton.IsBlack ? ChessType.Black : ChessType.White;

            var linkCount = CheckLink(x, y, ctype);

            if (linkCount >= winChessCount)
            {
                WinningEvent?.Invoke(CurrentPlayerState);
            }
            else
            {
                // 检测是否平局
                if (this._chessNumber == crossCount * crossCount)
                {
                    DrawEvent?.Invoke(this, new EventArgs());
                }
                else
                {
                    CurrentPlayerState = CurrentPlayerState == PlayerState.Black ? PlayerState.White : PlayerState.Black;
                }
            }
        }

        #region 棋盘数据

        public ChessButton GetChessButton(Int32 x, Int32 y)
        {
            if (x < 0 || x >= crossCount)
            {
                return null;
            }

            if (y < 0 || y >= crossCount)
            {
                return null;
            }

            return this._buttonMap[new Vector2(x, y)];
        }

        public ChessType GetChessType(Int32 x, Int32 y)
        {
            if (x < 0 || x >= crossCount)
            {
                return ChessType.None;
            }

            if (y < 0 || y >= crossCount)
            {
                return ChessType.None;
            }

            var chessButton = this._buttonMap[new Vector2(x, y)];
            return !chessButton.HasChess ? ChessType.None :
                chessButton.IsBlack ? ChessType.Black : ChessType.White;
        }

        public Boolean CanPlace(Int32 x, Int32 y)
        {
            return GetChessType(x, y) == ChessType.None;
        }

        #endregion 棋盘数据

        #region 检查连接情况

        // 检查垂直方向连接情况
        public Int32 CheckVerticalLink(Int32 px, Int32 py, ChessType type)
        {
            // 算上自己
            Int32 linkCount = 1;

            // 朝上
            for (Int32 y = py + 1; y < crossCount; y++)
            {
                if (GetChessType(px, y) == type)
                {
                    linkCount++;

                    if (linkCount >= winChessCount)
                    {
                        return linkCount;
                    }
                }
                else
                {
                    break;
                }
            }

            // 朝下
            for (Int32 y = py - 1; y >= 0; y--)
            {
                if (GetChessType(px, y) == type)
                {
                    linkCount++;

                    if (linkCount >= winChessCount)
                    {
                        return linkCount;
                    }
                }
                else
                {
                    break;
                }
            }

            return linkCount;
        }

        // 检查水平方向连接情况
        private Int32 CheckHorizentalLink(Int32 px, Int32 py, ChessType type)
        {
            Int32 linkCount = 1;

            // 朝右+
            for (Int32 x = px + 1; x < crossCount; x++)
            {
                if (GetChessType(x, py) == type)
                {
                    linkCount++;

                    if (linkCount >= winChessCount)
                    {
                        return linkCount;
                    }
                }
                else
                {
                    break;
                }
            }

            // 朝左
            for (Int32 x = px - 1; x >= 0; x--)
            {
                if (GetChessType(x, py) == type)
                {
                    linkCount++;

                    if (linkCount >= winChessCount)
                    {
                        return linkCount;
                    }
                }
                else
                {
                    break;
                }
            }

            return linkCount;
        }

        // 检查斜边情况
        private Int32 CheckBiasLink(Int32 px, Int32 py, ChessType type)
        {
            Int32 ret = 0;
            Int32 linkCount = 1;

            // 左下
            for (Int32 x = px - 1, y = py - 1; x >= 0 && y >= 0; x--, y--)
            {
                if (GetChessType(x, y) == type)
                {
                    linkCount++;

                    if (linkCount >= winChessCount)
                    {
                        return linkCount;
                    }
                }
                else
                {
                    break;
                }
            }

            // 右上
            for (Int32 x = px + 1, y = py + 1; x < crossCount && y < crossCount; x++, y++)
            {
                if (GetChessType(x, y) == type)
                {
                    linkCount++;

                    if (linkCount >= winChessCount)
                    {
                        return linkCount;
                    }
                }
                else
                {
                    break;
                }
            }

            ret = linkCount;
            linkCount = 1;
            // 左上
            for (Int32 x = px - 1, y = py + 1; x >= 0 && y < crossCount; x--, y++)
            {
                if (GetChessType(x, y) == type)
                {
                    linkCount++;

                    if (linkCount >= winChessCount)
                    {
                        return linkCount;
                    }
                }
                else
                {
                    break;
                }
            }

            // 右下
            for (Int32 x = px + 1, y = py - 1; x < crossCount && y >= 0; x++, y--)
            {
                if (GetChessType(x, y) == type)
                {
                    linkCount++;

                    if (linkCount >= winChessCount)
                    {
                        return linkCount;
                    }
                }
                else
                {
                    break;
                }
            }

            return Math.Max(ret, linkCount);
        }

        // 检查给定点周边的最大连接情况
        public Int32 CheckLink(Int32 px, Int32 py, ChessType type)
        {
            Int32 linkCount = 0;

            linkCount = Math.Max(CheckHorizentalLink(px, py, type), linkCount);
            linkCount = Math.Max(CheckVerticalLink(px, py, type), linkCount);
            linkCount = Math.Max(CheckBiasLink(px, py, type), linkCount);

            return linkCount;
        }

        #endregion 检查连接情况
    }
}