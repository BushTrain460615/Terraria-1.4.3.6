// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.TeleportPylonInfo
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;
using Terraria.DataStructures;

namespace Terraria.GameContent
{
  public struct TeleportPylonInfo : IEquatable<TeleportPylonInfo>
  {
    public Point16 PositionInTiles;
    public TeleportPylonType TypeOfPylon;

    public bool Equals(TeleportPylonInfo other) => this.PositionInTiles == other.PositionInTiles && this.TypeOfPylon == other.TypeOfPylon;
  }
}
