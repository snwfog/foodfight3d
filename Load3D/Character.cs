using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using FoodFighGame3D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FoodFightGame3D
{
  public class Character : BaseModel
  {
    public static float SPEED = 0.05f;

    private Matrix _world;
    private Model _model { get; set; }
    private Matrix _view { get; set; }
    private Matrix _projection { get; set; }

    private Character(Vector3 position, Matrix rotation) : base(position, rotation)
    {
      Speed = SPEED;
    }

    private static Character _Initialize(FoodFightGame3D game)
    {
      Character _instance = new Character(Vector3.Zero, Matrix.Identity);
      _instance._model = game.Content.Load<Model>("Ship");
      _instance._view = game.GetViewMatrix();
      _instance._projection = game.GetProjectionMatrix();

      return _instance;
    }

    public static Character GetNewInstance(FoodFightGame3D game)
    {
      return _Initialize(game);
    }

    private void _MovePosition(GameTime gameTime)
    {
      KeyboardState ks = Keyboard.GetState();
      if (ks.IsKeyDown(Keys.W) && ks.IsKeyDown(Keys.D))
      {
        this.Direction = new Vector3(0.707f, 0.707f, 0);
      }
      else if (ks.IsKeyDown(Keys.D) && ks.IsKeyDown(Keys.S))
      {
        this.Direction = new Vector3(0.707f, -0.707f, 0);
      }
      else if (ks.IsKeyDown(Keys.S) && ks.IsKeyDown(Keys.A))
      {
        this.Direction = new Vector3(-0.707f, -0.707f, 0);
      }
      else if (ks.IsKeyDown(Keys.A) && ks.IsKeyDown(Keys.W))
      {
        this.Direction = new Vector3(-0.707f, 0.707f, 0);
      }
      else if (ks.IsKeyDown(Keys.W))
      {
        this.Direction = new Vector3(0.0f, 1.0f, 0);
      }
      else if (ks.IsKeyDown(Keys.D))
      {
        this.Direction = new Vector3(1.0f, 0.0f, 0);
      }
      else if (ks.IsKeyDown(Keys.S))
      {
        this.Direction = new Vector3(0.0f, -1.0f, 0);
      }
      else if (ks.IsKeyDown(Keys.A))
      {
        this.Direction = new Vector3(-1.0f, 0.0f, 0);
      }
      else
      {
        this.Direction = Vector3.Zero;
      }

      this.Position += Vector3.Multiply(this.Direction, this.Speed);
    }

    public void Update(GameTime gameTime)
    {
      this._MovePosition(gameTime); // Move model to new _position
      this._world = Matrix.CreateTranslation(this.Position);
    }

    public void Draw(GameTime gameTime)
    {
      DrawModel(this._model, this._world, this._view, this._projection);
    }

    private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
    {
      foreach (ModelMesh mesh in model.Meshes)
      {
        foreach (BasicEffect effect in mesh.Effects)
        {
          //effect.TextureEnabled = false;
          //effect.Texture = otherTexture;
          effect.World = world;
          effect.View = view;
          effect.Projection = projection;
        }

        mesh.Draw();
      }
    }
  }
}
