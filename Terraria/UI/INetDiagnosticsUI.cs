// Decompiled with JetBrains decompiler
// Type: Terraria.UI.INetDiagnosticsUI
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Graphics;

namespace Terraria.UI
{
  public interface INetDiagnosticsUI
  {
    void Reset();

    void Draw(SpriteBatch spriteBatch);

    void CountReadMessage(int messageId, int messageLength);

    void CountSentMessage(int messageId, int messageLength);

    void CountReadModuleMessage(int moduleMessageId, int messageLength);

    void CountSentModuleMessage(int moduleMessageId, int messageLength);
  }
}
