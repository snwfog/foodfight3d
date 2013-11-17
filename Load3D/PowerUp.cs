using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FoodFight3D
{
  public class PowerUp : BaseModel
  {
    public static float ROTATION_SPEED = 1.5f;
    public enum PowerUpType { PEAR, APPLE, LEMON, ORANGE, TOMATOE }

    public static int PEAR_VALUE = 5, APPLE_VALUE = 5, LEMON_VALUE = 5, ORANGE_VALUE = 5, TOMATE_VALUE = 5;

    public int Value = 0;


    private bool _isConsumed = false;
    private PowerUp(Vector3 position) : base(position, Matrix.Identity) { }

    public static PowerUp GetNewInstance(FoodFightGame3D game, Vector3 position, PowerUpType type)
    {
      PowerUp _powerUp = new PowerUp(position);
      PowerUp.GameInstance = game;
      // _powerUp.Model = new CubePrimitive(game.GraphicsDevice);
      _powerUp.Model = PowerUpModel.GetNewInstance(game, type);

      switch (type)
      {
        case PowerUpType.PEAR:
          _powerUp.Value = PEAR_VALUE;
          break;
        case PowerUpType.APPLE:
          _powerUp.Value = APPLE_VALUE;
          break;
        case PowerUpType.LEMON:
          _powerUp.Value = LEMON_VALUE;
          break;
        case PowerUpType.ORANGE:
          _powerUp.Value = ORANGE_VALUE;
          break;
        case PowerUpType.TOMATOE:
          _powerUp.Value = TOMATE_VALUE;
          break;
      }

      game.AllPowerUps.Enqueue(_powerUp);
      if (game.AllPowerUps.Count > FoodFightGame3D.NUMBER_OF_POWERUP)
        game.AllPowerUps.Dequeue();

      return _powerUp;
    }

    public void Update(GameTime gameTime)
    {
      this.Rotation *= Matrix.CreateFromYawPitchRoll(
        MathHelper.ToRadians(ROTATION_SPEED), MathHelper.ToRadians(ROTATION_SPEED), 0);
    }

    public override void Draw(GameTime gameTime)
    {
      this.Update(gameTime);

      Matrix newWorld = this.Rotation * Hexagon.INCLINATION * Matrix.CreateTranslation(this.Position);
      base.Draw(gameTime, newWorld);
    }

    public bool IsConsumed() { return this._isConsumed; }

    public void PickUpBy(Character character)
    {
      character.Strength += this.Value;
      this._isConsumed = true;
    }
  }
}
