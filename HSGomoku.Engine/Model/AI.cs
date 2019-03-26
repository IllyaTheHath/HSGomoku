using System;

using HSGomoku.Engine.Components;

namespace HSGomoku.Engine.Model
{
    internal class AI
    {
        // 15*15共有572种五子连珠的可能性
        private static readonly Int32 maxFiveChainCount = 572;

        //玩家的可能性
        private readonly Boolean[,,] _ptable = new Boolean[GameBoard.crossCount, GameBoard.crossCount, maxFiveChainCount];

        //电脑的可能性
        private readonly Boolean[,,] _ctable = new Boolean[GameBoard.crossCount, GameBoard.crossCount, maxFiveChainCount];

        //记录2位玩家所有可能的连珠数，-1则为永远无法5连珠
        private readonly Int32[,] _win = new Int32[2, maxFiveChainCount];

        //记录每格的分值
        private readonly Int32[,] _cgrades = new Int32[GameBoard.crossCount, GameBoard.crossCount];

        private readonly Int32[,] _pgrades = new Int32[GameBoard.crossCount, GameBoard.crossCount];

        //记录棋盘
        private readonly Int32[,] _board = new Int32[GameBoard.crossCount, GameBoard.crossCount];

        private Int32 _cgrade, _pgrade;
        private readonly Int32 _icount;
        private Int32 _m;
        private Int32 _n;
        private Int32 _mat, _nat, _mde, _nde;

        public AI()
        {
            for (Int32 i = 0; i < GameBoard.crossCount; i++)
            {
                for (Int32 j = 0; j < GameBoard.crossCount; j++)
                {
                    this._pgrades[i, j] = 0;
                    this._cgrades[i, j] = 0;
                    this._board[i, j] = 0;
                }
            }

            //遍历所有的五连子可能情况的权值
            //横
            for (Int32 i = 0; i < GameBoard.crossCount; i++)
            {
                for (Int32 j = 0; j < GameBoard.crossCount - 4; j++)
                {
                    for (Int32 k = 0; k < GameBoard.winChessCount; k++)
                    {
                        this._ptable[j + k, i, this._icount] = true;
                        this._ctable[j + k, i, this._icount] = true;
                    }

                    this._icount++;
                }
            }

            //横
            for (Int32 i = 0; i < GameBoard.crossCount; i++)
            {
                for (Int32 j = 0; j < GameBoard.crossCount - 4; j++)
                {
                    for (Int32 k = 0; k < GameBoard.winChessCount; k++)
                    {
                        this._ptable[i, j + k, this._icount] = true;
                        this._ctable[i, j + k, this._icount] = true;
                    }

                    this._icount++;
                }
            }

            // 右斜
            for (Int32 i = 0; i < GameBoard.crossCount - 4; i++)
            {
                for (Int32 j = 0; j < GameBoard.crossCount - 4; j++)
                {
                    for (Int32 k = 0; k < GameBoard.winChessCount; k++)
                    {
                        this._ptable[j + k, i + k, this._icount] = true;
                        this._ctable[j + k, i + k, this._icount] = true;
                    }

                    this._icount++;
                }
            }

            // 左斜
            for (Int32 i = 0; i < GameBoard.crossCount - 4; i++)
            {
                for (Int32 j = GameBoard.crossCount - 1; j >= 4; j--)
                {
                    for (Int32 k = 0; k < GameBoard.winChessCount; k++)
                    {
                        this._ptable[j - k, i + k, this._icount] = true;
                        this._ctable[j - k, i + k, this._icount] = true;
                    }

                    this._icount++;
                }
            }

            for (Int32 i = 0; i < 2; i++)
            {
                for (Int32 j = 0; j < maxFiveChainCount; j++)
                {
                    this._win[i, j] = 0;
                }
            }

            this._icount = 0;
        }

        private void CalcScore()
        {
            this._cgrade = 0;
            this._pgrade = 0;
            this._board[this._m, this._n] = 1;//电脑下子位置

            for (Int32 i = 0; i < maxFiveChainCount; i++)
            {
                if (this._ctable[this._m, this._n, i] && this._win[0, i] != -1)
                {
                    this._win[0, i]++;//给白子的所有五连子可能的加载当前连子数
                }

                if (this._ptable[this._m, this._n, i])
                {
                    this._ptable[this._m, this._n, i] = false;
                    this._win[1, i] = -1;
                }
            }
        }

        private void CalcCore()
        {
            //遍历棋盘上的所有坐标
            for (Int32 i = 0; i < GameBoard.crossCount; i++)
            {
                for (Int32 j = 0; j < GameBoard.crossCount; j++)
                {
                    //该坐标的黑子奖励积分清零
                    this._pgrades[i, j] = 0;

                    //在还没下棋子的地方遍历
                    if (this._board[i, j] == 0)
                    {
                        //遍历该棋盘可落子点上的黑子所有权值的连子情况，并给该落子点加上相应奖励分
                        for (Int32 k = 0; k < maxFiveChainCount; k++)
                        {
                            if (this._ptable[i, j, k])
                            {
                                switch (this._win[1, k])
                                {
                                    case 1://一连子
                                        this._pgrades[i, j] += 5;
                                        break;

                                    case 2://两连子
                                        this._pgrades[i, j] += 50;
                                        break;

                                    case 3://三连子
                                        this._pgrades[i, j] += 180;
                                        break;

                                    case 4://四连子
                                        this._pgrades[i, j] += 400;
                                        break;
                                }
                            }
                        }

                        this._cgrades[i, j] = 0;//该坐标的白子的奖励积分清零
                        if (this._board[i, j] == 0)//在还没下棋子的地方遍历
                        {
                            //遍历该棋盘可落子点上的白子所有权值的连子情况，并给该落子点加上相应奖励分
                            for (Int32 k = 0; k < maxFiveChainCount; k++)
                            {
                                if (this._ctable[i, j, k])
                                {
                                    switch (this._win[0, k])
                                    {
                                        case 1://一连子
                                            this._cgrades[i, j] += 5;
                                            break;

                                        case 2: //两连子
                                            this._cgrades[i, j] += 52;
                                            break;

                                        case 3://三连子
                                            this._cgrades[i, j] += 130;
                                            break;

                                        case 4: //四连子
                                            this._cgrades[i, j] += 10000;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // AI计算输出, 需要玩家走过的点
        public void ComputerDo(Int32 playerX, Int32 playerY, out Int32 finalX, out Int32 finalY)
        {
            setPlayerPiece(playerX, playerY);

            CalcCore();

            for (Int32 i = 0; i < GameBoard.crossCount; i++)
            {
                for (Int32 j = 0; j < GameBoard.crossCount; j++)
                {
                    //找出棋盘上可落子点的黑子白子的各自最大权值，找出各自的最佳落子点
                    if (this._board[i, j] == 0)
                    {
                        if (this._cgrades[i, j] >= this._cgrade)
                        {
                            this._cgrade = this._cgrades[i, j];
                            this._mat = i;
                            this._nat = j;
                        }

                        if (this._pgrades[i, j] >= this._pgrade)
                        {
                            this._pgrade = this._pgrades[i, j];
                            this._mde = i;
                            this._nde = j;
                        }
                    }
                }
            }

            //如果白子的最佳落子点的权值比黑子的最佳落子点权值大，则电脑的最佳落子点为白子的最佳落子点，否则相反
            if (this._cgrade >= this._pgrade)
            {
                this._m = this._mat;
                this._n = this._nat;
            }
            else
            {
                this._m = this._mde;
                this._n = this._nde;
            }

            CalcScore();

            finalX = this._m;
            finalY = this._n;
        }

        private void setPlayerPiece(Int32 playerX, Int32 playerY)
        {
            Int32 m = playerX;
            Int32 n = playerY;

            if (this._board[m, n] == 0)
            {
                this._board[m, n] = 2;

                for (Int32 i = 0; i < maxFiveChainCount; i++)
                {
                    if (this._ptable[m, n, i] && this._win[1, i] != -1)
                    {
                        this._win[1, i]++;
                    }
                    if (this._ctable[m, n, i])
                    {
                        this._ctable[m, n, i] = false;
                        this._win[0, i] = -1;
                    }
                }
            }
        }
    }
}