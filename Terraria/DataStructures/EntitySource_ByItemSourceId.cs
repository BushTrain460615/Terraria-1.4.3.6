// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.EntitySource_ByItemSourceId
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.DataStructures
{
  public class EntitySource_ByItemSourceId : IEntitySource
  {
    public readonly Entity Entity;
    public readonly int SourceId;

    public EntitySource_ByItemSourceId(Entity entity, int itemSourceId)
    {
      this.Entity = entity;
      this.SourceId = itemSourceId;
    }
  }
}
