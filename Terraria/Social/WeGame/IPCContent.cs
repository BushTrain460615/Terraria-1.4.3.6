// Decompiled with JetBrains decompiler
// Type: Terraria.Social.WeGame.IPCContent
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Threading;

namespace Terraria.Social.WeGame
{
  public class IPCContent
  {
    public byte[] data;

    public CancellationToken CancelToken { get; set; }
  }
}
