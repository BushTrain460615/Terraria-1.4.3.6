// Decompiled with JetBrains decompiler
// Type: Terraria.Chat.Commands.IChatCommand
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.Chat.Commands
{
  public interface IChatCommand
  {
    void ProcessIncomingMessage(string text, byte clientId);

    void ProcessOutgoingMessage(ChatMessage message);
  }
}
