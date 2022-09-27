// Decompiled with JetBrains decompiler
// Type: Terraria.Social.Base.WorldWorkshopEntry
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.IO;

namespace Terraria.Social.Base
{
  public class WorldWorkshopEntry : AWorkshopEntry
  {
    public static string GetHeaderTextFor(
      WorldFileData world,
      ulong workshopEntryId,
      string[] tags,
      WorkshopItemPublicSettingId publicity,
      string previewImagePath)
    {
      return AWorkshopEntry.CreateHeaderJson("World", workshopEntryId, tags, publicity, previewImagePath);
    }
  }
}
