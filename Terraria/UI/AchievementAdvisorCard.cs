// Decompiled with JetBrains decompiler
// Type: Terraria.UI.AchievementAdvisorCard
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.Achievements;

namespace Terraria.UI
{
  public class AchievementAdvisorCard
  {
    private const int _iconSize = 64;
    private const int _iconSizeWithSpace = 66;
    private const int _iconsPerRow = 8;
    public Achievement achievement;
    public float order;
    public Rectangle frame;
    public int achievementIndex;

    public AchievementAdvisorCard(Achievement achievement, float order)
    {
      this.achievement = achievement;
      this.order = order;
      this.achievementIndex = Main.Achievements.GetIconIndex(achievement.Name);
      this.frame = new Rectangle(this.achievementIndex % 8 * 66, this.achievementIndex / 8 * 66, 64, 64);
    }

    public bool IsAchievableInWorld()
    {
      string name = this.achievement.Name;
      if (name == "MASTERMIND")
        return WorldGen.crimson;
      return !(name == "WORM_FODDER") || !WorldGen.crimson;
    }
  }
}
