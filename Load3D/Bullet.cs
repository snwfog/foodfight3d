using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using FoodFighGame3D.PrimitiveShape;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFighGame3D
{
  public class Bullet : CubePrimitive, IAutoMoveable
  {
    public static float SPEED = 0.4f;
    private BaseModel _owner;

    public Bullet(GraphicsDevice graphicsDevice) : base(graphicsDevice, 0.1f)
    {
      Speed = SPEED;
    }

    public static Bullet GetNewInstance(GraphicsDevice graphicsDevice, BaseModel owner)
    {
      Bullet _instance = new Bullet(graphicsDevice);
      _instance.Position = owner.Position;
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
  }
}
