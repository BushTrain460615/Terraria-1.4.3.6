// Decompiled with JetBrains decompiler
// Type: Terraria.Achievements.AchievementCondition
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Terraria.Achievements
{
  [JsonObject]
  public abstract class AchievementCondition
  {
    public readonly string Name;
    protected IAchievementTracker _tracker;
    [JsonProperty("Completed")]
    private bool _isCompleted;

    public event AchievementCondition.AchievementUpdate OnComplete;

    public bool IsCompleted => this._isCompleted;

    protected AchievementCondition(string name) => this.Name = name;

    public virtual void Load(JObject state) => this._isCompleted = JToken.op_Explicit(state["Completed"]);

    public virtual void Clear() => this._isCompleted = false;

    public virtual void Complete()
    {
      if (this._isCompleted)
        return;
      this._isCompleted = true;
      if (this.OnComplete == null)
        return;
      this.OnComplete(this);
    }

    protected virtual IAchievementTracker CreateAchievementTracker() => (IAchievementTracker) null;

    public IAchievementTracker GetAchievementTracker()
    {
      if (this._tracker == null)
        this._tracker = this.CreateAchievementTracker();
      return this._tracker;
    }

    public delegate void AchievementUpdate(AchievementCondition condition);
  }
}
