// Decompiled with JetBrains decompiler
// Type: Eco.Util.Queries.Interfaces.IEstimateUtilProvider
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using NHibernate;

namespace Eco.Util.Queries.Interfaces
{
  public interface IEstimateUtilProvider
  {
    IQuery GetEstimateValue(string tableName, string c1, string c2);

    IQuery GetTotalsSumEstimateValueByClassifier(string tableName, string c1, string c2);

    IQuery GetTotalsExoticToContinent(string tableName, string c1, string c2);

    IQuery GetEstimatedValueSum(string tableName, string c1);

    IQuery GetMultipleFieldsData(string tableName, string fields);

    IQuery GetSignificantMultipleFieldsData(string tableName, string c1, string c2);

    IQuery GetSignificantEstimateValues(string tableName, string c1);

    IQuery GetSignificantEstimateValuesGroupedBy(string tableName, string c1, string c2);

    IQuery GetEstimatedValues(string tableName, string c1);

    IQuery GetEstimateValuesForClassifier(string tableName, string classifier);

    IQuery GetSignificantTotalsEstimateValues(string tableName, string c1, string c2);

    IQuery GetMultipleFieldsData2(string tableName, string[] columns);

    IQuery GetEstimatedPollution(string tableName, string cond1);

    IQuery GetPollutantByPlantCategory(
      string plantCategory,
      string column1,
      double multiplier1,
      string column2,
      double multiplier2);

    IQuery GetUVIndexReduction(string column1);

    IQuery GetHourlyUFOREBResults(string column);

    IQuery GetLanduseLeafAreas(string table, string strata, string species);

    IQuery GetTopPollutant(string treeShrubCondition);

    IQuery GetHourlyPollution(string para, string treeShrubCondition, double valueMultiplier);

    IQuery GetEstimatedValues(string tableName, string col, string classifier);

    IQuery GetEstimateValuesWithSE(string tableName, string c1);

    IQuery GetSpeciesByStratum(string tableName, string c1, string c2);

    IQuery GetEstimateValuesForClassifier2(string tableName, string c1, string c2);

    IQuery GetEstimateValuesWithSEAndTotals(string tableName, string c1);

    IQuery GetSpeciesCountsWithLeafArea(string tableName, string c1);

    IQuery GetEstimatedSpeciesTotalsValuesWithSE(string tableName, string c1);

    IQuery GetEstimatedSpeciesByStratumValuesWithSE(string tableName, string c1, string c2);

    IQuery GetEstimatedStratumValues(string tableName, string c1);

    IQuery GetEstimatedPollutantsValues(string tableName, string c1);

    IQuery GetEstimatedPollutantsValuesByStratum(string tableName, string c1, string c2);

    IQuery GetGroundCoversExcludingTreesAndShrubs(string tableName, string c1, string c2);

    IQuery GetEstimatedPollutantsValuesBySpecies(string tableName, string c1);

    IQuery GetEstimatedPollutantsByStratumValues(string tableName, string c1, string c2);

    IQuery GetEstimatedPollutantsTotalValues(string tableName, string c1);

    IQuery GetEstimatedValues2(string tableName, string c1, string c2);

    IQuery GetAverageCondition(string tableName, string fields, string dieback);

    IQuery GetStudyAreaAverageCondition(
      string tableName,
      string c1,
      string c2,
      string dieback);

    IQuery GetStudyAreaValues(string tableName, string strataStudyArea, string speciesTotal);

    IQuery GetPollutantRemovalMonthlyValues(string c1);

    IQuery GetPollutionRemovalByMonthAndPollutant(string c1);

    IQuery GetPollutionDataCount(string c1);

    IQuery GetCarbonSum(string c1);

    IQuery GetAvailableBenefitTypes(string c1);

    IQuery GetCarbonData(string c1);

    IQuery GetAirQualityHealthImpactsAndValuesCombined(string c1);

    IQuery GetOxygenProductionOfTrees(string tableName, string c1, string c2);

    IQuery GetPestEstimateValues(string tableName, string c1);

    IQuery GetStratumTreePestEstimateValues(string tableName, string c1);

    IQuery GetSASEstimateValues(string tableName, string c1);

    IQuery GetSASEstimateValues(string tableName, string c1, string c2);

    IQuery MovingEcoForecastCohortResults(string fromDatabase);

    IQuery MovingEcoForecastPollutantResults(string fromDatabase);
  }
}
