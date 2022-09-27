// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.GeneralIssueReporter
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;

namespace Terraria.DataStructures
{
  public class GeneralIssueReporter : IProvideReports
  {
    private List<IssueReport> _reports = new List<IssueReport>();

    public void AddReport(string textToShow) => this._reports.Add(new IssueReport(textToShow));

    public List<IssueReport> GetReports() => this._reports;
  }
}
