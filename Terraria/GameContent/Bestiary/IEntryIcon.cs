// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.IEntryIcon
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.Bestiary
{
  public interface IEntryIcon
  {
    void Update(
      BestiaryUICollectionInfo providedInfo,
      Rectangle hitbox,
      EntryIconDrawSettings settings);

    void Draw(
      BestiaryUICollectionInfo providedInfo,
      SpriteBatch spriteBatch,
      EntryIconDrawSettings settings);

    bool GetUnlockState(BestiaryUICollectionInfo providedInfo);

    string GetHoverText(BestiaryUICollectionInfo providedInfo);

    IEntryIcon CreateClone();
  }
}
