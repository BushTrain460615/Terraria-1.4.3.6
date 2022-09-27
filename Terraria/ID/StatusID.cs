// Decompiled with JetBrains decompiler
// Type: Terraria.ID.StatusID
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using ReLogic.Reflection;

namespace Terraria.ID
{
  public class StatusID
  {
    public const int Ok = 0;
    public const int LaterVersion = 1;
    public const int UnknownError = 2;
    public const int EmptyFile = 3;
    public const int DecryptionError = 4;
    public const int BadSectionPointer = 5;
    public const int BadFooter = 6;
    public static readonly IdDictionary Search = IdDictionary.Create<StatusID, int>();
  }
}
