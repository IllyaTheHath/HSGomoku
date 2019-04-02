using HSGomoku.Engine.Components;
using HSGomoku.Engine.ScreenManage;
using HSGomoku.Engine.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HSGomoku.Engine.Screens
{
    internal class HelpScreen : Screen
    {
        private Button _btnBack;

        public HelpScreen(Game game) : base(game)
        {
        }

        public override void Init()
        {
            this._btnBack = new Button(
                this._content.Load<Texture2D>("img\\button_back"),
                new Vector2(400, 800),
                new Vector2(144, 72));
            this._btnBack.Click += (s, e) =>
            {
                ScreenManager.GoBack();
            };

            this._btnBack.Init();

            base.Init();
        }

        public override void Shutdown()
        {
            this._btnBack = null;

            base.Shutdown();
        }

        public override void Update(GameTime gameTime)
        {
            this._btnBack?.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this._device.Clear(new Color(233, 203, 166));
            //Resolution.BeginDraw();

            this._spriteBatch.Begin(SpriteSortMode.BackToFront,
                                BlendState.AlphaBlend,
                                SamplerState.LinearClamp,
                                DepthStencilState.Default,
                                RasterizerState.CullNone,
                                null,
                                Resolution.GetTransformationMatrix());

            this._btnBack?.Draw(this._spriteBatch, gameTime);

            this._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}