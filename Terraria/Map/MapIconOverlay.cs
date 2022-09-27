// Decompiled with JetBrains decompiler
// Type: Terraria.Map.MapIconOverlay
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Terraria.Map
{
  public class MapIconOverlay
  {
    private readonly List<IMapLayer> _layers = new List<IMapLayer>();

    public MapIconOverlay AddLayer(IMapLayer layer)
    {
      this._layers.Add(layer);
      return this;
    }

    public void Draw(
      Vector2 mapPosition,
      Vector2 mapOffset,
      Rectangle? clippingRect,
      float mapScale,
      float drawScale,
      ref string text)
    {
      MapOverlayDrawContext context = new MapOverlayDrawContext(mapPosition, mapOffset, clippingRect, mapScale, drawScale);
      foreach (IMapLayer layer in this._layers)
        layer.Draw(ref context, ref text);
    }
  }
}
