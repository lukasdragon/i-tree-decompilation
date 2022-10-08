// Decompiled with JetBrains decompiler
// Type: Eco.Util.Tasks.ReinventoryTask
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.NHibernate.Extensions;
using DaveyTree.Threading.Tasks.Schedulers;
using Eco.Domain.v6;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Eco.Util.Tasks
{
  public class ReinventoryTask
  {
    private string _projectFile;
    private short _year;
    private InputSession _is;

    public ReinventoryTask(InputSession inputSession, string projectFile, short year)
    {
      this._is = inputSession;
      this._projectFile = projectFile;
      this._year = year;
      this.Progress = (IProgress<ProgressEventArgs>) new System.Progress<ProgressEventArgs>();
    }

    public CancellationToken CancellationToken { get; set; }

    public IProgress<ProgressEventArgs> Progress { get; set; }

    public async Task<bool> Execute()
    {
      ReinventoryTask reinventoryTask = this;
      int num = await new CreateProjectTask(reinventoryTask._projectFile).DoWork(TaskCreationOptions.LongRunning) ? 1 : 0;
      reinventoryTask.ThrowIfCanceled();
      // ISSUE: reference to a compiler-generated method
      return num != 0 && await Task.Factory.StartNew<bool>(new Func<bool>(reinventoryTask.\u003CExecute\u003Eb__13_0), CancellationToken.None, TaskCreationOptions.None, StaTaskSchedulerEx.Instance());
    }

    private bool TransferData()
    {
      InputSession inputSession = new InputSession(this._projectFile);
      ReinventoryTask.Step[] stepArray = new ReinventoryTask.Step[25]
      {
        new ReinventoryTask.Step(this.CreateProject),
        new ReinventoryTask.Step(this.CreateProjectLocations),
        new ReinventoryTask.Step(this.CreateStreets),
        new ReinventoryTask.Step(this.CreateSeries),
        new ReinventoryTask.Step(this.CreateYear),
        new ReinventoryTask.Step(this.CreateElementPrices),
        new ReinventoryTask.Step(this.CreateYearlyCosts),
        new ReinventoryTask.Step(this.CreateYearLocationData),
        new ReinventoryTask.Step(this.CreateLandUses),
        new ReinventoryTask.Step(this.CreateGroundCovers),
        new ReinventoryTask.Step(this.CreateLookups),
        new ReinventoryTask.Step(this.CreateDBHs),
        new ReinventoryTask.Step(this.CreateDBHRptClasses),
        new ReinventoryTask.Step(this.CreateHealthRptClasses),
        new ReinventoryTask.Step(this.CreateConditions),
        new ReinventoryTask.Step(this.CreateStrata),
        new ReinventoryTask.Step(this.CreatePlots),
        new ReinventoryTask.Step(this.CreatePlotGroundCovers),
        new ReinventoryTask.Step(this.CreatePlotLandUses),
        new ReinventoryTask.Step(this.CreateReferenceObjects),
        new ReinventoryTask.Step(this.CreateShrubs),
        new ReinventoryTask.Step(this.CreateTrees),
        new ReinventoryTask.Step(this.CreateStems),
        new ReinventoryTask.Step(this.CreateBuildings),
        new ReinventoryTask.Step(this.CreatePlantingSites)
      };
      using (ISession session1 = this._is.CreateSession())
      {
        using (ISession session2 = inputSession.CreateSession())
        {
          ReinventoryTask.EntityMap entityMap = new ReinventoryTask.EntityMap(session2);
          session1.FlushMode = FlushMode.Manual;
          foreach (ReinventoryTask.Step step in stepArray)
          {
            this.ThrowIfCanceled();
            ISession os = session1;
            ISession ns = session2;
            ReinventoryTask.EntityMap em = entityMap;
            step(os, ns, em);
          }
        }
      }
      return true;
    }

    private void CreateProject(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Project entity = os.QueryOver<Project>().JoinQueryOver<Series>((Expression<Func<Project, IEnumerable<Series>>>) (p => p.Series)).JoinQueryOver<Year>((Expression<Func<Series, IEnumerable<Year>>>) (s => s.Years)).Where((Expression<Func<Year, bool>>) (y => (Guid?) y.Guid == this._is.YearKey)).SingleOrDefault<Project>();
      NHibernateUtil.Initialize((object) entity.Locations);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        this.ReportProgress("Creating project...", 1, 0);
        Project clone = entity.Clone(false);
        clone.Revision = entity.Revision;
        ns.Save((object) clone, (object) entity.Guid);
        em.Add<Project>(entity, clone);
        transaction.Commit();
        this.ReportProgress("Creating project...", 1, 1);
      }
    }

    private void CreateProjectLocations(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<ProjectLocation> projectLocationList = os.QueryOver<ProjectLocation>().JoinQueryOver<Project>((Expression<Func<ProjectLocation, Project>>) (pl => pl.Project)).JoinQueryOver<Series>((Expression<Func<Project, IEnumerable<Series>>>) (p => p.Series)).JoinQueryOver<Year>((Expression<Func<Series, IEnumerable<Year>>>) (s => s.Years)).Where((Expression<Func<Year, bool>>) (y => (Guid?) y.Guid == this._is.YearKey)).List();
        int num = 0;
        foreach (ProjectLocation entity in (IEnumerable<ProjectLocation>) projectLocationList)
        {
          if (num == 0)
            this.ReportProgress("Create project locations....", projectLocationList.Count, num++);
          ProjectLocation clone = entity.Clone(false);
          clone.Project = em.Load<Project>((Entity) entity.Project);
          clone.Revision = entity.Revision;
          ns.Save((object) clone, (object) entity.Guid);
          em.Add<ProjectLocation>(entity, clone);
          this.ReportProgress("Create project locations....", projectLocationList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateStreets(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<Street> streetList = os.QueryOver<Street>().JoinQueryOver<ProjectLocation>((Expression<Func<Street, ProjectLocation>>) (s => s.ProjectLocation)).JoinQueryOver<Project>((Expression<Func<ProjectLocation, Project>>) (pl => pl.Project)).JoinQueryOver<Series>((Expression<Func<Project, IEnumerable<Series>>>) (p => p.Series)).JoinQueryOver<Year>((Expression<Func<Series, IEnumerable<Year>>>) (s => s.Years)).Where((Expression<Func<Year, bool>>) (y => (Guid?) y.Guid == this._is.YearKey)).List();
        int num = 0;
        foreach (Street entity in (IEnumerable<Street>) streetList)
        {
          if (num == 0)
            this.ReportProgress("Creating streets...", streetList.Count, num++);
          Street clone = entity.Clone(false);
          clone.ProjectLocation = em.Load<ProjectLocation>((Entity) entity.ProjectLocation);
          clone.Revision = entity.Revision;
          ns.Save((object) clone, (object) entity.Guid);
          em.Add<Street>(entity, clone);
          this.ReportProgress("Creating streets...", streetList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateSeries(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Series entity = os.QueryOver<Series>().JoinQueryOver<Year>((Expression<Func<Series, IEnumerable<Year>>>) (s => s.Years)).Where((Expression<Func<Year, bool>>) (y => (Guid?) y.Guid == this._is.YearKey)).SingleOrDefault<Series>();
      Project project = ns.Load<Project>((object) entity.Project.Guid);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        this.ReportProgress("Creating series...", 1, 0);
        Series clone = entity.Clone(false);
        clone.Project = project;
        clone.Revision = entity.Revision;
        ns.Save((object) clone, (object) entity.Guid);
        em.Add<Series>(entity, clone);
        transaction.Commit();
        this.ReportProgress("Creating series...", 1, 1);
      }
    }

    private void CreateYear(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year entity = os.QueryOver<Year>().Where((Expression<Func<Year, bool>>) (y => (Guid?) y.Guid == this._is.YearKey)).SingleOrDefault<Year>();
      Series series = ns.Load<Series>((object) entity.Series.Guid);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        this.ReportProgress("Creating year...", 1, 0);
        Year clone = entity.Clone(false);
        clone.Series = series;
        clone.Id = this._year;
        clone.IsInitialMeasurement = false;
        clone.MobileKey = (string) null;
        clone.RevProcessed = 0;
        clone.Changed = true;
        clone.Revision = entity.Revision;
        ns.Save((object) clone);
        em.Add<Year>(entity, clone);
        transaction.Commit();
        this.ReportProgress("Creating year...", 1, 1);
      }
    }

    private void CreateElementPrices(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      Year year1 = em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<ElementPrice> elementPriceList = os.QueryOver<ElementPrice>().Where((Expression<Func<ElementPrice, bool>>) (ep => ep.Year == year)).List();
        int num = 0;
        foreach (ElementPrice entity in (IEnumerable<ElementPrice>) elementPriceList)
        {
          if (num == 0)
            this.ReportProgress("Creating yearly prices...", elementPriceList.Count, num++);
          ElementPrice clone = entity.Clone(false);
          clone.Year = year1;
          ns.Save((object) clone);
          em.Add<ElementPrice>(entity, clone);
          this.ReportProgress("Creating yearly prices...", elementPriceList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateYearlyCosts(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      Year year1 = em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<YearlyCost> yearlyCostList = os.QueryOver<YearlyCost>().Where((Expression<Func<YearlyCost, bool>>) (yc => yc.Year == year)).List();
        int num = 0;
        foreach (YearlyCost entity in (IEnumerable<YearlyCost>) yearlyCostList)
        {
          if (num == 0)
            this.ReportProgress("Creating yearly costs...", yearlyCostList.Count, num++);
          YearlyCost clone = entity.Clone(false);
          clone.Year = year1;
          ns.Save((object) clone);
          em.Add<YearlyCost>(entity, clone);
          this.ReportProgress("Creating yearly costs...", yearlyCostList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateYearLocationData(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      Year year1 = em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<YearLocationData> yearLocationDataList = os.QueryOver<YearLocationData>().Where((Expression<Func<YearLocationData, bool>>) (yd => yd.Year == year)).List();
        int num = 0;
        foreach (YearLocationData entity in (IEnumerable<YearLocationData>) yearLocationDataList)
        {
          if (num == 0)
            this.ReportProgress("Creating yearly location data...", yearLocationDataList.Count, num++);
          YearLocationData clone = entity.Clone(false);
          clone.Year = year1;
          clone.ProjectLocation = ns.Load<ProjectLocation>((object) entity.ProjectLocation.Guid);
          ns.Save((object) clone);
          em.Add<YearLocationData>(entity, clone);
          this.ReportProgress("Creating yearly location data...", yearLocationDataList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateLandUses(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      Year year1 = em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<LandUse> landUseList = os.QueryOver<LandUse>().Where((Expression<Func<LandUse, bool>>) (lu => lu.Year == year)).List();
        int num = 0;
        foreach (LandUse entity in (IEnumerable<LandUse>) landUseList)
        {
          if (num == 0)
            this.ReportProgress("Creating landuses...", landUseList.Count, num++);
          LandUse clone = entity.Clone(false);
          clone.Year = year1;
          clone.Revision = entity.Revision;
          ns.Save((object) clone, (object) entity.Guid);
          em.Add<LandUse>(entity, clone);
          this.ReportProgress("Creating landuses...", landUseList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateGroundCovers(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      Year year1 = em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<GroundCover> groundCoverList = os.QueryOver<GroundCover>().Where((Expression<Func<GroundCover, bool>>) (gc => gc.Year == year)).List();
        int num = 0;
        foreach (GroundCover entity in (IEnumerable<GroundCover>) groundCoverList)
        {
          if (num == 0)
            this.ReportProgress("Creating ground covers...", groundCoverList.Count, num++);
          GroundCover clone = entity.Clone(false);
          clone.Year = year1;
          clone.Revision = entity.Revision;
          ns.Save((object) clone, (object) entity.Guid);
          em.Add<GroundCover>(entity, clone);
          this.ReportProgress("Creating ground covers...", groundCoverList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateLookups(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      Year year1 = em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<Lookup> lookupList = os.QueryOver<Lookup>().Where((Expression<Func<Lookup, bool>>) (lu => lu.Year == year)).List();
        int num = 0;
        foreach (Lookup entity in (IEnumerable<Lookup>) lookupList)
        {
          if (num == 0)
            this.ReportProgress("Creating project categories...", lookupList.Count, num++);
          Lookup clone = entity.Clone(false);
          clone.Year = year1;
          clone.Revision = entity.Revision;
          ns.Save((object) clone, (object) entity.Guid);
          em.Add<Lookup>(entity, clone);
          this.ReportProgress("Creating project categories...", lookupList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreatePlantingSiteTypes(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      Year year1 = em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<PlantingSiteType> plantingSiteTypeList = os.QueryOver<PlantingSiteType>().Where((Expression<Func<PlantingSiteType, bool>>) (pst => pst.Year == year)).List();
        int num = 0;
        foreach (PlantingSiteType entity in (IEnumerable<PlantingSiteType>) plantingSiteTypeList)
        {
          if (num == 0)
            this.ReportProgress("Creating planting site types...", plantingSiteTypeList.Count, num++);
          PlantingSiteType clone = entity.Clone(false);
          clone.Year = year1;
          clone.Revision = entity.Revision;
          ns.Save((object) clone, (object) entity.Guid);
          em.Add<PlantingSiteType>(entity, clone);
          this.ReportProgress("Creating planting site types...", plantingSiteTypeList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateDBHs(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      Year year1 = em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<DBH> dbhList = os.QueryOver<DBH>().Where((Expression<Func<DBH, bool>>) (d => d.Year == year)).List();
        int num = 0;
        foreach (DBH entity in (IEnumerable<DBH>) dbhList)
        {
          if (num == 0)
            this.ReportProgress("Creating DBH categories...", dbhList.Count, num++);
          DBH clone = entity.Clone(false);
          clone.Year = year1;
          clone.Revision = entity.Revision;
          ns.Save((object) clone, (object) entity.Guid);
          em.Add<DBH>(entity, clone);
          this.ReportProgress("Creating DBH categories...", dbhList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateDBHRptClasses(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      Year year1 = em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<DBHRptClass> dbhRptClassList = os.QueryOver<DBHRptClass>().Where((Expression<Func<DBHRptClass, bool>>) (rc => rc.Year == year)).List();
        int num = 0;
        foreach (DBHRptClass entity in (IEnumerable<DBHRptClass>) dbhRptClassList)
        {
          if (num == 0)
            this.ReportProgress("Creating DBH report classes...", dbhRptClassList.Count, num++);
          DBHRptClass clone = entity.Clone(false);
          clone.Year = year1;
          ns.Save((object) clone);
          em.Add<DBHRptClass>(entity, clone);
          this.ReportProgress("Creating DBH report classes...", dbhRptClassList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateHealthRptClasses(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      Year year1 = em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<HealthRptClass> healthRptClassList = os.QueryOver<HealthRptClass>().Where((Expression<Func<HealthRptClass, bool>>) (hc => hc.Year == year)).List();
        int num = 0;
        foreach (HealthRptClass entity in (IEnumerable<HealthRptClass>) healthRptClassList)
        {
          if (num == 0)
            this.ReportProgress("Creating crown health report classes...", healthRptClassList.Count, num++);
          HealthRptClass clone = entity.Clone(false);
          clone.Year = year1;
          ns.Save((object) clone);
          em.Add<HealthRptClass>(entity, clone);
          this.ReportProgress("Creating crown health report classes...", healthRptClassList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateConditions(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      Year year1 = em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<Condition> conditionList = os.QueryOver<Condition>().Where((Expression<Func<Condition, bool>>) (c => c.Year == year)).List();
        int num = 0;
        foreach (Condition entity in (IEnumerable<Condition>) conditionList)
        {
          if (num == 0)
            this.ReportProgress("Creating crown health categories...", conditionList.Count, num++);
          Condition clone = entity.Clone(false);
          clone.Year = year1;
          clone.Revision = entity.Revision;
          ns.Save((object) clone, (object) entity.Guid);
          em.Add<Condition>(entity, clone);
          this.ReportProgress("Creating crown health categories...", conditionList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateStrata(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      Year year1 = em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<Strata> strataList = os.QueryOver<Strata>().Where((Expression<Func<Strata, bool>>) (s => s.Year == year)).List();
        int num = 0;
        foreach (Strata entity in (IEnumerable<Strata>) strataList)
        {
          if (num == 0)
            this.ReportProgress("Creating strata...", strataList.Count, num++);
          Strata clone = entity.Clone(false);
          clone.Year = year1;
          clone.Revision = entity.Revision;
          ns.Save((object) clone, (object) entity.Guid);
          em.Add<Strata>(entity, clone);
          this.ReportProgress("Creating strata...", strataList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreatePlots(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      Year year1 = em.Load<Year>((Entity) year);
      Series series = year.Series;
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<Plot> plotList = (IList<Plot>) os.QueryOver<Plot>().Where((Expression<Func<Plot, bool>>) (p => p.Year == year)).PagedList<Plot>();
        int num = 0;
        foreach (Plot entity in (IEnumerable<Plot>) plotList)
        {
          if (num == 0)
            this.ReportProgress("Creating plots...", plotList.Count, num++);
          Plot clone = entity.Clone(false);
          clone.Year = year1;
          clone.Strata = ns.Load<Strata>((object) entity.Strata.Guid);
          clone.Revision = entity.Revision;
          clone.IsComplete = !series.IsSample;
          clone.PriorYear = new Guid?(year.Guid);
          ns.Save((object) clone, (object) entity.Guid);
          em.Add<Plot>(entity, clone);
          if (num % 100 == 0)
            ns.Flush();
          this.ReportProgress("Creating plots...", plotList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreatePlotGroundCovers(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<PlotGroundCover> plotGroundCoverList = (IList<PlotGroundCover>) os.QueryOver<PlotGroundCover>().JoinQueryOver<Plot>((Expression<Func<PlotGroundCover, Plot>>) (gc => gc.Plot)).Where((Expression<Func<Plot, bool>>) (p => p.Year == year)).PagedList<PlotGroundCover>();
        int num = 0;
        foreach (PlotGroundCover entity in (IEnumerable<PlotGroundCover>) plotGroundCoverList)
        {
          if (num == 0)
            this.ReportProgress("Creating plot ground covers...", plotGroundCoverList.Count, num++);
          PlotGroundCover clone = entity.Clone(false);
          clone.Plot = ns.Load<Plot>((object) entity.Plot.Guid);
          clone.GroundCover = ns.Load<GroundCover>((object) entity.GroundCover.Guid);
          ns.Save((object) clone);
          em.Add<PlotGroundCover>(entity, clone);
          if (num % 100 == 0)
            ns.Flush();
          this.ReportProgress("Creating plot ground covers...", plotGroundCoverList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreatePlotLandUses(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<PlotLandUse> plotLandUseList = (IList<PlotLandUse>) os.QueryOver<PlotLandUse>().JoinQueryOver<Plot>((Expression<Func<PlotLandUse, Plot>>) (lu => lu.Plot)).Where((Expression<Func<Plot, bool>>) (p => p.Year == year)).PagedList<PlotLandUse>();
        int num = 0;
        foreach (PlotLandUse entity in (IEnumerable<PlotLandUse>) plotLandUseList)
        {
          if (num == 0)
            this.ReportProgress("Creating plot landuses...", plotLandUseList.Count, num++);
          PlotLandUse clone = entity.Clone(false);
          clone.Plot = ns.Load<Plot>((object) entity.Plot.Guid);
          clone.LandUse = ns.Load<LandUse>((object) entity.LandUse.Guid);
          ns.Save((object) clone, (object) entity.Guid);
          em.Add<PlotLandUse>(entity, clone);
          if (num % 100 == 0)
            ns.Flush();
          this.ReportProgress("Creating plot landuses...", plotLandUseList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateReferenceObjects(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<ReferenceObject> referenceObjectList = (IList<ReferenceObject>) os.QueryOver<ReferenceObject>().JoinQueryOver<Plot>((Expression<Func<ReferenceObject, Plot>>) (ro => ro.Plot)).Where((Expression<Func<Plot, bool>>) (p => p.Year == year)).PagedList<ReferenceObject>();
        int num = 0;
        foreach (ReferenceObject entity in (IEnumerable<ReferenceObject>) referenceObjectList)
        {
          if (num == 0)
            this.ReportProgress("Creating reference objects...", referenceObjectList.Count, num++);
          ReferenceObject clone = entity.Clone(false);
          clone.Plot = ns.Load<Plot>((object) entity.Plot.Guid);
          ns.Save((object) clone);
          em.Add<ReferenceObject>(entity, clone);
          if (num % 100 == 0)
            ns.Flush();
          this.ReportProgress("Creating reference objects...", referenceObjectList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateShrubs(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      em.Load<Year>((Entity) year);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<Shrub> shrubList = (IList<Shrub>) os.QueryOver<Shrub>().JoinQueryOver<Plot>((Expression<Func<Shrub, Plot>>) (sh => sh.Plot)).Where((Expression<Func<Plot, bool>>) (p => p.Year == year)).PagedList<Shrub>();
        int num = 0;
        foreach (Shrub entity in (IEnumerable<Shrub>) shrubList)
        {
          if (num == 0)
            this.ReportProgress("Creating shrubs...", shrubList.Count, num++);
          Shrub clone = entity.Clone(false);
          clone.Plot = ns.Load<Plot>((object) entity.Plot.Guid);
          ns.Save((object) clone);
          em.Add<Shrub>(entity, clone);
          if (num % 100 == 0)
            ns.Flush();
          this.ReportProgress("Creating shrubs...", shrubList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateTrees(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<Tree> treeList = (IList<Tree>) os.QueryOver<Tree>().WhereRestrictionOn((Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[5]
        {
          (object) TreeStatus.Ingrowth,
          (object) TreeStatus.InitialSample,
          (object) TreeStatus.NoChange,
          (object) TreeStatus.Planted,
          (object) TreeStatus.Unknown
        }).JoinQueryOver<Plot>((Expression<Func<Tree, Plot>>) (t => t.Plot)).Where((Expression<Func<Plot, bool>>) (p => p.Year == year)).PagedList<Tree>();
        int num = 0;
        foreach (Tree entity in (IEnumerable<Tree>) treeList)
        {
          if (num == 0)
            this.ReportProgress("Creating trees...", treeList.Count, num++);
          Tree clone = entity.Clone(false);
          clone.Plot = em.Load<Plot>((Entity) entity.Plot);
          clone.PlotLandUse = em.Load<PlotLandUse>((Entity) entity.PlotLandUse);
          clone.Street = em.Load<Street>((Entity) entity.Street);
          clone.SiteType = em.Load<SiteType>((Entity) entity.SiteType);
          clone.LocSite = em.Load<LocSite>((Entity) entity.LocSite);
          clone.MaintRec = em.Load<MaintRec>((Entity) entity.MaintRec);
          clone.MaintTask = em.Load<MaintTask>((Entity) entity.MaintTask);
          clone.SidewalkDamage = em.Load<Sidewalk>((Entity) entity.SidewalkDamage);
          clone.WireConflict = em.Load<WireConflict>((Entity) entity.WireConflict);
          clone.OtherOne = em.Load<OtherOne>((Entity) entity.OtherOne);
          clone.OtherTwo = em.Load<OtherTwo>((Entity) entity.OtherTwo);
          clone.OtherThree = em.Load<OtherThree>((Entity) entity.OtherThree);
          clone.PriorYear = new Guid?(year.Guid);
          ns.Save((object) clone);
          em.Add<Tree>(entity, clone);
          if (num % 100 == 0)
            ns.Flush();
          this.ReportProgress("Creating trees...", treeList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateStems(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<Stem> stemList = (IList<Stem>) os.QueryOver<Stem>().JoinQueryOver<Tree>((Expression<Func<Stem, Tree>>) (st => st.Tree)).JoinQueryOver<Plot>((Expression<Func<Tree, Plot>>) (t => t.Plot)).Where((Expression<Func<Plot, bool>>) (p => p.Year == year)).PagedList<Stem>();
        int num = 0;
        foreach (Stem entity in (IEnumerable<Stem>) stemList)
        {
          if (num == 0)
            this.ReportProgress("Creating stems...", stemList.Count, num++);
          Stem clone = entity.Clone(false);
          clone.Tree = em.Load<Tree>((Entity) entity.Tree);
          ns.Save((object) clone);
          em.Add<Stem>(entity, clone);
          if (num % 100 == 0)
            ns.Flush();
          this.ReportProgress("Creating stems...", stemList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreateBuildings(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<Building> buildingList = (IList<Building>) os.QueryOver<Building>().JoinQueryOver<Tree>((Expression<Func<Building, Tree>>) (b => b.Tree)).JoinQueryOver<Plot>((Expression<Func<Tree, Plot>>) (t => t.Plot)).Where((Expression<Func<Plot, bool>>) (p => p.Year == year)).PagedList<Building>();
        int num = 0;
        foreach (Building entity in (IEnumerable<Building>) buildingList)
        {
          if (num == 0)
            this.ReportProgress("Creating buildings...", buildingList.Count, num++);
          Building clone = entity.Clone(false);
          clone.Tree = em.Load<Tree>((Entity) entity.Tree);
          ns.Save((object) clone);
          em.Add<Building>(entity, clone);
          if (num % 100 == 0)
            ns.Flush();
          this.ReportProgress("Creating buildings...", buildingList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void CreatePlantingSites(ISession os, ISession ns, ReinventoryTask.EntityMap em)
    {
      Year year = os.Load<Year>((object) this._is.YearKey);
      using (ITransaction transaction = ns.BeginTransaction())
      {
        IList<PlantingSite> plantingSiteList = (IList<PlantingSite>) os.QueryOver<PlantingSite>().JoinQueryOver<Plot>((Expression<Func<PlantingSite, Plot>>) (ps => ps.Plot)).Where((Expression<Func<Plot, bool>>) (p => p.Year == year)).PagedList<PlantingSite>();
        int num = 0;
        foreach (PlantingSite entity in (IEnumerable<PlantingSite>) plantingSiteList)
        {
          if (num == 0)
            this.ReportProgress("Creating planting sites...", plantingSiteList.Count, num++);
          PlantingSite clone = entity.Clone(false);
          clone.Plot = em.Load<Plot>((Entity) entity.Plot);
          clone.PlantingSiteType = em.Load<PlantingSiteType>((Entity) entity.PlantingSiteType);
          clone.PlotLandUse = em.Load<PlotLandUse>((Entity) entity.PlotLandUse);
          clone.Street = em.Load<Street>((Entity) entity.Street);
          ns.Save((object) clone);
          em.Add<PlantingSite>(entity, clone);
          if (num % 100 == 0)
            ns.Flush();
          this.ReportProgress("Creating planting sites...", plantingSiteList.Count, num++);
        }
        transaction.Commit();
      }
    }

    private void ThrowIfCanceled()
    {
      CancellationToken cancellationToken = this.CancellationToken;
      this.CancellationToken.ThrowIfCancellationRequested();
    }

    private void ReportProgress(string status, int total, int progress)
    {
      if (this.Progress == null)
        return;
      int num = progress * 100 / total;
      if (progress < total * num / 100)
        return;
      this.Progress.Report(new ProgressEventArgs(status, total, progress));
    }

    private delegate void Step(ISession os, ISession ns, ReinventoryTask.EntityMap em);

    private class EntityMap
    {
      private ISession _s;
      private IDictionary<Guid, Guid> _map;

      public EntityMap(ISession s)
      {
        this._map = (IDictionary<Guid, Guid>) new Dictionary<Guid, Guid>();
        this._s = s;
      }

      public void Add<T>(T entity, T clone) where T : Entity => this._map[entity.Guid] = clone.Guid;

      public T Load<T>(Entity entity) where T : Entity => entity != null && this._map.ContainsKey(entity.Guid) ? this._s.Load<T>((object) this._map[entity.Guid]) : default (T);
    }
  }
}
