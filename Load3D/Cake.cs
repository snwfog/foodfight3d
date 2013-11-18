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
    public static int TIME_TO_LIVE = 600000;
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

      if (_timeToLive < TIME_TO_LIVE * 0.05)
        this.Model = CakeModel.GetNewInstance(GameInstance, 0.05f);
      else if (_timeToLive < TIME_TO_LIVE * 0.25)
        this.Model = CakeModel.GetNewInstance(GameInstance, 0.25f);
      else if (_timeToLive < TIME_TO_LIVE * 0.50)
        this.Model = CakeModel.GetNewInstance(GameInstance, 0.50f);
      else if (_timeToLive < TIME_TO_LIVE * 0.75)
        this.Model = CakeModel.GetNewInstance(GameInstance, 0.75f);
      else if (_timeToLive < TIME_TO_LIVE * 1.00)
        this.Model = CakeModel.GetNewInstance(GameInstance, 1.00f);

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
