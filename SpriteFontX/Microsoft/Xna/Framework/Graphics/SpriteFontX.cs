using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// XnaZero的GDI绘制文字类。 原库长时间未更新，无法用于FNA上，这是修改版本，以供此项目使用 <see cref="http://www.cnblogs.com/XnaZero/articles/1412148.html" />
    /// </summary>
    public class SpriteFontX
    {
        protected List<Texture2D> tex2ds;

        protected Texture2D currentTex2d;

        protected Int32 currentTop;

        protected Int32 currentLeft;

        protected Int32 currentMaxHeight;

        protected SizeF sizef;

        protected Bitmap bitmap;

        protected System.Drawing.Graphics gr;

        protected IGraphicsDeviceService gds;

        private MemoryStream _strm;

        private Bitmap _tempBp;

        private System.Drawing.Graphics _tempGr;

        private Brush _brush;

        /// <summary>
        /// 间距
        /// </summary>
        public Vector2 Spacing;

        /// <summary>
        /// CharTiles
        /// </summary>
        public Dictionary<Char, CharTile> CharTiles;

        /// <summary>
        /// 字体
        /// </summary>
        private Font font;

        public Font Font
        {
            get
            {
                return this.font;
            }
        }

        /// <summary>
        /// 指定文本呈现的质量
        /// </summary>
        private TextRenderingHint textRenderingHint;

        public TextRenderingHint TextRenderingHint
        {
            get
            {
                return this.textRenderingHint;
            }
        }

        /// <summary>
        /// 新建SpriteFontX..
        /// </summary>
        /// <param name="font">字体</param>
        /// <param name="gds"> 建纹理时用到的IGraphicsDeviceService</param>
        /// <param name="trh"> 指定文本呈现的质量</param>
        public SpriteFontX(Font font, IGraphicsDeviceService gds, TextRenderingHint trh = TextRenderingHint.AntiAliasGridFit)
        {
            Initialize(font, gds, trh);
        }

        /// <summary>
        /// 新建SpriteFontX..
        /// </summary>
        /// <param name="fontName">字体名字</param>
        /// <param name="size">    字体大小</param>
        /// <param name="gds">     建纹理时用到的IGraphicsDeviceService</param>
        /// <param name="trh">     指定文本呈现的质量</param>
        public SpriteFontX(String fontName, Single size, IGraphicsDeviceService gds, TextRenderingHint trh = TextRenderingHint.AntiAliasGridFit)
        {
            Initialize(new Font(fontName, size), gds, trh);
        }

        private void Initialize(Font font, IGraphicsDeviceService gds, TextRenderingHint trh)
        {
            this.font = font;
            this.gds = gds;
            this.textRenderingHint = trh;
            if (this._brush == null)
            {
                this._brush = Brushes.White;
                this._tempBp = new Bitmap(1, 1);
                this._tempGr = System.Drawing.Graphics.FromImage(this._tempBp);
                this._strm = new MemoryStream();
            }
            this.CharTiles = new Dictionary<Char, CharTile>();
            this.tex2ds = new List<Texture2D>();
            NewTex();
        }

        protected void NewTex()
        {
            this.currentTex2d = new Texture2D(this.gds.GraphicsDevice, 256, 256);
            this.tex2ds.Add(this.currentTex2d);
            this.currentTop = 0;
            this.currentLeft = 0;
            this.currentMaxHeight = 0;
        }

        protected unsafe void AddTex(Char chr)
        {
            if (this.CharTiles.ContainsKey(chr))
            {
                return;
            }
            String text = chr.ToString();
            this.sizef = this._tempGr.MeasureString(text, Font, PointF.Empty, StringFormat.GenericTypographic);
            if (this.sizef.Width <= 0f)
            {
                this.sizef.Width = this.sizef.Height / 2f;
            }
            if (this.bitmap == null || (Int32)Math.Ceiling(this.sizef.Width) != this.bitmap.Width || (Int32)Math.Ceiling(this.sizef.Height) != this.bitmap.Height)
            {
                this.bitmap = new Bitmap((Int32)Math.Ceiling(this.sizef.Width), (Int32)Math.Ceiling(this.sizef.Height), PixelFormat.Format32bppArgb);
                this.gr = System.Drawing.Graphics.FromImage(this.bitmap);
            }
            else
            {
                this.gr.Clear(System.Drawing.Color.Empty);
            }
            this.gr.TextRenderingHint = this.textRenderingHint;
            this.gr.DrawString(text, Font, this._brush, 0f, 0f, StringFormat.GenericTypographic);
            if (this.bitmap.Height > this.currentMaxHeight)
            {
                this.currentMaxHeight = this.bitmap.Height;
            }
            if (this.currentLeft + this.bitmap.Width + 1 > this.currentTex2d.Width)
            {
                this.currentTop += this.currentMaxHeight + 1;
                this.currentLeft = 0;
            }
            if (this.currentTop + this.currentMaxHeight > this.currentTex2d.Height)
            {
                NewTex();
            }
            CharTile charTile = new CharTile(this.currentTex2d, new Rectangle(this.currentLeft, this.currentTop, this.bitmap.Width, this.bitmap.Height));
            this.CharTiles.Add(chr, charTile);
            Int32[] array = new Int32[this.bitmap.Width * this.bitmap.Height];
            BitmapData bitmapData = this.bitmap.LockBits(new System.Drawing.Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Int32* ptr = (Int32*)((void*)bitmapData.Scan0);
            for (Int32 i = 0; i < array.Length; i++)
            {
                if (*ptr != 0)
                {
                    array[i] = *ptr;
                }
                ptr++;
            }
            this.bitmap.UnlockBits(bitmapData);
            this.gds.GraphicsDevice.Textures[0] = null;
            this.currentTex2d.SetData<Int32>(0, new Rectangle?(charTile.rect), array, 0, array.Length);
            this.currentLeft += charTile.rect.Width + 1;
        }

        /// <summary>
        /// 添加文字
        /// </summary>
        /// <param name="str">要添加的字</param>
        public void AddText(String str)
        {
            AddText(str.ToCharArray());
        }

        /// <summary>
        /// 添加文字
        /// </summary>
        /// <param name="str">要加载的字</param>
        public void AddText(Char[] chrs)
        {
            for (Int32 i = 0; i < chrs.Length; i++)
            {
                AddTex(chrs[i]);
            }
        }

        /// <summary>
        /// 绘制字符串 (不带Begin End)
        /// </summary>
        /// <param name="sb">      SpriteBatch</param>
        /// <param name="str">     字符串</param>
        /// <param name="position">位置</param>
        /// <param name="color">   颜色</param>
        /// <returns>绘制到的范围</returns>
        public Vector2 Draw(SpriteBatch sb, String str, Vector2 position, Color color)
        {
            return Draw(sb, str.ToCharArray(), position, new Vector2(Single.MaxValue, Single.MaxValue), Vector2.One, color);
        }

        /// <summary>
        /// 绘制字符数组 (不带Begin End)
        /// </summary>
        /// <param name="sb">      SpriteBatch</param>
        /// <param name="str">     字符数组</param>
        /// <param name="position">位置</param>
        /// <param name="color">   颜色</param>
        /// <returns>绘制到的范围</returns>
        public Vector2 Draw(SpriteBatch sb, Char[] str, Vector2 position, Color color)
        {
            return Draw(sb, str, position, new Vector2(Single.MaxValue, Single.MaxValue), Vector2.One, color);
        }

        /// <summary>
        /// 绘制字符数组 (不带Begin End)
        /// </summary>
        /// <param name="sb">      SpriteBatch</param>
        /// <param name="str">     字符串</param>
        /// <param name="position">位置</param>
        /// <param name="maxBound">绘制的最大范围限定</param>
        /// <param name="scale">   缩放</param>
        /// <param name="color">   颜色</param>
        /// <returns>绘制到的范围</returns>
        public Vector2 Draw(SpriteBatch sb, String str, Vector2 position, Vector2 maxBound, Vector2 scale, Color color)
        {
            return Draw(sb, str.ToCharArray(), position, maxBound, scale, color);
        }

        /// <summary>
        /// 绘制字符数组 (不带Begin End)
        /// </summary>
        /// <param name="sb">      SpriteBatch</param>
        /// <param name="str">     字符数组</param>
        /// <param name="position">位置</param>
        /// <param name="maxBound">绘制的最大范围限定</param>
        /// <param name="scale">   缩放</param>
        /// <param name="color">   颜色</param>
        /// <returns>绘制到的范围</returns>
        public Vector2 Draw(SpriteBatch sb, Char[] str, Vector2 position, Vector2 maxBound, Vector2 scale, Color color)
        {
            if (maxBound.X == 0f)
            {
                maxBound.X = Single.MaxValue;
            }
            else
            {
                maxBound.X += position.X;
            }
            if (maxBound.Y == 0f)
            {
                maxBound.Y = Single.MaxValue;
            }
            else
            {
                maxBound.Y += position.Y;
            }
            Vector2 vector = position;
            Single num = 0f;
            Single num2 = 0f;
            foreach (Char c in str)
            {
                AddTex(c);
                CharTile charTile = this.CharTiles[c];
                if (c == '\r' || vector.X + charTile.rect.Width * scale.X > maxBound.X)
                {
                    if (vector.X > num2)
                    {
                        num2 = vector.X;
                    }
                    vector.X = position.X;
                    vector.Y += num * scale.Y + this.Spacing.Y * scale.X;
                    num = 0f;
                }
                else if (c != '\n')
                {
                    if (charTile.rect.Height > num)
                    {
                        num = charTile.rect.Height;
                        if (vector.Y + num * scale.Y > maxBound.Y)
                        {
                            break;
                        }
                    }
                    if (sb != null)
                    {
                        sb.Draw(charTile.tex, vector, new Rectangle?(charTile.rect), color, 0f, Vector2.Zero, scale, 0, 0f);
                    }
                    vector.X += charTile.rect.Width * scale.X + this.Spacing.X * scale.X;
                }
            }
            if (vector.X > num2)
            {
                num2 = vector.X;
            }
            vector.X = num2 - this.Spacing.X * scale.X;
            vector.Y += num * scale.Y;
            return vector - position;
        }

        /// <summary>
        /// 检测显示字符串的的范围大小
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>宽度和高度</returns>
        public Vector2 MeasureString(String str)
        {
            return MeasureString(str.ToCharArray());
        }

        /// <summary>
        /// 检测显示字符数组的的范围大小
        /// </summary>
        /// <param name="str">字符数组</param>
        /// <returns>宽度和高度</returns>
        public Vector2 MeasureString(Char[] str)
        {
            return Draw(null, str, Vector2.Zero, Color.White);
        }

        /// <summary>
        /// 检测显示字符数组的的范围大小
        /// </summary>
        /// <param name="str">     字符串</param>
        /// <param name="maxBound">绘制的最大范围限定</param>
        /// <param name="scale">   缩放</param>
        /// <returns>宽度和高度</returns>
        public Vector2 MeasureString(String str, Vector2 maxBound, Vector2 scale)
        {
            return Draw(null, str, Vector2.Zero, maxBound, scale, Color.White);
        }

        /// <summary>
        /// 检测显示字符数组的的范围大小
        /// </summary>
        /// <param name="str">     字符数组</param>
        /// <param name="maxBound">绘制的最大范围限定</param>
        /// <param name="scale">   缩放</param>
        /// <returns>宽度和高度</returns>
        public Vector2 MeasureString(Char[] str, Vector2 maxBound, Vector2 scale)
        {
            return Draw(null, str, Vector2.Zero, maxBound, scale, Color.White);
        }
    }
}