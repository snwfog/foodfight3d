using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoodFight3D.PrimitiveShape;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFight3D.ObjectModel
{
  public class CakeModel : BasicObjectModel
  {
    public static FoodFightGame3D GameInstance;
    public static Matrix STRAIGHTNER_MATRIX = 
      Matrix.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90));

    public static int CAKE_HEIGHT = 2;

    private CylinderPrimitive _shape;

    private CakeModel(GraphicsDevice graphics, float height)
    {
      _shape = new CylinderPrimitive(graphics, CAKE_HEIGHT * height, 2, 32);
    }

    public static CakeModel GetNewInstance(FoodFightGame3D game, float height)
    {
      CakeModel _instance = new CakeModel(game.GraphicsDevice, height);
      _instance.Color = Color.Pink;
      return _instance;
    }

    public override void Draw(Matrix world, Matrix view, Matrix projection, Color color)
    {
      _shape.Draw(STRAIGHTNER_MATRIX * world, view, projection, color);
    }
  }
}
