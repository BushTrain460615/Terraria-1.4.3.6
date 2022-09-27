// Decompiled with JetBrains decompiler
// Type: Terraria.IO.WorldFileData
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using ReLogic.Utilities;
using System;
using System.Collections;
using System.IO;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.Utilities;

namespace Terraria.IO
{
  public class WorldFileData : FileData
  {
    private const ulong GUID_IN_WORLD_FILE_VERSION = 777389080577;
    public DateTime CreationTime;
    public int WorldSizeX;
    public int WorldSizeY;
    public ulong WorldGeneratorVersion;
    private string _seedText = "";
    private int _seed;
    public bool IsValid = true;
    public Guid UniqueId;
    public LocalizedText _worldSizeName;
    public int GameMode;
    public bool DrunkWorld;
    public bool NotTheBees;
    public bool ForTheWorthy;
    public bool Anniversary;
    public bool DontStarve;
    public bool HasCorruption = true;
    public bool IsHardMode;

    public string SeedText => this._seedText;

    public int Seed => this._seed;

    public string WorldSizeName => this._worldSizeName.Value;

    public bool HasCrimson
    {
      get => !this.HasCorruption;
      set => this.HasCorruption = !value;
    }

    public bool HasValidSeed => this.WorldGeneratorVersion > 0UL;

    public bool UseGuidAsMapName => this.WorldGeneratorVersion >= 777389080577UL;

    public string GetFullSeedText()
    {
      int num1 = 0;
      if (this.WorldSizeX == 4200 && this.WorldSizeY == 1200)
        num1 = 1;
      if (this.WorldSizeX == 6400 && this.WorldSizeY == 1800)
        num1 = 2;
      if (this.WorldSizeX == 8400 && this.WorldSizeY == 2400)
        num1 = 3;
      int num2 = 0;
      if (this.HasCorruption)
        num2 = 1;
      if (this.HasCrimson)
        num2 = 2;
      int num3 = this.GameMode + 1;
      return string.Format("{0}.{1}.{2}.{3}", (object) num1, (object) num3, (object) num2, (object) this._seedText);
    }

    public WorldFileData()
      : base("World")
    {
    }

    public WorldFileData(string path, bool cloudSave)
      : base("World", path, cloudSave)
    {
    }

    public override void SetAsActive() => Main.ActiveWorldFileData = this;

    public void SetWorldSize(int x, int y)
    {
      this.WorldSizeX = x;
      this.WorldSizeY = y;
      switch (x)
      {
        case 4200:
          this._worldSizeName = Language.GetText("UI.WorldSizeSmall");
          break;
        case 6400:
          this._worldSizeName = Language.GetText("UI.WorldSizeMedium");
          break;
        case 8400:
          this._worldSizeName = Language.GetText("UI.WorldSizeLarge");
          break;
        default:
          this._worldSizeName = Language.GetText("UI.WorldSizeUnknown");
          break;
      }
    }

    public static WorldFileData FromInvalidWorld(string path, bool cloudSave)
    {
      WorldFileData worldFileData = new WorldFileData(path, cloudSave);
      worldFileData.GameMode = 0;
      worldFileData.SetSeedToEmpty();
      worldFileData.WorldGeneratorVersion = 0UL;
      worldFileData.Metadata = FileMetadata.FromCurrentSettings(FileType.World);
      worldFileData.SetWorldSize(1, 1);
      worldFileData.HasCorruption = true;
      worldFileData.IsHardMode = false;
      worldFileData.IsValid = false;
      worldFileData.Name = FileUtilities.GetFileName(path, false);
      worldFileData.UniqueId = Guid.Empty;
      worldFileData.CreationTime = cloudSave ? DateTime.Now : File.GetCreationTime(path);
      return worldFileData;
    }

    public void SetSeedToEmpty() => this.SetSeed("");

    public void SetSeed(string seedText)
    {
      this._seedText = seedText;
      WorldGen.currentWorldSeed = seedText;
      if (!int.TryParse(seedText, out this._seed))
        this._seed = Crc32.Calculate(seedText);
      this._seed = this._seed == int.MinValue ? int.MaxValue : Math.Abs(this._seed);
    }

    public void SetSeedToRandom() => this.SetSeed(new UnifiedRandom().Next().ToString());

    public override void MoveToCloud()
    {
      if (this.IsCloudSave)
        return;
      string worldPathFromName = Main.GetWorldPathFromName(this.Name, true);
      if (!FileUtilities.MoveToCloud(this.Path, worldPathFromName))
        return;
      Main.LocalFavoriteData.ClearEntry((FileData) this);
      this._isCloudSave = true;
      this._path = worldPathFromName;
      Main.CloudFavoritesData.SaveFavorite((FileData) this);
    }

    public override void MoveToLocal()
    {
      if (!this.IsCloudSave)
        return;
      string worldPathFromName = Main.GetWorldPathFromName(this.Name, false);
      if (!FileUtilities.MoveToLocal(this.Path, worldPathFromName))
        return;
      Main.CloudFavoritesData.ClearEntry((FileData) this);
      this._isCloudSave = false;
      this._path = worldPathFromName;
      Main.LocalFavoriteData.SaveFavorite((FileData) this);
    }

    public void Rename(string newDisplayName)
    {
      if (newDisplayName == null)
        return;
      WorldGen.RenameWorld(this, newDisplayName, new Action<string>(this.OnWorldRenameSuccess));
    }

    public void CopyToLocal(string newFileName = null, string newDisplayName = null)
    {
      if (this.IsCloudSave)
        return;
      if (newFileName == null)
        newFileName = Guid.NewGuid().ToString();
      string worldPathFromName = Main.GetWorldPathFromName(newFileName, false);
      FileUtilities.Copy(this.Path, worldPathFromName, false);
      this._path = worldPathFromName;
      if (newDisplayName == null)
        return;
      WorldGen.RenameWorld(this, newDisplayName, new Action<string>(this.OnWorldRenameSuccess));
    }

    private void OnWorldRenameSuccess(string newWorldName)
    {
      this.Name = newWorldName;
      Main.DelayedProcesses.Add(this.DelayedGoToTitleScreen());
    }

    private IEnumerator DelayedGoToTitleScreen()
    {
      SoundEngine.PlaySound(10);
      Main.menuMode = 0;
      yield break;
    }
  }
}
