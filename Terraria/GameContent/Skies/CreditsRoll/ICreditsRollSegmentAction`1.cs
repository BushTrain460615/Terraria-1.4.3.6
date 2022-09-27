// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Skies.CreditsRoll.ICreditsRollSegmentAction`1
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.GameContent.Skies.CreditsRoll
{
  public interface ICreditsRollSegmentAction<T>
  {
    void BindTo(T obj);

    int ExpectedLengthOfActionInFrames { get; }

    void ApplyTo(T obj, float localTimeForObj);

    void SetDelay(float delay);
  }
}
