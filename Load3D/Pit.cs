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

    public Pit(Vector3 position) : base(position, Matrix.Identity)
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

      game.AllPits.Add(_pit);

      return _pit;
    }

    public EnemyCraft GetOwner() { return this._owner; }
    public void TakeOverBy(EnemyCraft craft)
    {
      _owner = craft;
      craft.OccupingPit(this);
      craft.Position = this.Position + (new Vector3(0, 0, 1));
    }

    public void TakingOverBy(EnemyCraft craft)
    {
      _owner = craft;
      craft.GetOccupyingPit().Evict();
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

    public static Pit GetAFreePit()
    {
      Pit _pit = GameInstance.AllPits.First();
      // Hashing function here
      for (int i = RANDOM.Next(GameInstance.AllPits.Count),
        d = 1 + (RANDOM.Next(GameInstance.AllPits.Count - 1)),
        j = 0,
        k = i;
        j < GameInstance.AllPits.Count;
        ++j, k = (i + j * d) % GameInstance.AllPits.Count)
      {
        _pit = GameInstance.AllPits[k];
        if (_pit.IsFree())
        {
          break;
        }
      }

      return _pit;
    }
  }
}
