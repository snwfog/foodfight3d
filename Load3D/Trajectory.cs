using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FoodFight3D
{
  public class Trajectory
  {
    protected float _initVx, _initVy; // Z axis is the Y axis in 2D
    public Trajectory(float initVx, float initVy)
    {
      this._initVx = initVx;
      this._initVy = initVy;
    }

    public virtual Vector2 GetPosition(float t)
    {
      return new Vector2(_initVx * t, _initVy * t);
    }
  }

  public class ParabolicTrajectory : Trajectory
  {
    protected float _a;
    public ParabolicTrajectory(float initVx, float initVy, float a) : base(initVx, initVy)
    {
      this._a = a;
    }

    public override Vector2 GetPosition(float t)
    {
      float _x = _initVx * t;
      float _y = (float)(_initVy * t 
        - 1.0 / 2.0 * _a * t * t);
      return new Vector2(_x, _y);
    }

  }
}
