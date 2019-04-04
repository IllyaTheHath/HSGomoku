using System;

using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HSGomoku.Engine.Components
{
    internal class ClickableControl : ControlBase
    {
        public event Action Click;

        public ClickableControl() : base()
        {
        }

        public ClickableControl(Texture2D texture, Vector2 position, Vector2 size)
            : base(texture, position, size)
        {
        }

        public override void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            // 单击事件
            if (IsMouseOver(mouse) && Input.MouseLeftClicked() && this._enabled)
            {
                OnClick();
            }
            base.Update(gameTime);
        }

        protected void OnClick()
        {
            Click?.Invoke();
        }
    }
}