#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ini;

#endregion

namespace SquareBall
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>

    public class SBGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Field field;

public static double Scale;
public static int ScreenW = 1024;
public static int ScreenH = 600;
public static int LeftM;
public static int TopM;

public static SoundEffect Kick;
public static SoundEffect Pass;
public static SoundEffect Goal;
public static SoundEffect Hit;

public static TextureFont Font;
protected int time;


        public SBGame()
            : base()
        {
            GameSettings.registerSetting("Game",
                "Players.MousePlayer", false, GameSettings.ValidateBool, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game",
               "Players.KeyPlayer1", true, GameSettings.ValidateBool, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game",
               "Players.KeyPlayer2", false, GameSettings.ValidateBool, GameSettings.SaveFormatStd);
            GameSettings.registerSetting("Game",
               "Players.Enemy", true, GameSettings.ValidateBool, GameSettings.SaveFormatStd);

            graphics = new GraphicsDeviceManager(this);

                var screen = System.Windows.Forms.Screen.AllScreens[0];
                Window.IsBorderless = true;
                Window.Position = new Point(0, 0);
                graphics.IsFullScreen = false;

                graphics.PreferredBackBufferWidth = ScreenW;
                graphics.PreferredBackBufferHeight = ScreenH;

            Content.RootDirectory = "Content";

            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            field = new Field(spriteBatch, this);
            Font = new TextureFont(Content, "Font");
            Kick = Content.Load<SoundEffect>("Kick");
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (gameTime.TotalGameTime.Minutes - field.Minutes < 5)
                field.Update(gameTime);
            else 
            {

            }
            // TODO: Add your update logic here
            

            base.Update(gameTime);
        }

        public void SetTitle(string str)
        {
            this.Window.Title = str;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(29,24,0));
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

            graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            field.Draw(gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
