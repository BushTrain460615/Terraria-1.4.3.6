﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Audio.CueAudioTrack
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Audio;
using System;

namespace Terraria.Audio
{
  public class CueAudioTrack : IAudioTrack, IDisposable
  {
    private Cue _cue;
    private string _cueName;
    private SoundBank _soundBank;

    public CueAudioTrack(SoundBank bank, string cueName)
    {
      this._soundBank = bank;
      this._cueName = cueName;
      this.Reuse();
    }

    public bool IsPlaying => this._cue.IsPlaying;

    public bool IsStopped => this._cue.IsStopped;

    public bool IsPaused => this._cue.IsPaused;

    public void Stop(AudioStopOptions options) => this._cue.Stop(options);

    public void Play() => this._cue.Play();

    public void SetVariable(string variableName, float value) => this._cue.SetVariable(variableName, value);

    public void Resume() => this._cue.Resume();

    public void Reuse()
    {
      if (this._cue != null)
        this.Stop(AudioStopOptions.Immediate);
      this._cue = this._soundBank.GetCue(this._cueName);
    }

    public void Pause() => this._cue.Pause();

    public void Dispose()
    {
    }

    public void Update()
    {
    }
  }
}
