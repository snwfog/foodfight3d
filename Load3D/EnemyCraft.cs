using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FoodFight3D
{
  public class EnemyCraft : BaseModel
  {
    public static float SPEED = 0.05f;

    private EnemyCraft(Vector3 position, Matrix rotation) : base(position, rotation)
    {
      Speed = SPEED;

    }



    private void _MovePosition(GameTime gameTime)
    {
      
    }

    public override void Draw(GameTime gameTime)
    {
    }
  }
}
