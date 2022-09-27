// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.AEntitySource_Tile
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
  public abstract class AEntitySource_Tile : IEntitySource
  {
    public readonly Point TileCoords;

    public AEntitySource_Tile(int tileCoordsX, int tileCoordsY) => this.TileCoords = new Point(tileCoordsX, tileCoordsY);
  }
}
