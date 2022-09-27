// Decompiled with JetBrains decompiler
// Type: Terraria.Audio.CustomSoundStyle
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;

namespace Terraria.Audio
{
  public class CustomSoundStyle : SoundStyle
  {
    private static readonly UnifiedRandom Random = new UnifiedRandom();
    private readonly SoundEffect[] _soundEffects;

    public override bool IsTrackable => true;

    public CustomSoundStyle(
      SoundEffect soundEffect,
      SoundType type = SoundType.Sound,
      float volume = 1f,
      float pitchVariance = 0.0f)
      : base(volume, pitchVariance, type)
    {
      this._soundEffects = new SoundEffect[1]{ soundEffect };
    }

    public CustomSoundStyle(
      SoundEffect[] soundEffects,
      SoundType type = SoundType.Sound,
      float volume = 1f,
      float pitchVariance = 0.0f)
      : base(volume, pitchVariance, type)
    {
      this._soundEffects = soundEffects;
    }

    public override SoundEffect GetRandomSound() => this._soundEffects[CustomSoundStyle.Random.Next(this._soundEffects.Length)];
  }
}
