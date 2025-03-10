﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Social.WeGame.IPCClient
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Terraria.Social.WeGame
{
  public class IPCClient : IPCBase
  {
    private bool _connectedFlag;

    public event Action OnConnected;

    public override event Action<byte[]> OnDataArrive
    {
      add => this._onDataArrive = this._onDataArrive + value;
      remove => this._onDataArrive = this._onDataArrive - value;
    }

    private NamedPipeClientStream GetPipeStream() => (NamedPipeClientStream) this._pipeStream;

    private void ProcessConnectedEvent()
    {
      if (!this._connectedFlag)
        return;
      if (this.OnConnected != null)
        this.OnConnected();
      this._connectedFlag = false;
    }

    private void ProcessPipeBrokenEvent()
    {
      if (!this._pipeBrokenFlag)
        return;
      this.Reset();
      this._pipeBrokenFlag = false;
    }

    private void CheckFlagAndFireEvent()
    {
      this.ProcessConnectedEvent();
      this.ProcessDataArriveEvent();
      this.ProcessPipeBrokenEvent();
    }

    public void Init(string clientName)
    {
    }

    public void ConnectTo(string serverName)
    {
      if (this.GetPipeStream() != null)
        return;
      this._pipeStream = (PipeStream) new NamedPipeClientStream(".", serverName, PipeDirection.InOut, PipeOptions.Asynchronous);
      this._cancelTokenSrc = new CancellationTokenSource();
      Task.Factory.StartNew((Action<object>) (content =>
      {
        this.GetPipeStream().Connect();
        if (((CancellationToken) content).IsCancellationRequested)
          return;
        this.GetPipeStream().ReadMode = PipeTransmissionMode.Message;
        this.BeginReadData();
        this._connectedFlag = true;
      }), (object) this._cancelTokenSrc.Token);
    }

    public void Tick() => this.CheckFlagAndFireEvent();

    public override void ReadCallback(IAsyncResult result)
    {
      IPCContent asyncState = (IPCContent) result.AsyncState;
      base.ReadCallback(result);
      if (!asyncState.CancelToken.IsCancellationRequested)
      {
        if (!this.GetPipeStream().IsConnected)
          return;
        this.BeginReadData();
      }
      else
        WeGameHelper.WriteDebugString("ReadCallback cancel");
    }
  }
}
