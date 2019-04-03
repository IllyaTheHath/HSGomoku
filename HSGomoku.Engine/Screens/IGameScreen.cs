using System;

namespace HSGomoku.Engine.Screens
{
    internal interface IGameScreen
    {
        void PlaceChess(Int32 x, Int32 y, Boolean checkWin = true);
    }
}