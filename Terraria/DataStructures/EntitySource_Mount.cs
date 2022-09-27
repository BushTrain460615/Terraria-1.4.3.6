// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.EntitySource_Mount
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.DataStructures
{
  public class EntitySource_Mount : IEntitySource
  {
    public readonly Entity Entity;
    public readonly int MountId;

    public EntitySource_Mount(Entity entity, int mountId)
    {
      this.Entity = entity;
      this.MountId = mountId;
    }
  }
}
