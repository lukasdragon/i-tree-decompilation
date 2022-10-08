// Decompiled with JetBrains decompiler
// Type: Eco.Util.Queries.Interfaces.SASIUtilProvider
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using NHibernate;
using System.Collections.Generic;

namespace Eco.Util.Queries.Interfaces
{
  public interface SASIUtilProvider
  {
    IQuery GetSQLQueryClearContentOfTable(string tableName);

    IQuery GetSQLQueryClearContentOfProject(string tableName);

    IQuery GetSQLQueryRemoveNullRecordsOfTable(string tableName, string fieldName);

    IQuery GetSQLQueryDropTable(string tableName);

    IQuery GetSQLQueryCreateEstimateTable(
      string tableName,
      List<Classifiers> partitionClassifiers);

    IQuery GetSQLQuerySelectEstimateTable(
      string tableName,
      List<Classifiers> partitionClassifiers);

    IQuery GetSQLQueryInsertEstimateTable(
      string tableName,
      List<Classifiers> partitionClassifiers);

    IQuery GetSQLQuerySelectYearlyMeanUVReduction(
      string UVReductionDatabaseFolder,
      string dbNameWithoutExtention);

    IQuery GetSQLQueryTransferUVIndexReduction(
      string UVReductionDatabaseFolder,
      string dbNameWithoutExtention);

    IQuery GetSQLQueryTransferBenMAP(
      string BenMAPDatabaseFolder,
      string dbNameWithoutExtention);

    IQuery GetSQLQueryFluxDomainValDomainFrom_08_DomainYearlySums(
      string PollutantsDatabaseFolder,
      string dbNameWithoutExtention);

    IQuery GetSQLQueryTransferPollutants(
      string PollutantsDatabaseFolder,
      string dbNameWithoutExtention);

    IQuery GetSQLQueryTransferHourlyUFOREBResults(
      string UFOREBdbFolder,
      string dbNameWithoutExtention);

    IQuery GetSQLQueryAdjustBenMAPResults(string Pollutant);

    IQuery GetSQLQuerySelectYearlySummaryByLanduse(
      string UFOREBdbFolder,
      string dbNameWithoutExtention);

    IQuery GetSQLQuerySelectYearlySummaryBySpecies(
      string UFOREBdbFolder,
      string dbNameWithoutExtention);

    IQuery GetSQLQuerySelectYearlyInterceptSum(
      string WaterDbFolder,
      string dbNameWithoutExtention);

    IQuery GetSQLQueryTransferHourlyHydroResults(
      string WaterDbFolder,
      string dbNameWithoutExtention);

    IQuery GetSQLQueryCombineTreeShrubEstimation(
      string leftTableName,
      string rightTableName,
      string targetTable,
      bool leftUnique,
      List<Classifiers> classifiersList,
      List<EstimateTypeEnum> estimateTypeList,
      List<int> estimateUnitList);

    IQuery GetSQLQueryOfLargeDBHTrees(
      string ClauseOfNonRemoved,
      bool isSampledProject,
      bool isActualDBH);

    IQuery GetSQLQueryOfPlotsWithSusspectedTreePercent(
      string ClauseOfNonRemoved,
      bool isCrownConditionCollected);
  }
}
