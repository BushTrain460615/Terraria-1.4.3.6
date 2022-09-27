// Decompiled with JetBrains decompiler
// Type: Terraria.Chat.Commands.EmoteCommand
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
  [ChatCommand("Emote")]
  public class EmoteCommand : IChatCommand
  {
    private static readonly Color RESPONSE_COLOR = new Color(200, 100, 0);

    public void ProcessIncomingMessage(string text, byte clientId)
    {
      if (!(text != ""))
        return;
      text = string.Format("*{0} {1}", (object) Main.player[(int) clientId].name, (object) text);
      ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), EmoteCommand.RESPONSE_COLOR);
    }

    public void ProcessOutgoingMessage(ChatMessage message)
    {
    }
  }
}
