using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodFight3D
{
  public class AmmoSlot : IWatchableElement
  {
    private Stack<PowerUp> _ammoSlot;

    public AmmoSlot() { _ammoSlot = new Stack<PowerUp>(); }
    public void ChargeAmmo(PowerUp powerUp) { _ammoSlot.Push(powerUp); }
    public PowerUp UseAmmo() { return _ammoSlot.Pop(); }
    public bool HasAmmo() { return this.Size() > 0; }
    public int Size() { return _ammoSlot.Count; }

    public string GetStatus()
    {
      if (_ammoSlot.Count == 0)
        return "<Empty>";

      PowerUp top = _ammoSlot.Peek();
      return "<" + top.GetType().ToString() + ">" + " [" + top.GetValue() + "]";
    }
  }
}
