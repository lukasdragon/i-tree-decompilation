// Decompiled with JetBrains decompiler
// Type: Eco.Util.Queries.SQLite.SASUtilProvider
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Util.Queries.Interfaces;
using NHibernate;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Eco.Util.Queries.SQLite
{
  internal class SASUtilProvider : SASIUtilProvider
  {
    private SASIQuerySupplier _SASqueryProvider;

    public SASUtilProvider(SASIQuerySupplier SASqueryProvider) => this._SASqueryProvider = SASqueryProvider;

    public IQuery GetSQLQueryClearContentOfTable(string tableName) => (IQuery) this._SASqueryProvider.Session.CreateSQLQuery(string.Format("DELETE FROM [{0}]", (object) tableName));

    public IQuery GetSQLQueryClearContentOfProject(string tableName) => (IQuery) this._SASqueryProvider.Session.CreateSQLQuery(string.Format("DELETE FROM [{0}] WHERE YearGuid = :yearGuid", (object) tableName));

    public IQuery GetSQLQueryRemoveNullRecordsOfTable(string tableName, string fieldName) => (IQuery) this._SASqueryProvider.Session.CreateSQLQuery(string.Format("DELETE FROM [{0}] WHERE [{1}] is null", (object) tableName, (object) fieldName));

    public IQuery GetSQLQueryDropTable(string tableName) => (IQuery) this._SASqueryProvider.Session.CreateSQLQuery(string.Format("DROP TABLE IF EXISTS [{0}]", (object) tableName));

    public IQuery GetSQLQueryCreateEstimateTable(
      string tableName,
      List<Classifiers> partitionClassifiers)
    {
      string str = "CREATE TABLE [" + tableName + "] ([YearGuid] BLOB,";
      foreach (Classifiers partitionClassifier in partitionClassifiers)
        str = str + "[" + partitionClassifier.ToString() + "] INTEGER NOT NULL DEFAULT 0,";
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery(str + "[EstimateType] INTEGER NOT NULL DEFAULT 0," + "[EquationType] INTEGER NOT NULL DEFAULT 0," + "[EstimateValue] REAL NOT NULL DEFAULT 0," + "[EstimateStandardError] REAL NOT NULL DEFAULT 0," + "[EstimateUnitsId] INTEGER NOT NULL DEFAULT 0)");
    }

    public IQuery GetSQLQuerySelectEstimateTable(
      string tableName,
      List<Classifiers> partitionClassifiers)
    {
      string str = "SELECT ";
      foreach (Classifiers partitionClassifier in partitionClassifiers)
        str = str + partitionClassifier.ToString() + ",";
      ISQLQuery sqlQuery = this._SASqueryProvider.Session.CreateSQLQuery(str + "EstimateValue,EstimateStandardError FROM [" + tableName + "] " + "WHERE YearGuid=:YearGuid AND EstimateType=:EstimateType AND EquationType=:EquationType AND EstimateUnitsId=:EstimateUnitsId");
      foreach (Classifiers partitionClassifier in partitionClassifiers)
        sqlQuery.AddScalar(partitionClassifier.ToString(), (IType) NHibernateUtil.Int32);
      sqlQuery.AddScalar("EstimateValue", (IType) NHibernateUtil.Double).AddScalar("EstimateStandardError", (IType) NHibernateUtil.Double);
      return (IQuery) sqlQuery;
    }

    public IQuery GetSQLQueryInsertEstimateTable(
      string tableName,
      List<Classifiers> partitionClassifiers)
    {
      string str1 = "INSERT INTO [" + tableName + "] (YearGuid";
      foreach (Classifiers partitionClassifier in partitionClassifiers)
        str1 = str1 + ",[" + partitionClassifier.ToString() + "]";
      string str2 = str1 + ",EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId) VALUES (:vYearGuid";
      foreach (Classifiers partitionClassifier in partitionClassifiers)
        str2 = str2 + ", :" + partitionClassifier.ToString();
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery(str2 + ",:EstimateType,:EquationType,:EstimateValue,:EstimateStandardError,:EstimateUnitsId)");
    }

    public IQuery GetSQLQuerySelectYearlyMeanUVReduction(
      string UVReductionDatabaseFolder,
      string dbNameWithoutExtention)
    {
      string str1 = Path.Combine(UVReductionDatabaseFolder, dbNameWithoutExtention + ".db");
      string str2 = "d" + new Random().Next().ToString((IFormatProvider) CultureInfo.InvariantCulture);
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery("\r\n                ATTACH '" + str1 + "' AS " + str2 + ";\r\n                SELECT\r\n                    Landuse, MeanShadedUVPF, MeanShadedUVRed, MeanShadedUVRedPct,\r\n                    MeanAllUVPF, MeanAllUVRed, MeanAllUVRedPct\r\n                FROM " + str2 + ".YearlyMeanUVReduction;\r\n            ").AddScalar("Landuse", (IType) NHibernateUtil.String).AddScalar("MeanShadedUVPF", (IType) NHibernateUtil.Double).AddScalar("MeanShadedUVRed", (IType) NHibernateUtil.Double).AddScalar("MeanShadedUVRedPct", (IType) NHibernateUtil.Double).AddScalar("MeanAllUVPF", (IType) NHibernateUtil.Double).AddScalar("MeanAllUVRed", (IType) NHibernateUtil.Double).AddScalar("MeanAllUVRedPct", (IType) NHibernateUtil.Double);
    }

    public IQuery GetSQLQueryTransferUVIndexReduction(
      string UVReductionDatabaseFolder,
      string dbNameWithoutExtention)
    {
      string str1 = Path.Combine(UVReductionDatabaseFolder, dbNameWithoutExtention + ".db");
      string str2 = "e" + new Random().Next().ToString((IFormatProvider) CultureInfo.InvariantCulture);
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery("\r\n                ATTACH '" + str1 + "' AS " + str2 + ";\r\n                INSERT INTO UVIndexReduction (\r\n                    YearGuid, [TimeStamp], UVIndex, ShadedUVProtectionFactor, ShadedUVReduction, ShadedUVRedutionPercent,\r\n                    OverallUVProtectionFactor, OverallUVReduction, OverallUVReductionPercent\r\n                )\r\n                SELECT \r\n                    :YearGuid, [Date], UVI, ShadedUVPF, ShadedUVRed, ShadedUVRedPct,\r\n                    AllUVPF, AllUVRed, AllUVRedPct\r\n                FROM " + str2 + ".TotalUVReduction;\r\n            ");
    }

    public IQuery GetSQLQueryTransferBenMAP(
      string BenMAPDatabaseFolder,
      string dbNameWithoutExtention)
    {
      string str1 = Path.Combine(BenMAPDatabaseFolder, dbNameWithoutExtention + ".db");
      string str2 = "f" + new Random().Next().ToString((IFormatProvider) CultureInfo.InvariantCulture);
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery("\r\n                ATTACH '" + str1 + "' AS " + str2 + ";\r\n                INSERT INTO BenMapTable (\r\n                    YearGuid, TreeShrub, HealthFactor, NO2Incidence, NO2Value, SO2Incidence,\r\n                    SO2Value, O3Incidence, O3Value, PM25Incidence, PM25Value\r\n                )\r\n                SELECT\r\n                    :YearGuid, TreeShrub, HealthFactor, NO2Incidence, NO2Value, SO2Incidence,\r\n                    SO2Value, O3Incidence, O3Value, PM25Incidence, PM25Value\r\n                FROM " + str2 + ".BenMapTable;\r\n            ");
    }

    public IQuery GetSQLQueryFluxDomainValDomainFrom_08_DomainYearlySums(
      string PollutantsDatabaseFolder,
      string dbNameWithoutExtention)
    {
      string str1 = Path.Combine(PollutantsDatabaseFolder, dbNameWithoutExtention + ".db");
      string str2 = "f_1" + new Random().Next().ToString((IFormatProvider) CultureInfo.InvariantCulture);
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery("\r\n                ATTACH '" + str1 + "' AS " + str2 + ";\r\n                SELECT\r\n                    Pollutant,\r\n                    [FluxDomain (m-tons)] AS FluxDomainMetricTon,\r\n                    [ValDomain ($1000)] AS ValDomainThousands\r\n                FROM " + str2 + ".[08_DomainYearlySums];\r\n            ").AddScalar("Pollutant", (IType) NHibernateUtil.String).AddScalar("FluxDomainMetricTon", (IType) NHibernateUtil.Double).AddScalar("ValDomainThousands", (IType) NHibernateUtil.Double);
    }

    public IQuery GetSQLQueryTransferPollutants(
      string PollutantsDatabaseFolder,
      string dbNameWithoutExtention)
    {
      string str1 = Path.Combine(PollutantsDatabaseFolder, dbNameWithoutExtention + ".db");
      string str2 = "g" + new Random().Next().ToString((IFormatProvider) CultureInfo.InvariantCulture);
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery("\r\n                ATTACH '" + str1 + "' AS " + str2 + ";\r\n                INSERT INTO Pollutants (\r\n                    YearGuid, [Year], JDay, [Month], [Day], [Hour], SiteID, Pollutant, PPM, uGm3,\r\n                    CityArea, TrCovArea, PARWm2, RainMh, TempF, TempK, ActAqImp, Flux, FluxMax,\r\n                    FluxMin, FluxWet, PerAqImp, Trans, [Value], ValueMax, ValueMin, TreeShrub\r\n                )\r\n                SELECT\r\n                    :YearGuid, [Year], JDay, [Month], [Day], [Hour], SiteID, Pollutant, PPM, uGm3,\r\n                    CityArea, TrCovArea, PARWm2, RainMh, TempF, TempK, ActAqImp, Flux, FluxMax,\r\n                    FluxMin, FluxWet, PerAqImp, Trans, [Value], ValueMax, ValueMin, :TreeShrub\r\n                FROM " + str2 + ".Pollutants;\r\n            ");
    }

    public IQuery GetSQLQueryTransferHourlyUFOREBResults(
      string UFOREBdbFolder,
      string dbNameWithoutExtention)
    {
      string str1 = Path.Combine(UFOREBdbFolder, dbNameWithoutExtention + ".db");
      string str2 = "k" + new Random().Next().ToString((IFormatProvider) CultureInfo.InvariantCulture);
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery("\r\n                ATTACH '" + str1 + "' AS " + str2 + ";\r\n                INSERT INTO HourlyUFOREBResults (\r\n                    YearGuid, [TimeStamp], Category, [Isoprene], [Monoterpene]\r\n                )\r\n                SELECT\r\n                    :YearGuid, [TimeStamp], :Category, [Isoprene emitted], [Monoterpene emitted]\r\n                FROM " + str2 + ".HourlyDomainSummary;\r\n            ");
    }

    public IQuery GetSQLQueryAdjustBenMAPResults(string Pollutant) => (IQuery) this._SASqueryProvider.Session.CreateSQLQuery(string.Format("UPDATE BenMapTable SET {0}Incidence = {0}Incidence * :ratio, {0}Value = {0}Value * :ratio WHERE YearGuid = :YearGuid AND {0}Incidence> 0", (object) Pollutant));

    public IQuery GetSQLQuerySelectYearlySummaryByLanduse(
      string UFOREBdbFolder,
      string dbNameWithoutExtention)
    {
      string str1 = Path.Combine(UFOREBdbFolder, dbNameWithoutExtention + ".db");
      string str2 = "h" + new Random().Next().ToString((IFormatProvider) CultureInfo.InvariantCulture);
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery("\r\n                ATTACH '" + str1 + "' AS " + str2 + ";\r\n                SELECT\r\n                    LanduseDesc AS StrataName,\r\n                    [Isoprene emitted] AS Isoprene,\r\n                    [Monoterpene emitted] AS Monoterpene,\r\n                    [Other VOCs emitted] AS VOCother\r\n                FROM " + str2 + ".YearlySummaryByLanduse;\r\n            ").AddScalar("StrataName", (IType) NHibernateUtil.String).AddScalar("Isoprene", (IType) NHibernateUtil.Double).AddScalar("Monoterpene", (IType) NHibernateUtil.Double).AddScalar("VOCother", (IType) NHibernateUtil.Double);
    }

    public IQuery GetSQLQuerySelectYearlySummaryBySpecies(
      string UFOREBdbFolder,
      string dbNameWithoutExtention)
    {
      string str1 = Path.Combine(UFOREBdbFolder, dbNameWithoutExtention + ".db");
      string str2 = "j" + new Random().Next().ToString((IFormatProvider) CultureInfo.InvariantCulture);
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery("\r\n                ATTACH '" + str1 + "' AS " + str2 + ";\r\n                SELECT\r\n                    [Code],\r\n                    sum([Isoprene emitted]) AS Isoprene,\r\n                    sum([Monoterpene emitted]) AS Monoterpene,\r\n                    0.0 AS VOCother\r\n                FROM " + str2 + ".HourlySummaryBySpecies\r\n                GROUP BY [Code];\r\n            ").AddScalar("Code", (IType) NHibernateUtil.String).AddScalar("Isoprene", (IType) NHibernateUtil.Double).AddScalar("Monoterpene", (IType) NHibernateUtil.Double).AddScalar("VOCother", (IType) NHibernateUtil.Double);
    }

    public IQuery GetSQLQuerySelectYearlyInterceptSum(
      string WaterDbFolder,
      string dbNameWithoutExtention)
    {
      string str1 = Path.Combine(WaterDbFolder, dbNameWithoutExtention + ".db");
      string str2 = "m" + new Random().Next().ToString((IFormatProvider) CultureInfo.InvariantCulture);
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery("\r\n                ATTACH '" + str1 + "' AS " + str2 + ";\r\n                SELECT\r\n                    [Veg Intercept no Adjust (m3/yr)] AS Intercepted,\r\n                    [VegIntercept (m3/yr)] AS AvoidedRunoff,\r\n                    [Potential Evaporation (m3/yr)] AS PotentialEvaporation,\r\n                    [Evaporation (m3/yr)] AS Evaporation,\r\n                    [Potential Evapotranspiration (m3/yr)] AS PotentialEvapotranspiration,\r\n                    [Transpiration (m3/yr)] AS Transpiration\r\n                FROM " + str2 + ".[01_InterceptYearlySums];\r\n            ").AddScalar("Intercepted", (IType) NHibernateUtil.Double).AddScalar("AvoidedRunoff", (IType) NHibernateUtil.Double).AddScalar("PotentialEvaporation", (IType) NHibernateUtil.Double).AddScalar("Evaporation", (IType) NHibernateUtil.Double).AddScalar("PotentialEvapotranspiration", (IType) NHibernateUtil.Double).AddScalar("Transpiration", (IType) NHibernateUtil.Double);
    }

    public IQuery GetSQLQueryTransferHourlyHydroResults(
      string WaterDbFolder,
      string dbNameWithoutExtention)
    {
      string str1 = Path.Combine(WaterDbFolder, dbNameWithoutExtention + ".db");
      string str2 = "n" + new Random().Next().ToString((IFormatProvider) CultureInfo.InvariantCulture);
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery("\r\n                ATTACH '" + str1 + "' AS " + str2 + ";\r\n                INSERT INTO HourlyHydroResults (\r\n                    YearGuid, [TimeStamp], Category, Rain, RainVolume, Evaporation,\r\n                    EvaporationVolume, PotentialEvaporation, PotentialEvaporationVolume, \r\n                    Transpiration, TranspirationVolume, PotentialEvapotranspiration,\r\n                    PotentialEvapotranspirationVolume, WaterInterception, WaterInterceptionVolume,\r\n                    AvoidedRunoff, AvoidedRunoffVolume\r\n                )\r\n                SELECT\r\n                    :YearGuid, [TimeStamp], :Category, [Rain (m/h)], [Rain (m3/h)], [Evaporation (m/h)],\r\n                    [Evaporation (m3/h)], [Potential Evaporation (m/h)], [Potential Evaporation (m3/h)],\r\n                    [Transpiration (m/h)], [Transpiration (m3/h)], [Potential Evapotranspiration (m/h)],\r\n                    [Potential Evapotranspiration (m3/h)], [Veg Intercept no Adjust (m/h)], [Veg Intercept no Adjust (m3/h)],\r\n                    [VegIntercept (m/h)], [VegIntercept (m3/h)]\r\n                FROM " + str2 + ".[04_InterceptHourly];\r\n            ");
    }

    public IQuery GetSQLQueryCombineTreeShrubEstimation(
      string leftTableName,
      string rightTableName,
      string targetTable,
      bool leftUnique,
      List<Classifiers> classifiersList,
      List<EstimateTypeEnum> estimateTypeList,
      List<int> estimateUnitList)
    {
      string str1 = "INSERT INTO [{2}] (YearGuid,";
      string str2 = "SELECT [{0}].YearGuid,";
      string str3 = " ON [{0}].YearGuid=[{1}].YearGuid AND ";
      foreach (Classifiers classifiers in classifiersList)
      {
        str1 = str1 + "[" + classifiers.ToString() + "],";
        str2 = str2 + "[{0}].[" + classifiers.ToString() + "],";
        str3 = str3 + " [{0}].[" + classifiers.ToString() + "]=[{1}].[" + classifiers.ToString() + "] AND ";
      }
      string str4 = str1 + "EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId) ";
      string str5 = str2 + "[{0}].EstimateType,[{0}].EquationType,";
      string str6 = (!leftUnique ? str5 + "[{0}].EstimateValue + [{1}].EstimateValue," : str5 + "[{0}].EstimateValue,") + "0,[{0}].EstimateUnitsId ";
      string str7 = str3 + "[{0}].EstimateType=[{1}].EstimateType AND [{0}].EquationType=[{1}].EquationType AND [{0}].EstimateUnitsId=[{1}].EstimateUnitsId ";
      string str8 = !leftUnique ? str6 + " FROM [{0}] INNER JOIN [{1}] " : str6 + " FROM [{0}] LEFT JOIN [{1}] ";
      int num = 1;
      string str9 = " WHERE [{0}].YearGuid=:YearGuid AND [{0}].EquationType=" + num.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND (";
      foreach (EstimateTypeEnum estimateType in estimateTypeList)
      {
        string str10 = str9;
        num = (int) estimateType;
        string str11 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        str9 = str10 + "[{0}].EstimateType=" + str11 + " OR ";
      }
      string str12 = str9.Substring(0, str9.Length - 4) + ") AND (";
      foreach (int estimateUnit in estimateUnitList)
        str12 = str12 + "[{0}].EstimateUnitsId=" + estimateUnit.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " OR ";
      string str13 = str12.Substring(0, str12.Length - 4) + ")";
      if (leftUnique)
        str13 += " AND [{1}].YearGuid IS NULL";
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery(string.Format(str4 + str8 + str7 + str13, (object) leftTableName, (object) rightTableName, (object) targetTable));
    }

    public IQuery GetSQLQueryOfLargeDBHTrees(
      string ClauseOfNonRemoved,
      bool isSampledProject,
      bool isActualDBH)
    {
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery(!isActualDBH ? "SELECT EcoPlots.PlotId, EcoTrees.TreeId, Sum(EcoDBHs.Value * EcoDBHs.Value) AS DBHSquare  FROM ((EcoPlots INNER JOIN EcoTrees ON EcoPlots.PlotKey = EcoTrees.PlotKey) INNER JOIN EcoStems ON EcoTrees.TreeKey = EcoStems.TreeKey) INNER JOIN EcoDBHs ON abs(EcoStems.Diameter - EcoDBHs.DBHId) < 0.1  WHERE EcoDBHs.YearKey= :YearKey AND EcoPlots.YearKey= :YearKey " + (isSampledProject ? "AND EcoPlots.IsComplete=true " : " ") + (string.IsNullOrEmpty(ClauseOfNonRemoved) ? " " : " AND " + ClauseOfNonRemoved) + " GROUP BY EcoPlots.PlotId, EcoTrees.TreeId  HAVING Sum(EcoDBHs.Value*EcoDBHs.Value) > :MaxDbhSqaure" : "SELECT EcoPlots.PlotId, EcoTrees.TreeId, Sum([Diameter]*[Diameter]) AS DBHSquare  FROM (EcoPlots INNER JOIN EcoTrees ON EcoPlots.PlotKey = EcoTrees.PlotKey) INNER JOIN EcoStems ON EcoTrees.TreeKey = EcoStems.TreeKey  WHERE EcoPlots.YearKey= :YearKey " + (isSampledProject ? "AND EcoPlots.IsComplete=true " : " ") + (string.IsNullOrEmpty(ClauseOfNonRemoved) ? " " : " AND " + ClauseOfNonRemoved) + " GROUP BY EcoPlots.PlotId, EcoTrees.TreeId  HAVING Sum([Diameter]*[Diameter]) > :MaxDbhSqaure").AddScalar("PlotId", (IType) NHibernateUtil.Int32).AddScalar("TreeId", (IType) NHibernateUtil.Int32).AddScalar("DBHSquare", (IType) NHibernateUtil.Double);
    }

    public IQuery GetSQLQueryOfPlotsWithSusspectedTreePercent(
      string ClauseOfNonRemoved,
      bool isCrownConditionCollected)
    {
      return (IQuery) this._SASqueryProvider.Session.CreateSQLQuery("SELECT EcoPlots.PlotId, EcoPlots.PercentTreeCover, Count(EcoTrees.TreeKey) AS TreeCount  FROM EcoPlots LEFT JOIN (EcoTrees LEFT JOIN EcoConditions ON EcoTrees.CrownCondition = EcoConditions.ConditionKey)  ON EcoPlots.PlotKey = EcoTrees.PlotKey  WHERE EcoPlots.YearKey= :YearKey AND EcoPlots.IsComplete = true " + (string.IsNullOrEmpty(ClauseOfNonRemoved) ? " " : " AND " + ClauseOfNonRemoved) + (isCrownConditionCollected ? " AND (EcoConditions.PctDieback Is Null Or EcoConditions.PctDieback <> 100) " : " ") + " GROUP BY EcoPlots.PlotId, EcoPlots.PercentTreeCover ").AddScalar("PlotId", (IType) NHibernateUtil.Int32).AddScalar("PercentTreeCover", (IType) NHibernateUtil.Int32).AddScalar("TreeCount", (IType) NHibernateUtil.Int32);
    }
  }
}
