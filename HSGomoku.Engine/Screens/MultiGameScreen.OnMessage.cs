using System;

using HSGomoku.Engine.ScreenManage;
using HSGomoku.Network.Messages;

using static HSGomoku.Engine.Utilities.Statistics;

namespace HSGomoku.Engine.Screens
{
    internal partial class MultiGameScreen : Screen, IGameScreen
    {
        private void SendClientJoinRequest()
        {
            var msg = this._client.CreateGameMessage<ClientJoinMessage>();
            this._client.SendMessage(msg);
        }

        private void OnMessage(GameMessage message)
        {
            //SDL2.SDL.SDL_ShowSimpleMessageBox(
            //                SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
            //                "OnMessage",
            //                message.ClientId + "-" + message.MsgCode + "-" + message.Content,
            //                Game.Window.Handle);
            switch (message.MsgCode)
            {
                case MsgCode.ClientJoin:
                    // 与服务器建立连接，发送匹配申请
                    ClientJoin();
                    break;

                // 对手离开了游戏
                case MsgCode.ClientLeave:
                    ClientLeave();
                    break;
                // 并没有匹配成功，等待匹配中
                case MsgCode.ClientMatch:
                    ClientMatch();
                    break;

                case MsgCode.ClientMatchSuccess:
                    ClientMatchSuccess(message);
                    break;

                case MsgCode.GameEnd:
                    GameEnd(message);
                    break;

                case MsgCode.PlayerPlaceChess:
                    PlayerPlaceChess(message);
                    break;

                case MsgCode.PlayerSurrender:
                    PlayerSurrender(message);
                    break;

                // 不会从服务器发来的消息
                case MsgCode.GameStart:
                case MsgCode.ServerRespondJoin:
                case MsgCode.ServerShutdown:
                // 无意义消息
                case MsgCode.Hello:
                case MsgCode.EmptyMessage:
                default:
                    // 客户端不用做任何事情
                    break;
            }
        }

        private void ClientJoin()
        {
            this._gameStatus = "向服务器发送匹配请求...";
            var msg = this._client.CreateGameMessage<ClientMatchMessage>();
            this._client.SendMessage(msg);
        }

        private void ClientMatch()
        {
            this._gameStatus = "正在匹配对手...";
        }

        private void ClientLeave()
        {
            RaiseLeaveWinEventTrue();
        }

        private void ClientMatchSuccess(GameMessage message)
        {
            var isBlack = (Boolean)message.ExtraData["IsBlack"];
            PlayerType = isBlack ? PlayerState.Black : PlayerState.White;
            CurrentPlayerState = PlayerState.Black;
            var msg = this._client.CreateGameMessage<GameStartMessage>();
            this._client.SendMessage(msg);
            this._gameStatus = "匹配成功，开始游戏";

            // 允许投降
            this._btnSurrender.Enabled = true;
        }

        private void GameEnd(GameMessage message)
        {
            this._gameStatus = "游戏结束";
            var draw = (Boolean)message.ExtraData["Draw"];
            if (draw)
            {
                RaiseDrawEventTrue();
            }
            else
            {
                var win = (Boolean)message.ExtraData["Win"];
                if (win)
                {
                    RaiseWinningEventTrue();
                }
                else
                {
                    RaiseLostEventTrue();
                }
            }
        }

        private void PlayerPlaceChess(GameMessage message)
        {
            var x = (Int32)message.ExtraData["X"];
            var y = (Int32)message.ExtraData["Y"];
            PlaceChess(x, y, false);
        }

        private void PlayerSurrender(GameMessage message)
        {
            var win = (Boolean)message.ExtraData["Win"];
            if (win)
            {
                RaiseSurrenderWinEventTrue();
            }
            else
            {
                RaiseSurrenderLostEventTrue();
            }
        }
    }
}