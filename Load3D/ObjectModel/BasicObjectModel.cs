using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFight3D
{
  public abstract class BasicObjectModel : IModel
  {
    public Model Model { get; set; }

    public Matrix World { get; set; }
    public Matrix View { get; set; }
    public Matrix Projection { get; set; }
    public Color Color { get; set; }

    public Texture2D Texture { get; set; }

    public virtual void Draw(Matrix world, Matrix view, Matrix projection, Color color)
    {
      this.DrawModel(this.Model, world, view, projection, color, this.Texture);
    }

    public virtual void Draw()
    {
      this.DrawModel(this.Model, this.World, this.View, this.Projection, this.Color, this.Texture);
    }

    protected void DrawModel(Model model, Matrix world, Matrix view, Matrix projection, 
      Color color, Texture2D texture)
    {
      foreach (ModelMesh mesh in model.Meshes)
      {
        foreach (BasicEffect effect in mesh.Effects)
        {
          if (texture != null)
          {
            effect.TextureEnabled = true;
            effect.Texture = texture;
          }

          effect.DiffuseColor = color.ToVector3();
          effect.World = world;
          effect.View = view;
          effect.Projection = projection;
        }

        mesh.Draw();
      }
    }
  }
}
