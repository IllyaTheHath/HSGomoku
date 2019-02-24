using System.Linq;

using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HSGomoku.Engine.UI
{
    internal sealed class GameHUD
    {
        private SpriteFontX _fontX;

        public void Load(ContentManager content, GraphicsDeviceManager graphics)
        {
            this._fontX = new SpriteFontX(FNAFont.Font18, graphics);
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
            spriteBatch.DrawStringX(this._fontX, "游戏HUD", new Vector2(1500, 60), Color.Black);
            spriteBatch.DrawStringX(this._fontX, "中文测试", new Vector2(1500, 120), Color.Black);
            spriteBatch.DrawStringX(this._fontX, "English Test", new Vector2(1500, 180), Color.Black);
            spriteBatch.End();
        }
    }
}