using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFight3D
{
  public class Hexagon : BasicObjectModel
  {
    public static FoodFightGame3D GameInstance;

    private CubePrimitive _shape;

    public static Matrix INCLINATION = 
      Matrix.CreateFromYawPitchRoll(
        0,
        MathHelper.ToRadians(45), 
        MathHelper.ToRadians(45));

    private Hexagon(GraphicsDevice graphics)
    {
      _shape = new CubePrimitive(graphics);
    }

    public static Hexagon GetNewInstance(FoodFightGame3D game)
    {
      Hexagon _instance = new Hexagon(game.GraphicsDevice);
      _instance.Color = Color.Red;
      return _instance;
    }

    public override void Draw(Matrix world, Matrix view, Matrix projection, Color color)
    {
      _shape.Draw(world, view, projection, color);
    }
  }
}
