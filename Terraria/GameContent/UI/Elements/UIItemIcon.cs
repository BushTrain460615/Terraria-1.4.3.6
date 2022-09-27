// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UIItemIcon
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
  public class UIItemIcon : UIElement
  {
    private Item _item;
    private bool _blackedOut;

    public UIItemIcon(Item item, bool blackedOut)
    {
      this._item = item;
      this.Width.Set(32f, 0.0f);
      this.Height.Set(32f, 0.0f);
      this._blackedOut = blackedOut;
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
      CalculatedStyle dimensions = this.GetDimensions();
      Main.DrawItemIcon(spriteBatch, this._item, dimensions.Center(), this._blackedOut ? Color.Black : Color.White, 32f);
    }
  }
}
