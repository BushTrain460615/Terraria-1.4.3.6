﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Chat.AchievementTagHandler
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.Achievements;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
  public class AchievementTagHandler : ITagHandler
  {
    TextSnippet ITagHandler.Parse(
      string text,
      Color baseColor,
      string options)
    {
      Achievement achievement = Main.Achievements.GetAchievement(text);
      return achievement == null ? new TextSnippet(text) : (TextSnippet) new AchievementTagHandler.AchievementSnippet(achievement);
    }

    public static string GenerateTag(Achievement achievement) => "[a:" + achievement.Name + "]";

    private class AchievementSnippet : TextSnippet
    {
      private Achievement _achievement;

      public AchievementSnippet(Achievement achievement)
        : base(achievement.FriendlyName.Value, Color.LightBlue)
      {
        this.CheckForHover = true;
        this._achievement = achievement;
      }

      public override void OnClick()
      {
        IngameOptions.Close();
        IngameFancyUI.OpenAchievementsAndGoto(this._achievement);
      }
    }
  }
}
