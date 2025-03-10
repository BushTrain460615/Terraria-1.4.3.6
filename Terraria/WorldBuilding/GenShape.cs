﻿// Decompiled with JetBrains decompiler
// Type: Terraria.WorldBuilding.GenShape
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
  public abstract class GenShape : GenBase
  {
    private ShapeData _outputData;
    protected bool _quitOnFail;

    public abstract bool Perform(Point origin, GenAction action);

    protected bool UnitApply(GenAction action, Point origin, int x, int y, params object[] args)
    {
      if (this._outputData != null)
        this._outputData.Add(x - origin.X, y - origin.Y);
      return action.Apply(origin, x, y, args);
    }

    public GenShape Output(ShapeData outputData)
    {
      this._outputData = outputData;
      return this;
    }

    public GenShape QuitOnFail(bool value = true)
    {
      this._quitOnFail = value;
      return this;
    }
  }
}
