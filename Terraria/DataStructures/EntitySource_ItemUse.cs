// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.EntitySource_ItemUse
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.DataStructures
{
  public class EntitySource_ItemUse : IEntitySource
  {
    public readonly Entity Entity;
    public readonly Item Item;

    public EntitySource_ItemUse(Entity entity, Item item)
    {
      this.Entity = entity;
      this.Item = item;
    }
  }
}
