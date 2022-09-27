// Decompiled with JetBrains decompiler
// Type: Terraria.Achievements.ConditionFloatTracker
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.Social;

namespace Terraria.Achievements
{
  public class ConditionFloatTracker : AchievementTracker<float>
  {
    public ConditionFloatTracker(float maxValue)
      : base(TrackerType.Float)
    {
      this._maxValue = maxValue;
    }

    public ConditionFloatTracker()
      : base(TrackerType.Float)
    {
    }

    public override void ReportUpdate()
    {
      if (SocialAPI.Achievements == null || this._name == null)
        return;
      SocialAPI.Achievements.UpdateFloatStat(this._name, this._value);
    }

    protected override void Load()
    {
    }
  }
}
