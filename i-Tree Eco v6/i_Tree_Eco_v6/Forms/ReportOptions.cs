// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ReportOptions
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace i_Tree_Eco_v6.Forms
{
  public class ReportOptions
  {
    private Dictionary<string, ReportCondition> _dReportConditions;

    public ReportOptions(Year y)
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ProjectOverviewForm));
      Project project = y.Series.Project;
      this._dReportConditions = new Dictionary<string, ReportCondition>();
      PropertyEqCondition<Year> propertyEqCondition1 = new PropertyEqCondition<Year>(y, "RecordOtherOne", (object) true);
      propertyEqCondition1.Option = componentResourceManager.GetString("chkOtherOne.Text");
      ConditionBase conditionBase1 = (ConditionBase) propertyEqCondition1;
      PropertyEqCondition<Year> propertyEqCondition2 = new PropertyEqCondition<Year>(y, "RecordOtherTwo", (object) true);
      propertyEqCondition2.Option = componentResourceManager.GetString("chkOtherTwo.Text");
      ConditionBase conditionBase2 = (ConditionBase) propertyEqCondition2;
      PropertyEqCondition<Year> propertyEqCondition3 = new PropertyEqCondition<Year>(y, "RecordOtherThree", (object) true);
      propertyEqCondition3.Option = componentResourceManager.GetString("chkOtherThree.Text");
      ConditionBase conditionBase3 = (ConditionBase) propertyEqCondition3;
      ConditionBase conditionBase4 = (ConditionBase) new OrCondition()
      {
        Conditions = new List<ConditionBase>()
        {
          conditionBase1,
          conditionBase2,
          conditionBase3
        }
      };
      PropertyEqCondition<Year> propertyEqCondition4 = new PropertyEqCondition<Year>(y, "RecordMaintRec", (object) true);
      propertyEqCondition4.Option = componentResourceManager.GetString("chkMaintRec.Text");
      ConditionBase conditionBase5 = (ConditionBase) propertyEqCondition4;
      PropertyEqCondition<Year> propertyEqCondition5 = new PropertyEqCondition<Year>(y, "RecordMaintTask", (object) true);
      propertyEqCondition5.Option = componentResourceManager.GetString("chkMaintTask.Text");
      ConditionBase conditionBase6 = (ConditionBase) propertyEqCondition5;
      PropertyEqCondition<Year> propertyEqCondition6 = new PropertyEqCondition<Year>(y, "RecordWireConflict", (object) true);
      propertyEqCondition6.Option = componentResourceManager.GetString("chkOverheadUtilityConflict.Text");
      ConditionBase conditionBase7 = (ConditionBase) propertyEqCondition6;
      PropertyEqCondition<Year> propertyEqCondition7 = new PropertyEqCondition<Year>(y, "RecordSidewalk", (object) true);
      propertyEqCondition7.Option = componentResourceManager.GetString("chkSidewalkConflict.Text");
      ConditionBase conditionBase8 = (ConditionBase) propertyEqCondition7;
      PropertyEqCondition<Year> propertyEqCondition8 = new PropertyEqCondition<Year>(y, "RecordShrub", (object) true);
      propertyEqCondition8.Option = componentResourceManager.GetString("chkShrubs.Text");
      ConditionBase conditionBase9 = (ConditionBase) propertyEqCondition8;
      PropertyEqCondition<Year> propertyEqCondition9 = new PropertyEqCondition<Year>(y, "RecordPercentShrub", (object) true);
      propertyEqCondition9.Option = componentResourceManager.GetString("chkPercentShrubCover.Text");
      ConditionBase conditionBase10 = (ConditionBase) propertyEqCondition9;
      PropertyEqCondition<Year> propertyEqCondition10 = new PropertyEqCondition<Year>(y, "RecordGroundCover", (object) true);
      propertyEqCondition10.Option = componentResourceManager.GetString("chkGroundCover.Text");
      ConditionBase conditionBase11 = (ConditionBase) propertyEqCondition10;
      PropertyEqCondition<Year> propertyEqCondition11 = new PropertyEqCondition<Year>(y, "RecordIPED", (object) true);
      propertyEqCondition11.Option = componentResourceManager.GetString("chkIPED.Text");
      ConditionBase conditionBase12 = (ConditionBase) propertyEqCondition11;
      PropertyEqCondition<Year> propertyEqCondition12 = new PropertyEqCondition<Year>(y, "RecordEnergy", (object) true);
      propertyEqCondition12.Option = componentResourceManager.GetString("chkEnergyEffect.Text");
      ConditionBase conditionBase13 = (ConditionBase) propertyEqCondition12;
      PropertyEqCondition<Year> propertyEqCondition13 = new PropertyEqCondition<Year>(y, "RecordStreetTree", (object) true);
      propertyEqCondition13.Option = componentResourceManager.GetString("chkSiteType.Text");
      ConditionBase conditionBase14 = (ConditionBase) propertyEqCondition13;
      PropertyEqCondition<Year> propertyEqCondition14 = new PropertyEqCondition<Year>(y, "RecordStrata", (object) true);
      propertyEqCondition14.Option = componentResourceManager.GetString("chkStrata.Text");
      ConditionBase conditionBase15 = (ConditionBase) propertyEqCondition14;
      PropertyEqCondition<Year> propertyEqCondition15 = new PropertyEqCondition<Year>(y, "RecordLanduse", (object) true);
      propertyEqCondition15.Option = componentResourceManager.GetString("chkLandUseTree.Text");
      ConditionBase conditionBase16 = (ConditionBase) propertyEqCondition15;
      PropertyNotNullCondition<Year> notNullCondition = new PropertyNotNullCondition<Year>(y, "MgmtStyle");
      notNullCondition.Option = componentResourceManager.GetString("chkPublic.Text");
      ConditionBase conditionBase17 = (ConditionBase) notNullCondition;
      PropertyEqCondition<Project> propertyEqCondition16 = new PropertyEqCondition<Project>(project, "NationCode", (object) "001");
      propertyEqCondition16.Option = "Location";
      ConditionBase conditionBase18 = (ConditionBase) propertyEqCondition16;
      ConditionBase conditionBase19 = (ConditionBase) new AndCondition()
      {
        Conditions = new List<ConditionBase>()
        {
          conditionBase18,
          (ConditionBase) new NotCondition((ConditionBase) new OrCondition()
          {
            Conditions = new List<ConditionBase>()
            {
              (ConditionBase) new PropertyEqCondition<Project>(project, "PrimaryPartitionCode", (object) "02"),
              (ConditionBase) new PropertyEqCondition<Project>(project, "PrimaryPartitionCode", (object) "15"),
              (ConditionBase) new PropertyEqCondition<Project>(project, "PrimaryPartitionCode", (object) "72"),
              (ConditionBase) new PropertyEqCondition<Project>(project, "PrimaryPartitionCode", (object) "66"),
              (ConditionBase) new PropertyEqCondition<Project>(project, "PrimaryPartitionCode", (object) "78")
            }
          })
        }
      };
      ConditionBase conditionBase20 = (ConditionBase) new OrCondition()
      {
        Conditions = new List<ConditionBase>()
        {
          conditionBase18,
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "002"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "230"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "021"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "107"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "229"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "222"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "212"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "192"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "190"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "189"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "188"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "178"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "173"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "172"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "165"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "162"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "148"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "147"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "142"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "140"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "128"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "122"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "121"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "119"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "113"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "095"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "085"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "074"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "073"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "265"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "069"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "053"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "052"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "047"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "040"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "039"),
          (ConditionBase) new PropertyEqCondition<Project>(project, "NationCode", (object) "028")
        }
      };
      ConditionBase conditionBase21 = (ConditionBase) new NotCondition((ConditionBase) new PropertyEqCondition<YearLocationData>(y.YearLocationData.FirstOrDefault<YearLocationData>(), "PollutionYear", (object) (short) -1));
      ReportCondition reportCondition1 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase1
        }
      };
      ReportCondition reportCondition2 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase2
        }
      };
      ReportCondition reportCondition3 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase3
        }
      };
      ReportCondition reportCondition4 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase4
        }
      };
      ReportCondition reportCondition5 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase5
        }
      };
      ReportCondition reportCondition6 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase6
        }
      };
      ReportCondition reportCondition7 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase7
        }
      };
      ReportCondition reportCondition8 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase8
        }
      };
      ReportCondition reportCondition9 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          (ConditionBase) new OrCondition()
          {
            Conditions = new List<ConditionBase>()
            {
              conditionBase5,
              conditionBase6,
              conditionBase8,
              conditionBase7
            }
          }
        }
      };
      ReportCondition reportCondition10 = new ReportCondition()
      {
        IsAvailable = y.Series.IsSample,
        Conditions = new List<ConditionBase>()
        {
          conditionBase10,
          conditionBase9
        }
      };
      ReportCondition reportCondition11 = new ReportCondition()
      {
        IsAvailable = y.Series.IsSample,
        Conditions = new List<ConditionBase>()
        {
          conditionBase11
        }
      };
      ReportCondition reportCondition12 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase12
        }
      };
      ReportCondition reportCondition13 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase13
        }
      };
      ReportCondition reportCondition14 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase14
        }
      };
      ReportCondition reportCondition15 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase15
        }
      };
      ReportCondition reportCondition16 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase16
        }
      };
      ReportCondition reportCondition17 = new ReportCondition()
      {
        IsAvailable = true,
        Conditions = new List<ConditionBase>()
        {
          conditionBase17
        }
      };
      ReportCondition reportCondition18 = new ReportCondition()
      {
        IsAvailable = y.Series.IsSample && conditionBase18.Test(),
        Conditions = new List<ConditionBase>()
        {
          conditionBase10,
          conditionBase11
        }
      };
      ReportCondition reportCondition19 = new ReportCondition()
      {
        IsAvailable = conditionBase20.Test(),
        Conditions = new List<ConditionBase>()
        {
          conditionBase15
        }
      };
      ReportCondition reportCondition20 = new ReportCondition()
      {
        IsAvailable = conditionBase19.Test() && conditionBase21.Test(),
        Conditions = new List<ConditionBase>()
        {
          conditionBase15
        }
      };
      ReportCondition reportCondition21 = new ReportCondition()
      {
        IsAvailable = y.Series.IsSample,
        Conditions = new List<ConditionBase>()
      };
      ReportCondition reportCondition22 = new ReportCondition()
      {
        IsAvailable = y.YearLocationData.FirstOrDefault<YearLocationData>() != null && conditionBase21.Test(),
        Conditions = new List<ConditionBase>()
      };
      ReportCondition reportCondition23 = new ReportCondition()
      {
        IsAvailable = y.Series.IsSample && y.YearLocationData.FirstOrDefault<YearLocationData>() != null && conditionBase21.Test(),
        Conditions = new List<ConditionBase>()
        {
          conditionBase9,
          conditionBase10
        }
      };
      ReportCondition reportCondition24 = new ReportCondition()
      {
        IsAvailable = y.Series.IsSample && conditionBase19.Test() && conditionBase21.Test(),
        Conditions = new List<ConditionBase>()
        {
          conditionBase11
        }
      };
      ReportCondition reportCondition25 = new ReportCondition()
      {
        IsAvailable = y.Series.IsSample && conditionBase19.Test() && conditionBase21.Test(),
        Conditions = new List<ConditionBase>()
        {
          conditionBase9,
          conditionBase10
        }
      };
      ReportCondition reportCondition26 = new ReportCondition()
      {
        IsAvailable = y.Series.IsSample && conditionBase19.Test() && conditionBase21.Test(),
        Conditions = new List<ConditionBase>()
        {
          (ConditionBase) new OrCondition()
          {
            Conditions = new List<ConditionBase>()
            {
              (ConditionBase) new AndCondition()
              {
                Conditions = new List<ConditionBase>()
                {
                  conditionBase9,
                  conditionBase10
                }
              },
              conditionBase11
            }
          }
        }
      };
      this._dReportConditions["rmAirQualityHealthImpactsAndValuesReports"] = reportCondition20;
      this._dReportConditions["airQualityHealthImpactsandValuesbyTrees"] = reportCondition20;
      this._dReportConditions["airQualityHealthImpactsandValuesbyShrubs"] = reportCondition25;
      this._dReportConditions["airQualityHealthImpactsandValuesbyTreesandShrubs"] = reportCondition25;
      this._dReportConditions["airQualityHealthImpactsandValuesbyGrass"] = reportCondition24;
      this._dReportConditions["airQualityHealthImpactsandValuesCombined"] = reportCondition26;
      this._dReportConditions["rbMDDTreeBenefitsPollutionRemoval"] = reportCondition22;
      this._dReportConditions["PollutionRemovalByTreesandShrubs"] = reportCondition22;
      this._dReportConditions["monthlyPollutantRemovalByTreesAndShrubsMenuItem"] = reportCondition22;
      this._dReportConditions["monthlyPollutantRemovalByTreesAndShrubsChartMenuItem"] = reportCondition22;
      this._dReportConditions["pollutionEffectsMenuItem"] = reportCondition22;
      this._dReportConditions["AirPollutantFluxDryDepositionLabel"] = reportCondition22;
      this._dReportConditions["rbWeatherDryDepTree"] = reportCondition22;
      this._dReportConditions["rbWeatherAirQualImprovementTree"] = reportCondition22;
      this._dReportConditions["AirQualityImprovementLabel"] = reportCondition22;
      this._dReportConditions["rbWeatherAirPollutantConcentrationUGM3"] = reportCondition22;
      this._dReportConditions["ForecastPollutantLabel"] = reportCondition22;
      this._dReportConditions["ForecastPollutantSummary"] = reportCondition22;
      this._dReportConditions["ForecastPollutantCO"] = reportCondition22;
      this._dReportConditions["ForecastPollutantNO2"] = reportCondition22;
      this._dReportConditions["ForecastPollutantO3"] = reportCondition22;
      this._dReportConditions["ForecastPollutantSO2"] = reportCondition22;
      this._dReportConditions["ForecastPollutantPM25"] = reportCondition22;
      this._dReportConditions["ForecastPollutantPM10"] = reportCondition22;
      this._dReportConditions["OtherLabel"] = reportCondition4;
      this._dReportConditions["rbFieldOne"] = reportCondition1;
      this._dReportConditions["rbFieldOneBySpecies"] = reportCondition1;
      this._dReportConditions["rbFieldOneReport"] = reportCondition1;
      this._dReportConditions["rbFieldTwo"] = reportCondition2;
      this._dReportConditions["rbFieldTwoBySpecies"] = reportCondition2;
      this._dReportConditions["rbFieldTwoReport"] = reportCondition2;
      this._dReportConditions["rbFieldThree"] = reportCondition3;
      this._dReportConditions["rbFieldThreeBySpecies"] = reportCondition3;
      this._dReportConditions["rbFieldThreeReport"] = reportCondition3;
      this._dReportConditions["MaintenanceLabel"] = reportCondition9;
      this._dReportConditions["rbMaintRec"] = reportCondition5;
      this._dReportConditions["rbMaintRecBySpecies"] = reportCondition5;
      this._dReportConditions["rbMaintRecReport"] = reportCondition5;
      this._dReportConditions["rbMaintTask"] = reportCondition6;
      this._dReportConditions["rbMaintTaskBySpecies"] = reportCondition6;
      this._dReportConditions["rbMaintTaskReport"] = reportCondition6;
      this._dReportConditions["rbMaintUtilityConflicts"] = reportCondition7;
      this._dReportConditions["rbUtilityConflictsBySpecies"] = reportCondition7;
      this._dReportConditions["rbUtilityConflictsReport"] = reportCondition7;
      this._dReportConditions["rbSidewalkConflicts"] = reportCondition8;
      this._dReportConditions["rbSidewalkConflictsBySpecies"] = reportCondition8;
      this._dReportConditions["rbSidewalkConflictsReport"] = reportCondition8;
      this._dReportConditions["rbWeatherAirQualImprovementShrub"] = reportCondition10;
      this._dReportConditions["rbWeatherDryDepShrub"] = reportCondition23;
      this._dReportConditions["leafAreaAndBiomassOfShrubsByStrataMenuItem"] = reportCondition10;
      this._dReportConditions["rbEvaporationByShrubs"] = reportCondition10;
      this._dReportConditions["rbWaterInterceptionByShrubs"] = reportCondition10;
      this._dReportConditions["rbAvoidedRunoffByShrubs"] = reportCondition10;
      this._dReportConditions["rbPotentialEvapotranspirationByShrubs"] = reportCondition10;
      this._dReportConditions["rbTranspirationByShrub"] = reportCondition10;
      this._dReportConditions["rbIsopreneByShrubs"] = reportCondition10;
      this._dReportConditions["rbMonoterpeneByShrubs"] = reportCondition10;
      this._dReportConditions["GroundCoverCompositionLabel"] = reportCondition11;
      this._dReportConditions["groundCoverCompositionByStrataMenuItem"] = reportCondition11;
      this._dReportConditions["PrimaryPestLabel"] = reportCondition12;
      this._dReportConditions["primaryPestSummaryOfTreesByStrataButton"] = reportCondition12;
      this._dReportConditions["primaryPestDetailsOfTreesByStrataButton"] = reportCondition12;
      this._dReportConditions["SignAndSymptomLabel"] = reportCondition12;
      this._dReportConditions["signSymptomOverviewBySpeciesButton"] = reportCondition12;
      this._dReportConditions["signSymptomDetailsSummaryBySpeciesButton"] = reportCondition12;
      this._dReportConditions["signSymptomDetailsCompleteBySpeciesButton"] = reportCondition12;
      this._dReportConditions["signSymptomOverviewByStrataButton"] = reportCondition12;
      this._dReportConditions["signSymptomDetailsSummaryByStrataButton"] = reportCondition12;
      this._dReportConditions["signSymptomDetailsCompleteByStrataButton"] = reportCondition12;
      this._dReportConditions["signSymptomReviewOfTreesButton"] = reportCondition12;
      this._dReportConditions["PestReviewLabel"] = reportCondition12;
      this._dReportConditions["pestReviewOfTreesButton"] = reportCondition12;
      this._dReportConditions["EnergyEffectsLabel"] = reportCondition13;
      this._dReportConditions["energyEffectsOfTreesMenuItem"] = reportCondition13;
      this._dReportConditions["energyEffectsMenuItem"] = reportCondition13;
      this._dReportConditions["rbMDDTreeBenefitsEnergyEffects"] = reportCondition13;
      this._dReportConditions["rbMDDTreeEnergyAvoidedPollutants"] = reportCondition13;
      this._dReportConditions["rbStreetTreesByStrata"] = reportCondition14;
      this._dReportConditions["rbPublicPrivateByStrata"] = reportCondition17;
      this._dReportConditions["numberOfTreesPerUnitAreaByStrataChartMenuItem"] = reportCondition15;
      this._dReportConditions["leafAreaOfTreesPerUnitAreaByStrataChartMenuItem"] = reportCondition15;
      this._dReportConditions["carbonStorageOfTreesPerUnitAreaByStrataChartMenuItem"] = reportCondition15;
      this._dReportConditions["annualCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem"] = reportCondition15;
      this._dReportConditions["oxygenProductionPerUnitAreaByStrataChartMenuItem"] = reportCondition15;
      this._dReportConditions["AreaLfBiomassByStrata"] = reportCondition15;
      this._dReportConditions["PctCoverByStrata"] = reportCondition15;
      this._dReportConditions["PctCoverTotal"] = reportCondition15;
      this._dReportConditions["AreaLfBiomassTotal"] = reportCondition15;
      this._dReportConditions["rbMDDCompositionOfPlots"] = reportCondition21;
      this._dReportConditions["rbMDDMiscPlotComments"] = reportCondition21;
      this._dReportConditions["rbMDDMiscShrubComments"] = reportCondition10;
      this._dReportConditions["AnnualNetCarbonSequestrationOfTreesLabel"] = reportCondition21;
      this._dReportConditions["AnnualNetCarbonSequestrationOfTreesBySpeciesMenuItem"] = reportCondition21;
      this._dReportConditions["AnnualNetCarbonSequestrationOfTreesByStrataChartMenuItem"] = reportCondition21;
      this._dReportConditions["AnnualNetCarbonSequestrationOfTreesPerUnitAreaByStrataChartMenuItem"] = reportCondition21;
      this._dReportConditions["LandUseCompositionLabel"] = reportCondition16;
      this._dReportConditions["landUseCompositionByStrataMenuItem"] = reportCondition16;
      this._dReportConditions["WildLifeSuitabilityLabel"] = reportCondition18;
      this._dReportConditions["WildLifeSuitabilityByPlots"] = reportCondition18;
      this._dReportConditions["WildLifeSuitabilityByStrata"] = reportCondition18;
      this._dReportConditions["UVEffectsOfTreesLabel"] = reportCondition19;
      this._dReportConditions["UVEffectsOfTreesByStrata"] = reportCondition19;
      this._dReportConditions["rbUVIndex"] = reportCondition19;
      this._dReportConditions["rbUVIndexReductionLabel"] = reportCondition19;
      this._dReportConditions["rbUVIndexReductionByTreesForUnderTrees"] = reportCondition19;
      this._dReportConditions["rbUVIndexReductionByTreesForOverall"] = reportCondition19;
      this._dReportConditions["PollutionRemovalByGrass"] = reportCondition24;
      this._dReportConditions["monthlyPollutantRemovalByGrassMenuItem"] = reportCondition24;
      this._dReportConditions["monthlyPollutantRemovalByGrassChartMenuItem"] = reportCondition24;
      this._dReportConditions["rbWeatherAirQualImprovementGrass"] = reportCondition24;
      this._dReportConditions["rbWeatherDryDepGrass"] = reportCondition24;
      this._dReportConditions["airQualityHealthImpactsandValuesbyGrass"] = reportCondition24;
      this._dReportConditions["SpeciesRichness"] = reportCondition21;
      this._dReportConditions["speciesRichnessShannonWienerDiversityIndexMenuItem"] = reportCondition21;
    }

    public ReportCondition GetCondition(string report)
    {
      ReportCondition condition = (ReportCondition) null;
      this._dReportConditions.TryGetValue(report, out condition);
      return condition;
    }
  }
}
