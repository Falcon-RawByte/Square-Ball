using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

using RawENGINE;

namespace Square_Ball_Mono.GameObjects
{
    class Card : DrawableGameComponent
    {

        Texture2D texture;
        Vector2 position;
        SpriteBatch batch;

        public Card(Game game, SpriteBatch _batch) : base(game)
        {
            batch = _batch;
            using (Stream stream = TitleContainer.OpenStream("Content/Enemy.png"))
            {
                texture = Texture2D.FromStream(game.GraphicsDevice, stream);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            batch.Draw(texture, position, Color.White);
            base.Draw(gameTime);
        }

        public virtual void Action()
        {
        }


    }
}
