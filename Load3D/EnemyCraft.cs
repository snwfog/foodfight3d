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
    public static int MIN_SHOOT_INTERVAL_TIMER = 200;
    public static int MAX_SHOOT_INTERVAL_TIMER = 800;
    public static int MIN_YAW_INTERVAL_TIMER = 200;
    public static int MAX_YAW_INTERVAL_TIMER = 800;
    public static int ENEMY_CRAFT_STRENGTH = 10;
    public static int HIT_ANIMATION_DURATION = 200;
    public static int FLASH_INTERVAL = 200;
    public static int ON_HIT_DAMAGE = 20;
    public static int MAX_TAKE_OVER_ELAPSED_TIME = 10000;

    private float _spawnAnimationTimer;
    private float _hitAnimationTimer;
    private float _flashTimer;
    private bool _flashToggle;


    private float _shootIntervalTimer, _currentShootIntervalTimer = MIN_SHOOT_INTERVAL_TIMER;
    private float _yawIntervalTimer, _currentYawIntervalTimer = MIN_YAW_INTERVAL_TIMER;
    private int _currentYawDirection;
    private int _health;

    private Pit _occupyPit;

    private Pit _takingOverPit;
    private bool _isTakingOverPit;
    public float _takeOverTimer;
    public float _takeOverInterval;

    private EnemyCraft(Vector3 position)
      : base(position, Matrix.Identity)
    {
      SpeedMovement = YAW_SPEED;
      this._health = ENEMY_CRAFT_STRENGTH;
      this._takeOverInterval = MAX_TAKE_OVER_ELAPSED_TIME;
    }

    public static EnemyCraft GetNewInstance(FoodFightGame3D game)
    {
      Plane.CraftType _type = Plane.CraftType.ENEMY_TYPE_1;

      switch (RANDOM.Next(4))
      {
        case 0:
          _type = Plane.CraftType.ENEMY_TYPE_1;
          break;
        case 1:
          _type = Plane.CraftType.ENEMY_TYPE_2;
          break;
        case 2:
          _type = Plane.CraftType.ENEMY_TYPE_3;
          break;
        case 3:
          _type = Plane.CraftType.ENEMY_TYPE_4;
          break;
      }

      return EnemyCraft.GetNewInstance(game, _type);
    }

    public static EnemyCraft GetNewInstance(FoodFightGame3D game, Plane.CraftType type)
    {
      return EnemyCraft.GetNewInstance(game, Vector3.Zero, type);
    }

    public static EnemyCraft GetNewInstance(FoodFightGame3D game, 
      Vector3 position, Plane.CraftType type)
    {
      EnemyCraft _craft = new EnemyCraft(position);
      _craft.Model = Plane.GetNewInstance(game, type);
      _craft.Spawn();

      game.AllEnemyCrafts.Enqueue(_craft);
      if (game.AllEnemyCrafts.Count > FoodFightGame3D.NUMBER_OF_ENEMY)
        game.AllEnemyCrafts.Dequeue();

      return _craft;
    }


    private void _CleanPits()
    {
      if (this._occupyPit != null && this._occupyPit.GetOwner() == this)
        this._occupyPit.Evict();
      if (this._takingOverPit != null && this._takingOverPit.GetOwner() == this)
        this._takingOverPit.Evict();
    }

    public void Spawn()
    {
      this._health = ENEMY_CRAFT_STRENGTH;
      this._spawnAnimationTimer = SPAWN_ANIMATION_TIMER;
      this._ReachedPit();
      this._CleanPits();
      GameInstance.SoundBank.PlayCue("SOUND_SPAWN_03");

      Pit _freePit = Pit.GetAFreePit();
      _freePit.TakeOverBy(this);
    }

    public void OccupingPit(Pit pit) { this._occupyPit = pit; }

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

      if (this._isTakingOverPit)
      {
        this._TakingOverPit(gameTime);
        return;
      }

      _spawnAnimationTimer -= gameTime.ElapsedGameTime.Milliseconds;
      if (_spawnAnimationTimer > 0)
      {
        _flashTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (_flashTimer > FLASH_INTERVAL)
        {
          if (_flashToggle) _flashToggle = false; else _flashToggle = true;
          _flashTimer = 0;
        }
        return;
      }

      if (!this._isTakingOverPit) _takeOverTimer += gameTime.ElapsedGameTime.Milliseconds;
      if (_takeOverTimer > _takeOverInterval)
      {
        this._takeOverTimer = 0;
        this._takeOverInterval = MAX_TAKE_OVER_ELAPSED_TIME
          + RANDOM.Next(MAX_TAKE_OVER_ELAPSED_TIME);
        if (RANDOM.Next(100) > 1)
        {
          Pit _freePit = Pit.GetAFreePit();
          if (_freePit.IsFree())
            this._TakeOverPit(Pit.GetAFreePit());
        }
      }

      if (_hitAnimationTimer < 0) _hitAnimationTimer = 0;
      if (_spawnAnimationTimer < 0) _spawnAnimationTimer = 0;

      if (_shootIntervalTimer > _currentShootIntervalTimer)
      {
        this.Shoot(gameTime);
        _shootIntervalTimer = 0;
        _currentShootIntervalTimer = RANDOM.Next(
          MIN_SHOOT_INTERVAL_TIMER, MAX_SHOOT_INTERVAL_TIMER)
          * (ENEMY_CRAFT_STRENGTH - this._health / ENEMY_CRAFT_STRENGTH);
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
        _currentYawIntervalTimer = RANDOM.Next(
          MIN_YAW_INTERVAL_TIMER, MAX_YAW_INTERVAL_TIMER);
      }

      this.UpdatePosition(gameTime);
    }

    public bool IsSpawning()
    {
      return _spawnAnimationTimer > 0;
    }

    public void Draw(GameTime gameTime)
    {
      if (_spawnAnimationTimer > 0 && _flashToggle)
        base.Draw(gameTime, this.GetWorldTransform(), Color.Green);
      else if (_hitAnimationTimer > 0)
        base.Draw(gameTime, this.GetWorldTransform(), Color.Red);
      else
        base.Draw(gameTime);
    }

    public bool IsTakingOverPit() { return this._isTakingOverPit; }

    private bool _IsReachedPit()
    {
      return Math.Abs(this.Position.X - this._takingOverPit.Position.X) < 0.1
        && Math.Abs(this.Position.Y - this._takingOverPit.Position.Y) < 0.1;
    }

    private void _ReachedPit() { this._isTakingOverPit = false; }

    private void _TakingOverPit(GameTime gameTime)
    {
      Vector2 _thisPosition = new Vector2(this.Position.X, this.Position.Y);
      Vector2 _pitPosition = new Vector2(this._takingOverPit.Position.X, 
        this._takingOverPit.Position.Y);
      Vector2 _vector = _pitPosition - _thisPosition;
      Vector3 _vector3 = (new Vector3(_vector.X, _vector.Y, 0));
      _vector3.Normalize();
      Vector3 _align = this.Rotation.Up;

      if (!this._IsReachedPit())
      {
        if (!(Math.Abs(_vector3.X - _align.X) < 0.005
              && Math.Abs(_vector3.Y - _align.Y) < 0.005))
        {
          if (this._ClockOrCounterClock(_vector3, _align))
            this.Yaw(0.01f);
          else
            this.Yaw(-0.01f);
          return;

        }

        this.GoForward();
      }
      else
      {
        this._ReachedPit();
        this._takingOverPit.TakeOverBy(this);
        this._takingOverPit = null;
      }
    }

    public Pit GetOccupyingPit() { return this._occupyPit; }

    private void _TakeOverPit(Pit pit)
    {
      pit.TakingOverBy(this);
      this._occupyPit = null;
      this._takingOverPit = pit;
      this._isTakingOverPit = true;
    }

    private bool _ClockOrCounterClock(Vector3 vec1, Vector3 vec2)
    {
      Vector3 _new = Vector3.Cross(vec1, vec2);
      Vector3 _new1 = Vector3.Cross(vec2, vec1);
      return _new.Z >= _new1.Z;
    }
  }
}
