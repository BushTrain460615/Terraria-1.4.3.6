// Decompiled with JetBrains decompiler
// Type: Terraria.Social.Base.WorkshopItemPublishSettings
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace Terraria.Social.Base
{
  public class WorkshopItemPublishSettings
  {
    public WorkshopTagOption[] UsedTags = new WorkshopTagOption[0];
    public WorkshopItemPublicSettingId Publicity;
    public string PreviewImagePath;

    public string[] GetUsedTagsInternalNames() => ((IEnumerable<WorkshopTagOption>) this.UsedTags).Select<WorkshopTagOption, string>((Func<WorkshopTagOption, string>) (x => x.InternalNameForAPIs)).ToArray<string>();
  }
}
