﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.States.UIManageControls
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
  public class UIManageControls : UIState
  {
    public static int ForceMoveTo = -1;
    private const float PanelTextureHeight = 30f;
    private static List<string> _BindingsFullLine = new List<string>()
    {
      "Throw",
      "Inventory",
      "RadialHotbar",
      "RadialQuickbar",
      "LockOn",
      "ToggleCreativeMenu",
      "sp3",
      "sp4",
      "sp5",
      "sp6",
      "sp7",
      "sp8",
      "sp18",
      "sp19",
      "sp9",
      "sp10",
      "sp11",
      "sp12",
      "sp13"
    };
    private static List<string> _BindingsHalfSingleLine = new List<string>()
    {
      "sp9",
      "sp10",
      "sp11",
      "sp12",
      "sp13"
    };
    private bool OnKeyboard = true;
    private bool OnGameplay = true;
    private List<UIElement> _bindsKeyboard = new List<UIElement>();
    private List<UIElement> _bindsGamepad = new List<UIElement>();
    private List<UIElement> _bindsKeyboardUI = new List<UIElement>();
    private List<UIElement> _bindsGamepadUI = new List<UIElement>();
    private UIElement _outerContainer;
    private UIList _uilist;
    private UIImageFramed _buttonKeyboard;
    private UIImageFramed _buttonGamepad;
    private UIImageFramed _buttonBorder1;
    private UIImageFramed _buttonBorder2;
    private UIKeybindingSimpleListItem _buttonProfile;
    private UIElement _buttonBack;
    private UIImageFramed _buttonVs1;
    private UIImageFramed _buttonVs2;
    private UIImageFramed _buttonBorderVs1;
    private UIImageFramed _buttonBorderVs2;
    private Asset<Texture2D> _KeyboardGamepadTexture;
    private Asset<Texture2D> _keyboardGamepadBorderTexture;
    private Asset<Texture2D> _GameplayVsUITexture;
    private Asset<Texture2D> _GameplayVsUIBorderTexture;
    private static int SnapPointIndex;

    public override void OnInitialize()
    {
      this._KeyboardGamepadTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs", (AssetRequestMode) 1);
      this._keyboardGamepadBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs_Border", (AssetRequestMode) 1);
      this._GameplayVsUITexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs_2", (AssetRequestMode) 1);
      this._GameplayVsUIBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Inputs_2_Border", (AssetRequestMode) 1);
      UIElement element1 = new UIElement();
      element1.Width.Set(0.0f, 0.8f);
      element1.MaxWidth.Set(600f, 0.0f);
      element1.Top.Set(220f, 0.0f);
      element1.Height.Set(-200f, 1f);
      element1.HAlign = 0.5f;
      this._outerContainer = element1;
      UIPanel element2 = new UIPanel();
      element2.Width.Set(0.0f, 1f);
      element2.Height.Set(-110f, 1f);
      element2.BackgroundColor = new Color(33, 43, 79) * 0.8f;
      element1.Append((UIElement) element2);
      this._buttonKeyboard = new UIImageFramed(this._KeyboardGamepadTexture, this._KeyboardGamepadTexture.Frame(2, 2));
      this._buttonKeyboard.VAlign = 0.0f;
      this._buttonKeyboard.HAlign = 0.0f;
      this._buttonKeyboard.Left.Set(0.0f, 0.0f);
      this._buttonKeyboard.Top.Set(8f, 0.0f);
      this._buttonKeyboard.OnClick += new UIElement.MouseEvent(this.KeyboardButtonClick);
      this._buttonKeyboard.OnMouseOver += new UIElement.MouseEvent(this.ManageBorderKeyboardOn);
      this._buttonKeyboard.OnMouseOut += new UIElement.MouseEvent(this.ManageBorderKeyboardOff);
      element2.Append((UIElement) this._buttonKeyboard);
      this._buttonGamepad = new UIImageFramed(this._KeyboardGamepadTexture, this._KeyboardGamepadTexture.Frame(2, 2, 1, 1));
      this._buttonGamepad.VAlign = 0.0f;
      this._buttonGamepad.HAlign = 0.0f;
      this._buttonGamepad.Left.Set(76f, 0.0f);
      this._buttonGamepad.Top.Set(8f, 0.0f);
      this._buttonGamepad.OnClick += new UIElement.MouseEvent(this.GamepadButtonClick);
      this._buttonGamepad.OnMouseOver += new UIElement.MouseEvent(this.ManageBorderGamepadOn);
      this._buttonGamepad.OnMouseOut += new UIElement.MouseEvent(this.ManageBorderGamepadOff);
      element2.Append((UIElement) this._buttonGamepad);
      this._buttonBorder1 = new UIImageFramed(this._keyboardGamepadBorderTexture, this._keyboardGamepadBorderTexture.Frame());
      this._buttonBorder1.VAlign = 0.0f;
      this._buttonBorder1.HAlign = 0.0f;
      this._buttonBorder1.Left.Set(0.0f, 0.0f);
      this._buttonBorder1.Top.Set(8f, 0.0f);
      this._buttonBorder1.Color = Color.Silver;
      this._buttonBorder1.IgnoresMouseInteraction = true;
      element2.Append((UIElement) this._buttonBorder1);
      this._buttonBorder2 = new UIImageFramed(this._keyboardGamepadBorderTexture, this._keyboardGamepadBorderTexture.Frame());
      this._buttonBorder2.VAlign = 0.0f;
      this._buttonBorder2.HAlign = 0.0f;
      this._buttonBorder2.Left.Set(76f, 0.0f);
      this._buttonBorder2.Top.Set(8f, 0.0f);
      this._buttonBorder2.Color = Color.Transparent;
      this._buttonBorder2.IgnoresMouseInteraction = true;
      element2.Append((UIElement) this._buttonBorder2);
      this._buttonVs1 = new UIImageFramed(this._GameplayVsUITexture, this._GameplayVsUITexture.Frame(2, 2));
      this._buttonVs1.VAlign = 0.0f;
      this._buttonVs1.HAlign = 0.0f;
      this._buttonVs1.Left.Set(172f, 0.0f);
      this._buttonVs1.Top.Set(8f, 0.0f);
      this._buttonVs1.OnClick += new UIElement.MouseEvent(this.VsGameplayButtonClick);
      this._buttonVs1.OnMouseOver += new UIElement.MouseEvent(this.ManageBorderGameplayOn);
      this._buttonVs1.OnMouseOut += new UIElement.MouseEvent(this.ManageBorderGameplayOff);
      element2.Append((UIElement) this._buttonVs1);
      this._buttonVs2 = new UIImageFramed(this._GameplayVsUITexture, this._GameplayVsUITexture.Frame(2, 2, 1, 1));
      this._buttonVs2.VAlign = 0.0f;
      this._buttonVs2.HAlign = 0.0f;
      this._buttonVs2.Left.Set(212f, 0.0f);
      this._buttonVs2.Top.Set(8f, 0.0f);
      this._buttonVs2.OnClick += new UIElement.MouseEvent(this.VsMenuButtonClick);
      this._buttonVs2.OnMouseOver += new UIElement.MouseEvent(this.ManageBorderMenuOn);
      this._buttonVs2.OnMouseOut += new UIElement.MouseEvent(this.ManageBorderMenuOff);
      element2.Append((UIElement) this._buttonVs2);
      this._buttonBorderVs1 = new UIImageFramed(this._GameplayVsUIBorderTexture, this._GameplayVsUIBorderTexture.Frame());
      this._buttonBorderVs1.VAlign = 0.0f;
      this._buttonBorderVs1.HAlign = 0.0f;
      this._buttonBorderVs1.Left.Set(172f, 0.0f);
      this._buttonBorderVs1.Top.Set(8f, 0.0f);
      this._buttonBorderVs1.Color = Color.Silver;
      this._buttonBorderVs1.IgnoresMouseInteraction = true;
      element2.Append((UIElement) this._buttonBorderVs1);
      this._buttonBorderVs2 = new UIImageFramed(this._GameplayVsUIBorderTexture, this._GameplayVsUIBorderTexture.Frame());
      this._buttonBorderVs2.VAlign = 0.0f;
      this._buttonBorderVs2.HAlign = 0.0f;
      this._buttonBorderVs2.Left.Set(212f, 0.0f);
      this._buttonBorderVs2.Top.Set(8f, 0.0f);
      this._buttonBorderVs2.Color = Color.Transparent;
      this._buttonBorderVs2.IgnoresMouseInteraction = true;
      element2.Append((UIElement) this._buttonBorderVs2);
      this._buttonProfile = new UIKeybindingSimpleListItem((Func<string>) (() => PlayerInput.CurrentProfile.ShowName), new Color(73, 94, 171, (int) byte.MaxValue) * 0.9f);
      this._buttonProfile.VAlign = 0.0f;
      this._buttonProfile.HAlign = 1f;
      this._buttonProfile.Width.Set(180f, 0.0f);
      this._buttonProfile.Height.Set(30f, 0.0f);
      this._buttonProfile.MarginRight = 30f;
      this._buttonProfile.Left.Set(0.0f, 0.0f);
      this._buttonProfile.Top.Set(8f, 0.0f);
      this._buttonProfile.OnClick += new UIElement.MouseEvent(this.profileButtonClick);
      element2.Append((UIElement) this._buttonProfile);
      this._uilist = new UIList();
      this._uilist.Width.Set(-25f, 1f);
      this._uilist.Height.Set(-50f, 1f);
      this._uilist.VAlign = 1f;
      this._uilist.PaddingBottom = 5f;
      this._uilist.ListPadding = 20f;
      element2.Append((UIElement) this._uilist);
      this.AssembleBindPanels();
      this.FillList();
      UIScrollbar uiScrollbar = new UIScrollbar();
      uiScrollbar.SetView(100f, 1000f);
      uiScrollbar.Height.Set(-67f, 1f);
      uiScrollbar.HAlign = 1f;
      uiScrollbar.VAlign = 1f;
      uiScrollbar.MarginBottom = 11f;
      element2.Append((UIElement) uiScrollbar);
      this._uilist.SetScrollbar(uiScrollbar);
      UITextPanel<LocalizedText> element3 = new UITextPanel<LocalizedText>(Language.GetText("UI.Keybindings"), 0.7f, true);
      element3.HAlign = 0.5f;
      element3.Top.Set(-45f, 0.0f);
      element3.Left.Set(-10f, 0.0f);
      element3.SetPadding(15f);
      element3.BackgroundColor = new Color(73, 94, 171);
      element1.Append((UIElement) element3);
      UITextPanel<LocalizedText> element4 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
      element4.Width.Set(-10f, 0.5f);
      element4.Height.Set(50f, 0.0f);
      element4.VAlign = 1f;
      element4.HAlign = 0.5f;
      element4.Top.Set(-45f, 0.0f);
      element4.OnMouseOver += new UIElement.MouseEvent(this.FadedMouseOver);
      element4.OnMouseOut += new UIElement.MouseEvent(this.FadedMouseOut);
      element4.OnClick += new UIElement.MouseEvent(this.GoBackClick);
      element1.Append((UIElement) element4);
      this._buttonBack = (UIElement) element4;
      this.Append(element1);
    }

    private void AssembleBindPanels()
    {
      List<string> bindings1 = new List<string>()
      {
        "MouseLeft",
        "MouseRight",
        "Up",
        "Down",
        "Left",
        "Right",
        "Jump",
        "Grapple",
        "SmartSelect",
        "SmartCursor",
        "QuickMount",
        "QuickHeal",
        "QuickMana",
        "QuickBuff",
        "Throw",
        "Inventory",
        "ToggleCreativeMenu",
        "ViewZoomIn",
        "ViewZoomOut",
        "sp9"
      };
      List<string> bindings2 = new List<string>()
      {
        "MouseLeft",
        "MouseRight",
        "Up",
        "Down",
        "Left",
        "Right",
        "Jump",
        "Grapple",
        "SmartSelect",
        "SmartCursor",
        "QuickMount",
        "QuickHeal",
        "QuickMana",
        "QuickBuff",
        "LockOn",
        "Throw",
        "Inventory",
        "sp9"
      };
      List<string> bindings3 = new List<string>()
      {
        "HotbarMinus",
        "HotbarPlus",
        "Hotbar1",
        "Hotbar2",
        "Hotbar3",
        "Hotbar4",
        "Hotbar5",
        "Hotbar6",
        "Hotbar7",
        "Hotbar8",
        "Hotbar9",
        "Hotbar10",
        "sp10"
      };
      List<string> bindings4 = new List<string>()
      {
        "MapZoomIn",
        "MapZoomOut",
        "MapAlphaUp",
        "MapAlphaDown",
        "MapFull",
        "MapStyle",
        "sp11"
      };
      List<string> bindings5 = new List<string>()
      {
        "sp1",
        "sp2",
        "RadialHotbar",
        "RadialQuickbar",
        "sp12"
      };
      List<string> bindings6 = new List<string>()
      {
        "sp3",
        "sp4",
        "sp5",
        "sp6",
        "sp7",
        "sp8",
        "sp14",
        "sp15",
        "sp16",
        "sp17",
        "sp18",
        "sp19",
        "sp13"
      };
      InputMode currentInputMode1 = InputMode.Keyboard;
      this._bindsKeyboard.Add((UIElement) this.CreateBindingGroup(0, bindings1, currentInputMode1));
      this._bindsKeyboard.Add((UIElement) this.CreateBindingGroup(1, bindings4, currentInputMode1));
      this._bindsKeyboard.Add((UIElement) this.CreateBindingGroup(2, bindings3, currentInputMode1));
      InputMode currentInputMode2 = InputMode.XBoxGamepad;
      this._bindsGamepad.Add((UIElement) this.CreateBindingGroup(0, bindings2, currentInputMode2));
      this._bindsGamepad.Add((UIElement) this.CreateBindingGroup(1, bindings4, currentInputMode2));
      this._bindsGamepad.Add((UIElement) this.CreateBindingGroup(2, bindings3, currentInputMode2));
      this._bindsGamepad.Add((UIElement) this.CreateBindingGroup(3, bindings5, currentInputMode2));
      this._bindsGamepad.Add((UIElement) this.CreateBindingGroup(4, bindings6, currentInputMode2));
      InputMode currentInputMode3 = InputMode.KeyboardUI;
      this._bindsKeyboardUI.Add((UIElement) this.CreateBindingGroup(0, bindings1, currentInputMode3));
      this._bindsKeyboardUI.Add((UIElement) this.CreateBindingGroup(1, bindings4, currentInputMode3));
      this._bindsKeyboardUI.Add((UIElement) this.CreateBindingGroup(2, bindings3, currentInputMode3));
      InputMode currentInputMode4 = InputMode.XBoxGamepadUI;
      this._bindsGamepadUI.Add((UIElement) this.CreateBindingGroup(0, bindings2, currentInputMode4));
      this._bindsGamepadUI.Add((UIElement) this.CreateBindingGroup(1, bindings4, currentInputMode4));
      this._bindsGamepadUI.Add((UIElement) this.CreateBindingGroup(2, bindings3, currentInputMode4));
      this._bindsGamepadUI.Add((UIElement) this.CreateBindingGroup(3, bindings5, currentInputMode4));
      this._bindsGamepadUI.Add((UIElement) this.CreateBindingGroup(4, bindings6, currentInputMode4));
    }

    private UISortableElement CreateBindingGroup(
      int elementIndex,
      List<string> bindings,
      InputMode currentInputMode)
    {
      UISortableElement bindingGroup = new UISortableElement(elementIndex);
      bindingGroup.HAlign = 0.5f;
      bindingGroup.Width.Set(0.0f, 1f);
      bindingGroup.Height.Set(2000f, 0.0f);
      UIPanel element1 = new UIPanel();
      element1.Width.Set(0.0f, 1f);
      element1.Height.Set(-16f, 1f);
      element1.VAlign = 1f;
      element1.BackgroundColor = new Color(33, 43, 79) * 0.8f;
      bindingGroup.Append((UIElement) element1);
      UIList uiList = new UIList();
      uiList.OverflowHidden = false;
      uiList.Width.Set(0.0f, 1f);
      uiList.Height.Set(-8f, 1f);
      uiList.VAlign = 1f;
      uiList.ListPadding = 5f;
      element1.Append((UIElement) uiList);
      Color backgroundColor = element1.BackgroundColor;
      switch (elementIndex)
      {
        case 0:
          element1.BackgroundColor = Color.Lerp(element1.BackgroundColor, Color.Green, 0.18f);
          break;
        case 1:
          element1.BackgroundColor = Color.Lerp(element1.BackgroundColor, Color.Goldenrod, 0.18f);
          break;
        case 2:
          element1.BackgroundColor = Color.Lerp(element1.BackgroundColor, Color.HotPink, 0.18f);
          break;
        case 3:
          element1.BackgroundColor = Color.Lerp(element1.BackgroundColor, Color.Indigo, 0.18f);
          break;
        case 4:
          element1.BackgroundColor = Color.Lerp(element1.BackgroundColor, Color.Turquoise, 0.18f);
          break;
      }
      this.CreateElementGroup(uiList, bindings, currentInputMode, element1.BackgroundColor);
      element1.BackgroundColor = element1.BackgroundColor.MultiplyRGBA(new Color(111, 111, 111));
      LocalizedText text = LocalizedText.Empty;
      switch (elementIndex)
      {
        case 0:
          text = currentInputMode == InputMode.Keyboard || currentInputMode == InputMode.XBoxGamepad ? Lang.menu[164] : Lang.menu[243];
          break;
        case 1:
          text = Lang.menu[165];
          break;
        case 2:
          text = Lang.menu[166];
          break;
        case 3:
          text = Lang.menu[167];
          break;
        case 4:
          text = Lang.menu[198];
          break;
      }
      UITextPanel<LocalizedText> uiTextPanel = new UITextPanel<LocalizedText>(text, 0.7f);
      uiTextPanel.VAlign = 0.0f;
      uiTextPanel.HAlign = 0.5f;
      UITextPanel<LocalizedText> element2 = uiTextPanel;
      bindingGroup.Append((UIElement) element2);
      bindingGroup.Recalculate();
      float totalHeight = uiList.GetTotalHeight();
      bindingGroup.Width.Set(0.0f, 1f);
      bindingGroup.Height.Set((float) ((double) totalHeight + 30.0 + 16.0), 0.0f);
      return bindingGroup;
    }

    private void CreateElementGroup(
      UIList parent,
      List<string> bindings,
      InputMode currentInputMode,
      Color color)
    {
      for (int index = 0; index < bindings.Count; ++index)
      {
        string binding = bindings[index];
        UISortableElement uiSortableElement = new UISortableElement(index);
        uiSortableElement.Width.Set(0.0f, 1f);
        uiSortableElement.Height.Set(30f, 0.0f);
        uiSortableElement.HAlign = 0.5f;
        parent.Add((UIElement) uiSortableElement);
        if (UIManageControls._BindingsHalfSingleLine.Contains(bindings[index]))
        {
          UIElement panel = this.CreatePanel(bindings[index], currentInputMode, color);
          panel.Width.Set(0.0f, 0.5f);
          panel.HAlign = 0.5f;
          panel.Height.Set(0.0f, 1f);
          panel.SetSnapPoint("Wide", UIManageControls.SnapPointIndex++);
          uiSortableElement.Append(panel);
        }
        else if (UIManageControls._BindingsFullLine.Contains(bindings[index]))
        {
          UIElement panel = this.CreatePanel(bindings[index], currentInputMode, color);
          panel.Width.Set(0.0f, 1f);
          panel.Height.Set(0.0f, 1f);
          panel.SetSnapPoint("Wide", UIManageControls.SnapPointIndex++);
          uiSortableElement.Append(panel);
        }
        else
        {
          UIElement panel1 = this.CreatePanel(bindings[index], currentInputMode, color);
          panel1.Width.Set(-5f, 0.5f);
          panel1.Height.Set(0.0f, 1f);
          panel1.SetSnapPoint("Thin", UIManageControls.SnapPointIndex++);
          uiSortableElement.Append(panel1);
          ++index;
          if (index < bindings.Count)
          {
            UIElement panel2 = this.CreatePanel(bindings[index], currentInputMode, color);
            panel2.Width.Set(-5f, 0.5f);
            panel2.Height.Set(0.0f, 1f);
            panel2.HAlign = 1f;
            panel2.SetSnapPoint("Thin", UIManageControls.SnapPointIndex++);
            uiSortableElement.Append(panel2);
          }
        }
      }
    }

    public UIElement CreatePanel(string bind, InputMode currentInputMode, Color color)
    {
      switch (bind)
      {
        case "sp1":
          UIKeybindingToggleListItem panel1 = new UIKeybindingToggleListItem((Func<string>) (() => Lang.menu[196].Value), (Func<bool>) (() => PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Contains(Buttons.DPadUp.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Contains(Buttons.DPadRight.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Contains(Buttons.DPadDown.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Contains(Buttons.DPadLeft.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Contains(Buttons.DPadUp.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Contains(Buttons.DPadRight.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Contains(Buttons.DPadDown.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Contains(Buttons.DPadLeft.ToString())), color);
          panel1.OnClick += new UIElement.MouseEvent(this.SnapButtonClick);
          return (UIElement) panel1;
        case "sp10":
          UIKeybindingSimpleListItem panel2 = new UIKeybindingSimpleListItem((Func<string>) (() => Lang.menu[86].Value), color);
          panel2.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
          {
            string copyableProfileName = UIManageControls.GetCopyableProfileName();
            PlayerInput.CurrentProfile.CopyHotbarSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
          });
          return (UIElement) panel2;
        case "sp11":
          UIKeybindingSimpleListItem panel3 = new UIKeybindingSimpleListItem((Func<string>) (() => Lang.menu[86].Value), color);
          panel3.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
          {
            string copyableProfileName = UIManageControls.GetCopyableProfileName();
            PlayerInput.CurrentProfile.CopyMapSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
          });
          return (UIElement) panel3;
        case "sp12":
          UIKeybindingSimpleListItem panel4 = new UIKeybindingSimpleListItem((Func<string>) (() => Lang.menu[86].Value), color);
          panel4.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
          {
            string copyableProfileName = UIManageControls.GetCopyableProfileName();
            PlayerInput.CurrentProfile.CopyGamepadSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
          });
          return (UIElement) panel4;
        case "sp13":
          UIKeybindingSimpleListItem panel5 = new UIKeybindingSimpleListItem((Func<string>) (() => Lang.menu[86].Value), color);
          panel5.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
          {
            string copyableProfileName = UIManageControls.GetCopyableProfileName();
            PlayerInput.CurrentProfile.CopyGamepadAdvancedSettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
          });
          return (UIElement) panel5;
        case "sp14":
          UIKeybindingToggleListItem panel6 = new UIKeybindingToggleListItem((Func<string>) (() => Lang.menu[205].Value), (Func<bool>) (() => PlayerInput.CurrentProfile.LeftThumbstickInvertX), color);
          panel6.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
          {
            if (!PlayerInput.CurrentProfile.AllowEditting)
              return;
            PlayerInput.CurrentProfile.LeftThumbstickInvertX = !PlayerInput.CurrentProfile.LeftThumbstickInvertX;
          });
          return (UIElement) panel6;
        case "sp15":
          UIKeybindingToggleListItem panel7 = new UIKeybindingToggleListItem((Func<string>) (() => Lang.menu[206].Value), (Func<bool>) (() => PlayerInput.CurrentProfile.LeftThumbstickInvertY), color);
          panel7.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
          {
            if (!PlayerInput.CurrentProfile.AllowEditting)
              return;
            PlayerInput.CurrentProfile.LeftThumbstickInvertY = !PlayerInput.CurrentProfile.LeftThumbstickInvertY;
          });
          return (UIElement) panel7;
        case "sp16":
          UIKeybindingToggleListItem panel8 = new UIKeybindingToggleListItem((Func<string>) (() => Lang.menu[207].Value), (Func<bool>) (() => PlayerInput.CurrentProfile.RightThumbstickInvertX), color);
          panel8.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
          {
            if (!PlayerInput.CurrentProfile.AllowEditting)
              return;
            PlayerInput.CurrentProfile.RightThumbstickInvertX = !PlayerInput.CurrentProfile.RightThumbstickInvertX;
          });
          return (UIElement) panel8;
        case "sp17":
          UIKeybindingToggleListItem panel9 = new UIKeybindingToggleListItem((Func<string>) (() => Lang.menu[208].Value), (Func<bool>) (() => PlayerInput.CurrentProfile.RightThumbstickInvertY), color);
          panel9.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
          {
            if (!PlayerInput.CurrentProfile.AllowEditting)
              return;
            PlayerInput.CurrentProfile.RightThumbstickInvertY = !PlayerInput.CurrentProfile.RightThumbstickInvertY;
          });
          return (UIElement) panel9;
        case "sp18":
          return (UIElement) new UIKeybindingSliderItem((Func<string>) (() =>
          {
            int holdTimeRequired = PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired;
            return holdTimeRequired == -1 ? Lang.menu[228].Value : Lang.menu[227].Value + " (" + ((float) holdTimeRequired / 60f).ToString("F2") + "s)";
          }), (Func<float>) (() => PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired == -1 ? 1f : (float) PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired / 301f), (Action<float>) (f =>
          {
            PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = (int) ((double) f * 301.0);
            if ((double) PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired != 301.0)
              return;
            PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = -1;
          }), (Action) (() =>
          {
            PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = (int) ((double) UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired == -1 ? 1f : (float) PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired / 301f, 0.0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX) * 301.0);
            if ((double) PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired != 301.0)
              return;
            PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired = -1;
          }), 1007, color);
        case "sp19":
          return (UIElement) new UIKeybindingSliderItem((Func<string>) (() =>
          {
            int inventoryMoveCd = PlayerInput.CurrentProfile.InventoryMoveCD;
            return Lang.menu[252].Value + " (" + ((float) inventoryMoveCd / 60f).ToString("F2") + "s)";
          }), (Func<float>) (() => Utils.GetLerpValue(4f, 12f, (float) PlayerInput.CurrentProfile.InventoryMoveCD, true)), (Action<float>) (f => PlayerInput.CurrentProfile.InventoryMoveCD = (int) Math.Round((double) MathHelper.Lerp(4f, 12f, f))), (Action) (() =>
          {
            if (UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD > 0)
              --UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD;
            if (UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD != 0)
              return;
            float lerpValue = Utils.GetLerpValue(4f, 12f, (float) PlayerInput.CurrentProfile.InventoryMoveCD, true);
            float num = UILinksInitializer.HandleSliderHorizontalInput(lerpValue, 0.0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX);
            if ((double) lerpValue == (double) num)
              return;
            UILinkPointNavigator.Shortcuts.INV_MOVE_OPTION_CD = 8;
            PlayerInput.CurrentProfile.InventoryMoveCD = (int) MathHelper.Clamp((float) (PlayerInput.CurrentProfile.InventoryMoveCD + Math.Sign(num - lerpValue)), 4f, 12f);
          }), 1008, color);
        case "sp2":
          UIKeybindingToggleListItem panel10 = new UIKeybindingToggleListItem((Func<string>) (() => Lang.menu[197].Value), (Func<bool>) (() => PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Contains(Buttons.DPadUp.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Contains(Buttons.DPadRight.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Contains(Buttons.DPadDown.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Contains(Buttons.DPadLeft.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Contains(Buttons.DPadUp.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Contains(Buttons.DPadRight.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Contains(Buttons.DPadDown.ToString()) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Contains(Buttons.DPadLeft.ToString())), color);
          panel10.OnClick += new UIElement.MouseEvent(this.RadialButtonClick);
          return (UIElement) panel10;
        case "sp3":
          return (UIElement) new UIKeybindingSliderItem((Func<string>) (() => Lang.menu[199].Value + " (" + PlayerInput.CurrentProfile.TriggersDeadzone.ToString("P1") + ")"), (Func<float>) (() => PlayerInput.CurrentProfile.TriggersDeadzone), (Action<float>) (f => PlayerInput.CurrentProfile.TriggersDeadzone = f), (Action) (() => PlayerInput.CurrentProfile.TriggersDeadzone = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.TriggersDeadzone, 0.0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f)), 1000, color);
        case "sp4":
          return (UIElement) new UIKeybindingSliderItem((Func<string>) (() => Lang.menu[200].Value + " (" + PlayerInput.CurrentProfile.InterfaceDeadzoneX.ToString("P1") + ")"), (Func<float>) (() => PlayerInput.CurrentProfile.InterfaceDeadzoneX), (Action<float>) (f => PlayerInput.CurrentProfile.InterfaceDeadzoneX = f), (Action) (() => PlayerInput.CurrentProfile.InterfaceDeadzoneX = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.0f, 0.95f, 0.35f, 0.35f)), 1001, color);
        case "sp5":
          return (UIElement) new UIKeybindingSliderItem((Func<string>) (() => Lang.menu[201].Value + " (" + PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX.ToString("P1") + ")"), (Func<float>) (() => PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX), (Action<float>) (f => PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX = f), (Action) (() => PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX, 0.0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f)), 1002, color);
        case "sp6":
          return (UIElement) new UIKeybindingSliderItem((Func<string>) (() => Lang.menu[202].Value + " (" + PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY.ToString("P1") + ")"), (Func<float>) (() => PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY), (Action<float>) (f => PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY = f), (Action) (() => PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY, 0.0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f)), 1003, color);
        case "sp7":
          return (UIElement) new UIKeybindingSliderItem((Func<string>) (() => Lang.menu[203].Value + " (" + PlayerInput.CurrentProfile.RightThumbstickDeadzoneX.ToString("P1") + ")"), (Func<float>) (() => PlayerInput.CurrentProfile.RightThumbstickDeadzoneX), (Action<float>) (f => PlayerInput.CurrentProfile.RightThumbstickDeadzoneX = f), (Action) (() => PlayerInput.CurrentProfile.RightThumbstickDeadzoneX = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.RightThumbstickDeadzoneX, 0.0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f)), 1004, color);
        case "sp8":
          return (UIElement) new UIKeybindingSliderItem((Func<string>) (() => Lang.menu[204].Value + " (" + PlayerInput.CurrentProfile.RightThumbstickDeadzoneY.ToString("P1") + ")"), (Func<float>) (() => PlayerInput.CurrentProfile.RightThumbstickDeadzoneY), (Action<float>) (f => PlayerInput.CurrentProfile.RightThumbstickDeadzoneY = f), (Action) (() => PlayerInput.CurrentProfile.RightThumbstickDeadzoneY = UILinksInitializer.HandleSliderHorizontalInput(PlayerInput.CurrentProfile.RightThumbstickDeadzoneY, 0.0f, 0.95f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f)), 1005, color);
        case "sp9":
          UIKeybindingSimpleListItem panel11 = new UIKeybindingSimpleListItem((Func<string>) (() => Lang.menu[86].Value), color);
          panel11.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
          {
            string copyableProfileName = UIManageControls.GetCopyableProfileName();
            PlayerInput.CurrentProfile.CopyGameplaySettingsFrom(PlayerInput.OriginalProfiles[copyableProfileName], currentInputMode);
          });
          return (UIElement) panel11;
        default:
          return (UIElement) new UIKeybindingListItem(bind, currentInputMode, color);
      }
    }

    public override void OnActivate()
    {
      if (Main.gameMenu)
      {
        this._outerContainer.Top.Set(220f, 0.0f);
        this._outerContainer.Height.Set(-220f, 1f);
      }
      else
      {
        this._outerContainer.Top.Set(120f, 0.0f);
        this._outerContainer.Height.Set(-120f, 1f);
      }
      if (!PlayerInput.UsingGamepadUI)
        return;
      UILinkPointNavigator.ChangePoint(3002);
    }

    private static string GetCopyableProfileName()
    {
      string copyableProfileName = "Redigit's Pick";
      if (PlayerInput.OriginalProfiles.ContainsKey(PlayerInput.CurrentProfile.Name))
        copyableProfileName = PlayerInput.CurrentProfile.Name;
      return copyableProfileName;
    }

    private void FillList()
    {
      List<UIElement> uiElementList = this._bindsKeyboard;
      if (!this.OnKeyboard)
        uiElementList = this._bindsGamepad;
      if (!this.OnGameplay)
        uiElementList = this.OnKeyboard ? this._bindsKeyboardUI : this._bindsGamepadUI;
      this._uilist.Clear();
      foreach (UIElement uiElement in uiElementList)
        this._uilist.Add(uiElement);
    }

    private void SnapButtonClick(UIMouseEvent evt, UIElement listeningElement)
    {
      if (!PlayerInput.CurrentProfile.AllowEditting)
        return;
      SoundEngine.PlaySound(12);
      List<string> keyStatu1 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"];
      Buttons buttons = Buttons.DPadUp;
      string str1 = buttons.ToString();
      if (keyStatu1.Contains(str1))
      {
        List<string> keyStatu2 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"];
        buttons = Buttons.DPadRight;
        string str2 = buttons.ToString();
        if (keyStatu2.Contains(str2))
        {
          List<string> keyStatu3 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"];
          buttons = Buttons.DPadDown;
          string str3 = buttons.ToString();
          if (keyStatu3.Contains(str3))
          {
            List<string> keyStatu4 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"];
            buttons = Buttons.DPadLeft;
            string str4 = buttons.ToString();
            if (keyStatu4.Contains(str4))
            {
              List<string> keyStatu5 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"];
              buttons = Buttons.DPadUp;
              string str5 = buttons.ToString();
              if (keyStatu5.Contains(str5))
              {
                List<string> keyStatu6 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"];
                buttons = Buttons.DPadRight;
                string str6 = buttons.ToString();
                if (keyStatu6.Contains(str6))
                {
                  List<string> keyStatu7 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"];
                  buttons = Buttons.DPadDown;
                  string str7 = buttons.ToString();
                  if (keyStatu7.Contains(str7))
                  {
                    List<string> keyStatu8 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"];
                    buttons = Buttons.DPadLeft;
                    string str8 = buttons.ToString();
                    if (keyStatu8.Contains(str8))
                    {
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Clear();
                      return;
                    }
                  }
                }
              }
            }
          }
        }
      }
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Clear();
      Dictionary<string, List<string>> keyStatus1 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
      List<string> stringList1 = new List<string>();
      buttons = Buttons.DPadUp;
      stringList1.Add(buttons.ToString());
      keyStatus1["DpadSnap1"] = stringList1;
      Dictionary<string, List<string>> keyStatus2 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
      List<string> stringList2 = new List<string>();
      buttons = Buttons.DPadRight;
      stringList2.Add(buttons.ToString());
      keyStatus2["DpadSnap2"] = stringList2;
      Dictionary<string, List<string>> keyStatus3 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
      List<string> stringList3 = new List<string>();
      buttons = Buttons.DPadDown;
      stringList3.Add(buttons.ToString());
      keyStatus3["DpadSnap3"] = stringList3;
      Dictionary<string, List<string>> keyStatus4 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
      List<string> stringList4 = new List<string>();
      buttons = Buttons.DPadLeft;
      stringList4.Add(buttons.ToString());
      keyStatus4["DpadSnap4"] = stringList4;
      Dictionary<string, List<string>> keyStatus5 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
      List<string> stringList5 = new List<string>();
      buttons = Buttons.DPadUp;
      stringList5.Add(buttons.ToString());
      keyStatus5["DpadSnap1"] = stringList5;
      Dictionary<string, List<string>> keyStatus6 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
      List<string> stringList6 = new List<string>();
      buttons = Buttons.DPadRight;
      stringList6.Add(buttons.ToString());
      keyStatus6["DpadSnap2"] = stringList6;
      Dictionary<string, List<string>> keyStatus7 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
      List<string> stringList7 = new List<string>();
      buttons = Buttons.DPadDown;
      stringList7.Add(buttons.ToString());
      keyStatus7["DpadSnap3"] = stringList7;
      Dictionary<string, List<string>> keyStatus8 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
      List<string> stringList8 = new List<string>();
      buttons = Buttons.DPadLeft;
      stringList8.Add(buttons.ToString());
      keyStatus8["DpadSnap4"] = stringList8;
    }

    private void RadialButtonClick(UIMouseEvent evt, UIElement listeningElement)
    {
      if (!PlayerInput.CurrentProfile.AllowEditting)
        return;
      SoundEngine.PlaySound(12);
      List<string> keyStatu1 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"];
      Buttons buttons = Buttons.DPadUp;
      string str1 = buttons.ToString();
      if (keyStatu1.Contains(str1))
      {
        List<string> keyStatu2 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"];
        buttons = Buttons.DPadRight;
        string str2 = buttons.ToString();
        if (keyStatu2.Contains(str2))
        {
          List<string> keyStatu3 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"];
          buttons = Buttons.DPadDown;
          string str3 = buttons.ToString();
          if (keyStatu3.Contains(str3))
          {
            List<string> keyStatu4 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"];
            buttons = Buttons.DPadLeft;
            string str4 = buttons.ToString();
            if (keyStatu4.Contains(str4))
            {
              List<string> keyStatu5 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"];
              buttons = Buttons.DPadUp;
              string str5 = buttons.ToString();
              if (keyStatu5.Contains(str5))
              {
                List<string> keyStatu6 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"];
                buttons = Buttons.DPadRight;
                string str6 = buttons.ToString();
                if (keyStatu6.Contains(str6))
                {
                  List<string> keyStatu7 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"];
                  buttons = Buttons.DPadDown;
                  string str7 = buttons.ToString();
                  if (keyStatu7.Contains(str7))
                  {
                    List<string> keyStatu8 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"];
                    buttons = Buttons.DPadLeft;
                    string str8 = buttons.ToString();
                    if (keyStatu8.Contains(str8))
                    {
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Clear();
                      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Clear();
                      return;
                    }
                  }
                }
              }
            }
          }
        }
      }
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Clear();
      PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Clear();
      Dictionary<string, List<string>> keyStatus1 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
      List<string> stringList1 = new List<string>();
      buttons = Buttons.DPadUp;
      stringList1.Add(buttons.ToString());
      keyStatus1["DpadRadial1"] = stringList1;
      Dictionary<string, List<string>> keyStatus2 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
      List<string> stringList2 = new List<string>();
      buttons = Buttons.DPadRight;
      stringList2.Add(buttons.ToString());
      keyStatus2["DpadRadial2"] = stringList2;
      Dictionary<string, List<string>> keyStatus3 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
      List<string> stringList3 = new List<string>();
      buttons = Buttons.DPadDown;
      stringList3.Add(buttons.ToString());
      keyStatus3["DpadRadial3"] = stringList3;
      Dictionary<string, List<string>> keyStatus4 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus;
      List<string> stringList4 = new List<string>();
      buttons = Buttons.DPadLeft;
      stringList4.Add(buttons.ToString());
      keyStatus4["DpadRadial4"] = stringList4;
      Dictionary<string, List<string>> keyStatus5 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
      List<string> stringList5 = new List<string>();
      buttons = Buttons.DPadUp;
      stringList5.Add(buttons.ToString());
      keyStatus5["DpadRadial1"] = stringList5;
      Dictionary<string, List<string>> keyStatus6 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
      List<string> stringList6 = new List<string>();
      buttons = Buttons.DPadRight;
      stringList6.Add(buttons.ToString());
      keyStatus6["DpadRadial2"] = stringList6;
      Dictionary<string, List<string>> keyStatus7 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
      List<string> stringList7 = new List<string>();
      buttons = Buttons.DPadDown;
      stringList7.Add(buttons.ToString());
      keyStatus7["DpadRadial3"] = stringList7;
      Dictionary<string, List<string>> keyStatus8 = PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus;
      List<string> stringList8 = new List<string>();
      buttons = Buttons.DPadLeft;
      stringList8.Add(buttons.ToString());
      keyStatus8["DpadRadial4"] = stringList8;
    }

    private void KeyboardButtonClick(UIMouseEvent evt, UIElement listeningElement)
    {
      this._buttonKeyboard.SetFrame(this._KeyboardGamepadTexture.Frame(2, 2));
      this._buttonGamepad.SetFrame(this._KeyboardGamepadTexture.Frame(2, 2, 1, 1));
      this.OnKeyboard = true;
      this.FillList();
    }

    private void GamepadButtonClick(UIMouseEvent evt, UIElement listeningElement)
    {
      this._buttonKeyboard.SetFrame(this._KeyboardGamepadTexture.Frame(2, 2, frameY: 1));
      this._buttonGamepad.SetFrame(this._KeyboardGamepadTexture.Frame(2, 2, 1));
      this.OnKeyboard = false;
      this.FillList();
    }

    private void ManageBorderKeyboardOn(UIMouseEvent evt, UIElement listeningElement)
    {
      this._buttonBorder2.Color = !this.OnKeyboard ? Color.Silver : Color.Black;
      this._buttonBorder1.Color = Main.OurFavoriteColor;
    }

    private void ManageBorderKeyboardOff(UIMouseEvent evt, UIElement listeningElement)
    {
      this._buttonBorder2.Color = !this.OnKeyboard ? Color.Silver : Color.Black;
      this._buttonBorder1.Color = this.OnKeyboard ? Color.Silver : Color.Black;
    }

    private void ManageBorderGamepadOn(UIMouseEvent evt, UIElement listeningElement)
    {
      this._buttonBorder1.Color = this.OnKeyboard ? Color.Silver : Color.Black;
      this._buttonBorder2.Color = Main.OurFavoriteColor;
    }

    private void ManageBorderGamepadOff(UIMouseEvent evt, UIElement listeningElement)
    {
      this._buttonBorder1.Color = this.OnKeyboard ? Color.Silver : Color.Black;
      this._buttonBorder2.Color = !this.OnKeyboard ? Color.Silver : Color.Black;
    }

    private void VsGameplayButtonClick(UIMouseEvent evt, UIElement listeningElement)
    {
      this._buttonVs1.SetFrame(this._GameplayVsUITexture.Frame(2, 2));
      this._buttonVs2.SetFrame(this._GameplayVsUITexture.Frame(2, 2, 1, 1));
      this.OnGameplay = true;
      this.FillList();
    }

    private void VsMenuButtonClick(UIMouseEvent evt, UIElement listeningElement)
    {
      this._buttonVs1.SetFrame(this._GameplayVsUITexture.Frame(2, 2, frameY: 1));
      this._buttonVs2.SetFrame(this._GameplayVsUITexture.Frame(2, 2, 1));
      this.OnGameplay = false;
      this.FillList();
    }

    private void ManageBorderGameplayOn(UIMouseEvent evt, UIElement listeningElement)
    {
      this._buttonBorderVs2.Color = !this.OnGameplay ? Color.Silver : Color.Black;
      this._buttonBorderVs1.Color = Main.OurFavoriteColor;
    }

    private void ManageBorderGameplayOff(UIMouseEvent evt, UIElement listeningElement)
    {
      this._buttonBorderVs2.Color = !this.OnGameplay ? Color.Silver : Color.Black;
      this._buttonBorderVs1.Color = this.OnGameplay ? Color.Silver : Color.Black;
    }

    private void ManageBorderMenuOn(UIMouseEvent evt, UIElement listeningElement)
    {
      this._buttonBorderVs1.Color = this.OnGameplay ? Color.Silver : Color.Black;
      this._buttonBorderVs2.Color = Main.OurFavoriteColor;
    }

    private void ManageBorderMenuOff(UIMouseEvent evt, UIElement listeningElement)
    {
      this._buttonBorderVs1.Color = this.OnGameplay ? Color.Silver : Color.Black;
      this._buttonBorderVs2.Color = !this.OnGameplay ? Color.Silver : Color.Black;
    }

    private void profileButtonClick(UIMouseEvent evt, UIElement listeningElement)
    {
      string name = PlayerInput.CurrentProfile.Name;
      List<string> list = PlayerInput.Profiles.Keys.ToList<string>();
      int index = list.IndexOf(name) + 1;
      if (index >= list.Count)
        index -= list.Count;
      PlayerInput.SetSelectedProfile(list[index]);
    }

    private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
    {
      SoundEngine.PlaySound(12);
      ((UIPanel) evt.Target).BackgroundColor = new Color(73, 94, 171);
      ((UIPanel) evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
    }

    private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
    {
      ((UIPanel) evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
      ((UIPanel) evt.Target).BorderColor = Color.Black;
    }

    private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
    {
      Main.menuMode = 1127;
      IngameFancyUI.Close();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
      this.SetupGamepadPoints(spriteBatch);
    }

    private void SetupGamepadPoints(SpriteBatch spriteBatch)
    {
      UILinkPointNavigator.Shortcuts.BackButtonCommand = 4;
      int num1 = 3000;
      UILinkPointNavigator.SetPosition(num1, this._buttonBack.GetInnerDimensions().ToRectangle().Center.ToVector2());
      UILinkPointNavigator.SetPosition(num1 + 1, this._buttonKeyboard.GetInnerDimensions().ToRectangle().Center.ToVector2());
      UILinkPointNavigator.SetPosition(num1 + 2, this._buttonGamepad.GetInnerDimensions().ToRectangle().Center.ToVector2());
      UILinkPointNavigator.SetPosition(num1 + 3, this._buttonProfile.GetInnerDimensions().ToRectangle().Center.ToVector2());
      UILinkPointNavigator.SetPosition(num1 + 4, this._buttonVs1.GetInnerDimensions().ToRectangle().Center.ToVector2());
      UILinkPointNavigator.SetPosition(num1 + 5, this._buttonVs2.GetInnerDimensions().ToRectangle().Center.ToVector2());
      int key1 = num1;
      UILinkPoint point1 = UILinkPointNavigator.Points[key1];
      point1.Unlink();
      point1.Up = num1 + 6;
      int key2 = num1 + 1;
      UILinkPoint point2 = UILinkPointNavigator.Points[key2];
      point2.Unlink();
      point2.Right = num1 + 2;
      point2.Down = num1 + 6;
      int key3 = num1 + 2;
      UILinkPoint point3 = UILinkPointNavigator.Points[key3];
      point3.Unlink();
      point3.Left = num1 + 1;
      point3.Right = num1 + 4;
      point3.Down = num1 + 6;
      int key4 = num1 + 4;
      UILinkPoint point4 = UILinkPointNavigator.Points[key4];
      point4.Unlink();
      point4.Left = num1 + 2;
      point4.Right = num1 + 5;
      point4.Down = num1 + 6;
      int key5 = num1 + 5;
      UILinkPoint point5 = UILinkPointNavigator.Points[key5];
      point5.Unlink();
      point5.Left = num1 + 4;
      point5.Right = num1 + 3;
      point5.Down = num1 + 6;
      int key6 = num1 + 3;
      UILinkPoint point6 = UILinkPointNavigator.Points[key6];
      point6.Unlink();
      point6.Left = num1 + 5;
      point6.Down = num1 + 6;
      float num2 = 1f / Main.UIScale;
      Rectangle clippingRectangle = this._uilist.GetClippingRectangle(spriteBatch);
      Vector2 minimum = clippingRectangle.TopLeft() * num2;
      Vector2 maximum = clippingRectangle.BottomRight() * num2;
      List<SnapPoint> snapPoints = this._uilist.GetSnapPoints();
      for (int index = 0; index < snapPoints.Count; ++index)
      {
        if (!snapPoints[index].Position.Between(minimum, maximum))
        {
          Vector2 position = snapPoints[index].Position;
          snapPoints.Remove(snapPoints[index]);
          --index;
        }
      }
      snapPoints.Sort((Comparison<SnapPoint>) ((x, y) => x.Id.CompareTo(y.Id)));
      for (int index = 0; index < snapPoints.Count; ++index)
      {
        int num3 = num1 + 6 + index;
        if (snapPoints[index].Name == "Thin")
        {
          UILinkPoint point7 = UILinkPointNavigator.Points[num3];
          point7.Unlink();
          UILinkPointNavigator.SetPosition(num3, snapPoints[index].Position);
          point7.Right = num3 + 1;
          point7.Down = index < snapPoints.Count - 2 ? num3 + 2 : num1;
          point7.Up = index < 2 ? num1 + 1 : (snapPoints[index - 1].Name == "Wide" ? num3 - 1 : num3 - 2);
          UILinkPointNavigator.Points[num1].Up = num3;
          UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num3;
          ++index;
          if (index < snapPoints.Count)
          {
            int num4 = num1 + 6 + index;
            UILinkPoint point8 = UILinkPointNavigator.Points[num4];
            point8.Unlink();
            UILinkPointNavigator.SetPosition(num4, snapPoints[index].Position);
            point8.Left = num4 - 1;
            point8.Down = index < snapPoints.Count - 1 ? (snapPoints[index + 1].Name == "Wide" ? num4 + 1 : num4 + 2) : num1;
            point8.Up = index < 2 ? num1 + 1 : num4 - 2;
            UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num4;
          }
        }
        else
        {
          UILinkPoint point9 = UILinkPointNavigator.Points[num3];
          point9.Unlink();
          UILinkPointNavigator.SetPosition(num3, snapPoints[index].Position);
          point9.Down = index < snapPoints.Count - 1 ? num3 + 1 : num1;
          point9.Up = index < 1 ? num1 + 1 : (snapPoints[index - 1].Name == "Wide" ? num3 - 1 : num3 - 2);
          UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num3;
          UILinkPointNavigator.Points[num1].Up = num3;
        }
      }
      if (UIManageControls.ForceMoveTo == -1)
        return;
      UILinkPointNavigator.ChangePoint((int) MathHelper.Clamp((float) UIManageControls.ForceMoveTo, (float) num1, (float) UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX));
      UIManageControls.ForceMoveTo = -1;
    }
  }
}
