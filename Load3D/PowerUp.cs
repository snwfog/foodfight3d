using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FoodFight3D
{
  public class PowerUp : BaseModel
  {
    public static FoodFightGame3D GameInstance;
    public static float ROTATION_SPEED = 0.5f;
    public static Matrix INCLINATION = Matrix.Multiply(
      Matrix.CreateRotationZ((float)Math.PI / 2)
      , Matrix.CreateRotationX((float)Math.PI / 2));


    private PowerUp(Vector3 position) : base(position, Matrix.Identity) {}

    public static PowerUp GetNewInstance(FoodFightGame3D game)
    {
      PowerUp _powerUp = new PowerUp(Vector3.One);
      PowerUp.GameInstance = game;
      _powerUp.Model = Hexagon.GetNewInstance(game);

      return _powerUp;
    }

    public override void Draw(GameTime gameTime)
    {
      Matrix world = Matrix.CreateTranslation(this.Position);
      Matrix view = GameInstance.GetViewMatrix();
      Matrix projection = GameInstance.GetProjectionMatrix();

      Model.Draw(world, view, projection, ((BasicObjectModel)Model).Color);
    }
  }
}
