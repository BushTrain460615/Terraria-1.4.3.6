﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.BigProgressBar.TwinsBigProgressBar
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
  public class TwinsBigProgressBar : IBigProgressBar
  {
    private float _lifePercentToShow;
    private int _headIndex;

    public bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info)
    {
      if (info.npcIndexToAimAt < 0 || info.npcIndexToAimAt > 200)
        return false;
      NPC npc1 = Main.npc[info.npcIndexToAimAt];
      if (!npc1.active)
        return false;
      int num = npc1.type == 126 ? 125 : 126;
      int lifeMax = npc1.lifeMax;
      int life = npc1.life;
      for (int index = 0; index < 200; ++index)
      {
        NPC npc2 = Main.npc[index];
        if (npc2.active && npc2.type == num)
        {
          lifeMax += npc2.lifeMax;
          life += npc2.life;
          break;
        }
      }
      this._lifePercentToShow = Utils.Clamp<float>((float) life / (float) lifeMax, 0.0f, 1f);
      this._headIndex = npc1.GetBossHeadTextureIndex();
      return true;
    }

    public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
    {
      Texture2D texture2D = TextureAssets.NpcHeadBoss[this._headIndex].Value;
      Rectangle barIconFrame = texture2D.Frame();
      BigProgressBarHelper.DrawFancyBar(spriteBatch, this._lifePercentToShow, texture2D, barIconFrame);
    }
  }
}
