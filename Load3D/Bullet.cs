using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using FoodFight3D.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFight3D
{
  public class Bullet : BaseModel, IAutoMoveable
  {
    public static float BULLET_SPEED_XY = 0.00555f;
    public static float BULLET_SPEED_Z = 1000.0125f;
    public static float BULLET_ACCELERATION = 1.0005f;
    public static float BULLET_SIZE = 0.1f;
    public static float TIME_TO_LIVE = 20000;

//    private BaseModel _owner;
    private float _timer;
    private int _dmg;
    private BaseModel _owner;

    public Bullet(GraphicsDevice graphicsDevice)
    {
      this.SpeedMovement = BULLET_SPEED_XY;
      this.Trajectory = new ParabolicTrajectory(
        BULLET_SPEED_XY, BULLET_SPEED_Z, BULLET_ACCELERATION);
    }

    public static Bullet GetNewInstance(FoodFightGame3D game, BaseModel owner, int dmg)
    {
      Bullet _instance = new Bullet(game.GraphicsDevice);
      Bullet.GameInstance = game;
      _instance.Position = Vector3.Add(owner.Position, owner.Rotation.Up);
      _instance.InitialPosition = _instance.Position;
      _instance.Rotation = owner.Rotation;
      _instance.Model = BulletModel.GetNewInstance(game);

      _instance._dmg = dmg;
      _instance._owner = owner;

      game.AllBullets.Enqueue(_instance);
      if (game.AllBullets.Count > FoodFightGame3D.NUMBER_OF_BULLET)
        game.AllBullets.Dequeue();

      return _instance;
    }

    public BaseModel GetOwner() { return this._owner; }
    public int GetDamage() { return this._dmg; }
    public void Expended() { this._timer = TIME_TO_LIVE; }
    public bool IsExpended() { return _timer >= TIME_TO_LIVE; }

    public void Update(GameTime gameTime)
    {
      _timer += gameTime.ElapsedGameTime.Milliseconds;
      this.UpdatePosition(gameTime);
    }

    public void UpdatePosition(GameTime gameTime)
    {
      this.FollowTrajectory(_timer); 
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.IsExpended())
        this.Draw(GameInstance.GetViewMatrix(),
          GameInstance.GetProjectionMatrix(), Color.Yellow);
    }

    public void Draw(Matrix view, Matrix projection, Color color)
    {
      Model.Draw(Matrix.CreateTranslation(this.Position), 
        view, projection, color);
    }
  }
}
