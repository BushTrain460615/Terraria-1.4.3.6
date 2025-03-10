﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.EmoteButton
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
  public class EmoteButton : UIElement
  {
    private Asset<Texture2D> _texture;
    private Asset<Texture2D> _textureBorder;
    private int _emoteIndex;
    private bool _hovered;
    private int _frameCounter;

    public EmoteButton(int emoteIndex)
    {
      this._texture = Main.Assets.Request<Texture2D>("Images/Extra_" + (short) 48.ToString(), (AssetRequestMode) 1);
      this._textureBorder = Main.Assets.Request<Texture2D>("Images/UI/EmoteBubbleBorder", (AssetRequestMode) 1);
      this._emoteIndex = emoteIndex;
      Rectangle frame = this.GetFrame();
      this.Width.Set((float) frame.Width, 0.0f);
      this.Height.Set((float) frame.Height, 0.0f);
    }

    private Rectangle GetFrame() => this._texture.Frame(8, 39, this._emoteIndex % 4 * 2 + (this._frameCounter >= 10 ? 1 : 0), this._emoteIndex / 4 + 1);

    private void UpdateFrame()
    {
      if (++this._frameCounter < 20)
        return;
      this._frameCounter = 0;
    }

    public override void Update(GameTime gameTime)
    {
      this.UpdateFrame();
      base.Update(gameTime);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
      CalculatedStyle dimensions = this.GetDimensions();
      Vector2 position = dimensions.Position() + new Vector2(dimensions.Width, dimensions.Height) / 2f;
      Rectangle frame = this.GetFrame();
      Rectangle rectangle = frame with
      {
        X = this._texture.Width() / 8,
        Y = 0
      };
      Vector2 origin = frame.Size() / 2f;
      Color white = Color.White;
      Color color = Color.Black;
      if (this._hovered)
        color = Main.OurFavoriteColor;
      spriteBatch.Draw(this._texture.Value, position, new Rectangle?(rectangle), white, 0.0f, origin, 1f, SpriteEffects.None, 0.0f);
      spriteBatch.Draw(this._texture.Value, position, new Rectangle?(frame), white, 0.0f, origin, 1f, SpriteEffects.None, 0.0f);
      spriteBatch.Draw(this._textureBorder.Value, position - Vector2.One * 2f, new Rectangle?(), color, 0.0f, origin, 1f, SpriteEffects.None, 0.0f);
      if (!this._hovered)
        return;
      string cursorText = "/" + Language.GetTextValue("EmojiName." + EmoteID.Search.GetName(this._emoteIndex));
      Main.instance.MouseText(cursorText);
    }

    public override void MouseOver(UIMouseEvent evt)
    {
      base.MouseOver(evt);
      SoundEngine.PlaySound(12);
      this._hovered = true;
    }

    public override void MouseOut(UIMouseEvent evt)
    {
      base.MouseOut(evt);
      this._hovered = false;
    }

    public override void Click(UIMouseEvent evt)
    {
      base.Click(evt);
      EmoteBubble.MakeLocalPlayerEmote(this._emoteIndex);
      IngameFancyUI.Close();
    }
  }
}
