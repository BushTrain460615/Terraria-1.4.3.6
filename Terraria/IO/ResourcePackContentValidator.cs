// Decompiled with JetBrains decompiler
// Type: Terraria.IO.ResourcePackContentValidator
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using ReLogic.Content;
using System.IO;
using Terraria.GameContent;

namespace Terraria.IO
{
  public class ResourcePackContentValidator
  {
    public void ValidateResourePack(ResourcePack pack)
    {
      if ((AssetReaderCollection) Main.instance.Services.GetService(typeof (AssetReaderCollection)) == null)
        return;
      pack.GetContentSource().GetAllAssetsStartingWith("Images" + Path.DirectorySeparatorChar.ToString());
      VanillaContentValidator.Instance.GetValidImageFilePaths();
    }
  }
}
