// Decompiled with JetBrains decompiler
// Type: Terraria.Utilities.FileBrowser.ExtensionFilter
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.Utilities.FileBrowser
{
  public struct ExtensionFilter
  {
    public string Name;
    public string[] Extensions;

    public ExtensionFilter(string filterName, params string[] filterExtensions)
    {
      this.Name = filterName;
      this.Extensions = filterExtensions;
    }
  }
}
