// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.IssueReport
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;

namespace Terraria.DataStructures
{
  public class IssueReport
  {
    public DateTime timeReported;
    public string reportText;

    public IssueReport(string reportText)
    {
      this.timeReported = DateTime.Now;
      this.reportText = reportText;
    }
  }
}
