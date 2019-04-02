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
    internal abstract class Screen
    {
        public Game Game { get; private set; }

        protected readonly GraphicsDevice _device;
        protected readonly SpriteBatch _spriteBatch;
        protected readonly ContentManager _content;
        protected readonly GraphicsDeviceManager _graphics;

        public String Name { get { return GetType().Name; } }

        public Screen(Game game)
        {
            Game = game;
            this._device = game.GraphicsDevice;
            this._spriteBatch = game.SpriteBatch;
            this._content = game.Content;
            this._graphics = game.GraphicsDeviceManager;
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