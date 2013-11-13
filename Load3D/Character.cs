using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FoodFightGame3D
{
  class Character
  {
    public Vector3 IDLE_DIRECTION = new Vector3(0, 0, 0);
    public Vector3 INITIAL_POSITION = new Vector3(5, 0, 0);
    public float SPEED_FACTOR = 0.05f;

    private Vector3 _direction;
    private Vector3 _position;
    private Matrix _world;
    private Model _model { get; set; }
    private Matrix _view { get; set; }
    private Matrix _projection { get; set; }

    private Character()
    {
      this._direction = this.IDLE_DIRECTION;
      this._position = this.INITIAL_POSITION;
    }

    private static Character Initialize(FoodFightGame3D game)
    {
      Character _instance = new Character();
      _instance._model = game.Content.Load<Model>("Ship");
      _instance._view = game.GetViewMatrix();
      _instance._projection = game.GetProjectionMatrix();

      return _instance;
    }

    public static Character GetNewInstance(FoodFightGame3D game)
    {
      return Initialize(game);
    }

    private void _MovePosition(GameTime gameTime)
    {
      KeyboardState ks = Keyboard.GetState();
      if (ks.IsKeyDown(Keys.W) && ks.IsKeyDown(Keys.D))
      {
        this._direction = new Vector3(0.707f, 0.707f, 0);
      }
      else if (ks.IsKeyDown(Keys.D) && ks.IsKeyDown(Keys.S))
      {
        this._direction = new Vector3(0.707f, -0.707f, 0);
      }
      else if (ks.IsKeyDown(Keys.S) && ks.IsKeyDown(Keys.A))
      {
        this._direction = new Vector3(-0.707f, -0.707f, 0);
      }
      else if (ks.IsKeyDown(Keys.A) && ks.IsKeyDown(Keys.W))
      {
        this._direction = new Vector3(-0.707f, 0.707f, 0);
      }
      else if (ks.IsKeyDown(Keys.W))
      {
        this._direction = new Vector3(0.0f, 1.0f, 0);
      }
      else if (ks.IsKeyDown(Keys.D))
      {
        this._direction = new Vector3(1.0f, 0.0f, 0);
      }
      else if (ks.IsKeyDown(Keys.S))
      {
        this._direction = new Vector3(0.0f, -1.0f, 0);
      }
      else if (ks.IsKeyDown(Keys.A))
      {
        this._direction = new Vector3(-1.0f, 0.0f, 0);
      }
      else
      {
        this._direction = this.IDLE_DIRECTION;
      }

      this._position += Vector3.Multiply(this._direction, this.SPEED_FACTOR);
    }

    public void Update(GameTime gameTime)
    {
      this._MovePosition(gameTime); // Move model to new _position
      this._world = Matrix.CreateTranslation(this._position);
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
