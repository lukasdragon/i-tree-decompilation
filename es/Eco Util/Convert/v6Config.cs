// Decompiled with JetBrains decompiler
// Type: Eco.Util.Convert.v6Config
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System.Collections.Generic;

namespace Eco.Util.Convert
{
  public class v6Config
  {
    private short? year;

    public v6Config()
    {
      this.FieldLandUseMap = new Dictionary<int, char>();
      this.SpListMap = new Dictionary<string, string>();
      this.PlantingSiteTypeMap = new Dictionary<string, double>();
      this.SeriesId = (string) null;
      this.year = new short?();
    }

    public Dictionary<int, char> FieldLandUseMap { get; set; }

    public Dictionary<string, string> SpListMap { get; set; }

    public Dictionary<string, double> PlantingSiteTypeMap { get; set; }

    public string SeriesId { get; set; }

    public short YearId
    {
      get => this.year.Value;
      set => this.year = new short?(value);
    }
  }
}
