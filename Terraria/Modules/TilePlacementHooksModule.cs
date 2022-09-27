// Decompiled with JetBrains decompiler
// Type: Terraria.Modules.TilePlacementHooksModule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.DataStructures;

namespace Terraria.Modules
{
  public class TilePlacementHooksModule
  {
    public PlacementHook check;
    public PlacementHook postPlaceEveryone;
    public PlacementHook postPlaceMyPlayer;
    public PlacementHook placeOverride;

    public TilePlacementHooksModule(TilePlacementHooksModule copyFrom = null)
    {
      if (copyFrom == null)
      {
        this.check = new PlacementHook();
        this.postPlaceEveryone = new PlacementHook();
        this.postPlaceMyPlayer = new PlacementHook();
        this.placeOverride = new PlacementHook();
      }
      else
      {
        this.check = copyFrom.check;
        this.postPlaceEveryone = copyFrom.postPlaceEveryone;
        this.postPlaceMyPlayer = copyFrom.postPlaceMyPlayer;
        this.placeOverride = copyFrom.placeOverride;
      }
    }
  }
}
