// Decompiled with JetBrains decompiler
// Type: Eco.Util.Convert.StreetsConversion
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.v6;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using Streets.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eco.Util.Convert
{
  public class StreetsConversion
  {
    private static IList<Streets.Domain.Project> m_strProjs;
    private static ISet<Eco.Domain.v6.Project> m_v6Projs;
    private static ISessionFactory ls_sf;
    private static v6Config config;

    public static bool begin(
      IList<Streets.Domain.Project> strProjs,
      ISet<Eco.Domain.v6.Project> v6Projs,
      ISessionFactory lsSessFact,
      v6Config c)
    {
      StreetsConversion.m_strProjs = strProjs;
      StreetsConversion.m_v6Projs = v6Projs;
      StreetsConversion.ls_sf = lsSessFact;
      StreetsConversion.config = c;
      try
      {
        foreach (Streets.Domain.Project strProj in (IEnumerable<Streets.Domain.Project>) strProjs)
          v6Projs.Add(StreetsConversion.convertProject(strProj));
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    private static Eco.Domain.v6.Project convertProject(Streets.Domain.Project strProj)
    {
      if (strProj == null)
        return (Eco.Domain.v6.Project) null;
      Eco.Domain.v6.Project project = new Eco.Domain.v6.Project()
      {
        Name = strProj.Name,
        LocationId = StreetsConversion.getLocationId(strProj),
        NationCode = strProj.NationCode,
        PrimaryPartitionCode = strProj.PrimaryPartitionCode,
        SecondaryPartitionCode = strProj.SecondaryPartitionCode,
        TertiaryPartitionCode = strProj.TertiaryPartitionCode
      };
      ProjectLocation v6ProjLoc = new ProjectLocation()
      {
        Project = project,
        LocationId = project.LocationId,
        NationCode = project.NationCode,
        PrimaryPartitionCode = project.PrimaryPartitionCode,
        SecondaryPartitionCode = project.SecondaryPartitionCode,
        TertiaryPartitionCode = project.TertiaryPartitionCode
      };
      foreach (Streets.Domain.Street street in (IEnumerable<Streets.Domain.Street>) strProj.Streets)
        v6ProjLoc.Streets.Add(new Eco.Domain.v6.Street()
        {
          ProjectLocation = v6ProjLoc,
          Name = street.Name
        });
      long num = 0;
      foreach (Zone zone in (IEnumerable<Zone>) strProj.Zones)
        num += (long) zone.Segments;
      Series series = new Series()
      {
        Project = project,
        Id = StreetsConversion.config.SeriesId,
        IsPermanent = false,
        SampleType = strProj.IsSample ? SampleType.StratumStreetTree : SampleType.Inventory,
        SampleMethod = strProj.IsSample ? SampleMethod.StratifiedRandom : SampleMethod.None,
        DefaultPlotSize = (float) (5280.0 * strProj.CityData.TotalLinearStreetMiles) / (float) num,
        DefaultPlotSizeUnit = PlotSize.Feet
      };
      Year v6Year = new Year()
      {
        Series = series,
        Id = StreetsConversion.config.YearId,
        Unit = strProj.EnglishUnits ? YearUnit.English : YearUnit.Metric,
        Budget = strProj.CityData.TotalBudget,
        IsInitialMeasurement = true,
        DBHActual = strProj.DBHDirectEntry,
        RecordPlantableSpace = true,
        RecordCrownSize = true,
        RecordHeight = true,
        RecordStreetTree = true,
        RecordNoteTree = true,
        RecordSiteType = strProj.EnableSiteType,
        RecordLocSite = strProj.EnableLocSite,
        RecordLocNo = strProj.EnableLocNo,
        RecordMaintRec = strProj.EnableMaintenance,
        RecordSidewalk = strProj.EnableSideWalk,
        RecordWireConflict = strProj.EnableWireConflict,
        RecordPlotAddress = strProj.EnableStreetAddress,
        RecordOtherOne = strProj.EnableOther1,
        RecordOtherTwo = strProj.EnableOther2,
        RecordOtherThree = strProj.EnableOther3,
        RecordGPS = strProj.EnableGPS,
        MgmtStyle = new MgmtStyleEnum?(strProj.EnablePublicPrivate ? MgmtStyleEnum.RecordPublicPrivate : MgmtStyleEnum.DefaultPublic),
        OtherOne = strProj.Other1Description,
        OtherTwo = strProj.Other2Description,
        OtherThree = strProj.Other3Description
      };
      using (ISession session = StreetsConversion.ls_sf.OpenSession())
      {
        foreach (CoverType coverType in (IEnumerable<CoverType>) session.CreateCriteria<CoverType>().List<CoverType>())
          v6Year.GroundCovers.Add(new GroundCover()
          {
            Year = v6Year,
            Id = coverType.Id,
            Description = coverType.Description,
            CoverTypeId = coverType.Id
          });
      }
      StreetsConversion.convertElementPrices(strProj.Benefit.ElementPrice, v6Year);
      StreetsConversion.createLookups(v6Year, strProj);
      foreach (Zone zone in (IEnumerable<Zone>) strProj.Zones)
        v6Year.Strata.Add(new Strata()
        {
          Year = v6Year,
          Id = zone.Number,
          Description = zone.Name,
          Abbreviation = zone.Number.ToString(),
          Size = (float) zone.Segments * series.DefaultPlotSize
        });
      foreach (Streets.Domain.DBH dbh in (IEnumerable<Streets.Domain.DBH>) strProj.Dbhs)
      {
        v6Year.DBHRptClasses.Add(new DBHRptClass()
        {
          Year = v6Year,
          Id = dbh.Id.Code,
          RangeStart = dbh.RangeStart,
          RangeEnd = dbh.RangeEnd
        });
        v6Year.DBHs.Add(new Eco.Domain.v6.DBH()
        {
          Year = v6Year,
          Id = dbh.Id.Code,
          Description = string.Format("> {0} and <= {1}", (object) dbh.RangeStart, (object) dbh.RangeEnd),
          Value = Math.Round((dbh.RangeStart + dbh.RangeEnd) / 2.0, 1)
        });
      }
      foreach (Streets.Domain.Condition condition in (IEnumerable<Streets.Domain.Condition>) strProj.Conditions)
        v6Year.Conditions.Add(new Eco.Domain.v6.Condition()
        {
          Year = v6Year,
          Id = condition.Id.Code,
          Description = condition.Description,
          PctDieback = 100.0 - condition.LeavesConditionFactor
        });
      using (ISession session = StreetsConversion.ls_sf.OpenSession())
      {
        foreach (LocationSpecies.Domain.FieldLandUse fieldLandUse in (IEnumerable<LocationSpecies.Domain.FieldLandUse>) session.CreateCriteria<LocationSpecies.Domain.FieldLandUse>().Add((ICriterion) new SimpleExpression("Code", (object) 'N', " <> ")).List<LocationSpecies.Domain.FieldLandUse>())
          v6Year.LandUses.Add(new LandUse()
          {
            Year = v6Year,
            Id = fieldLandUse.Code,
            Description = fieldLandUse.Description,
            LandUseId = fieldLandUse.Id
          });
      }
      foreach (ManagementCost managementCost in (IEnumerable<ManagementCost>) strProj.ManagementCosts)
        v6Year.YearlyCosts.Add(new YearlyCost()
        {
          Year = v6Year,
          Public = managementCost.IsPublic,
          Planting = managementCost.Planting,
          Pruning = managementCost.Pruning,
          TreeRemoval = managementCost.TreeRemoval,
          PestControl = managementCost.PestControl,
          Irrigation = managementCost.Irrigation,
          Repair = managementCost.Repair,
          CleanUp = managementCost.CleanUp,
          Legal = managementCost.Lawsuit,
          Administrative = managementCost.Admin,
          Inspection = managementCost.Irrigation,
          Other = managementCost.Other
        });
      YearLocationData yearLocationData = new YearLocationData()
      {
        Year = v6Year,
        ProjectLocation = v6ProjLoc
      };
      v6Year.YearLocationData.Add(yearLocationData);
      v6ProjLoc.YearLocationData.Add(yearLocationData);
      foreach (SpListItem sp in (IEnumerable<SpListItem>) strProj.SpList)
      {
        if (StreetsConversion.config.SpListMap[sp.Code] == null)
          v6Year.PlantingSiteTypes.Add(new PlantingSiteType()
          {
            Year = v6Year,
            Description = sp.ScientificName,
            Size = StreetsConversion.config.PlantingSiteTypeMap[sp.CommonName]
          });
      }
      StreetsConversion.convertInventory(strProj.Items, v6Year, v6ProjLoc);
      series.Years.Add(v6Year);
      project.Series.Add(series);
      project.Locations.Add(v6ProjLoc);
      return project;
    }

    private static int getLocationId(Streets.Domain.Project strProj)
    {
      using (ISession session = StreetsConversion.ls_sf.OpenSession())
      {
        LocationRelation locationRelation1 = session.CreateCriteria<LocationRelation>().Add((ICriterion) new SimpleExpression("Code", (object) strProj.NationCode, " = ")).Add((ICriterion) new SimpleExpression("Level", (object) (short) 2, " = ")).UniqueResult<LocationRelation>();
        LocationRelation locationRelation2 = session.CreateCriteria<LocationRelation>().Add((ICriterion) new SimpleExpression("Code", (object) strProj.PrimaryPartitionCode, " = ")).Add((ICriterion) new SimpleExpression("Level", (object) (short) 3, " = ")).Add((ICriterion) new SimpleExpression("Parent", (object) locationRelation1.Location, " = ")).UniqueResult<LocationRelation>();
        if (strProj.SecondaryPartitionCode == "000")
          return locationRelation2.Location.Id;
        LocationRelation locationRelation3 = session.CreateCriteria<LocationRelation>().Add((ICriterion) new SimpleExpression("Code", (object) strProj.SecondaryPartitionCode, " = ")).Add((ICriterion) new SimpleExpression("Level", (object) (short) 4, " = ")).Add((ICriterion) new SimpleExpression("Parent", (object) locationRelation2.Location, " = ")).UniqueResult<LocationRelation>();
        return strProj.TertiaryPartitionCode == "000" ? locationRelation3.Location.Id : session.CreateCriteria<LocationRelation>().Add((ICriterion) new SimpleExpression("Code", (object) strProj.TertiaryPartitionCode, " = ")).Add((ICriterion) new SimpleExpression("Level", (object) (short) 5, " = ")).Add((ICriterion) new SimpleExpression("Parent", (object) locationRelation3.Location, " = ")).UniqueResult<LocationRelation>().Location.Id;
      }
    }

    private static void convertElementPrices(Streets.Domain.ElementPrice strElPrice, Year v6Year)
    {
      Year year1 = v6Year;
      Carbon carbon = new Carbon();
      carbon.Year = v6Year;
      carbon.Price = strElPrice.CO2 * 3.6641910962724906 * 2203.377;
      year1.Carbon = carbon;
      Year year2 = v6Year;
      Electricity electricity = new Electricity();
      electricity.Year = v6Year;
      electricity.Price = strElPrice.Electricity;
      year2.Electricity = electricity;
      Year year3 = v6Year;
      NO2 no2 = new NO2();
      no2.Year = v6Year;
      no2.Price = strElPrice.NO2;
      year3.NO2 = no2;
      Year year4 = v6Year;
      O3 o3 = new O3();
      o3.Year = v6Year;
      o3.Price = strElPrice.O3;
      year4.O3 = o3;
      Year year5 = v6Year;
      PM10 pm10 = new PM10();
      pm10.Year = v6Year;
      pm10.Price = strElPrice.PM10;
      year5.PM10 = pm10;
      Year year6 = v6Year;
      SO2 so2 = new SO2();
      so2.Year = v6Year;
      so2.Price = strElPrice.SO2;
      year6.SO2 = so2;
      Year year7 = v6Year;
      Gas gas = new Gas();
      gas.Year = v6Year;
      gas.Price = strElPrice.Gas;
      year7.Gas = gas;
      Year year8 = v6Year;
      H2O h2O = new H2O();
      h2O.Year = v6Year;
      h2O.Price = strElPrice.H2O;
      year8.H2O = h2O;
      Year year9 = v6Year;
      Home home = new Home();
      home.Year = v6Year;
      home.Price = strElPrice.Home;
      year9.Home = home;
      Year year10 = v6Year;
      VOC voc = new VOC();
      voc.Year = v6Year;
      voc.Price = strElPrice.VOC;
      year10.VOC = voc;
    }

    private static void createLookups(Year v6Year, Streets.Domain.Project strProj)
    {
      foreach (Streets.Domain.SiteType siteType1 in (IEnumerable<Streets.Domain.SiteType>) strProj.SiteTypes)
      {
        ISet<Eco.Domain.v6.SiteType> siteTypes = v6Year.SiteTypes;
        Eco.Domain.v6.SiteType siteType2 = new Eco.Domain.v6.SiteType();
        siteType2.Year = v6Year;
        siteType2.Id = siteType1.Id.Code;
        siteType2.Description = siteType1.Description;
        siteTypes.Add(siteType2);
      }
      foreach (Streets.Domain.LocSite locSite1 in (IEnumerable<Streets.Domain.LocSite>) strProj.LocSites)
      {
        ISet<Eco.Domain.v6.LocSite> locSites = v6Year.LocSites;
        Eco.Domain.v6.LocSite locSite2 = new Eco.Domain.v6.LocSite();
        locSite2.Year = v6Year;
        locSite2.Id = locSite1.Id.Code;
        locSite2.Description = locSite1.Description;
        locSites.Add(locSite2);
      }
      foreach (Streets.Domain.MaintRec maintRec1 in (IEnumerable<Streets.Domain.MaintRec>) strProj.MaintRecs)
      {
        ISet<Eco.Domain.v6.MaintRec> maintRecs = v6Year.MaintRecs;
        Eco.Domain.v6.MaintRec maintRec2 = new Eco.Domain.v6.MaintRec();
        maintRec2.Year = v6Year;
        maintRec2.Id = maintRec1.Id.Code;
        maintRec2.Description = maintRec1.Description;
        maintRecs.Add(maintRec2);
      }
      foreach (Streets.Domain.MaintTask maintTask1 in (IEnumerable<Streets.Domain.MaintTask>) strProj.MaintTasks)
      {
        ISet<Eco.Domain.v6.MaintTask> maintTasks = v6Year.MaintTasks;
        Eco.Domain.v6.MaintTask maintTask2 = new Eco.Domain.v6.MaintTask();
        maintTask2.Year = v6Year;
        maintTask2.Id = maintTask1.Id.Code;
        maintTask2.Description = maintTask1.Description;
        maintTasks.Add(maintTask2);
      }
      foreach (Streets.Domain.OtherOne otherOne1 in (IEnumerable<Streets.Domain.OtherOne>) strProj.OtherOnes)
      {
        ISet<Eco.Domain.v6.OtherOne> otherOnes = v6Year.OtherOnes;
        Eco.Domain.v6.OtherOne otherOne2 = new Eco.Domain.v6.OtherOne();
        otherOne2.Year = v6Year;
        otherOne2.Id = otherOne1.Id.Code;
        otherOne2.Description = otherOne1.Description;
        otherOnes.Add(otherOne2);
      }
      foreach (Streets.Domain.OtherTwo otherTwo1 in (IEnumerable<Streets.Domain.OtherTwo>) strProj.OtherTwos)
      {
        ISet<Eco.Domain.v6.OtherTwo> otherTwos = v6Year.OtherTwos;
        Eco.Domain.v6.OtherTwo otherTwo2 = new Eco.Domain.v6.OtherTwo();
        otherTwo2.Year = v6Year;
        otherTwo2.Id = otherTwo1.Id.Code;
        otherTwo2.Description = otherTwo1.Description;
        otherTwos.Add(otherTwo2);
      }
      foreach (Streets.Domain.OtherThree otherThree1 in (IEnumerable<Streets.Domain.OtherThree>) strProj.OtherThrees)
      {
        ISet<Eco.Domain.v6.OtherThree> otherThrees = v6Year.OtherThrees;
        Eco.Domain.v6.OtherThree otherThree2 = new Eco.Domain.v6.OtherThree();
        otherThree2.Year = v6Year;
        otherThree2.Id = otherThree1.Id.Code;
        otherThree2.Description = otherThree1.Description;
        otherThrees.Add(otherThree2);
      }
      foreach (Streets.Domain.Sidewalk sidewalkDamage in (IEnumerable<Streets.Domain.Sidewalk>) strProj.SidewalkDamages)
      {
        ISet<Eco.Domain.v6.Sidewalk> sidewalkDamages = v6Year.SidewalkDamages;
        Eco.Domain.v6.Sidewalk sidewalk = new Eco.Domain.v6.Sidewalk();
        sidewalk.Year = v6Year;
        sidewalk.Id = sidewalkDamage.Id.Code;
        sidewalk.Description = sidewalkDamage.Description;
        sidewalkDamages.Add(sidewalk);
      }
      foreach (Streets.Domain.WireConflict wireConflict1 in (IEnumerable<Streets.Domain.WireConflict>) strProj.WireConflicts)
      {
        ISet<Eco.Domain.v6.WireConflict> wireConflicts = v6Year.WireConflicts;
        Eco.Domain.v6.WireConflict wireConflict2 = new Eco.Domain.v6.WireConflict();
        wireConflict2.Year = v6Year;
        wireConflict2.Id = wireConflict1.Id.Code;
        wireConflict2.Description = wireConflict1.Description;
        wireConflicts.Add(wireConflict2);
      }
    }

    private static void convertInventory(
      ISet<InventoryItem> strInventory,
      Year v6Year,
      ProjectLocation v6ProjLoc)
    {
      int num1 = 0;
      Dictionary<int, Plot> dictionary1 = new Dictionary<int, Plot>();
      foreach (InventoryItem strTree in (IEnumerable<InventoryItem>) strInventory)
      {
        ++num1;
        Plot plot;
        if (dictionary1.ContainsKey(strTree.StreetSeg))
        {
          plot = dictionary1[strTree.StreetSeg];
        }
        else
        {
          Strata strata = (Strata) null;
          foreach (Strata stratum in (IEnumerable<Strata>) v6Year.Strata)
          {
            if (stratum.Id == strTree.Zone.Number)
            {
              strata = stratum;
              break;
            }
          }
          DateTime? nullable = new DateTime?(new DateTime(9999, 12, 31));
          foreach (InventoryItem inventoryItem in (IEnumerable<InventoryItem>) strInventory)
          {
            if (inventoryItem.SurveyDate.HasValue && inventoryItem.SurveyDate.Value.CompareTo((object) nullable) < 0)
              nullable = inventoryItem.SurveyDate;
          }
          plot = new Plot()
          {
            Year = v6Year,
            Strata = strata,
            ProjectLocation = v6ProjLoc,
            Id = strTree.StreetSeg,
            Date = nullable,
            Size = v6Year.Series.DefaultPlotSize,
            PercentMeasured = 100,
            PercentPlantable = PctMidRange.PRINV,
            PercentShrubCover = PctMidRange.PR0,
            PercentTreeCover = PctMidRange.PRINV,
            IsComplete = true
          };
          v6Year.Plots.Add(plot);
          dictionary1.Add(strTree.StreetSeg, plot);
        }
        Eco.Domain.v6.Street street1 = (Eco.Domain.v6.Street) null;
        foreach (Eco.Domain.v6.Street street2 in (IEnumerable<Eco.Domain.v6.Street>) v6ProjLoc.Streets)
        {
          if (street2.Name == strTree.Street.Name)
          {
            street1 = street2;
            break;
          }
        }
        char ch = 'O';
        try
        {
          ch = StreetsConversion.config.FieldLandUseMap[strTree.LandUse.Id.Code];
        }
        catch (Exception ex)
        {
        }
        LandUse landUse1 = (LandUse) null;
        foreach (LandUse landUse2 in (IEnumerable<LandUse>) v6Year.LandUses)
        {
          if ((int) landUse2.Id == (int) ch)
          {
            landUse1 = landUse2;
            break;
          }
        }
        PlotLandUse plotLandUse1 = (PlotLandUse) null;
        foreach (PlotLandUse plotLandUse2 in (IEnumerable<PlotLandUse>) plot.PlotLandUses)
        {
          if ((int) plotLandUse2.LandUse.Id == (int) landUse1.Id)
          {
            plotLandUse1 = plotLandUse2;
            ++plotLandUse1.PercentOfPlot;
            break;
          }
        }
        if (plotLandUse1 == null)
          plot.PlotLandUses.Add(new PlotLandUse()
          {
            Plot = plot,
            LandUse = landUse1,
            PercentOfPlot = (short) 1
          });
        if (StreetsConversion.config.SpListMap[strTree.SpCode.Code] != null)
        {
          Eco.Domain.v6.Condition condition1 = (Eco.Domain.v6.Condition) null;
          foreach (Eco.Domain.v6.Condition condition2 in (IEnumerable<Eco.Domain.v6.Condition>) v6Year.Conditions)
          {
            if (condition2.Id == strTree.LeavesCondition.Id.Code)
            {
              condition1 = condition2;
              break;
            }
          }
          Tree v6Tree = new Tree()
          {
            Plot = plot,
            PlotLandUse = plotLandUse1,
            IPED = new Eco.Domain.v6.IPED()
            {
              TSDieback = strTree.IPED.TSDieback,
              TSEpiSprout = strTree.IPED.TSEpiSprout,
              TSWiltFoli = strTree.IPED.TSWiltFoli,
              TSEnvStress = strTree.IPED.TSEnvStress,
              TSHumStress = strTree.IPED.TSHumStress,
              TSNotes = strTree.IPED.TSNotes,
              FTChewFoli = strTree.IPED.FTChewFoli,
              FTDiscFoli = strTree.IPED.FTDiscFoli,
              FTAbnFoli = strTree.IPED.FTAbnFoli,
              FTInsectSigns = strTree.IPED.FTInsectSigns,
              FTFoliAffect = strTree.IPED.FTFoliAffect,
              FTNotes = strTree.IPED.FTNotes,
              BBInsectSigns = strTree.IPED.BBInsectSigns,
              BBInsectPres = strTree.IPED.BBInsectPres,
              BBDiseaseSigns = strTree.IPED.BBDiseaseSigns,
              BBProbLoc = strTree.IPED.BBProbLoc,
              BBAbnGrowth = strTree.IPED.BBAbnGrowth,
              BBNotes = strTree.IPED.BBNotes,
              Pest = strTree.IPED.Pest
            },
            Id = num1,
            Species = StreetsConversion.config.SpListMap[strTree.SpCode.Code],
            TreeHeight = -1f,
            Crown = new Crown()
            {
              BaseHeight = -1f,
              TopHeight = -1f,
              WidthEW = -1f,
              WidthNS = -1f,
              LightExposure = CrownLightExposure.FourSides,
              Condition = condition1
            },
            CityManaged = strTree.CityManaged,
            Street = street1,
            Address = strTree.StreetNumber,
            LocNo = strTree.LocNo,
            Latitude = new double?(strTree.Latitude),
            Longitude = new double?(strTree.Longitude),
            SurveyDate = strTree.SurveyDate,
            NoteThisTree = strTree.NoteThisTree,
            Comments = strTree.Comments
          };
          v6Tree.Stems.Add(new Stem()
          {
            Tree = v6Tree,
            Id = 1,
            Diameter = strTree.DBH <= 500.0 ? strTree.DBH : 500.0,
            DiameterHeight = strTree.Project.EnglishUnits ? 4.5 : 1.37,
            Measured = true
          });
          StreetsConversion.convertLookups(strTree, v6Tree);
          plot.Trees.Add(v6Tree);
        }
        else
        {
          PlantingSiteType plantingSiteType1 = (PlantingSiteType) null;
          foreach (PlantingSiteType plantingSiteType2 in (IEnumerable<PlantingSiteType>) v6Year.PlantingSiteTypes)
          {
            if (plantingSiteType2.Description == strTree.SpCode.ScientificName)
            {
              plantingSiteType1 = plantingSiteType2;
              break;
            }
          }
          plot.PlantingSites.Add(new PlantingSite()
          {
            Id = num1,
            Plot = plot,
            PlantingSiteType = plantingSiteType1,
            PlotLandUse = plotLandUse1,
            Street = street1,
            StreetAddress = strTree.StreetNumber,
            xCoordinate = new double?(strTree.Latitude),
            yCoordinate = new double?(strTree.Longitude)
          });
        }
      }
      foreach (Plot plot in (IEnumerable<Plot>) v6Year.Plots)
      {
        int num2 = 0;
        Dictionary<PlotLandUse, double> dictionary2 = new Dictionary<PlotLandUse, double>();
        foreach (PlotLandUse plotLandUse in (IEnumerable<PlotLandUse>) plot.PlotLandUses)
        {
          double num3 = 100.0 * (double) plotLandUse.PercentOfPlot / (double) (plot.Trees.Count + plot.PlantingSites.Count);
          num2 += (int) num3;
          dictionary2.Add(plotLandUse, num3 - (double) (int) num3);
          plotLandUse.PercentOfPlot = (short) num3;
        }
        for (short index = 0; (int) index < 100 - num2; ++index)
        {
          foreach (PlotLandUse plotLandUse in (IEnumerable<PlotLandUse>) plot.PlotLandUses)
          {
            if (dictionary2.ContainsKey(plotLandUse) && dictionary2[plotLandUse] == dictionary2.Values.Max())
            {
              ++plotLandUse.PercentOfPlot;
              dictionary2.Remove(plotLandUse);
              break;
            }
          }
        }
      }
    }

    private static void convertLookups(InventoryItem strTree, Tree v6Tree)
    {
      int num1;
      try
      {
        num1 = strTree.SiteType.Id.Code;
      }
      catch (Exception ex)
      {
        num1 = 0;
      }
      foreach (Eco.Domain.v6.SiteType siteType in (IEnumerable<Eco.Domain.v6.SiteType>) v6Tree.Plot.Year.SiteTypes)
      {
        if (siteType.Id == num1)
        {
          v6Tree.SiteType = siteType;
          break;
        }
      }
      int num2;
      try
      {
        num2 = strTree.LocSite.Id.Code;
      }
      catch (Exception ex)
      {
        num2 = 0;
      }
      foreach (Eco.Domain.v6.LocSite locSite in (IEnumerable<Eco.Domain.v6.LocSite>) v6Tree.Plot.Year.LocSites)
      {
        if (locSite.Id == num2)
        {
          v6Tree.LocSite = locSite;
          break;
        }
      }
      int num3;
      try
      {
        num3 = strTree.MaintRec.Id.Code;
      }
      catch (Exception ex)
      {
        num3 = 0;
      }
      foreach (Eco.Domain.v6.MaintRec maintRec in (IEnumerable<Eco.Domain.v6.MaintRec>) v6Tree.Plot.Year.MaintRecs)
      {
        if (maintRec.Id == num3)
        {
          v6Tree.MaintRec = maintRec;
          break;
        }
      }
      int num4;
      try
      {
        num4 = strTree.MaintTask.Id.Code;
      }
      catch (Exception ex)
      {
        num4 = 0;
      }
      foreach (Eco.Domain.v6.MaintTask maintTask in (IEnumerable<Eco.Domain.v6.MaintTask>) v6Tree.Plot.Year.MaintTasks)
      {
        if (maintTask.Id == num4)
        {
          v6Tree.MaintTask = maintTask;
          break;
        }
      }
      int num5;
      try
      {
        num5 = strTree.SidewalkDamage.Id.Code;
      }
      catch (Exception ex)
      {
        num5 = 0;
      }
      foreach (Eco.Domain.v6.Sidewalk sidewalkDamage in (IEnumerable<Eco.Domain.v6.Sidewalk>) v6Tree.Plot.Year.SidewalkDamages)
      {
        if (sidewalkDamage.Id == num5)
        {
          v6Tree.SidewalkDamage = sidewalkDamage;
          break;
        }
      }
      int num6;
      try
      {
        num6 = strTree.WireConflict.Id.Code;
      }
      catch (Exception ex)
      {
        num6 = 0;
      }
      foreach (Eco.Domain.v6.WireConflict wireConflict in (IEnumerable<Eco.Domain.v6.WireConflict>) v6Tree.Plot.Year.WireConflicts)
      {
        if (wireConflict.Id == num6)
        {
          v6Tree.WireConflict = wireConflict;
          break;
        }
      }
      int num7;
      try
      {
        num7 = strTree.OtherOne.Id.Code;
      }
      catch (Exception ex)
      {
        num7 = 0;
      }
      foreach (Eco.Domain.v6.OtherOne otherOne in (IEnumerable<Eco.Domain.v6.OtherOne>) v6Tree.Plot.Year.OtherOnes)
      {
        if (otherOne.Id == num7)
        {
          v6Tree.OtherOne = otherOne;
          break;
        }
      }
      int num8;
      try
      {
        num8 = strTree.OtherOne.Id.Code;
      }
      catch (Exception ex)
      {
        num8 = 0;
      }
      foreach (Eco.Domain.v6.OtherTwo otherTwo in (IEnumerable<Eco.Domain.v6.OtherTwo>) v6Tree.Plot.Year.OtherTwos)
      {
        if (otherTwo.Id == num8)
        {
          v6Tree.OtherTwo = otherTwo;
          break;
        }
      }
      int num9;
      try
      {
        num9 = strTree.OtherThree.Id.Code;
      }
      catch (Exception ex)
      {
        num9 = 0;
      }
      foreach (Eco.Domain.v6.OtherThree otherThree in (IEnumerable<Eco.Domain.v6.OtherThree>) v6Tree.Plot.Year.OtherThrees)
      {
        if (otherThree.Id == num9)
        {
          v6Tree.OtherThree = otherThree;
          break;
        }
      }
    }
  }
}
