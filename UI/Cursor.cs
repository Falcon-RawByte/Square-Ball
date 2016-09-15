using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace RawENGINE
{
    /// <summary>
    /// Курсор
    /// </summary>
    public class Cursor : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Vector2 pos = Vector2.Zero;
        Vector2 size = Vector2.Zero;
        Texture2D btnTex;

        private SpriteBatch sb;
        public SpriteBatch SpriteBatch
        {
            get { return sb; }
            set { sb = value; }
        }
        
        public Texture2D Texture
        {
            get { return btnTex; }
            set { btnTex = value; }
        }

        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }
        
        public Cursor(Game game, Texture2D texture, SpriteBatch batch)
            : base(game)
        {
            // TODO: Construct any child components here
            size.X = texture.Width; size.Y = texture.Height;
            btnTex = texture;
            sb = batch;

        }

        
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Draw(btnTex, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, (int)size.X, (int)size.Y), Color.White);
            base.Draw(gameTime);
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            pos = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
            base.Update(gameTime);
        }
    }
}