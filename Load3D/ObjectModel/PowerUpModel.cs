using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFight3D
{
  public class PowerUpModel : BasicObjectModel
  {
    public static FoodFightGame3D GameInstance;

    public static PowerUpModel GetNewInstance(FoodFightGame3D game, PowerUp.PowerUpType type)
    {
      PowerUpModel _instance = new PowerUpModel();

      string _modelName = "MODEL_PEAR";
      float _scaleFactor = 0.01f;
      Color _color = Color.Pink;

      switch (type)
      {
        case PowerUp.PowerUpType.PEAR:
          _modelName = "MODEL_PEAR";
          _scaleFactor = 0.01f;
          _color = Color.Green;
          break;
        case PowerUp.PowerUpType.APPLE:
          _modelName = "MODEL_APPLE";
          _scaleFactor = 0.01f;
          _color = Color.OrangeRed;
          break;
        case PowerUp.PowerUpType.LEMON:
          _modelName = "MODEL_LEMON_2";
          _scaleFactor = 0.012f;
          _color = Color.Yellow;
          break;
        case PowerUp.PowerUpType.ORANGE:
          _modelName = "MODEL_ORANGE";
          _scaleFactor = 0.25f;
          _color = Color.Orange;
          break;
      }


      _instance.Model = game.Content.Load<Model>(_modelName);
      _instance.World = Matrix.Identity * Matrix.CreateScale(_scaleFactor);
      _instance.View = game.GetViewMatrix();
      _instance.Projection = game.GetProjectionMatrix();
      _instance.Color = _color;

      return _instance;
    }

    public override void Draw(Matrix world, Matrix view, Matrix projection, Color color)
    {
      base.Draw(this.World * world, view, projection, this.Color);
    }
  }
}
