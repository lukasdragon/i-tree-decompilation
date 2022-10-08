// Decompiled with JetBrains decompiler
// Type: Eco.Domain.DTO.v6.YearDTO
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System.Collections.Generic;

namespace Eco.Domain.DTO.v6
{
  public class YearDTO : EntityDTO
  {
    public short Id;
    public char Unit;
    public bool DBHActual;
    public bool RecordStrata;
    public bool RecordHydro;
    public bool RecordShrub;
    public bool RecordEnergy;
    public bool RecordPlantableSpace;
    public bool RecordCrownSize;
    public bool RecordCrownCondition;
    public bool RecordHeight;
    public bool RecordPercentShrub;
    public bool RecordPlotCenter;
    public bool RecordReferenceObjects;
    public bool RecordGroundCover;
    public bool RecordStreetTree;
    public bool DefaultStreetTree;
    public bool RecordSiteType;
    public bool RecordLanduse;
    public bool RecordLocSite;
    public bool RecordLocNo;
    public bool RecordMaintRec;
    public bool RecordMaintTask;
    public bool RecordSidewalk;
    public bool RecordWireConflict;
    public bool RecordPlotStreet;
    public bool RecordPlotAddress;
    public bool RecordTreeStreet;
    public bool RecordTreeAddress;
    public bool RecordTreeStatus;
    public bool RecordTreeUserId;
    public bool RecordOtherOne;
    public bool RecordOtherTwo;
    public bool RecordOtherThree;
    public bool RecordGPS;
    public bool RecordTreeGPS;
    public bool RecordCLE;
    public bool RecordIPED;
    public bool RecordNoteTree;
    public bool DisplayCondition;
    public short? MgmtStyle;
    public string OtherOne;
    public string OtherTwo;
    public string OtherThree;
    public bool IsInitialMeasurement;
    public SeriesDTO Series;
    public List<PlotDTO> Plots;
    public List<StrataDTO> Strata;
    public List<GroundCoverDTO> GroundCovers;
    public List<LandUseDTO> LandUses;
    public List<LookupDTO> SiteTypes;
    public List<LookupDTO> LocSites;
    public List<LookupDTO> MaintRecs;
    public List<LookupDTO> MaintTasks;
    public List<LookupDTO> SidewalkDamages;
    public List<LookupDTO> WireConflicts;
    public List<LookupDTO> OtherOnes;
    public List<LookupDTO> OtherTwos;
    public List<LookupDTO> OtherThrees;
    public List<DBHDTO> DBHs;
    public List<ConditionDTO> Conditions;
    public List<PlantingSiteTypeDTO> PlantingSiteTypes;
  }
}
