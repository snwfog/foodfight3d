using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFight3D
{
  public class Bullet : BaseModel, IAutoMoveable
  {
    public static float SPEED = 0.4f;
    public static float SIZE = 0.1f;
//    private BaseModel _owner;

    public Bullet(GraphicsDevice graphicsDevice)
    {
      Speed = SPEED;
      BoundingSphere = new BoundingSphere(Position, SIZE);
      Model = new CubePrimitive(graphicsDevice, SIZE);
    }

    public static Bullet GetNewInstance(FoodFightGame3D game, BaseModel owner)
    {
      Bullet _instance = new Bullet(game.GraphicsDevice);
      Bullet.GameInstance = game;
      _instance.Position = Vector3.Add(owner.Position, new Vector3(0, 0.75f, 0));
      _instance.Rotation = owner.Rotation;

      return _instance;
    }

    public void Update(GameTime gameTime)
    {
      this.UpdatePosition(gameTime);
    }

    public void UpdatePosition(GameTime gameTime)
    {
      this.GoForward(0.05f);
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
