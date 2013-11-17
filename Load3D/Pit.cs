using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoodFight3D.ObjectModel;
using Microsoft.Xna.Framework;

namespace FoodFight3D
{
  public class Pit : BaseModel
  {

    private EnemyCraft _owner;

    private Pit(Vector3 position) : base(position, Matrix.Identity)
    {
    }

    public static Pit GetNewInstance(FoodFightGame3D game, Vector3 position)
    {
      return Pit.GetNewInstance(game, position, Color.Gray);
    }

    public static Pit GetNewInstance(FoodFightGame3D game, Vector3 position, Color color)
    {
      Pit _pit = new Pit(position);
      PowerUp.GameInstance = game;
      _pit.Model = PitModel.GetNewInstance(game);
      _pit.Color = color;

      game.AllPits.Enqueue(_pit);
      if (game.AllPits.Count > FoodFightGame3D.NUMBER_OF_PIT)
        game.AllPits.Dequeue();

      return _pit;
    }

    public void TakeOverBy(EnemyCraft craft)
    {
      _owner = craft;
      craft.Position = this.Position + (new Vector3(0, 0, 1));
    }

    public void Evict() { _owner = null; }
    public bool IsOccupied() { return this.IsFree(); }
    public bool IsFree() { return _owner == null; }

    public void Update(GameTime gameTime) { }

    public override void Draw(GameTime gameTime)
    {
      this.Update(gameTime);

      Matrix world = PitModel.STRAIGHTNER_MATRIX * Matrix.CreateTranslation(this.Position);
      base.Draw(gameTime, world);
    }
  }
}
