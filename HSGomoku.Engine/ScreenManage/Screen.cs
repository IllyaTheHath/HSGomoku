/************************************************************************/
/* Author : David Amador
 * Web:      http://www.david-amador.com
 * Twitter : http://www.twitter.com/DJ_Link
 *
 * You can use this for whatever you want. If you want to give me some credit for it that's cool but not mandatory
/************************************************************************/

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HSGomoku.Engine.ScreenManage
{
    public class Screen
    {
        protected GraphicsDevice device = null;
        protected SpriteBatch spriteBatch;
        protected ContentManager content;
        protected GraphicsDeviceManager graphics;

        protected String name;
        public String Name { get { return this.name; } }

        /// <summary>
        /// Screen Constructor
        /// </summary>
        public Screen(GraphicsDevice device, ContentManager content, GraphicsDeviceManager graphics)
        {
            this.device = device;
            this.spriteBatch = new SpriteBatch(this.device);
            this.content = content;
            this.graphics = graphics;
        }

        ~Screen()
        {
        }

        /// <summary>
        /// Virtual Function that's called when entering a Screen override it and add your own
        /// initialization code
        /// </summary>
        /// <returns></returns>
        public virtual void Init()
        {
        }

        /// <summary>
        /// Virtual Function that's called when exiting a Screen override it and add your own
        /// shutdown code
        /// </summary>
        /// <returns></returns>
        public virtual void Shutdown()
        {
        }

        public virtual void LoadContent()
        {
        }

        /// <summary>
        /// Override it to have access to elapsed time
        /// </summary>
        /// <param name="elapsed">GameTime</param>
        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime)
        {
        }
    }
}