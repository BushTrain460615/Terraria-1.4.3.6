// Decompiled with JetBrains decompiler
// Type: Terraria.Social.Base.AchievementsSocialModule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.Social.Base
{
  public abstract class AchievementsSocialModule : ISocialModule
  {
    public abstract void Initialize();

    public abstract void Shutdown();

    public abstract byte[] GetEncryptionKey();

    public abstract string GetSavePath();

    public abstract void UpdateIntStat(string name, int value);

    public abstract void UpdateFloatStat(string name, float value);

    public abstract void CompleteAchievement(string name);

    public abstract bool IsAchievementCompleted(string name);

    public abstract void StoreStats();
  }
}
