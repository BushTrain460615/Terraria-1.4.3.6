﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Localization.LocalizedText
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Terraria.Localization
{
  public class LocalizedText
  {
    public static readonly LocalizedText Empty = new LocalizedText("", "");
    private static Regex _substitutionRegex = new Regex("{(\\?(?:!)?)?([a-zA-Z][\\w\\.]*)}", RegexOptions.Compiled);
    public readonly string Key;

    public string Value { get; private set; }

    internal LocalizedText(string key, string text)
    {
      this.Key = key;
      this.Value = text;
    }

    internal void SetValue(string text) => this.Value = text;

    public string FormatWith(object obj)
    {
      string input = this.Value;
      PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
      return LocalizedText._substitutionRegex.Replace(input, (MatchEvaluator) (match =>
      {
        if (match.Groups[1].Length != 0)
          return "";
        PropertyDescriptor propertyDescriptor = properties.Find(match.Groups[2].ToString(), false);
        return propertyDescriptor == null ? "" : (propertyDescriptor.GetValue(obj) ?? (object) "").ToString();
      }));
    }

    public bool CanFormatWith(object obj)
    {
      PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
      foreach (Match match in LocalizedText._substitutionRegex.Matches(this.Value))
      {
        string name = match.Groups[2].ToString();
        PropertyDescriptor propertyDescriptor = properties.Find(name, false);
        if (propertyDescriptor == null)
          return false;
        object obj1 = propertyDescriptor.GetValue(obj);
        if (obj1 == null || match.Groups[1].Length != 0 && (obj1 as bool?).GetValueOrDefault() ^ match.Groups[1].Length == 1)
          return false;
      }
      return true;
    }

    public NetworkText ToNetworkText() => NetworkText.FromKey(this.Key);

    public NetworkText ToNetworkText(params object[] substitutions) => NetworkText.FromKey(this.Key, substitutions);

    public static explicit operator string(LocalizedText text) => text.Value;

    public string Format(object arg0) => string.Format(this.Value, arg0);

    public string Format(object arg0, object arg1) => string.Format(this.Value, arg0, arg1);

    public string Format(object arg0, object arg1, object arg2) => string.Format(this.Value, arg0, arg1, arg2);

    public string Format(params object[] args) => string.Format(this.Value, args);

    public override string ToString() => this.Value;
  }
}
