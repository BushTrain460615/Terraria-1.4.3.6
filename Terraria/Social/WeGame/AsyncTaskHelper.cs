// Decompiled with JetBrains decompiler
// Type: Terraria.Social.WeGame.AsyncTaskHelper
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;
using System.Threading.Tasks;

namespace Terraria.Social.WeGame
{
  public class AsyncTaskHelper
  {
    private CurrentThreadRunner _currentThreadRunner;

    private AsyncTaskHelper() => this._currentThreadRunner = new CurrentThreadRunner();

    public void RunAsyncTaskAndReply(Action task, Action replay) => Task.Factory.StartNew((Action) (() =>
    {
      task();
      this._currentThreadRunner.Run(replay);
    }));

    public void RunAsyncTask(Action task) => Task.Factory.StartNew(task);
  }
}
