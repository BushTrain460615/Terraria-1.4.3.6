// Decompiled with JetBrains decompiler
// Type: Terraria.UI.SnapPoint
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Terraria.UI
{
  [DebuggerDisplay("Snap Point - {Name} {Id}")]
  public class SnapPoint
  {
    public string Name;
    private Vector2 _anchor;
    private Vector2 _offset;

    public int Id { get; private set; }

    public Vector2 Position { get; private set; }

    public SnapPoint(string name, int id, Vector2 anchor, Vector2 offset)
    {
      this.Name = name;
      this.Id = id;
      this._anchor = anchor;
      this._offset = offset;
    }

    public void Calculate(UIElement element)
    {
      CalculatedStyle dimensions = element.GetDimensions();
      this.Position = dimensions.Position() + this._offset + this._anchor * new Vector2(dimensions.Width, dimensions.Height);
    }

    public void ThisIsAHackThatChangesTheSnapPointsInfo(Vector2 anchor, Vector2 offset, int id)
    {
      this._anchor = anchor;
      this._offset = offset;
      this.Id = id;
    }
  }
}
