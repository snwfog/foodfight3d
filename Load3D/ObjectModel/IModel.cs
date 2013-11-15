using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FoodFight3D
{
  public interface IModel
  {
    void Draw(Matrix world, Matrix view, Matrix projection, Color color);
  }
}
