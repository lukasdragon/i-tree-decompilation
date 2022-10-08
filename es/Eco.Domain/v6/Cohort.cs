// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Cohort
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;

namespace Eco.Domain.v6
{
  public class Cohort : Entity<Cohort>
  {
    public virtual Forecast Forecast
    {
      get => this.\u003CForecast\u003Ek__BackingField;
      set
      {
        if (this.\u003CForecast\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Forecast));
        this.\u003CForecast\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Forecast);
      }
    }

    public virtual int ForecastedYear
    {
      get => this.\u003CForecastedYear\u003Ek__BackingField;
      set
      {
        if (this.\u003CForecastedYear\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (ForecastedYear));
        this.\u003CForecastedYear\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.ForecastedYear);
      }
    }

    public virtual Strata Stratum
    {
      get => this.\u003CStratum\u003Ek__BackingField;
      set
      {
        if (this.\u003CStratum\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Stratum));
        this.\u003CStratum\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Stratum);
      }
    }

    public virtual string Species
    {
      get => this.\u003CSpecies\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CSpecies\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Species));
        this.\u003CSpecies\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Species);
      }
    }

    public virtual Condition Condition
    {
      get => this.\u003CCondition\u003Ek__BackingField;
      set
      {
        if (this.\u003CCondition\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Condition));
        this.\u003CCondition\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Condition);
      }
    }

    public virtual int CrownLightExposure
    {
      get => this.\u003CCrownLightExposure\u003Ek__BackingField;
      set
      {
        if (this.\u003CCrownLightExposure\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (CrownLightExposure));
        this.\u003CCrownLightExposure\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.CrownLightExposure);
      }
    }

    public virtual long CohortTag
    {
      get => this.\u003CCohortTag\u003Ek__BackingField;
      set
      {
        if (this.\u003CCohortTag\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (CohortTag));
        this.\u003CCohortTag\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.CohortTag);
      }
    }

    public virtual Cohort Parent
    {
      get => this.\u003CParent\u003Ek__BackingField;
      set
      {
        if (this.\u003CParent\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Parent));
        this.\u003CParent\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Parent);
      }
    }

    public virtual int NumTrees
    {
      get => this.\u003CNumTrees\u003Ek__BackingField;
      set
      {
        if (this.\u003CNumTrees\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (NumTrees));
        this.\u003CNumTrees\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.NumTrees);
      }
    }

    public virtual Mortality Mortality
    {
      get => this.\u003CMortality\u003Ek__BackingField;
      set
      {
        if (this.\u003CMortality\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Mortality));
        this.\u003CMortality\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Mortality);
      }
    }

    public virtual double AvgDBH
    {
      get => this.\u003CAvgDBH\u003Ek__BackingField;
      set
      {
        if (this.\u003CAvgDBH\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (AvgDBH));
        this.\u003CAvgDBH\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.AvgDBH);
      }
    }

    public virtual double AvgTreeHeight
    {
      get => this.\u003CAvgTreeHeight\u003Ek__BackingField;
      set
      {
        if (this.\u003CAvgTreeHeight\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (AvgTreeHeight));
        this.\u003CAvgTreeHeight\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.AvgTreeHeight);
      }
    }

    public virtual double AvgCrownWidth
    {
      get => this.\u003CAvgCrownWidth\u003Ek__BackingField;
      set
      {
        if (this.\u003CAvgCrownWidth\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (AvgCrownWidth));
        this.\u003CAvgCrownWidth\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.AvgCrownWidth);
      }
    }

    public virtual double AvgCrownHeight
    {
      get => this.\u003CAvgCrownHeight\u003Ek__BackingField;
      set
      {
        if (this.\u003CAvgCrownHeight\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (AvgCrownHeight));
        this.\u003CAvgCrownHeight\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.AvgCrownHeight);
      }
    }

    public virtual double AvgLeafArea
    {
      get => this.\u003CAvgLeafArea\u003Ek__BackingField;
      set
      {
        if (this.\u003CAvgLeafArea\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (AvgLeafArea));
        this.\u003CAvgLeafArea\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.AvgLeafArea);
      }
    }

    public virtual double LeafAreaIndex
    {
      get => this.\u003CLeafAreaIndex\u003Ek__BackingField;
      set
      {
        if (this.\u003CLeafAreaIndex\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (LeafAreaIndex));
        this.\u003CLeafAreaIndex\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.LeafAreaIndex);
      }
    }

    public virtual double AvgLeafBiomass
    {
      get => this.\u003CAvgLeafBiomass\u003Ek__BackingField;
      set
      {
        if (this.\u003CAvgLeafBiomass\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (AvgLeafBiomass));
        this.\u003CAvgLeafBiomass\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.AvgLeafBiomass);
      }
    }

    public virtual double CarbonStorage
    {
      get => this.\u003CCarbonStorage\u003Ek__BackingField;
      set
      {
        if (this.\u003CCarbonStorage\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (CarbonStorage));
        this.\u003CCarbonStorage\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.CarbonStorage);
      }
    }

    public virtual PctMidRange PercentCrownMissing
    {
      get => this.\u003CPercentCrownMissing\u003Ek__BackingField;
      set
      {
        if (this.\u003CPercentCrownMissing\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PercentCrownMissing));
        this.\u003CPercentCrownMissing\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PercentCrownMissing);
      }
    }

    public virtual string ToString(bool isMetric)
    {
      object[] objArray = new object[16];
      objArray[0] = (object) this.ForecastedYear;
      objArray[1] = (object) this.Stratum.Description;
      objArray[2] = (object) this.Species;
      objArray[3] = (object) this.Condition.PctDieback;
      objArray[4] = (object) this.CrownLightExposure;
      objArray[5] = (object) this.NumTrees;
      double num;
      string str1;
      if (this.Mortality != null)
      {
        num = this.Mortality.Percent;
        str1 = num.ToString();
      }
      else
        str1 = "null";
      objArray[6] = (object) str1;
      string str2;
      if (!isMetric)
      {
        num = this.AvgDBH * 2.54;
        string str3 = num.ToString();
        num = this.AvgDBH;
        string str4 = num.ToString();
        str2 = str3 + " (" + str4 + " in)";
      }
      else
      {
        num = this.AvgDBH;
        string str5 = num.ToString();
        num = this.AvgDBH / 2.54;
        string str6 = num.ToString();
        str2 = str5 + " (" + str6 + " in)";
      }
      objArray[7] = (object) str2;
      string str7;
      if (!isMetric)
      {
        num = this.AvgTreeHeight / 3.28084;
        string str8 = num.ToString();
        num = this.AvgTreeHeight;
        string str9 = num.ToString();
        str7 = str8 + " (" + str9 + " ft)";
      }
      else
      {
        num = this.AvgTreeHeight;
        string str10 = num.ToString();
        num = this.AvgTreeHeight * 3.28084;
        string str11 = num.ToString();
        str7 = str10 + " (" + str11 + " ft)";
      }
      objArray[8] = (object) str7;
      string str12;
      if (!isMetric)
      {
        num = this.AvgCrownWidth / 3.28084;
        string str13 = num.ToString();
        num = this.AvgCrownWidth;
        string str14 = num.ToString();
        str12 = str13 + " (" + str14 + " ft)";
      }
      else
      {
        num = this.AvgCrownWidth;
        string str15 = num.ToString();
        num = this.AvgCrownWidth * 3.28084;
        string str16 = num.ToString();
        str12 = str15 + " (" + str16 + " ft)";
      }
      objArray[9] = (object) str12;
      string str17;
      if (!isMetric)
      {
        num = this.AvgCrownHeight / 3.28084;
        string str18 = num.ToString();
        num = this.AvgCrownHeight;
        string str19 = num.ToString();
        str17 = str18 + " (" + str19 + " ft)";
      }
      else
      {
        num = this.AvgCrownHeight;
        string str20 = num.ToString();
        num = this.AvgCrownHeight * 3.28084;
        string str21 = num.ToString();
        str17 = str20 + " (" + str21 + " ft)";
      }
      objArray[10] = (object) str17;
      string str22;
      if (!isMetric)
      {
        num = this.AvgLeafArea / 3.28084 / 3.28084;
        string str23 = num.ToString();
        num = this.AvgLeafArea;
        string str24 = num.ToString();
        str22 = str23 + " (" + str24 + " ft2)";
      }
      else
      {
        num = this.AvgLeafArea;
        string str25 = num.ToString();
        num = this.AvgLeafArea * 3.28084 * 3.28084;
        string str26 = num.ToString();
        str22 = str25 + " (" + str26 + " ft2)";
      }
      objArray[11] = (object) str22;
      objArray[12] = (object) this.LeafAreaIndex;
      string str27;
      if (!isMetric)
      {
        num = this.AvgLeafBiomass * 0.45359237;
        string str28 = num.ToString();
        num = this.AvgLeafBiomass;
        string str29 = num.ToString();
        str27 = str28 + " (" + str29 + " lb)";
      }
      else
      {
        num = this.AvgLeafBiomass;
        string str30 = num.ToString();
        num = this.AvgLeafBiomass / 0.45359237;
        string str31 = num.ToString();
        str27 = str30 + " (" + str31 + " lb)";
      }
      objArray[13] = (object) str27;
      string str32;
      if (!isMetric)
      {
        num = this.CarbonStorage * 0.45359237;
        string str33 = num.ToString();
        num = this.CarbonStorage;
        string str34 = num.ToString();
        str32 = str33 + " (" + str34 + " lb)";
      }
      else
      {
        num = this.CarbonStorage;
        string str35 = num.ToString();
        num = this.CarbonStorage / 0.45359237;
        string str36 = num.ToString();
        str32 = str35 + " (" + str36 + " lb)";
      }
      objArray[14] = (object) str32;
      objArray[15] = (object) (int) this.PercentCrownMissing;
      return string.Format("ForecastedYear:{0}, Stratum:{1}, Species:{2}, Condition.Dieback:{3}, CLE:{4}, NumTrees:{5}, Mortality:{6}, AvgDBH:{7}, AvgTreeHeight:{8}, AvgCrownWidth:{9}, AvgCrownHeight:{10}, AvgLeafArea:{11}, LeafAreaIndex:{12}, AvgLeafBiomass:{13}, CarbonStorage:{14}, PercentCrownMissing:{15}", objArray);
    }

    public override object Clone() => (object) Cohort.Clone(this, new EntityMap());

    public override Cohort Clone(bool deep) => Cohort.Clone(this, new EntityMap(), deep);

    internal static Cohort Clone(Cohort c, EntityMap map, bool deep = true)
    {
      Cohort eNew;
      if (map.Contains((Entity) c))
      {
        eNew = map.GetEntity<Cohort>(c);
      }
      else
      {
        eNew = new Cohort();
        eNew.ForecastedYear = c.ForecastedYear;
        eNew.Species = c.Species;
        eNew.CrownLightExposure = c.CrownLightExposure;
        eNew.CohortTag = c.CohortTag;
        eNew.NumTrees = c.NumTrees;
        eNew.AvgDBH = c.AvgDBH;
        eNew.AvgTreeHeight = c.AvgTreeHeight;
        eNew.AvgCrownHeight = c.AvgCrownHeight;
        eNew.AvgCrownWidth = c.AvgCrownWidth;
        eNew.AvgLeafArea = c.AvgLeafArea;
        eNew.LeafAreaIndex = c.LeafAreaIndex;
        eNew.AvgLeafBiomass = c.AvgLeafBiomass;
        eNew.CarbonStorage = c.CarbonStorage;
        eNew.PercentCrownMissing = c.PercentCrownMissing;
        map.Add((Entity) c, (Entity) eNew);
      }
      if (deep)
      {
        eNew.Forecast = map.GetEntity<Forecast>(c.Forecast);
        eNew.Stratum = map.GetEntity<Strata>(c.Stratum);
        eNew.Condition = map.GetEntity<Condition>(c.Condition);
        eNew.Mortality = map.GetEntity<Mortality>(c.Mortality);
        eNew.Parent = map.GetEntity<Cohort>(c.Parent);
      }
      return eNew;
    }
  }
}
