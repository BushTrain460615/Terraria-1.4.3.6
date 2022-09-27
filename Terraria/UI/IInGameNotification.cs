// Decompiled with JetBrains decompiler
// Type: Terraria.UI.IInGameNotification
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.UI
{
  public interface IInGameNotification
  {
    object CreationObject { get; }

    bool ShouldBeRemoved { get; }

    void Update();

    void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition);

    void PushAnchor(ref Vector2 positionAnchorBottom);

    void DrawInNotificationsArea(
      SpriteBatch spriteBatch,
      Rectangle area,
      ref int gamepadPointLocalIndexTouse);
  }
}
