// Decompiled with JetBrains decompiler
// Type: Terraria.Social.Base.WorkshopSocialModule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.IO;

namespace Terraria.Social.Base
{
  public abstract class WorkshopSocialModule : ISocialModule
  {
    public WorkshopBranding Branding { get; protected set; }

    public AWorkshopProgressReporter ProgressReporter { get; protected set; }

    public AWorkshopTagsCollection SupportedTags { get; protected set; }

    public WorkshopIssueReporter IssueReporter { get; protected set; }

    public abstract void Initialize();

    public abstract void Shutdown();

    public abstract void PublishWorld(WorldFileData world, WorkshopItemPublishSettings settings);

    public abstract void PublishResourcePack(
      ResourcePack resourcePack,
      WorkshopItemPublishSettings settings);

    public abstract bool TryGetInfoForWorld(WorldFileData world, out FoundWorkshopEntryInfo info);

    public abstract bool TryGetInfoForResourcePack(
      ResourcePack resourcePack,
      out FoundWorkshopEntryInfo info);

    public abstract void LoadEarlyContent();

    public abstract List<string> GetListOfSubscribedResourcePackPaths();

    public abstract List<string> GetListOfSubscribedWorldPaths();

    public abstract bool TryGetPath(string pathEnd, out string fullPathFound);

    public abstract void ImportDownloadedWorldToLocalSaves(
      WorldFileData world,
      string newFileName = null,
      string newDisplayName = null);
  }
}
