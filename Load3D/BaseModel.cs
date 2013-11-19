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
    public Vector3 InitialPosition { get; set; }
    public Vector3 Direction { get; set; }
    public Matrix Rotation { get; set; }

    public BoundingSphere BoundingSphere { get; set; }
    public CubePrimitive BoundingBox { get; set; }
    public Trajectory Trajectory { get; set; }

    protected IModel Model;
    public Color Color = Color.White;

    public BaseModel()
      : this(Vector3.One, Matrix.Identity)
    {
    }

    public BaseModel(Vector3 position, Matrix rotation)
    {
      Position = position;
      Rotation = rotation;
      SpeedMovement = 1;
      SpeedRotation = 1;
      BoundingSphere = new BoundingSphere(Vector3.Zero, 1.0f);
      Trajectory = new Trajectory(SpeedMovement, 0);
    }

    public Matrix GetWorldTransform()
    {
      return this.Rotation * Matrix.CreateTranslation(this.Position);
    }


    public bool Intersect(BaseModel model)
    {
      return this.BoundingSphere.Transform(this.GetWorldTransform()).Intersects(
        model.BoundingSphere.Transform(model.GetWorldTransform()));
      return this.BoundingSphere.Intersects(model.BoundingSphere);
//      foreach (BoundingSphere _sphere in this.GetBoundingSphere())
//        foreach (BoundingSphere _otherSphere in model.GetBoundingSphere())
//          if (_sphere.Transform(this.GetWorldTransform())
//              .Intersects(_otherSphere.Transform(model.GetWorldTransform())))
//            return true;

//      return false;
    }

    public virtual List<BoundingSphere> GetBoundingSphere()
    {
      return this.Model.GetBoundingSpheres();
    }

    public virtual void Rotate(GameTime gameTime)
    {
      // http://stackoverflow.com/questions/8912931/xna-create-a-rotation-about-the-forward-vector 
    }

    protected void YawCounterClockwise() { this.Yaw(-1); }
    protected void YawClockwise() { this.Yaw(1); }

    protected void PitchForward() { this.Pitch(-1); }
    protected void PitchBackward() { this.Pitch(1); }

    protected void RollLeft() { this.Roll(-1); }
    protected void RollRight() { this.Roll(1); }

    protected void GoForward() { this.GoFrontBack(1); }
    protected void GoBackward() { this.GoFrontBack(-1); }

    protected void GoLeft() { this.GoLeftRight(-1); }
    protected void GoRight() { this.GoLeftRight(1); } 

    protected void Yaw(float angle)
    {
      this.Rotation *= Matrix.CreateFromAxisAngle(this.Rotation.Forward, angle * SpeedRotation);
    }

    protected void Pitch(float angle)
    {
      this.Rotation *= Matrix.CreateFromAxisAngle(this.Rotation.Right, angle * SpeedRotation);
    }

    protected void Roll(float angle)
    {
      this.Rotation *= Matrix.CreateFromAxisAngle(this.Rotation.Up, angle * SpeedRotation);
    }

    protected void GoFrontBack(float distance)
    {
      this.Position += this.Rotation.Up * distance * SpeedMovement;
    }

    protected void GoLeftRight(float distance)
    {
      this.Position += this.Rotation.Right * distance * SpeedMovement;
    }

    protected Vector3 GetPosition(float time)
    {
      return this.Position;
    }

    protected void FollowTrajectory(float time)
    {
      Vector2 _position = Trajectory.GetPosition(time);
      float _x = this.InitialPosition.X + this.Rotation.Up.X * _position.X;
      float _y = this.InitialPosition.Y + this.Rotation.Up.Y * _position.X;
      float _z = this.InitialPosition.Z + 0.000002f * _position.Y;

      this.Position = new Vector3(_x, _y, _z);
    }

    public virtual void Draw(GameTime gameTime)
    {
      this.Draw(gameTime, this.GetWorldTransform());
    }

    public virtual void Draw(GameTime gameTime, Matrix world)
    {
      this.Draw(gameTime, world, this.Color);
    }

    public virtual void Draw(GameTime gameTime, Matrix world, Color color)
    {
      Matrix view = GameInstance.GetViewMatrix();
      Matrix projection = GameInstance.GetProjectionMatrix();

      Model.Draw(world, view, projection, color);

    }
  }
}
