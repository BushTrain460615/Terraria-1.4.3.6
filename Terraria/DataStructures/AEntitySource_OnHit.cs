// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.AEntitySource_OnHit
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.DataStructures
{
  public abstract class AEntitySource_OnHit : IEntitySource
  {
    public readonly Entity EntityStriking;
    public readonly Entity EntityStruck;

    public AEntitySource_OnHit(Entity entityStriking, Entity entityStruck)
    {
      this.EntityStriking = entityStriking;
      this.EntityStruck = entityStruck;
    }
  }
}
