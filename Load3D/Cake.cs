using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoodFight3D.ObjectModel;
using Microsoft.Xna.Framework;

namespace FoodFight3D
{
  public class Cake : BaseModel, IWatchableElement
  {

    public static FoodFightGame3D GameInstance;
    public static int TIME_TO_LIVE = FoodFightGame3D.GAME_TIME;
    public static int BAR_COUNT = 20;

    private int _timeToLive;

    private Cake(Vector3 position) : base(position, Matrix.Identity)
    {
      _timeToLive = TIME_TO_LIVE;
    }

    public static Cake GetNewInstance(FoodFightGame3D game, Vector3 position)
    {
      Cake.GameInstance = game;
      Cake _cake = new Cake(position);
      _cake.Color = Color.Pink;
      _cake.Model = CakeModel.GetNewInstance(game, 1.00f);

      return _cake;
    }

    public void Update(GameTime gameTime)
    {
      _timeToLive -= gameTime.ElapsedGameTime.Milliseconds;
      if (_timeToLive <= 0)
        throw new GameOver("Game Over");
    }

    public int GetTimeToLive() { return _timeToLive; }
    public string GetStatus()
    {
      int bars = _timeToLive * BAR_COUNT / TIME_TO_LIVE;
      StringBuilder sb = new StringBuilder();

      for (int i = 0; i < bars; i++)
        sb.Append("|");

      return sb.ToString();
    }
  }
}
