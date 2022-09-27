// Decompiled with JetBrains decompiler
// Type: Terraria.Map.SpawnMapLayer
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.Map
{
  public class SpawnMapLayer : IMapLayer
  {
    public void Draw(ref MapOverlayDrawContext context, ref string text)
    {
      Player localPlayer = Main.LocalPlayer;
      Vector2 position1 = new Vector2((float) localPlayer.SpawnX, (float) localPlayer.SpawnY);
      Vector2 position2 = new Vector2((float) Main.spawnTileX, (float) Main.spawnTileY);
      if (context.Draw(TextureAssets.SpawnPoint.Value, position2, Alignment.Bottom).IsMouseOver)
        text = Language.GetTextValue("UI.SpawnPoint");
      if (localPlayer.SpawnX == -1 || !context.Draw(TextureAssets.SpawnBed.Value, position1, Alignment.Bottom).IsMouseOver)
        return;
      text = Language.GetTextValue("UI.SpawnBed");
    }
  }
}
