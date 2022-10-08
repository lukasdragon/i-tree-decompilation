// Decompiled with JetBrains decompiler
// Type: EcoIpedReportGenerator.IPEDInfo
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System.Collections.Generic;
using System.Data;

namespace EcoIpedReportGenerator
{
  public class IPEDInfo
  {
    public Dictionary<string, Dictionary<string, int>> Hit = new Dictionary<string, Dictionary<string, int>>();
    public Dictionary<string, Dictionary<string, int>> Miss = new Dictionary<string, Dictionary<string, int>>();
    public DataRow dr;
  }
}
