// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.RejectionMenuInfo
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.Audio;

namespace Terraria.DataStructures
{
  public class RejectionMenuInfo
  {
    public ReturnFromRejectionMenuAction ExitAction;
    public string TextToShow;

    public void DefaultExitAction()
    {
      SoundEngine.PlaySound(11);
      Main.menuMode = 0;
      Main.netMode = 0;
    }
  }
}
