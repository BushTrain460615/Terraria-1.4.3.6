// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.TileDataType
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;

namespace Terraria.DataStructures
{
  [Flags]
  public enum TileDataType
  {
    Tile = 1,
    TilePaint = 2,
    Wall = 4,
    WallPaint = 8,
    Liquid = 16, // 0x00000010
    Wiring = 32, // 0x00000020
    Actuator = 64, // 0x00000040
    Slope = 128, // 0x00000080
    All = Slope | Actuator | Wiring | Liquid | WallPaint | Wall | TilePaint | Tile, // 0x000000FF
  }
}
