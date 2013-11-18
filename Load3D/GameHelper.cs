using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FoodFight3D
{
  public class GameHelper
  {
    public static int MAX_X = 11;
    public static int MAX_Y = 11;
    public static Random RANDOM = new Random();
    List<Vector2> _position = new List<Vector2>();
    List<Vector2> _occupied = new List<Vector2>(); 

    private static GameHelper _instance;

    public GameHelper()
    {
      for (int _x = 0; _x < MAX_X; _x += 2)
      {
        for (int _y = 0; _y < MAX_Y; _y += 2)
        {
          _position.Add(new Vector2(_x, _y));
          _position.Add(new Vector2(-_x, _y));
          _position.Add(new Vector2(-_x, -_y));
          _position.Add(new Vector2(_x, -_y));

        }
      }
    }

    public static Vector2 GetPosition()
    {
      if (_instance == null)
        _instance = new GameHelper();

      Vector2 _p = Vector2.Zero;
      // Hashing function here
      for (int i = RANDOM.Next(_instance._position.Count),
        d = 1 + RANDOM.Next(_instance._position.Count),
        j = 0, k = i;
        j < _instance._position.Count;
        ++j, k = (i + j * d) % _instance._position.Count)
      {
        _p = _instance._position[k];
        if (!_instance._occupied.Contains(_p))
          break;
      }

      return _p;
    }
  }
}
