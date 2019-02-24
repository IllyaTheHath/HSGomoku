using System;
using System.Linq;

using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HSGomoku.Engine.UI
{
    /// <summary>
    /// FPS计数器 <see cref="http://www.david-amador.com/2009/11/how-to-do-a-xna-fps-counter/" />
    /// </summary>
    internal sealed class FpsCounter
    {
        private Int32 _totalFrames = 0;
        private Single _elapsedTime = 0.0f;
        private Int32 _fps = 0;

        private SpriteFontX _fontX;

        public void Load(ContentManager content, GraphicsDeviceManager graphics)
        {
            this._fontX = new SpriteFontX(FNAFont.Font10, graphics);
        }

        public void Update(GameTime gameTime)
        {
            // Update
            this._elapsedTime += (Single)gameTime.ElapsedGameTime.TotalMilliseconds;

            // 1 Second has passed
            if (this._elapsedTime >= 1000.0f)
            {
                this._fps = this._totalFrames;
                this._totalFrames = 0;
                this._elapsedTime = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Only update total frames when drawing
            this._totalFrames++;

            //spriteBatch.Begin();
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                                    BlendState.AlphaBlend,
                                    SamplerState.LinearClamp,
                                    DepthStencilState.Default,
                                    RasterizerState.CullNone,
                                    null,
                                    Resolution.GetTransformationMatrix());
            spriteBatch.DrawStringX(this._fontX,
                                    String.Format($"FPS={this._fps}"),
                                    new Vector2(1800, 10),
                                    Color.Black);
            spriteBatch.End();
        }
    }
}