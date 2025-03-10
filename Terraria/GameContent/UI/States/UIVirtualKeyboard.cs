﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.States.UIVirtualKeyboard
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
  public class UIVirtualKeyboard : UIState
  {
    private static UIVirtualKeyboard _currentInstance;
    private static string _cancelCacheSign = "";
    private static string _cancelCacheChest = "";
    private const string DEFAULT_KEYS = "1234567890qwertyuiopasdfghjkl'zxcvbnm,.?";
    private const string SHIFT_KEYS = "1234567890QWERTYUIOPASDFGHJKL'ZXCVBNM,.?";
    private const string SYMBOL_KEYS = "1234567890!@#$%^&*()-_+=/\\{}[]<>;:\"`|~£¥";
    private const float KEY_SPACING = 4f;
    private const float KEY_WIDTH = 48f;
    private const float KEY_HEIGHT = 37f;
    private UITextPanel<object>[] _keyList = new UITextPanel<object>[50];
    private UITextPanel<object> _shiftButton;
    private UITextPanel<object> _symbolButton;
    private UITextBox _textBox;
    private UITextPanel<LocalizedText> _submitButton;
    private UITextPanel<LocalizedText> _cancelButton;
    private UIText _label;
    private UITextPanel<object> _enterButton;
    private UITextPanel<object> _spacebarButton;
    private UITextPanel<object> _restoreButton;
    private Asset<Texture2D> _textureShift;
    private Asset<Texture2D> _textureBackspace;
    private Color _internalBorderColor = new Color(89, 116, 213);
    private Color _internalBorderColorSelected = Main.OurFavoriteColor;
    private UITextPanel<LocalizedText> _submitButton2;
    private UITextPanel<LocalizedText> _cancelButton2;
    private UIElement outerLayer1;
    private UIElement outerLayer2;
    private bool _allowEmpty;
    private UIVirtualKeyboard.KeyState _keyState;
    private UIVirtualKeyboard.KeyboardSubmitEvent _submitAction;
    private Action _cancelAction;
    private int _lastOffsetDown;
    public static int OffsetDown;
    public static bool ShouldHideText;
    private int _keyboardContext;
    private bool _edittingSign;
    private bool _edittingChest;
    private float _textBoxHeight;
    private float _labelHeight;
    public Func<string, bool> CustomTextValidationForUpdate;
    public Func<string, bool> CustomTextValidationForSubmit;
    public Func<bool> CustomEscapeAttempt;
    private bool _canSubmit;

    public string Text
    {
      get => this._textBox.Text;
      set
      {
        this._textBox.SetText(value);
        this.ValidateText();
      }
    }

    public bool HideContents
    {
      get => this._textBox.HideContents;
      set => this._textBox.HideContents = value;
    }

    public UIVirtualKeyboard(
      string labelText,
      string startingText,
      UIVirtualKeyboard.KeyboardSubmitEvent submitAction,
      Action cancelAction,
      int inputMode = 0,
      bool allowEmpty = false)
    {
      this._keyboardContext = inputMode;
      this._allowEmpty = allowEmpty;
      UIVirtualKeyboard.OffsetDown = 0;
      UIVirtualKeyboard.ShouldHideText = false;
      this._lastOffsetDown = 0;
      this._edittingSign = this._keyboardContext == 1;
      this._edittingChest = this._keyboardContext == 2;
      UIVirtualKeyboard._currentInstance = this;
      this._submitAction = submitAction;
      this._cancelAction = cancelAction;
      this._textureShift = Main.Assets.Request<Texture2D>("Images/UI/VK_Shift", (AssetRequestMode) 1);
      this._textureBackspace = Main.Assets.Request<Texture2D>("Images/UI/VK_Backspace", (AssetRequestMode) 1);
      this.Top.Pixels = (float) this._lastOffsetDown;
      float num1 = (float) (-5000 * this._edittingSign.ToInt());
      float num2 = 270f;
      float num3 = 0.0f;
      float num4 = 516f;
      UIElement element1 = new UIElement();
      element1.Width.Pixels = (float) ((double) num4 + 8.0 + 16.0);
      element1.Top.Precent = num3;
      element1.Top.Pixels = num2;
      element1.Height.Pixels = 266f;
      element1.HAlign = 0.5f;
      element1.SetPadding(0.0f);
      this.outerLayer1 = element1;
      UIElement uiElement = new UIElement();
      uiElement.Width.Pixels = (float) ((double) num4 + 8.0 + 16.0);
      uiElement.Top.Precent = num3;
      uiElement.Top.Pixels = num2;
      uiElement.Height.Pixels = 266f;
      uiElement.HAlign = 0.5f;
      uiElement.SetPadding(0.0f);
      this.outerLayer2 = uiElement;
      UIPanel uiPanel = new UIPanel();
      uiPanel.Width.Precent = 1f;
      uiPanel.Height.Pixels = 225f;
      uiPanel.BackgroundColor = new Color(23, 33, 69) * 0.7f;
      element1.Append((UIElement) uiPanel);
      float num5 = -50f;
      this._textBox = new UITextBox("", 0.78f, true);
      this._textBox.BackgroundColor = Color.Transparent;
      this._textBox.BorderColor = Color.Transparent;
      this._textBox.HAlign = 0.5f;
      this._textBox.Width.Pixels = num4;
      this._textBox.Top.Pixels = (float) ((double) num5 + (double) num2 - 10.0) + num1;
      this._textBox.Top.Precent = num3;
      this._textBox.Height.Pixels = 37f;
      this.Append((UIElement) this._textBox);
      for (int x = 0; x < 10; ++x)
      {
        for (int y = 0; y < 4; ++y)
        {
          UITextPanel<object> keyboardButton = this.CreateKeyboardButton((object) "1234567890qwertyuiopasdfghjkl'zxcvbnm,.?"[y * 10 + x].ToString(), x, y);
          keyboardButton.OnClick += new UIElement.MouseEvent(this.TypeText);
          uiPanel.Append((UIElement) keyboardButton);
        }
      }
      this._shiftButton = this.CreateKeyboardButton((object) "", 0, 4, style: false);
      this._shiftButton.PaddingLeft = 0.0f;
      this._shiftButton.PaddingRight = 0.0f;
      this._shiftButton.PaddingBottom = this._shiftButton.PaddingTop = 0.0f;
      this._shiftButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
      this._shiftButton.BorderColor = this._internalBorderColor * 0.7f;
      this._shiftButton.OnMouseOver += (UIElement.MouseEvent) ((evt, listeningElement) =>
      {
        this._shiftButton.BorderColor = this._internalBorderColorSelected;
        if (this._keyState == UIVirtualKeyboard.KeyState.Shift)
          return;
        this._shiftButton.BackgroundColor = new Color(73, 94, 171);
      });
      this._shiftButton.OnMouseOut += (UIElement.MouseEvent) ((evt, listeningElement) =>
      {
        this._shiftButton.BorderColor = this._internalBorderColor * 0.7f;
        if (this._keyState == UIVirtualKeyboard.KeyState.Shift)
          return;
        this._shiftButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
      });
      this._shiftButton.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
      {
        SoundEngine.PlaySound(12);
        this.SetKeyState(this._keyState == UIVirtualKeyboard.KeyState.Shift ? UIVirtualKeyboard.KeyState.Default : UIVirtualKeyboard.KeyState.Shift);
      });
      UIImage element2 = new UIImage(this._textureShift);
      element2.HAlign = 0.5f;
      element2.VAlign = 0.5f;
      element2.ImageScale = 0.85f;
      this._shiftButton.Append((UIElement) element2);
      uiPanel.Append((UIElement) this._shiftButton);
      this._symbolButton = this.CreateKeyboardButton((object) "@%", 1, 4, style: false);
      this._symbolButton.PaddingLeft = 0.0f;
      this._symbolButton.PaddingRight = 0.0f;
      this._symbolButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
      this._symbolButton.BorderColor = this._internalBorderColor * 0.7f;
      this._symbolButton.OnMouseOver += (UIElement.MouseEvent) ((evt, listeningElement) =>
      {
        this._symbolButton.BorderColor = this._internalBorderColorSelected;
        if (this._keyState == UIVirtualKeyboard.KeyState.Symbol)
          return;
        this._symbolButton.BackgroundColor = new Color(73, 94, 171);
      });
      this._symbolButton.OnMouseOut += (UIElement.MouseEvent) ((evt, listeningElement) =>
      {
        this._symbolButton.BorderColor = this._internalBorderColor * 0.7f;
        if (this._keyState == UIVirtualKeyboard.KeyState.Symbol)
          return;
        this._symbolButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
      });
      this._symbolButton.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
      {
        SoundEngine.PlaySound(12);
        this.SetKeyState(this._keyState == UIVirtualKeyboard.KeyState.Symbol ? UIVirtualKeyboard.KeyState.Default : UIVirtualKeyboard.KeyState.Symbol);
      });
      uiPanel.Append((UIElement) this._symbolButton);
      this.BuildSpaceBarArea(uiPanel);
      this._submitButton = new UITextPanel<LocalizedText>(this._edittingSign || this._edittingChest ? Language.GetText("UI.Save") : Language.GetText("UI.Submit"), 0.4f, true);
      this._submitButton.Height.Pixels = 37f;
      this._submitButton.Width.Precent = 0.4f;
      this._submitButton.HAlign = 1f;
      this._submitButton.VAlign = 1f;
      this._submitButton.PaddingLeft = 0.0f;
      this._submitButton.PaddingRight = 0.0f;
      this.ValidateText();
      this._submitButton.OnMouseOver += new UIElement.MouseEvent(this.FadedMouseOver);
      this._submitButton.OnMouseOut += new UIElement.MouseEvent(this.FadedMouseOut);
      this._submitButton.OnMouseOver += (UIElement.MouseEvent) ((evt, listeningElement) => this.ValidateText());
      this._submitButton.OnMouseOut += (UIElement.MouseEvent) ((evt, listeningElement) => this.ValidateText());
      this._submitButton.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) => UIVirtualKeyboard.Submit());
      element1.Append((UIElement) this._submitButton);
      this._cancelButton = new UITextPanel<LocalizedText>(Language.GetText("UI.Cancel"), 0.4f, true);
      this.StyleKey<LocalizedText>(this._cancelButton, true);
      this._cancelButton.Height.Pixels = 37f;
      this._cancelButton.Width.Precent = 0.4f;
      this._cancelButton.VAlign = 1f;
      this._cancelButton.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
      {
        SoundEngine.PlaySound(11);
        this._cancelAction();
      });
      this._cancelButton.OnMouseOver += new UIElement.MouseEvent(this.FadedMouseOver);
      this._cancelButton.OnMouseOut += new UIElement.MouseEvent(this.FadedMouseOut);
      element1.Append((UIElement) this._cancelButton);
      this._submitButton2 = new UITextPanel<LocalizedText>(this._edittingSign || this._edittingChest ? Language.GetText("UI.Save") : Language.GetText("UI.Submit"), 0.72f, true);
      this._submitButton2.TextColor = Color.Silver;
      this._submitButton2.DrawPanel = false;
      this._submitButton2.Height.Pixels = 60f;
      this._submitButton2.Width.Precent = 0.4f;
      this._submitButton2.HAlign = 0.5f;
      this._submitButton2.VAlign = 0.0f;
      this._submitButton2.OnMouseOver += (UIElement.MouseEvent) ((a, b) =>
      {
        ((UITextPanel<LocalizedText>) b).TextScale = 0.85f;
        ((UITextPanel<LocalizedText>) b).TextColor = Color.White;
      });
      this._submitButton2.OnMouseOut += (UIElement.MouseEvent) ((a, b) =>
      {
        ((UITextPanel<LocalizedText>) b).TextScale = 0.72f;
        ((UITextPanel<LocalizedText>) b).TextColor = Color.Silver;
      });
      this._submitButton2.Top.Pixels = 50f;
      this._submitButton2.PaddingLeft = 0.0f;
      this._submitButton2.PaddingRight = 0.0f;
      this.ValidateText();
      this._submitButton2.OnMouseOver += (UIElement.MouseEvent) ((evt, listeningElement) => this.ValidateText());
      this._submitButton2.OnMouseOut += (UIElement.MouseEvent) ((evt, listeningElement) => this.ValidateText());
      this._submitButton2.OnMouseOver += new UIElement.MouseEvent(this.FadedMouseOver);
      this._submitButton2.OnMouseOut += new UIElement.MouseEvent(this.FadedMouseOut);
      this._submitButton2.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
      {
        if (!this.TextIsValidForSubmit())
          return;
        SoundEngine.PlaySound(10);
        this._submitAction(this.Text.Trim());
      });
      this.outerLayer2.Append((UIElement) this._submitButton2);
      this._cancelButton2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Cancel"), 0.72f, true);
      this._cancelButton2.TextColor = Color.Silver;
      this._cancelButton2.DrawPanel = false;
      this._cancelButton2.OnMouseOver += (UIElement.MouseEvent) ((a, b) =>
      {
        ((UITextPanel<LocalizedText>) b).TextScale = 0.85f;
        ((UITextPanel<LocalizedText>) b).TextColor = Color.White;
      });
      this._cancelButton2.OnMouseOut += (UIElement.MouseEvent) ((a, b) =>
      {
        ((UITextPanel<LocalizedText>) b).TextScale = 0.72f;
        ((UITextPanel<LocalizedText>) b).TextColor = Color.Silver;
      });
      this._cancelButton2.Height.Pixels = 60f;
      this._cancelButton2.Width.Precent = 0.4f;
      this._cancelButton2.Top.Pixels = 114f;
      this._cancelButton2.VAlign = 0.0f;
      this._cancelButton2.HAlign = 0.5f;
      this._cancelButton2.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
      {
        SoundEngine.PlaySound(11);
        this._cancelAction();
      });
      this.outerLayer2.Append((UIElement) this._cancelButton2);
      UITextPanel<object> keyboardButton1 = this.CreateKeyboardButton((object) "", 8, 4, 2);
      keyboardButton1.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
      {
        SoundEngine.PlaySound(12);
        this._textBox.Backspace();
        this.ValidateText();
      });
      keyboardButton1.PaddingLeft = 0.0f;
      keyboardButton1.PaddingRight = 0.0f;
      keyboardButton1.PaddingBottom = keyboardButton1.PaddingTop = 0.0f;
      UIImage element3 = new UIImage(this._textureBackspace);
      element3.HAlign = 0.5f;
      element3.VAlign = 0.5f;
      element3.ImageScale = 0.92f;
      keyboardButton1.Append((UIElement) element3);
      uiPanel.Append((UIElement) keyboardButton1);
      UIText element4 = new UIText(labelText, 0.75f, true);
      element4.HAlign = 0.5f;
      element4.Width.Pixels = num4;
      element4.Top.Pixels = (float) ((double) num5 - 37.0 - 4.0) + num2 + num1;
      element4.Top.Precent = num3;
      element4.Height.Pixels = 37f;
      this.Append((UIElement) element4);
      this._label = element4;
      this.Append(element1);
      this._textBox.SetTextMaxLength(this._edittingSign ? 1200 : 20);
      this.Text = startingText;
      if (this.Text.Length == 0)
        this.SetKeyState(UIVirtualKeyboard.KeyState.Shift);
      UIVirtualKeyboard.ShouldHideText = true;
      UIVirtualKeyboard.OffsetDown = 9999;
      this.UpdateOffsetDown();
    }

    public void SetMaxInputLength(int length) => this._textBox.SetTextMaxLength(length);

    private void BuildSpaceBarArea(UIPanel mainPanel)
    {
      Action createTheseTwo = (Action) (() =>
      {
        bool flag = this.CanRestore();
        int x = flag ? 4 : 5;
        bool edittingSign = this._edittingSign;
        int width = flag & edittingSign ? 2 : 3;
        UITextPanel<object> keyboardButton1 = this.CreateKeyboardButton((object) Language.GetText("UI.SpaceButton"), 2, 4, this._edittingSign || this._edittingChest & flag ? width : 6);
        keyboardButton1.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) => this.PressSpace());
        mainPanel.Append((UIElement) keyboardButton1);
        this._spacebarButton = keyboardButton1;
        if (!edittingSign)
          return;
        UITextPanel<object> keyboardButton2 = this.CreateKeyboardButton((object) Language.GetText("UI.EnterButton"), x, 4, width);
        keyboardButton2.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
        {
          SoundEngine.PlaySound(12);
          this._textBox.Write("\n");
          this.ValidateText();
        });
        mainPanel.Append((UIElement) keyboardButton2);
        this._enterButton = keyboardButton2;
      });
      createTheseTwo();
      if (!this.CanRestore())
        return;
      UITextPanel<object> restoreBar = this.CreateKeyboardButton((object) Language.GetText("UI.RestoreButton"), 6, 4, 2);
      restoreBar.OnClick += (UIElement.MouseEvent) ((evt, listeningElement) =>
      {
        SoundEngine.PlaySound(12);
        this.RestoreCancelledInput(this._keyboardContext);
        this.ValidateText();
        restoreBar.Remove();
        this._enterButton.Remove();
        this._spacebarButton.Remove();
        createTheseTwo();
      });
      mainPanel.Append((UIElement) restoreBar);
      this._restoreButton = restoreBar;
    }

    private void PressSpace()
    {
      string text = " ";
      if (this.CustomTextValidationForUpdate != null && !this.CustomTextValidationForUpdate(this.Text + text))
      {
        SoundEngine.PlaySound(11);
      }
      else
      {
        SoundEngine.PlaySound(12);
        this._textBox.Write(text);
        this.ValidateText();
      }
    }

    private bool CanRestore()
    {
      if (this._edittingSign)
        return UIVirtualKeyboard._cancelCacheSign.Length > 0;
      return this._edittingChest && UIVirtualKeyboard._cancelCacheChest.Length > 0;
    }

    private void TypeText(UIMouseEvent evt, UIElement listeningElement)
    {
      string text = ((UITextPanel<object>) listeningElement).Text;
      if (this.CustomTextValidationForUpdate != null && !this.CustomTextValidationForUpdate(this.Text + text))
      {
        SoundEngine.PlaySound(11);
      }
      else
      {
        SoundEngine.PlaySound(12);
        int num = this.Text.Length == 0 ? 1 : 0;
        this._textBox.Write(text);
        this.ValidateText();
        if (num == 0 || this.Text.Length <= 0 || this._keyState != UIVirtualKeyboard.KeyState.Shift)
          return;
        this.SetKeyState(UIVirtualKeyboard.KeyState.Default);
      }
    }

    public void SetKeyState(UIVirtualKeyboard.KeyState keyState)
    {
      UITextPanel<object> uiTextPanel1 = (UITextPanel<object>) null;
      switch (this._keyState)
      {
        case UIVirtualKeyboard.KeyState.Symbol:
          uiTextPanel1 = this._symbolButton;
          break;
        case UIVirtualKeyboard.KeyState.Shift:
          uiTextPanel1 = this._shiftButton;
          break;
      }
      if (uiTextPanel1 != null)
      {
        if (uiTextPanel1.IsMouseHovering)
          uiTextPanel1.BackgroundColor = new Color(73, 94, 171);
        else
          uiTextPanel1.BackgroundColor = new Color(63, 82, 151) * 0.7f;
      }
      string str = (string) null;
      UITextPanel<object> uiTextPanel2 = (UITextPanel<object>) null;
      switch (keyState)
      {
        case UIVirtualKeyboard.KeyState.Default:
          str = "1234567890qwertyuiopasdfghjkl'zxcvbnm,.?";
          break;
        case UIVirtualKeyboard.KeyState.Symbol:
          str = "1234567890!@#$%^&*()-_+=/\\{}[]<>;:\"`|~£¥";
          uiTextPanel2 = this._symbolButton;
          break;
        case UIVirtualKeyboard.KeyState.Shift:
          str = "1234567890QWERTYUIOPASDFGHJKL'ZXCVBNM,.?";
          uiTextPanel2 = this._shiftButton;
          break;
      }
      for (int index = 0; index < str.Length; ++index)
        this._keyList[index].SetText((object) str[index].ToString());
      this._keyState = keyState;
      if (uiTextPanel2 == null)
        return;
      uiTextPanel2.BackgroundColor = new Color(93, 114, 191);
    }

    private void ValidateText()
    {
      if (this.TextIsValidForSubmit())
      {
        this._canSubmit = true;
        this._submitButton.TextColor = Color.White;
        if (this._submitButton.IsMouseHovering)
          this._submitButton.BackgroundColor = new Color(73, 94, 171);
        else
          this._submitButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
      }
      else
      {
        this._canSubmit = false;
        this._submitButton.TextColor = Color.Gray;
        if (this._submitButton.IsMouseHovering)
          this._submitButton.BackgroundColor = new Color(180, 60, 60) * 0.85f;
        else
          this._submitButton.BackgroundColor = new Color(150, 40, 40) * 0.85f;
      }
    }

    private bool TextIsValidForSubmit()
    {
      if (this.CustomTextValidationForUpdate != null)
        return this.CustomTextValidationForUpdate(this.Text);
      return this.Text.Trim().Length > 0 || this._edittingSign || this._edittingChest || this._allowEmpty;
    }

    private void StyleKey<T>(UITextPanel<T> button, bool external = false)
    {
      button.PaddingLeft = 0.0f;
      button.PaddingRight = 0.0f;
      button.BackgroundColor = new Color(63, 82, 151) * 0.7f;
      if (!external)
        button.BorderColor = this._internalBorderColor * 0.7f;
      button.OnMouseOver += (UIElement.MouseEvent) ((evt, listeningElement) =>
      {
        ((UIPanel) listeningElement).BackgroundColor = new Color(73, 94, 171) * 0.85f;
        if (external)
          return;
        ((UIPanel) listeningElement).BorderColor = this._internalBorderColorSelected * 0.85f;
      });
      button.OnMouseOut += (UIElement.MouseEvent) ((evt, listeningElement) =>
      {
        ((UIPanel) listeningElement).BackgroundColor = new Color(63, 82, 151) * 0.7f;
        if (external)
          return;
        ((UIPanel) listeningElement).BorderColor = this._internalBorderColor * 0.7f;
      });
    }

    private UITextPanel<object> CreateKeyboardButton(
      object text,
      int x,
      int y,
      int width = 1,
      bool style = true)
    {
      float num = 516f;
      UITextPanel<object> button = new UITextPanel<object>(text, 0.4f, true);
      button.Width.Pixels = (float) (48.0 * (double) width + 4.0 * (double) (width - 1));
      button.Height.Pixels = 37f;
      button.Left.Precent = 0.5f;
      button.Left.Pixels = (float) (52.0 * (double) x - (double) num * 0.5);
      button.Top.Pixels = 41f * (float) y;
      if (style)
        this.StyleKey<object>(button);
      for (int index = 0; index < width; ++index)
        this._keyList[y * 10 + x + index] = button;
      return button;
    }

    private bool ShouldShowKeyboard() => PlayerInput.SettingsForUI.ShowGamepadHints;

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
      if (Main.gameMenu)
      {
        if (this.ShouldShowKeyboard())
        {
          this.outerLayer2.Remove();
          if (!this.Elements.Contains(this.outerLayer1))
            this.Append(this.outerLayer1);
          this.outerLayer1.Activate();
          this.outerLayer2.Deactivate();
          this.Recalculate();
          this.RecalculateChildren();
          if ((double) this._labelHeight != 0.0)
          {
            this._textBox.Top.Pixels = this._textBoxHeight;
            this._label.Top.Pixels = this._labelHeight;
            this._textBox.Recalculate();
            this._label.Recalculate();
            this._labelHeight = this._textBoxHeight = 0.0f;
            UserInterface.ActiveInstance.ResetLasts();
          }
        }
        else
        {
          this.outerLayer1.Remove();
          if (!this.Elements.Contains(this.outerLayer2))
            this.Append(this.outerLayer2);
          this.outerLayer2.Activate();
          this.outerLayer1.Deactivate();
          this.Recalculate();
          this.RecalculateChildren();
          if ((double) this._textBoxHeight == 0.0)
          {
            this._textBoxHeight = this._textBox.Top.Pixels;
            this._labelHeight = this._label.Top.Pixels;
            this._textBox.Top.Pixels += 50f;
            this._label.Top.Pixels += 50f;
            this._textBox.Recalculate();
            this._label.Recalculate();
            UserInterface.ActiveInstance.ResetLasts();
          }
        }
      }
      if (!Main.editSign && this._edittingSign)
        IngameFancyUI.Close();
      else if (!Main.editChest && this._edittingChest)
      {
        IngameFancyUI.Close();
      }
      else
      {
        base.DrawSelf(spriteBatch);
        this.UpdateOffsetDown();
        UIVirtualKeyboard.OffsetDown = 0;
        UIVirtualKeyboard.ShouldHideText = false;
        this.SetupGamepadPoints(spriteBatch);
        PlayerInput.WritingText = true;
        Main.instance.HandleIME();
        Vector2 position;
        ref Vector2 local1 = ref position;
        double x1 = (double) (Main.screenWidth / 2);
        CalculatedStyle dimensions = this._textBox.GetDimensions();
        double y1 = (double) (dimensions.ToRectangle().Bottom + 32);
        local1 = new Vector2((float) x1, (float) y1);
        Main.instance.DrawWindowsIMEPanel(position, 0.5f);
        string inputText = Main.GetInputText(this.Text, this._edittingSign);
        if (this._edittingSign && Main.inputTextEnter)
        {
          inputText += "\n";
        }
        else
        {
          if (this._edittingChest && Main.inputTextEnter)
          {
            ChestUI.RenameChestSubmit(Main.player[Main.myPlayer]);
            IngameFancyUI.Close();
            return;
          }
          if (Main.inputTextEnter && UIVirtualKeyboard.CanSubmit)
            UIVirtualKeyboard.Submit();
          else if (this._edittingChest && Main.player[Main.myPlayer].chest < 0)
            ChestUI.RenameChestCancel();
          else if (Main.inputTextEscape && this.TryEscapingMenu())
            return;
        }
        if (IngameFancyUI.CanShowVirtualKeyboard(this._keyboardContext))
        {
          if (inputText != this.Text)
          {
            if (this.CustomTextValidationForUpdate == null || this.CustomTextValidationForUpdate(inputText))
              this.Text = inputText;
            else
              SoundEngine.PlaySound(11);
          }
          if (this._edittingSign)
            this.CopyTextToSign();
          if (this._edittingChest)
            this.CopyTextToChest();
        }
        byte num = (byte) (((int) byte.MaxValue + (int) Main.tileColor.R * 2) / 3);
        Color color = new Color((int) num, (int) num, (int) num, (int) byte.MaxValue);
        this._textBox.TextColor = Color.Lerp(Color.White, color, 0.2f);
        this._label.TextColor = Color.Lerp(Color.White, color, 0.2f);
        ref Vector2 local2 = ref position;
        double x2 = (double) (Main.screenWidth / 2);
        dimensions = this._textBox.GetDimensions();
        double y2 = (double) (dimensions.ToRectangle().Bottom + 32);
        local2 = new Vector2((float) x2, (float) y2);
        Main.instance.DrawWindowsIMEPanel(position, 0.5f);
      }
    }

    private bool TryEscapingMenu()
    {
      if (this.CustomEscapeAttempt != null)
        return this.CustomEscapeAttempt();
      if (this._edittingSign)
        Main.InputTextSignCancel();
      if (this._edittingChest)
        ChestUI.RenameChestCancel();
      IngameFancyUI.Close();
      return true;
    }

    private void UpdateOffsetDown()
    {
      this._textBox.HideSelf = UIVirtualKeyboard.ShouldHideText;
      int num1 = UIVirtualKeyboard.OffsetDown - this._lastOffsetDown;
      int num2 = num1;
      if (Math.Abs(num1) < 10)
        num2 = num1;
      this._lastOffsetDown += num2;
      if (num2 == 0)
        return;
      this.Top.Pixels += (float) num2;
      this.Recalculate();
    }

    public override void OnActivate()
    {
      if (!PlayerInput.UsingGamepadUI || this._restoreButton == null)
        return;
      UILinkPointNavigator.ChangePoint(3002);
    }

    public override void OnDeactivate()
    {
      base.OnDeactivate();
      PlayerInput.WritingText = false;
      UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS = 0;
    }

    private void SetupGamepadPoints(SpriteBatch spriteBatch)
    {
      UILinkPointNavigator.Shortcuts.BackButtonCommand = 6;
      UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS = 1;
      int num1 = 3002;
      int num2 = 5;
      int num3 = 10;
      int num4 = num3 * num2 - 1;
      int num5 = num3 * (num2 - 1);
      UILinkPointNavigator.SetPosition(3000, this._cancelButton.GetDimensions().Center());
      UILinkPoint point1 = UILinkPointNavigator.Points[3000];
      point1.Unlink();
      point1.Right = 3001;
      point1.Up = num1 + num5;
      UILinkPointNavigator.SetPosition(3001, this._submitButton.GetDimensions().Center());
      UILinkPoint point2 = UILinkPointNavigator.Points[3001];
      point2.Unlink();
      point2.Left = 3000;
      point2.Up = num1 + num4;
      for (int index1 = 0; index1 < num2; ++index1)
      {
        for (int index2 = 0; index2 < num3; ++index2)
        {
          int index3 = index1 * num3 + index2;
          int num6 = num1 + index3;
          if (this._keyList[index3] != null)
          {
            UILinkPointNavigator.SetPosition(num6, this._keyList[index3].GetDimensions().Center());
            UILinkPoint point3 = UILinkPointNavigator.Points[num6];
            point3.Unlink();
            int num7 = index2 - 1;
            while (num7 >= 0 && this._keyList[index1 * num3 + num7] == this._keyList[index3])
              --num7;
            point3.Left = num7 == -1 ? index1 * num3 + (num3 - 1) + num1 : index1 * num3 + num7 + num1;
            int index4 = index2 + 1;
            while (index4 <= num3 - 1 && this._keyList[index1 * num3 + index4] == this._keyList[index3])
              ++index4;
            point3.Right = index4 == num3 || this._keyList[index3] == this._keyList[index4] ? index1 * num3 + num1 : index1 * num3 + index4 + num1;
            if (index1 != 0)
              point3.Up = num6 - num3;
            point3.Down = index1 == num2 - 1 ? (index2 < num2 ? 3000 : 3001) : num6 + num3;
          }
        }
      }
    }

    public static void CycleSymbols()
    {
      if (UIVirtualKeyboard._currentInstance == null)
        return;
      switch (UIVirtualKeyboard._currentInstance._keyState)
      {
        case UIVirtualKeyboard.KeyState.Default:
          UIVirtualKeyboard._currentInstance.SetKeyState(UIVirtualKeyboard.KeyState.Shift);
          break;
        case UIVirtualKeyboard.KeyState.Symbol:
          UIVirtualKeyboard._currentInstance.SetKeyState(UIVirtualKeyboard.KeyState.Default);
          break;
        case UIVirtualKeyboard.KeyState.Shift:
          UIVirtualKeyboard._currentInstance.SetKeyState(UIVirtualKeyboard.KeyState.Symbol);
          break;
      }
    }

    public static void BackSpace()
    {
      if (UIVirtualKeyboard._currentInstance == null)
        return;
      SoundEngine.PlaySound(12);
      UIVirtualKeyboard._currentInstance._textBox.Backspace();
      UIVirtualKeyboard._currentInstance.ValidateText();
    }

    public static bool CanSubmit => UIVirtualKeyboard._currentInstance != null && UIVirtualKeyboard._currentInstance._canSubmit;

    public static void Submit()
    {
      if (UIVirtualKeyboard._currentInstance == null)
        return;
      UIVirtualKeyboard._currentInstance.InternalSubmit();
    }

    private void InternalSubmit()
    {
      string text = this.Text.Trim();
      if (!this.TextIsValidForSubmit())
        return;
      SoundEngine.PlaySound(10);
      this._submitAction(text);
    }

    public static void Cancel()
    {
      if (UIVirtualKeyboard._currentInstance == null)
        return;
      SoundEngine.PlaySound(11);
      UIVirtualKeyboard._currentInstance._cancelAction();
    }

    public static void Write(string text)
    {
      if (UIVirtualKeyboard._currentInstance == null)
        return;
      SoundEngine.PlaySound(12);
      int num = UIVirtualKeyboard._currentInstance.Text.Length == 0 ? 1 : 0;
      UIVirtualKeyboard._currentInstance._textBox.Write(text);
      UIVirtualKeyboard._currentInstance.ValidateText();
      if (num == 0 || UIVirtualKeyboard._currentInstance.Text.Length <= 0 || UIVirtualKeyboard._currentInstance._keyState != UIVirtualKeyboard.KeyState.Shift)
        return;
      UIVirtualKeyboard._currentInstance.SetKeyState(UIVirtualKeyboard.KeyState.Default);
    }

    public static void CursorLeft()
    {
      if (UIVirtualKeyboard._currentInstance == null)
        return;
      SoundEngine.PlaySound(12);
      UIVirtualKeyboard._currentInstance._textBox.CursorLeft();
    }

    public static void CursorRight()
    {
      if (UIVirtualKeyboard._currentInstance == null)
        return;
      SoundEngine.PlaySound(12);
      UIVirtualKeyboard._currentInstance._textBox.CursorRight();
    }

    public static bool CanDisplay(int keyboardContext) => keyboardContext != 1 || Main.screenHeight > 700;

    public static int KeyboardContext => UIVirtualKeyboard._currentInstance == null ? -1 : UIVirtualKeyboard._currentInstance._keyboardContext;

    public static void CacheCancelledInput(int cacheMode)
    {
      if (cacheMode != 1)
        return;
      UIVirtualKeyboard._cancelCacheSign = Main.npcChatText;
    }

    private void RestoreCancelledInput(int cacheMode)
    {
      if (cacheMode != 1)
        return;
      Main.npcChatText = UIVirtualKeyboard._cancelCacheSign;
      this.Text = Main.npcChatText;
      UIVirtualKeyboard._cancelCacheSign = "";
    }

    private void CopyTextToSign()
    {
      if (!this._edittingSign)
        return;
      int sign = Main.player[Main.myPlayer].sign;
      if (sign < 0 || Main.sign[sign] == null)
        return;
      Main.npcChatText = this.Text;
    }

    private void CopyTextToChest()
    {
      if (!this._edittingChest)
        return;
      Main.npcChatText = this.Text;
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

    public delegate void KeyboardSubmitEvent(string text);

    public enum KeyState
    {
      Default,
      Symbol,
      Shift,
    }
  }
}
