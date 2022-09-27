// Decompiled with JetBrains decompiler
// Type: Terraria.WorldBuilding.GenBase
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.Utilities;

namespace Terraria.WorldBuilding
{
  public class GenBase
  {
    protected static UnifiedRandom _random => WorldGen.genRand;

    protected static Tile[,] _tiles => Main.tile;

    protected static int _worldWidth => Main.maxTilesX;

    protected static int _worldHeight => Main.maxTilesY;

    public delegate bool CustomPerUnitAction(int x, int y, params object[] args);
  }
}
