﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.NetModules.NetTextModule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System.IO;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.Net;
using Terraria.UI.Chat;

namespace Terraria.GameContent.NetModules
{
  public class NetTextModule : NetModule
  {
    public static NetPacket SerializeClientMessage(ChatMessage message)
    {
      NetPacket packet = NetModule.CreatePacket<NetTextModule>(message.GetMaxSerializedSize());
      message.Serialize(packet.Writer);
      return packet;
    }

    public static NetPacket SerializeServerMessage(NetworkText text, Color color) => NetTextModule.SerializeServerMessage(text, color, byte.MaxValue);

    public static NetPacket SerializeServerMessage(
      NetworkText text,
      Color color,
      byte authorId)
    {
      NetPacket packet = NetModule.CreatePacket<NetTextModule>(1 + text.GetMaxSerializedSize() + 3);
      packet.Writer.Write(authorId);
      text.Serialize(packet.Writer);
      packet.Writer.WriteRGB(color);
      return packet;
    }

    private bool DeserializeAsClient(BinaryReader reader, int senderPlayerId)
    {
      byte messageAuthor = reader.ReadByte();
      ChatHelper.DisplayMessage(NetworkText.Deserialize(reader), reader.ReadRGB(), messageAuthor);
      return true;
    }

    private bool DeserializeAsServer(BinaryReader reader, int senderPlayerId)
    {
      ChatMessage message = ChatMessage.Deserialize(reader);
      ChatManager.Commands.ProcessIncomingMessage(message, senderPlayerId);
      return true;
    }

    public override bool Deserialize(BinaryReader reader, int senderPlayerId) => this.DeserializeAsClient(reader, senderPlayerId);
  }
}
