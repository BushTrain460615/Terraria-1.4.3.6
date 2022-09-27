// Decompiled with JetBrains decompiler
// Type: Terraria.Map.IMapLayer
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.Map
{
  public interface IMapLayer
  {
    void Draw(ref MapOverlayDrawContext context, ref string text);
  }
}
