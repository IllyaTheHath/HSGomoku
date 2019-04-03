using System;

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

namespace HSGomoku.Engine.Screens
{
    internal class MultiGameScreen : Screen, IGameScreen
    {
        private Texture2D _board;
        private Button _btnSurrender;
        private Button _btnBack;
        private GameHUD _gameHUD;

        private GameBoard _gameboard;
        private NetworkClient _client;

        private Boolean _surrender = false;

        public MultiGameScreen(Game game) : base(game)
        {
        }

        public override void Init()
        {
            this._btnSurrender = new Button(
                this._content.Load<Texture2D>("img\\button_surrender"),
                new Vector2(1440, 1290),
                new Vector2(144, 72));
            this._btnSurrender.Click += (s, e) =>
            {
                this._surrender = true;
            };
            this._btnBack = new Button(
                this._content.Load<Texture2D>("img\\button_back"),
                new Vector2(1726, 1290),
                new Vector2(144, 72));
            this._btnBack.Click += (s, e) =>
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

            base.Init();
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
            this._btnSurrender = null;
            this._btnBack = null;
            this._gameHUD = null;

            this._gameboard = null;

            this._client?.Shutdown();

            base.Shutdown();
        }

        public override void Update(GameTime gameTime)
        {
            this._btnSurrender?.Update(gameTime);
            this._btnBack?.Update(gameTime);
            this._gameboard?.Update(gameTime);

            if (this._surrender)
            {
                RaiseSurrenderEvent(CurrentPlayerState);
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
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        var data = msg.Data;
                        var message = SerializeTools.Deserialize<GameMessage>(data);
                        //Console.WriteLine(message.ClientId + "-" + message.Content + "-" + (Int32)message.MsgCode);
                        OnMessage(message);
                        this._client.SendMessage(this._client.CreateGameMessage<HelloMessage>());
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

            this._spriteBatch?.Draw(this._board, Vector2.Zero, Color.White);

            this._btnSurrender?.Draw(this._spriteBatch, gameTime);

            this._btnBack?.Draw(this._spriteBatch, gameTime);

            this._spriteBatch.End();

            // HUD
            this._gameHUD?.Draw(this._spriteBatch);

            this._gameboard?.Draw(this._spriteBatch, gameTime);

            base.Draw(gameTime);
        }

        private void OnMessage(GameMessage message)
        {
            SDL2.SDL.SDL_ShowSimpleMessageBox(
                            SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                            "OnMessage",
                            message.ClientId + "-" + message.MsgCode + "-" + message.Content,
                            Game.Window.Handle);
        }

        private void RaiseSurrenderEvent(PlayerState p)
        {
            CurrentPlayerState = PlayerState.None;
            var result = SDL2.SDL.SDL_ShowSimpleMessageBox(
                            SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                            "游戏结束",
                            $"{(p == PlayerState.Black ? "黑棋" : "白旗")}认输了",
                            Game.Window.Handle);
            Reset();
        }

        public void RaiseWinningEvent(PlayerState p)
        {
            var msg = this._client.CreateGameMessage<GameEndMessage>();
            this._client.SendMessage(msg);
            CurrentPlayerState = PlayerState.None;
            SDL2.SDL.SDL_ShowSimpleMessageBox(
                            SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                            "游戏结束",
                            $"{(p == PlayerState.Black ? "黑棋" : "白旗")}胜利了",
                            Game.Window.Handle);
            Reset();
        }

        private void RaiseDrawEvent()
        {
            var msg = this._client.CreateGameMessage<GameEndMessage>();
            this._client.SendMessage(msg);
            CurrentPlayerState = PlayerState.None;
            SDL2.SDL.SDL_ShowSimpleMessageBox(
                            SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
                            "游戏结束",
                            "平局",
                            Game.Window.Handle);
            Reset();
        }

        private void RaisePlaceChessEvent()
        {
            var msg = this._client.CreateGameMessage<PlayerPlaceChessMessage>();
            msg.Content = $"{CurrentPlayerState}-{LastChessPosition}";
            this._client.SendMessage(msg);
            CurrentPlayerState = CurrentPlayerState == PlayerState.Black ? PlayerState.White : PlayerState.Black;
        }

        public void Reset()
        {
            this._surrender = false;
            this._gameboard?.Reset();

            LastChessPosition = new Vector2(-1, -1);
            CurrentPlayerState = PlayerState.Black;
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
                    RaiseWinningEvent(CurrentPlayerState);
                }
                else
                {
                    // 检测是否平局
                    if (this._gameboard._chessNumber == GameBoard.crossCount * GameBoard.crossCount)
                    {
                        RaiseDrawEvent();
                    }
                    else
                    {
                        RaisePlaceChessEvent();
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