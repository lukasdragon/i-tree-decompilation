// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.Plot
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;
using NHibernate;
using System;
using System.Collections.Generic;

namespace Eco.Domain.v5
{
  public class Plot
  {
    private int? m_hash;
    private PlotId m_compositeId;
    private Project m_project;
    private ISet<SubPlot> m_subPlots;

    public Plot() => this.Init();

    public Plot(PlotId id)
    {
      this.m_compositeId = id;
      this.Init();
    }

    public virtual PlotId CompositeId => this.m_compositeId;

    public virtual Project Project => this.m_project;

    public virtual ISet<SubPlot> SubPlots => this.m_subPlots;

    public virtual int Id => this.IsTransient ? 0 : this.m_compositeId.Plot;

    public virtual string Address { get; set; }

    public virtual double? Latitude { get; set; }

    public virtual double? Longitude { get; set; }

    public virtual int MapLandUse { get; set; }

    public virtual DateTime? Date { get; set; }

    public virtual string Crew { get; set; }

    public virtual string Comments { get; set; }

    public virtual string ContactInfo { get; set; }

    public virtual long Revision { get; set; }

    private void Init() => this.m_subPlots = (ISet<SubPlot>) new HashSet<SubPlot>();

    public virtual bool IsTransient => this.m_compositeId == null;

    public virtual PlotDTO GetDTO()
    {
      PlotDTO dto = new PlotDTO()
      {
        Id = this.CompositeId.Plot,
        Address = this.Address,
        Comments = this.Comments,
        ContactInfo = this.ContactInfo,
        Crew = this.Crew,
        Date = this.Date,
        Latitude = this.Latitude,
        Longitude = this.Longitude,
        MapLandUse = this.MapLandUse,
        Revision = this.Revision,
        SubPlots = new List<SubPlotDTO>()
      };
      foreach (SubPlot subPlot in (IEnumerable<SubPlot>) this.SubPlots)
        dto.SubPlots.Add(subPlot.GetDTO());
      return dto;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Plot plot) || this.IsTransient ^ plot.IsTransient)
        return false;
      return this.IsTransient && plot.IsTransient ? this == plot : this.CompositeId.Equals((object) plot.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }

    public static void Delete(ProjectId prj, PlotDTO plot, ISession s)
    {
      PlotId id = new PlotId(prj.Location, prj.Series, prj.Year, plot.Id);
      Plot plot1 = s.Get<Plot>((object) id);
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Delete((object) plot1);
        transaction.Commit();
      }
    }

    public static void Create(ProjectId prj, PlotDTO plot, ISession s)
    {
      PlotId plotId = new PlotId(prj.Location, prj.Series, prj.Year, plot.Id);
      Plot plot1 = new Plot(plotId);
      plot1.Address = plot.Address;
      plot1.Comments = plot.Comments;
      plot1.ContactInfo = plot.ContactInfo;
      plot1.Crew = plot.Crew;
      plot1.Date = plot.Date;
      plot1.Latitude = plot.Latitude;
      plot1.Longitude = plot.Longitude;
      plot1.MapLandUse = plot.MapLandUse;
      plot1.Revision = plot.Revision;
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Save((object) plot1);
        transaction.Commit();
      }
      if (plot.SubPlots == null)
        return;
      foreach (SubPlotDTO subPlot in plot.SubPlots)
        SubPlot.Create(plotId, subPlot, s);
    }

    public static void Update(ProjectId prj, PlotDTO plot, ISession s)
    {
      PlotId plotId = new PlotId(prj.Location, prj.Series, prj.Year, plot.Id);
      Plot plot1 = s.Get<Plot>((object) plotId);
      if (plot1.Revision != plot.Revision)
        throw new RevisionMismatchException()
        {
          Revision1 = plot1.Revision,
          Revision2 = plot.Revision
        };
      using (ITransaction transaction = s.BeginTransaction())
      {
        plot1.Address = plot.Address;
        plot1.Comments = plot.Comments;
        plot1.ContactInfo = plot.ContactInfo;
        plot1.Crew = plot.Crew;
        plot1.Date = plot.Date;
        plot1.Latitude = plot.Latitude;
        plot1.Longitude = plot.Longitude;
        plot1.MapLandUse = plot.MapLandUse;
        transaction.Commit();
      }
      if (plot.SubPlots == null)
        return;
      if (plot1.SubPlots.Count > 0)
      {
        foreach (SubPlotDTO subPlot in plot.SubPlots)
          SubPlot.Update(plotId, subPlot, s);
      }
      else
      {
        foreach (SubPlotDTO subPlot in plot.SubPlots)
          SubPlot.Create(plotId, subPlot, s);
      }
    }
  }
}
