// Decompiled with JetBrains decompiler
// Type: Terraria.GameInput.PlayerInput
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Audio;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Chat;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameInput
{
  public class PlayerInput
  {
    public static Vector2 RawMouseScale = Vector2.One;
    public static TriggersPack Triggers = new TriggersPack();
    public static List<string> KnownTriggers = new List<string>()
    {
      "MouseLeft",
      "MouseRight",
      "Up",
      "Down",
      "Left",
      "Right",
      "Jump",
      "Throw",
      "Inventory",
      "Grapple",
      "SmartSelect",
      "SmartCursor",
      "QuickMount",
      "QuickHeal",
      "QuickMana",
      "QuickBuff",
      "MapZoomIn",
      "MapZoomOut",
      "MapAlphaUp",
      "MapAlphaDown",
      "MapFull",
      "MapStyle",
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
      "HotbarMinus",
      "HotbarPlus",
      "DpadRadial1",
      "DpadRadial2",
      "DpadRadial3",
      "DpadRadial4",
      "RadialHotbar",
      "RadialQuickbar",
      "DpadSnap1",
      "DpadSnap2",
      "DpadSnap3",
      "DpadSnap4",
      "MenuUp",
      "MenuDown",
      "MenuLeft",
      "MenuRight",
      "LockOn",
      "ViewZoomIn",
      "ViewZoomOut",
      "ToggleCreativeMenu"
    };
    private static bool _canReleaseRebindingLock = true;
    private static int _memoOfLastPoint = -1;
    public static int NavigatorRebindingLock;
    public static string BlockedKey = "";
    private static string _listeningTrigger;
    private static InputMode _listeningInputMode;
    public static Dictionary<string, PlayerInputProfile> Profiles = new Dictionary<string, PlayerInputProfile>();
    public static Dictionary<string, PlayerInputProfile> OriginalProfiles = new Dictionary<string, PlayerInputProfile>();
    private static string _selectedProfile;
    private static PlayerInputProfile _currentProfile;
    public static InputMode CurrentInputMode = InputMode.Keyboard;
    private static Buttons[] ButtonsGamepad = (Buttons[]) Enum.GetValues(typeof (Buttons));
    public static bool GrappleAndInteractAreShared;
    public static SmartSelectGamepadPointer smartSelectPointer = new SmartSelectGamepadPointer();
    public static bool UseSteamDeckIfPossible;
    private static string _invalidatorCheck = "";
    private static bool _lastActivityState;
    public static MouseState MouseInfo;
    public static MouseState MouseInfoOld;
    public static int MouseX;
    public static int MouseY;
    public static bool LockGamepadTileUseButton = false;
    public static List<string> MouseKeys = new List<string>();
    public static int PreUIX;
    public static int PreUIY;
    public static int PreLockOnX;
    public static int PreLockOnY;
    public static int ScrollWheelValue;
    public static int ScrollWheelValueOld;
    public static int ScrollWheelDelta;
    public static int ScrollWheelDeltaForUI;
    public static bool GamepadAllowScrolling;
    public static int GamepadScrollValue;
    public static Vector2 GamepadThumbstickLeft = Vector2.Zero;
    public static Vector2 GamepadThumbstickRight = Vector2.Zero;
    private const int _fastUseMouseItemSlotType = -2;
    private const int _fastUseEmpty = -1;
    private static int _fastUseItemInventorySlot = -1;
    private static bool _InBuildingMode;
    private static int _UIPointForBuildingMode = -1;
    public static bool WritingText;
    private static int _originalMouseX;
    private static int _originalMouseY;
    private static int _originalLastMouseX;
    private static int _originalLastMouseY;
    private static int _originalScreenWidth;
    private static int _originalScreenHeight;
    private static ZoomContext _currentWantedZoom;
    private static List<string> _buttonsLocked = new List<string>();
    public static bool PreventCursorModeSwappingToGamepad = false;
    public static bool PreventFirstMousePositionGrab = false;
    private static int[] DpadSnapCooldown = new int[4];
    public static bool AllowExecutionOfGamepadInstructions = true;

    public static event Action OnBindingChange;

    public static event Action OnActionableInput;

    public static void ListenFor(string triggerName, InputMode inputmode)
    {
      PlayerInput._listeningTrigger = triggerName;
      PlayerInput._listeningInputMode = inputmode;
    }

    public static string ListeningTrigger => PlayerInput._listeningTrigger;

    public static bool CurrentlyRebinding => PlayerInput._listeningTrigger != null;

    public static bool InvisibleGamepadInMenus
    {
      get
      {
        if (((Main.gameMenu || Main.ingameOptionsWindow || Main.playerInventory || Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1 ? 1 : (Main.InGameUI.CurrentState != null ? 1 : 0)) == 0 || PlayerInput._InBuildingMode ? 0 : (Main.InvisibleCursorForGamepad ? 1 : 0)) != 0)
          return true;
        return PlayerInput.CursorIsBusy && !PlayerInput._InBuildingMode;
      }
    }

    public static PlayerInputProfile CurrentProfile => PlayerInput._currentProfile;

    public static KeyConfiguration ProfileGamepadUI => PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI];

    public static bool UsingGamepad => PlayerInput.CurrentInputMode == InputMode.XBoxGamepad || PlayerInput.CurrentInputMode == InputMode.XBoxGamepadUI;

    public static bool UsingGamepadUI => PlayerInput.CurrentInputMode == InputMode.XBoxGamepadUI;

    public static bool IgnoreMouseInterface
    {
      get
      {
        if (Main.LocalPlayer.itemAnimation > 0 && !PlayerInput.UsingGamepad)
          return true;
        bool flag = PlayerInput.UsingGamepad && !UILinkPointNavigator.Available;
        return (!flag || !PlayerInput.SteamDeckIsUsed || PlayerInput.SettingsForUI.CurrentCursorMode != CursorMode.Mouse || Main.mouseRight) && flag;
      }
    }

    public static bool SteamDeckIsUsed => PlayerInput.UseSteamDeckIfPossible;

    private static bool InvalidateKeyboardSwap()
    {
      if (PlayerInput._invalidatorCheck.Length == 0)
        return false;
      string str = "";
      List<Keys> pressedKeys = PlayerInput.GetPressedKeys();
      for (int index = 0; index < pressedKeys.Count; ++index)
        str = str + pressedKeys[index].ToString() + ", ";
      if (str == PlayerInput._invalidatorCheck)
        return true;
      PlayerInput._invalidatorCheck = "";
      return false;
    }

    public static void ResetInputsOnActiveStateChange()
    {
      bool isActive = Main.instance.IsActive;
      if (PlayerInput._lastActivityState != isActive)
      {
        PlayerInput.MouseInfo = new MouseState();
        PlayerInput.MouseInfoOld = new MouseState();
        Main.keyState = Keyboard.GetState();
        Main.inputText = Keyboard.GetState();
        Main.oldInputText = Keyboard.GetState();
        Main.keyCount = 0;
        PlayerInput.Triggers.Reset();
        PlayerInput.Triggers.Reset();
        string str = "";
        List<Keys> pressedKeys = PlayerInput.GetPressedKeys();
        for (int index = 0; index < pressedKeys.Count; ++index)
          str = str + pressedKeys[index].ToString() + ", ";
        PlayerInput._invalidatorCheck = str;
      }
      PlayerInput._lastActivityState = isActive;
    }

    public static List<Keys> GetPressedKeys()
    {
      List<Keys> list = ((IEnumerable<Keys>) Main.keyState.GetPressedKeys()).ToList<Keys>();
      for (int index = list.Count - 1; index >= 0; --index)
      {
        if (list[index] == Keys.None || list[index] == Keys.Kanji)
          list.RemoveAt(index);
      }
      return list;
    }

    public static void TryEnteringFastUseModeForInventorySlot(int inventorySlot)
    {
      PlayerInput._fastUseItemInventorySlot = inventorySlot;
      if (inventorySlot >= 50 || inventorySlot < 0)
        return;
      Player localPlayer = Main.LocalPlayer;
      ItemSlot.PickupItemIntoMouse(localPlayer.inventory, 0, inventorySlot, localPlayer);
    }

    public static void TryEnteringFastUseModeForMouseItem() => PlayerInput._fastUseItemInventorySlot = -2;

    public static bool ShouldFastUseItem => PlayerInput._fastUseItemInventorySlot != -1;

    public static void TryEndingFastUse()
    {
      if (PlayerInput._fastUseItemInventorySlot >= 0 && PlayerInput._fastUseItemInventorySlot != -2)
      {
        Player localPlayer = Main.LocalPlayer;
        if (localPlayer.inventory[PlayerInput._fastUseItemInventorySlot].IsAir)
          Utils.Swap<Item>(ref Main.mouseItem, ref localPlayer.inventory[PlayerInput._fastUseItemInventorySlot]);
      }
      PlayerInput._fastUseItemInventorySlot = -1;
    }

    public static bool InBuildingMode => PlayerInput._InBuildingMode;

    public static void EnterBuildingMode()
    {
      SoundEngine.PlaySound(10);
      PlayerInput._InBuildingMode = true;
      PlayerInput._UIPointForBuildingMode = UILinkPointNavigator.CurrentPoint;
      if (Main.mouseItem.stack > 0)
        return;
      int pointForBuildingMode = PlayerInput._UIPointForBuildingMode;
      if (pointForBuildingMode >= 50 || pointForBuildingMode < 0 || Main.player[Main.myPlayer].inventory[pointForBuildingMode].stack <= 0)
        return;
      Utils.Swap<Item>(ref Main.mouseItem, ref Main.player[Main.myPlayer].inventory[pointForBuildingMode]);
    }

    public static void ExitBuildingMode()
    {
      SoundEngine.PlaySound(11);
      PlayerInput._InBuildingMode = false;
      UILinkPointNavigator.ChangePoint(PlayerInput._UIPointForBuildingMode);
      if (Main.mouseItem.stack > 0 && Main.player[Main.myPlayer].itemAnimation == 0)
      {
        int pointForBuildingMode = PlayerInput._UIPointForBuildingMode;
        if (pointForBuildingMode < 50 && pointForBuildingMode >= 0 && Main.player[Main.myPlayer].inventory[pointForBuildingMode].stack <= 0)
          Utils.Swap<Item>(ref Main.mouseItem, ref Main.player[Main.myPlayer].inventory[pointForBuildingMode]);
      }
      PlayerInput._UIPointForBuildingMode = -1;
    }

    public static void VerifyBuildingMode()
    {
      if (!PlayerInput._InBuildingMode)
        return;
      Player player = Main.player[Main.myPlayer];
      bool flag = false;
      if (Main.mouseItem.stack <= 0)
        flag = true;
      if (player.dead)
        flag = true;
      if (!flag)
        return;
      PlayerInput.ExitBuildingMode();
    }

    public static int RealScreenWidth => PlayerInput._originalScreenWidth;

    public static int RealScreenHeight => PlayerInput._originalScreenHeight;

    public static void SetSelectedProfile(string name)
    {
      if (!PlayerInput.Profiles.ContainsKey(name))
        return;
      PlayerInput._selectedProfile = name;
      PlayerInput._currentProfile = PlayerInput.Profiles[PlayerInput._selectedProfile];
    }

    public static void Initialize()
    {
      Main.InputProfiles.OnProcessText += new Preferences.TextProcessAction(PlayerInput.PrettyPrintProfiles);
      Player.Hooks.OnEnterWorld += new Action<Player>(PlayerInput.Hook_OnEnterWorld);
      PlayerInputProfile playerInputProfile1 = new PlayerInputProfile("Redigit's Pick");
      playerInputProfile1.Initialize(PresetProfiles.Redigit);
      PlayerInput.Profiles.Add(playerInputProfile1.Name, playerInputProfile1);
      PlayerInputProfile playerInputProfile2 = new PlayerInputProfile("Yoraiz0r's Pick");
      playerInputProfile2.Initialize(PresetProfiles.Yoraiz0r);
      PlayerInput.Profiles.Add(playerInputProfile2.Name, playerInputProfile2);
      PlayerInputProfile playerInputProfile3 = new PlayerInputProfile("Console (Playstation)");
      playerInputProfile3.Initialize(PresetProfiles.ConsolePS);
      PlayerInput.Profiles.Add(playerInputProfile3.Name, playerInputProfile3);
      PlayerInputProfile playerInputProfile4 = new PlayerInputProfile("Console (Xbox)");
      playerInputProfile4.Initialize(PresetProfiles.ConsoleXBox);
      PlayerInput.Profiles.Add(playerInputProfile4.Name, playerInputProfile4);
      PlayerInputProfile playerInputProfile5 = new PlayerInputProfile("Custom");
      playerInputProfile5.Initialize(PresetProfiles.Redigit);
      PlayerInput.Profiles.Add(playerInputProfile5.Name, playerInputProfile5);
      PlayerInputProfile playerInputProfile6 = new PlayerInputProfile("Redigit's Pick");
      playerInputProfile6.Initialize(PresetProfiles.Redigit);
      PlayerInput.OriginalProfiles.Add(playerInputProfile6.Name, playerInputProfile6);
      PlayerInputProfile playerInputProfile7 = new PlayerInputProfile("Yoraiz0r's Pick");
      playerInputProfile7.Initialize(PresetProfiles.Yoraiz0r);
      PlayerInput.OriginalProfiles.Add(playerInputProfile7.Name, playerInputProfile7);
      PlayerInputProfile playerInputProfile8 = new PlayerInputProfile("Console (Playstation)");
      playerInputProfile8.Initialize(PresetProfiles.ConsolePS);
      PlayerInput.OriginalProfiles.Add(playerInputProfile8.Name, playerInputProfile8);
      PlayerInputProfile playerInputProfile9 = new PlayerInputProfile("Console (Xbox)");
      playerInputProfile9.Initialize(PresetProfiles.ConsoleXBox);
      PlayerInput.OriginalProfiles.Add(playerInputProfile9.Name, playerInputProfile9);
      PlayerInput.SetSelectedProfile("Custom");
      PlayerInput.Triggers.Initialize();
    }

    public static void Hook_OnEnterWorld(Player player)
    {
      if (!PlayerInput.UsingGamepad || player.whoAmI != Main.myPlayer)
        return;
      Main.SmartCursorWanted = true;
    }

    public static bool Save()
    {
      Main.InputProfiles.Clear();
      Main.InputProfiles.Put("Selected Profile", (object) PlayerInput._selectedProfile);
      foreach (KeyValuePair<string, PlayerInputProfile> profile in PlayerInput.Profiles)
        Main.InputProfiles.Put(profile.Value.Name, (object) profile.Value.Save());
      return Main.InputProfiles.Save();
    }

    public static void Load()
    {
      Main.InputProfiles.Load();
      Dictionary<string, PlayerInputProfile> dictionary = new Dictionary<string, PlayerInputProfile>();
      string currentValue1 = (string) null;
      Main.InputProfiles.Get<string>("Selected Profile", ref currentValue1);
      List<string> allKeys = Main.InputProfiles.GetAllKeys();
      for (int index = 0; index < allKeys.Count; ++index)
      {
        string str = allKeys[index];
        if (!(str == "Selected Profile") && !string.IsNullOrEmpty(str))
        {
          Dictionary<string, object> currentValue2 = new Dictionary<string, object>();
          Main.InputProfiles.Get<Dictionary<string, object>>(str, ref currentValue2);
          if (currentValue2.Count > 0)
          {
            PlayerInputProfile playerInputProfile = new PlayerInputProfile(str);
            playerInputProfile.Initialize(PresetProfiles.None);
            if (playerInputProfile.Load(currentValue2))
              dictionary.Add(str, playerInputProfile);
          }
        }
      }
      if (dictionary.Count <= 0)
        return;
      PlayerInput.Profiles = dictionary;
      if (!string.IsNullOrEmpty(currentValue1) && PlayerInput.Profiles.ContainsKey(currentValue1))
        PlayerInput.SetSelectedProfile(currentValue1);
      else
        PlayerInput.SetSelectedProfile(PlayerInput.Profiles.Keys.First<string>());
    }

    public static void ManageVersion_1_3()
    {
      PlayerInputProfile profile = PlayerInput.Profiles["Custom"];
      string[,] strArray = new string[20, 2]
      {
        {
          "KeyUp",
          "Up"
        },
        {
          "KeyDown",
          "Down"
        },
        {
          "KeyLeft",
          "Left"
        },
        {
          "KeyRight",
          "Right"
        },
        {
          "KeyJump",
          "Jump"
        },
        {
          "KeyThrowItem",
          "Throw"
        },
        {
          "KeyInventory",
          "Inventory"
        },
        {
          "KeyQuickHeal",
          "QuickHeal"
        },
        {
          "KeyQuickMana",
          "QuickMana"
        },
        {
          "KeyQuickBuff",
          "QuickBuff"
        },
        {
          "KeyUseHook",
          "Grapple"
        },
        {
          "KeyAutoSelect",
          "SmartSelect"
        },
        {
          "KeySmartCursor",
          "SmartCursor"
        },
        {
          "KeyMount",
          "QuickMount"
        },
        {
          "KeyMapStyle",
          "MapStyle"
        },
        {
          "KeyFullscreenMap",
          "MapFull"
        },
        {
          "KeyMapZoomIn",
          "MapZoomIn"
        },
        {
          "KeyMapZoomOut",
          "MapZoomOut"
        },
        {
          "KeyMapAlphaUp",
          "MapAlphaUp"
        },
        {
          "KeyMapAlphaDown",
          "MapAlphaDown"
        }
      };
      for (int index = 0; index < strArray.GetLength(0); ++index)
      {
        string currentValue = (string) null;
        Main.Configuration.Get<string>(strArray[index, 0], ref currentValue);
        if (currentValue != null)
        {
          profile.InputModes[InputMode.Keyboard].KeyStatus[strArray[index, 1]] = new List<string>()
          {
            currentValue
          };
          profile.InputModes[InputMode.KeyboardUI].KeyStatus[strArray[index, 1]] = new List<string>()
          {
            currentValue
          };
        }
      }
    }

    public static bool CursorIsBusy => (double) ItemSlot.CircularRadialOpacity > 0.0 || (double) ItemSlot.QuicksRadialOpacity > 0.0;

    public static void LockGamepadButtons(string TriggerName)
    {
      List<string> collection = (List<string>) null;
      KeyConfiguration keyConfiguration = (KeyConfiguration) null;
      if (!PlayerInput.CurrentProfile.InputModes.TryGetValue(PlayerInput.CurrentInputMode, out keyConfiguration) || !keyConfiguration.KeyStatus.TryGetValue(TriggerName, out collection))
        return;
      PlayerInput._buttonsLocked.AddRange((IEnumerable<string>) collection);
    }

    public static bool IsGamepadButtonLockedFromUse(string keyName) => PlayerInput._buttonsLocked.Contains(keyName);

    public static void UpdateInput()
    {
      PlayerInput.SettingsForUI.UpdateCounters();
      PlayerInput.Triggers.Reset();
      PlayerInput.ScrollWheelValueOld = PlayerInput.ScrollWheelValue;
      PlayerInput.ScrollWheelValue = 0;
      PlayerInput.GamepadThumbstickLeft = Vector2.Zero;
      PlayerInput.GamepadThumbstickRight = Vector2.Zero;
      PlayerInput.GrappleAndInteractAreShared = (PlayerInput.UsingGamepad || PlayerInput.SteamDeckIsUsed) && PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].DoGrappleAndInteractShareTheSameKey;
      if (PlayerInput.InBuildingMode && !PlayerInput.UsingGamepad)
        PlayerInput.ExitBuildingMode();
      if (PlayerInput._canReleaseRebindingLock && PlayerInput.NavigatorRebindingLock > 0)
      {
        --PlayerInput.NavigatorRebindingLock;
        PlayerInput.Triggers.Current.UsedMovementKey = false;
        if (PlayerInput.NavigatorRebindingLock == 0 && PlayerInput._memoOfLastPoint != -1)
        {
          UIManageControls.ForceMoveTo = PlayerInput._memoOfLastPoint;
          PlayerInput._memoOfLastPoint = -1;
        }
      }
      PlayerInput._canReleaseRebindingLock = true;
      PlayerInput.VerifyBuildingMode();
      PlayerInput.MouseInput();
      int num = 0 | (PlayerInput.KeyboardInput() ? 1 : 0) | (PlayerInput.GamePadInput() ? 1 : 0);
      PlayerInput.Triggers.Update();
      PlayerInput.PostInput();
      PlayerInput.ScrollWheelDelta = PlayerInput.ScrollWheelValue - PlayerInput.ScrollWheelValueOld;
      PlayerInput.ScrollWheelDeltaForUI = PlayerInput.ScrollWheelDelta;
      PlayerInput.WritingText = false;
      PlayerInput.UpdateMainMouse();
      Main.mouseLeft = PlayerInput.Triggers.Current.MouseLeft;
      Main.mouseRight = PlayerInput.Triggers.Current.MouseRight;
      PlayerInput.CacheZoomableValues();
      if (num == 0 || PlayerInput.OnActionableInput == null)
        return;
      PlayerInput.OnActionableInput();
    }

    public static void UpdateMainMouse()
    {
      Main.lastMouseX = Main.mouseX;
      Main.lastMouseY = Main.mouseY;
      Main.mouseX = PlayerInput.MouseX;
      Main.mouseY = PlayerInput.MouseY;
    }

    public static void CacheZoomableValues()
    {
      PlayerInput.CacheOriginalInput();
      PlayerInput.CacheOriginalScreenDimensions();
    }

    public static void CacheMousePositionForZoom()
    {
      float num = 1f;
      PlayerInput._originalMouseX = (int) ((double) Main.mouseX * (double) num);
      PlayerInput._originalMouseY = (int) ((double) Main.mouseY * (double) num);
    }

    private static void CacheOriginalInput()
    {
      PlayerInput._originalMouseX = Main.mouseX;
      PlayerInput._originalMouseY = Main.mouseY;
      PlayerInput._originalLastMouseX = Main.lastMouseX;
      PlayerInput._originalLastMouseY = Main.lastMouseY;
    }

    public static void CacheOriginalScreenDimensions()
    {
      PlayerInput._originalScreenWidth = Main.screenWidth;
      PlayerInput._originalScreenHeight = Main.screenHeight;
    }

    public static Vector2 OriginalScreenSize => new Vector2((float) PlayerInput._originalScreenWidth, (float) PlayerInput._originalScreenHeight);

    private static bool GamePadInput()
    {
      bool flag1 = false;
      PlayerInput.ScrollWheelValue += PlayerInput.GamepadScrollValue;
      GamePadState gamePadState = new GamePadState();
      bool flag2 = false;
      for (int index = 0; index < 4; ++index)
      {
        GamePadState state = GamePad.GetState((PlayerIndex) index);
        if (state.IsConnected)
        {
          flag2 = true;
          gamePadState = state;
          break;
        }
      }
      if (Main.SettingBlockGamepadsEntirely || !flag2 || !Main.instance.IsActive && !Main.AllowUnfocusedInputOnGamepad)
        return false;
      Player player = Main.player[Main.myPlayer];
      bool flag3 = UILinkPointNavigator.Available && !PlayerInput.InBuildingMode;
      InputMode inputMode1 = InputMode.XBoxGamepad;
      if (Main.gameMenu | flag3 || player.talkNPC != -1 || player.sign != -1 || IngameFancyUI.CanCover())
        inputMode1 = InputMode.XBoxGamepadUI;
      if (!Main.gameMenu && PlayerInput.InBuildingMode)
        inputMode1 = InputMode.XBoxGamepad;
      if (PlayerInput.CurrentInputMode == InputMode.XBoxGamepad && inputMode1 == InputMode.XBoxGamepadUI)
        flag1 = true;
      if (PlayerInput.CurrentInputMode == InputMode.XBoxGamepadUI && inputMode1 == InputMode.XBoxGamepad)
        flag1 = true;
      if (flag1)
        PlayerInput.CurrentInputMode = inputMode1;
      KeyConfiguration inputMode2 = PlayerInput.CurrentProfile.InputModes[inputMode1];
      int num1 = 2145386496;
      for (int index = 0; index < PlayerInput.ButtonsGamepad.Length; ++index)
      {
        if (((Buttons) num1 & PlayerInput.ButtonsGamepad[index]) <= (Buttons) 0)
        {
          string newKey = PlayerInput.ButtonsGamepad[index].ToString();
          bool flag4 = PlayerInput._buttonsLocked.Contains(newKey);
          if (gamePadState.IsButtonDown(PlayerInput.ButtonsGamepad[index]))
          {
            if (!flag4)
            {
              if (PlayerInput.CheckRebindingProcessGamepad(newKey))
                return false;
              inputMode2.Processkey(PlayerInput.Triggers.Current, newKey, inputMode1);
              flag1 = true;
            }
          }
          else
            PlayerInput._buttonsLocked.Remove(newKey);
        }
      }
      PlayerInput.GamepadThumbstickLeft = gamePadState.ThumbSticks.Left * new Vector2(1f, -1f) * new Vector2((float) (PlayerInput.CurrentProfile.LeftThumbstickInvertX.ToDirectionInt() * -1), (float) (PlayerInput.CurrentProfile.LeftThumbstickInvertY.ToDirectionInt() * -1));
      PlayerInput.GamepadThumbstickRight = gamePadState.ThumbSticks.Right * new Vector2(1f, -1f) * new Vector2((float) (PlayerInput.CurrentProfile.RightThumbstickInvertX.ToDirectionInt() * -1), (float) (PlayerInput.CurrentProfile.RightThumbstickInvertY.ToDirectionInt() * -1));
      Vector2 gamepadThumbstickRight = PlayerInput.GamepadThumbstickRight;
      Vector2 gamepadThumbstickLeft = PlayerInput.GamepadThumbstickLeft;
      Vector2 vector2_1 = gamepadThumbstickRight;
      if (vector2_1 != Vector2.Zero)
        vector2_1.Normalize();
      Vector2 vector2_2 = gamepadThumbstickLeft;
      if (vector2_2 != Vector2.Zero)
        vector2_2.Normalize();
      float num2 = 0.6f;
      float triggersDeadzone = PlayerInput.CurrentProfile.TriggersDeadzone;
      if (inputMode1 == InputMode.XBoxGamepadUI)
      {
        num2 = 0.4f;
        if (PlayerInput.GamepadAllowScrolling)
          PlayerInput.GamepadScrollValue -= (int) ((double) gamepadThumbstickRight.Y * 16.0);
        PlayerInput.GamepadAllowScrolling = false;
      }
      Buttons buttons;
      if ((double) Vector2.Dot(-Vector2.UnitX, vector2_2) >= (double) num2 && (double) gamepadThumbstickLeft.X < -(double) PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX)
      {
        if (PlayerInput.CheckRebindingProcessGamepad(Buttons.LeftThumbstickLeft.ToString()))
          return false;
        KeyConfiguration keyConfiguration = inputMode2;
        TriggersSet current = PlayerInput.Triggers.Current;
        buttons = Buttons.LeftThumbstickLeft;
        string newKey = buttons.ToString();
        int mode = (int) inputMode1;
        keyConfiguration.Processkey(current, newKey, (InputMode) mode);
        flag1 = true;
      }
      if ((double) Vector2.Dot(Vector2.UnitX, vector2_2) >= (double) num2 && (double) gamepadThumbstickLeft.X > (double) PlayerInput.CurrentProfile.LeftThumbstickDeadzoneX)
      {
        buttons = Buttons.LeftThumbstickRight;
        if (PlayerInput.CheckRebindingProcessGamepad(buttons.ToString()))
          return false;
        KeyConfiguration keyConfiguration = inputMode2;
        TriggersSet current = PlayerInput.Triggers.Current;
        buttons = Buttons.LeftThumbstickRight;
        string newKey = buttons.ToString();
        int mode = (int) inputMode1;
        keyConfiguration.Processkey(current, newKey, (InputMode) mode);
        flag1 = true;
      }
      if ((double) Vector2.Dot(-Vector2.UnitY, vector2_2) >= (double) num2 && (double) gamepadThumbstickLeft.Y < -(double) PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY)
      {
        buttons = Buttons.LeftThumbstickUp;
        if (PlayerInput.CheckRebindingProcessGamepad(buttons.ToString()))
          return false;
        KeyConfiguration keyConfiguration = inputMode2;
        TriggersSet current = PlayerInput.Triggers.Current;
        buttons = Buttons.LeftThumbstickUp;
        string newKey = buttons.ToString();
        int mode = (int) inputMode1;
        keyConfiguration.Processkey(current, newKey, (InputMode) mode);
        flag1 = true;
      }
      if ((double) Vector2.Dot(Vector2.UnitY, vector2_2) >= (double) num2 && (double) gamepadThumbstickLeft.Y > (double) PlayerInput.CurrentProfile.LeftThumbstickDeadzoneY)
      {
        buttons = Buttons.LeftThumbstickDown;
        if (PlayerInput.CheckRebindingProcessGamepad(buttons.ToString()))
          return false;
        KeyConfiguration keyConfiguration = inputMode2;
        TriggersSet current = PlayerInput.Triggers.Current;
        buttons = Buttons.LeftThumbstickDown;
        string newKey = buttons.ToString();
        int mode = (int) inputMode1;
        keyConfiguration.Processkey(current, newKey, (InputMode) mode);
        flag1 = true;
      }
      if ((double) Vector2.Dot(-Vector2.UnitX, vector2_1) >= (double) num2 && (double) gamepadThumbstickRight.X < -(double) PlayerInput.CurrentProfile.RightThumbstickDeadzoneX)
      {
        buttons = Buttons.RightThumbstickLeft;
        if (PlayerInput.CheckRebindingProcessGamepad(buttons.ToString()))
          return false;
        KeyConfiguration keyConfiguration = inputMode2;
        TriggersSet current = PlayerInput.Triggers.Current;
        buttons = Buttons.RightThumbstickLeft;
        string newKey = buttons.ToString();
        int mode = (int) inputMode1;
        keyConfiguration.Processkey(current, newKey, (InputMode) mode);
        flag1 = true;
      }
      if ((double) Vector2.Dot(Vector2.UnitX, vector2_1) >= (double) num2 && (double) gamepadThumbstickRight.X > (double) PlayerInput.CurrentProfile.RightThumbstickDeadzoneX)
      {
        buttons = Buttons.RightThumbstickRight;
        if (PlayerInput.CheckRebindingProcessGamepad(buttons.ToString()))
          return false;
        KeyConfiguration keyConfiguration = inputMode2;
        TriggersSet current = PlayerInput.Triggers.Current;
        buttons = Buttons.RightThumbstickRight;
        string newKey = buttons.ToString();
        int mode = (int) inputMode1;
        keyConfiguration.Processkey(current, newKey, (InputMode) mode);
        flag1 = true;
      }
      if ((double) Vector2.Dot(-Vector2.UnitY, vector2_1) >= (double) num2 && (double) gamepadThumbstickRight.Y < -(double) PlayerInput.CurrentProfile.RightThumbstickDeadzoneY)
      {
        buttons = Buttons.RightThumbstickUp;
        if (PlayerInput.CheckRebindingProcessGamepad(buttons.ToString()))
          return false;
        KeyConfiguration keyConfiguration = inputMode2;
        TriggersSet current = PlayerInput.Triggers.Current;
        buttons = Buttons.RightThumbstickUp;
        string newKey = buttons.ToString();
        int mode = (int) inputMode1;
        keyConfiguration.Processkey(current, newKey, (InputMode) mode);
        flag1 = true;
      }
      if ((double) Vector2.Dot(Vector2.UnitY, vector2_1) >= (double) num2 && (double) gamepadThumbstickRight.Y > (double) PlayerInput.CurrentProfile.RightThumbstickDeadzoneY)
      {
        buttons = Buttons.RightThumbstickDown;
        if (PlayerInput.CheckRebindingProcessGamepad(buttons.ToString()))
          return false;
        KeyConfiguration keyConfiguration = inputMode2;
        TriggersSet current = PlayerInput.Triggers.Current;
        buttons = Buttons.RightThumbstickDown;
        string newKey = buttons.ToString();
        int mode = (int) inputMode1;
        keyConfiguration.Processkey(current, newKey, (InputMode) mode);
        flag1 = true;
      }
      if ((double) gamePadState.Triggers.Left > (double) triggersDeadzone)
      {
        buttons = Buttons.LeftTrigger;
        if (PlayerInput.CheckRebindingProcessGamepad(buttons.ToString()))
          return false;
        KeyConfiguration keyConfiguration = inputMode2;
        TriggersSet current = PlayerInput.Triggers.Current;
        buttons = Buttons.LeftTrigger;
        string newKey = buttons.ToString();
        int mode = (int) inputMode1;
        keyConfiguration.Processkey(current, newKey, (InputMode) mode);
        flag1 = true;
      }
      if ((double) gamePadState.Triggers.Right > (double) triggersDeadzone)
      {
        buttons = Buttons.RightTrigger;
        string newKey = buttons.ToString();
        if (PlayerInput.CheckRebindingProcessGamepad(newKey))
          return false;
        if ((inputMode1 != InputMode.XBoxGamepadUI || !PlayerInput.SteamDeckIsUsed ? 0 : (PlayerInput.SettingsForUI.CurrentCursorMode == CursorMode.Mouse ? 1 : 0)) != 0)
        {
          PlayerInput.Triggers.Current.MouseLeft = true;
        }
        else
        {
          inputMode2.Processkey(PlayerInput.Triggers.Current, newKey, inputMode1);
          flag1 = true;
        }
      }
      bool flag5 = ItemID.Sets.GamepadWholeScreenUseRange[player.inventory[player.selectedItem].type] || player.scope;
      int num3 = player.inventory[player.selectedItem].tileBoost + ItemID.Sets.GamepadExtraRange[player.inventory[player.selectedItem].type];
      if (player.yoyoString && ItemID.Sets.Yoyo[player.inventory[player.selectedItem].type])
        num3 += 5;
      else if (player.inventory[player.selectedItem].createTile < 0 && player.inventory[player.selectedItem].createWall <= 0 && player.inventory[player.selectedItem].shoot > 0)
        num3 += 10;
      else if (player.controlTorch)
        ++num3;
      if (flag5)
        num3 += 30;
      if (player.mount.Active && player.mount.Type == 8)
        num3 = 10;
      bool flag6 = false;
      bool flag7 = !Main.gameMenu && !flag3 && Main.SmartCursorWanted;
      if (!PlayerInput.CursorIsBusy)
      {
        bool flag8 = Main.mapFullscreen || !Main.gameMenu && !flag3;
        int x1 = Main.screenWidth / 2;
        int y1 = Main.screenHeight / 2;
        if (!Main.mapFullscreen & flag8 && !flag5)
        {
          Point point = Main.ReverseGravitySupport(player.Center - Main.screenPosition).ToPoint();
          x1 = point.X;
          y1 = point.Y;
        }
        if (((!(player.velocity == Vector2.Zero) || !(gamepadThumbstickLeft == Vector2.Zero) ? 0 : (gamepadThumbstickRight == Vector2.Zero ? 1 : 0)) & (flag7 ? 1 : 0)) != 0)
          x1 += player.direction * 10;
        float m11_1 = Main.GameViewMatrix.ZoomMatrix.M11;
        PlayerInput.smartSelectPointer.UpdateSize(new Vector2((float) (Player.tileRangeX * 16 + num3 * 16), (float) (Player.tileRangeY * 16 + num3 * 16)) * m11_1);
        if (flag5)
          PlayerInput.smartSelectPointer.UpdateSize(new Vector2((float) (Math.Max(Main.screenWidth, Main.screenHeight) / 2)));
        PlayerInput.smartSelectPointer.UpdateCenter(new Vector2((float) x1, (float) y1));
        if (gamepadThumbstickRight != Vector2.Zero & flag8)
        {
          Vector2 vector2_3 = new Vector2(8f);
          if (!Main.gameMenu && Main.mapFullscreen)
            vector2_3 = new Vector2(16f);
          if (flag7)
          {
            vector2_3 = new Vector2((float) (Player.tileRangeX * 16), (float) (Player.tileRangeY * 16));
            if (num3 != 0)
              vector2_3 += new Vector2((float) (num3 * 16), (float) (num3 * 16));
            if (flag5)
              vector2_3 = new Vector2((float) (Math.Max(Main.screenWidth, Main.screenHeight) / 2));
          }
          else if (!Main.mapFullscreen)
          {
            if (player.inventory[player.selectedItem].mech)
              vector2_3 += Vector2.Zero;
            else
              vector2_3 += new Vector2((float) num3) / 4f;
          }
          float m11_2 = Main.GameViewMatrix.ZoomMatrix.M11;
          Vector2 vector2_4 = gamepadThumbstickRight * vector2_3 * m11_2;
          int num4 = PlayerInput.MouseX - x1;
          int num5 = PlayerInput.MouseY - y1;
          if (flag7)
          {
            num4 = 0;
            num5 = 0;
          }
          int num6 = num4 + (int) vector2_4.X;
          int num7 = num5 + (int) vector2_4.Y;
          PlayerInput.MouseX = num6 + x1;
          PlayerInput.MouseY = num7 + y1;
          flag1 = true;
          flag6 = true;
          PlayerInput.SettingsForUI.SetCursorMode(CursorMode.Gamepad);
        }
        bool secondaryGamepadAim = PlayerInput.SettingsForUI.AllowSecondaryGamepadAim;
        if (gamepadThumbstickLeft != Vector2.Zero & flag8)
        {
          float num8 = 8f;
          if (!Main.gameMenu && Main.mapFullscreen)
            num8 = 3f;
          if (Main.mapFullscreen)
          {
            Vector2 vector2_5 = gamepadThumbstickLeft * num8;
            Main.mapFullscreenPos += vector2_5 * num8 * (1f / Main.mapFullscreenScale);
          }
          else if (((flag6 ? 0 : (Main.SmartCursorWanted ? 1 : 0)) & (secondaryGamepadAim ? 1 : 0)) != 0)
          {
            float m11_3 = Main.GameViewMatrix.ZoomMatrix.M11;
            Vector2 vector2_6 = gamepadThumbstickLeft * new Vector2((float) (Player.tileRangeX * 16), (float) (Player.tileRangeY * 16)) * m11_3;
            if (num3 != 0)
              vector2_6 = gamepadThumbstickLeft * new Vector2((float) ((Player.tileRangeX + num3) * 16), (float) ((Player.tileRangeY + num3) * 16)) * m11_3;
            if (flag5)
              vector2_6 = new Vector2((float) (Math.Max(Main.screenWidth, Main.screenHeight) / 2)) * gamepadThumbstickLeft;
            int x2 = (int) vector2_6.X;
            int y2 = (int) vector2_6.Y;
            PlayerInput.MouseX = x2 + x1;
            int num9 = y1;
            PlayerInput.MouseY = y2 + num9;
          }
          flag1 = true;
        }
        if (PlayerInput.CurrentInputMode == InputMode.XBoxGamepad)
        {
          PlayerInput.HandleDpadSnap();
          if (PlayerInput.SettingsForUI.AllowSecondaryGamepadAim)
          {
            int num10 = PlayerInput.MouseX - x1;
            int num11 = PlayerInput.MouseY - y1;
            int num12;
            int num13;
            if (!Main.gameMenu && !flag3)
            {
              if (flag5 && !Main.mapFullscreen)
              {
                float num14 = 1f;
                int num15 = Main.screenWidth / 2;
                int num16 = Main.screenHeight / 2;
                num12 = (int) Utils.Clamp<float>((float) num10, (float) -num15 * num14, (float) num15 * num14);
                num13 = (int) Utils.Clamp<float>((float) num11, (float) -num16 * num14, (float) num16 * num14);
              }
              else
              {
                float num17 = 0.0f;
                if (player.HeldItem.createTile >= 0 || player.HeldItem.createWall > 0 || player.HeldItem.tileWand >= 0)
                  num17 = 0.5f;
                float m11_4 = Main.GameViewMatrix.ZoomMatrix.M11;
                float num18 = (float) (-((double) (Player.tileRangeY + num3) - (double) num17) * 16.0) * m11_4;
                float max = (float) (((double) (Player.tileRangeY + num3) - (double) num17) * 16.0) * m11_4;
                float min = num18 - (float) (player.height / 16 / 2 * 16);
                num12 = (int) Utils.Clamp<float>((float) num10, (float) (-((double) (Player.tileRangeX + num3) - (double) num17) * 16.0) * m11_4, (float) (((double) (Player.tileRangeX + num3) - (double) num17) * 16.0) * m11_4);
                num13 = (int) Utils.Clamp<float>((float) num11, min, max);
              }
              if (flag7 && !flag1 | flag5)
              {
                float num19 = 0.81f;
                if (flag5)
                  num19 = 0.95f;
                num12 = (int) ((double) num12 * (double) num19);
                num13 = (int) ((double) num13 * (double) num19);
              }
            }
            else
            {
              num12 = Utils.Clamp<int>(num10, -x1 + 10, x1 - 10);
              num13 = Utils.Clamp<int>(num11, -y1 + 10, y1 - 10);
            }
            PlayerInput.MouseX = num12 + x1;
            PlayerInput.MouseY = num13 + y1;
          }
        }
      }
      if (flag1)
        PlayerInput.CurrentInputMode = inputMode1;
      if (PlayerInput.CurrentInputMode == InputMode.XBoxGamepad)
        Main.SetCameraGamepadLerp(0.1f);
      if (PlayerInput.CurrentInputMode != InputMode.XBoxGamepadUI & flag1)
        PlayerInput.PreventCursorModeSwappingToGamepad = true;
      if (!flag1)
        PlayerInput.PreventCursorModeSwappingToGamepad = false;
      if (PlayerInput.CurrentInputMode == InputMode.XBoxGamepadUI & flag1 && !PlayerInput.PreventCursorModeSwappingToGamepad)
        PlayerInput.SettingsForUI.SetCursorMode(CursorMode.Gamepad);
      return flag1;
    }

    private static void MouseInput()
    {
      bool flag = false;
      PlayerInput.MouseInfoOld = PlayerInput.MouseInfo;
      PlayerInput.MouseInfo = Mouse.GetState();
      PlayerInput.ScrollWheelValue += PlayerInput.MouseInfo.ScrollWheelValue;
      if (PlayerInput.MouseInfo.X != PlayerInput.MouseInfoOld.X || PlayerInput.MouseInfo.Y != PlayerInput.MouseInfoOld.Y || PlayerInput.MouseInfo.ScrollWheelValue != PlayerInput.MouseInfoOld.ScrollWheelValue)
      {
        PlayerInput.MouseX = (int) ((double) PlayerInput.MouseInfo.X * (double) PlayerInput.RawMouseScale.X);
        PlayerInput.MouseY = (int) ((double) PlayerInput.MouseInfo.Y * (double) PlayerInput.RawMouseScale.Y);
        if (!PlayerInput.PreventFirstMousePositionGrab)
        {
          flag = true;
          PlayerInput.SettingsForUI.SetCursorMode(CursorMode.Mouse);
        }
        PlayerInput.PreventFirstMousePositionGrab = false;
      }
      PlayerInput.MouseKeys.Clear();
      if (Main.instance.IsActive)
      {
        if (PlayerInput.MouseInfo.LeftButton == ButtonState.Pressed)
        {
          PlayerInput.MouseKeys.Add("Mouse1");
          flag = true;
        }
        if (PlayerInput.MouseInfo.RightButton == ButtonState.Pressed)
        {
          PlayerInput.MouseKeys.Add("Mouse2");
          flag = true;
        }
        if (PlayerInput.MouseInfo.MiddleButton == ButtonState.Pressed)
        {
          PlayerInput.MouseKeys.Add("Mouse3");
          flag = true;
        }
        if (PlayerInput.MouseInfo.XButton1 == ButtonState.Pressed)
        {
          PlayerInput.MouseKeys.Add("Mouse4");
          flag = true;
        }
        if (PlayerInput.MouseInfo.XButton2 == ButtonState.Pressed)
        {
          PlayerInput.MouseKeys.Add("Mouse5");
          flag = true;
        }
      }
      if (!flag)
        return;
      PlayerInput.CurrentInputMode = InputMode.Mouse;
      PlayerInput.Triggers.Current.UsedMovementKey = false;
    }

    private static bool KeyboardInput()
    {
      bool flag1 = false;
      bool flag2 = false;
      List<Keys> pressedKeys = PlayerInput.GetPressedKeys();
      PlayerInput.DebugKeys(pressedKeys);
      if (pressedKeys.Count == 0 && PlayerInput.MouseKeys.Count == 0)
      {
        Main.blockKey = Keys.None.ToString();
        return false;
      }
      for (int index = 0; index < pressedKeys.Count; ++index)
      {
        if (pressedKeys[index] == Keys.LeftShift || pressedKeys[index] == Keys.RightShift)
          flag1 = true;
        else if (pressedKeys[index] == Keys.LeftAlt || pressedKeys[index] == Keys.RightAlt)
          flag2 = true;
        Main.ChromaPainter.PressKey(pressedKeys[index]);
      }
      string blockKey = Main.blockKey;
      Keys keys = Keys.None;
      string str1 = keys.ToString();
      if (blockKey != str1)
      {
        bool flag3 = false;
        for (int index = 0; index < pressedKeys.Count; ++index)
        {
          keys = pressedKeys[index];
          if (keys.ToString() == Main.blockKey)
          {
            pressedKeys[index] = Keys.None;
            flag3 = true;
          }
        }
        if (!flag3)
        {
          keys = Keys.None;
          Main.blockKey = keys.ToString();
        }
      }
      KeyConfiguration inputMode = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard];
      if (Main.gameMenu && !PlayerInput.WritingText)
        inputMode = PlayerInput.CurrentProfile.InputModes[InputMode.KeyboardUI];
      List<string> stringList1 = new List<string>(pressedKeys.Count);
      for (int index = 0; index < pressedKeys.Count; ++index)
      {
        List<string> stringList2 = stringList1;
        keys = pressedKeys[index];
        string str2 = keys.ToString();
        stringList2.Add(str2);
      }
      if (PlayerInput.WritingText)
        stringList1.Clear();
      int count = stringList1.Count;
      stringList1.AddRange((IEnumerable<string>) PlayerInput.MouseKeys);
      bool flag4 = false;
      for (int index = 0; index < stringList1.Count; ++index)
      {
        if (index >= count || pressedKeys[index] != Keys.None)
        {
          string newKey = stringList1[index];
          string str3 = stringList1[index];
          keys = Keys.Tab;
          string str4 = keys.ToString();
          if (!(str3 == str4) || ((!flag1 ? 0 : (SocialAPI.Mode == SocialMode.Steam ? 1 : 0)) | (flag2 ? 1 : 0)) == 0)
          {
            if (PlayerInput.CheckRebindingProcessKeyboard(newKey))
              return false;
            KeyboardState oldKeyState = Main.oldKeyState;
            if (index >= count || !Main.oldKeyState.IsKeyDown(pressedKeys[index]))
              inputMode.Processkey(PlayerInput.Triggers.Current, newKey, InputMode.Keyboard);
            else
              inputMode.CopyKeyState(PlayerInput.Triggers.Old, PlayerInput.Triggers.Current, newKey);
            if (index >= count || pressedKeys[index] != Keys.None)
              flag4 = true;
          }
        }
      }
      if (flag4)
        PlayerInput.CurrentInputMode = InputMode.Keyboard;
      return flag4;
    }

    private static void DebugKeys(List<Keys> keys)
    {
    }

    private static void FixDerpedRebinds()
    {
      List<string> triggers = new List<string>()
      {
        "MouseLeft",
        "MouseRight",
        "Inventory"
      };
      foreach (InputMode inputMode in Enum.GetValues(typeof (InputMode)))
      {
        if (inputMode != InputMode.Mouse)
        {
          PlayerInput.FixKeysConflict(inputMode, triggers);
          foreach (string str in triggers)
          {
            if (PlayerInput.CurrentProfile.InputModes[inputMode].KeyStatus[str].Count < 1)
              PlayerInput.ResetKeyBinding(inputMode, str);
          }
        }
      }
    }

    private static void FixKeysConflict(InputMode inputMode, List<string> triggers)
    {
      for (int index1 = 0; index1 < triggers.Count; ++index1)
      {
        for (int index2 = index1 + 1; index2 < triggers.Count; ++index2)
        {
          List<string> keyStatu1 = PlayerInput.CurrentProfile.InputModes[inputMode].KeyStatus[triggers[index1]];
          List<string> keyStatu2 = PlayerInput.CurrentProfile.InputModes[inputMode].KeyStatus[triggers[index2]];
          foreach (string str in keyStatu1.Intersect<string>((IEnumerable<string>) keyStatu2).ToList<string>())
          {
            keyStatu1.Remove(str);
            keyStatu2.Remove(str);
          }
        }
      }
    }

    private static void ResetKeyBinding(InputMode inputMode, string trigger)
    {
      string key = "Redigit's Pick";
      if (PlayerInput.OriginalProfiles.ContainsKey(PlayerInput._selectedProfile))
        key = PlayerInput._selectedProfile;
      PlayerInput.CurrentProfile.InputModes[inputMode].KeyStatus[trigger].Clear();
      PlayerInput.CurrentProfile.InputModes[inputMode].KeyStatus[trigger].AddRange((IEnumerable<string>) PlayerInput.OriginalProfiles[key].InputModes[inputMode].KeyStatus[trigger]);
    }

    private static bool CheckRebindingProcessGamepad(string newKey)
    {
      PlayerInput._canReleaseRebindingLock = false;
      if (PlayerInput.CurrentlyRebinding && PlayerInput._listeningInputMode == InputMode.XBoxGamepad)
      {
        PlayerInput.NavigatorRebindingLock = 3;
        PlayerInput._memoOfLastPoint = UILinkPointNavigator.CurrentPoint;
        SoundEngine.PlaySound(12);
        if (PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus[PlayerInput.ListeningTrigger].Contains(newKey))
          PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus[PlayerInput.ListeningTrigger].Remove(newKey);
        else
          PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus[PlayerInput.ListeningTrigger] = new List<string>()
          {
            newKey
          };
        PlayerInput.ListenFor((string) null, InputMode.XBoxGamepad);
      }
      if (PlayerInput.CurrentlyRebinding && PlayerInput._listeningInputMode == InputMode.XBoxGamepadUI)
      {
        PlayerInput.NavigatorRebindingLock = 3;
        PlayerInput._memoOfLastPoint = UILinkPointNavigator.CurrentPoint;
        SoundEngine.PlaySound(12);
        if (PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus[PlayerInput.ListeningTrigger].Contains(newKey))
          PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus[PlayerInput.ListeningTrigger].Remove(newKey);
        else
          PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus[PlayerInput.ListeningTrigger] = new List<string>()
          {
            newKey
          };
        PlayerInput.ListenFor((string) null, InputMode.XBoxGamepadUI);
      }
      PlayerInput.FixDerpedRebinds();
      if (PlayerInput.OnBindingChange != null)
        PlayerInput.OnBindingChange();
      return PlayerInput.NavigatorRebindingLock > 0;
    }

    private static bool CheckRebindingProcessKeyboard(string newKey)
    {
      PlayerInput._canReleaseRebindingLock = false;
      if (PlayerInput.CurrentlyRebinding && PlayerInput._listeningInputMode == InputMode.Keyboard)
      {
        PlayerInput.NavigatorRebindingLock = 3;
        PlayerInput._memoOfLastPoint = UILinkPointNavigator.CurrentPoint;
        SoundEngine.PlaySound(12);
        if (PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[PlayerInput.ListeningTrigger].Contains(newKey))
          PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[PlayerInput.ListeningTrigger].Remove(newKey);
        else
          PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[PlayerInput.ListeningTrigger] = new List<string>()
          {
            newKey
          };
        PlayerInput.ListenFor((string) null, InputMode.Keyboard);
        Main.blockKey = newKey;
        Main.blockInput = false;
        Main.ChromaPainter.CollectBoundKeys();
      }
      if (PlayerInput.CurrentlyRebinding && PlayerInput._listeningInputMode == InputMode.KeyboardUI)
      {
        PlayerInput.NavigatorRebindingLock = 3;
        PlayerInput._memoOfLastPoint = UILinkPointNavigator.CurrentPoint;
        SoundEngine.PlaySound(12);
        if (PlayerInput.CurrentProfile.InputModes[InputMode.KeyboardUI].KeyStatus[PlayerInput.ListeningTrigger].Contains(newKey))
          PlayerInput.CurrentProfile.InputModes[InputMode.KeyboardUI].KeyStatus[PlayerInput.ListeningTrigger].Remove(newKey);
        else
          PlayerInput.CurrentProfile.InputModes[InputMode.KeyboardUI].KeyStatus[PlayerInput.ListeningTrigger] = new List<string>()
          {
            newKey
          };
        PlayerInput.ListenFor((string) null, InputMode.KeyboardUI);
        Main.blockKey = newKey;
        Main.blockInput = false;
        Main.ChromaPainter.CollectBoundKeys();
      }
      PlayerInput.FixDerpedRebinds();
      if (PlayerInput.OnBindingChange != null)
        PlayerInput.OnBindingChange();
      return PlayerInput.NavigatorRebindingLock > 0;
    }

    private static void PostInput()
    {
      Main.GamepadCursorAlpha = MathHelper.Clamp(Main.GamepadCursorAlpha + (!Main.SmartCursorIsUsed || UILinkPointNavigator.Available || !(PlayerInput.GamepadThumbstickLeft == Vector2.Zero) || !(PlayerInput.GamepadThumbstickRight == Vector2.Zero) ? 0.05f : -0.05f), 0.0f, 1f);
      if (PlayerInput.CurrentProfile.HotbarAllowsRadial)
      {
        int num = PlayerInput.Triggers.Current.HotbarPlus.ToInt() - PlayerInput.Triggers.Current.HotbarMinus.ToInt();
        if (PlayerInput.MiscSettingsTEMP.HotbarRadialShouldBeUsed)
        {
          switch (num)
          {
            case -1:
              PlayerInput.Triggers.Current.RadialQuickbar = true;
              PlayerInput.Triggers.JustReleased.RadialQuickbar = false;
              break;
            case 1:
              PlayerInput.Triggers.Current.RadialHotbar = true;
              PlayerInput.Triggers.JustReleased.RadialHotbar = false;
              break;
          }
        }
      }
      PlayerInput.MiscSettingsTEMP.HotbarRadialShouldBeUsed = false;
    }

    private static void HandleDpadSnap()
    {
      Vector2 zero = Vector2.Zero;
      Player player = Main.player[Main.myPlayer];
      for (int index = 0; index < 4; ++index)
      {
        bool flag = false;
        Vector2 vector2 = Vector2.Zero;
        if (Main.gameMenu || UILinkPointNavigator.Available && !PlayerInput.InBuildingMode)
          return;
        switch (index)
        {
          case 0:
            flag = PlayerInput.Triggers.Current.DpadMouseSnap1;
            vector2 = -Vector2.UnitY;
            break;
          case 1:
            flag = PlayerInput.Triggers.Current.DpadMouseSnap2;
            vector2 = Vector2.UnitX;
            break;
          case 2:
            flag = PlayerInput.Triggers.Current.DpadMouseSnap3;
            vector2 = Vector2.UnitY;
            break;
          case 3:
            flag = PlayerInput.Triggers.Current.DpadMouseSnap4;
            vector2 = -Vector2.UnitX;
            break;
        }
        if (PlayerInput.DpadSnapCooldown[index] > 0)
          --PlayerInput.DpadSnapCooldown[index];
        if (flag)
        {
          if (PlayerInput.DpadSnapCooldown[index] == 0)
          {
            int num = 6;
            if (ItemSlot.IsABuildingItem(player.inventory[player.selectedItem]))
              num = player.inventory[player.selectedItem].useTime;
            PlayerInput.DpadSnapCooldown[index] = num;
            zero += vector2;
          }
        }
        else
          PlayerInput.DpadSnapCooldown[index] = 0;
      }
      if (!(zero != Vector2.Zero))
        return;
      Main.SmartCursorWanted = false;
      Matrix zoomMatrix = Main.GameViewMatrix.ZoomMatrix;
      Matrix matrix1 = Matrix.Invert(zoomMatrix);
      Vector2 mouseScreen = Main.MouseScreen;
      Vector2.Transform(Main.screenPosition, matrix1);
      Matrix matrix2 = matrix1;
      Vector2 vector2_1 = Vector2.Transform((Vector2.Transform(mouseScreen, matrix2) + zero * new Vector2(16f) + Main.screenPosition).ToTileCoordinates().ToWorldCoordinates() - Main.screenPosition, zoomMatrix);
      PlayerInput.MouseX = (int) vector2_1.X;
      PlayerInput.MouseY = (int) vector2_1.Y;
      PlayerInput.SettingsForUI.SetCursorMode(CursorMode.Gamepad);
    }

    private static bool ShouldShowInstructionsForGamepad() => PlayerInput.UsingGamepad || PlayerInput.SteamDeckIsUsed;

    public static string ComposeInstructionsForGamepad()
    {
      string empty = string.Empty;
      InputMode key = InputMode.XBoxGamepad;
      if (Main.gameMenu || UILinkPointNavigator.Available)
        key = InputMode.XBoxGamepadUI;
      if (PlayerInput.InBuildingMode && !Main.gameMenu)
        key = InputMode.XBoxGamepad;
      KeyConfiguration inputMode = PlayerInput.CurrentProfile.InputModes[key];
      string str1;
      if (Main.mapFullscreen && !Main.gameMenu)
      {
        str1 = empty + "          " + PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.inter[118].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]) + PlayerInput.BuildCommand(Lang.inter[119].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"]);
        if (Main.netMode == 1 && Main.player[Main.myPlayer].HasItem(2997))
          str1 += PlayerInput.BuildCommand(Lang.inter[120].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
      }
      else if (key == InputMode.XBoxGamepadUI && !PlayerInput.InBuildingMode)
      {
        str1 = UILinkPointNavigator.GetInstructions();
      }
      else
      {
        string str2 = empty + PlayerInput.BuildCommand(Lang.misc[58].Value, false, inputMode.KeyStatus["Jump"]) + PlayerInput.BuildCommand(Lang.misc[59].Value, false, inputMode.KeyStatus["HotbarMinus"], inputMode.KeyStatus["HotbarPlus"]);
        if (PlayerInput.InBuildingMode)
          str2 += PlayerInput.BuildCommand(Lang.menu[6].Value, false, inputMode.KeyStatus["Inventory"], inputMode.KeyStatus["MouseRight"]);
        if (WiresUI.Open)
        {
          str1 = str2 + PlayerInput.BuildCommand(Lang.misc[53].Value, false, inputMode.KeyStatus["MouseLeft"]) + PlayerInput.BuildCommand(Lang.misc[56].Value, false, inputMode.KeyStatus["MouseRight"]);
        }
        else
        {
          Item obj = Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem];
          if (obj.damage > 0 && obj.ammo == 0)
            str1 = str2 + PlayerInput.BuildCommand(Lang.misc[60].Value, false, inputMode.KeyStatus["MouseLeft"]);
          else if (obj.createTile >= 0 || obj.createWall > 0)
            str1 = str2 + PlayerInput.BuildCommand(Lang.misc[61].Value, false, inputMode.KeyStatus["MouseLeft"]);
          else
            str1 = str2 + PlayerInput.BuildCommand(Lang.misc[63].Value, false, inputMode.KeyStatus["MouseLeft"]);
          bool flag1 = true;
          bool flag2 = Main.SmartInteractProj != -1 || Main.HasInteractibleObjectThatIsNotATile;
          bool flag3 = !Main.SmartInteractShowingGenuine && Main.SmartInteractShowingFake;
          if (((Main.SmartInteractShowingGenuine ? 1 : (Main.SmartInteractShowingFake ? 1 : 0)) | (flag2 ? 1 : 0)) != 0)
          {
            if (Main.SmartInteractNPC != -1)
            {
              if (flag3)
                flag1 = false;
              str1 += PlayerInput.BuildCommand(Lang.misc[80].Value, false, inputMode.KeyStatus["MouseRight"]);
            }
            else if (flag2)
            {
              if (flag3)
                flag1 = false;
              str1 += PlayerInput.BuildCommand(Lang.misc[79].Value, false, inputMode.KeyStatus["MouseRight"]);
            }
            else if (Main.SmartInteractX != -1 && Main.SmartInteractY != -1)
            {
              if (flag3)
                flag1 = false;
              Tile tile = Main.tile[Main.SmartInteractX, Main.SmartInteractY];
              if (TileID.Sets.TileInteractRead[(int) tile.type])
                str1 += PlayerInput.BuildCommand(Lang.misc[81].Value, false, inputMode.KeyStatus["MouseRight"]);
              else
                str1 += PlayerInput.BuildCommand(Lang.misc[79].Value, false, inputMode.KeyStatus["MouseRight"]);
            }
          }
          else if (WiresUI.Settings.DrawToolModeUI)
            str1 += PlayerInput.BuildCommand(Lang.misc[89].Value, false, inputMode.KeyStatus["MouseRight"]);
          if ((!PlayerInput.GrappleAndInteractAreShared || !WiresUI.Settings.DrawToolModeUI && (!Main.SmartInteractShowingGenuine || !Main.HasSmartInteractTarget) && (!Main.SmartInteractShowingFake || flag1)) && Main.LocalPlayer.QuickGrapple_GetItemToUse() != null)
            str1 += PlayerInput.BuildCommand(Lang.misc[57].Value, false, inputMode.KeyStatus["Grapple"]);
        }
      }
      return str1;
    }

    public static string BuildCommand(
      string CommandText,
      bool Last,
      params List<string>[] Bindings)
    {
      string str1 = "";
      if (Bindings.Length == 0)
        return str1;
      string str2 = str1 + PlayerInput.GenerateGlyphList(Bindings[0]);
      for (int index = 1; index < Bindings.Length; ++index)
      {
        string glyphList = PlayerInput.GenerateGlyphList(Bindings[index]);
        if (glyphList.Length > 0)
          str2 = str2 + "/" + glyphList;
      }
      if (str2.Length > 0)
      {
        str2 = str2 + ": " + CommandText;
        if (!Last)
          str2 += "   ";
      }
      return str2;
    }

    public static string GenerateInputTag_ForCurrentGamemode_WithHacks(
      bool tagForGameplay,
      string triggerName)
    {
      InputMode inputMode = PlayerInput.CurrentInputMode;
      switch (inputMode)
      {
        case InputMode.KeyboardUI:
        case InputMode.Mouse:
          inputMode = InputMode.Keyboard;
          break;
      }
      if (!(triggerName == "SmartSelect"))
      {
        if (triggerName == "SmartCursor" && inputMode == InputMode.Keyboard)
          return PlayerInput.GenerateRawInputList(new List<string>()
          {
            Keys.LeftAlt.ToString()
          });
      }
      else if (inputMode == InputMode.Keyboard)
        return PlayerInput.GenerateRawInputList(new List<string>()
        {
          Keys.LeftControl.ToString()
        });
      return PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay, triggerName);
    }

    public static string GenerateInputTag_ForCurrentGamemode(
      bool tagForGameplay,
      string triggerName)
    {
      InputMode key = PlayerInput.CurrentInputMode;
      switch (key)
      {
        case InputMode.KeyboardUI:
        case InputMode.Mouse:
          key = InputMode.Keyboard;
          break;
      }
      if (tagForGameplay)
      {
        switch (key)
        {
          case InputMode.XBoxGamepad:
          case InputMode.XBoxGamepadUI:
            return PlayerInput.GenerateGlyphList(PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus[triggerName]);
          default:
            return PlayerInput.GenerateRawInputList(PlayerInput.CurrentProfile.InputModes[key].KeyStatus[triggerName]);
        }
      }
      else
      {
        switch (key)
        {
          case InputMode.XBoxGamepad:
          case InputMode.XBoxGamepadUI:
            return PlayerInput.GenerateGlyphList(PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus[triggerName]);
          default:
            return PlayerInput.GenerateRawInputList(PlayerInput.CurrentProfile.InputModes[key].KeyStatus[triggerName]);
        }
      }
    }

    public static string GenerateInputTags_GamepadUI(string triggerName) => PlayerInput.GenerateGlyphList(PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepadUI].KeyStatus[triggerName]);

    public static string GenerateInputTags_Gamepad(string triggerName) => PlayerInput.GenerateGlyphList(PlayerInput.CurrentProfile.InputModes[InputMode.XBoxGamepad].KeyStatus[triggerName]);

    private static string GenerateGlyphList(List<string> list)
    {
      if (list.Count == 0)
        return "";
      string glyphList = GlyphTagHandler.GenerateTag(list[0]);
      for (int index = 1; index < list.Count; ++index)
        glyphList = glyphList + "/" + GlyphTagHandler.GenerateTag(list[index]);
      return glyphList;
    }

    private static string GenerateRawInputList(List<string> list)
    {
      if (list.Count == 0)
        return "";
      string rawInputList = list[0];
      for (int index = 1; index < list.Count; ++index)
        rawInputList = rawInputList + "/" + list[index];
      return rawInputList;
    }

    public static void NavigatorCachePosition()
    {
      PlayerInput.PreUIX = PlayerInput.MouseX;
      PlayerInput.PreUIY = PlayerInput.MouseY;
    }

    public static void NavigatorUnCachePosition()
    {
      PlayerInput.MouseX = PlayerInput.PreUIX;
      PlayerInput.MouseY = PlayerInput.PreUIY;
    }

    public static void LockOnCachePosition()
    {
      PlayerInput.PreLockOnX = PlayerInput.MouseX;
      PlayerInput.PreLockOnY = PlayerInput.MouseY;
    }

    public static void LockOnUnCachePosition()
    {
      PlayerInput.MouseX = PlayerInput.PreLockOnX;
      PlayerInput.MouseY = PlayerInput.PreLockOnY;
    }

    public static void PrettyPrintProfiles(ref string text)
    {
      string str1 = text;
      string[] separator = new string[1]{ "\r\n" };
      foreach (string str2 in str1.Split(separator, StringSplitOptions.None))
      {
        if (str2.Contains(": {"))
        {
          string str3 = str2.Substring(0, str2.IndexOf('"'));
          string oldValue = str2 + "\r\n  ";
          string newValue = oldValue.Replace(": {\r\n  ", ": \r\n" + str3 + "{\r\n  ");
          text = text.Replace(oldValue, newValue);
        }
      }
      text = text.Replace("[\r\n        ", "[");
      text = text.Replace("[\r\n      ", "[");
      text = text.Replace("\"\r\n      ", "\"");
      text = text.Replace("\",\r\n        ", "\", ");
      text = text.Replace("\",\r\n      ", "\", ");
      text = text.Replace("\r\n    ]", "]");
    }

    public static void PrettyPrintProfilesOld(ref string text)
    {
      text = text.Replace(": {\r\n  ", ": \r\n  {\r\n  ");
      text = text.Replace("[\r\n      ", "[");
      text = text.Replace("\"\r\n      ", "\"");
      text = text.Replace("\",\r\n      ", "\", ");
      text = text.Replace("\r\n    ]", "]");
    }

    public static void Reset(KeyConfiguration c, PresetProfiles style, InputMode mode)
    {
      switch (style)
      {
        case PresetProfiles.Redigit:
          switch (mode)
          {
            case InputMode.Keyboard:
              c.KeyStatus["MouseLeft"].Add("Mouse1");
              c.KeyStatus["MouseRight"].Add("Mouse2");
              c.KeyStatus["Up"].Add("W");
              c.KeyStatus["Down"].Add("S");
              c.KeyStatus["Left"].Add("A");
              c.KeyStatus["Right"].Add("D");
              c.KeyStatus["Jump"].Add("Space");
              c.KeyStatus["Inventory"].Add("Escape");
              c.KeyStatus["Grapple"].Add("E");
              c.KeyStatus["SmartSelect"].Add("LeftShift");
              c.KeyStatus["SmartCursor"].Add("LeftControl");
              c.KeyStatus["QuickMount"].Add("R");
              c.KeyStatus["QuickHeal"].Add("H");
              c.KeyStatus["QuickMana"].Add("J");
              c.KeyStatus["QuickBuff"].Add("B");
              c.KeyStatus["MapStyle"].Add("Tab");
              c.KeyStatus["MapFull"].Add("M");
              c.KeyStatus["MapZoomIn"].Add("Add");
              c.KeyStatus["MapZoomOut"].Add("Subtract");
              c.KeyStatus["MapAlphaUp"].Add("PageUp");
              c.KeyStatus["MapAlphaDown"].Add("PageDown");
              c.KeyStatus["Hotbar1"].Add("D1");
              c.KeyStatus["Hotbar2"].Add("D2");
              c.KeyStatus["Hotbar3"].Add("D3");
              c.KeyStatus["Hotbar4"].Add("D4");
              c.KeyStatus["Hotbar5"].Add("D5");
              c.KeyStatus["Hotbar6"].Add("D6");
              c.KeyStatus["Hotbar7"].Add("D7");
              c.KeyStatus["Hotbar8"].Add("D8");
              c.KeyStatus["Hotbar9"].Add("D9");
              c.KeyStatus["Hotbar10"].Add("D0");
              c.KeyStatus["ViewZoomOut"].Add("OemMinus");
              c.KeyStatus["ViewZoomIn"].Add("OemPlus");
              c.KeyStatus["ToggleCreativeMenu"].Add("C");
              return;
            case InputMode.KeyboardUI:
              c.KeyStatus["MouseLeft"].Add("Mouse1");
              c.KeyStatus["MouseLeft"].Add("Space");
              c.KeyStatus["MouseRight"].Add("Mouse2");
              c.KeyStatus["Up"].Add("W");
              c.KeyStatus["Up"].Add("Up");
              c.KeyStatus["Down"].Add("S");
              c.KeyStatus["Down"].Add("Down");
              c.KeyStatus["Left"].Add("A");
              c.KeyStatus["Left"].Add("Left");
              c.KeyStatus["Right"].Add("D");
              c.KeyStatus["Right"].Add("Right");
              c.KeyStatus["Inventory"].Add(Keys.Escape.ToString());
              c.KeyStatus["MenuUp"].Add(Buttons.DPadUp.ToString() ?? "");
              List<string> keyStatu1 = c.KeyStatus["MenuDown"];
              Buttons buttons1 = Buttons.DPadDown;
              string str1 = buttons1.ToString() ?? "";
              keyStatu1.Add(str1);
              List<string> keyStatu2 = c.KeyStatus["MenuLeft"];
              buttons1 = Buttons.DPadLeft;
              string str2 = buttons1.ToString() ?? "";
              keyStatu2.Add(str2);
              List<string> keyStatu3 = c.KeyStatus["MenuRight"];
              buttons1 = Buttons.DPadRight;
              string str3 = buttons1.ToString() ?? "";
              keyStatu3.Add(str3);
              return;
            case InputMode.Mouse:
              return;
            case InputMode.XBoxGamepad:
              List<string> keyStatu4 = c.KeyStatus["MouseLeft"];
              Buttons buttons2 = Buttons.RightTrigger;
              string str4 = buttons2.ToString() ?? "";
              keyStatu4.Add(str4);
              List<string> keyStatu5 = c.KeyStatus["MouseRight"];
              buttons2 = Buttons.B;
              string str5 = buttons2.ToString() ?? "";
              keyStatu5.Add(str5);
              List<string> keyStatu6 = c.KeyStatus["Up"];
              buttons2 = Buttons.LeftThumbstickUp;
              string str6 = buttons2.ToString() ?? "";
              keyStatu6.Add(str6);
              List<string> keyStatu7 = c.KeyStatus["Down"];
              buttons2 = Buttons.LeftThumbstickDown;
              string str7 = buttons2.ToString() ?? "";
              keyStatu7.Add(str7);
              List<string> keyStatu8 = c.KeyStatus["Left"];
              buttons2 = Buttons.LeftThumbstickLeft;
              string str8 = buttons2.ToString() ?? "";
              keyStatu8.Add(str8);
              List<string> keyStatu9 = c.KeyStatus["Right"];
              buttons2 = Buttons.LeftThumbstickRight;
              string str9 = buttons2.ToString() ?? "";
              keyStatu9.Add(str9);
              List<string> keyStatu10 = c.KeyStatus["Jump"];
              buttons2 = Buttons.LeftTrigger;
              string str10 = buttons2.ToString() ?? "";
              keyStatu10.Add(str10);
              List<string> keyStatu11 = c.KeyStatus["Inventory"];
              buttons2 = Buttons.Y;
              string str11 = buttons2.ToString() ?? "";
              keyStatu11.Add(str11);
              List<string> keyStatu12 = c.KeyStatus["Grapple"];
              buttons2 = Buttons.B;
              string str12 = buttons2.ToString() ?? "";
              keyStatu12.Add(str12);
              List<string> keyStatu13 = c.KeyStatus["LockOn"];
              buttons2 = Buttons.X;
              string str13 = buttons2.ToString() ?? "";
              keyStatu13.Add(str13);
              List<string> keyStatu14 = c.KeyStatus["QuickMount"];
              buttons2 = Buttons.A;
              string str14 = buttons2.ToString() ?? "";
              keyStatu14.Add(str14);
              List<string> keyStatu15 = c.KeyStatus["SmartSelect"];
              buttons2 = Buttons.RightStick;
              string str15 = buttons2.ToString() ?? "";
              keyStatu15.Add(str15);
              List<string> keyStatu16 = c.KeyStatus["SmartCursor"];
              buttons2 = Buttons.LeftStick;
              string str16 = buttons2.ToString() ?? "";
              keyStatu16.Add(str16);
              List<string> keyStatu17 = c.KeyStatus["HotbarMinus"];
              buttons2 = Buttons.LeftShoulder;
              string str17 = buttons2.ToString() ?? "";
              keyStatu17.Add(str17);
              List<string> keyStatu18 = c.KeyStatus["HotbarPlus"];
              buttons2 = Buttons.RightShoulder;
              string str18 = buttons2.ToString() ?? "";
              keyStatu18.Add(str18);
              List<string> keyStatu19 = c.KeyStatus["MapFull"];
              buttons2 = Buttons.Start;
              string str19 = buttons2.ToString() ?? "";
              keyStatu19.Add(str19);
              List<string> keyStatu20 = c.KeyStatus["DpadSnap1"];
              buttons2 = Buttons.DPadUp;
              string str20 = buttons2.ToString() ?? "";
              keyStatu20.Add(str20);
              List<string> keyStatu21 = c.KeyStatus["DpadSnap3"];
              buttons2 = Buttons.DPadDown;
              string str21 = buttons2.ToString() ?? "";
              keyStatu21.Add(str21);
              List<string> keyStatu22 = c.KeyStatus["DpadSnap4"];
              buttons2 = Buttons.DPadLeft;
              string str22 = buttons2.ToString() ?? "";
              keyStatu22.Add(str22);
              List<string> keyStatu23 = c.KeyStatus["DpadSnap2"];
              buttons2 = Buttons.DPadRight;
              string str23 = buttons2.ToString() ?? "";
              keyStatu23.Add(str23);
              List<string> keyStatu24 = c.KeyStatus["MapStyle"];
              buttons2 = Buttons.Back;
              string str24 = buttons2.ToString() ?? "";
              keyStatu24.Add(str24);
              return;
            case InputMode.XBoxGamepadUI:
              List<string> keyStatu25 = c.KeyStatus["MouseLeft"];
              Buttons buttons3 = Buttons.A;
              string str25 = buttons3.ToString() ?? "";
              keyStatu25.Add(str25);
              List<string> keyStatu26 = c.KeyStatus["MouseRight"];
              buttons3 = Buttons.LeftShoulder;
              string str26 = buttons3.ToString() ?? "";
              keyStatu26.Add(str26);
              List<string> keyStatu27 = c.KeyStatus["SmartCursor"];
              buttons3 = Buttons.RightShoulder;
              string str27 = buttons3.ToString() ?? "";
              keyStatu27.Add(str27);
              List<string> keyStatu28 = c.KeyStatus["Up"];
              buttons3 = Buttons.LeftThumbstickUp;
              string str28 = buttons3.ToString() ?? "";
              keyStatu28.Add(str28);
              List<string> keyStatu29 = c.KeyStatus["Down"];
              buttons3 = Buttons.LeftThumbstickDown;
              string str29 = buttons3.ToString() ?? "";
              keyStatu29.Add(str29);
              List<string> keyStatu30 = c.KeyStatus["Left"];
              buttons3 = Buttons.LeftThumbstickLeft;
              string str30 = buttons3.ToString() ?? "";
              keyStatu30.Add(str30);
              List<string> keyStatu31 = c.KeyStatus["Right"];
              buttons3 = Buttons.LeftThumbstickRight;
              string str31 = buttons3.ToString() ?? "";
              keyStatu31.Add(str31);
              List<string> keyStatu32 = c.KeyStatus["Inventory"];
              buttons3 = Buttons.B;
              string str32 = buttons3.ToString() ?? "";
              keyStatu32.Add(str32);
              List<string> keyStatu33 = c.KeyStatus["Inventory"];
              buttons3 = Buttons.Y;
              string str33 = buttons3.ToString() ?? "";
              keyStatu33.Add(str33);
              List<string> keyStatu34 = c.KeyStatus["HotbarMinus"];
              buttons3 = Buttons.LeftTrigger;
              string str34 = buttons3.ToString() ?? "";
              keyStatu34.Add(str34);
              List<string> keyStatu35 = c.KeyStatus["HotbarPlus"];
              buttons3 = Buttons.RightTrigger;
              string str35 = buttons3.ToString() ?? "";
              keyStatu35.Add(str35);
              List<string> keyStatu36 = c.KeyStatus["Grapple"];
              buttons3 = Buttons.X;
              string str36 = buttons3.ToString() ?? "";
              keyStatu36.Add(str36);
              List<string> keyStatu37 = c.KeyStatus["MapFull"];
              buttons3 = Buttons.Start;
              string str37 = buttons3.ToString() ?? "";
              keyStatu37.Add(str37);
              List<string> keyStatu38 = c.KeyStatus["SmartSelect"];
              buttons3 = Buttons.Back;
              string str38 = buttons3.ToString() ?? "";
              keyStatu38.Add(str38);
              List<string> keyStatu39 = c.KeyStatus["QuickMount"];
              buttons3 = Buttons.RightStick;
              string str39 = buttons3.ToString() ?? "";
              keyStatu39.Add(str39);
              List<string> keyStatu40 = c.KeyStatus["DpadSnap1"];
              buttons3 = Buttons.DPadUp;
              string str40 = buttons3.ToString() ?? "";
              keyStatu40.Add(str40);
              List<string> keyStatu41 = c.KeyStatus["DpadSnap3"];
              buttons3 = Buttons.DPadDown;
              string str41 = buttons3.ToString() ?? "";
              keyStatu41.Add(str41);
              List<string> keyStatu42 = c.KeyStatus["DpadSnap4"];
              buttons3 = Buttons.DPadLeft;
              string str42 = buttons3.ToString() ?? "";
              keyStatu42.Add(str42);
              List<string> keyStatu43 = c.KeyStatus["DpadSnap2"];
              buttons3 = Buttons.DPadRight;
              string str43 = buttons3.ToString() ?? "";
              keyStatu43.Add(str43);
              List<string> keyStatu44 = c.KeyStatus["MenuUp"];
              buttons3 = Buttons.DPadUp;
              string str44 = buttons3.ToString() ?? "";
              keyStatu44.Add(str44);
              List<string> keyStatu45 = c.KeyStatus["MenuDown"];
              buttons3 = Buttons.DPadDown;
              string str45 = buttons3.ToString() ?? "";
              keyStatu45.Add(str45);
              List<string> keyStatu46 = c.KeyStatus["MenuLeft"];
              buttons3 = Buttons.DPadLeft;
              string str46 = buttons3.ToString() ?? "";
              keyStatu46.Add(str46);
              List<string> keyStatu47 = c.KeyStatus["MenuRight"];
              buttons3 = Buttons.DPadRight;
              string str47 = buttons3.ToString() ?? "";
              keyStatu47.Add(str47);
              return;
            default:
              return;
          }
        case PresetProfiles.Yoraiz0r:
          switch (mode)
          {
            case InputMode.Keyboard:
              c.KeyStatus["MouseLeft"].Add("Mouse1");
              c.KeyStatus["MouseRight"].Add("Mouse2");
              c.KeyStatus["Up"].Add("W");
              c.KeyStatus["Down"].Add("S");
              c.KeyStatus["Left"].Add("A");
              c.KeyStatus["Right"].Add("D");
              c.KeyStatus["Jump"].Add("Space");
              c.KeyStatus["Inventory"].Add("Escape");
              c.KeyStatus["Grapple"].Add("E");
              c.KeyStatus["SmartSelect"].Add("LeftShift");
              c.KeyStatus["SmartCursor"].Add("LeftControl");
              c.KeyStatus["QuickMount"].Add("R");
              c.KeyStatus["QuickHeal"].Add("H");
              c.KeyStatus["QuickMana"].Add("J");
              c.KeyStatus["QuickBuff"].Add("B");
              c.KeyStatus["MapStyle"].Add("Tab");
              c.KeyStatus["MapFull"].Add("M");
              c.KeyStatus["MapZoomIn"].Add("Add");
              c.KeyStatus["MapZoomOut"].Add("Subtract");
              c.KeyStatus["MapAlphaUp"].Add("PageUp");
              c.KeyStatus["MapAlphaDown"].Add("PageDown");
              c.KeyStatus["Hotbar1"].Add("D1");
              c.KeyStatus["Hotbar2"].Add("D2");
              c.KeyStatus["Hotbar3"].Add("D3");
              c.KeyStatus["Hotbar4"].Add("D4");
              c.KeyStatus["Hotbar5"].Add("D5");
              c.KeyStatus["Hotbar6"].Add("D6");
              c.KeyStatus["Hotbar7"].Add("D7");
              c.KeyStatus["Hotbar8"].Add("D8");
              c.KeyStatus["Hotbar9"].Add("D9");
              c.KeyStatus["Hotbar10"].Add("D0");
              c.KeyStatus["ViewZoomOut"].Add("OemMinus");
              c.KeyStatus["ViewZoomIn"].Add("OemPlus");
              c.KeyStatus["ToggleCreativeMenu"].Add("C");
              return;
            case InputMode.KeyboardUI:
              c.KeyStatus["MouseLeft"].Add("Mouse1");
              c.KeyStatus["MouseLeft"].Add("Space");
              c.KeyStatus["MouseRight"].Add("Mouse2");
              c.KeyStatus["Up"].Add("W");
              c.KeyStatus["Up"].Add("Up");
              c.KeyStatus["Down"].Add("S");
              c.KeyStatus["Down"].Add("Down");
              c.KeyStatus["Left"].Add("A");
              c.KeyStatus["Left"].Add("Left");
              c.KeyStatus["Right"].Add("D");
              c.KeyStatus["Right"].Add("Right");
              c.KeyStatus["Inventory"].Add(Keys.Escape.ToString());
              c.KeyStatus["MenuUp"].Add(Buttons.DPadUp.ToString() ?? "");
              List<string> keyStatu48 = c.KeyStatus["MenuDown"];
              Buttons buttons4 = Buttons.DPadDown;
              string str48 = buttons4.ToString() ?? "";
              keyStatu48.Add(str48);
              List<string> keyStatu49 = c.KeyStatus["MenuLeft"];
              buttons4 = Buttons.DPadLeft;
              string str49 = buttons4.ToString() ?? "";
              keyStatu49.Add(str49);
              List<string> keyStatu50 = c.KeyStatus["MenuRight"];
              buttons4 = Buttons.DPadRight;
              string str50 = buttons4.ToString() ?? "";
              keyStatu50.Add(str50);
              return;
            case InputMode.Mouse:
              return;
            case InputMode.XBoxGamepad:
              List<string> keyStatu51 = c.KeyStatus["MouseLeft"];
              Buttons buttons5 = Buttons.RightTrigger;
              string str51 = buttons5.ToString() ?? "";
              keyStatu51.Add(str51);
              List<string> keyStatu52 = c.KeyStatus["MouseRight"];
              buttons5 = Buttons.B;
              string str52 = buttons5.ToString() ?? "";
              keyStatu52.Add(str52);
              List<string> keyStatu53 = c.KeyStatus["Up"];
              buttons5 = Buttons.LeftThumbstickUp;
              string str53 = buttons5.ToString() ?? "";
              keyStatu53.Add(str53);
              List<string> keyStatu54 = c.KeyStatus["Down"];
              buttons5 = Buttons.LeftThumbstickDown;
              string str54 = buttons5.ToString() ?? "";
              keyStatu54.Add(str54);
              List<string> keyStatu55 = c.KeyStatus["Left"];
              buttons5 = Buttons.LeftThumbstickLeft;
              string str55 = buttons5.ToString() ?? "";
              keyStatu55.Add(str55);
              List<string> keyStatu56 = c.KeyStatus["Right"];
              buttons5 = Buttons.LeftThumbstickRight;
              string str56 = buttons5.ToString() ?? "";
              keyStatu56.Add(str56);
              List<string> keyStatu57 = c.KeyStatus["Jump"];
              buttons5 = Buttons.LeftTrigger;
              string str57 = buttons5.ToString() ?? "";
              keyStatu57.Add(str57);
              List<string> keyStatu58 = c.KeyStatus["Inventory"];
              buttons5 = Buttons.Y;
              string str58 = buttons5.ToString() ?? "";
              keyStatu58.Add(str58);
              List<string> keyStatu59 = c.KeyStatus["Grapple"];
              buttons5 = Buttons.LeftShoulder;
              string str59 = buttons5.ToString() ?? "";
              keyStatu59.Add(str59);
              List<string> keyStatu60 = c.KeyStatus["SmartSelect"];
              buttons5 = Buttons.LeftStick;
              string str60 = buttons5.ToString() ?? "";
              keyStatu60.Add(str60);
              List<string> keyStatu61 = c.KeyStatus["SmartCursor"];
              buttons5 = Buttons.RightStick;
              string str61 = buttons5.ToString() ?? "";
              keyStatu61.Add(str61);
              List<string> keyStatu62 = c.KeyStatus["QuickMount"];
              buttons5 = Buttons.X;
              string str62 = buttons5.ToString() ?? "";
              keyStatu62.Add(str62);
              List<string> keyStatu63 = c.KeyStatus["QuickHeal"];
              buttons5 = Buttons.A;
              string str63 = buttons5.ToString() ?? "";
              keyStatu63.Add(str63);
              List<string> keyStatu64 = c.KeyStatus["RadialHotbar"];
              buttons5 = Buttons.RightShoulder;
              string str64 = buttons5.ToString() ?? "";
              keyStatu64.Add(str64);
              List<string> keyStatu65 = c.KeyStatus["MapFull"];
              buttons5 = Buttons.Start;
              string str65 = buttons5.ToString() ?? "";
              keyStatu65.Add(str65);
              List<string> keyStatu66 = c.KeyStatus["DpadSnap1"];
              buttons5 = Buttons.DPadUp;
              string str66 = buttons5.ToString() ?? "";
              keyStatu66.Add(str66);
              List<string> keyStatu67 = c.KeyStatus["DpadSnap3"];
              buttons5 = Buttons.DPadDown;
              string str67 = buttons5.ToString() ?? "";
              keyStatu67.Add(str67);
              List<string> keyStatu68 = c.KeyStatus["DpadSnap4"];
              buttons5 = Buttons.DPadLeft;
              string str68 = buttons5.ToString() ?? "";
              keyStatu68.Add(str68);
              List<string> keyStatu69 = c.KeyStatus["DpadSnap2"];
              buttons5 = Buttons.DPadRight;
              string str69 = buttons5.ToString() ?? "";
              keyStatu69.Add(str69);
              List<string> keyStatu70 = c.KeyStatus["MapStyle"];
              buttons5 = Buttons.Back;
              string str70 = buttons5.ToString() ?? "";
              keyStatu70.Add(str70);
              return;
            case InputMode.XBoxGamepadUI:
              List<string> keyStatu71 = c.KeyStatus["MouseLeft"];
              Buttons buttons6 = Buttons.A;
              string str71 = buttons6.ToString() ?? "";
              keyStatu71.Add(str71);
              List<string> keyStatu72 = c.KeyStatus["MouseRight"];
              buttons6 = Buttons.LeftShoulder;
              string str72 = buttons6.ToString() ?? "";
              keyStatu72.Add(str72);
              List<string> keyStatu73 = c.KeyStatus["SmartCursor"];
              buttons6 = Buttons.RightShoulder;
              string str73 = buttons6.ToString() ?? "";
              keyStatu73.Add(str73);
              List<string> keyStatu74 = c.KeyStatus["Up"];
              buttons6 = Buttons.LeftThumbstickUp;
              string str74 = buttons6.ToString() ?? "";
              keyStatu74.Add(str74);
              List<string> keyStatu75 = c.KeyStatus["Down"];
              buttons6 = Buttons.LeftThumbstickDown;
              string str75 = buttons6.ToString() ?? "";
              keyStatu75.Add(str75);
              List<string> keyStatu76 = c.KeyStatus["Left"];
              buttons6 = Buttons.LeftThumbstickLeft;
              string str76 = buttons6.ToString() ?? "";
              keyStatu76.Add(str76);
              List<string> keyStatu77 = c.KeyStatus["Right"];
              buttons6 = Buttons.LeftThumbstickRight;
              string str77 = buttons6.ToString() ?? "";
              keyStatu77.Add(str77);
              List<string> keyStatu78 = c.KeyStatus["LockOn"];
              buttons6 = Buttons.B;
              string str78 = buttons6.ToString() ?? "";
              keyStatu78.Add(str78);
              List<string> keyStatu79 = c.KeyStatus["Inventory"];
              buttons6 = Buttons.Y;
              string str79 = buttons6.ToString() ?? "";
              keyStatu79.Add(str79);
              List<string> keyStatu80 = c.KeyStatus["HotbarMinus"];
              buttons6 = Buttons.LeftTrigger;
              string str80 = buttons6.ToString() ?? "";
              keyStatu80.Add(str80);
              List<string> keyStatu81 = c.KeyStatus["HotbarPlus"];
              buttons6 = Buttons.RightTrigger;
              string str81 = buttons6.ToString() ?? "";
              keyStatu81.Add(str81);
              List<string> keyStatu82 = c.KeyStatus["Grapple"];
              buttons6 = Buttons.X;
              string str82 = buttons6.ToString() ?? "";
              keyStatu82.Add(str82);
              List<string> keyStatu83 = c.KeyStatus["MapFull"];
              buttons6 = Buttons.Start;
              string str83 = buttons6.ToString() ?? "";
              keyStatu83.Add(str83);
              List<string> keyStatu84 = c.KeyStatus["SmartSelect"];
              buttons6 = Buttons.Back;
              string str84 = buttons6.ToString() ?? "";
              keyStatu84.Add(str84);
              List<string> keyStatu85 = c.KeyStatus["QuickMount"];
              buttons6 = Buttons.RightStick;
              string str85 = buttons6.ToString() ?? "";
              keyStatu85.Add(str85);
              List<string> keyStatu86 = c.KeyStatus["DpadSnap1"];
              buttons6 = Buttons.DPadUp;
              string str86 = buttons6.ToString() ?? "";
              keyStatu86.Add(str86);
              List<string> keyStatu87 = c.KeyStatus["DpadSnap3"];
              buttons6 = Buttons.DPadDown;
              string str87 = buttons6.ToString() ?? "";
              keyStatu87.Add(str87);
              List<string> keyStatu88 = c.KeyStatus["DpadSnap4"];
              buttons6 = Buttons.DPadLeft;
              string str88 = buttons6.ToString() ?? "";
              keyStatu88.Add(str88);
              List<string> keyStatu89 = c.KeyStatus["DpadSnap2"];
              buttons6 = Buttons.DPadRight;
              string str89 = buttons6.ToString() ?? "";
              keyStatu89.Add(str89);
              List<string> keyStatu90 = c.KeyStatus["MenuUp"];
              buttons6 = Buttons.DPadUp;
              string str90 = buttons6.ToString() ?? "";
              keyStatu90.Add(str90);
              List<string> keyStatu91 = c.KeyStatus["MenuDown"];
              buttons6 = Buttons.DPadDown;
              string str91 = buttons6.ToString() ?? "";
              keyStatu91.Add(str91);
              List<string> keyStatu92 = c.KeyStatus["MenuLeft"];
              buttons6 = Buttons.DPadLeft;
              string str92 = buttons6.ToString() ?? "";
              keyStatu92.Add(str92);
              List<string> keyStatu93 = c.KeyStatus["MenuRight"];
              buttons6 = Buttons.DPadRight;
              string str93 = buttons6.ToString() ?? "";
              keyStatu93.Add(str93);
              return;
            default:
              return;
          }
        case PresetProfiles.ConsolePS:
          switch (mode)
          {
            case InputMode.Keyboard:
              c.KeyStatus["MouseLeft"].Add("Mouse1");
              c.KeyStatus["MouseRight"].Add("Mouse2");
              c.KeyStatus["Up"].Add("W");
              c.KeyStatus["Down"].Add("S");
              c.KeyStatus["Left"].Add("A");
              c.KeyStatus["Right"].Add("D");
              c.KeyStatus["Jump"].Add("Space");
              c.KeyStatus["Inventory"].Add("Escape");
              c.KeyStatus["Grapple"].Add("E");
              c.KeyStatus["SmartSelect"].Add("LeftShift");
              c.KeyStatus["SmartCursor"].Add("LeftControl");
              c.KeyStatus["QuickMount"].Add("R");
              c.KeyStatus["QuickHeal"].Add("H");
              c.KeyStatus["QuickMana"].Add("J");
              c.KeyStatus["QuickBuff"].Add("B");
              c.KeyStatus["MapStyle"].Add("Tab");
              c.KeyStatus["MapFull"].Add("M");
              c.KeyStatus["MapZoomIn"].Add("Add");
              c.KeyStatus["MapZoomOut"].Add("Subtract");
              c.KeyStatus["MapAlphaUp"].Add("PageUp");
              c.KeyStatus["MapAlphaDown"].Add("PageDown");
              c.KeyStatus["Hotbar1"].Add("D1");
              c.KeyStatus["Hotbar2"].Add("D2");
              c.KeyStatus["Hotbar3"].Add("D3");
              c.KeyStatus["Hotbar4"].Add("D4");
              c.KeyStatus["Hotbar5"].Add("D5");
              c.KeyStatus["Hotbar6"].Add("D6");
              c.KeyStatus["Hotbar7"].Add("D7");
              c.KeyStatus["Hotbar8"].Add("D8");
              c.KeyStatus["Hotbar9"].Add("D9");
              c.KeyStatus["Hotbar10"].Add("D0");
              c.KeyStatus["ViewZoomOut"].Add("OemMinus");
              c.KeyStatus["ViewZoomIn"].Add("OemPlus");
              c.KeyStatus["ToggleCreativeMenu"].Add("C");
              return;
            case InputMode.KeyboardUI:
              c.KeyStatus["MouseLeft"].Add("Mouse1");
              c.KeyStatus["MouseLeft"].Add("Space");
              c.KeyStatus["MouseRight"].Add("Mouse2");
              c.KeyStatus["Up"].Add("W");
              c.KeyStatus["Up"].Add("Up");
              c.KeyStatus["Down"].Add("S");
              c.KeyStatus["Down"].Add("Down");
              c.KeyStatus["Left"].Add("A");
              c.KeyStatus["Left"].Add("Left");
              c.KeyStatus["Right"].Add("D");
              c.KeyStatus["Right"].Add("Right");
              c.KeyStatus["MenuUp"].Add(Buttons.DPadUp.ToString() ?? "");
              List<string> keyStatu94 = c.KeyStatus["MenuDown"];
              Buttons buttons7 = Buttons.DPadDown;
              string str94 = buttons7.ToString() ?? "";
              keyStatu94.Add(str94);
              List<string> keyStatu95 = c.KeyStatus["MenuLeft"];
              buttons7 = Buttons.DPadLeft;
              string str95 = buttons7.ToString() ?? "";
              keyStatu95.Add(str95);
              List<string> keyStatu96 = c.KeyStatus["MenuRight"];
              buttons7 = Buttons.DPadRight;
              string str96 = buttons7.ToString() ?? "";
              keyStatu96.Add(str96);
              c.KeyStatus["Inventory"].Add(Keys.Escape.ToString());
              return;
            case InputMode.Mouse:
              return;
            case InputMode.XBoxGamepad:
              List<string> keyStatu97 = c.KeyStatus["MouseLeft"];
              Buttons buttons8 = Buttons.RightShoulder;
              string str97 = buttons8.ToString() ?? "";
              keyStatu97.Add(str97);
              List<string> keyStatu98 = c.KeyStatus["MouseRight"];
              buttons8 = Buttons.B;
              string str98 = buttons8.ToString() ?? "";
              keyStatu98.Add(str98);
              List<string> keyStatu99 = c.KeyStatus["Up"];
              buttons8 = Buttons.LeftThumbstickUp;
              string str99 = buttons8.ToString() ?? "";
              keyStatu99.Add(str99);
              List<string> keyStatu100 = c.KeyStatus["Down"];
              buttons8 = Buttons.LeftThumbstickDown;
              string str100 = buttons8.ToString() ?? "";
              keyStatu100.Add(str100);
              List<string> keyStatu101 = c.KeyStatus["Left"];
              buttons8 = Buttons.LeftThumbstickLeft;
              string str101 = buttons8.ToString() ?? "";
              keyStatu101.Add(str101);
              List<string> keyStatu102 = c.KeyStatus["Right"];
              buttons8 = Buttons.LeftThumbstickRight;
              string str102 = buttons8.ToString() ?? "";
              keyStatu102.Add(str102);
              List<string> keyStatu103 = c.KeyStatus["Jump"];
              buttons8 = Buttons.A;
              string str103 = buttons8.ToString() ?? "";
              keyStatu103.Add(str103);
              List<string> keyStatu104 = c.KeyStatus["LockOn"];
              buttons8 = Buttons.X;
              string str104 = buttons8.ToString() ?? "";
              keyStatu104.Add(str104);
              List<string> keyStatu105 = c.KeyStatus["Inventory"];
              buttons8 = Buttons.Y;
              string str105 = buttons8.ToString() ?? "";
              keyStatu105.Add(str105);
              List<string> keyStatu106 = c.KeyStatus["Grapple"];
              buttons8 = Buttons.LeftShoulder;
              string str106 = buttons8.ToString() ?? "";
              keyStatu106.Add(str106);
              List<string> keyStatu107 = c.KeyStatus["SmartSelect"];
              buttons8 = Buttons.LeftStick;
              string str107 = buttons8.ToString() ?? "";
              keyStatu107.Add(str107);
              List<string> keyStatu108 = c.KeyStatus["SmartCursor"];
              buttons8 = Buttons.RightStick;
              string str108 = buttons8.ToString() ?? "";
              keyStatu108.Add(str108);
              List<string> keyStatu109 = c.KeyStatus["HotbarMinus"];
              buttons8 = Buttons.LeftTrigger;
              string str109 = buttons8.ToString() ?? "";
              keyStatu109.Add(str109);
              List<string> keyStatu110 = c.KeyStatus["HotbarPlus"];
              buttons8 = Buttons.RightTrigger;
              string str110 = buttons8.ToString() ?? "";
              keyStatu110.Add(str110);
              List<string> keyStatu111 = c.KeyStatus["MapFull"];
              buttons8 = Buttons.Start;
              string str111 = buttons8.ToString() ?? "";
              keyStatu111.Add(str111);
              List<string> keyStatu112 = c.KeyStatus["DpadRadial1"];
              buttons8 = Buttons.DPadUp;
              string str112 = buttons8.ToString() ?? "";
              keyStatu112.Add(str112);
              List<string> keyStatu113 = c.KeyStatus["DpadRadial3"];
              buttons8 = Buttons.DPadDown;
              string str113 = buttons8.ToString() ?? "";
              keyStatu113.Add(str113);
              List<string> keyStatu114 = c.KeyStatus["DpadRadial4"];
              buttons8 = Buttons.DPadLeft;
              string str114 = buttons8.ToString() ?? "";
              keyStatu114.Add(str114);
              List<string> keyStatu115 = c.KeyStatus["DpadRadial2"];
              buttons8 = Buttons.DPadRight;
              string str115 = buttons8.ToString() ?? "";
              keyStatu115.Add(str115);
              List<string> keyStatu116 = c.KeyStatus["QuickMount"];
              buttons8 = Buttons.Back;
              string str116 = buttons8.ToString() ?? "";
              keyStatu116.Add(str116);
              return;
            case InputMode.XBoxGamepadUI:
              List<string> keyStatu117 = c.KeyStatus["MouseLeft"];
              Buttons buttons9 = Buttons.A;
              string str117 = buttons9.ToString() ?? "";
              keyStatu117.Add(str117);
              List<string> keyStatu118 = c.KeyStatus["MouseRight"];
              buttons9 = Buttons.LeftShoulder;
              string str118 = buttons9.ToString() ?? "";
              keyStatu118.Add(str118);
              List<string> keyStatu119 = c.KeyStatus["SmartCursor"];
              buttons9 = Buttons.RightShoulder;
              string str119 = buttons9.ToString() ?? "";
              keyStatu119.Add(str119);
              List<string> keyStatu120 = c.KeyStatus["Up"];
              buttons9 = Buttons.LeftThumbstickUp;
              string str120 = buttons9.ToString() ?? "";
              keyStatu120.Add(str120);
              List<string> keyStatu121 = c.KeyStatus["Down"];
              buttons9 = Buttons.LeftThumbstickDown;
              string str121 = buttons9.ToString() ?? "";
              keyStatu121.Add(str121);
              List<string> keyStatu122 = c.KeyStatus["Left"];
              buttons9 = Buttons.LeftThumbstickLeft;
              string str122 = buttons9.ToString() ?? "";
              keyStatu122.Add(str122);
              List<string> keyStatu123 = c.KeyStatus["Right"];
              buttons9 = Buttons.LeftThumbstickRight;
              string str123 = buttons9.ToString() ?? "";
              keyStatu123.Add(str123);
              List<string> keyStatu124 = c.KeyStatus["Inventory"];
              buttons9 = Buttons.B;
              string str124 = buttons9.ToString() ?? "";
              keyStatu124.Add(str124);
              List<string> keyStatu125 = c.KeyStatus["Inventory"];
              buttons9 = Buttons.Y;
              string str125 = buttons9.ToString() ?? "";
              keyStatu125.Add(str125);
              List<string> keyStatu126 = c.KeyStatus["HotbarMinus"];
              buttons9 = Buttons.LeftTrigger;
              string str126 = buttons9.ToString() ?? "";
              keyStatu126.Add(str126);
              List<string> keyStatu127 = c.KeyStatus["HotbarPlus"];
              buttons9 = Buttons.RightTrigger;
              string str127 = buttons9.ToString() ?? "";
              keyStatu127.Add(str127);
              List<string> keyStatu128 = c.KeyStatus["Grapple"];
              buttons9 = Buttons.X;
              string str128 = buttons9.ToString() ?? "";
              keyStatu128.Add(str128);
              List<string> keyStatu129 = c.KeyStatus["MapFull"];
              buttons9 = Buttons.Start;
              string str129 = buttons9.ToString() ?? "";
              keyStatu129.Add(str129);
              List<string> keyStatu130 = c.KeyStatus["SmartSelect"];
              buttons9 = Buttons.Back;
              string str130 = buttons9.ToString() ?? "";
              keyStatu130.Add(str130);
              List<string> keyStatu131 = c.KeyStatus["QuickMount"];
              buttons9 = Buttons.RightStick;
              string str131 = buttons9.ToString() ?? "";
              keyStatu131.Add(str131);
              List<string> keyStatu132 = c.KeyStatus["DpadRadial1"];
              buttons9 = Buttons.DPadUp;
              string str132 = buttons9.ToString() ?? "";
              keyStatu132.Add(str132);
              List<string> keyStatu133 = c.KeyStatus["DpadRadial3"];
              buttons9 = Buttons.DPadDown;
              string str133 = buttons9.ToString() ?? "";
              keyStatu133.Add(str133);
              List<string> keyStatu134 = c.KeyStatus["DpadRadial4"];
              buttons9 = Buttons.DPadLeft;
              string str134 = buttons9.ToString() ?? "";
              keyStatu134.Add(str134);
              List<string> keyStatu135 = c.KeyStatus["DpadRadial2"];
              buttons9 = Buttons.DPadRight;
              string str135 = buttons9.ToString() ?? "";
              keyStatu135.Add(str135);
              List<string> keyStatu136 = c.KeyStatus["MenuUp"];
              buttons9 = Buttons.DPadUp;
              string str136 = buttons9.ToString() ?? "";
              keyStatu136.Add(str136);
              List<string> keyStatu137 = c.KeyStatus["MenuDown"];
              buttons9 = Buttons.DPadDown;
              string str137 = buttons9.ToString() ?? "";
              keyStatu137.Add(str137);
              List<string> keyStatu138 = c.KeyStatus["MenuLeft"];
              buttons9 = Buttons.DPadLeft;
              string str138 = buttons9.ToString() ?? "";
              keyStatu138.Add(str138);
              List<string> keyStatu139 = c.KeyStatus["MenuRight"];
              buttons9 = Buttons.DPadRight;
              string str139 = buttons9.ToString() ?? "";
              keyStatu139.Add(str139);
              return;
            default:
              return;
          }
        case PresetProfiles.ConsoleXBox:
          switch (mode)
          {
            case InputMode.Keyboard:
              c.KeyStatus["MouseLeft"].Add("Mouse1");
              c.KeyStatus["MouseRight"].Add("Mouse2");
              c.KeyStatus["Up"].Add("W");
              c.KeyStatus["Down"].Add("S");
              c.KeyStatus["Left"].Add("A");
              c.KeyStatus["Right"].Add("D");
              c.KeyStatus["Jump"].Add("Space");
              c.KeyStatus["Inventory"].Add("Escape");
              c.KeyStatus["Grapple"].Add("E");
              c.KeyStatus["SmartSelect"].Add("LeftShift");
              c.KeyStatus["SmartCursor"].Add("LeftControl");
              c.KeyStatus["QuickMount"].Add("R");
              c.KeyStatus["QuickHeal"].Add("H");
              c.KeyStatus["QuickMana"].Add("J");
              c.KeyStatus["QuickBuff"].Add("B");
              c.KeyStatus["MapStyle"].Add("Tab");
              c.KeyStatus["MapFull"].Add("M");
              c.KeyStatus["MapZoomIn"].Add("Add");
              c.KeyStatus["MapZoomOut"].Add("Subtract");
              c.KeyStatus["MapAlphaUp"].Add("PageUp");
              c.KeyStatus["MapAlphaDown"].Add("PageDown");
              c.KeyStatus["Hotbar1"].Add("D1");
              c.KeyStatus["Hotbar2"].Add("D2");
              c.KeyStatus["Hotbar3"].Add("D3");
              c.KeyStatus["Hotbar4"].Add("D4");
              c.KeyStatus["Hotbar5"].Add("D5");
              c.KeyStatus["Hotbar6"].Add("D6");
              c.KeyStatus["Hotbar7"].Add("D7");
              c.KeyStatus["Hotbar8"].Add("D8");
              c.KeyStatus["Hotbar9"].Add("D9");
              c.KeyStatus["Hotbar10"].Add("D0");
              c.KeyStatus["ViewZoomOut"].Add("OemMinus");
              c.KeyStatus["ViewZoomIn"].Add("OemPlus");
              c.KeyStatus["ToggleCreativeMenu"].Add("C");
              return;
            case InputMode.KeyboardUI:
              c.KeyStatus["MouseLeft"].Add("Mouse1");
              c.KeyStatus["MouseLeft"].Add("Space");
              c.KeyStatus["MouseRight"].Add("Mouse2");
              c.KeyStatus["Up"].Add("W");
              c.KeyStatus["Up"].Add("Up");
              c.KeyStatus["Down"].Add("S");
              c.KeyStatus["Down"].Add("Down");
              c.KeyStatus["Left"].Add("A");
              c.KeyStatus["Left"].Add("Left");
              c.KeyStatus["Right"].Add("D");
              c.KeyStatus["Right"].Add("Right");
              c.KeyStatus["MenuUp"].Add(Buttons.DPadUp.ToString() ?? "");
              List<string> keyStatu140 = c.KeyStatus["MenuDown"];
              Buttons buttons10 = Buttons.DPadDown;
              string str140 = buttons10.ToString() ?? "";
              keyStatu140.Add(str140);
              List<string> keyStatu141 = c.KeyStatus["MenuLeft"];
              buttons10 = Buttons.DPadLeft;
              string str141 = buttons10.ToString() ?? "";
              keyStatu141.Add(str141);
              List<string> keyStatu142 = c.KeyStatus["MenuRight"];
              buttons10 = Buttons.DPadRight;
              string str142 = buttons10.ToString() ?? "";
              keyStatu142.Add(str142);
              c.KeyStatus["Inventory"].Add(Keys.Escape.ToString());
              return;
            case InputMode.Mouse:
              return;
            case InputMode.XBoxGamepad:
              List<string> keyStatu143 = c.KeyStatus["MouseLeft"];
              Buttons buttons11 = Buttons.RightTrigger;
              string str143 = buttons11.ToString() ?? "";
              keyStatu143.Add(str143);
              List<string> keyStatu144 = c.KeyStatus["MouseRight"];
              buttons11 = Buttons.B;
              string str144 = buttons11.ToString() ?? "";
              keyStatu144.Add(str144);
              List<string> keyStatu145 = c.KeyStatus["Up"];
              buttons11 = Buttons.LeftThumbstickUp;
              string str145 = buttons11.ToString() ?? "";
              keyStatu145.Add(str145);
              List<string> keyStatu146 = c.KeyStatus["Down"];
              buttons11 = Buttons.LeftThumbstickDown;
              string str146 = buttons11.ToString() ?? "";
              keyStatu146.Add(str146);
              List<string> keyStatu147 = c.KeyStatus["Left"];
              buttons11 = Buttons.LeftThumbstickLeft;
              string str147 = buttons11.ToString() ?? "";
              keyStatu147.Add(str147);
              List<string> keyStatu148 = c.KeyStatus["Right"];
              buttons11 = Buttons.LeftThumbstickRight;
              string str148 = buttons11.ToString() ?? "";
              keyStatu148.Add(str148);
              List<string> keyStatu149 = c.KeyStatus["Jump"];
              buttons11 = Buttons.A;
              string str149 = buttons11.ToString() ?? "";
              keyStatu149.Add(str149);
              List<string> keyStatu150 = c.KeyStatus["LockOn"];
              buttons11 = Buttons.X;
              string str150 = buttons11.ToString() ?? "";
              keyStatu150.Add(str150);
              List<string> keyStatu151 = c.KeyStatus["Inventory"];
              buttons11 = Buttons.Y;
              string str151 = buttons11.ToString() ?? "";
              keyStatu151.Add(str151);
              List<string> keyStatu152 = c.KeyStatus["Grapple"];
              buttons11 = Buttons.LeftTrigger;
              string str152 = buttons11.ToString() ?? "";
              keyStatu152.Add(str152);
              List<string> keyStatu153 = c.KeyStatus["SmartSelect"];
              buttons11 = Buttons.LeftStick;
              string str153 = buttons11.ToString() ?? "";
              keyStatu153.Add(str153);
              List<string> keyStatu154 = c.KeyStatus["SmartCursor"];
              buttons11 = Buttons.RightStick;
              string str154 = buttons11.ToString() ?? "";
              keyStatu154.Add(str154);
              List<string> keyStatu155 = c.KeyStatus["HotbarMinus"];
              buttons11 = Buttons.LeftShoulder;
              string str155 = buttons11.ToString() ?? "";
              keyStatu155.Add(str155);
              List<string> keyStatu156 = c.KeyStatus["HotbarPlus"];
              buttons11 = Buttons.RightShoulder;
              string str156 = buttons11.ToString() ?? "";
              keyStatu156.Add(str156);
              List<string> keyStatu157 = c.KeyStatus["MapFull"];
              buttons11 = Buttons.Start;
              string str157 = buttons11.ToString() ?? "";
              keyStatu157.Add(str157);
              List<string> keyStatu158 = c.KeyStatus["DpadRadial1"];
              buttons11 = Buttons.DPadUp;
              string str158 = buttons11.ToString() ?? "";
              keyStatu158.Add(str158);
              List<string> keyStatu159 = c.KeyStatus["DpadRadial3"];
              buttons11 = Buttons.DPadDown;
              string str159 = buttons11.ToString() ?? "";
              keyStatu159.Add(str159);
              List<string> keyStatu160 = c.KeyStatus["DpadRadial4"];
              buttons11 = Buttons.DPadLeft;
              string str160 = buttons11.ToString() ?? "";
              keyStatu160.Add(str160);
              List<string> keyStatu161 = c.KeyStatus["DpadRadial2"];
              buttons11 = Buttons.DPadRight;
              string str161 = buttons11.ToString() ?? "";
              keyStatu161.Add(str161);
              List<string> keyStatu162 = c.KeyStatus["QuickMount"];
              buttons11 = Buttons.Back;
              string str162 = buttons11.ToString() ?? "";
              keyStatu162.Add(str162);
              return;
            case InputMode.XBoxGamepadUI:
              List<string> keyStatu163 = c.KeyStatus["MouseLeft"];
              Buttons buttons12 = Buttons.A;
              string str163 = buttons12.ToString() ?? "";
              keyStatu163.Add(str163);
              List<string> keyStatu164 = c.KeyStatus["MouseRight"];
              buttons12 = Buttons.LeftShoulder;
              string str164 = buttons12.ToString() ?? "";
              keyStatu164.Add(str164);
              List<string> keyStatu165 = c.KeyStatus["SmartCursor"];
              buttons12 = Buttons.RightShoulder;
              string str165 = buttons12.ToString() ?? "";
              keyStatu165.Add(str165);
              List<string> keyStatu166 = c.KeyStatus["Up"];
              buttons12 = Buttons.LeftThumbstickUp;
              string str166 = buttons12.ToString() ?? "";
              keyStatu166.Add(str166);
              List<string> keyStatu167 = c.KeyStatus["Down"];
              buttons12 = Buttons.LeftThumbstickDown;
              string str167 = buttons12.ToString() ?? "";
              keyStatu167.Add(str167);
              List<string> keyStatu168 = c.KeyStatus["Left"];
              buttons12 = Buttons.LeftThumbstickLeft;
              string str168 = buttons12.ToString() ?? "";
              keyStatu168.Add(str168);
              List<string> keyStatu169 = c.KeyStatus["Right"];
              buttons12 = Buttons.LeftThumbstickRight;
              string str169 = buttons12.ToString() ?? "";
              keyStatu169.Add(str169);
              List<string> keyStatu170 = c.KeyStatus["Inventory"];
              buttons12 = Buttons.B;
              string str170 = buttons12.ToString() ?? "";
              keyStatu170.Add(str170);
              List<string> keyStatu171 = c.KeyStatus["Inventory"];
              buttons12 = Buttons.Y;
              string str171 = buttons12.ToString() ?? "";
              keyStatu171.Add(str171);
              List<string> keyStatu172 = c.KeyStatus["HotbarMinus"];
              buttons12 = Buttons.LeftTrigger;
              string str172 = buttons12.ToString() ?? "";
              keyStatu172.Add(str172);
              List<string> keyStatu173 = c.KeyStatus["HotbarPlus"];
              buttons12 = Buttons.RightTrigger;
              string str173 = buttons12.ToString() ?? "";
              keyStatu173.Add(str173);
              List<string> keyStatu174 = c.KeyStatus["Grapple"];
              buttons12 = Buttons.X;
              string str174 = buttons12.ToString() ?? "";
              keyStatu174.Add(str174);
              List<string> keyStatu175 = c.KeyStatus["MapFull"];
              buttons12 = Buttons.Start;
              string str175 = buttons12.ToString() ?? "";
              keyStatu175.Add(str175);
              List<string> keyStatu176 = c.KeyStatus["SmartSelect"];
              buttons12 = Buttons.Back;
              string str176 = buttons12.ToString() ?? "";
              keyStatu176.Add(str176);
              List<string> keyStatu177 = c.KeyStatus["QuickMount"];
              buttons12 = Buttons.RightStick;
              string str177 = buttons12.ToString() ?? "";
              keyStatu177.Add(str177);
              List<string> keyStatu178 = c.KeyStatus["DpadRadial1"];
              buttons12 = Buttons.DPadUp;
              string str178 = buttons12.ToString() ?? "";
              keyStatu178.Add(str178);
              List<string> keyStatu179 = c.KeyStatus["DpadRadial3"];
              buttons12 = Buttons.DPadDown;
              string str179 = buttons12.ToString() ?? "";
              keyStatu179.Add(str179);
              List<string> keyStatu180 = c.KeyStatus["DpadRadial4"];
              buttons12 = Buttons.DPadLeft;
              string str180 = buttons12.ToString() ?? "";
              keyStatu180.Add(str180);
              List<string> keyStatu181 = c.KeyStatus["DpadRadial2"];
              buttons12 = Buttons.DPadRight;
              string str181 = buttons12.ToString() ?? "";
              keyStatu181.Add(str181);
              List<string> keyStatu182 = c.KeyStatus["MenuUp"];
              buttons12 = Buttons.DPadUp;
              string str182 = buttons12.ToString() ?? "";
              keyStatu182.Add(str182);
              List<string> keyStatu183 = c.KeyStatus["MenuDown"];
              buttons12 = Buttons.DPadDown;
              string str183 = buttons12.ToString() ?? "";
              keyStatu183.Add(str183);
              List<string> keyStatu184 = c.KeyStatus["MenuLeft"];
              buttons12 = Buttons.DPadLeft;
              string str184 = buttons12.ToString() ?? "";
              keyStatu184.Add(str184);
              List<string> keyStatu185 = c.KeyStatus["MenuRight"];
              buttons12 = Buttons.DPadRight;
              string str185 = buttons12.ToString() ?? "";
              keyStatu185.Add(str185);
              return;
            default:
              return;
          }
      }
    }

    public static void SetZoom_UI() => PlayerInput.SetZoom_Scaled(1f / Main.UIScale);

    public static void SetZoom_World()
    {
      PlayerInput.SetZoom_Scaled(1f);
      PlayerInput.SetZoom_MouseInWorld();
    }

    public static void SetZoom_Unscaled()
    {
      Main.lastMouseX = PlayerInput._originalLastMouseX;
      Main.lastMouseY = PlayerInput._originalLastMouseY;
      Main.mouseX = PlayerInput._originalMouseX;
      Main.mouseY = PlayerInput._originalMouseY;
      Main.screenWidth = PlayerInput._originalScreenWidth;
      Main.screenHeight = PlayerInput._originalScreenHeight;
    }

    public static void SetZoom_Test()
    {
      Vector2 vector2_1 = Main.screenPosition + new Vector2((float) Main.screenWidth, (float) Main.screenHeight) / 2f;
      Vector2 vector2_2 = Main.screenPosition + new Vector2((float) PlayerInput._originalMouseX, (float) PlayerInput._originalMouseY);
      Vector2 vector2_3 = Main.screenPosition + new Vector2((float) PlayerInput._originalLastMouseX, (float) PlayerInput._originalLastMouseY);
      Vector2 vector2_4 = Main.screenPosition + new Vector2(0.0f, 0.0f);
      Vector2 vector2_5 = Main.screenPosition + new Vector2((float) Main.screenWidth, (float) Main.screenHeight);
      Vector2 vector2_6 = vector2_2 - vector2_1;
      Vector2 vector2_7 = vector2_3 - vector2_1;
      Vector2 vector2_8 = vector2_4 - vector2_1;
      Vector2 vector2_9 = vector2_1;
      Vector2 vector2_10 = vector2_5 - vector2_9;
      float num1 = 1f / Main.GameViewMatrix.Zoom.X;
      float num2 = 1f;
      Vector2 vector2_11 = vector2_1 - Main.screenPosition + vector2_6 * num1;
      Vector2 vector2_12 = vector2_1 - Main.screenPosition + vector2_7 * num1;
      Vector2 vector2_13 = vector2_1 + vector2_8 * num2;
      Main.mouseX = (int) vector2_11.X;
      Main.mouseY = (int) vector2_11.Y;
      Main.lastMouseX = (int) vector2_12.X;
      Main.lastMouseY = (int) vector2_12.Y;
      Main.screenPosition = vector2_13;
      Main.screenWidth = (int) ((double) PlayerInput._originalScreenWidth * (double) num2);
      Main.screenHeight = (int) ((double) PlayerInput._originalScreenHeight * (double) num2);
    }

    public static void SetZoom_MouseInWorld()
    {
      Vector2 vector2_1 = Main.screenPosition + new Vector2((float) Main.screenWidth, (float) Main.screenHeight) / 2f;
      Vector2 vector2_2 = Main.screenPosition + new Vector2((float) PlayerInput._originalMouseX, (float) PlayerInput._originalMouseY);
      Vector2 vector2_3 = Main.screenPosition + new Vector2((float) PlayerInput._originalLastMouseX, (float) PlayerInput._originalLastMouseY);
      Vector2 vector2_4 = vector2_2 - vector2_1;
      Vector2 vector2_5 = vector2_1;
      Vector2 vector2_6 = vector2_3 - vector2_5;
      float num = 1f / Main.GameViewMatrix.Zoom.X;
      Vector2 vector2_7 = vector2_1 - Main.screenPosition + vector2_4 * num;
      Main.mouseX = (int) vector2_7.X;
      Main.mouseY = (int) vector2_7.Y;
      Vector2 vector2_8 = vector2_1 - Main.screenPosition + vector2_6 * num;
      Main.lastMouseX = (int) vector2_8.X;
      Main.lastMouseY = (int) vector2_8.Y;
    }

    public static void SetDesiredZoomContext(ZoomContext context) => PlayerInput._currentWantedZoom = context;

    public static void SetZoom_Context()
    {
      switch (PlayerInput._currentWantedZoom)
      {
        case ZoomContext.Unscaled:
          PlayerInput.SetZoom_Unscaled();
          Main.SetRecommendedZoomContext(Matrix.Identity);
          break;
        case ZoomContext.World:
          PlayerInput.SetZoom_World();
          Main.SetRecommendedZoomContext(Main.GameViewMatrix.ZoomMatrix);
          break;
        case ZoomContext.Unscaled_MouseInWorld:
          PlayerInput.SetZoom_Unscaled();
          PlayerInput.SetZoom_MouseInWorld();
          Main.SetRecommendedZoomContext(Main.GameViewMatrix.ZoomMatrix);
          break;
        case ZoomContext.UI:
          PlayerInput.SetZoom_UI();
          Main.SetRecommendedZoomContext(Main.UIScaleMatrix);
          break;
      }
    }

    private static void SetZoom_Scaled(float scale)
    {
      Main.lastMouseX = (int) ((double) PlayerInput._originalLastMouseX * (double) scale);
      Main.lastMouseY = (int) ((double) PlayerInput._originalLastMouseY * (double) scale);
      Main.mouseX = (int) ((double) PlayerInput._originalMouseX * (double) scale);
      Main.mouseY = (int) ((double) PlayerInput._originalMouseY * (double) scale);
      Main.screenWidth = (int) ((double) PlayerInput._originalScreenWidth * (double) scale);
      Main.screenHeight = (int) ((double) PlayerInput._originalScreenHeight * (double) scale);
    }

    public class MiscSettingsTEMP
    {
      public static bool HotbarRadialShouldBeUsed = true;
    }

    public static class SettingsForUI
    {
      public static CursorMode CurrentCursorMode { get; private set; }

      public static void SetCursorMode(CursorMode cursorMode)
      {
        PlayerInput.SettingsForUI.CurrentCursorMode = cursorMode;
        if (PlayerInput.SettingsForUI.CurrentCursorMode != CursorMode.Mouse)
          return;
        PlayerInput.SettingsForUI.FramesSinceLastTimeInMouseMode = 0;
      }

      public static bool ShowGamepadHints => PlayerInput.UsingGamepad || PlayerInput.SteamDeckIsUsed;

      public static bool AllowSecondaryGamepadAim => PlayerInput.SettingsForUI.CurrentCursorMode == CursorMode.Gamepad || !PlayerInput.SteamDeckIsUsed;

      public static bool PushEquipmentAreaUp => PlayerInput.UsingGamepad && !PlayerInput.SteamDeckIsUsed;

      public static bool ShowGamepadCursor => !PlayerInput.SteamDeckIsUsed ? PlayerInput.UsingGamepad : PlayerInput.SettingsForUI.CurrentCursorMode == CursorMode.Gamepad;

      public static bool HighlightThingsForMouse => !PlayerInput.UsingGamepadUI || PlayerInput.SettingsForUI.CurrentCursorMode == CursorMode.Mouse;

      public static int FramesSinceLastTimeInMouseMode { get; private set; }

      public static bool PreventHighlightsForGamepad => PlayerInput.SettingsForUI.FramesSinceLastTimeInMouseMode == 0;

      public static void UpdateCounters()
      {
        if (PlayerInput.SettingsForUI.CurrentCursorMode == CursorMode.Mouse)
          return;
        ++PlayerInput.SettingsForUI.FramesSinceLastTimeInMouseMode;
      }

      public static void TryRevertingToMouseMode()
      {
        if (PlayerInput.SettingsForUI.FramesSinceLastTimeInMouseMode > 0)
          return;
        PlayerInput.SettingsForUI.SetCursorMode(CursorMode.Mouse);
        PlayerInput.CurrentInputMode = InputMode.Mouse;
        PlayerInput.Triggers.Current.UsedMovementKey = false;
        PlayerInput.NavigatorUnCachePosition();
      }
    }
  }
}
