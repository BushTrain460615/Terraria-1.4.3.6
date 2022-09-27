// Decompiled with JetBrains decompiler
// Type: Terraria.Enums.AnchorType
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;

namespace Terraria.Enums
{
  [Flags]
  public enum AnchorType
  {
    None = 0,
    SolidTile = 1,
    SolidWithTop = 2,
    Table = 4,
    SolidSide = 8,
    Tree = 16, // 0x00000010
    AlternateTile = 32, // 0x00000020
    EmptyTile = 64, // 0x00000040
    SolidBottom = 128, // 0x00000080
  }
}
