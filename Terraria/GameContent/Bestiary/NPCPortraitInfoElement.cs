﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.NPCPortraitInfoElement
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
  public class NPCPortraitInfoElement : IBestiaryInfoElement
  {
    private int? _filledStarsCount;

    public NPCPortraitInfoElement(int? rarityStars = null) => this._filledStarsCount = rarityStars;

    public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
    {
      UIElement uiElement = new UIElement()
      {
        Width = new StyleDimension(0.0f, 1f),
        Height = new StyleDimension(112f, 0.0f)
      };
      uiElement.SetPadding(0.0f);
      BestiaryEntry entry = new BestiaryEntry();
      Asset<Texture2D> portraitBackgroundAsset = (Asset<Texture2D>) null;
      Color white = Color.White;
      entry.Icon = info.OwnerEntry.Icon.CreateClone();
      entry.UIInfoProvider = info.OwnerEntry.UIInfoProvider;
      List<IBestiaryBackgroundOverlayAndColorProvider> overlays = new List<IBestiaryBackgroundOverlayAndColorProvider>();
      bool flag1 = info.UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
      if (flag1)
      {
        List<IBestiaryInfoElement> source1 = new List<IBestiaryInfoElement>();
        IEnumerable<IBestiaryBackgroundImagePathAndColorProvider> source2 = info.OwnerEntry.Info.OfType<IBestiaryBackgroundImagePathAndColorProvider>();
        IEnumerable<IPreferenceProviderElement> preferences = info.OwnerEntry.Info.OfType<IPreferenceProviderElement>();
        Func<IBestiaryBackgroundImagePathAndColorProvider, bool> predicate = (Func<IBestiaryBackgroundImagePathAndColorProvider, bool>) (provider => preferences.Any<IPreferenceProviderElement>((Func<IPreferenceProviderElement, bool>) (preference => preference.Matches(provider))));
        IEnumerable<IBestiaryBackgroundImagePathAndColorProvider> andColorProviders = source2.Where<IBestiaryBackgroundImagePathAndColorProvider>(predicate);
        bool flag2 = false;
        foreach (IBestiaryBackgroundImagePathAndColorProvider andColorProvider in andColorProviders)
        {
          Asset<Texture2D> backgroundImage = andColorProvider.GetBackgroundImage();
          if (backgroundImage != null)
          {
            portraitBackgroundAsset = backgroundImage;
            flag2 = true;
            Color? backgroundColor = andColorProvider.GetBackgroundColor();
            if (backgroundColor.HasValue)
            {
              white = backgroundColor.Value;
              break;
            }
            break;
          }
        }
        foreach (IBestiaryInfoElement bestiaryInfoElement in info.OwnerEntry.Info)
        {
          if (bestiaryInfoElement is IBestiaryBackgroundImagePathAndColorProvider andColorProvider1)
          {
            Asset<Texture2D> backgroundImage = andColorProvider1.GetBackgroundImage();
            if (backgroundImage != null)
            {
              if (!flag2)
                portraitBackgroundAsset = backgroundImage;
              Color? backgroundColor = andColorProvider1.GetBackgroundColor();
              if (backgroundColor.HasValue)
                white = backgroundColor.Value;
            }
            else
              continue;
          }
          if (!flag2 && bestiaryInfoElement is IBestiaryBackgroundOverlayAndColorProvider andColorProvider2 && andColorProvider2.GetBackgroundOverlayImage() != null)
            source1.Add(bestiaryInfoElement);
        }
        overlays.AddRange(source1.OrderBy<IBestiaryInfoElement, float>(new Func<IBestiaryInfoElement, float>(this.GetSortingValueForElement)).Select<IBestiaryInfoElement, IBestiaryBackgroundOverlayAndColorProvider>((Func<IBestiaryInfoElement, IBestiaryBackgroundOverlayAndColorProvider>) (x => x as IBestiaryBackgroundOverlayAndColorProvider)));
      }
      UIBestiaryNPCEntryPortrait npcEntryPortrait = new UIBestiaryNPCEntryPortrait(entry, portraitBackgroundAsset, white, overlays);
      npcEntryPortrait.Left = new StyleDimension(4f, 0.0f);
      npcEntryPortrait.HAlign = 0.0f;
      UIBestiaryNPCEntryPortrait element = npcEntryPortrait;
      uiElement.Append((UIElement) element);
      if (flag1 && this._filledStarsCount.HasValue)
      {
        UIElement starsContainer = this.CreateStarsContainer();
        uiElement.Append(starsContainer);
      }
      return uiElement;
    }

    private float GetSortingValueForElement(IBestiaryInfoElement element) => element is IBestiaryBackgroundOverlayAndColorProvider andColorProvider ? andColorProvider.DisplayPriority : 0.0f;

    private UIElement CreateStarsContainer()
    {
      int num1 = 14;
      int num2 = 14;
      int num3 = -4;
      int num4 = num1 + num3;
      int val2 = 5;
      int val1 = 5;
      int num5 = this._filledStarsCount.Value;
      float num6 = 1f;
      int num7 = num4 * Math.Min(val1, val2) - num3;
      double num8 = (double) num4 * Math.Ceiling((double) val2 / (double) val1) - (double) num3;
      UIPanel uiPanel = new UIPanel(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel", (AssetRequestMode) 1), (Asset<Texture2D>) null, 5, 21);
      uiPanel.Width = new StyleDimension((float) num7 + num6 * 2f, 0.0f);
      uiPanel.Height = new StyleDimension((float) num8 + num6 * 2f, 0.0f);
      uiPanel.BackgroundColor = Color.Gray * 0.0f;
      uiPanel.BorderColor = Color.Transparent;
      uiPanel.Left = new StyleDimension(10f, 0.0f);
      uiPanel.Top = new StyleDimension(6f, 0.0f);
      uiPanel.VAlign = 0.0f;
      UIElement starsContainer = (UIElement) uiPanel;
      starsContainer.SetPadding(0.0f);
      for (int index = val2 - 1; index >= 0; --index)
      {
        string str = "Images/UI/Bestiary/Icon_Rank_Light";
        if (index >= num5)
          str = "Images/UI/Bestiary/Icon_Rank_Dim";
        UIImage uiImage = new UIImage(Main.Assets.Request<Texture2D>(str, (AssetRequestMode) 1));
        uiImage.Left = new StyleDimension((float) ((double) (num4 * (index % val1)) - (double) num7 * 0.5 + (double) num1 * 0.5), 0.0f);
        uiImage.Top = new StyleDimension((float) ((double) (num4 * (index / val1)) - num8 * 0.5 + (double) num2 * 0.5), 0.0f);
        uiImage.HAlign = 0.5f;
        uiImage.VAlign = 0.5f;
        UIImage element = uiImage;
        starsContainer.Append((UIElement) element);
      }
      return starsContainer;
    }
  }
}
