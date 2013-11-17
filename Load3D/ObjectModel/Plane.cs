using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FoodFight3D
{
  public class Plane : BasicObjectModel
  {
    public static FoodFightGame3D GameInstance;

    public enum CraftType
    {
      JIMMY, ENEMY_TYPE_1, ENEMY_TYPE_2, ENEMY_TYPE_3, ENEMY_TYPE_4, SPECIAL
    }

    public List<BoundingSphere> GetBoundingSphere()
    {
        foreach (ModelMesh _mesh in this.Model.Meshes)
            this.BoundingSphere.Add(_mesh.BoundingSphere);
        return this.BoundingSphere;
    }

    public static Plane GetNewInstance(FoodFightGame3D game, CraftType type)
    {
      Plane _instance = new Plane();
      _instance.Model = game.Content.Load<Model>("Ship");
      _instance.World = Matrix.Identity;
      _instance.View = game.GetViewMatrix();
      _instance.Projection = game.GetProjectionMatrix();

      switch (type)
      {
        case CraftType.JIMMY:
          _instance.Texture = game.Content.Load<Texture2D>("SHIP_TEXTURE_02");
          break;
        case CraftType.ENEMY_TYPE_1:
          _instance.Texture = game.Content.Load<Texture2D>("SHIP_TEXTURE_06");
          break;
        case CraftType.ENEMY_TYPE_2:
          _instance.Texture = game.Content.Load<Texture2D>("SHIP_TEXTURE_07");
          break;
        case CraftType.ENEMY_TYPE_3:
          _instance.Texture = game.Content.Load<Texture2D>("SHIP_TEXTURE_08");
          break;
        case CraftType.ENEMY_TYPE_4:
          _instance.Texture = game.Content.Load<Texture2D>("SHIP_TEXTURE_09");
          break;
        case CraftType.SPECIAL:
          _instance.Texture = game.Content.Load<Texture2D>("SHIP_TEXTURE_11");
          break;
      }

      return _instance;
    }
  }
}
