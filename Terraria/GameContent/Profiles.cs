// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Profiles
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.Localization;

namespace Terraria.GameContent
{
  public class Profiles
  {
    public class LegacyNPCProfile : ITownNPCProfile
    {
      private string _rootFilePath;
      private int _defaultVariationHeadIndex;
      private Asset<Texture2D> _defaultNoAlt;
      private Asset<Texture2D> _defaultParty;

      public LegacyNPCProfile(string npcFileTitleFilePath, int defaultHeadIndex)
      {
        this._rootFilePath = npcFileTitleFilePath;
        this._defaultVariationHeadIndex = defaultHeadIndex;
        this._defaultNoAlt = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default", (AssetRequestMode) 0);
        this._defaultParty = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default_Party", (AssetRequestMode) 0);
      }

      public int RollVariation() => 0;

      public string GetNameForVariant(NPC npc) => NPC.getNewNPCName(npc.type);

      public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn || npc.altTexture != 1 ? this._defaultNoAlt : this._defaultParty;

      public int GetHeadTextureIndex(NPC npc) => this._defaultVariationHeadIndex;
    }

    public class TransformableNPCProfile : ITownNPCProfile
    {
      private string _rootFilePath;
      private int _defaultVariationHeadIndex;
      private Asset<Texture2D> _defaultNoAlt;
      private Asset<Texture2D> _defaultTransformed;
      private Asset<Texture2D> _defaultCredits;

      public TransformableNPCProfile(string npcFileTitleFilePath, int defaultHeadIndex)
      {
        this._rootFilePath = npcFileTitleFilePath;
        this._defaultVariationHeadIndex = defaultHeadIndex;
        this._defaultNoAlt = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default", (AssetRequestMode) 0);
        this._defaultTransformed = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default_Transformed", (AssetRequestMode) 0);
        this._defaultCredits = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default_Credits", (AssetRequestMode) 0);
      }

      public int RollVariation() => 0;

      public string GetNameForVariant(NPC npc) => NPC.getNewNPCName(npc.type);

      public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
      {
        if (npc.altTexture == 3)
          return this._defaultCredits;
        return npc.IsABestiaryIconDummy || npc.altTexture != 2 ? this._defaultNoAlt : this._defaultTransformed;
      }

      public int GetHeadTextureIndex(NPC npc) => this._defaultVariationHeadIndex;
    }

    public class VariantNPCProfile : ITownNPCProfile
    {
      private string _rootFilePath;
      private string _npcBaseName;
      private int[] _variantHeadIDs;
      private string[] _variants;
      private Dictionary<string, Asset<Texture2D>> _variantTextures = new Dictionary<string, Asset<Texture2D>>();

      public VariantNPCProfile(
        string npcFileTitleFilePath,
        string npcBaseName,
        int[] variantHeadIds,
        params string[] variantTextureNames)
      {
        this._rootFilePath = npcFileTitleFilePath;
        this._npcBaseName = npcBaseName;
        this._variantHeadIDs = variantHeadIds;
        this._variants = variantTextureNames;
        foreach (string variant in this._variants)
        {
          string key = this._rootFilePath + "_" + variant;
          this._variantTextures[key] = Main.Assets.Request<Texture2D>(key, (AssetRequestMode) 0);
        }
      }

      public int RollVariation() => Main.rand.Next(this._variants.Length);

      public string GetNameForVariant(NPC npc) => Language.RandomFromCategory(this._npcBaseName + "Names_" + this._variants[npc.townNpcVariationIndex], WorldGen.genRand).Value;

      public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
      {
        string key = this._rootFilePath + "_" + this._variants[npc.townNpcVariationIndex];
        return npc.IsABestiaryIconDummy || npc.altTexture != 1 || !this._variantTextures.ContainsKey(key + "_Party") ? this._variantTextures[key] : this._variantTextures[key + "_Party"];
      }

      public int GetHeadTextureIndex(NPC npc) => this._variantHeadIDs[npc.townNpcVariationIndex];
    }
  }
}
