// Decompiled with JetBrains decompiler
// Type: Terraria.Social.Base.UserJoinToServerRequest
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;

namespace Terraria.Social.Base
{
  public abstract class UserJoinToServerRequest
  {
    internal string UserDisplayName { get; private set; }

    internal string UserFullIdentifier { get; private set; }

    public event Action OnAccepted;

    public event Action OnRejected;

    public UserJoinToServerRequest(string userDisplayName, string fullIdentifier)
    {
      this.UserDisplayName = userDisplayName;
      this.UserFullIdentifier = fullIdentifier;
    }

    public void Accept()
    {
      if (this.OnAccepted == null)
        return;
      this.OnAccepted();
    }

    public void Reject()
    {
      if (this.OnRejected == null)
        return;
      this.OnRejected();
    }

    public abstract bool IsValid();

    public abstract string GetUserWrapperText();
  }
}
