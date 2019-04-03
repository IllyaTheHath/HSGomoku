using System;
using System.Collections.Generic;
using System.Linq;

using HSGomoku.Network;
using HSGomoku.Network.Messages;

namespace HSGomoku.Server
{
    public class Program
    {
        private static HashSet<Int64> connected;
        private static HashSet<Int64> matching;
        private static Dictionary<Int64, Int64> matched;

        private static NetworkServer server;

        private static void Main(String[] args)
        {
            connected = new HashSet<Int64>();
            matching = new HashSet<Int64>();
            matched = new Dictionary<Int64, Int64>();

            server = new NetworkServer();
            server.Start();
            server.OnGameMessage += HandleMessage;

            while (true)
            {
                var line = Console.ReadLine();
                if (line == "send")
                {
                    var msg = server.CreateGameMessage<HelloMessage>();
                    server.SendMessage(msg);
                }
                if (line == "exit")
                {
                    break;
                }
            }
            server.Shutdown();
        }

        private static void HandleMessage(GameMessage message)
        {
            switch (message.MsgCode)
            {
                case MsgCode.ClientJoin:
                    ClientJoin(message.ClientId);
                    break;

                case MsgCode.ClientLeave:
                    ClientLeave(message.ClientId);
                    break;

                case MsgCode.ClientMatch:
                    MatchClients(message.ClientId);
                    break;

                case MsgCode.GameStart:
                    // 客户端告知服务器游戏已经正常开始
                    // 服务器不用做任何事情
                    break;

                case MsgCode.GameEnd:
                    // 哪个客户端发来游戏结束消息，说明哪个客户端获得胜利
                    GameEnd(message.ClientId);
                    break;

                case MsgCode.PlayerPlaceChess:
                    PlayerPlaceChess(message);
                    break;

                case MsgCode.PlayerSurrender:
                    PlayerSurrender(message.ClientId);
                    break;

                // 不会从客户端发来的消息
                case MsgCode.ClientMatchSuccess:
                case MsgCode.ServerRespondJoin:
                case MsgCode.ServerShutdown:
                // 无意义消息
                case MsgCode.Hello:
                case MsgCode.EmptyMessage:
                default:
                    // 服务器不用做任何事情
                    break;
            }
        }

        private static void ClientJoin(Int64 clientId)
        {
            connected.Add(clientId);
            var msg = server.CreateGameMessage<ClientJoinMessage>();
            server.SendMessage(msg, server.GetClient(clientId));
        }

        private static void ClientLeave(Int64 clientId)
        {
            Int64 matchedId = 0;
            if (matching.Contains(clientId))
            {
                matching.Remove(clientId);
            }
            else if (matched.ContainsKey(clientId))
            {
                matchedId = matched[clientId];
                matched.Remove(clientId);
            }
            else if (matched.ContainsValue(clientId))
            {
                matchedId = matched.First(m => m.Value == clientId).Key;
                matched.Remove(matchedId);
            }
            connected.Remove(clientId);
            connected.Remove(matchedId);
            var msg = server.CreateGameMessage<ClientLeaveMessage>();
            server.SendMessage(msg, server.GetClient(matchedId));
        }

        private static void MatchClients(Int64 clientId)
        {
            lock (matching)
            {
                // 之前没有人匹配，直接放入匹配队列
                if (matching.Count == 0)
                {
                    matching.Add(clientId);
                    var msg = server.CreateGameMessage<ClientMatchMessage>();
                    server.SendMessage(msg, server.GetClient(clientId));
                }
                // 否则，选取第一个人进行匹配
                else
                {
                    var matchedId = matching.First();
                    matching.Remove(matchedId);
                    matched.Add(clientId, matchedId);
                    // 匹配成功，向双方发送消息，其中包含系统分配到的棋子类型
                    var msg = server.CreateGameMessage<ClientMatchSuccessMessage>();
                    msg.ExtraData["IsBlack"] = new Random().NextDouble() > 0.5;
                    server.SendMessage(msg, server.GetClient(clientId));
                    msg.ExtraData["IsBlack"] = !(Boolean)msg.ExtraData["IsBlack"];
                    server.SendMessage(msg, server.GetClient(matchedId));
                }
            }
        }

        private static void GameEnd(Int64 clientId)
        {
            Int64 matchedId = 0;
            if (matched.ContainsKey(clientId))
            {
                matchedId = matched[clientId];
                // 游戏结束时将双方移出游戏队列
                matched.Remove(clientId);
            }
            else if (matched.ContainsValue(clientId))
            {
                matchedId = matched.First(m => m.Value == clientId).Key;
                matched.Remove(matchedId);
            }
            connected.Remove(clientId);
            connected.Remove(matchedId);
            var msg = server.CreateGameMessage<GameEndMessage>();
            msg.ExtraData["Win"] = true;
            server.SendMessage(msg, server.GetClient(clientId));
            msg.ExtraData["Win"] = false;
            server.SendMessage(msg, server.GetClient(matchedId));
        }

        private static void PlayerPlaceChess(GameMessage message)
        {
            var clientId = message.ClientId;
            Int64 matchedId = 0;
            if (matched.ContainsKey(clientId))
            {
                matchedId = matched[clientId];
            }
            else if (matched.ContainsValue(clientId))
            {
                matchedId = matched.First(m => m.Value == clientId).Key;
            }
            var msg = server.CreateGameMessage<PlayerPlaceChessMessage>();
            msg.ExtraData["X"] = message.ExtraData["X"];
            msg.ExtraData["Y"] = message.ExtraData["Y"];
            server.SendMessage(msg, server.GetClient(matchedId));
        }

        private static void PlayerSurrender(Int64 clientId)
        {
            Int64 matchedId = 0;
            if (matched.ContainsKey(clientId))
            {
                matchedId = matched[clientId];
                // 玩家投降时将双方移出游戏队列
                matched.Remove(clientId);
            }
            else if (matched.ContainsValue(clientId))
            {
                matchedId = matched.First(m => m.Value == clientId).Key;
                matched.Remove(matchedId);
            }
            connected.Remove(clientId);
            connected.Remove(matchedId);
            var msg = server.CreateGameMessage<PlayerSurrenderMessage>();
            server.SendMessage(msg, server.GetClient(matchedId));
        }
    }
}