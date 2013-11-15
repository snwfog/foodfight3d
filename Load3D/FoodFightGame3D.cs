#region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace FoodFight3D
{
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class FoodFightGame3D : Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    private Matrix _viewMatrix;
    private Matrix _projectionMatrix;
    private Character _jimmy;
    private Texture2D otherTexture;

    private Rectangle _windowBound;

    public List<Bullet> AllBullets;

    private float testTimer = 0;
    private float shootingLimit = 1000;

    private PowerUp powerUpTest;

    public FoodFightGame3D()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      this._viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 5), new Vector3(0, 0, 0), Vector3.UnitY);

      this._projectionMatrix = Matrix.CreateOrthographic(8, 6, 50, -50);
//      this._projectionMatrix = Matrix.CreateOrthographicOffCenter(0, 3, 2, 0, -2, 2);
//      this._projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
//        MathHelper.ToRadians(45), 800f / 600f, 1f, 100f);

      this._windowBound = new Rectangle(0, 0, 800, 600);
      this.graphics.PreferredBackBufferWidth = this._windowBound.Width;
      this.graphics.PreferredBackBufferHeight = this._windowBound.Height;
      //this.graphics.IsFullScreen = true;

      this._jimmy = Character.GetNewInstance(this);


      this.AllBullets = new List<Bullet>();
    }

    public Matrix GetViewMatrix() { return this._viewMatrix; }
    public Matrix GetProjectionMatrix() { return this._projectionMatrix; }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      // TODO: Add your initialization logic here

      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      powerUpTest = PowerUp.GetNewInstance(this);
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();

      this._jimmy.Update(gameTime);

      testTimer += gameTime.ElapsedGameTime.Milliseconds;
      if (testTimer >= shootingLimit)
      {
        testTimer = 0;
        AllBullets.Add(Bullet.GetNewInstance(this, this._jimmy));
      }

      foreach (Bullet bullet in AllBullets)
      {
          bullet.Update(gameTime);
      }

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);

      this._jimmy.Draw(gameTime);
//      this.cubeTest.Draw(Matrix.Identity, this._viewMatrix, this._projectionMatrix, Color.Yellow);
      foreach (Bullet bullet in AllBullets)
      {
          bullet.Draw(this._viewMatrix, this._projectionMatrix, Color.Pink);
      }

      powerUpTest.Draw(gameTime);

      base.Draw(gameTime);
    }

     private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
     {
       foreach (ModelMesh mesh in model.Meshes)
       {
         foreach (BasicEffect effect in mesh.Effects)
         {
           //effect.TextureEnabled = false;
           //effect.Texture = otherTexture;
           effect.World = world;
           effect.View = view;
           effect.Projection = projection;
         }
 
         mesh.Draw();
       }
     }

  }
}
