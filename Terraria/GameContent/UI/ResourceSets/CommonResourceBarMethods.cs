// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.ResourceSets.CommonResourceBarMethods
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.GameContent.UI.ResourceSets
{
  public class CommonResourceBarMethods
  {
    public static void DrawLifeMouseOver()
    {
      if (Main.mouseText)
        return;
      Player localPlayer = Main.LocalPlayer;
      localPlayer.cursorItemIconEnabled = false;
      string text = localPlayer.statLife.ToString() + "/" + localPlayer.statLifeMax2.ToString();
      Main.instance.MouseTextHackZoom(text);
      Main.mouseText = true;
    }

    public static void DrawManaMouseOver()
    {
      if (Main.mouseText)
        return;
      Player localPlayer = Main.LocalPlayer;
      localPlayer.cursorItemIconEnabled = false;
      string text = localPlayer.statMana.ToString() + "/" + localPlayer.statManaMax2.ToString();
      Main.instance.MouseTextHackZoom(text);
      Main.mouseText = true;
    }
  }
}
