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
    public static float BULLET_SPEED = 0.15f;
    public static float BULLET_SIZE = 0.1f;
    public static float TIME_TO_LIVE = 5000;

//    private BaseModel _owner;
    private float _timer = 0;

    public Bullet(GraphicsDevice graphicsDevice)
    {
      SpeedMovement = BULLET_SPEED;
    }

    public static Bullet GetNewInstance(FoodFightGame3D game, BaseModel owner)
    {
      Bullet _instance = new Bullet(game.GraphicsDevice);
      Bullet.GameInstance = game;
      _instance.Position = Vector3.Add(owner.Position, owner.Rotation.Up);
      _instance.Rotation = owner.Rotation;
      _instance.Model = BulletModel.GetNewInstance(game);

      game.AllBullets.Enqueue(_instance);
      if (game.AllBullets.Count > FoodFightGame3D.NUMBER_OF_BULLET)
        game.AllBullets.Dequeue();

      return _instance;
    }

    public bool IsDead()
    {
      return _timer >= TIME_TO_LIVE;
    }

    public void Update(GameTime gameTime)
    {
      _timer += gameTime.ElapsedGameTime.Milliseconds;
      this.UpdatePosition(gameTime);
    }

    public void UpdatePosition(GameTime gameTime)
    {
      this.GoForward();
    }

    public override void Draw(GameTime gameTime)
    {
      this.Draw(GameInstance.GetViewMatrix(), GameInstance.GetProjectionMatrix(), Color.Yellow);
      
    }

    public void Draw(Matrix view, Matrix projection, Color color)
    {
      Model.Draw(Matrix.CreateTranslation(this.Position), view, projection, color);
    }
  }
}
