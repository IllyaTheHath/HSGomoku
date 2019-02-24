using HSGomoku.Engine.ScreenManage;
using HSGomoku.Engine.UI;
using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HSGomoku.Engine.Screens
{
    internal class GameScreen : Screen
    {
        private Texture2D _board;

        private readonly GameHUD _gameHUD = new GameHUD();
        //private readonly FpsCounter _fpsCounter = new FpsCounter();

        public GameScreen(GraphicsDevice device, ContentManager content, GraphicsDeviceManager graphics)
            : base(device, content, graphics)
        {
            this.name = "GameScreen";
        }

        public override void Init()
        {
            base.Init();
        }

        public override void LoadContent()
        {
            // 棋盘
            this._board = this.content.Load<Texture2D>("img\\board");

            // HUD
            this._gameHUD.Load(this.content, this.graphics);

            //// FPS计数器
            //this._fpsCounter.Load(this.content, this.graphics);

            base.LoadContent();
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        public override void Update(GameTime gameTime)
        {
            //this._fpsCounter.Update(gameTime);

            if (Input.KeyPressed(Keys.W))
            {
                ScreenManager.GotoScreen(nameof(StartScreen));
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //// 清理
            this.device.Clear(new Color(233, 203, 166));
            //Resolution.BeginDraw();

            // 背景棋盘
            //this._spriteBatch.Begin();
            this.spriteBatch.Begin(SpriteSortMode.BackToFront,
                                BlendState.AlphaBlend,
                                SamplerState.LinearClamp,
                                DepthStencilState.Default,
                                RasterizerState.CullNone,
                                null,
                                Resolution.GetTransformationMatrix());

            this.spriteBatch.Draw(this._board, Vector2.Zero, Color.White);
            this.spriteBatch.End();

            // HUD
            this._gameHUD.Draw(this.spriteBatch);

            //// FPS计数器
            //this._fpsCounter.Draw(this.spriteBatch);

            base.Draw(gameTime);
        }
    }
}