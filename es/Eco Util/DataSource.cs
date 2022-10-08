// Decompiled with JetBrains decompiler
// Type: Eco.Util.DataSource
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;

namespace Eco.Util
{
  public class DataSource : IComparable
  {
    public string Name { get; set; }

    public SourceType SourceType { get; set; }

    public int CompareTo(object obj)
    {
      if (obj == null || !(obj is DataSource dataSource))
        return 1;
      if (this.SourceType == dataSource.SourceType)
        return this.Name.CompareTo(dataSource.Name);
      SourceType sourceType = this.SourceType;
      string str = sourceType.ToString();
      sourceType = dataSource.SourceType;
      string strB = sourceType.ToString();
      return str.CompareTo(strB);
    }

    public override string ToString() => Enum.IsDefined(typeof (SourceType), (object) this.SourceType) ? string.Format("{0}: {1}", (object) this.SourceType.ToString(), (object) this.Name) : string.Empty;
  }
}
