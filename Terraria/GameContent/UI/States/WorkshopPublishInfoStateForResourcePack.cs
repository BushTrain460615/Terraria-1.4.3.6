﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.States.WorkshopPublishInfoStateForResourcePack
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.IO;
using Terraria.Social;
using Terraria.Social.Base;
using Terraria.UI;

namespace Terraria.GameContent.UI.States
{
  public class WorkshopPublishInfoStateForResourcePack : AWorkshopPublishInfoState<ResourcePack>
  {
    public WorkshopPublishInfoStateForResourcePack(
      UIState stateToGoBackTo,
      ResourcePack resourcePack)
      : base(stateToGoBackTo, resourcePack)
    {
      this._instructionsTextKey = "Workshop.ResourcePackPublishDescription";
      this._publishedObjectNameDescriptorTexKey = "Workshop.ResourcePackName";
    }

    protected override string GetPublishedObjectDisplayName() => this._dataObject == null ? "null" : this._dataObject.Name;

    protected override void GoToPublishConfirmation()
    {
      if (SocialAPI.Workshop != null && this._dataObject != null)
        SocialAPI.Workshop.PublishResourcePack(this._dataObject, this.GetPublishSettings());
      Main.menuMode = 888;
      Main.MenuUI.SetState(this._previousUIState);
    }

    protected override List<WorkshopTagOption> GetTagsToShow() => SocialAPI.Workshop.SupportedTags.ResourcePackTags;

    protected override bool TryFindingTags(out FoundWorkshopEntryInfo info) => SocialAPI.Workshop.TryGetInfoForResourcePack(this._dataObject, out info);
  }
}
