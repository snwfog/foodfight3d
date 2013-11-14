using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FoodFighGame3D
{
  interface IAutoMoveable
  {
    void UpdatePosition(GameTime gameTime);
  }
}