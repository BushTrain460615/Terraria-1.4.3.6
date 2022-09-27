// Decompiled with JetBrains decompiler
// Type: Terraria.Audio.SoundInstanceGarbageCollector
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Terraria.Audio
{
  public static class SoundInstanceGarbageCollector
  {
    private static readonly List<SoundEffectInstance> _activeSounds = new List<SoundEffectInstance>(128);

    public static void Track(SoundEffectInstance sound)
    {
    }

    public static void Update()
    {
      for (int index = 0; index < SoundInstanceGarbageCollector._activeSounds.Count; ++index)
      {
        if (SoundInstanceGarbageCollector._activeSounds[index] == null)
        {
          SoundInstanceGarbageCollector._activeSounds.RemoveAt(index);
          --index;
        }
        else if (SoundInstanceGarbageCollector._activeSounds[index].State == SoundState.Stopped)
        {
          SoundInstanceGarbageCollector._activeSounds[index].Dispose();
          SoundInstanceGarbageCollector._activeSounds.RemoveAt(index);
          --index;
        }
      }
    }
  }
}
