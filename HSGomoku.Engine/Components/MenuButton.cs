using System;

using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HSGomoku.Engine.Components
{
    internal class MenuButton : ClickableControl
    {
        private readonly String _screenName;
        private Boolean scaleUp = false;

        public MenuButton() : base()
        {
        }

        public MenuButton(Texture2D texture, Vector2 position, Vector2 size)
            : base(texture, position, size)
        {
        }

        public MenuButton(String screenName)
        {
            this._screenName = screenName;
            this.Click += GotoScreen;
        }

        public MenuButton(Texture2D texture, Vector2 position, Vector2 size, String screenName)
            : base(texture, position, size)
        {
            this._screenName = screenName;
            this.Click += GotoScreen;
        }

        private void GotoScreen(Object sender, EventArgs e)
        {
            ScreenManage.ScreenManager.GotoScreen(this._screenName);
        }

        public override void Update(GameTime gameTime)
        {
            if (Initialized && Visible)
            {
                var mouse = Mouse.GetState();

                // 鼠标经过
                if (IsMouseOver(mouse))
                {
                    if (this.scale <= 1)
                    {
                        this.scaleUp = true;
                    }
                    if (this.scale >= 1.35)
                    {
                        this.scaleUp = false;
                    }
                    if (this.scaleUp)
                    {
                        this.scale += 0.01f * Statistics.SpeedMultiply;
                    }
                    else
                    {
                        this.scale -= 0.01f * Statistics.SpeedMultiply;
                    }
                }
                else
                {
                    if (this.scale > 1)
                    {
                        this.scale -= 0.01f * Statistics.SpeedMultiply;
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}