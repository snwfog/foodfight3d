using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoodFight3D.PrimitiveShape;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFight3D.ObjectModel
{
  public class PitModel : BasicObjectModel
  {
    public static FoodFightGame3D GameInstance;
    public static Matrix STRAIGHTNER_MATRIX = 
      Matrix.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90));


    private CylinderPrimitive _shape;

    private PitModel(GraphicsDevice graphics)
    {
      _shape = new CylinderPrimitive(graphics, 0.2f, 2, 32);
    }

    public static PitModel GetNewInstance(FoodFightGame3D game)
    {
      PitModel _instance = new PitModel(game.GraphicsDevice);
      _instance.Color = Color.Gray;
      return _instance;
    }

    public override void Draw(Matrix world, Matrix view, Matrix projection, Color color)
    {
      _shape.Draw(world, view, projection, color);
    }
  }
}
