// Decompiled with JetBrains decompiler
// Type: Terraria.Social.WeGame.CurrentThreadRunner
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;
using System.Windows.Threading;

namespace Terraria.Social.WeGame
{
  public class CurrentThreadRunner
  {
    private Dispatcher _dsipatcher;

    public CurrentThreadRunner() => this._dsipatcher = Dispatcher.CurrentDispatcher;

    public void Run(Action f) => this._dsipatcher.BeginInvoke((Delegate) f);
  }
}
