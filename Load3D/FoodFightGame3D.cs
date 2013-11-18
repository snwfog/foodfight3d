#region Using Statements

using System.Collections.Generic;
using FoodFight3D.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
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
    public static int DMG_MULTIPLIER = 1000;
    public static int NUMBER_OF_BULLET = 100;
    public static int NUMBER_OF_POWERUP = 10;
    public static int NUMBER_OF_PIT = 10;
    public static int NUMBER_OF_ENEMY = 4;

    GraphicsDeviceManager graphics;
    SpriteBatch SpriteBatch;

    public AudioEngine AudioEngine { get; set; }
    public SoundBank SoundBank { get; set; }
    public WaveBank WaveBank { get; set; }
    public SpriteFont Mono12 { get; set; }

    private Perspective _perspective = Perspective.UP;
    private Matrix _viewMatrix;
    private Matrix _projectionMatrix;
    private Character _jimmy;

    private Rectangle _windowBound;

    public enum Perspective { UP, FIRST, SPECTATOR }

    public List<UI2DElement> AllUIElements = new List<UI2DElement>();

    public Queue<Bullet> AllBullets = new Queue<Bullet>();
    public Queue<PowerUp> AllPowerUps = new Queue<PowerUp>();
    public Queue<Pit> AllPits = new Queue<Pit>();
    public Queue<EnemyCraft> AllEnemyCrafts = new Queue<EnemyCraft>();

    private float testTimer = 0;
    private float shootingLimit = 1000;

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
      SpriteBatch = new SpriteBatch(GraphicsDevice);
      AudioEngine = new AudioEngine("Content\\foodfight3d_xact.xgs");
      SoundBank = new SoundBank(AudioEngine, "Content\\foodfight3dsb.xsb");
      WaveBank = new WaveBank(AudioEngine, "Content\\foodfight3dwb.xwb");
      Mono12 = Content.Load<SpriteFont>("manaspace12");

      _Init();

    }

    private void _Init()
    {
//      SoundBank.PlayCue("SOUND_MAIN_LOOP");

      PowerUp.GetNewInstance(this, (new Vector3(8, 0, 0)), PowerUp.PowerUpType.PEAR);
      PowerUp.GetNewInstance(this, (new Vector3(4, 4, 0)), PowerUp.PowerUpType.APPLE);
      PowerUp.GetNewInstance(this, (new Vector3(0, 4, 0)), PowerUp.PowerUpType.ORANGE);
      PowerUp.GetNewInstance(this, (new Vector3(0, 0, 2)), PowerUp.PowerUpType.LEMON);

      Pit.GetNewInstance(this, (new Vector3(4, 4, -1)));
      Pit.GetNewInstance(this, (new Vector3(-4, 4, -1)));
      Pit.GetNewInstance(this, (new Vector3(-4, -4, -1)));
      Pit.GetNewInstance(this, (new Vector3(4, -4, -1)));
      EnemyCraft.GetNewInstance(this, new Vector3(2, 2, 0), Plane.CraftType.SPECIAL);

      UI2DElement.GetNewInstance(this, new Vector2(300, 300), this._jimmy);
      UI2DElement.GetNewInstance(this, new Vector2(400, 300), this._jimmy.GetAmmoSlot());
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

      foreach (UI2DElement element in AllUIElements) element.Update(gameTime);

      foreach (Bullet bullet in AllBullets)
      {
        if (bullet.IsExpended()) continue;

        bullet.Update(gameTime);
        if (bullet.GetOwner() is EnemyCraft && bullet.Intersect(this._jimmy))
          this._jimmy.HitBy(bullet);
        else if (bullet.GetOwner() is Character)
          foreach (EnemyCraft _craft in AllEnemyCrafts)
            if (bullet.Intersect(_craft))
              _craft.HitBy(bullet);
      }

      foreach (PowerUp powerup in AllPowerUps)
      {
        powerup.Update(gameTime);
        if (powerup.Intersect(this._jimmy))
          this._jimmy.PickUp(powerup);
      }

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
      foreach (UI2DElement element in AllUIElements) element.Draw(SpriteBatch);
      foreach (Bullet bullet in AllBullets) bullet.Draw(gameTime);
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
          this._viewMatrix = Matrix.CreateLookAt(
            new Vector3(0, 0, 20), 
            new Vector3(0, 0, 0), 
            Vector3.UnitX);
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
          this._viewMatrix = Matrix.CreateLookAt(
            new Vector3(-20, 0, 20), 
            new Vector3(0, 0, 0), 
            Vector3.UnitX);

          this._projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45), 800f / 600f, 1f, 100f);
          break;
      }
    }
  }
}
