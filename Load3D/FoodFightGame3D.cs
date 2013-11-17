﻿#region Using Statements

using System.Collections.Generic;
using FoodFight3D.ObjectModel;
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
    public static int NUMBER_OF_BULLET = 100;
    public static int NUMBER_OF_POWERUP = 10;
    public static int NUMBER_OF_PIT = 10;
    public static int NUMBER_OF_ENEMY = 4;

    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    private Perspective _perspective = Perspective.UP;
    private Matrix _viewMatrix;
    private Matrix _projectionMatrix;
    private Character _jimmy;
    private Texture2D otherTexture;

    private Rectangle _windowBound;

    public enum Perspective { UP, FIRST, SPECTATOR }

    public Queue<Bullet> AllBullets = new Queue<Bullet>();
    public Queue<PowerUp> AllPowerUps = new Queue<PowerUp>();
    public Queue<Pit> AllPits = new Queue<Pit>();
    public Queue<EnemyCraft> AllEnemyCrafts = new Queue<EnemyCraft>(); 

    private float testTimer = 0;
    private float shootingLimit = 1000;

    private PowerUp powerUpTest;

    public FoodFightGame3D()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      this._windowBound = new Rectangle(0, 0, 800, 600);
      this.graphics.PreferredBackBufferWidth = this._windowBound.Width;
      this.graphics.PreferredBackBufferHeight = this._windowBound.Height;
      //this.graphics.IsFullScreen = true;

      this._jimmy = Character.GetNewInstance(this);
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

      PowerUp.GetNewInstance(this, (new Vector3(8, 0, 0)), PowerUp.PowerUpType.PEAR);
      PowerUp.GetNewInstance(this, (new Vector3(0, 4, 0)), PowerUp.PowerUpType.ORANGE);
      PowerUp.GetNewInstance(this, (new Vector3(0, 0, 2)), PowerUp.PowerUpType.LEMON);
      PowerUp.GetNewInstance(this, (new Vector3(-8, 0, 0)), PowerUp.PowerUpType.TOMATOE);

      Pit.GetNewInstance(this, (new Vector3(4, 4, -1)));
      Pit.GetNewInstance(this, (new Vector3(-4, 4, -1)));
      Pit.GetNewInstance(this, (new Vector3(-4, -4, -1)));
      Pit.GetNewInstance(this, (new Vector3(4, -4, -1)));
      EnemyCraft.GetNewInstance(this, new Vector3(2, 2, 0), Plane.CraftType.SPECIAL);
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

      this._UpdateCamera();
      this._jimmy.Update(gameTime);


      testTimer += gameTime.ElapsedGameTime.Milliseconds;
      if (testTimer >= shootingLimit)
      {
        testTimer = 0;
        _jimmy.Shoot(gameTime);
      }

      foreach (Bullet bullet in AllBullets) bullet.Update(gameTime);
      foreach (PowerUp powerup in AllPowerUps) powerup.Update(gameTime);
      foreach (Pit pit in AllPits) pit.Update(gameTime);
      foreach (EnemyCraft craft in AllEnemyCrafts) craft.Update(gameTime);

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
      foreach (Bullet bullet in AllBullets) bullet.Draw(this._viewMatrix, this._projectionMatrix, Color.Pink);
      foreach (PowerUp powerup in AllPowerUps) powerup.Draw(gameTime);
      foreach (Pit pit in AllPits) pit.Draw(gameTime);
      foreach (EnemyCraft craft in AllEnemyCrafts) craft.Draw(gameTime);

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

    private void _UpdateCamera()
    {
      KeyboardState ks = Keyboard.GetState();
      if (ks.IsKeyDown(Keys.D1))
        _perspective = Perspective.UP;
      else if (ks.IsKeyDown(Keys.D2))
        _perspective = Perspective.FIRST;
      else if (ks.IsKeyDown(Keys.D3))
        _perspective = Perspective.SPECTATOR;


      Vector3 _cameraPosition, _lookAtPosition;
      //      this._projectionMatrix = Matrix.CreateOrthographicOffCenter(0, 3, 2, 0, -2, 2);
      //      this._projectionMatrix = Matrix.CreatePerspective(2, 2, 1, 100);
      switch (_perspective)
      {
        case Perspective.UP:
          this._viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 20), new Vector3(0, 0, 0), Vector3.UnitX);
          this._projectionMatrix = Matrix.CreateOrthographic(32, 24, -50, 50);
          break;
        case Perspective.FIRST:
          _cameraPosition = Vector3.Add(this._jimmy.Position, new Vector3(0, 0, 0.3f));
          _lookAtPosition = Vector3.Add(this._jimmy.Position, this._jimmy.Rotation.Up);
          this._viewMatrix = Matrix.CreateLookAt(_cameraPosition, _lookAtPosition, Vector3.UnitZ);
          this._projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45), 800f / 600f, 1f, 100f);
          break;
        case Perspective.SPECTATOR:
          this._viewMatrix = Matrix.CreateLookAt(new Vector3(20, 0, 20), new Vector3(0, 0, 0), -Vector3.UnitX);
          this._projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45), 800f / 600f, 1f, 100f);
          break;
      }
    }
  }
}
