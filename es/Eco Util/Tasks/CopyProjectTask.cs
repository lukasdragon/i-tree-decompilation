// Decompiled with JetBrains decompiler
// Type: Eco.Util.Tasks.CopyProjectTask
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.NHibernate.Extensions;
using Eco.Domain.v6;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Eco.Util.Tasks
{
  public class CopyProjectTask : ATask
  {
    private readonly InputSession _src;
    private readonly InputSession _dst;
    private readonly IProgress<ProgressEventArgs> _progress;
    private readonly CancellationToken? _ct;
    private ISession _isSrc;
    private ISession _isDst;

    public CopyProjectTask(
      InputSession src,
      InputSession dst,
      CancellationToken? ct = null,
      IProgress<ProgressEventArgs> progress = null)
    {
      this._src = src;
      this._dst = dst;
      this._ct = ct;
      this._progress = progress;
    }

    protected override void Work()
    {
      using (this._isSrc = this._src.CreateSession())
      {
        using (this._isDst = this._dst.CreateSession())
        {
          this.Copy<Project>(Eco.Util.Resources.Strings.ConvertProjects);
          this.Copy<ProjectLocation>(Eco.Util.Resources.Strings.ConvertProjectLocations, (System.Action<ProjectLocation, ProjectLocation>) ((source, dest) => dest.Project = this._isDst.Load<Project>((object) source.Project.Guid)));
          this.Copy<Street>(Eco.Util.Resources.Strings.ConvertStreets, (System.Action<Street, Street>) ((source, dest) => dest.ProjectLocation = this._isDst.Load<ProjectLocation>((object) source.ProjectLocation.Guid)));
          this.Copy<Series>(Eco.Util.Resources.Strings.ConvertSeries, (System.Action<Series, Series>) ((source, dest) => dest.Project = this._isDst.Load<Project>((object) source.Project.Guid)));
          this.Copy<Year>(Eco.Util.Resources.Strings.ConvertYears, (System.Action<Year, Year>) ((source, dest) =>
          {
            dest.Series = this._isDst.Load<Series>((object) source.Series.Guid);
            dest.Changed = true;
          }));
          this.Copy<Condition>(Eco.Util.Resources.Strings.ConvertConditions, (System.Action<Condition, Condition>) ((source, dest) => dest.Year = this._isDst.Load<Year>((object) source.Year.Guid)));
          this.Copy<ElementPrice>(Eco.Util.Resources.Strings.ConvertBenefitPrices, (System.Action<ElementPrice, ElementPrice>) ((source, dest) => dest.Year = this._isDst.Load<Year>((object) source.Year.Guid)));
          this.Copy<Strata>(Eco.Util.Resources.Strings.ConvertStrata, (System.Action<Strata, Strata>) ((source, dest) => dest.Year = this._isDst.Load<Year>((object) source.Year.Guid)));
          this.Copy<LandUse>(Eco.Util.Resources.Strings.ConvertLandUses, (System.Action<LandUse, LandUse>) ((source, dest) => dest.Year = this._isDst.Load<Year>((object) source.Year.Guid)));
          this.Copy<DBH>(Eco.Util.Resources.Strings.ConvertDBHs, (System.Action<DBH, DBH>) ((source, dest) => dest.Year = this._isDst.Load<Year>((object) source.Year.Guid)));
          this.Copy<DBHRptClass>(Eco.Util.Resources.Strings.ConvertDBHRptClasses, (System.Action<DBHRptClass, DBHRptClass>) ((source, dest) => dest.Year = this._isDst.Load<Year>((object) source.Year.Guid)));
          this.Copy<GroundCover>(Eco.Util.Resources.Strings.ConvertGroundCovers, (System.Action<GroundCover, GroundCover>) ((source, dest) => dest.Year = this._isDst.Load<Year>((object) source.Year.Guid)));
          this.Copy<Lookup>(Eco.Util.Resources.Strings.ConvertProjectSettings, (System.Action<Lookup, Lookup>) ((source, dest) => dest.Year = this._isDst.Load<Year>((object) source.Year.Guid)));
          this.Copy<PlantingSiteType>(Eco.Util.Resources.Strings.ConvertPlantingSiteTypes, (System.Action<PlantingSiteType, PlantingSiteType>) ((source, dest) => dest.Year = this._isDst.Load<Year>((object) source.Year.Guid)));
          this.Copy<HealthRptClass>(Eco.Util.Resources.Strings.ConvertHealthClasses, (System.Action<HealthRptClass, HealthRptClass>) ((source, dest) => dest.Year = this._isDst.Load<Year>((object) source.Year.Guid)));
          this.Copy<MobileLogEntry>(Eco.Util.Resources.Strings.ConvertMobileLogEntries, (System.Action<MobileLogEntry, MobileLogEntry>) ((source, dest) => dest.Year = this._isDst.Load<Year>((object) source.Year.Guid)));
          this.Copy<YearLocationData>(Eco.Util.Resources.Strings.ConvertYearLocationData, (System.Action<YearLocationData, YearLocationData>) ((source, dest) =>
          {
            dest.Year = this._isDst.Load<Year>((object) source.Year.Guid);
            dest.ProjectLocation = this._isDst.Load<ProjectLocation>((object) source.ProjectLocation.Guid);
          }));
          this.Copy<YearlyCost>(Eco.Util.Resources.Strings.ConvertYearlyCosts, (System.Action<YearlyCost, YearlyCost>) ((source, dest) => dest.Year = this._isDst.Load<Year>((object) source.Year.Guid)));
          this.Copy<Plot>(Eco.Util.Resources.Strings.ConvertPlots, (System.Action<Plot, Plot>) ((source, dest) =>
          {
            dest.Year = this._isDst.Load<Year>((object) source.Year.Guid);
            if (source.Strata != null)
              dest.Strata = this._isDst.Load<Strata>((object) source.Strata.Guid);
            if (source.ProjectLocation != null)
              dest.ProjectLocation = this._isDst.Load<ProjectLocation>((object) source.ProjectLocation.Guid);
            if (source.Street == null)
              return;
            dest.Street = this._isDst.Load<Street>((object) source.Street.Guid);
          }));
          this.Copy<PlotLandUse>(Eco.Util.Resources.Strings.ConvertPlotLandUses, (System.Action<PlotLandUse, PlotLandUse>) ((source, dest) =>
          {
            dest.Plot = this._isDst.Load<Plot>((object) source.Plot.Guid);
            dest.LandUse = this._isDst.Load<LandUse>((object) source.LandUse.Guid);
          }));
          this.Copy<PlotGroundCover>(Eco.Util.Resources.Strings.ConvertPlotGroundCovers, (System.Action<PlotGroundCover, PlotGroundCover>) ((source, dest) =>
          {
            dest.Plot = this._isDst.Load<Plot>((object) source.Plot.Guid);
            dest.GroundCover = this._isDst.Load<GroundCover>((object) source.GroundCover.Guid);
          }));
          this.Copy<ReferenceObject>(Eco.Util.Resources.Strings.ConvertReferenceObjects, (System.Action<ReferenceObject, ReferenceObject>) ((source, dest) => dest.Plot = this._isDst.Load<Plot>((object) source.Plot.Guid)));
          this.Copy<PlantingSite>(Eco.Util.Resources.Strings.ConvertPlantingSites, (System.Action<PlantingSite, PlantingSite>) ((source, dest) =>
          {
            dest.Plot = this._isDst.Load<Plot>((object) source.Plot.Guid);
            if (source.PlotLandUse != null)
              dest.PlotLandUse = this._isDst.Load<PlotLandUse>((object) source.PlotLandUse.Guid);
            if (source.PlantingSiteType != null)
              dest.PlantingSiteType = this._isDst.Load<PlantingSiteType>((object) source.PlantingSiteType.Guid);
            if (source.Street == null)
              return;
            dest.Street = this._isDst.Load<Street>((object) source.Street.Guid);
          }));
          this.Copy<Tree>(Eco.Util.Resources.Strings.ConvertTrees, (System.Action<Tree, Tree>) ((source, dest) =>
          {
            dest.Plot = this._isDst.Load<Plot>((object) source.Plot.Guid);
            if (source.PlotLandUse != null)
              dest.PlotLandUse = this._isDst.Load<PlotLandUse>((object) source.PlotLandUse.Guid);
            if (source.Street != null)
              dest.Street = this._isDst.Load<Street>((object) source.Street.Guid);
            if (source.SiteType != null)
              dest.SiteType = this._isDst.Load<SiteType>((object) source.SiteType.Guid);
            if (source.LocSite != null)
              dest.LocSite = this._isDst.Load<LocSite>((object) source.LocSite.Guid);
            if (source.MaintRec != null)
              dest.MaintRec = this._isDst.Load<MaintRec>((object) source.MaintRec.Guid);
            if (source.MaintTask != null)
              dest.MaintTask = this._isDst.Load<MaintTask>((object) source.MaintTask.Guid);
            if (source.SidewalkDamage != null)
              dest.SidewalkDamage = this._isDst.Load<Sidewalk>((object) source.SidewalkDamage.Guid);
            if (source.WireConflict != null)
              dest.WireConflict = this._isDst.Load<WireConflict>((object) source.WireConflict.Guid);
            if (source.OtherOne != null)
              dest.OtherOne = this._isDst.Load<OtherOne>((object) source.OtherOne.Guid);
            if (source.OtherTwo != null)
              dest.OtherTwo = this._isDst.Load<OtherTwo>((object) source.OtherTwo.Guid);
            if (source.OtherThree == null)
              return;
            dest.OtherThree = this._isDst.Load<OtherThree>((object) source.OtherThree.Guid);
          }));
          this.Copy<Stem>(Eco.Util.Resources.Strings.ConvertStems, (System.Action<Stem, Stem>) ((source, dest) => dest.Tree = this._isDst.Load<Tree>((object) source.Tree.Guid)));
          this.Copy<Building>(Eco.Util.Resources.Strings.ConvertBuildings, (System.Action<Building, Building>) ((source, dest) => dest.Tree = this._isDst.Load<Tree>((object) source.Tree.Guid)));
          this.Copy<Shrub>(Eco.Util.Resources.Strings.ConvertShrubs, (System.Action<Shrub, Shrub>) ((source, dest) => dest.Plot = this._isDst.Load<Plot>((object) source.Plot.Guid)));
          this.Copy<Forecast>(Eco.Util.Resources.Strings.ConvertForecasts, (System.Action<Forecast, Forecast>) ((source, dest) => dest.Year = this._isDst.Load<Year>((object) source.Year.Guid)));
          this.Copy<Replanting>(Eco.Util.Resources.Strings.ConvertReplantings, (System.Action<Replanting, Replanting>) ((source, dest) => dest.Forecast = this._isDst.Load<Forecast>((object) source.Forecast.Guid)));
          this.Copy<ForecastEvent>(Eco.Util.Resources.Strings.ConvertForecastEvents, (System.Action<ForecastEvent, ForecastEvent>) ((source, dest) => dest.Forecast = this._isDst.Load<Forecast>((object) source.Forecast.Guid)));
          this.Copy<Mortality>(Eco.Util.Resources.Strings.ConvertMortalities, (System.Action<Mortality, Mortality>) ((source, dest) => dest.Forecast = this._isDst.Load<Forecast>((object) source.Forecast.Guid)));
        }
      }
    }

    private void Report(string status, int total, int progress)
    {
      if (this._progress == null)
        return;
      this._progress.Report(new ProgressEventArgs(status, total, progress));
    }

    private void ThrowIfCanceled()
    {
      if (!this._ct.HasValue)
        return;
      this._ct.Value.ThrowIfCancellationRequested();
    }

    private void Copy<T>(string msg, System.Action<T, T> convert = null) where T : Entity<T>
    {
      using (ITransaction transaction1 = this._isSrc.BeginTransaction())
      {
        IList<T> objList = (IList<T>) IQueryOverEx.PagedList<T>(this._isSrc.QueryOver<T>(), 1000);
        int num = 0;
        using (ITransaction transaction2 = this._isDst.BeginTransaction())
        {
          foreach (T obj1 in (IEnumerable<T>) objList)
          {
            this.ThrowIfCanceled();
            this.Report(msg, objList.Count, num++);
            T obj2 = obj1.Clone(false);
            obj2.Revision = obj1.Revision;
            if (convert != null)
              convert(obj1, obj2);
            this._isDst.Save((object) obj2, (object) obj1.Guid);
            if (num % 100 == 0)
              this._isDst.Flush();
          }
          transaction2.Commit();
        }
        transaction1.Commit();
      }
    }
  }
}
