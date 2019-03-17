using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HSGomoku.Engine.Components
{
    internal class ControlBase
    {
        protected Texture2D _texture;

        // 是否已经初始化
        public Boolean Initialized { get; private set; } = false;

        // 背景颜色
        public Color backColor = new Color(255, 255, 255, 255);

        // 组件是否休眠
        public Boolean suspended = false;

        // 位置
        public Vector2 position = new Vector2(0, 0);

        // 大小
        public Vector2 size = new Vector2(32, 32);

        public Single scale = 1.0f;

        //// 盒子大小
        //public Rectangle BoundingBox
        //{
        //    get
        //    {
        //        return new Rectangle((Int32)this.position.X, (Int32)this.position.Y, (Int32)this.size.X, (Int32)this.size.Y);
        //    }
        //}

        // 是否启用
        protected Boolean _enabled = true;

        public Boolean Enabled
        {
            get
            {
                return this._enabled;
            }
            set
            {
                this._enabled = value;

                if (!this.suspended)
                {
                    OnEnabledChanged(new EventArgs());
                }
            }
        }

        // 是否可见
        protected Boolean _visible = true;

        public Boolean Visible
        {
            get
            {
                return this._visible;
            }
            set
            {
                this._visible = value;

                if (!this.suspended)
                {
                    OnVisibleChanged(new EventArgs());
                }
            }
        }

        public event EventHandler VisibleChanged;

        public event EventHandler EnabledChanged;

        public ControlBase()
        {
        }

        public ControlBase(Texture2D texture, Vector2 position, Vector2 size)
        {
            this._texture = texture;
            this.position = position;
            this.size = size;
        }

        public virtual void Init()
        {
            Initialized = true;
        }

        public virtual void Init(Texture2D texture)
        {
            Initialized = true;
            this._texture = texture;
        }

        public virtual void Init(GraphicsDeviceManager graphics)
        {
            if (!Initialized)
            {
                Initialized = true;
            }
        }

        public virtual void Init(Texture2D texture, GraphicsDeviceManager graphics)
        {
            if (!Initialized)
            {
                Initialized = true;
                this._texture = texture;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Initialized && Visible)
            {
                //spriteBatch.Draw(this._texture,
                //    BoundingBox,
                //    this.backColor);
                spriteBatch.Draw(this._texture,
                    this.position,
                    null,
                    this.backColor,
                    0f,
                    Vector2.Zero,
                    this.scale,
                    SpriteEffects.None,
                    0f);
            }
        }

        public Boolean IsMouseOver(MouseState mouse)
        {
            Rectangle mouseRectange = new Rectangle(mouse.X, mouse.Y, 1, 1);
            Rectangle boundingBox = new Rectangle((Int32)this.position.X, (Int32)this.position.Y, (Int32)this.size.X, (Int32)this.size.Y);
            if (mouseRectange.Intersects(boundingBox))
            {
                return true;
            }
            return false;
        }

        protected void OnVisibleChanged(EventArgs e)
        {
            if (VisibleChanged != null)
            {
                VisibleChanged.Invoke(this, e);
            }
        }

        protected void OnEnabledChanged(EventArgs e)
        {
            if (EnabledChanged != null)
            {
                EnabledChanged.Invoke(this, e);
            }
        }
    }
}