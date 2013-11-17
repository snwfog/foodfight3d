using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using FoodFighGame3D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FoodFight3D
{
  public class Character : BaseModel
  {
    public static float MOVEMENT_SPEED = 0.05f;
    public static float ROTATION_SPEED = 0.05f;
    public static int DEFAULT_STRENGTH = 100;

    public Queue<PowerUp> lastPickedUpPowerUps = new Queue<PowerUp>(1);
    public int Strength = DEFAULT_STRENGTH;

    private Character(Vector3 position, Matrix rotation) : base(position, rotation)
    {
      SpeedMovement = MOVEMENT_SPEED;
      SpeedRotation = ROTATION_SPEED;
    }

    private static Character _Initialize(FoodFightGame3D game)
    {
      Character _instance = new Character(Vector3.Zero, Matrix.Identity);
      Character.GameInstance = game;

      _instance.Model = Plane.GetNewInstance(game, Plane.CraftType.JIMMY);
      return _instance;
    }

    public static Character GetNewInstance(FoodFightGame3D game)
    {
      return _Initialize(game);
    }

    public void Shoot(GameTime gameTime)
    {
      Bullet bullet = Bullet.GetNewInstance(GameInstance, this);
    }

    public void PickUp(PowerUp powerUp)
    {
      lastPickedUpPowerUps.Enqueue(powerUp);
      if (lastPickedUpPowerUps.Count > 1)
      {
        PowerUp up = lastPickedUpPowerUps.Dequeue();
        up.PickUpBy(this);
      }
    }

    public PowerUp ThrowUp()
    {
      return lastPickedUpPowerUps.Dequeue();
    }

    private void _MovePosition()
    {
      KeyboardState ks = Keyboard.GetState();
      if (ks.IsKeyDown(Keys.A))
      {
        // Yaw Left
        this.YawCounterClockwise();
      }
      else if (ks.IsKeyDown(Keys.D))
      {
        this.YawClockwise();
      }

      if (ks.IsKeyDown(Keys.W))
      {
        this.GoForward();
      }
      else if (ks.IsKeyDown(Keys.S))
      {
        this.GoBackward();
      }

      if (ks.IsKeyDown(Keys.Q))
      {
        this.GoLeft();
      }
      else if (ks.IsKeyDown(Keys.E))
      {
        this.GoRight();
      }
    }

    public void Update(GameTime gameTime)
    {
//      this._MovePosition(gameTime); // Move model to new _position
      this._MovePosition();
    }
  }
}
