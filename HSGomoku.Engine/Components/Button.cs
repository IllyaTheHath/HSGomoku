using System;

using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HSGomoku.Engine.Components
{
    internal class Button : ClickableControl
    {
        private Boolean colorDown = false;

        public Button() : base()
        {
        }

        public Button(Texture2D texture, Vector2 position, Vector2 size)
            : base(texture, position, size)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (Initialized && Visible)
            {
                var mouse = Mouse.GetState();

                // 鼠标经过
                if (IsMouseOver(mouse))
                {
                    if (this.backColor.A >= 255)
                    {
                        this.colorDown = false;
                    }
                    if (this.backColor.A <= 0)
                    {
                        this.colorDown = true;
                    }
                    if (this.colorDown)
                    {
                        if (this.backColor.A + (Byte)(4 * Statistics.SpeedMultiply) <= 255)
                        {
                            this.backColor.A += (Byte)(4 * Statistics.SpeedMultiply);
                        }
                        else
                        {
                            this.backColor.A = 255;
                        }
                    }
                    else
                    {
                        if (this.backColor.A - (Byte)(4 * Statistics.SpeedMultiply) >= 0)
                        {
                            this.backColor.A -= (Byte)(4 * Statistics.SpeedMultiply);
                        }
                        else
                        {
                            this.backColor.A = 0;
                        }
                    }
                }
                else
                {
                    if (this.backColor.A < 255)
                    {
                        if (this.backColor.A + (Byte)(4 * Statistics.SpeedMultiply) <= 255)
                        {
                            this.backColor.A += (Byte)(4 * Statistics.SpeedMultiply);
                        }
                        else
                        {
                            this.backColor.A = 255;
                        }
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }

        public void InvokeClick()
        {
            OnClick();
        }
    }
}