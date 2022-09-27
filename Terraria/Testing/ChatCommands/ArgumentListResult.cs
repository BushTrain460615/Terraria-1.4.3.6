// Decompiled with JetBrains decompiler
// Type: Terraria.Testing.ChatCommands.ArgumentListResult
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Terraria.Testing.ChatCommands
{
  public class ArgumentListResult : IEnumerable<string>, IEnumerable
  {
    public static readonly ArgumentListResult Empty = new ArgumentListResult(true);
    public static readonly ArgumentListResult Invalid = new ArgumentListResult(false);
    public readonly bool IsValid;
    private readonly List<string> _results;

    public int Count => this._results.Count;

    public string this[int index] => this._results[index];

    public ArgumentListResult(IEnumerable<string> results)
    {
      this._results = results.ToList<string>();
      this.IsValid = true;
    }

    private ArgumentListResult(bool isValid)
    {
      this._results = new List<string>();
      this.IsValid = isValid;
    }

    public IEnumerator<string> GetEnumerator() => (IEnumerator<string>) this._results.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
