using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace FoodFight3D
{
  public class PowerUp : BaseModel
  {
    public static float ROTATION_SPEED = 1.5f;
    public enum PowerUpType { PEAR, APPLE, LEMON, ORANGE }

    public static int PEAR_VALUE = 5, APPLE_VALUE = 5, LEMON_VALUE = 5, ORANGE_VALUE = 5, TOMATE_VALUE = 5;


    private bool _isConsumed = false;
    private PowerUpType _type;
    private int _value;
    private Cue _pickUpCue;

    private PowerUp(Vector3 position, PowerUpType type) : base(position, Matrix.Identity)
    {
      this._type = type;
    }

    public int GetValue() { return this._value; }

    public static PowerUp GetNewInstance(FoodFightGame3D game, 
      Vector3 position, PowerUpType type)
    {
      PowerUp _powerUp = new PowerUp(position, type);
      PowerUp.GameInstance = game;
      _powerUp.Model = PowerUpModel.GetNewInstance(game, type);

      switch (_powerUp._type)
      {
        case PowerUpType.PEAR:
          _powerUp._value = PEAR_VALUE;
          _powerUp._pickUpCue = GameInstance.SoundBank.GetCue("SOUND_PICKUP_01");
          break;
        case PowerUpType.APPLE:
          _powerUp._value = APPLE_VALUE;
          _powerUp._pickUpCue = GameInstance.SoundBank.GetCue("SOUND_PICKUP_02");
          break;
        case PowerUpType.LEMON:
          _powerUp._value = LEMON_VALUE;
          _powerUp._pickUpCue = GameInstance.SoundBank.GetCue("SOUND_PICKUP_03");
          break;
        case PowerUpType.ORANGE:
          _powerUp._value = ORANGE_VALUE;
          _powerUp._pickUpCue = GameInstance.SoundBank.GetCue("SOUND_PICKUP_01");
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

    public PowerUpType GetType() { return this._type; }

    public bool Intersect(BaseModel model)
    {
      return !this.IsConsumed() && base.Intersect(model);
    }

    public override void Draw(GameTime gameTime)
    {
      this.Update(gameTime);
      if (this.IsConsumed()) return; 

      Matrix newWorld = this.Rotation 
        * Hexagon.INCLINATION 
        * Matrix.CreateTranslation(this.Position);

      base.Draw(gameTime, newWorld);
    }

    public bool IsConsumed() { return this._isConsumed; }

    public void PickUpBy(Character character)
    {
      character.Strength += this._value;
      this._isConsumed = true;
      this._pickUpCue.Play();
    }
  }
}
