// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.EntrySorter`2
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.Localization;

namespace Terraria.DataStructures
{
  public class EntrySorter<TEntryType, TStepType> : IComparer<TEntryType>
    where TEntryType : new()
    where TStepType : IEntrySortStep<TEntryType>
  {
    public List<TStepType> Steps = new List<TStepType>();
    private int _prioritizedStep;

    public void AddSortSteps(List<TStepType> sortSteps) => this.Steps.AddRange((IEnumerable<TStepType>) sortSteps);

    public int Compare(TEntryType x, TEntryType y)
    {
      int num = 0;
      if (this._prioritizedStep != -1)
      {
        num = this.Steps[this._prioritizedStep].Compare(x, y);
        if (num != 0)
          return num;
      }
      for (int index = 0; index < this.Steps.Count; ++index)
      {
        if (index != this._prioritizedStep)
        {
          num = this.Steps[index].Compare(x, y);
          if (num != 0)
            return num;
        }
      }
      return num;
    }

    public void SetPrioritizedStepIndex(int index) => this._prioritizedStep = index;

    public string GetDisplayName() => Language.GetTextValue(this.Steps[this._prioritizedStep].GetDisplayNameKey());
  }
}
