﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.CustomEntryIcon
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria.Localization;

namespace Terraria.GameContent.Bestiary
{
  public class CustomEntryIcon : IEntryIcon
  {
    private LocalizedText _text;
    private Asset<Texture2D> _textureAsset;
    private Rectangle _sourceRectangle;
    private Func<bool> _unlockCondition;

    public CustomEntryIcon(string nameLanguageKey, string texturePath, Func<bool> unlockCondition)
    {
      this._text = Language.GetText(nameLanguageKey);
      this._textureAsset = Main.Assets.Request<Texture2D>(texturePath, (AssetRequestMode) 1);
      this._unlockCondition = unlockCondition;
      this.UpdateUnlockState(false);
    }

    public IEntryIcon CreateClone() => (IEntryIcon) new CustomEntryIcon(this._text.Key, this._textureAsset.Name, this._unlockCondition);

    public void Update(
      BestiaryUICollectionInfo providedInfo,
      Rectangle hitbox,
      EntryIconDrawSettings settings)
    {
      this.UpdateUnlockState(this.GetUnlockState(providedInfo));
    }

    public void Draw(
      BestiaryUICollectionInfo providedInfo,
      SpriteBatch spriteBatch,
      EntryIconDrawSettings settings)
    {
      Rectangle iconbox = settings.iconbox;
      spriteBatch.Draw(this._textureAsset.Value, iconbox.Center.ToVector2() + Vector2.One, new Rectangle?(this._sourceRectangle), Color.White, 0.0f, this._sourceRectangle.Size() / 2f, 1f, SpriteEffects.None, 0.0f);
    }

    public string GetHoverText(BestiaryUICollectionInfo providedInfo) => this.GetUnlockState(providedInfo) ? this._text.Value : "???";

    private void UpdateUnlockState(bool state)
    {
      this._sourceRectangle = this._textureAsset.Frame(2, frameX: state.ToInt());
      this._sourceRectangle.Inflate(-2, -2);
    }

    public bool GetUnlockState(BestiaryUICollectionInfo providedInfo) => providedInfo.UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
  }
}
