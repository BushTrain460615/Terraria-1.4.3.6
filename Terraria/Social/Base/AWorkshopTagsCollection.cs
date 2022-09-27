// Decompiled with JetBrains decompiler
// Type: Terraria.Social.Base.AWorkshopTagsCollection
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;

namespace Terraria.Social.Base
{
  public abstract class AWorkshopTagsCollection
  {
    public readonly List<WorkshopTagOption> WorldTags = new List<WorkshopTagOption>();
    public readonly List<WorkshopTagOption> ResourcePackTags = new List<WorkshopTagOption>();

    protected void AddWorldTag(string tagNameKey, string tagInternalName) => this.WorldTags.Add(new WorkshopTagOption(tagNameKey, tagInternalName));

    protected void AddResourcePackTag(string tagNameKey, string tagInternalName) => this.ResourcePackTags.Add(new WorkshopTagOption(tagNameKey, tagInternalName));
  }
}
