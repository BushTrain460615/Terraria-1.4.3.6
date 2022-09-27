// Decompiled with JetBrains decompiler
// Type: Terraria.Utilities.FileBrowser.FileBrowser
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.Utilities.FileBrowser
{
  public class FileBrowser
  {
    private static IFileBrowser _platformWrapper = (IFileBrowser) new NativeFileDialog();

    public static string OpenFilePanel(string title, string extension)
    {
      ExtensionFilter[] extensionFilterArray;
      if (!string.IsNullOrEmpty(extension))
        extensionFilterArray = new ExtensionFilter[1]
        {
          new ExtensionFilter("", new string[1]{ extension })
        };
      else
        extensionFilterArray = (ExtensionFilter[]) null;
      ExtensionFilter[] extensions = extensionFilterArray;
      return Terraria.Utilities.FileBrowser.FileBrowser.OpenFilePanel(title, extensions);
    }

    public static string OpenFilePanel(string title, ExtensionFilter[] extensions) => Terraria.Utilities.FileBrowser.FileBrowser._platformWrapper.OpenFilePanel(title, extensions);
  }
}
