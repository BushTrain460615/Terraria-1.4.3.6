// Decompiled with JetBrains decompiler
// Type: Terraria.Chat.ChatCommandId
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using ReLogic.Utilities;
using System.IO;
using System.Text;
using Terraria.Chat.Commands;

namespace Terraria.Chat
{
  public struct ChatCommandId
  {
    private readonly string _name;

    private ChatCommandId(string name) => this._name = name;

    public static ChatCommandId FromType<T>() where T : IChatCommand
    {
      ChatCommandAttribute cacheableAttribute = AttributeUtilities.GetCacheableAttribute<T, ChatCommandAttribute>();
      return cacheableAttribute != null ? new ChatCommandId(cacheableAttribute.Name) : new ChatCommandId((string) null);
    }

    public void Serialize(BinaryWriter writer) => writer.Write(this._name ?? "");

    public static ChatCommandId Deserialize(BinaryReader reader) => new ChatCommandId(reader.ReadString());

    public int GetMaxSerializedSize() => 4 + Encoding.UTF8.GetByteCount(this._name ?? "");
  }
}
