// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.BigProgressBar.NeverValidProgressBar
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
  public class NeverValidProgressBar : IBigProgressBar
  {
    public bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info) => false;

    public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
    {
    }
  }
}
