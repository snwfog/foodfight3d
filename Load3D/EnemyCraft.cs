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

    private Texture2D _texture;
    private float _spawnAnimationTimer;

    private float _shootIntervalTimer, _currentShootIntervalTimer = MIN_SHOOT_INTERVAL_TIMER;
    private float _yawIntervalTimer, _currentYawIntervalTimer = MIN_YAW_INTERVAL_TIMER;
    private int _currentYawDirection;

    private EnemyCraft(Vector3 position) : base(position, Matrix.Identity)
    {
      SpeedMovement = YAW_SPEED;
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
      foreach (Pit pit in GameInstance.AllPits)
      {
        if (pit.IsFree())
        {
          pit.TakeOverBy(this);
        }
      }
      
    }

    public void Update(GameTime gameTime)
    {
      _shootIntervalTimer += gameTime.ElapsedGameTime.Milliseconds;
      _yawIntervalTimer += gameTime.ElapsedGameTime.Milliseconds;

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
      }

      this.UpdatePosition(gameTime);
    }

    public void Shoot(GameTime gameTime)
    {
      Bullet.GetNewInstance(GameInstance, this);
    }

    // More of a move pit...
    public void UpdatePosition(GameTime gameTime)
    {
      this.Yaw(YAW_SPEED * this._currentYawDirection);
    }
  }
}
