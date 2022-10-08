// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Year
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v6;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Eco.Domain.v6
{
  public class Year : Entity<Year>
  {
    public Year()
    {
      this.Init();
      this.Unit = YearUnit.English;
      this.Changed = true;
      this.DBHActual = true;
    }

    protected virtual IDictionary<string, bool> Options
    {
      get => this.\u003COptions\u003Ek__BackingField;
      set
      {
        if (this.Options == value)
          return;
        this.OnPropertyChanging("DBHActual");
        this.OnPropertyChanging("RecordStrata");
        this.OnPropertyChanging("RecordHydro");
        this.OnPropertyChanging("RecordShrub");
        this.OnPropertyChanging("RecordEnergy");
        this.OnPropertyChanging("RecordPlantableSpace");
        this.OnPropertyChanging("RecordCrownSize");
        this.OnPropertyChanging("RecordCrownCondition");
        this.OnPropertyChanging("RecordHeight");
        this.OnPropertyChanging("RecordPercentShrub");
        this.OnPropertyChanging("RecordPlotCenter");
        this.OnPropertyChanging("RecordReferenceObjects");
        this.OnPropertyChanging("RecordGroundCover");
        this.OnPropertyChanging("RecordStreetTree");
        this.OnPropertyChanging("DefaultStreetTree");
        this.OnPropertyChanging("RecordSiteType");
        this.OnPropertyChanging("RecordLanduse");
        this.OnPropertyChanging("RecordLocSite");
        this.OnPropertyChanging("RecordLocNo");
        this.OnPropertyChanging("RecordMaintRec");
        this.OnPropertyChanging("RecordMaintTask");
        this.OnPropertyChanging("RecordSidewalk");
        this.OnPropertyChanging("RecordWireConflict");
        this.OnPropertyChanging("RecordPlotStreet");
        this.OnPropertyChanging("RecordPlotAddress");
        this.OnPropertyChanging("RecordTreeStreet");
        this.OnPropertyChanging("RecordTreeAddress");
        this.OnPropertyChanging("RecordTreeStatus");
        this.OnPropertyChanging("RecordTreeUserId");
        this.OnPropertyChanging("RecordOtherOne");
        this.OnPropertyChanging("RecordOtherTwo");
        this.OnPropertyChanging("RecordOtherThree");
        this.OnPropertyChanging("RecordGPS");
        this.OnPropertyChanging("RecordTreeGPS");
        this.OnPropertyChanging("RecordCLE");
        this.OnPropertyChanging("RecordIPED");
        this.OnPropertyChanging("RecordNoteTree");
        this.OnPropertyChanging("RecordMgmtStyle");
        this.OnPropertyChanging("MgmtStyle");
        this.OnPropertyChanging("MgmtStyleDefault");
        this.OnPropertyChanging("DisplayCondition");
        this.OnPropertyChanging("FIA");
        this.OnPropertyChanging(nameof (Options));
        this.\u003COptions\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DBHActual);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordStrata);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordHydro);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordShrub);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordEnergy);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordPlantableSpace);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordCrownSize);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordCrownCondition);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordHeight);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordPercentShrub);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordPlotCenter);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordReferenceObjects);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordGroundCover);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordStreetTree);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DefaultStreetTree);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordSiteType);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordLanduse);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordLocSite);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordLocNo);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordMaintRec);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordMaintTask);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordSidewalk);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordWireConflict);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordPlotStreet);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordPlotAddress);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordTreeStreet);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordTreeAddress);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordTreeStatus);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordTreeUserId);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordOtherOne);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordOtherTwo);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordOtherThree);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordGPS);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordTreeGPS);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordCLE);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordIPED);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordNoteTree);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordMgmtStyle);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MgmtStyle);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MgmtStyleDefault);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DisplayCondition);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.FIA);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Options);
      }
    }

    public virtual Series Series
    {
      get => this.\u003CSeries\u003Ek__BackingField;
      set
      {
        if (this.\u003CSeries\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Series));
        this.\u003CSeries\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Series);
      }
    }

    public virtual short Id
    {
      get => this.\u003CId\u003Ek__BackingField;
      set
      {
        if ((int) this.\u003CId\u003Ek__BackingField == (int) value)
          return;
        this.OnPropertyChanging(nameof (Id));
        this.\u003CId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Id);
      }
    }

    public virtual ISet<Plot> Plots
    {
      get => this.\u003CPlots\u003Ek__BackingField;
      protected internal set
      {
        if (this.Plots == value)
          return;
        this.OnPropertyChanging(nameof (Plots));
        this.\u003CPlots\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Plots);
      }
    }

    public virtual ISet<Eco.Domain.v6.Strata> Strata
    {
      get => this.\u003CStrata\u003Ek__BackingField;
      protected internal set
      {
        if (this.Strata == value)
          return;
        this.OnPropertyChanging(nameof (Strata));
        this.\u003CStrata\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Strata);
      }
    }

    public virtual ISet<GroundCover> GroundCovers
    {
      get => this.\u003CGroundCovers\u003Ek__BackingField;
      protected internal set
      {
        if (this.GroundCovers == value)
          return;
        this.OnPropertyChanging(nameof (GroundCovers));
        this.\u003CGroundCovers\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.GroundCovers);
      }
    }

    public virtual ISet<LandUse> LandUses
    {
      get => this.\u003CLandUses\u003Ek__BackingField;
      protected internal set
      {
        if (this.LandUses == value)
          return;
        this.OnPropertyChanging(nameof (LandUses));
        this.\u003CLandUses\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.LandUses);
      }
    }

    public virtual ISet<Eco.Domain.v6.YearLocationData> YearLocationData
    {
      get => this.\u003CYearLocationData\u003Ek__BackingField;
      protected internal set
      {
        if (this.YearLocationData == value)
          return;
        this.OnPropertyChanging(nameof (YearLocationData));
        this.\u003CYearLocationData\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.YearLocationData);
      }
    }

    public virtual ISet<YearlyCost> YearlyCosts
    {
      get => this.\u003CYearlyCosts\u003Ek__BackingField;
      protected internal set
      {
        if (this.YearlyCosts == value)
          return;
        this.OnPropertyChanging(nameof (YearlyCosts));
        this.\u003CYearlyCosts\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.YearlyCosts);
      }
    }

    public virtual ISet<SiteType> SiteTypes
    {
      get => this.\u003CSiteTypes\u003Ek__BackingField;
      protected internal set
      {
        if (this.SiteTypes == value)
          return;
        this.OnPropertyChanging(nameof (SiteTypes));
        this.\u003CSiteTypes\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.SiteTypes);
      }
    }

    public virtual ISet<LocSite> LocSites
    {
      get => this.\u003CLocSites\u003Ek__BackingField;
      protected internal set
      {
        if (this.LocSites == value)
          return;
        this.OnPropertyChanging(nameof (LocSites));
        this.\u003CLocSites\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.LocSites);
      }
    }

    public virtual ISet<MaintRec> MaintRecs
    {
      get => this.\u003CMaintRecs\u003Ek__BackingField;
      protected internal set
      {
        if (this.MaintRecs == value)
          return;
        this.OnPropertyChanging(nameof (MaintRecs));
        this.\u003CMaintRecs\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MaintRecs);
      }
    }

    public virtual ISet<MaintTask> MaintTasks
    {
      get => this.\u003CMaintTasks\u003Ek__BackingField;
      protected internal set
      {
        if (this.MaintTasks == value)
          return;
        this.OnPropertyChanging(nameof (MaintTasks));
        this.\u003CMaintTasks\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MaintTasks);
      }
    }

    public virtual ISet<Sidewalk> SidewalkDamages
    {
      get => this.\u003CSidewalkDamages\u003Ek__BackingField;
      protected internal set
      {
        if (this.SidewalkDamages == value)
          return;
        this.OnPropertyChanging(nameof (SidewalkDamages));
        this.\u003CSidewalkDamages\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.SidewalkDamages);
      }
    }

    public virtual ISet<WireConflict> WireConflicts
    {
      get => this.\u003CWireConflicts\u003Ek__BackingField;
      protected internal set
      {
        if (this.WireConflicts == value)
          return;
        this.OnPropertyChanging(nameof (WireConflicts));
        this.\u003CWireConflicts\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.WireConflicts);
      }
    }

    public virtual ISet<Eco.Domain.v6.OtherOne> OtherOnes
    {
      get => this.\u003COtherOnes\u003Ek__BackingField;
      protected internal set
      {
        if (this.OtherOnes == value)
          return;
        this.OnPropertyChanging(nameof (OtherOnes));
        this.\u003COtherOnes\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.OtherOnes);
      }
    }

    public virtual ISet<Eco.Domain.v6.OtherTwo> OtherTwos
    {
      get => this.\u003COtherTwos\u003Ek__BackingField;
      protected internal set
      {
        if (this.OtherTwos == value)
          return;
        this.OnPropertyChanging(nameof (OtherTwos));
        this.\u003COtherTwos\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.OtherTwos);
      }
    }

    public virtual ISet<Eco.Domain.v6.OtherThree> OtherThrees
    {
      get => this.\u003COtherThrees\u003Ek__BackingField;
      protected internal set
      {
        if (this.OtherThrees == value)
          return;
        this.OnPropertyChanging(nameof (OtherThrees));
        this.\u003COtherThrees\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.OtherThrees);
      }
    }

    public virtual ISet<DBH> DBHs
    {
      get => this.\u003CDBHs\u003Ek__BackingField;
      protected internal set
      {
        if (this.DBHs == value)
          return;
        this.OnPropertyChanging(nameof (DBHs));
        this.\u003CDBHs\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DBHs);
      }
    }

    public virtual ISet<DBHRptClass> DBHRptClasses
    {
      get => this.\u003CDBHRptClasses\u003Ek__BackingField;
      protected internal set
      {
        if (this.DBHRptClasses == value)
          return;
        this.OnPropertyChanging(nameof (DBHRptClasses));
        this.\u003CDBHRptClasses\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DBHRptClasses);
      }
    }

    public virtual ISet<HealthRptClass> HealthRptClasses
    {
      get => this.\u003CHealthRptClasses\u003Ek__BackingField;
      protected internal set
      {
        if (this.HealthRptClasses == value)
          return;
        this.OnPropertyChanging(nameof (HealthRptClasses));
        this.\u003CHealthRptClasses\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.HealthRptClasses);
      }
    }

    public virtual ISet<Condition> Conditions
    {
      get => this.\u003CConditions\u003Ek__BackingField;
      protected internal set
      {
        if (this.Conditions == value)
          return;
        this.OnPropertyChanging(nameof (Conditions));
        this.\u003CConditions\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Conditions);
      }
    }

    public virtual ISet<PlantingSiteType> PlantingSiteTypes
    {
      get => this.\u003CPlantingSiteTypes\u003Ek__BackingField;
      protected internal set
      {
        if (this.PlantingSiteTypes == value)
          return;
        this.OnPropertyChanging(nameof (PlantingSiteTypes));
        this.\u003CPlantingSiteTypes\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PlantingSiteTypes);
      }
    }

    public virtual ISet<Forecast> Forecasts
    {
      get => this.\u003CForecasts\u003Ek__BackingField;
      protected internal set
      {
        if (this.Forecasts == value)
          return;
        this.OnPropertyChanging(nameof (Forecasts));
        this.\u003CForecasts\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Forecasts);
      }
    }

    public virtual ISet<YearResult> Results
    {
      get => this.\u003CResults\u003Ek__BackingField;
      protected internal set
      {
        if (this.Results == value)
          return;
        this.OnPropertyChanging(nameof (Results));
        this.\u003CResults\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Results);
      }
    }

    public virtual double Budget
    {
      get => this.\u003CBudget\u003Ek__BackingField;
      set
      {
        if (this.\u003CBudget\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Budget));
        this.\u003CBudget\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Budget);
      }
    }

    public virtual YearUnit Unit
    {
      get => this.\u003CUnit\u003Ek__BackingField;
      set
      {
        if (this.\u003CUnit\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Unit));
        this.\u003CUnit\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Unit);
      }
    }

    public virtual bool IsInitialMeasurement
    {
      get => this.\u003CIsInitialMeasurement\u003Ek__BackingField;
      set
      {
        if (this.\u003CIsInitialMeasurement\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (IsInitialMeasurement));
        this.\u003CIsInitialMeasurement\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.IsInitialMeasurement);
      }
    }

    public virtual bool DBHActual
    {
      get => this.GetOption(nameof (DBHActual));
      set
      {
        if (this.DBHActual == value)
          return;
        this.OnPropertyChanging(nameof (DBHActual));
        this.SetOption(nameof (DBHActual), value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DBHActual);
      }
    }

    public virtual bool RecordStrata
    {
      get => this.GetOption("RecStrata");
      set
      {
        if (this.RecordStrata == value)
          return;
        this.OnPropertyChanging(nameof (RecordStrata));
        this.SetOption("RecStrata", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordStrata);
      }
    }

    public virtual bool RecordHydro
    {
      get => this.GetOption("RecHydro");
      set
      {
        if (this.RecordHydro == value)
          return;
        this.OnPropertyChanging(nameof (RecordHydro));
        this.SetOption("RecHydro", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordHydro);
      }
    }

    public virtual bool RecordShrub
    {
      get => this.GetOption("RecShrub");
      set
      {
        if (this.RecordShrub == value)
          return;
        this.OnPropertyChanging(nameof (RecordShrub));
        this.SetOption("RecShrub", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordShrub);
      }
    }

    public virtual bool RecordEnergy
    {
      get => this.GetOption("RecEnergy");
      set
      {
        if (this.RecordEnergy == value)
          return;
        this.OnPropertyChanging(nameof (RecordEnergy));
        this.SetOption("RecEnergy", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordEnergy);
      }
    }

    public virtual bool RecordPlantableSpace
    {
      get => this.GetOption("RecPlantableSpace");
      set
      {
        if (this.RecordPlantableSpace == value)
          return;
        this.OnPropertyChanging(nameof (RecordPlantableSpace));
        this.SetOption("RecPlantableSpace", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordPlantableSpace);
      }
    }

    public virtual bool RecordCrownSize
    {
      get => this.GetOption("RecCrownSize");
      set
      {
        if (this.RecordCrownSize == value)
          return;
        this.OnPropertyChanging(nameof (RecordCrownSize));
        this.SetOption("RecCrownSize", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordCrownSize);
      }
    }

    public virtual bool RecordCrownCondition
    {
      get => this.GetOption("RecCrownCondition");
      set
      {
        if (this.RecordCrownCondition == value)
          return;
        this.OnPropertyChanging(nameof (RecordCrownCondition));
        this.SetOption("RecCrownCondition", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordCrownCondition);
      }
    }

    public virtual bool RecordHeight
    {
      get => this.GetOption("RecHeight");
      set
      {
        if (this.RecordHeight == value)
          return;
        this.OnPropertyChanging(nameof (RecordHeight));
        this.SetOption("RecHeight", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordHeight);
      }
    }

    public virtual bool RecordPercentShrub
    {
      get => this.GetOption("RecPercentShrub");
      set
      {
        if (this.RecordPercentShrub == value)
          return;
        this.OnPropertyChanging(nameof (RecordPercentShrub));
        this.SetOption("RecPercentShrub", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordPercentShrub);
      }
    }

    public virtual bool RecordPlotCenter
    {
      get => this.GetOption("RecPlotCenter");
      set
      {
        if (this.RecordPlotCenter == value)
          return;
        this.OnPropertyChanging(nameof (RecordPlotCenter));
        this.SetOption("RecPlotCenter", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordPlotCenter);
      }
    }

    public virtual bool RecordReferenceObjects
    {
      get => this.GetOption("RecReferenceObjects");
      set
      {
        if (this.RecordReferenceObjects == value)
          return;
        this.OnPropertyChanging(nameof (RecordReferenceObjects));
        this.SetOption("RecReferenceObjects", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordReferenceObjects);
      }
    }

    public virtual bool RecordGroundCover
    {
      get => this.GetOption("RecGroundCover");
      set
      {
        if (this.RecordGroundCover == value)
          return;
        this.OnPropertyChanging(nameof (RecordGroundCover));
        this.SetOption("RecGroundCover", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordGroundCover);
      }
    }

    public virtual bool RecordStreetTree
    {
      get => this.GetOption("RecStreetTree");
      set
      {
        if (this.RecordStreetTree == value)
          return;
        this.OnPropertyChanging(nameof (RecordStreetTree));
        this.SetOption("RecStreetTree", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordStreetTree);
      }
    }

    public virtual bool DefaultStreetTree
    {
      get => this.GetOption("StreetTreeDefault");
      set
      {
        if (this.DefaultStreetTree == value)
          return;
        this.OnPropertyChanging(nameof (DefaultStreetTree));
        this.SetOption("StreetTreeDefault", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DefaultStreetTree);
      }
    }

    public virtual bool RecordSiteType
    {
      get => this.GetOption("RecSiteType");
      set
      {
        if (this.RecordSiteType == value)
          return;
        this.OnPropertyChanging(nameof (RecordSiteType));
        this.SetOption("RecSiteType", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordSiteType);
      }
    }

    public virtual bool RecordLanduse
    {
      get => this.GetOption("RecLanduse");
      set
      {
        if (this.RecordLanduse == value)
          return;
        this.OnPropertyChanging(nameof (RecordLanduse));
        this.SetOption("RecLanduse", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordLanduse);
      }
    }

    public virtual bool RecordLocSite
    {
      get => this.GetOption("RecLocSite");
      set
      {
        if (this.RecordLocSite == value)
          return;
        this.OnPropertyChanging(nameof (RecordLocSite));
        this.SetOption("RecLocSite", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordLocSite);
      }
    }

    public virtual bool RecordLocNo
    {
      get => this.GetOption("RecLocNo");
      set
      {
        if (this.RecordLocNo == value)
          return;
        this.OnPropertyChanging(nameof (RecordLocNo));
        this.SetOption("RecLocNo", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordLocNo);
      }
    }

    public virtual bool RecordMaintRec
    {
      get => this.GetOption("RecMaintRec");
      set
      {
        if (this.RecordMaintRec == value)
          return;
        this.OnPropertyChanging(nameof (RecordMaintRec));
        this.SetOption("RecMaintRec", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordMaintRec);
      }
    }

    public virtual bool RecordMaintTask
    {
      get => this.GetOption("RecMaintTask");
      set
      {
        if (this.RecordMaintTask == value)
          return;
        this.OnPropertyChanging(nameof (RecordMaintTask));
        this.SetOption("RecMaintTask", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordMaintTask);
      }
    }

    public virtual bool RecordSidewalk
    {
      get => this.GetOption("RecSidewalk");
      set
      {
        if (this.RecordSidewalk == value)
          return;
        this.OnPropertyChanging(nameof (RecordSidewalk));
        this.SetOption("RecSidewalk", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordSidewalk);
      }
    }

    public virtual bool RecordWireConflict
    {
      get => this.GetOption("RecWireConflict");
      set
      {
        if (this.RecordWireConflict == value)
          return;
        this.OnPropertyChanging(nameof (RecordWireConflict));
        this.SetOption("RecWireConflict", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordWireConflict);
      }
    }

    public virtual bool RecordPlotStreet
    {
      get => this.GetOption("RecPlotStreet");
      set
      {
        if (this.RecordPlotStreet == value)
          return;
        this.OnPropertyChanging(nameof (RecordPlotStreet));
        this.SetOption("RecPlotStreet", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordPlotStreet);
      }
    }

    public virtual bool RecordPlotAddress
    {
      get => this.GetOption("RecPlotAddress");
      set
      {
        if (this.RecordPlotAddress == value)
          return;
        this.OnPropertyChanging(nameof (RecordPlotAddress));
        this.SetOption("RecPlotAddress", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordPlotAddress);
      }
    }

    public virtual bool RecordTreeStreet
    {
      get => this.GetOption("RecTreeStreet");
      set
      {
        if (this.RecordTreeStreet == value)
          return;
        this.OnPropertyChanging(nameof (RecordTreeStreet));
        this.SetOption("RecTreeStreet", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordTreeStreet);
      }
    }

    public virtual bool RecordTreeAddress
    {
      get => this.GetOption("RecTreeAddress");
      set
      {
        if (this.RecordTreeAddress == value)
          return;
        this.OnPropertyChanging(nameof (RecordTreeAddress));
        this.SetOption("RecTreeAddress", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordTreeAddress);
      }
    }

    public virtual bool RecordTreeStatus
    {
      get => this.GetOption("RecTreeStatus");
      set
      {
        if (this.RecordTreeStatus == value)
          return;
        this.OnPropertyChanging(nameof (RecordTreeStatus));
        this.SetOption("RecTreeStatus", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordTreeStatus);
      }
    }

    public virtual bool RecordTreeUserId
    {
      get => this.GetOption("RecTreeUserId");
      set
      {
        if (this.RecordTreeUserId == value)
          return;
        this.OnPropertyChanging(nameof (RecordTreeUserId));
        this.SetOption("RecTreeUserId", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordTreeUserId);
      }
    }

    public virtual bool RecordOtherOne
    {
      get => this.GetOption("RecOtherOne");
      set
      {
        if (this.RecordOtherOne == value)
          return;
        this.OnPropertyChanging(nameof (RecordOtherOne));
        this.SetOption("RecOtherOne", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordOtherOne);
      }
    }

    public virtual bool RecordOtherTwo
    {
      get => this.GetOption("RecOtherTwo");
      set
      {
        if (this.RecordOtherTwo == value)
          return;
        this.OnPropertyChanging(nameof (RecordOtherTwo));
        this.SetOption("RecOtherTwo", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordOtherTwo);
      }
    }

    public virtual bool RecordOtherThree
    {
      get => this.GetOption("RecOtherThree");
      set
      {
        if (this.RecordOtherThree == value)
          return;
        this.OnPropertyChanging(nameof (RecordOtherThree));
        this.SetOption("RecOtherThree", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordOtherThree);
      }
    }

    public virtual bool RecordGPS
    {
      get => this.GetOption("RecGPS");
      set
      {
        if (this.RecordGPS == value)
          return;
        this.OnPropertyChanging(nameof (RecordGPS));
        this.SetOption("RecGPS", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordGPS);
      }
    }

    public virtual bool RecordTreeGPS
    {
      get => this.GetOption("RecTreeGPS");
      set
      {
        if (this.RecordTreeGPS == value)
          return;
        this.OnPropertyChanging(nameof (RecordTreeGPS));
        this.SetOption("RecTreeGPS", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordTreeGPS);
      }
    }

    public virtual bool RecordCLE
    {
      get => this.GetOption("RecCLE");
      set
      {
        if (this.RecordCLE == value)
          return;
        this.OnPropertyChanging(nameof (RecordCLE));
        this.SetOption("RecCLE", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordCLE);
      }
    }

    public virtual bool RecordIPED
    {
      get => this.GetOption("RecIPED");
      set
      {
        if (this.RecordIPED == value)
          return;
        this.OnPropertyChanging(nameof (RecordIPED));
        this.SetOption("RecIPED", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordIPED);
      }
    }

    public virtual bool RecordNoteTree
    {
      get => this.GetOption("RecNoteTree");
      set
      {
        if (this.RecordNoteTree == value)
          return;
        this.OnPropertyChanging(nameof (RecordNoteTree));
        this.SetOption("RecNoteTree", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordNoteTree);
      }
    }

    public virtual bool RecordMgmtStyle
    {
      get => this.GetOption("RecMgmtStyle");
      set
      {
        if (this.RecordMgmtStyle == value)
          return;
        this.OnPropertyChanging("MgmtStyle");
        this.OnPropertyChanging(nameof (RecordMgmtStyle));
        this.SetOption("RecMgmtStyle", value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MgmtStyle);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RecordMgmtStyle);
      }
    }

    public virtual bool MgmtStyleDefault
    {
      get => this.GetOption(nameof (MgmtStyleDefault));
      set
      {
        if (this.MgmtStyleDefault == value)
          return;
        this.OnPropertyChanging("MgmtStyle");
        this.OnPropertyChanging(nameof (MgmtStyleDefault));
        this.SetOption(nameof (MgmtStyleDefault), value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MgmtStyle);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MgmtStyleDefault);
      }
    }

    public virtual bool DisplayCondition
    {
      get => this.GetOption(nameof (DisplayCondition));
      set
      {
        if (this.DisplayCondition == value)
          return;
        this.OnPropertyChanging(nameof (DisplayCondition));
        this.SetOption(nameof (DisplayCondition), value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DisplayCondition);
      }
    }

    public virtual bool FIA
    {
      get => this.GetOption(nameof (FIA));
      set
      {
        if (this.FIA == value)
          return;
        this.OnPropertyChanging(nameof (FIA));
        this.SetOption(nameof (FIA), value);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.FIA);
      }
    }

    public virtual MgmtStyleEnum? MgmtStyle
    {
      get
      {
        MgmtStyleEnum? mgmtStyle = new MgmtStyleEnum?();
        if (this.RecordMgmtStyle)
        {
          mgmtStyle = new MgmtStyleEnum?(MgmtStyleEnum.RecordPublicPrivate);
          MgmtStyleEnum? nullable = mgmtStyle;
          MgmtStyleEnum mgmtStyleEnum = this.MgmtStyleDefault ? MgmtStyleEnum.DefaultPublic : MgmtStyleEnum.DefaultPrivate;
          mgmtStyle = nullable.HasValue ? new MgmtStyleEnum?(nullable.GetValueOrDefault() | mgmtStyleEnum) : new MgmtStyleEnum?();
        }
        return mgmtStyle;
      }
      set
      {
        if (Nullable.Equals<MgmtStyleEnum>(this.MgmtStyle, value))
          return;
        this.OnPropertyChanging(nameof (MgmtStyle));
        this.RecordMgmtStyle = value.HasValue;
        if (value.HasValue)
          this.MgmtStyleDefault = (value.Value & MgmtStyleEnum.DefaultPrivate) == MgmtStyleEnum.DefaultPublic;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MgmtStyle);
      }
    }

    public virtual string OtherOne
    {
      get => this.\u003COtherOne\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003COtherOne\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (OtherOne));
        this.\u003COtherOne\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.OtherOne);
      }
    }

    public virtual string OtherTwo
    {
      get => this.\u003COtherTwo\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003COtherTwo\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (OtherTwo));
        this.\u003COtherTwo\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.OtherTwo);
      }
    }

    public virtual string OtherThree
    {
      get => this.\u003COtherThree\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003COtherThree\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (OtherThree));
        this.\u003COtherThree\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.OtherThree);
      }
    }

    public virtual string MobileKey
    {
      get => this.\u003CMobileKey\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CMobileKey\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging("ConfigEnabled");
        this.OnPropertyChanging(nameof (MobileKey));
        this.\u003CMobileKey\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.ConfigEnabled);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MobileKey);
      }
    }

    public virtual string MobileEmail
    {
      get => this.\u003CMobileEmail\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CMobileEmail\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (MobileEmail));
        this.\u003CMobileEmail\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MobileEmail);
      }
    }

    public virtual Carbon Carbon
    {
      get => this.\u003CCarbon\u003Ek__BackingField;
      set
      {
        if (this.\u003CCarbon\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Carbon));
        this.\u003CCarbon\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Carbon);
      }
    }

    public virtual CO CO
    {
      get => this.\u003CCO\u003Ek__BackingField;
      set
      {
        if (this.\u003CCO\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (CO));
        this.\u003CCO\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.CO);
      }
    }

    public virtual Electricity Electricity
    {
      get => this.\u003CElectricity\u003Ek__BackingField;
      set
      {
        if (this.\u003CElectricity\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Electricity));
        this.\u003CElectricity\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Electricity);
      }
    }

    public virtual NO2 NO2
    {
      get => this.\u003CNO2\u003Ek__BackingField;
      set
      {
        if (this.\u003CNO2\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (NO2));
        this.\u003CNO2\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.NO2);
      }
    }

    public virtual O3 O3
    {
      get => this.\u003CO3\u003Ek__BackingField;
      set
      {
        if (this.\u003CO3\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (O3));
        this.\u003CO3\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.O3);
      }
    }

    public virtual PM10 PM10
    {
      get => this.\u003CPM10\u003Ek__BackingField;
      set
      {
        if (this.\u003CPM10\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PM10));
        this.\u003CPM10\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PM10);
      }
    }

    public virtual PM25 PM25
    {
      get => this.\u003CPM25\u003Ek__BackingField;
      set
      {
        if (this.\u003CPM25\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PM25));
        this.\u003CPM25\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PM25);
      }
    }

    public virtual SO2 SO2
    {
      get => this.\u003CSO2\u003Ek__BackingField;
      set
      {
        if (this.\u003CSO2\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (SO2));
        this.\u003CSO2\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.SO2);
      }
    }

    public virtual Gas Gas
    {
      get => this.\u003CGas\u003Ek__BackingField;
      set
      {
        if (this.\u003CGas\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Gas));
        this.\u003CGas\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Gas);
      }
    }

    public virtual H2O H2O
    {
      get => this.\u003CH2O\u003Ek__BackingField;
      set
      {
        if (this.\u003CH2O\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (H2O));
        this.\u003CH2O\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.H2O);
      }
    }

    public virtual ExchangeRate ExchangeRate
    {
      get => this.\u003CExchangeRate\u003Ek__BackingField;
      set
      {
        if (this.\u003CExchangeRate\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (ExchangeRate));
        this.\u003CExchangeRate\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.ExchangeRate);
      }
    }

    public virtual Home Home
    {
      get => this.\u003CHome\u003Ek__BackingField;
      set
      {
        if (this.\u003CHome\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Home));
        this.\u003CHome\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Home);
      }
    }

    public virtual VOC VOC
    {
      get => this.\u003CVOC\u003Ek__BackingField;
      set
      {
        if (this.\u003CVOC\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (VOC));
        this.\u003CVOC\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.VOC);
      }
    }

    public virtual PopulationDensity PopulationDensity
    {
      get => this.\u003CPopulationDensity\u003Ek__BackingField;
      set
      {
        if (this.\u003CPopulationDensity\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PopulationDensity));
        this.\u003CPopulationDensity\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PopulationDensity);
      }
    }

    public virtual string Comments
    {
      get => this.\u003CComments\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CComments\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Comments));
        this.\u003CComments\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Comments);
      }
    }

    public virtual int RevProcessed
    {
      get => this.\u003CRevProcessed\u003Ek__BackingField;
      set
      {
        if (this.\u003CRevProcessed\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (RevProcessed));
        this.\u003CRevProcessed\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RevProcessed);
      }
    }

    public virtual bool Changed
    {
      get => this.\u003CChanged\u003Ek__BackingField;
      set
      {
        if (this.\u003CChanged\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging("ConfigEnabled");
        this.OnPropertyChanging(nameof (Changed));
        this.\u003CChanged\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.ConfigEnabled);
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Changed);
      }
    }

    public virtual CultureInfo Culture
    {
      get => this.\u003CCulture\u003Ek__BackingField;
      set
      {
        if (this.\u003CCulture\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Culture));
        this.\u003CCulture\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Culture);
      }
    }

    public virtual Version LocationVersion
    {
      get => this.\u003CLocationVersion\u003Ek__BackingField;
      set
      {
        if (this.\u003CLocationVersion\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (LocationVersion));
        this.\u003CLocationVersion\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.LocationVersion);
      }
    }

    public virtual Version SpeciesVersion
    {
      get => this.\u003CSpeciesVersion\u003Ek__BackingField;
      set
      {
        if (this.\u003CSpeciesVersion\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (SpeciesVersion));
        this.\u003CSpeciesVersion\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.SpeciesVersion);
      }
    }

    public virtual bool ConfigEnabled => this.Changed && string.IsNullOrEmpty(this.MobileKey);

    private void Init()
    {
      this.Plots = (ISet<Plot>) new HashSet<Plot>();
      this.Strata = (ISet<Eco.Domain.v6.Strata>) new HashSet<Eco.Domain.v6.Strata>();
      this.GroundCovers = (ISet<GroundCover>) new HashSet<GroundCover>();
      this.LandUses = (ISet<LandUse>) new HashSet<LandUse>();
      this.YearlyCosts = (ISet<YearlyCost>) new HashSet<YearlyCost>();
      this.YearLocationData = (ISet<Eco.Domain.v6.YearLocationData>) new HashSet<Eco.Domain.v6.YearLocationData>();
      this.LocSites = (ISet<LocSite>) new HashSet<LocSite>();
      this.MaintRecs = (ISet<MaintRec>) new HashSet<MaintRec>();
      this.MaintTasks = (ISet<MaintTask>) new HashSet<MaintTask>();
      this.SidewalkDamages = (ISet<Sidewalk>) new HashSet<Sidewalk>();
      this.WireConflicts = (ISet<WireConflict>) new HashSet<WireConflict>();
      this.OtherOnes = (ISet<Eco.Domain.v6.OtherOne>) new HashSet<Eco.Domain.v6.OtherOne>();
      this.OtherTwos = (ISet<Eco.Domain.v6.OtherTwo>) new HashSet<Eco.Domain.v6.OtherTwo>();
      this.OtherThrees = (ISet<Eco.Domain.v6.OtherThree>) new HashSet<Eco.Domain.v6.OtherThree>();
      this.DBHs = (ISet<DBH>) new HashSet<DBH>();
      this.DBHRptClasses = (ISet<DBHRptClass>) new HashSet<DBHRptClass>();
      this.HealthRptClasses = (ISet<HealthRptClass>) new HashSet<HealthRptClass>();
      this.Conditions = (ISet<Condition>) new HashSet<Condition>();
      this.PlantingSiteTypes = (ISet<PlantingSiteType>) new HashSet<PlantingSiteType>();
      this.Forecasts = (ISet<Forecast>) new HashSet<Forecast>();
      this.Options = (IDictionary<string, bool>) new Dictionary<string, bool>();
    }

    private bool GetOption(string opt)
    {
      bool option;
      this.Options.TryGetValue(opt, out option);
      return option;
    }

    private void SetOption(string opt, bool val) => this.Options[opt] = val;

    public virtual YearDTO GetDTO()
    {
      YearDTO yearDto = new YearDTO();
      yearDto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      yearDto.Id = this.Id;
      yearDto.Unit = (char) this.Unit;
      yearDto.DBHActual = this.DBHActual;
      yearDto.RecordStrata = this.RecordStrata;
      yearDto.RecordHydro = this.RecordHydro;
      yearDto.RecordShrub = this.RecordShrub;
      yearDto.RecordEnergy = this.RecordEnergy;
      yearDto.RecordPlantableSpace = this.RecordPlantableSpace;
      yearDto.RecordCrownSize = this.RecordCrownSize;
      yearDto.RecordCrownCondition = this.RecordCrownCondition;
      yearDto.RecordHeight = this.RecordHeight;
      yearDto.RecordPercentShrub = this.RecordPercentShrub;
      yearDto.RecordPlotCenter = this.RecordPlotCenter;
      yearDto.RecordReferenceObjects = this.RecordReferenceObjects;
      yearDto.RecordGroundCover = this.RecordGroundCover;
      yearDto.DefaultStreetTree = this.DefaultStreetTree;
      yearDto.RecordStreetTree = this.RecordStreetTree;
      yearDto.RecordSiteType = this.RecordSiteType;
      yearDto.RecordLanduse = this.RecordLanduse;
      yearDto.RecordLocSite = this.RecordLocSite;
      yearDto.RecordLocNo = this.RecordLocNo;
      yearDto.RecordMaintRec = this.RecordMaintRec;
      yearDto.RecordMaintTask = this.RecordMaintTask;
      yearDto.RecordSidewalk = this.RecordSidewalk;
      yearDto.RecordWireConflict = this.RecordWireConflict;
      yearDto.RecordPlotAddress = this.RecordPlotAddress;
      yearDto.RecordTreeStatus = this.RecordTreeStatus;
      yearDto.RecordTreeUserId = this.RecordTreeUserId;
      yearDto.RecordOtherOne = this.RecordOtherOne;
      yearDto.RecordOtherTwo = this.RecordOtherTwo;
      yearDto.RecordOtherThree = this.RecordOtherThree;
      yearDto.RecordGPS = this.RecordGPS;
      yearDto.RecordTreeGPS = this.RecordTreeGPS;
      yearDto.RecordTreeAddress = this.RecordTreeAddress;
      yearDto.RecordCLE = this.RecordCLE;
      yearDto.RecordIPED = this.RecordIPED;
      yearDto.RecordNoteTree = this.RecordNoteTree;
      yearDto.DisplayCondition = this.DisplayCondition;
      MgmtStyleEnum? mgmtStyle = this.MgmtStyle;
      yearDto.MgmtStyle = mgmtStyle.HasValue ? new short?((short) mgmtStyle.GetValueOrDefault()) : new short?();
      yearDto.OtherOne = this.OtherOne;
      yearDto.OtherTwo = this.OtherTwo;
      yearDto.OtherThree = this.OtherThree;
      yearDto.IsInitialMeasurement = this.IsInitialMeasurement;
      yearDto.Series = this.Series.GetDTO();
      yearDto.Strata = this.Strata.Count > 0 ? new List<StrataDTO>() : (List<StrataDTO>) null;
      yearDto.GroundCovers = this.GroundCovers.Count > 0 ? new List<GroundCoverDTO>() : (List<GroundCoverDTO>) null;
      yearDto.LandUses = this.LandUses.Count > 0 ? new List<LandUseDTO>() : (List<LandUseDTO>) null;
      yearDto.SiteTypes = this.SiteTypes.Count > 0 ? new List<LookupDTO>() : (List<LookupDTO>) null;
      yearDto.LocSites = this.LocSites.Count > 0 ? new List<LookupDTO>() : (List<LookupDTO>) null;
      yearDto.MaintRecs = this.MaintRecs.Count > 0 ? new List<LookupDTO>() : (List<LookupDTO>) null;
      yearDto.MaintTasks = this.MaintTasks.Count > 0 ? new List<LookupDTO>() : (List<LookupDTO>) null;
      yearDto.SidewalkDamages = this.SidewalkDamages.Count > 0 ? new List<LookupDTO>() : (List<LookupDTO>) null;
      yearDto.WireConflicts = this.WireConflicts.Count > 0 ? new List<LookupDTO>() : (List<LookupDTO>) null;
      yearDto.OtherOnes = this.OtherOnes.Count > 0 ? new List<LookupDTO>() : (List<LookupDTO>) null;
      yearDto.OtherTwos = this.OtherTwos.Count > 0 ? new List<LookupDTO>() : (List<LookupDTO>) null;
      yearDto.OtherThrees = this.OtherThrees.Count > 0 ? new List<LookupDTO>() : (List<LookupDTO>) null;
      yearDto.DBHs = this.DBHs.Count > 0 ? new List<DBHDTO>() : (List<DBHDTO>) null;
      yearDto.Conditions = this.Conditions.Count > 0 ? new List<ConditionDTO>() : (List<ConditionDTO>) null;
      yearDto.PlantingSiteTypes = this.PlantingSiteTypes.Count > 0 ? new List<PlantingSiteTypeDTO>() : (List<PlantingSiteTypeDTO>) null;
      yearDto.Revision = this.Revision;
      YearDTO dto = yearDto;
      foreach (Eco.Domain.v6.Strata stratum in (IEnumerable<Eco.Domain.v6.Strata>) this.Strata)
        dto.Strata.Add(stratum.GetDTO());
      foreach (GroundCover groundCover in (IEnumerable<GroundCover>) this.GroundCovers)
        dto.GroundCovers.Add(groundCover.GetDTO());
      foreach (LandUse landUse in (IEnumerable<LandUse>) this.LandUses)
        dto.LandUses.Add(landUse.GetDTO());
      foreach (SiteType siteType in (IEnumerable<SiteType>) this.SiteTypes)
        dto.SiteTypes.Add(siteType.GetDTO());
      foreach (LocSite locSite in (IEnumerable<LocSite>) this.LocSites)
        dto.LocSites.Add(locSite.GetDTO());
      foreach (MaintRec maintRec in (IEnumerable<MaintRec>) this.MaintRecs)
        dto.MaintRecs.Add(maintRec.GetDTO());
      foreach (MaintTask maintTask in (IEnumerable<MaintTask>) this.MaintTasks)
        dto.MaintTasks.Add(maintTask.GetDTO());
      foreach (Sidewalk sidewalkDamage in (IEnumerable<Sidewalk>) this.SidewalkDamages)
        dto.SidewalkDamages.Add(sidewalkDamage.GetDTO());
      foreach (WireConflict wireConflict in (IEnumerable<WireConflict>) this.WireConflicts)
        dto.WireConflicts.Add(wireConflict.GetDTO());
      foreach (Eco.Domain.v6.OtherOne otherOne in (IEnumerable<Eco.Domain.v6.OtherOne>) this.OtherOnes)
        dto.OtherOnes.Add(otherOne.GetDTO());
      foreach (Eco.Domain.v6.OtherTwo otherTwo in (IEnumerable<Eco.Domain.v6.OtherTwo>) this.OtherTwos)
        dto.OtherTwos.Add(otherTwo.GetDTO());
      foreach (Eco.Domain.v6.OtherThree otherThree in (IEnumerable<Eco.Domain.v6.OtherThree>) this.OtherThrees)
        dto.OtherThrees.Add(otherThree.GetDTO());
      foreach (DBH dbH in (IEnumerable<DBH>) this.DBHs)
        dto.DBHs.Add(dbH.GetDTO());
      foreach (Condition condition in (IEnumerable<Condition>) this.Conditions)
        dto.Conditions.Add(condition.GetDTO());
      foreach (PlantingSiteType plantingSiteType in (IEnumerable<PlantingSiteType>) this.PlantingSiteTypes)
        dto.PlantingSiteTypes.Add(plantingSiteType.GetDTO());
      return dto;
    }

    public override Year Clone(bool deep) => Year.Clone(this, new EntityMap(), false);

    public override object Clone() => (object) Year.Clone(this, new EntityMap());

    public static Year Clone(Year y, EntityMap map, bool deep = true)
    {
      Year eNew;
      if (map.Contains((Entity) y))
      {
        eNew = map.GetEntity<Year>(y);
      }
      else
      {
        eNew = new Year()
        {
          Id = y.Id,
          Budget = y.Budget,
          Unit = y.Unit,
          IsInitialMeasurement = y.IsInitialMeasurement,
          DBHActual = y.DBHActual,
          RecordStrata = y.RecordStrata,
          RecordGroundCover = y.RecordGroundCover,
          RecordStreetTree = y.RecordStreetTree,
          DefaultStreetTree = y.DefaultStreetTree,
          RecordPercentShrub = y.RecordPercentShrub,
          RecordPlotCenter = y.RecordPlotCenter,
          RecordReferenceObjects = y.RecordReferenceObjects,
          RecordHydro = y.RecordHydro,
          RecordShrub = y.RecordShrub,
          RecordEnergy = y.RecordEnergy,
          RecordPlantableSpace = y.RecordPlantableSpace,
          RecordCrownSize = y.RecordCrownSize,
          RecordCrownCondition = y.RecordCrownCondition,
          RecordHeight = y.RecordHeight
        };
        eNew.RecordStreetTree = y.RecordStreetTree;
        eNew.RecordSiteType = y.RecordSiteType;
        eNew.RecordLanduse = y.RecordLanduse;
        eNew.RecordLocSite = y.RecordLocSite;
        eNew.RecordLocNo = y.RecordLocNo;
        eNew.RecordMaintRec = y.RecordMaintRec;
        eNew.RecordMaintTask = y.RecordMaintTask;
        eNew.RecordSidewalk = y.RecordSidewalk;
        eNew.RecordWireConflict = y.RecordWireConflict;
        eNew.RecordPlotAddress = y.RecordPlotAddress;
        eNew.RecordTreeStatus = y.RecordTreeStatus;
        eNew.RecordTreeGPS = y.RecordTreeGPS;
        eNew.RecordTreeAddress = y.RecordTreeAddress;
        eNew.RecordTreeUserId = y.RecordTreeUserId;
        eNew.RecordOtherOne = y.RecordOtherOne;
        eNew.RecordOtherTwo = y.RecordOtherTwo;
        eNew.RecordOtherThree = y.RecordOtherThree;
        eNew.RecordGPS = y.RecordGPS;
        eNew.RecordCLE = y.RecordCLE;
        eNew.RecordIPED = y.RecordIPED;
        eNew.RecordNoteTree = y.RecordNoteTree;
        eNew.DisplayCondition = y.DisplayCondition;
        eNew.MgmtStyle = y.MgmtStyle;
        eNew.MobileKey = y.MobileKey;
        eNew.MobileEmail = y.MobileEmail;
        eNew.Comments = y.Comments;
        eNew.Changed = y.Changed;
        eNew.OtherOne = y.OtherOne;
        eNew.OtherTwo = y.OtherTwo;
        eNew.OtherThree = y.OtherThree;
        map.Add((Entity) y, (Entity) eNew);
        if (deep)
        {
          eNew.Carbon = ElementPrice.Clone<Carbon>(y.Carbon, map);
          eNew.CO = ElementPrice.Clone<CO>(y.CO, map);
          eNew.Electricity = ElementPrice.Clone<Electricity>(y.Electricity, map);
          eNew.NO2 = ElementPrice.Clone<NO2>(y.NO2, map);
          eNew.O3 = ElementPrice.Clone<O3>(y.O3, map);
          eNew.PM10 = ElementPrice.Clone<PM10>(y.PM10, map);
          eNew.PM25 = ElementPrice.Clone<PM25>(y.PM25, map);
          eNew.SO2 = ElementPrice.Clone<SO2>(y.SO2, map);
          eNew.Gas = ElementPrice.Clone<Gas>(y.Gas, map);
          eNew.H2O = ElementPrice.Clone<H2O>(y.H2O, map);
          eNew.ExchangeRate = ElementPrice.Clone<ExchangeRate>(y.ExchangeRate, map);
          eNew.Home = ElementPrice.Clone<Home>(y.Home, map);
          eNew.VOC = ElementPrice.Clone<VOC>(y.VOC, map);
          eNew.PopulationDensity = ElementPrice.Clone<PopulationDensity>(y.PopulationDensity, map);
          foreach (YearlyCost yearlyCost in (IEnumerable<YearlyCost>) y.YearlyCosts)
            eNew.YearlyCosts.Add(YearlyCost.Clone(yearlyCost, map));
          foreach (Eco.Domain.v6.YearLocationData wd in (IEnumerable<Eco.Domain.v6.YearLocationData>) y.YearLocationData)
            eNew.YearLocationData.Add(Eco.Domain.v6.YearLocationData.Clone(wd, map));
          foreach (LandUse landUse in (IEnumerable<LandUse>) y.LandUses)
            eNew.LandUses.Add(LandUse.Clone(landUse, map));
          foreach (GroundCover groundCover in (IEnumerable<GroundCover>) y.GroundCovers)
            eNew.GroundCovers.Add(GroundCover.Clone(groundCover, map));
          foreach (SiteType siteType in (IEnumerable<SiteType>) y.SiteTypes)
            eNew.SiteTypes.Add(Lookup<SiteType>.Clone(siteType, map));
          foreach (LocSite locSite in (IEnumerable<LocSite>) y.LocSites)
            eNew.LocSites.Add(Lookup<LocSite>.Clone(locSite, map));
          foreach (MaintRec maintRec in (IEnumerable<MaintRec>) y.MaintRecs)
            eNew.MaintRecs.Add(Lookup<MaintRec>.Clone(maintRec, map));
          foreach (MaintTask maintTask in (IEnumerable<MaintTask>) y.MaintTasks)
            eNew.MaintTasks.Add(Lookup<MaintTask>.Clone(maintTask, map));
          foreach (Sidewalk sidewalkDamage in (IEnumerable<Sidewalk>) y.SidewalkDamages)
            eNew.SidewalkDamages.Add(Lookup<Sidewalk>.Clone(sidewalkDamage, map));
          foreach (WireConflict wireConflict in (IEnumerable<WireConflict>) y.WireConflicts)
            eNew.WireConflicts.Add(Lookup<WireConflict>.Clone(wireConflict, map));
          foreach (Eco.Domain.v6.OtherOne otherOne in (IEnumerable<Eco.Domain.v6.OtherOne>) y.OtherOnes)
            eNew.OtherOnes.Add(Lookup<Eco.Domain.v6.OtherOne>.Clone(otherOne, map));
          foreach (Eco.Domain.v6.OtherTwo otherTwo in (IEnumerable<Eco.Domain.v6.OtherTwo>) y.OtherTwos)
            eNew.OtherTwos.Add(Lookup<Eco.Domain.v6.OtherTwo>.Clone(otherTwo, map));
          foreach (Eco.Domain.v6.OtherThree otherThree in (IEnumerable<Eco.Domain.v6.OtherThree>) y.OtherThrees)
            eNew.OtherThrees.Add(Lookup<Eco.Domain.v6.OtherThree>.Clone(otherThree, map));
          foreach (PlantingSiteType plantingSiteType in (IEnumerable<PlantingSiteType>) y.PlantingSiteTypes)
            eNew.PlantingSiteTypes.Add(PlantingSiteType.Clone(plantingSiteType, map));
          foreach (DBH dbH in (IEnumerable<DBH>) y.DBHs)
            eNew.DBHs.Add(DBH.Clone(dbH, map));
          foreach (DBHRptClass dbhRptClass in (IEnumerable<DBHRptClass>) y.DBHRptClasses)
            eNew.DBHRptClasses.Add(DBHRptClass.Clone(dbhRptClass, map));
          foreach (HealthRptClass healthRptClass in (IEnumerable<HealthRptClass>) y.HealthRptClasses)
            eNew.HealthRptClasses.Add(HealthRptClass.Clone(healthRptClass, map));
          foreach (Condition condition in (IEnumerable<Condition>) y.Conditions)
            eNew.Conditions.Add(Condition.Clone(condition, map));
          foreach (Eco.Domain.v6.Strata stratum in (IEnumerable<Eco.Domain.v6.Strata>) y.Strata)
            eNew.Strata.Add(Eco.Domain.v6.Strata.Clone(stratum, map));
          foreach (Plot plot in (IEnumerable<Plot>) y.Plots)
            eNew.Plots.Add(Plot.Clone(plot, map));
          foreach (Forecast forecast in (IEnumerable<Forecast>) y.Forecasts)
            eNew.Forecasts.Add(Forecast.Clone(forecast, map));
        }
      }
      if (deep)
        eNew.Series = map.GetEntity<Series>(y.Series);
      return eNew;
    }

    private class Opts
    {
      public const string DBHActual = "DBHActual";
      public const string RecHydro = "RecHydro";
      public const string RecShrub = "RecShrub";
      public const string RecEnergy = "RecEnergy";
      public const string RecPlantableSpace = "RecPlantableSpace";
      public const string RecStrata = "RecStrata";
      public const string RecPercentShrub = "RecPercentShrub";
      public const string RecPlotCenter = "RecPlotCenter";
      public const string RecReferenceObjects = "RecReferenceObjects";
      public const string RecGroundCover = "RecGroundCover";
      public const string RecCrownSize = "RecCrownSize";
      public const string RecCrownCondition = "RecCrownCondition";
      public const string RecHeight = "RecHeight";
      public const string RecSiteType = "RecSiteType";
      public const string RecStreetTree = "RecStreetTree";
      public const string StreetTreeDefault = "StreetTreeDefault";
      public const string RecLanduse = "RecLanduse";
      public const string RecLocSite = "RecLocSite";
      public const string RecLocNo = "RecLocNo";
      public const string RecMaintRec = "RecMaintRec";
      public const string RecMaintTask = "RecMaintTask";
      public const string RecSidewalk = "RecSidewalk";
      public const string RecTreeStatus = "RecTreeStatus";
      public const string RecWireConflict = "RecWireConflict";
      public const string RecPlotStreet = "RecPlotStreet";
      public const string RecPlotAddress = "RecPlotAddress";
      public const string RecTreeStreet = "RecTreeStreet";
      public const string RecTreeAddress = "RecTreeAddress";
      public const string RecOtherOne = "RecOtherOne";
      public const string RecOtherTwo = "RecOtherTwo";
      public const string RecOtherThree = "RecOtherThree";
      public const string RecGPS = "RecGPS";
      public const string RecTreeGPS = "RecTreeGPS";
      public const string RecCLE = "RecCLE";
      public const string RecIPED = "RecIPED";
      public const string RecNoteTree = "RecNoteTree";
      public const string RecMgmtStyle = "RecMgmtStyle";
      public const string MgmtStyleDefault = "MgmtStyleDefault";
      public const string DisplayCondition = "DisplayCondition";
      public const string FIA = "FIA";
      public const string RecTreeUserId = "RecTreeUserId";
    }
  }
}
