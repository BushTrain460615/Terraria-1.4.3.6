// Decompiled with JetBrains decompiler
// Type: Terraria.Chat.Commands.RockPaperScissorsCommand
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.GameContent.UI;

namespace Terraria.Chat.Commands
{
  [ChatCommand("RPS")]
  public class RockPaperScissorsCommand : IChatCommand
  {
    public void ProcessIncomingMessage(string text, byte clientId)
    {
    }

    public void ProcessOutgoingMessage(ChatMessage message)
    {
      int num = Main.rand.NextFromList<int>(37, 38, 36);
      if (Main.netMode == 0)
      {
        EmoteBubble.NewBubble(num, new WorldUIAnchor((Entity) Main.LocalPlayer), 360);
        EmoteBubble.CheckForNPCsToReactToEmoteBubble(num, Main.LocalPlayer);
      }
      else
        NetMessage.SendData(120, number: Main.myPlayer, number2: ((float) num));
      message.Consume();
    }
  }
}
