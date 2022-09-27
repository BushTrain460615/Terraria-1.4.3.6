// Decompiled with JetBrains decompiler
// Type: Terraria.Audio.IAudioTrack
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Audio;
using System;

namespace Terraria.Audio
{
  public interface IAudioTrack : IDisposable
  {
    bool IsPlaying { get; }

    bool IsStopped { get; }

    bool IsPaused { get; }

    void Stop(AudioStopOptions options);

    void Play();

    void Pause();

    void SetVariable(string variableName, float value);

    void Resume();

    void Reuse();

    void Update();
  }
}
