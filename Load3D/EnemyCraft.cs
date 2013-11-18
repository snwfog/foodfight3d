using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoodFighGame3D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFight3D
{
  public class EnemyCraft : BaseModel, IAutoShootable, IAutoMoveable
  {
    public static float YAW_SPEED = 0.022f;
    public static int SPAWN_ANIMATION_TIMER = 4000; // Time for the animation of the spawning;
    public static int MIN_SHOOT_INTERVAL_TIMER = 100;
    public static int MAX_SHOOT_INTERVAL_TIMER = 2000;
    public static int MIN_YAW_INTERVAL_TIMER = 200;
    public static int MAX_YAW_INTERVAL_TIMER = 800;
    public static int ENEMY_CRAFT_STRENGTH = 5;
    public static int HIT_ANIMATION_DURATION = 200;
    public static int FLASH_INTERVAL = 200;

    private float _spawnAnimationTimer;
    private float _hitAnimationTimer;
    private float _flashTimer;

    private float _shootIntervalTimer, _currentShootIntervalTimer = MIN_SHOOT_INTERVAL_TIMER;
    private float _yawIntervalTimer, _currentYawIntervalTimer = MIN_YAW_INTERVAL_TIMER;
    private int _currentYawDirection;
    private int _health;

    private EnemyCraft(Vector3 position)
      : base(position, Matrix.Identity)
    {
      SpeedMovement = YAW_SPEED;
      this._health = ENEMY_CRAFT_STRENGTH;
    }

    public static EnemyCraft GetNewInstance(FoodFightGame3D game, Vector3 position, Plane.CraftType type)
    {
      EnemyCraft _craft = new EnemyCraft(position);
      _craft.Model = Plane.GetNewInstance(game, type);
      _craft.Spawn();

      game.AllEnemyCrafts.Enqueue(_craft);
      if (game.AllEnemyCrafts.Count > FoodFightGame3D.NUMBER_OF_ENEMY)
        game.AllEnemyCrafts.Dequeue();

      return _craft;
    }

    public void Spawn()
    {
      this._health = ENEMY_CRAFT_STRENGTH;

      foreach (Pit pit in GameInstance.AllPits)
        if (pit.IsFree())
          if (RANDOM.Next(GameInstance.AllPits.Count) == 0)
            pit.TakeOverBy(this);

    }

    public void Shoot(GameTime gameTime)
    {
      Bullet.GetNewInstance(GameInstance, this, 10);
    }

    // More of a move pit...
    public void UpdatePosition(GameTime gameTime)
    {
      this.Yaw(YAW_SPEED * this._currentYawDirection);
    }

    public void HitBy(Bullet bullet)
    {
      try
      {
        bullet.Expended();
        this._health -= bullet.GetDamage() * FoodFightGame3D.DMG_MULTIPLIER;
        this._hitAnimationTimer = HIT_ANIMATION_DURATION;
        GameInstance.SoundBank.PlayCue("SOUND_HIT_01");
        if (this._health < 0)
        {
          switch (RANDOM.Next(2))
          {
            case 0:
              GameInstance.SoundBank.PlayCue("SOUND_EXPLODE_01");
              break;
            case 1:
              GameInstance.SoundBank.PlayCue("SOUND_EXPLODE_02");
              break;
          }

          throw new Exception();
        }
      }
      catch (Exception)
      {
        this.Spawn();
      }
    }

    public void Update(GameTime gameTime)
    {
      _shootIntervalTimer += gameTime.ElapsedGameTime.Milliseconds;
      _yawIntervalTimer += gameTime.ElapsedGameTime.Milliseconds;
      _hitAnimationTimer -= gameTime.ElapsedGameTime.Milliseconds;

      if (_shootIntervalTimer > _currentShootIntervalTimer)
      {
        this.Shoot(gameTime);
        _shootIntervalTimer = 0;
        _currentShootIntervalTimer = RANDOM.Next(MIN_SHOOT_INTERVAL_TIMER, MAX_SHOOT_INTERVAL_TIMER);
      }

      if (_yawIntervalTimer > _currentYawIntervalTimer)
      {
        int rand = RANDOM.Next(2);

        switch (rand)
        {
          case 0:
            this._currentYawDirection = 0;
            break;
          case 1:
            this._currentYawDirection = -1;
            break;
          case 2:
            this._currentYawDirection = 1;
            break;
        }

        _yawIntervalTimer = 0;
        _currentYawIntervalTimer = RANDOM.Next(MIN_YAW_INTERVAL_TIMER, MAX_YAW_INTERVAL_TIMER);
        if (_hitAnimationTimer < 0) _hitAnimationTimer = 0;
      }

      this.UpdatePosition(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
      if (_hitAnimationTimer > 0)
        base.Draw(gameTime, this.GetWorldTransform(), Color.Red);
      else
        base.Draw(gameTime);
    }
  }
}
