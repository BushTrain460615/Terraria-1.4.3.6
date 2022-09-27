// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.ResourceSets.PlayerResourceSetsManager2
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.IO;

namespace Terraria.GameContent.UI.ResourceSets
{
  public class PlayerResourceSetsManager2 : SelectionHolder<IPlayerResourcesDisplaySet>
  {
    protected override void Configuration_Save(Preferences obj) => obj.Put("PlayerResourcesSet", (object) this.ActiveSelectionConfigKey);

    protected override void Configuration_OnLoad(Preferences obj) => this.ActiveSelectionConfigKey = Main.Configuration.Get<string>("PlayerResourcesSet", "New");

    protected override void PopulateOptionsAndLoadContent(AssetRequestMode mode)
    {
      this.Options["New"] = (IPlayerResourcesDisplaySet) new FancyClassicPlayerResourcesDisplaySet("New", "New", "FancyClassic", mode);
      this.Options["Default"] = (IPlayerResourcesDisplaySet) new ClassicPlayerResourcesDisplaySet("Default", "Default");
      this.Options["HorizontalBars"] = (IPlayerResourcesDisplaySet) new HorizontalBarsPlayerReosurcesDisplaySet("HorizontalBars", "HorizontalBars", "HorizontalBars", mode);
    }

    public void TryToHoverOverResources() => this.ActiveSelection.TryToHover();

    public void Draw() => this.ActiveSelection.Draw();
  }
}
