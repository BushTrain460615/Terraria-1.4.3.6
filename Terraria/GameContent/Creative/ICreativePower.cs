// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Creative.ICreativePower
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;
using System.IO;
using Terraria.UI;

namespace Terraria.GameContent.Creative
{
  public interface ICreativePower
  {
    ushort PowerId { get; set; }

    string ServerConfigName { get; set; }

    PowerPermissionLevel CurrentPermissionLevel { get; set; }

    PowerPermissionLevel DefaultPermissionLevel { get; set; }

    void DeserializeNetMessage(BinaryReader reader, int userId);

    void ProvidePowerButtons(CreativePowerUIElementRequestInfo info, List<UIElement> elements);

    bool GetIsUnlocked();
  }
}
