// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.Tree
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;
using NHibernate;
using System.Collections.Generic;

namespace Eco.Domain.v5
{
  public class Tree
  {
    private int? m_hash;
    private TreeId m_compositeId;
    private SubPlot m_subPlot;
    private ISet<Stem> m_stems;
    private ISet<BuildingInteraction> m_buildingInteractions;

    public Tree() => this.Init();

    public Tree(TreeId id)
    {
      this.m_compositeId = id;
      this.Init();
    }

    public virtual TreeId CompositeId => this.m_compositeId;

    public virtual SubPlot SubPlot => this.m_subPlot;

    public virtual int Id => this.IsTransient ? 0 : this.m_compositeId.Tree;

    public virtual ISet<Stem> Stems => this.m_stems;

    public virtual ISet<BuildingInteraction> BuildingInteractions => this.m_buildingInteractions;

    public virtual char FieldLandUse { get; set; }

    public virtual int DirectionFromCenter { get; set; }

    public virtual float DistanceFromCenter { get; set; }

    public virtual char Status { get; set; }

    public virtual string Species { get; set; }

    public virtual float CrownBaseHeight { get; set; }

    public virtual float CrownTopHeight { get; set; }

    public virtual float TreeHeight { get; set; }

    public virtual float CrownWidthNS { get; set; }

    public virtual float CrownWidthEW { get; set; }

    public virtual int CrownLightExposure { get; set; }

    public virtual int CrownPosition { get; set; }

    public virtual int PercentCrownMissing { get; set; }

    public virtual int CrownTransparency { get; set; }

    public virtual int CrownDensity { get; set; }

    public virtual int CrownDieback { get; set; }

    public virtual int PercentImpervious { get; set; }

    public virtual int PercentShrub { get; set; }

    public virtual char Site { get; set; }

    public virtual int PestTSDieback { get; set; }

    public virtual int PestTSEpiSprout { get; set; }

    public virtual int PestTSWiltFoli { get; set; }

    public virtual int PestTSEnvStress { get; set; }

    public virtual int PestTSHumStress { get; set; }

    public virtual string PestTSNotes { get; set; }

    public virtual int PestFTChewFoli { get; set; }

    public virtual int PestFTDiscFoli { get; set; }

    public virtual int PestFTAbnFoli { get; set; }

    public virtual int PestFTInsectSigns { get; set; }

    public virtual int PestFTFoliAffect { get; set; }

    public virtual string PestFTNotes { get; set; }

    public virtual int PestBBInsectSigns { get; set; }

    public virtual int PestBBInsectPres { get; set; }

    public virtual int PestBBDiseaseSigns { get; set; }

    public virtual int PestBBProbLoc { get; set; }

    public virtual int PestBBAbnGrowth { get; set; }

    public virtual string PestBBNotes { get; set; }

    public virtual int PestPest { get; set; }

    public virtual bool PestTS { get; set; }

    public virtual bool PestFT { get; set; }

    public virtual bool PestBB { get; set; }

    public virtual string Comments { get; set; }

    private void Init()
    {
      this.m_stems = (ISet<Stem>) new HashSet<Stem>();
      this.m_buildingInteractions = (ISet<BuildingInteraction>) new HashSet<BuildingInteraction>();
    }

    public virtual bool IsTransient => this.m_compositeId == null;

    public virtual TreeDTO GetDTO()
    {
      TreeDTO dto = new TreeDTO()
      {
        Id = this.Id,
        FieldLandUse = this.FieldLandUse,
        DirectionFromCenter = this.DirectionFromCenter,
        DistanceFromCenter = this.DistanceFromCenter,
        Status = this.Status,
        Species = this.Species,
        CrownBaseHeight = this.CrownBaseHeight,
        CrownTopHeight = this.CrownTopHeight,
        TreeHeight = this.TreeHeight,
        CrownWidthNS = this.CrownWidthNS,
        CrownWidthEW = this.CrownWidthEW,
        CrownLightExposure = this.CrownLightExposure,
        CrownPosition = this.CrownPosition,
        PercentCrownMissing = this.PercentCrownMissing,
        CrownTransparency = this.CrownTransparency,
        CrownDensity = this.CrownDensity,
        CrownDieback = this.CrownDieback,
        PercentImpervious = this.PercentImpervious,
        PercentShrub = this.PercentShrub,
        Site = this.Site,
        Comments = this.Comments,
        PestTSDieback = this.PestTSDieback,
        PestTSEpiSprout = this.PestTSEpiSprout,
        PestTSWiltFoli = this.PestTSWiltFoli,
        PestTSEnvStress = this.PestTSEnvStress,
        PestTSHumStress = this.PestTSHumStress,
        PestTSNotes = this.PestTSNotes,
        PestFTChewFoli = this.PestFTChewFoli,
        PestFTDiscFoli = this.PestFTDiscFoli,
        PestFTAbnFoli = this.PestFTAbnFoli,
        PestFTInsectSigns = this.PestFTInsectSigns,
        PestFTFoliAffect = this.PestFTFoliAffect,
        PestFTNotes = this.PestFTNotes,
        PestBBInsectSigns = this.PestBBInsectSigns,
        PestBBInsectPres = this.PestBBInsectPres,
        PestBBDiseaseSigns = this.PestBBDiseaseSigns,
        PestBBProbLoc = this.PestBBProbLoc,
        PestBBAbnGrowth = this.PestBBAbnGrowth,
        PestBBNotes = this.PestBBNotes,
        PestPest = this.PestPest,
        PestTS = this.PestTS,
        PestFT = this.PestFT,
        PestBB = this.PestBB,
        Stems = this.Stems.Count > 0 ? new List<StemDTO>() : (List<StemDTO>) null,
        Buildings = this.BuildingInteractions.Count > 0 ? new List<BuildingInteractionDTO>() : (List<BuildingInteractionDTO>) null
      };
      foreach (Stem stem in (IEnumerable<Stem>) this.Stems)
        dto.Stems.Add(stem.GetDTO());
      foreach (BuildingInteraction buildingInteraction in (IEnumerable<BuildingInteraction>) this.BuildingInteractions)
        dto.Buildings.Add(buildingInteraction.GetDTO());
      return dto;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Tree tree) || this.IsTransient ^ tree.IsTransient)
        return false;
      return this.IsTransient && tree.IsTransient ? this == tree : this.CompositeId.Equals((object) tree.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }

    public static void Delete(SubPlotId subplot, TreeDTO tree, ISession s)
    {
      TreeId id = new TreeId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, tree.Id);
      Tree tree1 = s.Get<Tree>((object) id);
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Delete((object) tree1);
        transaction.Commit();
      }
    }

    public static void Create(SubPlotId subplot, TreeDTO tree, ISession s)
    {
      TreeId treeId = new TreeId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, tree.Id);
      Tree tree1 = new Tree(treeId);
      tree1.Comments = tree.Comments;
      tree1.CrownBaseHeight = tree.CrownBaseHeight;
      tree1.CrownDieback = tree.CrownDieback;
      tree1.CrownLightExposure = tree.CrownLightExposure;
      tree1.CrownPosition = tree.CrownPosition;
      tree1.CrownTopHeight = tree.CrownTopHeight;
      tree1.CrownWidthEW = tree.CrownWidthEW;
      tree1.CrownWidthNS = tree.CrownWidthNS;
      tree1.DirectionFromCenter = tree.DirectionFromCenter;
      tree1.DistanceFromCenter = tree.DistanceFromCenter;
      tree1.FieldLandUse = tree.FieldLandUse;
      tree1.PercentCrownMissing = tree.PercentCrownMissing;
      tree1.CrownTransparency = tree.CrownTransparency;
      tree1.CrownDensity = tree.CrownDensity;
      tree1.PercentImpervious = tree.PercentImpervious;
      tree1.PercentShrub = tree.PercentShrub;
      tree1.PestBB = tree.PestBB;
      tree1.PestBBAbnGrowth = tree.PestBBAbnGrowth;
      tree1.PestBBDiseaseSigns = tree.PestBBDiseaseSigns;
      tree1.PestBBInsectPres = tree.PestBBInsectPres;
      tree1.PestBBInsectSigns = tree.PestBBInsectSigns;
      tree1.PestBBNotes = tree.PestBBNotes;
      tree1.PestBBProbLoc = tree.PestBBProbLoc;
      tree1.PestFT = tree.PestFT;
      tree1.PestFTAbnFoli = tree.PestFTAbnFoli;
      tree1.PestFTChewFoli = tree.PestFTChewFoli;
      tree1.PestFTDiscFoli = tree.PestFTDiscFoli;
      tree1.PestFTFoliAffect = tree.PestFTFoliAffect;
      tree1.PestFTInsectSigns = tree.PestFTInsectSigns;
      tree1.PestFTNotes = tree.PestFTNotes;
      tree1.PestPest = tree.PestPest;
      tree1.PestTS = tree.PestTS;
      tree1.PestTSDieback = tree.PestTSDieback;
      tree1.PestTSEnvStress = tree.PestTSEnvStress;
      tree1.PestTSEpiSprout = tree.PestTSEpiSprout;
      tree1.PestTSHumStress = tree.PestTSHumStress;
      tree1.PestTSNotes = tree.PestTSNotes;
      tree1.PestTSWiltFoli = tree.PestTSWiltFoli;
      tree1.Site = tree.Site;
      tree1.Species = tree.Species;
      tree1.Status = tree.Status;
      tree1.TreeHeight = tree.TreeHeight;
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Save((object) tree1);
        transaction.Commit();
      }
      if (tree.Buildings != null)
      {
        foreach (BuildingInteractionDTO building in tree.Buildings)
          BuildingInteraction.Create(treeId, building, s);
      }
      if (tree.Stems == null)
        return;
      foreach (StemDTO stem in tree.Stems)
        Stem.Create(treeId, stem, s);
    }

    public static void Update(SubPlotId subplot, TreeDTO tree, ISession s)
    {
      TreeId treeId = new TreeId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, tree.Id);
      Tree tree1 = s.Get<Tree>((object) treeId);
      using (ITransaction transaction = s.BeginTransaction())
      {
        tree1.Comments = tree.Comments;
        tree1.CrownBaseHeight = tree.CrownBaseHeight;
        tree1.CrownDieback = tree.CrownDieback;
        tree1.CrownLightExposure = tree.CrownLightExposure;
        tree1.CrownPosition = tree.CrownPosition;
        tree1.CrownTopHeight = tree.CrownTopHeight;
        tree1.CrownWidthEW = tree.CrownWidthEW;
        tree1.CrownWidthNS = tree.CrownWidthNS;
        tree1.DirectionFromCenter = tree.DirectionFromCenter;
        tree1.DistanceFromCenter = tree.DistanceFromCenter;
        tree1.FieldLandUse = tree.FieldLandUse;
        tree1.PercentCrownMissing = tree.PercentCrownMissing;
        tree1.CrownTransparency = tree.CrownTransparency;
        tree1.CrownDensity = tree.CrownDensity;
        tree1.PercentImpervious = tree.PercentImpervious;
        tree1.PercentShrub = tree.PercentShrub;
        tree1.PestBB = tree.PestBB;
        tree1.PestBBAbnGrowth = tree.PestBBAbnGrowth;
        tree1.PestBBDiseaseSigns = tree.PestBBDiseaseSigns;
        tree1.PestBBInsectPres = tree.PestBBInsectPres;
        tree1.PestBBInsectSigns = tree.PestBBInsectSigns;
        tree1.PestBBNotes = tree.PestBBNotes;
        tree1.PestBBProbLoc = tree.PestBBProbLoc;
        tree1.PestFT = tree.PestFT;
        tree1.PestFTAbnFoli = tree.PestFTAbnFoli;
        tree1.PestFTChewFoli = tree.PestFTChewFoli;
        tree1.PestFTDiscFoli = tree.PestFTDiscFoli;
        tree1.PestFTFoliAffect = tree.PestFTFoliAffect;
        tree1.PestFTInsectSigns = tree.PestFTInsectSigns;
        tree1.PestFTNotes = tree.PestFTNotes;
        tree1.PestPest = tree.PestPest;
        tree1.PestTS = tree.PestTS;
        tree1.PestTSDieback = tree.PestTSDieback;
        tree1.PestTSEnvStress = tree.PestTSEnvStress;
        tree1.PestTSEpiSprout = tree.PestTSEpiSprout;
        tree1.PestTSHumStress = tree.PestTSHumStress;
        tree1.PestTSNotes = tree.PestTSNotes;
        tree1.PestTSWiltFoli = tree.PestTSWiltFoli;
        tree1.Site = tree.Site;
        tree1.Species = tree.Species;
        tree1.Status = tree.Status;
        tree1.TreeHeight = tree.TreeHeight;
        transaction.Commit();
      }
      if (tree.Buildings != null)
      {
        foreach (BuildingInteractionDTO building in tree.Buildings)
        {
          if ((building.State & State.Deleted) != State.Existing)
            BuildingInteraction.Delete(treeId, building, s);
          else if ((building.State & State.New) != State.Existing)
            BuildingInteraction.Create(treeId, building, s);
          else if ((building.State & State.Modified) != State.Existing)
            BuildingInteraction.Update(treeId, building, s);
        }
      }
      if (tree.Stems == null)
        return;
      foreach (StemDTO stem in tree.Stems)
      {
        if ((stem.State & State.Deleted) != State.Existing)
          Stem.Delete(treeId, stem, s);
        else if ((stem.State & State.New) != State.Existing)
          Stem.Create(treeId, stem, s);
        else if ((stem.State & State.Modified) != State.Existing)
          Stem.Update(treeId, stem, s);
      }
    }
  }
}
