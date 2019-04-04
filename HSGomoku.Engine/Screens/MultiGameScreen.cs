using System;
using System.Linq;
using System.Threading;
using System.Timers;

using HSGomoku.Engine.Components;
using HSGomoku.Engine.ScreenManage;
using HSGomoku.Engine.UI;
using HSGomoku.Engine.Utilities;
using HSGomoku.Network;
using HSGomoku.Network.Messages;
using HSGomoku.Network.Utils;

using Lidgren.Network;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static HSGomoku.Engine.Utilities.Statistics;

using Timer = System.Timers.Timer;

namespace HSGomoku.Engine.Screens
{
    internal partial class MultiGameScreen : Screen, IGameScreen
    {
        private Texture2D _board;
        private Button _btnSurrender;
        private Button _btnBack;
        private GameHUD _gameHUD;
        private SpriteFontX _fontX;

        private GameBoard _gameboard;
        private NetworkClient _client;

        private Boolean _surrender;
        private Boolean _connected;

        private Int32 _connectTime;
        private Timer _connectTimer;

        public MultiGameScreen(Game game) : base(game)
        {
        }

        public override void Init()
        {
            this._surrender = false;
            this._connected = false;
            this._connectTime = 0;
            this._connectTimer = new Timer(100);
            this._connectTimer.Elapsed += Timer_Elapsed;

            this._btnSurrender = new Button(
                this._content.Load<Texture2D>("img\\button_surrender"),
                new Vector2(1440, 1290),
                new Vector2(144, 72));
            this._btnSurrender.Enabled = false;
            this._btnSurrender.Click += () =>
            {
                this._surrender = true;
            };
            this._btnBack = new Button(
                this._content.Load<Texture2D>("img\\button_back"),
                new Vector2(1726, 1290),
                new Vector2(144, 72));
            this._btnBack.Click += () =>
            {
                Reset();
                ScreenManager.GoBack();
            };

            this._btnSurrender.Init();
            this._btnBack.Init();

            this._gameHUD = new GameHUD();

            // GameBoard
            this._gameboard = new GameBoard(this._content, this);

            // 网络客户端
            this._client = new NetworkClient();
            this._client.Start();
            //this._client.Connect();
            this._client.DiscoverPeers();
            this._connectTimer.Start();
            CurrentPlayerState = PlayerState.None;
            PlayerType = PlayerState.None;

            base.Init();
        }

        private void Timer_Elapsed(Object sender, ElapsedEventArgs e)
        {
            this._connectTime += 100;
            if (this._connectTime > 3000 && !this._connected)
            {
                //  连接服务器失败
                this._connectTimer.Stop();
                this._connectTime = 0;
                SDL2.SDL.SDL_ShowSimpleMessageBox(
                            SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                            "连接服务器失败",
                            "连接服务器失败",
                            Game.Window.Handle);
                this._btnBack.InvokeClick();
            }
        }

        public override void LoadContent()
        {
            // 棋盘
            this._board = this._content.Load<Texture2D>("img\\board");

            // HUD
            this._gameHUD.Load(this._content, this._graphics);

            // 额外HUD文字
            this._fontX = new SpriteFontX(FNAFont.Font16, this._graphics);

            base.LoadContent();
        }

        public override void Shutdown()
        {
            // 通知服务器客户端已离开
            var msg = this._client?.CreateGameMessage<ClientLeaveMessage>();
            this._client?.SendMessage(msg);
            this._board = null;
            this._btnSurrender = null;
            this._btnBack = null;
            this._gameHUD = null;

            this._gameboard = null;

            this._connectTimer?.Stop();
            this._connectTimer?.Dispose();

            new Thread(() =>
            {
                // 防止客户端销毁时，消息还没能发送过去
                Thread.Sleep(10);
                this._client?.Shutdown();
            }).Start();
            PlayerType = PlayerState.None;

            base.Shutdown();
        }

        public override void Update(GameTime gameTime)
        {
            this._btnSurrender?.Update(gameTime);
            this._btnBack?.Update(gameTime);
            this._gameboard?.Update(gameTime);

            if (this._surrender)
            {
                this._surrender = false;
                RaiseSurrenderEvent();
            }

            // 接收服务器传来的消息
            NetIncomingMessage msg;
            while ((msg = this._client.NetClient.ReadMessage()) != null)
            {
                this._client.DecryptMessage(msg);
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        Console.WriteLine("Found server at " + msg.SenderEndPoint + " name: " + msg.ReadString());
                        this._client.Connect(msg.SenderEndPoint);
                        break;

                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(msg.ReadString());
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                        if (status == NetConnectionStatus.Connected)
                        {
                            Console.WriteLine("connected to" + msg.SenderConnection.RemoteUniqueIdentifier);
                            // 连接服务器成功，关闭监测线程
                            this._connected = true;
                            this._connectTimer.Stop();
                            this._connectTime = 0;
                            // 向服务器发送握手消息
                            SendClientJoinRequest();
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        var data = msg.Data;
                        var message = SerializeTools.Deserialize<GameMessage>(data);
                        //Console.WriteLine(message.ClientId + "-" + message.Content + "-" + (Int32)message.MsgCode);
                        OnMessage(message);
                        //this._client.SendMessage(this._client.CreateGameMessage<HelloMessage>());
                        break;
                }
                this._client.NetClient.Recycle(msg);
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
                this._spriteBatch?.Draw(this._board, Vector2.Zero, Color.White);
            }

            this._btnSurrender?.Draw(this._spriteBatch, gameTime);

            this._btnBack?.Draw(this._spriteBatch, gameTime);

            this._spriteBatch.End();

            // HUD
            this._gameHUD?.Draw(this._spriteBatch);

            // 额外HUD文字
            this._spriteBatch.DrawStringX(this._fontX, "游戏状态: ", new Vector2(1470, 1000), Color.Black);
            this._spriteBatch.DrawStringX(this._fontX, "", new Vector2(1500, 1050), Color.Black);

            this._gameboard?.Draw(this._spriteBatch, gameTime);

            base.Draw(gameTime);
        }

        private void RaiseSurrenderEvent()
        {
            this._btnSurrender.Enabled = false;
            var msg = this._client.CreateGameMessage<PlayerSurrenderMessage>();
            this._client.SendMessage(msg);
        }

        private void RaiseWinningEvent()
        {
            var msg = this._client.CreateGameMessage<GameEndMessage>();
            msg.ExtraData["Draw"] = false;
            this._client.SendMessage(msg);
        }

        private void RaiseDrawEvent()
        {
            var msg = this._client.CreateGameMessage<GameEndMessage>();
            msg.ExtraData["Draw"] = true;
            this._client.SendMessage(msg);
        }

        private void RaiseLeaveWinEventTrue()
        {
            CurrentPlayerState = PlayerState.None;
            SDL2.SDL.SDL_ShowSimpleMessageBox(
                            SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                            "游戏结束",
                            "对手离开了游戏, 你赢了",
                            Game.Window.Handle);
            this._btnBack.InvokeClick();
        }

        private void RaiseSurrenderWinEventTrue()
        {
            CurrentPlayerState = PlayerState.None;
            SDL2.SDL.SDL_ShowSimpleMessageBox(
                            SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                            "游戏结束",
                            "对手投降, 你赢了",
                            Game.Window.Handle);
            this._btnBack.InvokeClick();
        }

        private void RaiseSurrenderLostEventTrue()
        {
            CurrentPlayerState = PlayerState.None;
            SDL2.SDL.SDL_ShowSimpleMessageBox(
                            SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                            "游戏结束",
                            "你投降了, 你输了",
                            Game.Window.Handle);
            this._btnBack.InvokeClick();
        }

        private void RaiseWinningEventTrue()
        {
            CurrentPlayerState = PlayerState.None;
            SDL2.SDL.SDL_ShowSimpleMessageBox(
                            SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                            "游戏结束",
                            "你赢了",
                            Game.Window.Handle);
            this._btnBack.InvokeClick();
        }

        private void RaiseLostEventTrue()
        {
            new Thread(() =>
            {
                CurrentPlayerState = PlayerState.None;
                SDL2.SDL.SDL_ShowSimpleMessageBox(
                                SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                                "游戏结束",
                                "你输了",
                                Game.Window.Handle);
                this._btnBack.InvokeClick();
            }).Start();
        }

        private void RaiseDrawEventTrue()
        {
            CurrentPlayerState = PlayerState.None;
            SDL2.SDL.SDL_ShowSimpleMessageBox(
                            SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                            "游戏结束",
                            "平局",
                            Game.Window.Handle);
            this._btnBack.InvokeClick();
        }

        private void RaisePlaceChessEvent(Int32 x, Int32 y)
        {
            var msg = this._client.CreateGameMessage<PlayerPlaceChessMessage>();
            msg.Content = $"{CurrentPlayerState}-{LastChessPosition}";
            msg.ExtraData["X"] = x;
            msg.ExtraData["Y"] = y;
            this._client.SendMessage(msg);
            CurrentPlayerState = CurrentPlayerState == PlayerState.Black ? PlayerState.White : PlayerState.Black;
        }

        private void Reset()
        {
            this._surrender = false;
            this._gameboard?.Reset();

            LastChessPosition = new Vector2(-1, -1);
            CurrentPlayerState = PlayerState.Black;
            PlayerType = PlayerState.None;
        }

        public void PlaceChess(Int32 x, Int32 y, Boolean checkWin = true)
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
            if (checkWin)
            {
                var ctype = chessButton.IsBlack ? ChessType.Black : ChessType.White;

                var linkCount = this._gameboard.CheckLink(x, y, ctype);

                if (linkCount >= GameBoard.winChessCount)
                {
                    RaisePlaceChessEvent(x, y);
                    RaiseWinningEvent();
                }
                else
                {
                    // 检测是否平局
                    if (this._gameboard._chessNumber == GameBoard.crossCount * GameBoard.crossCount)
                    {
                        RaisePlaceChessEvent(x, y);
                        RaiseDrawEvent();
                    }
                    else
                    {
                        RaisePlaceChessEvent(x, y);
                    }
                }
            }
            else
            {
                CurrentPlayerState = CurrentPlayerState == PlayerState.Black ? PlayerState.White : PlayerState.Black;
            }
        }
    }
}