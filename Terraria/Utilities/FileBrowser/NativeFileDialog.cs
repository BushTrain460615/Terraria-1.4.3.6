// Decompiled with JetBrains decompiler
// Type: Terraria.Utilities.FileBrowser.NativeFileDialog
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace Terraria.Utilities.FileBrowser
{
  public class NativeFileDialog : IFileBrowser
  {
    public string OpenFilePanel(string title, ExtensionFilter[] extensions)
    {
      string outPath;
      return nativefiledialog.NFD_OpenDialog(string.Join(",", ((IEnumerable<ExtensionFilter>) extensions).SelectMany<ExtensionFilter, string>((Func<ExtensionFilter, IEnumerable<string>>) (x => (IEnumerable<string>) x.Extensions)).ToArray<string>()), (string) null, out outPath) == nativefiledialog.nfdresult_t.NFD_OKAY ? outPath : (string) null;
    }
  }
}
