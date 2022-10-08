// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.SubPlot
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;
using NHibernate;
using System.Collections.Generic;

namespace Eco.Domain.v5
{
  public class SubPlot
  {
    private int? m_hash;
    private SubPlotId m_compositeId;
    private Plot m_plot;
    private ISet<Tree> m_trees;
    private ISet<Shrub> m_shrubs;
    private ISet<FieldLandUse> m_fieldLandUses;
    private ISet<ReferenceObject> m_referenceObjects;
    private ISet<SubPlotCover> m_covers;

    public SubPlot() => this.Init();

    public SubPlot(SubPlotId id)
    {
      this.m_compositeId = id;
      this.Init();
    }

    public virtual SubPlotId CompositeId => this.m_compositeId;

    public virtual Plot Plot => this.m_plot;

    public virtual int Id => this.IsTransient ? 0 : this.m_compositeId.SubPlot;

    public virtual ISet<Tree> Trees => this.m_trees;

    public virtual ISet<Shrub> Shrubs => this.m_shrubs;

    public virtual ISet<FieldLandUse> FieldLandUses => this.m_fieldLandUses;

    public virtual ISet<ReferenceObject> ReferenceObjects => this.m_referenceObjects;

    public virtual ISet<SubPlotCover> Covers => this.m_covers;

    public virtual float Size { get; set; }

    public virtual int OffsetPoint { get; set; }

    public virtual string Photo { get; set; }

    public virtual bool Stake { get; set; }

    public virtual short PercentTreeCover { get; set; }

    public virtual short PercentShrubCover { get; set; }

    public virtual short PercentPlantable { get; set; }

    public virtual short PercentMeasured { get; set; }

    public virtual string Comments { get; set; }

    private void Init()
    {
      this.m_trees = (ISet<Tree>) new HashSet<Tree>();
      this.m_shrubs = (ISet<Shrub>) new HashSet<Shrub>();
      this.m_fieldLandUses = (ISet<FieldLandUse>) new HashSet<FieldLandUse>();
      this.m_referenceObjects = (ISet<ReferenceObject>) new HashSet<ReferenceObject>();
      this.m_covers = (ISet<SubPlotCover>) new HashSet<SubPlotCover>();
    }

    public virtual bool IsTransient => this.m_compositeId == null;

    public virtual SubPlotDTO GetDTO()
    {
      SubPlotDTO dto = new SubPlotDTO()
      {
        Id = this.Id,
        Comments = this.Comments,
        OffsetPoint = this.OffsetPoint,
        PercentMeasured = this.PercentMeasured,
        PercentPlantable = this.PercentPlantable,
        PercentShrubCover = this.PercentShrubCover,
        PercentTreeCover = this.PercentTreeCover,
        Size = this.Size,
        Stake = this.Stake,
        Shrubs = this.Shrubs.Count > 0 ? new List<ShrubDTO>() : (List<ShrubDTO>) null,
        Trees = this.Trees.Count > 0 ? new List<TreeDTO>() : (List<TreeDTO>) null,
        LandUses = this.FieldLandUses.Count > 0 ? new List<FieldLandUseDTO>() : (List<FieldLandUseDTO>) null,
        ReferenceObjects = this.ReferenceObjects.Count > 0 ? new List<ReferenceObjectDTO>() : (List<ReferenceObjectDTO>) null,
        Covers = this.Covers.Count > 0 ? new List<SubPlotCoverDTO>() : (List<SubPlotCoverDTO>) null,
        Photo = this.Photo
      };
      foreach (Shrub shrub in (IEnumerable<Shrub>) this.Shrubs)
        dto.Shrubs.Add(shrub.GetDTO());
      foreach (Tree tree in (IEnumerable<Tree>) this.Trees)
        dto.Trees.Add(tree.GetDTO());
      foreach (FieldLandUse fieldLandUse in (IEnumerable<FieldLandUse>) this.FieldLandUses)
        dto.LandUses.Add(fieldLandUse.GetDTO());
      foreach (ReferenceObject referenceObject in (IEnumerable<ReferenceObject>) this.ReferenceObjects)
        dto.ReferenceObjects.Add(referenceObject.GetDTO());
      foreach (SubPlotCover cover in (IEnumerable<SubPlotCover>) this.Covers)
        dto.Covers.Add(cover.GetDTO());
      return dto;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is SubPlot subPlot) || this.IsTransient ^ subPlot.IsTransient)
        return false;
      return this.IsTransient && subPlot.IsTransient ? this == subPlot : this.CompositeId.Equals((object) subPlot.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }

    public static void Update(PlotId plot, SubPlotDTO subplot, ISession s)
    {
      SubPlotId subPlotId = new SubPlotId(plot.Location, plot.Series, plot.Year, plot.Plot, subplot.Id);
      SubPlot subPlot = s.Get<SubPlot>((object) subPlotId);
      using (ITransaction transaction = s.BeginTransaction())
      {
        subPlot.Comments = subplot.Comments;
        subPlot.OffsetPoint = subplot.OffsetPoint;
        subPlot.PercentMeasured = subplot.PercentMeasured;
        subPlot.PercentPlantable = subplot.PercentPlantable;
        subPlot.PercentShrubCover = subplot.PercentShrubCover;
        subPlot.PercentTreeCover = subplot.PercentTreeCover;
        subPlot.Photo = subplot.Photo;
        subPlot.Size = subplot.Size;
        subPlot.Stake = subplot.Stake;
        transaction.Commit();
      }
      if (subplot.Covers != null)
      {
        foreach (SubPlotCoverDTO cover in subplot.Covers)
        {
          if ((cover.State & State.Deleted) != State.Existing)
            SubPlotCover.Delete(subPlotId, cover, s);
          else if ((cover.State & State.New) != State.Existing)
            SubPlotCover.Create(subPlotId, cover, s);
          else if ((cover.State & State.Modified) != State.Existing)
            SubPlotCover.Update(subPlotId, cover, s);
        }
      }
      if (subplot.LandUses != null)
      {
        foreach (FieldLandUseDTO landUse in subplot.LandUses)
        {
          if ((landUse.State & State.Deleted) != State.Existing)
            FieldLandUse.Delete(subPlotId, landUse, s);
          else if ((landUse.State & State.New) != State.Existing)
            FieldLandUse.Create(subPlotId, landUse, s);
          else if ((landUse.State & State.Modified) != State.Existing)
            FieldLandUse.Update(subPlotId, landUse, s);
        }
      }
      if (subplot.ReferenceObjects != null)
      {
        foreach (ReferenceObjectDTO referenceObject in subplot.ReferenceObjects)
        {
          if ((referenceObject.State & State.Deleted) != State.Existing)
            ReferenceObject.Delete(subPlotId, referenceObject, s);
          else if ((referenceObject.State & State.New) != State.Existing)
            ReferenceObject.Create(subPlotId, referenceObject, s);
          else if ((referenceObject.State & State.Modified) != State.Existing)
            ReferenceObject.Update(subPlotId, referenceObject, s);
        }
      }
      if (subplot.Trees != null)
      {
        foreach (TreeDTO tree in subplot.Trees)
        {
          if ((tree.State & State.Deleted) != State.Existing)
            Tree.Delete(subPlotId, tree, s);
          else if ((tree.State & State.New) != State.Existing)
            Tree.Create(subPlotId, tree, s);
          else if ((tree.State & State.Modified) != State.Existing)
            Tree.Update(subPlotId, tree, s);
        }
      }
      if (subplot.Shrubs == null)
        return;
      foreach (ShrubDTO shrub in subplot.Shrubs)
      {
        if ((shrub.State & State.Deleted) != State.Existing)
          Shrub.Delete(subPlotId, shrub, s);
        else if ((shrub.State & State.New) != State.Existing)
          Shrub.Create(subPlotId, shrub, s);
        else if ((shrub.State & State.Modified) != State.Existing)
          Shrub.Update(subPlotId, shrub, s);
      }
    }

    public static void Create(PlotId plot, SubPlotDTO subplot, ISession s)
    {
      SubPlotId subPlotId = new SubPlotId(plot.Location, plot.Series, plot.Year, plot.Plot, subplot.Id);
      SubPlot subPlot = new SubPlot(subPlotId);
      subPlot.Comments = subplot.Comments;
      subPlot.OffsetPoint = subplot.OffsetPoint;
      subPlot.PercentMeasured = subplot.PercentMeasured;
      subPlot.PercentPlantable = subplot.PercentPlantable;
      subPlot.PercentShrubCover = subplot.PercentShrubCover;
      subPlot.PercentTreeCover = subplot.PercentTreeCover;
      subPlot.Photo = subplot.Photo;
      subPlot.Size = subplot.Size;
      subPlot.Stake = subplot.Stake;
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Save((object) subPlot);
        transaction.Commit();
      }
      if (subplot.Covers != null)
      {
        foreach (SubPlotCoverDTO cover in subplot.Covers)
          SubPlotCover.Create(subPlotId, cover, s);
      }
      if (subplot.LandUses != null)
      {
        foreach (FieldLandUseDTO landUse in subplot.LandUses)
          FieldLandUse.Create(subPlotId, landUse, s);
      }
      if (subplot.ReferenceObjects != null)
      {
        foreach (ReferenceObjectDTO referenceObject in subplot.ReferenceObjects)
          ReferenceObject.Create(subPlotId, referenceObject, s);
      }
      if (subplot.Trees != null)
      {
        foreach (TreeDTO tree in subplot.Trees)
          Tree.Create(subPlotId, tree, s);
      }
      if (subplot.Shrubs == null)
        return;
      foreach (ShrubDTO shrub in subplot.Shrubs)
        Shrub.Create(subPlotId, shrub, s);
    }
  }
}
