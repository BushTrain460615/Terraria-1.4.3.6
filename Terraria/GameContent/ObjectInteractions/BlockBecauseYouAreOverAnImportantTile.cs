﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.ObjectInteractions.BlockBecauseYouAreOverAnImportantTile
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.GameContent.ObjectInteractions
{
  public class BlockBecauseYouAreOverAnImportantTile : ISmartInteractBlockReasonProvider
  {
    public bool ShouldBlockSmartInteract(SmartInteractScanSettings settings)
    {
      int tileTargetX = Player.tileTargetX;
      int tileTargetY = Player.tileTargetY;
      if (!WorldGen.InWorld(tileTargetX, tileTargetY, 10))
        return true;
      Tile tile = Main.tile[tileTargetX, tileTargetY];
      if (tile == null)
        return true;
      if (tile.active())
      {
        switch (tile.type)
        {
          case 4:
          case 33:
          case 334:
          case 395:
          case 410:
          case 455:
          case 471:
          case 480:
          case 509:
          case 520:
            return true;
        }
      }
      return false;
    }
  }
}
