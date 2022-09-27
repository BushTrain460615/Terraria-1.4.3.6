// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.EntitySource_OnHit_ByProjectileSourceID
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.DataStructures
{
  public class EntitySource_OnHit_ByProjectileSourceID : AEntitySource_OnHit
  {
    public readonly int SourceId;

    public EntitySource_OnHit_ByProjectileSourceID(
      Entity entityStriking,
      Entity entityStruck,
      int projectileSourceId)
      : base(entityStriking, entityStruck)
    {
      this.SourceId = projectileSourceId;
    }
  }
}
