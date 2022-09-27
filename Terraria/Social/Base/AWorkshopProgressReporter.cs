// Decompiled with JetBrains decompiler
// Type: Terraria.Social.Base.AWorkshopProgressReporter
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.Social.Base
{
  public abstract class AWorkshopProgressReporter
  {
    public abstract bool HasOngoingTasks { get; }

    public abstract bool TryGetProgress(out float progress);
  }
}
