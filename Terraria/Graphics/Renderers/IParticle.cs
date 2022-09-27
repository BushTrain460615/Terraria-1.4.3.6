// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.Renderers.IParticle
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Renderers
{
  public interface IParticle
  {
    bool ShouldBeRemovedFromRenderer { get; }

    void Update(ref ParticleRendererSettings settings);

    void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch);
  }
}
