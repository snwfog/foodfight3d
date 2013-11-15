using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFight3D
{
  public class Plane : BasicObjectModel
  {
    public static FoodFightGame3D GameInstance;

    private Matrix _world { get; set; }
    private Matrix _view { get; set; }
    private Matrix _projection { get; set; }

    public static Plane GetNewInstance(FoodFightGame3D game)
    {
      return _Initialize(game);
    }

    private static Plane _Initialize(FoodFightGame3D game)
    {
      Plane _instance = new Plane();
      _instance.Model = game.Content.Load<Model>("Ship");
      _instance._world = Matrix.Identity;
      _instance._view = game.GetViewMatrix();
      _instance._projection = game.GetProjectionMatrix();

      _instance.Texture = game.Content.Load<Texture2D>("SHIP_TEXTURE_09");

      return _instance;
    }

    public override void Draw(Matrix world, Matrix view, Matrix projection, Color color)
    {
      base.DrawModel(this.Model, world, view, projection, color, this.Texture);
    }
  }
}
