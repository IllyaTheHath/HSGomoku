using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace System.Linq
{
    public static class SpriteBatchExt
    {
        /// <summary>
        /// 绘制字符数组 (不带Begin End)
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="sfx">字体X</param>
        /// <param name="str">字符串</param>
        /// <param name="position">位置</param>
        /// <param name="maxBound">绘制的最大范围限定</param>
        /// <param name="scale">缩放</param>
        /// <param name="color">颜色</param>
        /// <returns>绘制到的范围</returns>
        public static Vector2 DrawStringX(this SpriteBatch sb, SpriteFontX sfx, String str, Vector2 position, Color color)
        {
            return sfx.Draw(sb, str, position, color);
        }

        /// <summary>
        /// 绘制字符数组 (不带Begin End)
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="sfx">字体X</param>
        /// <param name="str">字符数组</param>
        /// <param name="position">位置</param>
        /// <param name="maxBound">绘制的最大范围限定</param>
        /// <param name="scale">缩放</param>
        /// <param name="color">颜色</param>
        /// <returns>绘制到的范围</returns>
        public static Vector2 DrawStringX(this SpriteBatch sb, SpriteFontX sfx, Char[] str, Vector2 position, Color color)
        {
            return sfx.Draw(sb, str, position, color);
        }

        /// <summary>
        /// 绘制字符数组 (不带Begin End)
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="sfx">字体X</param>
        /// <param name="str">字符串</param>
        /// <param name="position">位置</param>
        /// <param name="maxBound">绘制的最大范围限定</param>
        /// <param name="scale">缩放</param>
        /// <param name="color">颜色</param>
        /// <returns>绘制到的范围</returns>
        public static Vector2 DrawStringX(this SpriteBatch sb, SpriteFontX sfx, String str, Vector2 position, Vector2 maxBound, Vector2 scale, Color color)
        {
            return sfx.Draw(sb, str, position, maxBound, scale, color);
        }

        /// <summary>
        /// 绘制字符数组 (不带Begin End)
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="sfx">字体X</param>
        /// <param name="str">字符数组</param>
        /// <param name="position">位置</param>
        /// <param name="maxBound">绘制的最大范围限定</param>
        /// <param name="scale">缩放</param>
        /// <param name="color">颜色</param>
        /// <returns>绘制到的范围</returns>
        public static Vector2 DrawStringX(this SpriteBatch sb, SpriteFontX sfx, Char[] str, Vector2 position, Vector2 maxBound, Vector2 scale, Color color)
        {
            return sfx.Draw(sb, str, position, maxBound, scale, color);
        }
    }
}
