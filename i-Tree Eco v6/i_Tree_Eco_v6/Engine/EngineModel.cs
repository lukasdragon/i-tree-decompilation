// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Engine.EngineModel
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Cache;
using Eco.Util.Convert;
using i_Tree_Eco_v6.Forms;
using i_Tree_Eco_v6.Forms.Resources;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UFORE;
using UFORE.BioEmission;
using UFORE.Deposition;
using UFORE.LAI;
using UFORE.Weather;
using UFOREWrapper;

namespace i_Tree_Eco_v6.Engine
{
  internal class EngineModel
  {
    private ProgramSession m_ps;
    private ISession s;
    private Year currYear;
    private Series currSeries;
    private Project currProject;
    private int LocationID;
    private ProjectLocation currProjectLocation;
    private YearLocationData currYearLocation;
    public frmEngineProgress engineProgressForm;
    public CancellationToken engineCancellationToken;
    public EngineProgressArg engineReportArg;
    public IProgress<EngineProgressArg> engineProgress;
    private string workingErrorLogFileParameter;
    private string workingErrorLogFileEstimator;
    private string workingInputDatabaseName;
    private string workingInventoryDatabaseName;
    private string workingEngineEstimateDatabaseName;
    private string workingPestDatabaseName;
    private string workingLocSppDatabaseName;
    private string workingParamCalcInputFile;
    private string workingBenMapDatabaseName;
    private string tmpTreeLaiDB;
    private string tmpShrubLaiDB;
    private string tmpTreeWeatherDB;
    private string tmpShrubWeatherDB;
    private string tmpTreePollSiteDB;
    private string tmpShrubPollSiteDB;
    private string tmpTreeDryDepDB;
    private string tmpShrubDryDepDB;
    private string tmpTreeBenMapOutputDB;
    private string tmpShrubBenMapOutputDB;
    private string tmpTreeBioEmissionOutputDB;
    private string tmpShrubBioEmissionOutputDB;
    private string tmpTreeInterceptionOutputDB;
    private string tmpShrubInterceptionOutputDB;
    private string tmpFinalBenMapTreeDB;
    private string tmpFinalBenMapShrubDB;
    private string tmpUFOREDTreeDB;
    private string tmpUFOREDShrubDB;
    private string tmpUFOREBTreeDB;
    private string tmpUFOREBShrubDB;
    private string tmpWaterInterceptionTreeDB;
    private string tmpWaterInterceptionShrubDB;
    private Dictionary<string, Species> allSpecies = new Dictionary<string, Species>();
    private Dictionary<int, string> allSpeciesClassValueOrderToSppCode = new Dictionary<int, string>();
    private Dictionary<string, Pest> allPests = new Dictionary<string, Pest>();
    private Dictionary<int, Stratum> allStrata = new Dictionary<int, Stratum>();
    private Dictionary<string, StratumGenus> allStratumGenus = new Dictionary<string, StratumGenus>();
    private int ClassValueForStrataStudyArea;
    private int ClassValueForSpeciesAllSpecies;
    private DateTime dttmConvert1;
    private DateTime dttmConvert2;
    private DateTime dttmParameter1;
    private DateTime dttmParameter2;
    private DateTime dttmInsertSingle1;
    private DateTime dttmInsertSingle2;
    private DateTime dttmEstimate1;
    private DateTime dttmEstimate2;
    private DateTime dttmLoadEstimate1;
    private DateTime dttmLoadEstimate2;
    private DateTime dttmUFOREDBI1;
    private DateTime dttmUFOREDBI2;

    public void prepareToStart(ProgramSession passin_m_ps)
    {
      try
      {
        this.m_ps = passin_m_ps;
        this.s = this.m_ps.InputSession.CreateSession();
        this.currYear = this.s.Get<Year>((object) this.m_ps.InputSession.YearKey);
        this.currSeries = this.currYear.Series;
        this.currProject = this.currSeries.Project;
        this.LocationID = this.currProject.LocationId;
        this.currProjectLocation = this.currProject.Locations.Where<ProjectLocation>((Func<ProjectLocation, bool>) (p => p.LocationId == this.LocationID)).First<ProjectLocation>();
        this.currYearLocation = this.currYear.YearLocationData.Where<YearLocationData>((Func<YearLocationData, bool>) (p => p.ProjectLocation.Equals((object) this.currProjectLocation))).First<YearLocationData>();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public void CopyTempFiles()
    {
      try
      {
        string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\iTree\\tempModel";
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
        this.workingErrorLogFileParameter = DefaultTreeData.getTempFileName(str, "ErrLogPar.txt");
        this.workingErrorLogFileEstimator = DefaultTreeData.getTempFileName(str, "ErrLogEst.txt");
        this.workingInputDatabaseName = DefaultTreeData.getTempFileName(str, "Input.mdb");
        File.Copy(this.m_ps.InputSession.InputDb, this.workingInputDatabaseName, true);
        this.workingInventoryDatabaseName = DefaultTreeData.getTempFileName(str, "Inven.mdb");
        File.Copy(Application.StartupPath + "\\Data\\InventoryTemplate.mdb", this.workingInventoryDatabaseName, true);
        this.workingEngineEstimateDatabaseName = DefaultTreeData.getTempFileName(str, "Esti.mdb");
        File.Copy(Application.StartupPath + "\\Data\\EstimateTemplate.mdb", this.workingEngineEstimateDatabaseName, true);
        this.workingPestDatabaseName = DefaultTreeData.getTempFileName(str, "Pest.mdb");
        File.Copy(Application.StartupPath + "\\Data\\Pest.mdb", this.workingPestDatabaseName, true);
        this.workingLocSppDatabaseName = DefaultTreeData.getTempFileName(str, "LocSpp.mdb");
        File.Copy(Application.StartupPath + "\\Data\\LocationSpecies.mdb", this.workingLocSppDatabaseName, true);
        this.workingParamCalcInputFile = DefaultTreeData.getTempFileName(str, "prn");
        File.Copy(Application.StartupPath + "\\Data\\UforeParamCalcTemplatev2.prn", this.workingParamCalcInputFile, true);
        this.workingBenMapDatabaseName = DefaultTreeData.getTempFileName(str, "BenInpt.mdb");
        File.Copy(Application.StartupPath + "\\Data\\UFORE_BenMap.mdb", this.workingBenMapDatabaseName, true);
        this.tmpTreeLaiDB = DefaultTreeData.getTempFileName(str, "TreeLai.mdb");
        this.tmpShrubLaiDB = DefaultTreeData.getTempFileName(str, "ShrubLai.mdb");
        this.tmpTreeWeatherDB = DefaultTreeData.getTempFileName(str, "TreeWeather.mdb");
        this.tmpShrubWeatherDB = DefaultTreeData.getTempFileName(str, "ShrubWeather.mdb");
        this.tmpTreePollSiteDB = DefaultTreeData.getTempFileName(str, "TreePoll.mdb");
        this.tmpShrubPollSiteDB = DefaultTreeData.getTempFileName(str, "ShrubPoll.mdb");
        this.tmpTreeDryDepDB = DefaultTreeData.getTempFileName(str, "TreeDep.mdb");
        this.tmpShrubDryDepDB = DefaultTreeData.getTempFileName(str, "ShrubDep.mdb");
        this.tmpTreeBenMapOutputDB = DefaultTreeData.getTempFileName(str, "TreeBenMap.mdb");
        this.tmpShrubBenMapOutputDB = DefaultTreeData.getTempFileName(str, "ShrubBenMap.mdb");
        this.tmpTreeBioEmissionOutputDB = DefaultTreeData.getTempFileName(str, "TreeBioEmi.mdb");
        this.tmpShrubBioEmissionOutputDB = DefaultTreeData.getTempFileName(str, "ShrubBioEmi.mdb");
        this.tmpTreeInterceptionOutputDB = DefaultTreeData.getTempFileName(str, "TreeInter.mdb");
        this.tmpShrubInterceptionOutputDB = DefaultTreeData.getTempFileName(str, "ShrubInter.mdb");
        this.tmpFinalBenMapTreeDB = DefaultTreeData.getTempFileName(str, "FinBenMAPTree.mdb");
        this.tmpFinalBenMapShrubDB = DefaultTreeData.getTempFileName(str, "FinBenMAPShrub.mdb");
        this.tmpUFOREDTreeDB = DefaultTreeData.getTempFileName(str, "UFOREDTree.mdb");
        this.tmpUFOREDShrubDB = DefaultTreeData.getTempFileName(str, "UFOREDShrub.mdb");
        this.tmpUFOREBTreeDB = DefaultTreeData.getTempFileName(str, "UFOREBTree.mdb");
        this.tmpUFOREBShrubDB = DefaultTreeData.getTempFileName(str, "UFOREBShrub.mdb");
        this.tmpWaterInterceptionTreeDB = DefaultTreeData.getTempFileName(str, "WaterInterceptTree.mdb");
        this.tmpWaterInterceptionShrubDB = DefaultTreeData.getTempFileName(str, "WaterInterceptShrub.mdb");
      }
      catch (Exception ex)
      {
        this.deleteTempFiles();
        throw;
      }
    }

    private void deleteSingleFile(string aFile)
    {
      if (!(aFile != ""))
        return;
      if (!File.Exists(aFile))
        return;
      try
      {
        File.Delete(aFile);
      }
      catch
      {
      }
    }

    public void deleteTempFiles()
    {
      this.deleteSingleFile(this.workingInputDatabaseName);
      this.deleteSingleFile(this.workingInventoryDatabaseName);
      this.deleteSingleFile(this.workingEngineEstimateDatabaseName);
      this.deleteSingleFile(this.workingPestDatabaseName);
      this.deleteSingleFile(this.workingLocSppDatabaseName);
      this.deleteSingleFile(this.workingParamCalcInputFile);
      this.deleteSingleFile(this.workingErrorLogFileParameter);
      this.deleteSingleFile(this.workingErrorLogFileEstimator);
      this.deleteSingleFile(this.workingBenMapDatabaseName);
      this.deleteSingleFile(this.tmpTreeLaiDB);
      this.deleteSingleFile(this.tmpShrubLaiDB);
      this.deleteSingleFile(this.tmpTreeWeatherDB);
      this.deleteSingleFile(this.tmpShrubWeatherDB);
      this.deleteSingleFile(this.tmpTreePollSiteDB);
      this.deleteSingleFile(this.tmpShrubPollSiteDB);
      this.deleteSingleFile(this.tmpTreeDryDepDB);
      this.deleteSingleFile(this.tmpShrubDryDepDB);
      this.deleteSingleFile(this.tmpTreeBenMapOutputDB);
      this.deleteSingleFile(this.tmpShrubBenMapOutputDB);
      this.deleteSingleFile(this.tmpTreeBioEmissionOutputDB);
      this.deleteSingleFile(this.tmpShrubBioEmissionOutputDB);
      this.deleteSingleFile(this.tmpTreeInterceptionOutputDB);
      this.deleteSingleFile(this.tmpShrubInterceptionOutputDB);
      this.deleteSingleFile(this.tmpFinalBenMapTreeDB);
      this.deleteSingleFile(this.tmpFinalBenMapShrubDB);
      this.deleteSingleFile(this.tmpUFOREDTreeDB);
      this.deleteSingleFile(this.tmpUFOREDShrubDB);
      this.deleteSingleFile(this.tmpUFOREBTreeDB);
      this.deleteSingleFile(this.tmpUFOREBShrubDB);
      this.deleteSingleFile(this.tmpWaterInterceptionTreeDB);
      this.deleteSingleFile(this.tmpWaterInterceptionShrubDB);
    }

    public async Task<bool> run_NonBackgroundPart(ProgramSession m_ps)
    {
      EngineModel engineModel1 = this;
      try
      {
        engineModel1.prepareToStart(m_ps);
        engineModel1.CopyTempFiles();
        if (engineModel1.currYearLocation.PollutionYear == (short) 0)
        {
          engineModel1.deleteTempFiles();
          int num = (int) MessageBox.Show(EngineModelRes.PollutionYearMissingMsg, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
          return false;
        }
        if (!engineModel1.DataValidation())
        {
          engineModel1.deleteTempFiles();
          return false;
        }
        Application.DoEvents();
        using (CacheDownloadForm cdf = new CacheDownloadForm())
        {
          if (engineModel1.currProject == null || engineModel1.currYearLocation == null)
          {
            engineModel1.deleteTempFiles();
            return false;
          }
          m_ps.m_cache = new EcoCache(engineModel1.currProject, engineModel1.currYearLocation, cdf, m_ps.CacheUrl);
          new Thread((ThreadStart) (() => m_ps.m_cache.UploadCache(m_ps.InputSession.InputDb))).Start();
          await m_ps.m_cache.Cache();
        }
        if (m_ps.m_cache.CacheStatus == -1)
        {
          engineModel1.deleteTempFiles();
          int num = (int) MessageBox.Show(EcoCacheRes.CacheServerError, EcoCacheRes.CacheTitle, MessageBoxButtons.OK);
          return false;
        }
        engineModel1.engineProgressForm.Show();
        engineModel1.engineReportArg.CurrentStep = 1;
        engineModel1.engineReportArg.Description = EngineModelRes.EngineProgressForm_Start;
        engineModel1.engineReportArg.Percent = 0;
        engineModel1.engineProgressForm.DisplayProgress(engineModel1.engineReportArg);
        Application.DoEvents();
        engineModel1.engineReportArg.Percent = 5;
        engineModel1.engineProgressForm.DisplayProgress(engineModel1.engineReportArg);
        EngineModel engineModel2 = engineModel1;
        string inputDb1 = m_ps.InputSession.InputDb;
        Guid? yearKey = m_ps.InputSession.YearKey;
        Guid yearGuid = yearKey.Value;
        engineModel2.clearResults(inputDb1, yearGuid);
        engineModel1.engineReportArg.Percent = 10;
        engineModel1.engineProgressForm.DisplayProgress(engineModel1.engineReportArg);
        engineModel1.dttmConvert1 = DateTime.Now;
        engineModel1.dttmConvert2 = DateTime.Now;
        engineModel1.engineReportArg.CurrentStep = 2;
        engineModel1.engineReportArg.Description = EngineModelRes.EngineProgressForm_ParameterCalculating_Text;
        engineModel1.engineReportArg.Percent = 25;
        engineModel1.engineProgressForm.DisplayProgress(engineModel1.engineReportArg);
        Application.DoEvents();
        ParameterWrapper parameterWrapper = new ParameterWrapper();
        engineModel1.dttmParameter1 = DateTime.Now;
        string logFileParameter = engineModel1.workingErrorLogFileParameter;
        string locSppDatabaseName1 = engineModel1.workingLocSppDatabaseName;
        string locSppDatabaseName2 = engineModel1.workingLocSppDatabaseName;
        string inputDatabaseName1 = engineModel1.workingInputDatabaseName;
        string inventoryDatabaseName1 = engineModel1.workingInventoryDatabaseName;
        string paramCalcInputFile = engineModel1.workingParamCalcInputFile;
        string name1 = engineModel1.currProject.Name;
        string id1 = engineModel1.currSeries.Id;
        int id2 = (int) engineModel1.currYear.Id;
        parameterWrapper.runParameterCalculator(logFileParameter, locSppDatabaseName1, locSppDatabaseName2, inputDatabaseName1, inventoryDatabaseName1, paramCalcInputFile, name1, id1, id2);
        engineModel1.engineReportArg.Percent = 50;
        engineModel1.engineProgressForm.DisplayProgress(engineModel1.engineReportArg);
        Application.DoEvents();
        engineModel1.dttmParameter2 = DateTime.Now;
        engineModel1.InsertModelNote(m_ps.InputSession.InputDb, engineModel1.currYear.Guid, engineModel1.workingErrorLogFileParameter, true);
        bool flag1 = false;
        StreamReader streamReader1 = new StreamReader(engineModel1.workingErrorLogFileParameter);
        string str1;
        while ((str1 = streamReader1.ReadLine()) != null)
        {
          if (str1.Contains("@@@ Fatal"))
          {
            flag1 = true;
            break;
          }
        }
        streamReader1.Close();
        if (flag1)
        {
          LoggedErrorForm loggedErrorForm = new LoggedErrorForm();
          loggedErrorForm.initializeForm(engineModel1.workingErrorLogFileParameter, true);
          int num = (int) loggedErrorForm.ShowDialog();
          engineModel1.deleteTempFiles();
          return false;
        }
        engineModel1.engineReportArg.CurrentStep = 3;
        engineModel1.engineReportArg.Description = EngineModelRes.EngineProgressForm_LoadingSingleTreeResult;
        engineModel1.engineReportArg.Percent = 0;
        engineModel1.engineProgressForm.DisplayProgress(engineModel1.engineReportArg);
        Application.DoEvents();
        engineModel1.PrepareSpeciesAndPestDictionary(engineModel1.LocationID, engineModel1.currProject.Name, engineModel1.currSeries.Id, (int) engineModel1.currYear.Id, engineModel1.workingLocSppDatabaseName, engineModel1.workingPestDatabaseName, engineModel1.workingInventoryDatabaseName);
        engineModel1.engineReportArg.Percent = 5;
        engineModel1.engineProgressForm.DisplayProgress(engineModel1.engineReportArg);
        Application.DoEvents();
        engineModel1.dttmInsertSingle1 = DateTime.Now;
        EngineModel engineModel3 = engineModel1;
        yearKey = m_ps.InputSession.YearKey;
        Guid yearId = yearKey.Value;
        string name2 = engineModel1.currProject.Name;
        string id3 = engineModel1.currSeries.Id;
        int id4 = (int) engineModel1.currYear.Id;
        string inputDb2 = m_ps.InputSession.InputDb;
        string inputDatabaseName2 = engineModel1.workingInputDatabaseName;
        string inventoryDatabaseName2 = engineModel1.workingInventoryDatabaseName;
        int percent = engineModel1.engineReportArg.Percent;
        engineModel3.transferSingleTreeFromUFOREresults(yearId, name2, id3, id4, inputDb2, inputDatabaseName2, inventoryDatabaseName2, percent, 100);
        engineModel1.dttmInsertSingle2 = DateTime.Now;
        if (engineModel1.engineProgressForm.IsCancelled())
          throw new Exception("User canceled");
        engineModel1.engineReportArg.CurrentStep = 4;
        engineModel1.engineReportArg.Description = EngineModelRes.EngineProgressForm_Estimating;
        engineModel1.engineReportArg.Percent = 5;
        engineModel1.engineProgressForm.DisplayProgress(engineModel1.engineReportArg);
        Application.DoEvents();
        engineModel1.dttmEstimate1 = DateTime.Now;
        new EstimatorWrapper().runStatisticEstimator(engineModel1.workingErrorLogFileEstimator, engineModel1.workingLocSppDatabaseName, engineModel1.workingLocSppDatabaseName, engineModel1.workingPestDatabaseName, engineModel1.workingInputDatabaseName, engineModel1.workingInventoryDatabaseName, engineModel1.workingEngineEstimateDatabaseName, engineModel1.currProject.Name, engineModel1.currSeries.Id, (int) engineModel1.currYear.Id);
        engineModel1.engineReportArg.Percent = 85;
        engineModel1.engineProgressForm.DisplayProgress(engineModel1.engineReportArg);
        Application.DoEvents();
        engineModel1.InsertModelNote(m_ps.InputSession.InputDb, engineModel1.currYear.Guid, engineModel1.workingErrorLogFileEstimator, false);
        bool flag2 = false;
        StreamReader streamReader2 = new StreamReader(engineModel1.workingErrorLogFileEstimator);
        string str2;
        while ((str2 = streamReader2.ReadLine()) != null)
        {
          if (str2.Contains("@@@ Fatal"))
          {
            flag2 = true;
            break;
          }
        }
        streamReader2.Close();
        if (flag2)
        {
          LoggedErrorForm loggedErrorForm = new LoggedErrorForm();
          loggedErrorForm.initializeForm(engineModel1.workingErrorLogFileEstimator, true);
          int num = (int) loggedErrorForm.ShowDialog();
          engineModel1.deleteTempFiles();
          return false;
        }
        engineModel1.dttmEstimate2 = DateTime.Now;
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
        engineModel1.deleteTempFiles();
        return false;
      }
    }

    public bool run_BackgroundPart()
    {
      try
      {
        this.engineReportArg.Percent = 0;
        this.engineReportArg.CurrentStep = 5;
        this.engineReportArg.Description = EngineModelRes.EngineProgressForm_LoadingEstimatingResults;
        this.engineProgress.Report(this.engineReportArg);
        if (this.engineCancellationToken.IsCancellationRequested)
        {
          int num = (int) MessageBox.Show("User cancelled", EngineModelRes.EngineTitle, MessageBoxButtons.OK);
          return false;
        }
        EngineModel.setupClassifiers(this.m_ps.InputSession.InputDb);
        if (this.engineCancellationToken.IsCancellationRequested)
        {
          int num = (int) MessageBox.Show("User cancelled", EngineModelRes.EngineTitle, MessageBoxButtons.OK);
          return false;
        }
        this.engineReportArg.Percent = 5;
        this.engineProgress.Report(this.engineReportArg);
        EngineModel.setupEstimateType(this.m_ps.InputSession.InputDb);
        if (this.engineCancellationToken.IsCancellationRequested)
        {
          int num = (int) MessageBox.Show("User cancelled", EngineModelRes.EngineTitle, MessageBoxButtons.OK);
          return false;
        }
        this.engineReportArg.Percent = 10;
        this.engineProgress.Report(this.engineReportArg);
        EngineModel.setupUnits(this.m_ps.InputSession.InputDb);
        if (this.engineCancellationToken.IsCancellationRequested)
        {
          int num = (int) MessageBox.Show("User cancelled", EngineModelRes.EngineTitle, MessageBoxButtons.OK);
          return false;
        }
        this.engineReportArg.Percent = 15;
        this.engineProgress.Report(this.engineReportArg);
        this.dttmLoadEstimate1 = DateTime.Now;
        this.LoadingEstimateResults(this.m_ps.InputSession.YearKey.Value, this.currProject.Name, this.currSeries.Id, (int) this.currYear.Id, this.LocationID, this.currSeries.SampleType, this.m_ps.InputSession.InputDb, this.workingEngineEstimateDatabaseName, this.workingInventoryDatabaseName, this.workingLocSppDatabaseName, this.workingPestDatabaseName, this.engineReportArg.Percent, 100);
        this.dttmLoadEstimate2 = DateTime.Now;
        if (this.engineCancellationToken.IsCancellationRequested)
        {
          int num = (int) MessageBox.Show("User cancelled", EngineModelRes.EngineTitle, MessageBoxButtons.OK);
          return false;
        }
        this.engineReportArg.Percent = 0;
        this.engineReportArg.CurrentStep = 6;
        this.engineReportArg.Description = EngineModelRes.EngineProgressForm_CalculatingPollution;
        this.engineProgress.Report(this.engineReportArg);
        this.dttmUFOREDBI1 = DateTime.Now;
        this.ProcessUFORE_D_B_I_background(this.m_ps, this.m_ps.InputSession.InputDb, this.workingLocSppDatabaseName, this.workingEngineEstimateDatabaseName, this.workingBenMapDatabaseName);
        this.dttmUFOREDBI2 = DateTime.Now;
        bool flag = false;
        StreamReader streamReader1 = new StreamReader(this.workingErrorLogFileParameter);
        string str1;
        while ((str1 = streamReader1.ReadLine()) != null)
        {
          if (str1.Contains("@@@ Fatal") || str1.Contains("!!! Error") || str1.Contains("*** Error"))
          {
            flag = true;
            break;
          }
        }
        streamReader1.Close();
        StreamReader streamReader2 = new StreamReader(this.workingErrorLogFileEstimator);
        string str2;
        while ((str2 = streamReader2.ReadLine()) != null)
        {
          if (str2.Contains("@@@ Fatal") || str2.Contains("!!! Error") || str2.Contains("*** Error"))
          {
            flag = true;
            break;
          }
        }
        streamReader2.Close();
        if (flag)
        {
          this.engineReportArg.Percent = 100;
          this.engineReportArg.CurrentStep = this.engineReportArg.TotalSteps;
          this.engineProgress.Report(this.engineReportArg);
          int num = (int) MessageBox.Show(EngineModelRes.ProcessingErrorDisplay, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
        }
        this.engineReportArg.Percent = 100;
        this.engineReportArg.CurrentStep = this.engineReportArg.TotalSteps;
        this.engineProgress.Report(this.engineReportArg);
        this.deleteTempFiles();
        int num1 = 0;
        using (OleDbConnection oleDbConnection = new OleDbConnection())
        {
          EngineModel.OpenConnection(oleDbConnection, this.m_ps.InputSession.InputDb);
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "SELECT Count(EcoTrees.TreeId) AS CountOfTreeId FROM EcoPlots INNER JOIN EcoTrees ON EcoPlots.PlotKey = EcoTrees.PlotKey WHERE (((EcoPlots.YearKey)={guid {" + this.currYear.Guid.ToString() + "}}))";
          OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
          oleDbDataReader.Read();
          num1 = oleDbDataReader.GetInt32(oleDbDataReader.GetOrdinal("CountOfTreeId"));
          oleDbDataReader.Close();
          EngineModel.CloseConnection(oleDbConnection);
        }
        int num2 = (int) MessageBox.Show("Trees: " + num1.ToString() + "\nDB preparation: " + this.dttmConvert1.ToString() + "-->" + this.dttmConvert2.ToString() + "\nparameter calculator: " + this.dttmParameter1.ToString() + "-->" + this.dttmParameter2.ToString() + "\nindividual tree table loading: " + this.dttmInsertSingle1.ToString() + "-->" + this.dttmInsertSingle2.ToString() + "\nestimator: " + this.dttmEstimate1.ToString() + "-->" + this.dttmEstimate2.ToString() + "\nloading estimate results: " + this.dttmLoadEstimate1.ToString() + "-->" + this.dttmLoadEstimate2.ToString() + "\nUFOREDBI: " + this.dttmUFOREDBI1.ToString() + "-->" + this.dttmUFOREDBI2.ToString());
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
        this.deleteTempFiles();
        return false;
      }
    }

    public static void setupClassifiers(string inputDB)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection())
      {
        EngineModel.OpenConnection(oleDbConnection, inputDB);
        OleDbTransaction oleDbTransaction = oleDbConnection.BeginTransaction();
        try
        {
          OleDbCommand oleDbCommand1 = new OleDbCommand();
          oleDbCommand1.Connection = oleDbConnection;
          oleDbCommand1.Transaction = oleDbTransaction;
          Dictionary<int, string> dictionary1 = new Dictionary<int, string>();
          oleDbCommand1.CommandText = "SELECT TableOfStatisticalEstimates.StatisticalTableId, TableOfStatisticalEstimates.TableTitle, PartitionDefinitionsTable.ClassifierId  FROM PartitionDefinitionsTable RIGHT JOIN TableOfStatisticalEstimates ON PartitionDefinitionsTable.StatisticalTableId = TableOfStatisticalEstimates.StatisticalTableId";
          OleDbDataReader oleDbDataReader1 = oleDbCommand1.ExecuteReader();
          while (oleDbDataReader1.Read())
          {
            if (oleDbDataReader1["ClassifierId"] == DBNull.Value && !dictionary1.ContainsKey(oleDbDataReader1.GetInt32(oleDbDataReader1.GetOrdinal("StatisticalTableId"))))
              dictionary1.Add(oleDbDataReader1.GetInt32(oleDbDataReader1.GetOrdinal("StatisticalTableId")), oleDbDataReader1.GetString(oleDbDataReader1.GetOrdinal("TableTitle")));
          }
          oleDbDataReader1.Close();
          oleDbCommand1.CommandText = "select * from classifiers";
          OleDbDataReader oleDbDataReader2 = oleDbCommand1.ExecuteReader();
          if (oleDbDataReader2.HasRows)
          {
            Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
            while (oleDbDataReader2.Read())
              dictionary2.Add(oleDbDataReader2.GetInt32(oleDbDataReader2.GetOrdinal("ClassifierId")), oleDbDataReader2.GetString(oleDbDataReader2.GetOrdinal("ClassAbbreviation")));
            oleDbDataReader2.Close();
            int num;
            foreach (Classifiers key in Enum.GetValues(typeof (Classifiers)))
            {
              if (dictionary2.ContainsKey((int) key))
              {
                if (key.ToString().ToLower() != dictionary2[(int) key].ToLower())
                {
                  OleDbCommand oleDbCommand2 = oleDbCommand1;
                  string str1 = key.ToString();
                  num = (int) key;
                  string str2 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                  string str3 = "Update classifiers set ClassAbbreviation='" + str1 + "' where ClassifierId=" + str2;
                  oleDbCommand2.CommandText = str3;
                  oleDbCommand1.ExecuteNonQuery();
                  OleDbCommand oleDbCommand3 = oleDbCommand1;
                  num = (int) key;
                  string str4 = "SELECT TableOfStatisticalEstimates.StatisticalTableId, TableOfStatisticalEstimates.TableTitle, PartitionDefinitionsTable.ClassifierId  FROM PartitionDefinitionsTable INNER JOIN TableOfStatisticalEstimates ON PartitionDefinitionsTable.StatisticalTableId = TableOfStatisticalEstimates.StatisticalTableId  WHERE PartitionDefinitionsTable.ClassifierId=" + num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                  oleDbCommand3.CommandText = str4;
                  OleDbDataReader oleDbDataReader3 = oleDbCommand1.ExecuteReader();
                  while (oleDbDataReader3.Read())
                  {
                    if (!dictionary1.ContainsKey(oleDbDataReader3.GetInt32(oleDbDataReader3.GetOrdinal("StatisticalTableId"))))
                      dictionary1.Add(oleDbDataReader3.GetInt32(oleDbDataReader3.GetOrdinal("StatisticalTableId")), oleDbDataReader3.GetString(oleDbDataReader3.GetOrdinal("TableTitle")));
                  }
                  oleDbDataReader3.Close();
                }
              }
              else
              {
                OleDbCommand oleDbCommand4 = oleDbCommand1;
                string[] strArray = new string[7];
                strArray[0] = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES (";
                num = (int) key;
                strArray[1] = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[2] = ",'";
                strArray[3] = key.ToString();
                strArray[4] = "','";
                strArray[5] = key.ToString();
                strArray[6] = "', 0)";
                string str5 = string.Concat(strArray);
                oleDbCommand4.CommandText = str5;
                oleDbCommand1.ExecuteNonQuery();
                OleDbCommand oleDbCommand5 = oleDbCommand1;
                num = (int) key;
                string str6 = "SELECT TableOfStatisticalEstimates.StatisticalTableId, TableOfStatisticalEstimates.TableTitle, PartitionDefinitionsTable.ClassifierId  FROM PartitionDefinitionsTable INNER JOIN TableOfStatisticalEstimates ON PartitionDefinitionsTable.StatisticalTableId = TableOfStatisticalEstimates.StatisticalTableId  WHERE PartitionDefinitionsTable.ClassifierId=" + num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                oleDbCommand5.CommandText = str6;
                OleDbDataReader oleDbDataReader4 = oleDbCommand1.ExecuteReader();
                while (oleDbDataReader4.Read())
                {
                  if (!dictionary1.ContainsKey(oleDbDataReader4.GetInt32(oleDbDataReader4.GetOrdinal("StatisticalTableId"))))
                    dictionary1.Add(oleDbDataReader4.GetInt32(oleDbDataReader4.GetOrdinal("StatisticalTableId")), oleDbDataReader4.GetString(oleDbDataReader4.GetOrdinal("TableTitle")));
                }
                oleDbDataReader4.Close();
              }
            }
          }
          else
          {
            oleDbDataReader2.Close();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (0, \"None\", \"None\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (1, \"DBH\", \"DBH Class\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (2, \"Dieback\", \"Dieback Class\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (3, \"Strata\", \"strata\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (4, \"Species\", \"The species of the tree/shrub\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (5, \"Height\", \"Height Class\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (6, \"Continent\", \"Continent of origin of species\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (7, \"GroundCover\", \"The type of ground covers being sampled\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (8, \"Geopoliticalunit\", \"Native within geopolitical unit\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (9, \"PrimaryIndex\", \"Pollen Index\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (10, \"ALB\", \"Asian longhorned beetle susceptibility index\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (11, \"GM\", \"Gypsy moth susceptibility index\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (12, \"Landuse\", \"Actual Field Landuse\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (13, \"EnergyUse\", \"Energy Use\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (14, \"EnergyType\", \"Energy Type\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (15, \"Pollutant\", \"Air-Born Pollutant\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (16, \"VOCs\", \"Volatile Organic Compound\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (17, \"EnergySource\", \"Energy Source\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (18, \"Hour\", \"Hour\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (19, \"Month\", \"Month\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (20, \"EAB\", \"EAB\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (21, \"DED\", \"DED\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (22, \"TSDieback\", \"Tree Stress Dieback\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (23, \"TSEpiSprout\", \"Tree Stress Epicormic Sprouts\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (24, \"TSWiltFoli\", \"Tree Stress Wilted Foliage\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (25, \"TSEnvStress\", \"Tree Stress Environmental Stress\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (26, \"TSHumStress\", \"Tress Stress Human Stress\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (27, \"FTChewFoli\", \"Foliage/Twigs Chewed Foliage\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (28, \"FTDiscFoli\", \"Foliage/Twigs Discolored Foliage\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (29, \"FTAbnFoli\", \"Foliage/Twigs Abnormal Foliage\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (30, \"FTInsectSigns\", \"Foliage/Twigs Insect Signs\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (31, \"FTFoliAffect\", \"Foliage/Twigs Foliage Affected\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (32, \"BBInsectSigns\", \"Branches/Bole Signs of Insects\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (33, \"BBInsectPres\", \"Branches/Bole Insect Presence\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (34, \"BBDiseaseSigns\", \"Branches/Bole Signs of Disease\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (35, \"BBAbnGrowth\", \"Branches/Bole Abnormal Growth\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (36, \"BBProbLoc\", \"Branches/Bole Probable Location\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (37, \"PestPest\", \"PrimaryPest\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (38, \"PestIndicator\", \"Pest Indicator\", 0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n\t                    (39, \"CityManaged\", \"Private or public tree\",0)";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n                        (40, \"StreetTree\", \"Street tree or not\", 0);";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n                        (41, \"Living\", \"Living trees\", 0);";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n                        (42, \"CDBH\", \"Customized DBH\", 0);";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "INSERT INTO [Classifiers] ([ClassifierId], [ClassAbbreviation], [ClassDescription], [ClassFlag]) VALUES\r\n                        (43, \"CDieback\", \"Health Class\", 0);";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "SELECT StatisticalTableId, TableTitle FROM TableOfStatisticalEstimates";
            OleDbDataReader oleDbDataReader5 = oleDbCommand1.ExecuteReader();
            while (oleDbDataReader5.Read())
            {
              if (!dictionary1.ContainsKey(oleDbDataReader5.GetInt32(oleDbDataReader5.GetOrdinal("StatisticalTableId"))))
                dictionary1.Add(oleDbDataReader5.GetInt32(oleDbDataReader5.GetOrdinal("StatisticalTableId")), oleDbDataReader5.GetString(oleDbDataReader5.GetOrdinal("TableTitle")));
            }
            oleDbDataReader5.Close();
          }
          if (dictionary1.Count > 0)
          {
            string str = "";
            foreach (int key in dictionary1.Keys)
            {
              str = !(str == "") ? str + "," + key.ToString((IFormatProvider) CultureInfo.InvariantCulture) : key.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              oleDbCommand1.CommandText = "DROP TABLE [" + dictionary1[key] + "]";
              try
              {
                oleDbCommand1.ExecuteNonQuery();
              }
              catch
              {
              }
            }
            oleDbCommand1.CommandText = "DELETE * FROM TableOfStatisticalEstimates WHERE StatisticalTableId IN (" + str + ")";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "DELETE * FROM PartitionDefinitionsTable WHERE StatisticalTableId IN (" + str + ")";
            oleDbCommand1.ExecuteNonQuery();
          }
        }
        catch (Exception ex)
        {
          oleDbTransaction.Rollback();
          EngineModel.CloseConnection(oleDbConnection);
          throw;
        }
        oleDbTransaction.Commit();
        EngineModel.CloseConnection(oleDbConnection);
      }
    }

    public static void setupEstimateType(string inputDB)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection())
      {
        EngineModel.OpenConnection(oleDbConnection, inputDB);
        try
        {
          Dictionary<int, string> dictionary1 = new Dictionary<int, string>();
          dictionary1.Add(0, "None");
          dictionary1.Add(1, "Number of Trees");
          dictionary1.Add(2, "Carbon Storage");
          dictionary1.Add(3, "Gross Carbon Sequestration");
          dictionary1.Add(4, "Net Carbon Sequestration");
          dictionary1.Add(5, "Leaf Area");
          dictionary1.Add(6, "Leaf Biomass");
          dictionary1.Add(7, "Tree Biomass");
          dictionary1.Add(8, "Compensatory value");
          dictionary1.Add(9, "Energy - fuels");
          dictionary1.Add(10, "Carbon Avoided");
          dictionary1.Add(11, "Percent of population");
          dictionary1.Add(12, "Cover Area");
          dictionary1.Add(13, "Species diversity");
          dictionary1.Add(14, "Building Interaction");
          dictionary1.Add(15, "Electricity Avoided");
          dictionary1.Add(31, "Pollution removal");
          dictionary1.Add(32, "VOC Emissions");
          dictionary1.Add(33, "Water Interception");
          dictionary1.Add(34, "Avoided Runoff");
          dictionary1.Add(35, "Potential Evapotranspiration");
          dictionary1.Add(36, "Transpiration");
          dictionary1.Add(37, "Potential Evaporation");
          dictionary1.Add(38, "Evaporation");
          Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "select * from EstimationTypeTable";
          OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
          if (oleDbDataReader.HasRows)
          {
            while (oleDbDataReader.Read())
              dictionary2.Add(oleDbDataReader.GetInt32(oleDbDataReader.GetOrdinal("EstimateTypeId")), oleDbDataReader.GetString(oleDbDataReader.GetOrdinal("QuantityEstimated")));
          }
          oleDbDataReader.Close();
          foreach (EstimateTypeEnum key in Enum.GetValues(typeof (EstimateTypeEnum)))
          {
            if (dictionary2.ContainsKey((int) key))
            {
              if (dictionary1[(int) key] != dictionary2[(int) key])
              {
                oleDbCommand.CommandText = "UPDATE [EstimationTypeTable] SET [QuantityEstimated] = '" + dictionary1[(int) key] + "' WHERE [EstimateTypeId]=" + ((int) key).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                oleDbCommand.ExecuteNonQuery();
              }
            }
            else
            {
              oleDbCommand.CommandText = "INSERT INTO [EstimationTypeTable] ([EstimateTypeId], [QuantityEstimated], [EstimatedTypeFlag]) VALUES (" + ((int) key).ToString((IFormatProvider) CultureInfo.InvariantCulture) + ", '" + dictionary1[(int) key] + "', 0)";
              oleDbCommand.ExecuteNonQuery();
            }
          }
          EngineModel.CloseConnection(oleDbConnection);
        }
        catch (Exception ex)
        {
          EngineModel.CloseConnection(oleDbConnection);
          throw;
        }
      }
    }

    public static void setupEquationType(string inputDB)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection())
      {
        try
        {
          EngineModel.OpenConnection(oleDbConnection, inputDB);
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "select * from EquationTypeTable";
          OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
          if (oleDbDataReader.Read())
          {
            oleDbDataReader.Close();
          }
          else
          {
            oleDbDataReader.Close();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (1, \"None\", \"None\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n                    (2, \"Percent\", \"Percent\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (5, \"Sample mean\", \"Sample mean\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (7, \"Sample Covariance\", \"Sample Covariance\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (8, \"Stratum sample mean estim\", \"Stratum sample mean estim\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (10, \"Per area stratum mean est\", \"Per area stratum mean estimator\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (12, \"Percent stratum mean esti\", \"Percent stratum sample mean estimator\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (14, \"Total stratum estimator\", \"Total stratum estimator\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (16, \"Ratio stratum estimator\", \"Ratio stratum estimator\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (18, \"Strata smaple mean estima\", \"Strata sample mean estimator\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (20, \"Per area strata sample es\", \"Per area strata sample mean estimator\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (22, \"Percent strata mean estim\", \"Percent strata sample mean estimator\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (24, \"Total strata estimator\", \"Total strata estimator\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (26, \"Ratio strata estimator\", \"Ratio strata estimator\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (50, \"Shannon-Wiener diversity\", \"Shannon-Wiener diversity\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (51, \"Menhinick's Diversity Ind\", \"Menhinick's Diversity Ind\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (52, \"Simpson's Diversity index\", \"Simpson's Diversity index\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (53, \"Sahnnon-Wiener Evenness I\", \"Sahnnon-Wiener Evenness I\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (54, \"Sanders' Rarefraction Tec\", \"Sanders' Rarefraction Tec\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [EquationTypeTable] ([EquationTypeId], [EquationTypeName], [EquationTypeDescription], [EquationTypeFlag]) VALUES\r\n\t                (55, \"SpeciesRichness\", \"SpeciesRichness\", 0)";
            oleDbCommand.ExecuteNonQuery();
          }
          EngineModel.CloseConnection(oleDbConnection);
        }
        catch (Exception ex)
        {
          EngineModel.CloseConnection(oleDbConnection);
          throw;
        }
      }
    }

    public static void setupUnits(string inputDB)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection())
      {
        EngineModel.OpenConnection(oleDbConnection, inputDB);
        try
        {
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "select * from UnitsTable";
          OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
          if (oleDbDataReader.Read())
          {
            oleDbDataReader.Close();
          }
          else
          {
            oleDbDataReader.Close();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (1, \"None\", \"None\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n                        (2, \"Count\", \"No.\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (3, \"Percent\", \"%\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (4, \"Boolean\", \"(none)\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (5, \"Inches\", \"in\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (6, \"Feet\", \"ft\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (7, \"Miles\", \"mi\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (8, \"Square inch\", \"in2\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (9, \"Square feet\", \"ft2\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (10, \"Square miles\", \"mi2\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (11, \"Centimeters\", \"cm\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (12, \"Meters\", \"m\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (13, \"Kilometer\", \"km\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (14, \"Square centimeter\", \"cm2\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (15, \"Square meter\", \"m2\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (16, \"Square kilometer\", \"km2\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (17, \"Ounces\", \"oz\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (18, \"Pounds\", \"lbs\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (19, \"Ton\", \"ton\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (20, \"Grams\", \"g\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (21, \"Kilograms\", \"kg\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (22, \"Metric Tons\", \"mt\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (23, \"Acre\", \"a\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (24, \"Hectare\", \"ha\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (25, \"Hour\", \"Hr\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (26, \"Day\", \"Dy\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (27, \"Month\", \"mth\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (28, \"Year\", \"Yr\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (29, \"Monetary unit\", \"$\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (30, \"Megawatt-hours\", \"mwh\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (31, \"Million British Thermal Units\", \"Mbtu\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (32, \"Growing Period\", \"gr\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (33, \"Cubic inch\", \"in3\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (34, \"Cubic feet\", \"ft3\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (35, \"Cubic miles\", \"mi3\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (36, \"Cubic Centimeter\", \"cm3\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (37, \"Cubic Meter\", \"m3\", 0)";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "INSERT INTO [UnitsTable] ([UnitsId], [UnitsDescription], [UnitsAbbreviation], [UnitsFlag]) VALUES\r\n\t                    (38, \"Cubic Kilometer\", \"km3\", 0)";
            oleDbCommand.ExecuteNonQuery();
          }
          EngineModel.CloseConnection(oleDbConnection);
        }
        catch (Exception ex)
        {
          EngineModel.CloseConnection(oleDbConnection);
          throw;
        }
      }
    }

    private void LoadingEstimateResults(
      Guid yearId,
      string LocationName,
      string SeriesName,
      int timePeriod,
      int LocationID,
      SampleType seriesSampleType,
      string inputDB,
      string UforeOutputDB,
      string UforeInventoryDB,
      string locSppDB,
      string pestDB,
      int fromPercent,
      int toPercent)
    {
      OleDbConnection oleDbConnection1 = new OleDbConnection();
      OleDbConnection oleDbConnection2 = new OleDbConnection();
      OleDbConnection oleDbConnection3 = new OleDbConnection();
      OleDbConnection oleDbConnection4 = new OleDbConnection();
      OleDbConnection oleDbConnection5 = new OleDbConnection();
      try
      {
        EngineModel.OpenConnection(oleDbConnection1, inputDB);
        EngineModel.OpenConnection(oleDbConnection2, UforeOutputDB);
        EngineModel.OpenConnection(oleDbConnection3, UforeInventoryDB);
        EngineModel.OpenConnection(oleDbConnection4, locSppDB);
        EngineModel.OpenConnection(oleDbConnection5, pestDB);
        OleDbCommand oleDbCommand1 = new OleDbCommand();
        OleDbCommand oleDbCommand2 = new OleDbCommand();
        OleDbCommand oleDbCommand3 = new OleDbCommand();
        int index1 = 0;
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        int num6 = 0;
        int num7 = 0;
        int num8 = 0;
        int num9 = 0;
        int[] numArray1 = new int[100];
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if (this.engineReportArg.Percent + 5 > toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += 5;
        this.engineProgress.Report(this.engineReportArg);
        oleDbCommand1.Connection = oleDbConnection2;
        oleDbCommand1.CommandText = "select * from Classifiers";
        OleDbDataReader oleDbDataReader1 = oleDbCommand1.ExecuteReader();
        int ordinal1 = oleDbDataReader1.GetOrdinal("ClassifierId");
        int ordinal2 = oleDbDataReader1.GetOrdinal("ClassifierType");
        int ordinal3 = oleDbDataReader1.GetOrdinal("ClassAbbreviation");
        while (oleDbDataReader1.Read())
        {
          switch (oleDbDataReader1.GetInt32(ordinal2))
          {
            case 1:
              if (oleDbDataReader1.GetString(ordinal3) == "DBH")
              {
                num2 = oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 1;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "TSDieback")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 22;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "TSEpiSprout")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 23;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "TSWiltFoli")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 24;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "TSEnvStress")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 25;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "TSHumStress")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 26;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "FTChewFoli")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 27;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "FTDiscFoli")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 28;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "FTAbnFoli")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 29;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "FTInsectSigns")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 30;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "FTFoliAffect")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 31;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "BBInsectSigns")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 32;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "BBInsectPres")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 33;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "BBDiseaseSigns")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 34;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "BBAbnGrowth")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 35;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "BBProbLoc")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 36;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "PestPest")
              {
                num9 = oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 37;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "PestIndicator")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 38;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "CityManaged")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 39;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "StreetTree")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 40;
                continue;
              }
              continue;
            case 2:
              if (oleDbDataReader1.GetString(ordinal3) == "Dieback")
              {
                num3 = oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 2;
                continue;
              }
              if (oleDbDataReader1.GetString(ordinal3) == "Living")
              {
                oleDbDataReader1.GetInt32(ordinal1);
                numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 41;
                continue;
              }
              continue;
            case 3:
              index1 = oleDbDataReader1.GetInt32(ordinal1);
              numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 3;
              continue;
            case 4:
              num1 = oleDbDataReader1.GetInt32(ordinal1);
              numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 4;
              continue;
            case 6:
              num6 = oleDbDataReader1.GetInt32(ordinal1);
              numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 6;
              continue;
            case 7:
              num5 = oleDbDataReader1.GetInt32(ordinal1);
              numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 7;
              continue;
            case 8:
              num7 = oleDbDataReader1.GetInt32(ordinal1);
              numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 8;
              continue;
            case 12:
              num8 = oleDbDataReader1.GetInt32(ordinal1);
              numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 12;
              continue;
            case 13:
              num4 = oleDbDataReader1.GetInt32(ordinal1);
              numArray1[oleDbDataReader1.GetInt32(ordinal1)] = 13;
              continue;
            default:
              continue;
          }
        }
        oleDbDataReader1.Close();
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if (this.engineReportArg.Percent + 5 > toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += 5;
        this.engineProgress.Report(this.engineReportArg);
        oleDbCommand1.Connection = oleDbConnection3;
        oleDbCommand1.CommandText = "select * from StrataTable Where LocationName='" + LocationName + "' AND SeriesName='" + SeriesName + "' AND Year=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        OleDbDataReader oleDbDataReader2 = oleDbCommand1.ExecuteReader();
        oleDbCommand2.Connection = oleDbConnection2;
        int ordinal4 = oleDbDataReader2.GetOrdinal("StratumID");
        int ordinal5 = oleDbDataReader2.GetOrdinal("StratumDescription");
        while (oleDbDataReader2.Read())
        {
          oleDbCommand2.CommandText = "Update FixedValueTable Set FixedValue='" + oleDbDataReader2.GetString(ordinal5) + "' WHERE ClassifierId=" + index1.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND FixedValue='" + oleDbDataReader2.GetInt32(ordinal4).ToString() + "'";
          oleDbCommand2.ExecuteNonQuery();
        }
        oleDbDataReader2.Close();
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if (this.engineReportArg.Percent + 5 > toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += 5;
        this.engineProgress.Report(this.engineReportArg);
        oleDbCommand1.Connection = oleDbConnection4;
        oleDbCommand1.CommandText = "select * from _EnergyEffects";
        OleDbDataReader oleDbDataReader3 = oleDbCommand1.ExecuteReader();
        oleDbCommand2.Connection = oleDbConnection2;
        int ordinal6 = oleDbDataReader3.GetOrdinal("EnergyEffectID");
        int ordinal7 = oleDbDataReader3.GetOrdinal("EnergyEffectDescription");
        while (oleDbDataReader3.Read())
        {
          oleDbCommand2.CommandText = "Update FixedValueTable Set FixedValue='" + oleDbDataReader3.GetString(ordinal7) + "' WHERE ClassifierId=" + num4.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND FixedValue='" + oleDbDataReader3.GetInt32(ordinal6).ToString() + "'";
          oleDbCommand2.ExecuteNonQuery();
        }
        oleDbDataReader3.Close();
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if (this.engineReportArg.Percent + 5 > toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += 5;
        this.engineProgress.Report(this.engineReportArg);
        oleDbCommand1.Connection = oleDbConnection3;
        oleDbCommand1.CommandText = "select * from CoverTypesTable Where LocationName='" + LocationName + "' AND SeriesName='" + SeriesName + "' AND Year=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        OleDbDataReader oleDbDataReader4 = oleDbCommand1.ExecuteReader();
        oleDbCommand2.Connection = oleDbConnection2;
        int ordinal8 = oleDbDataReader4.GetOrdinal("CoverTypeID");
        int ordinal9 = oleDbDataReader4.GetOrdinal("CoverDescription");
        while (oleDbDataReader4.Read())
        {
          oleDbCommand2.CommandText = "Update FixedValueTable Set FixedValue='" + oleDbDataReader4.GetString(ordinal9) + "' WHERE ClassifierId=" + num5.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND FixedValue='" + oleDbDataReader4.GetInt32(ordinal8).ToString() + "'";
          oleDbCommand2.ExecuteNonQuery();
        }
        oleDbDataReader4.Close();
        try
        {
          oleDbCommand1.Connection = oleDbConnection4;
          oleDbCommand1.CommandText = "select * from ContinentTable";
          OleDbDataReader oleDbDataReader5 = oleDbCommand1.ExecuteReader();
          oleDbCommand2.Connection = oleDbConnection2;
          int ordinal10 = oleDbDataReader5.GetOrdinal("ContinentID");
          int ordinal11 = oleDbDataReader5.GetOrdinal("ContinentName");
          while (oleDbDataReader5.Read())
          {
            oleDbCommand2.CommandText = "Update FixedValueTable Set FixedValue='" + oleDbDataReader5.GetString(ordinal11) + "' WHERE ClassifierId=" + num6.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND FixedValue='" + oleDbDataReader5.GetInt16(ordinal10).ToString() + "'";
            oleDbCommand2.ExecuteNonQuery();
          }
          oleDbDataReader5.Close();
        }
        catch (Exception ex)
        {
          throw;
        }
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if (this.engineReportArg.Percent + 5 > toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += 5;
        this.engineProgress.Report(this.engineReportArg);
        bool flag = true;
        int num10 = 0;
        oleDbCommand1.Connection = oleDbConnection1;
        oleDbCommand1.CommandText = "select * from EcoLanduses Where YearKey={" + yearId.ToString() + "} AND LandUseCode='N'";
        OleDbDataReader oleDbDataReader6 = oleDbCommand1.ExecuteReader();
        if (oleDbDataReader6.Read())
          flag = false;
        oleDbDataReader6.Close();
        if (flag)
        {
          oleDbCommand1.Connection = oleDbConnection2;
          oleDbCommand1.CommandText = "select * from FixedValueTable where ClassifierId=" + num8.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND FixedValue='N'";
          OleDbDataReader oleDbDataReader7 = oleDbCommand1.ExecuteReader();
          if (oleDbDataReader7.Read())
            num10 = oleDbDataReader7.GetInt32(oleDbDataReader7.GetOrdinal("FixedValueId"));
          oleDbDataReader7.Close();
          if (num10 > 0)
          {
            oleDbCommand1.CommandText = "Delete * from FixedValueTable where ClassifierId=" + num8.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND FixedValue='N'";
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "Update FixedValueTable set FixedValueId=FixedValueId-1 where ClassifierId=" + num8.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND FixedValueId > " + num10.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            oleDbCommand1.ExecuteNonQuery();
            oleDbCommand1.CommandText = "SELECT LocationTable.LocationName, SeriesTable.SeriesName, TimePeriodTable.TimePeriod, TimePeriodTable.TimePeriodId FROM (LocationTable INNER JOIN SeriesTable ON LocationTable.LocationId = SeriesTable.LocationId) INNER JOIN TimePeriodTable ON SeriesTable.SeriesId = TimePeriodTable.SeriesId WHERE LocationTable.LocationName='" + LocationName + "' AND SeriesTable.SeriesName='" + SeriesName + "' AND TimePeriodTable.TimePeriod=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            OleDbDataReader oleDbDataReader8 = oleDbCommand1.ExecuteReader();
            oleDbDataReader8.Read();
            int int32 = oleDbDataReader8.GetInt32(oleDbDataReader8.GetOrdinal("TimePeriodId"));
            oleDbDataReader8.Close();
            oleDbCommand1.CommandText = "SELECT TableOfStatisticalEstimates.StatisticalTableId, TableOfStatisticalEstimates.TableTitle, PartitionDefinitionsTable.ClassifierId, PartitionDefinitionsTable.PartitionName FROM TableOfStatisticalEstimates INNER JOIN PartitionDefinitionsTable ON TableOfStatisticalEstimates.StatisticalTableId = PartitionDefinitionsTable.StatisticalTableId WHERE PartitionDefinitionsTable.ClassifierId=" + num8.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            OleDbDataReader oleDbDataReader9 = oleDbCommand1.ExecuteReader();
            oleDbCommand2.Connection = oleDbConnection2;
            while (oleDbDataReader9.Read())
            {
              oleDbCommand2.CommandText = "delete * from [" + oleDbDataReader9.GetString(oleDbDataReader9.GetOrdinal("TableTitle")) + "] where TimePeriodId = " + int32.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND [" + oleDbDataReader9.GetString(oleDbDataReader9.GetOrdinal("PartitionName")) + "]=" + num10.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              oleDbCommand2.ExecuteNonQuery();
              oleDbCommand2.CommandText = "Update [" + oleDbDataReader9.GetString(oleDbDataReader9.GetOrdinal("TableTitle")) + "] set [" + oleDbDataReader9.GetString(oleDbDataReader9.GetOrdinal("PartitionName")) + "] = [" + oleDbDataReader9.GetString(oleDbDataReader9.GetOrdinal("PartitionName")) + "]-1 where TimePeriodId = " + int32.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND [" + oleDbDataReader9.GetString(oleDbDataReader9.GetOrdinal("PartitionName")) + "]>" + num10.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              oleDbCommand2.ExecuteNonQuery();
            }
            oleDbDataReader9.Close();
          }
        }
        oleDbCommand1.Connection = oleDbConnection1;
        oleDbCommand1.CommandText = "select * from EcoLanduses Where YearKey={" + yearId.ToString() + "}";
        OleDbDataReader oleDbDataReader10 = oleDbCommand1.ExecuteReader();
        oleDbCommand2.Connection = oleDbConnection2;
        int ordinal12 = oleDbDataReader10.GetOrdinal("LandUseCode");
        int ordinal13 = oleDbDataReader10.GetOrdinal("LandUseDescription");
        while (oleDbDataReader10.Read())
        {
          oleDbCommand2.CommandText = "Update FixedValueTable Set FixedValue='" + oleDbDataReader10.GetString(ordinal13) + "' WHERE ClassifierId=" + num8.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND FixedValue='" + oleDbDataReader10.GetString(ordinal12) + "'";
          oleDbCommand2.ExecuteNonQuery();
        }
        oleDbDataReader10.Close();
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if (this.engineReportArg.Percent + 5 > toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += 5;
        this.engineProgress.Report(this.engineReportArg);
        oleDbCommand1.Connection = oleDbConnection2;
        oleDbCommand1.CommandText = "select * from FixedValueTable Where ClassifierID=" + num7.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        OleDbDataReader oleDbDataReader11 = oleDbCommand1.ExecuteReader();
        int num11;
        while (oleDbDataReader11.Read())
        {
          string str1 = oleDbDataReader11.GetString(oleDbDataReader11.GetOrdinal("FixedValue"));
          string str2 = str1.Substring(0, 3);
          string str3 = str1.Substring(3, 2);
          if (str2 == "000")
          {
            oleDbCommand2.Connection = oleDbConnection2;
            OleDbCommand oleDbCommand4 = oleDbCommand2;
            string str4 = num7.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            num11 = oleDbDataReader11.GetInt32(oleDbDataReader11.GetOrdinal("FixedValueID"));
            string str5 = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            string str6 = "Update FixedValueTable set FixedValue='Others' where ClassifierID=" + str4 + " And FixedValueID=" + str5;
            oleDbCommand4.CommandText = str6;
            oleDbCommand2.ExecuteNonQuery();
          }
          else
          {
            oleDbCommand2.Connection = oleDbConnection4;
            oleDbCommand2.CommandText = "select * from NationTable Where NationID='" + str2 + "'";
            OleDbDataReader oleDbDataReader12 = oleDbCommand2.ExecuteReader();
            oleDbDataReader12.Read();
            string str7 = oleDbDataReader12.GetString(oleDbDataReader12.GetOrdinal("NationName"));
            oleDbDataReader12.Close();
            if (str3 != "00")
            {
              oleDbCommand2.CommandText = "select * from PrimaryPartitionTable Where NationID='" + str2 + "' and PrimaryPartitionId='" + str3 + "'";
              OleDbDataReader oleDbDataReader13 = oleDbCommand2.ExecuteReader();
              if (oleDbDataReader13.Read())
                str7 = oleDbDataReader13.GetString(oleDbDataReader13.GetOrdinal("PrimaryPartitionName"));
              oleDbDataReader13.Close();
            }
            oleDbCommand2.Connection = oleDbConnection2;
            OleDbCommand oleDbCommand5 = oleDbCommand2;
            string[] strArray = new string[6]
            {
              "Update FixedValueTable set FixedValue='",
              str7,
              "' where ClassifierID=",
              num7.ToString((IFormatProvider) CultureInfo.InvariantCulture),
              " And FixedValueID=",
              null
            };
            num11 = oleDbDataReader11.GetInt32(oleDbDataReader11.GetOrdinal("FixedValueID"));
            strArray[5] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            string str8 = string.Concat(strArray);
            oleDbCommand5.CommandText = str8;
            oleDbCommand2.ExecuteNonQuery();
          }
        }
        oleDbDataReader11.Close();
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if (this.engineReportArg.Percent + 5 > toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += 5;
        this.engineProgress.Report(this.engineReportArg);
        oleDbCommand1.Connection = oleDbConnection4;
        oleDbCommand1.CommandText = "select * from _EnergyTypeTable order by EnergyTypeId";
        OleDbDataReader oleDbDataReader14 = oleDbCommand1.ExecuteReader();
        oleDbCommand2.Connection = oleDbConnection1;
        while (oleDbDataReader14.Read())
        {
          OleDbCommand oleDbCommand6 = oleDbCommand2;
          string[] strArray = new string[9];
          strArray[0] = "insert into ClassValueTable(YearGuid,ClassifierId,ClassValueOrder,ClassValueName,ClassValueName1) Values({";
          strArray[1] = yearId.ToString();
          strArray[2] = "},";
          num11 = 14;
          strArray[3] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray[4] = ",";
          num11 = oleDbDataReader14.GetInt32(oleDbDataReader14.GetOrdinal("EnergyTypeId"));
          strArray[5] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray[6] = ",'";
          strArray[7] = oleDbDataReader14.GetString(oleDbDataReader14.GetOrdinal("EnergyTypeDescription")).Replace("'", "''");
          strArray[8] = "','')";
          string str = string.Concat(strArray);
          oleDbCommand6.CommandText = str;
          oleDbCommand2.ExecuteNonQuery();
        }
        oleDbDataReader14.Close();
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if (this.engineReportArg.Percent + 5 > toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += 5;
        this.engineProgress.Report(this.engineReportArg);
        oleDbCommand2.CommandText = "ALTER TABLE EstimationUnitsTable ADD convertedUnitID Integer";
        oleDbCommand2.Connection = oleDbConnection2;
        oleDbCommand2.ExecuteNonQuery();
        oleDbCommand1.Connection = oleDbConnection2;
        oleDbCommand1.CommandText = "Select * from EstimationUnitsTable";
        OleDbDataReader oleDbDataReader15 = oleDbCommand1.ExecuteReader();
        int[] numArray2 = new int[50];
        while (oleDbDataReader15.Read())
        {
          int int32 = oleDbDataReader15.GetInt32(oleDbDataReader15.GetOrdinal("EstimationUnitsId"));
          int unit = EngineModel.getUnit(oleDbConnection1, oleDbDataReader15.GetInt32(oleDbDataReader15.GetOrdinal("PrimaryUnitsId")), oleDbDataReader15.GetInt32(oleDbDataReader15.GetOrdinal("SecondaryUnitsId")), oleDbDataReader15.GetInt32(oleDbDataReader15.GetOrdinal("TertiaryUnitsId")));
          numArray2[int32] = unit;
          oleDbCommand2.CommandText = "UPDATE EstimationUnitsTable SET convertedUnitID=" + unit.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " WHERE EstimationUnitsId=" + int32.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          oleDbCommand2.ExecuteNonQuery();
        }
        oleDbDataReader15.Close();
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if (this.engineReportArg.Percent + 5 > toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += 5;
        this.engineProgress.Report(this.engineReportArg);
        oleDbCommand2.Connection = oleDbConnection1;
        oleDbCommand1.Connection = oleDbConnection2;
        oleDbCommand1.CommandText = "SELECT ClassifierId, FixedValueId as ClassValueId, FixedValue as ClassValue FROM FixedValueTable ORDER BY ClassifierId, FixedValueId Union SELECT ClassifierId, UserValueId as ClassValueId, UserValueName as ClassValue FROM UserValueTable";
        OleDbDataReader oleDbDataReader16 = oleDbCommand1.ExecuteReader();
        while (oleDbDataReader16.Read())
        {
          try
          {
            int int32 = oleDbDataReader16.GetInt32(oleDbDataReader16.GetOrdinal("ClassifierId"));
            if (int32 == index1)
            {
              if (oleDbDataReader16.GetInt32(oleDbDataReader16.GetOrdinal("ClassValueId")) > this.ClassValueForStrataStudyArea)
                this.ClassValueForStrataStudyArea = oleDbDataReader16.GetInt32(oleDbDataReader16.GetOrdinal("ClassValueId"));
              OleDbCommand oleDbCommand7 = oleDbCommand2;
              string[] strArray = new string[9];
              strArray[0] = "insert into ClassValueTable(YearGuid,ClassifierId,ClassValueOrder,ClassValueName,ClassValueName1) Values({";
              strArray[1] = yearId.ToString();
              strArray[2] = "},";
              num11 = 3;
              strArray[3] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray[4] = ",";
              num11 = oleDbDataReader16.GetInt32(oleDbDataReader16.GetOrdinal("ClassValueId"));
              strArray[5] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray[6] = ",'";
              strArray[7] = oleDbDataReader16.GetString(oleDbDataReader16.GetOrdinal("ClassValue")).Replace("'", "''");
              strArray[8] = "','')";
              string str = string.Concat(strArray);
              oleDbCommand7.CommandText = str;
              oleDbCommand2.ExecuteNonQuery();
            }
            else if (this.ClassValueForStrataStudyArea > 0)
            {
              ++this.ClassValueForStrataStudyArea;
              OleDbCommand oleDbCommand8 = oleDbCommand2;
              string[] strArray = new string[7]
              {
                "insert into ClassValueTable(YearGuid,ClassifierId,ClassValueOrder,ClassValueName,ClassValueName1) Values({",
                yearId.ToString(),
                "},",
                null,
                null,
                null,
                null
              };
              num11 = 3;
              strArray[3] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray[4] = ",";
              strArray[5] = this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray[6] = ",'Study Area','')";
              string str = string.Concat(strArray);
              oleDbCommand8.CommandText = str;
              oleDbCommand2.ExecuteNonQuery();
              this.ClassValueForStrataStudyArea = -this.ClassValueForStrataStudyArea;
            }
            if (int32 == num1)
            {
              if (oleDbDataReader16.GetInt32(oleDbDataReader16.GetOrdinal("ClassValueId")) > this.ClassValueForSpeciesAllSpecies)
                this.ClassValueForSpeciesAllSpecies = oleDbDataReader16.GetInt32(oleDbDataReader16.GetOrdinal("ClassValueId"));
              string key = oleDbDataReader16.GetString(oleDbDataReader16.GetOrdinal("ClassValue"));
              string str9 = this.allSpecies[key].SpeciesCommonName.Replace("'", "''");
              string str10 = this.allSpecies[key].SpeciesScientificName.Replace("'", "''");
              OleDbCommand oleDbCommand9 = oleDbCommand2;
              string[] strArray = new string[11];
              strArray[0] = "insert into ClassValueTable(YearGuid,ClassifierId,ClassValueOrder,ClassValueName,ClassValueName1) Values({";
              strArray[1] = yearId.ToString();
              strArray[2] = "},";
              num11 = 4;
              strArray[3] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray[4] = ",";
              num11 = oleDbDataReader16.GetInt32(oleDbDataReader16.GetOrdinal("ClassValueId"));
              strArray[5] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray[6] = ",'";
              strArray[7] = str9;
              strArray[8] = "','";
              strArray[9] = str10;
              strArray[10] = "')";
              string str11 = string.Concat(strArray);
              oleDbCommand9.CommandText = str11;
              oleDbCommand2.ExecuteNonQuery();
              this.allSpeciesClassValueOrderToSppCode.Add(oleDbDataReader16.GetInt32(oleDbDataReader16.GetOrdinal("ClassValueId")), key);
            }
            else if (this.ClassValueForSpeciesAllSpecies > 0)
            {
              ++this.ClassValueForSpeciesAllSpecies;
              OleDbCommand oleDbCommand10 = oleDbCommand2;
              string[] strArray = new string[7]
              {
                "insert into ClassValueTable(YearGuid,ClassifierId,ClassValueOrder,ClassValueName,ClassValueName1) Values({",
                yearId.ToString(),
                "},",
                null,
                null,
                null,
                null
              };
              num11 = 4;
              strArray[3] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray[4] = ",";
              strArray[5] = this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray[6] = ",'Total','Total')";
              string str = string.Concat(strArray);
              oleDbCommand10.CommandText = str;
              oleDbCommand2.ExecuteNonQuery();
              this.ClassValueForSpeciesAllSpecies = -this.ClassValueForSpeciesAllSpecies;
            }
            if (int32 == num9)
            {
              string str12 = this.allPests[oleDbDataReader16.GetString(oleDbDataReader16.GetOrdinal("ClassValue"))].CommonName.Replace("'", "''");
              string str13 = this.allPests[oleDbDataReader16.GetString(oleDbDataReader16.GetOrdinal("ClassValue"))].ScientificName.Replace("'", "''");
              OleDbCommand oleDbCommand11 = oleDbCommand2;
              string[] strArray = new string[11];
              strArray[0] = "insert into ClassValueTable(YearGuid,ClassifierId,ClassValueOrder,ClassValueName,ClassValueName1) Values({";
              strArray[1] = yearId.ToString();
              strArray[2] = "},";
              num11 = 37;
              strArray[3] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray[4] = ",";
              num11 = oleDbDataReader16.GetInt32(oleDbDataReader16.GetOrdinal("ClassValueId"));
              strArray[5] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray[6] = ",'";
              strArray[7] = str12;
              strArray[8] = "','";
              strArray[9] = str13;
              strArray[10] = "')";
              string str14 = string.Concat(strArray);
              oleDbCommand11.CommandText = str14;
              oleDbCommand2.ExecuteNonQuery();
            }
            if (int32 != index1)
            {
              if (int32 != num1)
              {
                if (int32 != num9)
                {
                  OleDbCommand oleDbCommand12 = oleDbCommand2;
                  string[] strArray = new string[9]
                  {
                    "insert into ClassValueTable(YearGuid,ClassifierId,ClassValueOrder,ClassValueName,ClassValueName1) Values({",
                    yearId.ToString(),
                    "},",
                    numArray1[int32].ToString((IFormatProvider) CultureInfo.InvariantCulture),
                    ",",
                    null,
                    null,
                    null,
                    null
                  };
                  num11 = oleDbDataReader16.GetInt32(oleDbDataReader16.GetOrdinal("ClassValueId"));
                  strArray[5] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                  strArray[6] = ",'";
                  strArray[7] = oleDbDataReader16.GetString(oleDbDataReader16.GetOrdinal("ClassValue")).Replace("'", "''");
                  strArray[8] = "','')";
                  string str = string.Concat(strArray);
                  oleDbCommand12.CommandText = str;
                  oleDbCommand2.ExecuteNonQuery();
                }
              }
            }
          }
          catch (Exception ex)
          {
            throw;
          }
        }
        oleDbDataReader16.Close();
        this.ClassValueForStrataStudyArea = -this.ClassValueForStrataStudyArea;
        this.ClassValueForSpeciesAllSpecies = -this.ClassValueForSpeciesAllSpecies;
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if (this.engineReportArg.Percent + 5 > toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += 5;
        this.engineProgress.Report(this.engineReportArg);
        int[] numArray3 = new int[10];
        int[] numArray4 = new int[10];
        string[] strArray1 = new string[10];
        string[] strArray2 = new string[10];
        int[] numArray5 = new int[10];
        int[] numArray6 = new int[10];
        string[] strArray3 = new string[10];
        string[] strArray4 = new string[10];
        oleDbCommand2.Connection = oleDbConnection2;
        int num12 = -1;
        OleDbCommand oleDbCommand13 = oleDbCommand2;
        string[] strArray5 = new string[6];
        strArray5[0] = "select * from EstimationUnitsTable where PrimaryUnitsId=";
        num11 = 3;
        strArray5[1] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray5[2] = " and SecondaryUnitsId=";
        num11 = 1;
        strArray5[3] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray5[4] = " and TertiaryUnitsId=";
        num11 = 1;
        strArray5[5] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        string str15 = string.Concat(strArray5);
        oleDbCommand13.CommandText = str15;
        OleDbDataReader oleDbDataReader17 = oleDbCommand2.ExecuteReader();
        if (oleDbDataReader17.Read())
          num12 = oleDbDataReader17.GetInt32(oleDbDataReader17.GetOrdinal("EstimationUnitsId"));
        oleDbDataReader17.Close();
        oleDbCommand1.Connection = oleDbConnection2;
        oleDbCommand1.CommandText = "select count(*) from TableOfStatisticalEstimates";
        OleDbDataReader oleDbDataReader18 = oleDbCommand1.ExecuteReader();
        oleDbDataReader18.Read();
        int int32_1 = oleDbDataReader18.GetInt32(0);
        oleDbDataReader18.Close();
        int percent = this.engineReportArg.Percent;
        double num13 = (double) (((toPercent - fromPercent) / 2 - this.engineReportArg.Percent) / int32_1);
        oleDbCommand1.CommandText = "select * from TableOfStatisticalEstimates";
        OleDbDataReader oleDbDataReader19 = oleDbCommand1.ExecuteReader();
        int num14 = 0;
        Classifiers classifiers;
        while (oleDbDataReader19.Read())
        {
          try
          {
            int int32_2 = oleDbDataReader19.GetInt32(oleDbDataReader19.GetOrdinal("StatisticalTableId"));
            string str16 = oleDbDataReader19.GetString(oleDbDataReader19.GetOrdinal("TableTitle"));
            int int16_1 = (int) oleDbDataReader19.GetInt16(oleDbDataReader19.GetOrdinal("TableData"));
            if (seriesSampleType == SampleType.Inventory)
            {
              oleDbCommand2.CommandText = "UPDATE [" + str16 + "] SET EstimateStandardError=0";
              oleDbCommand2.ExecuteNonQuery();
            }
            if (str16.Length < 15)
            {
              oleDbCommand2.CommandText = "UPDATE [" + str16 + "] SET EstimateValue=100*EstimateValue, EstimateStandardError=100*EstimateStandardError WHERE EstimationUnitsId=" + num12.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              oleDbCommand2.ExecuteNonQuery();
            }
            else if (str16.Substring(0, 15).ToLower() != "stratacovertype")
            {
              oleDbCommand2.CommandText = "UPDATE [" + str16 + "] SET EstimateValue=100*EstimateValue, EstimateStandardError=100*EstimateStandardError WHERE EstimationUnitsId=" + num12.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              oleDbCommand2.ExecuteNonQuery();
            }
            int int16_2 = (int) oleDbDataReader19.GetInt16(oleDbDataReader19.GetOrdinal("TableData"));
            oleDbCommand2.CommandText = "select * from PartitionDefinitionsTable where StatisticalTableId=" + int32_2.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " order by PartitionOrder";
            int index2 = 0;
            OleDbDataReader oleDbDataReader20 = oleDbCommand2.ExecuteReader();
            while (oleDbDataReader20.Read())
            {
              ++index2;
              numArray3[index2] = oleDbDataReader20.GetInt32(oleDbDataReader20.GetOrdinal("ClassifierId"));
              strArray1[index2] = oleDbDataReader20.GetString(oleDbDataReader20.GetOrdinal("PartitionName"));
              numArray5[index2] = numArray1[numArray3[index2]];
              string[] strArray6 = strArray3;
              int index3 = index2;
              classifiers = (Classifiers) numArray5[index2];
              string str17 = classifiers.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray6[index3] = str17;
              if (numArray3[index2] == index1)
              {
                oleDbCommand3.Connection = oleDbConnection2;
                oleDbCommand3.CommandText = "Update [" + str16 + "] set [" + strArray1[index2] + "]=" + this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " where [" + strArray1[index2] + "]=0";
                oleDbCommand3.ExecuteNonQuery();
              }
              if (numArray3[index2] == num1)
              {
                oleDbCommand3.Connection = oleDbConnection2;
                oleDbCommand3.CommandText = "Update [" + str16 + "] set [" + strArray1[index2] + "]=" + this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " where [" + strArray1[index2] + "]=0";
                oleDbCommand3.ExecuteNonQuery();
              }
            }
            oleDbDataReader20.Close();
            List<int> Classifiers = new List<int>();
            if (numArray3[2] == num4)
            {
              oleDbCommand2.CommandText = "UPDATE [" + str16 + "] SET EstimateTypeId = 10 WHERE EstimateTypeId = 17";
              oleDbCommand2.ExecuteNonQuery();
              oleDbCommand2.CommandText = "UPDATE [" + str16 + "] SET EstimateTypeId = 9 WHERE EstimateTypeId = 18";
              oleDbCommand2.ExecuteNonQuery();
              oleDbCommand2.CommandText = "UPDATE [" + str16 + "] SET EstimateTypeId = 15 WHERE EstimateTypeId = 19";
              oleDbCommand2.ExecuteNonQuery();
            }
            if (numArray3[2] == num1 && index2 == 2)
            {
              Classifiers.Clear();
              for (int index4 = 1; index4 <= index2; ++index4)
                Classifiers.Add(numArray1[numArray3[index4]]);
              string estimationTable = EngineModel.getEstimationTable(oleDbConnection1, int16_2, Classifiers);
              string[] strArray7 = new string[33];
              strArray7[0] = "INSERT INTO [";
              strArray7[1] = estimationTable;
              strArray7[2] = "] (YearGuid,[";
              strArray7[3] = strArray3[1];
              strArray7[4] = "],[";
              strArray7[5] = strArray3[2];
              strArray7[6] = "], EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId)  IN '";
              strArray7[7] = inputDB;
              strArray7[8] = "'  SELECT {";
              strArray7[9] = yearId.ToString();
              strArray7[10] = "},[";
              strArray7[11] = strArray1[1];
              strArray7[12] = "],[";
              strArray7[13] = strArray1[2];
              strArray7[14] = "], EstimateTypeId,";
              num11 = 1;
              strArray7[15] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray7[16] = ",EstimateValue,EstimateStandardError,convertedUnitID  FROM [";
              strArray7[17] = str16;
              strArray7[18] = "] INNER JOIN EstimationUnitsTable ON [";
              strArray7[19] = str16;
              strArray7[20] = "].EstimationUnitsId = EstimationUnitsTable.EstimationUnitsId  WHERE [";
              strArray7[21] = strArray1[1];
              strArray7[22] = "]<>";
              strArray7[23] = this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray7[24] = " or ([";
              strArray7[25] = strArray1[1];
              strArray7[26] = "]=";
              strArray7[27] = this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray7[28] = " and [";
              strArray7[29] = strArray1[2];
              strArray7[30] = "]=";
              strArray7[31] = this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray7[32] = ")";
              string str18 = string.Concat(strArray7);
              oleDbCommand3.Connection = oleDbConnection2;
              oleDbCommand3.CommandText = str18;
              oleDbCommand3.ExecuteNonQuery();
              Classifiers.Clear();
              for (int index5 = 1; index5 <= index2 - 1; ++index5)
                Classifiers.Add(numArray1[numArray3[index5]]);
              string[] strArray8 = new string[20]
              {
                "INSERT INTO [",
                EngineModel.getEstimationTable(oleDbConnection1, int16_2, Classifiers),
                "] (YearGuid,[",
                strArray3[1],
                "], EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId)  IN '",
                inputDB,
                "'  SELECT {",
                yearId.ToString(),
                "},[",
                strArray1[1],
                "], EstimateTypeId,",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
              };
              num11 = 1;
              strArray8[11] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray8[12] = ",EstimateValue,EstimateStandardError,convertedUnitID  FROM [";
              strArray8[13] = str16;
              strArray8[14] = "] INNER JOIN EstimationUnitsTable ON [";
              strArray8[15] = str16;
              strArray8[16] = "].EstimationUnitsId = EstimationUnitsTable.EstimationUnitsId  WHERE [";
              strArray8[17] = strArray1[2];
              strArray8[18] = "]=";
              strArray8[19] = this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              string str19 = string.Concat(strArray8);
              oleDbCommand3.Connection = oleDbConnection2;
              oleDbCommand3.CommandText = str19;
              oleDbCommand3.ExecuteNonQuery();
              Classifiers.Clear();
              Classifiers.Add(numArray1[numArray3[2]]);
              string[] strArray9 = new string[20]
              {
                "INSERT INTO [",
                EngineModel.getEstimationTable(oleDbConnection1, int16_2, Classifiers),
                "] (YearGuid,[",
                strArray3[2],
                "], EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId)  IN '",
                inputDB,
                "'  SELECT {",
                yearId.ToString(),
                "},[",
                strArray1[2],
                "], EstimateTypeId,",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
              };
              num11 = 1;
              strArray9[11] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray9[12] = ",EstimateValue,EstimateStandardError,convertedUnitID  FROM [";
              strArray9[13] = str16;
              strArray9[14] = "] INNER JOIN EstimationUnitsTable ON [";
              strArray9[15] = str16;
              strArray9[16] = "].EstimationUnitsId = EstimationUnitsTable.EstimationUnitsId  WHERE [";
              strArray9[17] = strArray1[1];
              strArray9[18] = "]=";
              strArray9[19] = this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              string str20 = string.Concat(strArray9);
              oleDbCommand3.Connection = oleDbConnection2;
              oleDbCommand3.CommandText = str20;
              oleDbCommand3.ExecuteNonQuery();
            }
            else if (numArray3[2] == num1 && index2 > 2)
            {
              Classifiers.Clear();
              for (int index6 = 1; index6 <= index2; ++index6)
              {
                Classifiers.Add(numArray1[numArray3[index6]]);
                strArray2[index6] = strArray1[index6];
                strArray4[index6] = strArray3[index6];
              }
              string estimationTable1 = EngineModel.getEstimationTable(oleDbConnection1, int16_2, Classifiers);
              string str21 = "";
              string str22 = "";
              string str23 = " where ([" + strArray2[1] + "]<>" + this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " Or ([" + strArray2[1] + "]=" + this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and [" + strArray2[2] + "]=" + this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "))";
              int num15 = index2;
              for (int index7 = 1; index7 <= num15; ++index7)
              {
                str21 = str21 + "[" + strArray2[index7] + "],";
                str22 = str22 + "[" + strArray4[index7] + "],";
              }
              for (int index8 = 3; index8 <= num15; ++index8)
                str23 = str23 + " and [" + strArray2[index8] + "] <>0";
              string[] strArray10 = new string[18]
              {
                "INSERT INTO [",
                estimationTable1,
                "] (YearGuid,",
                str22,
                " EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId)  IN '",
                inputDB,
                "'  SELECT {",
                yearId.ToString(),
                "},",
                str21,
                " EstimateTypeId,",
                null,
                null,
                null,
                null,
                null,
                null,
                null
              };
              num11 = 1;
              strArray10[11] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray10[12] = ",EstimateValue,EstimateStandardError,convertedUnitID  FROM [";
              strArray10[13] = str16;
              strArray10[14] = "] INNER JOIN EstimationUnitsTable ON [";
              strArray10[15] = str16;
              strArray10[16] = "].EstimationUnitsId = EstimationUnitsTable.EstimationUnitsId  ";
              strArray10[17] = str23;
              string str24 = string.Concat(strArray10);
              oleDbCommand3.Connection = oleDbConnection2;
              oleDbCommand3.CommandText = str24;
              oleDbCommand3.ExecuteNonQuery();
              string str25 = "";
              string str26 = "";
              string str27 = "";
              Classifiers.Clear();
              for (int index9 = 2; index9 <= index2; ++index9)
              {
                Classifiers.Add(numArray1[numArray3[index9]]);
                strArray2[index9 - 1] = strArray1[index9];
                strArray4[index9 - 1] = strArray3[index9];
              }
              string estimationTable2 = EngineModel.getEstimationTable(oleDbConnection1, int16_2, Classifiers);
              string str28 = "";
              string str29 = "";
              string str30 = " where [" + strArray1[1] + "]=" + this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              int num16 = index2 - 1;
              for (int index10 = 1; index10 <= num16; ++index10)
              {
                str28 = str28 + "[" + strArray2[index10] + "],";
                str29 = str29 + "[" + strArray4[index10] + "],";
              }
              for (int index11 = 2; index11 <= num16; ++index11)
                str30 = str30 + " and [" + strArray2[index11] + "] <>0";
              string[] strArray11 = new string[18]
              {
                "INSERT INTO [",
                estimationTable2,
                "] (YearGuid,",
                str29,
                " EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId)  IN '",
                inputDB,
                "'  SELECT {",
                yearId.ToString(),
                "},",
                str28,
                " EstimateTypeId,",
                null,
                null,
                null,
                null,
                null,
                null,
                null
              };
              num11 = 1;
              strArray11[11] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray11[12] = ",EstimateValue,EstimateStandardError,convertedUnitID  FROM [";
              strArray11[13] = str16;
              strArray11[14] = "] INNER JOIN EstimationUnitsTable ON [";
              strArray11[15] = str16;
              strArray11[16] = "].EstimationUnitsId = EstimationUnitsTable.EstimationUnitsId  ";
              strArray11[17] = str30;
              string str31 = string.Concat(strArray11);
              oleDbCommand3.Connection = oleDbConnection2;
              oleDbCommand3.CommandText = str31;
              oleDbCommand3.ExecuteNonQuery();
              strArray2[1] = strArray1[1];
              strArray4[1] = strArray3[1];
              str25 = "";
              str26 = "";
              str27 = "";
              Classifiers[0] = numArray1[index1];
              string estimationTable3 = EngineModel.getEstimationTable(oleDbConnection1, int16_2, Classifiers);
              string str32 = "";
              string str33 = "";
              string str34 = " where [" + strArray1[2] + "]=" + this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              int num17 = index2 - 1;
              for (int index12 = 1; index12 <= num17; ++index12)
              {
                str32 = str32 + "[" + strArray2[index12] + "],";
                str33 = str33 + "[" + strArray4[index12] + "],";
              }
              for (int index13 = 2; index13 <= num17; ++index13)
                str34 = str34 + " and [" + strArray2[index13] + "] <>0";
              string[] strArray12 = new string[18]
              {
                "INSERT INTO [",
                estimationTable3,
                "] (YearGuid,",
                str33,
                " EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId)  IN '",
                inputDB,
                "'  SELECT {",
                yearId.ToString(),
                "},",
                str32,
                " EstimateTypeId,",
                null,
                null,
                null,
                null,
                null,
                null,
                null
              };
              num11 = 1;
              strArray12[11] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray12[12] = ",EstimateValue,EstimateStandardError,convertedUnitID  FROM [";
              strArray12[13] = str16;
              strArray12[14] = "] INNER JOIN EstimationUnitsTable ON [";
              strArray12[15] = str16;
              strArray12[16] = "].EstimationUnitsId = EstimationUnitsTable.EstimationUnitsId  ";
              strArray12[17] = str34;
              string str35 = string.Concat(strArray12);
              oleDbCommand3.Connection = oleDbConnection2;
              oleDbCommand3.CommandText = str35;
              oleDbCommand3.ExecuteNonQuery();
              if (index2 == 4)
              {
                if (numArray3[1] == index1)
                {
                  if (numArray3[2] == num1)
                  {
                    if (numArray3[3] == num2)
                    {
                      if (numArray3[4] == num3)
                      {
                        Classifiers.Clear();
                        Classifiers.Add(numArray1[numArray3[2]]);
                        Classifiers.Add(numArray1[numArray3[4]]);
                        string estimationTable4 = EngineModel.getEstimationTable(oleDbConnection1, int16_2, Classifiers);
                        string str36 = "[" + strArray1[2] + "],[" + strArray1[4] + "],";
                        string str37 = "[" + strArray3[2] + "],[" + strArray3[4] + "],";
                        string str38 = " where [" + strArray1[1] + "]=" + this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and [" + strArray1[3] + "] = 0 and [" + strArray1[4] + "]<>0";
                        string[] strArray13 = new string[18]
                        {
                          "INSERT INTO [",
                          estimationTable4,
                          "] (YearGuid,",
                          str37,
                          " EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId)  IN '",
                          inputDB,
                          "'  SELECT {",
                          yearId.ToString(),
                          "},",
                          str36,
                          " EstimateTypeId,",
                          null,
                          null,
                          null,
                          null,
                          null,
                          null,
                          null
                        };
                        num11 = 1;
                        strArray13[11] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                        strArray13[12] = ",EstimateValue,EstimateStandardError,convertedUnitID  FROM [";
                        strArray13[13] = str16;
                        strArray13[14] = "] INNER JOIN EstimationUnitsTable ON [";
                        strArray13[15] = str16;
                        strArray13[16] = "].EstimationUnitsId = EstimationUnitsTable.EstimationUnitsId  ";
                        strArray13[17] = str38;
                        string str39 = string.Concat(strArray13);
                        oleDbCommand3.Connection = oleDbConnection2;
                        oleDbCommand3.CommandText = str39;
                        oleDbCommand3.ExecuteNonQuery();
                        Classifiers.Clear();
                        Classifiers.Add(numArray1[numArray3[1]]);
                        Classifiers.Add(numArray1[numArray3[2]]);
                        Classifiers.Add(numArray1[numArray3[4]]);
                        string estimationTable5 = EngineModel.getEstimationTable(oleDbConnection1, int16_2, Classifiers);
                        string str40 = "[" + strArray1[1] + "],[" + strArray1[2] + "],[" + strArray1[4] + "],";
                        string str41 = "[" + strArray3[1] + "],[" + strArray3[2] + "],[" + strArray3[4] + "],";
                        string str42 = " where ([" + strArray1[1] + "]<>" + this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " or ([" + strArray1[1] + "]=" + this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and [" + strArray1[2] + "]=" + this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ")) and [" + strArray1[3] + "] = 0 and [" + strArray1[4] + "]<>0";
                        string[] strArray14 = new string[18]
                        {
                          "INSERT INTO [",
                          estimationTable5,
                          "] (YearGuid,",
                          str41,
                          " EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId)  IN '",
                          inputDB,
                          "'  SELECT {",
                          yearId.ToString(),
                          "},",
                          str40,
                          " EstimateTypeId,",
                          null,
                          null,
                          null,
                          null,
                          null,
                          null,
                          null
                        };
                        num11 = 1;
                        strArray14[11] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                        strArray14[12] = ",EstimateValue,EstimateStandardError,convertedUnitID  FROM [";
                        strArray14[13] = str16;
                        strArray14[14] = "] INNER JOIN EstimationUnitsTable ON [";
                        strArray14[15] = str16;
                        strArray14[16] = "].EstimationUnitsId = EstimationUnitsTable.EstimationUnitsId  ";
                        strArray14[17] = str42;
                        string str43 = string.Concat(strArray14);
                        oleDbCommand3.Connection = oleDbConnection2;
                        oleDbCommand3.CommandText = str43;
                        oleDbCommand3.ExecuteNonQuery();
                      }
                    }
                  }
                }
              }
            }
            else
            {
              Classifiers.Clear();
              for (int index14 = 1; index14 <= index2; ++index14)
              {
                Classifiers.Add(numArray1[numArray3[index14]]);
                strArray2[index14] = strArray1[index14];
                strArray4[index14] = strArray3[index14];
              }
              string estimationTable = EngineModel.getEstimationTable(oleDbConnection1, int16_2, Classifiers);
              string str44 = "";
              string str45 = "";
              string str46 = "";
              int num18 = index2;
              for (int index15 = 1; index15 <= num18; ++index15)
              {
                str44 = str44 + "[" + strArray2[index15] + "],";
                str45 = str45 + "[" + strArray4[index15] + "],";
              }
              for (int index16 = 2; index16 <= num18; ++index16)
                str46 = index16 != 2 ? str46 + " and [" + strArray2[index16] + "] <>0" : " WHERE [" + strArray2[index16] + "] <>0";
              string[] strArray15 = new string[18]
              {
                "INSERT INTO [",
                estimationTable,
                "] (YearGuid,",
                str45,
                " EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId)  IN '",
                inputDB,
                "'  SELECT {",
                yearId.ToString(),
                "},",
                str44,
                " EstimateTypeId,",
                null,
                null,
                null,
                null,
                null,
                null,
                null
              };
              num11 = 1;
              strArray15[11] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              strArray15[12] = ",EstimateValue,EstimateStandardError,convertedUnitID  FROM [";
              strArray15[13] = str16;
              strArray15[14] = "] INNER JOIN EstimationUnitsTable ON [";
              strArray15[15] = str16;
              strArray15[16] = "].EstimationUnitsId = EstimationUnitsTable.EstimationUnitsId  ";
              strArray15[17] = str46;
              string str47 = string.Concat(strArray15);
              oleDbCommand3.Connection = oleDbConnection2;
              oleDbCommand3.CommandText = str47;
              oleDbCommand3.ExecuteNonQuery();
            }
          }
          catch (Exception ex)
          {
            throw;
          }
          ++num14;
          if (this.engineCancellationToken.IsCancellationRequested)
            throw new Exception("User cancelled");
          if (percent + (int) ((double) num14 * num13) >= this.engineReportArg.Percent + 1)
          {
            this.engineReportArg.Percent = percent + (int) ((double) num14 * num13);
            this.engineProgress.Report(this.engineReportArg);
          }
        }
        oleDbDataReader19.Close();
        double num19 = (double) (toPercent - this.engineReportArg.Percent) / 7.0;
        oleDbCommand1.Connection = oleDbConnection1;
        OleDbCommand oleDbCommand14 = oleDbCommand1;
        string[] strArray16 = new string[5]
        {
          "select * from ClassValueTable where YearGUID={",
          this.currYear.Guid.ToString(),
          "} and ClassifierId=",
          null,
          null
        };
        num11 = 37;
        strArray16[3] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray16[4] = " and ClassValueName='Affected'";
        string str48 = string.Concat(strArray16);
        oleDbCommand14.CommandText = str48;
        OleDbDataReader oleDbDataReader21 = oleDbCommand1.ExecuteReader();
        oleDbDataReader21.Read();
        int int16_3 = (int) oleDbDataReader21.GetInt16(oleDbDataReader21.GetOrdinal("ClassValueOrder"));
        oleDbDataReader21.Close();
        OleDbCommand oleDbCommand15 = oleDbCommand1;
        string[] strArray17 = new string[5]
        {
          "select * from ClassValueTable where YearGUID={",
          this.currYear.Guid.ToString(),
          "} and ClassifierId=",
          null,
          null
        };
        num11 = 38;
        strArray17[3] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray17[4] = " and ClassValueName='Affected'";
        string str49 = string.Concat(strArray17);
        oleDbCommand15.CommandText = str49;
        OleDbDataReader oleDbDataReader22 = oleDbCommand1.ExecuteReader();
        oleDbDataReader22.Read();
        int int16_4 = (int) oleDbDataReader22.GetInt16(oleDbDataReader22.GetOrdinal("ClassValueOrder"));
        oleDbDataReader22.Close();
        List<int> Classifiers1 = new List<int>();
        List<int> Classifiers2 = new List<int>();
        Classifiers1.Add(3);
        Classifiers1.Add(37);
        Classifiers2.Add(3);
        Classifiers2.Add(38);
        string estimationTable6 = EngineModel.getEstimationTable(oleDbConnection1, 2, Classifiers1);
        string estimationTable7 = EngineModel.getEstimationTable(oleDbConnection1, 2, Classifiers2);
        OleDbCommand oleDbCommand16 = oleDbCommand1;
        string[] strArray18 = new string[22];
        strArray18[0] = "INSERT INTO [";
        strArray18[1] = estimationTable6;
        strArray18[2] = "] ( YearGuid, ";
        classifiers = Classifiers.Strata;
        strArray18[3] = classifiers.ToString();
        strArray18[4] = ", ";
        classifiers = Classifiers.PestPest;
        strArray18[5] = classifiers.ToString();
        strArray18[6] = ", EstimateType, EquationType, EstimateValue, EstimateStandardError, EstimateUnitsId )  SELECT YearGuid, ";
        classifiers = Classifiers.Strata;
        strArray18[7] = classifiers.ToString();
        strArray18[8] = ", ";
        strArray18[9] = int16_3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray18[10] = ", EstimateType,EquationType,EstimateValue, EstimateStandardError, EstimateUnitsId  FROM [";
        strArray18[11] = estimationTable7;
        strArray18[12] = "]  WHERE YearGuid={guid {";
        strArray18[13] = this.currYear.Guid.ToString();
        strArray18[14] = "}} AND ";
        classifiers = Classifiers.Strata;
        strArray18[15] = classifiers.ToString();
        strArray18[16] = "<>";
        strArray18[17] = this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray18[18] = " AND ";
        classifiers = Classifiers.PestIndicator;
        strArray18[19] = classifiers.ToString();
        strArray18[20] = "=";
        strArray18[21] = int16_4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        string str50 = string.Concat(strArray18);
        oleDbCommand16.CommandText = str50;
        oleDbCommand1.ExecuteNonQuery();
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if ((double) this.engineReportArg.Percent + num19 > (double) toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += (int) num19;
        this.engineProgress.Report(this.engineReportArg);
        Classifiers1.Clear();
        Classifiers2.Clear();
        Classifiers1.Add(3);
        Classifiers1.Add(4);
        Classifiers1.Add(37);
        Classifiers2.Add(3);
        Classifiers2.Add(4);
        Classifiers2.Add(38);
        string estimationTable8 = EngineModel.getEstimationTable(oleDbConnection1, 2, Classifiers1);
        string estimationTable9 = EngineModel.getEstimationTable(oleDbConnection1, 2, Classifiers2);
        OleDbCommand oleDbCommand17 = oleDbCommand1;
        string[] strArray19 = new string[30];
        strArray19[0] = "INSERT INTO [";
        strArray19[1] = estimationTable8;
        strArray19[2] = "] ( YearGuid, ";
        classifiers = Classifiers.Strata;
        strArray19[3] = classifiers.ToString();
        strArray19[4] = ", ";
        classifiers = Classifiers.Species;
        strArray19[5] = classifiers.ToString();
        strArray19[6] = ", ";
        classifiers = Classifiers.PestPest;
        strArray19[7] = classifiers.ToString();
        strArray19[8] = ", EstimateType, EquationType, EstimateValue, EstimateStandardError, EstimateUnitsId )  SELECT YearGuid, ";
        classifiers = Classifiers.Strata;
        strArray19[9] = classifiers.ToString();
        strArray19[10] = ", ";
        classifiers = Classifiers.Species;
        strArray19[11] = classifiers.ToString();
        strArray19[12] = ", ";
        strArray19[13] = int16_3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray19[14] = ", EstimateType,EquationType,EstimateValue, EstimateStandardError, EstimateUnitsId  FROM [";
        strArray19[15] = estimationTable9;
        strArray19[16] = "]  WHERE YearGuid={guid {";
        Guid guid = this.currYear.Guid;
        strArray19[17] = guid.ToString();
        strArray19[18] = "}} AND ";
        classifiers = Classifiers.Strata;
        strArray19[19] = classifiers.ToString();
        strArray19[20] = "<>";
        strArray19[21] = this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray19[22] = " AND ";
        classifiers = Classifiers.Species;
        strArray19[23] = classifiers.ToString();
        strArray19[24] = "<>";
        strArray19[25] = this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray19[26] = " AND ";
        classifiers = Classifiers.PestIndicator;
        strArray19[27] = classifiers.ToString();
        strArray19[28] = "=";
        strArray19[29] = int16_4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        string str51 = string.Concat(strArray19);
        oleDbCommand17.CommandText = str51;
        oleDbCommand1.ExecuteNonQuery();
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if ((double) this.engineReportArg.Percent + num19 > (double) toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += (int) num19;
        this.engineProgress.Report(this.engineReportArg);
        Classifiers1.Clear();
        Classifiers2.Clear();
        Classifiers1.Add(4);
        Classifiers1.Add(37);
        Classifiers2.Add(4);
        Classifiers2.Add(38);
        string estimationTable10 = EngineModel.getEstimationTable(oleDbConnection1, 2, Classifiers1);
        string estimationTable11 = EngineModel.getEstimationTable(oleDbConnection1, 2, Classifiers2);
        OleDbCommand oleDbCommand18 = oleDbCommand1;
        string[] strArray20 = new string[22];
        strArray20[0] = "INSERT INTO [";
        strArray20[1] = estimationTable10;
        strArray20[2] = "] ( YearGuid, ";
        classifiers = Classifiers.Species;
        strArray20[3] = classifiers.ToString();
        strArray20[4] = ", ";
        classifiers = Classifiers.PestPest;
        strArray20[5] = classifiers.ToString();
        strArray20[6] = ", EstimateType, EquationType, EstimateValue, EstimateStandardError, EstimateUnitsId )  SELECT YearGuid, ";
        classifiers = Classifiers.Species;
        strArray20[7] = classifiers.ToString();
        strArray20[8] = ", ";
        strArray20[9] = int16_3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray20[10] = ", EstimateType,EquationType,EstimateValue, EstimateStandardError, EstimateUnitsId  FROM [";
        strArray20[11] = estimationTable11;
        strArray20[12] = "]  WHERE YearGuid={guid {";
        guid = this.currYear.Guid;
        strArray20[13] = guid.ToString();
        strArray20[14] = "}} AND ";
        classifiers = Classifiers.Species;
        strArray20[15] = classifiers.ToString();
        strArray20[16] = "<>";
        strArray20[17] = this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray20[18] = " AND ";
        classifiers = Classifiers.PestIndicator;
        strArray20[19] = classifiers.ToString();
        strArray20[20] = "=";
        strArray20[21] = int16_4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        string str52 = string.Concat(strArray20);
        oleDbCommand18.CommandText = str52;
        oleDbCommand1.ExecuteNonQuery();
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if ((double) this.engineReportArg.Percent + num19 > (double) toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += (int) num19;
        this.engineProgress.Report(this.engineReportArg);
        List<int> Classifiers3 = new List<int>();
        Classifiers3.Add(3);
        Classifiers3.Add(6);
        string estimationTable12 = EngineModel.getEstimationTable(oleDbConnection1, 2, Classifiers3);
        int unit1 = EngineModel.getUnit(oleDbConnection1, 3, 1, 1);
        OleDbCommand oleDbCommand19 = oleDbCommand1;
        string[] strArray21 = new string[8];
        strArray21[0] = "select * from [";
        strArray21[1] = estimationTable12;
        strArray21[2] = "] where YearGUID={";
        guid = this.currYear.Guid;
        strArray21[3] = guid.ToString();
        strArray21[4] = "} and  EstimateType=";
        num11 = 1;
        strArray21[5] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray21[6] = " and  EstimateUnitsId =";
        strArray21[7] = unit1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        string str53 = string.Concat(strArray21);
        oleDbCommand19.CommandText = str53;
        OleDbDataReader oleDbDataReader23 = oleDbCommand1.ExecuteReader();
        if (oleDbDataReader23.HasRows)
        {
          oleDbDataReader23.Close();
        }
        else
        {
          oleDbDataReader23.Close();
          OleDbCommand oleDbCommand20 = oleDbCommand1;
          string[] strArray22 = new string[5]
          {
            "select * from ClassValueTable where YearGUID={",
            null,
            null,
            null,
            null
          };
          guid = this.currYear.Guid;
          strArray22[1] = guid.ToString();
          strArray22[2] = "} and  ClassifierId=";
          num11 = 41;
          strArray22[3] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray22[4] = " and  ClassValueName = 'Living'";
          string str54 = string.Concat(strArray22);
          oleDbCommand20.CommandText = str54;
          OleDbDataReader oleDbDataReader24 = oleDbCommand1.ExecuteReader();
          oleDbDataReader24.Read();
          int int16_5 = (int) oleDbDataReader24.GetInt16(oleDbDataReader24.GetOrdinal("ClassValueOrder"));
          oleDbDataReader24.Close();
          int num20 = 0;
          OleDbCommand oleDbCommand21 = oleDbCommand1;
          guid = this.currYear.Guid;
          string str55 = guid.ToString();
          num11 = 8;
          string str56 = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str57 = "select * from ClassValueTable where YearGUID={" + str55 + "} and  ClassifierId=" + str56;
          oleDbCommand21.CommandText = str57;
          OleDbDataReader oleDbDataReader25 = oleDbCommand1.ExecuteReader();
          while (oleDbDataReader25.Read())
          {
            if (oleDbDataReader25.GetString(oleDbDataReader25.GetOrdinal("ClassValueName")).ToLower() != "others")
              num20 = (int) oleDbDataReader25.GetInt16(oleDbDataReader25.GetOrdinal("ClassValueOrder"));
          }
          oleDbDataReader25.Close();
          int num21 = 0;
          int num22 = 0;
          OleDbCommand oleDbCommand22 = oleDbCommand1;
          guid = this.currYear.Guid;
          string str58 = guid.ToString();
          num11 = 6;
          string str59 = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str60 = "select * from ClassValueTable where YearGUID={" + str58 + "} and  ClassifierId=" + str59;
          oleDbCommand22.CommandText = str60;
          OleDbDataReader oleDbDataReader26 = oleDbCommand1.ExecuteReader();
          while (oleDbDataReader26.Read())
          {
            if (num22 < (int) oleDbDataReader26.GetInt16(oleDbDataReader26.GetOrdinal("ClassValueOrder")))
              num22 = (int) oleDbDataReader26.GetInt16(oleDbDataReader26.GetOrdinal("ClassValueOrder"));
            if (oleDbDataReader26.GetString(oleDbDataReader26.GetOrdinal("ClassValueName")).ToLower() == "state")
              num21 = (int) oleDbDataReader26.GetInt16(oleDbDataReader26.GetOrdinal("ClassValueOrder"));
          }
          oleDbDataReader26.Close();
          if (num21 == 0)
          {
            num21 = num22 + 1;
            OleDbCommand oleDbCommand23 = oleDbCommand1;
            string[] strArray23 = new string[7]
            {
              "insert into ClassValueTable(YearGuid,ClassifierId,ClassValueOrder,ClassValueName,ClassValueName1) Values({",
              yearId.ToString(),
              "},",
              null,
              null,
              null,
              null
            };
            num11 = 6;
            strArray23[3] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            strArray23[4] = ",";
            strArray23[5] = num21.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            strArray23[6] = ",'STATE','')";
            string str61 = string.Concat(strArray23);
            oleDbCommand23.CommandText = str61;
            oleDbCommand1.ExecuteNonQuery();
          }
          Classifiers3.Clear();
          Classifiers3.Add(3);
          Classifiers3.Add(8);
          Classifiers3.Add(41);
          string estimationTable13 = EngineModel.getEstimationTable(oleDbConnection1, 2, Classifiers3);
          Classifiers3.Clear();
          Classifiers3.Add(3);
          Classifiers3.Add(6);
          Classifiers3.Add(41);
          string estimationTable14 = EngineModel.getEstimationTable(oleDbConnection1, 2, Classifiers3);
          OleDbCommand oleDbCommand24 = oleDbCommand1;
          string[] strArray24 = new string[22];
          strArray24[0] = "INSERT INTO [";
          strArray24[1] = estimationTable12;
          strArray24[2] = "] ( YearGuid, ";
          classifiers = Classifiers.Strata;
          strArray24[3] = classifiers.ToString();
          strArray24[4] = ", ";
          classifiers = Classifiers.Continent;
          strArray24[5] = classifiers.ToString();
          strArray24[6] = ", EstimateType, EquationType, EstimateValue, EstimateStandardError, EstimateUnitsId )  SELECT YearGuid,";
          classifiers = Classifiers.Strata;
          strArray24[7] = classifiers.ToString();
          strArray24[8] = ", ";
          classifiers = Classifiers.Continent;
          strArray24[9] = classifiers.ToString();
          strArray24[10] = ", EstimateType,EquationType, EstimateValue,EstimateStandardError, EstimateUnitsId  FROM [";
          strArray24[11] = estimationTable14;
          strArray24[12] = "]  WHERE YearGuid={";
          guid = this.currYear.Guid;
          strArray24[13] = guid.ToString();
          strArray24[14] = "} AND ";
          classifiers = Classifiers.Living;
          strArray24[15] = classifiers.ToString();
          strArray24[16] = "=";
          strArray24[17] = int16_5.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray24[18] = " and  EstimateType=";
          num11 = 1;
          strArray24[19] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray24[20] = " AND  EstimateUnitsId=";
          strArray24[21] = unit1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str62 = string.Concat(strArray24);
          oleDbCommand24.CommandText = str62;
          oleDbCommand1.ExecuteNonQuery();
          OleDbCommand oleDbCommand25 = oleDbCommand1;
          string[] strArray25 = new string[26];
          strArray25[0] = "INSERT INTO [";
          strArray25[1] = estimationTable12;
          strArray25[2] = "] ( YearGuid, ";
          classifiers = Classifiers.Strata;
          strArray25[3] = classifiers.ToString();
          strArray25[4] = ", ";
          classifiers = Classifiers.Continent;
          strArray25[5] = classifiers.ToString();
          strArray25[6] = ", EstimateType, EquationType, EstimateValue, EstimateStandardError, EstimateUnitsId )  SELECT YearGuid,";
          classifiers = Classifiers.Strata;
          strArray25[7] = classifiers.ToString();
          strArray25[8] = ", ";
          strArray25[9] = num21.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray25[10] = ", EstimateType,EquationType, EstimateValue,EstimateStandardError, EstimateUnitsId  FROM [";
          strArray25[11] = estimationTable13;
          strArray25[12] = "]  WHERE YearGuid={";
          guid = this.currYear.Guid;
          strArray25[13] = guid.ToString();
          strArray25[14] = "} AND ";
          classifiers = Classifiers.Geopoliticalunit;
          strArray25[15] = classifiers.ToString();
          strArray25[16] = "=";
          strArray25[17] = num20.ToString();
          strArray25[18] = " and ";
          classifiers = Classifiers.Living;
          strArray25[19] = classifiers.ToString();
          strArray25[20] = "=";
          strArray25[21] = int16_5.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray25[22] = " and  EstimateType=";
          num11 = 1;
          strArray25[23] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray25[24] = " AND  EstimateUnitsId=";
          strArray25[25] = unit1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str63 = string.Concat(strArray25);
          oleDbCommand25.CommandText = str63;
          oleDbCommand1.ExecuteNonQuery();
        }
        OleDbCommand oleDbCommand26 = new OleDbCommand();
        oleDbCommand26.Connection = oleDbConnection3;
        oleDbCommand26.CommandText = "select * from StrataTable where LocationName='" + LocationName.Replace("'", "''") + "' and SeriesName='" + SeriesName.Replace("'", "''") + "' and [Year]=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        OleDbDataReader oleDbDataReader27 = oleDbCommand26.ExecuteReader();
        Dictionary<string, double> dictionary = new Dictionary<string, double>();
        double num23 = 0.0;
        while (oleDbDataReader27.Read())
        {
          dictionary.Add(oleDbDataReader27.GetString(oleDbDataReader27.GetOrdinal("StratumDescription")), (double) oleDbDataReader27.GetFloat(oleDbDataReader27.GetOrdinal("StratumArea")));
          num23 += (double) oleDbDataReader27.GetFloat(oleDbDataReader27.GetOrdinal("StratumArea"));
        }
        oleDbDataReader27.Close();
        int unit2 = EngineModel.getUnit(oleDbConnection1, 3, 1, 1);
        int unit3 = EngineModel.getUnit(oleDbConnection1, 16, 1, 1);
        Classifiers3.Clear();
        Classifiers3.Add(3);
        Classifiers3.Add(7);
        string estimationTable15 = EngineModel.getEstimationTable(oleDbConnection1, 1, Classifiers3);
        OleDbCommand oleDbCommand27 = oleDbCommand26;
        string[] strArray26 = new string[26]
        {
          "SELECT Strata, GroundCover, EstimateType, EstimateValue, EstimateStandardError, EstimateUnitsId, ClassValueTable.ClassValueName as TREESHRUB, ClassValueTable_1.ClassValueName as StrataName  FROM ([",
          estimationTable15,
          "] INNER JOIN ClassValueTable ON ([",
          estimationTable15,
          "].GroundCover = ClassValueTable.ClassValueOrder) AND ([",
          estimationTable15,
          "].YearGuid = ClassValueTable.YearGuid)) INNER JOIN ClassValueTable AS ClassValueTable_1 ON ([",
          estimationTable15,
          "].Strata = ClassValueTable_1.ClassValueOrder) AND ([",
          estimationTable15,
          "].YearGuid = ClassValueTable_1.YearGuid)  WHERE [",
          estimationTable15,
          "].YearGUID={",
          yearId.ToString(),
          "} and [",
          estimationTable15,
          "].EstimateType = ",
          null,
          null,
          null,
          null,
          null,
          null,
          null,
          null,
          null
        };
        num11 = 12;
        strArray26[17] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray26[18] = " and [";
        strArray26[19] = estimationTable15;
        strArray26[20] = "].EstimateUnitsId = ";
        strArray26[21] = unit2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray26[22] = " and ClassValueTable.ClassifierId=";
        num11 = 7;
        strArray26[23] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray26[24] = " AND (ClassValueTable.ClassValueName='Tree' Or ClassValueTable.ClassValueName='Shrub') AND ClassValueTable_1.ClassifierId=";
        num11 = 3;
        strArray26[25] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        string str64 = string.Concat(strArray26);
        oleDbCommand27.CommandText = str64;
        oleDbCommand26.Connection = oleDbConnection1;
        OleDbDataReader oleDbDataReader28 = oleDbCommand26.ExecuteReader();
        Classifiers3.Clear();
        Classifiers3.Add(3);
        string estimationTable16 = EngineModel.getEstimationTable(oleDbConnection1, 2, Classifiers3);
        OleDbCommand oleDbCommand28 = oleDbCommand1;
        string[] strArray27 = new string[11];
        strArray27[0] = "INSERT INTO [";
        strArray27[1] = estimationTable16;
        strArray27[2] = "] (YearGuid,[";
        classifiers = Classifiers.Strata;
        strArray27[3] = classifiers.ToString();
        strArray27[4] = "],EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId) VALUES({";
        strArray27[5] = yearId.ToString();
        strArray27[6] = "},@strata,";
        num11 = 12;
        strArray27[7] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray27[8] = ",";
        num11 = 1;
        strArray27[9] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray27[10] = ",@estvalue,@serror,@unit)";
        string str65 = string.Concat(strArray27);
        oleDbCommand28.CommandText = str65;
        oleDbCommand1.Parameters.Clear();
        oleDbCommand1.Parameters.Add("@strata", OleDbType.Integer);
        oleDbCommand1.Parameters.Add("@estvalue", OleDbType.Double);
        oleDbCommand1.Parameters.Add("@serror", OleDbType.Double);
        oleDbCommand1.Parameters.Add("@unit", OleDbType.Integer);
        oleDbCommand1.Connection = oleDbConnection1;
        string estimationTable17 = EngineModel.getEstimationTable(oleDbConnection1, 3, Classifiers3);
        OleDbCommand oleDbCommand29 = oleDbCommand2;
        string[] strArray28 = new string[11];
        strArray28[0] = "INSERT INTO [";
        strArray28[1] = estimationTable17;
        strArray28[2] = "] (YearGuid,[";
        classifiers = Classifiers.Strata;
        strArray28[3] = classifiers.ToString();
        strArray28[4] = "],EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId) VALUES({";
        strArray28[5] = yearId.ToString();
        strArray28[6] = "},@strata,";
        num11 = 12;
        strArray28[7] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray28[8] = ",";
        num11 = 1;
        strArray28[9] = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray28[10] = ",@estvalue,@serror,@unit)";
        string str66 = string.Concat(strArray28);
        oleDbCommand29.CommandText = str66;
        oleDbCommand2.Parameters.Clear();
        oleDbCommand2.Parameters.Add("@strata", OleDbType.Integer);
        oleDbCommand2.Parameters.Add("@estvalue", OleDbType.Double);
        oleDbCommand2.Parameters.Add("@serror", OleDbType.Double);
        oleDbCommand2.Parameters.Add("@unit", OleDbType.Integer);
        oleDbCommand2.Connection = oleDbConnection1;
        while (oleDbDataReader28.Read())
        {
          if (oleDbDataReader28.GetString(oleDbDataReader28.GetOrdinal("TREESHRUB")) == "Tree")
          {
            OleDbParameter parameter1 = oleDbCommand1.Parameters["@strata"];
            OleDbDataReader oleDbDataReader29 = oleDbDataReader28;
            OleDbDataReader oleDbDataReader30 = oleDbDataReader28;
            classifiers = Classifiers.Strata;
            string name1 = classifiers.ToString();
            int ordinal14 = oleDbDataReader30.GetOrdinal(name1);
            // ISSUE: variable of a boxed type
            __Boxed<int> int32_3 = (System.ValueType) oleDbDataReader29.GetInt32(ordinal14);
            parameter1.Value = (object) int32_3;
            oleDbCommand1.Parameters["@estvalue"].Value = (object) oleDbDataReader28.GetDouble(oleDbDataReader28.GetOrdinal("EstimateValue"));
            oleDbCommand1.Parameters["@serror"].Value = (object) oleDbDataReader28.GetDouble(oleDbDataReader28.GetOrdinal("EstimateStandardError"));
            oleDbCommand1.Parameters["@unit"].Value = (object) unit2;
            oleDbCommand1.ExecuteNonQuery();
            OleDbParameter parameter2 = oleDbCommand1.Parameters["@strata"];
            OleDbDataReader oleDbDataReader31 = oleDbDataReader28;
            OleDbDataReader oleDbDataReader32 = oleDbDataReader28;
            classifiers = Classifiers.Strata;
            string name2 = classifiers.ToString();
            int ordinal15 = oleDbDataReader32.GetOrdinal(name2);
            // ISSUE: variable of a boxed type
            __Boxed<int> int32_4 = (System.ValueType) oleDbDataReader31.GetInt32(ordinal15);
            parameter2.Value = (object) int32_4;
            double num24 = num23 / 100.0;
            if (dictionary.ContainsKey(oleDbDataReader28.GetString(oleDbDataReader28.GetOrdinal("StrataName"))))
              num24 = dictionary[oleDbDataReader28.GetString(oleDbDataReader28.GetOrdinal("StrataName"))] / 100.0;
            oleDbCommand1.Parameters["@estvalue"].Value = (object) (oleDbDataReader28.GetDouble(oleDbDataReader28.GetOrdinal("EstimateValue")) / 100.0 * num24);
            oleDbCommand1.Parameters["@serror"].Value = (object) (oleDbDataReader28.GetDouble(oleDbDataReader28.GetOrdinal("EstimateStandardError")) / 100.0 * num24);
            oleDbCommand1.Parameters["@unit"].Value = (object) unit3;
            oleDbCommand1.ExecuteNonQuery();
          }
          else
          {
            OleDbParameter parameter3 = oleDbCommand2.Parameters["@strata"];
            OleDbDataReader oleDbDataReader33 = oleDbDataReader28;
            OleDbDataReader oleDbDataReader34 = oleDbDataReader28;
            classifiers = Classifiers.Strata;
            string name3 = classifiers.ToString();
            int ordinal16 = oleDbDataReader34.GetOrdinal(name3);
            // ISSUE: variable of a boxed type
            __Boxed<int> int32_5 = (System.ValueType) oleDbDataReader33.GetInt32(ordinal16);
            parameter3.Value = (object) int32_5;
            oleDbCommand2.Parameters["@estvalue"].Value = (object) oleDbDataReader28.GetDouble(oleDbDataReader28.GetOrdinal("EstimateValue"));
            oleDbCommand2.Parameters["@serror"].Value = (object) oleDbDataReader28.GetDouble(oleDbDataReader28.GetOrdinal("EstimateStandardError"));
            oleDbCommand2.Parameters["@unit"].Value = (object) unit2;
            oleDbCommand2.ExecuteNonQuery();
            OleDbParameter parameter4 = oleDbCommand2.Parameters["@strata"];
            OleDbDataReader oleDbDataReader35 = oleDbDataReader28;
            OleDbDataReader oleDbDataReader36 = oleDbDataReader28;
            classifiers = Classifiers.Strata;
            string name4 = classifiers.ToString();
            int ordinal17 = oleDbDataReader36.GetOrdinal(name4);
            // ISSUE: variable of a boxed type
            __Boxed<int> int32_6 = (System.ValueType) oleDbDataReader35.GetInt32(ordinal17);
            parameter4.Value = (object) int32_6;
            double num25 = num23 / 100.0;
            if (dictionary.ContainsKey(oleDbDataReader28.GetString(oleDbDataReader28.GetOrdinal("StrataName"))))
              num25 = dictionary[oleDbDataReader28.GetString(oleDbDataReader28.GetOrdinal("StrataName"))] / 100.0;
            oleDbCommand2.Parameters["@estvalue"].Value = (object) (oleDbDataReader28.GetDouble(oleDbDataReader28.GetOrdinal("EstimateValue")) / 100.0 * num25);
            oleDbCommand2.Parameters["@serror"].Value = (object) (oleDbDataReader28.GetDouble(oleDbDataReader28.GetOrdinal("EstimateStandardError")) / 100.0 * num25);
            oleDbCommand2.Parameters["@unit"].Value = (object) unit3;
            oleDbCommand2.ExecuteNonQuery();
          }
        }
        oleDbDataReader28.Close();
        this.PrepareTablesForPollutionCalculation(oleDbConnection1, oleDbConnection2, oleDbConnection3, yearId, LocationName, SeriesName, timePeriod, this.ClassValueForStrataStudyArea, this.ClassValueForSpeciesAllSpecies, seriesSampleType);
        if (this.engineCancellationToken.IsCancellationRequested)
          throw new Exception("User cancelled");
        if ((double) this.engineReportArg.Percent + num19 > (double) toPercent)
          this.engineReportArg.Percent = toPercent;
        else
          this.engineReportArg.Percent += (int) num19;
        this.engineProgress.Report(this.engineReportArg);
        if (seriesSampleType == SampleType.Inventory)
        {
          double[] numArray7 = new double[200];
          for (int index17 = 1; index17 < 200; ++index17)
            numArray7[index17] = -1.0;
          int num26 = 0;
          double num27 = 1.0;
          double num28 = 0.0;
          double num29 = 0.0;
          oleDbCommand1.Connection = oleDbConnection1;
          OleDbCommand oleDbCommand30 = oleDbCommand1;
          num11 = 24;
          string str67 = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          num11 = 24;
          string str68 = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str69 = "select EstimationUnitsId from EstimationUnitsTable where SecondaryUnitsId=" + str67 + " or TertiaryUnitsId=" + str68;
          oleDbCommand30.CommandText = str69;
          string str70 = "";
          OleDbDataReader oleDbDataReader37 = oleDbCommand1.ExecuteReader();
          while (oleDbDataReader37.Read())
          {
            string str71 = str70;
            num11 = oleDbDataReader37.GetInt32(oleDbDataReader37.GetOrdinal("EstimationUnitsId"));
            string str72 = num11.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            str70 = str71 + str72 + ",";
          }
          oleDbDataReader37.Close();
          if (str70.Length > 0)
          {
            str70 = str70.Substring(0, str70.Length);
            oleDbCommand1.CommandText = "select * from EcoYears where YearKey={" + yearId.ToString() + "}";
            OleDbDataReader oleDbDataReader38 = oleDbCommand1.ExecuteReader();
            oleDbDataReader38.Read();
            string str73 = oleDbDataReader38.GetString(oleDbDataReader38.GetOrdinal("Units"));
            oleDbDataReader38.Close();
            OleDbCommand oleDbCommand31 = oleDbCommand1;
            string str74 = yearId.ToString();
            num11 = 3;
            string str75 = num11.ToString();
            string str76 = "select * from ClassValueTable Where YearGuid={" + str74 + "} and ClassifierId=" + str75;
            oleDbCommand31.CommandText = str76;
            OleDbDataReader oleDbDataReader39 = oleDbCommand1.ExecuteReader();
            while (oleDbDataReader39.Read())
            {
              int int16_6 = (int) oleDbDataReader39.GetInt16(oleDbDataReader39.GetOrdinal("ClassValueOrder"));
              string str77 = oleDbDataReader39.GetString(oleDbDataReader39.GetOrdinal("ClassValueName"));
              double num30 = 0.0;
              double num31 = 0.0;
              if (int16_6 != this.ClassValueForStrataStudyArea)
              {
                oleDbCommand2.Connection = oleDbConnection3;
                oleDbCommand2.CommandText = "select StratumArea from StrataTable  Where LocationName='" + LocationName.Replace("'", "''") + "'  and SeriesName='" + SeriesName.Replace("'", "''") + "'  and Year=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and StratumDescription='" + str77.Replace("'", "''") + "'";
                OleDbDataReader oleDbDataReader40 = oleDbCommand2.ExecuteReader();
                if (oleDbDataReader40.Read())
                  num30 = (double) oleDbDataReader40.GetFloat(oleDbDataReader40.GetOrdinal("StratumArea"));
                oleDbDataReader40.Close();
                oleDbCommand2.Connection = oleDbConnection1;
                oleDbCommand2.CommandText = "select [Size] from EcoStrata  Where YearKey={" + yearId.ToString() + "}  and Description='" + str77.Replace("'", "''") + "'";
                OleDbDataReader oleDbDataReader41 = oleDbCommand2.ExecuteReader();
                if (oleDbDataReader41.Read())
                  num31 = !(str73 == "M") ? 0.404686 * (double) oleDbDataReader41.GetFloat(oleDbDataReader41.GetOrdinal("Size")) : (double) oleDbDataReader41.GetFloat(oleDbDataReader41.GetOrdinal("Size"));
                oleDbDataReader41.Close();
                if (num26 < int16_6)
                  num26 = int16_6;
                if (num31 < 0.8 * num30 || num31 <= 0.0)
                {
                  num28 += num30;
                  num29 += num30;
                  num31 = num30;
                }
                else
                {
                  num28 += num30;
                  num29 += num31;
                }
                if (num31 != 0.0)
                  numArray7[int16_6] = num30 / num31;
              }
            }
            oleDbDataReader39.Close();
            num27 = num28 / num29;
          }
          if (this.engineCancellationToken.IsCancellationRequested)
            throw new Exception("User cancelled");
          if ((double) this.engineReportArg.Percent + num19 > (double) toPercent)
            this.engineReportArg.Percent = toPercent;
          else
            this.engineReportArg.Percent += (int) num19;
          this.engineProgress.Report(this.engineReportArg);
          oleDbCommand1.CommandText = "select StatisticalTableId,TableTitle from TableOfStatisticalEstimates";
          OleDbDataReader oleDbDataReader42 = oleDbCommand1.ExecuteReader();
          while (oleDbDataReader42.Read())
          {
            int int32_7 = oleDbDataReader42.GetInt32(oleDbDataReader42.GetOrdinal("StatisticalTableId"));
            string str78 = oleDbDataReader42.GetString(oleDbDataReader42.GetOrdinal("TableTitle"));
            oleDbCommand2.Connection = oleDbConnection1;
            if (str70 != "")
            {
              oleDbCommand2.CommandText = "select * from PartitionDefinitionsTable Where StatisticalTableId=" + int32_7.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " order by PartitionOrder";
              OleDbDataReader oleDbDataReader43 = oleDbCommand2.ExecuteReader();
              oleDbDataReader43.Read();
              if (oleDbDataReader43.GetInt32(oleDbDataReader43.GetOrdinal("ClassifierId")) == 3)
              {
                oleDbCommand3.Connection = oleDbConnection1;
                for (int index18 = 1; index18 <= num26; ++index18)
                {
                  if (numArray7[index18] != -1.0)
                  {
                    OleDbCommand oleDbCommand32 = oleDbCommand3;
                    string[] strArray29 = new string[13]
                    {
                      "Update [",
                      str78,
                      "] set EstimateValue=EstimateValue*",
                      numArray7[index18].ToString(),
                      " WHERE YearGuid={",
                      yearId.ToString(),
                      "} and [",
                      null,
                      null,
                      null,
                      null,
                      null,
                      null
                    };
                    classifiers = Classifiers.Strata;
                    strArray29[7] = classifiers.ToString();
                    strArray29[8] = "]=";
                    strArray29[9] = index18.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                    strArray29[10] = " and EstimateUnitsId in (";
                    strArray29[11] = str70;
                    strArray29[12] = ")";
                    string str79 = string.Concat(strArray29);
                    oleDbCommand32.CommandText = str79;
                    oleDbCommand3.ExecuteNonQuery();
                  }
                }
                OleDbCommand oleDbCommand33 = oleDbCommand3;
                string[] strArray30 = new string[13]
                {
                  "Update [",
                  str78,
                  "] set EstimateValue=EstimateValue*",
                  num27.ToString(),
                  " WHERE YearGuid={",
                  yearId.ToString(),
                  "} and [",
                  null,
                  null,
                  null,
                  null,
                  null,
                  null
                };
                classifiers = Classifiers.Strata;
                strArray30[7] = classifiers.ToString();
                strArray30[8] = "]=";
                strArray30[9] = this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray30[10] = " and EstimateUnitsId in (";
                strArray30[11] = str70;
                strArray30[12] = ")";
                string str80 = string.Concat(strArray30);
                oleDbCommand33.CommandText = str80;
                oleDbCommand3.ExecuteNonQuery();
              }
              else
              {
                oleDbCommand3.Connection = oleDbConnection1;
                oleDbCommand3.CommandText = "Update [" + str78 + "] set EstimateValue=EstimateValue*" + num27.ToString() + " WHERE YearGuid={" + yearId.ToString() + "} and EstimateUnitsId in (" + str70 + ")";
                oleDbCommand3.ExecuteNonQuery();
              }
              oleDbDataReader43.Close();
            }
          }
        }
        EngineModel.CloseConnection(oleDbConnection1);
        EngineModel.CloseConnection(oleDbConnection2);
        EngineModel.CloseConnection(oleDbConnection3);
        EngineModel.CloseConnection(oleDbConnection4);
        EngineModel.CloseConnection(oleDbConnection5);
      }
      catch (Exception ex)
      {
        EngineModel.CloseConnection(oleDbConnection1);
        EngineModel.CloseConnection(oleDbConnection2);
        EngineModel.CloseConnection(oleDbConnection3);
        EngineModel.CloseConnection(oleDbConnection4);
        EngineModel.CloseConnection(oleDbConnection5);
        throw;
      }
    }

    private void transferSingleTreeFromUFOREresults(
      Guid yearId,
      string LocationName,
      string SeriesName,
      int timePeriod,
      string inputDB,
      string workingInputDB,
      string UforeInventoryDB,
      int fromProgressPercent,
      int toProgressPercent)
    {
      using (OleDbConnection oleDbConnection1 = new OleDbConnection())
      {
        using (OleDbConnection oleDbConnection2 = new OleDbConnection())
        {
          using (OleDbConnection oleDbConnection3 = new OleDbConnection())
          {
            try
            {
              EngineModel.OpenConnection(oleDbConnection1, inputDB);
              OleDbCommand oleDbCommand1 = new OleDbCommand();
              oleDbCommand1.Connection = oleDbConnection1;
              EngineModel.OpenConnection(oleDbConnection2, UforeInventoryDB);
              OleDbCommand oleDbCommand2 = new OleDbCommand();
              oleDbCommand2.Connection = oleDbConnection2;
              EngineModel.OpenConnection(oleDbConnection3, workingInputDB);
              OleDbCommand oleDbCommand3 = new OleDbCommand();
              if (this.currSeries.SampleType == SampleType.Inventory)
              {
                int num = 0;
                oleDbCommand2.CommandText = "select * from CoverTypesTable where LocationName='" + LocationName.Replace("'", "''") + "' and SeriesName='" + SeriesName.Replace("'", "''") + "' and Year=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and CoverDescription='Tree'";
                OleDbDataReader oleDbDataReader1 = oleDbCommand2.ExecuteReader();
                if (oleDbDataReader1.Read())
                  num = oleDbDataReader1.GetInt32(oleDbDataReader1.GetOrdinal("CoverTypeID"));
                oleDbDataReader1.Close();
                oleDbCommand2.CommandText = "Update CoversTable set PercentCover=100 where LocationName='" + LocationName.Replace("'", "''") + "' and SeriesName='" + SeriesName.Replace("'", "''") + "' and Year=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and CoverTypeID=" + num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                oleDbCommand2.ExecuteNonQuery();
                oleDbCommand2.CommandText = "UPDATE SubplotsTable INNER JOIN TreesTable ON (SubplotsTable.SubplotID = TreesTable.SubplotID) AND (SubplotsTable.PlotID = TreesTable.PlotID) AND (SubplotsTable.Year = TreesTable.Year) AND (SubplotsTable.SeriesName = TreesTable.SeriesName) AND (SubplotsTable.LocationName = TreesTable.LocationName) SET SubplotsTable.SubplotSize = [TreesTable].[CrownGroundArea]/10000  WHERE SubplotsTable.LocationName='" + LocationName.Replace("'", "''") + "' AND SubplotsTable.SeriesName='" + SeriesName.Replace("'", "''") + "' AND SubplotsTable.Year=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                oleDbCommand2.ExecuteNonQuery();
                oleDbCommand2.CommandText = "SELECT PlotsTable.StratumID, Sum(SubplotsTable.SubplotSize) AS SumOfSubplotSize  FROM PlotsTable INNER JOIN SubplotsTable ON (PlotsTable.PlotID = SubplotsTable.PlotID) AND (PlotsTable.Year = SubplotsTable.Year) AND (PlotsTable.SeriesName = SubplotsTable.SeriesName) AND (PlotsTable.LocationName = SubplotsTable.LocationName)  WHERE PlotsTable.LocationName='" + LocationName.Replace("'", "''") + "' AND PlotsTable.SeriesName='" + SeriesName.Replace("'", "''") + "' AND PlotsTable.Year=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " GROUP BY PlotsTable.StratumID";
                OleDbDataReader oleDbDataReader2 = oleDbCommand2.ExecuteReader();
                oleDbCommand3.CommandText = "UPDATE StrataTable SET StratumArea = @area WHERE LocationName='" + LocationName.Replace("'", "''") + "' AND SeriesName='" + SeriesName.Replace("'", "''") + "' AND [Year]=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND StratumID = @sid";
                oleDbCommand3.Connection = oleDbConnection2;
                oleDbCommand3.Parameters.Clear();
                oleDbCommand3.Parameters.Add("@area", OleDbType.Double);
                oleDbCommand3.Parameters.Add("@sid", OleDbType.Integer);
                while (oleDbDataReader2.Read())
                {
                  oleDbCommand3.Parameters["@area"].Value = (object) oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("SumOfSubplotSize"));
                  oleDbCommand3.Parameters["@sid"].Value = (object) oleDbDataReader2.GetInt32(oleDbDataReader2.GetOrdinal("StratumID"));
                  oleDbCommand3.ExecuteNonQuery();
                }
                oleDbDataReader2.Close();
              }
              oleDbCommand2.CommandText = "SELECT count(TreesTable.TreeID) FROM PermanentTreesTable INNER JOIN TreesTable ON (PermanentTreesTable.TreeID = TreesTable.TreeID) AND (PermanentTreesTable.SubplotID = TreesTable.SubplotID) AND (PermanentTreesTable.PlotID = TreesTable.PlotID) AND (PermanentTreesTable.Year = TreesTable.Year) AND (PermanentTreesTable.SeriesName = TreesTable.SeriesName) AND (PermanentTreesTable.LocationName = TreesTable.LocationName) WHERE TreesTable.LocationName='" + LocationName + "' AND TreesTable.SeriesName='" + SeriesName + "' AND TreesTable.Year=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              OleDbDataReader oleDbDataReader3 = oleDbCommand2.ExecuteReader();
              oleDbDataReader3.Read();
              int int32_1 = oleDbDataReader3.GetInt32(0);
              oleDbDataReader3.Close();
              int percent1 = this.engineReportArg.Percent;
              double num1 = (double) (toProgressPercent - 5 - percent1) / 2.0 / (double) int32_1;
              oleDbCommand2.CommandText = "SELECT * from EnergyTable WHERE LocationName='" + LocationName + "' AND SeriesName='" + SeriesName + "' AND Year=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture);
              OleDbDataReader oleDbDataReader4 = oleDbCommand2.ExecuteReader();
              int ordinal1 = oleDbDataReader4.GetOrdinal("PlotID");
              int ordinal2 = oleDbDataReader4.GetOrdinal("TreeID");
              int ordinal3 = oleDbDataReader4.GetOrdinal("BldgInteractionID");
              int ordinal4 = oleDbDataReader4.GetOrdinal("EnergyUse");
              int ordinal5 = oleDbDataReader4.GetOrdinal("EnergyType");
              int ordinal6 = oleDbDataReader4.GetOrdinal("CarbonAvoided");
              int ordinal7 = oleDbDataReader4.GetOrdinal("FuelsAvoided");
              int ordinal8 = oleDbDataReader4.GetOrdinal("ElectricityAvoided");
              string str1 = "INSERT INTO IndividualTreeEnergyEffects (YearGuid,PlotID,TreeID,BldgInteractionID,EnergyUse,EnergyType,CarbonAvoided,FuelsAvoided,ElectricityAvoided) VALUES(";
              int num2 = 0;
              int num3 = -1;
              int num4 = -1;
              while (oleDbDataReader4.Read())
              {
                oleDbCommand1.CommandText = str1 + "{" + yearId.ToString() + "}," + oleDbDataReader4.GetInt32(ordinal1).ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + oleDbDataReader4.GetInt32(ordinal2).ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + oleDbDataReader4.GetInt32(ordinal3).ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + oleDbDataReader4.GetInt16(ordinal4).ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + oleDbDataReader4.GetInt16(ordinal5).ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + oleDbDataReader4.GetDouble(ordinal6).ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + oleDbDataReader4.GetDouble(ordinal7).ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + oleDbDataReader4.GetDouble(ordinal8).ToString((IFormatProvider) CultureInfo.InvariantCulture) + ")";
                oleDbCommand1.ExecuteNonQuery();
                if (num3 != oleDbDataReader4.GetInt32(ordinal1) && num4 != oleDbDataReader4.GetInt32(ordinal2))
                {
                  num3 = oleDbDataReader4.GetInt32(ordinal1);
                  num4 = oleDbDataReader4.GetInt32(ordinal2);
                  ++num2;
                  if ((double) percent1 + num1 * (double) num2 >= (double) (this.engineReportArg.Percent + 1) && (double) percent1 + num1 * (double) num2 <= 100.0)
                  {
                    this.engineReportArg.Percent = (int) ((double) percent1 + num1 * (double) num2);
                    this.engineProgressForm.DisplayProgress(this.engineReportArg);
                    Application.DoEvents();
                  }
                }
              }
              oleDbDataReader4.Close();
              Dictionary<string, string> dictionary = new Dictionary<string, string>();
              oleDbCommand2.Connection = oleDbConnection3;
              oleDbCommand2.CommandText = "select IncCrownCondition from EcoYears where YearKey={" + yearId.ToString() + "}";
              OleDbDataReader oleDbDataReader5 = oleDbCommand2.ExecuteReader();
              oleDbDataReader5.Read();
              int num5 = oleDbDataReader5.GetBoolean(oleDbDataReader5.GetOrdinal("IncCrownCondition")) ? 1 : 0;
              oleDbDataReader5.Close();
              bool flag;
              if (num5 != 0)
              {
                oleDbCommand2.Connection = oleDbConnection3;
                oleDbCommand2.CommandText = "SELECT EcoConditions.ConditionId, EcoConditions.Description, EcoConditions.PctDieback From EcoConditions where YearKey={" + yearId.ToString() + "} and PctDieback>=0 Order by PctDieback";
                OleDbDataReader oleDbDataReader6 = oleDbCommand2.ExecuteReader();
                int num6 = 0;
                dictionary.Clear();
                while (oleDbDataReader6.Read())
                {
                  try
                  {
                    dictionary.Add(Math.Truncate(oleDbDataReader6.GetDouble(oleDbDataReader6.GetOrdinal("PctDieback"))).ToString((IFormatProvider) CultureInfo.InvariantCulture), oleDbDataReader6.GetString(oleDbDataReader6.GetOrdinal("Description")));
                  }
                  catch
                  {
                  }
                  ++num6;
                }
                oleDbDataReader6.Close();
                flag = num6 == 0 || num6 > 7;
              }
              else
                flag = true;
              if (flag)
              {
                dictionary.Clear();
                dictionary.Add("0", "Excellent");
                for (int index = 1; index <= 9; ++index)
                  dictionary.Add(index.ToString((IFormatProvider) CultureInfo.InvariantCulture), "Good");
                for (int index = 10; index <= 24; ++index)
                  dictionary.Add(index.ToString((IFormatProvider) CultureInfo.InvariantCulture), "Fair");
                for (int index = 25; index <= 49; ++index)
                  dictionary.Add(index.ToString((IFormatProvider) CultureInfo.InvariantCulture), "Poor");
                for (int index = 50; index <= 74; ++index)
                  dictionary.Add(index.ToString((IFormatProvider) CultureInfo.InvariantCulture), "Critical");
                for (int index = 75; index <= 98; ++index)
                  dictionary.Add(index.ToString((IFormatProvider) CultureInfo.InvariantCulture), "Dying");
                for (int index = 99; index <= 100; ++index)
                  dictionary.Add(index.ToString((IFormatProvider) CultureInfo.InvariantCulture), "Dead");
              }
              this.engineReportArg.Percent += 3;
              if (this.engineReportArg.Percent > 100)
                this.engineReportArg.Percent = 100;
              this.engineProgressForm.DisplayProgress(this.engineReportArg);
              Application.DoEvents();
              oleDbCommand2.Connection = oleDbConnection2;
              int percent2 = this.engineReportArg.Percent;
              double num7 = (double) (toProgressPercent - 5 - percent2) / (double) int32_1;
              oleDbCommand2.CommandText = "SELECT TreesTable.*, PermanentTreesTable.SppCode FROM PermanentTreesTable INNER JOIN TreesTable ON (PermanentTreesTable.TreeID = TreesTable.TreeID) AND (PermanentTreesTable.SubplotID = TreesTable.SubplotID) AND (PermanentTreesTable.PlotID = TreesTable.PlotID) AND (PermanentTreesTable.Year = TreesTable.Year) AND (PermanentTreesTable.SeriesName = TreesTable.SeriesName) AND (PermanentTreesTable.LocationName = TreesTable.LocationName) WHERE TreesTable.LocationName='" + LocationName + "' AND TreesTable.SeriesName='" + SeriesName + "' AND TreesTable.Year=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " Order by TreesTable.PlotId, TreesTable.TreeId";
              OleDbDataReader oleDbDataReader7 = oleDbCommand2.ExecuteReader();
              oleDbCommand3.Connection = oleDbConnection3;
              oleDbCommand3.CommandText = "select PlotId, TreeId, CrownDieback,TreeSite From Trees Where LocationName='" + LocationName.Replace("'", "''") + "' and Series='" + SeriesName.Replace("'", "''") + "' and Year=" + timePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " Order by PlotId, TreeId";
              OleDbDataReader oleDbDataReader8 = oleDbCommand3.ExecuteReader();
              string str2 = "INSERT INTO IndividualTreeEffects (YearGuid,PlotId,TreeId,SppScientificName,SppCommonName,DBH,Height,BasalArea,GroundArea,TreeCondition,LeafArea,LeafBioMass,LeafAreaIndex,LeafBiomassIndex,CarbonStorage,GrossCarbonSeq,NetCarbonSeq,TreeValue,StreetTree,NativeToState,TreeBiomass) VALUES(";
              int ordinal9 = oleDbDataReader7.GetOrdinal("PlotID");
              int ordinal10 = oleDbDataReader7.GetOrdinal("TreeID");
              int ordinal11 = oleDbDataReader7.GetOrdinal("SppCode");
              int ordinal12 = oleDbDataReader7.GetOrdinal("CalculationDBH");
              int ordinal13 = oleDbDataReader7.GetOrdinal("CalculationHeight");
              int ordinal14 = oleDbDataReader7.GetOrdinal("BasalArea");
              int ordinal15 = oleDbDataReader7.GetOrdinal("TreeBiomass");
              int ordinal16 = oleDbDataReader7.GetOrdinal("CarbonStorage");
              int ordinal17 = oleDbDataReader7.GetOrdinal("GrossCarbonSequestration");
              int ordinal18 = oleDbDataReader7.GetOrdinal("NetCarbonSequestration");
              int ordinal19 = oleDbDataReader7.GetOrdinal("LeafArea");
              int ordinal20 = oleDbDataReader7.GetOrdinal("LeafBiomass");
              int ordinal21 = oleDbDataReader7.GetOrdinal("CompensatoryValue");
              int ordinal22 = oleDbDataReader7.GetOrdinal("CrownGroundArea");
              int ordinal23 = oleDbDataReader7.GetOrdinal("LeafAreaIndex");
              int ordinal24 = oleDbDataReader7.GetOrdinal("LeafBiomassIndex");
              int num8 = 0;
              while (oleDbDataReader7.Read())
              {
                string str3;
                string str4;
                string str5;
                if (this.allSpecies.ContainsKey(oleDbDataReader7.GetString(ordinal11)))
                {
                  Species allSpecy = this.allSpecies[oleDbDataReader7.GetString(ordinal11)];
                  str3 = allSpecy.SpeciesCommonName.Replace("'", "''");
                  str4 = allSpecy.SpeciesScientificName.Replace("'", "''");
                  str5 = allSpecy.NativeToState;
                }
                else
                {
                  str3 = oleDbDataReader7.GetString(ordinal11);
                  str4 = oleDbDataReader7.GetString(ordinal11);
                  str5 = "NO";
                }
                string str6 = "";
                string str7 = "";
                while (oleDbDataReader8.Read())
                {
                  if (oleDbDataReader7.GetInt32(ordinal9) == oleDbDataReader8.GetInt32(oleDbDataReader8.GetOrdinal("PlotID")) && oleDbDataReader7.GetInt32(ordinal10) == oleDbDataReader8.GetInt32(oleDbDataReader8.GetOrdinal("TreeID")))
                  {
                    int num9 = (int) Math.Truncate(oleDbDataReader8.GetDouble(oleDbDataReader8.GetOrdinal("CrownDieback")));
                    try
                    {
                      str6 = dictionary[num9.ToString((IFormatProvider) CultureInfo.InvariantCulture)].Replace("'", "''");
                    }
                    catch
                    {
                      str6 = "Not-Entered";
                    }
                    str7 = !(oleDbDataReader8.GetString(oleDbDataReader8.GetOrdinal("TreeSite")) == "S") ? "NO" : "YES";
                    break;
                  }
                  if (oleDbDataReader8.GetInt32(oleDbDataReader8.GetOrdinal("PlotID")) > oleDbDataReader7.GetInt32(ordinal9) || oleDbDataReader8.GetInt32(oleDbDataReader8.GetOrdinal("PlotID")) == oleDbDataReader7.GetInt32(ordinal9) && oleDbDataReader8.GetInt32(oleDbDataReader8.GetOrdinal("TreeID")) > oleDbDataReader7.GetInt32(ordinal10))
                    break;
                }
                string[] strArray = new string[44];
                strArray[0] = str2;
                strArray[1] = "{";
                strArray[2] = yearId.ToString();
                strArray[3] = "},";
                int int32_2 = oleDbDataReader7.GetInt32(ordinal9);
                strArray[4] = int32_2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[5] = ",";
                int32_2 = oleDbDataReader7.GetInt32(ordinal10);
                strArray[6] = int32_2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[7] = ",'";
                strArray[8] = str4;
                strArray[9] = "','";
                strArray[10] = str3;
                strArray[11] = "',";
                strArray[12] = oleDbDataReader7.GetFloat(ordinal12).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[13] = ",";
                strArray[14] = oleDbDataReader7.GetFloat(ordinal13).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[15] = ",";
                strArray[16] = oleDbDataReader7.GetFloat(ordinal14).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[17] = ",";
                strArray[18] = oleDbDataReader7.GetDouble(ordinal22).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[19] = ",'";
                strArray[20] = str6;
                strArray[21] = "',";
                strArray[22] = oleDbDataReader7.GetDouble(ordinal19).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[23] = ",";
                strArray[24] = oleDbDataReader7.GetDouble(ordinal20).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[25] = ",";
                strArray[26] = oleDbDataReader7.GetFloat(ordinal23).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[27] = ",";
                strArray[28] = oleDbDataReader7.GetFloat(ordinal24).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[29] = ",";
                strArray[30] = oleDbDataReader7.GetDouble(ordinal16).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[31] = ",";
                strArray[32] = oleDbDataReader7.GetDouble(ordinal17).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[33] = ",";
                strArray[34] = oleDbDataReader7.GetDouble(ordinal18).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[35] = ",";
                strArray[36] = oleDbDataReader7.GetFloat(ordinal21).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[37] = ",'";
                strArray[38] = str7;
                strArray[39] = "','";
                strArray[40] = str5;
                strArray[41] = "',";
                strArray[42] = oleDbDataReader7.GetDouble(ordinal15).ToString((IFormatProvider) CultureInfo.InvariantCulture);
                strArray[43] = ")";
                string str8 = string.Concat(strArray);
                oleDbCommand1.CommandText = str8;
                oleDbCommand1.ExecuteNonQuery();
                ++num8;
                if ((double) percent2 + num7 * (double) num8 >= (double) (this.engineReportArg.Percent + 1) && (double) percent2 + num7 * (double) num8 <= 100.0)
                {
                  this.engineReportArg.Percent = (int) ((double) percent2 + num7 * (double) num8);
                  this.engineProgressForm.DisplayProgress(this.engineReportArg);
                  Application.DoEvents();
                }
              }
              oleDbDataReader7.Close();
              oleDbCommand1.CommandText = "INSERT INTO IndividualTreePollutionEffects ( YearGuid, PlotId, TreeId, SppScientificName, SppCommonName ) SELECT IndividualTreeEffects.YearGuid, IndividualTreeEffects.PlotId, IndividualTreeEffects.TreeId, IndividualTreeEffects.SppScientificName, IndividualTreeEffects.SppCommonName FROM IndividualTreeEffects  WHERE (((IndividualTreeEffects.YearGuid)={guid {" + yearId.ToString() + "}}))";
              oleDbCommand1.ExecuteNonQuery();
              this.engineReportArg.Percent = 100;
              this.engineProgressForm.DisplayProgress(this.engineReportArg);
              Application.DoEvents();
              oleDbDataReader8.Close();
              EngineModel.CloseConnection(oleDbConnection1);
              EngineModel.CloseConnection(oleDbConnection2);
              EngineModel.CloseConnection(oleDbConnection3);
            }
            catch (Exception ex)
            {
              EngineModel.CloseConnection(oleDbConnection1);
              EngineModel.CloseConnection(oleDbConnection2);
              EngineModel.CloseConnection(oleDbConnection3);
              throw;
            }
          }
        }
      }
    }

    private void clearResults(string inputDBname, Guid yearGuid)
    {
      try
      {
        using (OleDbConnection oleDbConnection = new OleDbConnection())
        {
          EngineModel.OpenConnection(oleDbConnection, inputDBname);
          try
          {
            this.clearResults(oleDbConnection, yearGuid);
            EngineModel.CloseConnection(oleDbConnection);
          }
          catch (Exception ex)
          {
            EngineModel.CloseConnection(oleDbConnection);
            throw;
          }
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void clearResults(OleDbConnection inputDBCon, Guid yearGuid)
    {
      try
      {
        OleDbCommand oleDbCommand1 = new OleDbCommand();
        oleDbCommand1.Connection = inputDBCon;
        OleDbCommand oleDbCommand2 = new OleDbCommand();
        oleDbCommand2.Connection = inputDBCon;
        oleDbCommand1.CommandText = "Delete * from ClassValueTable Where YearGUID = {" + yearGuid.ToString() + "}";
        oleDbCommand1.ExecuteNonQuery();
        oleDbCommand1.CommandText = "Delete * from IndividualTreeEffects Where YearGUID = {" + yearGuid.ToString() + "}";
        oleDbCommand1.ExecuteNonQuery();
        oleDbCommand1.CommandText = "Delete * from IndividualTreeEnergyEffects Where YearGUID = {" + yearGuid.ToString() + "}";
        oleDbCommand1.ExecuteNonQuery();
        oleDbCommand1.CommandText = "Delete * from IndividualTreePollutionEffects Where YearGUID = {" + yearGuid.ToString() + "}";
        oleDbCommand1.ExecuteNonQuery();
        oleDbCommand1.CommandText = "Delete * from Pollutants Where YearGUID = {" + yearGuid.ToString() + "}";
        oleDbCommand1.ExecuteNonQuery();
        oleDbCommand1.CommandText = "Delete * from ModelNotes Where YearGuid={" + yearGuid.ToString() + "}";
        oleDbCommand1.ExecuteNonQuery();
        oleDbCommand1.CommandText = "Delete * from BenMapTable Where YearGuid={" + yearGuid.ToString() + "}";
        oleDbCommand1.ExecuteNonQuery();
        oleDbCommand1.CommandText = "select * from TableOfStatisticalEstimates";
        OleDbDataReader oleDbDataReader = oleDbCommand1.ExecuteReader();
        while (oleDbDataReader.Read())
        {
          string str = oleDbDataReader.GetString(oleDbDataReader.GetOrdinal("TableTitle"));
          oleDbCommand2.CommandText = "Delete * from [" + str + "] where YearGuid={" + yearGuid.ToString() + "}";
          oleDbCommand2.ExecuteNonQuery();
        }
        oleDbDataReader.Close();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static int getUnit(
      OleDbConnection EstimationDBCon,
      int PrimaryUnit,
      int SecondaryUnit,
      int TertiaryUnit)
    {
      try
      {
        OleDbCommand oleDbCommand = new OleDbCommand();
        oleDbCommand.Connection = EstimationDBCon;
        oleDbCommand.CommandText = "select * from EstimationUnitsTable Where PrimaryUnitsId=" + PrimaryUnit.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and SecondaryUnitsId=" + SecondaryUnit.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and TertiaryUnitsId=" + TertiaryUnit.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        OleDbDataReader oleDbDataReader1 = oleDbCommand.ExecuteReader();
        int unit;
        if (oleDbDataReader1.Read())
        {
          unit = oleDbDataReader1.GetInt32(oleDbDataReader1.GetOrdinal("EstimationUnitsId"));
          oleDbDataReader1.Close();
        }
        else
        {
          oleDbDataReader1.Close();
          oleDbCommand.CommandText = "select top 1 * from EstimationUnitsTable Order by EstimationUnitsId DESC";
          unit = 1;
          OleDbDataReader oleDbDataReader2 = oleDbCommand.ExecuteReader();
          if (oleDbDataReader2.Read())
            unit = oleDbDataReader2.GetInt32(oleDbDataReader2.GetOrdinal("EstimationUnitsId")) + 1;
          oleDbDataReader2.Close();
          oleDbCommand.CommandText = "Insert into EstimationUnitsTable (EstimationUnitsId,PrimaryUnitsId,SecondaryUnitsId,TertiaryUnitsId) values(" + unit.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + PrimaryUnit.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + SecondaryUnit.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + TertiaryUnit.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ")";
          oleDbCommand.ExecuteNonQuery();
        }
        return unit;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static string getEstimationTable(
      OleDbConnection EstimationDBCon,
      int TableData,
      List<int> Classifiers)
    {
      try
      {
        int count = Classifiers.Count;
        int num1 = 1000;
        int num2 = 0;
        int num3 = 0;
        for (int index = 0; index < count; ++index)
        {
          num3 += Classifiers[index];
          if (Classifiers[index] < num1)
            num1 = Classifiers[index];
          if (Classifiers[index] > num2)
            num2 = Classifiers[index];
        }
        OleDbCommand oleDbCommand1 = new OleDbCommand();
        OleDbCommand oleDbCommand2 = new OleDbCommand();
        oleDbCommand1.Connection = EstimationDBCon;
        oleDbCommand1.CommandText = "SELECT TableOfStatisticalEstimates.StatisticalTableId, TableOfStatisticalEstimates.TableTitle  FROM PartitionDefinitionsTable INNER JOIN TableOfStatisticalEstimates ON  PartitionDefinitionsTable.StatisticalTableId = TableOfStatisticalEstimates.StatisticalTableId  WHERE (((TableOfStatisticalEstimates.TableData)=" + TableData.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "))  GROUP BY TableOfStatisticalEstimates.StatisticalTableId, TableOfStatisticalEstimates.TableTitle  HAVING (((Count(PartitionDefinitionsTable.PartitionOrder))=" + count.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ") AND  ((Max(PartitionDefinitionsTable.ClassifierId))=" + num2.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ") AND  ((Min(PartitionDefinitionsTable.ClassifierId))=" + num1.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ") AND  ((Sum(PartitionDefinitionsTable.ClassifierId))=" + num3.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "))  ORDER BY TableOfStatisticalEstimates.StatisticalTableId";
        OleDbDataReader oleDbDataReader1 = oleDbCommand1.ExecuteReader();
        while (oleDbDataReader1.Read())
        {
          int int32 = oleDbDataReader1.GetInt32(oleDbDataReader1.GetOrdinal("StatisticalTableId"));
          oleDbCommand2.Connection = EstimationDBCon;
          oleDbCommand2.CommandText = "select * from PartitionDefinitionsTable where StatisticalTableID=" + int32.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " Order by PartitionOrder";
          List<int> source = new List<int>();
          OleDbDataReader oleDbDataReader2 = oleDbCommand2.ExecuteReader();
          while (oleDbDataReader2.Read())
            source.Add(oleDbDataReader2.GetInt32(oleDbDataReader2.GetOrdinal("ClassifierID")));
          oleDbDataReader2.Close();
          if (Classifiers.Count == source.Count)
          {
            bool flag = true;
            for (int index = 0; index < Classifiers.Count; ++index)
            {
              if (Classifiers.ElementAt<int>(index) != source.ElementAt<int>(index))
                flag = false;
            }
            if (flag)
            {
              string estimationTable = oleDbDataReader1.GetString(oleDbDataReader1.GetOrdinal("TableTitle"));
              oleDbDataReader1.Close();
              return estimationTable;
            }
          }
        }
        oleDbDataReader1.Close();
        List<string> source1 = new List<string>();
        foreach (int classifier in Classifiers)
        {
          oleDbCommand1.CommandText = "select * from Classifiers where ClassifierId=" + classifier.ToString();
          OleDbDataReader oleDbDataReader3 = oleDbCommand1.ExecuteReader();
          oleDbDataReader3.Read();
          source1.Add(oleDbDataReader3.GetString(oleDbDataReader3.GetOrdinal("ClassAbbreviation")));
          oleDbDataReader3.Close();
        }
        string estimationTable1 = "";
        string str1 = "";
        foreach (string str2 in source1)
        {
          str1 = str1 + str2 + ",";
          estimationTable1 = estimationTable1 + "_" + str2.Replace(" ", "_");
        }
        string str3 = str1.Substring(0, str1.Length - 1);
        string[] restrictionValues = new string[4]
        {
          null,
          null,
          null,
          "TABLE"
        };
        List<string> stringList = new List<string>();
        foreach (DataRow row in (InternalDataCollectionBase) EstimationDBCon.GetSchema("Tables", restrictionValues).Rows)
          stringList.Add(row.ItemArray[2].ToString().ToUpper());
        bool flag1 = true;
        for (int index = 0; index <= 1000; ++index)
        {
          string str4 = index != 0 ? estimationTable1.ToUpper() + TableData.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "_" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture) : estimationTable1.ToUpper() + TableData.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          flag1 = false;
          foreach (string str5 in stringList)
          {
            if (str4 == str5)
              flag1 = true;
          }
          if (!flag1)
          {
            estimationTable1 = str4;
            break;
          }
        }
        if (flag1)
          throw new Exception("Can not get estimation table name");
        string str6 = "CREATE TABLE [" + estimationTable1 + "] ([YearGuid] GUID DEFAULT \"GenGUID()\",";
        foreach (string str7 in source1)
          str6 = str6 + "[" + str7 + "] LONG NOT NULL DEFAULT 0,";
        string str8 = str6 + "[EstimateType] LONG NOT NULL DEFAULT 0," + "[EquationType] LONG NOT NULL DEFAULT 0," + "[EstimateValue] DOUBLE NOT NULL DEFAULT 0," + "[EstimateStandardError] DOUBLE NOT NULL DEFAULT 0," + "[EstimateUnitsId] LONG NOT NULL DEFAULT 0)";
        oleDbCommand1.CommandText = str8;
        oleDbCommand1.ExecuteNonQuery();
        int num4 = 1;
        int num5 = 1;
        oleDbCommand1.CommandText = "SELECT TOP 1 StatisticalTableId, TableEntryId FROM TableOfStatisticalEstimates";
        OleDbDataReader oleDbDataReader4 = oleDbCommand1.ExecuteReader();
        if (oleDbDataReader4.HasRows)
        {
          oleDbDataReader4.Close();
          oleDbCommand1.CommandText = "SELECT Max(TableOfStatisticalEstimates.StatisticalTableId) AS MaxOfStatisticalTableId, Max(TableOfStatisticalEstimates.TableEntryId) AS MaxOfTableEntryId FROM TableOfStatisticalEstimates";
          oleDbDataReader4 = oleDbCommand1.ExecuteReader();
          if (oleDbDataReader4.Read())
          {
            num4 = oleDbDataReader4.GetInt32(oleDbDataReader4.GetOrdinal("MaxOfTableEntryId")) + 1;
            num5 = oleDbDataReader4.GetInt32(oleDbDataReader4.GetOrdinal("MaxOfStatisticalTableId")) + 1;
          }
        }
        oleDbDataReader4.Close();
        oleDbCommand1.CommandText = "delete * from [TableOfStatisticalEstimates] where [StatisticalTableId]=" + num5.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        oleDbCommand1.ExecuteNonQuery();
        oleDbCommand1.CommandText = "INSERT INTO [TableOfStatisticalEstimates] ([TableEntryId],[StatisticalTableId],[TableTitle],[TableData],[TableDescription],[TableFlag]) VALUES(" + num4.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + num5.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ",\"" + estimationTable1 + "\"," + TableData.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ",\"" + str3 + "\",0)";
        oleDbCommand1.ExecuteNonQuery();
        for (int index = 1; index <= source1.Count; ++index)
        {
          oleDbCommand1.CommandText = "INSERT INTO [PartitionDefinitionsTable] ([StatisticalTableId],[ClassifierId],[PartitionOrder],[PartitionName],[PartitionFlag]) VALUES(" + num5.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + Classifiers.ElementAt<int>(index - 1).ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + index.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ",\"" + source1.ElementAt<string>(index - 1) + "\",0)";
          oleDbCommand1.ExecuteNonQuery();
        }
        return estimationTable1;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void TransferingWholeTableData(
      OleDbConnection uforeDBCon,
      string uforeDBSql,
      string[] uforeDBtableFields,
      Guid inputYearGuiId,
      OleDbConnection inputDBCon,
      string inputDBTable,
      string[] inputDBtableFields,
      int fieldNo,
      int[] uforeUnitsToReportUnits)
    {
      try
      {
        OleDbCommand oleDbCommand1 = new OleDbCommand();
        oleDbCommand1.Connection = uforeDBCon;
        oleDbCommand1.CommandText = uforeDBSql;
        OleDbDataReader oleDbDataReader = oleDbCommand1.ExecuteReader();
        int num = 1;
        int[] numArray = new int[fieldNo + 1];
        for (int index = 1; index <= fieldNo; ++index)
          numArray[index] = oleDbDataReader.GetOrdinal(uforeDBtableFields[index]);
        int ordinal1 = oleDbDataReader.GetOrdinal("EstimateTypeId");
        oleDbDataReader.GetOrdinal("EquationTypeId");
        int ordinal2 = oleDbDataReader.GetOrdinal("EstimateValue");
        int ordinal3 = oleDbDataReader.GetOrdinal("EstimateStandardError");
        int ordinal4 = oleDbDataReader.GetOrdinal("EstimationUnitsId");
        string str1 = "Insert into [" + inputDBTable + "]([YearGuid]";
        for (int index = 1; index <= fieldNo; ++index)
          str1 = str1 + ",[" + inputDBtableFields[index] + "]";
        string str2 = str1 + ",[EstimateType],[EquationType],[EstimateValue],[EstimateStandardError],[EstimateUnitsId]) Values({" + inputYearGuiId.ToString() + "}";
        for (int index = 1; index <= fieldNo; ++index)
          str2 = str2 + ",@f" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        string str3 = str2 + ",@et,@eq,@ev,@este,@unit)";
        OleDbCommand oleDbCommand2 = new OleDbCommand();
        oleDbCommand2.Connection = inputDBCon;
        oleDbCommand2.CommandText = str3;
        oleDbCommand2.Parameters.Clear();
        for (int index = 1; index <= fieldNo; ++index)
          oleDbCommand2.Parameters.Add("@f" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture), OleDbType.Integer);
        oleDbCommand2.Parameters.Add("@et", OleDbType.Integer);
        oleDbCommand2.Parameters.Add("@eq", OleDbType.Integer);
        oleDbCommand2.Parameters.Add("@ev", OleDbType.Double);
        oleDbCommand2.Parameters.Add("@este", OleDbType.Double);
        oleDbCommand2.Parameters.Add("@unit", OleDbType.Integer);
        while (oleDbDataReader.Read())
        {
          oleDbCommand2.CommandText = str3;
          for (int index = 1; index <= fieldNo; ++index)
            oleDbCommand2.Parameters["@f" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture)].Value = (object) oleDbDataReader.GetInt32(numArray[index]);
          oleDbCommand2.Parameters["@et"].Value = (object) oleDbDataReader.GetInt32(ordinal1);
          oleDbCommand2.Parameters["@eq"].Value = (object) num;
          oleDbCommand2.Parameters["@ev"].Value = (object) oleDbDataReader.GetDouble(ordinal2);
          oleDbCommand2.Parameters["@este"].Value = (object) oleDbDataReader.GetDouble(ordinal3);
          oleDbCommand2.Parameters["@unit"].Value = (object) uforeUnitsToReportUnits[oleDbDataReader.GetInt32(ordinal4)];
          oleDbCommand2.ExecuteNonQuery();
        }
        oleDbDataReader.Close();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void PrepareSpeciesAndPestDictionary(
      int LocationID,
      string LocationName,
      string SeriesName,
      int TimePeriod,
      string LocationSpeciesDatabase,
      string PestDatabase,
      string InventoryDatabase)
    {
      try
      {
        OleDbCommand oleDbCommand = new OleDbCommand();
        using (OleDbConnection oleDbConnection1 = new OleDbConnection())
        {
          int num1 = 0;
          EngineModel.OpenConnection(oleDbConnection1, LocationSpeciesDatabase);
          oleDbCommand.Connection = oleDbConnection1;
          int num2 = LocationID;
          while (num1 == 0)
          {
            oleDbCommand.Connection = oleDbConnection1;
            oleDbCommand.CommandText = "select * from _locationRelations WHere LocationId=" + num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
            num2 = 0;
            while (oleDbDataReader.Read())
            {
              num2 = oleDbDataReader.GetInt32(oleDbDataReader.GetOrdinal("ParentId"));
              if (oleDbDataReader.GetInt32(oleDbDataReader.GetOrdinal("Level")) == 3)
              {
                num1 = oleDbDataReader.GetInt32(oleDbDataReader.GetOrdinal("LocationId"));
                break;
              }
            }
            oleDbDataReader.Close();
            if (num2 == 0)
              break;
          }
          Dictionary<int, int> dictionary = new Dictionary<int, int>();
          oleDbCommand.Connection = oleDbConnection1;
          oleDbCommand.CommandText = "select distinct SpeciesID from _SpeciesNativeLocation where LocationId=" + num1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          OleDbDataReader oleDbDataReader1 = oleDbCommand.ExecuteReader();
          while (oleDbDataReader1.Read())
            dictionary.Add(oleDbDataReader1.GetInt32(0), oleDbDataReader1.GetInt32(0));
          oleDbDataReader1.Close();
          string str = "";
          using (OleDbConnection oleDbConnection2 = new OleDbConnection())
          {
            EngineModel.OpenConnection(oleDbConnection2, InventoryDatabase);
            oleDbCommand.Connection = oleDbConnection2;
            oleDbCommand.CommandText = "Select distinct SppCode from PermanentTreesTable  Where LocationName='" + LocationName.Replace("'", "''") + "' and SeriesName='" + SeriesName.Replace("'", "''") + "' and Year=" + TimePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " Union  Select distinct SppCode from ShrubsTable  Where LocationName='" + LocationName.Replace("'", "''") + "' and SeriesName='" + SeriesName.Replace("'", "''") + "' and Year=" + TimePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            OleDbDataReader oleDbDataReader2 = oleDbCommand.ExecuteReader();
            while (oleDbDataReader2.Read())
              str = str + "'" + oleDbDataReader2.GetString(oleDbDataReader2.GetOrdinal("SppCode")) + "',";
            str = str.Substring(0, str.Length - 1);
            oleDbDataReader2.Close();
            EngineModel.CloseConnection(oleDbConnection2);
          }
          oleDbCommand.Connection = oleDbConnection1;
          oleDbCommand.CommandText = "SELECT t1.SpeciesId AS sppId1, t1.ScientificName AS sName1, t1.CommonName AS cName1, t1.Code AS Code1, t1.ParentId AS ParentID1, t1.SpeciesTypeId AS sppTypeId1, t1.LeafTypeId AS LeafTypeID1,  t2.ScientificName AS sName2, t2.CommonName AS cName2, t2.Code AS Code2, t2.ParentId AS ParentID2, t2.SpeciesTypeId AS sppTypeId2, t2.LeafTypeId AS LeafTypeID2,  t3.ScientificName AS sName3, t3.CommonName AS cName3, t3.Code AS Code3, t3.ParentId AS ParentId3, t3.SpeciesTypeId AS sppTypeId3, t3.LeafTypeId AS LeafTypeId3  FROM (_Species AS t1 LEFT JOIN _Species AS t2 ON t1.ParentId = t2.SpeciesId) LEFT JOIN _Species AS t3 ON t2.ParentId = t3.SpeciesId  WHERE t1.Code In (" + str + ")";
          OleDbDataReader oleDbDataReader3 = oleDbCommand.ExecuteReader();
          while (oleDbDataReader3.Read())
          {
            string key = oleDbDataReader3.GetString(oleDbDataReader3.GetOrdinal("Code1"));
            Species species = new Species()
            {
              SpeciesCommonName = oleDbDataReader3.GetString(oleDbDataReader3.GetOrdinal("cName1")),
              SpeciesShortScientificName = oleDbDataReader3.GetString(oleDbDataReader3.GetOrdinal("sName1")),
              SppCode = key,
              SppID = oleDbDataReader3.GetInt32(oleDbDataReader3.GetOrdinal("sppId1"))
            };
            species.NativeToState = !dictionary.ContainsKey(species.SppID) ? "NO" : "YES";
            switch (oleDbDataReader3.GetInt32(oleDbDataReader3.GetOrdinal("sppTypeId1")))
            {
              case 1:
                species.GenusCode = key;
                species.GenusCommonName = species.SpeciesCommonName;
                species.GenusScientificName = species.SpeciesShortScientificName;
                species.SpeciesScientificName = species.SpeciesShortScientificName;
                species.LeafType = !(key == "PICLASS") ? "D" : "E";
                this.allSpecies.Add(key, species);
                continue;
              case 5:
                species.GenusCode = key;
                species.GenusCommonName = species.SpeciesShortScientificName;
                species.GenusScientificName = species.SpeciesShortScientificName;
                species.SpeciesScientificName = species.SpeciesShortScientificName;
                species.LeafType = oleDbDataReader3.GetInt32(oleDbDataReader3.GetOrdinal("LeafTypeID1")) != 2 ? "D" : "E";
                this.allSpecies.Add(key, species);
                continue;
              case 7:
                species.LeafType = oleDbDataReader3.GetInt32(oleDbDataReader3.GetOrdinal("LeafTypeID1")) != 2 ? "D" : "E";
                if (oleDbDataReader3.GetInt32(oleDbDataReader3.GetOrdinal("sppTypeId2")) == 5)
                {
                  species.GenusCode = oleDbDataReader3.GetString(oleDbDataReader3.GetOrdinal("Code2"));
                  species.GenusCommonName = oleDbDataReader3.GetString(oleDbDataReader3.GetOrdinal("cName2"));
                  species.GenusScientificName = oleDbDataReader3.GetString(oleDbDataReader3.GetOrdinal("sName2"));
                  species.SpeciesScientificName = species.GenusScientificName + " " + species.SpeciesShortScientificName;
                }
                else
                {
                  species.GenusCode = oleDbDataReader3.GetString(oleDbDataReader3.GetOrdinal("Code3"));
                  species.GenusCommonName = oleDbDataReader3.GetString(oleDbDataReader3.GetOrdinal("cName3"));
                  species.GenusScientificName = oleDbDataReader3.GetString(oleDbDataReader3.GetOrdinal("sName3"));
                  species.SpeciesScientificName = species.GenusScientificName + " " + oleDbDataReader3.GetString(oleDbDataReader3.GetOrdinal("sName2")) + " " + species.SpeciesShortScientificName;
                }
                this.allSpecies.Add(key, species);
                continue;
              default:
                int num3 = (int) MessageBox.Show(key + " " + EngineModelRes.SppCodeNotSppGenusClass, EngineModelRes.EngineTitle);
                continue;
            }
          }
          oleDbDataReader3.Close();
          EngineModel.CloseConnection(oleDbConnection1);
        }
        using (OleDbConnection oleDbConnection = new OleDbConnection())
        {
          EngineModel.OpenConnection(oleDbConnection, PestDatabase);
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "select * from Pests";
          OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
          while (oleDbDataReader.Read())
            this.allPests.Add(oleDbDataReader.GetString(oleDbDataReader.GetOrdinal("ScientificName")), new Pest()
            {
              PestId = oleDbDataReader.GetInt32(oleDbDataReader.GetOrdinal("Id")),
              CommonName = oleDbDataReader.GetString(oleDbDataReader.GetOrdinal("CommonName")),
              ScientificName = oleDbDataReader.GetString(oleDbDataReader.GetOrdinal("ScientificName")),
              Category = oleDbDataReader.GetString(oleDbDataReader.GetOrdinal("Category"))
            });
          oleDbDataReader.Close();
          EngineModel.CloseConnection(oleDbConnection);
          this.allPests.Add("None", new Pest()
          {
            PestId = 0,
            CommonName = "None",
            ScientificName = "None",
            Category = "None"
          });
          this.allPests.Add("Affected", new Pest()
          {
            PestId = 10000,
            CommonName = "Affected",
            ScientificName = "Affected",
            Category = "Affected"
          });
          this.allPests.Add("Unknown", new Pest()
          {
            PestId = 9999,
            CommonName = "Unknown",
            ScientificName = "Unknown",
            Category = "Unknown"
          });
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static void OpenConnection(OleDbConnection conToOpen, string databaseName)
    {
      try
      {
        conToOpen.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + databaseName;
        conToOpen.Open();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
      }
    }

    public static void CloseConnection(OleDbConnection conToClose)
    {
      if (conToClose == null || conToClose.State == ConnectionState.Closed)
        return;
      conToClose.Close();
    }

    private void InsertModelNote(
      string theDatabase,
      Guid yearID,
      string theFile,
      bool isParameterCalculatorNote)
    {
      try
      {
        using (OleDbConnection oleDbConnection = new OleDbConnection())
        {
          EngineModel.OpenConnection(oleDbConnection, theDatabase);
          try
          {
            OleDbCommand oleDbCommand = new OleDbCommand();
            oleDbCommand.Connection = oleDbConnection;
            oleDbCommand.CommandText = "select * from ModelNotes where YearGUID={" + yearID.ToString() + "}";
            OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
            if (oleDbDataReader.Read())
            {
              oleDbDataReader.Close();
              if (isParameterCalculatorNote)
                oleDbCommand.CommandText = "UPDATE ModelNotes SET ParameterCalculatorNote = @note WHERE YearGuid=@id";
              else
                oleDbCommand.CommandText = "UPDATE ModelNotes SET EstimatorNote = @note WHERE YearGuid=@id";
              using (StreamReader streamReader = new StreamReader(theFile))
              {
                oleDbCommand.Parameters.Add("@note", OleDbType.LongVarWChar).Value = (object) streamReader.ReadToEnd();
                streamReader.Close();
              }
              oleDbCommand.Parameters.Add("@id", OleDbType.Guid).Value = (object) yearID;
              oleDbCommand.ExecuteNonQuery();
            }
            else
            {
              oleDbDataReader.Close();
              if (isParameterCalculatorNote)
                oleDbCommand.CommandText = "INSERT INTO ModelNotes (YearGuid,ParameterCalculatorNote) VALUES (@id,@note)";
              else
                oleDbCommand.CommandText = "INSERT INTO ModelNotes (YearGuid,EstimatorNote) VALUES (@id,@note)";
              oleDbCommand.Parameters.Add("@id", OleDbType.Guid).Value = (object) yearID;
              using (StreamReader streamReader = new StreamReader(theFile))
              {
                oleDbCommand.Parameters.Add("@note", OleDbType.LongVarWChar).Value = (object) streamReader.ReadToEnd();
                streamReader.Close();
              }
              oleDbCommand.ExecuteNonQuery();
            }
          }
          catch (Exception ex)
          {
            EngineModel.CloseConnection(oleDbConnection);
            throw;
          }
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void PrepareTablesForPollutionCalculation(
      OleDbConnection inputDbCon,
      OleDbConnection uforeOutputDbCon,
      OleDbConnection uforeOutputInventoryDbCon,
      Guid yearId,
      string LocationName,
      string SeriesName,
      int TimePeriod,
      int StudyAreaClassValueOrder,
      int ClassValueOrderForSpeciesTotal,
      SampleType seriesSampleType)
    {
      try
      {
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        double num4 = 0.0;
        double num5 = 0.0;
        double num6 = 0.0;
        double num7 = 0.0;
        OleDbCommand oleDbCommand1 = new OleDbCommand();
        OleDbCommand oleDbCommand2 = new OleDbCommand();
        oleDbCommand1.Connection = uforeOutputInventoryDbCon;
        oleDbCommand1.CommandText = "select * from StrataTable Where LocationName='" + LocationName.Replace("'", "''") + "' and SeriesName='" + SeriesName.Replace("'", "''") + "' and Year=" + TimePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        OleDbDataReader oleDbDataReader1 = oleDbCommand1.ExecuteReader();
        while (oleDbDataReader1.Read())
        {
          Stratum stratum = new Stratum();
          stratum.ClassValue = oleDbDataReader1.GetInt32(oleDbDataReader1.GetOrdinal("StratumID"));
          stratum.Abbreviation = oleDbDataReader1.GetString(oleDbDataReader1.GetOrdinal("StratumAbbreviation"));
          stratum.Description = oleDbDataReader1.GetString(oleDbDataReader1.GetOrdinal("StratumDescription"));
          switch (oleDbDataReader1.GetInt16(oleDbDataReader1.GetOrdinal("StratumAreaUnits")))
          {
            case 15:
              stratum.AreaHectareBasedEngineInventoryDB = (double) oleDbDataReader1.GetFloat(oleDbDataReader1.GetOrdinal("StratumArea")) / 10000.0;
              break;
            case 16:
              stratum.AreaHectareBasedEngineInventoryDB = 100.0 * (double) oleDbDataReader1.GetFloat(oleDbDataReader1.GetOrdinal("StratumArea"));
              break;
            case 23:
              stratum.AreaHectareBasedEngineInventoryDB = 0.404686 * (double) oleDbDataReader1.GetFloat(oleDbDataReader1.GetOrdinal("StratumArea"));
              break;
            case 24:
              stratum.AreaHectareBasedEngineInventoryDB = (double) oleDbDataReader1.GetFloat(oleDbDataReader1.GetOrdinal("StratumArea"));
              break;
          }
          num7 += stratum.AreaHectareBasedEngineInventoryDB;
          oleDbCommand2.CommandText = "select * from EcoStrata WHERE YearKey={" + yearId.ToString() + "} and Description='" + stratum.Description.Replace("'", "''") + "'";
          oleDbCommand2.Connection = inputDbCon;
          OleDbDataReader oleDbDataReader2 = oleDbCommand2.ExecuteReader();
          if (oleDbDataReader2.Read())
            stratum.AreaHectare = this.currYear.Unit != YearUnit.Metric ? (double) oleDbDataReader2.GetFloat(oleDbDataReader2.GetOrdinal("Size")) * 0.404686 : (double) oleDbDataReader2.GetFloat(oleDbDataReader2.GetOrdinal("Size"));
          oleDbDataReader2.Close();
          this.allStrata.Add(stratum.ClassValue, stratum);
        }
        oleDbDataReader1.Close();
        List<int> Classifiers = new List<int>();
        Classifiers.Add(3);
        Classifiers.Add(7);
        string estimationTable1 = EngineModel.getEstimationTable(inputDbCon, 1, Classifiers);
        int unit1 = EngineModel.getUnit(inputDbCon, 3, 1, 1);
        int num8 = 0;
        int num9 = 0;
        oleDbCommand1.Connection = inputDbCon;
        oleDbCommand1.CommandText = "select * from ClassValueTable Where YearGuid=@yearId and ClassifierId=@classifierId and (ClassValueName=@treeWord or ClassValueName=@shrubWord)";
        oleDbCommand1.Parameters.Clear();
        oleDbCommand1.Parameters.Add("@yearId", OleDbType.Guid).Value = (object) yearId;
        oleDbCommand1.Parameters.Add("@classifierId", OleDbType.Integer).Value = (object) 7;
        oleDbCommand1.Parameters.Add("@treeWord", OleDbType.VarWChar).Value = (object) "Tree";
        oleDbCommand1.Parameters.Add("@shrubWord", OleDbType.VarWChar).Value = (object) "Shrub";
        OleDbDataReader oleDbDataReader3 = oleDbCommand1.ExecuteReader();
        while (oleDbDataReader3.Read())
        {
          if (oleDbDataReader3.GetString(oleDbDataReader3.GetOrdinal("ClassValueName")) == "Tree")
            num8 = (int) oleDbDataReader3.GetInt16(oleDbDataReader3.GetOrdinal("ClassValueOrder"));
          else
            num9 = (int) oleDbDataReader3.GetInt16(oleDbDataReader3.GetOrdinal("ClassValueOrder"));
        }
        oleDbDataReader3.Close();
        OleDbCommand oleDbCommand3 = oleDbCommand1;
        string[] strArray = new string[20];
        strArray[0] = "select * from [";
        strArray[1] = estimationTable1;
        strArray[2] = "] where YearGuid={";
        strArray[3] = yearId.ToString();
        strArray[4] = "} and (";
        Classifiers classifiers1 = Classifiers.GroundCover;
        strArray[5] = classifiers1.ToString();
        strArray[6] = "=";
        strArray[7] = num8.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray[8] = " or ";
        classifiers1 = Classifiers.GroundCover;
        strArray[9] = classifiers1.ToString();
        strArray[10] = "=";
        strArray[11] = num9.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray[12] = ")and EstimateType=";
        strArray[13] = 12.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray[14] = " and EstimateUnitsId=";
        strArray[15] = unit1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray[16] = " order by ";
        Classifiers classifiers2 = Classifiers.Strata;
        strArray[17] = classifiers2.ToString();
        strArray[18] = ",";
        classifiers2 = Classifiers.GroundCover;
        strArray[19] = classifiers2.ToString();
        string str = string.Concat(strArray);
        oleDbCommand3.CommandText = str;
        oleDbCommand1.Parameters.Clear();
        OleDbDataReader oleDbDataReader4 = oleDbCommand1.ExecuteReader();
        while (oleDbDataReader4.Read())
        {
          OleDbDataReader oleDbDataReader5 = oleDbDataReader4;
          OleDbDataReader oleDbDataReader6 = oleDbDataReader4;
          classifiers2 = Classifiers.Strata;
          string name1 = classifiers2.ToString();
          int ordinal1 = oleDbDataReader6.GetOrdinal(name1);
          int int32 = oleDbDataReader5.GetInt32(ordinal1);
          if (int32 == StudyAreaClassValueOrder)
          {
            OleDbDataReader oleDbDataReader7 = oleDbDataReader4;
            OleDbDataReader oleDbDataReader8 = oleDbDataReader4;
            classifiers2 = Classifiers.GroundCover;
            string name2 = classifiers2.ToString();
            int ordinal2 = oleDbDataReader8.GetOrdinal(name2);
            if (oleDbDataReader7.GetInt32(ordinal2) == num8)
              num1 = oleDbDataReader4.GetDouble(oleDbDataReader4.GetOrdinal("EstimateValue"));
            else
              num2 = oleDbDataReader4.GetDouble(oleDbDataReader4.GetOrdinal("EstimateValue"));
          }
          else
          {
            Stratum allStratum = this.allStrata[int32];
            OleDbDataReader oleDbDataReader9 = oleDbDataReader4;
            OleDbDataReader oleDbDataReader10 = oleDbDataReader4;
            classifiers2 = Classifiers.GroundCover;
            string name3 = classifiers2.ToString();
            int ordinal3 = oleDbDataReader10.GetOrdinal(name3);
            if (oleDbDataReader9.GetInt32(ordinal3) == num8)
              allStratum.TreeCoverPercentageBasedEngineInventoryDB = oleDbDataReader4.GetDouble(oleDbDataReader4.GetOrdinal("EstimateValue"));
            else
              allStratum.ShrubCoverPercentageBasedEngineInventoryDB = oleDbDataReader4.GetDouble(oleDbDataReader4.GetOrdinal("EstimateValue"));
          }
        }
        oleDbDataReader4.Close();
        Classifiers.Clear();
        Classifiers.Add(3);
        string estimationTable2 = EngineModel.getEstimationTable(inputDbCon, 2, Classifiers);
        int unit2 = EngineModel.getUnit(inputDbCon, 16, 1, 1);
        oleDbCommand1.Connection = inputDbCon;
        oleDbCommand1.CommandText = "select * from [" + estimationTable2 + "] where YearGuid={" + yearId.ToString() + "} and EstimateType=" + 5.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and EstimateUnitsId=" + unit2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        oleDbCommand1.Parameters.Clear();
        OleDbDataReader oleDbDataReader11 = oleDbCommand1.ExecuteReader();
        while (oleDbDataReader11.Read())
        {
          OleDbDataReader oleDbDataReader12 = oleDbDataReader11;
          OleDbDataReader oleDbDataReader13 = oleDbDataReader11;
          classifiers2 = Classifiers.Strata;
          string name = classifiers2.ToString();
          int ordinal = oleDbDataReader13.GetOrdinal(name);
          int int32 = oleDbDataReader12.GetInt32(ordinal);
          if (int32 != StudyAreaClassValueOrder)
          {
            this.allStrata[int32].TreeLeafAreaHectare = 100.0 * oleDbDataReader11.GetDouble(oleDbDataReader11.GetOrdinal("EstimateValue"));
            num3 += 100.0 * oleDbDataReader11.GetDouble(oleDbDataReader11.GetOrdinal("EstimateValue"));
          }
        }
        oleDbDataReader11.Close();
        Classifiers.Clear();
        Classifiers.Add(3);
        string estimationTable3 = EngineModel.getEstimationTable(inputDbCon, 3, Classifiers);
        int unit3 = EngineModel.getUnit(inputDbCon, 16, 1, 1);
        oleDbCommand1.Connection = inputDbCon;
        oleDbCommand1.CommandText = "select * from [" + estimationTable3 + "] where YearGuid={" + yearId.ToString() + "} and EstimateType=" + 5.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and EstimateUnitsId=" + unit3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        oleDbCommand1.Parameters.Clear();
        OleDbDataReader oleDbDataReader14 = oleDbCommand1.ExecuteReader();
        while (oleDbDataReader14.Read())
        {
          OleDbDataReader oleDbDataReader15 = oleDbDataReader14;
          OleDbDataReader oleDbDataReader16 = oleDbDataReader14;
          classifiers2 = Classifiers.Strata;
          string name = classifiers2.ToString();
          int ordinal = oleDbDataReader16.GetOrdinal(name);
          int int32 = oleDbDataReader15.GetInt32(ordinal);
          if (int32 != StudyAreaClassValueOrder)
          {
            this.allStrata[int32].ShrubLeafAreaHectare = 100.0 * oleDbDataReader14.GetDouble(oleDbDataReader14.GetOrdinal("EstimateValue"));
            num4 += 100.0 * oleDbDataReader14.GetDouble(oleDbDataReader14.GetOrdinal("EstimateValue"));
          }
        }
        oleDbDataReader14.Close();
        double num10 = num7 * num1 != 0.0 ? num3 / (num7 * num1 / 100.0) : 0.0;
        double num11 = num7 * num2 != 0.0 ? num4 / (num7 * num2 / 100.0) : 0.0;
        oleDbCommand1.Connection = uforeOutputDbCon;
        oleDbCommand1.CommandText = "CREATE TABLE [lu_cover] (TOT_AREA DOUBLE, CM_PSHRB DOUBLE, CM_PTREE DOUBLE, LANDUSE VARCHAR(100), MN_PSHRB DOUBLE, MN_PTREE DOUBLE, LU_DESC VARCHAR(100), LND_AREA DOUBLE, LT_LAI DOUBLE, LS_LAI DOUBLE)";
        oleDbCommand1.ExecuteNonQuery();
        foreach (Stratum stratum in this.allStrata.Values)
        {
          stratum.TreeLAI = stratum.AreaHectareBasedEngineInventoryDB * stratum.TreeCoverPercentageBasedEngineInventoryDB != 0.0 ? stratum.TreeLeafAreaHectare / (stratum.AreaHectareBasedEngineInventoryDB * stratum.TreeCoverPercentageBasedEngineInventoryDB / 100.0) : 0.0;
          stratum.ShrubLAI = stratum.AreaHectareBasedEngineInventoryDB * stratum.ShrubCoverPercentageBasedEngineInventoryDB != 0.0 ? stratum.ShrubLeafAreaHectare / (stratum.AreaHectareBasedEngineInventoryDB * stratum.ShrubCoverPercentageBasedEngineInventoryDB / 100.0) : 0.0;
          if (stratum.TreeLAI != 0.0)
          {
            oleDbCommand1.CommandText = "INSERT INTO [lu_cover] (TOT_AREA,CM_PSHRB,CM_PTREE,LANDUSE,MN_PSHRB,MN_PTREE,LU_DESC,LND_AREA,LT_LAI,LS_LAI) VALUES(" + num7.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + num2.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + num1.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ",'" + stratum.Abbreviation.Replace("'", "''") + "'," + stratum.ShrubCoverPercentageBasedEngineInventoryDB.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + stratum.TreeCoverPercentageBasedEngineInventoryDB.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ",'" + stratum.Description.Replace("'", "''") + "'," + stratum.AreaHectareBasedEngineInventoryDB.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + stratum.TreeLAI.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + stratum.ShrubLAI.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ")";
            oleDbCommand1.ExecuteNonQuery();
          }
        }
        Classifiers.Clear();
        Classifiers.Add(3);
        Classifiers.Add(4);
        string estimationTable4 = EngineModel.getEstimationTable(inputDbCon, 2, Classifiers);
        int unit4 = EngineModel.getUnit(inputDbCon, 22, 1, 1);
        oleDbCommand1.Connection = inputDbCon;
        oleDbCommand1.CommandText = "select * from [" + estimationTable4 + "] where YearGuid={" + yearId.ToString() + "} and EstimateType=" + 6.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and EstimateUnitsId=" + unit4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        oleDbCommand1.Parameters.Clear();
        OleDbDataReader oleDbDataReader17 = oleDbCommand1.ExecuteReader();
        while (oleDbDataReader17.Read())
        {
          int int32_1 = oleDbDataReader17.GetInt32(oleDbDataReader17.GetOrdinal(Classifiers.Strata.ToString()));
          int int32_2 = oleDbDataReader17.GetInt32(oleDbDataReader17.GetOrdinal(Classifiers.Species.ToString()));
          if (int32_1 != StudyAreaClassValueOrder && int32_2 != ClassValueOrderForSpeciesTotal)
          {
            string key = int32_1.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "_" + this.allSpecies[this.allSpeciesClassValueOrderToSppCode[int32_2]].GenusScientificName + "_TREE";
            StratumGenus stratumGenus;
            if (this.allStratumGenus.ContainsKey(key))
            {
              stratumGenus = this.allStratumGenus[key];
            }
            else
            {
              Species allSpecy = this.allSpecies[this.allSpeciesClassValueOrderToSppCode[int32_2]];
              stratumGenus = new StratumGenus();
              stratumGenus.Form_Ind = "TREE";
              stratumGenus.GenusScientificName = allSpecy.GenusScientificName;
              stratumGenus.LeafType = !(allSpecy.LeafType == "E") ? "DECIDUOUS" : "EVERGREEN";
              Stratum allStratum = this.allStrata[int32_1];
              stratumGenus.StratumAbbreviation = allStratum.Abbreviation;
              stratumGenus.StratumDescription = allStratum.Description;
              stratumGenus.StratumClassValue = int32_1;
              this.allStratumGenus.Add(key, stratumGenus);
            }
            stratumGenus.LeafBiomassKilograms += 1000.0 * oleDbDataReader17.GetDouble(oleDbDataReader17.GetOrdinal("EstimateValue"));
            num5 += 1000.0 * oleDbDataReader17.GetDouble(oleDbDataReader17.GetOrdinal("EstimateValue"));
          }
        }
        oleDbDataReader17.Close();
        Classifiers.Clear();
        Classifiers.Add(3);
        Classifiers.Add(4);
        string estimationTable5 = EngineModel.getEstimationTable(inputDbCon, 3, Classifiers);
        unit4 = EngineModel.getUnit(inputDbCon, 22, 24, 1);
        oleDbCommand1.Connection = inputDbCon;
        oleDbCommand1.CommandText = "select * from [" + estimationTable5 + "] where YearGuid={" + yearId.ToString() + "} and EstimateType=" + 6.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and EstimateUnitsId=" + unit4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        oleDbCommand1.Parameters.Clear();
        OleDbDataReader oleDbDataReader18 = oleDbCommand1.ExecuteReader();
        while (oleDbDataReader18.Read())
        {
          int int32_3 = oleDbDataReader18.GetInt32(oleDbDataReader18.GetOrdinal(Classifiers.Strata.ToString()));
          int int32_4 = oleDbDataReader18.GetInt32(oleDbDataReader18.GetOrdinal(Classifiers.Species.ToString()));
          if (int32_3 != StudyAreaClassValueOrder && int32_4 != ClassValueOrderForSpeciesTotal)
          {
            string key = int32_3.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "_" + this.allSpecies[this.allSpeciesClassValueOrderToSppCode[int32_4]].GenusScientificName + "_SHRUB";
            Stratum allStratum = this.allStrata[int32_3];
            StratumGenus stratumGenus;
            if (this.allStratumGenus.ContainsKey(key))
            {
              stratumGenus = this.allStratumGenus[key];
            }
            else
            {
              Species allSpecy = this.allSpecies[this.allSpeciesClassValueOrderToSppCode[int32_4]];
              stratumGenus = new StratumGenus();
              stratumGenus.Form_Ind = "SHRUB";
              stratumGenus.GenusScientificName = allSpecy.GenusScientificName;
              stratumGenus.LeafType = !(allSpecy.LeafType == "E") ? "DECIDUOUS" : "EVERGREEN";
              stratumGenus.StratumAbbreviation = allStratum.Abbreviation;
              stratumGenus.StratumDescription = allStratum.Description;
              stratumGenus.StratumClassValue = int32_3;
              this.allStratumGenus.Add(key, stratumGenus);
            }
            stratumGenus.LeafBiomassKilograms += 1000.0 * oleDbDataReader18.GetDouble(oleDbDataReader18.GetOrdinal("EstimateValue")) * allStratum.AreaHectareBasedEngineInventoryDB;
            num6 += 1000.0 * oleDbDataReader18.GetDouble(oleDbDataReader18.GetOrdinal("EstimateValue")) * allStratum.AreaHectareBasedEngineInventoryDB;
          }
        }
        oleDbDataReader18.Close();
        oleDbCommand1.Connection = uforeOutputDbCon;
        oleDbCommand1.CommandText = "CREATE TABLE [lbiomass] (CTOT_LB DOUBLE, CTOT_LIV DOUBLE, LANDUSE VARCHAR(100), GENUS VARCHAR(100), LVE_TYPE VARCHAR(20), STOT_LB DOUBLE, STOT_LIV DOUBLE, FORM_IND VARCHAR(10))";
        oleDbCommand1.ExecuteNonQuery();
        foreach (StratumGenus stratumGenus in this.allStratumGenus.Values)
        {
          if (stratumGenus.Form_Ind == "TREE")
          {
            oleDbCommand1.CommandText = "INSERT INTO lbiomass (CTOT_LB,LANDUSE,GENUS,LVE_TYPE,STOT_LB,FORM_IND) VALUES(" + num5.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ",'" + stratumGenus.StratumAbbreviation.Replace("'", "''") + "','" + stratumGenus.GenusScientificName.Replace("'", "''") + "','" + stratumGenus.LeafType + "'," + stratumGenus.LeafBiomassKilograms.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ",'TREE')";
            oleDbCommand1.ExecuteNonQuery();
          }
          else
          {
            oleDbCommand1.CommandText = "INSERT INTO lbiomass (CTOT_LB,LANDUSE,GENUS,LVE_TYPE,STOT_LB,FORM_IND) VALUES(" + num6.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ",'" + stratumGenus.StratumAbbreviation.Replace("'", "''") + "','" + stratumGenus.GenusScientificName.Replace("'", "''") + "','" + stratumGenus.LeafType + "'," + stratumGenus.LeafBiomassKilograms.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ",'SHRUB')";
            oleDbCommand1.ExecuteNonQuery();
          }
        }
        double num12 = 0.0;
        double num13 = 0.0;
        double num14 = 0.0;
        double num15 = 0.0;
        double num16;
        double num17;
        if (seriesSampleType == SampleType.Inventory)
        {
          oleDbCommand1.Connection = uforeOutputInventoryDbCon;
          oleDbCommand1.CommandText = "SELECT TreesTable.CrownGroundArea, PermanentTreesTable.SppCode FROM PermanentTreesTable INNER JOIN TreesTable ON (PermanentTreesTable.TreeID = TreesTable.TreeID) AND (PermanentTreesTable.SubplotID = TreesTable.SubplotID) AND (PermanentTreesTable.PlotID = TreesTable.PlotID) AND (PermanentTreesTable.Year = TreesTable.Year) AND (PermanentTreesTable.SeriesName = TreesTable.SeriesName) AND (PermanentTreesTable.LocationName = TreesTable.LocationName) WHERE TreesTable.LocationName='" + LocationName + "' AND TreesTable.SeriesName='" + SeriesName + "' AND TreesTable.Year=" + TimePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " Order by TreesTable.PlotId, TreesTable.TreeId";
          OleDbDataReader oleDbDataReader19 = oleDbCommand1.ExecuteReader();
          while (oleDbDataReader19.Read())
          {
            Species allSpecy = this.allSpecies[oleDbDataReader19.GetString(oleDbDataReader19.GetOrdinal("SppCode"))];
            num12 += oleDbDataReader19.GetDouble(oleDbDataReader19.GetOrdinal("CrownGroundArea"));
            if (allSpecy.LeafType == "E")
              num13 += oleDbDataReader19.GetDouble(oleDbDataReader19.GetOrdinal("CrownGroundArea"));
          }
          oleDbDataReader19.Close();
          num16 = num12 != 0.0 ? num13 / num12 * 100.0 : 0.0;
          num17 = 0.0;
        }
        else
        {
          oleDbCommand1.Connection = uforeOutputInventoryDbCon;
          oleDbCommand1.CommandText = "SELECT PlotsTable.StratumID, Sum(SubplotsTable.SubplotSize) AS SumOfSubplotSize  FROM PlotsTable INNER JOIN SubplotsTable ON (PlotsTable.PlotID = SubplotsTable.PlotID) AND (PlotsTable.Year = SubplotsTable.Year) AND (PlotsTable.SeriesName = SubplotsTable.SeriesName) AND (PlotsTable.LocationName = SubplotsTable.LocationName)  WHERE PlotsTable.LocationName='" + LocationName.Replace("'", "''") + "' AND PlotsTable.SeriesName='" + SeriesName.Replace("'", "''") + "' AND PlotsTable.Year=" + TimePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " GROUP BY PlotsTable.StratumID";
          OleDbDataReader oleDbDataReader20 = oleDbCommand1.ExecuteReader();
          while (oleDbDataReader20.Read())
            this.allStrata[oleDbDataReader20.GetInt32(oleDbDataReader20.GetOrdinal("StratumID"))].sumPlotAreaHectare = oleDbDataReader20.GetDouble(oleDbDataReader20.GetOrdinal("SumOfSubplotSize"));
          oleDbDataReader20.Close();
          foreach (Stratum stratum in this.allStrata.Values)
          {
            oleDbCommand1.CommandText = "SELECT PermanentTreesTable.SppCode, TreesTable.CrownGroundArea  FROM ((PlotsTable INNER JOIN SubplotsTable ON (PlotsTable.PlotID = SubplotsTable.PlotID) AND (PlotsTable.Year = SubplotsTable.Year)  AND (PlotsTable.SeriesName = SubplotsTable.SeriesName)  AND (PlotsTable.LocationName = SubplotsTable.LocationName))  INNER JOIN PermanentTreesTable ON (SubplotsTable.SubplotID = PermanentTreesTable.SubplotID)  AND (SubplotsTable.PlotID = PermanentTreesTable.PlotID)  AND (SubplotsTable.Year = PermanentTreesTable.Year)  AND (SubplotsTable.SeriesName = PermanentTreesTable.SeriesName)  AND (SubplotsTable.LocationName = PermanentTreesTable.LocationName))  INNER JOIN TreesTable ON (PermanentTreesTable.TreeID = TreesTable.TreeID)  AND (PermanentTreesTable.SubplotID = TreesTable.SubplotID)  AND (PermanentTreesTable.PlotID = TreesTable.PlotID)  AND (PermanentTreesTable.Year = TreesTable.Year)  AND (PermanentTreesTable.SeriesName = TreesTable.SeriesName)  AND (PermanentTreesTable.LocationName = TreesTable.LocationName)  WHERE PlotsTable.LocationName='" + LocationName.Replace("'", "''") + "' AND PlotsTable.SeriesName='" + SeriesName.Replace("'", "''") + "' AND PlotsTable.Year=" + TimePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND PlotsTable.StratumID=" + stratum.ClassValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            OleDbDataReader oleDbDataReader21 = oleDbCommand1.ExecuteReader();
            while (oleDbDataReader21.Read())
            {
              stratum.sumPlotTreeCoverSquareMeters += oleDbDataReader21.GetDouble(oleDbDataReader21.GetOrdinal("CrownGroundArea"));
              if (this.allSpecies[oleDbDataReader21.GetString(oleDbDataReader21.GetOrdinal("SppCode"))].LeafType == "E")
                stratum.sumPlotTreeCoverEvergreenSquareMeters += oleDbDataReader21.GetDouble(oleDbDataReader21.GetOrdinal("CrownGroundArea"));
            }
            oleDbDataReader21.Close();
            oleDbCommand1.CommandText = "SELECT ShrubsTable.SppCode, ShrubsTable.GroundArea  FROM (PlotsTable INNER JOIN SubplotsTable ON (PlotsTable.PlotID = SubplotsTable.PlotID)  AND (PlotsTable.Year = SubplotsTable.Year)  AND (PlotsTable.SeriesName = SubplotsTable.SeriesName)  AND (PlotsTable.LocationName = SubplotsTable.LocationName))  INNER JOIN ShrubsTable ON (SubplotsTable.SubplotID = ShrubsTable.SubplotID)  AND (SubplotsTable.PlotID = ShrubsTable.PlotID)  AND (SubplotsTable.Year = ShrubsTable.Year)  AND (SubplotsTable.SeriesName = ShrubsTable.SeriesName)  AND (SubplotsTable.LocationName = ShrubsTable.LocationName)  WHERE PlotsTable.LocationName='" + LocationName.Replace("'", "''") + "' AND PlotsTable.SeriesName='" + SeriesName.Replace("'", "''") + "' AND PlotsTable.Year=" + TimePeriod.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " AND PlotsTable.StratumID=" + stratum.ClassValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            OleDbDataReader oleDbDataReader22 = oleDbCommand1.ExecuteReader();
            while (oleDbDataReader22.Read())
            {
              stratum.sumPlotShrubCoverSquareMeters += oleDbDataReader22.GetDouble(oleDbDataReader22.GetOrdinal("GroundArea"));
              if (this.allSpecies[oleDbDataReader22.GetString(oleDbDataReader22.GetOrdinal("SppCode"))].LeafType == "E")
                stratum.sumPlotShrubEvergreenSquareMeters += oleDbDataReader22.GetDouble(oleDbDataReader22.GetOrdinal("GroundArea"));
            }
            oleDbDataReader22.Close();
            if (stratum.sumPlotAreaHectare != 0.0)
            {
              num12 += stratum.sumPlotTreeCoverSquareMeters * stratum.AreaHectareBasedEngineInventoryDB / stratum.sumPlotAreaHectare;
              num13 += stratum.sumPlotTreeCoverEvergreenSquareMeters * stratum.AreaHectareBasedEngineInventoryDB / stratum.sumPlotAreaHectare;
              num14 += stratum.sumPlotShrubCoverSquareMeters * stratum.AreaHectareBasedEngineInventoryDB / stratum.sumPlotAreaHectare;
              num15 += stratum.sumPlotShrubEvergreenSquareMeters * stratum.AreaHectareBasedEngineInventoryDB / stratum.sumPlotAreaHectare;
            }
          }
          num16 = num12 != 0.0 ? 100.0 * num13 / num12 : 0.0;
          num17 = num14 != 0.0 ? 100.0 * num15 / num14 : 0.0;
        }
        oleDbCommand1.Connection = uforeOutputDbCon;
        oleDbCommand1.CommandText = "CREATE TABLE [study_area_parameters] (TreeLAI DOUBLE, TreeCoverPercent DOUBLE, TreeEvergreenPercent DOUBLE, ShrubLAI DOUBLE, ShrubCoverPercent DOUBLE, ShrubEvergreenPercent DOUBLE, StudyAreaSquareMeters DOUBLE)";
        oleDbCommand1.ExecuteNonQuery();
        oleDbCommand1.CommandText = "INSERT INTO [study_area_parameters] (TreeLAI, TreeCoverPercent, TreeEvergreenPercent, ShrubLAI, ShrubCoverPercent, ShrubEvergreenPercent, StudyAreaSquareMeters) VALUES(" + num10.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + num1.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + num16.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + num11.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + num2.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + num17.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + (num7 * 10000.0).ToString((IFormatProvider) CultureInfo.InvariantCulture) + ")";
        oleDbCommand1.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private bool DataValidation()
    {
      try
      {
        using (OleDbConnection oleDbConnection = new OleDbConnection())
        {
          EngineModel.OpenConnection(oleDbConnection, this.workingInputDatabaseName);
          OleDbCommand oleDbCommand1 = new OleDbCommand();
          oleDbCommand1.Connection = oleDbConnection;
          if ((this.currProject.NationCode == "001" || this.currProject.NationCode == "230") && (int) this.currYearLocation.PollutionYear != (int) this.currYearLocation.WeatherYear)
          {
            int num = (int) MessageBox.Show(EngineModelRes.PollutionYearMustBeWeatherYearMsg, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
            return false;
          }
          if (this.currProject.NationCode == "002")
          {
            if (this.currYearLocation.PollutionYear != (short) 2010)
            {
              int num = (int) MessageBox.Show(EngineModelRes.PollutionYearMustBeMsg + " 2010", EngineModelRes.EngineTitle, MessageBoxButtons.OK);
              return false;
            }
            if (this.currYearLocation.WeatherYear != (short) 2010)
            {
              int num = (int) MessageBox.Show(EngineModelRes.PollutionYearMustBeWeatherYearMsg, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
              return false;
            }
          }
          if (this.currProject.NationCode == "021")
          {
            if (this.currYearLocation.PollutionYear != (short) 2013)
            {
              int num = (int) MessageBox.Show(EngineModelRes.PollutionYearMustBeMsg + " 2013", EngineModelRes.EngineTitle, MessageBoxButtons.OK);
              return false;
            }
            if (this.currYearLocation.WeatherYear != (short) 2013)
            {
              int num = (int) MessageBox.Show(EngineModelRes.PollutionYearMustBeWeatherYearMsg, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
              return false;
            }
          }
          if (this.currSeries.SampleType == SampleType.Inventory)
          {
            double num = this.currYear.Unit != YearUnit.English ? 0.404686 : 0.1;
            oleDbCommand1.CommandText = "Update EcoPlots Set PlotSize=" + num.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " WHERE YearKey={" + this.currYear.Guid.ToString() + "} and PlotSize<=0";
            oleDbCommand1.ExecuteNonQuery();
          }
          else
          {
            oleDbCommand1.CommandText = "select * from EcoStrata where YearKey={" + this.currYear.Guid.ToString() + "} and [Size]<=0";
            OleDbDataReader oleDbDataReader1 = oleDbCommand1.ExecuteReader();
            if (oleDbDataReader1.HasRows)
            {
              oleDbDataReader1.Close();
              int num = (int) MessageBox.Show(EngineModelRes.NegtiveOrZeroStrataSizeMsg, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
              return false;
            }
            oleDbDataReader1.Close();
            oleDbCommand1.CommandText = "SELECT EcoPlots.PlotId, EcoPlots.PlotSize FROM EcoPlots WHERE YearKey={" + this.currYear.Guid.ToString() + "} and PlotSize<=0";
            OleDbDataReader oleDbDataReader2 = oleDbCommand1.ExecuteReader();
            if (oleDbDataReader2.HasRows)
            {
              oleDbDataReader2.Close();
              int num = (int) MessageBox.Show(EngineModelRes.NegativeOrZeroPlotSizeMsg, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
              return false;
            }
            oleDbDataReader2.Close();
            oleDbCommand1.CommandText = "Delete * From EcoPlots where YearKey={" + this.currYear.Guid.ToString() + "} and IsComplete=false";
            int num1 = oleDbCommand1.ExecuteNonQuery();
            if (num1 > 0)
            {
              int num2 = (int) MessageBox.Show(num1.ToString() + " " + EngineModelRes.inCompletePlotsDeletionMsg, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
            }
          }
          oleDbCommand1.CommandText = "UPDATE EcoPlots SET PercentTreeCover = 0 WHERE YearKey={" + this.currYear.Guid.ToString() + "} AND PercentTreeCover < 0";
          oleDbCommand1.ExecuteNonQuery();
          oleDbCommand1.CommandText = "UPDATE EcoPlots SET PercentShrubCover = 0 WHERE YearKey={" + this.currYear.Guid.ToString() + "} AND PercentShrubCover < 0";
          oleDbCommand1.ExecuteNonQuery();
          OleDbCommand oleDbCommand2 = oleDbCommand1;
          Guid guid = this.currYear.Guid;
          string str1 = "UPDATE EcoPlots SET PercentPlantable = 0 WHERE YearKey={" + guid.ToString() + "} AND PercentPlantable < 0";
          oleDbCommand2.CommandText = str1;
          oleDbCommand1.ExecuteNonQuery();
          OleDbCommand oleDbCommand3 = oleDbCommand1;
          guid = this.currYear.Guid;
          string str2 = "UPDATE (EcoPlots INNER JOIN EcoTrees ON EcoPlots.PlotKey = EcoTrees.PlotKey) INNER JOIN EcoBuildings ON EcoTrees.TreeKey = EcoBuildings.TreeKey SET EcoBuildings.DirectiontoBuilding = [EcoBuildings].[DirectiontoBuilding]+360  WHERE EcoBuildings.DirectiontoBuilding<=0 AND EcoPlots.YearKey={" + guid.ToString() + "}";
          oleDbCommand3.CommandText = str2;
          oleDbCommand1.ExecuteNonQuery();
          EngineModel.CloseConnection(oleDbConnection);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, EngineModelRes.EngineTitle, MessageBoxButtons.OK);
        return false;
      }
      return true;
    }

    private void ProcessUFORE_D_B_I_background(
      ProgramSession m_ps,
      string inputDatabase,
      string LocSppDatabase,
      string uforeOutputDatabase,
      string benMAPDatabase)
    {
      using (OleDbConnection oleDbConnection = new OleDbConnection())
      {
        try
        {
          EngineModel.OpenConnection(oleDbConnection, LocSppDatabase);
          OleDbCommand oleDbCommand = new OleDbCommand();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "select * from _LocationRelations Where LocationId=" + this.currProject.LocationId.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          OleDbDataReader oleDbDataReader1 = oleDbCommand.ExecuteReader();
          oleDbDataReader1.Read();
          string str;
          switch (oleDbDataReader1.GetInt32(oleDbDataReader1.GetOrdinal("Level")))
          {
            case 3:
              str = "PriPart";
              break;
            case 4:
              str = "SecPart";
              break;
            case 5:
              str = "TerPart";
              break;
            default:
              str = "TerPart";
              break;
          }
          oleDbDataReader1.Close();
          EngineModel.CloseConnection(oleDbConnection);
          EngineModel.OpenConnection(oleDbConnection, uforeOutputDatabase);
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "select * from study_area_parameters";
          OleDbDataReader oleDbDataReader2 = oleDbCommand.ExecuteReader();
          oleDbDataReader2.Read();
          double dArea = oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("StudyAreaSquareMeters"));
          double num1 = oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("TreeLAI"));
          if (num1 < 0.01)
            num1 = 0.0101;
          double dTrCov1 = oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("TreeCoverPercent"));
          if (dTrCov1 == 0.0)
            dTrCov1 = 1E-08;
          else if (dTrCov1 >= 100.0)
            dTrCov1 = 99.99999999;
          double num2 = oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("TreeEvergreenPercent"));
          if (num2 == 0.0)
            num2 = 1E-08;
          else if (num2 >= 100.0)
            num2 = 99.99999999;
          double num3 = oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("ShrubLAI"));
          if (num3 < 0.01)
            num3 = 0.0101;
          double dTrCov2 = oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("ShrubCoverPercent"));
          if (dTrCov2 == 0.0)
            dTrCov2 = 0.0;
          else if (dTrCov2 >= 100.0)
            dTrCov2 = 99.99999999;
          double num4 = oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("ShrubEvergreenPercent"));
          if (num4 == 0.0)
            num4 = 1E-08;
          else if (num4 >= 100.0)
            num4 = 99.99999999;
          oleDbDataReader2.Close();
          EngineModel.CloseConnection(oleDbConnection);
          if (this.engineCancellationToken.IsCancellationRequested)
            throw new Exception("User cancelled");
          this.engineReportArg.Percent = 0;
          this.engineReportArg.CurrentStep = 6;
          this.engineReportArg.Description = EngineModelRes.EngineProgressForm_CalculatingModelDTree;
          this.engineProgress.Report(this.engineReportArg);
          DateTime now1 = DateTime.Now;
          UFORE_D uforeD = new UFORE_D(this.engineCancellationToken, this.engineProgress, this.engineReportArg);
          uforeD.ModelYear = (int) this.currYearLocation.PollutionYear;
          uforeD.StartYear = (int) this.currYearLocation.PollutionYear;
          uforeD.EndYear = (int) this.currYearLocation.PollutionYear;
          uforeD.RuralUrban = !this.currProjectLocation.IsUrban ? ForestData.RURAL_URBAN.RURAL : ForestData.RURAL_URBAN.URBAN;
          uforeD.ModelDomain = str;
          uforeD.LocDB = LocSppDatabase;
          uforeD.NationIDSelected = this.currProject.NationCode;
          uforeD.PriPartIDSelected = this.currProject.PrimaryPartitionCode;
          uforeD.SecPartIDSelected = this.currProject.SecondaryPartitionCode;
          uforeD.TerPartIDSelected = this.currProject.TertiaryPartitionCode;
          uforeD.Area = dArea;
          uforeD.SurfaceWeatherFile = m_ps.m_cache.WeatherFileName;
          uforeD.UpperAirFile = m_ps.m_cache.RadioSondeFileName;
          uforeD.COMasterDB = m_ps.m_cache.PollutionCOdbName;
          uforeD.NO2MasterDB = m_ps.m_cache.PollutionNO2dbName;
          uforeD.O3MasterDB = m_ps.m_cache.PollutionO3dbName;
          uforeD.PM10MasterDB = m_ps.m_cache.PollutionPM10dbName;
          uforeD.PM25MasterDB = m_ps.m_cache.PollutionPM25dbName;
          uforeD.SO2MasterDB = m_ps.m_cache.PollutionSO2dbName;
          uforeD.BenMAPInputDB = benMAPDatabase;
          uforeD.VegType = DryDeposition.VEG_TYPE.TREE;
          uforeD.Lai = num1;
          uforeD.TreeCovPct = dTrCov1;
          uforeD.EvergreenPct = num2;
          uforeD.LaiDB = this.tmpTreeLaiDB;
          uforeD.WeatherDB = this.tmpTreeWeatherDB;
          uforeD.PollSiteDB = this.tmpTreePollSiteDB;
          uforeD.DryDepDB = this.tmpTreeDryDepDB;
          uforeD.BenMAPResultDB = this.tmpTreeBenMapOutputDB;
          uforeD.FinBenMAPDB = this.tmpFinalBenMapTreeDB;
          uforeD.FinDryDepDB = this.tmpUFOREDTreeDB;
          uforeD.RunUFORE_D();
          if (this.engineCancellationToken.IsCancellationRequested)
            throw new Exception("User cancelled");
          this.engineReportArg.Percent = 0;
          this.engineReportArg.CurrentStep = 7;
          this.engineReportArg.Description = EngineModelRes.EngineProgressForm_CalculatingModelBTree;
          this.engineProgress.Report(this.engineReportArg);
          DateTime now2 = DateTime.Now;
          bool flag = this.currSeries.SampleType == SampleType.Inventory;
          new UFORE_B(LocSppDatabase, this.currProject.NationCode, this.currProject.PrimaryPartitionCode, this.currProject.SecondaryPartitionCode, this.currProject.TertiaryPartitionCode, LocSppDatabase, uforeOutputDatabase, "lbiomass", "lu_cover", this.tmpTreeLaiDB, this.tmpTreeDryDepDB, this.tmpTreeBioEmissionOutputDB, DryDeposition.VEG_TYPE.TREE, flag, this.tmpUFOREBTreeDB, this.engineCancellationToken, this.engineProgress, this.engineReportArg).RunUFORE_B();
          if (this.engineCancellationToken.IsCancellationRequested)
            throw new Exception("User cancelled");
          this.engineReportArg.Percent = 0;
          this.engineReportArg.CurrentStep = 8;
          this.engineReportArg.Description = EngineModelRes.EngineProgressForm_CalculatingModelITree;
          this.engineProgress.Report(this.engineReportArg);
          DateTime now3 = DateTime.Now;
          new Interception(DryDeposition.VEG_TYPE.TREE, dArea, dTrCov1, this.tmpTreeWeatherDB, this.tmpTreeDryDepDB, this.tmpTreeInterceptionOutputDB, this.tmpWaterInterceptionTreeDB).SummarizeIntercept();
          DateTime now4 = DateTime.Now;
          if (this.currSeries.SampleType != SampleType.Inventory && this.currYear.RecordShrub && dTrCov2 > 1E-08)
          {
            if (this.engineCancellationToken.IsCancellationRequested)
              throw new Exception("User cancelled");
            this.engineReportArg.Percent = 0;
            this.engineReportArg.CurrentStep = 9;
            this.engineReportArg.Description = EngineModelRes.EngineProgressForm_CalculatingModelDShrub;
            this.engineProgress.Report(this.engineReportArg);
            uforeD.VegType = DryDeposition.VEG_TYPE.SHRUB;
            uforeD.Lai = num3;
            uforeD.TreeCovPct = dTrCov2;
            uforeD.EvergreenPct = num4;
            uforeD.LaiDB = this.tmpShrubLaiDB;
            uforeD.WeatherDB = this.tmpShrubWeatherDB;
            uforeD.PollSiteDB = this.tmpShrubPollSiteDB;
            uforeD.DryDepDB = this.tmpShrubDryDepDB;
            uforeD.BenMAPResultDB = this.tmpShrubBenMapOutputDB;
            uforeD.FinBenMAPDB = this.tmpFinalBenMapShrubDB;
            uforeD.FinDryDepDB = this.tmpUFOREDShrubDB;
            uforeD.RunUFORE_D();
            if (this.engineCancellationToken.IsCancellationRequested)
              throw new Exception("User cancelled");
            this.engineReportArg.Percent = 0;
            this.engineReportArg.CurrentStep = 10;
            this.engineReportArg.Description = EngineModelRes.EngineProgressForm_CalculatingModelBShrub;
            this.engineProgress.Report(this.engineReportArg);
            new UFORE_B(LocSppDatabase, this.currProject.NationCode, this.currProject.PrimaryPartitionCode, this.currProject.SecondaryPartitionCode, this.currProject.TertiaryPartitionCode, LocSppDatabase, uforeOutputDatabase, "lbiomass", "lu_cover", this.tmpShrubLaiDB, this.tmpShrubDryDepDB, this.tmpShrubBioEmissionOutputDB, DryDeposition.VEG_TYPE.SHRUB, flag, this.tmpUFOREBShrubDB, this.engineCancellationToken, this.engineProgress, this.engineReportArg).RunUFORE_B();
            if (this.engineCancellationToken.IsCancellationRequested)
              throw new Exception("User cancelled");
            this.engineReportArg.Percent = 0;
            this.engineReportArg.CurrentStep = 11;
            this.engineReportArg.Description = EngineModelRes.EngineProgressForm_CalculatingModelIShrub;
            this.engineProgress.Report(this.engineReportArg);
            new Interception(DryDeposition.VEG_TYPE.SHRUB, dArea, dTrCov2, this.tmpShrubWeatherDB, this.tmpShrubDryDepDB, this.tmpShrubInterceptionOutputDB, this.tmpWaterInterceptionShrubDB).SummarizeIntercept();
          }
          DateTime now5 = DateTime.Now;
          EngineModel.OpenConnection(oleDbConnection, inputDatabase);
          oleDbCommand.Connection = oleDbConnection;
          if (this.engineCancellationToken.IsCancellationRequested)
          {
            EngineModel.CloseConnection(oleDbConnection);
            throw new Exception("User cancelled");
          }
          this.engineReportArg.Percent = 0;
          this.engineReportArg.CurrentStep = 12;
          this.engineReportArg.Description = EngineModelRes.EngineProgressForm_LoadingPollutionResults;
          this.engineProgress.Report(this.engineReportArg);
          if (File.Exists(this.tmpFinalBenMapTreeDB))
          {
            oleDbCommand.CommandText = "INSERT INTO BenMapTable (YearGuid, TreeShrub, HealthFactor, NO2Incidence, NO2Value, SO2Incidence, SO2Value, O3Incidence, O3Value, PM25Incidence, PM25Value ) SELECT {" + this.currYear.Guid.ToString() + "}, BenMapTable.TreeShrub, BenMapTable.HealthFactor, BenMapTable.NO2Incidence, BenMapTable.NO2Value, BenMapTable.SO2Incidence, BenMapTable.SO2Value, BenMapTable.O3Incidence, BenMapTable.O3Value, BenMapTable.PM25Incidence, BenMapTable.PM25Value FROM BenMapTable IN '" + this.tmpFinalBenMapTreeDB + "'";
            oleDbCommand.ExecuteNonQuery();
          }
          if (this.engineCancellationToken.IsCancellationRequested)
          {
            EngineModel.CloseConnection(oleDbConnection);
            throw new Exception("User cancelled");
          }
          this.engineReportArg.Percent = 10;
          this.engineProgress.Report(this.engineReportArg);
          oleDbCommand.CommandText = "INSERT INTO Pollutants ( YearGuid, [Year], JDay, [Month], [Day], [Hour], SiteID, Pollutant, PPM, uGm3, CityArea, TrCovArea, PARWm2, RainMh, TempF, TempK, ActAqImp, Flux, FluxMax, FluxMin, FluxWet, PerAqImp, Trans, [Value], ValueMax, ValueMin, TreeShrub ) SELECT {" + this.currYear.Guid.ToString() + "}, Year([TimeStamp]), DatePart('y',[TimeStamp]), Month([TimeStamp]), Day([TimeStamp]), Hour([TimeStamp]),MonitorSiteID, Pollutant, PPM, uGm3, DomainArea, TrCovArea, PARWm2, RainMh, TempF, TempK, ActAqImp, Flux, FluxMax, FluxMin, FluxWet, PerAqImp, Trans, [Value], ValueMax, ValueMin, 'T' FROM DryDeposition IN '" + this.tmpTreeDryDepDB + "'";
          oleDbCommand.ExecuteNonQuery();
          if (this.engineCancellationToken.IsCancellationRequested)
          {
            EngineModel.CloseConnection(oleDbConnection);
            throw new Exception("User cancelled");
          }
          this.engineReportArg.Percent = 15;
          this.engineProgress.Report(this.engineReportArg);
          if (this.currProject.NationCode == "001" || this.currProject.NationCode == "002" || this.currProject.NationCode == "230" || this.currProject.NationCode == "021")
            EngineModel.AdjustPollutionValuesInTablePollutantsBasedOnBenMapResults(this.currYear.Guid, (int) this.currYearLocation.PollutionYear, oleDbConnection, this.tmpUFOREDTreeDB, "T");
          if (this.engineCancellationToken.IsCancellationRequested)
          {
            EngineModel.CloseConnection(oleDbConnection);
            throw new Exception("User cancelled");
          }
          this.engineReportArg.Percent = 20;
          this.engineProgress.Report(this.engineReportArg);
          this.PopulateMonthPollutantTable(m_ps, oleDbConnection, true);
          if (this.engineCancellationToken.IsCancellationRequested)
          {
            EngineModel.CloseConnection(oleDbConnection);
            throw new Exception("User cancelled");
          }
          this.engineReportArg.Percent = 30;
          this.engineProgress.Report(this.engineReportArg);
          this.PopulatingUFORE_BResults(m_ps, oleDbConnection, this.tmpUFOREBTreeDB, true);
          if (this.engineCancellationToken.IsCancellationRequested)
          {
            EngineModel.CloseConnection(oleDbConnection);
            throw new Exception("User cancelled");
          }
          this.engineReportArg.Percent = 40;
          this.engineProgress.Report(this.engineReportArg);
          this.PopulatingWaterInterception(m_ps, oleDbConnection, this.tmpWaterInterceptionTreeDB, true);
          if (this.currSeries.SampleType != SampleType.Inventory && this.currYear.RecordShrub && dTrCov2 > 1E-08)
          {
            if (this.engineCancellationToken.IsCancellationRequested)
            {
              EngineModel.CloseConnection(oleDbConnection);
              throw new Exception("User cancelled");
            }
            this.engineReportArg.Percent = 50;
            this.engineProgress.Report(this.engineReportArg);
            if (File.Exists(this.tmpFinalBenMapShrubDB))
            {
              oleDbCommand.CommandText = "INSERT INTO BenMapTable (YearGuid, TreeShrub, HealthFactor, NO2Incidence, NO2Value, SO2Incidence, SO2Value, O3Incidence, O3Value, PM25Incidence, PM25Value ) SELECT {" + this.currYear.Guid.ToString() + "}, BenMapTable.TreeShrub, BenMapTable.HealthFactor, BenMapTable.NO2Incidence, BenMapTable.NO2Value, BenMapTable.SO2Incidence, BenMapTable.SO2Value, BenMapTable.O3Incidence, BenMapTable.O3Value, BenMapTable.PM25Incidence, BenMapTable.PM25Value FROM BenMapTable IN '" + this.tmpFinalBenMapShrubDB + "'";
              oleDbCommand.ExecuteNonQuery();
            }
            if (this.engineCancellationToken.IsCancellationRequested)
            {
              EngineModel.CloseConnection(oleDbConnection);
              throw new Exception("User cancelled");
            }
            this.engineReportArg.Percent = 60;
            this.engineProgress.Report(this.engineReportArg);
            oleDbCommand.CommandText = "INSERT INTO Pollutants ( YearGuid, [Year], JDay, [Month], [Day], [Hour], SiteID, Pollutant, PPM, uGm3, CityArea, TrCovArea, PARWm2, RainMh, TempF, TempK, ActAqImp, Flux, FluxMax, FluxMin, FluxWet, PerAqImp, Trans, [Value], ValueMax, ValueMin, TreeShrub ) SELECT {" + this.currYear.Guid.ToString() + "}, Year([TimeStamp]), DatePart('y',[TimeStamp]), Month([TimeStamp]), Day([TimeStamp]), Hour([TimeStamp]),MonitorSiteID, Pollutant, PPM, uGm3, DomainArea, TrCovArea, PARWm2, RainMh, TempF, TempK, ActAqImp, Flux, FluxMax, FluxMin, FluxWet, PerAqImp, Trans, [Value], ValueMax, ValueMin, 'S' FROM DryDeposition IN '" + this.tmpShrubDryDepDB + "'";
            oleDbCommand.ExecuteNonQuery();
            if (this.engineCancellationToken.IsCancellationRequested)
            {
              EngineModel.CloseConnection(oleDbConnection);
              throw new Exception("User cancelled");
            }
            this.engineReportArg.Percent = 65;
            this.engineProgress.Report(this.engineReportArg);
            if (this.currProject.NationCode == "001" || this.currProject.NationCode == "002" || this.currProject.NationCode == "230" || this.currProject.NationCode == "021")
              EngineModel.AdjustPollutionValuesInTablePollutantsBasedOnBenMapResults(this.currYear.Guid, (int) this.currYearLocation.PollutionYear, oleDbConnection, this.tmpUFOREDShrubDB, "S");
            if (this.engineCancellationToken.IsCancellationRequested)
            {
              EngineModel.CloseConnection(oleDbConnection);
              throw new Exception("User cancelled");
            }
            this.engineReportArg.Percent = 70;
            this.engineProgress.Report(this.engineReportArg);
            this.PopulateMonthPollutantTable(m_ps, oleDbConnection, false);
            if (this.engineCancellationToken.IsCancellationRequested)
            {
              EngineModel.CloseConnection(oleDbConnection);
              throw new Exception("User cancelled");
            }
            this.engineReportArg.Percent = 75;
            this.engineProgress.Report(this.engineReportArg);
            this.PopulatingUFORE_BResults(m_ps, oleDbConnection, this.tmpUFOREBShrubDB, false);
            if (this.engineCancellationToken.IsCancellationRequested)
            {
              EngineModel.CloseConnection(oleDbConnection);
              throw new Exception("User cancelled");
            }
            this.engineReportArg.Percent = 80;
            this.engineProgress.Report(this.engineReportArg);
            this.PopulatingWaterInterception(m_ps, oleDbConnection, this.tmpWaterInterceptionShrubDB, false);
          }
          if (this.engineCancellationToken.IsCancellationRequested)
          {
            EngineModel.CloseConnection(oleDbConnection);
            throw new Exception("User cancelled");
          }
          this.engineReportArg.Percent = 85;
          this.engineProgress.Report(this.engineReportArg);
          this.PopulateMonthPollutantTableForCombinedTreeShrub(m_ps, oleDbConnection);
          if (this.engineCancellationToken.IsCancellationRequested)
          {
            EngineModel.CloseConnection(oleDbConnection);
            throw new Exception("User cancelled");
          }
          this.engineReportArg.Percent = 90;
          this.engineProgress.Report(this.engineReportArg);
          this.PopulatingUFORE_BResultsForCombinedTreeShrub(m_ps, oleDbConnection);
          if (this.engineCancellationToken.IsCancellationRequested)
          {
            EngineModel.CloseConnection(oleDbConnection);
            throw new Exception("User cancelled");
          }
          this.engineReportArg.Percent = 95;
          this.engineProgress.Report(this.engineReportArg);
          this.PopulatingWaterInterceptionForCombiedTreeShrub(m_ps, oleDbConnection);
          EngineModel.CloseConnection(oleDbConnection);
        }
        catch (Exception ex)
        {
          EngineModel.CloseConnection(oleDbConnection);
          throw;
        }
      }
    }

    public static void AdjustPollutionValuesInTablePollutantsBasedOnBenMapResults(
      Guid yearGUID,
      int pollutionDataYear,
      OleDbConnection inputDBCon,
      string DryDepDatabase,
      string T_or_S)
    {
      try
      {
        OleDbCommand oleDbCommand1 = new OleDbCommand();
        oleDbCommand1.Connection = inputDBCon;
        oleDbCommand1.CommandText = "SELECT NO2Value, SO2Value, O3Value, PM25Value FROM BenMapTable WHERE YearGuid={guid {" + yearGUID.ToString() + "}} AND TreeShrub='" + T_or_S + "'";
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        double num4 = 0.0;
        OleDbDataReader oleDbDataReader1 = oleDbCommand1.ExecuteReader();
        bool flag;
        if (!oleDbDataReader1.HasRows)
        {
          flag = false;
        }
        else
        {
          flag = true;
          while (oleDbDataReader1.Read())
          {
            if (Math.Abs(oleDbDataReader1.GetDouble(oleDbDataReader1.GetOrdinal("NO2Value")) + 1.0) > 1E-07)
              num1 += oleDbDataReader1.GetDouble(oleDbDataReader1.GetOrdinal("NO2Value"));
            if (Math.Abs(oleDbDataReader1.GetDouble(oleDbDataReader1.GetOrdinal("SO2Value")) + 1.0) > 1E-07)
              num2 += oleDbDataReader1.GetDouble(oleDbDataReader1.GetOrdinal("SO2Value"));
            if (Math.Abs(oleDbDataReader1.GetDouble(oleDbDataReader1.GetOrdinal("O3Value")) + 1.0) > 1E-07)
              num3 += oleDbDataReader1.GetDouble(oleDbDataReader1.GetOrdinal("O3Value"));
            if (Math.Abs(oleDbDataReader1.GetDouble(oleDbDataReader1.GetOrdinal("PM25Value")) + 1.0) > 1E-07)
              num4 += oleDbDataReader1.GetDouble(oleDbDataReader1.GetOrdinal("PM25Value"));
          }
        }
        oleDbDataReader1.Close();
        double num5 = 0.0;
        double num6 = 0.0;
        double num7 = 0.0;
        double num8 = 0.0;
        double num9 = 0.0;
        double num10 = 0.0;
        double num11 = 0.0;
        double num12 = 0.0;
        using (OleDbConnection oleDbConnection = new OleDbConnection())
        {
          EngineModel.OpenConnection(oleDbConnection, DryDepDatabase);
          try
          {
            OleDbCommand oleDbCommand2 = new OleDbCommand();
            oleDbCommand2.Connection = oleDbConnection;
            oleDbCommand2.CommandText = "select Pollutant, [FluxDomain (m-tons)], [ValDomain ($1000)] from 08_DomainYearlySums";
            OleDbDataReader oleDbDataReader2 = oleDbCommand2.ExecuteReader();
            while (oleDbDataReader2.Read())
            {
              string str = oleDbDataReader2.GetString(oleDbDataReader2.GetOrdinal("Pollutant"));
              if (!(str == "NO2"))
              {
                if (!(str == "SO2"))
                {
                  if (!(str == "O3"))
                  {
                    if (str == "PM2.5")
                    {
                      num8 = 1000000.0 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("FluxDomain (m-tons)"));
                      num12 = 1000.0 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("ValDomain ($1000)"));
                    }
                  }
                  else
                  {
                    num7 = 1000000.0 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("FluxDomain (m-tons)"));
                    num11 = 1000.0 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("ValDomain ($1000)"));
                  }
                }
                else
                {
                  num6 = 1000000.0 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("FluxDomain (m-tons)"));
                  num10 = 1000.0 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("ValDomain ($1000)"));
                }
              }
              else
              {
                num5 = 1000000.0 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("FluxDomain (m-tons)"));
                num9 = 1000.0 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("ValDomain ($1000)"));
              }
            }
            oleDbDataReader2.Close();
            EngineModel.CloseConnection(oleDbConnection);
          }
          catch (Exception ex)
          {
            EngineModel.CloseConnection(oleDbConnection);
            throw;
          }
        }
        string str1 = num5 != 0.0 ? (!flag ? (num9 / num5).ToString((IFormatProvider) CultureInfo.InvariantCulture) : (num1 / num5).ToString((IFormatProvider) CultureInfo.InvariantCulture)) : "0";
        oleDbCommand1.CommandText = "Update Pollutants set [Value]=[Flux]*" + str1 + ", [ValueMax]=[FluxMax]*" + str1 + ",[ValueMin]=[FluxMin]*" + str1 + " Where [YearGUID]={" + yearGUID.ToString() + "} and [Year]=" + pollutionDataYear.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and Pollutant='NO2' and TreeShrub='" + T_or_S + "'";
        oleDbCommand1.ExecuteNonQuery();
        string str2 = num6 != 0.0 ? (!flag ? (num10 / num6).ToString((IFormatProvider) CultureInfo.InvariantCulture) : (num2 / num6).ToString((IFormatProvider) CultureInfo.InvariantCulture)) : "0";
        oleDbCommand1.CommandText = "Update Pollutants set [Value]=[Flux]*" + str2 + ", [ValueMax]=[FluxMax]*" + str2 + ",[ValueMin]=[FluxMin]*" + str2 + " Where [YearGUID]={" + yearGUID.ToString() + "} and [Year]=" + pollutionDataYear.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and Pollutant='SO2' and TreeShrub='" + T_or_S + "'";
        oleDbCommand1.ExecuteNonQuery();
        string str3 = num7 != 0.0 ? (!flag ? (num11 / num7).ToString((IFormatProvider) CultureInfo.InvariantCulture) : (num3 / num7).ToString((IFormatProvider) CultureInfo.InvariantCulture)) : "0";
        oleDbCommand1.CommandText = "Update Pollutants set [Value]=[Flux]*" + str3 + ", [ValueMax]=[FluxMax]*" + str3 + ",[ValueMin]=[FluxMin]*" + str3 + " Where [YearGUID]={" + yearGUID.ToString() + "} and [Year]=" + pollutionDataYear.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and Pollutant='O3' and TreeShrub='" + T_or_S + "'";
        oleDbCommand1.ExecuteNonQuery();
        string str4 = num8 != 0.0 ? (!flag ? (num12 / num8).ToString((IFormatProvider) CultureInfo.InvariantCulture) : (num4 / num8).ToString((IFormatProvider) CultureInfo.InvariantCulture)) : "0";
        oleDbCommand1.CommandText = "Update Pollutants set [Value]=[Flux]*" + str4 + ", [ValueMax]=[FluxMax]*" + str4 + ",[ValueMin]=[FluxMin]*" + str4 + " Where [YearGUID]={" + yearGUID.ToString() + "} and [Year]=" + pollutionDataYear.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and Pollutant='PM2.5' and TreeShrub='" + T_or_S + "'";
        oleDbCommand1.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void PopulateMonthPollutantTable(
      ProgramSession m_ps,
      OleDbConnection inputDBCon,
      bool isForTree)
    {
      try
      {
        OleDbCommand oleDbCommand1 = new OleDbCommand();
        oleDbCommand1.Connection = inputDBCon;
        Dictionary<string, int> dictionary1 = new Dictionary<string, int>();
        oleDbCommand1.CommandText = "select * from ClassValueTable where YearGuid={" + this.currYear.Guid.ToString() + "} and ClassifierId=" + 19.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        OleDbDataReader oleDbDataReader1 = oleDbCommand1.ExecuteReader();
        if (oleDbDataReader1.HasRows)
        {
          oleDbDataReader1.Close();
        }
        else
        {
          oleDbDataReader1.Close();
          oleDbCommand1.CommandText = "INSERT INTO ClassValueTable (YearGuid, ClassifierId, ClassValueOrder,ClassValueName) VALUES ({" + this.currYear.Guid.ToString() + "}," + 19.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ",@x,@y)";
          oleDbCommand1.Parameters.Clear();
          oleDbCommand1.Parameters.Add("@x", OleDbType.Integer);
          oleDbCommand1.Parameters.Add("@y", OleDbType.VarWChar);
          for (int index = 1; index <= 12; ++index)
          {
            oleDbCommand1.Parameters["@x"].Value = (object) index;
            oleDbCommand1.Parameters["@y"].Value = (object) index.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            oleDbCommand1.ExecuteNonQuery();
          }
        }
        OleDbCommand oleDbCommand2 = oleDbCommand1;
        Guid guid = this.currYear.Guid;
        string str1 = guid.ToString();
        int key1 = 15;
        string str2 = key1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        string str3 = "select * from ClassValueTable where YearGuid={" + str1 + "} and ClassifierId=" + str2;
        oleDbCommand2.CommandText = str3;
        OleDbDataReader oleDbDataReader2 = oleDbCommand1.ExecuteReader();
        if (oleDbDataReader2.HasRows)
        {
          while (oleDbDataReader2.Read())
            dictionary1.Add(oleDbDataReader2.GetString(oleDbDataReader2.GetOrdinal("ClassValueName")), (int) oleDbDataReader2.GetInt16(oleDbDataReader2.GetOrdinal("ClassValueOrder")));
          oleDbDataReader2.Close();
        }
        else
        {
          oleDbDataReader2.Close();
          OleDbCommand oleDbCommand3 = oleDbCommand1;
          string[] strArray = new string[5]
          {
            "INSERT INTO ClassValueTable (YearGuid, ClassifierId, ClassValueOrder,ClassValueName) VALUES ({",
            null,
            null,
            null,
            null
          };
          guid = this.currYear.Guid;
          strArray[1] = guid.ToString();
          strArray[2] = "},";
          key1 = 15;
          strArray[3] = key1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray[4] = ",@n,@t)";
          string str4 = string.Concat(strArray);
          oleDbCommand3.CommandText = str4;
          oleDbCommand1.Parameters.Clear();
          oleDbCommand1.Parameters.Add("@n", OleDbType.Integer);
          oleDbCommand1.Parameters.Add("@t", OleDbType.VarWChar);
          dictionary1.Add("CO", 1);
          oleDbCommand1.Parameters["@n"].Value = (object) 1;
          oleDbCommand1.Parameters["@t"].Value = (object) "CO";
          oleDbCommand1.ExecuteNonQuery();
          dictionary1.Add("NO2", 2);
          oleDbCommand1.Parameters["@n"].Value = (object) 2;
          oleDbCommand1.Parameters["@t"].Value = (object) "NO2";
          oleDbCommand1.ExecuteNonQuery();
          dictionary1.Add("O3", 3);
          oleDbCommand1.Parameters["@n"].Value = (object) 3;
          oleDbCommand1.Parameters["@t"].Value = (object) "O3";
          oleDbCommand1.ExecuteNonQuery();
          dictionary1.Add("PM10*", 4);
          oleDbCommand1.Parameters["@n"].Value = (object) 4;
          oleDbCommand1.Parameters["@t"].Value = (object) "PM10*";
          oleDbCommand1.ExecuteNonQuery();
          dictionary1.Add("PM2.5", 5);
          oleDbCommand1.Parameters["@n"].Value = (object) 5;
          oleDbCommand1.Parameters["@t"].Value = (object) "PM2.5";
          oleDbCommand1.ExecuteNonQuery();
          dictionary1.Add("SO2", 6);
          oleDbCommand1.Parameters["@n"].Value = (object) 6;
          oleDbCommand1.Parameters["@t"].Value = (object) "SO2";
          oleDbCommand1.ExecuteNonQuery();
        }
        List<int> Classifiers = new List<int>();
        Classifiers.Add(19);
        Classifiers.Add(15);
        string str5 = !isForTree ? EngineModel.getEstimationTable(inputDBCon, 3, Classifiers) : EngineModel.getEstimationTable(inputDBCon, 2, Classifiers);
        int unit1 = EngineModel.getUnit(inputDBCon, 22, 1, 1);
        int unit2 = EngineModel.getUnit(inputDBCon, 29, 1, 1);
        if (isForTree)
        {
          OleDbCommand oleDbCommand4 = oleDbCommand1;
          guid = this.currYear.Guid;
          string str6 = "SELECT Q.Pollutant, Q.Month, Sum([Q.AvgOfFlux]*[Q.AvgOfTrCovArea])/1000000 AS myAmount, Sum([Q.AvgOfValue]*[Q.AvgOfTrCovArea]) AS myValue FROM [SELECT Pollutant,Month,Day,Hour,TreeShrub, Avg(Flux) AS AvgOfFlux,Avg(Value) AS AvgOfValue, Avg(TrCovArea) AS AvgOfTrCovArea FROM Pollutants WHERE YearGUID={" + guid.ToString() + "} and TreeShrub='T' GROUP BY Pollutant,Month, Day, Hour, TreeShrub]. AS Q GROUP BY Pollutant, Month";
          oleDbCommand4.CommandText = str6;
        }
        else
        {
          OleDbCommand oleDbCommand5 = oleDbCommand1;
          guid = this.currYear.Guid;
          string str7 = "SELECT Q.Pollutant, Q.Month, Sum([Q.AvgOfFlux]*[Q.AvgOfTrCovArea])/1000000 AS myAmount, Sum([Q.AvgOfValue]*[Q.AvgOfTrCovArea]) AS myValue FROM [SELECT Pollutant,Month,Day,Hour,TreeShrub, Avg(Flux) AS AvgOfFlux,Avg(Value) AS AvgOfValue, Avg(TrCovArea) AS AvgOfTrCovArea FROM Pollutants WHERE YearGUID={" + guid.ToString() + "} and TreeShrub='S' GROUP BY Pollutant,Month, Day, Hour, TreeShrub]. AS Q GROUP BY Pollutant, Month";
          oleDbCommand5.CommandText = str7;
        }
        OleDbDataReader oleDbDataReader3 = oleDbCommand1.ExecuteReader();
        int ordinal1 = oleDbDataReader3.GetOrdinal("myAmount");
        int ordinal2 = oleDbDataReader3.GetOrdinal("myValue");
        int ordinal3 = oleDbDataReader3.GetOrdinal("Month");
        int ordinal4 = oleDbDataReader3.GetOrdinal("Pollutant");
        OleDbCommand oleDbCommand6 = new OleDbCommand();
        oleDbCommand6.Connection = inputDBCon;
        OleDbCommand oleDbCommand7 = oleDbCommand6;
        string[] strArray1 = new string[13]
        {
          "INSERT INTO [",
          str5,
          "] (YearGuid,EstimateType,EquationType,EstimateStandardError,EstimateUnitsId,[",
          Classifiers.Month.ToString(),
          "],[",
          Classifiers.Pollutant.ToString(),
          "],EstimateValue) VALUES({",
          null,
          null,
          null,
          null,
          null,
          null
        };
        guid = this.currYear.Guid;
        strArray1[7] = guid.ToString();
        strArray1[8] = "},";
        key1 = 31;
        strArray1[9] = key1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray1[10] = ",";
        key1 = 1;
        strArray1[11] = key1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray1[12] = ",0,@u,@m,@p,@v)";
        string str8 = string.Concat(strArray1);
        oleDbCommand7.CommandText = str8;
        oleDbCommand6.Parameters.Clear();
        oleDbCommand6.Parameters.Add("@u", OleDbType.Integer);
        oleDbCommand6.Parameters.Add("@m", OleDbType.Integer);
        oleDbCommand6.Parameters.Add("@p", OleDbType.Integer);
        oleDbCommand6.Parameters.Add("@v", OleDbType.Double);
        Dictionary<int, double> dictionary2 = new Dictionary<int, double>();
        Dictionary<int, double> dictionary3 = new Dictionary<int, double>();
        double num1 = 0.0;
        double num2 = 0.0;
        while (oleDbDataReader3.Read())
        {
          oleDbCommand6.Parameters["@u"].Value = (object) unit1;
          oleDbCommand6.Parameters["@m"].Value = (object) oleDbDataReader3.GetInt32(ordinal3);
          oleDbCommand6.Parameters["@p"].Value = (object) dictionary1[oleDbDataReader3.GetString(ordinal4)];
          oleDbCommand6.Parameters["@v"].Value = (object) oleDbDataReader3.GetDouble(ordinal1);
          oleDbCommand6.ExecuteNonQuery();
          if (dictionary2.ContainsKey(dictionary1[oleDbDataReader3.GetString(ordinal4)]))
          {
            Dictionary<int, double> dictionary4 = dictionary2;
            key1 = dictionary1[oleDbDataReader3.GetString(ordinal4)];
            dictionary4[key1] += oleDbDataReader3.GetDouble(ordinal1);
          }
          else
            dictionary2.Add(dictionary1[oleDbDataReader3.GetString(ordinal4)], oleDbDataReader3.GetDouble(ordinal1));
          num1 += oleDbDataReader3.GetDouble(ordinal1);
          oleDbCommand6.Parameters["@u"].Value = (object) unit2;
          oleDbCommand6.Parameters["@m"].Value = (object) oleDbDataReader3.GetInt32(ordinal3);
          oleDbCommand6.Parameters["@p"].Value = (object) dictionary1[oleDbDataReader3.GetString(ordinal4)];
          oleDbCommand6.Parameters["@v"].Value = (object) oleDbDataReader3.GetDouble(ordinal2);
          oleDbCommand6.ExecuteNonQuery();
          if (dictionary3.ContainsKey(dictionary1[oleDbDataReader3.GetString(ordinal4)]))
          {
            Dictionary<int, double> dictionary5 = dictionary3;
            key1 = dictionary1[oleDbDataReader3.GetString(ordinal4)];
            dictionary5[key1] += oleDbDataReader3.GetDouble(ordinal2);
          }
          else
            dictionary3.Add(dictionary1[oleDbDataReader3.GetString(ordinal4)], oleDbDataReader3.GetDouble(ordinal2));
          num2 += oleDbDataReader3.GetDouble(ordinal2);
        }
        oleDbDataReader3.Close();
        Dictionary<int, double> dictionary6 = new Dictionary<int, double>();
        Dictionary<int, double> dictionary7 = new Dictionary<int, double>();
        double num3 = 0.0;
        double num4 = 0.0;
        double num5 = 0.0;
        foreach (Stratum stratum in this.allStrata.Values)
        {
          if (isForTree)
          {
            dictionary7.Add(stratum.ClassValue, stratum.AreaHectareBasedEngineInventoryDB * stratum.TreeCoverPercentageBasedEngineInventoryDB / 100.0);
            num3 += stratum.AreaHectareBasedEngineInventoryDB * stratum.TreeCoverPercentageBasedEngineInventoryDB / 100.0;
            num4 += stratum.TreeLeafAreaHectare;
          }
          else
          {
            dictionary7.Add(stratum.ClassValue, stratum.AreaHectareBasedEngineInventoryDB * stratum.ShrubCoverPercentageBasedEngineInventoryDB / 100.0);
            num3 += stratum.AreaHectareBasedEngineInventoryDB * stratum.ShrubCoverPercentageBasedEngineInventoryDB / 100.0;
            num4 += stratum.ShrubLeafAreaHectare;
          }
        }
        foreach (StratumGenus stratumGenus in this.allStratumGenus.Values)
        {
          if (isForTree)
          {
            if (stratumGenus.Form_Ind == "TREE")
            {
              num5 += stratumGenus.LeafBiomassKilograms;
              if (dictionary6.ContainsKey(stratumGenus.StratumClassValue))
                dictionary6[stratumGenus.StratumClassValue] += stratumGenus.LeafBiomassKilograms;
              else
                dictionary6.Add(stratumGenus.StratumClassValue, stratumGenus.LeafBiomassKilograms);
            }
          }
          else if (stratumGenus.Form_Ind == "SHRUB")
          {
            num5 += stratumGenus.LeafBiomassKilograms;
            if (dictionary6.ContainsKey(stratumGenus.StratumClassValue))
              dictionary6[stratumGenus.StratumClassValue] += stratumGenus.LeafBiomassKilograms;
            else
              dictionary6.Add(stratumGenus.StratumClassValue, stratumGenus.LeafBiomassKilograms);
          }
        }
        Classifiers.Clear();
        Classifiers.Add(3);
        Classifiers.Add(15);
        string str9 = !isForTree ? EngineModel.getEstimationTable(inputDBCon, 3, Classifiers) : EngineModel.getEstimationTable(inputDBCon, 2, Classifiers);
        oleDbCommand6.CommandText = "INSERT INTO [" + str9 + "] (YearGuid,EstimateType,EquationType,EstimateStandardError,EstimateUnitsId,[" + Classifiers.Strata.ToString() + "],[" + Classifiers.Pollutant.ToString() + "],EstimateValue) VALUES({" + this.currYear.Guid.ToString() + "}," + 31.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + 1.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ",0,@u,@m,@p,@v)";
        oleDbCommand6.Parameters.Clear();
        oleDbCommand6.Parameters.Add("@u", OleDbType.Integer);
        oleDbCommand6.Parameters.Add("@m", OleDbType.Integer);
        oleDbCommand6.Parameters.Add("@p", OleDbType.Integer);
        oleDbCommand6.Parameters.Add("@v", OleDbType.Double);
        foreach (int key2 in dictionary6.Keys)
        {
          Stratum allStratum = this.allStrata[key2];
          if (dictionary1.ContainsKey("CO"))
          {
            int key3 = dictionary1["CO"];
            oleDbCommand6.Parameters["@u"].Value = (object) unit1;
            oleDbCommand6.Parameters["@m"].Value = (object) allStratum.ClassValue;
            oleDbCommand6.Parameters["@p"].Value = (object) key3;
            oleDbCommand6.Parameters["@v"].Value = (object) (dictionary2[key3] * dictionary6[allStratum.ClassValue] / num5);
            oleDbCommand6.ExecuteNonQuery();
            oleDbCommand6.Parameters["@u"].Value = (object) unit2;
            oleDbCommand6.Parameters["@m"].Value = (object) allStratum.ClassValue;
            oleDbCommand6.Parameters["@p"].Value = (object) key3;
            oleDbCommand6.Parameters["@v"].Value = (object) (dictionary3[key3] * dictionary6[allStratum.ClassValue] / num5);
            oleDbCommand6.ExecuteNonQuery();
          }
          if (dictionary1.ContainsKey("NO2"))
          {
            int key4 = dictionary1["NO2"];
            oleDbCommand6.Parameters["@u"].Value = (object) unit1;
            oleDbCommand6.Parameters["@m"].Value = (object) allStratum.ClassValue;
            oleDbCommand6.Parameters["@p"].Value = (object) key4;
            oleDbCommand6.Parameters["@v"].Value = (object) (dictionary2[key4] * dictionary6[allStratum.ClassValue] / num5);
            oleDbCommand6.ExecuteNonQuery();
            oleDbCommand6.Parameters["@u"].Value = (object) unit2;
            oleDbCommand6.Parameters["@m"].Value = (object) allStratum.ClassValue;
            oleDbCommand6.Parameters["@p"].Value = (object) key4;
            oleDbCommand6.Parameters["@v"].Value = (object) (dictionary3[key4] * dictionary6[allStratum.ClassValue] / num5);
            oleDbCommand6.ExecuteNonQuery();
          }
          if (dictionary1.ContainsKey("O3"))
          {
            int key5 = dictionary1["O3"];
            oleDbCommand6.Parameters["@u"].Value = (object) unit1;
            oleDbCommand6.Parameters["@m"].Value = (object) allStratum.ClassValue;
            oleDbCommand6.Parameters["@p"].Value = (object) key5;
            oleDbCommand6.Parameters["@v"].Value = (object) (dictionary2[key5] * dictionary6[allStratum.ClassValue] / num5);
            oleDbCommand6.ExecuteNonQuery();
            oleDbCommand6.Parameters["@u"].Value = (object) unit2;
            oleDbCommand6.Parameters["@m"].Value = (object) allStratum.ClassValue;
            oleDbCommand6.Parameters["@p"].Value = (object) key5;
            oleDbCommand6.Parameters["@v"].Value = (object) (dictionary3[key5] * dictionary6[allStratum.ClassValue] / num5);
            oleDbCommand6.ExecuteNonQuery();
          }
          if (dictionary1.ContainsKey("PM10*"))
          {
            int key6 = dictionary1["PM10*"];
            oleDbCommand6.Parameters["@u"].Value = (object) unit1;
            oleDbCommand6.Parameters["@m"].Value = (object) allStratum.ClassValue;
            oleDbCommand6.Parameters["@p"].Value = (object) key6;
            oleDbCommand6.Parameters["@v"].Value = (object) (dictionary2[key6] * dictionary7[allStratum.ClassValue] / num3);
            oleDbCommand6.ExecuteNonQuery();
            oleDbCommand6.Parameters["@u"].Value = (object) unit2;
            oleDbCommand6.Parameters["@m"].Value = (object) allStratum.ClassValue;
            oleDbCommand6.Parameters["@p"].Value = (object) key6;
            oleDbCommand6.Parameters["@v"].Value = (object) (dictionary3[key6] * dictionary7[allStratum.ClassValue] / num3);
            oleDbCommand6.ExecuteNonQuery();
          }
          if (dictionary1.ContainsKey("PM2.5"))
          {
            int key7 = dictionary1["PM2.5"];
            oleDbCommand6.Parameters["@u"].Value = (object) unit1;
            oleDbCommand6.Parameters["@m"].Value = (object) allStratum.ClassValue;
            oleDbCommand6.Parameters["@p"].Value = (object) key7;
            oleDbCommand6.Parameters["@v"].Value = (object) (dictionary2[key7] * dictionary7[allStratum.ClassValue] / num3);
            oleDbCommand6.ExecuteNonQuery();
            oleDbCommand6.Parameters["@u"].Value = (object) unit2;
            oleDbCommand6.Parameters["@m"].Value = (object) allStratum.ClassValue;
            oleDbCommand6.Parameters["@p"].Value = (object) key7;
            oleDbCommand6.Parameters["@v"].Value = (object) (dictionary3[key7] * dictionary7[allStratum.ClassValue] / num3);
            oleDbCommand6.ExecuteNonQuery();
          }
          if (dictionary1.ContainsKey("SO2"))
          {
            int key8 = dictionary1["SO2"];
            oleDbCommand6.Parameters["@u"].Value = (object) unit1;
            oleDbCommand6.Parameters["@m"].Value = (object) allStratum.ClassValue;
            oleDbCommand6.Parameters["@p"].Value = (object) key8;
            oleDbCommand6.Parameters["@v"].Value = (object) (dictionary2[key8] * dictionary7[allStratum.ClassValue] / num3);
            oleDbCommand6.ExecuteNonQuery();
            oleDbCommand6.Parameters["@u"].Value = (object) unit2;
            oleDbCommand6.Parameters["@m"].Value = (object) allStratum.ClassValue;
            oleDbCommand6.Parameters["@p"].Value = (object) key8;
            oleDbCommand6.Parameters["@v"].Value = (object) (dictionary3[key8] * dictionary7[allStratum.ClassValue] / num3);
            oleDbCommand6.ExecuteNonQuery();
          }
        }
        if (dictionary1.ContainsKey("CO"))
        {
          int key9 = dictionary1["CO"];
          oleDbCommand6.Parameters["@u"].Value = (object) unit1;
          oleDbCommand6.Parameters["@m"].Value = (object) this.ClassValueForStrataStudyArea;
          oleDbCommand6.Parameters["@p"].Value = (object) key9;
          oleDbCommand6.Parameters["@v"].Value = (object) dictionary2[key9];
          oleDbCommand6.ExecuteNonQuery();
          oleDbCommand6.Parameters["@u"].Value = (object) unit2;
          oleDbCommand6.Parameters["@m"].Value = (object) this.ClassValueForStrataStudyArea;
          oleDbCommand6.Parameters["@p"].Value = (object) key9;
          oleDbCommand6.Parameters["@v"].Value = (object) dictionary3[key9];
          oleDbCommand6.ExecuteNonQuery();
        }
        if (dictionary1.ContainsKey("NO2"))
        {
          int key10 = dictionary1["NO2"];
          oleDbCommand6.Parameters["@u"].Value = (object) unit1;
          oleDbCommand6.Parameters["@m"].Value = (object) this.ClassValueForStrataStudyArea;
          oleDbCommand6.Parameters["@p"].Value = (object) key10;
          oleDbCommand6.Parameters["@v"].Value = (object) dictionary2[key10];
          oleDbCommand6.ExecuteNonQuery();
          oleDbCommand6.Parameters["@u"].Value = (object) unit2;
          oleDbCommand6.Parameters["@m"].Value = (object) this.ClassValueForStrataStudyArea;
          oleDbCommand6.Parameters["@p"].Value = (object) key10;
          oleDbCommand6.Parameters["@v"].Value = (object) dictionary3[key10];
          oleDbCommand6.ExecuteNonQuery();
        }
        if (dictionary1.ContainsKey("O3"))
        {
          int key11 = dictionary1["O3"];
          oleDbCommand6.Parameters["@u"].Value = (object) unit1;
          oleDbCommand6.Parameters["@m"].Value = (object) this.ClassValueForStrataStudyArea;
          oleDbCommand6.Parameters["@p"].Value = (object) key11;
          oleDbCommand6.Parameters["@v"].Value = (object) dictionary2[key11];
          oleDbCommand6.ExecuteNonQuery();
          oleDbCommand6.Parameters["@u"].Value = (object) unit2;
          oleDbCommand6.Parameters["@m"].Value = (object) this.ClassValueForStrataStudyArea;
          oleDbCommand6.Parameters["@p"].Value = (object) key11;
          oleDbCommand6.Parameters["@v"].Value = (object) dictionary3[key11];
          oleDbCommand6.ExecuteNonQuery();
        }
        if (dictionary1.ContainsKey("PM10*"))
        {
          int key12 = dictionary1["PM10*"];
          oleDbCommand6.Parameters["@u"].Value = (object) unit1;
          oleDbCommand6.Parameters["@m"].Value = (object) this.ClassValueForStrataStudyArea;
          oleDbCommand6.Parameters["@p"].Value = (object) key12;
          oleDbCommand6.Parameters["@v"].Value = (object) dictionary2[key12];
          oleDbCommand6.ExecuteNonQuery();
          oleDbCommand6.Parameters["@u"].Value = (object) unit2;
          oleDbCommand6.Parameters["@m"].Value = (object) this.ClassValueForStrataStudyArea;
          oleDbCommand6.Parameters["@p"].Value = (object) key12;
          oleDbCommand6.Parameters["@v"].Value = (object) dictionary3[key12];
          oleDbCommand6.ExecuteNonQuery();
        }
        if (dictionary1.ContainsKey("PM2.5"))
        {
          int key13 = dictionary1["PM2.5"];
          oleDbCommand6.Parameters["@u"].Value = (object) unit1;
          oleDbCommand6.Parameters["@m"].Value = (object) this.ClassValueForStrataStudyArea;
          oleDbCommand6.Parameters["@p"].Value = (object) key13;
          oleDbCommand6.Parameters["@v"].Value = (object) dictionary2[key13];
          oleDbCommand6.ExecuteNonQuery();
          oleDbCommand6.Parameters["@u"].Value = (object) unit2;
          oleDbCommand6.Parameters["@m"].Value = (object) this.ClassValueForStrataStudyArea;
          oleDbCommand6.Parameters["@p"].Value = (object) key13;
          oleDbCommand6.Parameters["@v"].Value = (object) dictionary3[key13];
          oleDbCommand6.ExecuteNonQuery();
        }
        if (dictionary1.ContainsKey("SO2"))
        {
          int key14 = dictionary1["SO2"];
          oleDbCommand6.Parameters["@u"].Value = (object) unit1;
          oleDbCommand6.Parameters["@m"].Value = (object) this.ClassValueForStrataStudyArea;
          oleDbCommand6.Parameters["@p"].Value = (object) key14;
          oleDbCommand6.Parameters["@v"].Value = (object) dictionary2[key14];
          oleDbCommand6.ExecuteNonQuery();
          oleDbCommand6.Parameters["@u"].Value = (object) unit2;
          oleDbCommand6.Parameters["@m"].Value = (object) this.ClassValueForStrataStudyArea;
          oleDbCommand6.Parameters["@p"].Value = (object) key14;
          oleDbCommand6.Parameters["@v"].Value = (object) dictionary3[key14];
          oleDbCommand6.ExecuteNonQuery();
        }
        if (!isForTree)
          return;
        oleDbCommand6.Connection = inputDBCon;
        oleDbCommand6.CommandText = "UPDATE IndividualTreePollutionEffects SET  CORemoved = @com,  CORemovalValue = @cov,  NO2Removed = @no2m,  NO2RemovalValue = @no2v,  O3Removed = @o3m,  O3RemovalValue = @o3v,  PM10Removed = @pm10m,  PM10RemovalValue = @pm10v,  PM25Removed = @pm25m,  PM25RemovalValue = @pm25v,  SO2Removed = @so2m,  SO2RemovalValue = @so2v  WHERE YearGuid={" + this.currYear.Guid.ToString() + "} AND PlotId=@plotid AND TreeId=@treeid";
        oleDbCommand6.Parameters.Clear();
        oleDbCommand6.Parameters.Add("@com", OleDbType.Double);
        oleDbCommand6.Parameters.Add("@cov", OleDbType.Double);
        oleDbCommand6.Parameters.Add("@no2m", OleDbType.Double);
        oleDbCommand6.Parameters.Add("@no2v", OleDbType.Double);
        oleDbCommand6.Parameters.Add("@o3m", OleDbType.Double);
        oleDbCommand6.Parameters.Add("@o3v", OleDbType.Double);
        oleDbCommand6.Parameters.Add("@pm10m", OleDbType.Double);
        oleDbCommand6.Parameters.Add("@pm10v", OleDbType.Double);
        oleDbCommand6.Parameters.Add("@pm25m", OleDbType.Double);
        oleDbCommand6.Parameters.Add("@pm25v", OleDbType.Double);
        oleDbCommand6.Parameters.Add("@so2m", OleDbType.Double);
        oleDbCommand6.Parameters.Add("@so2v", OleDbType.Double);
        oleDbCommand6.Parameters.Add("@plotid", OleDbType.Integer);
        oleDbCommand6.Parameters.Add("@treeid", OleDbType.Integer);
        oleDbCommand1.CommandText = "SELECT EcoPlots.PlotId, EcoTrees.TreeId, EcoTrees.Species, IndividualTreeEffects.GroundArea, IndividualTreeEffects.LeafArea, IndividualTreeEffects.LeafBioMass FROM EcoPlots INNER JOIN (EcoTrees INNER JOIN IndividualTreeEffects ON EcoTrees.TreeId = IndividualTreeEffects.TreeId) ON (EcoPlots.PlotKey = EcoTrees.PlotKey) AND (EcoPlots.YearKey = IndividualTreeEffects.YearGuid) AND (EcoPlots.PlotId = IndividualTreeEffects.PlotId) WHERE (((EcoPlots.YearKey)={guid {" + this.currYear.Guid.ToString() + "}}))";
        OleDbDataReader oleDbDataReader4 = oleDbCommand1.ExecuteReader();
        oleDbDataReader4.GetOrdinal("Species");
        int ordinal5 = oleDbDataReader4.GetOrdinal("GroundArea");
        oleDbDataReader4.GetOrdinal("LeafArea");
        int ordinal6 = oleDbDataReader4.GetOrdinal("LeafBioMass");
        int ordinal7 = oleDbDataReader4.GetOrdinal("PlotID");
        int ordinal8 = oleDbDataReader4.GetOrdinal("TreeID");
        while (oleDbDataReader4.Read())
        {
          oleDbCommand6.Parameters["@plotid"].Value = (object) oleDbDataReader4.GetInt32(ordinal7);
          oleDbCommand6.Parameters["@treeid"].Value = (object) oleDbDataReader4.GetInt32(ordinal8);
          if (dictionary1.ContainsKey("CO"))
          {
            oleDbCommand6.Parameters["@com"].Value = (object) (1000000.0 * dictionary2[dictionary1["CO"]] * (oleDbDataReader4.GetDouble(ordinal6) / num5));
            oleDbCommand6.Parameters["@cov"].Value = (object) (dictionary3[dictionary1["CO"]] * oleDbDataReader4.GetDouble(ordinal6) / num5);
          }
          else
          {
            oleDbCommand6.Parameters["@com"].Value = (object) 0;
            oleDbCommand6.Parameters["@cov"].Value = (object) 0;
          }
          if (dictionary1.ContainsKey("NO2"))
          {
            oleDbCommand6.Parameters["@no2m"].Value = (object) (1000000.0 * dictionary2[dictionary1["NO2"]] * (oleDbDataReader4.GetDouble(ordinal6) / num5));
            oleDbCommand6.Parameters["@no2v"].Value = (object) (dictionary3[dictionary1["NO2"]] * oleDbDataReader4.GetDouble(ordinal6) / num5);
          }
          else
          {
            oleDbCommand6.Parameters["@no2m"].Value = (object) 0;
            oleDbCommand6.Parameters["@no2v"].Value = (object) 0;
          }
          if (dictionary1.ContainsKey("O3"))
          {
            oleDbCommand6.Parameters["@o3m"].Value = (object) (1000000.0 * dictionary2[dictionary1["O3"]] * (oleDbDataReader4.GetDouble(ordinal6) / num5));
            oleDbCommand6.Parameters["@o3v"].Value = (object) (dictionary3[dictionary1["O3"]] * oleDbDataReader4.GetDouble(ordinal6) / num5);
          }
          else
          {
            oleDbCommand6.Parameters["@o3m"].Value = (object) 0;
            oleDbCommand6.Parameters["@o3v"].Value = (object) 0;
          }
          if (dictionary1.ContainsKey("PM10*"))
          {
            oleDbCommand6.Parameters["@pm10m"].Value = (object) (1000000.0 * dictionary2[dictionary1["PM10*"]] * (oleDbDataReader4.GetDouble(ordinal5) / (num3 * 10000.0)));
            oleDbCommand6.Parameters["@pm10v"].Value = (object) (dictionary3[dictionary1["PM10*"]] * oleDbDataReader4.GetDouble(ordinal5) / (num3 * 10000.0));
          }
          else
          {
            oleDbCommand6.Parameters["@pm10m"].Value = (object) 0;
            oleDbCommand6.Parameters["@pm10v"].Value = (object) 0;
          }
          if (dictionary1.ContainsKey("PM2.5"))
          {
            oleDbCommand6.Parameters["@pm25m"].Value = (object) (1000000.0 * dictionary2[dictionary1["PM2.5"]] * (oleDbDataReader4.GetDouble(ordinal5) / (num3 * 10000.0)));
            oleDbCommand6.Parameters["@pm25v"].Value = (object) (dictionary3[dictionary1["PM2.5"]] * oleDbDataReader4.GetDouble(ordinal5) / (num3 * 10000.0));
          }
          else
          {
            oleDbCommand6.Parameters["@pm25m"].Value = (object) 0;
            oleDbCommand6.Parameters["@pm25v"].Value = (object) 0;
          }
          if (dictionary1.ContainsKey("SO2"))
          {
            oleDbCommand6.Parameters["@so2m"].Value = (object) (1000000.0 * dictionary2[dictionary1["SO2"]] * (oleDbDataReader4.GetDouble(ordinal5) / (num3 * 10000.0)));
            oleDbCommand6.Parameters["@so2v"].Value = (object) (dictionary3[dictionary1["SO2"]] * oleDbDataReader4.GetDouble(ordinal5) / (num3 * 10000.0));
          }
          else
          {
            oleDbCommand6.Parameters["@so2m"].Value = (object) 0;
            oleDbCommand6.Parameters["@so2v"].Value = (object) 0;
          }
          oleDbCommand6.ExecuteNonQuery();
        }
        oleDbDataReader4.Close();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void PopulateMonthPollutantTableForCombinedTreeShrub(
      ProgramSession m_ps,
      OleDbConnection inputDBCon)
    {
      try
      {
        int unit1 = EngineModel.getUnit(inputDBCon, 22, 1, 1);
        int unit2 = EngineModel.getUnit(inputDBCon, 29, 1, 1);
        List<int> classifierList = new List<int>();
        classifierList.Add(19);
        classifierList.Add(15);
        List<string> classifierNameList = new List<string>();
        classifierNameList.Add(Classifiers.Month.ToString());
        classifierNameList.Add(Classifiers.Pollutant.ToString());
        this.CombinedTreeShrubData(inputDBCon, this.currYear.Guid, classifierList, classifierNameList, 31, unit1, 1);
        this.CombinedTreeShrubData(inputDBCon, this.currYear.Guid, classifierList, classifierNameList, 31, unit2, 1);
        classifierList.Clear();
        classifierList.Add(3);
        classifierList.Add(15);
        classifierNameList.Clear();
        classifierNameList.Add(Classifiers.Strata.ToString());
        classifierNameList.Add(Classifiers.Pollutant.ToString());
        this.CombinedTreeShrubData(inputDBCon, this.currYear.Guid, classifierList, classifierNameList, 31, unit1, 1);
        this.CombinedTreeShrubData(inputDBCon, this.currYear.Guid, classifierList, classifierNameList, 31, unit2, 1);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private int CompareValues(List<int> v, List<int> t)
    {
      for (int index = 0; index < v.Count; ++index)
      {
        if (v[index] < t[index])
          return -1;
        if (v[index] > t[index])
          return 1;
      }
      return 0;
    }

    private int CompareValues(int v1, int v2, int v3, int t1, int t2, int t3)
    {
      if (v1 == t1 && v2 == t2 && v3 == t3)
        return 0;
      if (v1 < t1)
        return -1;
      if (v1 > t1)
        return 1;
      if (v2 < t2)
        return -1;
      if (v2 > t2)
        return 1;
      if (v3 < t3)
        return -1;
      return v3 > t3 ? 1 : 0;
    }

    private void PopulatingUFORE_BResults(
      ProgramSession m_ps,
      OleDbConnection inputDBCon,
      string UFOREBdatabase,
      bool isForTree)
    {
      try
      {
        string str1 = !isForTree ? "SHRUB" : "TREE";
        double num1 = 1.0;
        if (this.currSeries.SampleType == SampleType.Inventory)
          num1 = 0.001;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        OleDbCommand oleDbCommand1 = new OleDbCommand();
        oleDbCommand1.Connection = inputDBCon;
        oleDbCommand1.CommandText = "select * from ClassValueTable where YearGUID={" + this.currYear.Guid.ToString() + "} and ClassifierId=" + 16.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        OleDbDataReader oleDbDataReader1 = oleDbCommand1.ExecuteReader();
        if (oleDbDataReader1.HasRows)
        {
          while (oleDbDataReader1.Read())
          {
            string str2 = oleDbDataReader1.GetString(oleDbDataReader1.GetOrdinal("ClassValueName"));
            if (!(str2 == "Monoterpene"))
            {
              if (!(str2 == "Isoprene"))
              {
                if (str2 == "VOC Other")
                  num4 = (int) oleDbDataReader1.GetInt16(oleDbDataReader1.GetOrdinal("ClassValueOrder"));
              }
              else
                num3 = (int) oleDbDataReader1.GetInt16(oleDbDataReader1.GetOrdinal("ClassValueOrder"));
            }
            else
              num2 = (int) oleDbDataReader1.GetInt16(oleDbDataReader1.GetOrdinal("ClassValueOrder"));
          }
          oleDbDataReader1.Close();
        }
        else
        {
          oleDbDataReader1.Close();
          oleDbCommand1.CommandText = "INSERT INTO ClassValueTable (YearGUID,ClassifierID,ClassValueOrder,ClassValueName) VALUES(@guid,@ID,@VO,@name)";
          oleDbCommand1.Parameters.Clear();
          oleDbCommand1.Parameters.Add("@guid", OleDbType.Guid);
          oleDbCommand1.Parameters.Add("@ID", OleDbType.Integer);
          oleDbCommand1.Parameters.Add("@VO", OleDbType.Integer);
          oleDbCommand1.Parameters.Add("@name", OleDbType.VarChar);
          num2 = 1;
          oleDbCommand1.Parameters["@guid"].Value = (object) this.currYear.Guid;
          oleDbCommand1.Parameters["@ID"].Value = (object) 16;
          oleDbCommand1.Parameters["@VO"].Value = (object) 1;
          oleDbCommand1.Parameters["@name"].Value = (object) "Monoterpene";
          oleDbCommand1.ExecuteNonQuery();
          num3 = 2;
          oleDbCommand1.Parameters["@guid"].Value = (object) this.currYear.Guid;
          oleDbCommand1.Parameters["@ID"].Value = (object) 16;
          oleDbCommand1.Parameters["@VO"].Value = (object) 2;
          oleDbCommand1.Parameters["@name"].Value = (object) "Isoprene";
          oleDbCommand1.ExecuteNonQuery();
          num4 = 3;
          oleDbCommand1.Parameters["@guid"].Value = (object) this.currYear.Guid;
          oleDbCommand1.Parameters["@ID"].Value = (object) 16;
          oleDbCommand1.Parameters["@VO"].Value = (object) 3;
          oleDbCommand1.Parameters["@name"].Value = (object) "VOC Other";
          oleDbCommand1.ExecuteNonQuery();
        }
        int unit1 = EngineModel.getUnit(inputDBCon, 22, 1, 1);
        int unit2 = EngineModel.getUnit(inputDBCon, 21, 1, 28);
        int unit3 = EngineModel.getUnit(inputDBCon, 22, 24, 1);
        double num5 = 0.0;
        double num6 = 0.0;
        double num7 = 0.0;
        double num8 = 0.0;
        Dictionary<int, double> dictionary1 = new Dictionary<int, double>();
        Dictionary<int, double> dictionary2 = new Dictionary<int, double>();
        Dictionary<int, double> dictionary3 = new Dictionary<int, double>();
        Dictionary<string, double> dictionary4 = new Dictionary<string, double>();
        Dictionary<string, double> dictionary5 = new Dictionary<string, double>();
        Dictionary<string, double> dictionary6 = new Dictionary<string, double>();
        Dictionary<string, double> dictionary7 = new Dictionary<string, double>();
        foreach (Stratum stratum in this.allStrata.Values)
        {
          if (stratum.ClassValue != this.ClassValueForStrataStudyArea)
            num8 += stratum.AreaHectareBasedEngineInventoryDB;
        }
        foreach (StratumGenus stratumGenus in this.allStratumGenus.Values)
        {
          if (stratumGenus.Form_Ind == str1)
          {
            if (dictionary7.ContainsKey(stratumGenus.GenusScientificName))
              dictionary7[stratumGenus.GenusScientificName] += stratumGenus.LeafBiomassKilograms;
            else
              dictionary7.Add(stratumGenus.GenusScientificName, stratumGenus.LeafBiomassKilograms);
          }
        }
        OleDbCommand oleDbCommand2 = new OleDbCommand();
        oleDbCommand2.Connection = inputDBCon;
        using (OleDbConnection oleDbConnection = new OleDbConnection())
        {
          EngineModel.OpenConnection(oleDbConnection, UFOREBdatabase);
          OleDbCommand oleDbCommand3 = new OleDbCommand();
          oleDbCommand3.Connection = oleDbConnection;
          oleDbCommand3.CommandText = "select * from YearlySummaryByLanduse";
          OleDbDataReader oleDbDataReader2 = oleDbCommand3.ExecuteReader();
          while (oleDbDataReader2.Read())
          {
            string str3 = oleDbDataReader2.GetString(oleDbDataReader2.GetOrdinal("LanduseDesc"));
            oleDbCommand1.CommandText = "select * from ClassValueTable where YearGUID={" + this.currYear.Guid.ToString() + "} and ClassifierId=" + 3.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and ClassValueName=@n";
            oleDbCommand1.Parameters.Clear();
            oleDbCommand1.Parameters.Add("@n", OleDbType.VarWChar).Value = (object) str3;
            OleDbDataReader oleDbDataReader3 = oleDbCommand1.ExecuteReader();
            oleDbDataReader3.Read();
            int int16 = (int) oleDbDataReader3.GetInt16(oleDbDataReader3.GetOrdinal("ClassValueOrder"));
            oleDbDataReader3.Close();
            dictionary1.Add(int16, num1 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("Monoterpene emitted")));
            dictionary2.Add(int16, num1 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("Isoprene emitted")));
            dictionary3.Add(int16, num1 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("Other VOCs emitted")));
            num5 += num1 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("Monoterpene emitted"));
            num6 += num1 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("Isoprene emitted"));
            num7 += num1 * oleDbDataReader2.GetDouble(oleDbDataReader2.GetOrdinal("Other VOCs emitted"));
          }
          oleDbDataReader2.Close();
          oleDbCommand3.CommandText = "select * from YearlySummaryByGenera";
          OleDbDataReader oleDbDataReader4 = oleDbCommand3.ExecuteReader();
          while (oleDbDataReader4.Read())
          {
            string str4 = oleDbDataReader4.GetString(oleDbDataReader4.GetOrdinal("Genera"));
            dictionary4.Add(str4.ToUpper(), num1 * oleDbDataReader4.GetDouble(oleDbDataReader4.GetOrdinal("Monoterpene emitted")));
            dictionary5.Add(str4.ToUpper(), num1 * oleDbDataReader4.GetDouble(oleDbDataReader4.GetOrdinal("Isoprene emitted")));
            dictionary6.Add(str4.ToUpper(), num1 * oleDbDataReader4.GetDouble(oleDbDataReader4.GetOrdinal("Other VOCs emitted")));
          }
          oleDbDataReader4.Close();
          EngineModel.CloseConnection(oleDbConnection);
        }
        List<int> Classifiers = new List<int>();
        Classifiers.Add(3);
        Classifiers.Add(16);
        string str5 = !isForTree ? EngineModel.getEstimationTable(inputDBCon, 3, Classifiers) : EngineModel.getEstimationTable(inputDBCon, 2, Classifiers);
        OleDbCommand oleDbCommand4 = oleDbCommand2;
        string[] strArray1 = new string[15];
        strArray1[0] = "INSERT INTO [";
        strArray1[1] = str5;
        strArray1[2] = "] (YearGuid,[";
        Classifiers classifiers1 = Classifiers.Strata;
        strArray1[3] = classifiers1.ToString();
        strArray1[4] = "],[";
        classifiers1 = Classifiers.VOCs;
        strArray1[5] = classifiers1.ToString();
        strArray1[6] = "],EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId) VALUES ({";
        strArray1[7] = this.currYear.Guid.ToString();
        strArray1[8] = "},@st,@voc,";
        strArray1[9] = 32.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray1[10] = ",";
        strArray1[11] = 1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray1[12] = ",@v,0,";
        strArray1[13] = unit2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray1[14] = ")";
        string str6 = string.Concat(strArray1);
        oleDbCommand4.CommandText = str6;
        oleDbCommand2.Parameters.Clear();
        oleDbCommand2.Parameters.Add("@st", OleDbType.Integer);
        oleDbCommand2.Parameters.Add("@voc", OleDbType.Integer);
        oleDbCommand2.Parameters.Add("@v", OleDbType.Double);
        foreach (int key in dictionary1.Keys)
        {
          oleDbCommand2.Parameters["@st"].Value = (object) key;
          oleDbCommand2.Parameters["@voc"].Value = (object) num2;
          oleDbCommand2.Parameters["@v"].Value = (object) dictionary1[key];
          oleDbCommand2.ExecuteNonQuery();
          oleDbCommand2.Parameters["@st"].Value = (object) key;
          oleDbCommand2.Parameters["@voc"].Value = (object) num3;
          oleDbCommand2.Parameters["@v"].Value = (object) dictionary2[key];
          oleDbCommand2.ExecuteNonQuery();
          oleDbCommand2.Parameters["@st"].Value = (object) key;
          oleDbCommand2.Parameters["@voc"].Value = (object) num4;
          oleDbCommand2.Parameters["@v"].Value = (object) dictionary3[key];
          oleDbCommand2.ExecuteNonQuery();
        }
        oleDbCommand2.Parameters["@st"].Value = (object) this.ClassValueForStrataStudyArea;
        oleDbCommand2.Parameters["@voc"].Value = (object) num2;
        oleDbCommand2.Parameters["@v"].Value = (object) num5;
        oleDbCommand2.ExecuteNonQuery();
        oleDbCommand2.Parameters["@st"].Value = (object) this.ClassValueForStrataStudyArea;
        oleDbCommand2.Parameters["@voc"].Value = (object) num3;
        oleDbCommand2.Parameters["@v"].Value = (object) num6;
        oleDbCommand2.ExecuteNonQuery();
        oleDbCommand2.Parameters["@st"].Value = (object) this.ClassValueForStrataStudyArea;
        oleDbCommand2.Parameters["@voc"].Value = (object) num4;
        oleDbCommand2.Parameters["@v"].Value = (object) num7;
        oleDbCommand2.ExecuteNonQuery();
        Classifiers.Clear();
        Classifiers.Add(3);
        Classifiers.Add(4);
        string str7 = !isForTree ? EngineModel.getEstimationTable(inputDBCon, 3, Classifiers) : EngineModel.getEstimationTable(inputDBCon, 2, Classifiers);
        Classifiers.Clear();
        Classifiers.Add(3);
        Classifiers.Add(4);
        Classifiers.Add(16);
        string str8 = !isForTree ? EngineModel.getEstimationTable(inputDBCon, 3, Classifiers) : EngineModel.getEstimationTable(inputDBCon, 2, Classifiers);
        OleDbCommand oleDbCommand5 = oleDbCommand2;
        string[] strArray2 = new string[17];
        strArray2[0] = "INSERT INTO [";
        strArray2[1] = str8;
        strArray2[2] = "] (YearGuid,[";
        Classifiers classifiers2 = Classifiers.Strata;
        strArray2[3] = classifiers2.ToString();
        strArray2[4] = "],[";
        classifiers2 = Classifiers.Species;
        strArray2[5] = classifiers2.ToString();
        strArray2[6] = "],[";
        classifiers2 = Classifiers.VOCs;
        strArray2[7] = classifiers2.ToString();
        strArray2[8] = "],EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId) VALUES ({";
        strArray2[9] = this.currYear.Guid.ToString();
        strArray2[10] = "},@st,@spp,@voc,";
        int num9 = 32;
        strArray2[11] = num9.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray2[12] = ",";
        num9 = 1;
        strArray2[13] = num9.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray2[14] = ",@v,0,";
        strArray2[15] = unit2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray2[16] = ")";
        string str9 = string.Concat(strArray2);
        oleDbCommand5.CommandText = str9;
        oleDbCommand2.Parameters.Clear();
        oleDbCommand2.Parameters.Add("@st", OleDbType.Integer);
        oleDbCommand2.Parameters.Add("@spp", OleDbType.Integer);
        oleDbCommand2.Parameters.Add("@voc", OleDbType.Integer);
        oleDbCommand2.Parameters.Add("@v", OleDbType.Double);
        if (isForTree)
        {
          OleDbCommand oleDbCommand6 = oleDbCommand1;
          string[] strArray3 = new string[18];
          strArray3[0] = "select * from [";
          strArray3[1] = str7;
          strArray3[2] = "] where YearGUID={";
          strArray3[3] = this.currYear.Guid.ToString();
          strArray3[4] = "} and  EstimateType=";
          num9 = 6;
          strArray3[5] = num9.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray3[6] = " and  EquationType=";
          num9 = 1;
          strArray3[7] = num9.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray3[8] = " and  EstimateUnitsId=";
          strArray3[9] = unit1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray3[10] = " and [";
          classifiers2 = Classifiers.Strata;
          strArray3[11] = classifiers2.ToString();
          strArray3[12] = "] <> ";
          strArray3[13] = this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray3[14] = " and [";
          classifiers2 = Classifiers.Species;
          strArray3[15] = classifiers2.ToString();
          strArray3[16] = "] <> ";
          strArray3[17] = this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str10 = string.Concat(strArray3);
          oleDbCommand6.CommandText = str10;
        }
        else
        {
          OleDbCommand oleDbCommand7 = oleDbCommand1;
          string[] strArray4 = new string[18];
          strArray4[0] = "select * from [";
          strArray4[1] = str7;
          strArray4[2] = "] where YearGUID={";
          strArray4[3] = this.currYear.Guid.ToString();
          strArray4[4] = "} and  EstimateType=";
          num9 = 6;
          strArray4[5] = num9.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray4[6] = " and  EquationType=";
          num9 = 1;
          strArray4[7] = num9.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray4[8] = " and  EstimateUnitsId=";
          strArray4[9] = unit3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray4[10] = " and [";
          classifiers2 = Classifiers.Strata;
          strArray4[11] = classifiers2.ToString();
          strArray4[12] = "] <> ";
          strArray4[13] = this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray4[14] = " and [";
          classifiers2 = Classifiers.Species;
          strArray4[15] = classifiers2.ToString();
          strArray4[16] = "] <> ";
          strArray4[17] = this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str11 = string.Concat(strArray4);
          oleDbCommand7.CommandText = str11;
        }
        OleDbDataReader oleDbDataReader5 = oleDbCommand1.ExecuteReader();
        OleDbDataReader oleDbDataReader6 = oleDbDataReader5;
        classifiers2 = Classifiers.Strata;
        string name1 = classifiers2.ToString();
        int ordinal1 = oleDbDataReader6.GetOrdinal(name1);
        OleDbDataReader oleDbDataReader7 = oleDbDataReader5;
        classifiers2 = Classifiers.Species;
        string name2 = classifiers2.ToString();
        int ordinal2 = oleDbDataReader7.GetOrdinal(name2);
        int ordinal3 = oleDbDataReader5.GetOrdinal("EstimateValue");
        double num10 = 0.0;
        double num11 = 0.0;
        int num12 = 0;
        while (oleDbDataReader5.Read())
        {
          int int32_1 = oleDbDataReader5.GetInt32(ordinal1);
          int int32_2 = oleDbDataReader5.GetInt32(ordinal2);
          double num13;
          double num14;
          double num15;
          if (this.allStrata.ContainsKey(int32_1))
          {
            double num16 = !isForTree ? this.allStrata[int32_1].AreaHectareBasedEngineInventoryDB * oleDbDataReader5.GetDouble(ordinal3) : oleDbDataReader5.GetDouble(ordinal3);
            string genusScientificName = this.allSpecies[this.allSpeciesClassValueOrderToSppCode[int32_2]].GenusScientificName;
            double num17 = !dictionary7.ContainsKey(genusScientificName) ? 0.0 : dictionary7[genusScientificName];
            if (num16 * 1000.0 > num17)
              num16 = num17 / 1000.0;
            if (num17 != 0.0)
            {
              num13 = dictionary4[genusScientificName.ToUpper()] * (num16 * 1000.0 / num17);
              num14 = dictionary5[genusScientificName.ToUpper()] * (num16 * 1000.0 / num17);
              num15 = dictionary6[genusScientificName.ToUpper()] * (num16 * 1000.0 / num17);
            }
            else
            {
              num10 = 0.0;
              num13 = 0.0;
              num14 = 0.0;
              num15 = 0.0;
            }
          }
          else
          {
            num10 = 0.0;
            num13 = 0.0;
            num14 = 0.0;
            num15 = 0.0;
          }
          oleDbCommand2.Parameters["@st"].Value = (object) int32_1;
          oleDbCommand2.Parameters["@spp"].Value = (object) int32_2;
          oleDbCommand2.Parameters["@voc"].Value = (object) num2;
          oleDbCommand2.Parameters["@v"].Value = (object) num13;
          oleDbCommand2.ExecuteNonQuery();
          oleDbCommand2.Parameters["@st"].Value = (object) int32_1;
          oleDbCommand2.Parameters["@spp"].Value = (object) int32_2;
          oleDbCommand2.Parameters["@voc"].Value = (object) num3;
          oleDbCommand2.Parameters["@v"].Value = (object) num14;
          oleDbCommand2.ExecuteNonQuery();
          oleDbCommand2.Parameters["@st"].Value = (object) int32_1;
          oleDbCommand2.Parameters["@spp"].Value = (object) int32_2;
          oleDbCommand2.Parameters["@voc"].Value = (object) num4;
          oleDbCommand2.Parameters["@v"].Value = (object) num15;
          oleDbCommand2.ExecuteNonQuery();
        }
        oleDbDataReader5.Close();
        foreach (int key in dictionary1.Keys)
        {
          oleDbCommand2.Parameters["@st"].Value = (object) key;
          oleDbCommand2.Parameters["@spp"].Value = (object) this.ClassValueForSpeciesAllSpecies;
          oleDbCommand2.Parameters["@voc"].Value = (object) num2;
          oleDbCommand2.Parameters["@v"].Value = (object) dictionary1[key];
          oleDbCommand2.ExecuteNonQuery();
          oleDbCommand2.Parameters["@st"].Value = (object) key;
          oleDbCommand2.Parameters["@spp"].Value = (object) this.ClassValueForSpeciesAllSpecies;
          oleDbCommand2.Parameters["@voc"].Value = (object) num3;
          oleDbCommand2.Parameters["@v"].Value = (object) dictionary2[key];
          oleDbCommand2.ExecuteNonQuery();
          oleDbCommand2.Parameters["@st"].Value = (object) key;
          oleDbCommand2.Parameters["@spp"].Value = (object) this.ClassValueForSpeciesAllSpecies;
          oleDbCommand2.Parameters["@voc"].Value = (object) num4;
          oleDbCommand2.Parameters["@v"].Value = (object) dictionary3[key];
          oleDbCommand2.ExecuteNonQuery();
        }
        oleDbCommand2.Parameters["@st"].Value = (object) this.ClassValueForStrataStudyArea;
        oleDbCommand2.Parameters["@spp"].Value = (object) this.ClassValueForSpeciesAllSpecies;
        oleDbCommand2.Parameters["@voc"].Value = (object) num2;
        oleDbCommand2.Parameters["@v"].Value = (object) num5;
        oleDbCommand2.ExecuteNonQuery();
        oleDbCommand2.Parameters["@st"].Value = (object) this.ClassValueForStrataStudyArea;
        oleDbCommand2.Parameters["@spp"].Value = (object) this.ClassValueForSpeciesAllSpecies;
        oleDbCommand2.Parameters["@voc"].Value = (object) num3;
        oleDbCommand2.Parameters["@v"].Value = (object) num6;
        oleDbCommand2.ExecuteNonQuery();
        oleDbCommand2.Parameters["@st"].Value = (object) this.ClassValueForStrataStudyArea;
        oleDbCommand2.Parameters["@spp"].Value = (object) this.ClassValueForSpeciesAllSpecies;
        oleDbCommand2.Parameters["@voc"].Value = (object) num4;
        oleDbCommand2.Parameters["@v"].Value = (object) num7;
        oleDbCommand2.ExecuteNonQuery();
        Classifiers.Clear();
        Classifiers.Add(4);
        string str12 = !isForTree ? EngineModel.getEstimationTable(inputDBCon, 3, Classifiers) : EngineModel.getEstimationTable(inputDBCon, 2, Classifiers);
        Classifiers.Clear();
        Classifiers.Add(4);
        Classifiers.Add(16);
        string str13 = !isForTree ? EngineModel.getEstimationTable(inputDBCon, 3, Classifiers) : EngineModel.getEstimationTable(inputDBCon, 2, Classifiers);
        OleDbCommand oleDbCommand8 = oleDbCommand2;
        string[] strArray5 = new string[15];
        strArray5[0] = "INSERT INTO [";
        strArray5[1] = str13;
        strArray5[2] = "] (YearGuid,[";
        Classifiers classifiers3 = Classifiers.Species;
        strArray5[3] = classifiers3.ToString();
        strArray5[4] = "],[";
        classifiers3 = Classifiers.VOCs;
        strArray5[5] = classifiers3.ToString();
        strArray5[6] = "],EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId) VALUES ({";
        strArray5[7] = this.currYear.Guid.ToString();
        strArray5[8] = "},@spp,@voc,";
        strArray5[9] = 32.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray5[10] = ",";
        int num18 = 1;
        strArray5[11] = num18.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray5[12] = ",@v,0,";
        strArray5[13] = unit2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray5[14] = ")";
        string str14 = string.Concat(strArray5);
        oleDbCommand8.CommandText = str14;
        oleDbCommand2.Parameters.Clear();
        oleDbCommand2.Parameters.Add("@spp", OleDbType.Integer);
        oleDbCommand2.Parameters.Add("@voc", OleDbType.Integer);
        oleDbCommand2.Parameters.Add("@v", OleDbType.Double);
        Guid guid;
        if (isForTree)
        {
          OleDbCommand oleDbCommand9 = oleDbCommand1;
          string[] strArray6 = new string[14];
          strArray6[0] = "select * from [";
          strArray6[1] = str12;
          strArray6[2] = "] where YearGUID={";
          guid = this.currYear.Guid;
          strArray6[3] = guid.ToString();
          strArray6[4] = "} and  EstimateType=";
          num18 = 6;
          strArray6[5] = num18.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray6[6] = " and  EquationType=";
          num18 = 1;
          strArray6[7] = num18.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray6[8] = " and  EstimateUnitsId=";
          strArray6[9] = unit1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray6[10] = " and [";
          classifiers3 = Classifiers.Species;
          strArray6[11] = classifiers3.ToString();
          strArray6[12] = "] <> ";
          strArray6[13] = this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str15 = string.Concat(strArray6);
          oleDbCommand9.CommandText = str15;
        }
        else
        {
          OleDbCommand oleDbCommand10 = oleDbCommand1;
          string[] strArray7 = new string[14];
          strArray7[0] = "select * from [";
          strArray7[1] = str12;
          strArray7[2] = "] where YearGUID={";
          guid = this.currYear.Guid;
          strArray7[3] = guid.ToString();
          strArray7[4] = "} and  EstimateType=";
          num18 = 6;
          strArray7[5] = num18.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray7[6] = " and  EquationType=";
          num18 = 1;
          strArray7[7] = num18.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray7[8] = " and  EstimateUnitsId=";
          strArray7[9] = unit3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray7[10] = " and [";
          classifiers3 = Classifiers.Species;
          strArray7[11] = classifiers3.ToString();
          strArray7[12] = "] <> ";
          strArray7[13] = this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str16 = string.Concat(strArray7);
          oleDbCommand10.CommandText = str16;
        }
        OleDbDataReader oleDbDataReader8 = oleDbCommand1.ExecuteReader();
        OleDbDataReader oleDbDataReader9 = oleDbDataReader8;
        classifiers3 = Classifiers.Species;
        string name3 = classifiers3.ToString();
        int ordinal4 = oleDbDataReader9.GetOrdinal(name3);
        int ordinal5 = oleDbDataReader8.GetOrdinal("EstimateValue");
        double num19 = 0.0;
        num11 = 0.0;
        num12 = 0;
        while (oleDbDataReader8.Read())
        {
          int int32 = oleDbDataReader8.GetInt32(ordinal4);
          double num20 = !isForTree ? num8 * oleDbDataReader8.GetDouble(ordinal5) : oleDbDataReader8.GetDouble(ordinal5);
          string genusScientificName = this.allSpecies[this.allSpeciesClassValueOrderToSppCode[int32]].GenusScientificName;
          double num21 = !dictionary7.ContainsKey(genusScientificName) ? 0.0 : dictionary7[genusScientificName];
          if (num20 * 1000.0 > num21)
            num20 = num21 / 1000.0;
          double num22;
          double num23;
          double num24;
          if (num21 != 0.0)
          {
            num22 = dictionary4[genusScientificName.ToUpper()] * (num20 * 1000.0 / num21);
            num23 = dictionary5[genusScientificName.ToUpper()] * (num20 * 1000.0 / num21);
            num24 = dictionary6[genusScientificName.ToUpper()] * (num20 * 1000.0 / num21);
          }
          else
          {
            num19 = 0.0;
            num22 = 0.0;
            num23 = 0.0;
            num24 = 0.0;
          }
          oleDbCommand2.Parameters["@spp"].Value = (object) int32;
          oleDbCommand2.Parameters["@voc"].Value = (object) num2;
          oleDbCommand2.Parameters["@v"].Value = (object) num22;
          oleDbCommand2.ExecuteNonQuery();
          oleDbCommand2.Parameters["@spp"].Value = (object) int32;
          oleDbCommand2.Parameters["@voc"].Value = (object) num3;
          oleDbCommand2.Parameters["@v"].Value = (object) num23;
          oleDbCommand2.ExecuteNonQuery();
          oleDbCommand2.Parameters["@spp"].Value = (object) int32;
          oleDbCommand2.Parameters["@voc"].Value = (object) num4;
          oleDbCommand2.Parameters["@v"].Value = (object) num24;
          oleDbCommand2.ExecuteNonQuery();
        }
        oleDbDataReader8.Close();
        oleDbCommand2.Parameters["@spp"].Value = (object) this.ClassValueForSpeciesAllSpecies;
        oleDbCommand2.Parameters["@voc"].Value = (object) num2;
        oleDbCommand2.Parameters["@v"].Value = (object) num5;
        oleDbCommand2.ExecuteNonQuery();
        oleDbCommand2.Parameters["@spp"].Value = (object) this.ClassValueForSpeciesAllSpecies;
        oleDbCommand2.Parameters["@voc"].Value = (object) num3;
        oleDbCommand2.Parameters["@v"].Value = (object) num6;
        oleDbCommand2.ExecuteNonQuery();
        oleDbCommand2.Parameters["@spp"].Value = (object) this.ClassValueForSpeciesAllSpecies;
        oleDbCommand2.Parameters["@voc"].Value = (object) num4;
        oleDbCommand2.Parameters["@v"].Value = (object) num7;
        oleDbCommand2.ExecuteNonQuery();
        if (!isForTree)
          return;
        oleDbCommand2.CommandText = "UPDATE IndividualTreePollutionEffects SET  ISOPRENE = @iso,  MONOTERP = @mon,  OVOC = @voc  WHERE YearGuid=@yid AND PlotId=@plotid AND TreeId=@treeid";
        oleDbCommand2.Parameters.Clear();
        oleDbCommand2.Parameters.Add("@iso", OleDbType.Double);
        oleDbCommand2.Parameters.Add("@mon", OleDbType.Double);
        oleDbCommand2.Parameters.Add("@voc", OleDbType.Double);
        oleDbCommand2.Parameters.Add("@yid", OleDbType.Guid).Value = (object) this.currYear.Guid;
        oleDbCommand2.Parameters.Add("@plotid", OleDbType.Integer);
        oleDbCommand2.Parameters.Add("@treeid", OleDbType.Integer);
        OleDbCommand oleDbCommand11 = oleDbCommand1;
        guid = this.currYear.Guid;
        string str17 = "SELECT EcoPlots.PlotId, EcoTrees.TreeId, EcoTrees.Species, IndividualTreeEffects.GroundArea, IndividualTreeEffects.LeafArea, IndividualTreeEffects.LeafBioMass FROM EcoPlots INNER JOIN (EcoTrees INNER JOIN IndividualTreeEffects ON EcoTrees.TreeId = IndividualTreeEffects.TreeId) ON (EcoPlots.PlotKey = EcoTrees.PlotKey) AND (EcoPlots.YearKey = IndividualTreeEffects.YearGuid) AND (EcoPlots.PlotId = IndividualTreeEffects.PlotId) WHERE (((EcoPlots.YearKey)={guid {" + guid.ToString() + "}}))";
        oleDbCommand11.CommandText = str17;
        OleDbDataReader oleDbDataReader10 = oleDbCommand1.ExecuteReader();
        int ordinal6 = oleDbDataReader10.GetOrdinal("Species");
        int ordinal7 = oleDbDataReader10.GetOrdinal("LeafBioMass");
        int ordinal8 = oleDbDataReader10.GetOrdinal("PlotID");
        int ordinal9 = oleDbDataReader10.GetOrdinal("TreeID");
        while (oleDbDataReader10.Read())
        {
          double num25 = oleDbDataReader10.GetDouble(ordinal7);
          string genusScientificName = this.allSpecies[oleDbDataReader10.GetString(ordinal6)].GenusScientificName;
          double num26 = !dictionary7.ContainsKey(genusScientificName) ? 0.0 : dictionary7[genusScientificName];
          double num27;
          double num28;
          double num29;
          if (num26 != 0.0)
          {
            num27 = dictionary4[genusScientificName.ToUpper()] * (1000.0 * num25 / num26);
            num28 = dictionary5[genusScientificName.ToUpper()] * (1000.0 * num25 / num26);
            num29 = dictionary6[genusScientificName.ToUpper()] * (1000.0 * num25 / num26);
          }
          else
          {
            num27 = 0.0;
            num28 = 0.0;
            num29 = 0.0;
          }
          oleDbCommand2.Parameters["@iso"].Value = (object) num27;
          oleDbCommand2.Parameters["@mon"].Value = (object) num28;
          oleDbCommand2.Parameters["@voc"].Value = (object) num29;
          oleDbCommand2.Parameters["@plotid"].Value = (object) oleDbDataReader10.GetInt32(ordinal8);
          oleDbCommand2.Parameters["@treeid"].Value = (object) oleDbDataReader10.GetInt32(ordinal9);
          oleDbCommand2.ExecuteNonQuery();
        }
        oleDbDataReader10.Close();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void PopulatingUFORE_BResultsForCombinedTreeShrub(
      ProgramSession m_ps,
      OleDbConnection inputDBCon)
    {
      try
      {
        int estimateTypeValue = 32;
        int unit = EngineModel.getUnit(inputDBCon, 21, 1, 28);
        int equationTypeValue = 1;
        List<int> classifierList = new List<int>();
        List<string> classifierNameList = new List<string>();
        classifierList.Add(3);
        classifierList.Add(16);
        classifierNameList.Add(Classifiers.Strata.ToString());
        classifierNameList.Add(Classifiers.VOCs.ToString());
        this.CombinedTreeShrubData(inputDBCon, this.currYear.Guid, classifierList, classifierNameList, estimateTypeValue, unit, equationTypeValue);
        classifierList.Clear();
        classifierList.Add(3);
        classifierList.Add(4);
        classifierList.Add(16);
        classifierNameList.Clear();
        classifierNameList.Add(Classifiers.Strata.ToString());
        classifierNameList.Add(Classifiers.Species.ToString());
        classifierNameList.Add(Classifiers.VOCs.ToString());
        this.CombinedTreeShrubData(inputDBCon, this.currYear.Guid, classifierList, classifierNameList, estimateTypeValue, unit, equationTypeValue);
        classifierList.Clear();
        classifierList.Add(4);
        classifierList.Add(16);
        classifierNameList.Clear();
        classifierNameList.Add(Classifiers.Species.ToString());
        classifierNameList.Add(Classifiers.VOCs.ToString());
        this.CombinedTreeShrubData(inputDBCon, this.currYear.Guid, classifierList, classifierNameList, estimateTypeValue, unit, equationTypeValue);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void CombinedTreeShrubData(
      OleDbConnection inputDBCon,
      Guid yearId,
      List<int> classifierList,
      List<string> classifierNameList,
      int estimateTypeValue,
      int estimateUnitValue,
      int equationTypeValue)
    {
      try
      {
        OleDbCommand oleDbCommand1 = new OleDbCommand();
        oleDbCommand1.Connection = inputDBCon;
        OleDbCommand oleDbCommand2 = new OleDbCommand();
        oleDbCommand2.Connection = inputDBCon;
        OleDbCommand oleDbCommand3 = new OleDbCommand();
        oleDbCommand3.Connection = inputDBCon;
        string str1 = "";
        string str2 = "";
        List<int> v = new List<int>();
        List<int> t = new List<int>();
        for (int index = 0; index < classifierNameList.Count; ++index)
        {
          if (index == 0)
          {
            str1 = "[" + classifierNameList.ElementAt<string>(index) + "]";
            str2 = "@f" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
          else
          {
            str1 = str1 + ",[" + classifierNameList.ElementAt<string>(index) + "]";
            str2 = str2 + ",@f" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
          v.Add(0);
          t.Add(0);
        }
        string estimationTable1 = EngineModel.getEstimationTable(inputDBCon, 2, classifierList);
        string estimationTable2 = EngineModel.getEstimationTable(inputDBCon, 3, classifierList);
        string estimationTable3 = EngineModel.getEstimationTable(inputDBCon, 4, classifierList);
        oleDbCommand1.CommandText = "select * from [" + estimationTable1 + "] where YearGUID={" + yearId.ToString() + "} and  EstimateType=" + estimateTypeValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and EquationType=" + equationTypeValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and  EstimateUnitsId=" + estimateUnitValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " Order by " + str1;
        OleDbDataReader oleDbDataReader1 = oleDbCommand1.ExecuteReader();
        oleDbCommand2.CommandText = "select * from [" + estimationTable2 + "] where YearGUID={" + yearId.ToString() + "} and  EstimateType=" + estimateTypeValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and EquationType=" + equationTypeValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and  EstimateUnitsId=" + estimateUnitValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " Order by " + str1;
        OleDbDataReader oleDbDataReader2 = oleDbCommand2.ExecuteReader();
        List<int> source1 = new List<int>();
        for (int index = 0; index < classifierNameList.Count; ++index)
          source1.Add(oleDbDataReader1.GetOrdinal(classifierNameList.ElementAt<string>(index)));
        int ordinal1 = oleDbDataReader1.GetOrdinal("EstimateValue");
        List<int> source2 = new List<int>();
        for (int index = 0; index < classifierNameList.Count; ++index)
          source2.Add(oleDbDataReader2.GetOrdinal(classifierNameList.ElementAt<string>(index)));
        int ordinal2 = oleDbDataReader2.GetOrdinal("EstimateValue");
        oleDbCommand3.CommandText = "delete * from [" + estimationTable3 + "] where YearGUID={" + yearId.ToString() + "} and  EstimateType=" + estimateTypeValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and EquationType=" + equationTypeValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " and  EstimateUnitsId=" + estimateUnitValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        oleDbCommand3.ExecuteNonQuery();
        oleDbCommand3.CommandText = "INSERT INTO [" + estimationTable3 + "] (YearGuid," + str1 + ",EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId) VALUES ({" + yearId.ToString() + "}," + str2 + "," + estimateTypeValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "," + equationTypeValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ",@v,0," + estimateUnitValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) + ")";
        oleDbCommand3.Parameters.Clear();
        for (int index = 0; index < classifierNameList.Count; ++index)
          oleDbCommand3.Parameters.Add("@f" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture), OleDbType.Integer);
        oleDbCommand3.Parameters.Add("@v", OleDbType.Double);
        bool flag1 = oleDbDataReader1.Read();
        bool flag2 = oleDbDataReader2.Read();
        while (flag1 || flag2)
        {
          if (flag1 & flag2)
          {
            for (int index = 0; index < classifierNameList.Count; ++index)
            {
              v[index] = oleDbDataReader1.GetInt32(source1[index]);
              t[index] = oleDbDataReader2.GetInt32(source2[index]);
            }
            switch (this.CompareValues(v, t))
            {
              case -1:
                for (int index = 0; index < classifierNameList.Count; ++index)
                  oleDbCommand3.Parameters["@f" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture)].Value = (object) oleDbDataReader1.GetInt32(source1.ElementAt<int>(index));
                oleDbCommand3.Parameters["@v"].Value = (object) oleDbDataReader1.GetDouble(ordinal1);
                oleDbCommand3.ExecuteNonQuery();
                flag1 = oleDbDataReader1.Read();
                break;
              case 0:
                for (int index = 0; index < classifierNameList.Count; ++index)
                  oleDbCommand3.Parameters["@f" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture)].Value = (object) oleDbDataReader1.GetInt32(source1.ElementAt<int>(index));
                oleDbCommand3.Parameters["@v"].Value = (object) (oleDbDataReader1.GetDouble(ordinal1) + oleDbDataReader2.GetDouble(ordinal2));
                oleDbCommand3.ExecuteNonQuery();
                flag1 = oleDbDataReader1.Read();
                flag2 = oleDbDataReader2.Read();
                break;
              case 1:
                for (int index = 0; index < classifierNameList.Count; ++index)
                  oleDbCommand3.Parameters["@f" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture)].Value = (object) oleDbDataReader2.GetInt32(source2.ElementAt<int>(index));
                oleDbCommand3.Parameters["@v"].Value = (object) oleDbDataReader2.GetDouble(ordinal2);
                oleDbCommand3.ExecuteNonQuery();
                flag2 = oleDbDataReader2.Read();
                break;
            }
          }
          if (flag1 && !flag2)
          {
            for (int index = 0; index < classifierNameList.Count; ++index)
              oleDbCommand3.Parameters["@f" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture)].Value = (object) oleDbDataReader1.GetInt32(source1.ElementAt<int>(index));
            oleDbCommand3.Parameters["@v"].Value = (object) oleDbDataReader1.GetDouble(ordinal1);
            oleDbCommand3.ExecuteNonQuery();
            flag1 = oleDbDataReader1.Read();
          }
          if (!flag1 & flag2)
          {
            for (int index = 0; index < classifierNameList.Count; ++index)
              oleDbCommand3.Parameters["@f" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture)].Value = (object) oleDbDataReader2.GetInt32(source2.ElementAt<int>(index));
            oleDbCommand3.Parameters["@v"].Value = (object) oleDbDataReader2.GetDouble(ordinal2);
            oleDbCommand3.ExecuteNonQuery();
            flag2 = oleDbDataReader2.Read();
          }
        }
        oleDbDataReader1.Close();
        oleDbDataReader2.Close();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void PopulatingWaterInterception(
      ProgramSession m_ps,
      OleDbConnection inputDBCon,
      string WaterInterceptiondatabase,
      bool isForTree)
    {
      try
      {
        int unit1 = EngineModel.getUnit(inputDBCon, 16, 1, 1);
        int unit2 = EngineModel.getUnit(inputDBCon, 16, 24, 1);
        int unit3 = EngineModel.getUnit(inputDBCon, 37, 1, 28);
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        Dictionary<int, double> dictionary1 = new Dictionary<int, double>();
        OleDbCommand oleDbCommand1 = new OleDbCommand();
        using (OleDbConnection oleDbConnection = new OleDbConnection())
        {
          EngineModel.OpenConnection(oleDbConnection, WaterInterceptiondatabase);
          oleDbCommand1.Connection = oleDbConnection;
          oleDbCommand1.CommandText = "select * from 01_InterceptYearlySums";
          OleDbDataReader oleDbDataReader = oleDbCommand1.ExecuteReader();
          oleDbDataReader.Read();
          num1 = oleDbDataReader.GetDouble(oleDbDataReader.GetOrdinal("VegIntercept (m3/yr)"));
          oleDbDataReader.Close();
          EngineModel.CloseConnection(oleDbConnection);
        }
        List<int> Classifiers = new List<int>();
        Classifiers.Add(3);
        string estimationTable1;
        int num4;
        Classifiers classifiers1;
        if (isForTree)
        {
          estimationTable1 = EngineModel.getEstimationTable(inputDBCon, 2, Classifiers);
          OleDbCommand oleDbCommand2 = oleDbCommand1;
          string[] strArray = new string[14];
          strArray[0] = "select * from [";
          strArray[1] = estimationTable1;
          strArray[2] = "] where YearGUID={";
          strArray[3] = this.currYear.Guid.ToString();
          strArray[4] = "}  and EstimateType=";
          num4 = 5;
          strArray[5] = num4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray[6] = " and EquationType=";
          num4 = 1;
          strArray[7] = num4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray[8] = " and EstimateUnitsId=";
          strArray[9] = unit1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray[10] = " and [";
          classifiers1 = Classifiers.Strata;
          strArray[11] = classifiers1.ToString();
          strArray[12] = "] <> ";
          strArray[13] = this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str = string.Concat(strArray);
          oleDbCommand2.CommandText = str;
        }
        else
        {
          estimationTable1 = EngineModel.getEstimationTable(inputDBCon, 3, Classifiers);
          OleDbCommand oleDbCommand3 = oleDbCommand1;
          string[] strArray = new string[14];
          strArray[0] = "select * from [";
          strArray[1] = estimationTable1;
          strArray[2] = "] where YearGUID={";
          strArray[3] = this.currYear.Guid.ToString();
          strArray[4] = "}  and EstimateType=";
          num4 = 5;
          strArray[5] = num4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray[6] = " and EquationType=";
          num4 = 1;
          strArray[7] = num4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray[8] = " and EstimateUnitsId=";
          strArray[9] = unit2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray[10] = " and [";
          classifiers1 = Classifiers.Strata;
          strArray[11] = classifiers1.ToString();
          strArray[12] = "] <> ";
          strArray[13] = this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str = string.Concat(strArray);
          oleDbCommand3.CommandText = str;
        }
        oleDbCommand1.Connection = inputDBCon;
        OleDbDataReader oleDbDataReader1 = oleDbCommand1.ExecuteReader();
        while (oleDbDataReader1.Read())
        {
          double num5;
          if (isForTree)
          {
            num5 = oleDbDataReader1.GetDouble(oleDbDataReader1.GetOrdinal("EstimateValue"));
          }
          else
          {
            Dictionary<int, Stratum> allStrata = this.allStrata;
            OleDbDataReader oleDbDataReader2 = oleDbDataReader1;
            OleDbDataReader oleDbDataReader3 = oleDbDataReader1;
            classifiers1 = Classifiers.Strata;
            string name = classifiers1.ToString();
            int ordinal = oleDbDataReader3.GetOrdinal(name);
            int int32 = oleDbDataReader2.GetInt32(ordinal);
            Stratum stratum = allStrata[int32];
            num3 += stratum.AreaHectareBasedEngineInventoryDB;
            num5 = oleDbDataReader1.GetDouble(oleDbDataReader1.GetOrdinal("EstimateValue")) * stratum.AreaHectareBasedEngineInventoryDB;
          }
          num2 += num5;
          Dictionary<int, double> dictionary2 = dictionary1;
          OleDbDataReader oleDbDataReader4 = oleDbDataReader1;
          OleDbDataReader oleDbDataReader5 = oleDbDataReader1;
          classifiers1 = Classifiers.Strata;
          string name1 = classifiers1.ToString();
          int ordinal1 = oleDbDataReader5.GetOrdinal(name1);
          int int32_1 = oleDbDataReader4.GetInt32(ordinal1);
          double num6 = num5;
          dictionary2.Add(int32_1, num6);
        }
        oleDbDataReader1.Close();
        OleDbCommand oleDbCommand4 = new OleDbCommand();
        oleDbCommand4.Connection = inputDBCon;
        OleDbCommand oleDbCommand5 = oleDbCommand4;
        string[] strArray1 = new string[13];
        strArray1[0] = "INSERT INTO [";
        strArray1[1] = estimationTable1;
        strArray1[2] = "] (YearGUID,[";
        classifiers1 = Classifiers.Strata;
        strArray1[3] = classifiers1.ToString();
        strArray1[4] = "],EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId) VALUES({";
        strArray1[5] = this.currYear.Guid.ToString();
        strArray1[6] = "},@st,";
        num4 = 34;
        strArray1[7] = num4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray1[8] = ",";
        num4 = 1;
        strArray1[9] = num4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray1[10] = ",@v,0,";
        strArray1[11] = unit3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray1[12] = ")";
        string str1 = string.Concat(strArray1);
        oleDbCommand5.CommandText = str1;
        oleDbCommand4.Parameters.Clear();
        oleDbCommand4.Parameters.Add("@st", OleDbType.Integer);
        oleDbCommand4.Parameters.Add("@v", OleDbType.Double);
        foreach (int key in dictionary1.Keys)
        {
          oleDbCommand4.Parameters["@st"].Value = (object) key;
          oleDbCommand4.Parameters["@v"].Value = (object) (dictionary1[key] / num2 * num1);
          oleDbCommand4.ExecuteNonQuery();
        }
        oleDbCommand4.Parameters["@st"].Value = (object) this.ClassValueForStrataStudyArea;
        oleDbCommand4.Parameters["@v"].Value = (object) num1;
        oleDbCommand4.ExecuteNonQuery();
        Classifiers.Clear();
        Classifiers.Add(3);
        Classifiers.Add(4);
        string estimationTable2;
        int num7;
        Classifiers classifiers2;
        if (isForTree)
        {
          estimationTable2 = EngineModel.getEstimationTable(inputDBCon, 2, Classifiers);
          OleDbCommand oleDbCommand6 = oleDbCommand1;
          string[] strArray2 = new string[18];
          strArray2[0] = "select * from [";
          strArray2[1] = estimationTable2;
          strArray2[2] = "] where YearGUID={";
          strArray2[3] = this.currYear.Guid.ToString();
          strArray2[4] = "}  and EstimateType=";
          num7 = 5;
          strArray2[5] = num7.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray2[6] = " and EquationType=";
          num7 = 1;
          strArray2[7] = num7.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray2[8] = " and EstimateUnitsId=";
          strArray2[9] = unit1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray2[10] = " and [";
          classifiers2 = Classifiers.Strata;
          strArray2[11] = classifiers2.ToString();
          strArray2[12] = "] <> ";
          strArray2[13] = this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray2[14] = " and [";
          classifiers2 = Classifiers.Species;
          strArray2[15] = classifiers2.ToString();
          strArray2[16] = "] <> ";
          strArray2[17] = this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str2 = string.Concat(strArray2);
          oleDbCommand6.CommandText = str2;
        }
        else
        {
          estimationTable2 = EngineModel.getEstimationTable(inputDBCon, 3, Classifiers);
          OleDbCommand oleDbCommand7 = oleDbCommand1;
          string[] strArray3 = new string[18];
          strArray3[0] = "select * from [";
          strArray3[1] = estimationTable2;
          strArray3[2] = "] where YearGUID={";
          strArray3[3] = this.currYear.Guid.ToString();
          strArray3[4] = "}  and EstimateType=";
          num7 = 5;
          strArray3[5] = num7.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray3[6] = " and EquationType=";
          num7 = 1;
          strArray3[7] = num7.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray3[8] = " and EstimateUnitsId=";
          strArray3[9] = unit2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray3[10] = " and [";
          classifiers2 = Classifiers.Strata;
          strArray3[11] = classifiers2.ToString();
          strArray3[12] = "] <> ";
          strArray3[13] = this.ClassValueForStrataStudyArea.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray3[14] = " and [";
          classifiers2 = Classifiers.Species;
          strArray3[15] = classifiers2.ToString();
          strArray3[16] = "] <> ";
          strArray3[17] = this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str3 = string.Concat(strArray3);
          oleDbCommand7.CommandText = str3;
        }
        oleDbCommand1.Connection = inputDBCon;
        OleDbDataReader oleDbDataReader6 = oleDbCommand1.ExecuteReader();
        OleDbDataReader oleDbDataReader7 = oleDbDataReader6;
        classifiers2 = Classifiers.Strata;
        string name2 = classifiers2.ToString();
        int ordinal2 = oleDbDataReader7.GetOrdinal(name2);
        OleDbDataReader oleDbDataReader8 = oleDbDataReader6;
        classifiers2 = Classifiers.Species;
        string name3 = classifiers2.ToString();
        int ordinal3 = oleDbDataReader8.GetOrdinal(name3);
        int ordinal4 = oleDbDataReader6.GetOrdinal("EstimateValue");
        OleDbCommand oleDbCommand8 = oleDbCommand4;
        string[] strArray4 = new string[15];
        strArray4[0] = "INSERT INTO [";
        strArray4[1] = estimationTable2;
        strArray4[2] = "] (YearGUID,[";
        classifiers2 = Classifiers.Strata;
        strArray4[3] = classifiers2.ToString();
        strArray4[4] = "],[";
        classifiers2 = Classifiers.Species;
        strArray4[5] = classifiers2.ToString();
        strArray4[6] = "],EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId) VALUES({";
        strArray4[7] = this.currYear.Guid.ToString();
        strArray4[8] = "},@st,@sp,";
        num7 = 34;
        strArray4[9] = num7.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray4[10] = ",";
        num7 = 1;
        strArray4[11] = num7.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray4[12] = ",@v,0,";
        strArray4[13] = unit3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray4[14] = ")";
        string str4 = string.Concat(strArray4);
        oleDbCommand8.CommandText = str4;
        oleDbCommand4.Parameters.Clear();
        oleDbCommand4.Parameters.Add("@st", OleDbType.Integer);
        oleDbCommand4.Parameters.Add("@sp", OleDbType.Integer);
        oleDbCommand4.Parameters.Add("@v", OleDbType.Double);
        while (oleDbDataReader6.Read())
        {
          int int32_2 = oleDbDataReader6.GetInt32(ordinal2);
          int int32_3 = oleDbDataReader6.GetInt32(ordinal3);
          double num8;
          if (isForTree)
          {
            num8 = oleDbDataReader6.GetDouble(ordinal4);
          }
          else
          {
            Stratum allStratum = this.allStrata[int32_2];
            num8 = oleDbDataReader6.GetDouble(ordinal4) * allStratum.AreaHectareBasedEngineInventoryDB;
          }
          oleDbCommand4.Parameters["@st"].Value = (object) int32_2;
          oleDbCommand4.Parameters["@sp"].Value = (object) int32_3;
          oleDbCommand4.Parameters["@v"].Value = (object) (num8 / num2 * num1);
          oleDbCommand4.ExecuteNonQuery();
        }
        oleDbDataReader6.Close();
        foreach (int key in dictionary1.Keys)
        {
          oleDbCommand4.Parameters["@st"].Value = (object) key;
          oleDbCommand4.Parameters["@sp"].Value = (object) this.ClassValueForSpeciesAllSpecies;
          oleDbCommand4.Parameters["@v"].Value = (object) (dictionary1[key] / num2 * num1);
          oleDbCommand4.ExecuteNonQuery();
        }
        oleDbCommand4.Parameters["@st"].Value = (object) this.ClassValueForStrataStudyArea;
        oleDbCommand4.Parameters["@sp"].Value = (object) this.ClassValueForSpeciesAllSpecies;
        oleDbCommand4.Parameters["@v"].Value = (object) num1;
        oleDbCommand4.ExecuteNonQuery();
        Classifiers.Clear();
        Classifiers.Add(4);
        string estimationTable3;
        int num9;
        Classifiers classifiers3;
        if (isForTree)
        {
          estimationTable3 = EngineModel.getEstimationTable(inputDBCon, 2, Classifiers);
          OleDbCommand oleDbCommand9 = oleDbCommand1;
          string[] strArray5 = new string[14];
          strArray5[0] = "select * from [";
          strArray5[1] = estimationTable3;
          strArray5[2] = "] where YearGUID={";
          strArray5[3] = this.currYear.Guid.ToString();
          strArray5[4] = "}  and EstimateType=";
          num9 = 5;
          strArray5[5] = num9.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray5[6] = " and EquationType=";
          num9 = 1;
          strArray5[7] = num9.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray5[8] = " and EstimateUnitsId=";
          strArray5[9] = unit1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray5[10] = " and [";
          classifiers3 = Classifiers.Species;
          strArray5[11] = classifiers3.ToString();
          strArray5[12] = "] <> ";
          strArray5[13] = this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str5 = string.Concat(strArray5);
          oleDbCommand9.CommandText = str5;
        }
        else
        {
          estimationTable3 = EngineModel.getEstimationTable(inputDBCon, 3, Classifiers);
          OleDbCommand oleDbCommand10 = oleDbCommand1;
          string[] strArray6 = new string[14];
          strArray6[0] = "select * from [";
          strArray6[1] = estimationTable3;
          strArray6[2] = "] where YearGUID={";
          strArray6[3] = this.currYear.Guid.ToString();
          strArray6[4] = "}  and EstimateType=";
          num9 = 5;
          strArray6[5] = num9.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray6[6] = " and EquationType=";
          num9 = 1;
          strArray6[7] = num9.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray6[8] = " and EstimateUnitsId=";
          strArray6[9] = unit2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          strArray6[10] = " and [";
          classifiers3 = Classifiers.Species;
          strArray6[11] = classifiers3.ToString();
          strArray6[12] = "] <> ";
          strArray6[13] = this.ClassValueForSpeciesAllSpecies.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str6 = string.Concat(strArray6);
          oleDbCommand10.CommandText = str6;
        }
        oleDbCommand1.Connection = inputDBCon;
        OleDbDataReader oleDbDataReader9 = oleDbCommand1.ExecuteReader();
        OleDbDataReader oleDbDataReader10 = oleDbDataReader9;
        classifiers3 = Classifiers.Species;
        string name4 = classifiers3.ToString();
        int ordinal5 = oleDbDataReader10.GetOrdinal(name4);
        int ordinal6 = oleDbDataReader9.GetOrdinal("EstimateValue");
        OleDbCommand oleDbCommand11 = oleDbCommand4;
        string[] strArray7 = new string[13];
        strArray7[0] = "INSERT INTO [";
        strArray7[1] = estimationTable3;
        strArray7[2] = "] (YearGUID,[";
        classifiers3 = Classifiers.Species;
        strArray7[3] = classifiers3.ToString();
        strArray7[4] = "],EstimateType,EquationType,EstimateValue,EstimateStandardError,EstimateUnitsId) VALUES({";
        strArray7[5] = this.currYear.Guid.ToString();
        strArray7[6] = "},@sp,";
        num9 = 34;
        strArray7[7] = num9.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray7[8] = ",";
        num9 = 1;
        strArray7[9] = num9.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray7[10] = ",@v,0,";
        strArray7[11] = unit3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        strArray7[12] = ")";
        string str7 = string.Concat(strArray7);
        oleDbCommand11.CommandText = str7;
        oleDbCommand4.Parameters.Clear();
        oleDbCommand4.Parameters.Add("@sp", OleDbType.Integer);
        oleDbCommand4.Parameters.Add("@v", OleDbType.Double);
        while (oleDbDataReader9.Read())
        {
          int int32 = oleDbDataReader9.GetInt32(ordinal5);
          double num10 = !isForTree ? oleDbDataReader9.GetDouble(ordinal6) * num3 : oleDbDataReader9.GetDouble(ordinal6);
          oleDbCommand4.Parameters["@sp"].Value = (object) int32;
          oleDbCommand4.Parameters["@v"].Value = (object) (num10 / num2 * num1);
          oleDbCommand4.ExecuteNonQuery();
        }
        oleDbDataReader9.Close();
        oleDbCommand4.Parameters["@sp"].Value = (object) this.ClassValueForSpeciesAllSpecies;
        oleDbCommand4.Parameters["@v"].Value = (object) num1;
        oleDbCommand4.ExecuteNonQuery();
        if (!isForTree)
          return;
        oleDbCommand1.CommandText = "select PlotId,TreeId,LeafArea from IndividualTreeEffects where YearGUID={" + this.currYear.Guid.ToString() + "}";
        OleDbDataReader oleDbDataReader11 = oleDbCommand1.ExecuteReader();
        int ordinal7 = oleDbDataReader11.GetOrdinal("PlotId");
        int ordinal8 = oleDbDataReader11.GetOrdinal("TreeId");
        int ordinal9 = oleDbDataReader11.GetOrdinal("LeafArea");
        oleDbCommand4.CommandText = "UPDATE IndividualTreeEffects SET  WaterInterception = @i  WHERE YearGuid={" + this.currYear.Guid.ToString() + "} AND PlotId=@plotid AND TreeId=@treeid";
        oleDbCommand4.Parameters.Clear();
        oleDbCommand4.Parameters.Add("@i", OleDbType.Double);
        oleDbCommand4.Parameters.Add("@plotid", OleDbType.Integer);
        oleDbCommand4.Parameters.Add("@treeid", OleDbType.Integer);
        while (oleDbDataReader11.Read())
        {
          oleDbCommand4.Parameters["@i"].Value = (object) (oleDbDataReader11.GetDouble(ordinal9) / 1000000.0 * num1 / num2);
          oleDbCommand4.Parameters["@plotid"].Value = (object) oleDbDataReader11.GetInt32(ordinal7);
          oleDbCommand4.Parameters["@treeid"].Value = (object) oleDbDataReader11.GetInt32(ordinal8);
          oleDbCommand4.ExecuteNonQuery();
        }
        oleDbDataReader11.Close();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void PopulatingWaterInterceptionForCombiedTreeShrub(
      ProgramSession m_ps,
      OleDbConnection inputDBCon)
    {
      try
      {
        int unit = EngineModel.getUnit(inputDBCon, 37, 1, 28);
        List<int> classifierList = new List<int>();
        classifierList.Add(3);
        List<string> classifierNameList = new List<string>();
        classifierNameList.Add(Classifiers.Strata.ToString());
        this.CombinedTreeShrubData(inputDBCon, this.currYear.Guid, classifierList, classifierNameList, 34, unit, 1);
        classifierList.Clear();
        classifierList.Add(3);
        classifierList.Add(4);
        classifierNameList.Clear();
        classifierNameList.Add(Classifiers.Strata.ToString());
        classifierNameList.Add(Classifiers.Species.ToString());
        this.CombinedTreeShrubData(inputDBCon, this.currYear.Guid, classifierList, classifierNameList, 34, unit, 1);
        classifierList.Clear();
        classifierList.Add(4);
        classifierNameList.Clear();
        classifierNameList.Add(Classifiers.Species.ToString());
        this.CombinedTreeShrubData(inputDBCon, this.currYear.Guid, classifierList, classifierNameList, 34, unit, 1);
      }
      catch (Exception ex)
      {
        throw;
      }
    }
  }
}
