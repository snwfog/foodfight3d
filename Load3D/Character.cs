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
  public class Character : BaseModel, IWatchableElement
  {
    public static float MOVEMENT_SPEED = 0.05f;
    public static float ROTATION_SPEED = 0.05f;
    public static int DEFAULT_STRENGTH = 100;
    public static int MIN_SHOOT_INTERVAL = 200;
    public static int HIT_ANIMATION_DURATION = 200;
    public static int IN_PIT_REDUCE_PER_TICK = 3;
    public static int IN_PIT_TICK = 200;

    public int Strength = DEFAULT_STRENGTH;

    private AmmoSlot _ammoSlot;
    private float _shootTimer;
    private float _hitAnimationTimer;

    private Pit _inPit;
    private bool _isInPIt;
    private float _inPitTickTimer;
    private float _inPitCueTimer;

    private Character(Vector3 position, Matrix rotation) : base(position, rotation)
    {
      SpeedMovement = MOVEMENT_SPEED;
      SpeedRotation = ROTATION_SPEED;

      _ammoSlot = new AmmoSlot();
    }

    public static Character GetNewInstance(FoodFightGame3D game, Vector3 position)
    {
      Character _instance = new Character(position, Matrix.Identity);
      Character.GameInstance = game;

      _instance.Model = Plane.GetNewInstance(game, Plane.CraftType.JIMMY);
      _instance.Color = Color.White;
      return _instance;
    }

    public void Shoot(GameTime gameTime)
    {
      PowerUp up = this.ThrowUp();
      Bullet bullet = Bullet.GetNewInstance(GameInstance, this, up.GetValue());

      switch (RANDOM.Next(2))
      {
        case 0:
          GameInstance.SoundBank.PlayCue("SOUND_SHOOT_01");
          break;
        case 1:
          GameInstance.SoundBank.PlayCue("SOUND_SHOOT_02");
          break;
      }
    }

    public AmmoSlot GetAmmoSlot()
    {
      return this._ammoSlot;
    }

    public void PickUp(PowerUp powerUp)
    {
      _ammoSlot.ChargeAmmo(powerUp);
      powerUp.PickUpBy(this);
    }

    public PowerUp ThrowUp()
    {
      PowerUp up = _ammoSlot.UseAmmo();
      this.Strength -= up.GetValue();
      return up;
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
      _hitAnimationTimer -= gameTime.ElapsedGameTime.Milliseconds;
      if (_hitAnimationTimer < 0) _hitAnimationTimer = 0;

      this._MovePosition();
      this._ShootProjectile(gameTime);
      if (this._isInPIt) this._SlowInPit(gameTime);
    }

    public void HitBy(Bullet bullet)
    {
      bullet.Expended();
      this.Strength -= bullet.GetDamage() * FoodFightGame3D.DMG_MULTIPLIER;
      this._hitAnimationTimer = HIT_ANIMATION_DURATION;
      GameInstance.SoundBank.PlayCue("SOUND_HIT_01");
    }

    public void SlowDownBy(Pit pit)
    {
      this.SpeedMovement = 0.01f;
      this.SpeedRotation = 0.01f;

      this._isInPIt = true;
      this._inPit = pit;
    }

    private void _SlowInPit(GameTime gameTime)
    {
      this._inPitTickTimer += gameTime.ElapsedGameTime.Milliseconds;
      if (this._inPit.Intersect(this))
      {
        if (GameInstance.SoundBank.GetCue("SOUND_SPAWN_01").IsStopped)
          GameInstance.SoundBank.PlayCue("SOUND_SPAWN_01");
        if (this._inPitTickTimer > IN_PIT_TICK)
        {
          this.Strength -= IN_PIT_REDUCE_PER_TICK;
          this._inPitTickTimer = 0;
        }
      }
      else
      {
        this.SpeedMovement = MOVEMENT_SPEED;
        this.SpeedRotation = ROTATION_SPEED;
        this._isInPIt = false;
      }
    }

    private void _ShootProjectile(GameTime gameTime)
    {
      this._shootTimer += gameTime.ElapsedGameTime.Milliseconds;

      if (_shootTimer >= MIN_SHOOT_INTERVAL
          && Keyboard.GetState().IsKeyDown(Keys.Space)
          && this._ammoSlot.HasAmmo())
      {
        this._shootTimer = 0;
        this.Shoot(gameTime);
      }
    }

    public string GetStatus()
    {
      return this.Strength.ToString();
    }

    public void Draw(GameTime gameTime)
    {
      if (_hitAnimationTimer > 0)
        base.Draw(gameTime, this.GetWorldTransform(), Color.Pink);
      else
        base.Draw(gameTime);
    }
  }
}
