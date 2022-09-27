// Decompiled with JetBrains decompiler
// Type: Terraria.Localization.Language
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Text.RegularExpressions;
using Terraria.Utilities;

namespace Terraria.Localization
{
  public static class Language
  {
    public static GameCulture ActiveCulture => LanguageManager.Instance.ActiveCulture;

    public static LocalizedText GetText(string key) => LanguageManager.Instance.GetText(key);

    public static string GetTextValue(string key) => LanguageManager.Instance.GetTextValue(key);

    public static string GetTextValue(string key, object arg0) => LanguageManager.Instance.GetTextValue(key, arg0);

    public static string GetTextValue(string key, object arg0, object arg1) => LanguageManager.Instance.GetTextValue(key, arg0, arg1);

    public static string GetTextValue(string key, object arg0, object arg1, object arg2) => LanguageManager.Instance.GetTextValue(key, arg0, arg1, arg2);

    public static string GetTextValue(string key, params object[] args) => LanguageManager.Instance.GetTextValue(key, args);

    public static string GetTextValueWith(string key, object obj) => LanguageManager.Instance.GetText(key).FormatWith(obj);

    public static bool Exists(string key) => LanguageManager.Instance.Exists(key);

    public static int GetCategorySize(string key) => LanguageManager.Instance.GetCategorySize(key);

    public static LocalizedText[] FindAll(Regex regex) => LanguageManager.Instance.FindAll(regex);

    public static LocalizedText[] FindAll(LanguageSearchFilter filter) => LanguageManager.Instance.FindAll(filter);

    public static LocalizedText SelectRandom(
      LanguageSearchFilter filter,
      UnifiedRandom random = null)
    {
      return LanguageManager.Instance.SelectRandom(filter, random);
    }

    public static LocalizedText RandomFromCategory(
      string categoryName,
      UnifiedRandom random = null)
    {
      return LanguageManager.Instance.RandomFromCategory(categoryName, random);
    }
  }
}
