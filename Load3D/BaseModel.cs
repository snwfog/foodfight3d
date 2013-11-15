﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FoodFight3D
{
  public abstract class BaseModel
  {
    public static Random RANDOM = new Random();
    public static FoodFightGame3D GameInstance { get; set; }

    public Vector3 Position { get; set; }
    public Vector3 Direction { get; set; }
    public Matrix Rotation { get; set; }
    public float Speed { get; set; }
    public BoundingSphere BoundingSphere { get; set; }
    protected IModel Model;

    public BaseModel() : this(Vector3.One, Matrix.Identity) {}

    public BaseModel(Vector3 position, Matrix rotation)
    {
      Position = position;
      Rotation = rotation;
      Speed = 1;
      BoundingSphere = new BoundingSphere(position, 1.0f);
    }

    public virtual BoundingSphere GetBoundingSphere()
    {
      return new BoundingSphere(Position, 1);
    }

    public virtual void Rotate(GameTime gameTime)
    {
      // http://stackoverflow.com/questions/8912931/xna-create-a-rotation-about-the-forward-vector 
    }

    protected void Yaw(float angle)
    {
      this.Rotation *= Matrix.CreateFromAxisAngle(this.Rotation.Up, angle * Speed);
    }

    protected void GoForward(float distance)
    {
      this.Position += this.Rotation.Up * distance * Speed;
    }

    public abstract void Draw(GameTime gameTime);
  }
}
