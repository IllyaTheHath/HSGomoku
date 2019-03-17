using System;
using System.Linq;

using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using static HSGomoku.Engine.Utilities.Statistics;

namespace HSGomoku.Engine.UI
{
    internal sealed class GameHUD
    {
        private SpriteFontX _fontX;
        private SpriteFontX _fontXTitle;
        private Texture2D _pixel;

        public void Load(ContentManager content, GraphicsDeviceManager graphics)
        {
            this._fontX = new SpriteFontX(FNAFont.Font16, graphics);
            this._fontXTitle = new SpriteFontX(FNAFont.Font20, graphics);
            this._pixel = content.Load<Texture2D>("img\\pixel");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                                    BlendState.AlphaBlend,
                                    SamplerState.LinearClamp,
                                    DepthStencilState.Default,
                                    RasterizerState.CullNone,
                                    null,
                                    Resolution.GetTransformationMatrix());

            // 分割线
            //spriteBatch.Draw(this._pixel, new Vector2(1440, 720), new Rectangle(1440, 720, 430, 4), Color.Black);

            // 标题
            spriteBatch.DrawStringX(this._fontXTitle, "五子棋", new Vector2(1580, 50), Color.Black);

            // 玩家1信息
            spriteBatch.DrawStringX(this._fontX, "黑棋 :", new Vector2(1470, 150), Color.Black);
            spriteBatch.DrawStringX(this._fontX, "玩家1", new Vector2(1500, 200), Color.Black);
            spriteBatch.DrawStringX(this._fontX, "已走x步", new Vector2(1500, 250), Color.Black);

            // 玩家2信息
            spriteBatch.DrawStringX(this._fontX, "白棋 :", new Vector2(1470, 450), Color.Black);
            spriteBatch.DrawStringX(this._fontX, "玩家2", new Vector2(1500, 500), Color.Black);
            spriteBatch.DrawStringX(this._fontX, "已走x步", new Vector2(1500, 550), Color.Black);

            // 对局信息
            spriteBatch.DrawStringX(this._fontX, "您是：玩家1", new Vector2(1470, 750), Color.Black);

            String currentPlayer = String.Empty;
            if (CurrentPlayerState == PlayerState.None)
            {
                currentPlayer = "无";
            }
            if (CurrentPlayerState == PlayerState.Black)
            {
                currentPlayer = "玩家1";
            }
            if (CurrentPlayerState == PlayerState.White)
            {
                currentPlayer = "玩家2";
            }
            spriteBatch.DrawStringX(this._fontX, $"当前执子玩家：{currentPlayer}", new Vector2(1470, 800), Color.Black);
            String lastChessPosition = $"{Utils.NumberToAlphabet((Int32)LastChessPosition.X)}{(Int32)LastChessPosition.Y}";
            spriteBatch.DrawStringX(this._fontX, $"上个棋子落点：{lastChessPosition}", new Vector2(1470, 850), Color.Black);

            spriteBatch.End();
        }
    }
}