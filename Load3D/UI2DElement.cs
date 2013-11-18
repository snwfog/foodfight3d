using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFight3D
{
  public class UI2DElement
  {
    public static FoodFightGame3D GameInstance;

    private Vector2 _position;
    private IWatchableElement _watch;
    private UI2DElement(Vector2 position, IWatchableElement watch)
    {
      this._position = position;
      this._watch = watch;
    }
    
    public static UI2DElement GetNewInstance(FoodFightGame3D game, Vector2 position, IWatchableElement watch)
    {
      UI2DElement.GameInstance = game;
      UI2DElement _instance = new UI2DElement(position, watch);

      game.AllUIElements.Add(_instance);
      return _instance;
    }

    public void Update(GameTime gameTime)
    {
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      spriteBatch.DrawString(GameInstance.Mono12, _watch.GetStatus(), this._position, Color.White);
      spriteBatch.End();
    }
  }
}
