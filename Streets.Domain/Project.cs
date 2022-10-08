// Decompiled with JetBrains decompiler
// Type: Streets.Domain.Project
// Assembly: Streets.Domain, Version=1.1.5560.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C8C80D5-C46C-4F96-B160-AECDF3D99BE1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Streets.Domain.dll

using System.Collections.Generic;

namespace Streets.Domain
{
  public class Project
  {
    private ISet<InventoryItem> m_items;
    private ISet<ManagementCost> m_managementCosts;
    private ISet<Street> m_streets;
    private ISet<Zone> m_zones;
    private ISet<SpListItem> m_spList;
    private ISet<FieldLandUse> m_fieldLandUses;
    private ISet<LocSite> m_locSites;
    private ISet<MaintRec> m_maintRecs;
    private ISet<MaintTask> m_maintTasks;
    private ISet<OtherOne> m_otherOnes;
    private ISet<OtherTwo> m_otherTwos;
    private ISet<OtherThree> m_otherThrees;
    private ISet<DBH> m_dbhs;
    private ISet<Condition> m_conditions;
    private ISet<Sidewalk> m_sidewalkDamages;
    private ISet<SiteType> m_siteTypes;
    private ISet<WireConflict> m_wireConflicts;

    public Project()
    {
      this.m_items = (ISet<InventoryItem>) new HashSet<InventoryItem>();
      this.m_managementCosts = (ISet<ManagementCost>) new HashSet<ManagementCost>();
      this.m_streets = (ISet<Street>) new HashSet<Street>();
      this.m_zones = (ISet<Zone>) new HashSet<Zone>();
      this.m_spList = (ISet<SpListItem>) new HashSet<SpListItem>();
      this.m_fieldLandUses = (ISet<FieldLandUse>) new HashSet<FieldLandUse>();
      this.m_locSites = (ISet<LocSite>) new HashSet<LocSite>();
      this.m_maintRecs = (ISet<MaintRec>) new HashSet<MaintRec>();
      this.m_maintTasks = (ISet<MaintTask>) new HashSet<MaintTask>();
      this.m_otherOnes = (ISet<OtherOne>) new HashSet<OtherOne>();
      this.m_otherTwos = (ISet<OtherTwo>) new HashSet<OtherTwo>();
      this.m_otherThrees = (ISet<OtherThree>) new HashSet<OtherThree>();
      this.m_dbhs = (ISet<DBH>) new HashSet<DBH>();
      this.m_conditions = (ISet<Condition>) new HashSet<Condition>();
      this.m_sidewalkDamages = (ISet<Sidewalk>) new HashSet<Sidewalk>();
      this.m_siteTypes = (ISet<SiteType>) new HashSet<SiteType>();
      this.m_wireConflicts = (ISet<WireConflict>) new HashSet<WireConflict>();
    }

    public virtual int Id { get; set; }

    public virtual bool EnglishUnits { get; set; }

    public virtual string PDADataPath { get; set; }

    public virtual string StorageCardPath { get; set; }

    public virtual bool EnablePublicPrivate { get; set; }

    public virtual bool DBHDirectEntry { get; set; }

    public virtual bool EnableOther1 { get; set; }

    public virtual bool EnableOther2 { get; set; }

    public virtual bool EnableOther3 { get; set; }

    public virtual string Other1Description { get; set; }

    public virtual string Other2Description { get; set; }

    public virtual string Other3Description { get; set; }

    public virtual bool EnableGPS { get; set; }

    public virtual bool EnableBlock { get; set; }

    public virtual bool ShowSpeciesCommonName { get; set; }

    public virtual bool EnableLandUse { get; set; }

    public virtual bool EnableSiteType { get; set; }

    public virtual bool EnableLocSite { get; set; }

    public virtual bool EnableLocNo { get; set; }

    public virtual bool EnableMaintenance { get; set; }

    public virtual bool EnableSideWalk { get; set; }

    public virtual bool EnableWireConflict { get; set; }

    public virtual bool EnableCondition { get; set; }

    public virtual bool EnableStreetAddress { get; set; }

    public virtual bool EnableArea { get; set; }

    public virtual bool EnableSurveyor { get; set; }

    public virtual bool EnableSurveyDate { get; set; }

    public virtual bool EnablePest { get; set; }

    public virtual short Year { get; set; }

    public virtual string Name { get; set; }

    public virtual bool IsSample { get; set; }

    public virtual string ClimateZone { get; set; }

    public virtual string NationCode { get; set; }

    public virtual string PrimaryPartitionCode { get; set; }

    public virtual string SecondaryPartitionCode { get; set; }

    public virtual string TertiaryPartitionCode { get; set; }

    public virtual double AvgStreetLength { get; set; }

    public virtual CityData CityData { get; set; }

    public virtual Benefit Benefit { get; set; }

    public virtual ISet<InventoryItem> Items => this.m_items;

    public virtual ISet<ManagementCost> ManagementCosts => this.m_managementCosts;

    public virtual ISet<Street> Streets => this.m_streets;

    public virtual ISet<Zone> Zones => this.m_zones;

    public virtual ISet<SpListItem> SpList => this.m_spList;

    public virtual ISet<FieldLandUse> FieldLandUses => this.m_fieldLandUses;

    public virtual ISet<LocSite> LocSites => this.m_locSites;

    public virtual ISet<MaintRec> MaintRecs => this.m_maintRecs;

    public virtual ISet<MaintTask> MaintTasks => this.m_maintTasks;

    public virtual ISet<OtherOne> OtherOnes => this.m_otherOnes;

    public virtual ISet<OtherTwo> OtherTwos => this.m_otherTwos;

    public virtual ISet<OtherThree> OtherThrees => this.m_otherThrees;

    public virtual ISet<DBH> Dbhs => this.m_dbhs;

    public virtual ISet<Condition> Conditions => this.m_conditions;

    public virtual ISet<Sidewalk> SidewalkDamages => this.m_sidewalkDamages;

    public virtual ISet<SiteType> SiteTypes => this.m_siteTypes;

    public virtual ISet<WireConflict> WireConflicts => this.m_wireConflicts;

    public virtual bool IsTransient => this.Id == 0;

    public override bool Equals(object obj) => obj is Project project && !(this.GetType() != project.GetType()) && !(this.IsTransient ^ project.IsTransient) && this == project && this.Id.Equals(project.Id);

    public override int GetHashCode() => 3 * this.Id;
  }
}
