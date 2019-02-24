namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// CharTile
    /// </summary>
    public class CharTile
    {
        public CharTile(Texture2D tex, Rectangle rect)
        {
            this.tex = tex;
            this.rect = rect;
        }

        /// <summary>
        /// Char所在纹理
        /// </summary>
        public readonly Texture2D tex;

        /// <summary>
        /// Char所在位置
        /// </summary>
        public readonly Rectangle rect;
    }
}
