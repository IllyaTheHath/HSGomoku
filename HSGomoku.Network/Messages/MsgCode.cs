using System;

namespace HSGomoku.Network.Messages
{
    //返回值
    public static class GameServerReturnCode
    {
        public const Int32 OK = 0;
        public const Int32 DUPLICATE_USERNAME = 1000;
        public const Int32 USER_NOT_EXIST = 1001;
        public const Int32 CAN_NOT_FIRE_WHILE_NOT_MOVING = 1002;
        public const Int32 PLAYER_POSITION_HIT = 1003; //玩家碰撞
    }

    public static class MsgCode
    {
        public static readonly Int32 UserJoin = 1;
        public static readonly Int32 UserLeave = 2;

        public static readonly Int32 NewRound = 10;

        public static readonly Int32 ClientJoin = 100;
        public static readonly Int32 ClientLeave = 101;
    }
}