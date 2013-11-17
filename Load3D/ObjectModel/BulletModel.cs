using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoodFight3D.PrimitiveShape;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFight3D.ObjectModel
{
  public class BulletModel : BasicObjectModel
  {
    public static FoodFightGame3D GameInstance;
    public static Color BULLET_COLOR = Color.Pink;

    private CubePrimitive _shape;

    private BulletModel(GraphicsDevice graphics)
    {
      _shape = new CubePrimitive(graphics, Bullet.BULLET_SIZE);
    }

    public static BulletModel GetNewInstance(FoodFightGame3D game)
    {
      BulletModel _instance = new BulletModel(game.GraphicsDevice);
      _instance.Color = Color.Pink;
      return _instance;
    }

    public override void Draw(Matrix world, Matrix view, Matrix projection, Color color)
    {
      _shape.Draw(world, view, projection, color);
    }
  }
}
