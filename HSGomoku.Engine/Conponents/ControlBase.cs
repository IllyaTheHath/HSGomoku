using System;
using System.Linq;

using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HSGomoku.Engine.Conponents
{
    internal class ControlBase
    {
        protected Texture2D _texture;
        protected SpriteFontX _fontX;

        // 是否已经初始化
        public Boolean Initialized { get; private set; } = false;

        // 背景颜色
        public Color backColor = new Color(255, 255, 255, 255);

        // 文字颜色
        public Color textColor = new Color(255, 255, 255, 255);

        // 文字大小
        protected Int32 _textSize = 10;

        public Int32 TextSize
        {
            get { return this._textSize; }
            set
            {
                this._textSize = value;

                if (!Suspended)
                {
                    OnTextSizeChanged();
                }
            }
        }

        // 组件显示文字
        protected String _text = String.Empty;

        public String Text
        {
            get { return this._text; }
            set
            {
                this._text = value;

                if (!Suspended)
                {
                    OnTextChanged(new EventArgs());
                }
            }
        }

        // 文字偏移量
        public Vector2 FontOffset { get; set; } = Vector2.Zero;

        // 组件是否休眠
        public Boolean Suspended { get; set; } = false;

        // 位置
        public Vector2 Position { get; set; } = new Vector2(0, 0);

        // 大小
        public Vector2 Size { get; set; } = new Vector2(32, 32);

        public Single Scale { get; set; } = 1.0f;

        // 盒子大小
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((Int32)Position.X, (Int32)Position.Y, (Int32)Size.X, (Int32)Size.Y);
            }
        }

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

                if (!Suspended)
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

                if (!Suspended)
                {
                    OnVisibleChanged(new EventArgs());
                }
            }
        }

        public event EventHandler TextChanged;

        public event EventHandler VisibleChanged;

        public event EventHandler EnabledChanged;

        public ControlBase()
        {
        }

        public ControlBase(Texture2D texture, Vector2 position, Vector2 size)
        {
            this._texture = texture;
            Position = position;
            Size = size;
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
                if (this._text != String.Empty)
                {
                    this._fontX = new SpriteFontX(FNAFont.GetFont(TextSize), graphics);
                }
            }
        }

        public virtual void Init(Texture2D texture, GraphicsDeviceManager graphics)
        {
            if (!Initialized)
            {
                Initialized = true;
                this._texture = texture;
                if (this._text != String.Empty)
                {
                    this._fontX = new SpriteFontX(FNAFont.GetFont(TextSize), graphics);
                }
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
                    Position,
                    null,
                    this.backColor,
                    0f,
                    Vector2.Zero,
                    Scale,
                    SpriteEffects.None,
                    0f);
                if (this._text != String.Empty && this._fontX != null)
                {
                    spriteBatch.DrawStringX(this._fontX,
                        Text,
                        new Vector2(Position.X + FontOffset.X, Position.Y + FontOffset.Y),
                        this.textColor);
                }
            }
        }

        public Boolean IsMouseOver(MouseState mouse)
        {
            Rectangle mouseRectange = new Rectangle(mouse.X, mouse.Y, 1, 1);
            if (mouseRectange.Intersects(BoundingBox))
            {
                return true;
            }
            return false;
        }

        protected void OnTextChanged(EventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged.Invoke(this, e);
            }
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

        private void OnTextSizeChanged()
        {
            //this._fontX = new SpriteFontX(FNAFont.GetFont(TextSize), graphics);
        }
    }
}