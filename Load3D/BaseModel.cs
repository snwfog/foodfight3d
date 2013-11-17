using System;
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

    public float SpeedMovement;
    public float SpeedRotation;

    public Vector3 Position { get; set; }
    public Vector3 Direction { get; set; }
    public Matrix Rotation { get; set; }
    public BoundingSphere BoundingSphere { get; set; }
    protected IModel Model;
    public Color Color = Color.White;

    public BaseModel() : this(Vector3.One, Matrix.Identity)
    {
    }

    public BaseModel(Vector3 position, Matrix rotation)
    {
      Position = position;
      Rotation = rotation;
      SpeedMovement = 1;
      SpeedRotation = 1;
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

    protected void YawCounterClockwise()
    {
      this.Yaw(-1);
    }

    protected void YawClockwise()
    {
      this.Yaw(1);
    }

    protected void PitchForward()
    {
      this.Pitch(-1);
    }

    protected void PitchBackward()
    {
      this.Pitch(1);
    }

    protected void RollLeft()
    {
      this.Roll(-1);
    }

    protected void RollRight()
    {
      this.Roll(1);
    }

    protected void GoForward()
    {
      this.GoFrontBack(1);
    }

    protected void GoBackward()
    {
      this.GoFrontBack(-1);
    }

    protected void GoLeft()
    {
      this.GoLeftRight(-1);
    }

    protected void GoRight()
    {
      this.GoLeftRight(1);
    }



    protected void Yaw(float angle)
    {
      this.Rotation *= Matrix.CreateFromAxisAngle(this.Rotation.Forward, angle*SpeedRotation);
    }

    protected void Pitch(float angle)
    {
      this.Rotation *= Matrix.CreateFromAxisAngle(this.Rotation.Right, angle*SpeedRotation);
    }

    protected void Roll(float angle)
    {
      this.Rotation *= Matrix.CreateFromAxisAngle(this.Rotation.Up, angle*SpeedRotation);
    }

    protected void GoFrontBack(float distance)
    {
      this.Position += this.Rotation.Up * distance * SpeedMovement;
    }

    protected void GoLeftRight(float distance)
    {
      this.Position += this.Rotation.Right * distance * SpeedMovement;
    }

    public virtual void Draw(GameTime gameTime)
    {
      Matrix world = this.Rotation * Matrix.CreateTranslation(this.Position);
      this.Draw(gameTime, world);
    }

    public virtual void Draw(GameTime gameTime, Matrix world)
    {
      Matrix view = GameInstance.GetViewMatrix();
      Matrix projection = GameInstance.GetProjectionMatrix();

      if (this.Color == null)
        Model.Draw(world, view, projection, ((BasicObjectModel)Model).Color);
      else
        Model.Draw(world, view, projection, Color);
      
    }
  }
}
