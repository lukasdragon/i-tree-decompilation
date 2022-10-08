// Decompiled with JetBrains decompiler
// Type: Eco.Util.Tasks.ConvertV5Task
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.v5;
using Eco.Domain.v6;
using Eco.Util.Convert;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Eco.Util.Tasks
{
  public class ConvertV5Task : ATask
  {
    private string m_v5db;
    private string m_v6db;
    private ISessionFactory m_sfLS;
    private ISession m_v5s;
    private ISession m_v6s;
    private ConversionMap m_map;
    private CancellationToken m_token;
    private Dictionary<Year, YearData> m_yd;
    private Dictionary<Eco.Domain.v6.Plot, PlotData> m_pd;
    private IProgress<ProgressEventArgs> m_progress;

    public ConvertV5Task(string v5db, string v6db, ISessionFactory sfLS)
    {
      this.m_v5db = v5db;
      this.m_v6db = v6db;
      this.m_sfLS = sfLS;
    }

    public ConvertV5Task(
      string v5db,
      string v6db,
      ISessionFactory sfLS,
      CancellationToken token,
      IProgress<ProgressEventArgs> progress)
      : this(v5db, v6db, sfLS)
    {
      this.m_token = token;
      this.m_progress = progress;
    }

    protected override void Work()
    {
      this.m_yd = new Dictionary<Year, YearData>();
      this.m_pd = new Dictionary<Eco.Domain.v6.Plot, PlotData>();
      this.m_map = new ConversionMap();
      using (ISessionFactory sessionFactory = this.BuildFactory(this.m_v5db, typeof (Eco.Domain.v5.Location)))
      {
        using (InputSession inputSession = new InputSession(this.m_v6db))
        {
          using (this.m_v5s = sessionFactory.OpenSession())
          {
            using (this.m_v6s = inputSession.CreateSession())
            {
              this.ConvertLocations();
              this.ConvertSeries();
              this.ConvertProjects();
              this.ConvertCoverTypes();
              this.ConvertMapLandUses();
              this.ConvertPlots();
              this.ConvertSubPlotCovers();
              this.ConvertPlotFieldLandUses();
              this.ConvertTrees();
              this.ConvertStems();
              this.ConvertBuildingInteractions();
              this.ConvertReferenceObjects();
              this.ConvertShrubs();
            }
          }
        }
      }
    }

    private void Report(string status, int total, int progress)
    {
      if (this.m_progress == null)
        return;
      this.m_progress.Report(new ProgressEventArgs(status, total, progress));
    }

    private void ThrowIfCanceled()
    {
      CancellationToken token = this.m_token;
      this.m_token.ThrowIfCancellationRequested();
    }

    private void ConvertLocations()
    {
      using (this.m_v5s.BeginTransaction())
      {
        IList<Eco.Domain.v5.Location> locationList = this.m_v5s.CreateCriteria<Eco.Domain.v5.Location>().List<Eco.Domain.v5.Location>();
        int num = 0;
        using (ITransaction transaction = this.m_v6s.BeginTransaction())
        {
          foreach (Eco.Domain.v5.Location location1 in (IEnumerable<Eco.Domain.v5.Location>) locationList)
          {
            Eco.Domain.v5.Location v5Loc = location1;
            this.ThrowIfCanceled();
            this.Report(Eco.Util.Resources.Strings.ConvertProjects, locationList.Count, num++);
            int id = 219;
            string str1 = "001";
            string str2 = "00";
            string str3 = "000";
            string str4 = "00000";
            LocationSpecies.Domain.Location location2;
            using (IStatelessSession statelessSession = this.m_sfLS.OpenStatelessSession())
            {
              LocationRelation lr = (LocationRelation) null;
              LocationSpecies.Domain.Location nation = statelessSession.QueryOver<LocationSpecies.Domain.Location>().JoinAlias((System.Linq.Expressions.Expression<Func<LocationSpecies.Domain.Location, object>>) (l => l.Relations), (System.Linq.Expressions.Expression<Func<object>>) (() => lr)).Where((System.Linq.Expressions.Expression<Func<LocationSpecies.Domain.Location, bool>>) (l => lr.Code == v5Loc.NationCode && (int) lr.Level == 2)).Cacheable().SingleOrDefault();
              if (nation != null)
              {
                LocationSpecies.Domain.Location state = statelessSession.QueryOver<LocationSpecies.Domain.Location>().JoinAlias((System.Linq.Expressions.Expression<Func<LocationSpecies.Domain.Location, object>>) (l => l.Relations), (System.Linq.Expressions.Expression<Func<object>>) (() => lr)).Where((System.Linq.Expressions.Expression<Func<LocationSpecies.Domain.Location, bool>>) (l => lr.Code == v5Loc.StateCode && lr.Parent == nation)).Cacheable().SingleOrDefault();
                str1 = v5Loc.NationCode;
                if (state == null)
                {
                  id = nation.Id;
                }
                else
                {
                  LocationSpecies.Domain.Location county = statelessSession.QueryOver<LocationSpecies.Domain.Location>().JoinAlias((System.Linq.Expressions.Expression<Func<LocationSpecies.Domain.Location, object>>) (l => l.Relations), (System.Linq.Expressions.Expression<Func<object>>) (() => lr)).Where((System.Linq.Expressions.Expression<Func<LocationSpecies.Domain.Location, bool>>) (l => lr.Code == v5Loc.CountyCode && lr.Parent == state)).Cacheable().SingleOrDefault();
                  str2 = v5Loc.StateCode;
                  if (county == null)
                  {
                    id = state.Id;
                  }
                  else
                  {
                    LocationSpecies.Domain.Location location3 = statelessSession.QueryOver<LocationSpecies.Domain.Location>().JoinAlias((System.Linq.Expressions.Expression<Func<LocationSpecies.Domain.Location, object>>) (l => l.Relations), (System.Linq.Expressions.Expression<Func<object>>) (() => lr)).Where((System.Linq.Expressions.Expression<Func<LocationSpecies.Domain.Location, bool>>) (l => lr.Code == v5Loc.CityCode && lr.Parent == county)).Cacheable().SingleOrDefault();
                    str3 = v5Loc.CountyCode;
                    if (location3 == null)
                    {
                      id = county.Id;
                    }
                    else
                    {
                      id = location3.Id;
                      str4 = v5Loc.CityCode;
                    }
                  }
                }
              }
              location2 = statelessSession.Get<LocationSpecies.Domain.Location>((object) id);
            }
            Eco.Domain.v6.Project project = new Eco.Domain.v6.Project()
            {
              Name = v5Loc.Id,
              NationCode = str1,
              PrimaryPartitionCode = str2,
              SecondaryPartitionCode = str3,
              TertiaryPartitionCode = str4,
              UFOREVersion = v5Loc.UFOREVersion,
              LocationId = id,
              Comments = v5Loc.Comments
            };
            project.Locations.Add(new ProjectLocation()
            {
              Project = project,
              LocationId = project.LocationId,
              NationCode = project.NationCode,
              IsUrban = location2 != null && location2.PercentUrban > 50.0,
              PrimaryPartitionCode = project.PrimaryPartitionCode,
              SecondaryPartitionCode = project.SecondaryPartitionCode,
              TertiaryPartitionCode = project.TertiaryPartitionCode
            });
            this.m_v6s.Save((object) project);
            this.m_map.Add((object) v5Loc, (Eco.Domain.v6.Entity) project);
          }
          this.Report(Eco.Util.Resources.Strings.SavingProjects, -1, 0);
          transaction.Commit();
        }
      }
    }

    private void ConvertSeries()
    {
      using (this.m_v5s.BeginTransaction())
      {
        IList<Eco.Domain.v5.Series> seriesList = this.m_v5s.CreateCriteria<Eco.Domain.v5.Series>().List<Eco.Domain.v5.Series>();
        int num = 0;
        using (ITransaction transaction = this.m_v6s.BeginTransaction())
        {
          foreach (Eco.Domain.v5.Series key in (IEnumerable<Eco.Domain.v5.Series>) seriesList)
          {
            this.ThrowIfCanceled();
            this.Report(Eco.Util.Resources.Strings.ConvertSeries, seriesList.Count, num++);
            Eco.Domain.v6.Series series = new Eco.Domain.v6.Series()
            {
              Project = this.m_map.GetEntity<Eco.Domain.v6.Project>((object) key.Location),
              Id = key.Id,
              IsPermanent = key.IsPermanent,
              SampleType = (SampleType) key.SampleType,
              SampleMethod = (SampleMethod) key.SampleMethod,
              DefaultPlotSize = key.DefaultSubplotSize,
              DefaultPlotSizeUnit = (PlotSize) key.DefaultSubplotSizeUnit,
              GISProjection = key.GISProjection,
              GISUnit = key.GISUnit,
              Comments = key.Comments
            };
            this.m_v6s.Save((object) series);
            this.m_map.Add((object) key, (Eco.Domain.v6.Entity) series);
          }
          this.Report(Eco.Util.Resources.Strings.SavingSeries, -1, 0);
          transaction.Commit();
        }
      }
    }

    private void ConvertProjects()
    {
      using (this.m_v5s.BeginTransaction())
      {
        IList<Eco.Domain.v5.Project> projectList = this.m_v5s.CreateCriteria<Eco.Domain.v5.Project>().List<Eco.Domain.v5.Project>();
        int num = 0;
        using (ITransaction transaction = this.m_v6s.BeginTransaction())
        {
          foreach (Eco.Domain.v5.Project project in (IEnumerable<Eco.Domain.v5.Project>) projectList)
          {
            this.ThrowIfCanceled();
            this.Report(Eco.Util.Resources.Strings.ConvertYears, projectList.Count, num++);
            Eco.Domain.v6.Series entity = this.m_map.GetEntity<Eco.Domain.v6.Series>((object) project.Series);
            Year year = new Year()
            {
              Series = entity,
              Id = project.Id,
              Unit = (YearUnit) project.Unit,
              IsInitialMeasurement = project.IsInitialMeasurement,
              DBHActual = true,
              Changed = true,
              RecordHydro = project.RecordHydro,
              RecordPercentShrub = entity.IsSample && project.RecordShrub,
              RecordShrub = entity.IsSample && project.RecordShrub,
              RecordEnergy = project.RecordEnergy,
              RecordPlantableSpace = project.RecordPlantableSpace,
              RecordCrownSize = true,
              RecordCrownCondition = true,
              RecordHeight = true,
              RecordGroundCover = entity.IsSample,
              RecordReferenceObjects = entity.IsSample,
              RecordLanduse = true,
              RecordPlotCenter = entity.IsSample,
              RecordStreetTree = true,
              RecordTreeStatus = true,
              RecordStrata = entity.IsSample,
              RecordPlotAddress = entity.IsSample,
              RecordTreeAddress = !entity.IsSample,
              RecordGPS = entity.IsSample,
              RecordCLE = project.RecordCLE,
              RecordIPED = project.RecordIPED,
              Comments = project.Comments
            };
            this.m_map.Add((object) project, (Eco.Domain.v6.Entity) year);
            YearData yd = new YearData();
            this.CreateLandUses(year, yd);
            this.CreateConditions(year, yd);
            YearHelper.CreateDBHRptClasses(year);
            YearHelper.CreateHealthRptClasses(year);
            this.CreateLookup<MaintType, MaintRec>(year, year.MaintRecs);
            this.CreateLookup<LocationSpecies.Domain.MaintTask, Eco.Domain.v6.MaintTask>(year, year.MaintTasks);
            this.CreateLookup<SidewalkDamage, Sidewalk>(year, year.SidewalkDamages);
            this.CreateLookup<LocationSpecies.Domain.WireConflict, Eco.Domain.v6.WireConflict>(year, year.WireConflicts);
            this.ConvertYearLocationData(project, year);
            this.ConvertElementPrices(project, year);
            this.m_yd[year] = yd;
            this.m_v6s.Save((object) year);
          }
          this.Report(Eco.Util.Resources.Strings.SavingYears, -1, 0);
          transaction.Commit();
        }
      }
    }

    private void CreateLookup<LSLU, T>(Year y, ISet<T> lookups)
      where LSLU : LocationSpecies.Domain.Lookup
      where T : Eco.Domain.v6.Lookup
    {
      using (IStatelessSession statelessSession = this.m_sfLS.OpenStatelessSession())
      {
        using (statelessSession.BeginTransaction())
        {
          foreach (LSLU lslu in (IEnumerable<LSLU>) statelessSession.CreateCriteria<LSLU>().SetCacheable(true).List<LSLU>())
          {
            T instance = Activator.CreateInstance<T>();
            instance.Id = lslu.Id;
            instance.Description = lslu.Description;
            instance.Year = y;
            lookups.Add(instance);
          }
        }
      }
    }

    private double GetElectricityPrice(int locId)
    {
      LocationCost locationCost = this.GetLocationCost(locId);
      return locationCost != null ? locationCost.Electricity : 0.0;
    }

    private double GetGasPrice(int locId)
    {
      LocationCost locationCost = this.GetLocationCost(locId);
      return locationCost != null ? locationCost.Fuels / 10.002387672 : 0.0;
    }

    private double GetCarbonPrice(int locId)
    {
      LocationEnvironmentalValue environmentalValue = this.GetEnvironmentalValue(locId);
      return environmentalValue != null ? (double) environmentalValue.Carbon : 0.0;
    }

    private double GetH2OPrice(int locId)
    {
      LocationEnvironmentalValue environmentalValue = this.GetEnvironmentalValue(locId);
      return environmentalValue != null ? environmentalValue.RainfallInterception * 264.172 : 0.0;
    }

    private LocationCost GetLocationCost(int locId)
    {
      using (ISession session = this.m_sfLS.OpenSession())
      {
        LocationSpecies.Domain.Location location;
        LocationRelation locationRelation;
        for (location = session.Get<LocationSpecies.Domain.Location>((object) locId); location != null && location.LocationCost == null; location = locationRelation == null ? (LocationSpecies.Domain.Location) null : locationRelation.Parent)
          locationRelation = session.CreateCriteria<LocationRelation>().Add((ICriterion) Restrictions.Eq("Location", (object) location)).Add((ICriterion) Restrictions.IsNotNull("Code")).SetCacheable(true).UniqueResult<LocationRelation>();
        return location?.LocationCost;
      }
    }

    private LocationEnvironmentalValue GetEnvironmentalValue(int locId)
    {
      using (ISession session = this.m_sfLS.OpenSession())
      {
        LocationSpecies.Domain.Location location;
        LocationRelation locationRelation;
        for (location = session.Get<LocationSpecies.Domain.Location>((object) locId); location != null && location.EnvironmentalValue == null; location = locationRelation == null ? (LocationSpecies.Domain.Location) null : locationRelation.Parent)
          locationRelation = session.CreateCriteria<LocationRelation>().Add((ICriterion) Restrictions.Eq("Location", (object) location)).Add((ICriterion) Restrictions.IsNotNull("Code")).SetCacheable(true).UniqueResult<LocationRelation>();
        return location?.EnvironmentalValue;
      }
    }

    private void ConvertElementPrices(Eco.Domain.v5.Project v5Proj, Year v6Year)
    {
      Eco.Domain.v6.Project project = v6Year.Series.Project;
      if (v5Proj.Carbon != null)
      {
        Year year = v6Year;
        Eco.Domain.v6.Carbon carbon = new Eco.Domain.v6.Carbon();
        carbon.Price = v5Proj.Carbon.Price;
        carbon.Year = v6Year;
        year.Carbon = carbon;
      }
      else
      {
        Year year = v6Year;
        Eco.Domain.v6.Carbon carbon = new Eco.Domain.v6.Carbon();
        carbon.Price = this.GetCarbonPrice(project.LocationId);
        carbon.Year = v6Year;
        year.Carbon = carbon;
      }
      if (v5Proj.Electricity != null)
      {
        Year year = v6Year;
        Eco.Domain.v6.Electricity electricity = new Eco.Domain.v6.Electricity();
        electricity.Price = v5Proj.Electricity.Price;
        electricity.Year = v6Year;
        year.Electricity = electricity;
      }
      else
      {
        Year year = v6Year;
        Eco.Domain.v6.Electricity electricity = new Eco.Domain.v6.Electricity();
        electricity.Price = this.GetElectricityPrice(project.LocationId);
        electricity.Year = v6Year;
        year.Electricity = electricity;
      }
      if (v5Proj.Gas != null)
      {
        Year year = v6Year;
        Eco.Domain.v6.Gas gas = new Eco.Domain.v6.Gas();
        gas.Price = v5Proj.Gas.Price;
        gas.Year = v6Year;
        year.Gas = gas;
      }
      else
      {
        Year year = v6Year;
        Eco.Domain.v6.Gas gas = new Eco.Domain.v6.Gas();
        gas.Price = this.GetGasPrice(project.LocationId);
        gas.Year = v6Year;
        year.Gas = gas;
      }
      if (v5Proj.ExchangeRate != null)
      {
        Year year = v6Year;
        Eco.Domain.v6.ExchangeRate exchangeRate = new Eco.Domain.v6.ExchangeRate();
        exchangeRate.Price = v5Proj.ExchangeRate.Price;
        exchangeRate.Year = v6Year;
        year.ExchangeRate = exchangeRate;
      }
      if (v5Proj.H2O != null)
      {
        Year year = v6Year;
        Eco.Domain.v6.H2O h2O = new Eco.Domain.v6.H2O();
        h2O.Price = v5Proj.H2O.Price;
        h2O.Year = v6Year;
        year.H2O = h2O;
      }
      else
      {
        Year year = v6Year;
        Eco.Domain.v6.H2O h2O = new Eco.Domain.v6.H2O();
        h2O.Price = this.GetH2OPrice(project.LocationId);
        h2O.Year = v6Year;
        year.H2O = h2O;
      }
      if (v5Proj.CO != null)
      {
        Year year = v6Year;
        Eco.Domain.v6.CO co = new Eco.Domain.v6.CO();
        co.Price = v5Proj.CO.Price;
        co.Year = v6Year;
        year.CO = co;
      }
      if (v5Proj.NO2 != null)
      {
        Year year = v6Year;
        Eco.Domain.v6.NO2 no2 = new Eco.Domain.v6.NO2();
        no2.Price = v5Proj.NO2.Price;
        no2.Year = v6Year;
        year.NO2 = no2;
      }
      if (v5Proj.O3 != null)
      {
        Year year = v6Year;
        Eco.Domain.v6.O3 o3 = new Eco.Domain.v6.O3();
        o3.Price = v5Proj.O3.Price;
        o3.Year = v6Year;
        year.O3 = o3;
      }
      if (v5Proj.PM10 != null)
      {
        Year year = v6Year;
        Eco.Domain.v6.PM10 pm10 = new Eco.Domain.v6.PM10();
        pm10.Price = v5Proj.PM10.Price;
        pm10.Year = v6Year;
        year.PM10 = pm10;
      }
      if (v5Proj.PM25 != null)
      {
        Year year = v6Year;
        Eco.Domain.v6.PM25 pm25 = new Eco.Domain.v6.PM25();
        pm25.Price = v5Proj.PM25.Price;
        pm25.Year = v6Year;
        year.PM25 = pm25;
      }
      if (v5Proj.SO2 == null)
        return;
      Year year1 = v6Year;
      Eco.Domain.v6.SO2 so2 = new Eco.Domain.v6.SO2();
      so2.Price = v5Proj.SO2.Price;
      so2.Year = v6Year;
      year1.SO2 = so2;
    }

    private void CreateLandUses(Year v6Year, YearData yd)
    {
      using (IStatelessSession statelessSession = this.m_sfLS.OpenStatelessSession())
      {
        foreach (LocationSpecies.Domain.FieldLandUse fieldLandUse in (IEnumerable<LocationSpecies.Domain.FieldLandUse>) statelessSession.CreateCriteria<LocationSpecies.Domain.FieldLandUse>().Add((ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq("Code", (object) 'N'))).SetCacheable(true).List<LocationSpecies.Domain.FieldLandUse>())
        {
          this.ThrowIfCanceled();
          LandUse landUse = new LandUse()
          {
            Year = v6Year,
            Id = fieldLandUse.Code,
            Description = fieldLandUse.Description,
            LandUseId = fieldLandUse.Id
          };
          yd.LandUses[landUse.Id] = landUse;
          v6Year.LandUses.Add(landUse);
        }
      }
    }

    private void CreateConditions(Year v6Year, YearData yd)
    {
      YearHelper.CreateConditions(v6Year);
      foreach (Condition condition in (IEnumerable<Condition>) v6Year.Conditions)
      {
        this.ThrowIfCanceled();
        yd.Conditions[condition.PctDieback] = condition;
      }
    }

    private void ConvertYearLocationData(Eco.Domain.v5.Project v5Proj, Year v6Year)
    {
      ProjectLocation projectLocation = v6Year.Series.Project.Locations.FirstOrDefault<ProjectLocation>();
      YearLocationData yearLocationData1 = new YearLocationData()
      {
        Year = v6Year,
        ProjectLocation = projectLocation,
        WeatherYear = v5Proj.Series.Location.WeatherYear,
        WeatherStationId = v5Proj.Series.Location.WeatherStationID,
        PollutionYear = v5Proj.Series.Location.WeatherYear
      };
      using (IStatelessSession statelessSession = this.m_sfLS.OpenStatelessSession())
      {
        LocationSpecies.Domain.Location location = statelessSession.Get<LocationSpecies.Domain.Location>((object) projectLocation.LocationId);
        int num1 = location != null ? 1 : 0;
        int? population = location.Population;
        int num2 = population.HasValue ? 1 : 0;
        if ((num1 & num2) != 0)
        {
          YearLocationData yearLocationData2 = yearLocationData1;
          population = location.Population;
          int num3 = population.Value;
          yearLocationData2.Population = num3;
        }
      }
      v6Year.YearLocationData.Add(yearLocationData1);
      projectLocation.YearLocationData.Add(yearLocationData1);
    }

    private void ConvertCoverTypes()
    {
      using (this.m_v5s.BeginTransaction())
      {
        IList<Eco.Domain.v5.CoverType> coverTypeList = this.m_v5s.CreateCriteria<Eco.Domain.v5.CoverType>().List<Eco.Domain.v5.CoverType>();
        int num = 0;
        using (ITransaction transaction = this.m_v6s.BeginTransaction())
        {
          foreach (Eco.Domain.v5.CoverType key in (IEnumerable<Eco.Domain.v5.CoverType>) coverTypeList)
          {
            this.ThrowIfCanceled();
            this.Report(Eco.Util.Resources.Strings.ConvertGroundCovers, coverTypeList.Count, num++);
            GroundCover groundCover = new GroundCover()
            {
              Year = this.m_map.GetEntity<Year>((object) key.Project),
              Id = key.Id,
              Description = key.Description,
              CoverTypeId = key.Id <= 0 || key.Id >= 11 ? 0 : key.Id
            };
            this.m_v6s.Save((object) groundCover);
            this.m_map.Add((object) key, (Eco.Domain.v6.Entity) groundCover);
          }
          this.Report(Eco.Util.Resources.Strings.SavingGroundCovers, -1, 0);
          transaction.Commit();
        }
      }
    }

    private void ConvertMapLandUses()
    {
      using (this.m_v5s.BeginTransaction())
      {
        IList<MapLandUse> mapLandUseList = this.m_v5s.CreateCriteria<MapLandUse>().List<MapLandUse>();
        int num = 0;
        using (ITransaction transaction = this.m_v6s.BeginTransaction())
        {
          foreach (MapLandUse key in (IEnumerable<MapLandUse>) mapLandUseList)
          {
            this.ThrowIfCanceled();
            this.Report(Eco.Util.Resources.Strings.ConvertStrata, mapLandUseList.Count, num++);
            Year entity = this.m_map.GetEntity<Year>((object) key.Project);
            YearData yearData = this.m_yd[entity];
            Strata strata = new Strata()
            {
              Year = entity,
              Id = key.Id,
              Description = key.Description,
              Abbreviation = key.Abbreviation,
              Size = key.Size
            };
            yearData.Strata[strata.Id] = strata;
            this.m_map.Add((object) key, (Eco.Domain.v6.Entity) strata);
            entity.Strata.Add(strata);
          }
          this.Report(Eco.Util.Resources.Strings.SavingStrata, -1, 0);
          transaction.Commit();
        }
      }
    }

    private void ConvertPlots()
    {
      using (this.m_v5s.BeginTransaction())
      {
        IList<Eco.Domain.v5.Plot> plotList = this.m_v5s.CreateCriteria<Eco.Domain.v5.Plot>().Fetch(SelectMode.Fetch, "SubPlots").SetResultTransformer(Transformers.DistinctRootEntity).List<Eco.Domain.v5.Plot>();
        int num = 0;
        using (ITransaction transaction = this.m_v6s.BeginTransaction())
        {
          foreach (Eco.Domain.v5.Plot key in (IEnumerable<Eco.Domain.v5.Plot>) plotList)
          {
            this.ThrowIfCanceled();
            this.Report(Eco.Util.Resources.Strings.ConvertPlots, plotList.Count, num++);
            Eco.Domain.v6.Project entity1 = this.m_map.GetEntity<Eco.Domain.v6.Project>((object) key.Project.Series.Location);
            Eco.Domain.v6.Series entity2 = this.m_map.GetEntity<Eco.Domain.v6.Series>((object) key.Project.Series);
            Year entity3 = this.m_map.GetEntity<Year>((object) key.Project);
            Strata stratum = this.m_yd[entity3].Strata[key.MapLandUse];
            ProjectLocation projectLocation = entity1.Locations.FirstOrDefault<ProjectLocation>();
            SubPlot subPlot = key.SubPlots.FirstOrDefault<SubPlot>();
            Eco.Domain.v6.Plot plot = new Eco.Domain.v6.Plot()
            {
              Year = entity3,
              Strata = stratum,
              ProjectLocation = projectLocation,
              Id = key.Id,
              Address = entity2.IsSample ? key.Address : (string) null,
              Latitude = key.Latitude,
              Longitude = key.Longitude,
              Date = key.Date,
              Crew = key.Crew,
              ContactInfo = key.ContactInfo,
              Comments = key.Comments,
              IsComplete = subPlot != null
            };
            if (subPlot != null)
            {
              plot.Size = subPlot.Size;
              plot.Photo = subPlot.Photo;
              plot.Stake = subPlot.Stake;
              plot.PercentTreeCover = (PctMidRange) subPlot.PercentTreeCover;
              plot.PercentShrubCover = (PctMidRange) subPlot.PercentShrubCover;
              plot.PercentPlantable = (PctMidRange) subPlot.PercentPlantable;
              plot.PercentMeasured = (int) subPlot.PercentMeasured;
            }
            else
            {
              float defaultSubplotSize = key.Project.Series.DefaultSubplotSize;
              plot.Size = (double) defaultSubplotSize + 1.0 <= 1.4012984643248171E-45 ? (key.Project.Unit != 'E' ? 0.0404686f : 0.1f) : defaultSubplotSize;
            }
            this.m_v6s.Save((object) plot);
            this.m_map.Add((object) key, (Eco.Domain.v6.Entity) plot);
          }
          this.Report(Eco.Util.Resources.Strings.SavingPlots, -1, 0);
          transaction.Commit();
        }
      }
    }

    private void ConvertSubPlotCovers()
    {
      using (this.m_v5s.BeginTransaction())
      {
        IList<SubPlotCover> subPlotCoverList = this.m_v5s.CreateCriteria<SubPlotCover>().List<SubPlotCover>();
        int num = 0;
        using (ITransaction transaction = this.m_v6s.BeginTransaction())
        {
          foreach (SubPlotCover subPlotCover in (IEnumerable<SubPlotCover>) subPlotCoverList)
          {
            this.ThrowIfCanceled();
            this.Report(Eco.Util.Resources.Strings.ConvertPlotGroundCovers, subPlotCoverList.Count, num++);
            Eco.Domain.v6.Plot entity1 = this.m_map.GetEntity<Eco.Domain.v6.Plot>((object) subPlotCover.SubPlot.Plot);
            GroundCover entity2 = this.m_map.GetEntity<GroundCover>((object) subPlotCover.CoverType);
            this.m_v6s.Save((object) new PlotGroundCover()
            {
              Plot = entity1,
              GroundCover = entity2,
              PercentCovered = subPlotCover.PercentCovered
            });
          }
          this.Report(Eco.Util.Resources.Strings.SavingPlotGroundCovers, -1, 0);
          transaction.Commit();
        }
      }
    }

    private void ConvertPlotFieldLandUses()
    {
      using (this.m_v5s.BeginTransaction())
      {
        IList<Eco.Domain.v5.FieldLandUse> fieldLandUseList = this.m_v5s.CreateCriteria<Eco.Domain.v5.FieldLandUse>().List<Eco.Domain.v5.FieldLandUse>();
        int num = 0;
        using (ITransaction transaction = this.m_v6s.BeginTransaction())
        {
          foreach (Eco.Domain.v5.FieldLandUse fieldLandUse in (IEnumerable<Eco.Domain.v5.FieldLandUse>) fieldLandUseList)
          {
            this.ThrowIfCanceled();
            this.Report(Eco.Util.Resources.Strings.ConvertPlotLandUses, fieldLandUseList.Count, num++);
            Year entity1 = this.m_map.GetEntity<Year>((object) fieldLandUse.SubPlot.Plot.Project);
            Eco.Domain.v6.Plot entity2 = this.m_map.GetEntity<Eco.Domain.v6.Plot>((object) fieldLandUse.SubPlot.Plot);
            YearData yearData = this.m_yd[entity1];
            PlotData plotData = (PlotData) null;
            if (!this.m_pd.TryGetValue(entity2, out plotData))
            {
              plotData = new PlotData();
              this.m_pd[entity2] = plotData;
            }
            char key = fieldLandUse.Id;
            if (!yearData.LandUses.ContainsKey(key))
              key = 'O';
            if (!plotData.LandUses.ContainsKey(key))
            {
              LandUse landUse = yearData.LandUses[key];
              PlotLandUse plotLandUse = new PlotLandUse()
              {
                Plot = entity2,
                LandUse = landUse,
                PercentOfPlot = fieldLandUse.PercentOfSubPlot
              };
              plotData.LandUses.Add(landUse.Id, plotLandUse);
              this.m_v6s.Save((object) plotLandUse);
            }
            else
            {
              PlotLandUse landUse = plotData.LandUses[key];
              landUse.PercentOfPlot += fieldLandUse.PercentOfSubPlot;
              this.m_v6s.Save((object) landUse);
            }
          }
          this.Report(Eco.Util.Resources.Strings.SavingPlotLandUses, -1, 0);
          transaction.Commit();
        }
      }
    }

    private void ConvertTrees()
    {
      using (this.m_v5s.BeginTransaction())
      {
        IList<Eco.Domain.v5.Tree> treeList = this.m_v5s.CreateCriteria<Eco.Domain.v5.Tree>().List<Eco.Domain.v5.Tree>();
        int num = 0;
        using (ITransaction transaction = this.m_v6s.BeginTransaction())
        {
          foreach (Eco.Domain.v5.Tree key in (IEnumerable<Eco.Domain.v5.Tree>) treeList)
          {
            this.ThrowIfCanceled();
            this.Report(Eco.Util.Resources.Strings.ConvertTrees, treeList.Count, num++);
            Eco.Domain.v6.Series entity1 = this.m_map.GetEntity<Eco.Domain.v6.Series>((object) key.SubPlot.Plot.Project.Series);
            Year entity2 = this.m_map.GetEntity<Year>((object) key.SubPlot.Plot.Project);
            Eco.Domain.v6.Plot entity3 = this.m_map.GetEntity<Eco.Domain.v6.Plot>((object) key.SubPlot.Plot);
            YearData yearData = this.m_yd[entity2];
            PlotData plotData = this.m_pd[entity3];
            char fieldLandUse = key.FieldLandUse;
            PlotLandUse plotLandUse = (PlotLandUse) null;
            if (plotData.LandUses.ContainsKey(fieldLandUse))
              plotLandUse = plotData.LandUses[fieldLandUse];
            else if (plotData.LandUses.ContainsKey('O'))
              plotLandUse = plotData.LandUses['O'];
            else if (plotData.LandUses.Count == 1)
              plotLandUse = plotData.LandUses.Values.First<PlotLandUse>();
            Condition condition = yearData.Conditions[(double) key.CrownDieback];
            char ch = key.Status;
            if (entity2.IsInitialMeasurement)
            {
              switch ((TreeStatus) ch)
              {
                case TreeStatus.HealthyRemoved:
                case TreeStatus.HazardRemoved:
                case TreeStatus.LandUseChangeRemoved:
                case TreeStatus.NoChange:
                case TreeStatus.InitialSample:
                case TreeStatus.UnknownRemoved:
                  ch = 'U';
                  break;
              }
            }
            Eco.Domain.v6.Tree tree = new Eco.Domain.v6.Tree()
            {
              Plot = entity3,
              PlotLandUse = plotLandUse,
              Crown = new Crown()
              {
                BaseHeight = key.CrownBaseHeight,
                TopHeight = key.CrownTopHeight,
                WidthNS = key.CrownWidthNS,
                WidthEW = key.CrownWidthEW,
                LightExposure = (CrownLightExposure) key.CrownLightExposure,
                PercentMissing = (PctMidRange) key.PercentCrownMissing,
                Condition = condition
              },
              IPED = new IPED()
              {
                TSDieback = key.PestTSDieback,
                TSEpiSprout = key.PestTSEpiSprout,
                TSWiltFoli = key.PestTSWiltFoli,
                TSEnvStress = key.PestTSEnvStress,
                TSHumStress = key.PestTSHumStress,
                TSNotes = key.PestTSNotes,
                FTChewFoli = key.PestFTChewFoli,
                FTDiscFoli = key.PestFTDiscFoli,
                FTAbnFoli = key.PestFTAbnFoli,
                FTInsectSigns = key.PestFTInsectSigns,
                FTFoliAffect = key.PestFTFoliAffect,
                FTNotes = key.PestFTNotes,
                BBInsectSigns = key.PestBBInsectSigns,
                BBInsectPres = key.PestBBInsectPres,
                BBDiseaseSigns = key.PestBBDiseaseSigns,
                BBProbLoc = key.PestBBProbLoc,
                BBAbnGrowth = key.PestBBAbnGrowth,
                BBNotes = key.PestBBNotes,
                Pest = key.PestPest
              },
              Id = key.Id,
              DirectionFromCenter = key.DirectionFromCenter,
              DistanceFromCenter = key.DistanceFromCenter,
              Status = ch,
              Species = key.Species,
              TreeHeight = key.TreeHeight,
              PercentImpervious = (PctMidRange) key.PercentImpervious,
              PercentShrub = (PctMidRange) key.PercentShrub,
              StreetTree = key.Site != 'N',
              Address = entity1.IsSample ? (string) null : key.SubPlot.Plot.Address,
              SurveyDate = entity3.Date,
              Comments = key.Comments
            };
            this.m_v6s.Save((object) tree);
            this.m_map.Add((object) key, (Eco.Domain.v6.Entity) tree);
          }
          this.Report(Eco.Util.Resources.Strings.SavingTrees, -1, 0);
          transaction.Commit();
        }
      }
    }

    private void ConvertStems()
    {
      using (this.m_v5s.BeginTransaction())
      {
        IList<Eco.Domain.v5.Stem> stemList = this.m_v5s.CreateCriteria<Eco.Domain.v5.Stem>().List<Eco.Domain.v5.Stem>();
        int num1 = 0;
        using (ITransaction transaction = this.m_v6s.BeginTransaction())
        {
          foreach (Eco.Domain.v5.Stem stem in (IEnumerable<Eco.Domain.v5.Stem>) stemList)
          {
            this.ThrowIfCanceled();
            this.Report(Eco.Util.Resources.Strings.ConvertStems, stemList.Count, num1++);
            Year entity = this.m_map.GetEntity<Year>((object) stem.Tree.SubPlot.Plot.Project);
            double num2 = -1.0;
            if (entity.Unit == YearUnit.English && Math.Abs((double) stem.DiameterHeight - 4.5) > double.Epsilon || entity.Unit == YearUnit.Metric && Math.Abs((double) stem.DiameterHeight - 1.37) > double.Epsilon)
              num2 = (double) stem.DiameterHeight;
            this.m_v6s.Save((object) new Eco.Domain.v6.Stem()
            {
              Tree = this.m_map.GetEntity<Eco.Domain.v6.Tree>((object) stem.Tree),
              Id = stem.Id,
              Diameter = (double) stem.Diameter,
              DiameterHeight = num2,
              Measured = stem.Measured
            });
          }
          this.Report(Eco.Util.Resources.Strings.SavingStems, -1, 0);
          transaction.Commit();
        }
      }
    }

    private void ConvertBuildingInteractions()
    {
      using (this.m_v5s.BeginTransaction())
      {
        IList<BuildingInteraction> buildingInteractionList = this.m_v5s.CreateCriteria<BuildingInteraction>().List<BuildingInteraction>();
        int num = 0;
        using (ITransaction transaction = this.m_v6s.BeginTransaction())
        {
          foreach (BuildingInteraction buildingInteraction in (IEnumerable<BuildingInteraction>) buildingInteractionList)
          {
            this.ThrowIfCanceled();
            this.Report(Eco.Util.Resources.Strings.ConvertBuildings, buildingInteractionList.Count, num++);
            this.m_v6s.Save((object) new Building()
            {
              Tree = this.m_map.GetEntity<Eco.Domain.v6.Tree>((object) buildingInteraction.Tree),
              Id = buildingInteraction.Id,
              Direction = buildingInteraction.Direction,
              Distance = buildingInteraction.Distance
            });
          }
          this.Report(Eco.Util.Resources.Strings.SavingBuildings, -1, 0);
          transaction.Commit();
        }
      }
    }

    private void ConvertReferenceObjects()
    {
      using (this.m_v5s.BeginTransaction())
      {
        IList<Eco.Domain.v5.ReferenceObject> referenceObjectList = this.m_v5s.CreateCriteria<Eco.Domain.v5.ReferenceObject>().List<Eco.Domain.v5.ReferenceObject>();
        int num = 0;
        using (ITransaction transaction = this.m_v6s.BeginTransaction())
        {
          foreach (Eco.Domain.v5.ReferenceObject referenceObject in (IEnumerable<Eco.Domain.v5.ReferenceObject>) referenceObjectList)
          {
            this.ThrowIfCanceled();
            this.Report(Eco.Util.Resources.Strings.ConvertReferenceObjects, referenceObjectList.Count, num++);
            this.m_v6s.Save((object) new Eco.Domain.v6.ReferenceObject()
            {
              Plot = this.m_map.GetEntity<Eco.Domain.v6.Plot>((object) referenceObject.SubPlot.Plot),
              Direction = referenceObject.Direction,
              Distance = referenceObject.Distance,
              Object = (ReferenceObjectType) referenceObject.Object,
              DBH = referenceObject.DBH,
              Notes = referenceObject.Notes
            });
          }
          this.Report(Eco.Util.Resources.Strings.SavingReferenceObjects, -1, 0);
          transaction.Commit();
        }
      }
    }

    private void ConvertShrubs()
    {
      using (this.m_v5s.BeginTransaction())
      {
        IList<Eco.Domain.v5.Shrub> shrubList = this.m_v5s.CreateCriteria<Eco.Domain.v5.Shrub>().List<Eco.Domain.v5.Shrub>();
        int num = 0;
        using (ITransaction transaction = this.m_v6s.BeginTransaction())
        {
          foreach (Eco.Domain.v5.Shrub shrub in (IEnumerable<Eco.Domain.v5.Shrub>) shrubList)
          {
            this.ThrowIfCanceled();
            this.Report(Eco.Util.Resources.Strings.ConvertShrubs, shrubList.Count, num++);
            this.m_v6s.Save((object) new Eco.Domain.v6.Shrub()
            {
              Plot = this.m_map.GetEntity<Eco.Domain.v6.Plot>((object) shrub.SubPlot.Plot),
              Id = shrub.Id,
              PercentOfShrubArea = shrub.PercentOfShrubArea,
              Species = shrub.Species,
              Height = shrub.Height,
              PercentMissing = (PctMidRange) shrub.PercentMissing,
              Comments = shrub.Comments
            });
          }
          this.Report(Eco.Util.Resources.Strings.SavingShrubs, -1, 0);
          transaction.Commit();
        }
      }
    }

    private ISessionFactory BuildFactory(string dbFile, Type t)
    {
      Configuration configuration = new Configuration();
      configuration.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
      configuration.SetProperty("dialect", "NHibernate.JetDriver.JetDialect, NHibernate.JetDriver");
      configuration.SetProperty("connection.driver_class", "NHibernate.JetDriver.JetDriver, NHibernate.JetDriver");
      configuration.SetProperty("connection.connection_string", string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}", (object) Path.GetFullPath(dbFile)));
      configuration.AddAssembly(t.Assembly);
      return configuration.BuildSessionFactory();
    }
  }
}
