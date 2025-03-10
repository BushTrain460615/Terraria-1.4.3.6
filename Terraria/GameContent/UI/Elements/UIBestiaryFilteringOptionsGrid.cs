﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UIBestiaryFilteringOptionsGrid
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
  public class UIBestiaryFilteringOptionsGrid : UIPanel
  {
    private EntryFilterer<BestiaryEntry, IBestiaryEntryFilter> _filterer;
    private List<GroupOptionButton<int>> _filterButtons;
    private List<bool> _areFiltersAvailable;
    private List<List<BestiaryEntry>> _filterAvailabilityTests;
    private UIElement _container;

    public event Action OnClickingOption;

    public UIBestiaryFilteringOptionsGrid(
      EntryFilterer<BestiaryEntry, IBestiaryEntryFilter> filterer)
    {
      this._filterer = filterer;
      this._filterButtons = new List<GroupOptionButton<int>>();
      this._areFiltersAvailable = new List<bool>();
      this._filterAvailabilityTests = new List<List<BestiaryEntry>>();
      this.Width = new StyleDimension(0.0f, 1f);
      this.Height = new StyleDimension(0.0f, 1f);
      this.BackgroundColor = new Color(35, 40, 83) * 0.5f;
      this.BorderColor = new Color(35, 40, 83) * 0.5f;
      this.IgnoresMouseInteraction = false;
      this.SetPadding(0.0f);
      this.BuildContainer();
    }

    private void BuildContainer()
    {
      int widthWithSpacing;
      int heightWithSpacing;
      int perRow;
      int howManyRows;
      this.GetDisplaySettings(out int _, out int _, out widthWithSpacing, out heightWithSpacing, out perRow, out float _, out float _, out howManyRows);
      UIPanel uiPanel = new UIPanel();
      uiPanel.Width = new StyleDimension((float) (perRow * widthWithSpacing + 10), 0.0f);
      uiPanel.Height = new StyleDimension((float) (howManyRows * heightWithSpacing + 10), 0.0f);
      uiPanel.HAlign = 1f;
      uiPanel.VAlign = 0.0f;
      uiPanel.Left = new StyleDimension(0.0f, 0.0f);
      uiPanel.Top = new StyleDimension(0.0f, 0.0f);
      UIPanel element = uiPanel;
      element.BorderColor = new Color(89, 116, 213, (int) byte.MaxValue) * 0.9f;
      element.BackgroundColor = new Color(73, 94, 171) * 0.9f;
      element.SetPadding(0.0f);
      this.Append((UIElement) element);
      this._container = (UIElement) element;
    }

    public void SetupAvailabilityTest(List<BestiaryEntry> allAvailableEntries)
    {
      this._filterAvailabilityTests.Clear();
      for (int index1 = 0; index1 < this._filterer.AvailableFilters.Count; ++index1)
      {
        List<BestiaryEntry> bestiaryEntryList = new List<BestiaryEntry>();
        this._filterAvailabilityTests.Add(bestiaryEntryList);
        IBestiaryEntryFilter availableFilter = this._filterer.AvailableFilters[index1];
        for (int index2 = 0; index2 < allAvailableEntries.Count; ++index2)
        {
          if (availableFilter.FitsFilter(allAvailableEntries[index2]))
            bestiaryEntryList.Add(allAvailableEntries[index2]);
        }
      }
    }

    public void UpdateAvailability()
    {
      int widthPerButton;
      int heightPerButton;
      int widthWithSpacing;
      int heightWithSpacing;
      int perRow;
      float offsetLeft;
      float offsetTop;
      this.GetDisplaySettings(out widthPerButton, out heightPerButton, out widthWithSpacing, out heightWithSpacing, out perRow, out offsetLeft, out offsetTop, out int _);
      this._container.RemoveAllChildren();
      this._filterButtons.Clear();
      this._areFiltersAvailable.Clear();
      int pixels1 = -1;
      int pixels2 = -1;
      for (int index = 0; index < this._filterer.AvailableFilters.Count; ++index)
      {
        int num1 = index / perRow;
        int num2 = index % perRow;
        IBestiaryEntryFilter availableFilter = this._filterer.AvailableFilters[index];
        List<BestiaryEntry> availabilityTest = this._filterAvailabilityTests[index];
        if (this.GetIsFilterAvailableForEntries(availableFilter, availabilityTest))
        {
          GroupOptionButton<int> groupOptionButton1 = new GroupOptionButton<int>(index, (LocalizedText) null, (LocalizedText) null, Color.White, (string) null);
          groupOptionButton1.Width = new StyleDimension((float) widthPerButton, 0.0f);
          groupOptionButton1.Height = new StyleDimension((float) heightPerButton, 0.0f);
          groupOptionButton1.HAlign = 0.0f;
          groupOptionButton1.VAlign = 0.0f;
          groupOptionButton1.Top = new StyleDimension(offsetTop + (float) (num1 * heightWithSpacing), 0.0f);
          groupOptionButton1.Left = new StyleDimension(offsetLeft + (float) (num2 * widthWithSpacing), 0.0f);
          GroupOptionButton<int> groupOptionButton2 = groupOptionButton1;
          groupOptionButton2.OnClick += new UIElement.MouseEvent(this.ClickOption);
          groupOptionButton2.SetSnapPoint("Filters", index);
          groupOptionButton2.ShowHighlightWhenSelected = false;
          this.AddOnHover(availableFilter, (UIElement) groupOptionButton2);
          this._container.Append((UIElement) groupOptionButton2);
          UIElement image = availableFilter.GetImage();
          if (image != null)
          {
            image.Left = new StyleDimension((float) pixels1, 0.0f);
            image.Top = new StyleDimension((float) pixels2, 0.0f);
            groupOptionButton2.Append(image);
          }
          this._filterButtons.Add(groupOptionButton2);
        }
        else
        {
          this._filterer.ActiveFilters.Remove(availableFilter);
          GroupOptionButton<int> groupOptionButton = new GroupOptionButton<int>(-2, (LocalizedText) null, (LocalizedText) null, Color.White, (string) null);
          groupOptionButton.Width = new StyleDimension((float) widthPerButton, 0.0f);
          groupOptionButton.Height = new StyleDimension((float) heightPerButton, 0.0f);
          groupOptionButton.HAlign = 0.0f;
          groupOptionButton.VAlign = 0.0f;
          groupOptionButton.Top = new StyleDimension(offsetTop + (float) (num1 * heightWithSpacing), 0.0f);
          groupOptionButton.Left = new StyleDimension(offsetLeft + (float) (num2 * widthWithSpacing), 0.0f);
          groupOptionButton.FadeFromBlack = 0.5f;
          GroupOptionButton<int> element1 = groupOptionButton;
          element1.ShowHighlightWhenSelected = false;
          element1.SetPadding(0.0f);
          element1.SetSnapPoint("Filters", index);
          Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow", (AssetRequestMode) 1);
          UIImageFramed uiImageFramed = new UIImageFramed(asset, asset.Frame(16, 5, frameY: 4));
          uiImageFramed.HAlign = 0.5f;
          uiImageFramed.VAlign = 0.5f;
          uiImageFramed.Color = Color.White * 0.2f;
          UIImageFramed element2 = uiImageFramed;
          element2.Left = new StyleDimension((float) pixels1, 0.0f);
          element2.Top = new StyleDimension((float) pixels2, 0.0f);
          element1.Append((UIElement) element2);
          this._filterButtons.Add(element1);
          this._container.Append((UIElement) element1);
        }
      }
      this.UpdateButtonSelections();
    }

    public void GetEntriesToShow(
      out int maxEntriesWidth,
      out int maxEntriesHeight,
      out int maxEntriesToHave)
    {
      int perRow;
      int howManyRows;
      this.GetDisplaySettings(out int _, out int _, out int _, out int _, out perRow, out float _, out float _, out howManyRows);
      maxEntriesWidth = perRow;
      maxEntriesHeight = howManyRows;
      maxEntriesToHave = this._filterer.AvailableFilters.Count;
    }

    private void GetDisplaySettings(
      out int widthPerButton,
      out int heightPerButton,
      out int widthWithSpacing,
      out int heightWithSpacing,
      out int perRow,
      out float offsetLeft,
      out float offsetTop,
      out int howManyRows)
    {
      widthPerButton = 32;
      heightPerButton = 32;
      int num = 2;
      widthWithSpacing = widthPerButton + num;
      heightWithSpacing = heightPerButton + num;
      perRow = (int) Math.Ceiling(Math.Sqrt((double) this._filterer.AvailableFilters.Count));
      perRow = 12;
      howManyRows = (int) Math.Ceiling((double) this._filterer.AvailableFilters.Count / (double) perRow);
      offsetLeft = (float) (perRow * widthWithSpacing - num) * 0.5f;
      offsetTop = (float) (howManyRows * heightWithSpacing - num) * 0.5f;
      offsetLeft = 6f;
      offsetTop = 6f;
    }

    private void UpdateButtonSelections()
    {
      foreach (GroupOptionButton<int> filterButton in this._filterButtons)
      {
        bool flag = this._filterer.IsFilterActive(filterButton.OptionValue);
        filterButton.SetCurrentOption(flag ? filterButton.OptionValue : -1);
        if (flag)
          filterButton.SetColor(new Color(152, 175, 235), 1f);
        else
          filterButton.SetColor(Colors.InventoryDefaultColor, 0.7f);
      }
    }

    private bool GetIsFilterAvailableForEntries(
      IBestiaryEntryFilter filter,
      List<BestiaryEntry> entries)
    {
      bool? forcedDisplay = filter.ForcedDisplay;
      if (forcedDisplay.HasValue)
        return forcedDisplay.Value;
      for (int index = 0; index < entries.Count; ++index)
      {
        if (filter.FitsFilter(entries[index]) && entries[index].UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0)
          return true;
      }
      return false;
    }

    private void AddOnHover(IBestiaryEntryFilter filter, UIElement button) => button.OnUpdate += (UIElement.ElementEvent) (element => this.ShowButtonName(element, filter));

    private void ShowButtonName(UIElement element, IBestiaryEntryFilter number)
    {
      if (!element.IsMouseHovering)
        return;
      string textValue = Language.GetTextValue(number.GetDisplayNameKey());
      Main.instance.MouseText(textValue);
    }

    private void ClickOption(UIMouseEvent evt, UIElement listeningElement)
    {
      this._filterer.ToggleFilter(((GroupOptionButton<int>) listeningElement).OptionValue);
      this.UpdateButtonSelections();
      if (this.OnClickingOption == null)
        return;
      this.OnClickingOption();
    }
  }
}
