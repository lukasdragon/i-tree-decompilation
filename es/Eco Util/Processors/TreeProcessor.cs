// Decompiled with JetBrains decompiler
// Type: Eco.Util.Processors.TreeProcessor
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.DTO.v6;
using Eco.Domain.v6;
using System;

namespace Eco.Util.Processors
{
  public class TreeProcessor : Processor
  {
    public TreeProcessor(Updater updater)
      : base(updater)
    {
    }

    public void Create(TreeDTO dto, Plot p)
    {
      if (this.Updater.Session.Get<Tree>((object) dto.Guid) != null)
        return;
      Tree t = new Tree() { Plot = p };
      this.UpdateTree(dto, t);
      this.Save(dto, t);
    }

    private void Save(TreeDTO dto, Tree t)
    {
      this.Updater.Session.Save((object) t, (object) dto.Guid);
      if (dto.Buildings != null)
      {
        foreach (BuildingDTO building in dto.Buildings)
        {
          if ((building.State & State.Deleted) == State.Existing)
            this.Updater.BuildingProcessor.Create(building, t);
        }
      }
      if (dto.Stems == null)
        return;
      foreach (StemDTO stem in dto.Stems)
      {
        if ((stem.State & State.Deleted) == State.Existing)
          this.Updater.StemProcessor.Create(stem, t);
      }
    }

    public void Update(TreeDTO dto, Plot p)
    {
      Tree tree1 = this.Updater.Session.Get<Tree>((object) dto.Guid);
      if (tree1 == null)
      {
        Tree tree2 = new Tree() { Plot = p };
        this.UpdateTree(dto, tree2);
        switch (this.Updater.ResolveConflict(Conflict.EntityDeleted, (Entity) tree2, (Entity) null, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Save(dto, tree2);
            break;
        }
      }
      else if (tree1.Revision != dto.Revision)
      {
        Tree tree3 = new Tree();
        tree3.Plot = p;
        tree3.Revision = tree1.Revision;
        Tree tree4 = tree3;
        this.UpdateTree(dto, tree4);
        switch (this.Updater.ResolveConflict(Conflict.RevisionMismatch, (Entity) tree4, (Entity) tree1, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Updater.Session.Delete((object) tree1);
            this.Updater.Session.Flush();
            this.Save(dto, tree4);
            break;
        }
      }
      else
      {
        if ((dto.State & State.Modified) != State.Existing)
        {
          this.UpdateTree(dto, tree1);
          this.Updater.Session.Update((object) tree1);
        }
        if (dto.Buildings != null)
        {
          foreach (BuildingDTO building in dto.Buildings)
          {
            if ((dto.State & State.Deleted) != State.Existing)
              this.Updater.BuildingProcessor.Delete(building);
            else if ((dto.State & State.New) != State.Existing)
              this.Updater.BuildingProcessor.Create(building, tree1);
            else
              this.Updater.BuildingProcessor.Update(building, tree1);
          }
        }
        if (dto.Stems == null)
          return;
        foreach (StemDTO stem in dto.Stems)
        {
          if ((dto.State & State.Deleted) != State.Existing)
            this.Updater.StemProcessor.Delete(stem);
          else if ((dto.State & State.New) != State.Existing)
            this.Updater.StemProcessor.Create(stem, tree1);
          else
            this.Updater.StemProcessor.Update(stem, tree1);
        }
      }
    }

    public void Delete(TreeDTO dto) => this.DeleteEntity<Tree>(dto.Guid);

    private void UpdateTree(TreeDTO dto, Tree t)
    {
      t.Id = dto.Id;
      t.UserId = dto.UserId;
      t.Crown = dto.Crown != null ? this.UpdateCrown(dto.Crown, new Crown()) : new Crown();
      t.IPED = dto.IPED != null ? this.UpdateIPED(dto.IPED, new IPED()) : new IPED();
      t.PlotLandUse = dto.PlotLandUse.HasValue ? this.Updater.Session.Load<PlotLandUse>((object) dto.PlotLandUse) : (PlotLandUse) null;
      t.CityManaged = dto.CityManaged;
      t.DirectionFromCenter = dto.Direction.GetValueOrDefault(-1);
      t.DistanceFromCenter = dto.Distance.GetValueOrDefault(-1f);
      t.Status = dto.Status.GetValueOrDefault('O');
      t.Species = dto.Species;
      t.TreeHeight = dto.TreeHeight.GetValueOrDefault(-1f);
      t.PercentImpervious = (PctMidRange) dto.PercentImpervious.GetValueOrDefault(-1);
      t.PercentShrub = (PctMidRange) dto.PercentShrub.GetValueOrDefault(-1);
      t.StreetTree = dto.StreetTree.GetValueOrDefault(false);
      t.SiteType = dto.SiteType.HasValue ? this.Updater.Session.Load<SiteType>((object) dto.SiteType) : (SiteType) null;
      t.Street = dto.Street.HasValue ? this.Updater.Session.Load<Street>((object) dto.Street) : (Street) null;
      t.Address = dto.Address;
      t.LocSite = dto.LocSite.HasValue ? this.Updater.Session.Load<LocSite>((object) dto.LocSite) : (LocSite) null;
      t.LocNo = dto.LocNo;
      t.MaintRec = dto.MaintRec.HasValue ? this.Updater.Session.Load<MaintRec>((object) dto.MaintRec) : (MaintRec) null;
      t.MaintTask = dto.MaintTask.HasValue ? this.Updater.Session.Load<MaintTask>((object) dto.MaintTask) : (MaintTask) null;
      t.SidewalkDamage = dto.SidewalkDamage.HasValue ? this.Updater.Session.Load<Sidewalk>((object) dto.SidewalkDamage) : (Sidewalk) null;
      t.WireConflict = dto.WireConflict.HasValue ? this.Updater.Session.Load<WireConflict>((object) dto.WireConflict) : (WireConflict) null;
      t.OtherOne = dto.OtherOne.HasValue ? this.Updater.Session.Load<OtherOne>((object) dto.OtherOne) : (OtherOne) null;
      t.OtherTwo = dto.OtherTwo.HasValue ? this.Updater.Session.Load<OtherTwo>((object) dto.OtherTwo) : (OtherTwo) null;
      t.OtherThree = dto.OtherThree.HasValue ? this.Updater.Session.Load<OtherThree>((object) dto.OtherThree) : (OtherThree) null;
      t.Latitude = new double?(dto.Latitude.GetValueOrDefault());
      t.Longitude = new double?(dto.Longitude.GetValueOrDefault());
      t.SurveyDate = new DateTime?(dto.SurveyDate.GetValueOrDefault());
      t.NoteThisTree = dto.NoteThisTree.GetValueOrDefault(false);
      t.Comments = dto.Comments;
      t.Revision = Math.Max(t.Revision, dto.Revision) + 1;
    }

    private Crown UpdateCrown(CrownDTO dto, Crown c)
    {
      c.BaseHeight = dto.BaseHeight.GetValueOrDefault(-1f);
      c.TopHeight = dto.TopHeight.GetValueOrDefault(-1f);
      c.WidthNS = dto.WidthNS.GetValueOrDefault(-1f);
      c.WidthEW = dto.WidthEW.GetValueOrDefault(-1f);
      c.LightExposure = (CrownLightExposure) dto.LightExposure.GetValueOrDefault(-1);
      c.PercentMissing = (PctMidRange) dto.PercentMissing.GetValueOrDefault(-1);
      c.Condition = dto.Condition.HasValue ? this.Updater.Session.Load<Condition>((object) dto.Condition) : (Condition) null;
      return c;
    }

    private IPED UpdateIPED(IPEDDTO dto, IPED iped)
    {
      iped.TSDieback = dto.TSDieback.GetValueOrDefault();
      iped.TSEpiSprout = dto.TSEpiSprout.GetValueOrDefault();
      iped.TSWiltFoli = dto.TSWiltFoli.GetValueOrDefault();
      iped.TSEnvStress = dto.TSEnvStress.GetValueOrDefault();
      iped.TSNotes = dto.TSNotes;
      iped.FTChewFoli = dto.FTChewFoli.GetValueOrDefault();
      iped.FTDiscFoli = dto.FTDiscFoli.GetValueOrDefault();
      iped.FTAbnFoli = dto.FTAbnFoli.GetValueOrDefault();
      iped.FTInsectSigns = dto.FTInsectSigns.GetValueOrDefault();
      iped.FTFoliAffect = dto.FTFoliAffect.GetValueOrDefault();
      iped.FTNotes = dto.FTNotes;
      iped.BBInsectSigns = dto.BBInsectSigns.GetValueOrDefault();
      iped.BBInsectPres = dto.BBInsectPres.GetValueOrDefault();
      iped.BBDiseaseSigns = dto.BBDiseaseSigns.GetValueOrDefault();
      iped.BBProbLoc = dto.BBProbLoc.GetValueOrDefault();
      iped.BBAbnGrowth = dto.BBAbnGrowth.GetValueOrDefault();
      iped.BBNotes = dto.BBNotes;
      iped.Pest = dto.Pest.GetValueOrDefault();
      return iped;
    }
  }
}
