// Decompiled with JetBrains decompiler
// Type: Eco.Util.Processors.PlotProcessor
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.DTO.v6;
using Eco.Domain.v6;
using System;

namespace Eco.Util.Processors
{
  public class PlotProcessor : Processor
  {
    public PlotProcessor(Updater updater)
      : base(updater)
    {
    }

    public void Create(PlotDTO dto, Year y)
    {
      if (this.Updater.Session.Get<Plot>((object) dto.Guid) != null)
        return;
      Plot p = new Plot() { Year = y };
      this.UpdatePlot(dto, p);
      this.Save(dto, p);
    }

    private void Save(PlotDTO dto, Plot p)
    {
      this.Updater.Session.Save((object) p, (object) dto.Guid);
      if (dto.PlotGroundCovers != null)
      {
        foreach (PlotGroundCoverDTO plotGroundCover in dto.PlotGroundCovers)
        {
          if ((plotGroundCover.State & State.Deleted) == State.Existing)
            this.Updater.PlotGroundCoverProcessor.Create(plotGroundCover, p);
        }
      }
      if (dto.LandUses != null)
      {
        foreach (PlotLandUseDTO landUse in dto.LandUses)
        {
          if ((landUse.State & State.Deleted) == State.Existing)
            this.Updater.PlotLandUseProcessor.Create(landUse, p);
        }
      }
      if (dto.ReferenceObjects != null)
      {
        foreach (ReferenceObjectDTO referenceObject in dto.ReferenceObjects)
        {
          if ((referenceObject.State & State.Deleted) == State.Existing)
            this.Updater.ReferenceObjectProcessor.Create(referenceObject, p);
        }
      }
      if (dto.Shrubs != null)
      {
        foreach (ShrubDTO shrub in dto.Shrubs)
        {
          if ((shrub.State & State.Deleted) == State.Existing)
            this.Updater.ShrubProcessor.Create(shrub, p);
        }
      }
      if (dto.Trees == null)
        return;
      foreach (TreeDTO tree in dto.Trees)
      {
        if ((tree.State & State.Deleted) == State.Existing)
          this.Updater.TreeProcessor.Create(tree, p);
      }
    }

    public void Update(PlotDTO dto, Year y)
    {
      Plot plot1 = this.Updater.Session.Get<Plot>((object) dto.Guid);
      if (plot1 == null)
      {
        Plot plot2 = new Plot() { Year = y };
        this.UpdatePlot(dto, plot2);
        switch (this.Updater.ResolveConflict(Conflict.EntityDeleted, (Entity) plot2, (Entity) null, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Save(dto, plot2);
            break;
        }
      }
      else if (plot1.Revision != dto.Revision)
      {
        Plot plot3 = new Plot();
        plot3.Year = y;
        plot3.Revision = plot1.Revision;
        Plot plot4 = plot3;
        this.UpdatePlot(dto, plot4);
        switch (this.Updater.ResolveConflict(Conflict.RevisionMismatch, (Entity) plot4, (Entity) plot1, dto.Revision))
        {
          case Resolution.UseTheirs:
            this.Updater.Session.Delete((object) plot1);
            this.Updater.Session.Flush();
            this.Save(dto, plot4);
            break;
        }
      }
      else
      {
        if ((dto.State & State.Modified) != State.Existing)
        {
          this.UpdatePlot(dto, plot1);
          this.Updater.Session.Update((object) plot1);
        }
        if (dto.Trees != null)
        {
          foreach (TreeDTO tree in dto.Trees)
          {
            if ((tree.State & State.Deleted) != State.Existing)
              this.Updater.TreeProcessor.Delete(tree);
          }
          this.Updater.Session.Flush();
        }
        if (dto.Shrubs != null)
        {
          foreach (ShrubDTO shrub in dto.Shrubs)
          {
            if ((shrub.State & State.Deleted) != State.Existing)
              this.Updater.ShrubProcessor.Delete(shrub);
          }
          this.Updater.Session.Flush();
        }
        if (dto.ReferenceObjects != null)
        {
          foreach (ReferenceObjectDTO referenceObject in dto.ReferenceObjects)
          {
            if ((referenceObject.State & State.Deleted) != State.Existing)
              this.Updater.ReferenceObjectProcessor.Delete(referenceObject);
          }
          this.Updater.Session.Flush();
        }
        if (dto.LandUses != null)
        {
          foreach (PlotLandUseDTO landUse in dto.LandUses)
          {
            if ((landUse.State & State.Deleted) != State.Existing)
              this.Updater.PlotLandUseProcessor.Delete(landUse);
          }
          this.Updater.Session.Flush();
        }
        if (dto.PlotGroundCovers != null)
        {
          foreach (PlotGroundCoverDTO plotGroundCover in dto.PlotGroundCovers)
          {
            if ((plotGroundCover.State & State.Deleted) != State.Existing)
              this.Updater.PlotGroundCoverProcessor.Delete(plotGroundCover);
          }
          this.Updater.Session.Flush();
        }
        if (dto.PlotGroundCovers != null)
        {
          foreach (PlotGroundCoverDTO plotGroundCover in dto.PlotGroundCovers)
          {
            if (plotGroundCover.State == State.Modified)
              this.Updater.PlotGroundCoverProcessor.Update(plotGroundCover, plot1);
          }
          this.Updater.Session.Flush();
          foreach (PlotGroundCoverDTO plotGroundCover in dto.PlotGroundCovers)
          {
            if ((plotGroundCover.State & State.New) != State.Existing)
              this.Updater.PlotGroundCoverProcessor.Create(plotGroundCover, plot1);
          }
          this.Updater.Session.Flush();
        }
        if (dto.LandUses != null)
        {
          foreach (PlotLandUseDTO landUse in dto.LandUses)
          {
            if (landUse.State == State.Modified)
              this.Updater.PlotLandUseProcessor.Update(landUse, plot1);
          }
          this.Updater.Session.Flush();
          foreach (PlotLandUseDTO landUse in dto.LandUses)
          {
            if ((landUse.State & State.New) != State.Existing)
              this.Updater.PlotLandUseProcessor.Create(landUse, plot1);
          }
          this.Updater.Session.Flush();
        }
        if (dto.ReferenceObjects != null)
        {
          foreach (ReferenceObjectDTO referenceObject in dto.ReferenceObjects)
          {
            if (referenceObject.State == State.Modified)
              this.Updater.ReferenceObjectProcessor.Update(referenceObject, plot1);
          }
          this.Updater.Session.Flush();
          foreach (ReferenceObjectDTO referenceObject in dto.ReferenceObjects)
          {
            if ((referenceObject.State & State.New) != State.Existing)
              this.Updater.ReferenceObjectProcessor.Create(referenceObject, plot1);
          }
          this.Updater.Session.Flush();
        }
        if (dto.Shrubs != null)
        {
          foreach (ShrubDTO shrub in dto.Shrubs)
          {
            if (shrub.State == State.Modified)
              this.Updater.ShrubProcessor.Update(shrub, plot1);
          }
          this.Updater.Session.Flush();
          foreach (ShrubDTO shrub in dto.Shrubs)
          {
            if ((shrub.State & State.New) != State.Existing)
              this.Updater.ShrubProcessor.Create(shrub, plot1);
          }
          this.Updater.Session.Flush();
        }
        if (dto.Trees == null)
          return;
        foreach (TreeDTO tree in dto.Trees)
        {
          if (tree.State == State.Modified)
            this.Updater.TreeProcessor.Update(tree, plot1);
        }
        this.Updater.Session.Flush();
        foreach (TreeDTO tree in dto.Trees)
        {
          if ((tree.State & State.New) != State.Existing)
            this.Updater.TreeProcessor.Create(tree, plot1);
        }
        this.Updater.Session.Flush();
      }
    }

    public void Delete(PlotDTO dto) => this.DeleteEntity<Plot>(dto.Guid);

    private void UpdatePlot(PlotDTO dto, Plot p)
    {
      p.Id = dto.Id;
      p.Address = dto.Address;
      p.Comments = dto.Comments;
      p.ContactInfo = dto.ContactInfo;
      p.Crew = dto.Crew;
      p.Date = new DateTime?(dto.Date.GetValueOrDefault());
      p.Latitude = new double?(dto.Latitude.GetValueOrDefault());
      p.Longitude = new double?(dto.Longitude.GetValueOrDefault());
      p.PercentMeasured = dto.PercentMeasured.GetValueOrDefault(100);
      p.PercentPlantable = (PctMidRange) dto.PercentPlantable.GetValueOrDefault((short) -1);
      p.PercentShrubCover = (PctMidRange) dto.PercentShrubCover.GetValueOrDefault((short) -1);
      p.PercentTreeCover = (PctMidRange) dto.PercentTreeCover.GetValueOrDefault((short) -1);
      p.Photo = dto.Photo;
      p.Size = dto.Size.GetValueOrDefault(-1f);
      p.Stake = dto.Stake.GetValueOrDefault(false);
      p.IsComplete = dto.IsComplete.GetValueOrDefault(false);
      p.Strata = dto.Strata.HasValue ? this.Updater.Session.Get<Strata>((object) dto.Strata) : (Strata) null;
      p.Street = dto.Street.HasValue ? this.Updater.Session.Get<Street>((object) dto.Street) : (Street) null;
      p.Revision = Math.Max(p.Revision, dto.Revision) + 1;
    }
  }
}
