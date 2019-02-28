using System;

using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HSGomoku.Engine.UI
{
    internal sealed class Button
    {
        private readonly Texture2D texture;
        private Vector2 position;
        private Rectangle rectangle;
        private Color color = new Color(255, 255, 255, 255);

        public Vector2 size;
        private Boolean down;
        public Boolean isClicked;

        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            this.texture = newTexture;
            this.size = new Vector2(144, 72);
        }

        public void Update(MouseState mouse)
        {
            this.rectangle = new Rectangle((Int32)this.position.X, (Int32)this.position.Y, (Int32)this.size.X, (Int32)this.size.Y);

            Rectangle mouseRectange = new Rectangle((Int32)(mouse.X / Resolution.ScreenScale.X), (Int32)(mouse.Y / Resolution.ScreenScale.Y), 1, 1);

            if (mouseRectange.Intersects(this.rectangle))
            {
                if (this.color.A == 255)
                {
                    this.down = false;
                }
                if (this.color.A == 0)
                {
                    this.down = true;
                }
                if (this.down)
                {
                    this.color.A += 3;
                }
                else
                {
                    this.color.A -= 3;
                }
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    this.isClicked = true;
                }
            }
            else if (this.color.A < 255)
            {
                this.color.A += 3;
                this.isClicked = false;
            }
        }

        public void SetPosition(Vector2 newPosistion)
        {
            this.position = newPosistion;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.rectangle, this.color);
        }
    }
}