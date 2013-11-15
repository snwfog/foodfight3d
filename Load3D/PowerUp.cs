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
    public static float ROTATION_SPEED = 1.5f;

    private PowerUp(Vector3 position) : base(position, Matrix.Identity) {}

    public static PowerUp GetNewInstance(FoodFightGame3D game)
    {
      PowerUp _powerUp = new PowerUp(Vector3.One);
      PowerUp.GameInstance = game;
      _powerUp.Model = Hexagon.GetNewInstance(game);

      return _powerUp;
    }

    public void Update(GameTime gameTime)
    {
      this.Rotation *= Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(ROTATION_SPEED), 0, 0);
    }

    public override void Draw(GameTime gameTime)
    {
      this.Update(gameTime);

      Matrix world = this.Rotation * Matrix.CreateTranslation(this.Position);
      Matrix view = GameInstance.GetViewMatrix();
      Matrix projection = GameInstance.GetProjectionMatrix();

      Model.Draw(world, view, projection, ((BasicObjectModel)Model).Color);
    }
  }
}
