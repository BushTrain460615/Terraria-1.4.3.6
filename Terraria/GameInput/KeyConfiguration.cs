﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameInput.KeyConfiguration
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;
using System.Linq;

namespace Terraria.GameInput
{
  public class KeyConfiguration
  {
    public Dictionary<string, List<string>> KeyStatus = new Dictionary<string, List<string>>();

    public bool DoGrappleAndInteractShareTheSameKey => this.KeyStatus["Grapple"].Count > 0 && this.KeyStatus["MouseRight"].Count > 0 && this.KeyStatus["MouseRight"].Contains(this.KeyStatus["Grapple"][0]);

    public void SetupKeys()
    {
      this.KeyStatus.Clear();
      foreach (string knownTrigger in PlayerInput.KnownTriggers)
        this.KeyStatus.Add(knownTrigger, new List<string>());
    }

    public void Processkey(TriggersSet set, string newKey, InputMode mode)
    {
      foreach (KeyValuePair<string, List<string>> keyStatu in this.KeyStatus)
      {
        if (keyStatu.Value.Contains(newKey))
        {
          set.KeyStatus[keyStatu.Key] = true;
          set.LatestInputMode[keyStatu.Key] = mode;
        }
      }
      if (!set.Up && !set.Down && !set.Left && !set.Right && !set.HotbarPlus && !set.HotbarMinus && (!Main.gameMenu && !Main.ingameOptionsWindow || !set.MenuUp && !set.MenuDown && !set.MenuLeft && !set.MenuRight))
        return;
      set.UsedMovementKey = true;
    }

    public void CopyKeyState(TriggersSet oldSet, TriggersSet newSet, string newKey)
    {
      foreach (KeyValuePair<string, List<string>> keyStatu in this.KeyStatus)
      {
        if (keyStatu.Value.Contains(newKey))
          newSet.KeyStatus[keyStatu.Key] = oldSet.KeyStatus[keyStatu.Key];
      }
    }

    public void ReadPreferences(Dictionary<string, List<string>> dict)
    {
      foreach (KeyValuePair<string, List<string>> keyValuePair in dict)
      {
        if (this.KeyStatus.ContainsKey(keyValuePair.Key))
        {
          this.KeyStatus[keyValuePair.Key].Clear();
          foreach (string str in keyValuePair.Value)
            this.KeyStatus[keyValuePair.Key].Add(str);
        }
      }
    }

    public Dictionary<string, List<string>> WritePreferences()
    {
      Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
      foreach (KeyValuePair<string, List<string>> keyStatu in this.KeyStatus)
      {
        if (keyStatu.Value.Count > 0)
          dictionary.Add(keyStatu.Key, keyStatu.Value.ToList<string>());
      }
      if (!dictionary.ContainsKey("MouseLeft") || dictionary["MouseLeft"].Count == 0)
        dictionary["MouseLeft"] = new List<string>()
        {
          "Mouse1"
        };
      if (!dictionary.ContainsKey("Inventory") || dictionary["Inventory"].Count == 0)
        dictionary["Inventory"] = new List<string>()
        {
          "Escape"
        };
      return dictionary;
    }
  }
}
