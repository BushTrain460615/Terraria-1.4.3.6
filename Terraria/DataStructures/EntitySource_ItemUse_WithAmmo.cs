// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.EntitySource_ItemUse_WithAmmo
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.DataStructures
{
  public class EntitySource_ItemUse_WithAmmo : EntitySource_ItemUse
  {
    public readonly int AmmoItemIdUsed;

    public EntitySource_ItemUse_WithAmmo(Entity entity, Item item, int ammoItemIdUsed)
      : base(entity, item)
    {
      this.AmmoItemIdUsed = ammoItemIdUsed;
    }
  }
}
