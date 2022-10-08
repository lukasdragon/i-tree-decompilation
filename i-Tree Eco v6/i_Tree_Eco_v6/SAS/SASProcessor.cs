// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.SAS.SASProcessor
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Zip;
using DaveyTree.NHibernate;
using DaveyTree.NHibernate.Extensions;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Convert;
using Eco.Util.Queries.Interfaces;
using Eco.Util.Views;
using i_Tree_Eco_v6.Energy;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Forms;
using i_Tree_Eco_v6.Properties;
using LocationSpecies.Domain;
using mdblib;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using TreeEnergy;

namespace i_Tree_Eco_v6.SAS
{
  internal class SASProcessor : IDisposable
  {
    private ProgramSession m_ps;
    private ISession project_s;
    public Year currYear;
    public Series currSeries;
    public Project currProject;
    private int LocationID;
    public ProjectLocation currProjectLocation;
    public YearLocationData currYearLocation;
    private bool TreatFullInventoryAsSampled = true;
    private Regex StrataRGX = new Regex("[^a-zA-Z0-9 _]");
    public string outputFolder = "";
    public string workingInputDatabaseName = "";
    public string zipFileToSend = "";
    public string zipFileReceived = "";
    public string userEmail = "";
    private SASDictionary<string, int> dictStrataNameToClassValue = new SASDictionary<string, int>("Strata Description");
    private SASDictionary<string, int> dictDBHtoClassValue = new SASDictionary<string, int>("DBH");
    private SASDictionary<string, int> dictCDBHidToClassValue = new SASDictionary<string, int>("CDBH_ID");
    private SASDictionary<string, int> dictDiebackToClassValue = new SASDictionary<string, int>("Dieback");
    private SASDictionary<string, int> dictCDiebackIdToClassValue = new SASDictionary<string, int>("CDieback_ID");
    private SASDictionary<string, int> dictContinentToClassValue = new SASDictionary<string, int>("Continent");
    private SASDictionary<string, int> dictPrimaryIndexToClassValue = new SASDictionary<string, int>("Primary Index");
    private SASDictionary<string, int> dictFieldLanduseToClassValue = new SASDictionary<string, int>("Landuse");
    private SASDictionary<string, int> dictSpeciesCodeToClassValue = new SASDictionary<string, int>("Species Code");
    private SASDictionary<string, int> dictEnergyUseToClassValue = new SASDictionary<string, int>("Energy Use");
    private SASDictionary<string, int> dictEnergyTypeToClassValue = new SASDictionary<string, int>("Energy Type");
    private SASDictionary<string, int> dictPollutantToClassValue = new SASDictionary<string, int>("Pollutant");
    private SASDictionary<string, int> dictVOCToClassValue = new SASDictionary<string, int>("VOC");
    private SASDictionary<string, int> dictMonthToClassValue = new SASDictionary<string, int>("Month");
    private SASDictionary<string, int> dictHourToClassValue = new SASDictionary<string, int>("Hour");
    private SASDictionary<string, int> dictPestAndPestSymptomClassIDandSymptomIDtoClassValue = new SASDictionary<string, int>("Pest");
    private SASDictionary<int, string> dictStrataClassValueToName = new SASDictionary<int, string>("Strata class value to Description");
    private SASDictionary<int, string> dictSpeciesClassValueToCode = new SASDictionary<int, string>("Species class value to code");
    private SASDictionary<int, string> dictSpeciesClassValueToGenusCode = new SASDictionary<int, string>("Species Class Value to Genus Code");
    private SASDictionary<int, int> dictSpeciesClassValueToEvergreen = new SASDictionary<int, int>("Species Class Value");
    private SASDictionary<string, string> dictGenusCodePlusSpeciesCodeToSpeciesCode = new SASDictionary<string, string>("Genus + Species Code to Species Code");
    private SASDictionary<string, double> dictStrataNameToDensityRatio = new SASDictionary<string, double>("Strata Description");
    private SASDictionary<string, double> dictStrataNameToCoverPercentRatio = new SASDictionary<string, double>("Strata Description");
    private double TotalStudyAreaHectare;
    private List<int> listDiebackClassValue = new List<int>();
    private List<int> listContinentClassValue = new List<int>();
    private List<int> listPrimaryIndexClassValue = new List<int>();
    private List<int> listFieldLanduseClassValue = new List<int>();
    private SASDictionary<int, float> dictStrataSizeByUserEntered = new SASDictionary<int, float>("Strata Size");
    private int PestNoneClassValue;
    private int PestUnknownClassValue;
    private int PestAffectedClassValue;
    private string tempSppGenusTableName = "tmpSppNameGenusTable";
    private SASDictionary<int, StratumSAS> dictStrataSASInfo = new SASDictionary<int, StratumSAS>("Strata Class Value");
    private SASDictionary<string, StratumGenusSAS> dictStratumGenusSASInfo = new SASDictionary<string, StratumGenusSAS>("Strata Class Value and Genus Scientific Name");
    private int ClassValueForStrataStudyArea;
    private int ClassValueForSpeciesAllSpecies;
    private int ClassValueForTreeOfGroundCover;
    private int ClassValueForShrubOfGroundCover;
    private int ClassValueForPlantableSpaceOfGroundCover;
    private bool HasSpeciesTotalInCITYDEN_CSV;
    private bool HasSpeciesTotalInCITYCND_CSV;
    private bool HasSpeciesTotalInCITYCND2_CSV;
    private bool HasSpeciesTotalInCITYDBH_CSV;
    private bool HasSpeciesTotalInCITYDBH2_CSV;
    private bool HasSpeciesTotalInCITYSUM_CSV;
    private NumberStyles nsFloat = NumberStyles.Float;
    private NumberStyles nsInteger = NumberStyles.Integer;
    private CultureInfo ciUsed = CultureInfo.InvariantCulture;
    private double thousand = 1000.0;
    private double tenThousand = 10000.0;
    private double million = 1000000.0;
    private bool SASResultContainTree = true;
    private string ClauseOfNonRemovedTrees = string.Format("(EcoTrees.TreeStatus IS null OR (EcoTrees.TreeStatus NOT IN ('{0}', '{1}', '{2}', '{3}')))", (object) 'H', (object) 'C', (object) 'L', (object) 'R');
    private static char[] TreeRemoveStatus = new char[4]
    {
      'H',
      'C',
      'L',
      'R'
    };
    private char[] AllTreeStatus = new char[Enum.GetValues(typeof (TreeStatus)).Length];

    public void prepareToStart(ProgramSession passin_m_ps, Form comingForm)
    {
      try
      {
        int num = 0;
        foreach (TreeStatus treeStatus in Enum.GetValues(typeof (TreeStatus)))
          this.AllTreeStatus[num++] = (char) treeStatus;
        this.m_ps = passin_m_ps;
        this.project_s = this.m_ps.InputSession.CreateSession();
        this.currYear = this.project_s.Get<Year>((object) this.m_ps.InputSession.YearKey);
        this.currSeries = this.currYear.Series;
        if (this.currSeries.SampleType != SampleType.Inventory && !this.currYear.RecordStrata)
        {
          using (ITransaction transaction = this.project_s.BeginTransaction())
          {
            this.currYear.RecordStrata = true;
            this.project_s.SaveOrUpdate((object) this.currYear);
            transaction.Commit();
          }
          EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(this.currYear), (Control) comingForm);
        }
        this.currProject = this.currSeries.Project;
        this.LocationID = this.currProject.LocationId;
        this.currProjectLocation = this.currProject.Locations.Where<ProjectLocation>((Func<ProjectLocation, bool>) (p => p.LocationId == this.LocationID)).First<ProjectLocation>();
        this.currYearLocation = this.currYear.YearLocationData.Where<YearLocationData>((Func<YearLocationData, bool>) (p => p.ProjectLocation.Equals((object) this.currProjectLocation))).First<YearLocationData>();
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\iTree\\tempModel";
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        this.outputFolder = path + "\\" + this.currYear.Guid.ToString() + DateTime.Now.TimeOfDay.TotalSeconds.ToString("#0", (IFormatProvider) this.ciUsed);
        if (!Directory.Exists(this.outputFolder))
        {
          Directory.CreateDirectory(this.outputFolder);
        }
        else
        {
          foreach (FileSystemInfo file in new DirectoryInfo(this.outputFolder).GetFiles())
            file.Delete();
        }
        this.workingInputDatabaseName = Path.Combine(this.outputFolder, "SAS_" + this.ToUSCharacters(this.ToASCII(Path.GetFileName(this.m_ps.InputSession.InputDb))));
        System.IO.File.Copy(this.m_ps.InputSession.InputDb, this.workingInputDatabaseName, true);
        this.zipFileToSend = Path.Combine(this.outputFolder, "_" + this.currProject.NationCode + "_" + this.currProject.PrimaryPartitionCode + "_" + this.currProject.SecondaryPartitionCode + "_" + this.currProject.TertiaryPartitionCode + "_" + this.ToUSCharacters(this.ToASCII(Path.GetFileName(this.m_ps.InputSession.InputDb)))) + "." + DateTime.Now.Year.ToString((IFormatProvider) this.ciUsed) + "_" + DateTime.Now.Month.ToString((IFormatProvider) this.ciUsed) + "_" + DateTime.Now.Day.ToString((IFormatProvider) this.ciUsed) + "_" + DateTime.Now.TimeOfDay.TotalSeconds.ToString("#0", (IFormatProvider) this.ciUsed) + ".zip";
        this.zipFileReceived = Path.Combine(this.outputFolder, "resultFromSAS.zip");
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private double getDiameter(double stemTableDiameterFieldValue)
    {
      if (this.currYear.DBHActual)
        return stemTableDiameterFieldValue;
      foreach (DBH dbH in (IEnumerable<DBH>) this.currYear.DBHs)
      {
        if (dbH.DBHId == stemTableDiameterFieldValue)
          return dbH.Value;
      }
      throw new Exception("DBH id " + stemTableDiameterFieldValue.ToString("#0", (IFormatProvider) this.ciUsed) + " is not valid.");
    }

    public bool Export(
      bool needValidate,
      IProgress<SASProgressArg> uploadProgress,
      CancellationToken uploadCancellationToken,
      SASProgressArg uploadProgressArg,
      int progressFromRange,
      int progressToRange)
    {
      int num1 = 16;
      int num2 = 0;
      string fatalError = "";
      string warningError = "";
      if (needValidate)
      {
        Dictionary<string, string> treeSynonymCultivar = new Dictionary<string, string>();
        Dictionary<string, string> shrubSynonymCultivar = new Dictionary<string, string>();
        Dictionary<string, string> mortalityTreeSynonymCultivar = new Dictionary<string, string>();
        if (!this.DataValidationOfSynonymOrCultivar(false, treeSynonymCultivar, shrubSynonymCultivar, mortalityTreeSynonymCultivar))
        {
          SynonymOrCultivarForm synonymOrCultivarForm = new SynonymOrCultivarForm();
          synonymOrCultivarForm.InitializeForm(this.m_ps, false, treeSynonymCultivar, shrubSynonymCultivar, mortalityTreeSynonymCultivar);
          int num3 = (int) synonymOrCultivarForm.ShowDialog();
        }
        if (!this.DataValidation(this.workingInputDatabaseName, ref fatalError, ref warningError))
        {
          LoggedErrorForm loggedErrorForm = new LoggedErrorForm();
          string fromLoggedFileOrErrorMessage = SASResources.HdrFatalErrors + "\r\n\r\n" + fatalError;
          if (warningError.Length > 0)
            fromLoggedFileOrErrorMessage = fromLoggedFileOrErrorMessage + "\r\n\r\n" + SASResources.HdrWarnings + "\r\n\r\n" + warningError;
          loggedErrorForm.initializeForm(fromLoggedFileOrErrorMessage, false);
          int num4 = (int) loggedErrorForm.ShowDialog();
          return false;
        }
      }
      if (uploadCancellationToken.IsCancellationRequested)
        return false;
      int num5 = num2 + 1;
      uploadProgressArg.Percent = (int) ((double) num5 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
      uploadProgress.Report(uploadProgressArg);
      if (!this.ModifyDatabaseForSubmission(uploadProgress, uploadCancellationToken, uploadProgressArg, 0, 100) || uploadCancellationToken.IsCancellationRequested)
        return false;
      int num6 = num5 + 1;
      uploadProgressArg.Description = SASResources.MsgProcessing;
      uploadProgressArg.Percent = (int) ((double) num6 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
      uploadProgress.Report(uploadProgressArg);
      string aStr1 = "StudyArea";
      string aStr2 = "00";
      double num7 = 0.0;
      double num8 = 0.0;
      double num9 = 0.0;
      int num10 = 153;
      int num11 = 0;
      int num12 = 0;
      double num13 = 0.0;
      double num14 = 0.0;
      double num15 = 0.0;
      double num16 = 0.0;
      double num17 = 0.0;
      double num18 = 0.0;
      double num19 = 0.0;
      ClimateRegion climateRegion = (ClimateRegion) null;
      int num20 = 0;
      SortedDictionary<Month, double> sortedDictionary1 = new SortedDictionary<Month, double>();
      SortedDictionary<Month, double> sortedDictionary2 = new SortedDictionary<Month, double>();
      IList<FieldLandUse> fieldLandUseList = (IList<FieldLandUse>) new List<FieldLandUse>();
      try
      {
        using (ISession session = this.m_ps.LocSp.OpenSession())
        {
          using (session.BeginTransaction())
          {
            Location location = session.QueryOver<Location>().Fetch<Location, Location>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<Location, object>>) (loc => loc.GrowthPeriod)).Fetch<Location, Location>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<Location, object>>) (loc => loc.AlbedoStation)).Where((System.Linq.Expressions.Expression<Func<Location, bool>>) (loc => loc.Id == this.currProject.LocationId)).TransformUsing((IResultTransformer) new DistinctRootEntityResultTransformer()).Cacheable().SingleOrDefault();
            if (location != null)
            {
              GrowthPeriod growthPeriod = location.GrowthPeriod;
              AlbedoStation albedoStation = location.AlbedoStation;
              num7 = location.Latitude;
              num8 = location.Longitude;
              num13 = location.Elevation;
              num20 = location.TropicalClimate.HasValue ? (int) location.TropicalClimate.Value : 0;
              if (growthPeriod != null)
              {
                num10 = (int) growthPeriod.FrostFreeDays;
                num12 = (int) growthPeriod.LeafOffDays;
                num11 = (int) growthPeriod.LeafOnDays;
              }
              if (albedoStation != null)
              {
                num14 = albedoStation.Latitude;
                num15 = albedoStation.Longitude;
                num16 = albedoStation.CoefficientA;
                num17 = albedoStation.CoefficientPhi;
                num18 = albedoStation.CoefficientC;
                num19 = albedoStation.TerrainFactor;
                sortedDictionary2 = new SortedDictionary<Month, double>(albedoStation.Albedo.Values);
              }
              Location loc = location;
              StateLat stateLat = (StateLat) null;
              string str = (string) null;
              UTCTimeZone utcTimeZone = (UTCTimeZone) null;
              LocationRelation locationRelation;
              for (; loc != null; loc = locationRelation.Parent)
              {
                if (stateLat == null)
                {
                  NHibernateUtil.Initialize((object) loc.StateLat);
                  stateLat = loc.StateLat;
                }
                if (climateRegion == null)
                {
                  NHibernateUtil.Initialize((object) loc.ClimateRegion);
                  climateRegion = loc.ClimateRegion;
                }
                if (str == null)
                  str = loc.Abbreviation;
                if (utcTimeZone == null)
                {
                  NHibernateUtil.Initialize((object) loc.TimeZone);
                  utcTimeZone = loc.TimeZone;
                }
                if (stateLat == null || climateRegion == null || str == null || utcTimeZone == null)
                {
                  locationRelation = session.QueryOver<LocationRelation>().Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Location == loc)).Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Code != default (string))).Fetch<LocationRelation, LocationRelation>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<LocationRelation, object>>) (r => r.Parent)).Cacheable().SingleOrDefault();
                  if (locationRelation == null)
                    break;
                }
                else
                  break;
              }
              if (stateLat != null)
                sortedDictionary1 = new SortedDictionary<Month, double>(stateLat.OzoneValues);
              if (str != null)
                aStr2 = str;
              if (utcTimeZone != null)
                num9 = (double) utcTimeZone.Offset;
            }
            fieldLandUseList = session.QueryOver<FieldLandUse>().Fetch<FieldLandUse, FieldLandUse>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<FieldLandUse, object>>) (flu => flu.Decompositions)).OrderBy((System.Linq.Expressions.Expression<Func<FieldLandUse, object>>) (flu => (object) flu.Id)).Asc.TransformUsing((IResultTransformer) new DistinctRootEntityResultTransformer()).List();
          }
        }
        int num21 = num6 + 1;
        uploadProgressArg.Percent = (int) ((double) num21 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
        uploadProgress.Report(uploadProgressArg);
        if (uploadCancellationToken.IsCancellationRequested)
          return false;
        using (InputSession inputSession = new InputSession(this.workingInputDatabaseName))
        {
          inputSession.YearKey = new Guid?(this.m_ps.InputSession.YearKey.Value);
          ISession session = inputSession.CreateSession();
          using (StreamWriter streamWriter = new StreamWriter(this.outputFolder + "\\ReOrderIDs.txt"))
          {
            bool flag = false;
            if (this.currSeries.SampleType == SampleType.Inventory)
            {
              if (session.QueryOver<Tree>().JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear)).Select((IProjection) Projections.Group<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Id)), (IProjection) Projections.Count<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Id))).OrderBy((IProjection) Projections.Count<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Id))).Desc<Tree, Plot>().TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int)>()).Take(1).SingleOrDefault<(int, int)>().Item2 > 1)
                flag = true;
            }
            else
              flag = true;
            streamWriter.WriteLine("TreeShrubKey,TorS,PlotID,OldTreeShrubID,NewTreeShrubID");
            if (flag)
            {
              int val = Math.Max(session.QueryOver<Tree>().Select((IProjection) Projections.Max<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Id))).SingleOrDefault<int>(), session.QueryOver<Shrub>().Select((IProjection) Projections.Max<Shrub>((System.Linq.Expressions.Expression<Func<Shrub, object>>) (s => (object) s.Id))).SingleOrDefault<int>()) + 1;
              try
              {
                session.GetNamedQuery("dropTableReOrderIDTable").ExecuteUpdate();
              }
              catch (Exception ex)
              {
              }
              session.GetNamedQuery("createTableReOrderIDTable").ExecuteUpdate();
              session.GetNamedQuery("populateTreeIDsInReOrderIDTable").SetGuid("yearGuid", this.currYear.Guid).ExecuteUpdate();
              session.GetNamedQuery("updateTreeIDsInEcoTrees").SetInt32("maxId", val).ExecuteUpdate();
              session.GetNamedQuery("reduceMaxIdOfTreeIDsInEcoTrees").SetInt32("maxId", val).SetGuid("yearGuid", this.currYear.Guid).ExecuteUpdate();
              if (this.currYear.RecordShrub && this.currSeries.SampleType != SampleType.Inventory)
              {
                session.GetNamedQuery("populateShrubIDsInReOrderIDTable").SetGuid("yearGuid", this.currYear.Guid).ExecuteUpdate();
                session.GetNamedQuery("updateShrubIDsInEcoTrees").SetInt32("maxId", val).ExecuteUpdate();
                session.GetNamedQuery("reduceMaxIdOfShrubIDsInEcoTrees").SetInt32("maxId", val).SetGuid("yearGuid", this.currYear.Guid).ExecuteUpdate();
              }
              foreach ((Guid, int, int, int, int) tuple in (IEnumerable<(Guid, int, int, int, int)>) session.GetNamedQuery("selectAllFromReOrderIDTable").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(Guid, int, int, int, int)>()).List<(Guid, int, int, int, int)>())
                streamWriter.WriteLine(tuple.Item1.ToString() + "," + (tuple.Item5 == 1 ? "T" : "S") + "," + tuple.Item2.ToString((IFormatProvider) this.ciUsed) + "," + tuple.Item3.ToString((IFormatProvider) this.ciUsed) + "," + tuple.Item4.ToString((IFormatProvider) this.ciUsed));
              streamWriter.Close();
            }
          }
          session.Close();
          inputSession.Close();
        }
        int num22 = num21 + 1;
        uploadProgressArg.Percent = (int) ((double) num22 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
        uploadProgress.Report(uploadProgressArg);
        if (uploadCancellationToken.IsCancellationRequested)
          return false;
        if (num10 < 0)
          num10 = 153;
        int num23 = num22 + 1;
        uploadProgressArg.Percent = (int) ((double) num23 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
        uploadProgress.Report(uploadProgressArg);
        if (uploadCancellationToken.IsCancellationRequested)
          return false;
        int num24 = num23 + 1;
        uploadProgressArg.Percent = (int) ((double) num24 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
        uploadProgress.Report(uploadProgressArg);
        if (uploadCancellationToken.IsCancellationRequested)
          return false;
        using (InputSession inputSession = new InputSession(this.workingInputDatabaseName))
        {
          inputSession.YearKey = new Guid?(this.m_ps.InputSession.YearKey.Value);
          ISession session = inputSession.CreateSession();
          using (StreamWriter streamWriter = new StreamWriter(this.outputFolder + "\\u4Fdlu.txt"))
          {
            IList<LandUse> landUseList = session.QueryOver<LandUse>().Where((System.Linq.Expressions.Expression<Func<LandUse, bool>>) (ls => ls.Year == this.currYear)).List();
            string str1 = "8";
            foreach (LandUse landUse in (IEnumerable<LandUse>) landUseList)
              str1 = str1 + "," + landUse.LandUseId.ToString((IFormatProvider) this.ciUsed);
            SASDictionary<int, string> sasDictionary1 = new SASDictionary<int, string>("Landuse ID");
            foreach (FieldLandUse fieldLandUse in (IEnumerable<FieldLandUse>) fieldLandUseList)
            {
              if (uploadCancellationToken.IsCancellationRequested)
                return false;
              string str2 = this.ReturnFixedLengthString("0", 7, 0) + this.ReturnFixedLengthString("100", 7, 0) + this.ReturnFixedLengthString((0.33 * (double) num10 / 153.0).ToString("#0.00", (IFormatProvider) this.ciUsed), 7, 0);
              string str3 = "";
              float num25;
              foreach (Decomposition decomposition in (IEnumerable<Decomposition>) fieldLandUse.Decompositions)
              {
                string[] strArray = new string[5]
                {
                  str3,
                  " ",
                  null,
                  null,
                  null
                };
                num25 = decomposition.Proportion;
                strArray[2] = this.ReturnFixedLengthString(num25.ToString("#0.0", (IFormatProvider) this.ciUsed), 5, 0);
                strArray[3] = "  ";
                strArray[4] = this.ReturnFixedLengthString(decomposition.Years.ToString((IFormatProvider) this.ciUsed), 10, -1);
                str3 = string.Concat(strArray);
              }
              SASDictionary<int, string> sasDictionary2 = sasDictionary1;
              int id = fieldLandUse.Id;
              string[] strArray1 = new string[10];
              strArray1[0] = "  ";
              num25 = fieldLandUse.BiomassAdjustment;
              strArray1[1] = this.ReturnFixedLengthString(num25.ToString("#0.00", (IFormatProvider) this.ciUsed), 5, -1);
              strArray1[2] = "   ";
              num25 = fieldLandUse.LocationValueFactor * 100f;
              strArray1[3] = this.ReturnFixedLengthString(num25.ToString("##0", (IFormatProvider) this.ciUsed), 4, 0);
              strArray1[4] = "    ";
              strArray1[5] = this.ReturnFixedLengthString(fieldLandUse.Decompositions.Count.ToString((IFormatProvider) this.ciUsed), 3, 0);
              strArray1[6] = "    ";
              strArray1[7] = this.ReturnFixedLengthString("1", 3, 0);
              strArray1[8] = str3.PadRight(36).PadLeft(38);
              strArray1[9] = str2;
              string str4 = string.Concat(strArray1);
              sasDictionary2.Add(id, str4);
            }
            streamWriter.WriteLine("The field landuse codes, descriptions, biomass adjustment indicator, decomposition parameters and growth rates (in/yr).");
            streamWriter.WriteLine("LANDUSDESCRIP             BIO ADJ LOC FAC DM CNT GW CNT DM PRO1 DM YRS1   DM PRO2 DM YRS2   LOWER1 UPPER1");
            if (this.currYear.RecordLanduse)
            {
              foreach (LandUse landUse in (IEnumerable<LandUse>) landUseList)
                streamWriter.WriteLine(this.ReturnFixedLengthString(landUse.Id.ToString((IFormatProvider) this.ciUsed), 6, -1) + this.ReturnFixedLengthString(landUse.Description, 20, -1) + sasDictionary1[landUse.LandUseId]);
            }
            else
              streamWriter.WriteLine(this.ReturnFixedLengthString("C", 6, -1) + this.ReturnFixedLengthString("Commercial", 20, -1) + sasDictionary1[8]);
          }
          int num26 = num24 + 1;
          uploadProgressArg.Percent = (int) ((double) num26 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
          uploadProgress.Report(uploadProgressArg);
          if (uploadCancellationToken.IsCancellationRequested)
            return false;
          using (StreamWriter streamWriter = new StreamWriter(this.outputFolder + "\\city18a.prn"))
          {
            string str5 = this.ReturnFixedLengthString(aStr1, 20, -1) + "    " + this.ReturnFixedLengthString(aStr2, 3, 1) + " " + this.ReturnFixedLengthString((-num9).ToString((IFormatProvider) this.ciUsed), 5, -1) + this.ReturnFixedLengthString(num14.ToString("#0.00", (IFormatProvider) this.ciUsed), 8, 1) + " " + this.ReturnFixedLengthString((-num15).ToString("#0.00", (IFormatProvider) this.ciUsed), 8, 1) + " " + this.ReturnFixedLengthString(num13.ToString((IFormatProvider) this.ciUsed), 5, 1) + this.ReturnFixedLengthString(num16.ToString("#0.000", (IFormatProvider) this.ciUsed), 7, 1) + this.ReturnFixedLengthString(((int) num17).ToString((IFormatProvider) this.ciUsed), 5, 1) + this.ReturnFixedLengthString(num18.ToString("#0.00", (IFormatProvider) this.ciUsed), 6, 1) + this.ReturnFixedLengthString(num19.ToString("#0.00", (IFormatProvider) this.ciUsed), 6, 1);
            foreach (Month key in Enum.GetValues(typeof (Month)))
            {
              string aStr3 = "0";
              if (sortedDictionary1.ContainsKey(key))
                aStr3 = sortedDictionary1[key].ToString("0.00", (IFormatProvider) this.ciUsed);
              str5 = key == Month.January || key == Month.February || key == Month.November || key == Month.December ? str5 + this.ReturnFixedLengthString(aStr3, 6, 1) : str5 + this.ReturnFixedLengthString(aStr3, 5, 1);
            }
            if (uploadCancellationToken.IsCancellationRequested)
              return false;
            foreach (Month key in Enum.GetValues(typeof (Month)))
            {
              string aStr4 = "0";
              if (sortedDictionary2.ContainsKey(key))
                aStr4 = sortedDictionary2[key].ToString("0.00", (IFormatProvider) this.ciUsed);
              str5 = key == Month.July ? str5 + this.ReturnFixedLengthString(aStr4, 5, 1) : str5 + this.ReturnFixedLengthString(aStr4, 6, 1);
            }
            string str6 = str5 + this.ReturnFixedLengthString(num11.ToString((IFormatProvider) this.ciUsed), 5, 1) + this.ReturnFixedLengthString(num12.ToString((IFormatProvider) this.ciUsed), 5, 1) + this.ReturnFixedLengthString("111111111", 12, 1) + this.ReturnFixedLengthString("100", 6, 1);
            streamWriter.WriteLine(str6);
          }
          int num27 = num26 + 1;
          uploadProgressArg.Percent = (int) ((double) num27 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
          uploadProgress.Report(uploadProgressArg);
          if (uploadCancellationToken.IsCancellationRequested)
            return false;
          using (StreamWriter streamWriter = new StreamWriter(this.outputFolder + "\\enrgmap_new.txt"))
          {
            string str = "";
            if (climateRegion != null)
              str = climateRegion.Name.Replace(" ", "").Replace("-", "");
            streamWriter.WriteLine("This file maps U4ACE cities to the cities with available energy data.");
            streamWriter.WriteLine("U4ACE CITY            U4ACE STATE    REGION");
            streamWriter.WriteLine(this.ReturnFixedLengthString(aStr1, 20, -1) + "  " + this.ReturnFixedLengthString(aStr2, 15, -1) + str.ToUpper(this.ciUsed));
          }
          int num28 = num27 + 1;
          uploadProgressArg.Percent = (int) ((double) num28 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
          uploadProgress.Report(uploadProgressArg);
          if (uploadCancellationToken.IsCancellationRequested)
            return false;
          int num29 = num28 + 1;
          uploadProgressArg.Percent = (int) ((double) num29 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
          uploadProgress.Report(uploadProgressArg);
          if (uploadCancellationToken.IsCancellationRequested)
            return false;
          Plot plot = (Plot) null;
          Strata strata = (Strata) null;
          using (StreamWriter streamWriter = new StreamWriter(this.outputFolder + "\\oplts.txt"))
          {
            IList<(int, int, string)> tupleList = session.QueryOver<Plot>((System.Linq.Expressions.Expression<Func<Plot>>) (() => plot)).JoinQueryOver<Strata>((System.Linq.Expressions.Expression<Func<Plot, Strata>>) (p => p.Strata), (System.Linq.Expressions.Expression<Func<Strata>>) (() => strata)).Where((System.Linq.Expressions.Expression<Func<bool>>) (() => plot.Year == this.currYear && plot.IsComplete == true)).Select((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plot.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) strata.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => strata.Abbreviation))).OrderBy((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plot.Id))).Asc.TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int, string)>()).List<(int, int, string)>();
            streamWriter.WriteLine("");
            streamWriter.WriteLine("Plot No       Original Landuse");
            foreach ((int, int, string) tuple in (IEnumerable<(int, int, string)>) tupleList)
              streamWriter.WriteLine(this.ReturnFixedLengthString(tuple.Item1.ToString((IFormatProvider) this.ciUsed), 13, -1) + " " + this.ReturnFixedLengthString(tuple.Item2.ToString(), 16, 0));
            streamWriter.Close();
          }
          using (StreamWriter streamWriter1 = new StreamWriter(this.outputFolder + "\\tot.txt"))
          {
            using (StreamWriter streamWriter2 = new StreamWriter(this.outputFolder + "\\tot.txt.userentered"))
            {
              IList<Guid> source = session.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true)).Select(Projections.Distinct((IProjection) Projections.Property<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Strata.Guid)))).List<Guid>();
              IList<Strata> strataList = session.QueryOver<Strata>().WhereRestrictionOn((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => (object) s.Guid)).IsIn((ICollection) source.ToArray<Guid>()).OrderBy((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => (object) s.Id)).Asc.List<Strata>();
              Dictionary<string, int> dictionary = new Dictionary<string, int>();
              if (this.currYear.Unit == YearUnit.English)
              {
                streamWriter1.WriteLine("ENGLISH");
                streamWriter2.WriteLine("ENGLISH");
              }
              else
              {
                streamWriter1.WriteLine("METRIC");
                streamWriter2.WriteLine("METRIC");
              }
              streamWriter1.WriteLine(this.ReturnFixedLengthString(aStr1, 20, -1) + "  " + aStr2);
              streamWriter2.WriteLine(this.ReturnFixedLengthString(aStr1, 20, -1) + "  " + aStr2);
              streamWriter1.WriteLine("Code     Landuse Descrip.                Area    ");
              streamWriter2.WriteLine("Code     Landuse Descrip.                Area    ");
              foreach (Strata strata1 in (IEnumerable<Strata>) strataList)
              {
                string str7;
                int id;
                string str8;
                string str9;
                for (str7 = this.StrataRGX.Replace(strata1.Description.Trim(), "_"); dictionary.ContainsKey(str7); str7 = str8 + str9)
                {
                  str8 = str7;
                  id = strata1.Id;
                  str9 = id.ToString((IFormatProvider) this.ciUsed);
                }
                dictionary.Add(str7, 1);
                StreamWriter streamWriter3 = streamWriter1;
                string[] strArray2 = new string[5];
                id = strata1.Id;
                strArray2[0] = this.ReturnFixedLengthString(id.ToString((IFormatProvider) this.ciUsed), 8, -1);
                strArray2[1] = " ";
                strArray2[2] = this.ReturnFixedLengthString(str7, 30, -1);
                strArray2[3] = "  ";
                float size = strata1.Size;
                strArray2[4] = size.ToString((IFormatProvider) this.ciUsed);
                string str10 = string.Concat(strArray2);
                streamWriter3.WriteLine(str10);
                if (this.currYear.RecordStrata)
                {
                  StreamWriter streamWriter4 = streamWriter2;
                  string[] strArray3 = new string[5];
                  id = strata1.Id;
                  strArray3[0] = this.ReturnFixedLengthString(id.ToString((IFormatProvider) this.ciUsed), 8, -1);
                  strArray3[1] = " ";
                  strArray3[2] = this.ReturnFixedLengthString(str7, 30, -1);
                  strArray3[3] = "  ";
                  size = this.dictStrataSizeByUserEntered[strata1.Id];
                  strArray3[4] = size.ToString((IFormatProvider) this.ciUsed);
                  string str11 = string.Concat(strArray3);
                  streamWriter4.WriteLine(str11);
                }
                else
                {
                  StreamWriter streamWriter5 = streamWriter2;
                  string[] strArray4 = new string[5];
                  id = strata1.Id;
                  strArray4[0] = this.ReturnFixedLengthString(id.ToString((IFormatProvider) this.ciUsed), 8, -1);
                  strArray4[1] = " ";
                  strArray4[2] = this.ReturnFixedLengthString(str7, 30, -1);
                  strArray4[3] = "  ";
                  size = strata1.Size;
                  strArray4[4] = size.ToString((IFormatProvider) this.ciUsed);
                  string str12 = string.Concat(strArray4);
                  streamWriter5.WriteLine(str12);
                }
              }
            }
          }
          int num30 = num29 + 1;
          uploadProgressArg.Percent = (int) ((double) num30 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
          uploadProgress.Report(uploadProgressArg);
          if (uploadCancellationToken.IsCancellationRequested)
            return false;
          SASDictionary<string, string> sasDictionary3 = new SASDictionary<string, string>("Plot_Landuse");
          using (StreamWriter streamWriter = new StreamWriter(this.outputFolder + "\\Plots.txt"))
          {
            string aStr5 = "7.0";
            if (this.currYear.Unit == YearUnit.English)
              streamWriter.WriteLine("VERSION     " + this.ReturnFixedLengthString(aStr5, 9, -1) + "ENGLISH");
            else
              streamWriter.WriteLine("VERSION     " + this.ReturnFixedLengthString(aStr5, 9, -1) + "METRIC");
            string str13 = this.ReturnFixedLengthString("PLOT I.D", 15, -1) + this.ReturnFixedLengthString("Landuse", 7, -1) + this.ReturnFixedLengthString("IN % us", 7, -1) + this.ReturnFixedLengthString("% BLDG", 6, -1) + this.ReturnFixedLengthString("% CEMEN", 8, -1) + this.ReturnFixedLengthString("% TAR", 5, -1) + this.ReturnFixedLengthString("%OTH IMP", 9, -1) + this.ReturnFixedLengthString("% SOIL", 6, -1) + this.ReturnFixedLengthString("% ROCK", 7, -1) + this.ReturnFixedLengthString("% DUFF/MUL", 11, -1) + this.ReturnFixedLengthString("% HERB/IVY", 10, -1) + this.ReturnFixedLengthString("% GRASS", 8, -1) + this.ReturnFixedLengthString("W % GRASS", 10, -1) + this.ReturnFixedLengthString("% WATER", 8, -1) + this.ReturnFixedLengthString("% SHRUB", 8, -1) + this.ReturnFixedLengthString("%TREE", 6, -1) + this.ReturnFixedLengthString("PLANTABLE", 10, -1) + this.ReturnFixedLengthString("PLOT SIZE", 9, -1);
            streamWriter.WriteLine(str13);
            IList<Plot> plotList = session.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true)).OrderBy((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)).Asc.List<Plot>();
            PlotLandUse aPlotLanduse = (PlotLandUse) null;
            LandUse aLandUse = (LandUse) null;
            Plot aPlot = (Plot) null;
            IList<(int, char, short)> tupleList1 = session.QueryOver<PlotLandUse>((System.Linq.Expressions.Expression<Func<PlotLandUse>>) (() => aPlotLanduse)).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<PlotLandUse, Plot>>) (pl => pl.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => aPlot)).Where((System.Linq.Expressions.Expression<Func<bool>>) (() => aPlot.Year == this.currYear && aPlot.IsComplete == true)).JoinQueryOver<LandUse>((System.Linq.Expressions.Expression<Func<LandUse>>) (() => aPlotLanduse.LandUse), (System.Linq.Expressions.Expression<Func<LandUse>>) (() => aLandUse)).Select((IProjection) Projections.Group((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)), (IProjection) Projections.Group((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aLandUse.Id)), (IProjection) Projections.Sum((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlotLanduse.PercentOfPlot))).OrderBy((IProjection) Projections.Group((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id))).Asc.TransformUsing((IResultTransformer) new TupleResultTransformer<(int, char, short)>()).List<(int, char, short)>();
            GroundCover aGroundCover = (GroundCover) null;
            PlotGroundCover aPlotGroundCover = (PlotGroundCover) null;
            IList<(int, int, int)> tupleList2 = session.QueryOver<PlotGroundCover>((System.Linq.Expressions.Expression<Func<PlotGroundCover>>) (() => aPlotGroundCover)).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<PlotGroundCover, Plot>>) (pgc => pgc.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => aPlot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true)).JoinQueryOver<GroundCover>((System.Linq.Expressions.Expression<Func<GroundCover>>) (() => aPlotGroundCover.GroundCover), (System.Linq.Expressions.Expression<Func<GroundCover>>) (() => aGroundCover)).Where((System.Linq.Expressions.Expression<Func<GroundCover, bool>>) (gc => gc.Year == this.currYear)).Select((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aGroundCover.CoverTypeId)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlotGroundCover.PercentCovered))).OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)).Asc.TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int, int)>()).List<(int, int, int)>();
            int index1 = 0;
            int index2 = 0;
            foreach (Plot plot1 in (IEnumerable<Plot>) plotList)
            {
              int num31 = 0;
              int num32 = 0;
              int num33 = 0;
              int num34 = 0;
              int num35 = 0;
              int num36 = 0;
              int num37 = 0;
              int num38 = 0;
              int num39 = 0;
              int num40 = 0;
              int num41 = 0;
              if (!this.currYear.RecordGroundCover)
              {
                num39 = 100;
              }
              else
              {
                while (index1 < tupleList2.Count && tupleList2[index1].Item1 < plot1.Id)
                  ++index1;
                int num42 = 0;
                for (; index1 < tupleList2.Count && tupleList2[index1].Item1 == plot1.Id; ++index1)
                {
                  switch (tupleList2[index1].Item2)
                  {
                    case 1:
                      num31 += tupleList2[index1].Item3;
                      break;
                    case 2:
                      num32 += tupleList2[index1].Item3;
                      break;
                    case 3:
                      num33 += tupleList2[index1].Item3;
                      break;
                    case 4:
                      num34 += tupleList2[index1].Item3;
                      break;
                    case 5:
                      num35 += tupleList2[index1].Item3;
                      break;
                    case 6:
                      num36 += tupleList2[index1].Item3;
                      break;
                    case 7:
                      num37 += tupleList2[index1].Item3;
                      break;
                    case 8:
                      num38 += tupleList2[index1].Item3;
                      break;
                    case 9:
                      num39 += tupleList2[index1].Item3;
                      break;
                    case 10:
                      num40 += tupleList2[index1].Item3;
                      break;
                    case 11:
                      num41 += tupleList2[index1].Item3;
                      break;
                  }
                  ++num42;
                }
                if (num42 == 0)
                {
                  int num43 = (int) MessageBox.Show(string.Format(SASResources.ErrorPlotNoGroundCover, (object) string.Format("{0} {1}", (object) v6Strings.Plot_SingularName, (object) plot1.Id)), SASResources.ExportTitle, MessageBoxButtons.OK);
                  return false;
                }
              }
              double num44 = (double) plot1.Size * (double) plot1.PercentMeasured / 100.0;
              double num45 = num44;
              char ch1 = 'C';
              if (this.currYear.RecordLanduse)
              {
                while (index2 < tupleList1.Count && tupleList1[index2].Item1 < plot1.Id)
                  ++index2;
                int num46 = 0;
                for (; index2 < tupleList1.Count && tupleList1[index2].Item1 == plot1.Id; ++index2)
                {
                  char ch2 = tupleList1[index2].Item2;
                  double num47 = num44 * (double) tupleList1[index2].Item3 / 100.0;
                  string str14 = "";
                  string str15;
                  if (num46 == 0)
                  {
                    sasDictionary3.Add(plot1.Id.ToString((IFormatProvider) this.ciUsed) + ch2.ToString(), plot1.Id.ToString((IFormatProvider) this.ciUsed));
                    str15 = str14 + this.ReturnFixedLengthString(plot1.Id.ToString((IFormatProvider) this.ciUsed), 21, -1) + this.ReturnFixedLengthString(ch2.ToString(), 7, -1);
                  }
                  else
                  {
                    sasDictionary3.Add(plot1.Id.ToString((IFormatProvider) this.ciUsed) + ch2.ToString(), plot1.Id.ToString((IFormatProvider) this.ciUsed) + "." + num46.ToString((IFormatProvider) this.ciUsed));
                    str15 = str14 + this.ReturnFixedLengthString(plot1.Id.ToString((IFormatProvider) this.ciUsed) + "." + num46.ToString((IFormatProvider) this.ciUsed), 21, -1) + this.ReturnFixedLengthString(ch2.ToString(), 7, -1);
                  }
                  string str16 = str15 + this.ReturnFixedLengthString(tupleList1[index2].Item3.ToString((IFormatProvider) this.ciUsed), 7, -1) + this.ReturnFixedLengthString(num31.ToString((IFormatProvider) this.ciUsed), 6, -1) + this.ReturnFixedLengthString(num32.ToString((IFormatProvider) this.ciUsed), 8, -1) + this.ReturnFixedLengthString(num33.ToString((IFormatProvider) this.ciUsed), 5, -1) + this.ReturnFixedLengthString(num41.ToString((IFormatProvider) this.ciUsed), 9, -1) + this.ReturnFixedLengthString(num35.ToString((IFormatProvider) this.ciUsed), 6, -1) + this.ReturnFixedLengthString(num34.ToString((IFormatProvider) this.ciUsed), 7, -1) + this.ReturnFixedLengthString(num36.ToString((IFormatProvider) this.ciUsed), 11, -1) + this.ReturnFixedLengthString(num37.ToString((IFormatProvider) this.ciUsed), 10, -1) + this.ReturnFixedLengthString(num38.ToString((IFormatProvider) this.ciUsed), 8, -1) + this.ReturnFixedLengthString(num39.ToString((IFormatProvider) this.ciUsed), 10, -1) + this.ReturnFixedLengthString(num40.ToString((IFormatProvider) this.ciUsed), 8, -1) + (plot1.PercentShrubCover < PctMidRange.PR0 ? this.ReturnFixedLengthString("0", 8, -1) : this.ReturnFixedLengthString(((int) plot1.PercentShrubCover).ToString((IFormatProvider) this.ciUsed), 8, -1)) + (plot1.PercentTreeCover < PctMidRange.PR0 ? this.ReturnFixedLengthString("0", 6, -1) : this.ReturnFixedLengthString(((int) plot1.PercentTreeCover).ToString((IFormatProvider) this.ciUsed), 6, -1)) + (plot1.PercentPlantable < PctMidRange.PR0 ? this.ReturnFixedLengthString("0", 10, -1) : this.ReturnFixedLengthString(((int) plot1.PercentPlantable).ToString((IFormatProvider) this.ciUsed), 10, -1)) + this.ReturnFixedLengthString(num47.ToString("#0.0#######", (IFormatProvider) this.ciUsed), 8, -1);
                  streamWriter.WriteLine(str16);
                  ++num46;
                }
                if (num46 == 0)
                {
                  int num48 = (int) MessageBox.Show(string.Format(SASResources.ErrorPlotNoLandUses, (object) string.Format("{0} {1}", (object) v6Strings.Plot_SingularName, (object) plot1.Id)), SASResources.ExportTitle, MessageBoxButtons.OK);
                  return false;
                }
              }
              else
              {
                sasDictionary3.Add(plot1.Id.ToString((IFormatProvider) this.ciUsed) + ch1.ToString(), plot1.Id.ToString((IFormatProvider) this.ciUsed));
                string str17 = this.ReturnFixedLengthString(plot1.Id.ToString((IFormatProvider) this.ciUsed), 21, -1) + this.ReturnFixedLengthString(ch1.ToString(), 7, -1) + this.ReturnFixedLengthString("100", 7, -1) + this.ReturnFixedLengthString(num31.ToString((IFormatProvider) this.ciUsed), 6, -1) + this.ReturnFixedLengthString(num32.ToString((IFormatProvider) this.ciUsed), 8, -1) + this.ReturnFixedLengthString(num33.ToString((IFormatProvider) this.ciUsed), 5, -1) + this.ReturnFixedLengthString(num41.ToString((IFormatProvider) this.ciUsed), 9, -1) + this.ReturnFixedLengthString(num35.ToString((IFormatProvider) this.ciUsed), 6, -1) + this.ReturnFixedLengthString(num34.ToString((IFormatProvider) this.ciUsed), 7, -1) + this.ReturnFixedLengthString(num36.ToString((IFormatProvider) this.ciUsed), 11, -1) + this.ReturnFixedLengthString(num37.ToString((IFormatProvider) this.ciUsed), 10, -1) + this.ReturnFixedLengthString(num38.ToString((IFormatProvider) this.ciUsed), 8, -1) + this.ReturnFixedLengthString(num39.ToString((IFormatProvider) this.ciUsed), 10, -1) + this.ReturnFixedLengthString(num40.ToString((IFormatProvider) this.ciUsed), 8, -1) + (plot1.PercentShrubCover < PctMidRange.PR0 ? this.ReturnFixedLengthString("0", 8, -1) : this.ReturnFixedLengthString(((int) plot1.PercentShrubCover).ToString((IFormatProvider) this.ciUsed), 8, -1)) + (plot1.PercentTreeCover < PctMidRange.PR0 ? this.ReturnFixedLengthString("0", 6, -1) : this.ReturnFixedLengthString(((int) plot1.PercentTreeCover).ToString((IFormatProvider) this.ciUsed), 6, -1)) + (plot1.PercentPlantable < PctMidRange.PR0 ? this.ReturnFixedLengthString("0", 10, -1) : this.ReturnFixedLengthString(((int) plot1.PercentPlantable).ToString((IFormatProvider) this.ciUsed), 10, -1)) + this.ReturnFixedLengthString(num45.ToString("#0.0#######", (IFormatProvider) this.ciUsed), 8, -1);
                streamWriter.WriteLine(str17);
              }
            }
            streamWriter.Close();
          }
          if (this.currYear.FIA)
          {
            using (StreamWriter streamWriter = new StreamWriter(this.outputFolder + "\\FIA_Microplot_Map.txt"))
            {
              streamWriter.WriteLine(this.ReturnFixedLengthString("FIA_MicroPlotID", 16, -1) + " " + this.ReturnFixedLengthString("FIA_PlotID", 15, -1));
              IQueryOver<Plot, Plot> queryOver = session.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true && p.Comments != "")).Select((IProjection) Projections.Property<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)), (IProjection) Projections.Property<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Comments)));
              System.Linq.Expressions.Expression<Func<Plot, object>> path = (System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id);
              foreach ((int, string) tuple in (IEnumerable<(int, string)>) queryOver.OrderBy(path).Asc.TransformUsing((IResultTransformer) new TupleResultTransformer<(int, string)>()).List<(int, string)>())
              {
                if (tuple.Item2.Trim() != "")
                  streamWriter.WriteLine(this.ReturnFixedLengthString(tuple.Item1.ToString((IFormatProvider) this.ciUsed), 16, -1) + " " + this.ReturnFixedLengthString(tuple.Item2, 15, -1));
              }
              streamWriter.Close();
            }
          }
          int num49 = num30 + 1;
          uploadProgressArg.Percent = (int) ((double) num49 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
          uploadProgress.Report(uploadProgressArg);
          if (uploadCancellationToken.IsCancellationRequested)
            return false;
          if (this.currYear.RecordGroundCover)
          {
            using (StreamWriter streamWriter = new StreamWriter(this.outputFolder + "\\GroundCovers.txt"))
            {
              string str = this.ReturnFixedLengthString("PlotID", 8, -1) + " " + this.ReturnFixedLengthString("GroundCoverID", 13, -1) + " " + this.ReturnFixedLengthString("Percentage", 10, -1);
              streamWriter.WriteLine(str);
              Plot aPlot = (Plot) null;
              PlotGroundCover aPlotGroundCover = (PlotGroundCover) null;
              GroundCover aGroundCover = (GroundCover) null;
              IQueryOver<PlotGroundCover, GroundCover> queryOver = session.QueryOver<PlotGroundCover>((System.Linq.Expressions.Expression<Func<PlotGroundCover>>) (() => aPlotGroundCover)).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<PlotGroundCover, Plot>>) (pgc => pgc.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => aPlot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true)).JoinQueryOver<GroundCover>((System.Linq.Expressions.Expression<Func<GroundCover>>) (() => aPlotGroundCover.GroundCover), (System.Linq.Expressions.Expression<Func<GroundCover>>) (() => aGroundCover));
              System.Linq.Expressions.Expression<Func<GroundCover, bool>> expression = (System.Linq.Expressions.Expression<Func<GroundCover, bool>>) (gc => gc.Year == this.currYear);
              foreach ((int, int, int) tuple in (IEnumerable<(int, int, int)>) queryOver.Where(expression).Select((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aGroundCover.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlotGroundCover.PercentCovered))).OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aGroundCover.Id)).Asc.TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int, int)>()).List<(int, int, int)>())
                streamWriter.WriteLine(this.ReturnFixedLengthString(tuple.Item1.ToString((IFormatProvider) this.ciUsed), 8, -1) + " " + this.ReturnFixedLengthString(tuple.Item2.ToString((IFormatProvider) this.ciUsed), 13, -1) + " " + this.ReturnFixedLengthString(tuple.Item3.ToString((IFormatProvider) this.ciUsed), 10, -1));
              streamWriter.Close();
            }
            using (StreamWriter streamWriter = new StreamWriter(this.outputFolder + "\\GroundCoverID.txt"))
            {
              IQueryOver<GroundCover, GroundCover> queryOver = session.QueryOver<GroundCover>().Where((System.Linq.Expressions.Expression<Func<GroundCover, bool>>) (gc => gc.Year == this.currYear)).Select((IProjection) Projections.Property<GroundCover>((System.Linq.Expressions.Expression<Func<GroundCover, object>>) (gc => (object) gc.Id)));
              System.Linq.Expressions.Expression<Func<GroundCover, object>> path = (System.Linq.Expressions.Expression<Func<GroundCover, object>>) (gc => (object) gc.Id);
              foreach (int num50 in (IEnumerable<int>) queryOver.OrderBy(path).Asc.List<int>())
                streamWriter.WriteLine(num50.ToString((IFormatProvider) this.ciUsed));
              streamWriter.Close();
            }
          }
          int num51 = num49 + 1;
          uploadProgressArg.Percent = (int) ((double) num51 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
          uploadProgress.Report(uploadProgressArg);
          if (uploadCancellationToken.IsCancellationRequested)
            return false;
          using (StreamWriter streamWriter = new StreamWriter(this.outputFolder + "\\Trees.txt"))
          {
            if (this.currYear.Unit == YearUnit.English)
              streamWriter.WriteLine("Version   9.0   ENGLISH");
            else
              streamWriter.WriteLine("Version   9.0   METRIC");
            streamWriter.WriteLine(this.ReturnFixedLengthString("PLOT I.", 18, -1) + this.ReturnFixedLengthString("SPP CODE", 9, -1) + this.ReturnFixedLengthString("# DBH/In", 10, -1) + this.ReturnFixedLengthString("DBH1/In", 8, -1) + this.ReturnFixedLengthString("DBH2/In", 8, -1) + this.ReturnFixedLengthString("DBH3/In", 8, -1) + this.ReturnFixedLengthString("DBH4/In", 8, -1) + this.ReturnFixedLengthString("DBH5/In", 8, -1) + this.ReturnFixedLengthString("DBH6/In", 8, -1) + this.ReturnFixedLengthString("TOT[ht]", 9, -1) + this.ReturnFixedLengthString("Bole(ht", 8, -1) + this.ReturnFixedLengthString("Avg. Crwn Width", 16, -1) + this.ReturnFixedLengthString("Cond.", 6, -1) + this.ReturnFixedLengthString("%Area", 7, -1) + this.ReturnFixedLengthString("%Leaf", 6, -1) + this.ReturnFixedLengthString("Number", 14, -1) + this.ReturnFixedLengthString("SHRB IND", 9, -1) + this.ReturnFixedLengthString("NO. BLDS", 9, -1) + this.ReturnFixedLengthString("BLD DIR", 8, -1) + this.ReturnFixedLengthString("BLD DIS", 8, -1) + this.ReturnFixedLengthString("BLD DIR", 8, -1) + this.ReturnFixedLengthString("BLD DIS", 8, -1) + this.ReturnFixedLengthString("BLD DIR", 8, -1) + this.ReturnFixedLengthString("BLD DIS", 8, -1) + this.ReturnFixedLengthString("BLD DIR", 8, -1) + this.ReturnFixedLengthString("BLD DIS", 8, -1) + this.ReturnFixedLengthString("Street tr", 10, -1) + this.ReturnFixedLengthString("CLE", 4, -1) + this.ReturnFixedLengthString("FieldLanduse", 13, -1) + this.ReturnFixedLengthString("Dieback", 14, -1) + this.ReturnFixedLengthString("CCND", 8, -1) + this.ReturnFixedLengthString("Custom DBH", 10, -1) + this.ReturnFixedLengthString("GUID", 40, -1) + this.ReturnFixedLengthString("LiveTop", 8, 1));
            string str18 = new string(' ', 46);
            Plot aPlot = (Plot) null;
            Tree aTree = (Tree) null;
            PlotLandUse aPlotLandUse = (PlotLandUse) null;
            Condition aCondition = (Condition) null;
            LandUse aLandUse = (LandUse) null;
            treeTxtRow result = (treeTxtRow) null;
            IQueryOver<Tree, Tree> queryOver1 = session.QueryOver<Tree>((System.Linq.Expressions.Expression<Func<Tree>>) (() => aTree));
            if (this.currYear.RecordTreeStatus)
              queryOver1.Where((System.Linq.Expressions.Expression<Func<bool>>) (() => !aTree.Status.IsIn(SASProcessor.TreeRemoveStatus)));
            queryOver1.JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => aPlot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true));
            if (this.currYear.RecordCrownCondition)
              queryOver1.JoinQueryOver<Condition>((System.Linq.Expressions.Expression<Func<Condition>>) (() => aTree.Crown.Condition), (System.Linq.Expressions.Expression<Func<Condition>>) (() => aCondition)).Where((System.Linq.Expressions.Expression<Func<Condition, bool>>) (c => c.Year == this.currYear));
            if (this.currYear.RecordLanduse)
              queryOver1.JoinQueryOver<PlotLandUse>((System.Linq.Expressions.Expression<Func<PlotLandUse>>) (() => aTree.PlotLandUse), (System.Linq.Expressions.Expression<Func<PlotLandUse>>) (() => aPlotLandUse)).JoinQueryOver<LandUse>((System.Linq.Expressions.Expression<Func<PlotLandUse, LandUse>>) (plu => plu.LandUse), (System.Linq.Expressions.Expression<Func<LandUse>>) (() => aLandUse));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            IList<treeTxtRow> treeTxtRowList = !this.currYear.RecordCrownCondition ? (!this.currYear.RecordLanduse ? queryOver1.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aTree.Id)).Asc.SelectList((Func<QueryOverProjectionBuilder<Tree>, QueryOverProjectionBuilder<Tree>>) (list => list.Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aPlot.Id)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.PlotId)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.Id)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.TreeId)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.Guid)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.TreeKey)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => this.aTree.Species)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => this.result.Tree_Species)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.TreeHeight)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.TreeHeightTotal)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => this.aTree.Crown)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => this.result.Tree_Crown)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.StreetTree)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.Tree_StreetTree)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) 'C')).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.EcoLanduses_LandUseCode)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) Condition.Default.PctDieback)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.EcoConditions_PctDieback)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) 1)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.EcoConditions_ConditionId)))).TransformUsing(Transformers.AliasToBean<treeTxtRow>()).List<treeTxtRow>() : queryOver1.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aTree.Id)).Asc.SelectList((Func<QueryOverProjectionBuilder<Tree>, QueryOverProjectionBuilder<Tree>>) (list => list.Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aPlot.Id)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.PlotId)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.Id)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.TreeId)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.Guid)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.TreeKey)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => this.aTree.Species)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => this.result.Tree_Species)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.TreeHeight)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.TreeHeightTotal)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => this.aTree.Crown)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => this.result.Tree_Crown)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.StreetTree)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.Tree_StreetTree)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aLandUse.Id)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.EcoLanduses_LandUseCode)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) Condition.Default.PctDieback)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.EcoConditions_PctDieback)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) 1)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.EcoConditions_ConditionId)))).TransformUsing(Transformers.AliasToBean<treeTxtRow>()).List<treeTxtRow>()) : (!this.currYear.RecordLanduse ? queryOver1.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aTree.Id)).Asc.SelectList((Func<QueryOverProjectionBuilder<Tree>, QueryOverProjectionBuilder<Tree>>) (list => list.Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aPlot.Id)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.PlotId)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.Id)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.TreeId)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.Guid)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.TreeKey)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => this.aTree.Species)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => this.result.Tree_Species)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.TreeHeight)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.TreeHeightTotal)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => this.aTree.Crown)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => this.result.Tree_Crown)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.StreetTree)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.Tree_StreetTree)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) 'C')).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.EcoLanduses_LandUseCode)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aCondition.PctDieback)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.EcoConditions_PctDieback)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aCondition.Id)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.EcoConditions_ConditionId)))).TransformUsing(Transformers.AliasToBean<treeTxtRow>()).List<treeTxtRow>() : queryOver1.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aTree.Id)).Asc.SelectList((Func<QueryOverProjectionBuilder<Tree>, QueryOverProjectionBuilder<Tree>>) (list => list.Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aPlot.Id)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.PlotId)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.Id)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.TreeId)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.Guid)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.TreeKey)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => this.aTree.Species)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => this.result.Tree_Species)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.TreeHeight)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.TreeHeightTotal)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => this.aTree.Crown)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => this.result.Tree_Crown)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aTree.StreetTree)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.Tree_StreetTree)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aLandUse.Id)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.EcoLanduses_LandUseCode)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aCondition.PctDieback)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.EcoConditions_PctDieback)).Select((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.aCondition.Id)).WithAlias((System.Linq.Expressions.Expression<Func<object>>) (() => (object) this.result.EcoConditions_ConditionId)))).TransformUsing(Transformers.AliasToBean<treeTxtRow>()).List<treeTxtRow>());
            Shrub aShrub = (Shrub) null;
            IList<(int, int, Guid, string, int, float, PctMidRange)> valueTupleList1;
            if (this.currYear.RecordShrub)
              valueTupleList1 = session.QueryOver<Shrub>((System.Linq.Expressions.Expression<Func<Shrub>>) (() => aShrub)).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Shrub, Plot>>) (sh => sh.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => aPlot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true)).Select((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aShrub.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aShrub.Guid)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => aShrub.Species)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aShrub.PercentOfShrubArea)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aShrub.Height)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aShrub.PercentMissing))).OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aShrub.Id)).Asc.TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int, Guid, string, int, float, PctMidRange)>()).List<(int, int, Guid, string, int, float, PctMidRange)>();
            else
              valueTupleList1 = (IList<(int, int, Guid, string, int, float, PctMidRange)>) new List<(int, int, Guid, string, int, float, PctMidRange)>();
            Stem aStem = (Stem) null;
            IQueryOver<Stem, Tree> queryOver2 = session.QueryOver<Stem>((System.Linq.Expressions.Expression<Func<Stem>>) (() => aStem)).JoinQueryOver<Tree>((System.Linq.Expressions.Expression<Func<Stem, Tree>>) (st => st.Tree), (System.Linq.Expressions.Expression<Func<Tree>>) (() => aTree));
            if (this.currYear.RecordTreeStatus)
              queryOver2.Where((System.Linq.Expressions.Expression<Func<bool>>) (() => !aTree.Status.IsIn(SASProcessor.TreeRemoveStatus)));
            IList<(int, int, int, double)> tupleList = queryOver2.JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => aPlot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true)).OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aTree.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aStem.Id)).Asc.Select((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aTree.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aStem.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aStem.Diameter))).TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int, int, double)>()).List<(int, int, int, double)>();
            Building aBuilding = (Building) null;
            IList<(int, int, int, short, float)> valueTupleList2;
            if (this.currYear.RecordEnergy)
            {
              IQueryOver<Building, Tree> queryOver3 = session.QueryOver<Building>((System.Linq.Expressions.Expression<Func<Building>>) (() => aBuilding)).JoinQueryOver<Tree>((System.Linq.Expressions.Expression<Func<Building, Tree>>) (b => b.Tree), (System.Linq.Expressions.Expression<Func<Tree>>) (() => aTree));
              if (this.currYear.RecordTreeStatus)
                queryOver3.Where((System.Linq.Expressions.Expression<Func<bool>>) (() => !aTree.Status.IsIn(SASProcessor.TreeRemoveStatus)));
              valueTupleList2 = queryOver3.JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => aPlot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true)).OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aTree.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aBuilding.Id)).Asc.Select((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aPlot.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aTree.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aBuilding.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aBuilding.Direction)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aBuilding.Distance))).TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int, int, short, float)>()).List<(int, int, int, short, float)>();
            }
            else
              valueTupleList2 = (IList<(int, int, int, short, float)>) new List<(int, int, int, short, float)>();
            int index3 = 0;
            int index4 = 0;
            int index5 = 0;
            int index6 = 0;
            int num52 = -1;
            int num53 = -1;
            while (true)
            {
              if (index3 >= treeTxtRowList.Count)
                goto label_369;
label_307:
              if (index3 < treeTxtRowList.Count)
              {
                num52 = treeTxtRowList[index3].PlotId;
                num53 = index4 >= valueTupleList1.Count ? num52 + 1 : valueTupleList1[index4].Item1;
              }
              else if (index4 < valueTupleList1.Count)
              {
                num53 = valueTupleList1[index4].Item1;
                num52 = num53 + 1;
              }
              int num54;
              if (num52 <= num53)
              {
                int treeId = treeTxtRowList[index3].TreeId;
                SASDictionary<string, string> sasDictionary4 = sasDictionary3;
                num54 = treeTxtRowList[index3].PlotId;
                string key = num54.ToString((IFormatProvider) this.ciUsed) + treeTxtRowList[index3].EcoLanduses_LandUseCode.ToString();
                string str19 = this.ReturnFixedLengthString(sasDictionary4[key], 17, -1) + " " + this.ReturnFixedLengthString(treeTxtRowList[index3].Tree_Species, 9, -1);
                for (; index5 < tupleList.Count; ++index5)
                {
                  int num55 = tupleList[index5].Item1;
                  int num56 = tupleList[index5].Item2;
                  if (num55 >= num52 && (num55 != num52 || num56 >= treeId))
                  {
                    if (num55 != num52 || num56 != treeId)
                    {
                      if (this.currSeries.SampleType == SampleType.Inventory)
                        throw new Exception(string.Format(SASResources.ErrorNoStemFound, (object) string.Format("{0} {1}", (object) v6Strings.Tree_SingularName, (object) treeId)));
                      throw new Exception(string.Format(SASResources.ErrorNoStemFound, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) num52, (object) v6Strings.Tree_SingularName, (object) treeId)));
                    }
                    break;
                  }
                }
                int num57 = 0;
                string str20 = "";
                double d = 0.0;
                while (num57 < 6)
                {
                  ++num57;
                  double diameter = this.getDiameter(tupleList[index5].Item4);
                  str20 += this.ReturnFixedLengthString(diameter.ToString("#0.00", (IFormatProvider) this.ciUsed), 8, -1);
                  d += diameter * diameter;
                  ++index5;
                  if (index5 < tupleList.Count)
                  {
                    int num58 = tupleList[index5].Item1;
                    int num59 = tupleList[index5].Item2;
                    if (num58 > num52 || num58 == num52 && num59 > treeId)
                      break;
                  }
                  else
                    break;
                }
                if (num57 < 6)
                  str20 = str20.PadRight(48);
                string str21 = str19 + this.ReturnFixedLengthString(num57.ToString((IFormatProvider) this.ciUsed), 10, -1) + str20 + this.ReturnFixedLengthString(treeTxtRowList[index3].TreeHeightTotal.ToString("#0.000", (IFormatProvider) this.ciUsed), 9, -1) + this.ReturnFixedLengthString(treeTxtRowList[index3].Tree_Crown.BaseHeight.ToString("#0.000", (IFormatProvider) this.ciUsed), 8, -1) + this.ReturnFixedLengthString(((float) (((double) treeTxtRowList[index3].Tree_Crown.WidthEW + (double) treeTxtRowList[index3].Tree_Crown.WidthNS) / 2.0)).ToString("#0.000", (IFormatProvider) this.ciUsed), 16, -1);
                double dieback = this.currYear.RecordCrownCondition ? treeTxtRowList[index3].EcoConditions_PctDieback : Condition.Default.PctDieback;
                int conditionsConditionId = treeTxtRowList[index3].EcoConditions_ConditionId;
                string str22 = dieback >= 1.0 ? (dieback > 10.0 ? (dieback > 25.0 ? (dieback > 50.0 ? (dieback > 75.0 ? (dieback > 99.0 ? str21 + this.ReturnFixedLengthString("K", 6, -1) : str21 + this.ReturnFixedLengthString("D", 6, -1)) : str21 + this.ReturnFixedLengthString("C", 6, -1)) : str21 + this.ReturnFixedLengthString("P", 6, -1)) : str21 + this.ReturnFixedLengthString("F", 6, -1)) : str21 + this.ReturnFixedLengthString("G", 6, -1)) : str21 + this.ReturnFixedLengthString("E", 6, -1);
                string str23;
                if (dieback == 100.0)
                {
                  str23 = str22 + this.ReturnFixedLengthString(".", 7, -1) + this.ReturnFixedLengthString("0", 6, -1);
                }
                else
                {
                  string str24 = str22;
                  string str25 = this.ReturnFixedLengthString(".", 7, -1);
                  num54 = (int) (100 - treeTxtRowList[index3].Tree_Crown.PercentMissing);
                  string str26 = this.ReturnFixedLengthString(num54.ToString((IFormatProvider) this.ciUsed), 6, -1);
                  str23 = str24 + str25 + str26;
                }
                string str27 = str23;
                num54 = treeTxtRowList[index3].TreeId;
                string str28 = this.ReturnFixedLengthString(num54.ToString((IFormatProvider) this.ciUsed), 13, -1);
                string str29 = this.ReturnFixedLengthString("T", 9, -1);
                string str30 = str27 + str28 + " " + str29;
                string str31;
                if (this.currYear.RecordEnergy)
                {
                  int num60 = 0;
                  string str32 = "";
                  for (; index6 < valueTupleList2.Count; ++index6)
                  {
                    int num61 = valueTupleList2[index6].Item1;
                    int num62 = valueTupleList2[index6].Item2;
                    if (num61 >= num52 && (num61 != num52 || num62 >= treeId))
                      break;
                  }
                  for (; index6 < valueTupleList2.Count && num60 < 4; ++index6)
                  {
                    int num63 = valueTupleList2[index6].Item1;
                    int num64 = valueTupleList2[index6].Item2;
                    if (num63 == num52 && num64 == treeId)
                    {
                      ++num60;
                      str32 = str32 + this.ReturnFixedLengthString(valueTupleList2[index6].Item4.ToString((IFormatProvider) this.ciUsed), 8, -1) + this.ReturnFixedLengthString(valueTupleList2[index6].Item5.ToString("#0.00", (IFormatProvider) this.ciUsed), 8, -1);
                    }
                    else
                      break;
                  }
                  if (num60 < 4)
                    str32 += this.ReturnFixedLengthString(" ", (4 - num60) * 16, -1);
                  str31 = str30 + this.ReturnFixedLengthString(num60.ToString((IFormatProvider) this.ciUsed), 9, -1) + str32;
                }
                else
                  str31 = str30 + this.ReturnFixedLengthString("0", 9, -1) + this.ReturnFixedLengthString(" ", 64, -1);
                string str33 = !treeTxtRowList[index3].Tree_StreetTree ? str31 + this.ReturnFixedLengthString("N", 10, -1) : str31 + this.ReturnFixedLengthString("Y", 10, -1);
                int num65;
                if (this.currYear.RecordCLE)
                {
                  num65 = (int) treeTxtRowList[index3].Tree_Crown.LightExposure;
                  if (num65 < 0)
                    num65 = 3;
                }
                else
                  num65 = 3;
                string str34 = str33 + this.ReturnFixedLengthString(num65.ToString((IFormatProvider) this.ciUsed), 4, -1) + this.ReturnFixedLengthString(treeTxtRowList[index3].EcoLanduses_LandUseCode.ToString(), 13, -1) + this.ReturnFixedLengthString(dieback.ToString((IFormatProvider) this.ciUsed), 14, -1);
                string str35;
                if (this.currYear.RecordCrownCondition && this.currYear.HealthRptClasses.Count > 0)
                {
                  int num66 = this.ReturnHealthClassId(this.currYear.HealthRptClasses, dieback);
                  if (num66 > 0)
                    str35 = str34 + this.ReturnFixedLengthString(" " + num66.ToString((IFormatProvider) this.ciUsed), 8, -1);
                  else
                    break;
                }
                else
                  str35 = str34 + this.ReturnFixedLengthString(" . ", 8, -1);
                double num67 = Math.Round(Math.Sqrt(d), 3);
                DBHRptClass dbhRptClass1 = (DBHRptClass) null;
                foreach (DBHRptClass dbhRptClass2 in (IEnumerable<DBHRptClass>) this.currYear.DBHRptClasses)
                {
                  if (num67 > dbhRptClass2.RangeStart && (dbhRptClass1 == null || dbhRptClass2.RangeStart > dbhRptClass1.RangeStart))
                    dbhRptClass1 = dbhRptClass2;
                }
                if (dbhRptClass1 == null)
                {
                  if (this.currYear.DBHRptClasses.Count > 0)
                    goto label_360;
                }
                else if (dbhRptClass1.Id <= 0)
                  goto label_362;
                string str36 = str35;
                string str37;
                if (dbhRptClass1 == null)
                {
                  str37 = (string) null;
                }
                else
                {
                  num54 = dbhRptClass1.Id;
                  str37 = num54.ToString((IFormatProvider) this.ciUsed);
                }
                string str38 = this.ReturnFixedLengthString(" " + str37, 10, -1);
                string str39 = str36 + str38 + this.ReturnFixedLengthString(" " + treeTxtRowList[index3].TreeKey.ToString(), 40, -1) + this.ReturnFixedLengthString(treeTxtRowList[index3].Tree_Crown.TopHeight.ToString("#0.000", (IFormatProvider) this.ciUsed), 8, 1);
                streamWriter.WriteLine(str39);
                ++index3;
                continue;
              }
              (int, int, Guid, string, int, float, PctMidRange) valueTuple = valueTupleList1[index4];
              string[] strArray = new string[5]
              {
                this.ReturnFixedLengthString(valueTuple.Item1.ToString((IFormatProvider) this.ciUsed), 17, -1) + " " + this.ReturnFixedLengthString(valueTupleList1[index4].Item4, 9, -1) + this.ReturnFixedLengthString("0", 10, -1) + " ".PadLeft(48),
                null,
                null,
                null,
                null
              };
              valueTuple = valueTupleList1[index4];
              strArray[1] = this.ReturnFixedLengthString(valueTuple.Item6.ToString("#0.000", (IFormatProvider) this.ciUsed), 9, -1);
              strArray[2] = this.ReturnFixedLengthString(".", 8, -1);
              strArray[3] = this.ReturnFixedLengthString(".", 16, -1);
              strArray[4] = this.ReturnFixedLengthString(".", 6, -1);
              string str40 = string.Concat(strArray);
              valueTuple = valueTupleList1[index4];
              string str41 = this.ReturnFixedLengthString(valueTuple.Item5.ToString((IFormatProvider) this.ciUsed), 7, -1);
              string str42 = str40 + str41;
              num54 = (int) (100 - valueTupleList1[index4].Item7);
              string str43 = this.ReturnFixedLengthString(num54.ToString((IFormatProvider) this.ciUsed), 6, -1);
              string str44 = str42 + str43;
              valueTuple = valueTupleList1[index4];
              string str45 = this.ReturnFixedLengthString(valueTuple.Item2.ToString((IFormatProvider) this.ciUsed), 13, -1);
              string str46 = this.ReturnFixedLengthString("S", 9, -1);
              string str47 = str44 + str45 + " " + str46 + this.ReturnFixedLengthString("0", 9, -1) + " ".PadLeft(64) + this.ReturnFixedLengthString("N", 10, -1) + this.ReturnFixedLengthString(".", 4, -1);
              string str48 = str18;
              valueTuple = valueTupleList1[index4];
              string str49 = this.ReturnFixedLengthString(valueTuple.Item3.ToString(), 40, -1);
              string str50 = str47 + str48 + str49;
              streamWriter.WriteLine(str50);
              ++index4;
              continue;
label_369:
              if (index4 < valueTupleList1.Count)
                goto label_307;
              else
                goto label_373;
            }
            throw new Exception("There are trees with dieback/condition not falling into a valid health class. This means the condition is not correctly collected or the health classes are not configured correctly. Please correct your tree data or re-configure health classes.");
label_360:
            throw new Exception("It seems there are tree diameters do not fit into any DBH classes. Please double check DBH classes and make sure all your trees are included.");
label_362:
            throw new Exception("It seems there are tree diameters falling into a DBH class with Id 0 or negative value. Please correct your DBH class Id to a possitive value");
          }
label_373:
          int num68 = num51 + 1;
          uploadProgressArg.Percent = (int) ((double) num68 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
          uploadProgress.Report(uploadProgressArg);
          if (uploadCancellationToken.IsCancellationRequested)
            return false;
          using (StreamWriter streamWriter6 = new StreamWriter(this.outputFolder + "\\PestInfo.txt"))
          {
            using (StreamWriter streamWriter7 = new StreamWriter(this.outputFolder + "\\PestInfoStatus.txt"))
            {
              streamWriter6.WriteLine(this.ReturnFixedLengthString("TreeID", 15, -1) + this.ReturnFixedLengthString("PestClassifierID", 20, -1) + this.ReturnFixedLengthString("Pest/PestSign Code", 20, -1));
              streamWriter7.WriteLine(this.ReturnFixedLengthString("TreeID", 15, -1) + this.ReturnFixedLengthString("HavingPest", 10, -1));
              Tree aTree = (Tree) null;
              IQueryOver<Tree, Tree> queryOver4 = session.QueryOver<Tree>((System.Linq.Expressions.Expression<Func<Tree>>) (() => aTree));
              if (this.currYear.RecordTreeStatus)
                queryOver4.Where((System.Linq.Expressions.Expression<Func<bool>>) (() => !aTree.Status.IsIn(SASProcessor.TreeRemoveStatus)));
              IQueryOver<Tree, Plot> queryOver5 = queryOver4.JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot));
              System.Linq.Expressions.Expression<Func<Plot, bool>> expression = (System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true);
              foreach ((int, Eco.Domain.v6.IPED) tuple in (IEnumerable<(int, Eco.Domain.v6.IPED)>) queryOver5.Where(expression).OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aTree.Id)).Asc.Select((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) aTree.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => aTree.IPED))).TransformUsing((IResultTransformer) new TupleResultTransformer<(int, Eco.Domain.v6.IPED)>()).List<(int, Eco.Domain.v6.IPED)>())
              {
                int num69 = tuple.Item1;
                string str51 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 15, -1);
                StreamWriter streamWriter8 = streamWriter6;
                string str52 = str51;
                num69 = 37;
                string str53 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.Pest;
                string str54 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str55 = str52 + str53 + str54;
                streamWriter8.WriteLine(str55);
                StreamWriter streamWriter9 = streamWriter6;
                string str56 = str51;
                num69 = 22;
                string str57 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.TSDieback;
                string str58 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str59 = str56 + str57 + str58;
                streamWriter9.WriteLine(str59);
                StreamWriter streamWriter10 = streamWriter6;
                string str60 = str51;
                num69 = 23;
                string str61 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.TSEpiSprout;
                string str62 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str63 = str60 + str61 + str62;
                streamWriter10.WriteLine(str63);
                StreamWriter streamWriter11 = streamWriter6;
                string str64 = str51;
                num69 = 24;
                string str65 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.TSWiltFoli;
                string str66 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str67 = str64 + str65 + str66;
                streamWriter11.WriteLine(str67);
                StreamWriter streamWriter12 = streamWriter6;
                string str68 = str51;
                num69 = 25;
                string str69 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.TSEnvStress;
                string str70 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str71 = str68 + str69 + str70;
                streamWriter12.WriteLine(str71);
                StreamWriter streamWriter13 = streamWriter6;
                string str72 = str51;
                num69 = 26;
                string str73 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.TSHumStress;
                string str74 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str75 = str72 + str73 + str74;
                streamWriter13.WriteLine(str75);
                StreamWriter streamWriter14 = streamWriter6;
                string str76 = str51;
                num69 = 27;
                string str77 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.FTChewFoli;
                string str78 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str79 = str76 + str77 + str78;
                streamWriter14.WriteLine(str79);
                StreamWriter streamWriter15 = streamWriter6;
                string str80 = str51;
                num69 = 28;
                string str81 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.FTDiscFoli;
                string str82 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str83 = str80 + str81 + str82;
                streamWriter15.WriteLine(str83);
                StreamWriter streamWriter16 = streamWriter6;
                string str84 = str51;
                num69 = 29;
                string str85 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.FTAbnFoli;
                string str86 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str87 = str84 + str85 + str86;
                streamWriter16.WriteLine(str87);
                StreamWriter streamWriter17 = streamWriter6;
                string str88 = str51;
                num69 = 30;
                string str89 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.FTInsectSigns;
                string str90 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str91 = str88 + str89 + str90;
                streamWriter17.WriteLine(str91);
                StreamWriter streamWriter18 = streamWriter6;
                string str92 = str51;
                num69 = 31;
                string str93 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.FTFoliAffect;
                string str94 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str95 = str92 + str93 + str94;
                streamWriter18.WriteLine(str95);
                StreamWriter streamWriter19 = streamWriter6;
                string str96 = str51;
                num69 = 32;
                string str97 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.BBInsectSigns;
                string str98 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str99 = str96 + str97 + str98;
                streamWriter19.WriteLine(str99);
                StreamWriter streamWriter20 = streamWriter6;
                string str100 = str51;
                num69 = 33;
                string str101 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.BBInsectPres;
                string str102 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str103 = str100 + str101 + str102;
                streamWriter20.WriteLine(str103);
                StreamWriter streamWriter21 = streamWriter6;
                string str104 = str51;
                num69 = 34;
                string str105 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.BBDiseaseSigns;
                string str106 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str107 = str104 + str105 + str106;
                streamWriter21.WriteLine(str107);
                StreamWriter streamWriter22 = streamWriter6;
                string str108 = str51;
                num69 = 36;
                string str109 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.BBProbLoc;
                string str110 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str111 = str108 + str109 + str110;
                streamWriter22.WriteLine(str111);
                StreamWriter streamWriter23 = streamWriter6;
                string str112 = str51;
                num69 = 35;
                string str113 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                num69 = tuple.Item2.BBAbnGrowth;
                string str114 = this.ReturnFixedLengthString(num69.ToString((IFormatProvider) this.ciUsed), 20, -1);
                string str115 = str112 + str113 + str114;
                streamWriter23.WriteLine(str115);
                if (tuple.Item2.Pest == 0)
                  streamWriter7.WriteLine(str51 + this.ReturnFixedLengthString("0", 20, -1));
                else
                  streamWriter7.WriteLine(str51 + this.ReturnFixedLengthString("1", 20, -1));
              }
              streamWriter6.Close();
              streamWriter7.Close();
            }
          }
          int num70 = num68 + 1;
          uploadProgressArg.Percent = (int) ((double) num70 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
          uploadProgress.Report(uploadProgressArg);
          if (uploadCancellationToken.IsCancellationRequested)
            return false;
          using (StreamWriter streamWriter = new StreamWriter(this.outputFolder + "\\readMe.txt"))
          {
            streamWriter.WriteLine("Send Time: " + DateTime.Now.ToString("MM/dd/yyyy H:mm:ss"));
            streamWriter.WriteLine("i-Tree Eco Input Data Information:");
            streamWriter.WriteLine("EcoVersion: " + Application.ProductVersion);
            streamWriter.WriteLine("Location: " + this.currProject.Name);
            streamWriter.WriteLine("Series: " + this.currSeries.Id);
            streamWriter.WriteLine("Year: " + this.currYear.Id.ToString((IFormatProvider) this.ciUsed));
            streamWriter.WriteLine("ProjectYearRevision: " + this.currYear.Revision.ToString((IFormatProvider) this.ciUsed));
            if (this.currYear.Unit == YearUnit.English)
              streamWriter.WriteLine("Measure Unit: E");
            else
              streamWriter.WriteLine("Measure Unit: M");
            if (this.currSeries.SampleType == SampleType.Inventory)
            {
              if (this.TreatFullInventoryAsSampled)
              {
                streamWriter.WriteLine("SampleType: P");
                streamWriter.WriteLine("SampleTypeActual: I");
              }
              else
              {
                streamWriter.WriteLine("SampleType: I");
                streamWriter.WriteLine("SampleTypeActual: I");
              }
            }
            else if (this.currSeries.SampleType == SampleType.RegularPlot)
            {
              streamWriter.WriteLine("SampleType: P");
              streamWriter.WriteLine("SampleTypeActual: P");
            }
            else
            {
              streamWriter.WriteLine("SampleType: " + this.currSeries.SampleType.ToString());
              streamWriter.WriteLine("SampleType: " + this.currSeries.SampleType.ToString());
            }
            streamWriter.WriteLine("StrataCollected: " + (this.currYear.RecordStrata ? "Y" : "N"));
            streamWriter.WriteLine("FIA: " + (this.currYear.FIA ? "Y" : "N"));
            streamWriter.WriteLine("GroundCoverCollected: " + (this.currYear.RecordGroundCover ? "Y" : "N"));
            streamWriter.WriteLine("WeatherStationID: " + this.currYearLocation.WeatherStationId.ToString());
            streamWriter.WriteLine("WeatherDataYear: " + this.currYearLocation.WeatherYear.ToString((IFormatProvider) this.ciUsed));
            int num71 = (int) this.currYearLocation.PollutionYear;
            if (num71 == -1 && this.GetPollutionYears(this.currProjectLocation.LocationId).Contains((int) this.currYearLocation.WeatherYear))
              num71 = (int) this.currYearLocation.WeatherYear;
            streamWriter.WriteLine("PollutionDataYear: " + num71.ToString((IFormatProvider) this.ciUsed));
            streamWriter.WriteLine("ProjectYearGUID: " + this.currYear.Guid.ToString());
            streamWriter.WriteLine("LocationID: " + this.currProjectLocation.LocationId.ToString((IFormatProvider) this.ciUsed));
            streamWriter.WriteLine("NationID: " + this.currProject.NationCode);
            streamWriter.WriteLine("PrimaryPartitionID: " + this.currProject.PrimaryPartitionCode);
            streamWriter.WriteLine("SecondaryPartitionID: " + this.currProject.SecondaryPartitionCode);
            streamWriter.WriteLine("TertiaryPartitionID: " + this.currProject.TertiaryPartitionCode);
            streamWriter.WriteLine("CityPopulation: " + this.currYearLocation.Population.ToString((IFormatProvider) this.ciUsed));
            if (this.currYear.PopulationDensity == null)
              streamWriter.WriteLine("PopulationDensity: NA");
            else
              streamWriter.WriteLine("PopulationDensity: " + this.currYear.PopulationDensity.Price.ToString((IFormatProvider) this.ciUsed));
            streamWriter.WriteLine("CoordinateLat: " + num7.ToString((IFormatProvider) this.ciUsed));
            streamWriter.WriteLine("CoordinateLong: " + num8.ToString((IFormatProvider) this.ciUsed));
            streamWriter.WriteLine("FrostFreeLength: " + num10.ToString((IFormatProvider) this.ciUsed));
            if (this.currProjectLocation.IsUrban)
              streamWriter.WriteLine("MoreThan50PercentAreaInCity: 1");
            else
              streamWriter.WriteLine("MoreThan50PercentAreaInCity: 0");
            string str116 = "None";
            if (this.currProjectLocation.UseTropical)
            {
              switch (num20)
              {
                case 1:
                  str116 = TropicalClimate.Dry.ToString();
                  break;
                case 2:
                  str116 = TropicalClimate.Moist.ToString();
                  break;
                case 3:
                  str116 = TropicalClimate.Wet.ToString();
                  break;
                default:
                  str116 = "None";
                  break;
              }
            }
            streamWriter.WriteLine("TropicalClimate: " + str116);
            string str117 = this.project_s.GetNamedQuery("selectDatabaseFormat").List<string>().SingleOrDefault<string>();
            streamWriter.WriteLine("DatabaseFormat: " + str117);
            int num72 = 0;
            if (this.currYear.DBHRptClasses.Count > 0)
              num72 = this.currYear.DBHRptClasses.Max<DBHRptClass>((Func<DBHRptClass, int>) (i => i.Id));
            int num73 = 0;
            if (this.currYear.HealthRptClasses.Count > 0)
              num73 = this.currYear.HealthRptClasses.Max<HealthRptClass>((Func<HealthRptClass, int>) (i => i.Id));
            streamWriter.WriteLine("MaxDBHId: " + (num72 < 0 ? "0" : num72.ToString((IFormatProvider) this.ciUsed)));
            streamWriter.WriteLine("MaxConditionId: " + (num73 < 0 ? "0" : num73.ToString((IFormatProvider) this.ciUsed)));
            streamWriter.Close();
          }
          int num74 = num70 + 1;
          uploadProgressArg.Percent = (int) ((double) num74 / (double) num1 * (double) (progressToRange - progressFromRange) + (double) progressFromRange);
          uploadProgress.Report(uploadProgressArg);
          if (uploadCancellationToken.IsCancellationRequested)
            return false;
          session.Close();
        }
      }
      catch (Exception ex)
      {
        int num75 = (int) MessageBox.Show(ex.Message, SASResources.ExportTitle, MessageBoxButtons.OK);
        return false;
      }
      return true;
    }

    private int ReturnHealthClassId(ISet<HealthRptClass> categories, double dieback)
    {
      int num1 = -1;
      double num2 = 0.0;
      foreach (HealthRptClass category in (IEnumerable<HealthRptClass>) categories)
      {
        if (dieback <= category.Extent)
        {
          if (num1 == -1)
          {
            num1 = category.Id;
            num2 = category.Extent;
          }
          else if (category.Extent < num2)
          {
            num1 = category.Id;
            num2 = category.Extent;
          }
        }
      }
      return num1;
    }

    private IList<int> GetPollutionYears(int LocationId) => RetryExecutionHandler.Execute<IList<int>>((Func<IList<int>>) (() =>
    {
      using (ISession session = this.m_ps.LocSp.OpenSession())
      {
        IList<int> pollutionYears;
        do
        {
          pollutionYears = session.CreateCriteria<PollutantStationPollutant>().CreateAlias("Locations", "l").Add((ICriterion) Restrictions.Eq("l.Id", (object) LocationId)).SetProjection(Projections.Distinct((IProjection) Projections.Property("MonYear"))).AddOrder(Order.Asc("MonYear")).SetCacheable(true).List<int>();
          if (pollutionYears.Count == 0)
          {
            IList<Location> locationList = session.CreateCriteria<Location>().CreateAlias("Children", "c").CreateAlias("c.Location", "l").Add((ICriterion) Restrictions.IsNotNull("c.Code")).Add((ICriterion) Restrictions.Eq("l.Id", (object) LocationId)).SetCacheable(true).List<Location>();
            if (locationList.Count > 0)
              LocationId = locationList[0].Id;
          }
        }
        while (pollutionYears.Count == 0 && LocationId != 1);
        return pollutionYears;
      }
    }));

    private string ReturnFixedLengthString(string aStr, int fixedLength, int alignment)
    {
      if (aStr.Length >= fixedLength)
        return aStr + " ";
      string str = "";
      switch (alignment)
      {
        case -1:
          str = aStr.PadRight(fixedLength, ' ');
          break;
        case 0:
          int num = (fixedLength - aStr.Length) / 2;
          str = num != 0 ? aStr.PadLeft(num + aStr.Length, ' ') : aStr;
          if (str.Length != fixedLength)
          {
            str = str.PadRight(fixedLength, ' ');
            break;
          }
          break;
        case 1:
          str = aStr.PadLeft(fixedLength, ' ');
          break;
      }
      return str;
    }

    public bool ZipExportedFiles()
    {
      try
      {
        string str = Path.Combine(this.outputFolder, "Original_" + this.ToUSCharacters(this.ToASCII(Path.GetFileName(this.m_ps.InputSession.InputDb))));
        System.IO.File.Copy(this.m_ps.InputSession.InputDb, str);
        using (InputSession inputSession = new InputSession(str))
        {
          inputSession.YearKey = new Guid?(this.m_ps.InputSession.YearKey.Value);
          ISession session = inputSession.CreateSession();
          SASIUtilProvider sasUtilProvider = inputSession.GetSASQuerySupplier(session).GetSASUtilProvider();
          sasUtilProvider.GetSQLQueryClearContentOfTable("BenMapTable").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("ClassValueTable").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("HourlyHydroResults").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("HourlyUFOREBResults").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("IndividualTreeEffects").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("IndividualTreeEnergyEffects").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("IndividualTreePollutionEffects").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("ModelNotes").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("Pollutants").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("UVIndexReduction").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("UVIndexReductionByStrataYearly").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryRemoveNullRecordsOfTable("EcoForecastCohorts", "ParentKey").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("EcoForecastCohorts").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("EcoForecastCohortResults").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("EcoForecastPollutantResults").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("EcoForecastEvents").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("EcoForecastMortalities").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("EcoForecastPlantedTrees").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("EcoForecasts").ExecuteUpdate();
          foreach ((int, string) tuple in (IEnumerable<(int, string)>) session.GetNamedQuery("selectRecordsFromTableOfStatisticalEstimates").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, string)>()).List<(int, string)>())
          {
            IQuery sqlQueryDropTable = sasUtilProvider.GetSQLQueryDropTable(tuple.Item2);
            try
            {
              sqlQueryDropTable.ExecuteUpdate();
            }
            catch (Exception ex)
            {
            }
          }
          sasUtilProvider.GetSQLQueryClearContentOfTable("PartitionDefinitionsTable").ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("TableOfStatisticalEstimates").ExecuteUpdate();
          session.GetNamedQuery("deleteOtherProjectsData").SetGuid("yearGuid", this.currYear.Guid).ExecuteUpdate();
          sasUtilProvider.GetSQLQueryClearContentOfTable("EcoYearResults").ExecuteUpdate();
          using (ITransaction transaction = session.BeginTransaction())
          {
            session.SaveOrUpdate((object) new YearResult()
            {
              Year = this.currYear,
              RevProcessed = this.currYear.Revision,
              DateTime = new DateTime?(DateTime.Now),
              Email = this.userEmail,
              Data = Path.GetFileName(this.zipFileToSend)
            });
            Year year = session.Get<Year>((object) this.currYear.Guid);
            year.Changed = false;
            ++year.Revision;
            session.SaveOrUpdate((object) year);
            transaction.Commit();
          }
          session.Close();
          inputSession.Close();
          if (this.project_s.GetNamedQuery("selectDatabaseFormat").List<string>().SingleOrDefault<string>() == "ACCESS")
          {
            try
            {
              AccessDatabase.Open(str, true).Compact();
            }
            catch (Exception ex)
            {
            }
          }
        }
        using (C1ZipFile c1ZipFile = new C1ZipFile())
        {
          c1ZipFile.Create(this.zipFileToSend);
          c1ZipFile.Entries.Add(str);
          c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "city18a.prn"));
          c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "enrgmap_new.txt"));
          c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "oplts.txt"));
          if (System.IO.File.Exists(Path.Combine(this.outputFolder, "OriginalLanduse.txt")))
            c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "OriginalLanduse.txt"));
          c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "PestInfo.txt"));
          c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "PestInfoStatus.txt"));
          c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "Plots.txt"));
          if (System.IO.File.Exists(Path.Combine(this.outputFolder, "FIA_Microplot_Map.txt")))
            c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "FIA_Microplot_Map.txt"));
          if (System.IO.File.Exists(Path.Combine(this.outputFolder, "GroundCovers.txt")))
            c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "GroundCovers.txt"));
          if (System.IO.File.Exists(Path.Combine(this.outputFolder, "GroundCoverID.txt")))
            c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "GroundCoverID.txt"));
          c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "ReOrderIDs.txt"));
          c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "tot.txt"));
          c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "tot.txt.userentered"));
          if (System.IO.File.Exists(Path.Combine(this.outputFolder, "landuseArea.txt")))
            c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "landuseArea.txt"));
          c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "Trees.txt"));
          c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "u4Fdlu.txt"));
          c1ZipFile.Entries.Add(Path.Combine(this.outputFolder, "readMe.txt"));
          c1ZipFile.Close();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, SASResources.ExportTitle, MessageBoxButtons.OK);
        return false;
      }
      return true;
    }

    public void UnzipFile(string destFolder, string zipFile)
    {
      using (C1ZipFile c1ZipFile = new C1ZipFile())
      {
        c1ZipFile.Open(zipFile);
        for (int index = 0; index < c1ZipFile.Entries.Count; ++index)
          c1ZipFile.Entries.Extract(index, Path.Combine(destFolder, c1ZipFile.Entries[index].FileName));
        c1ZipFile.Close();
      }
    }

    public void AddContactInfoToReadMeFile(
      string sName,
      string sAddress,
      string sPhone,
      string sPhoneExtension,
      string sEmail,
      string sNotes)
    {
      using (StreamWriter streamWriter = new StreamWriter(this.outputFolder + "\\ReadMe.txt", true))
      {
        this.userEmail = sEmail;
        streamWriter.WriteLine("");
        streamWriter.WriteLine("");
        streamWriter.WriteLine("");
        streamWriter.WriteLine("Contact Information:");
        streamWriter.WriteLine("Name: " + sName);
        streamWriter.WriteLine("Address: " + sAddress);
        if (sPhoneExtension == "")
          streamWriter.WriteLine("Phone: " + sPhone);
        else
          streamWriter.WriteLine("Phone: " + sPhone + "   ext " + sPhoneExtension);
        streamWriter.WriteLine("Email: " + sEmail);
        streamWriter.WriteLine("Notes: ");
        streamWriter.WriteLine("   " + sNotes);
        streamWriter.Close();
      }
      using (StreamWriter streamWriter = new StreamWriter(this.zipFileToSend + ".email.txt", true))
      {
        streamWriter.WriteLine(sEmail);
        streamWriter.Close();
      }
    }

    private bool ModifyDatabaseForSubmission(
      IProgress<SASProgressArg> uploadProgress,
      CancellationToken uploadCancellationToken,
      SASProgressArg uploadProgressArg,
      int progressFromRange,
      int progressToRange)
    {
      try
      {
        using (InputSession inputSession = new InputSession(this.workingInputDatabaseName))
        {
          inputSession.YearKey = new Guid?(this.m_ps.InputSession.YearKey.Value);
          ISession session = inputSession.CreateSession();
          using (ISession ls = this.m_ps.LocSp.OpenSession())
          {
            if (!new DefaultTreeData().calculate(session, this.m_ps.InputSession.YearKey.Value, ls, uploadProgress, uploadCancellationToken, uploadProgressArg, progressFromRange, progressToRange))
              return false;
          }
          foreach (Strata strata in (IEnumerable<Strata>) session.QueryOver<Strata>().List<Strata>())
          {
            int id = strata.Id;
            float size = strata.Size;
            if (!this.dictStrataSizeByUserEntered.ContainsKey(id))
              this.dictStrataSizeByUserEntered.Add(id, size);
          }
          if (this.currSeries.SampleType == SampleType.Inventory)
          {
            IList<Guid> guidList = session.GetNamedQuery("getPlotsWithoutTrees").SetGuid("YearKey", this.currYear.Guid).List<Guid>();
            IQuery namedQuery1 = session.GetNamedQuery("removePlots");
            foreach (Guid val in (IEnumerable<Guid>) guidList)
              namedQuery1.SetGuid("PlotKey", val).ExecuteUpdate();
            session.GetNamedQuery("setIsCompleteToTrueInEcoPlots").SetGuid("yearGuid", this.currYear.Guid).ExecuteUpdate();
            double val1 = this.currYear.Unit != YearUnit.English ? 0.0404686 : 0.1;
            session.GetNamedQuery("updatePlotSize").SetSingle("plotSize", (float) val1).SetGuid("yearGuid", this.currYear.Guid).ExecuteUpdate();
            Plot plotType = (Plot) null;
            Strata strataType = (Strata) null;
            IList<(Guid, float)> tupleList = session.QueryOver<Plot>((System.Linq.Expressions.Expression<Func<Plot>>) (() => plotType)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear)).JoinQueryOver<Strata>((System.Linq.Expressions.Expression<Func<Plot, Strata>>) (p => p.Strata), (System.Linq.Expressions.Expression<Func<Strata>>) (() => strataType)).Select((IProjection) Projections.Group((System.Linq.Expressions.Expression<Func<object>>) (() => (object) strataType.Guid)), (IProjection) Projections.Sum((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plotType.Size))).TransformUsing((IResultTransformer) new TupleResultTransformer<(Guid, float)>()).List<(Guid, float)>();
            IQuery namedQuery2 = session.GetNamedQuery("updateSingleStrataSize");
            foreach ((Guid, float) tuple in (IEnumerable<(Guid, float)>) tupleList)
              namedQuery2.SetDouble("sumPlotSize", (double) tuple.Item2).SetGuid("strataGuid", tuple.Item1).ExecuteUpdate();
            session.GetNamedQuery("removeStrataWithoutPlots").SetGuid("yearGuid", this.currYear.Guid).ExecuteUpdate();
          }
          else
            session.GetNamedQuery("removeIncompletePlots").SetGuid("yearGuid", this.currYear.Guid).ExecuteUpdate();
          session.GetNamedQuery("updateInvalidPercentsInEcoPlots").SetGuid("yearGuid", this.currYear.Guid).ExecuteUpdate();
          session.GetNamedQuery("updateInvalidBuildingAngle").SetGuid("yearGuid", this.currYear.Guid).ExecuteUpdate();
          session.GetNamedQuery("updateMissingParameters").SetGuid("yearGuid", this.currYear.Guid).ExecuteUpdate();
          inputSession.Close();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, SASResources.ExportTitle, MessageBoxButtons.OK);
        return false;
      }
      return true;
    }

    public bool DataValidationOfSynonymOrCultivar(
      bool includeIncompletePlots,
      Dictionary<string, string> treeSynonymCultivar,
      Dictionary<string, string> shrubSynonymCultivar,
      Dictionary<string, string> mortalityTreeSynonymCultivar)
    {
      using (ISession session = this.m_ps.LocSp.OpenSession())
      {
        Dictionary<string, string> dictionary1 = session.QueryOver<Species>().Fetch<Species, Species>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.Code), (System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.ReplacedBy.Code)).Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (sp => sp.Code != default (string) && sp.ReplacedBy != default (object))).Cacheable().List().ToDictionary<Species, string, string>((Func<Species, string>) (sp => sp.Code), (Func<Species, string>) (sp => sp.ReplacedBy.Code));
        Dictionary<string, string> dictionary2 = session.QueryOver<Species>().Fetch<Species, Species>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.Code), (System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.Parent.Code)).Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (sp => sp.Code != default (string) && (int) sp.Rank == 8)).Cacheable().List().ToDictionary<Species, string, string>((Func<Species, string>) (sp => sp.Code), (Func<Species, string>) (sp => sp.Parent.Code));
        treeSynonymCultivar.Clear();
        shrubSynonymCultivar.Clear();
        if (this.currSeries.SampleType == SampleType.Inventory)
        {
          IList<string> stringList = this.project_s.QueryOver<Tree>().JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (tr => tr.Plot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (pl => pl.Year == this.currYear)).Select(Projections.Distinct((IProjection) Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => tr.Species)))).List<string>();
          if (stringList != null)
          {
            foreach (string key in (IEnumerable<string>) stringList)
            {
              if (dictionary1.ContainsKey(key))
                treeSynonymCultivar.Add(key, dictionary1[key]);
              else if (dictionary2.ContainsKey(key))
                treeSynonymCultivar.Add(key, dictionary2[key]);
            }
          }
        }
        else
        {
          List<bool> trueFalse = new List<bool>();
          trueFalse.Add(true);
          if (includeIncompletePlots)
            trueFalse.Add(false);
          IList<string> stringList1 = this.project_s.QueryOver<Tree>().JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (tr => tr.Plot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (pl => pl.Year == this.currYear && pl.IsComplete.IsIn(trueFalse))).Select(Projections.Distinct((IProjection) Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => tr.Species)))).List<string>();
          if (stringList1 != null)
          {
            foreach (string key in (IEnumerable<string>) stringList1)
            {
              if (dictionary1.ContainsKey(key))
                treeSynonymCultivar.Add(key, dictionary1[key]);
              else if (dictionary2.ContainsKey(key))
                treeSynonymCultivar.Add(key, dictionary2[key]);
            }
          }
          if (this.currYear.RecordShrub)
          {
            IList<string> stringList2 = this.project_s.QueryOver<Shrub>().JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Shrub, Plot>>) (sh => sh.Plot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (pl => pl.Year == this.currYear && pl.IsComplete.IsIn(trueFalse))).Select(Projections.Distinct((IProjection) Projections.Property<Shrub>((System.Linq.Expressions.Expression<Func<Shrub, object>>) (sh => sh.Species)))).List<string>();
            if (stringList2 != null)
            {
              foreach (string key in (IEnumerable<string>) stringList2)
              {
                if (dictionary1.ContainsKey(key))
                  shrubSynonymCultivar.Add(key, dictionary1[key]);
                else if (dictionary2.ContainsKey(key))
                  shrubSynonymCultivar.Add(key, dictionary2[key]);
              }
            }
          }
        }
        mortalityTreeSynonymCultivar.Clear();
        IList<Mortality> mortalityList = this.project_s.QueryOver<Mortality>().Where((System.Linq.Expressions.Expression<Func<Mortality, bool>>) (m => m.Type == "Genus")).JoinQueryOver<Forecast>((System.Linq.Expressions.Expression<Func<Mortality, Forecast>>) (m => m.Forecast)).Where((System.Linq.Expressions.Expression<Func<Forecast, bool>>) (f => f.Year == this.currYear)).List<Mortality>();
        if (mortalityList != null)
        {
          foreach (Mortality mortality in (IEnumerable<Mortality>) mortalityList)
          {
            string key = mortality.Value;
            if (dictionary1.ContainsKey(key))
              mortalityTreeSynonymCultivar.Add(key, dictionary1[key]);
            else if (dictionary2.ContainsKey(key))
              mortalityTreeSynonymCultivar.Add(key, dictionary2[key]);
          }
        }
        return treeSynonymCultivar.Count <= 0 && shrubSynonymCultivar.Count <= 0;
      }
    }

    private IList<int> FindTrees(System.Linq.Expressions.Expression<Func<Tree, bool>> where = null)
    {
      IQueryable<Tree> source = this.project_s.Query<Tree>();
      if (where != null)
        source = source.Where<Tree>(where);
      if (this.currYear.RecordTreeStatus)
        source = source.Where<Tree>((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => !SASProcessor.TreeRemoveStatus.Contains<char>(t.Status)));
      if (this.currYear.RecordCrownCondition)
        source = source.Where<Tree>((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Crown.Condition == default (object) || t.Crown.Condition.PctDieback != 100.0));
      return (IList<int>) source.Where<Tree>((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Plot.Year == this.currYear)).OrderBy<Tree, int>((System.Linq.Expressions.Expression<Func<Tree, int>>) (t => t.Id)).Select<Tree, int>((System.Linq.Expressions.Expression<Func<Tree, int>>) (t => t.Id)).ToList<int>();
    }

    private IList<(int TreeId, int StemId)> FindTreeStems(System.Linq.Expressions.Expression<Func<Stem, bool>> where = null)
    {
      IQueryable<Stem> source = this.project_s.Query<Stem>();
      if (where != null)
        source = source.Where<Stem>(where);
      if (this.currYear.RecordTreeStatus)
        source = source.Where<Stem>((System.Linq.Expressions.Expression<Func<Stem, bool>>) (st => !SASProcessor.TreeRemoveStatus.Contains<char>(st.Tree.Status)));
      if (this.currYear.RecordCrownCondition)
        source = source.Where<Stem>((System.Linq.Expressions.Expression<Func<Stem, bool>>) (st => st.Tree.Crown.Condition == default (object) || st.Tree.Crown.Condition.PctDieback != 100.0));
      return (IList<(int, int)>) source.Where<Stem>((System.Linq.Expressions.Expression<Func<Stem, bool>>) (st => st.Tree.Plot.Year == this.currYear)).OrderBy<Stem, int>((System.Linq.Expressions.Expression<Func<Stem, int>>) (st => st.Tree.Id)).ThenBy<Stem, int>((System.Linq.Expressions.Expression<Func<Stem, int>>) (st => st.Id)).Select<Stem, (int, int)>((System.Linq.Expressions.Expression<Func<Stem, (int, int)>>) (st => ValueTuple.Create<int, int>(st.Tree.Id, st.Id))).ToList<(int, int)>();
    }

    private IList<int> FindPlots(System.Linq.Expressions.Expression<Func<Plot, bool>> where = null)
    {
      IQueryable<Plot> source = this.project_s.Query<Plot>().Where<Plot>((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true));
      if (where != null)
        source = source.Where<Plot>(where);
      return (IList<int>) source.Select<Plot, int>((System.Linq.Expressions.Expression<Func<Plot, int>>) (p => p.Id)).ToList<int>();
    }

    private IList<(int PlotId, int TreeId)> FindPlotTrees(System.Linq.Expressions.Expression<Func<Tree, bool>> where = null)
    {
      IQueryable<Tree> source = this.project_s.Query<Tree>();
      if (where != null)
        source = source.Where<Tree>(where);
      if (this.currYear.RecordTreeStatus)
        source = source.Where<Tree>((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => !SASProcessor.TreeRemoveStatus.Contains<char>(t.Status)));
      if (this.currYear.RecordCrownCondition)
        source = source.Where<Tree>((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Crown.Condition == default (object) || t.Crown.Condition.PctDieback != 100.0));
      return (IList<(int, int)>) source.Where<Tree>((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Plot.Year == this.currYear && t.Plot.IsComplete == true)).OrderBy<Tree, int>((System.Linq.Expressions.Expression<Func<Tree, int>>) (t => t.Plot.Id)).ThenBy<Tree, int>((System.Linq.Expressions.Expression<Func<Tree, int>>) (t => t.Id)).Select<Tree, (int, int)>((System.Linq.Expressions.Expression<Func<Tree, (int, int)>>) (t => ValueTuple.Create<int, int>(t.Plot.Id, t.Id))).ToList<(int, int)>();
    }

    private IList<(int PlotId, int TreeId, int StemId)> FindPlotTreeStems(
      System.Linq.Expressions.Expression<Func<Stem, bool>> where = null)
    {
      IQueryable<Stem> source = this.project_s.Query<Stem>();
      if (where != null)
        source = source.Where<Stem>(where);
      if (this.currYear.RecordTreeStatus)
        source = source.Where<Stem>((System.Linq.Expressions.Expression<Func<Stem, bool>>) (st => !SASProcessor.TreeRemoveStatus.Contains<char>(st.Tree.Status)));
      if (this.currYear.RecordCrownCondition)
        source = source.Where<Stem>((System.Linq.Expressions.Expression<Func<Stem, bool>>) (st => st.Tree.Crown.Condition == default (object) || st.Tree.Crown.Condition.PctDieback != 100.0));
      return (IList<(int, int, int)>) source.Where<Stem>((System.Linq.Expressions.Expression<Func<Stem, bool>>) (st => st.Tree.Plot.Year == this.currYear && st.Tree.Plot.IsComplete == true)).OrderBy<Stem, int>((System.Linq.Expressions.Expression<Func<Stem, int>>) (st => st.Tree.Plot.Id)).ThenBy<Stem, int>((System.Linq.Expressions.Expression<Func<Stem, int>>) (st => st.Tree.Id)).ThenBy<Stem, int>((System.Linq.Expressions.Expression<Func<Stem, int>>) (st => st.Id)).Select<Stem, (int, int, int)>((System.Linq.Expressions.Expression<Func<Stem, (int, int, int)>>) (st => ValueTuple.Create<int, int, int>(st.Tree.Plot.Id, st.Tree.Id, st.Id))).ToList<(int, int, int)>();
    }

    public bool DataValidation(string inputDbsName, ref string fatalError, ref string warningError)
    {
      double num1 = 0.0;
      foreach (DBHRptClass dbhRptClass in (IEnumerable<DBHRptClass>) this.currYear.DBHRptClasses)
      {
        if (num1 < dbhRptClass.RangeEnd)
          num1 = dbhRptClass.RangeEnd;
      }
      bool flag1 = false;
      try
      {
        using (ISession session = this.m_ps.LocSp.OpenSession())
        {
          using (session.BeginTransaction())
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            SASProcessor.\u003C\u003Ec__DisplayClass87_0 cDisplayClass870 = new SASProcessor.\u003C\u003Ec__DisplayClass87_0();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass870.\u003C\u003E4__this = this;
            Location location = session.Get<Location>((object) this.currProjectLocation.LocationId);
            List<string> values1 = new List<string>();
            List<string> values2 = new List<string>();
            List<string> values3 = new List<string>();
            List<string> values4 = new List<string>();
            List<string> values5 = new List<string>();
            List<string> values6 = new List<string>();
            List<string> values7 = new List<string>();
            List<string> values8 = new List<string>();
            List<string> values9 = new List<string>();
            List<string> values10 = new List<string>();
            List<string> values11 = new List<string>();
            List<string> values12 = new List<string>();
            List<string> values13 = new List<string>();
            List<string> values14 = new List<string>();
            bool flag2 = true;
            if ((location.Attributes & LocationAttributes.CanBeProcessed) == LocationAttributes.None)
            {
              values2.Add(SASResources.ErrorLocationNotSupported);
              flag2 = false;
            }
            string str1 = "00000";
            string str2 = "000";
            string str3 = "00";
            string str4 = "000";
            LocationRelation locationRelation1 = session.QueryOver<LocationRelation>().Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Location.Id == this.currProject.LocationId)).Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Code != default (string))).Cacheable().SingleOrDefault();
            LocationRelation locationRelation2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            for (cDisplayClass870.lr = locationRelation1; cDisplayClass870.lr != null; cDisplayClass870.lr = locationRelation2)
            {
              // ISSUE: reference to a compiler-generated field
              switch (cDisplayClass870.lr.Level)
              {
                case 2:
                  // ISSUE: reference to a compiler-generated field
                  str4 = cDisplayClass870.lr.Code;
                  break;
                case 3:
                  // ISSUE: reference to a compiler-generated field
                  str3 = cDisplayClass870.lr.Code;
                  break;
                case 4:
                  // ISSUE: reference to a compiler-generated field
                  str2 = cDisplayClass870.lr.Code;
                  break;
                case 5:
                  // ISSUE: reference to a compiler-generated field
                  str1 = cDisplayClass870.lr.Code;
                  break;
              }
              // ISSUE: reference to a compiler-generated field
              locationRelation2 = session.QueryOver<LocationRelation>().Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Location == cDisplayClass870.lr.Parent)).Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Code != default (string))).Cacheable().SingleOrDefault();
            }
            if (this.currProject.NationCode != str4 || this.currProject.PrimaryPartitionCode != str3 || this.currProject.SecondaryPartitionCode != str2 || this.currProject.TertiaryPartitionCode != str1)
              values2.Add(SASResources.ErrorLocationDataInvalid);
            if (flag2)
            {
              if (this.currYearLocation.PollutionYear == (short) -1)
                values9.Add(SASResources.WarningWeatherYear);
              else if ((int) this.currYearLocation.PollutionYear != (int) this.currYearLocation.WeatherYear)
                values2.Add(SASResources.ErrorLocationDataInvalid);
            }
            if (flag2)
            {
              try
              {
                if (!this.checkWeatherStationFromServer((int) this.currYearLocation.WeatherYear, this.currYearLocation.WeatherStationId))
                  values2.Add(string.Format(SASResources.ErrorWeatherYearUnavailable, (object) this.currYearLocation.WeatherYear, (object) this.currYearLocation.WeatherStationId));
              }
              catch (Exception ex)
              {
                values2.Add(string.Format(SASResources.ErrorWeatherValidationFailed, (object) ex.Message));
              }
            }
            if (NationFeatures.isUsingBenMAPresults(this.currProject.NationCode) && this.currYearLocation.Population <= 0)
              values2.Add(SASResources.ErrorPopulationNotSet);
            if (!NationFeatures.isUSA(this.currProject.NationCode) && (this.currYear.ExchangeRate == null || this.currYear.ExchangeRate.Price == 0.0))
            {
              values3.Add(SASResources.ErrorCurrencyExchangeRate);
              flag1 = true;
            }
            IList<Strata> strataList = this.project_s.QueryOver<Strata>().Where((System.Linq.Expressions.Expression<Func<Strata, bool>>) (r => r.Year == this.currYear)).List<Strata>();
            this.dictStrataSizeByUserEntered.Clear();
            SASDictionary<string, string> sasDictionary1 = new SASDictionary<string, string>("Strata Abbreviation");
            SASDictionary<string, string> sasDictionary2 = new SASDictionary<string, string>("Strata Description");
            SASDictionary<string, double> sasDictionary3 = new SASDictionary<string, double>("Strata Description");
            foreach (Strata strata in (IEnumerable<Strata>) strataList)
            {
              int id = strata.Id;
              string key1 = strata.Description.Trim();
              string key2 = strata.Abbreviation.Trim();
              float size = strata.Size;
              string str5 = string.Format("{0} {1}", (object) v6Strings.Strata_SingularName, (object) strata.Id);
              if ((double) size <= 0.0)
                values4.Add(string.Format(SASResources.ErrorArea, (object) str5));
              if (!this.dictStrataSizeByUserEntered.ContainsKey(id))
              {
                this.dictStrataSizeByUserEntered.Add(id, size);
                sasDictionary2.Add(key1, key2);
                sasDictionary3.Add(key1, (double) size);
              }
              else
                values4.Add(string.Format(SASResources.ErrorDupDescription, (object) str5));
              if (!sasDictionary1.ContainsKey(key2))
                sasDictionary1.Add(key2, key2);
              else
                values4.Add(string.Format(SASResources.ErrorDupAbbreviation, (object) str5));
              if (key1.IndexOf("  ") > -1)
                values4.Add(string.Format(SASResources.ErrorSpaceDescription, (object) str5));
              if (key1.IndexOf(",") > -1)
                values4.Add(string.Format(SASResources.ErrorCommaDescription, (object) str5));
            }
            if (this.currYear.FIA)
            {
              int num2 = 0;
              foreach (string key in sasDictionary2.Keys)
              {
                if (sasDictionary2.ContainsKey("M" + key))
                {
                  ++num2;
                  if (sasDictionary2[key] + "M" != sasDictionary2["M" + key] || sasDictionary3[key] != sasDictionary3["M" + key])
                    values4.Add(string.Format(SASResources.ErrorStrataFIA, (object) string.Format("{0} {1}", (object) v6Strings.Strata_SingularName, (object) key)));
                }
              }
            }
            if (this.currYear.RecordLanduse)
            {
              IList<int> intList = session.QueryOver<FieldLandUse>().Select((IProjection) Projections.Id()).Cacheable().List<int>();
              SASDictionary<char, char> sasDictionary4 = new SASDictionary<char, char>();
              SASDictionary<string, string> sasDictionary5 = new SASDictionary<string, string>();
              IQueryOver<LandUse, LandUse> queryOver = this.project_s.QueryOver<LandUse>();
              System.Linq.Expressions.Expression<Func<LandUse, bool>> expression = (System.Linq.Expressions.Expression<Func<LandUse, bool>>) (lu => lu.Year == this.currYear);
              foreach (LandUse landUse in (IEnumerable<LandUse>) queryOver.Where(expression).List<LandUse>())
              {
                char id = landUse.Id;
                string description = landUse.Description;
                if (!intList.Contains(landUse.LandUseId))
                  values5.Add(string.Format(SASResources.ErrorInvalidCategory, (object) string.Format("{0} {1}", (object) v6Strings.LandUse_SingularName, (object) id)));
                if (!char.IsLetter(id) && !char.IsDigit(id))
                  values5.Add(string.Format(SASResources.ErrorInvalidCode, (object) string.Format("{0} {1}", (object) v6Strings.LandUse_SingularName, (object) id)));
                if (sasDictionary4.ContainsKey(char.ToUpper(id, this.ciUsed)))
                  values5.Add(string.Format(SASResources.ErrorDupCode, (object) string.Format("{0} {1}", (object) v6Strings.LandUse_SingularName, (object) id)));
                else
                  sasDictionary4.Add(char.ToUpper(id, this.ciUsed), char.ToUpper(id, this.ciUsed));
                if (description.IndexOf("  ", StringComparison.InvariantCultureIgnoreCase) > -1)
                  values5.Add(string.Format(SASResources.ErrorSpaceDescription, (object) string.Format("{0} {1}", (object) v6Strings.LandUse_SingularName, (object) id)));
                if (sasDictionary5.ContainsKey(description.ToUpper(this.ciUsed)))
                  values5.Add(string.Format(SASResources.ErrorDupDescription, (object) string.Format("{0} {1}", (object) v6Strings.LandUse_SingularName, (object) id)));
                else
                  sasDictionary5.Add(description.ToUpper(this.ciUsed), description.ToUpper(this.ciUsed));
                if (description.IndexOf(",", StringComparison.InvariantCultureIgnoreCase) > -1)
                  values5.Add(string.Format(SASResources.ErrorCommaDescription, (object) string.Format("{0} {1}", (object) v6Strings.LandUse_SingularName, (object) id)));
              }
            }
            if (this.currYear.RecordGroundCover)
            {
              IList<int> intList = session.QueryOver<CoverType>().Select((IProjection) Projections.Id()).Cacheable().List<int>();
              SASDictionary<string, string> sasDictionary6 = new SASDictionary<string, string>();
              IQueryOver<GroundCover, GroundCover> queryOver = this.project_s.QueryOver<GroundCover>();
              System.Linq.Expressions.Expression<Func<GroundCover, bool>> expression = (System.Linq.Expressions.Expression<Func<GroundCover, bool>>) (gc => gc.Year == this.currYear);
              foreach (GroundCover groundCover in (IEnumerable<GroundCover>) queryOver.Where(expression).List<GroundCover>())
              {
                string description = groundCover.Description;
                int id = groundCover.Id;
                if (!intList.Contains(groundCover.CoverTypeId))
                  values6.Add(string.Format(SASResources.ErrorInvalidCategory, (object) string.Format("{0} {1}", (object) v6Strings.GroundCover_SingularName, (object) id)));
                if (description.IndexOf("  ", StringComparison.InvariantCultureIgnoreCase) > -1)
                  values6.Add(string.Format(SASResources.ErrorSpaceDescription, (object) string.Format("{0} {1}", (object) v6Strings.GroundCover_SingularName, (object) id)));
                if (sasDictionary6.ContainsKey(description.ToUpper(this.ciUsed)))
                  values6.Add(string.Format(SASResources.ErrorDupDescription, (object) string.Format("{0} {1}", (object) v6Strings.GroundCover_SingularName, (object) id)));
                else
                  sasDictionary6.Add(description.ToUpper(this.ciUsed), description.ToUpper(this.ciUsed));
                if (description.IndexOf(",", StringComparison.InvariantCultureIgnoreCase) > -1)
                  values6.Add(string.Format(SASResources.ErrorCommaDescription, (object) string.Format("{0} {1}", (object) v6Strings.GroundCover_SingularName, (object) id)));
              }
            }
            // ISSUE: reference to a compiler-generated field
            cDisplayClass870.strataType = (Strata) null;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass870.plotType = (Plot) null;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass870.treeType = (Tree) null;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass870.shrubType = (Shrub) null;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass870.plotLandUseType = (PlotLandUse) null;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass870.plotGroundCoverType = (PlotGroundCover) null;
            if (this.currSeries.SampleType == SampleType.Inventory)
            {
              IList<string> speciesCodes = this.project_s.QueryOver<Tree>().JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (tr => tr.Plot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (pl => pl.Year == this.currYear)).Select(Projections.Distinct((IProjection) Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => tr.Species)))).List<string>();
              if (speciesCodes != null && speciesCodes.Count != 0)
              {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                SASProcessor.\u003C\u003Ec__DisplayClass87_1 cDisplayClass871 = new SASProcessor.\u003C\u003Ec__DisplayClass87_1();
                // ISSUE: reference to a compiler-generated field
                cDisplayClass871.InvalidSppCode = this.checkSpecies(speciesCodes);
                // ISSUE: reference to a compiler-generated field
                if (cDisplayClass871.InvalidSppCode.Count > 0)
                {
                  string str6 = "";
                  // ISSUE: reference to a compiler-generated field
                  for (int index = 0; index < cDisplayClass871.InvalidSppCode.Count; ++index)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    str6 = index != 0 ? str6 + ",'" + cDisplayClass871.InvalidSppCode.ElementAt<string>(index) + "'" : "'" + cDisplayClass871.InvalidSppCode.ElementAt<string>(index) + "'";
                  }
                  IQueryOver<Tree, Tree> queryOver = this.project_s.QueryOver<Tree>();
                  // ISSUE: reference to a compiler-generated field
                  System.Linq.Expressions.Expression<Func<Tree, bool>> expression = (System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Species.IsIn(cDisplayClass871.InvalidSppCode));
                  foreach (Tree tree in (IEnumerable<Tree>) queryOver.Where(expression).List<Tree>())
                    values7.Add(string.Format(SASResources.ErrorInvalidSpecies, (object) string.Format("{0} {1}", (object) v6Strings.Tree_SingularName, (object) tree.Id), (object) tree.Species));
                }
              }
              else
                values7.Add(string.Format(SASResources.ErrorNoData, (object) v6Strings.Tree_SingularName));
              foreach ((int, int, double) tuple in (IEnumerable<(int, int, double)>) this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider().GetSQLQueryOfLargeDBHTrees(this.currYear.RecordTreeStatus ? this.ClauseOfNonRemovedTrees : "", false, this.currYear.DBHActual).SetGuid("YearKey", this.currYear.Guid).SetDouble("MaxDbhSqaure", num1 * num1).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, int, double)>()).List<(int, int, double)>())
                values12.Add(string.Format("The tree (TreeId: {0}) has DBH (or combined DBH from multiple stems) set at {1}. Please verify this is correct.", (object) tuple.Item2, (object) Math.Round(Math.Sqrt(tuple.Item3), 1)));
              if (this.currYear.RecordTreeStatus)
              {
                IOrderedQueryable<Tree> source = this.project_s.Query<Tree>().Where<Tree>((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => !this.AllTreeStatus.Contains<char>(t.Status))).Where<Tree>((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Plot.Year == this.currYear)).OrderBy<Tree, int>((System.Linq.Expressions.Expression<Func<Tree, int>>) (t => t.Id));
                System.Linq.Expressions.Expression<Func<Tree, int>> selector = (System.Linq.Expressions.Expression<Func<Tree, int>>) (t => t.Id);
                foreach (int num3 in (IEnumerable<int>) source.Select<Tree, int>(selector).ToList<int>())
                  values7.Add(string.Format(SASResources.ErrorInvalidTreeStatus, (object) string.Format("{0} {1}", (object) v6Strings.Tree_SingularName, (object) num3)));
              }
              if (this.currYear.RecordCrownCondition)
              {
                foreach (int tree in (IEnumerable<int>) this.FindTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Crown.Condition == default (object) || t.Crown.Condition.PctDieback < 0.0)))
                  values7.Add(string.Format(SASResources.ErrorInvalidCrownHealth, (object) string.Format("{0} {1}", (object) v6Strings.Tree_SingularName, (object) tree)));
              }
              if (this.currYear.RecordHeight)
              {
                foreach (int tree in (IEnumerable<int>) this.FindTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.TreeHeight <= 0.0f)))
                  values7.Add(string.Format(SASResources.ErrorInvalidHeight, (object) string.Format("{0} {1}", (object) v6Strings.Tree_SingularName, (object) tree)));
              }
              if (this.currYear.RecordCrownSize)
              {
                foreach (int tree in (IEnumerable<int>) this.FindTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => (int) t.Crown.PercentMissing != 100 && (t.Crown.TopHeight <= 0.0f || t.Crown.WidthEW <= 0.0f || t.Crown.WidthNS <= 0.0f))))
                  values7.Add(string.Format(SASResources.ErrorInvalidCrownSize, (object) string.Format("{0} {1}", (object) v6Strings.Tree_SingularName, (object) tree)));
                foreach (int tree in (IEnumerable<int>) this.FindTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => (int) t.Crown.PercentMissing != 100 && t.Crown.BaseHeight < 0.0f)))
                  values7.Add(string.Format(SASResources.ErrorInvalidCrownBaseHeight, (object) string.Format("{0} {1}", (object) v6Strings.Tree_SingularName, (object) tree)));
                foreach (int tree in (IEnumerable<int>) this.FindTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => (int) t.Crown.PercentMissing != 100 && t.Crown.BaseHeight >= t.Crown.TopHeight)))
                  values7.Add(string.Format(SASResources.ErrorInvalidCrownTopHeight, (object) string.Format("{0} {1}", (object) v6Strings.Tree_SingularName, (object) tree)));
                foreach (int tree in (IEnumerable<int>) this.FindTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => (int) t.Crown.PercentMissing < 0)))
                  values7.Add(string.Format(SASResources.ErrorInvalidCrownPercentMissing, (object) string.Format("{0} {1}", (object) v6Strings.Tree_SingularName, (object) tree)));
              }
              if (this.currYear.RecordCrownSize && this.currYear.RecordHeight)
              {
                foreach (int tree in (IEnumerable<int>) this.FindTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Crown.TopHeight > t.TreeHeight)))
                  values7.Add(string.Format(SASResources.ErrorInvalidHeight, (object) string.Format("{0} {1}", (object) v6Strings.Tree_SingularName, (object) tree)));
              }
              ParameterExpression parameterExpression1 = System.Linq.Expressions.Expression.Parameter(typeof (Tree), "t");
              // ISSUE: method reference
              // ISSUE: method reference
              // ISSUE: type reference
              BinaryExpression left = System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Tree.get_Stems))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (ICollection<Stem>.get_Count), __typeref (ICollection<Stem>))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) 0, typeof (int)));
              // ISSUE: method reference
              // ISSUE: method reference
              MethodCallExpression right = System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new System.Linq.Expressions.Expression[2]
              {
                (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Tree.get_Stems))),
                (System.Linq.Expressions.Expression) (s => s.Diameter <= 0.0)
              });
              foreach (int tree in (IEnumerable<int>) this.FindTrees(System.Linq.Expressions.Expression.Lambda<Func<Tree, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.OrElse((System.Linq.Expressions.Expression) left, (System.Linq.Expressions.Expression) right), parameterExpression1)))
                values7.Add(string.Format(SASResources.ErrorInvalidStem, (object) string.Format("{0} {1}", (object) v6Strings.Tree_SingularName, (object) tree)));
              if (!this.currYear.DBHActual)
              {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                SASProcessor.\u003C\u003Ec__DisplayClass87_2 cDisplayClass872 = new SASProcessor.\u003C\u003Ec__DisplayClass87_2();
                // ISSUE: reference to a compiler-generated field
                cDisplayClass872.dbhs = this.currYear.DBHs.Select<DBH, double>((Func<DBH, double>) (d => d.DBHId)).ToArray<double>();
                // ISSUE: reference to a compiler-generated field
                foreach ((int TreeId, int StemId) treeStem in (IEnumerable<(int TreeId, int StemId)>) this.FindTreeStems((System.Linq.Expressions.Expression<Func<Stem, bool>>) (st => !cDisplayClass872.dbhs.Contains<double>(st.Diameter))))
                  values7.Add(string.Format(SASResources.ErrorInvalidDBH, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Tree_SingularName, (object) treeStem.TreeId, (object) v6Strings.Stem_SingularName, (object) treeStem.StemId)));
              }
              if (this.currYear.RecordLanduse)
              {
                foreach (int tree in (IEnumerable<int>) this.FindTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.PlotLandUse == default (object) || t.Plot != t.PlotLandUse.Plot)))
                  values7.Add(string.Format(SASResources.ErrorInvalidLandUse, (object) string.Format("{0} {1}", (object) v6Strings.Tree_SingularName, (object) tree)));
              }
              if (this.currYear.RecordEnergy)
              {
                ParameterExpression parameterExpression2 = System.Linq.Expressions.Expression.Parameter(typeof (Tree), "t");
                // ISSUE: method reference
                MethodInfo methodFromHandle = (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any));
                // ISSUE: method reference
                System.Linq.Expressions.Expression[] expressionArray = new System.Linq.Expressions.Expression[2]
                {
                  (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression2, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Tree.get_Buildings))),
                  (System.Linq.Expressions.Expression) (b => (int) b.Direction < 0 || (int) b.Direction > 360 || b.Distance <= 0.0f)
                };
                foreach (int tree in (IEnumerable<int>) this.FindTrees(System.Linq.Expressions.Expression.Lambda<Func<Tree, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, methodFromHandle, expressionArray), parameterExpression2)))
                  values7.Add(string.Format(SASResources.ErrorInvalidBuilding, (object) string.Format("{0} {1}", (object) v6Strings.Tree_SingularName, (object) tree)));
              }
            }
            if (this.currSeries.SampleType == SampleType.RegularPlot)
            {
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              SASProcessor.\u003C\u003Ec__DisplayClass87_3 cDisplayClass873 = new SASProcessor.\u003C\u003Ec__DisplayClass87_3();
              // ISSUE: reference to a compiler-generated field
              cDisplayClass873.CS\u0024\u003C\u003E8__locals1 = cDisplayClass870;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              IQueryOver<Plot, Strata> queryOver1 = this.project_s.QueryOver<Plot>((System.Linq.Expressions.Expression<Func<Plot>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear)).JoinQueryOver<Strata>((System.Linq.Expressions.Expression<Func<Plot, Strata>>) (p => p.Strata), (System.Linq.Expressions.Expression<Func<Strata>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.strataType));
              System.Linq.Expressions.Expression<Func<Strata, bool>> expression1 = (System.Linq.Expressions.Expression<Func<Strata, bool>>) (s => s.Year == this.currYear);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              foreach ((int, double, double) tuple in (IEnumerable<(int, double, double)>) queryOver1.Where(expression1).Select((IProjection) Projections.Group((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.strataType.Id)), (IProjection) Projections.Group((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.strataType.Size)), (IProjection) Projections.Sum((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType.Size))).TransformUsing((IResultTransformer) new TupleResultTransformer<(int, double, double)>()).List<(int, double, double)>())
              {
                if (tuple.Item3 > tuple.Item2)
                  values4.Add(string.Format(SASResources.ErrorStrataPlotArea, (object) string.Format("{0} {1}", (object) v6Strings.Strata_SingularName, (object) tuple.Item1)));
              }
              bool flag3 = true;
              bool flag4 = true;
              bool flag5 = true;
              bool flag6 = true;
              IList<string> stringList = this.project_s.QueryOver<Tree>().JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (tr => tr.Plot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (pl => pl.Year == this.currYear && pl.IsComplete == true)).Select(Projections.Distinct((IProjection) Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => tr.Species)))).List<string>();
              if (stringList != null && stringList.Count != 0)
                flag4 = false;
              IList<string> speciesCodes1 = this.project_s.QueryOver<Tree>().JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (tr => tr.Plot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (pl => pl.Year == this.currYear)).Select(Projections.Distinct((IProjection) Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => tr.Species)))).List<string>();
              if (speciesCodes1 != null && speciesCodes1.Count != 0)
                flag3 = false;
              if (!flag3)
              {
                List<string> values15 = this.checkSpecies(speciesCodes1);
                if (values15.Count > 0)
                {
                  if (!flag4)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    IQueryOver<Tree, Plot> queryOver2 = this.project_s.QueryOver<Tree>((System.Linq.Expressions.Expression<Func<Tree>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.treeType)).Where((ICriterion) Restrictions.On((System.Linq.Expressions.Expression<Func<object>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.treeType.Species)).IsIn((ICollection) values15)).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType));
                    System.Linq.Expressions.Expression<Func<Plot, bool>> expression2 = (System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    foreach ((int, int, string) tuple in (IEnumerable<(int, int, string)>) queryOver2.Where(expression2).Select(Projections.Distinct((IProjection) Projections.ProjectionList().Add((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType.Id))).Add((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.treeType.Id))).Add((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.treeType.Species))))).TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int, string)>()).List<(int, int, string)>())
                      values7.Add(string.Format(SASResources.ErrorInvalidSpecies, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) tuple.Item1, (object) v6Strings.Tree_SingularName, (object) tuple.Item2), (object) tuple.Item3));
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  IQueryOver<Tree, Plot> queryOver3 = this.project_s.QueryOver<Tree>((System.Linq.Expressions.Expression<Func<Tree>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.treeType)).Where((ICriterion) Restrictions.On((System.Linq.Expressions.Expression<Func<object>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.treeType.Species)).IsIn((ICollection) values15)).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType));
                  System.Linq.Expressions.Expression<Func<Plot, bool>> expression3 = (System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == false);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  foreach ((int, int, string) tuple in (IEnumerable<(int, int, string)>) queryOver3.Where(expression3).Select(Projections.Distinct((IProjection) Projections.ProjectionList().Add((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType.Id))).Add((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.treeType.Id))).Add((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.treeType.Species))))).TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int, string)>()).List<(int, int, string)>())
                    values12.Add(string.Format(SASResources.ErrorInvalidSpecies, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) tuple.Item1, (object) v6Strings.Tree_SingularName, (object) tuple.Item2), (object) tuple.Item3));
                }
              }
              if (this.currYear.RecordShrub)
              {
                if (this.project_s.QueryOver<Shrub>().JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Shrub, Plot>>) (tr => tr.Plot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (pl => pl.Year == this.currYear && pl.IsComplete == true)).Select(Projections.Distinct((IProjection) Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => tr.Species)))).List<string>() != null)
                  flag6 = false;
                IList<string> speciesCodes2 = this.project_s.QueryOver<Shrub>().JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Shrub, Plot>>) (tr => tr.Plot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (pl => pl.Year == this.currYear)).Select(Projections.Distinct((IProjection) Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => tr.Species)))).List<string>();
                if (speciesCodes2 != null)
                  flag5 = false;
                if (!flag5)
                {
                  List<string> values16 = this.checkSpecies(speciesCodes2);
                  if (values16.Count > 0)
                  {
                    if (!flag6)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      IQueryOver<Shrub, Plot> queryOver4 = this.project_s.QueryOver<Shrub>((System.Linq.Expressions.Expression<Func<Shrub>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.shrubType)).Where((ICriterion) Restrictions.On((System.Linq.Expressions.Expression<Func<object>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.shrubType.Species)).IsIn((ICollection) values16)).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Shrub, Plot>>) (t => t.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType));
                      System.Linq.Expressions.Expression<Func<Plot, bool>> expression4 = (System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true);
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      foreach ((int, int, string) tuple in (IEnumerable<(int, int, string)>) queryOver4.Where(expression4).Select(Projections.Distinct((IProjection) Projections.ProjectionList().Add((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType.Id))).Add((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.shrubType.Id))).Add((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.shrubType.Species))))).TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int, string)>()).List<(int, int, string)>())
                        values13.Add(string.Format(SASResources.ErrorInvalidSpecies, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) tuple.Item1, (object) v6Strings.Shrub_SingularName, (object) tuple.Item2), (object) tuple.Item3));
                    }
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    IQueryOver<Shrub, Plot> queryOver5 = this.project_s.QueryOver<Shrub>((System.Linq.Expressions.Expression<Func<Shrub>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.shrubType)).Where((ICriterion) Restrictions.On((System.Linq.Expressions.Expression<Func<object>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.shrubType.Species)).IsIn((ICollection) values16)).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Shrub, Plot>>) (t => t.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType));
                    System.Linq.Expressions.Expression<Func<Plot, bool>> expression5 = (System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == false);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    foreach ((int, int, string) tuple in (IEnumerable<(int, int, string)>) queryOver5.Where(expression5).Select(Projections.Distinct((IProjection) Projections.ProjectionList().Add((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType.Id))).Add((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.shrubType.Id))).Add((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.shrubType.Species))))).TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int, string)>()).List<(int, int, string)>())
                      values14.Add(string.Format(SASResources.ErrorInvalidSpecies, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) tuple.Item1, (object) v6Strings.Shrub_SingularName, (object) tuple.Item2), (object) tuple.Item3));
                  }
                }
              }
              if (flag4 & flag6)
                values8.Add(SASResources.ErrorPlotsNoData);
              SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
              foreach ((int, int, double) tuple in (IEnumerable<(int, int, double)>) sasUtilProvider.GetSQLQueryOfLargeDBHTrees(this.currYear.RecordTreeStatus ? this.ClauseOfNonRemovedTrees : "", true, this.currYear.DBHActual).SetGuid("YearKey", this.currYear.Guid).SetDouble("MaxDbhSqaure", num1 * num1).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, int, double)>()).List<(int, int, double)>())
                values12.Add(string.Format("The tree (PlotId: {0},TreeId: {1}) has DBH (or combined DBH from multiple stems) set at {2}. Please verify this is correct.", (object) tuple.Item1, (object) tuple.Item2, (object) Math.Round(Math.Sqrt(tuple.Item3), 1)));
              IQueryOver<Plot, Plot> queryOver6 = this.project_s.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true && (int) p.PercentTreeCover < 0)).Select((IProjection) Projections.Property<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)));
              System.Linq.Expressions.Expression<Func<Plot, object>> path1 = (System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id);
              foreach (int num4 in (IEnumerable<int>) queryOver6.OrderBy(path1).Asc.List<int>())
                values8.Add(string.Format(SASResources.ErrorPlotPercentTreeCover, (object) string.Format("{0} {1}", (object) v6Strings.Plot_SingularName, (object) num4)));
              IList<(int, int, int)> tupleList1 = sasUtilProvider.GetSQLQueryOfPlotsWithSusspectedTreePercent(this.currYear.RecordTreeStatus ? this.ClauseOfNonRemovedTrees : "", this.currYear.RecordCrownCondition).SetGuid("YearKey", this.currYear.Guid).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, int, int)>()).List<(int, int, int)>();
              bool flag7 = true;
              foreach ((int, int, int) tuple in (IEnumerable<(int, int, int)>) tupleList1)
              {
                if (tuple.Item2 > 0)
                  flag7 = false;
                if (tuple.Item3 != 0 && tuple.Item2 == 0)
                  values10.Add(string.Format(SASResources.WarningPlotPercentTreeCover, (object) string.Format("{0} {1}", (object) v6Strings.Plot_SingularName, (object) tuple.Item1)));
              }
              if (flag7)
                values8.Add(SASResources.ErrorPlotNoTreeCover);
              if (this.currYear.RecordPercentShrub)
              {
                IQueryOver<Plot, Plot> queryOver7 = this.project_s.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true && (int) p.PercentShrubCover < 0)).Select((IProjection) Projections.Property<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)));
                System.Linq.Expressions.Expression<Func<Plot, object>> path2 = (System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id);
                foreach (int num5 in (IEnumerable<int>) queryOver7.OrderBy(path2).Asc.List<int>())
                  values8.Add(string.Format(SASResources.ErrorPlotPercentShrubCover, (object) string.Format("{0} {1}", (object) v6Strings.Plot_SingularName, (object) num5)));
              }
              if (this.currYear.RecordPlantableSpace)
              {
                IQueryOver<Plot, Plot> queryOver8 = this.project_s.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true && (int) p.PercentPlantable < 0)).Select((IProjection) Projections.Property<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)));
                System.Linq.Expressions.Expression<Func<Plot, object>> path3 = (System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id);
                foreach (int num6 in (IEnumerable<int>) queryOver8.OrderBy(path3).Asc.List<int>())
                  values8.Add(string.Format(SASResources.ErrorPlotPercentPlantableSpace, (object) string.Format("{0} {1}", (object) v6Strings.Plot_SingularName, (object) num6)));
              }
              IQueryOver<Plot, Plot> queryOver9 = this.project_s.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true && (p.Size < 0.0f || p.PercentMeasured <= 0 || p.PercentMeasured > 100))).Select((IProjection) Projections.Property<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)));
              System.Linq.Expressions.Expression<Func<Plot, object>> path4 = (System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id);
              foreach (int num7 in (IEnumerable<int>) queryOver9.OrderBy(path4).Asc.List<int>())
                values8.Add(string.Format(SASResources.ErrorPlotPercentMeasured, (object) string.Format("{0} {1}", (object) v6Strings.Plot_SingularName, (object) num7)));
              IList<Guid> guidList = this.project_s.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true)).Select(Projections.Distinct((IProjection) Projections.Property<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Strata.Guid)))).List<Guid>();
              // ISSUE: reference to a compiler-generated field
              cDisplayClass873.plotStrataKeys = guidList;
              // ISSUE: reference to a compiler-generated field
              IQueryOver<Strata, Strata> queryOver10 = this.project_s.QueryOver<Strata>().Where((System.Linq.Expressions.Expression<Func<Strata, bool>>) (s => s.Year == this.currYear && !s.Guid.IsIn(cDisplayClass873.plotStrataKeys.ToList<Guid>())));
              IProjection[] projectionArray = new IProjection[1]
              {
                (IProjection) Projections.Property<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => (object) s.Id))
              };
              foreach (int num8 in (IEnumerable<int>) queryOver10.Select(projectionArray).List<int>())
                values4.Add(string.Format(SASResources.ErrorStrataNoCompletePlots, (object) string.Format("{0} {1}", (object) v6Strings.Strata_SingularName, (object) num8)));
              int num9 = this.project_s.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == false)).Select((IProjection) Projections.Count<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id))).SingleOrDefault<int>();
              if (num9 != 0)
                values10.Add(string.Format(SASResources.WarningPlotsIncomplete, (object) num9));
              if (this.currYear.RecordTreeStatus)
              {
                IOrderedQueryable<Tree> source = this.project_s.Query<Tree>().Where<Tree>((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => (int?) (int) t.Status == new int?() || !this.AllTreeStatus.Contains<char>(t.Status))).Where<Tree>((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Plot.Year == this.currYear && t.Plot.IsComplete == true)).OrderBy<Tree, int>((System.Linq.Expressions.Expression<Func<Tree, int>>) (t => t.Plot.Id)).ThenBy<Tree, int>((System.Linq.Expressions.Expression<Func<Tree, int>>) (t => t.Id));
                System.Linq.Expressions.Expression<Func<Tree, (int, int)>> selector = (System.Linq.Expressions.Expression<Func<Tree, (int, int)>>) (t => ValueTuple.Create<int, int>(t.Plot.Id, t.Id));
                foreach ((int, int) tuple in (IEnumerable<(int, int)>) source.Select<Tree, (int, int)>(selector).ToList<(int, int)>())
                  values7.Add(string.Format(SASResources.ErrorInvalidTreeStatus, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) tuple.Item1, (object) v6Strings.Tree_SingularName, (object) tuple.Item2)));
              }
              if (this.currYear.RecordCrownCondition)
              {
                foreach ((int PlotId, int TreeId) plotTree in (IEnumerable<(int PlotId, int TreeId)>) this.FindPlotTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Crown.Condition == default (object) || t.Crown.Condition.PctDieback < 0.0)))
                  values7.Add(string.Format(SASResources.ErrorInvalidCrownHealth, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) plotTree.PlotId, (object) v6Strings.Tree_SingularName, (object) plotTree.TreeId)));
              }
              if (this.currYear.RecordHeight)
              {
                foreach ((int PlotId, int TreeId) plotTree in (IEnumerable<(int PlotId, int TreeId)>) this.FindPlotTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.TreeHeight <= 0.0f)))
                  values7.Add(string.Format(SASResources.ErrorInvalidHeight, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) plotTree.PlotId, (object) v6Strings.Tree_SingularName, (object) plotTree.TreeId)));
              }
              if (this.currYear.RecordCrownSize)
              {
                foreach ((int PlotId, int TreeId) plotTree in (IEnumerable<(int PlotId, int TreeId)>) this.FindPlotTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => (int) t.Crown.PercentMissing != 100 && (t.Crown.TopHeight <= 0.0f || t.Crown.WidthEW <= 0.0f || t.Crown.WidthNS <= 0.0f))))
                  values7.Add(string.Format(SASResources.ErrorInvalidCrownSize, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) plotTree.PlotId, (object) v6Strings.Tree_SingularName, (object) plotTree.TreeId)));
                foreach ((int PlotId, int TreeId) plotTree in (IEnumerable<(int PlotId, int TreeId)>) this.FindPlotTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => (int) t.Crown.PercentMissing != 100 && t.Crown.BaseHeight < 0.0f)))
                  values7.Add(string.Format(SASResources.ErrorInvalidCrownBaseHeight, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) plotTree.PlotId, (object) v6Strings.Tree_SingularName, (object) plotTree.TreeId)));
                foreach ((int PlotId, int TreeId) plotTree in (IEnumerable<(int PlotId, int TreeId)>) this.FindPlotTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => (int) t.Crown.PercentMissing != 100 && t.Crown.BaseHeight >= t.Crown.TopHeight)))
                  values12.Add(string.Format(SASResources.ErrorInvalidCrownTopHeight, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) plotTree.PlotId, (object) v6Strings.Tree_SingularName, (object) plotTree.TreeId)));
                foreach ((int PlotId, int TreeId) plotTree in (IEnumerable<(int PlotId, int TreeId)>) this.FindPlotTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => (int) t.Crown.PercentMissing < 0)))
                  values7.Add(string.Format(SASResources.ErrorInvalidCrownPercentMissing, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) plotTree.PlotId, (object) v6Strings.Tree_SingularName, (object) plotTree.TreeId)));
              }
              if (this.currYear.RecordCrownSize && this.currYear.RecordHeight)
              {
                foreach ((int PlotId, int TreeId) plotTree in (IEnumerable<(int PlotId, int TreeId)>) this.FindPlotTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Crown.TopHeight > t.TreeHeight)))
                  values7.Add(string.Format(SASResources.ErrorInvalidHeight, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) plotTree.PlotId, (object) v6Strings.Tree_SingularName, (object) plotTree.TreeId)));
              }
              ParameterExpression parameterExpression3 = System.Linq.Expressions.Expression.Parameter(typeof (Tree), "t");
              // ISSUE: method reference
              // ISSUE: method reference
              // ISSUE: type reference
              BinaryExpression left1 = System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression3, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Tree.get_Stems))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (ICollection<Stem>.get_Count), __typeref (ICollection<Stem>))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) 0, typeof (int)));
              // ISSUE: method reference
              // ISSUE: method reference
              MethodCallExpression right1 = System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new System.Linq.Expressions.Expression[2]
              {
                (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression3, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Tree.get_Stems))),
                (System.Linq.Expressions.Expression) (s => s.Diameter <= 0.0)
              });
              foreach ((int PlotId, int TreeId) plotTree in (IEnumerable<(int PlotId, int TreeId)>) this.FindPlotTrees(System.Linq.Expressions.Expression.Lambda<Func<Tree, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.OrElse((System.Linq.Expressions.Expression) left1, (System.Linq.Expressions.Expression) right1), parameterExpression3)))
                values7.Add(string.Format(SASResources.ErrorInvalidStem, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) plotTree.PlotId, (object) v6Strings.Tree_SingularName, (object) plotTree.TreeId)));
              if (!this.currYear.DBHActual)
              {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                SASProcessor.\u003C\u003Ec__DisplayClass87_4 cDisplayClass874 = new SASProcessor.\u003C\u003Ec__DisplayClass87_4();
                // ISSUE: reference to a compiler-generated field
                cDisplayClass874.dbhs = this.currYear.DBHs.Select<DBH, double>((Func<DBH, double>) (d => d.DBHId)).ToArray<double>();
                // ISSUE: reference to a compiler-generated field
                foreach ((int PlotId, int TreeId, int StemId) plotTreeStem in (IEnumerable<(int PlotId, int TreeId, int StemId)>) this.FindPlotTreeStems((System.Linq.Expressions.Expression<Func<Stem, bool>>) (st => !cDisplayClass874.dbhs.Contains<double>(st.Diameter))))
                  values7.Add(string.Format(SASResources.ErrorInvalidDBH, (object) string.Format("{0} {1}, {2} {3}, {4} {5}", (object) v6Strings.Plot_SingularName, (object) plotTreeStem.PlotId, (object) v6Strings.Tree_SingularName, (object) plotTreeStem.TreeId, (object) v6Strings.Stem_SingularName, (object) plotTreeStem.StemId)));
              }
              if (this.currYear.RecordLanduse)
              {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                SASProcessor.\u003C\u003Ec__DisplayClass87_5 cDisplayClass875 = new SASProcessor.\u003C\u003Ec__DisplayClass87_5();
                foreach ((int PlotId, int TreeId) plotTree in (IEnumerable<(int PlotId, int TreeId)>) this.FindPlotTrees((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.PlotLandUse == default (object) || t.Plot != t.PlotLandUse.Plot)))
                  values7.Add(string.Format(SASResources.ErrorInvalidLandUse, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) plotTree.PlotId, (object) v6Strings.Tree_SingularName, (object) plotTree.TreeId)));
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                IList<(int, int)> tupleList2 = this.project_s.QueryOver<PlotLandUse>((System.Linq.Expressions.Expression<Func<PlotLandUse>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotLandUseType)).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<PlotLandUse, Plot>>) (plu => plu.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true)).Select((IProjection) Projections.Group((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType.Id)), (IProjection) Projections.Sum((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotLandUseType.PercentOfPlot))).TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int)>()).List<(int, int)>();
                // ISSUE: reference to a compiler-generated field
                cDisplayClass875.lstPlotIdOfWrongPercent = new List<int>();
                foreach ((int, int) tuple in (IEnumerable<(int, int)>) tupleList2)
                {
                  if (tuple.Item2 != 100)
                  {
                    // ISSUE: reference to a compiler-generated field
                    cDisplayClass875.lstPlotIdOfWrongPercent.Add(tuple.Item1);
                    values8.Add(string.Format(SASResources.ErrorPlotTotalLandUse, (object) string.Format("{0} {1}", (object) v6Strings.Plot_SingularName, (object) tuple.Item1)));
                  }
                }
                // ISSUE: reference to a compiler-generated field
                if (cDisplayClass875.lstPlotIdOfWrongPercent.Count == 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  cDisplayClass875.lstPlotIdOfWrongPercent.Add(-100);
                }
                // ISSUE: reference to a compiler-generated field
                foreach (int plot in (IEnumerable<int>) this.FindPlots((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => !cDisplayClass875.lstPlotIdOfWrongPercent.Contains(p.Id) && p.PlotLandUses.Count == 0)))
                {
                  // ISSUE: reference to a compiler-generated field
                  cDisplayClass875.lstPlotIdOfWrongPercent.Add(plot);
                  values8.Add(string.Format(SASResources.ErrorPlotNoLandUses, (object) string.Format("{0} {1}", (object) v6Strings.Plot_SingularName, (object) plot)));
                }
                ParameterExpression parameterExpression4 = System.Linq.Expressions.Expression.Parameter(typeof (Plot), "p");
                // ISSUE: field reference
                // ISSUE: method reference
                // ISSUE: type reference
                // ISSUE: method reference
                UnaryExpression left2 = System.Linq.Expressions.Expression.Not((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Field((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) cDisplayClass875, typeof (SASProcessor.\u003C\u003Ec__DisplayClass87_5)), FieldInfo.GetFieldFromHandle(__fieldref (SASProcessor.\u003C\u003Ec__DisplayClass87_5.lstPlotIdOfWrongPercent))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (List<int>.Contains), __typeref (List<int>)), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression4, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Plot.get_Id)))));
                // ISSUE: method reference
                // ISSUE: method reference
                MethodCallExpression right2 = System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new System.Linq.Expressions.Expression[2]
                {
                  (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression4, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Plot.get_PlotLandUses))),
                  (System.Linq.Expressions.Expression) (ls => (int) ls.PercentOfPlot <= 0)
                });
                foreach (int plot in (IEnumerable<int>) this.FindPlots(System.Linq.Expressions.Expression.Lambda<Func<Plot, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.AndAlso((System.Linq.Expressions.Expression) left2, (System.Linq.Expressions.Expression) right2), parameterExpression4)))
                  values8.Add(string.Format(SASResources.ErrorInvalidLandUse, (object) string.Format("{0} {1}", (object) v6Strings.Plot_SingularName, (object) plot)));
              }
              if (this.currYear.RecordGroundCover)
              {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                SASProcessor.\u003C\u003Ec__DisplayClass87_6 cDisplayClass876 = new SASProcessor.\u003C\u003Ec__DisplayClass87_6();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                IList<(int, int)> tupleList3 = this.project_s.QueryOver<PlotGroundCover>((System.Linq.Expressions.Expression<Func<PlotGroundCover>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotGroundCoverType)).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<PlotGroundCover, Plot>>) (plu => plu.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true)).Select((IProjection) Projections.Group((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotType.Id)), (IProjection) Projections.Sum((System.Linq.Expressions.Expression<Func<object>>) (() => (object) cDisplayClass873.CS\u0024\u003C\u003E8__locals1.plotGroundCoverType.PercentCovered))).TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int)>()).List<(int, int)>();
                // ISSUE: reference to a compiler-generated field
                cDisplayClass876.lstPlotIdOfWrongPercent = new List<int>();
                foreach ((int, int) tuple in (IEnumerable<(int, int)>) tupleList3)
                {
                  if (tuple.Item2 != 100)
                  {
                    // ISSUE: reference to a compiler-generated field
                    cDisplayClass876.lstPlotIdOfWrongPercent.Add(tuple.Item1);
                    values8.Add(string.Format(SASResources.ErrorPlotTotalGroundCover, (object) string.Format("{0} {1}", (object) v6Strings.Plot_SingularName, (object) tuple.Item1)));
                  }
                }
                // ISSUE: reference to a compiler-generated field
                if (cDisplayClass876.lstPlotIdOfWrongPercent.Count == 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  cDisplayClass876.lstPlotIdOfWrongPercent.Add(-100);
                }
                // ISSUE: reference to a compiler-generated field
                foreach (int plot in (IEnumerable<int>) this.FindPlots((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => !cDisplayClass876.lstPlotIdOfWrongPercent.Contains(p.Id) && p.PlotGroundCovers.Count == 0)))
                {
                  // ISSUE: reference to a compiler-generated field
                  cDisplayClass876.lstPlotIdOfWrongPercent.Add(plot);
                  values8.Add(string.Format(SASResources.ErrorPlotNoGroundCover, (object) string.Format("{0} {1}", (object) v6Strings.Plot_SingularName, (object) plot)));
                }
                ParameterExpression parameterExpression5 = System.Linq.Expressions.Expression.Parameter(typeof (Plot), "p");
                // ISSUE: field reference
                // ISSUE: method reference
                // ISSUE: type reference
                // ISSUE: method reference
                UnaryExpression left3 = System.Linq.Expressions.Expression.Not((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Field((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) cDisplayClass876, typeof (SASProcessor.\u003C\u003Ec__DisplayClass87_6)), FieldInfo.GetFieldFromHandle(__fieldref (SASProcessor.\u003C\u003Ec__DisplayClass87_6.lstPlotIdOfWrongPercent))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (List<int>.Contains), __typeref (List<int>)), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression5, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Plot.get_Id)))));
                // ISSUE: method reference
                // ISSUE: method reference
                MethodCallExpression right3 = System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any)), new System.Linq.Expressions.Expression[2]
                {
                  (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression5, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Plot.get_PlotGroundCovers))),
                  (System.Linq.Expressions.Expression) (gc => gc.PercentCovered <= 0)
                });
                foreach (int plot in (IEnumerable<int>) this.FindPlots(System.Linq.Expressions.Expression.Lambda<Func<Plot, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.AndAlso((System.Linq.Expressions.Expression) left3, (System.Linq.Expressions.Expression) right3), parameterExpression5)))
                  values8.Add(string.Format(SASResources.ErrorPlotGroundCover, (object) string.Format("{0} {1}", (object) v6Strings.Plot_SingularName, (object) plot)));
              }
              if (this.currYear.RecordEnergy)
              {
                ParameterExpression parameterExpression6 = System.Linq.Expressions.Expression.Parameter(typeof (Tree), "t");
                // ISSUE: method reference
                MethodInfo methodFromHandle = (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Enumerable.Any));
                // ISSUE: method reference
                System.Linq.Expressions.Expression[] expressionArray = new System.Linq.Expressions.Expression[2]
                {
                  (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression6, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Tree.get_Buildings))),
                  (System.Linq.Expressions.Expression) (b => (int) b.Direction < 0 || (int) b.Direction > 360 || b.Distance <= 0.0f)
                };
                foreach ((int PlotId, int TreeId) plotTree in (IEnumerable<(int PlotId, int TreeId)>) this.FindPlotTrees(System.Linq.Expressions.Expression.Lambda<Func<Tree, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, methodFromHandle, expressionArray), parameterExpression6)))
                  values7.Add(string.Format(SASResources.ErrorInvalidBuilding, (object) string.Format("{0} {1}, {2} {3}", (object) v6Strings.Plot_SingularName, (object) plotTree.PlotId, (object) v6Strings.Tree_SingularName, (object) plotTree.TreeId)));
              }
            }
            if (values1.Count > 0)
            {
              fatalError = fatalError + string.Format(SASResources.FmtErrorProjectSettings, (object) SASResources.SevertityError, (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values1))) + "\r\n\r\n";
              flag1 = true;
            }
            if (values2.Count > 0)
            {
              fatalError = fatalError + string.Format(SASResources.FmtErrorProjectLocation, (object) SASResources.SevertityError, (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values2))) + "\r\n\r\n";
              flag1 = true;
            }
            if (values3.Count > 0)
            {
              fatalError = fatalError + string.Format(SASResources.FmtErrorBenefitPrices, (object) SASResources.SevertityError, (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values3))) + "\r\n\r\n";
              flag1 = true;
            }
            if (values4.Count > 0)
            {
              fatalError = fatalError + string.Format(SASResources.FmtErrorStrata, new object[2]
              {
                (object) SASResources.SevertityError,
                (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values4))
              }) + "\r\n\r\n";
              flag1 = true;
            }
            if (values5.Count > 0)
            {
              fatalError = fatalError + string.Format(SASResources.FmtErrorLandUses, (object) SASResources.SevertityError, (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values5))) + "\r\n\r\n";
              flag1 = true;
            }
            if (values6.Count > 0)
            {
              fatalError = fatalError + string.Format(SASResources.FmtErrorGroundCovers, (object) SASResources.SevertityError, (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values6))) + "\r\n\r\n";
              flag1 = true;
            }
            if (values8.Count > 0)
            {
              fatalError = fatalError + string.Format(SASResources.FmtErrorPlots, (object) SASResources.SevertityError, (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values8))) + "\r\n\r\n";
              flag1 = true;
            }
            if (values7.Count > 0)
            {
              fatalError = fatalError + string.Format(SASResources.FmtErrorTrees, (object) SASResources.SevertityError, (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values7))) + "\r\n\r\n";
              flag1 = true;
            }
            if (values13.Count > 0)
            {
              fatalError = fatalError + string.Format(SASResources.FmtErrorShrubs, (object) SASResources.SevertityError, (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values13))) + "\r\n\r\n";
              flag1 = true;
            }
            if (values9.Count > 0)
              warningError = warningError + string.Format(SASResources.FmtErrorProjectLocation, (object) SASResources.SeverityWarning, (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values9))) + "\r\n\r\n";
            if (values10.Count > 0)
              warningError = warningError + string.Format(SASResources.FmtErrorPlots, (object) SASResources.SeverityWarning, (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values10))) + "\r\n\r\n";
            if (values11.Count > 0)
              warningError = warningError + string.Format(SASResources.FmtErrorDbhCategory, new object[2]
              {
                (object) SASResources.SeverityWarning,
                (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values11))
              }) + "\r\n\r\n";
            if (values12.Count > 0)
              warningError = warningError + string.Format(SASResources.FmtErrorTrees, (object) SASResources.SeverityWarning, (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values12))) + "\r\n\r\n";
            if (values14.Count > 0)
              warningError = warningError + string.Format(SASResources.FmtErrorShrubs, (object) SASResources.SeverityWarning, (object) ("    " + string.Join("\r\n    ", (IEnumerable<string>) values14))) + "\r\n\r\n";
          }
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return !flag1;
    }

    private void OpenConnection(OleDbConnection conToOpen, string databaseName)
    {
      try
      {
        conToOpen.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + databaseName;
        conToOpen.Open();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, SASResources.ExportTitle, MessageBoxButtons.OK);
      }
    }

    private void CloseConnection(OleDbConnection conToClose)
    {
      if (conToClose == null || conToClose.State == ConnectionState.Closed)
        return;
      conToClose.Close();
    }

    public int getQueueNumberOfSummittedProject(string ftpUrl, string projectFile)
    {
      if (!ftpUrl.EndsWith("/", StringComparison.InvariantCultureIgnoreCase))
        ftpUrl += "/";
      string[] strArray;
      try
      {
        FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(ftpUrl);
        ftpWebRequest.KeepAlive = false;
        ftpWebRequest.Credentials = (ICredentials) new NetworkCredential("uforedatatransfer", "$ufore2data");
        ftpWebRequest.Proxy = (IWebProxy) null;
        ftpWebRequest.Method = "NLST";
        ftpWebRequest.UsePassive = true;
        FtpWebResponse response = (FtpWebResponse) ftpWebRequest.GetResponse();
        StreamReader streamReader = new StreamReader(response.GetResponseStream());
        strArray = streamReader.ReadToEnd().Replace("\n", string.Empty).Split('\r');
        streamReader.Close();
        response.Close();
      }
      catch (Exception ex)
      {
        throw ex;
      }
      List<FileNameTimeStamp> source = new List<FileNameTimeStamp>();
      foreach (string str in strArray)
      {
        if (str.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase))
        {
          try
          {
            FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(ftpUrl + str);
            ftpWebRequest.KeepAlive = false;
            ftpWebRequest.Credentials = (ICredentials) new NetworkCredential("uforedatatransfer", "$ufore2data");
            ftpWebRequest.Proxy = (IWebProxy) null;
            ftpWebRequest.Method = "MDTM";
            ftpWebRequest.UsePassive = true;
            FtpWebResponse response = (FtpWebResponse) ftpWebRequest.GetResponse();
            DateTime lastModified = response.LastModified;
            response.Close();
            FileNameTimeStamp fileNameTimeStamp;
            fileNameTimeStamp.FileName = str;
            fileNameTimeStamp.TimeStamp = lastModified;
            source.Add(fileNameTimeStamp);
          }
          catch (Exception ex)
          {
            if (ex.Message.IndexOf("550", StringComparison.InvariantCultureIgnoreCase) <= -1)
            {
              if (ex.Message.IndexOf("450", StringComparison.InvariantCultureIgnoreCase) <= -1)
                throw ex;
            }
          }
        }
      }
      List<FileNameTimeStamp> list = source.OrderBy<FileNameTimeStamp, DateTime>((Func<FileNameTimeStamp, DateTime>) (f => f.TimeStamp)).ToList<FileNameTimeStamp>();
      for (int index = 0; index < list.Count; ++index)
      {
        if (list.ElementAt<FileNameTimeStamp>(index).FileName.ToLower(this.ciUsed) == projectFile.ToLower(this.ciUsed))
          return index + 1;
      }
      return -1;
    }

    public string getExactCaseFileNameFromDownloadFolderInFTPServer(string fileName) => this.getExactCaseFileNameInFTPServer(Settings.Default.ToUsersFtpUrl, fileName);

    public string getExactCaseFileNameFromFailedFolderInFTPServer(string fileName)
    {
      string fromUsersFtpUrl = Settings.Default.FromUsersFtpUrl;
      string fileNameInFtpServer = this.getExactCaseFileNameInFTPServer(!fromUsersFtpUrl.EndsWith("/", StringComparison.InvariantCultureIgnoreCase) ? fromUsersFtpUrl + "/failed" : fromUsersFtpUrl + "failed", fileName);
      if (fileNameInFtpServer == "")
      {
        string internationalUrl = Settings.Default.FromUsersFtpInternationalUrl;
        fileNameInFtpServer = this.getExactCaseFileNameInFTPServer(!internationalUrl.EndsWith("/", StringComparison.InvariantCultureIgnoreCase) ? internationalUrl + "/failed" : internationalUrl + "failed", fileName);
      }
      return fileNameInFtpServer;
    }

    public string getExactCaseFileNameFromUploadFolderInFTPServer(string fileName) => this.getExactCaseFileNameInFTPServer(Settings.Default.FromUsersFtpUrl, fileName);

    private string getExactCaseFileNameInFTPServer(string ftpFolderUrl, string fileName)
    {
      string requestUriString = !ftpFolderUrl.EndsWith("/", StringComparison.InvariantCultureIgnoreCase) ? ftpFolderUrl + "/" + fileName : ftpFolderUrl + fileName;
      try
      {
        FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(requestUriString);
        ftpWebRequest.KeepAlive = false;
        ftpWebRequest.Credentials = (ICredentials) new NetworkCredential("uforedatatransfer", "$ufore2data");
        ftpWebRequest.Proxy = (IWebProxy) null;
        ftpWebRequest.Method = "NLST";
        ftpWebRequest.UsePassive = true;
        FtpWebResponse response = (FtpWebResponse) ftpWebRequest.GetResponse();
        StreamReader streamReader = new StreamReader(response.GetResponseStream());
        string fileNameInFtpServer = streamReader.ReadToEnd().Replace("\n", string.Empty).Replace("\r", string.Empty);
        streamReader.Close();
        response.Close();
        return fileNameInFtpServer;
      }
      catch (Exception ex)
      {
        if (ex.Message.IndexOf("550", StringComparison.InvariantCultureIgnoreCase) > -1 || ex.Message.IndexOf("450", StringComparison.InvariantCultureIgnoreCase) > -1)
          return "";
        throw ex;
      }
    }

    public bool UploadZipFile(
      IProgress<SASProgressArg> uploadProgress,
      CancellationToken uploadCancellationToken,
      SASProgressArg uploadProgressArg,
      int progressFromRange,
      int progressToRange,
      Form form)
    {
      string zipFileToSend = this.zipFileToSend;
      string str = false ? Settings.Default.FromUsersFtpInternationalUrl : Settings.Default.FromUsersFtpUrl;
      string requestUriString = !str.EndsWith("/", StringComparison.InvariantCultureIgnoreCase) ? str + "/" + Path.GetFileName(zipFileToSend) : str + Path.GetFileName(zipFileToSend);
      NetworkCredential networkCredential = new NetworkCredential("uforedatatransfer", "$ufore2data");
      FileInfo fileInfo = new FileInfo(zipFileToSend);
      long length = fileInfo.Length;
      try
      {
        using (FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read))
        {
          FtpWebRequest ftpWebRequest1 = (FtpWebRequest) WebRequest.Create(requestUriString);
          ftpWebRequest1.Credentials = (ICredentials) networkCredential;
          ftpWebRequest1.Proxy = (IWebProxy) null;
          ftpWebRequest1.UseBinary = true;
          ftpWebRequest1.Method = "STOR";
          ftpWebRequest1.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
          ftpWebRequest1.KeepAlive = false;
          ftpWebRequest1.UsePassive = true;
          int count1 = 4096;
          byte[] buffer = new byte[count1];
          int num1 = 0;
          Stream requestStream = ftpWebRequest1.GetRequestStream();
          int count2 = fileStream.Read(buffer, 0, count1);
          int num2 = num1 + count2;
          while (count2 != 0)
          {
            if (uploadCancellationToken.IsCancellationRequested)
            {
              ftpWebRequest1.Abort();
              FtpWebRequest ftpWebRequest2 = (FtpWebRequest) WebRequest.Create(requestUriString);
              ftpWebRequest2.Credentials = (ICredentials) networkCredential;
              ftpWebRequest2.Proxy = (IWebProxy) null;
              ftpWebRequest2.Method = "DELE";
              ftpWebRequest2.UsePassive = true;
              FtpWebResponse response1 = (FtpWebResponse) ftpWebRequest2.GetResponse();
              FtpWebRequest ftpWebRequest3 = (FtpWebRequest) WebRequest.Create(requestUriString + ".email.txt");
              ftpWebRequest3.Credentials = (ICredentials) networkCredential;
              ftpWebRequest3.Proxy = (IWebProxy) null;
              ftpWebRequest3.Method = "DELE";
              ftpWebRequest3.UsePassive = true;
              try
              {
                FtpWebResponse response2 = (FtpWebResponse) ftpWebRequest3.GetResponse();
                break;
              }
              catch (Exception ex)
              {
                break;
              }
            }
            else
            {
              requestStream.Write(buffer, 0, count2);
              count2 = fileStream.Read(buffer, 0, count1);
              num2 += count2;
              uploadProgressArg.Percent = (int) ((double) (progressToRange - progressFromRange) * (double) num2 / (double) length + (double) progressFromRange);
              uploadProgress.Report(uploadProgressArg);
            }
          }
          requestStream.Close();
          fileStream.Close();
          uploadProgressArg.Percent = progressToRange;
          uploadProgress.Report(uploadProgressArg);
          return true;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        return false;
      }
    }

    public bool UploadEmailAddressFile(
      IProgress<SASProgressArg> uploadProgress,
      CancellationToken uploadCancellationToken,
      SASProgressArg uploadProgressArg,
      int progressFromRange,
      int progressToRange,
      Form form)
    {
      string str1 = this.zipFileToSend + ".email.txt";
      string str2 = false ? Settings.Default.FromUsersFtpInternationalUrl : Settings.Default.FromUsersFtpUrl;
      string requestUriString = !str2.EndsWith("/", StringComparison.InvariantCultureIgnoreCase) ? str2 + "/" + Path.GetFileName(str1) : str2 + Path.GetFileName(str1);
      NetworkCredential networkCredential = new NetworkCredential("uforedatatransfer", "$ufore2data");
      FileInfo fileInfo = new FileInfo(str1);
      long length = fileInfo.Length;
      try
      {
        using (FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read))
        {
          FtpWebRequest ftpWebRequest1 = (FtpWebRequest) WebRequest.Create(requestUriString);
          ftpWebRequest1.Credentials = (ICredentials) networkCredential;
          ftpWebRequest1.Proxy = (IWebProxy) null;
          ftpWebRequest1.UseBinary = true;
          ftpWebRequest1.Method = "STOR";
          ftpWebRequest1.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
          ftpWebRequest1.KeepAlive = false;
          ftpWebRequest1.UsePassive = true;
          int count1 = 4096;
          byte[] buffer = new byte[count1];
          int num1 = 0;
          Stream requestStream = ftpWebRequest1.GetRequestStream();
          int count2 = fileStream.Read(buffer, 0, count1);
          int num2 = num1 + count2;
          while (count2 != 0)
          {
            if (uploadCancellationToken.IsCancellationRequested)
            {
              ftpWebRequest1.Abort();
              FtpWebRequest ftpWebRequest2 = (FtpWebRequest) WebRequest.Create(requestUriString);
              ftpWebRequest2.Credentials = (ICredentials) networkCredential;
              ftpWebRequest2.Proxy = (IWebProxy) null;
              ftpWebRequest2.Method = "DELE";
              ftpWebRequest2.UsePassive = true;
              FtpWebResponse response = (FtpWebResponse) ftpWebRequest2.GetResponse();
              break;
            }
            requestStream.Write(buffer, 0, count2);
            count2 = fileStream.Read(buffer, 0, count1);
            num2 += count2;
            uploadProgressArg.Percent = (int) ((double) (progressToRange - progressFromRange) * (double) num2 / (double) length + (double) progressFromRange);
            uploadProgress.Report(uploadProgressArg);
          }
          requestStream.Close();
          fileStream.Close();
          uploadProgressArg.Percent = progressToRange;
          uploadProgress.Report(uploadProgressArg);
          return true;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        return false;
      }
    }

    public void DownloadZipFile(
      string RemoteFilename,
      IProgress<SASProgressArg> downloadProgress,
      CancellationToken downloadCancellationToken)
    {
      string zipFileReceived = this.zipFileReceived;
      string requestUriString = Settings.Default.ToUsersFtpUrl + "/" + RemoteFilename;
      NetworkCredential networkCredential = new NetworkCredential("uforedatatransfer", "$ufore2data");
      try
      {
        using (FileStream fileStream = new FileStream(zipFileReceived, FileMode.Create))
        {
          FtpWebRequest ftpWebRequest1 = (FtpWebRequest) WebRequest.Create(requestUriString);
          ftpWebRequest1.Credentials = (ICredentials) networkCredential;
          ftpWebRequest1.Proxy = (IWebProxy) null;
          ftpWebRequest1.UseBinary = true;
          ftpWebRequest1.Method = "SIZE";
          ftpWebRequest1.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
          ftpWebRequest1.KeepAlive = false;
          ftpWebRequest1.UsePassive = true;
          FtpWebResponse response = (FtpWebResponse) ftpWebRequest1.GetResponse();
          long num1 = response.StatusCode != FtpStatusCode.FileStatus ? 0L : (response.ContentLength > 0L ? response.ContentLength : 0L);
          response.Close();
          FtpWebRequest ftpWebRequest2 = (FtpWebRequest) WebRequest.Create(requestUriString);
          ftpWebRequest2.Credentials = (ICredentials) networkCredential;
          ftpWebRequest2.Proxy = (IWebProxy) null;
          ftpWebRequest2.UseBinary = true;
          ftpWebRequest2.Method = "RETR";
          ftpWebRequest2.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
          ftpWebRequest2.KeepAlive = false;
          ftpWebRequest2.UsePassive = true;
          int count1 = 4096;
          byte[] buffer = new byte[count1];
          long num2 = 0;
          using (Stream responseStream = ftpWebRequest2.GetResponse().GetResponseStream())
          {
            int count2 = responseStream.Read(buffer, 0, count1);
            long num3 = num2 + (long) count2;
            while (count2 != 0)
            {
              fileStream.Write(buffer, 0, count2);
              count2 = responseStream.Read(buffer, 0, count1);
              num3 += (long) count2;
              downloadProgress.Report(new SASProgressArg()
              {
                Description = SASResources.Downloading,
                Percent = (int) (num3 * 100L / num1)
              });
              downloadCancellationToken.ThrowIfCancellationRequested();
            }
            responseStream.Close();
          }
          fileStream.Close();
        }
      }
      catch (Exception ex)
      {
        throw new SASException(string.Format(SASResources.DownloadError, (object) RemoteFilename), ex);
      }
    }

    public void LoadReadme(
      ref string projectName,
      ref string series,
      ref int year,
      ref Guid yearId,
      ref int yearRevision,
      ref string SampleTypeSASProcessed)
    {
      using (StreamReader streamReader = new StreamReader(this.outputFolder + "\\readMe.txt"))
      {
        LineValues lineValues = new LineValues();
        lineValues.separater = ':';
        string aLine;
        while ((aLine = streamReader.ReadLine()) != null)
        {
          lineValues.SetLine(aLine);
          if (lineValues.Count == 2)
          {
            string upper = lineValues.ElementAt(1).ToUpper(this.ciUsed);
            if (!(upper == "LOCATION"))
            {
              if (!(upper == "SERIES"))
              {
                if (!(upper == "YEAR"))
                {
                  if (!(upper == "SAMPLETYPE"))
                  {
                    if (!(upper == "PROJECTYEARGUID"))
                    {
                      if (upper == "PROJECTYEARREVISION")
                        yearRevision = int.Parse(lineValues.ElementAt(2).Trim(), this.nsInteger, (IFormatProvider) this.ciUsed);
                    }
                    else
                      yearId = new Guid(lineValues.ElementAt(2).Trim());
                  }
                  else
                    SampleTypeSASProcessed = lineValues.ElementAt(2).Trim();
                }
                else
                  year = int.Parse(lineValues.ElementAt(2).Trim(), this.nsInteger, (IFormatProvider) this.ciUsed);
              }
              else
                series = lineValues.ElementAt(2).Trim();
            }
            else
              projectName = lineValues.ElementAt(2).Trim();
          }
        }
        streamReader.Close();
      }
    }

    private int NumbersInLine(LineValues aLine)
    {
      int num = 0;
      for (int index = 1; index <= aLine.Count; ++index)
      {
        if (double.TryParse(aLine.ElementAt(index).Trim(), this.nsFloat, (IFormatProvider) this.ciUsed, out double _))
          ++num;
      }
      return num;
    }

    private string normalizeDBHRange(string aDBHrange)
    {
      string[] strArray = aDBHrange.Split('-');
      return strArray[0].Trim() + " - " + strArray[1].Trim();
    }

    private string removeExtraBlankSpace(string inWord)
    {
      string str = inWord;
      int length;
      do
      {
        length = str.Length;
        str = str.Replace("  ", " ");
      }
      while (length != str.Length && length != 0);
      return str;
    }

    private bool isFirstDBHClassLine(LineValues aLine)
    {
      if (aLine.Count <= 3 || !(aLine.ElementAt(1).Trim() == ""))
        return false;
      string inWord = "";
      for (int index = 1; index <= aLine.Count; ++index)
      {
        inWord = aLine.ElementAt(index).Trim();
        if (inWord != "")
          break;
      }
      string aLine1 = this.removeExtraBlankSpace(inWord);
      LineValues lineValues = new LineValues();
      lineValues.separater = ' ';
      lineValues.SetLine(aLine1);
      return lineValues.Count == 2 && lineValues.ElementAt(2).ToLower(this.ciUsed) == "to";
    }

    public void getClassValues()
    {
      string path2 = "";
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        LineValues aLine1 = new LineValues();
        LineValues lineValues1 = new LineValues();
        LineValues lineValues2 = new LineValues();
        lineValues2.separater = ' ';
        IQuery namedQuery = this.project_s.GetNamedQuery("insertClassValue");
        namedQuery.SetGuid("yearGuid", this.currYear.Guid);
        namedQuery.SetString("classValueName1", "");
        namedQuery.SetInt32("classValueFlag", 0);
        namedQuery.SetString("sppCode", (string) null);
        this.dictDBHtoClassValue.Clear();
        path2 = "CITYDBH.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          namedQuery.SetInt32("classifier", 1);
          short val = 0;
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine2 = str.Trim();
            aLine1.SetLine(aLine2);
            if (this.isFirstDBHClassLine(aLine1))
            {
              lineValues1.SetLine(streamReader.ReadLine().Trim());
              for (int index = 1; index <= lineValues1.Count; ++index)
              {
                if (lineValues1.ElementAt(index).Trim() != "")
                {
                  lineValues2.SetLine(aLine1.ElementAt(index).Trim());
                  ++val;
                  namedQuery.SetInt16("classValueOrder", val).SetString("classValueName", lineValues2.ElementAt(1).Trim() + " - " + lineValues1.ElementAt(index).Trim());
                  namedQuery.ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  this.dictDBHtoClassValue.Add(lineValues2.ElementAt(1).Trim() + " - " + lineValues1.ElementAt(index).Trim(), (int) val);
                }
              }
            }
          }
          streamReader.Close();
        }
        double num1;
        if (this.dictDBHtoClassValue.Count == 0)
        {
          this.SASResultContainTree = false;
          string str1 = "0.0";
          for (short val = 1; val <= (short) 16; ++val)
          {
            num1 = (double) ((int) val * 3) * 2.54;
            string str2 = num1.ToString("#.0", (IFormatProvider) this.ciUsed);
            namedQuery.SetInt16("classValueOrder", val).SetString("classValueName", str1 + " - " + str2);
            namedQuery.ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            this.dictDBHtoClassValue.Add(str1 + " - " + str2, (int) val);
            num1 = (double) ((int) val * 3) * 2.54 + 0.1;
            str1 = num1.ToString("#.0", (IFormatProvider) this.ciUsed);
          }
        }
        else
          this.SASResultContainTree = true;
        this.dictCDBHidToClassValue.Clear();
        namedQuery.SetInt32("classifier", 42);
        short val1 = 0;
        foreach (DBHRptClass dbhRptClass in (IEnumerable<DBHRptClass>) this.currYear.DBHRptClasses)
        {
          ++val1;
          this.dictCDBHidToClassValue.Add(dbhRptClass.Id.ToString((IFormatProvider) this.ciUsed), (int) val1);
          namedQuery.SetInt16("classValueOrder", val1);
          if (this.currYear.Unit == YearUnit.Metric)
          {
            IQuery query = namedQuery;
            num1 = dbhRptClass.RangeStart;
            string str3 = num1.ToString((IFormatProvider) this.ciUsed);
            num1 = dbhRptClass.RangeEnd;
            string str4 = num1.ToString((IFormatProvider) this.ciUsed);
            string val2 = str3 + " - " + str4;
            query.SetString("classValueName", val2);
          }
          else
          {
            IQuery query = namedQuery;
            num1 = Math.Round(2.54 * dbhRptClass.RangeStart, 1);
            string str5 = num1.ToString((IFormatProvider) this.ciUsed);
            num1 = Math.Round(2.54 * dbhRptClass.RangeEnd, 1);
            string str6 = num1.ToString((IFormatProvider) this.ciUsed);
            string val3 = str5 + " - " + str6;
            query.SetString("classValueName", val3);
          }
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
        }
        this.dictDiebackToClassValue.Clear();
        this.listDiebackClassValue.Clear();
        namedQuery.SetInt32("classifier", 2);
        path2 = "CONDCLAS.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          bool flag = false;
          short val4 = 0;
          string str;
          while ((str = streamReader.ReadLine()) != null && !flag)
          {
            string aLine3 = str.Trim();
            if (aLine3.Length > 0)
            {
              aLine1.SetLine(aLine3);
              if (aLine1.ElementAt(aLine1.Count).Trim().ToUpper(this.ciUsed) == "MEAN")
              {
                for (int index = 1; index <= aLine1.Count; ++index)
                {
                  string upper = aLine1.ElementAt(index).Trim().ToUpper(this.ciUsed);
                  if (upper != "" && upper != "MEAN")
                  {
                    ++val4;
                    namedQuery.SetInt16("classValueOrder", val4).SetString("classValueName", upper);
                    namedQuery.ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    this.dictDiebackToClassValue.Add(aLine1.ElementAt(index).Trim(), (int) val4);
                    this.listDiebackClassValue.Add((int) val4);
                  }
                }
                flag = true;
              }
            }
          }
          streamReader.Close();
        }
        this.dictCDiebackIdToClassValue.Clear();
        short val5 = 0;
        if (this.currYear.HealthRptClasses.Count > 0)
        {
          namedQuery.SetInt32("classifier", 43);
          foreach (HealthRptClass healthRptClass in (IEnumerable<HealthRptClass>) this.currYear.HealthRptClasses)
          {
            ++val5;
            this.dictCDiebackIdToClassValue.Add(healthRptClass.Id.ToString((IFormatProvider) this.ciUsed), (int) val5);
            namedQuery.SetInt16("classValueOrder", val5).SetString("classValueName", healthRptClass.Description);
            namedQuery.ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
          }
        }
        this.dictStrataNameToClassValue.Clear();
        this.dictStrataClassValueToName.Clear();
        namedQuery.SetInt32("classifier", 3);
        path2 = "AREAESTM.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          bool flag1 = false;
          short num2 = 0;
          string str7;
          while ((str7 = streamReader.ReadLine()) != null)
          {
            string aLine4 = str7.Trim();
            if (aLine4.Length > 0)
            {
              aLine1.SetLine(aLine4);
              bool flag2 = aLine1.Count == 1;
              if (aLine1.ElementAt(1).ToUpper(this.ciUsed).Trim() == "CITY TOTAL")
                flag1 = true;
              if (flag2)
              {
                string str8 = aLine1.ElementAt(1).Trim();
                if (str8 != "")
                {
                  ++num2;
                  namedQuery.SetInt16("classValueOrder", num2).SetString("classValueName", str8);
                  namedQuery.ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  this.dictStrataNameToClassValue.Add(str8, (int) num2);
                  this.dictStrataClassValueToName.Add((int) num2, str8);
                }
              }
            }
          }
          if (!flag1)
            throw new Exception("Processing file " + path2 + Environment.NewLine + "'City Total' can not be found");
          short num3 = (short) ((int) num2 + 1);
          namedQuery.SetInt16("classValueOrder", num3).SetString("classValueName", "CITY TOTAL");
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.ClassValueForStrataStudyArea = (int) num3;
          this.dictStrataNameToClassValue.Add("CITY TOTAL", (int) num3);
          this.dictStrataClassValueToName.Add((int) num3, "CITY TOTAL");
          streamReader.Close();
        }
        this.dictSpeciesCodeToClassValue.Clear();
        this.dictSpeciesClassValueToCode.Clear();
        this.dictSpeciesClassValueToGenusCode.Clear();
        this.dictSpeciesClassValueToEvergreen.Clear();
        this.dictGenusCodePlusSpeciesCodeToSpeciesCode.Clear();
        path2 = "SPPLIST.CSV";
        namedQuery.SetInt32("classifier", 4);
        short num4 = 0;
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine5 = str.Trim();
            if (aLine5.Length > 0)
            {
              aLine1.SetLine(aLine5);
              if (aLine1.ElementAt(2).Trim().ToUpper(this.ciUsed) != "SCIENTIFIC NAME" && aLine1.ElementAt(3).Trim().ToUpper(this.ciUsed) != "COMMON NAME")
              {
                if (!this.m_ps.Species.ContainsKey(aLine1.ElementAt(1).Trim()))
                {
                  streamReader.Close();
                  throw new Exception("Species code " + aLine1.ElementAt(1).Trim() + " does not exist in ProgramSession.Species.");
                }
                ++num4;
                SpeciesView specy = this.m_ps.Species[aLine1.ElementAt(1).Trim()];
                int val6 = !(aLine1.ElementAt(5).Trim() == "EVERGREEN") ? 0 : 1;
                namedQuery.SetInt16("classValueOrder", num4).SetString("classValueName", specy.CommonName).SetString("classValueName1", specy.ScientificName).SetInt32("classValueFlag", val6).SetString("sppCode", aLine1.ElementAt(1).Trim());
                namedQuery.ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                this.dictSpeciesCodeToClassValue.Add(aLine1.ElementAt(1).Trim(), (int) num4);
                this.dictSpeciesClassValueToCode.Add((int) num4, aLine1.ElementAt(1).Trim());
                this.dictSpeciesClassValueToGenusCode.Add((int) num4, aLine1.ElementAt(4).Trim());
                this.dictSpeciesClassValueToEvergreen.Add((int) num4, val6);
                this.dictGenusCodePlusSpeciesCodeToSpeciesCode.Add(aLine1.ElementAt(2).Trim(), aLine1.ElementAt(1).Trim());
              }
            }
          }
          streamReader.Close();
          short num5 = (short) ((int) num4 + 1);
          namedQuery.SetInt16("classValueOrder", num5).SetString("classValueName", "Total").SetString("classValueName1", "Total").SetInt32("classValueFlag", 0).SetString("sppCode", "Total");
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.ClassValueForSpeciesAllSpecies = (int) num5;
          this.dictSpeciesCodeToClassValue.Add("Total", (int) num5);
          this.dictSpeciesClassValueToCode.Add((int) num5, "Total");
        }
        path2 = "";
        this.checkSpeciesIntegrityInOtherFiles();
        namedQuery.SetString("classValueName1", "");
        namedQuery.SetInt32("classValueFlag", 0);
        namedQuery.SetString("sppCode", (string) null);
        this.dictContinentToClassValue.Clear();
        this.listContinentClassValue.Clear();
        namedQuery.SetInt32("classifier", 6);
        path2 = "EXOTICS.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          short val7 = 0;
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine6 = str.Trim();
            if (aLine6.Length > 0)
            {
              aLine1.SetLine(aLine6);
              if (aLine1.Count >= 3)
              {
                for (int index = 2; index <= aLine1.Count; ++index)
                {
                  ++val7;
                  namedQuery.SetInt16("classValueOrder", val7).SetString("classValueName", aLine1.ElementAt(index).Trim());
                  namedQuery.ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  this.dictContinentToClassValue.Add(aLine1.ElementAt(index).Trim(), (int) val7);
                  this.listContinentClassValue.Add((int) val7);
                }
                break;
              }
            }
          }
          streamReader.Close();
        }
        if (this.dictContinentToClassValue.Count == 0)
        {
          short val8 = 1;
          string str9 = "STATE";
          namedQuery.SetInt16("classValueOrder", val8).SetString("classValueName", str9);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str9, (int) val8);
          this.listContinentClassValue.Add((int) val8);
          short val9 = (short) ((int) val8 + 1);
          string str10 = "Africa";
          namedQuery.SetInt16("classValueOrder", val9).SetString("classValueName", str10);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str10, (int) val9);
          this.listContinentClassValue.Add((int) val9);
          short val10 = (short) ((int) val9 + 1);
          string str11 = "Africa & Asia";
          namedQuery.SetInt16("classValueOrder", val10).SetString("classValueName", str11);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str11, (int) val10);
          this.listContinentClassValue.Add((int) val10);
          short val11 = (short) ((int) val10 + 1);
          string str12 = "Africa & Australia";
          namedQuery.SetInt16("classValueOrder", val11).SetString("classValueName", str12);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str12, (int) val11);
          this.listContinentClassValue.Add((int) val11);
          short val12 = (short) ((int) val11 + 1);
          string str13 = "Asia";
          namedQuery.SetInt16("classValueOrder", val12).SetString("classValueName", str13);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str13, (int) val12);
          this.listContinentClassValue.Add((int) val12);
          short val13 = (short) ((int) val12 + 1);
          string str14 = "Asia & Australia";
          namedQuery.SetInt16("classValueOrder", val13).SetString("classValueName", str14);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str14, (int) val13);
          this.listContinentClassValue.Add((int) val13);
          short val14 = (short) ((int) val13 + 1);
          string str15 = "Asia & Australia +";
          namedQuery.SetInt16("classValueOrder", val14).SetString("classValueName", str15);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str15, (int) val14);
          this.listContinentClassValue.Add((int) val14);
          short val15 = (short) ((int) val14 + 1);
          string str16 = "Australia";
          namedQuery.SetInt16("classValueOrder", val15).SetString("classValueName", str16);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str16, (int) val15);
          this.listContinentClassValue.Add((int) val15);
          short val16 = (short) ((int) val15 + 1);
          string str17 = "Europe";
          namedQuery.SetInt16("classValueOrder", val16).SetString("classValueName", str17);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str17, (int) val16);
          this.listContinentClassValue.Add((int) val16);
          short val17 = (short) ((int) val16 + 1);
          string str18 = "Europe & Africa";
          namedQuery.SetInt16("classValueOrder", val17).SetString("classValueName", str18);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str18, (int) val17);
          this.listContinentClassValue.Add((int) val17);
          short val18 = (short) ((int) val17 + 1);
          string str19 = "Europe & Asia";
          namedQuery.SetInt16("classValueOrder", val18).SetString("classValueName", str19);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str19, (int) val18);
          this.listContinentClassValue.Add((int) val18);
          short val19 = (short) ((int) val18 + 1);
          string str20 = "Europe & Asia +";
          namedQuery.SetInt16("classValueOrder", val19).SetString("classValueName", str20);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str20, (int) val19);
          this.listContinentClassValue.Add((int) val19);
          short val20 = (short) ((int) val19 + 1);
          string str21 = "North America";
          namedQuery.SetInt16("classValueOrder", val20).SetString("classValueName", str21);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str21, (int) val20);
          this.listContinentClassValue.Add((int) val20);
          short val21 = (short) ((int) val20 + 1);
          string str22 = "North America +";
          namedQuery.SetInt16("classValueOrder", val21).SetString("classValueName", str22);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str22, (int) val21);
          this.listContinentClassValue.Add((int) val21);
          short val22 = (short) ((int) val21 + 1);
          string str23 = "North & South America";
          namedQuery.SetInt16("classValueOrder", val22).SetString("classValueName", str23);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str23, (int) val22);
          this.listContinentClassValue.Add((int) val22);
          short val23 = (short) ((int) val22 + 1);
          string str24 = "North & South America +";
          namedQuery.SetInt16("classValueOrder", val23).SetString("classValueName", str24);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str24, (int) val23);
          this.listContinentClassValue.Add((int) val23);
          short val24 = (short) ((int) val23 + 1);
          string str25 = "Other";
          namedQuery.SetInt16("classValueOrder", val24).SetString("classValueName", str25);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str25, (int) val24);
          this.listContinentClassValue.Add((int) val24);
          short val25 = (short) ((int) val24 + 1);
          string str26 = "South America";
          namedQuery.SetInt16("classValueOrder", val25).SetString("classValueName", str26);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str26, (int) val25);
          this.listContinentClassValue.Add((int) val25);
          short val26 = (short) ((int) val25 + 1);
          string str27 = "Unknown";
          namedQuery.SetInt16("classValueOrder", val26).SetString("classValueName", str27);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictContinentToClassValue.Add(str27, (int) val26);
          this.listContinentClassValue.Add((int) val26);
        }
        this.dictPrimaryIndexToClassValue.Clear();
        this.listPrimaryIndexClassValue.Clear();
        namedQuery.SetInt32("classifier", 9);
        path2 = "DIVERSIT.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          short val27 = 0;
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine7 = str.Trim();
            if (aLine7.Length > 0)
            {
              aLine1.SetLine(aLine7);
              if (aLine1.ElementAt(1).Trim().ToUpper(this.ciUsed) == "LANDUSE" && aLine1.Count >= 3)
              {
                for (int index = 6; index <= aLine1.Count; ++index)
                {
                  ++val27;
                  namedQuery.SetInt16("classValueOrder", val27).SetString("classValueName", aLine1.ElementAt(index).Trim());
                  namedQuery.ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  this.dictPrimaryIndexToClassValue.Add(aLine1.ElementAt(index).Trim(), (int) val27);
                  this.listPrimaryIndexClassValue.Add((int) val27);
                }
                break;
              }
            }
          }
          streamReader.Close();
        }
        if (this.dictPrimaryIndexToClassValue.Count == 0)
        {
          short val28 = 1;
          string str28 = "S";
          namedQuery.SetInt16("classValueOrder", val28).SetString("classValueName", str28);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictPrimaryIndexToClassValue.Add(str28, (int) val28);
          this.listPrimaryIndexClassValue.Add((int) val28);
          short val29 = (short) ((int) val28 + 1);
          string str29 = "SPP/HA";
          namedQuery.SetInt16("classValueOrder", val29).SetString("classValueName", str29);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictPrimaryIndexToClassValue.Add(str29, (int) val29);
          this.listPrimaryIndexClassValue.Add((int) val29);
          short val30 = (short) ((int) val29 + 1);
          string str30 = "SHANNON";
          namedQuery.SetInt16("classValueOrder", val30).SetString("classValueName", str30);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictPrimaryIndexToClassValue.Add(str30, (int) val30);
          this.listPrimaryIndexClassValue.Add((int) val30);
          short val31 = (short) ((int) val30 + 1);
          string str31 = "MENHINICK";
          namedQuery.SetInt16("classValueOrder", val31).SetString("classValueName", str31);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictPrimaryIndexToClassValue.Add(str31, (int) val31);
          this.listPrimaryIndexClassValue.Add((int) val31);
          short val32 = (short) ((int) val31 + 1);
          string str32 = "SIMPSON";
          namedQuery.SetInt16("classValueOrder", val32).SetString("classValueName", str32);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictPrimaryIndexToClassValue.Add(str32, (int) val32);
          this.listPrimaryIndexClassValue.Add((int) val32);
          short val33 = (short) ((int) val32 + 1);
          string str33 = "EVENNESS";
          namedQuery.SetInt16("classValueOrder", val33).SetString("classValueName", str33);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictPrimaryIndexToClassValue.Add(str33, (int) val33);
          this.listPrimaryIndexClassValue.Add((int) val33);
          short val34 = (short) ((int) val33 + 1);
          string str34 = "RAREFACTION";
          namedQuery.SetInt16("classValueOrder", val34).SetString("classValueName", str34);
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          this.dictPrimaryIndexToClassValue.Add(str34, (int) val34);
          this.listPrimaryIndexClassValue.Add((int) val34);
        }
        this.dictFieldLanduseToClassValue.Clear();
        this.listFieldLanduseClassValue.Clear();
        namedQuery.SetInt32("classifier", 12);
        path2 = "LANDUSEM.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          short val35 = 0;
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine8 = str.Trim();
            if (aLine8.Length > 0)
            {
              aLine1.SetLine(aLine8);
              if (aLine1.ElementAt(1).Trim() != "" && aLine1.Count >= 2)
              {
                ++val35;
                namedQuery.SetInt16("classValueOrder", val35).SetString("classValueName", aLine1.ElementAt(1).Trim());
                namedQuery.ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                this.dictFieldLanduseToClassValue.Add(aLine1.ElementAt(1).Trim(), (int) val35);
                this.listFieldLanduseClassValue.Add((int) val35);
              }
            }
          }
          streamReader.Close();
        }
        path2 = "";
        this.dictEnergyUseToClassValue.Clear();
        namedQuery.SetInt32("classifier", 13);
        namedQuery.SetInt16("classValueOrder", (short) 1).SetString("classValueName", "Cooling");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictEnergyUseToClassValue.Add("Cooling", 1);
        namedQuery.SetInt16("classValueOrder", (short) 2).SetString("classValueName", "Heating");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictEnergyUseToClassValue.Add("Heating", 2);
        this.dictEnergyTypeToClassValue.Clear();
        namedQuery.SetInt32("classifier", 14);
        namedQuery.SetInt16("classValueOrder", (short) 1).SetString("classValueName", "Shade");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictEnergyTypeToClassValue.Add("Shade", 1);
        namedQuery.SetInt16("classValueOrder", (short) 2).SetString("classValueName", "Climate");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictEnergyTypeToClassValue.Add("Climate", 2);
        namedQuery.SetInt16("classValueOrder", (short) 3).SetString("classValueName", "Windbreak");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictEnergyTypeToClassValue.Add("Windbreak", 3);
        this.dictPollutantToClassValue.Clear();
        namedQuery.SetInt32("classifier", 15);
        namedQuery.SetInt16("classValueOrder", (short) 1).SetString("classValueName", "CO");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictPollutantToClassValue.Add("CO", 1);
        namedQuery.SetInt16("classValueOrder", (short) 2).SetString("classValueName", "NO2");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictPollutantToClassValue.Add("NO2", 2);
        namedQuery.SetInt16("classValueOrder", (short) 3).SetString("classValueName", "O3");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictPollutantToClassValue.Add("O3", 3);
        namedQuery.SetInt16("classValueOrder", (short) 4).SetString("classValueName", "PM10*");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictPollutantToClassValue.Add("PM10*", 4);
        namedQuery.SetInt16("classValueOrder", (short) 5).SetString("classValueName", "PM2.5");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictPollutantToClassValue.Add("PM2.5", 5);
        namedQuery.SetInt16("classValueOrder", (short) 6).SetString("classValueName", "SO2");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictPollutantToClassValue.Add("SO2", 6);
        this.dictVOCToClassValue.Clear();
        namedQuery.SetInt32("classifier", 16);
        namedQuery.SetInt16("classValueOrder", (short) 1).SetString("classValueName", "Monoterpene");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictVOCToClassValue.Add("Monoterpene", 1);
        namedQuery.SetInt16("classValueOrder", (short) 2).SetString("classValueName", "Isoprene");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictVOCToClassValue.Add("Isoprene", 2);
        namedQuery.SetInt16("classValueOrder", (short) 3).SetString("classValueName", "VOC Other");
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictVOCToClassValue.Add("VOC Other", 3);
        this.dictHourToClassValue.Clear();
        namedQuery.SetInt32("classifier", 18);
        for (short val36 = 1; val36 <= (short) 24; ++val36)
        {
          this.dictHourToClassValue.Add(val36.ToString(), (int) val36);
          namedQuery.SetInt16("classValueOrder", val36).SetString("classValueName", val36.ToString((IFormatProvider) this.ciUsed));
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
        }
        this.dictMonthToClassValue.Clear();
        namedQuery.SetInt32("classifier", 19);
        for (short val37 = 1; val37 <= (short) 12; ++val37)
        {
          this.dictMonthToClassValue.Add(val37.ToString((IFormatProvider) this.ciUsed), (int) val37);
          namedQuery.SetInt16("classValueOrder", val37).SetString("classValueName", val37.ToString((IFormatProvider) this.ciUsed));
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
        }
        this.dictPestAndPestSymptomClassIDandSymptomIDtoClassValue.Clear();
        int val38 = 0;
        IList<IPED.Domain.Lookup> lookupList = (IList<IPED.Domain.Lookup>) null;
        namedQuery.SetString("classValueName1", "").SetString("sppCode", (string) null);
        for (int index = 1; index <= 15; ++index)
        {
          switch (index)
          {
            case 1:
              val38 = 35;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.BBAbnGrowth).ToList<IPED.Domain.Lookup>();
              break;
            case 2:
              val38 = 34;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.BBDiseaseSigns).ToList<IPED.Domain.Lookup>();
              break;
            case 3:
              val38 = 33;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.BBInsectPres).ToList<IPED.Domain.Lookup>();
              break;
            case 4:
              val38 = 32;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.BBInsectSigns).ToList<IPED.Domain.Lookup>();
              break;
            case 5:
              val38 = 36;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.BBProbLoc).ToList<IPED.Domain.Lookup>();
              break;
            case 6:
              val38 = 29;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.FTAbnFoli).ToList<IPED.Domain.Lookup>();
              break;
            case 7:
              val38 = 27;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.FTChewFoli).ToList<IPED.Domain.Lookup>();
              break;
            case 8:
              val38 = 28;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.FTDiscFoli).ToList<IPED.Domain.Lookup>();
              break;
            case 9:
              val38 = 31;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.FTFoliAffect).ToList<IPED.Domain.Lookup>();
              break;
            case 10:
              val38 = 30;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.FTInsectSigns).ToList<IPED.Domain.Lookup>();
              break;
            case 11:
              val38 = 22;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.TSDieback).ToList<IPED.Domain.Lookup>();
              break;
            case 12:
              val38 = 25;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.TSEnvStress).ToList<IPED.Domain.Lookup>();
              break;
            case 13:
              val38 = 23;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.TSEpiSprout).ToList<IPED.Domain.Lookup>();
              break;
            case 14:
              val38 = 26;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.TSHumStress).ToList<IPED.Domain.Lookup>();
              break;
            case 15:
              val38 = 24;
              lookupList = (IList<IPED.Domain.Lookup>) ((IEnumerable<IPED.Domain.Lookup>) this.m_ps.IPEDData.TSWiltFoli).ToList<IPED.Domain.Lookup>();
              break;
          }
          namedQuery.SetInt32("classifier", val38);
          short val39 = 0;
          foreach (IPED.Domain.Lookup lookup in (IEnumerable<IPED.Domain.Lookup>) lookupList)
          {
            ++val39;
            namedQuery.SetInt16("classValueOrder", val39).SetString("classValueName", lookup.Description).SetInt32("classValueFlag", lookup.Code);
            namedQuery.ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            this.dictPestAndPestSymptomClassIDandSymptomIDtoClassValue.Add(val38.ToString((IFormatProvider) this.ciUsed) + "_" + lookup.Code.ToString((IFormatProvider) this.ciUsed), (int) val39);
          }
        }
        int val40 = 37;
        namedQuery.SetInt32("classifier", val40);
        namedQuery.SetString("sppCode", (string) null);
        short val41 = (short) (0 + 1);
        this.PestNoneClassValue = (int) val41;
        namedQuery.SetInt16("classValueOrder", val41).SetString("classValueName", "None").SetString("classValueName1", "None").SetInt32("classValueFlag", 0);
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictPestAndPestSymptomClassIDandSymptomIDtoClassValue.Add(val40.ToString((IFormatProvider) this.ciUsed) + "_0", (int) val41);
        short val42 = (short) ((int) val41 + 1);
        this.PestUnknownClassValue = (int) val42;
        namedQuery.SetInt16("classValueOrder", val42).SetString("classValueName", "Unknown").SetString("classValueName1", "Unknown").SetInt32("classValueFlag", -1);
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        this.dictPestAndPestSymptomClassIDandSymptomIDtoClassValue.Add(val40.ToString((IFormatProvider) this.ciUsed) + "_-1", (int) val42);
        short val43 = (short) ((int) val42 + 1);
        this.PestAffectedClassValue = (int) val43;
        namedQuery.SetInt16("classValueOrder", val43).SetString("classValueName", "Affected").SetString("classValueName1", "Affected").SetInt32("classValueFlag", 1);
        namedQuery.ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        foreach (IPED.Domain.Pest pest in this.m_ps.IPEDData.Pests.OrderBy<IPED.Domain.Pest, int>((Func<IPED.Domain.Pest, int>) (p => p.Id)).ToList<IPED.Domain.Pest>())
        {
          if (pest.Id != 0 && pest.Id != -1 && pest.Id != 1)
          {
            ++val43;
            namedQuery.SetInt16("classValueOrder", val43).SetString("classValueName", pest.CommonName).SetString("classValueName1", pest.ScientificName).SetInt32("classValueFlag", pest.Id);
            namedQuery.ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            this.dictPestAndPestSymptomClassIDandSymptomIDtoClassValue.Add(val40.ToString((IFormatProvider) this.ciUsed) + "_" + pest.Id.ToString((IFormatProvider) this.ciUsed), (int) val43);
          }
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        if (path2 == "")
          throw;
        else
          throw new Exception("Processing file " + path2 + ":" + Environment.NewLine + ex.Message);
      }
    }

    public void loadSASResults(IProgress<SASProgressArg> progress, CancellationToken ct)
    {
      System.Action[] actionArray = new System.Action[29]
      {
        (System.Action) (() => this.getAREAESTM()),
        (System.Action) (() => this.getCITYCND()),
        (System.Action) (() => this.getCITYCND2()),
        (System.Action) (() => this.getCITYDBH()),
        (System.Action) (() => this.getCITYDBH2()),
        (System.Action) (() => this.getCITYDEN()),
        (System.Action) (() => this.getCITYSUM()),
        (System.Action) (() => this.getCONDCLAS()),
        (System.Action) (() => this.getCONDCLAS2()),
        (System.Action) (() => this.getDBHCLASS()),
        (System.Action) (() => this.getDBHCLASS2()),
        (System.Action) (() => this.getDBHCNDCL()),
        (System.Action) (() => this.getDIVERSIT()),
        (System.Action) (() => this.getEXOTICS()),
        (System.Action) (() => this.getLANDUSEM()),
        (System.Action) (() => this.getLEAFCLAS()),
        (System.Action) (() => this.getNEWENRGY_TRENRGY()),
        (System.Action) (() => this.getSingleTreeEnergyCalculated(progress, ct)),
        (System.Action) (() => this.getPERCOVES()),
        (System.Action) (() => this.getTOTLESTM()),
        (System.Action) (() => this.getTRESHBLF()),
        (System.Action) (() => this.getSHRUBEST()),
        (System.Action) (() => this.getIPED()),
        (System.Action) (() => this.getErrorNotes()),
        (System.Action) (() => this.getPLOTINV(progress, ct)),
        (System.Action) (() => this.PrepareForLoadingUFOREDBC()),
        (System.Action) (() => this.getUFORE_D_B_I()),
        (System.Action) (() => this.getUVReduction()),
        (System.Action) (() => this.PostProcessData())
      };
      int length = actionArray.Length;
      int num1 = 0;
      foreach (System.Action action in actionArray)
      {
        ct.ThrowIfCancellationRequested();
        try
        {
          action();
          ++num1;
        }
        catch (Exception ex)
        {
          if (new DriveInfo(this.m_ps.InputSession.InputDb).AvailableFreeSpace < 5000000L)
          {
            int num2 = (int) MessageBox.Show("Error occurs in retrieving. It might be the drive space is full.");
          }
          throw ex;
        }
        progress.Report(new SASProgressArg()
        {
          Description = "Loading results...",
          Percent = 100 * num1 / length
        });
      }
    }

    private void PostProcessData()
    {
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        IQuery query1 = this.project_s.GetNamedQuery("updateToNewClassValueNameByClassValueName").SetGuid("YearGuid", this.currYear.Guid);
        query1.SetString("NewClassValueName", "Study Area").SetInt32("ClassifierId", 3).SetString("OldClassValueName", "CITY TOTAL").ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber();
        Dictionary<int, string> dictionary = new Dictionary<int, string>();
        foreach (Strata stratum in (IEnumerable<Strata>) this.currYear.Strata)
          dictionary.Add(stratum.Id, stratum.Description);
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, "tot.txt")))
        {
          LineValues lineValues = new LineValues();
          lineValues.separater = ' ';
          streamReader.ReadLine();
          streamReader.ReadLine();
          streamReader.ReadLine();
          string str1;
          while ((str1 = streamReader.ReadLine()) != null)
          {
            string aLine = str1.Trim();
            if (!string.IsNullOrEmpty(aLine))
            {
              lineValues.SetLine(aLine);
              int key = int.Parse(lineValues.ElementAt(1));
              string str2 = "";
              for (int index = 2; index <= lineValues.Count - 1; ++index)
                str2 = str2 + lineValues.ElementAt(index) + " ";
              string val = str2.Trim();
              if (val != dictionary[key])
              {
                query1.SetString("NewClassValueName", dictionary[key]).SetInt32("ClassifierId", 3).SetString("OldClassValueName", val).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
              }
            }
          }
          streamReader.Close();
        }
        query1.SetInt32("ClassifierId", 2);
        query1.SetString("NewClassValueName", "Excellent").SetString("OldClassValueName", "E").ExecuteUpdate();
        query1.SetString("NewClassValueName", "Good").SetString("OldClassValueName", "G").ExecuteUpdate();
        query1.SetString("NewClassValueName", "Fair").SetString("OldClassValueName", "F").ExecuteUpdate();
        query1.SetString("NewClassValueName", "Poor").SetString("OldClassValueName", "P").ExecuteUpdate();
        query1.SetString("NewClassValueName", "Critical").SetString("OldClassValueName", "C").ExecuteUpdate();
        query1.SetString("NewClassValueName", "Dying").SetString("OldClassValueName", "D").ExecuteUpdate();
        query1.SetString("NewClassValueName", "Dead").SetString("OldClassValueName", "K").ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber(7);
        IList<(int, string, string, int, string)> tupleList = this.project_s.GetNamedQuery("selectClassValuesOfSingleClassifier").SetGuid("YearGuid", this.currYear.Guid).SetInt32("ClassifierId", 1).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, string, string, int, string)>()).List<(int, string, string, int, string)>();
        IQuery query2 = this.project_s.GetNamedQuery("updateClassValueNameByClassValueOrder").SetGuid("YearGuid", this.currYear.Guid).SetInt32("ClassifierId", 1);
        LineValues lineValues1 = new LineValues();
        lineValues1.separater = '-';
        if (tupleList.Count > 0)
        {
          lineValues1.SetLine(tupleList[0].Item2);
          double.Parse(lineValues1.ElementAt(1), (IFormatProvider) this.ciUsed);
          double num1 = double.Parse(lineValues1.ElementAt(2), (IFormatProvider) this.ciUsed);
          for (int index = 1; index < tupleList.Count; ++index)
          {
            double num2 = num1;
            lineValues1.SetLine(tupleList[index].Item2);
            double num3 = double.Parse(lineValues1.ElementAt(1), (IFormatProvider) this.ciUsed);
            num1 = double.Parse(lineValues1.ElementAt(2), (IFormatProvider) this.ciUsed);
            if (Math.Abs(num3 - num2) < 0.100001)
            {
              query2.SetString("ClassValueName", num2.ToString("#0.0", (IFormatProvider) this.ciUsed) + " - " + num1.ToString("#0.0", (IFormatProvider) this.ciUsed)).SetInt32("ClassValueOrder", tupleList[index].Item1).ExecuteUpdate();
              sasTransaction.IncreaseOperationNumber();
            }
          }
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw;
      }
    }

    public void getUFORE_D_B_I()
    {
      SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
      try
      {
        if (System.IO.File.Exists(Path.Combine(this.outputFolder, "FinBenMapTree.mdb")) || System.IO.File.Exists(Path.Combine(this.outputFolder, "FinBenMapTree.db")))
          sasUtilProvider.GetSQLQueryTransferBenMAP(this.outputFolder, "FinBenMapTree").SetGuid("YearGuid", this.currYear.Guid).ExecuteUpdate();
        sasUtilProvider.GetSQLQueryTransferPollutants(this.outputFolder, "UFOREDTree").SetGuid("YearGuid", this.currYear.Guid).SetString("TreeShrub", "T").ExecuteUpdate();
        if (this.currSeries.SampleType != SampleType.Inventory && (this.currYear.RecordShrub || this.currYear.RecordPercentShrub))
        {
          if (System.IO.File.Exists(Path.Combine(this.outputFolder, "FinBenMapShrub.mdb")) || System.IO.File.Exists(Path.Combine(this.outputFolder, "FinBenMapShrub.db")))
            sasUtilProvider.GetSQLQueryTransferBenMAP(this.outputFolder, "FinBenMapShrub").SetGuid("YearGuid", this.currYear.Guid).ExecuteUpdate();
          if (System.IO.File.Exists(Path.Combine(this.outputFolder, "UFOREDShrub.mdb")) || System.IO.File.Exists(Path.Combine(this.outputFolder, "UFOREDShrub.db")))
            sasUtilProvider.GetSQLQueryTransferPollutants(this.outputFolder, "UFOREDShrub").SetGuid("YearGuid", this.currYear.Guid).SetString("TreeShrub", "S").ExecuteUpdate();
        }
        Path.Combine(this.outputFolder, "FinBenMapGrass.mdb");
        Path.Combine(this.outputFolder, "UFOREDGrass.mdb");
        if (this.currSeries.SampleType != SampleType.Inventory && this.currYear.RecordGroundCover)
        {
          if (System.IO.File.Exists(Path.Combine(this.outputFolder, "FinBenMapGrass.mdb")) || System.IO.File.Exists(Path.Combine(this.outputFolder, "FinBenMapGrass.db")))
            sasUtilProvider.GetSQLQueryTransferBenMAP(this.outputFolder, "FinBenMapGrass").SetGuid("YearGuid", this.currYear.Guid).ExecuteUpdate();
          if (System.IO.File.Exists(Path.Combine(this.outputFolder, "UFOREDGrass.mdb")) || System.IO.File.Exists(Path.Combine(this.outputFolder, "UFOREDGrass.db")))
            sasUtilProvider.GetSQLQueryTransferPollutants(this.outputFolder, "UFOREDGrass").SetGuid("YearGuid", this.currYear.Guid).SetString("TreeShrub", "G").ExecuteUpdate();
        }
        if (NationFeatures.isUsingBenMAPresults(this.currProject.NationCode))
        {
          double num1 = 0.0;
          using (ISession session = this.m_ps.LocSp.OpenSession())
            num1 = ((double?) session.QueryOver<Location>().Where((System.Linq.Expressions.Expression<Func<Location, bool>>) (l => l.Id == this.currProject.LocationId)).Cacheable().SingleOrDefault()?.Area).GetValueOrDefault();
          double val = 1.0;
          if (this.currYear.RecordStrata)
          {
            if (num1 != 0.0)
              val *= this.TotalStudyAreaHectare * this.tenThousand / num1;
            if (this.currYear.FIA)
              val /= 2.0;
          }
          else if (num1 != 0.0)
          {
            double num2 = 0.0;
            using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, "PLOTINV.csv")))
            {
              LineValues lineValues = new LineValues();
              lineValues.separater = ',';
              string str;
              while ((str = streamReader.ReadLine()) != null)
              {
                string aLine = str.Trim();
                if (aLine != "")
                {
                  lineValues.SetLine(aLine);
                  if (lineValues.ElementAt(3).Trim() == "INVENTORY TOTAL")
                    num2 = double.Parse(lineValues.ElementAt(6), (IFormatProvider) this.ciUsed);
                }
              }
              streamReader.Close();
            }
            val *= num2 / num1;
          }
          SASTransaction sasTransaction = new SASTransaction();
          sasTransaction.MaxNumber = 500;
          sasTransaction.Begin(this.project_s);
          sasTransaction.IncreaseOperationNumber(sasUtilProvider.GetSQLQueryAdjustBenMAPResults("NO2").SetDouble("ratio", val).SetGuid("YearGuid", this.currYear.Guid).ExecuteUpdate());
          sasTransaction.IncreaseOperationNumber(sasUtilProvider.GetSQLQueryAdjustBenMAPResults("SO2").SetDouble("ratio", val).SetGuid("YearGuid", this.currYear.Guid).ExecuteUpdate());
          sasTransaction.IncreaseOperationNumber(sasUtilProvider.GetSQLQueryAdjustBenMAPResults("O3").SetDouble("ratio", val).SetGuid("YearGuid", this.currYear.Guid).ExecuteUpdate());
          sasTransaction.IncreaseOperationNumber(sasUtilProvider.GetSQLQueryAdjustBenMAPResults("PM25").SetDouble("ratio", val).SetGuid("YearGuid", this.currYear.Guid).ExecuteUpdate());
          sasTransaction.End();
        }
        if (NationFeatures.isUsingBenMAPresults(this.currProject.NationCode))
          this.AdjustPollutionValuesInTablePollutantsBasedOnBenMapResults2(sasUtilProvider);
        this.PopulateUFORE_DResults2(EstimateDataTypes.Tree);
        this.PopulatingUFORE_BResults2(EstimateDataTypes.Tree);
        this.PopulatingWaterInterception(EstimateDataTypes.Tree);
        if (this.currSeries.SampleType != SampleType.Inventory && (this.currYear.RecordShrub || this.currYear.RecordPercentShrub) && (System.IO.File.Exists(Path.Combine(this.outputFolder, "UFOREDShrub.mdb")) || System.IO.File.Exists(Path.Combine(this.outputFolder, "UFOREDShrub.db"))))
        {
          this.PopulateUFORE_DResults2(EstimateDataTypes.Shrub);
          this.PopulatingUFORE_BResults2(EstimateDataTypes.Shrub);
          this.PopulatingWaterInterception(EstimateDataTypes.Shrub);
        }
        if (this.currSeries.SampleType != SampleType.Inventory && this.currYear.RecordGroundCover && (System.IO.File.Exists(Path.Combine(this.outputFolder, "UFOREDGrass.mdb")) || System.IO.File.Exists(Path.Combine(this.outputFolder, "UFOREDGrass.db"))))
          this.PopulateUFORE_DResults2(EstimateDataTypes.Grass);
        this.PopulateUFORE_DResultsForCombinedTreeShrub();
        this.PopulatingUFORE_BResultsForCombinedTreeShrub();
        this.PopulatingWaterInterceptionForCombiedTreeShrub();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void getUVReduction()
    {
      if (!NationFeatures.isUVavailable(this.currProject.NationCode) || !this.currYear.RecordStrata)
        return;
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
        IList<(string, double, double, double, double, double, double)> tupleList = sasUtilProvider.GetSQLQuerySelectYearlyMeanUVReduction(this.outputFolder, "UVredTree").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(string, double, double, double, double, double, double)>()).List<(string, double, double, double, double, double, double)>();
        IQuery query1 = this.project_s.GetNamedQuery("insertUVIndexReductionByStrataYearly").SetGuid("YearGuid", this.currYear.Guid);
        IQuery query2 = this.project_s.GetNamedQuery("insertUVIndexReductionByStrataYearlyForEntireStudyArea").SetGuid("YearGuid", this.currYear.Guid);
        foreach ((string, double, double, double, double, double, double) tuple in (IEnumerable<(string, double, double, double, double, double, double)>) tupleList)
        {
          Strata strata = (Strata) null;
          foreach (Strata stratum in (IEnumerable<Strata>) this.currYear.Strata)
          {
            if (stratum.Description.ToUpper() == tuple.Item1.ToUpper())
            {
              strata = stratum;
              break;
            }
          }
          if (strata != null)
          {
            query1.SetGuid("StrataKey", strata.Guid).SetDouble("MeanShadedUVProtectionFactor", tuple.Item2).SetDouble("MeanShadedUVReduction", tuple.Item3).SetDouble("MeanShadedUVRedutionPercent", tuple.Item4).SetDouble("MeanOverallUVProtectionFactor", tuple.Item5).SetDouble("MeanOverallUVReduction", tuple.Item6).SetDouble("MeanOverallUVReductionPercent", tuple.Item7).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
          }
          else if (tuple.Item1.ToUpper() == "TOTAL")
          {
            query2.SetDouble("MeanShadedUVProtectionFactor", tuple.Item2).SetDouble("MeanShadedUVReduction", tuple.Item3).SetDouble("MeanShadedUVRedutionPercent", tuple.Item4).SetDouble("MeanOverallUVProtectionFactor", tuple.Item5).SetDouble("MeanOverallUVReduction", tuple.Item6).SetDouble("MeanOverallUVReductionPercent", tuple.Item7).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
          }
        }
        sasTransaction.IncreaseOperationNumber(sasUtilProvider.GetSQLQueryTransferUVIndexReduction(this.outputFolder, "UVredTree").SetGuid("YearGuid", this.currYear.Guid).ExecuteUpdate());
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw;
      }
    }

    public void AdjustPollutionValuesInTablePollutantsBasedOnBenMapResults2(
      SASIUtilProvider aSASIUtilProvider)
    {
      try
      {
        IList<(string, double, double, double, double)> tupleList = this.project_s.GetNamedQuery("selectBenMAPCompensatoryValues").SetGuid("YearGuid", this.currYear.Guid).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(string, double, double, double, double)>()).List<(string, double, double, double, double)>();
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        double num4 = 0.0;
        double num5 = 0.0;
        double num6 = 0.0;
        double num7 = 0.0;
        double num8 = 0.0;
        double num9 = 0.0;
        double num10 = 0.0;
        double num11 = 0.0;
        double num12 = 0.0;
        bool flag;
        if (tupleList.Count == 0)
        {
          flag = false;
        }
        else
        {
          flag = true;
          foreach ((string, double, double, double, double) tuple in (IEnumerable<(string, double, double, double, double)>) tupleList)
          {
            string str = tuple.Item1;
            if (!(str == "T"))
            {
              if (!(str == "S"))
              {
                if (str == "G")
                {
                  num9 += tuple.Item2 == -1.0 ? 0.0 : tuple.Item2;
                  num10 += tuple.Item3 == -1.0 ? 0.0 : tuple.Item3;
                  num11 += tuple.Item4 == -1.0 ? 0.0 : tuple.Item4;
                  num12 += tuple.Item5 == -1.0 ? 0.0 : tuple.Item5;
                }
              }
              else
              {
                num5 += tuple.Item2 == -1.0 ? 0.0 : tuple.Item2;
                num6 += tuple.Item3 == -1.0 ? 0.0 : tuple.Item3;
                num7 += tuple.Item4 == -1.0 ? 0.0 : tuple.Item4;
                num8 += tuple.Item5 == -1.0 ? 0.0 : tuple.Item5;
              }
            }
            else
            {
              num1 += tuple.Item2 == -1.0 ? 0.0 : tuple.Item2;
              num2 += tuple.Item3 == -1.0 ? 0.0 : tuple.Item3;
              num3 += tuple.Item4 == -1.0 ? 0.0 : tuple.Item4;
              num4 += tuple.Item5 == -1.0 ? 0.0 : tuple.Item5;
            }
          }
        }
        string str1 = !(this.project_s.GetNamedQuery("selectDatabaseFormat").List<string>().SingleOrDefault<string>() == "ACCESS") ? ".db" : ".mdb";
        double num13 = 0.0;
        double num14 = 0.0;
        double num15 = 0.0;
        double num16 = 0.0;
        double num17 = 0.0;
        double num18 = 0.0;
        double num19 = 0.0;
        double num20 = 0.0;
        double num21 = 0.0;
        double num22 = 0.0;
        double num23 = 0.0;
        double num24 = 0.0;
        double num25 = 0.0;
        double num26 = 0.0;
        double num27 = 0.0;
        double num28 = 0.0;
        double num29 = 0.0;
        double num30 = 0.0;
        double num31 = 0.0;
        double num32 = 0.0;
        double num33 = 0.0;
        double num34 = 0.0;
        double num35 = 0.0;
        double num36 = 0.0;
        if (System.IO.File.Exists(Path.Combine(this.outputFolder, "UFOREDTree" + str1)))
        {
          foreach ((string, double, double) tuple in (IEnumerable<(string, double, double)>) aSASIUtilProvider.GetSQLQueryFluxDomainValDomainFrom_08_DomainYearlySums(this.outputFolder, "UFOREDTree").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(string, double, double)>()).List<(string, double, double)>())
          {
            string str2 = tuple.Item1;
            if (!(str2 == "NO2"))
            {
              if (!(str2 == "SO2"))
              {
                if (!(str2 == "O3"))
                {
                  if (str2 == "PM2.5")
                  {
                    num16 = this.million * tuple.Item2;
                    num28 = this.thousand * tuple.Item3;
                  }
                }
                else
                {
                  num15 = this.million * tuple.Item2;
                  num27 = this.thousand * tuple.Item3;
                }
              }
              else
              {
                num14 = this.million * tuple.Item2;
                num26 = this.thousand * tuple.Item3;
              }
            }
            else
            {
              num13 = this.million * tuple.Item2;
              num25 = this.thousand * tuple.Item3;
            }
          }
        }
        if (System.IO.File.Exists(Path.Combine(this.outputFolder, "UFOREDShrub" + str1)))
        {
          foreach ((string, double, double) tuple in (IEnumerable<(string, double, double)>) aSASIUtilProvider.GetSQLQueryFluxDomainValDomainFrom_08_DomainYearlySums(this.outputFolder, "UFOREDShrub").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(string, double, double)>()).List<(string, double, double)>())
          {
            string str3 = tuple.Item1;
            if (!(str3 == "NO2"))
            {
              if (!(str3 == "SO2"))
              {
                if (!(str3 == "O3"))
                {
                  if (str3 == "PM2.5")
                  {
                    num20 = this.million * tuple.Item2;
                    num32 = this.thousand * tuple.Item3;
                  }
                }
                else
                {
                  num19 = this.million * tuple.Item2;
                  num31 = this.thousand * tuple.Item3;
                }
              }
              else
              {
                num18 = this.million * tuple.Item2;
                num30 = this.thousand * tuple.Item3;
              }
            }
            else
            {
              num17 = this.million * tuple.Item2;
              num29 = this.thousand * tuple.Item3;
            }
          }
        }
        if (System.IO.File.Exists(Path.Combine(this.outputFolder, "UFOREDGrass" + str1)))
        {
          foreach ((string, double, double) tuple in (IEnumerable<(string, double, double)>) aSASIUtilProvider.GetSQLQueryFluxDomainValDomainFrom_08_DomainYearlySums(this.outputFolder, "UFOREDGrass").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(string, double, double)>()).List<(string, double, double)>())
          {
            string str4 = tuple.Item1;
            if (!(str4 == "NO2"))
            {
              if (!(str4 == "SO2"))
              {
                if (!(str4 == "O3"))
                {
                  if (str4 == "PM2.5")
                  {
                    num24 = this.million * tuple.Item2;
                    num36 = this.thousand * tuple.Item3;
                  }
                }
                else
                {
                  num23 = this.million * tuple.Item2;
                  num35 = this.thousand * tuple.Item3;
                }
              }
              else
              {
                num22 = this.million * tuple.Item2;
                num34 = this.thousand * tuple.Item3;
              }
            }
            else
            {
              num21 = this.million * tuple.Item2;
              num33 = this.thousand * tuple.Item3;
            }
          }
        }
        IQuery query = this.project_s.GetNamedQuery("updatePollutants").SetGuid("YearGuid", this.currYear.Guid).SetInt32("Year", (int) this.currYearLocation.PollutionYear);
        query.SetDouble("ratio", num13 != 0.0 ? (!flag ? num25 / num13 : num1 / num13) : 0.0).SetString("Pollutant", "NO2").SetString("TreeShrub", "T").ExecuteUpdate();
        query.SetDouble("ratio", num17 != 0.0 ? (!flag ? num29 / num17 : num5 / num17) : 0.0).SetString("Pollutant", "NO2").SetString("TreeShrub", "S").ExecuteUpdate();
        query.SetDouble("ratio", num21 != 0.0 ? (!flag ? num33 / num21 : num9 / num21) : 0.0).SetString("Pollutant", "NO2").SetString("TreeShrub", "G").ExecuteUpdate();
        query.SetDouble("ratio", num14 != 0.0 ? (!flag ? num26 / num14 : num2 / num14) : 0.0).SetString("Pollutant", "SO2").SetString("TreeShrub", "T").ExecuteUpdate();
        query.SetDouble("ratio", num18 != 0.0 ? (!flag ? num30 / num18 : num6 / num18) : 0.0).SetString("Pollutant", "SO2").SetString("TreeShrub", "S").ExecuteUpdate();
        query.SetDouble("ratio", num22 != 0.0 ? (!flag ? num34 / num22 : num10 / num22) : 0.0).SetString("Pollutant", "SO2").SetString("TreeShrub", "G").ExecuteUpdate();
        query.SetDouble("ratio", num15 != 0.0 ? (!flag ? num27 / num15 : num3 / num15) : 0.0).SetString("Pollutant", "O3").SetString("TreeShrub", "T").ExecuteUpdate();
        query.SetDouble("ratio", num19 != 0.0 ? (!flag ? num31 / num19 : num7 / num19) : 0.0).SetString("Pollutant", "O3").SetString("TreeShrub", "S").ExecuteUpdate();
        query.SetDouble("ratio", num23 != 0.0 ? (!flag ? num35 / num23 : num11 / num23) : 0.0).SetString("Pollutant", "O3").SetString("TreeShrub", "G").ExecuteUpdate();
        query.SetDouble("ratio", num16 != 0.0 ? (!flag ? num28 / num16 : num4 / num16) : 0.0).SetString("Pollutant", "PM2.5").SetString("TreeShrub", "T").ExecuteUpdate();
        query.SetDouble("ratio", num20 != 0.0 ? (!flag ? num32 / num20 : num8 / num20) : 0.0).SetString("Pollutant", "PM2.5").SetString("TreeShrub", "S").ExecuteUpdate();
        query.SetDouble("ratio", num24 != 0.0 ? (!flag ? num36 / num24 : num12 / num24) : 0.0).SetString("Pollutant", "PM2.5").SetString("TreeShrub", "G").ExecuteUpdate();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void getPLOTINV(IProgress<SASProgressArg> progress, CancellationToken ct)
    {
      string path2 = "";
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        Tree treeType = (Tree) null;
        Plot plotType = (Plot) null;
        IQueryOver<Tree, Plot> queryOver = this.project_s.QueryOver<Tree>((System.Linq.Expressions.Expression<Func<Tree>>) (() => treeType)).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => plotType));
        if (this.currSeries.SampleType == SampleType.Inventory)
          queryOver.Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear));
        else
          queryOver.Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.currYear && p.IsComplete == true));
        if (this.currYear.RecordTreeStatus)
          queryOver.Where((System.Linq.Expressions.Expression<Func<bool>>) (() => !treeType.Status.IsIn(SASProcessor.TreeRemoveStatus)));
        int num1 = queryOver.Select((IProjection) Projections.Count((System.Linq.Expressions.Expression<Func<object>>) (() => (object) treeType.Id))).SingleOrDefault<int>();
        SASDictionary<string, int> treeIdNewToOld = new SASDictionary<string, int>("Tree ID");
        LineValues lineValues1 = new LineValues();
        path2 = "ReOrderIDs.txt";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          string aLine;
          while ((aLine = streamReader.ReadLine()) != null)
          {
            if (aLine.Trim() != "")
            {
              lineValues1.SetLine(aLine);
              if (lineValues1.ElementAt(2).Trim() == "T")
                treeIdNewToOld.Add(lineValues1.ElementAt(5).Trim(), int.Parse(lineValues1.ElementAt(4), this.nsInteger, (IFormatProvider) this.ciUsed));
            }
          }
          streamReader.Close();
        }
        IQuery namedQuery = this.project_s.GetNamedQuery("insertIndividualTreeEffects");
        namedQuery.SetGuid("YearGuid", this.currYear.Guid).SetDouble("WaterInterception", 0.0).SetDouble("AvoidedRunoff", 0.0).SetDouble("Evaporation", 0.0).SetDouble("PotentialEvaporation", 0.0).SetDouble("Transpiration", 0.0).SetDouble("PotentialEvapotranspiration", 0.0);
        SASDictionary<string, FIA_FutureIndividualTreeCharacters> sasDictionary = new SASDictionary<string, FIA_FutureIndividualTreeCharacters>("Reordered Tree ID");
        path2 = "INVENT_FIA.csv";
        if (this.currYear.FIA)
        {
          if (!System.IO.File.Exists(Path.Combine(this.outputFolder, path2)))
            throw new Exception("File INVENT_FIA.csv does not exist.");
          using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
          {
            LineValues lineValues2 = new LineValues();
            lineValues2.separater = ',';
            string aLine;
            while ((aLine = streamReader.ReadLine()) != null)
            {
              lineValues2.SetLine(aLine);
              if (int.TryParse(lineValues2.ElementAt(1), out int _))
                sasDictionary.Add(lineValues2.ElementAt(1).Trim(), new FIA_FutureIndividualTreeCharacters()
                {
                  FutureDBH = double.Parse(lineValues2.ElementAt(16)),
                  FutureHeight = double.Parse(lineValues2.ElementAt(17)),
                  BiomassAdj = double.Parse(lineValues2.ElementAt(18)),
                  LeafType = lineValues2.ElementAt(19).Trim(),
                  FutureTreeBiomass = double.Parse(lineValues2.ElementAt(20))
                });
            }
            streamReader.Close();
          }
        }
        new LineValues().separater = ' ';
        path2 = "PLOTINV.csv";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          double result = 0.0;
          int num2 = 0;
          int num3 = num1 / 100 + 1;
          DateTime now = DateTime.Now;
          string aLine;
          while ((aLine = streamReader.ReadLine()) != null)
          {
            if (aLine.Trim() != "")
            {
              lineValues1.SetLine(aLine);
              if (lineValues1.Count > 5 && lineValues1.ElementAt(1).Trim().Length != 0 && double.TryParse(lineValues1.ElementAt(1), this.nsFloat, (IFormatProvider) this.ciUsed, out result))
              {
                if (ct.IsCancellationRequested)
                  return;
                if (num2 % num3 == 0)
                  progress.Report(new SASProgressArg()
                  {
                    Description = "Loading individual tree results...",
                    Percent = 100 * num2 / num1 > 100 ? 100 : 100 * num2 / num1
                  });
                ++num2;
                FIA_FutureIndividualTreeCharacters individualTreeCharacters = (FIA_FutureIndividualTreeCharacters) null;
                if (sasDictionary.ContainsKey(lineValues1.ElementAt(2).Trim()))
                  individualTreeCharacters = sasDictionary[lineValues1.ElementAt(2).Trim()];
                string key = this.dictGenusCodePlusSpeciesCodeToSpeciesCode.ContainsKey(lineValues1.ElementAt(3).Trim()) ? this.dictGenusCodePlusSpeciesCodeToSpeciesCode[lineValues1.ElementAt(3).Trim()] : throw new Exception("Species scientific name '" + lineValues1.ElementAt(3).Trim() + "' not processed correctly in getClassValue function. Contact i-Tree support for this issue");
                double num4 = this.TreatDotBlank(lineValues1.ElementAt(4).Trim());
                namedQuery.SetInt32("PlotId", int.Parse(lineValues1.ElementAt(1), this.nsInteger, (IFormatProvider) this.ciUsed)).SetInt32("TreeId", this.revertToOldTreeId(lineValues1.ElementAt(2).Trim(), treeIdNewToOld)).SetString("SppScientificName", this.m_ps.Species[key].ScientificName).SetString("SppCommonName", this.m_ps.Species[key].CommonName).SetDouble("DBH", this.TreatDotBlank(lineValues1.ElementAt(4).Trim())).SetDouble("Height", this.TreatDotBlank(lineValues1.ElementAt(5).Trim())).SetDouble("BasalArea", 3.14259165 * num4 * num4 / 4.0 / this.tenThousand).SetDouble("GroundArea", this.TreatDotBlank(lineValues1.ElementAt(6).Trim())).SetString("TreeCondition", lineValues1.ElementAt(7).Trim()).SetDouble("LeafArea", this.TreatDotBlank(lineValues1.ElementAt(8).Trim())).SetDouble("LeafBioMass", this.TreatDotBlank(lineValues1.ElementAt(9).Trim())).SetDouble("LeafAreaIndex", this.TreatDotBlank(lineValues1.ElementAt(10).Trim()));
                if (this.TreatDotBlank(lineValues1.ElementAt(6).Trim()) == 0.0)
                  namedQuery.SetDouble("LeafBiomassIndex", 0.0);
                else
                  namedQuery.SetDouble("LeafBiomassIndex", this.TreatDotBlank(lineValues1.ElementAt(9).Trim()) / this.TreatDotBlank(lineValues1.ElementAt(6).Trim()));
                namedQuery.SetDouble("CarbonStorage", this.TreatDotBlank(lineValues1.ElementAt(11).Trim())).SetDouble("GrossCarbonSeq", this.TreatDotBlank(lineValues1.ElementAt(12).Trim())).SetDouble("NetCarbonSeq", 0.0).SetDouble("TreeValue", this.TreatDotBlank(lineValues1.ElementAt(13).Trim())).SetString("StreetTree", lineValues1.ElementAt(14).Trim()).SetString("NativeToState", lineValues1.ElementAt(15).Trim()).SetDouble("TreeBiomass", 0.0);
                if (individualTreeCharacters == null)
                  namedQuery.SetDouble("FutureDBH", 0.0).SetDouble("FutureHeight", 0.0).SetDouble("FutureTreeBiomass", 0.0).SetDouble("BiomassAdj", 0.0).SetString("LeafType", (string) null);
                else
                  namedQuery.SetDouble("FutureDBH", individualTreeCharacters.FutureDBH).SetDouble("FutureHeight", individualTreeCharacters.FutureHeight).SetDouble("FutureTreeBiomass", individualTreeCharacters.FutureTreeBiomass).SetDouble("BiomassAdj", individualTreeCharacters.BiomassAdj).SetString("LeafType", individualTreeCharacters.LeafType);
                namedQuery.ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception("Processing file " + path2 + ": " + Environment.NewLine + ex.Message);
      }
    }

    private int revertToOldTreeId(string reOrderedTreeID, SASDictionary<string, int> treeIdNewToOld) => treeIdNewToOld.ContainsKey(reOrderedTreeID) ? treeIdNewToOld[reOrderedTreeID] : int.Parse(reOrderedTreeID, this.nsInteger, (IFormatProvider) this.ciUsed);

    private void getErrorNotes()
    {
      string path2 = "";
      try
      {
        IQuery namedQuery = this.project_s.GetNamedQuery("insertModelNotes");
        path2 = "ERRLOG.OUT";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          namedQuery.SetGuid("YearGuid", this.currYear.Guid).SetString("ParameterCalculatorNote", streamReader.ReadToEnd()).ExecuteUpdate();
          streamReader.Close();
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Processing file " + path2 + ": " + Environment.NewLine + ex.Message);
      }
    }

    private void getIPED()
    {
      if (!this.currYear.RecordIPED)
        return;
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Count, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        string str1 = "";
        string str2 = "";
        string str3 = "";
        string str4 = "";
        double num4 = 0.0;
        double num5 = 0.0;
        List<Classifiers> classifiersList = new List<Classifiers>();
        classifiersList.Add(Classifiers.PestPest);
        classifiersList.Add(Classifiers.TSDieback);
        classifiersList.Add(Classifiers.TSEpiSprout);
        classifiersList.Add(Classifiers.TSWiltFoli);
        classifiersList.Add(Classifiers.TSEnvStress);
        classifiersList.Add(Classifiers.TSHumStress);
        classifiersList.Add(Classifiers.FTChewFoli);
        classifiersList.Add(Classifiers.FTDiscFoli);
        classifiersList.Add(Classifiers.FTAbnFoli);
        classifiersList.Add(Classifiers.FTInsectSigns);
        classifiersList.Add(Classifiers.FTFoliAffect);
        classifiersList.Add(Classifiers.BBInsectSigns);
        classifiersList.Add(Classifiers.BBInsectPres);
        classifiersList.Add(Classifiers.BBDiseaseSigns);
        classifiersList.Add(Classifiers.BBAbnGrowth);
        classifiersList.Add(Classifiers.BBProbLoc);
        Dictionary<Classifiers, IQuery> dictionary1 = new Dictionary<Classifiers, IQuery>();
        XmlDocument xmlDocument = new XmlDocument();
        path2 = "IPED_TABLE.XML";
        xmlDocument.Load(Path.Combine(this.outputFolder, path2));
        SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        foreach (Classifiers key in classifiersList)
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(key);
          sasTransaction.Pause();
          string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable, partitionClassifiers);
          insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          dictionary1.Add(key, insertEstimateTable);
        }
        XmlNode findNodeByTagName1 = this.GetIPED_FindNodeByTagName(this.GetIPED_FindNodeByName((XmlNode) xmlDocument.DocumentElement, "By Landuse"), "Table");
        int i1 = -1;
        int i2 = -1;
        num3 = -1;
        int i3 = -1;
        int i4 = -1;
        int i5 = -1;
        int i6 = -1;
        str1 = "";
        str2 = "";
        str3 = "";
        str4 = "";
        num4 = 0.0;
        num5 = 0.0;
        for (int i7 = 0; i7 < findNodeByTagName1.ChildNodes.Count; ++i7)
        {
          if (findNodeByTagName1.ChildNodes[i7].InnerText.ToLower().IndexOf("standard error") >= 0)
          {
            XmlNode childNode = findNodeByTagName1.ChildNodes[i7];
            for (int i8 = 0; i8 < childNode.ChildNodes.Count; ++i8)
            {
              switch (childNode.ChildNodes[i8].ChildNodes[0].InnerText.Trim().ToLower(this.ciUsed))
              {
                case "common name":
                  num3 = i8;
                  break;
                case "landuse":
                  i1 = i8;
                  break;
                case "landuse description":
                  i2 = i8;
                  break;
                case "number of trees":
                  i5 = i8;
                  break;
                case "pestcode":
                  i3 = i8;
                  break;
                case "pestval":
                  i4 = i8;
                  break;
                case "standard error":
                  i6 = i8;
                  break;
              }
            }
            break;
          }
        }
        Dictionary<int, double> dictionary2 = new Dictionary<int, double>();
        Dictionary<int, double> dictionary3 = new Dictionary<int, double>();
        for (int i9 = 0; i9 < findNodeByTagName1.ChildNodes.Count; ++i9)
        {
          if (findNodeByTagName1.ChildNodes[i9].InnerText.ToLower().IndexOf("standard error") == -1 && findNodeByTagName1.ChildNodes[i9].InnerText.Trim() != "")
          {
            findNodeByTagName1.ChildNodes[i9].ChildNodes[i1].ChildNodes[0].InnerText.Trim();
            string key1 = findNodeByTagName1.ChildNodes[i9].ChildNodes[i2].ChildNodes[0].InnerText.Trim();
            string s = findNodeByTagName1.ChildNodes[i9].ChildNodes[i3].ChildNodes[0].InnerText.Trim();
            string str5 = findNodeByTagName1.ChildNodes[i9].ChildNodes[i4].ChildNodes[0].InnerText.Trim();
            if (str5 == "" || str5 == ".")
              str5 = "0";
            double val1 = this.TreatDotBlank(findNodeByTagName1.ChildNodes[i9].ChildNodes[i5].ChildNodes[0].InnerText.Trim());
            double val2 = this.TreatDotBlank(findNodeByTagName1.ChildNodes[i9].ChildNodes[i6].ChildNodes[0].InnerText.Trim());
            Classifiers key2 = (Classifiers) int.Parse(s, this.nsInteger, (IFormatProvider) this.ciUsed);
            dictionary1[key2].SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32(key2.ToString(), this.dictPestAndPestSymptomClassIDandSymptomIDtoClassValue[s + "_" + str5]).SetDouble("EstimateValue", val1).SetDouble("EstimateStandardError", val2).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            if (key2 == Classifiers.PestPest)
            {
              if (this.dictPestAndPestSymptomClassIDandSymptomIDtoClassValue[s + "_" + str5] == this.PestAffectedClassValue)
                dictionary2.Add(this.dictStrataNameToClassValue[key1], val1);
              if (this.dictPestAndPestSymptomClassIDandSymptomIDtoClassValue[s + "_" + str5] == this.PestNoneClassValue)
                dictionary3.Add(this.dictStrataNameToClassValue[key1], val1);
            }
          }
        }
        dictionary1.Clear();
        foreach (Classifiers key in classifiersList)
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Species);
          partitionClassifiers.Add(key);
          sasTransaction.Pause();
          string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable, partitionClassifiers);
          insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          dictionary1.Add(key, insertEstimateTable);
        }
        XmlNode findNodeByTagName2 = this.GetIPED_FindNodeByTagName(this.GetIPED_FindNodeByName((XmlNode) xmlDocument.DocumentElement, "By Landuse and Species"), "Table");
        int i10 = -1;
        int i11 = -1;
        int i12 = -1;
        int i13 = -1;
        int i14 = -1;
        int i15 = -1;
        int i16 = -1;
        str1 = "";
        str2 = "";
        str3 = "";
        str4 = "";
        num4 = 0.0;
        num5 = 0.0;
        for (int i17 = 0; i17 < findNodeByTagName2.ChildNodes.Count; ++i17)
        {
          if (findNodeByTagName2.ChildNodes[i17].InnerText.ToLower().IndexOf("standard error") >= 0)
          {
            XmlNode childNode = findNodeByTagName2.ChildNodes[i17];
            for (int i18 = 0; i18 < childNode.ChildNodes.Count; ++i18)
            {
              switch (childNode.ChildNodes[i18].ChildNodes[0].InnerText.Trim().ToLower(this.ciUsed))
              {
                case "common name":
                  i12 = i18;
                  break;
                case "landuse":
                  i10 = i18;
                  break;
                case "landuse description":
                  i11 = i18;
                  break;
                case "number of trees":
                  i15 = i18;
                  break;
                case "pestcode":
                  i13 = i18;
                  break;
                case "pestval":
                  i14 = i18;
                  break;
                case "standard error":
                  i16 = i18;
                  break;
              }
            }
            break;
          }
        }
        for (int i19 = 0; i19 < findNodeByTagName2.ChildNodes.Count; ++i19)
        {
          if (findNodeByTagName2.ChildNodes[i19].InnerText.ToLower().IndexOf("standard error") == -1 && findNodeByTagName2.ChildNodes[i19].InnerText.Trim() != "")
          {
            findNodeByTagName2.ChildNodes[i19].ChildNodes[i10].ChildNodes[0].InnerText.Trim();
            string key3 = findNodeByTagName2.ChildNodes[i19].ChildNodes[i11].ChildNodes[0].InnerText.Trim();
            string key4 = findNodeByTagName2.ChildNodes[i19].ChildNodes[i12].ChildNodes[0].InnerText.Trim();
            if (key4.Length != 0)
            {
              string s = findNodeByTagName2.ChildNodes[i19].ChildNodes[i13].ChildNodes[0].InnerText.Trim();
              string str6 = findNodeByTagName2.ChildNodes[i19].ChildNodes[i14].ChildNodes[0].InnerText.Trim();
              if (str6 == "" || str6 == ".")
                str6 = "0";
              double val3 = this.TreatDotBlank(findNodeByTagName2.ChildNodes[i19].ChildNodes[i15].ChildNodes[0].InnerText.Trim());
              double val4 = this.TreatDotBlank(findNodeByTagName2.ChildNodes[i19].ChildNodes[i16].ChildNodes[0].InnerText.Trim());
              Classifiers key5 = (Classifiers) int.Parse(s, this.nsInteger, (IFormatProvider) this.ciUsed);
              IQuery query1 = dictionary1[key5];
              Classifiers classifiers = Classifiers.Strata;
              string name1 = classifiers.ToString();
              int val5 = this.dictStrataNameToClassValue[key3];
              IQuery query2 = query1.SetInt32(name1, val5);
              classifiers = Classifiers.Species;
              string name2 = classifiers.ToString();
              int val6 = this.dictSpeciesCodeToClassValue[key4];
              query2.SetInt32(name2, val6).SetInt32(key5.ToString(), this.dictPestAndPestSymptomClassIDandSymptomIDtoClassValue[s + "_" + str6]).SetDouble("EstimateValue", val3).SetDouble("EstimateStandardError", val4).ExecuteUpdate();
              sasTransaction.IncreaseOperationNumber();
            }
          }
        }
        dictionary1.Clear();
        foreach (Classifiers key in classifiersList)
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Species);
          partitionClassifiers.Add(key);
          sasTransaction.Pause();
          string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable, partitionClassifiers);
          insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          dictionary1.Add(key, insertEstimateTable);
        }
        XmlNode findNodeByTagName3 = this.GetIPED_FindNodeByTagName(this.GetIPED_FindNodeByName((XmlNode) xmlDocument.DocumentElement, "By Species"), "Table");
        num1 = -1;
        num2 = -1;
        int i20 = -1;
        int i21 = -1;
        int i22 = -1;
        int i23 = -1;
        int i24 = -1;
        str1 = "";
        str2 = "";
        str3 = "";
        str4 = "";
        num4 = 0.0;
        num5 = 0.0;
        for (int i25 = 0; i25 < findNodeByTagName3.ChildNodes.Count; ++i25)
        {
          if (findNodeByTagName3.ChildNodes[i25].InnerText.ToLower().IndexOf("standard error") >= 0)
          {
            XmlNode childNode = findNodeByTagName3.ChildNodes[i25];
            for (int i26 = 0; i26 < childNode.ChildNodes.Count; ++i26)
            {
              switch (childNode.ChildNodes[i26].ChildNodes[0].InnerText.Trim().ToLower(this.ciUsed))
              {
                case "common name":
                  i20 = i26;
                  break;
                case "landuse":
                  num1 = i26;
                  break;
                case "landuse description":
                  num2 = i26;
                  break;
                case "number of trees":
                  i23 = i26;
                  break;
                case "pestcode":
                  i21 = i26;
                  break;
                case "pestval":
                  i22 = i26;
                  break;
                case "standard error":
                  i24 = i26;
                  break;
              }
            }
            break;
          }
        }
        Dictionary<int, double> dictionary4 = new Dictionary<int, double>();
        Dictionary<int, double> dictionary5 = new Dictionary<int, double>();
        for (int i27 = 0; i27 < findNodeByTagName3.ChildNodes.Count; ++i27)
        {
          if (findNodeByTagName3.ChildNodes[i27].InnerText.ToLower().IndexOf("standard error") == -1 && findNodeByTagName3.ChildNodes[i27].InnerText.Trim() != "")
          {
            string key6 = findNodeByTagName3.ChildNodes[i27].ChildNodes[i20].ChildNodes[0].InnerText.Trim();
            if (key6.Length != 0)
            {
              string s = findNodeByTagName3.ChildNodes[i27].ChildNodes[i21].ChildNodes[0].InnerText.Trim();
              string str7 = findNodeByTagName3.ChildNodes[i27].ChildNodes[i22].ChildNodes[0].InnerText.Trim();
              if (str7 == "" || str7 == ".")
                str7 = "0";
              double val7 = this.TreatDotBlank(findNodeByTagName3.ChildNodes[i27].ChildNodes[i23].ChildNodes[0].InnerText.Trim());
              double val8 = this.TreatDotBlank(findNodeByTagName3.ChildNodes[i27].ChildNodes[i24].ChildNodes[0].InnerText.Trim());
              Classifiers key7 = (Classifiers) int.Parse(s, this.nsInteger, (IFormatProvider) this.ciUsed);
              dictionary1[key7].SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[key6]).SetInt32(key7.ToString(), this.dictPestAndPestSymptomClassIDandSymptomIDtoClassValue[s + "_" + str7]).SetDouble("EstimateValue", val7).SetDouble("EstimateStandardError", val8).ExecuteUpdate();
              sasTransaction.IncreaseOperationNumber();
              if (key7 == Classifiers.PestPest)
              {
                if (this.dictPestAndPestSymptomClassIDandSymptomIDtoClassValue[s + "_" + str7] == this.PestAffectedClassValue)
                  dictionary4.Add(this.dictSpeciesCodeToClassValue[key6], val7);
                if (this.dictPestAndPestSymptomClassIDandSymptomIDtoClassValue[s + "_" + str7] == this.PestNoneClassValue)
                  dictionary5.Add(this.dictSpeciesCodeToClassValue[key6], val7);
              }
            }
          }
        }
        partitionClassifiers.Clear();
        partitionClassifiers.Add(Classifiers.Strata);
        partitionClassifiers.Add(Classifiers.PestPest);
        sasTransaction.Pause();
        string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
        sasTransaction.Resume();
        IQuery insertEstimateTable1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers);
        insertEstimateTable1.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
        XmlNode findNodeByTagName4 = this.GetIPED_FindNodeByTagName(this.GetIPED_FindNodeByName((XmlNode) xmlDocument.DocumentElement, "Status by Landuse"), "Table");
        int i28 = -1;
        int i29 = -1;
        num3 = -1;
        int i30 = -1;
        int i31 = -1;
        int i32 = -1;
        int i33 = -1;
        str1 = "";
        str2 = "";
        str3 = "";
        str4 = "";
        num4 = 0.0;
        num5 = 0.0;
        for (int i34 = 0; i34 < findNodeByTagName4.ChildNodes.Count; ++i34)
        {
          if (findNodeByTagName4.ChildNodes[i34].InnerText.ToLower().IndexOf("standard error") >= 0)
          {
            XmlNode childNode = findNodeByTagName4.ChildNodes[i34];
            for (int i35 = 0; i35 < childNode.ChildNodes.Count; ++i35)
            {
              switch (childNode.ChildNodes[i35].ChildNodes[0].InnerText.Trim().ToLower(this.ciUsed))
              {
                case "common name":
                  num3 = i35;
                  break;
                case "landuse":
                  i28 = i35;
                  break;
                case "landuse description":
                  i29 = i35;
                  break;
                case "number of trees":
                  i32 = i35;
                  break;
                case "pestcode":
                  i30 = i35;
                  break;
                case "pestval":
                  i31 = i35;
                  break;
                case "standard error":
                  i33 = i35;
                  break;
              }
            }
            break;
          }
        }
        Classifiers classifiers1;
        for (int i36 = 0; i36 < findNodeByTagName4.ChildNodes.Count; ++i36)
        {
          if (findNodeByTagName4.ChildNodes[i36].InnerText.ToLower().IndexOf("standard error") == -1 && findNodeByTagName4.ChildNodes[i36].InnerText.Trim() != "")
          {
            findNodeByTagName4.ChildNodes[i36].ChildNodes[i28].ChildNodes[0].InnerText.Trim();
            string key = findNodeByTagName4.ChildNodes[i36].ChildNodes[i29].ChildNodes[0].InnerText.Trim();
            string str8 = findNodeByTagName4.ChildNodes[i36].ChildNodes[i30].ChildNodes[0].InnerText.Trim();
            string str9 = findNodeByTagName4.ChildNodes[i36].ChildNodes[i31].ChildNodes[0].InnerText.Trim();
            if (str9 == "" || str9 == ".")
              str9 = "0";
            double val9 = this.TreatDotBlank(findNodeByTagName4.ChildNodes[i36].ChildNodes[i32].ChildNodes[0].InnerText.Trim());
            double val10 = this.TreatDotBlank(findNodeByTagName4.ChildNodes[i36].ChildNodes[i33].ChildNodes[0].InnerText.Trim());
            if (str8 == 38.ToString((IFormatProvider) this.ciUsed))
            {
              if (str9 == "0")
              {
                if (!dictionary3.ContainsKey(this.dictStrataNameToClassValue[key]))
                {
                  IQuery query3 = insertEstimateTable1;
                  classifiers1 = Classifiers.Strata;
                  string name3 = classifiers1.ToString();
                  int val11 = this.dictStrataNameToClassValue[key];
                  IQuery query4 = query3.SetInt32(name3, val11);
                  classifiers1 = Classifiers.PestPest;
                  string name4 = classifiers1.ToString();
                  int pestNoneClassValue = this.PestNoneClassValue;
                  query4.SetInt32(name4, pestNoneClassValue).SetDouble("EstimateValue", val9).SetDouble("EstimateStandardError", val10).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  dictionary3.Add(this.dictStrataNameToClassValue[key], val9);
                }
              }
              else if (str9 == "1" && !dictionary2.ContainsKey(this.dictStrataNameToClassValue[key]))
              {
                IQuery query5 = insertEstimateTable1;
                classifiers1 = Classifiers.Strata;
                string name5 = classifiers1.ToString();
                int val12 = this.dictStrataNameToClassValue[key];
                IQuery query6 = query5.SetInt32(name5, val12);
                classifiers1 = Classifiers.PestPest;
                string name6 = classifiers1.ToString();
                int affectedClassValue = this.PestAffectedClassValue;
                query6.SetInt32(name6, affectedClassValue).SetDouble("EstimateValue", val9).SetDouble("EstimateStandardError", val10).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                dictionary2.Add(this.dictStrataNameToClassValue[key], val9);
              }
            }
          }
        }
        foreach (int key in this.dictStrataNameToClassValue.Values)
        {
          if (dictionary2.ContainsKey(key) || dictionary3.ContainsKey(key))
          {
            if (!dictionary3.ContainsKey(key))
            {
              IQuery query7 = insertEstimateTable1;
              classifiers1 = Classifiers.Strata;
              string name7 = classifiers1.ToString();
              int val = key;
              IQuery query8 = query7.SetInt32(name7, val);
              classifiers1 = Classifiers.PestPest;
              string name8 = classifiers1.ToString();
              int pestNoneClassValue = this.PestNoneClassValue;
              query8.SetInt32(name8, pestNoneClassValue).SetDouble("EstimateValue", 0.0).SetDouble("EstimateStandardError", 0.0).ExecuteUpdate();
              sasTransaction.IncreaseOperationNumber();
            }
            if (!dictionary2.ContainsKey(key))
            {
              IQuery query9 = insertEstimateTable1;
              classifiers1 = Classifiers.Strata;
              string name9 = classifiers1.ToString();
              int val = key;
              IQuery query10 = query9.SetInt32(name9, val);
              classifiers1 = Classifiers.PestPest;
              string name10 = classifiers1.ToString();
              int affectedClassValue = this.PestAffectedClassValue;
              query10.SetInt32(name10, affectedClassValue).SetDouble("EstimateValue", 0.0).SetDouble("EstimateStandardError", 0.0).ExecuteUpdate();
              sasTransaction.IncreaseOperationNumber();
            }
          }
        }
        partitionClassifiers.Clear();
        partitionClassifiers.Add(Classifiers.Species);
        partitionClassifiers.Add(Classifiers.PestPest);
        sasTransaction.Pause();
        string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
        sasTransaction.Resume();
        IQuery insertEstimateTable2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers);
        insertEstimateTable2.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
        XmlNode findNodeByTagName5 = this.GetIPED_FindNodeByTagName(this.GetIPED_FindNodeByName((XmlNode) xmlDocument.DocumentElement, "Status by Species"), "Table");
        num1 = -1;
        num2 = -1;
        int i37 = -1;
        int i38 = -1;
        int i39 = -1;
        int i40 = -1;
        int i41 = -1;
        str1 = "";
        str2 = "";
        str3 = "";
        str4 = "";
        num4 = 0.0;
        num5 = 0.0;
        for (int i42 = 0; i42 < findNodeByTagName5.ChildNodes.Count; ++i42)
        {
          if (findNodeByTagName5.ChildNodes[i42].InnerText.ToLower().IndexOf("standard error") >= 0)
          {
            XmlNode childNode = findNodeByTagName5.ChildNodes[i42];
            for (int i43 = 0; i43 < childNode.ChildNodes.Count; ++i43)
            {
              switch (childNode.ChildNodes[i43].ChildNodes[0].InnerText.Trim().ToLower(this.ciUsed))
              {
                case "common name":
                  i37 = i43;
                  break;
                case "landuse":
                  num1 = i43;
                  break;
                case "landuse description":
                  num2 = i43;
                  break;
                case "number of trees":
                  i40 = i43;
                  break;
                case "pestcode":
                  i38 = i43;
                  break;
                case "pestval":
                  i39 = i43;
                  break;
                case "standard error":
                  i41 = i43;
                  break;
              }
            }
            break;
          }
        }
        Classifiers classifiers2;
        for (int i44 = 0; i44 < findNodeByTagName5.ChildNodes.Count; ++i44)
        {
          if (findNodeByTagName5.ChildNodes[i44].InnerText.ToLower().IndexOf("standard error") == -1 && findNodeByTagName5.ChildNodes[i44].InnerText.Trim() != "")
          {
            string key = findNodeByTagName5.ChildNodes[i44].ChildNodes[i37].ChildNodes[0].InnerText.Trim();
            if (key.Length != 0)
            {
              string str10 = findNodeByTagName5.ChildNodes[i44].ChildNodes[i38].ChildNodes[0].InnerText.Trim();
              string str11 = findNodeByTagName5.ChildNodes[i44].ChildNodes[i39].ChildNodes[0].InnerText.Trim();
              if (str11 == "" || str11 == ".")
                str11 = "0";
              double val13 = this.TreatDotBlank(findNodeByTagName5.ChildNodes[i44].ChildNodes[i40].ChildNodes[0].InnerText.Trim());
              double val14 = this.TreatDotBlank(findNodeByTagName5.ChildNodes[i44].ChildNodes[i41].ChildNodes[0].InnerText.Trim());
              if (str10 == 38.ToString((IFormatProvider) this.ciUsed))
              {
                if (str11 == "0")
                {
                  if (!dictionary5.ContainsKey(this.dictSpeciesCodeToClassValue[key]))
                  {
                    IQuery query11 = insertEstimateTable2;
                    classifiers2 = Classifiers.Species;
                    string name11 = classifiers2.ToString();
                    int val15 = this.dictSpeciesCodeToClassValue[key];
                    IQuery query12 = query11.SetInt32(name11, val15);
                    classifiers2 = Classifiers.PestPest;
                    string name12 = classifiers2.ToString();
                    int pestNoneClassValue = this.PestNoneClassValue;
                    query12.SetInt32(name12, pestNoneClassValue).SetDouble("EstimateValue", val13).SetDouble("EstimateStandardError", val14).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    dictionary5.Add(this.dictSpeciesCodeToClassValue[key], val13);
                  }
                }
                else if (str11 == "1" && !dictionary4.ContainsKey(this.dictSpeciesCodeToClassValue[key]))
                {
                  IQuery query13 = insertEstimateTable2;
                  classifiers2 = Classifiers.Species;
                  string name13 = classifiers2.ToString();
                  int val16 = this.dictSpeciesCodeToClassValue[key];
                  IQuery query14 = query13.SetInt32(name13, val16);
                  classifiers2 = Classifiers.PestPest;
                  string name14 = classifiers2.ToString();
                  int affectedClassValue = this.PestAffectedClassValue;
                  query14.SetInt32(name14, affectedClassValue).SetDouble("EstimateValue", val13).SetDouble("EstimateStandardError", val14).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  dictionary4.Add(this.dictSpeciesCodeToClassValue[key], val13);
                }
              }
            }
          }
        }
        foreach (int key in this.dictSpeciesCodeToClassValue.Values)
        {
          if (dictionary5.ContainsKey(key) || dictionary4.ContainsKey(key))
          {
            if (!dictionary5.ContainsKey(key))
            {
              IQuery query15 = insertEstimateTable2;
              classifiers2 = Classifiers.Species;
              string name15 = classifiers2.ToString();
              int val = key;
              IQuery query16 = query15.SetInt32(name15, val);
              classifiers2 = Classifiers.PestPest;
              string name16 = classifiers2.ToString();
              int pestNoneClassValue = this.PestNoneClassValue;
              query16.SetInt32(name16, pestNoneClassValue).SetDouble("EstimateValue", 0.0).SetDouble("EstimateStandardError", 0.0).ExecuteUpdate();
              sasTransaction.IncreaseOperationNumber();
            }
            if (!dictionary4.ContainsKey(key))
            {
              IQuery query17 = insertEstimateTable2;
              classifiers2 = Classifiers.Species;
              string name17 = classifiers2.ToString();
              int val = key;
              IQuery query18 = query17.SetInt32(name17, val);
              classifiers2 = Classifiers.PestPest;
              string name18 = classifiers2.ToString();
              int affectedClassValue = this.PestAffectedClassValue;
              query18.SetInt32(name18, affectedClassValue).SetDouble("EstimateValue", 0.0).SetDouble("EstimateStandardError", 0.0).ExecuteUpdate();
              sasTransaction.IncreaseOperationNumber();
            }
          }
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private XmlNode GetIPED_FindNodeByName(XmlNode parentNodeObj, string ChildNodeName)
    {
      for (int i = 0; i < parentNodeObj.ChildNodes.Count; ++i)
      {
        if (parentNodeObj.ChildNodes[i].Attributes.GetNamedItem("ss:Name") != null && parentNodeObj.ChildNodes[i].Attributes.GetNamedItem("ss:Name").Value.ToUpper(this.ciUsed) == ChildNodeName.ToUpper(this.ciUsed))
          return parentNodeObj.ChildNodes[i];
      }
      throw new Exception(string.Format(SASResources.ErrorNodeNotFound, (object) ChildNodeName));
    }

    private XmlNode GetIPED_FindNodeByTagName(XmlNode parentNodeObj, string ChildTagName)
    {
      for (int i = 0; i < parentNodeObj.ChildNodes.Count; ++i)
      {
        if (parentNodeObj.ChildNodes[i].Name.ToUpper(this.ciUsed) == ChildTagName.ToUpper(this.ciUsed))
          return parentNodeObj.ChildNodes[i];
      }
      throw new Exception(string.Format(SASResources.ErrorTagNotFound, (object) ChildTagName));
    }

    private void getSHRUBEST()
    {
      if (!this.currYear.RecordShrub)
        return;
      int unit1 = this.getUnit(this.project_s, Units.Squaremeter, Units.Hectare, Units.None);
      int unit2 = this.getUnit(this.project_s, Units.Squarekilometer, Units.Hectare, Units.None);
      int unit3 = this.getUnit(this.project_s, Units.Kilograms, Units.Hectare, Units.None);
      int unit4 = this.getUnit(this.project_s, Units.MetricTons, Units.Hectare, Units.None);
      int unit5 = this.getUnit(this.project_s, Units.Squarekilometer, Units.None, Units.None);
      int unit6 = this.getUnit(this.project_s, Units.Squaremeter, Units.None, Units.None);
      int unit7 = this.getUnit(this.project_s, Units.MetricTons, Units.None, Units.None);
      int unit8 = this.getUnit(this.project_s, Units.Kilograms, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        this.getDensityCoverPercentRatio();
        SASDictionary<string, double> sasDictionary1 = new SASDictionary<string, double>("Species");
        SASDictionary<string, double> sasDictionary2 = new SASDictionary<string, double>("Species");
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        using (StreamReader streamReader = new StreamReader(this.outputFolder + "\\SHRUBEST.CSV"))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          sasTransaction.Pause();
          string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Shrub, partitionClassifiers);
          sasTransaction.Resume();
          SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
          IQuery insertEstimateTable1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers);
          insertEstimateTable1.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1);
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Species);
          sasTransaction.Pause();
          string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Shrub, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers);
          insertEstimateTable2.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1);
          double result = 0.0;
          bool flag1 = false;
          string key1 = "";
          LineValues lineValues = new LineValues();
          string str;
          Classifiers classifiers;
          while ((str = streamReader.ReadLine()) != null)
          {
            flag1 = false;
            string aLine = str.Trim();
            if (!(aLine == ""))
            {
              lineValues.SetLine(aLine);
              if (lineValues.Count == 1)
                key1 = lineValues.ElementAt(1).Trim();
              if (lineValues.ElementAt(1).ToUpper(this.ciUsed).Trim() == "CITY TOTAL")
                key1 = "CITY TOTAL";
              bool flag2;
              if (lineValues.Count >= 3)
              {
                string s = lineValues.ElementAt(3).Trim();
                flag2 = s == "" || s == "." || double.TryParse(s, this.nsFloat, (IFormatProvider) this.ciUsed, out result);
              }
              else
                flag2 = false;
              if (flag2)
              {
                if (lineValues.ElementAt(1).Trim() != "")
                  key1 = lineValues.ElementAt(1).Trim();
                string key2 = !(lineValues.ElementAt(2).Trim() != "") ? "Total" : lineValues.ElementAt(2).Trim();
                if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  key2 = "Total";
                if (!(key2.ToUpper(this.ciUsed).Trim() == "NONE.") && !(key2.ToUpper(this.ciUsed).Trim() == "NONE"))
                {
                  double num = this.dictStrataNameToDensityRatio[key1];
                  IQuery query1 = insertEstimateTable2;
                  classifiers = Classifiers.Strata;
                  string name1 = classifiers.ToString();
                  int val1 = this.dictStrataNameToClassValue[key1];
                  IQuery query2 = query1.SetInt32(name1, val1);
                  classifiers = Classifiers.Species;
                  string name2 = classifiers.ToString();
                  int val2 = this.dictSpeciesCodeToClassValue[key2];
                  query2.SetInt32(name2, val2).SetInt32("EstimateType", 5).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(3).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(4).Trim())).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(3).Trim()) / this.million).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(4).Trim()) / this.million).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    IQuery query3 = insertEstimateTable1;
                    classifiers = Classifiers.Strata;
                    string name3 = classifiers.ToString();
                    int val3 = this.dictStrataNameToClassValue[key1];
                    query3.SetInt32(name3, val3).SetInt32("EstimateType", 5).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(3).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(4).Trim())).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(3).Trim()) / this.million).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(4).Trim()) / this.million).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 6).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(5).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(6).Trim())).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(5).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(6).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32("EstimateType", 6).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(5).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(6).Trim())).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(5).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(6).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 5).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(7).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(8).Trim())).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(7).Trim()) * this.million).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(8).Trim()) * this.million).SetInt32("EstimateUnitsId", unit6).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32("EstimateType", 5).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(7).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(8).Trim())).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(7).Trim()) * this.million).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(8).Trim()) * this.million).SetInt32("EstimateUnitsId", unit6).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 6).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(9).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(10).Trim())).SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(9).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(10).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit8).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32("EstimateType", 6).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(9).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(10).Trim())).SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(9).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(10).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit8).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  if (key1 != "CITY TOTAL")
                  {
                    if (sasDictionary1.ContainsKey(key2))
                      sasDictionary1[key2] += this.TreatDotBlank(lineValues.ElementAt(7).Trim());
                    else
                      sasDictionary1.Add(key2, this.TreatDotBlank(lineValues.ElementAt(7).Trim()));
                    if (sasDictionary2.ContainsKey(key2))
                      sasDictionary2[key2] += this.TreatDotBlank(lineValues.ElementAt(9).Trim());
                    else
                      sasDictionary2.Add(key2, this.TreatDotBlank(lineValues.ElementAt(9).Trim()));
                  }
                }
              }
            }
          }
          streamReader.Close();
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Species);
          sasTransaction.Pause();
          string estimationTable3 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Shrub, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable3 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable3, partitionClassifiers);
          insertEstimateTable3.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1).SetDouble("EstimateStandardError", 0.0);
          foreach (KeyValuePair<string, double> keyValuePair in (Dictionary<string, double>) sasDictionary1)
          {
            int num = this.dictSpeciesCodeToClassValue[keyValuePair.Key];
            IQuery query = insertEstimateTable3;
            classifiers = Classifiers.Species;
            string name = classifiers.ToString();
            int val = num;
            query.SetInt32(name, val).SetInt32("EstimateType", 5);
            insertEstimateTable3.SetDouble("EstimateValue", keyValuePair.Value).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            insertEstimateTable3.SetDouble("EstimateValue", keyValuePair.Value * this.million).SetInt32("EstimateUnitsId", unit6).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            insertEstimateTable3.SetDouble("EstimateValue", keyValuePair.Value / this.TotalStudyAreaHectare).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            insertEstimateTable3.SetDouble("EstimateValue", keyValuePair.Value * this.million / this.TotalStudyAreaHectare).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
          }
          foreach (KeyValuePair<string, double> keyValuePair in (Dictionary<string, double>) sasDictionary2)
          {
            int val = this.dictSpeciesCodeToClassValue[keyValuePair.Key];
            insertEstimateTable3.SetInt32(Classifiers.Species.ToString(), val).SetInt32("EstimateType", 6);
            insertEstimateTable3.SetDouble("EstimateValue", keyValuePair.Value).SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            insertEstimateTable3.SetDouble("EstimateValue", keyValuePair.Value * this.thousand).SetInt32("EstimateUnitsId", unit8).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            insertEstimateTable3.SetDouble("EstimateValue", keyValuePair.Value / this.TotalStudyAreaHectare).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            insertEstimateTable3.SetDouble("EstimateValue", keyValuePair.Value * this.thousand / this.TotalStudyAreaHectare).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
          }
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw;
      }
    }

    private void getTRESHBLF()
    {
      string path2 = "";
      int unit1 = this.getUnit(this.project_s, Units.Squaremeter, Units.Hectare, Units.None);
      int unit2 = this.getUnit(this.project_s, Units.Squarekilometer, Units.Hectare, Units.None);
      int unit3 = this.getUnit(this.project_s, Units.Kilograms, Units.Hectare, Units.None);
      int unit4 = this.getUnit(this.project_s, Units.MetricTons, Units.Hectare, Units.None);
      int unit5 = this.getUnit(this.project_s, Units.Squarekilometer, Units.None, Units.None);
      int unit6 = this.getUnit(this.project_s, Units.Squaremeter, Units.None, Units.None);
      int unit7 = this.getUnit(this.project_s, Units.MetricTons, Units.None, Units.None);
      int unit8 = this.getUnit(this.project_s, Units.Kilograms, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        this.getDensityCoverPercentRatio();
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "TRESHBLF.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          sasTransaction.Pause();
          string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.TreeShrubCombined, partitionClassifiers);
          sasTransaction.Resume();
          SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
          IQuery insertEstimateTable1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers);
          insertEstimateTable1.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1);
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Species);
          sasTransaction.Pause();
          string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.TreeShrubCombined, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers);
          insertEstimateTable2.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1);
          double result = 0.0;
          bool flag1 = false;
          string key1 = "";
          LineValues lineValues = new LineValues();
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            flag1 = false;
            string aLine = str.Trim();
            if (!(aLine == ""))
            {
              lineValues.SetLine(aLine);
              if (lineValues.Count == 1)
                key1 = lineValues.ElementAt(1).Trim();
              if (lineValues.ElementAt(1).ToUpper(this.ciUsed).Trim() == "CITY TOTAL")
                key1 = "CITY TOTAL";
              bool flag2;
              if (lineValues.Count >= 3)
              {
                string s = lineValues.ElementAt(3).Trim();
                flag2 = s == "" || s == "." || double.TryParse(s, this.nsFloat, (IFormatProvider) this.ciUsed, out result);
              }
              else
                flag2 = false;
              if (flag2)
              {
                if (lineValues.ElementAt(1).Trim() != "")
                  key1 = lineValues.ElementAt(1).Trim();
                string key2 = !(lineValues.ElementAt(2).Trim() != "") ? "Total" : lineValues.ElementAt(2).Trim();
                if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  key2 = "Total";
                if (!(key2.ToUpper(this.ciUsed).Trim() == "NONE.") && !(key2.ToUpper(this.ciUsed).Trim() == "NONE"))
                {
                  double num = this.dictStrataNameToDensityRatio[key1];
                  insertEstimateTable2.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[key2]).SetInt32("EstimateType", 5).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(3).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(4).Trim())).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(3).Trim()) / this.million).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(4).Trim()) / this.million).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32("EstimateType", 5).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(3).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(4).Trim())).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(3).Trim()) / this.million).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(4).Trim()) / this.million).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 6).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(5).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(6).Trim())).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(5).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(6).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32("EstimateType", 6).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(5).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(6).Trim())).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(5).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(6).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 5).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(7).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(8).Trim())).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(7).Trim()) * this.million).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(8).Trim()) * this.million).SetInt32("EstimateUnitsId", unit6).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32("EstimateType", 5).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(7).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(8).Trim())).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(7).Trim()) * this.million).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(8).Trim()) * this.million).SetInt32("EstimateUnitsId", unit6).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 6).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(9).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(10).Trim())).SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(9).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(10).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit8).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32("EstimateType", 6).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(9).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(10).Trim())).SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(9).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(10).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit8).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception("Processing file " + path2 + ": " + Environment.NewLine + ex.Message);
      }
    }

    private void getTOTLESTM()
    {
      string path2 = "";
      int unit1 = this.getUnit(this.project_s, Units.Count, Units.None, Units.None);
      int unit2 = this.getUnit(this.project_s, Units.MetricTons, Units.None, Units.None);
      int unit3 = this.getUnit(this.project_s, Units.Kilograms, Units.None, Units.None);
      int unit4 = this.getUnit(this.project_s, Units.MetricTons, Units.Year, Units.None);
      int unit5 = this.getUnit(this.project_s, Units.Kilograms, Units.Year, Units.None);
      int unit6 = this.getUnit(this.project_s, Units.Squarekilometer, Units.None, Units.None);
      int unit7 = this.getUnit(this.project_s, Units.Squaremeter, Units.None, Units.None);
      int unit8 = this.getUnit(this.project_s, Units.Monetaryunit, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "TOTLESTM.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          sasTransaction.Pause();
          string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
          IQuery insertEstimateTable1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers);
          insertEstimateTable1.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1);
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Species);
          sasTransaction.Pause();
          string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers);
          insertEstimateTable2.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1);
          double result = 0.0;
          bool flag1 = false;
          string key1 = "";
          LineValues lineValues = new LineValues();
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            flag1 = false;
            string aLine = str.Trim();
            if (!(aLine == ""))
            {
              lineValues.SetLine(aLine);
              if (lineValues.Count == 1)
                key1 = lineValues.ElementAt(1).Trim();
              if (lineValues.ElementAt(1).ToUpper(this.ciUsed).Trim() == "CITY TOTAL")
                key1 = "CITY TOTAL";
              bool flag2;
              if (lineValues.Count >= 3)
              {
                string s = lineValues.ElementAt(3).Trim();
                flag2 = s == "" || s == "." || double.TryParse(s, this.nsFloat, (IFormatProvider) this.ciUsed, out result);
              }
              else
                flag2 = false;
              if (flag2)
              {
                if (lineValues.ElementAt(1).Trim() != "")
                  key1 = lineValues.ElementAt(1).Trim();
                string key2 = !(lineValues.ElementAt(2).Trim() != "") ? "Total" : lineValues.ElementAt(2).Trim();
                if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  key2 = "Total";
                if (!(key2.ToUpper(this.ciUsed).Trim() == "NONE.") && !(key2.ToUpper(this.ciUsed).Trim() == "NONE"))
                {
                  insertEstimateTable2.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[key2]).SetInt32("EstimateType", 1).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(3).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(4).Trim())).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32("EstimateType", 1).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(3).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(4).Trim())).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 2).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(5).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(6).Trim())).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(5).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(6).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32("EstimateType", 2).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(5).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(6).Trim())).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(5).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(6).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 3).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(7).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(8).Trim())).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(7).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(8).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32("EstimateType", 3).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(7).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(8).Trim())).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(7).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(8).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 4).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(9).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(10).Trim())).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(9).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(10).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32("EstimateType", 4).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(9).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(10).Trim())).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(9).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(10).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 5).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(11).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(12).Trim())).SetInt32("EstimateUnitsId", unit6).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(11).Trim()) * this.million).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(12).Trim()) * this.million).SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32("EstimateType", 5).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(11).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(12).Trim())).SetInt32("EstimateUnitsId", unit6).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(11).Trim()) * this.million).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(12).Trim()) * this.million).SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 6).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(13).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(14).Trim())).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(13).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(14).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32("EstimateType", 6).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(13).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(14).Trim())).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(13).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(14).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 8).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(15).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(16).Trim())).SetInt32("EstimateUnitsId", unit8).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32("EstimateType", 8).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(15).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(16).Trim())).SetInt32("EstimateUnitsId", unit8).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception("Processing file " + path2 + ": " + Environment.NewLine + ex.Message);
      }
    }

    private void getPERCOVES()
    {
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        this.getDensityCoverPercentRatio();
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        partitionClassifiers.Clear();
        partitionClassifiers.Add(Classifiers.Strata);
        partitionClassifiers.Add(Classifiers.GroundCover);
        sasTransaction.Pause();
        string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Cover, partitionClassifiers);
        sasTransaction.Resume();
        IQuery insertEstimateTable = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider().GetSQLQueryInsertEstimateTable(estimationTable, partitionClassifiers);
        insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 12).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
        IQuery namedQuery = this.project_s.GetNamedQuery("insertClassValue");
        namedQuery.SetGuid("yearGuid", this.currYear.Guid);
        namedQuery.SetInt32("classifier", 7);
        namedQuery.SetString("classValueName1", "");
        namedQuery.SetInt32("classValueFlag", 0);
        namedQuery.SetString("sppCode", (string) null);
        LineValues aLine1 = new LineValues();
        path2 = "CUSTOMGROUNDCOVER.csv";
        if (this.currYear.RecordGroundCover && System.IO.File.Exists(Path.Combine(this.outputFolder, path2)))
        {
          List<GroundCover> list = this.currYear.GroundCovers.OrderBy<GroundCover, int>((Func<GroundCover, int>) (gc => gc.Id)).ToList<GroundCover>();
          short val1 = 0;
          SASDictionary<int, short> sasDictionary = new SASDictionary<int, short>();
          foreach (GroundCover groundCover in (IEnumerable<GroundCover>) list)
          {
            ++val1;
            namedQuery.SetInt16("classValueOrder", val1).SetString("classValueName", groundCover.Description);
            namedQuery.ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            sasDictionary.Add(groundCover.Id, val1);
          }
          short val2 = (short) ((int) val1 + 1);
          this.ClassValueForPlantableSpaceOfGroundCover = (int) val2;
          namedQuery.SetInt16("classValueOrder", val2).SetString("classValueName", "PLANTABLE SPACE");
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          short val3 = (short) ((int) val2 + 1);
          this.ClassValueForTreeOfGroundCover = (int) val3;
          namedQuery.SetInt16("classValueOrder", val3).SetString("classValueName", "TREE");
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          short val4 = (short) ((int) val3 + 1);
          this.ClassValueForShrubOfGroundCover = (int) val4;
          namedQuery.SetInt16("classValueOrder", val4).SetString("classValueName", "SHRUB");
          namedQuery.ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber();
          using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
          {
            string str;
            while ((str = streamReader.ReadLine()) != null)
            {
              string aLine2 = str.Trim();
              if (!(aLine2 == ""))
              {
                aLine1.SetLine(aLine2);
                if (aLine1.ElementAt(1).ToUpper(this.ciUsed) != "LANDUSE")
                {
                  string key = aLine1.ElementAt(1).Trim();
                  double num = this.dictStrataNameToCoverPercentRatio[key];
                  insertEstimateTable.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key]).SetInt32(Classifiers.GroundCover.ToString(), (int) sasDictionary[int.Parse(aLine1.ElementAt(2))]).SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(3).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(4).Trim())).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                }
              }
            }
            streamReader.Close();
          }
          path2 = "PERCOVES.CSV";
          using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
          {
            int index1 = -1;
            int index2 = -1;
            int index3 = -1;
            string str;
            while ((str = streamReader.ReadLine()) != null)
            {
              string aLine3 = str.Trim();
              if (!(aLine3 == ""))
              {
                aLine1.SetLine(aLine3);
                if (aLine1.ElementAt(1).Trim() == "" && aLine1.Count >= 3)
                {
                  for (int index4 = 2; index4 <= aLine1.Count; ++index4)
                  {
                    if (aLine1.ElementAt(index4).Trim().ToUpper(this.ciUsed) == "PLANTABLE SPACE")
                      index3 = index4;
                    if (aLine1.ElementAt(index4).Trim().ToUpper(this.ciUsed) == "TREE")
                      index1 = index4;
                    if (aLine1.ElementAt(index4).Trim().ToUpper(this.ciUsed) == "SHRUB")
                      index2 = index4;
                  }
                }
                if (this.NumbersInLine(aLine1) >= 4)
                {
                  double num = this.dictStrataNameToCoverPercentRatio[aLine1.ElementAt(1).Trim()];
                  IQuery query1 = insertEstimateTable;
                  Classifiers classifiers = Classifiers.Strata;
                  string name1 = classifiers.ToString();
                  int val5 = this.dictStrataNameToClassValue[aLine1.ElementAt(1).Trim()];
                  query1.SetInt32(name1, val5);
                  if (index3 != -1)
                  {
                    IQuery query2 = insertEstimateTable;
                    classifiers = Classifiers.GroundCover;
                    string name2 = classifiers.ToString();
                    int spaceOfGroundCover = this.ClassValueForPlantableSpaceOfGroundCover;
                    query2.SetInt32(name2, spaceOfGroundCover).SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(index3).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(index3 + 1).Trim())).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  if (index1 != -1)
                  {
                    IQuery query3 = insertEstimateTable;
                    classifiers = Classifiers.GroundCover;
                    string name3 = classifiers.ToString();
                    int treeOfGroundCover = this.ClassValueForTreeOfGroundCover;
                    query3.SetInt32(name3, treeOfGroundCover).SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(index1).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(index1 + 1).Trim())).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  if (index2 != -1)
                  {
                    IQuery query4 = insertEstimateTable;
                    classifiers = Classifiers.GroundCover;
                    string name4 = classifiers.ToString();
                    int shrubOfGroundCover = this.ClassValueForShrubOfGroundCover;
                    query4.SetInt32(name4, shrubOfGroundCover).SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(index2).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(index2 + 1).Trim())).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                }
              }
            }
            streamReader.Close();
          }
        }
        else
        {
          path2 = "PERCOVES.CSV";
          List<int> source = new List<int>();
          using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
          {
            string str;
            while ((str = streamReader.ReadLine()) != null)
            {
              string aLine4 = str.Trim();
              if (!(aLine4 == ""))
              {
                aLine1.SetLine(aLine4);
                if (aLine1.ElementAt(1).Trim() == "" && aLine1.Count >= 3)
                {
                  short val = 0;
                  for (int index = 2; index <= aLine1.Count; ++index)
                  {
                    if (aLine1.ElementAt(index).Trim() != "")
                    {
                      ++val;
                      namedQuery.SetInt16("classValueOrder", val).SetString("classValueName", aLine1.ElementAt(index).Trim());
                      namedQuery.ExecuteUpdate();
                      sasTransaction.IncreaseOperationNumber();
                      source.Add((int) val);
                      if (aLine1.ElementAt(index).Trim().ToUpper(this.ciUsed) == "PLANTABLE SPACE")
                        this.ClassValueForPlantableSpaceOfGroundCover = (int) val;
                      if (aLine1.ElementAt(index).Trim().ToUpper(this.ciUsed) == "TREE")
                        this.ClassValueForTreeOfGroundCover = (int) val;
                      if (aLine1.ElementAt(index).Trim().ToUpper(this.ciUsed) == "SHRUB")
                        this.ClassValueForShrubOfGroundCover = (int) val;
                    }
                  }
                }
                if (this.NumbersInLine(aLine1) >= 4)
                {
                  double num = this.dictStrataNameToCoverPercentRatio[aLine1.ElementAt(1).Trim()];
                  IQuery query5 = insertEstimateTable;
                  Classifiers classifiers = Classifiers.Strata;
                  string name5 = classifiers.ToString();
                  int val6 = this.dictStrataNameToClassValue[aLine1.ElementAt(1).Trim()];
                  query5.SetInt32(name5, val6);
                  for (int index = 0; index < source.Count; ++index)
                  {
                    IQuery query6 = insertEstimateTable;
                    classifiers = Classifiers.GroundCover;
                    string name6 = classifiers.ToString();
                    int val7 = source.ElementAt<int>(index);
                    query6.SetInt32(name6, val7).SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(index * 2 + 2).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(index * 2 + 3).Trim())).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                }
              }
            }
            streamReader.Close();
          }
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        if (path2 == "")
          throw;
        else
          throw new Exception("Processing file " + path2 + ": " + Environment.NewLine + ex.Message);
      }
    }

    private void getNEWENRGY_TRENRGY()
    {
      double valueOrDefault1;
      double valueOrDefault2;
      double valueOrDefault3;
      using (ISession sessLocSpec = this.m_ps.LocSp.OpenSession())
      {
        Location location = sessLocSpec.QueryOver<LocationRelation>().Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Location.Id == this.currProject.LocationId)).Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Code != default (string))).Cacheable().SingleOrDefault().Location;
        LocationSpecificValues locationSpecificValues = new LocationSpecificValues(sessLocSpec, 1.0);
        locationSpecificValues.GetLocationSpecificValues(location);
        LocationEnvironmentalEffect locEnvEffect = locationSpecificValues.locEnvEffect;
        double? nullable = locEnvEffect.CO2MMBtu;
        valueOrDefault1 = nullable.GetValueOrDefault();
        nullable = locEnvEffect.CO2MWh;
        valueOrDefault2 = nullable.GetValueOrDefault();
        nullable = locEnvEffect.TransmissionLoss;
        valueOrDefault3 = nullable.GetValueOrDefault();
      }
      string path2 = "";
      int unit1 = this.getUnit(this.project_s, Units.MillionBritishThermalUnits, Units.None, Units.None);
      int unit2 = this.getUnit(this.project_s, Units.Megawatthours, Units.None, Units.None);
      int unit3 = this.getUnit(this.project_s, Units.MetricTons, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        partitionClassifiers.Clear();
        partitionClassifiers.Add(Classifiers.Strata);
        partitionClassifiers.Add(Classifiers.EnergyUse);
        sasTransaction.Pause();
        string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
        sasTransaction.Resume();
        IQuery insertEstimateTable = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider().GetSQLQueryInsertEstimateTable(estimationTable, partitionClassifiers);
        insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1);
        int num1 = 1;
        int num2 = 2;
        LineValues aLine1 = new LineValues();
        path2 = "NWENRGY.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine2 = str.Trim();
            if (!(aLine2 == ""))
            {
              aLine1.SetLine(aLine2);
              if (this.NumbersInLine(aLine1) >= 4)
              {
                IQuery query1 = insertEstimateTable;
                Classifiers classifiers = Classifiers.Strata;
                string name1 = classifiers.ToString();
                int val1 = this.dictStrataNameToClassValue[aLine1.ElementAt(1).Trim()];
                IQuery query2 = query1.SetInt32(name1, val1);
                classifiers = Classifiers.EnergyUse;
                string name2 = classifiers.ToString();
                int val2 = num1;
                query2.SetInt32(name2, val2).SetInt32("EstimateType", 9).SetDouble("EstimateValue", 0.0).SetDouble("EstimateStandardError", 0.0).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                double val3 = this.TreatDotBlank(aLine1.ElementAt(14).Trim()) + this.TreatDotBlank(aLine1.ElementAt(16).Trim());
                insertEstimateTable.SetInt32("EstimateType", 15).SetDouble("EstimateValue", val3).SetDouble("EstimateStandardError", 0.0).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                double val4 = this.TreatDotBlank(aLine1.ElementAt(2).Trim()) + this.TreatDotBlank(aLine1.ElementAt(4).Trim()) + this.TreatDotBlank(aLine1.ElementAt(6).Trim());
                IQuery query3 = insertEstimateTable;
                classifiers = Classifiers.EnergyUse;
                string name3 = classifiers.ToString();
                int val5 = num2;
                query3.SetInt32(name3, val5).SetInt32("EstimateType", 9).SetDouble("EstimateValue", val4).SetDouble("EstimateStandardError", 0.0).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                double val6 = this.TreatDotBlank(aLine1.ElementAt(8).Trim()) + this.TreatDotBlank(aLine1.ElementAt(10).Trim()) + this.TreatDotBlank(aLine1.ElementAt(12).Trim());
                insertEstimateTable.SetInt32("EstimateType", 15).SetDouble("EstimateValue", val6).SetDouble("EstimateStandardError", 0.0).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                IQuery query4 = insertEstimateTable;
                classifiers = Classifiers.EnergyUse;
                string name4 = classifiers.ToString();
                int val7 = num1;
                query4.SetInt32(name4, val7).SetInt32("EstimateType", 10).SetDouble("EstimateValue", val3 * valueOrDefault2 / 2204.62 / (11.0 / 3.0) / (1.0 - valueOrDefault3 / 100.0)).SetDouble("EstimateStandardError", 0.0).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                IQuery query5 = insertEstimateTable;
                classifiers = Classifiers.EnergyUse;
                string name5 = classifiers.ToString();
                int val8 = num2;
                query5.SetInt32(name5, val8).SetDouble("EstimateValue", val6 * valueOrDefault2 / 2204.62 / (11.0 / 3.0) / (1.0 - valueOrDefault3 / 100.0) + val4 * valueOrDefault1 / 2204.62 / (11.0 / 3.0)).SetDouble("EstimateStandardError", 0.0).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception("Processing file " + path2 + ": " + Environment.NewLine + ex.Message);
      }
    }

    private void getLEAFCLAS()
    {
      string path2 = "";
      int unit1 = this.getUnit(this.project_s, Units.Squarekilometer, Units.None, Units.None);
      int unit2 = this.getUnit(this.project_s, Units.Squaremeter, Units.None, Units.None);
      int unit3 = this.getUnit(this.project_s, Units.MetricTons, Units.None, Units.None);
      int unit4 = this.getUnit(this.project_s, Units.Kilograms, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "LEAFCLAS.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.DBH);
          sasTransaction.Pause();
          string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          if (!this.SASResultContainTree)
          {
            streamReader.Close();
            sasTransaction.End();
            return;
          }
          SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
          IQuery insertEstimateTable1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers);
          insertEstimateTable1.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1);
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Species);
          partitionClassifiers.Add(Classifiers.DBH);
          sasTransaction.Pause();
          string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers);
          insertEstimateTable2.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1);
          LineValues aLine1 = new LineValues();
          List<int> source = new List<int>();
          string key1 = "";
          string key2 = "";
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine2 = str.Trim();
            if (!(aLine2 == ""))
            {
              aLine1.SetLine(aLine2);
              int num1 = this.NumbersInLine(aLine1);
              if (aLine1.ElementAt(1).Trim().Length != 0)
                key1 = aLine1.ElementAt(1).Trim();
              if (aLine1.Count >= 2)
              {
                if (aLine1.ElementAt(2).Trim().Length != 0)
                  key2 = aLine1.ElementAt(2).Trim();
                if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  key2 = "Total";
              }
              if (aLine1.Count >= 3 && aLine1.ElementAt(1).Trim().Length == 0 && aLine1.ElementAt(2).Trim().Length == 0 && num1 >= 1)
              {
                LineValues lineValues1 = new LineValues();
                LineValues lineValues2 = new LineValues();
                lineValues1.SetLine(aLine2);
                streamReader.ReadLine();
                lineValues2.SetLine(streamReader.ReadLine());
                source.Clear();
                for (int index = 1; index <= lineValues1.Count; ++index)
                {
                  if (lineValues1.ElementAt(index).Trim().Length != 0)
                  {
                    double num2 = double.Parse(lineValues1.ElementAt(index), this.nsFloat, (IFormatProvider) this.ciUsed);
                    double num3 = double.Parse(lineValues2.ElementAt(index), this.nsFloat, (IFormatProvider) this.ciUsed);
                    if (this.dictDBHtoClassValue.ContainsKey(num2.ToString("#0.0", (IFormatProvider) this.ciUsed) + " - " + num3.ToString("#0.0", (IFormatProvider) this.ciUsed)))
                      source.Add(this.dictDBHtoClassValue[num2.ToString("#0.0", (IFormatProvider) this.ciUsed) + " - " + num3.ToString("#0.0", (IFormatProvider) this.ciUsed)]);
                    else
                      break;
                  }
                }
              }
              if (aLine1.Count >= 4 && !(key2.ToUpper(this.ciUsed).Trim() == "NONE.") && !(key2.ToUpper(this.ciUsed).Trim() == "NONE") && (aLine1.ElementAt(1).Trim() == "CITY TOTAL" || aLine1.ElementAt(1).Trim().Length == 0 && aLine1.ElementAt(2).Trim().Length != 0 && num1 >= 2))
              {
                insertEstimateTable2.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]);
                insertEstimateTable2.SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[key2]);
                for (int index = 0; index < source.Count; ++index)
                {
                  insertEstimateTable2.SetInt32(Classifiers.DBH.ToString(), source.ElementAt<int>(index)).SetInt32("EstimateType", 5).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 3).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 4).Trim())).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 3).Trim()) * this.million).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 4).Trim()) * this.million).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetInt32("EstimateType", 6).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 5).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 6).Trim())).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 5).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 6).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                }
                if (key2.ToLower(this.ciUsed).Trim() == "total")
                {
                  insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]);
                  for (int index = 0; index < source.Count; ++index)
                  {
                    insertEstimateTable1.SetInt32(Classifiers.DBH.ToString(), source.ElementAt<int>(index)).SetInt32("EstimateType", 5).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 3).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 4).Trim())).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 3).Trim()) * this.million).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 4).Trim()) * this.million).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetInt32("EstimateType", 6).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 5).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 6).Trim())).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 5).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 4 + 6).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception("Processing file " + path2 + ": " + Environment.NewLine + ex.Message);
      }
    }

    private void getLANDUSEM()
    {
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "LANDUSEM.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Landuse);
          sasTransaction.Pause();
          string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Landuse, partitionClassifiers);
          sasTransaction.Resume();
          SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
          IQuery insertEstimateTable1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers);
          insertEstimateTable1.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 12).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Landuse);
          partitionClassifiers.Add(Classifiers.Strata);
          sasTransaction.Pause();
          string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers);
          insertEstimateTable2.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 11).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          LineValues lineValues = new LineValues();
          List<int> source = new List<int>();
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine = str.Trim();
            if (!(aLine == ""))
            {
              lineValues.SetLine(aLine);
              if (lineValues.ElementAt(1).Trim() == "" && lineValues.Count >= 2)
              {
                for (int index = 1; index <= lineValues.Count; ++index)
                {
                  if (lineValues.ElementAt(index).Trim() != "")
                    source.Add(this.dictStrataNameToClassValue[lineValues.ElementAt(index).Trim()]);
                }
              }
              if (lineValues.ElementAt(1).Trim() != "" && lineValues.Count >= 2)
              {
                for (int index = 0; index < source.Count; ++index)
                {
                  insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), source.ElementAt<int>(index)).SetInt32(Classifiers.Landuse.ToString(), this.dictFieldLanduseToClassValue[lineValues.ElementAt(1).Trim()]).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(index + 2).Trim())).SetDouble("EstimateStandardError", 0.0).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetInt32(Classifiers.Landuse.ToString(), this.dictFieldLanduseToClassValue[lineValues.ElementAt(1).Trim()]).SetInt32(Classifiers.Strata.ToString(), source.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(index + 2).Trim())).SetDouble("EstimateStandardError", 0.0).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception("Processing file " + path2 + ": " + Environment.NewLine + ex.Message);
      }
    }

    private void getEXOTICS()
    {
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "EXOTICS.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Continent);
          sasTransaction.Pause();
          string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider().GetSQLQueryInsertEstimateTable(estimationTable, partitionClassifiers);
          insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          LineValues aLine1 = new LineValues();
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine2 = str.Trim();
            if (!(aLine2 == ""))
            {
              aLine1.SetLine(aLine2);
              if (this.NumbersInLine(aLine1) >= 3)
              {
                string key = aLine1.ElementAt(1).Trim();
                insertEstimateTable.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key]);
                for (int index = 0; index < this.listContinentClassValue.Count; ++index)
                {
                  insertEstimateTable.SetInt32(Classifiers.Continent.ToString(), this.listContinentClassValue.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index + 2).Trim())).SetDouble("EstimateStandardError", 0.0).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private void getDIVERSIT()
    {
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        this.getDensityCoverPercentRatio();
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "DIVERSIT.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.PrimaryIndex);
          sasTransaction.Pause();
          string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider().GetSQLQueryInsertEstimateTable(estimationTable, partitionClassifiers);
          insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 13).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          LineValues aLine1 = new LineValues();
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine2 = str.Trim();
            if (!(aLine2 == ""))
            {
              aLine1.SetLine(aLine2);
              if (this.NumbersInLine(aLine1) >= 3)
              {
                string key = aLine1.ElementAt(1).Trim();
                insertEstimateTable.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key]);
                double num = this.dictStrataNameToDensityRatio[key];
                for (int index = 0; index < this.listPrimaryIndexClassValue.Count; ++index)
                {
                  insertEstimateTable.SetInt32(Classifiers.PrimaryIndex.ToString(), this.listPrimaryIndexClassValue.ElementAt<int>(index));
                  if (index == 1)
                    insertEstimateTable.SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(index + 6).Trim())).SetDouble("EstimateStandardError", 0.0).ExecuteUpdate();
                  else
                    insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index + 6).Trim())).SetDouble("EstimateStandardError", 0.0).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private void getDBHCNDCL()
    {
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "DBHCNDCL.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.DBH);
          partitionClassifiers.Add(Classifiers.Dieback);
          sasTransaction.Pause();
          string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
          IQuery insertEstimateTable1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers);
          insertEstimateTable1.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.DBH);
          partitionClassifiers.Add(Classifiers.Dieback);
          sasTransaction.Pause();
          string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers);
          insertEstimateTable2.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Species);
          partitionClassifiers.Add(Classifiers.DBH);
          partitionClassifiers.Add(Classifiers.Dieback);
          sasTransaction.Pause();
          string estimationTable3 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable3 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable3, partitionClassifiers);
          insertEstimateTable3.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          if (!this.SASResultContainTree)
          {
            streamReader.Close();
            sasTransaction.End();
            return;
          }
          string key1 = "";
          string key2 = "";
          LineValues aLine1 = new LineValues();
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine2 = str.Trim();
            if (!(aLine2 == ""))
            {
              aLine1.SetLine(aLine2);
              if (aLine1.Count == 1)
                key1 = aLine1.ElementAt(1).Trim();
              if (aLine1.ElementAt(1).ToUpper(this.ciUsed).Trim() == "CITY TOTAL")
                key1 = "CITY TOTAL";
              if (this.NumbersInLine(aLine1) >= 3)
              {
                if (aLine1.ElementAt(2).Trim() != "")
                  key2 = aLine1.ElementAt(2).Trim();
                if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  key2 = "Total";
                string key3 = this.normalizeDBHRange(aLine1.ElementAt(3).Trim());
                if (key2.ToUpper(this.ciUsed).Trim() != "NONE." && key2.ToUpper(this.ciUsed).Trim() != "NONE")
                {
                  insertEstimateTable3.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]);
                  insertEstimateTable3.SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[key2]);
                  if (this.dictDBHtoClassValue.ContainsKey(key3))
                  {
                    insertEstimateTable3.SetInt32(Classifiers.DBH.ToString(), this.dictDBHtoClassValue[key3]);
                    for (int index = 0; index < this.listDiebackClassValue.Count; ++index)
                    {
                      insertEstimateTable3.SetInt32(Classifiers.Dieback.ToString(), this.listDiebackClassValue.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 4).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 5).Trim())).ExecuteUpdate();
                      sasTransaction.IncreaseOperationNumber();
                    }
                    if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                    {
                      insertEstimateTable2.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]);
                      insertEstimateTable2.SetInt32(Classifiers.DBH.ToString(), this.dictDBHtoClassValue[key3]);
                      for (int index = 0; index < this.listDiebackClassValue.Count; ++index)
                      {
                        insertEstimateTable2.SetInt32(Classifiers.Dieback.ToString(), this.listDiebackClassValue.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 4).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 5).Trim())).ExecuteUpdate();
                        sasTransaction.IncreaseOperationNumber();
                      }
                    }
                    if (key1 == "CITY TOTAL" && key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                    {
                      insertEstimateTable1.SetInt32(Classifiers.DBH.ToString(), this.dictDBHtoClassValue[key3]);
                      for (int index = 0; index < this.listDiebackClassValue.Count; ++index)
                      {
                        insertEstimateTable1.SetInt32(Classifiers.Dieback.ToString(), this.listDiebackClassValue.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 4).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 5).Trim())).ExecuteUpdate();
                        sasTransaction.IncreaseOperationNumber();
                      }
                    }
                  }
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private void getDBHCLASS()
    {
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "DBHCLASS.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.DBH);
          sasTransaction.Pause();
          string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
          IQuery insertEstimateTable1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers);
          insertEstimateTable1.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Species);
          partitionClassifiers.Add(Classifiers.DBH);
          sasTransaction.Pause();
          string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers);
          insertEstimateTable2.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          if (!this.SASResultContainTree)
          {
            streamReader.Close();
            sasTransaction.End();
            return;
          }
          string key1 = "";
          LineValues aLine1 = new LineValues();
          List<int> source = new List<int>();
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine2 = str.Trim();
            if (!(aLine2 == ""))
            {
              aLine1.SetLine(aLine2);
              if (this.isFirstDBHClassLine(aLine1))
              {
                LineValues lineValues1 = new LineValues();
                lineValues1.SetLine(aLine2);
                LineValues lineValues2 = new LineValues();
                lineValues2.SetLine(streamReader.ReadLine());
                LineValues lineValues3 = new LineValues();
                lineValues3.separater = ' ';
                source.Clear();
                for (int index = 1; index <= lineValues2.Count; ++index)
                {
                  if (lineValues2.ElementAt(index).Trim() != "")
                  {
                    lineValues3.SetLine(lineValues1.ElementAt(index).Trim());
                    source.Add(this.dictDBHtoClassValue[lineValues3.ElementAt(1).Trim() + " - " + lineValues2.ElementAt(index).Trim()]);
                  }
                }
              }
              else
              {
                if (aLine1.Count == 1)
                  key1 = aLine1.ElementAt(1).Trim();
                if (aLine1.ElementAt(1).ToUpper(this.ciUsed).Trim() == "CITY TOTAL")
                  key1 = "CITY TOTAL";
                if (this.NumbersInLine(aLine1) >= 3)
                {
                  string key2 = !(aLine1.ElementAt(2).Trim() != "") ? "Total" : aLine1.ElementAt(2).Trim();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                    key2 = "Total";
                  if (key2.ToUpper(this.ciUsed).Trim() != "NONE." && key2.ToUpper(this.ciUsed).Trim() != "NONE")
                  {
                    insertEstimateTable2.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]);
                    insertEstimateTable2.SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[key2]);
                    for (int index = 0; index < source.Count; ++index)
                    {
                      insertEstimateTable2.SetInt32(Classifiers.DBH.ToString(), source.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 3).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 4).Trim())).ExecuteUpdate();
                      sasTransaction.IncreaseOperationNumber();
                    }
                    if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                    {
                      insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]);
                      for (int index = 0; index < source.Count; ++index)
                      {
                        insertEstimateTable1.SetInt32(Classifiers.DBH.ToString(), source.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 3).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 4).Trim())).ExecuteUpdate();
                        sasTransaction.IncreaseOperationNumber();
                      }
                    }
                  }
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private void getDBHCLASS2()
    {
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "DBHCLASS2.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.CDBH);
          sasTransaction.Pause();
          string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
          IQuery insertEstimateTable1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers);
          insertEstimateTable1.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Species);
          partitionClassifiers.Add(Classifiers.CDBH);
          sasTransaction.Pause();
          string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers);
          insertEstimateTable2.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          string str1 = "";
          string key1 = "";
          LineValues aLine1 = new LineValues();
          aLine1.separater = ',';
          List<int> source = new List<int>();
          string str2;
          while ((str2 = streamReader.ReadLine()) != null)
          {
            string aLine2 = str2.Trim();
            if (!string.IsNullOrWhiteSpace(aLine2))
            {
              aLine1.SetLine(aLine2);
              for (int index = 1; index <= aLine1.Count; ++index)
              {
                if (!string.IsNullOrWhiteSpace(aLine1.ElementAt(index)))
                {
                  if (this.dictCDBHidToClassValue.ContainsKey(aLine1.ElementAt(index).Trim()))
                    source.Add(this.dictCDBHidToClassValue[aLine1.ElementAt(index).Trim()]);
                  else
                    source.Add(-1);
                }
              }
              break;
            }
          }
          str1 = streamReader.ReadLine();
          string str3;
          while ((str3 = streamReader.ReadLine()) != null)
          {
            string aLine3 = str3.Trim();
            if (!(aLine3 == ""))
            {
              aLine1.SetLine(aLine3);
              if (aLine1.Count == 1)
                key1 = aLine1.ElementAt(1).Trim();
              if (aLine1.ElementAt(1).ToUpper(this.ciUsed).Trim() == "CITY TOTAL")
                key1 = "CITY TOTAL";
              if (this.NumbersInLine(aLine1) >= 3)
              {
                string key2 = string.IsNullOrWhiteSpace(aLine1.ElementAt(2).Trim()) ? "Total" : aLine1.ElementAt(2).Trim();
                if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  key2 = "Total";
                if (key2.ToUpper(this.ciUsed).Trim() != "NONE." && key2.ToUpper(this.ciUsed).Trim() != "NONE")
                {
                  insertEstimateTable2.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]);
                  insertEstimateTable2.SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[key2]);
                  for (int index = 0; index < source.Count; ++index)
                  {
                    if (source.ElementAt<int>(index) != -1)
                    {
                      insertEstimateTable2.SetInt32(Classifiers.CDBH.ToString(), source.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 3).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 4).Trim())).ExecuteUpdate();
                      sasTransaction.IncreaseOperationNumber();
                    }
                  }
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]);
                    for (int index = 0; index < source.Count; ++index)
                    {
                      if (source.ElementAt<int>(index) != -1)
                      {
                        insertEstimateTable1.SetInt32(Classifiers.CDBH.ToString(), source.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 3).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 4).Trim())).ExecuteUpdate();
                        sasTransaction.IncreaseOperationNumber();
                      }
                    }
                  }
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private void getSingleTreeEnergyCalculated(
      IProgress<SASProgressArg> progress,
      CancellationToken ct)
    {
      if (!this.currYear.RecordEnergy)
        return;
      int unit1 = this.getUnit(this.project_s, Units.MetricTons, Units.None, Units.None);
      int unit2 = this.getUnit(this.project_s, Units.MillionBritishThermalUnits, Units.None, Units.None);
      int unit3 = this.getUnit(this.project_s, Units.Megawatthours, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        using (ISession sessLocSpec = this.m_ps.LocSp.OpenSession())
        {
          Location location1 = sessLocSpec.Load<Location>((object) this.currProject.LocationId);
          Location location2 = sessLocSpec.CreateCriteria<LocationRelation>().Add((ICriterion) Restrictions.Eq("Location", (object) location1)).Add((ICriterion) Restrictions.IsNotNull("Code")).SetCacheable(true).UniqueResult<LocationRelation>().Location;
          IndividualTree individualTree = new IndividualTree(sessLocSpec, location2, 1.0, this.currYear.Unit == YearUnit.Metric, this.currYear.RecordHeight);
          ICriteria criteria = this.project_s.CreateCriteria<Tree>().CreateAlias("Plot", "p").Add((ICriterion) Restrictions.Eq("p.Year", (object) this.currYear));
          if (this.currSeries.SampleType == SampleType.RegularPlot)
            criteria.Add((ICriterion) Restrictions.Eq("p.IsComplete", (object) true));
          if (this.currYear.RecordTreeStatus)
            criteria.Add(Restrictions.Where<Tree>((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => !t.Status.IsIn(SASProcessor.TreeRemoveStatus))));
          IList<Tree> treeList = (IList<Tree>) criteria.Fetch("Plot").PagedList<Tree>(1000);
          SASDictionary<Strata, TreeBenefit> sasDictionary1 = new SASDictionary<Strata, TreeBenefit>("Strata");
          IList<Strata> strataList = this.project_s.QueryOver<Strata>().List();
          List<TreeBenefit> treeBenefitList = new List<TreeBenefit>();
          foreach (Strata key in (IEnumerable<Strata>) strataList)
            sasDictionary1.Add(key, new TreeBenefit());
          SASDictionary<Plot, int> sasDictionary2 = new SASDictionary<Plot, int>("Plot");
          if (this.currYear.RecordGroundCover)
          {
            List<GroundCover> list = this.currYear.GroundCovers.Where<GroundCover>((Func<GroundCover, bool>) (g => g.CoverTypeId == 1)).ToList<GroundCover>();
            foreach (PlotGroundCover plotGroundCover in (IEnumerable<PlotGroundCover>) this.project_s.CreateCriteria<PlotGroundCover>().Add((ICriterion) Restrictions.In("GroundCover", (ICollection) list)).List<PlotGroundCover>())
            {
              if (sasDictionary2.ContainsKey(plotGroundCover.Plot))
                sasDictionary2[plotGroundCover.Plot] += plotGroundCover.PercentCovered;
              else
                sasDictionary2.Add(plotGroundCover.Plot, plotGroundCover.PercentCovered);
            }
          }
          SASDictionary<string, Species> sasDictionary3 = new SASDictionary<string, Species>("Species");
          int num1 = 0;
          int count = treeList.Count;
          foreach (Tree tree1 in (IEnumerable<Tree>) treeList)
          {
            ct.ThrowIfCancellationRequested();
            ++num1;
            progress.Report(new SASProgressArg()
            {
              Description = "Calculating individual tree energy ...",
              Percent = 100 * num1 / count
            });
            if (tree1.Buildings.Count > 0)
            {
              Tuple.Create<int, int>(tree1.Plot.Id, tree1.Id);
              Species species;
              if (!sasDictionary3.ContainsKey(tree1.Species))
              {
                species = sessLocSpec.CreateCriteria<Species>().Add((ICriterion) Restrictions.Eq("Code", (object) tree1.Species)).SetCacheable(true).UniqueResult<Species>();
                sasDictionary3.Add(species.Code, species);
              }
              else
                species = sasDictionary3[tree1.Species];
              EnergyTreeView tree2 = new EnergyTreeView(tree1, species);
              foreach (Building building1 in (IEnumerable<Building>) tree1.Buildings)
              {
                EnergyBuildingView building2 = new EnergyBuildingView(building1);
                double? percentTreeBuildingCover = new double?();
                if (this.currYear.RecordGroundCover && this.currSeries.IsSample)
                {
                  int num2 = 0;
                  if (sasDictionary2.ContainsKey(tree1.Plot))
                    num2 = sasDictionary2[tree1.Plot];
                  percentTreeBuildingCover = new double?((double) (tree1.Plot.PercentTreeCover + num2));
                  double? nullable = percentTreeBuildingCover;
                  double num3 = 100.0;
                  percentTreeBuildingCover = nullable.GetValueOrDefault() > num3 & nullable.HasValue ? new double?(100.0) : percentTreeBuildingCover;
                }
                TreeBenefit energyBenefits = individualTree.CalculateEnergyBenefits((EnergyBuilding) building2, (EnergyTree) tree2, percentTreeBuildingCover);
                if (energyBenefits != null)
                  sasDictionary1[tree1.Plot.Strata] += energyBenefits;
              }
            }
          }
          if (this.currSeries.SampleType == SampleType.Inventory)
          {
            SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
            List<Classifiers> partitionClassifiers = new List<Classifiers>();
            partitionClassifiers.Add(Classifiers.Strata);
            partitionClassifiers.Add(Classifiers.EnergyUse);
            sasTransaction.Pause();
            string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
            sasUtilProvider.GetSQLQueryClearContentOfProject(estimationTable).SetGuid("yearGuid", this.currYear.Guid).ExecuteUpdate();
            sasTransaction.Resume();
            IQuery insertEstimateTable = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable, partitionClassifiers);
            insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1).SetDouble("EstimateStandardError", 0.0);
            TreeBenefit treeBenefit = new TreeBenefit();
            foreach (KeyValuePair<Strata, TreeBenefit> keyValuePair in (Dictionary<Strata, TreeBenefit>) sasDictionary1)
            {
              if (this.dictStrataNameToClassValue.ContainsKey(keyValuePair.Key.Description.Trim()))
              {
                treeBenefit += keyValuePair.Value;
                insertEstimateTable.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[keyValuePair.Key.Description.Trim()]);
                insertEstimateTable.SetInt32(Classifiers.EnergyUse.ToString(), 2).SetInt32("EstimateType", 9).SetDouble("EstimateValue", keyValuePair.Value.MbtuTotal).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 15).SetDouble("EstimateValue", (keyValuePair.Value.KWhElectricityHeatClimate + keyValuePair.Value.KwhElectricityHeatShade + keyValuePair.Value.KWhElectricityHeatWind) / this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 10).SetDouble("EstimateValue", (keyValuePair.Value.CarbonDioxideLbsElectricHeatClimate + keyValuePair.Value.CarbonDioxideLbsElectricHeatShade + keyValuePair.Value.CarbonDioxideLbsElectricHeatWind + keyValuePair.Value.CarbonDioxideLbsMbtuHeatClimate + keyValuePair.Value.CarbonDioxideLbsMbtuHeatShade + keyValuePair.Value.CarbonDioxideLbsMbtuHeatWind) / 2204.62 / (11.0 / 3.0)).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32(Classifiers.EnergyUse.ToString(), 1).SetInt32("EstimateType", 15).SetDouble("EstimateValue", (keyValuePair.Value.KWhElectricityCoolClimate + keyValuePair.Value.KWhElectricityCoolShade) / this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 10).SetDouble("EstimateValue", (keyValuePair.Value.CarbonDioxideLbsElectricCoolClimate + keyValuePair.Value.CarbonDioxideLbsElectricCoolShade) / 2204.62 / (11.0 / 3.0)).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
              }
            }
            IQuery query1 = insertEstimateTable;
            Classifiers classifiers = Classifiers.Strata;
            string name1 = classifiers.ToString();
            int forStrataStudyArea = this.ClassValueForStrataStudyArea;
            query1.SetInt32(name1, forStrataStudyArea);
            IQuery query2 = insertEstimateTable;
            classifiers = Classifiers.EnergyUse;
            string name2 = classifiers.ToString();
            query2.SetInt32(name2, 2).SetInt32("EstimateType", 9).SetDouble("EstimateValue", treeBenefit.MbtuTotal).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            insertEstimateTable.SetInt32("EstimateType", 15).SetDouble("EstimateValue", (treeBenefit.KWhElectricityHeatClimate + treeBenefit.KwhElectricityHeatShade + treeBenefit.KWhElectricityHeatWind) / this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            insertEstimateTable.SetInt32("EstimateType", 10).SetDouble("EstimateValue", (treeBenefit.CarbonDioxideLbsElectricHeatClimate + treeBenefit.CarbonDioxideLbsElectricHeatShade + treeBenefit.CarbonDioxideLbsElectricHeatWind + treeBenefit.CarbonDioxideLbsMbtuHeatClimate + treeBenefit.CarbonDioxideLbsMbtuHeatShade + treeBenefit.CarbonDioxideLbsMbtuHeatWind) / 2204.62 / (11.0 / 3.0)).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            IQuery query3 = insertEstimateTable;
            classifiers = Classifiers.EnergyUse;
            string name3 = classifiers.ToString();
            query3.SetInt32(name3, 1).SetInt32("EstimateType", 15).SetDouble("EstimateValue", (treeBenefit.KWhElectricityCoolClimate + treeBenefit.KWhElectricityCoolShade) / this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            insertEstimateTable.SetInt32("EstimateType", 10).SetDouble("EstimateValue", (treeBenefit.CarbonDioxideLbsElectricCoolClimate + treeBenefit.CarbonDioxideLbsElectricCoolShade) / 2204.62 / (11.0 / 3.0)).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
          }
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw;
      }
    }

    private void getCONDCLAS()
    {
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "CONDCLAS.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Dieback);
          sasTransaction.Pause();
          string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
          IQuery insertEstimateTable1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers);
          insertEstimateTable1.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Species);
          partitionClassifiers.Add(Classifiers.Dieback);
          sasTransaction.Pause();
          string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers);
          insertEstimateTable2.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          string key1 = "";
          LineValues aLine1 = new LineValues();
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine2 = str.Trim();
            if (!(aLine2 == ""))
            {
              aLine1.SetLine(aLine2);
              if (aLine1.Count == 1)
                key1 = aLine1.ElementAt(1).Trim();
              if (aLine1.ElementAt(1).ToUpper(this.ciUsed).Trim() == "CITY TOTAL")
                key1 = "CITY TOTAL";
              if (this.NumbersInLine(aLine1) >= 3)
              {
                string key2 = !(aLine1.ElementAt(2).Trim() != "") ? "Total" : aLine1.ElementAt(2).Trim();
                if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  key2 = "Total";
                if (key2.ToUpper(this.ciUsed).Trim() != "NONE." && key2.ToUpper(this.ciUsed).Trim() != "NONE")
                {
                  insertEstimateTable2.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]);
                  insertEstimateTable2.SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[key2]);
                  for (int index = 0; index < this.listDiebackClassValue.Count; ++index)
                  {
                    insertEstimateTable2.SetInt32(Classifiers.Dieback.ToString(), this.listDiebackClassValue.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 3).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 4).Trim())).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]);
                    for (int index = 0; index < this.listDiebackClassValue.Count; ++index)
                    {
                      insertEstimateTable1.SetInt32(Classifiers.Dieback.ToString(), this.listDiebackClassValue.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 3).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 4).Trim())).ExecuteUpdate();
                      sasTransaction.IncreaseOperationNumber();
                    }
                  }
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private void getCONDCLAS2()
    {
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "CONDCLAS2.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.CDieback);
          sasTransaction.Pause();
          string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
          IQuery insertEstimateTable1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers);
          insertEstimateTable1.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Species);
          partitionClassifiers.Add(Classifiers.CDieback);
          sasTransaction.Pause();
          string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers);
          insertEstimateTable2.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 1).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit);
          if (!this.SASResultContainTree)
          {
            streamReader.Close();
            sasTransaction.End();
            return;
          }
          string str1 = "";
          string key1 = "";
          LineValues aLine1 = new LineValues();
          aLine1.separater = ',';
          List<int> source = new List<int>();
          string str2;
          while ((str2 = streamReader.ReadLine()) != null)
          {
            string aLine2 = str2.Trim();
            if (!string.IsNullOrWhiteSpace(aLine2))
            {
              aLine1.SetLine(aLine2);
              for (int index = 1; index <= aLine1.Count; ++index)
              {
                if (!string.IsNullOrWhiteSpace(aLine1.ElementAt(index).Trim()))
                {
                  if (this.dictCDiebackIdToClassValue.ContainsKey(aLine1.ElementAt(index).Trim()))
                    source.Add(this.dictCDiebackIdToClassValue[aLine1.ElementAt(index).Trim()]);
                  else
                    source.Add(-1);
                }
              }
              break;
            }
          }
          str1 = streamReader.ReadLine();
          string str3;
          while ((str3 = streamReader.ReadLine()) != null)
          {
            string aLine3 = str3.Trim();
            if (!string.IsNullOrWhiteSpace(aLine3))
            {
              aLine1.SetLine(aLine3);
              if (aLine1.Count == 1)
                key1 = aLine1.ElementAt(1).Trim();
              if (aLine1.ElementAt(1).ToUpper(this.ciUsed).Trim() == "CITY TOTAL")
                key1 = "CITY TOTAL";
              if (this.NumbersInLine(aLine1) >= 3)
              {
                string key2 = !(aLine1.ElementAt(2).Trim() != "") ? "Total" : aLine1.ElementAt(2).Trim();
                if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  key2 = "Total";
                if (key2.ToUpper(this.ciUsed).Trim() != "NONE." && key2.ToUpper(this.ciUsed).Trim() != "NONE")
                {
                  insertEstimateTable2.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]);
                  insertEstimateTable2.SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[key2]);
                  for (int index = 0; index < source.Count; ++index)
                  {
                    if (source.ElementAt<int>(index) != -1)
                    {
                      insertEstimateTable2.SetInt32(Classifiers.CDieback.ToString(), source.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 3).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 4).Trim())).ExecuteUpdate();
                      sasTransaction.IncreaseOperationNumber();
                    }
                  }
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]);
                    for (int index = 0; index < source.Count; ++index)
                    {
                      if (source.ElementAt<int>(index) != -1)
                      {
                        insertEstimateTable1.SetInt32(Classifiers.CDieback.ToString(), source.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 3).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 4).Trim())).ExecuteUpdate();
                        sasTransaction.IncreaseOperationNumber();
                      }
                    }
                  }
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private void getCITYSUM()
    {
      string path2 = "";
      int unit1 = this.getUnit(this.project_s, Units.Count, Units.None, Units.None);
      int unit2 = this.getUnit(this.project_s, Units.MetricTons, Units.None, Units.None);
      int unit3 = this.getUnit(this.project_s, Units.Kilograms, Units.None, Units.None);
      int unit4 = this.getUnit(this.project_s, Units.MetricTons, Units.Year, Units.None);
      int unit5 = this.getUnit(this.project_s, Units.Kilograms, Units.Year, Units.None);
      int unit6 = this.getUnit(this.project_s, Units.Squarekilometer, Units.None, Units.None);
      int unit7 = this.getUnit(this.project_s, Units.Squaremeter, Units.None, Units.None);
      int unit8 = this.getUnit(this.project_s, Units.Monetaryunit, Units.None, Units.None);
      int unit9 = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "CITYSUM.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Species);
          sasTransaction.Pause();
          string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider().GetSQLQueryInsertEstimateTable(estimationTable, partitionClassifiers);
          insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1);
          LineValues aLine1 = new LineValues();
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine2 = str.Trim();
            if (!(aLine2 == ""))
            {
              aLine1.SetLine(aLine2);
              if (this.NumbersInLine(aLine1) > 3 && !(aLine1.ElementAt(1).Trim().ToUpper(this.ciUsed).Trim() == "NONE.") && !(aLine1.ElementAt(1).Trim().ToUpper(this.ciUsed).Trim() == "NONE"))
              {
                insertEstimateTable.SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[aLine1.ElementAt(1).Trim()]);
                if (this.dictSpeciesCodeToClassValue[aLine1.ElementAt(1).Trim()] == this.ClassValueForSpeciesAllSpecies)
                  this.HasSpeciesTotalInCITYSUM_CSV = true;
                insertEstimateTable.SetInt32("EstimateType", 1).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(3).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(4).Trim())).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(2).Trim()));
                if (this.TreatDotBlank(aLine1.ElementAt(3).Trim()) == 0.0)
                  insertEstimateTable.SetDouble("EstimateStandardError", 0.0);
                else
                  insertEstimateTable.SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(2).Trim()) * this.TreatDotBlank(aLine1.ElementAt(4).Trim()) / this.TreatDotBlank(aLine1.ElementAt(3).Trim()));
                insertEstimateTable.SetInt32("EstimateUnitsId", unit9).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 2).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(6).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(7).Trim())).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(6).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(7).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(5).Trim()));
                if (this.TreatDotBlank(aLine1.ElementAt(6).Trim()) == 0.0)
                  insertEstimateTable.SetDouble("EstimateStandardError", 0.0);
                else
                  insertEstimateTable.SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(5).Trim()) * this.TreatDotBlank(aLine1.ElementAt(7).Trim()) / this.TreatDotBlank(aLine1.ElementAt(6).Trim()));
                insertEstimateTable.SetInt32("EstimateUnitsId", unit9).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 3).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(9).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(10).Trim())).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(9).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(10).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(8).Trim()));
                if (this.TreatDotBlank(aLine1.ElementAt(9).Trim()) == 0.0)
                  insertEstimateTable.SetDouble("EstimateStandardError", 0.0);
                else
                  insertEstimateTable.SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(8).Trim()) * this.TreatDotBlank(aLine1.ElementAt(10).Trim()) / this.TreatDotBlank(aLine1.ElementAt(9).Trim()));
                insertEstimateTable.SetInt32("EstimateUnitsId", unit9).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 4).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(11).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(12).Trim())).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(11).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(12).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 5).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(14).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(15).Trim())).SetInt32("EstimateUnitsId", unit6).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(14).Trim()) * this.million).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(15).Trim()) * this.million).SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(13).Trim()));
                if (this.TreatDotBlank(aLine1.ElementAt(14).Trim()) == 0.0)
                  insertEstimateTable.SetDouble("EstimateStandardError", 0.0);
                else
                  insertEstimateTable.SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(13).Trim()) * this.TreatDotBlank(aLine1.ElementAt(15).Trim()) / this.TreatDotBlank(aLine1.ElementAt(14).Trim()));
                insertEstimateTable.SetInt32("EstimateUnitsId", unit9).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 6).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(17).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(18).Trim())).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(17).Trim()) * this.thousand).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(18).Trim()) * this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(16).Trim()));
                if (this.TreatDotBlank(aLine1.ElementAt(17).Trim()) == 0.0)
                  insertEstimateTable.SetDouble("EstimateStandardError", 0.0);
                else
                  insertEstimateTable.SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(16).Trim()) * this.TreatDotBlank(aLine1.ElementAt(18).Trim()) / this.TreatDotBlank(aLine1.ElementAt(17).Trim()));
                insertEstimateTable.SetInt32("EstimateUnitsId", unit9).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 8).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(20).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(21).Trim())).SetInt32("EstimateUnitsId", unit8).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(19).Trim()));
                if (this.TreatDotBlank(aLine1.ElementAt(20).Trim()) == 0.0)
                  insertEstimateTable.SetDouble("EstimateStandardError", 0.0);
                else
                  insertEstimateTable.SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(19).Trim()) * this.TreatDotBlank(aLine1.ElementAt(21).Trim()) / this.TreatDotBlank(aLine1.ElementAt(20).Trim()));
                insertEstimateTable.SetInt32("EstimateUnitsId", unit9).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private void getCITYDEN()
    {
      string path2 = "";
      int unit1 = this.getUnit(this.project_s, Units.Count, Units.Hectare, Units.None);
      int unit2 = this.getUnit(this.project_s, Units.Kilograms, Units.Hectare, Units.None);
      int unit3 = this.getUnit(this.project_s, Units.Kilograms, Units.Year, Units.Hectare);
      int val = unit3;
      int unit4 = this.getUnit(this.project_s, Units.Squaremeter, Units.Hectare, Units.None);
      int unit5 = this.getUnit(this.project_s, Units.Kilograms, Units.Hectare, Units.None);
      int unit6 = this.getUnit(this.project_s, Units.Monetaryunit, Units.Hectare, Units.None);
      int unit7 = this.getUnit(this.project_s, Units.Percent, Units.Hectare, Units.None);
      int unit8 = this.getUnit(this.project_s, Units.Percent, Units.Year, Units.Hectare);
      int unit9 = this.getUnit(this.project_s, Units.MetricTons, Units.Year, Units.Hectare);
      int unit10 = this.getUnit(this.project_s, Units.MetricTons, Units.Hectare, Units.None);
      int unit11 = this.getUnit(this.project_s, Units.Squarekilometer, Units.Hectare, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        double num = this.dictStrataNameToDensityRatio["CITY TOTAL"];
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "CITYDEN.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Species);
          sasTransaction.Pause();
          string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider().GetSQLQueryInsertEstimateTable(estimationTable, partitionClassifiers);
          insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1);
          LineValues aLine1 = new LineValues();
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine2 = str.Trim();
            if (!(aLine2 == ""))
            {
              aLine1.SetLine(aLine2);
              if (this.NumbersInLine(aLine1) > 3 && !(aLine1.ElementAt(1).Trim().ToUpper(this.ciUsed).Trim() == "NONE.") && !(aLine1.ElementAt(1).Trim().ToUpper(this.ciUsed).Trim() == "NONE"))
              {
                insertEstimateTable.SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[aLine1.ElementAt(1).Trim()]);
                if (this.dictSpeciesCodeToClassValue[aLine1.ElementAt(1).Trim()] == this.ClassValueForSpeciesAllSpecies)
                  this.HasSpeciesTotalInCITYDEN_CSV = true;
                insertEstimateTable.SetInt32("EstimateType", 1).SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(3).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(4).Trim())).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(2).Trim()));
                if (this.TreatDotBlank(aLine1.ElementAt(3).Trim()) == 0.0)
                  insertEstimateTable.SetDouble("EstimateStandardError", 0.0);
                else
                  insertEstimateTable.SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(2).Trim()) * (this.TreatDotBlank(aLine1.ElementAt(4).Trim()) / this.TreatDotBlank(aLine1.ElementAt(3).Trim())));
                insertEstimateTable.SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 2).SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(6).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(7).Trim())).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(6).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(7).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit10).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(5).Trim()));
                if (this.TreatDotBlank(aLine1.ElementAt(6).Trim()) == 0.0)
                  insertEstimateTable.SetDouble("EstimateStandardError", 0.0);
                else
                  insertEstimateTable.SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(5).Trim()) * (this.TreatDotBlank(aLine1.ElementAt(7).Trim()) / this.TreatDotBlank(aLine1.ElementAt(6).Trim())));
                insertEstimateTable.SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 3).SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(9).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(10).Trim())).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(9).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(10).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit9).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(8).Trim()));
                if (this.TreatDotBlank(aLine1.ElementAt(9).Trim()) == 0.0)
                  insertEstimateTable.SetDouble("EstimateStandardError", 0.0);
                else
                  insertEstimateTable.SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(8).Trim()) * (this.TreatDotBlank(aLine1.ElementAt(10).Trim()) / this.TreatDotBlank(aLine1.ElementAt(9).Trim())));
                insertEstimateTable.SetInt32("EstimateUnitsId", unit8).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 4).SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(11).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(12).Trim())).SetInt32("EstimateUnitsId", val).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(11).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(12).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit9).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 5).SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(14).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(15).Trim())).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(14).Trim()) / this.million).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(15).Trim()) / this.million).SetInt32("EstimateUnitsId", unit11).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(13).Trim()));
                if (this.TreatDotBlank(aLine1.ElementAt(14).Trim()) == 0.0)
                  insertEstimateTable.SetDouble("EstimateStandardError", 0.0);
                else
                  insertEstimateTable.SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(13).Trim()) * (this.TreatDotBlank(aLine1.ElementAt(15).Trim()) / this.TreatDotBlank(aLine1.ElementAt(14).Trim())));
                insertEstimateTable.SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 6).SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(17).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(18).Trim())).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(17).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(18).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit10).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(16).Trim()));
                if (this.TreatDotBlank(aLine1.ElementAt(17).Trim()) == 0.0)
                  insertEstimateTable.SetDouble("EstimateStandardError", 0.0);
                else
                  insertEstimateTable.SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(16).Trim()) * (this.TreatDotBlank(aLine1.ElementAt(18).Trim()) / this.TreatDotBlank(aLine1.ElementAt(17).Trim())));
                insertEstimateTable.SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetInt32("EstimateType", 8).SetDouble("EstimateValue", num * this.TreatDotBlank(aLine1.ElementAt(20).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(aLine1.ElementAt(21).Trim())).SetInt32("EstimateUnitsId", unit6).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
                insertEstimateTable.SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(19).Trim()));
                if (this.TreatDotBlank(aLine1.ElementAt(20).Trim()) == 0.0)
                  insertEstimateTable.SetDouble("EstimateStandardError", 0.0);
                else
                  insertEstimateTable.SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(19).Trim()) * (this.TreatDotBlank(aLine1.ElementAt(21).Trim()) / this.TreatDotBlank(aLine1.ElementAt(20).Trim())));
                insertEstimateTable.SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private void getCITYCND()
    {
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers1 = new List<Classifiers>();
        path2 = "CITYCND.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
          partitionClassifiers1.Clear();
          partitionClassifiers1.Add(Classifiers.Species);
          partitionClassifiers1.Add(Classifiers.Dieback);
          sasTransaction.Pause();
          string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers1);
          sasTransaction.Resume();
          string tableName = estimationTable;
          List<Classifiers> partitionClassifiers2 = partitionClassifiers1;
          IQuery insertEstimateTable = sasUtilProvider.GetSQLQueryInsertEstimateTable(tableName, partitionClassifiers2);
          insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1).SetInt32("EstimateType", 1).SetInt32("EstimateUnitsId", unit);
          LineValues aLine1 = new LineValues();
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine2 = str.Trim();
            aLine1.SetLine(aLine2);
            if (this.NumbersInLine(aLine1) > 3 && !(aLine1.ElementAt(1).Trim().ToUpper(this.ciUsed).Trim() == "NONE.") && !(aLine1.ElementAt(1).Trim().ToUpper(this.ciUsed).Trim() == "NONE"))
            {
              int val = this.dictSpeciesCodeToClassValue[aLine1.ElementAt(1).Trim()];
              if (val == this.ClassValueForSpeciesAllSpecies)
                this.HasSpeciesTotalInCITYCND_CSV = true;
              for (int index = 0; index < this.listDiebackClassValue.Count; ++index)
              {
                insertEstimateTable.SetInt32(Classifiers.Species.ToString(), val).SetInt32(Classifiers.Dieback.ToString(), this.listDiebackClassValue.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 2).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 3).Trim())).ExecuteUpdate();
                sasTransaction.IncreaseOperationNumber();
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private void getCITYCND2()
    {
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers1 = new List<Classifiers>();
        path2 = "CITYCND2.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
          partitionClassifiers1.Clear();
          partitionClassifiers1.Add(Classifiers.Species);
          partitionClassifiers1.Add(Classifiers.CDieback);
          sasTransaction.Pause();
          string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers1);
          sasTransaction.Resume();
          string tableName = estimationTable;
          List<Classifiers> partitionClassifiers2 = partitionClassifiers1;
          IQuery insertEstimateTable = sasUtilProvider.GetSQLQueryInsertEstimateTable(tableName, partitionClassifiers2);
          insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1).SetInt32("EstimateType", 1).SetInt32("EstimateUnitsId", unit);
          if (!this.SASResultContainTree)
          {
            streamReader.Close();
            sasTransaction.End();
            return;
          }
          string str1 = "";
          LineValues aLine1 = new LineValues();
          aLine1.separater = ',';
          List<int> source = new List<int>();
          string str2;
          while ((str2 = streamReader.ReadLine()) != null)
          {
            string aLine2 = str2.Trim();
            if (!string.IsNullOrWhiteSpace(aLine2))
            {
              aLine1.SetLine(aLine2);
              for (int index = 1; index <= aLine1.Count; ++index)
              {
                if (!string.IsNullOrWhiteSpace(aLine1.ElementAt(index).Trim()))
                {
                  if (this.dictCDiebackIdToClassValue.ContainsKey(aLine1.ElementAt(index).Trim()))
                    source.Add(this.dictCDiebackIdToClassValue[aLine1.ElementAt(index).Trim()]);
                  else
                    source.Add(-1);
                }
              }
              break;
            }
          }
          str1 = streamReader.ReadLine();
          string str3;
          while ((str3 = streamReader.ReadLine()) != null)
          {
            string aLine3 = str3.Trim();
            aLine1.SetLine(aLine3);
            if (this.NumbersInLine(aLine1) > 3 && !(aLine1.ElementAt(1).Trim().ToUpper(this.ciUsed).Trim() == "NONE.") && !(aLine1.ElementAt(1).Trim().ToUpper(this.ciUsed).Trim() == "NONE"))
            {
              int val = this.dictSpeciesCodeToClassValue[aLine1.ElementAt(1).Trim()];
              if (val == this.ClassValueForSpeciesAllSpecies)
                this.HasSpeciesTotalInCITYCND2_CSV = true;
              for (int index = 0; index < source.Count; ++index)
              {
                if (source.ElementAt<int>(index) != -1)
                {
                  insertEstimateTable.SetInt32(Classifiers.Species.ToString(), val).SetInt32(Classifiers.CDieback.ToString(), source.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 2).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 3).Trim())).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private void getCITYDBH()
    {
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "CITYDBH.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Species);
          partitionClassifiers.Add(Classifiers.DBH);
          sasTransaction.Pause();
          string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider().GetSQLQueryInsertEstimateTable(estimationTable, partitionClassifiers);
          insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1).SetInt32("EstimateType", 1).SetInt32("EstimateUnitsId", unit);
          LineValues lineValues1 = new LineValues();
          LineValues lineValues2 = new LineValues();
          LineValues aLine1 = new LineValues();
          List<int> source = new List<int>();
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            string aLine2 = str.Trim();
            if (aLine2.Length > 0)
            {
              aLine1.SetLine(aLine2);
              if (this.isFirstDBHClassLine(aLine1))
              {
                lineValues1.SetLine(aLine2);
                lineValues2.SetLine(streamReader.ReadLine());
                LineValues lineValues3 = new LineValues();
                lineValues3.separater = ' ';
                source.Clear();
                for (int index = 1; index <= lineValues2.Count; ++index)
                {
                  if (lineValues2.ElementAt(index).Trim() != "")
                  {
                    lineValues3.SetLine(lineValues1.ElementAt(index).Trim());
                    source.Add(this.dictDBHtoClassValue[lineValues3.ElementAt(1).Trim() + " - " + lineValues2.ElementAt(index).Trim()]);
                  }
                }
              }
              else if (this.isCITYDBHDataLine(aLine1) && !(aLine1.ElementAt(1).Trim().ToUpper(this.ciUsed).Trim() == "NONE.") && !(aLine1.ElementAt(1).Trim().ToUpper(this.ciUsed).Trim() == "NONE"))
              {
                insertEstimateTable.SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[aLine1.ElementAt(1).Trim()]);
                if (this.dictSpeciesCodeToClassValue[aLine1.ElementAt(1).Trim()] == this.ClassValueForSpeciesAllSpecies)
                  this.HasSpeciesTotalInCITYDBH_CSV = true;
                for (int index = 0; index < source.Count; ++index)
                {
                  insertEstimateTable.SetInt32(Classifiers.DBH.ToString(), source.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 2).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(aLine1.ElementAt(index * 2 + 3).Trim())).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private void getCITYDBH2()
    {
      string path2 = "";
      int unit = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "CITYDBH2.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Species);
          partitionClassifiers.Add(Classifiers.CDBH);
          sasTransaction.Pause();
          string estimationTable = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider().GetSQLQueryInsertEstimateTable(estimationTable, partitionClassifiers);
          insertEstimateTable.SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1).SetInt32("EstimateType", 1).SetInt32("EstimateUnitsId", unit);
          string str1 = "";
          LineValues lineValues = new LineValues();
          lineValues.separater = ',';
          List<int> source = new List<int>();
          string str2;
          while ((str2 = streamReader.ReadLine()) != null)
          {
            string aLine = str2.Trim();
            if (!string.IsNullOrWhiteSpace(aLine))
            {
              lineValues.SetLine(aLine);
              for (int index = 1; index <= lineValues.Count; ++index)
              {
                if (!string.IsNullOrWhiteSpace(lineValues.ElementAt(index).Trim()))
                {
                  if (this.dictCDBHidToClassValue.ContainsKey(lineValues.ElementAt(index).Trim()))
                    source.Add(this.dictCDBHidToClassValue[lineValues.ElementAt(index).Trim()]);
                  else
                    source.Add(-1);
                }
              }
              break;
            }
          }
          str1 = streamReader.ReadLine();
          string str3;
          while ((str3 = streamReader.ReadLine()) != null)
          {
            string aLine = str3.Trim();
            if (!string.IsNullOrWhiteSpace(aLine))
            {
              lineValues.SetLine(aLine);
              insertEstimateTable.SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[lineValues.ElementAt(1).Trim()]);
              if (this.dictSpeciesCodeToClassValue[lineValues.ElementAt(1).Trim()] == this.ClassValueForSpeciesAllSpecies)
                this.HasSpeciesTotalInCITYDBH2_CSV = true;
              for (int index = 0; index < source.Count; ++index)
              {
                if (source.ElementAt<int>(index) != -1)
                {
                  insertEstimateTable.SetInt32(Classifiers.CDBH.ToString(), source.ElementAt<int>(index)).SetDouble("EstimateValue", this.TreatDotBlank(lineValues.ElementAt(index * 2 + 2).Trim())).SetDouble("EstimateStandardError", this.TreatDotBlank(lineValues.ElementAt(index * 2 + 3).Trim())).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private bool isCITYDBHDataLine(LineValues aLine)
    {
      double result = 0.0;
      return aLine.Count > 3 && this.NumbersInLine(aLine) > 3 && aLine.ElementAt(1).Trim() != "" && !double.TryParse(aLine.ElementAt(1), this.nsFloat, (IFormatProvider) this.ciUsed, out result);
    }

    private void getAREAESTM()
    {
      string path2 = "";
      int unit1 = this.getUnit(this.project_s, Units.Count, Units.Hectare, Units.None);
      int unit2 = this.getUnit(this.project_s, Units.Kilograms, Units.Hectare, Units.None);
      int unit3 = this.getUnit(this.project_s, Units.MetricTons, Units.Hectare, Units.None);
      int unit4 = this.getUnit(this.project_s, Units.Kilograms, Units.Year, Units.Hectare);
      int unit5 = this.getUnit(this.project_s, Units.MetricTons, Units.Year, Units.Hectare);
      int unit6 = this.getUnit(this.project_s, Units.Squaremeter, Units.Hectare, Units.None);
      int unit7 = this.getUnit(this.project_s, Units.Squarekilometer, Units.Hectare, Units.None);
      int unit8 = this.getUnit(this.project_s, Units.Monetaryunit, Units.Hectare, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        this.getDensityCoverPercentRatio();
        SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        path2 = "AREAESTM.CSV";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          sasTransaction.Pause();
          string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers);
          insertEstimateTable1.SetGuid("vYearGuid", this.currYear.Guid);
          insertEstimateTable1.SetInt32("EquationType", 1);
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Species);
          sasTransaction.Pause();
          string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
          sasTransaction.Resume();
          IQuery insertEstimateTable2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers);
          insertEstimateTable2.SetGuid("vYearGuid", this.currYear.Guid);
          insertEstimateTable2.SetInt32("EquationType", 1);
          double result = 0.0;
          bool flag1 = false;
          string key1 = "";
          LineValues lineValues = new LineValues();
          string str;
          while ((str = streamReader.ReadLine()) != null)
          {
            flag1 = false;
            string aLine = str.Trim();
            if (!(aLine == ""))
            {
              lineValues.SetLine(aLine);
              if (lineValues.Count == 1)
                key1 = lineValues.ElementAt(1).Trim();
              if (lineValues.ElementAt(1).ToUpper(this.ciUsed).Trim() == "CITY TOTAL")
                key1 = "CITY TOTAL";
              bool flag2;
              if (lineValues.Count >= 3)
              {
                string s = lineValues.ElementAt(3).Trim();
                flag2 = s == "" || s == "." || double.TryParse(s, this.nsFloat, (IFormatProvider) this.ciUsed, out result);
              }
              else
                flag2 = false;
              if (flag2)
              {
                if (lineValues.ElementAt(1).Trim() != "")
                  key1 = lineValues.ElementAt(1).Trim();
                string key2 = !(lineValues.ElementAt(2).Trim() != "") ? "Total" : lineValues.ElementAt(2).Trim();
                if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  key2 = "Total";
                if (!(key2.ToUpper(this.ciUsed).Trim() == "NONE.") && !(key2.ToUpper(this.ciUsed).Trim() == "NONE"))
                {
                  double num = this.dictStrataNameToDensityRatio[key1];
                  insertEstimateTable2.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32(Classifiers.Species.ToString(), this.dictSpeciesCodeToClassValue[key2]).SetInt32("EstimateType", 1).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(3).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(4).Trim())).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32("EstimateType", 1).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(3).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(4).Trim())).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 2).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(5).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(6).Trim())).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(5).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(6).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32("EstimateType", 2).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(5).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(6).Trim())).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(5).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(6).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 3).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(7).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(8).Trim())).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(7).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(8).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32("EstimateType", 3).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(7).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(8).Trim())).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(7).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(8).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 4).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(9).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(10).Trim())).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(9).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(10).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32("EstimateType", 4).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(9).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(10).Trim())).SetInt32("EstimateUnitsId", unit4).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(9).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(10).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit5).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 5).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(11).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(12).Trim())).SetInt32("EstimateUnitsId", unit6).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(11).Trim()) / this.million).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(12).Trim()) / this.million).SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32("EstimateType", 5).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(11).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(12).Trim())).SetInt32("EstimateUnitsId", unit6).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32("EstimateType", 5).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(11).Trim()) / this.million).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(12).Trim()) / this.million).SetInt32("EstimateUnitsId", unit7).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 6).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(13).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(14).Trim())).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  insertEstimateTable2.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(13).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(14).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32("EstimateType", 6).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(13).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(14).Trim())).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                    insertEstimateTable1.SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(13).Trim()) / this.thousand).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(14).Trim()) / this.thousand).SetInt32("EstimateUnitsId", unit3).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                  insertEstimateTable2.SetInt32("EstimateType", 8).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(15).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(16).Trim())).SetInt32("EstimateUnitsId", unit8).ExecuteUpdate();
                  sasTransaction.IncreaseOperationNumber();
                  if (key2.Equals("Total", StringComparison.InvariantCultureIgnoreCase))
                  {
                    insertEstimateTable1.SetInt32(Classifiers.Strata.ToString(), this.dictStrataNameToClassValue[key1]).SetInt32("EstimateType", 8).SetDouble("EstimateValue", num * this.TreatDotBlank(lineValues.ElementAt(15).Trim())).SetDouble("EstimateStandardError", num * this.TreatDotBlank(lineValues.ElementAt(16).Trim())).SetInt32("EstimateUnitsId", unit8).ExecuteUpdate();
                    sasTransaction.IncreaseOperationNumber();
                  }
                }
              }
            }
          }
          streamReader.Close();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw new Exception(string.Format(SASResources.ErrorProcessingFile, (object) path2, (object) ex.Message));
      }
    }

    private double TreatDotBlank(string aWord) => aWord == "" || aWord == "." ? 0.0 : double.Parse(aWord, this.nsFloat, (IFormatProvider) this.ciUsed);

    private void PopulateUFORE_DResults2(EstimateDataTypes estDataType)
    {
      string str;
      switch (estDataType)
      {
        case EstimateDataTypes.Tree:
          str = "T";
          break;
        case EstimateDataTypes.Shrub:
          str = "S";
          break;
        default:
          str = "G";
          break;
      }
      int unit1 = this.getUnit(this.project_s, Units.MetricTons, Units.None, Units.None);
      int unit2 = this.getUnit(this.project_s, Units.Monetaryunit, Units.None, Units.None);
      int unit3 = this.getUnit(this.project_s, Units.Squarekilometer, Units.None, Units.None);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        partitionClassifiers.Add(Classifiers.Month);
        partitionClassifiers.Add(Classifiers.Pollutant);
        sasTransaction.Pause();
        string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, estDataType, partitionClassifiers);
        sasTransaction.Resume();
        IList<(string, string, int, double, double)> tupleList1 = this.project_s.GetNamedQuery("selectYearlyPollutantRemovalAndValue").SetGuid("YearGuid", this.currYear.Guid).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(string, string, int, double, double)>()).List<(string, string, int, double, double)>();
        SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
        IQuery query1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers).SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 31).SetInt32("EquationType", 1);
        SASDictionary<string, double> sasDictionary1 = new SASDictionary<string, double>();
        SASDictionary<string, double> sasDictionary2 = new SASDictionary<string, double>();
        foreach ((string, string, int, double, double) tuple in (IEnumerable<(string, string, int, double, double)>) tupleList1)
        {
          if (tuple.Item2 == str)
          {
            if (sasDictionary1.ContainsKey(tuple.Item1))
            {
              sasDictionary1[tuple.Item1] += tuple.Item4 / this.million;
              sasDictionary2[tuple.Item1] += tuple.Item5;
            }
            else
            {
              sasDictionary1.Add(tuple.Item1, tuple.Item4 / this.million);
              sasDictionary2.Add(tuple.Item1, tuple.Item5);
            }
            query1.SetInt32(Classifiers.Month.ToString(), this.dictMonthToClassValue[tuple.Item3.ToString()]).SetInt32(Classifiers.Pollutant.ToString(), this.dictPollutantToClassValue[tuple.Item1]).SetDouble("EstimateValue", tuple.Item4 / this.million).SetDouble("EstimateStandardError", 0.0).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
            query1.SetDouble("EstimateValue", tuple.Item5).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber();
          }
        }
        if (estDataType != EstimateDataTypes.Grass)
        {
          double num = 0.0;
          foreach (StratumSAS stratumSas in this.dictStrataSASInfo.Values)
          {
            if (estDataType == EstimateDataTypes.Tree)
            {
              if (stratumSas.ClassValue != this.ClassValueForStrataStudyArea)
                num += stratumSas.TreeLeafAreaHectare;
            }
            else if (stratumSas.ClassValue != this.ClassValueForStrataStudyArea)
              num += stratumSas.ShrubLeafAreaHectare;
          }
          if (num == 0.0)
            num = 1.0;
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          sasTransaction.Pause();
          string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, estDataType, partitionClassifiers);
          sasTransaction.Resume();
          IQuery selectEstimateTable1 = sasUtilProvider.GetSQLQuerySelectEstimateTable(estimationTable2, partitionClassifiers);
          selectEstimateTable1.SetGuid("YearGuid", this.currYear.Guid).SetInt32("EstimateType", 5).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit3);
          IList<(int, double, double)> tupleList2 = selectEstimateTable1.SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, double, double)>()).List<(int, double, double)>();
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Pollutant);
          sasTransaction.Pause();
          string estimationTable3 = this.getEstimationTable(this.m_ps.InputSession, estDataType, partitionClassifiers);
          sasTransaction.Resume();
          IQuery query2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable3, partitionClassifiers).SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 31).SetInt32("EquationType", 1).SetDouble("EstimateStandardError", 0.0);
          foreach ((int, double, double) tuple in (IEnumerable<(int, double, double)>) tupleList2)
          {
            foreach (KeyValuePair<string, double> keyValuePair in (Dictionary<string, double>) sasDictionary1)
            {
              IQuery query3 = query2;
              Classifiers classifiers = Classifiers.Strata;
              string name1 = classifiers.ToString();
              int val1 = tuple.Item1;
              IQuery query4 = query3.SetInt32(name1, val1);
              classifiers = Classifiers.Pollutant;
              string name2 = classifiers.ToString();
              int val2 = this.dictPollutantToClassValue[keyValuePair.Key];
              query4.SetInt32(name2, val2).SetDouble("EstimateValue", keyValuePair.Value * tuple.Item2 * 100.0 / num).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
              sasTransaction.IncreaseOperationNumber();
              query2.SetDouble("EstimateValue", sasDictionary2[keyValuePair.Key] * tuple.Item2 * 100.0 / num).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
              sasTransaction.IncreaseOperationNumber();
            }
          }
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Species);
          sasTransaction.Pause();
          string estimationTable4 = this.getEstimationTable(this.m_ps.InputSession, estDataType, partitionClassifiers);
          sasTransaction.Resume();
          IQuery selectEstimateTable2 = sasUtilProvider.GetSQLQuerySelectEstimateTable(estimationTable4, partitionClassifiers);
          selectEstimateTable2.SetGuid("YearGuid", this.currYear.Guid).SetInt32("EstimateType", 5).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit3);
          IList<(int, int, double, double)> tupleList3 = selectEstimateTable2.SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, int, double, double)>()).List<(int, int, double, double)>();
          partitionClassifiers.Clear();
          partitionClassifiers.Add(Classifiers.Strata);
          partitionClassifiers.Add(Classifiers.Species);
          partitionClassifiers.Add(Classifiers.Pollutant);
          sasTransaction.Pause();
          string estimationTable5 = this.getEstimationTable(this.m_ps.InputSession, estDataType, partitionClassifiers);
          sasTransaction.Resume();
          IQuery query5 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable5, partitionClassifiers).SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 31).SetInt32("EquationType", 1).SetDouble("EstimateStandardError", 0.0);
          foreach ((int, int, double, double) tuple in (IEnumerable<(int, int, double, double)>) tupleList3)
          {
            foreach (KeyValuePair<string, double> keyValuePair in (Dictionary<string, double>) sasDictionary1)
            {
              IQuery query6 = query5;
              Classifiers classifiers = Classifiers.Strata;
              string name3 = classifiers.ToString();
              int val3 = tuple.Item1;
              IQuery query7 = query6.SetInt32(name3, val3);
              classifiers = Classifiers.Species;
              string name4 = classifiers.ToString();
              int val4 = tuple.Item2;
              IQuery query8 = query7.SetInt32(name4, val4);
              classifiers = Classifiers.Pollutant;
              string name5 = classifiers.ToString();
              int val5 = this.dictPollutantToClassValue[keyValuePair.Key];
              query8.SetInt32(name5, val5).SetDouble("EstimateValue", keyValuePair.Value * tuple.Item3 * 100.0 / num).SetInt32("EstimateUnitsId", unit1).ExecuteUpdate();
              sasTransaction.IncreaseOperationNumber();
              query5.SetDouble("EstimateValue", sasDictionary2[keyValuePair.Key] * tuple.Item3 * 100.0 / num).SetInt32("EstimateUnitsId", unit2).ExecuteUpdate();
              sasTransaction.IncreaseOperationNumber();
            }
          }
          if (estDataType == EstimateDataTypes.Tree)
            this.project_s.GetNamedQuery("populateIndividualTreePollutionEffects").SetGuid("YearGuid", this.currYear.Guid).SetDouble("COgramPerM2", sasDictionary1["CO"] * 100.0 / num).SetDouble("COvaluePerM2", sasDictionary2["CO"] / (num * 10000.0)).SetDouble("NO2gramPerM2", sasDictionary1["NO2"] * 100.0 / num).SetDouble("NO2valuePerM2", sasDictionary2["NO2"] / (num * 10000.0)).SetDouble("O3gramPerM2", sasDictionary1["O3"] * 100.0 / num).SetDouble("O3valuePerM2", sasDictionary2["O3"] / (num * 10000.0)).SetDouble("PM10gramPerM2", sasDictionary1["PM10*"] * 100.0 / num).SetDouble("PM10valuePerM2", sasDictionary2["PM10*"] / (num * 10000.0)).SetDouble("PM25gramPerM2", sasDictionary1["PM2.5"] * 100.0 / num).SetDouble("PM25valuePerM2", sasDictionary2["PM2.5"] / (num * 10000.0)).SetDouble("SO2gramPerM2", sasDictionary1["SO2"] * 100.0 / num).SetDouble("SO2valuePerM2", sasDictionary2["SO2"] / (num * 10000.0)).ExecuteUpdate();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw;
      }
    }

    public void PrepareForLoadingUFOREDBC()
    {
      this.dictStrataSASInfo.Clear();
      this.dictStratumGenusSASInfo.Clear();
      SASDictionary<string, double> sasDictionary1 = new SASDictionary<string, double>("Strata Description");
      using (StreamReader streamReader = new StreamReader(this.outputFolder + "\\tot.txt"))
      {
        string str1 = "";
        LineValues lineValues1 = new LineValues();
        lineValues1.separater = ' ';
        int startIndex = -1;
        LineValues lineValues2 = new LineValues();
        lineValues2.separater = ' ';
        string str2;
        while ((str2 = streamReader.ReadLine()) != null)
        {
          string aLine1 = str2.Trim();
          if (aLine1 != "")
          {
            if (aLine1.ToUpper(this.ciUsed) == "ENGLISH" || aLine1.ToUpper(this.ciUsed) == "METRIC")
            {
              str1 = aLine1.ToUpper(this.ciUsed);
            }
            else
            {
              lineValues1.SetLine(aLine1);
              if (lineValues1.ElementAt(1).Trim().ToUpper(this.ciUsed) == "CODE" && lineValues1.ElementAt(lineValues1.Count).Trim().ToUpper(this.ciUsed) == "AREA")
                startIndex = aLine1.ToUpper(this.ciUsed).IndexOf("LANDUSE");
              else if (startIndex != -1)
              {
                string aLine2 = aLine1.Substring(startIndex, aLine1.Length - startIndex);
                lineValues2.SetLine(aLine2);
                string str3 = "";
                for (int index = 1; index < lineValues2.Count; ++index)
                  str3 = str3 + lineValues2.ElementAt(index) + " ";
                string key = str3.Trim();
                if (str1 == "ENGLISH")
                  sasDictionary1.Add(key, double.Parse(lineValues1.ElementAt(lineValues1.Count), this.nsFloat, (IFormatProvider) this.ciUsed) * 0.404686);
                else
                  sasDictionary1.Add(key, double.Parse(lineValues1.ElementAt(lineValues1.Count), this.nsFloat, (IFormatProvider) this.ciUsed));
              }
            }
          }
        }
        streamReader.Close();
      }
      IList<(string, double, double, double, double, double, double)> tupleList1 = this.project_s.GetNamedQuery("selectSumIndividualTreeEffectsByStrata").SetGuid("YearGuid", this.currYear.Guid).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(string, double, double, double, double, double, double)>()).List<(string, double, double, double, double, double, double)>();
      SASDictionary<string, double> sasDictionary2 = new SASDictionary<string, double>("Strata Description");
      SASDictionary<string, double> sasDictionary3 = new SASDictionary<string, double>("Strata Description");
      SASDictionary<string, double> sasDictionary4 = new SASDictionary<string, double>("Strata Description");
      foreach ((string, double, double, double, double, double, double) tuple in (IEnumerable<(string, double, double, double, double, double, double)>) tupleList1)
      {
        string key = this.StrataRGX.Replace(tuple.Item1.Trim(), "_");
        sasDictionary2.Add(key, tuple.Item3);
        sasDictionary3.Add(key, tuple.Item4);
        sasDictionary4.Add(key, tuple.Item5);
      }
      List<Classifiers> partitionClassifiers = new List<Classifiers>();
      partitionClassifiers.Add(Classifiers.Strata);
      int unit1 = this.getUnit(this.project_s, Units.Squarekilometer, Units.None, Units.None);
      string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
      SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
      IQuery selectEstimateTable1 = sasUtilProvider.GetSQLQuerySelectEstimateTable(estimationTable1, partitionClassifiers);
      selectEstimateTable1.SetGuid("YearGuid", this.currYear.Guid).SetInt32("EstimateType", 5).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit1);
      IList<(int, double, double)> tupleList2 = selectEstimateTable1.SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, double, double)>()).List<(int, double, double)>();
      SASDictionary<int, double> sasDictionary5 = new SASDictionary<int, double>("Strata Class Value");
      foreach ((int, double, double) tuple in (IEnumerable<(int, double, double)>) tupleList2)
        sasDictionary5.Add(tuple.Item1, tuple.Item2);
      string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Shrub, partitionClassifiers);
      IQuery selectEstimateTable2 = sasUtilProvider.GetSQLQuerySelectEstimateTable(estimationTable2, partitionClassifiers);
      selectEstimateTable2.SetGuid("YearGuid", this.currYear.Guid).SetInt32("EstimateType", 5).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit1);
      IList<(int, double, double)> tupleList3 = selectEstimateTable2.SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, double, double)>()).List<(int, double, double)>();
      SASDictionary<int, double> sasDictionary6 = new SASDictionary<int, double>("Strata Class Value");
      foreach ((int, double, double) tuple in (IEnumerable<(int, double, double)>) tupleList3)
        sasDictionary6.Add(tuple.Item1, tuple.Item2);
      partitionClassifiers.Clear();
      partitionClassifiers.Add(Classifiers.Strata);
      partitionClassifiers.Add(Classifiers.GroundCover);
      string estimationTable3 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Cover, partitionClassifiers);
      int unit2 = this.getUnit(this.project_s, Units.Percent, Units.None, Units.None);
      IQuery selectEstimateTable3 = sasUtilProvider.GetSQLQuerySelectEstimateTable(estimationTable3, partitionClassifiers);
      selectEstimateTable3.SetGuid("YearGuid", this.currYear.Guid).SetInt32("EstimateType", 12).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit2);
      IList<(int, int, double, double)> tupleList4 = selectEstimateTable3.SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, int, double, double)>()).List<(int, int, double, double)>();
      SASDictionary<int, double> sasDictionary7 = new SASDictionary<int, double>("Strata Class Value");
      SASDictionary<int, double> sasDictionary8 = new SASDictionary<int, double>("Strata Class Value");
      foreach ((int, int, double, double) tuple in (IEnumerable<(int, int, double, double)>) tupleList4)
      {
        if (tuple.Item2 == this.ClassValueForTreeOfGroundCover)
          sasDictionary7.Add(tuple.Item1, tuple.Item3);
        else if (tuple.Item2 == this.ClassValueForShrubOfGroundCover)
          sasDictionary8.Add(tuple.Item1, tuple.Item3);
      }
      foreach (Strata stratum in (IEnumerable<Strata>) this.currYear.Strata)
      {
        string key = this.StrataRGX.Replace(stratum.Description.Trim(), "_");
        if (this.dictStrataNameToClassValue.ContainsKey(key))
        {
          StratumSAS stratumSas = new StratumSAS();
          stratumSas.ClassValue = this.dictStrataNameToClassValue[key];
          stratumSas.Description = key;
          stratumSas.Abbreviation = stratum.Abbreviation.Trim();
          if (this.currSeries.SampleType == SampleType.Inventory)
          {
            if (sasDictionary2.ContainsKey(key))
            {
              stratumSas.TreeCoverAreaHectare = sasDictionary2[key] / this.tenThousand;
              stratumSas.TreeLeafAreaHectare = sasDictionary3[key] / this.tenThousand;
            }
            else
            {
              stratumSas.TreeCoverAreaHectare = 0.0;
              stratumSas.TreeLeafAreaHectare = 0.0;
            }
            stratumSas.ShrubCoverAreaHectare = 0.0;
            stratumSas.ShrubLeafAreaHectare = 0.0;
          }
          else
          {
            stratumSas.TreeLeafAreaHectare = !sasDictionary5.ContainsKey(this.dictStrataNameToClassValue[key]) ? 0.0 : 100.0 * sasDictionary5[this.dictStrataNameToClassValue[key]];
            stratumSas.TreeCoverAreaHectare = !sasDictionary1.ContainsKey(key) || !sasDictionary7.ContainsKey(this.dictStrataNameToClassValue[key]) ? 0.0 : sasDictionary1[key] * sasDictionary7[this.dictStrataNameToClassValue[key]] / 100.0;
            stratumSas.ShrubLeafAreaHectare = !sasDictionary6.ContainsKey(this.dictStrataNameToClassValue[key]) ? 0.0 : 100.0 * sasDictionary6[this.dictStrataNameToClassValue[key]];
            stratumSas.ShrubCoverAreaHectare = !sasDictionary1.ContainsKey(key) || !sasDictionary8.ContainsKey(this.dictStrataNameToClassValue[key]) ? 0.0 : sasDictionary1[key] * sasDictionary8[this.dictStrataNameToClassValue[key]] / 100.0;
          }
          this.dictStrataSASInfo.Add(this.dictStrataNameToClassValue[key], stratumSas);
        }
      }
      partitionClassifiers.Clear();
      partitionClassifiers.Add(Classifiers.Strata);
      partitionClassifiers.Add(Classifiers.Species);
      int unit3 = this.getUnit(this.project_s, Units.MetricTons, Units.None, Units.None);
      string estimationTable4 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, partitionClassifiers);
      IQuery selectEstimateTable4 = sasUtilProvider.GetSQLQuerySelectEstimateTable(estimationTable4, partitionClassifiers);
      selectEstimateTable4.SetGuid("YearGuid", this.currYear.Guid).SetInt32("EstimateType", 6).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit3);
      foreach ((int, int, double, double) tuple in (IEnumerable<(int, int, double, double)>) selectEstimateTable4.SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, int, double, double)>()).List<(int, int, double, double)>())
      {
        if (tuple.Item1 != this.ClassValueForStrataStudyArea && tuple.Item2 != this.ClassValueForSpeciesAllSpecies)
        {
          int num = tuple.Item1;
          int key1 = tuple.Item2;
          if (this.dictSpeciesClassValueToGenusCode.ContainsKey(key1))
          {
            string str = this.dictSpeciesClassValueToGenusCode[key1];
            string key2 = num.ToString((IFormatProvider) this.ciUsed) + "_" + str;
            if (this.dictStratumGenusSASInfo.ContainsKey(key2))
            {
              this.dictStratumGenusSASInfo[key2].TreeLeafBiomassKilograms += tuple.Item3 * this.thousand;
            }
            else
            {
              StratumGenusSAS stratumGenusSas = new StratumGenusSAS();
              stratumGenusSas.GenusScientificName = str;
              stratumGenusSas.StratumClassValue = num;
              stratumGenusSas.TreeLeafBiomassKilograms = tuple.Item3 * this.thousand;
              if (this.dictSpeciesClassValueToEvergreen[key1] == 1)
                stratumGenusSas.LeafType = "EVERGREEN";
              this.dictStratumGenusSASInfo.Add(key2, stratumGenusSas);
            }
          }
        }
      }
      string estimationTable5 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Shrub, partitionClassifiers);
      IQuery selectEstimateTable5 = sasUtilProvider.GetSQLQuerySelectEstimateTable(estimationTable5, partitionClassifiers);
      selectEstimateTable5.SetGuid("YearGuid", this.currYear.Guid).SetInt32("EstimateType", 6).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit3);
      foreach ((int, int, double, double) tuple in (IEnumerable<(int, int, double, double)>) selectEstimateTable5.SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, int, double, double)>()).List<(int, int, double, double)>())
      {
        if (tuple.Item1 != this.ClassValueForStrataStudyArea && tuple.Item2 != this.ClassValueForSpeciesAllSpecies)
        {
          int num = tuple.Item1;
          int key3 = tuple.Item2;
          if (this.dictSpeciesClassValueToGenusCode.ContainsKey(key3))
          {
            string str = this.dictSpeciesClassValueToGenusCode[key3];
            string key4 = num.ToString((IFormatProvider) this.ciUsed) + "_" + str;
            if (this.dictStratumGenusSASInfo.ContainsKey(key4))
            {
              this.dictStratumGenusSASInfo[key4].ShrubLeafBiomassKilograms += tuple.Item3 * this.thousand;
            }
            else
            {
              StratumGenusSAS stratumGenusSas = new StratumGenusSAS();
              stratumGenusSas.GenusScientificName = str;
              stratumGenusSas.StratumClassValue = num;
              stratumGenusSas.ShrubLeafBiomassKilograms = tuple.Item3 * this.thousand;
              if (this.dictSpeciesClassValueToEvergreen[key3] == 1)
                stratumGenusSas.LeafType = "EVERGREEN";
              this.dictStratumGenusSASInfo.Add(key4, stratumGenusSas);
            }
          }
        }
      }
    }

    private void PopulatingUFORE_BResults2(EstimateDataTypes tableDataType)
    {
      int unit1 = this.getUnit(this.project_s, Units.MetricTons, Units.None, Units.None);
      int unit2 = this.getUnit(this.project_s, Units.Kilograms, Units.None, Units.Year);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        double num = 0.001;
        int val1 = this.dictVOCToClassValue["Isoprene"];
        int val2 = this.dictVOCToClassValue["Monoterpene"];
        int val3 = this.dictVOCToClassValue["VOC Other"];
        (double, double, double) valueTuple1 = (0.0, 0.0, 0.0);
        (double, double, double) valueTuple2 = (0.0, 0.0, 0.0);
        SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
        IList<(string, double, double, double)> tupleList1 = sasUtilProvider.GetSQLQuerySelectYearlySummaryByLanduse(this.outputFolder, tableDataType == EstimateDataTypes.Tree ? "UFOREBTree" : "UFOREBShrub").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(string, double, double, double)>()).List<(string, double, double, double)>();
        Dictionary<string, (double, double, double)> dictionary1 = new Dictionary<string, (double, double, double)>();
        foreach ((string, double, double, double) tuple in (IEnumerable<(string, double, double, double)>) tupleList1)
        {
          dictionary1.Add(tuple.Item1, (tuple.Item2 * num, tuple.Item3 * num, tuple.Item4 * num));
          valueTuple2.Item1 += tuple.Item2 * num;
          valueTuple2.Item2 += tuple.Item3 * num;
          valueTuple2.Item3 += tuple.Item4 * num;
        }
        IList<(string, double, double, double)> tupleList2 = sasUtilProvider.GetSQLQuerySelectYearlySummaryBySpecies(this.outputFolder, tableDataType == EstimateDataTypes.Tree ? "UFOREBTree" : "UFOREBShrub").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(string, double, double, double)>()).List<(string, double, double, double)>();
        Dictionary<string, (double, double, double)> dictionary2 = new Dictionary<string, (double, double, double)>();
        foreach ((string, double, double, double) tuple in (IEnumerable<(string, double, double, double)>) tupleList2)
          dictionary2.Add(tuple.Item1, (tuple.Item2 * num, tuple.Item3 * num, tuple.Item4 * num));
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        partitionClassifiers.Clear();
        partitionClassifiers.Add(Classifiers.Strata);
        sasTransaction.Pause();
        string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, tableDataType, partitionClassifiers);
        sasTransaction.Resume();
        IList<(int, double, double)> tupleList3 = sasUtilProvider.GetSQLQuerySelectEstimateTable(estimationTable1, partitionClassifiers).SetGuid("YearGuid", this.currYear.Guid).SetInt32("EstimateType", 6).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit1).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, double, double)>()).List<(int, double, double)>();
        partitionClassifiers.Clear();
        partitionClassifiers.Add(Classifiers.Strata);
        partitionClassifiers.Add(Classifiers.VOCs);
        sasTransaction.Pause();
        string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, tableDataType, partitionClassifiers);
        sasTransaction.Resume();
        IQuery query1 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers).SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 32).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit2);
        foreach ((int, double, double) tuple in (IEnumerable<(int, double, double)>) tupleList3)
        {
          (double, double, double) valueTuple3 = tuple.Item1 != this.ClassValueForStrataStudyArea ? (!dictionary1.ContainsKey(this.dictStrataClassValueToName[tuple.Item1]) ? valueTuple1 : dictionary1[this.dictStrataClassValueToName[tuple.Item1]]) : valueTuple2;
          query1.SetInt32(Classifiers.Strata.ToString(), tuple.Item1).SetInt32(Classifiers.VOCs.ToString(), val1).SetDouble("EstimateValue", valueTuple3.Item1).SetDouble("EstimateStandardError", tuple.Item2 == 0.0 ? 0.0 : tuple.Item3 / tuple.Item2 * valueTuple3.Item1).ExecuteUpdate();
          query1.SetInt32(Classifiers.Strata.ToString(), tuple.Item1).SetInt32(Classifiers.VOCs.ToString(), val2).SetDouble("EstimateValue", valueTuple3.Item2).SetDouble("EstimateStandardError", tuple.Item2 == 0.0 ? 0.0 : tuple.Item3 / tuple.Item2 * valueTuple3.Item2).ExecuteUpdate();
          query1.SetInt32(Classifiers.Strata.ToString(), tuple.Item1).SetInt32(Classifiers.VOCs.ToString(), val3).SetDouble("EstimateValue", valueTuple3.Item3).SetDouble("EstimateStandardError", tuple.Item2 == 0.0 ? 0.0 : tuple.Item3 / tuple.Item2 * valueTuple3.Item3).ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber(3);
        }
        partitionClassifiers.Clear();
        partitionClassifiers.Add(Classifiers.Species);
        sasTransaction.Pause();
        string estimationTable3 = this.getEstimationTable(this.m_ps.InputSession, tableDataType, partitionClassifiers);
        sasTransaction.Resume();
        IList<(int, double, double)> tupleList4 = sasUtilProvider.GetSQLQuerySelectEstimateTable(estimationTable3, partitionClassifiers).SetGuid("YearGuid", this.currYear.Guid).SetInt32("EstimateType", 6).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit1).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, double, double)>()).List<(int, double, double)>();
        partitionClassifiers.Clear();
        partitionClassifiers.Add(Classifiers.Species);
        partitionClassifiers.Add(Classifiers.VOCs);
        sasTransaction.Pause();
        string estimationTable4 = this.getEstimationTable(this.m_ps.InputSession, tableDataType, partitionClassifiers);
        sasTransaction.Resume();
        IQuery query2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable4, partitionClassifiers).SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 32).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit2);
        Dictionary<int, double> dictionary3 = new Dictionary<int, double>();
        Dictionary<int, (double, double, double)> dictionary4 = new Dictionary<int, (double, double, double)>();
        (double, double, double) valueTuple4 = (0.0, 0.0, 0.0);
        foreach ((int, double, double) tuple in (IEnumerable<(int, double, double)>) tupleList4)
        {
          dictionary3.Add(tuple.Item1, tuple.Item2 * this.thousand);
          if (tuple.Item2 == 0.0)
            dictionary4.Add(tuple.Item1, (0.0, 0.0, 0.0));
          else if (tuple.Item1 != this.ClassValueForSpeciesAllSpecies)
          {
            if (dictionary2.ContainsKey(this.dictSpeciesClassValueToCode[tuple.Item1]))
            {
              (double, double, double) valueTuple5 = dictionary2[this.dictSpeciesClassValueToCode[tuple.Item1]];
              dictionary4.Add(tuple.Item1, (valueTuple5.Item1 / (tuple.Item2 * 1000.0), valueTuple5.Item2 / (tuple.Item2 * 1000.0), valueTuple5.Item3 / (tuple.Item2 * 1000.0)));
            }
            else
              dictionary4.Add(tuple.Item1, (0.0, 0.0, 0.0));
          }
          (double, double, double) valueTuple6 = tuple.Item1 != this.ClassValueForSpeciesAllSpecies ? (!dictionary2.ContainsKey(this.dictSpeciesClassValueToCode[tuple.Item1]) ? valueTuple1 : dictionary2[this.dictSpeciesClassValueToCode[tuple.Item1]]) : valueTuple2;
          query2.SetInt32(Classifiers.Species.ToString(), tuple.Item1).SetInt32(Classifiers.VOCs.ToString(), val1).SetDouble("EstimateValue", valueTuple6.Item1).SetDouble("EstimateStandardError", tuple.Item2 == 0.0 ? 0.0 : tuple.Item3 / tuple.Item2 * valueTuple6.Item1).ExecuteUpdate();
          query2.SetInt32(Classifiers.Species.ToString(), tuple.Item1).SetInt32(Classifiers.VOCs.ToString(), val2).SetDouble("EstimateValue", valueTuple6.Item2).SetDouble("EstimateStandardError", tuple.Item2 == 0.0 ? 0.0 : tuple.Item3 / tuple.Item2 * valueTuple6.Item2).ExecuteUpdate();
          query2.SetInt32(Classifiers.Species.ToString(), tuple.Item1).SetInt32(Classifiers.VOCs.ToString(), val3).SetDouble("EstimateValue", valueTuple6.Item3).SetDouble("EstimateStandardError", tuple.Item2 == 0.0 ? 0.0 : tuple.Item3 / tuple.Item2 * valueTuple6.Item3).ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber(3);
        }
        partitionClassifiers.Clear();
        partitionClassifiers.Add(Classifiers.Strata);
        partitionClassifiers.Add(Classifiers.Species);
        sasTransaction.Pause();
        string estimationTable5 = this.getEstimationTable(this.m_ps.InputSession, tableDataType, partitionClassifiers);
        sasTransaction.Resume();
        IList<(int, int, double, double)> tupleList5 = sasUtilProvider.GetSQLQuerySelectEstimateTable(estimationTable5, partitionClassifiers).SetGuid("YearGuid", this.currYear.Guid).SetInt32("EstimateType", 6).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit1).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, int, double, double)>()).List<(int, int, double, double)>();
        partitionClassifiers.Clear();
        partitionClassifiers.Add(Classifiers.Strata);
        partitionClassifiers.Add(Classifiers.Species);
        partitionClassifiers.Add(Classifiers.VOCs);
        sasTransaction.Pause();
        string estimationTable6 = this.getEstimationTable(this.m_ps.InputSession, tableDataType, partitionClassifiers);
        sasTransaction.Resume();
        IQuery query3 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable6, partitionClassifiers).SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EstimateType", 32).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit2);
        foreach ((int, int, double, double) tuple in (IEnumerable<(int, int, double, double)>) tupleList5)
        {
          if (tuple.Item1 == this.ClassValueForStrataStudyArea || tuple.Item2 == this.ClassValueForSpeciesAllSpecies)
          {
            (double, double, double) valueTuple7 = tuple.Item1 != this.ClassValueForStrataStudyArea || tuple.Item2 != this.ClassValueForSpeciesAllSpecies ? (tuple.Item1 == this.ClassValueForStrataStudyArea || tuple.Item2 != this.ClassValueForSpeciesAllSpecies ? (!dictionary2.ContainsKey(this.dictSpeciesClassValueToCode[tuple.Item2]) ? valueTuple1 : dictionary2[this.dictSpeciesClassValueToCode[tuple.Item2]]) : (!dictionary1.ContainsKey(this.dictStrataClassValueToName[tuple.Item1]) ? valueTuple1 : dictionary1[this.dictStrataClassValueToName[tuple.Item1]])) : valueTuple2;
            query3.SetInt32(Classifiers.Strata.ToString(), tuple.Item1).SetInt32(Classifiers.Species.ToString(), tuple.Item2).SetInt32(Classifiers.VOCs.ToString(), val1).SetDouble("EstimateValue", valueTuple7.Item1).SetDouble("EstimateStandardError", tuple.Item3 == 0.0 ? 0.0 : tuple.Item4 / tuple.Item3 * valueTuple7.Item1).ExecuteUpdate();
            query3.SetInt32(Classifiers.Strata.ToString(), tuple.Item1).SetInt32(Classifiers.Species.ToString(), tuple.Item2).SetInt32(Classifiers.VOCs.ToString(), val2).SetDouble("EstimateValue", valueTuple7.Item2).SetDouble("EstimateStandardError", tuple.Item3 == 0.0 ? 0.0 : tuple.Item4 / tuple.Item3 * valueTuple7.Item2).ExecuteUpdate();
            query3.SetInt32(Classifiers.Strata.ToString(), tuple.Item1).SetInt32(Classifiers.Species.ToString(), tuple.Item2).SetInt32(Classifiers.VOCs.ToString(), val3).SetDouble("EstimateValue", valueTuple7.Item3).SetDouble("EstimateStandardError", tuple.Item3 == 0.0 ? 0.0 : tuple.Item4 / tuple.Item3 * valueTuple7.Item3).ExecuteUpdate();
          }
          else
          {
            string str = this.dictSpeciesClassValueToCode[tuple.Item2];
            (double, double, double) valueTuple8 = !dictionary4.ContainsKey(tuple.Item2) ? valueTuple4 : dictionary4[tuple.Item2];
            query3.SetInt32(Classifiers.Strata.ToString(), tuple.Item1).SetInt32(Classifiers.Species.ToString(), tuple.Item2).SetInt32(Classifiers.VOCs.ToString(), val1).SetDouble("EstimateValue", tuple.Item3 * this.thousand * valueTuple8.Item1).SetDouble("EstimateStandardError", tuple.Item4 * this.thousand * valueTuple8.Item1).ExecuteUpdate();
            query3.SetInt32(Classifiers.Strata.ToString(), tuple.Item1).SetInt32(Classifiers.Species.ToString(), tuple.Item2).SetInt32(Classifiers.VOCs.ToString(), val2).SetDouble("EstimateValue", tuple.Item3 * this.thousand * valueTuple8.Item2).SetDouble("EstimateStandardError", tuple.Item4 * this.thousand * valueTuple8.Item2).ExecuteUpdate();
            query3.SetInt32(Classifiers.Strata.ToString(), tuple.Item1).SetInt32(Classifiers.Species.ToString(), tuple.Item2).SetInt32(Classifiers.VOCs.ToString(), val3).SetDouble("EstimateValue", tuple.Item3 * this.thousand * valueTuple8.Item3).SetDouble("EstimateStandardError", tuple.Item4 * this.thousand * valueTuple8.Item3).ExecuteUpdate();
          }
          sasTransaction.IncreaseOperationNumber(3);
        }
        if (tableDataType == EstimateDataTypes.Tree)
        {
          IList<(int, int, string, double, double)> tupleList6 = this.project_s.GetNamedQuery("selectIndividualTreeLeafAreaBiomass").SetGuid("YearGuid", this.currYear.Guid).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, int, string, double, double)>()).List<(int, int, string, double, double)>();
          IQuery query4 = this.project_s.GetNamedQuery("updateBioEmissionInIndividualTreePollutionEffects").SetGuid("YearGuid", this.currYear.Guid);
          foreach ((int, int, string, double, double) tuple in (IEnumerable<(int, int, string, double, double)>) tupleList6)
          {
            if (this.dictSpeciesCodeToClassValue.ContainsKey(tuple.Item3) && dictionary4.ContainsKey(this.dictSpeciesCodeToClassValue[tuple.Item3]))
            {
              (double, double, double) valueTuple9 = dictionary4[this.dictSpeciesCodeToClassValue[tuple.Item3]];
              query4.SetInt32("PlotId", tuple.Item1).SetInt32("TreeId", tuple.Item2).SetDouble("ISOPRENE", tuple.Item5 * this.thousand * valueTuple9.Item1).SetDouble("MONOTERP", tuple.Item5 * this.thousand * valueTuple9.Item2).SetDouble("OVOC", tuple.Item5 * this.thousand * valueTuple9.Item3).ExecuteUpdate();
              sasTransaction.IncreaseOperationNumber();
            }
          }
        }
        IQuery hourlyUforebResults;
        if (tableDataType == EstimateDataTypes.Tree)
        {
          hourlyUforebResults = sasUtilProvider.GetSQLQueryTransferHourlyUFOREBResults(this.outputFolder, "UFOREBTree");
          hourlyUforebResults.SetGuid("YearGuid", this.currYear.Guid).SetString("Category", "T");
        }
        else
        {
          hourlyUforebResults = sasUtilProvider.GetSQLQueryTransferHourlyUFOREBResults(this.outputFolder, "UFOREBShrub");
          hourlyUforebResults.SetGuid("YearGuid", this.currYear.Guid).SetString("Category", "S");
        }
        hourlyUforebResults.ExecuteUpdate();
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw;
      }
    }

    private void PopulatingWaterInterception(EstimateDataTypes tableDataType)
    {
      int unit1 = this.getUnit(this.project_s, Units.Squarekilometer, Units.None, Units.None);
      this.getUnit(this.project_s, Units.Squarekilometer, Units.Hectare, Units.None);
      int unit2 = this.getUnit(this.project_s, Units.CubicMeter, Units.None, Units.Year);
      SASTransaction sasTransaction = new SASTransaction();
      sasTransaction.MaxNumber = 500;
      sasTransaction.Begin(this.project_s);
      try
      {
        double num1 = 0.0;
        string dbNameWithoutExtention;
        string val;
        if (tableDataType == EstimateDataTypes.Tree)
        {
          dbNameWithoutExtention = "WaterInterceptTree";
          val = "T";
        }
        else
        {
          dbNameWithoutExtention = "WaterInterceptShrub";
          val = "S";
        }
        SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
        (double, double, double, double, double, double) tuple1 = sasUtilProvider.GetSQLQuerySelectYearlyInterceptSum(this.outputFolder, dbNameWithoutExtention).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(double, double, double, double, double, double)>()).List<(double, double, double, double, double, double)>().SingleOrDefault<(double, double, double, double, double, double)>();
        if (tuple1.Item1 == 0.0 && tuple1.Item2 == 0.0 && tuple1.Item3 == 0.0 && tuple1.Item4 == 0.0 && tuple1.Item5 == 0.0 && tuple1.Item6 == 0.0)
          return;
        foreach (int key in this.dictStrataSASInfo.Keys)
        {
          StratumSAS stratumSas = this.dictStrataSASInfo[key];
          if (key != this.ClassValueForStrataStudyArea)
          {
            if (tableDataType == EstimateDataTypes.Tree)
              num1 += stratumSas.TreeLeafAreaHectare / 100.0;
            else
              num1 += stratumSas.ShrubLeafAreaHectare / 100.0;
          }
        }
        if (num1 == 0.0)
          num1 = 1.0;
        IQuery query1 = sasUtilProvider.GetSQLQueryTransferHourlyHydroResults(this.outputFolder, dbNameWithoutExtention).SetGuid("YearGuid", this.currYear.Guid).SetString("Category", val);
        sasTransaction.IncreaseOperationNumber(query1.ExecuteUpdate());
        List<Classifiers> partitionClassifiers = new List<Classifiers>();
        partitionClassifiers.Add(Classifiers.Strata);
        sasTransaction.Pause();
        string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, tableDataType, partitionClassifiers);
        sasTransaction.Resume();
        IQuery query2 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable1, partitionClassifiers).SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit2).SetDouble("EstimateStandardError", 0.0);
        foreach (KeyValuePair<int, StratumSAS> keyValuePair in (Dictionary<int, StratumSAS>) this.dictStrataSASInfo)
        {
          double num2 = tableDataType != EstimateDataTypes.Tree ? keyValuePair.Value.ShrubLeafAreaHectare : keyValuePair.Value.TreeLeafAreaHectare;
          query2.SetInt32(Classifiers.Strata.ToString(), keyValuePair.Key);
          query2.SetInt32("EstimateType", 33).SetDouble("EstimateValue", num2 / 100.0 / num1 * tuple1.Item1).ExecuteUpdate();
          query2.SetInt32("EstimateType", 34).SetDouble("EstimateValue", num2 / 100.0 / num1 * tuple1.Item2).ExecuteUpdate();
          query2.SetInt32("EstimateType", 37).SetDouble("EstimateValue", num2 / 100.0 / num1 * tuple1.Item3).ExecuteUpdate();
          query2.SetInt32("EstimateType", 38).SetDouble("EstimateValue", num2 / 100.0 / num1 * tuple1.Item4).ExecuteUpdate();
          query2.SetInt32("EstimateType", 35).SetDouble("EstimateValue", num2 / 100.0 / num1 * tuple1.Item5).ExecuteUpdate();
          query2.SetInt32("EstimateType", 36).SetDouble("EstimateValue", num2 / 100.0 / num1 * tuple1.Item6).ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber(6);
        }
        query2.SetInt32(Classifiers.Strata.ToString(), this.ClassValueForStrataStudyArea);
        query2.SetInt32("EstimateType", 33).SetDouble("EstimateValue", tuple1.Item1).ExecuteUpdate();
        query2.SetInt32("EstimateType", 34).SetDouble("EstimateValue", tuple1.Item2).ExecuteUpdate();
        query2.SetInt32("EstimateType", 37).SetDouble("EstimateValue", tuple1.Item3).ExecuteUpdate();
        query2.SetInt32("EstimateType", 38).SetDouble("EstimateValue", tuple1.Item4).ExecuteUpdate();
        query2.SetInt32("EstimateType", 35).SetDouble("EstimateValue", tuple1.Item5).ExecuteUpdate();
        query2.SetInt32("EstimateType", 36).SetDouble("EstimateValue", tuple1.Item6).ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber(6);
        partitionClassifiers.Clear();
        partitionClassifiers.Add(Classifiers.Strata);
        partitionClassifiers.Add(Classifiers.Species);
        sasTransaction.Pause();
        string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, tableDataType, partitionClassifiers);
        sasTransaction.Resume();
        IList<(int, int, double, double)> tupleList1 = sasUtilProvider.GetSQLQuerySelectEstimateTable(estimationTable2, partitionClassifiers).SetGuid("YearGuid", this.currYear.Guid).SetInt32("EstimateType", 5).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit1).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, int, double, double)>()).List<(int, int, double, double)>();
        IQuery query3 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable2, partitionClassifiers).SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit2).SetDouble("EstimateStandardError", 0.0);
        foreach ((int, int, double, double) tuple2 in (IEnumerable<(int, int, double, double)>) tupleList1)
        {
          if (tuple2.Item1 != this.ClassValueForStrataStudyArea && tuple2.Item2 != this.ClassValueForSpeciesAllSpecies)
          {
            query3.SetInt32(Classifiers.Strata.ToString(), tuple2.Item1);
            query3.SetInt32(Classifiers.Species.ToString(), tuple2.Item2);
            query3.SetInt32("EstimateType", 33).SetDouble("EstimateValue", tuple2.Item3 / num1 * tuple1.Item1).ExecuteUpdate();
            query3.SetInt32("EstimateType", 34).SetDouble("EstimateValue", tuple2.Item3 / num1 * tuple1.Item2).ExecuteUpdate();
            query3.SetInt32("EstimateType", 37).SetDouble("EstimateValue", tuple2.Item3 / num1 * tuple1.Item3).ExecuteUpdate();
            query3.SetInt32("EstimateType", 38).SetDouble("EstimateValue", tuple2.Item3 / num1 * tuple1.Item4).ExecuteUpdate();
            query3.SetInt32("EstimateType", 35).SetDouble("EstimateValue", tuple2.Item3 / num1 * tuple1.Item5).ExecuteUpdate();
            query3.SetInt32("EstimateType", 36).SetDouble("EstimateValue", tuple2.Item3 / num1 * tuple1.Item6).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber(6);
          }
        }
        query3.SetInt32(Classifiers.Species.ToString(), this.ClassValueForSpeciesAllSpecies);
        foreach (KeyValuePair<int, StratumSAS> keyValuePair in (Dictionary<int, StratumSAS>) this.dictStrataSASInfo)
        {
          double num3 = tableDataType != EstimateDataTypes.Tree ? keyValuePair.Value.ShrubLeafAreaHectare : keyValuePair.Value.TreeLeafAreaHectare;
          query3.SetInt32(Classifiers.Strata.ToString(), keyValuePair.Key);
          query3.SetInt32("EstimateType", 33).SetDouble("EstimateValue", num3 / 100.0 / num1 * tuple1.Item1).ExecuteUpdate();
          query3.SetInt32("EstimateType", 34).SetDouble("EstimateValue", num3 / 100.0 / num1 * tuple1.Item2).ExecuteUpdate();
          query3.SetInt32("EstimateType", 37).SetDouble("EstimateValue", num3 / 100.0 / num1 * tuple1.Item3).ExecuteUpdate();
          query3.SetInt32("EstimateType", 38).SetDouble("EstimateValue", num3 / 100.0 / num1 * tuple1.Item4).ExecuteUpdate();
          query3.SetInt32("EstimateType", 35).SetDouble("EstimateValue", num3 / 100.0 / num1 * tuple1.Item5).ExecuteUpdate();
          query3.SetInt32("EstimateType", 36).SetDouble("EstimateValue", num3 / 100.0 / num1 * tuple1.Item6).ExecuteUpdate();
          sasTransaction.IncreaseOperationNumber(6);
        }
        query3.SetInt32(Classifiers.Strata.ToString(), this.ClassValueForStrataStudyArea);
        query3.SetInt32(Classifiers.Species.ToString(), this.ClassValueForSpeciesAllSpecies);
        query3.SetInt32("EstimateType", 33).SetDouble("EstimateValue", tuple1.Item1).ExecuteUpdate();
        query3.SetInt32("EstimateType", 34).SetDouble("EstimateValue", tuple1.Item2).ExecuteUpdate();
        query3.SetInt32("EstimateType", 37).SetDouble("EstimateValue", tuple1.Item3).ExecuteUpdate();
        query3.SetInt32("EstimateType", 38).SetDouble("EstimateValue", tuple1.Item4).ExecuteUpdate();
        query3.SetInt32("EstimateType", 35).SetDouble("EstimateValue", tuple1.Item5).ExecuteUpdate();
        query3.SetInt32("EstimateType", 36).SetDouble("EstimateValue", tuple1.Item6).ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber(6);
        partitionClassifiers.Clear();
        partitionClassifiers.Add(Classifiers.Species);
        sasTransaction.Pause();
        string estimationTable3 = this.getEstimationTable(this.m_ps.InputSession, tableDataType, partitionClassifiers);
        sasTransaction.Resume();
        IList<(int, double, double)> tupleList2 = sasUtilProvider.GetSQLQuerySelectEstimateTable(estimationTable3, partitionClassifiers).SetGuid("YearGuid", this.currYear.Guid).SetInt32("EstimateType", 5).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit1).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, double, double)>()).List<(int, double, double)>();
        IQuery query4 = sasUtilProvider.GetSQLQueryInsertEstimateTable(estimationTable3, partitionClassifiers).SetGuid("vYearGuid", this.currYear.Guid).SetInt32("EquationType", 1).SetInt32("EstimateUnitsId", unit2).SetDouble("EstimateStandardError", 0.0);
        foreach ((int, double, double) tuple3 in (IEnumerable<(int, double, double)>) tupleList2)
        {
          if (tuple3.Item1 != this.ClassValueForSpeciesAllSpecies)
          {
            query4.SetInt32(Classifiers.Species.ToString(), tuple3.Item1);
            query4.SetInt32("EstimateType", 33).SetDouble("EstimateValue", tuple3.Item2 / num1 * tuple1.Item1).ExecuteUpdate();
            query4.SetInt32("EstimateType", 34).SetDouble("EstimateValue", tuple3.Item2 / num1 * tuple1.Item2).ExecuteUpdate();
            query4.SetInt32("EstimateType", 37).SetDouble("EstimateValue", tuple3.Item2 / num1 * tuple1.Item3).ExecuteUpdate();
            query4.SetInt32("EstimateType", 38).SetDouble("EstimateValue", tuple3.Item2 / num1 * tuple1.Item4).ExecuteUpdate();
            query4.SetInt32("EstimateType", 35).SetDouble("EstimateValue", tuple3.Item2 / num1 * tuple1.Item5).ExecuteUpdate();
            query4.SetInt32("EstimateType", 36).SetDouble("EstimateValue", tuple3.Item2 / num1 * tuple1.Item6).ExecuteUpdate();
            sasTransaction.IncreaseOperationNumber(6);
          }
        }
        query4.SetInt32(Classifiers.Species.ToString(), this.ClassValueForSpeciesAllSpecies);
        query4.SetInt32("EstimateType", 33).SetDouble("EstimateValue", tuple1.Item1).ExecuteUpdate();
        query4.SetInt32("EstimateType", 34).SetDouble("EstimateValue", tuple1.Item2).ExecuteUpdate();
        query4.SetInt32("EstimateType", 37).SetDouble("EstimateValue", tuple1.Item3).ExecuteUpdate();
        query4.SetInt32("EstimateType", 38).SetDouble("EstimateValue", tuple1.Item4).ExecuteUpdate();
        query4.SetInt32("EstimateType", 35).SetDouble("EstimateValue", tuple1.Item5).ExecuteUpdate();
        query4.SetInt32("EstimateType", 36).SetDouble("EstimateValue", tuple1.Item6).ExecuteUpdate();
        sasTransaction.IncreaseOperationNumber(6);
        if (tableDataType == EstimateDataTypes.Tree)
        {
          IQuery namedQuery = this.project_s.GetNamedQuery("updateHydroInIndividualTreeEffects");
          namedQuery.SetGuid("YearGuid", this.currYear.Guid);
          namedQuery.SetDouble("InterceptedPerM2", tuple1.Item1 / num1 / this.million);
          namedQuery.SetDouble("AvoidedRunoffPerM2", tuple1.Item2 / num1 / this.million);
          namedQuery.SetDouble("EvaporationPerM2", tuple1.Item4 / num1 / this.million);
          namedQuery.SetDouble("PotentialEvaporationPerM2", tuple1.Item3 / num1 / this.million);
          namedQuery.SetDouble("TranspirationPerM2", tuple1.Item6 / num1 / this.million);
          namedQuery.SetDouble("PotentialEvapotranspirationM2", tuple1.Item5 / num1 / this.million);
          namedQuery.ExecuteUpdate();
        }
        sasTransaction.End();
      }
      catch (Exception ex)
      {
        sasTransaction.Abort();
        throw;
      }
    }

    private void CombinedTreeShrubData2(
      List<Classifiers> classifierList,
      List<EstimateTypeEnum> estimateTypeList,
      List<int> estimateUnitList)
    {
      try
      {
        SASIUtilProvider sasUtilProvider = this.m_ps.InputSession.GetSASQuerySupplier(this.project_s).GetSASUtilProvider();
        string estimationTable1 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Tree, classifierList);
        string estimationTable2 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.Shrub, classifierList);
        string estimationTable3 = this.getEstimationTable(this.m_ps.InputSession, EstimateDataTypes.TreeShrubCombined, classifierList);
        SASTransaction sasTransaction = new SASTransaction();
        sasTransaction.MaxNumber = 500;
        sasTransaction.Begin(this.project_s);
        try
        {
          IQuery treeShrubEstimation1 = sasUtilProvider.GetSQLQueryCombineTreeShrubEstimation(estimationTable1, estimationTable2, estimationTable3, false, classifierList, estimateTypeList, estimateUnitList);
          sasTransaction.IncreaseOperationNumber(treeShrubEstimation1.SetGuid("YearGuid", this.currYear.Guid).ExecuteUpdate());
          IQuery treeShrubEstimation2 = sasUtilProvider.GetSQLQueryCombineTreeShrubEstimation(estimationTable1, estimationTable2, estimationTable3, true, classifierList, estimateTypeList, estimateUnitList);
          sasTransaction.IncreaseOperationNumber(treeShrubEstimation2.SetGuid("YearGuid", this.currYear.Guid).ExecuteUpdate());
          IQuery treeShrubEstimation3 = sasUtilProvider.GetSQLQueryCombineTreeShrubEstimation(estimationTable2, estimationTable1, estimationTable3, true, classifierList, estimateTypeList, estimateUnitList);
          sasTransaction.IncreaseOperationNumber(treeShrubEstimation3.SetGuid("YearGuid", this.currYear.Guid).ExecuteUpdate());
          sasTransaction.End();
        }
        catch (Exception ex)
        {
          sasTransaction.Abort();
          throw;
        }
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

    private void PopulateUFORE_DResultsForCombinedTreeShrub()
    {
      try
      {
        int unit1 = this.getUnit(this.project_s, Units.MetricTons, Units.None, Units.None);
        int unit2 = this.getUnit(this.project_s, Units.Kilograms, Units.None, Units.None);
        int unit3 = this.getUnit(this.project_s, Units.Monetaryunit, Units.None, Units.None);
        List<int> estimateUnitList = new List<int>();
        estimateUnitList.Add(unit1);
        estimateUnitList.Add(unit2);
        estimateUnitList.Add(unit3);
        List<EstimateTypeEnum> estimateTypeList = new List<EstimateTypeEnum>();
        estimateTypeList.Add(EstimateTypeEnum.PollutionRemoval);
        List<Classifiers> classifierList = new List<Classifiers>();
        classifierList.Add(Classifiers.Month);
        classifierList.Add(Classifiers.Pollutant);
        this.CombinedTreeShrubData2(classifierList, estimateTypeList, estimateUnitList);
        classifierList.Clear();
        classifierList.Add(Classifiers.Strata);
        classifierList.Add(Classifiers.Pollutant);
        this.CombinedTreeShrubData2(classifierList, estimateTypeList, estimateUnitList);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public void getDensityCoverPercentRatio()
    {
      string path2 = "";
      try
      {
        if (this.dictStrataNameToDensityRatio.Count > 0)
          return;
        double num1 = 0.0;
        SASDictionary<string, double> sasDictionary1 = new SASDictionary<string, double>("Strata Description");
        SASDictionary<int, string> sasDictionary2 = new SASDictionary<int, string>("Strata ID");
        path2 = "tot.txt";
        using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
        {
          LineValues lineValues = new LineValues();
          lineValues.separater = ' ';
          int num2 = 0;
          string str1;
          while ((str1 = streamReader.ReadLine()) != null)
          {
            string aLine = str1.Trim();
            if (aLine != "")
            {
              lineValues.SetLine(aLine);
              if (lineValues.ElementAt(1).Trim().ToUpper(this.ciUsed) == "CODE" && lineValues.ElementAt(lineValues.Count).Trim().ToUpper(this.ciUsed) == "AREA")
              {
                string str2 = lineValues.ElementAt(1);
                for (int index = 2; lineValues.ElementAt(index).Length == 0; ++index)
                  str2 += " ";
                num2 = str2.Length + 1;
                break;
              }
            }
          }
          string str3;
          while ((str3 = streamReader.ReadLine()) != null)
          {
            string aLine1 = str3.Trim();
            if (aLine1.Length != 0)
            {
              lineValues.SetLine(aLine1);
              string s = aLine1.Substring(0, num2).Trim();
              string aLine2 = aLine1.Substring(num2).Trim();
              lineValues.SetLine(aLine2);
              string str4 = lineValues.ElementAt(1);
              for (int index = 2; index < lineValues.Count; ++index)
                str4 = str4 + " " + lineValues.ElementAt(index);
              string key = str4.Trim();
              sasDictionary1.Add(key, double.Parse(lineValues.ElementAt(lineValues.Count), this.nsFloat, (IFormatProvider) this.ciUsed));
              sasDictionary2.Add(int.Parse(s), key);
              num1 += double.Parse(lineValues.ElementAt(lineValues.Count), this.nsFloat, (IFormatProvider) this.ciUsed);
            }
          }
          streamReader.Close();
        }
        double num3 = 0.0;
        SASDictionary<int, double> sasDictionary3 = new SASDictionary<int, double>("Strata ID");
        if (this.currSeries.SampleType == SampleType.Inventory && this.currYear.RecordStrata)
        {
          Plot plot = (Plot) null;
          Strata strata = (Strata) null;
          IList<(int, int)> tupleList = this.project_s.QueryOver<Plot>((System.Linq.Expressions.Expression<Func<Plot>>) (() => plot)).JoinQueryOver<Strata>((System.Linq.Expressions.Expression<Func<Plot, Strata>>) (p => p.Strata), (System.Linq.Expressions.Expression<Func<Strata>>) (() => strata)).Where((System.Linq.Expressions.Expression<Func<bool>>) (() => plot.Year == this.currYear && plot.IsComplete == true)).Select((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plot.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) strata.Id))).OrderBy((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plot.Id))).Asc.TransformUsing((IResultTransformer) new TupleResultTransformer<(int, int)>()).List<(int, int)>();
          SASDictionary<string, int> sasDictionary4 = new SASDictionary<string, int>("Plot ID");
          foreach ((int, int) tuple in (IEnumerable<(int, int)>) tupleList)
            sasDictionary4.Add(tuple.Item1.ToString((IFormatProvider) this.ciUsed), tuple.Item2);
          path2 = "PLOTINV.csv";
          using (StreamReader streamReader = new StreamReader(Path.Combine(this.outputFolder, path2)))
          {
            LineValues lineValues = new LineValues();
            lineValues.separater = ',';
            string str;
            while ((str = streamReader.ReadLine()) != null)
            {
              string aLine = str.Trim();
              if (aLine != "")
              {
                lineValues.SetLine(aLine);
                if (lineValues.Count > 5 && lineValues.ElementAt(1).Trim() != "" && double.TryParse(lineValues.ElementAt(1), this.nsFloat, (IFormatProvider) this.ciUsed, out double _))
                {
                  string key1 = lineValues.ElementAt(1).Trim();
                  int key2 = sasDictionary4[key1];
                  num3 += this.TreatDotBlank(lineValues.ElementAt(6).Trim());
                  if (sasDictionary3.ContainsKey(key2))
                    sasDictionary3[key2] += this.TreatDotBlank(lineValues.ElementAt(6).Trim());
                  else
                    sasDictionary3.Add(key2, this.TreatDotBlank(lineValues.ElementAt(6).Trim()));
                }
              }
            }
            streamReader.Close();
          }
        }
        path2 = "";
        IList<Strata> strataList = this.project_s.QueryOver<Strata>().Where((System.Linq.Expressions.Expression<Func<Strata, bool>>) (s => s.Year == this.currYear)).List<Strata>();
        double num4 = 0.0;
        foreach (Strata strata in (IEnumerable<Strata>) strataList)
        {
          if (this.currYear.RecordStrata)
          {
            if (sasDictionary2.ContainsKey(strata.Id))
            {
              if (sasDictionary1.ContainsKey(sasDictionary2[strata.Id]))
              {
                this.dictStrataNameToDensityRatio.Add(sasDictionary2[strata.Id], sasDictionary1[sasDictionary2[strata.Id]] / (double) strata.Size);
                num4 += (double) strata.Size;
              }
              else
                this.dictStrataNameToDensityRatio.Add(sasDictionary2[strata.Id], 1.0);
              if (sasDictionary3.ContainsKey(strata.Id))
              {
                if (this.currSeries.SampleType == SampleType.Inventory)
                {
                  double num5 = this.currYear.Unit != YearUnit.English ? (double) strata.Size * this.tenThousand : (double) strata.Size * this.tenThousand * 0.404686;
                  this.dictStrataNameToCoverPercentRatio.Add(sasDictionary2[strata.Id], sasDictionary3[strata.Id] / num5);
                }
                else
                  this.dictStrataNameToCoverPercentRatio.Add(sasDictionary2[strata.Id], 1.0);
              }
              else
                this.dictStrataNameToCoverPercentRatio.Add(sasDictionary2[strata.Id], 1.0);
            }
          }
          else
          {
            this.dictStrataNameToDensityRatio.Add(sasDictionary2[strata.Id], 1.0);
            this.dictStrataNameToCoverPercentRatio.Add(sasDictionary2[strata.Id], 1.0);
          }
        }
        if (this.currYear.RecordStrata)
        {
          this.dictStrataNameToDensityRatio.Add("CITY TOTAL", num1 / num4);
          if (this.currSeries.SampleType == SampleType.Inventory)
          {
            double num6 = this.currYear.Unit != YearUnit.English ? num4 * this.tenThousand : num4 * this.tenThousand * 0.404686;
            this.dictStrataNameToCoverPercentRatio.Add("CITY TOTAL", num3 / num6);
          }
          else
            this.dictStrataNameToCoverPercentRatio.Add("CITY TOTAL", 1.0);
          this.TotalStudyAreaHectare = this.currYear.Unit == YearUnit.Metric ? num4 : num4 * 0.404686;
        }
        else
        {
          this.dictStrataNameToDensityRatio.Add("CITY TOTAL", 1.0);
          this.dictStrataNameToCoverPercentRatio.Add("CITY TOTAL", 1.0);
          this.TotalStudyAreaHectare = num3 / this.tenThousand;
        }
      }
      catch (Exception ex)
      {
        if (path2 == "")
          throw;
        else
          throw new Exception("Processing file " + path2 + ": " + Environment.NewLine + ex.Message);
      }
    }

    private void PopulatingUFORE_BResultsForCombinedTreeShrub()
    {
      try
      {
        List<int> estimateUnitList = new List<int>();
        estimateUnitList.Add(this.getUnit(this.project_s, Units.Kilograms, Units.None, Units.Year));
        estimateUnitList.Add(this.getUnit(this.project_s, Units.MetricTons, Units.None, Units.Year));
        List<EstimateTypeEnum> estimateTypeList = new List<EstimateTypeEnum>();
        estimateTypeList.Add(EstimateTypeEnum.VOCEmissions);
        List<Classifiers> classifierList = new List<Classifiers>();
        classifierList.Clear();
        classifierList.Add(Classifiers.Strata);
        classifierList.Add(Classifiers.VOCs);
        this.CombinedTreeShrubData2(classifierList, estimateTypeList, estimateUnitList);
        classifierList.Clear();
        classifierList.Add(Classifiers.Strata);
        classifierList.Add(Classifiers.Species);
        classifierList.Add(Classifiers.VOCs);
        this.CombinedTreeShrubData2(classifierList, estimateTypeList, estimateUnitList);
        classifierList.Clear();
        classifierList.Add(Classifiers.Species);
        classifierList.Add(Classifiers.VOCs);
        this.CombinedTreeShrubData2(classifierList, estimateTypeList, estimateUnitList);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void PopulatingWaterInterceptionForCombiedTreeShrub()
    {
      try
      {
        List<int> estimateUnitList = new List<int>();
        estimateUnitList.Add(this.getUnit(this.project_s, Units.CubicMeter, Units.None, Units.Year));
        List<EstimateTypeEnum> estimateTypeList = new List<EstimateTypeEnum>();
        estimateTypeList.Add(EstimateTypeEnum.WaterInterception);
        estimateTypeList.Add(EstimateTypeEnum.AvoidedRunoff);
        estimateTypeList.Add(EstimateTypeEnum.PotentialEvapotranspiration);
        estimateTypeList.Add(EstimateTypeEnum.Transpiration);
        estimateTypeList.Add(EstimateTypeEnum.PotentialEvaporation);
        estimateTypeList.Add(EstimateTypeEnum.Evaporation);
        List<Classifiers> classifierList = new List<Classifiers>();
        classifierList.Add(Classifiers.Strata);
        this.CombinedTreeShrubData2(classifierList, estimateTypeList, estimateUnitList);
        classifierList.Clear();
        classifierList.Add(Classifiers.Strata);
        classifierList.Add(Classifiers.Species);
        this.CombinedTreeShrubData2(classifierList, estimateTypeList, estimateUnitList);
        classifierList.Clear();
        classifierList.Add(Classifiers.Species);
        this.CombinedTreeShrubData2(classifierList, estimateTypeList, estimateUnitList);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void checkSpeciesIntegrityInOtherFiles()
    {
      using (StreamReader streamReader1 = new StreamReader(Path.Combine(this.outputFolder, "CITYSUM.CSV")))
      {
        using (StreamReader streamReader2 = new StreamReader(Path.Combine(this.outputFolder, "SHRUBEST.CSV")))
        {
          using (StreamReader streamReader3 = new StreamReader(Path.Combine(this.outputFolder, "PLOTINV.csv")))
          {
            LineValues lineValues1 = new LineValues();
            double result1 = 0.0;
            string str1;
            while ((str1 = streamReader1.ReadLine()) != null)
            {
              string aLine = str1.Trim();
              if (aLine.Length > 0)
              {
                lineValues1.SetLine(aLine);
                bool flag;
                if (lineValues1.Count >= 3)
                {
                  string s = lineValues1.ElementAt(2).Trim();
                  flag = s == "." || double.TryParse(s, this.nsFloat, (IFormatProvider) this.ciUsed, out result1);
                }
                else
                  flag = false;
                if (flag)
                {
                  string key = lineValues1.ElementAt(1).Trim();
                  if (key != "" && key.ToUpper(this.ciUsed).Trim() != "TOTAL" && !this.dictSpeciesCodeToClassValue.ContainsKey(key))
                  {
                    streamReader1.Close();
                    streamReader2.Close();
                    streamReader3.Close();
                    throw new Exception("Species '" + key + "' from CITYSUM.csv does not exist in SpeciesList.csv");
                  }
                }
              }
            }
            streamReader1.Close();
            if (this.currYear.RecordShrub && this.currSeries.SampleType != SampleType.Inventory)
            {
              string str2;
              while ((str2 = streamReader2.ReadLine()) != null)
              {
                string aLine = str2.Trim();
                if (aLine.Length > 0)
                {
                  lineValues1.SetLine(aLine);
                  bool flag;
                  if (lineValues1.Count >= 3)
                  {
                    string s = lineValues1.ElementAt(3).Trim();
                    flag = s == "." || double.TryParse(s, this.nsFloat, (IFormatProvider) this.ciUsed, out result1);
                  }
                  else
                    flag = false;
                  if (flag)
                  {
                    string key = lineValues1.ElementAt(2).Trim();
                    if (key != "" && key.ToUpper(this.ciUsed).Trim() != "TOTAL" && key.ToUpper(this.ciUsed).Trim() != "NONE." && key.ToUpper(this.ciUsed).Trim() != "NONE" && !this.dictSpeciesCodeToClassValue.ContainsKey(key))
                    {
                      streamReader2.Close();
                      streamReader3.Close();
                      throw new Exception("Species '" + key + "' from SHRUBEST.csv does not exist in SpeciesList.csv");
                    }
                  }
                }
              }
            }
            streamReader2.Close();
            double result2 = 0.0;
            LineValues lineValues2 = new LineValues();
            new LineValues().separater = ' ';
            string aLine1;
            while ((aLine1 = streamReader3.ReadLine()) != null)
            {
              if (aLine1.Trim() != "")
              {
                lineValues2.SetLine(aLine1);
                if (lineValues2.Count > 5 && lineValues2.ElementAt(1).Trim().Length != 0 && double.TryParse(lineValues2.ElementAt(1), this.nsFloat, (IFormatProvider) this.ciUsed, out result2))
                {
                  string key = lineValues2.ElementAt(3).Trim();
                  if (!this.dictGenusCodePlusSpeciesCodeToSpeciesCode.ContainsKey(key))
                    throw new Exception("Species '" + key + "' from PLOTINV.csv does not exist in SpeciesList.csv");
                }
              }
            }
            streamReader3.Close();
          }
        }
      }
    }

    public void Dispose()
    {
      try
      {
        if (this.project_s.IsOpen)
          this.project_s.Close();
      }
      catch (Exception ex)
      {
      }
      if (!Directory.Exists(this.outputFolder))
        return;
      try
      {
        Directory.Delete(this.outputFolder, true);
      }
      catch (Exception ex)
      {
      }
    }

    private string ToASCII(string s) => string.Join<char>("", s.Normalize(NormalizationForm.FormD).Where<char>((Func<char, bool>) (c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)));

    private string ToUSCharacters(string s)
    {
      string usCharacters = "";
      for (int index = 0; index < s.Length; ++index)
      {
        char ch = s.ElementAt<char>(index);
        switch (ch)
        {
          case '.':
          case '0':
          case '1':
          case '2':
          case '3':
          case '4':
          case '5':
          case '6':
          case '7':
          case '8':
          case '9':
          case 'A':
          case 'B':
          case 'C':
          case 'D':
          case 'E':
          case 'F':
          case 'G':
          case 'H':
          case 'I':
          case 'J':
          case 'K':
          case 'L':
          case 'M':
          case 'N':
          case 'O':
          case 'P':
          case 'Q':
          case 'R':
          case 'S':
          case 'T':
          case 'U':
          case 'V':
          case 'W':
          case 'X':
          case 'Y':
          case 'Z':
          case '_':
          case 'a':
          case 'b':
          case 'c':
          case 'd':
          case 'e':
          case 'f':
          case 'g':
          case 'h':
          case 'i':
          case 'j':
          case 'k':
          case 'l':
          case 'm':
          case 'n':
          case 'o':
          case 'p':
          case 'q':
          case 'r':
          case 's':
          case 't':
          case 'u':
          case 'v':
          case 'w':
          case 'x':
          case 'y':
          case 'z':
            usCharacters += ch.ToString();
            break;
          default:
            usCharacters += "_";
            break;
        }
      }
      return usCharacters;
    }

    private List<string> checkSpecies(IList<string> speciesCodes)
    {
      List<string> stringList = new List<string>();
      foreach (string speciesCode in (IEnumerable<string>) speciesCodes)
      {
        if (!this.m_ps.Species.ContainsKey(speciesCode))
          stringList.Add(speciesCode);
      }
      return stringList;
    }

    private bool checkWeatherStationFromServer(int weatherYear, string weatherStationID)
    {
      WeatherStationValidator stationValidator = new WeatherStationValidator();
      int error = stationValidator.Validate(weatherYear, weatherStationID);
      if (error == 2)
        throw new SASException(stationValidator.TranslateError(error));
      return error == 0;
    }

    public string getEstimationTable(
      InputSession mInput,
      EstimateDataTypes TableData,
      List<Classifiers> partitionClassifiers)
    {
      using (ISession session = mInput.CreateSession())
      {
        try
        {
          int count = partitionClassifiers.Count;
          int partitionClassifier1 = (int) partitionClassifiers[0];
          int partitionClassifier2 = (int) partitionClassifiers[0];
          int val1 = 0;
          for (int index = 0; index < count; ++index)
          {
            val1 += (int) partitionClassifiers[index];
            if (partitionClassifiers[index] < (Classifiers) partitionClassifier1)
              partitionClassifier1 = (int) partitionClassifiers[index];
            if (partitionClassifiers[index] > (Classifiers) partitionClassifier2)
              partitionClassifier2 = (int) partitionClassifiers[index];
          }
          foreach ((int, string) tuple in (IEnumerable<(int, string)>) session.GetNamedQuery("selectEstimateTableWithRestriction").SetInt32("dataType", (int) TableData).SetInt32("classifierNo", count).SetInt32("maxClassifier", partitionClassifier2).SetInt32("minClassifier", partitionClassifier1).SetInt32("sumClassifier", val1).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, string)>()).List<(int, string)>())
          {
            IList<int> intList = session.GetNamedQuery("selectTablePartitions").SetInt32("tableId", tuple.Item1).List<int>();
            bool flag = true;
            for (int index = 0; index < partitionClassifiers.Count; ++index)
            {
              if (partitionClassifiers[index] != (Classifiers) intList[index])
                flag = false;
            }
            if (flag)
              return tuple.Item2;
          }
          string str1 = "";
          string str2 = "";
          foreach (Classifiers partitionClassifier3 in partitionClassifiers)
          {
            str2 = str2 + partitionClassifier3.ToString() + ",";
            str1 = str1 + "_" + partitionClassifier3.ToString().Replace(" ", "_");
          }
          string estimationTable = str1.ToUpper() + ((int) TableData).ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string val2 = str2.Substring(0, str2.Length - 1);
          SASIUtilProvider sasUtilProvider = mInput.GetSASQuerySupplier(session).GetSASUtilProvider();
          IQuery sqlQueryDropTable = sasUtilProvider.GetSQLQueryDropTable(estimationTable);
          try
          {
            sqlQueryDropTable.ExecuteUpdate();
          }
          catch (Exception ex)
          {
          }
          sasUtilProvider.GetSQLQueryCreateEstimateTable(estimationTable, partitionClassifiers).ExecuteUpdate();
          int val3 = session.GetNamedQuery("selectBiggestStatisticalTableId").List<int>().SingleOrDefault<int>() + 1;
          session.GetNamedQuery("insertTableOfStatisticalEstimates").SetInt32("entryId", val3).SetInt32("statisticalTableID", val3).SetString("statisticalTableName", estimationTable).SetInt32("dataType", (int) TableData).SetString("statisticalTableDescription", val2).SetInt32("tableFlag", 0).ExecuteUpdate();
          for (int index = 0; index < partitionClassifiers.Count; ++index)
            session.GetNamedQuery("insertPartitionDefinitionsTable").SetInt32("statisticalTableID", val3).SetInt32("classifierId", (int) partitionClassifiers[index]).SetInt32("partitionOrder", index + 1).SetString("partitionName", partitionClassifiers[index].ToString()).SetInt32("partitionFlag", 0).ExecuteUpdate();
          return estimationTable;
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    public int getUnit(ISession s, Units PrimaryUnit, Units SecondaryUnit, Units TertiaryUnit)
    {
      try
      {
        int unit = s.GetNamedQuery("selectEstimateUnit").SetInt32("primaryUnitId", (int) PrimaryUnit).SetInt32("secondaryUnitId", (int) SecondaryUnit).SetInt32("tertiaryUnitId", (int) TertiaryUnit).List<int>().SingleOrDefault<int>();
        if (unit != 0)
          return unit;
        int val = s.GetNamedQuery("selectBiggestEstimateUnitId").List<int>().SingleOrDefault<int>() + 1;
        s.GetNamedQuery("insertEstimationUnitsTable").SetInt32("estimateUnit", val).SetInt32("primaryUnit", (int) PrimaryUnit).SetInt32("secondaryUnit", (int) SecondaryUnit).SetInt32("tertiaryUnit", (int) TertiaryUnit).SetInt32("estimationUnitsFlag", 0).ExecuteUpdate();
        return val;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public void setupClassifiers(InputSession mInput)
    {
      using (ISession session = mInput.CreateSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          try
          {
            IList<(int, string)> tupleList1 = session.GetNamedQuery("selectNoPartitionStatisticaEstimatelTables").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, string)>()).List<(int, string)>();
            Dictionary<int, string> dictionary1 = new Dictionary<int, string>();
            foreach ((int, string) tuple in (IEnumerable<(int, string)>) tupleList1)
              dictionary1.Add(tuple.Item1, tuple.Item2);
            IList<(int, string)> tupleList2 = session.GetNamedQuery("GetClassifierNames").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, string)>()).List<(int, string)>();
            Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
            foreach ((int, string) tuple in (IEnumerable<(int, string)>) tupleList2)
              dictionary2.Add(tuple.Item1, tuple.Item2);
            foreach (Classifiers classifiers in Enum.GetValues(typeof (Classifiers)))
            {
              if (dictionary2.ContainsKey((int) classifiers))
              {
                if (classifiers.ToString().ToLower() != dictionary2[(int) classifiers].ToLower())
                {
                  session.GetNamedQuery("updateClassifierAbbreviation").SetString("abbr", classifiers.ToString()).SetInt32("classifierId", (int) classifiers).ExecuteUpdate();
                  IQuery namedQuery = session.GetNamedQuery("selectStatisticaEstimatelTablesByClassifierId");
                  namedQuery.SetInt32("classifierId", (int) classifiers);
                  foreach ((int, string) tuple in (IEnumerable<(int, string)>) namedQuery.SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, string)>()).List<(int, string)>())
                  {
                    if (!dictionary1.ContainsKey(tuple.Item1))
                      dictionary1.Add(tuple.Item1, tuple.Item2);
                  }
                }
              }
              else
              {
                IQuery namedQuery1 = session.GetNamedQuery("insertClassifiers");
                namedQuery1.SetInt32("classifierId", (int) classifiers).SetString("classAbbr", classifiers.ToString());
                switch (classifiers)
                {
                  case Classifiers.None:
                    namedQuery1.SetString("classDesc", "None");
                    break;
                  case Classifiers.DBH:
                    namedQuery1.SetString("classDesc", "DBH Class");
                    break;
                  case Classifiers.Dieback:
                    namedQuery1.SetString("classDesc", "Dieback Class");
                    break;
                  case Classifiers.Strata:
                    namedQuery1.SetString("classDesc", "Strata");
                    break;
                  case Classifiers.Species:
                    namedQuery1.SetString("classDesc", "The species of the tree/shrub");
                    break;
                  case Classifiers.Height:
                    namedQuery1.SetString("classDesc", "Height Class");
                    break;
                  case Classifiers.Continent:
                    namedQuery1.SetString("classDesc", "Continent of origin of species");
                    break;
                  case Classifiers.GroundCover:
                    namedQuery1.SetString("classDesc", "The type of ground covers being sampled");
                    break;
                  case Classifiers.Geopoliticalunit:
                    namedQuery1.SetString("classDesc", "Native within geopolitical unit");
                    break;
                  case Classifiers.PrimaryIndex:
                    namedQuery1.SetString("classDesc", "Pollen Index");
                    break;
                  case Classifiers.ALB:
                    namedQuery1.SetString("classDesc", "Asian longhorned beetle susceptibility index");
                    break;
                  case Classifiers.GM:
                    namedQuery1.SetString("classDesc", "Gypsy moth susceptibility index");
                    break;
                  case Classifiers.Landuse:
                    namedQuery1.SetString("classDesc", "Actual Field Landuse");
                    break;
                  case Classifiers.EnergyUse:
                    namedQuery1.SetString("classDesc", "Energy Use");
                    break;
                  case Classifiers.EnergyType:
                    namedQuery1.SetString("classDesc", "Energy Type");
                    break;
                  case Classifiers.Pollutant:
                    namedQuery1.SetString("classDesc", "Air-Born Pollutant");
                    break;
                  case Classifiers.VOCs:
                    namedQuery1.SetString("classDesc", "Volatile Organic Compound");
                    break;
                  case Classifiers.EnergySource:
                    namedQuery1.SetString("classDesc", "Energy Source");
                    break;
                  case Classifiers.Hour:
                    namedQuery1.SetString("classDesc", "Hour");
                    break;
                  case Classifiers.Month:
                    namedQuery1.SetString("classDesc", "Month");
                    break;
                  case Classifiers.EAB:
                    namedQuery1.SetString("classDesc", "Emerald Ash Borer");
                    break;
                  case Classifiers.DED:
                    namedQuery1.SetString("classDesc", "Dutch Elm Disease");
                    break;
                  case Classifiers.TSDieback:
                    namedQuery1.SetString("classDesc", "Tree Stress Dieback");
                    break;
                  case Classifiers.TSEpiSprout:
                    namedQuery1.SetString("classDesc", "Tree Stress Epicormic Sprouts");
                    break;
                  case Classifiers.TSWiltFoli:
                    namedQuery1.SetString("classDesc", "Tree Stree Wilted Foliage");
                    break;
                  case Classifiers.TSEnvStress:
                    namedQuery1.SetString("classDesc", "Tree Stress Envionmental Stress");
                    break;
                  case Classifiers.TSHumStress:
                    namedQuery1.SetString("classDesc", "Tree Stress Human Stress");
                    break;
                  case Classifiers.FTChewFoli:
                    namedQuery1.SetString("classDesc", "Foliage/Twigs Chewed Foliage");
                    break;
                  case Classifiers.FTDiscFoli:
                    namedQuery1.SetString("classDesc", "Foliage/Twigs Discolored Foliage");
                    break;
                  case Classifiers.FTAbnFoli:
                    namedQuery1.SetString("classDesc", "Foliage/Twigs Abnormal Foliage");
                    break;
                  case Classifiers.FTInsectSigns:
                    namedQuery1.SetString("classDesc", "Foliage/Twigs Insect Sign");
                    break;
                  case Classifiers.FTFoliAffect:
                    namedQuery1.SetString("classDesc", "Foliage/Twigs Foliage Affected");
                    break;
                  case Classifiers.BBInsectSigns:
                    namedQuery1.SetString("classDesc", "Branches/Bole Signs of Insects");
                    break;
                  case Classifiers.BBInsectPres:
                    namedQuery1.SetString("classDesc", "Branches/Bole Insect Presence");
                    break;
                  case Classifiers.BBDiseaseSigns:
                    namedQuery1.SetString("classDesc", "Branches/Bole Signs of Disease");
                    break;
                  case Classifiers.BBAbnGrowth:
                    namedQuery1.SetString("classDesc", "Branches/Bole Abnormal Growth");
                    break;
                  case Classifiers.BBProbLoc:
                    namedQuery1.SetString("classDesc", "Branches/Bole Probable Location");
                    break;
                  case Classifiers.PestPest:
                    namedQuery1.SetString("classDesc", "Primary Pest");
                    break;
                  case Classifiers.PestIndicator:
                    namedQuery1.SetString("classDesc", "Pest Indicator");
                    break;
                  case Classifiers.CityManaged:
                    namedQuery1.SetString("classDesc", "Private or public tree");
                    break;
                  case Classifiers.StreetTree:
                    namedQuery1.SetString("classDesc", "Stree tree or not");
                    break;
                  case Classifiers.Living:
                    namedQuery1.SetString("classDesc", "Living trees");
                    break;
                  case Classifiers.CDBH:
                    namedQuery1.SetString("classDesc", "Customized DBH");
                    break;
                  case Classifiers.CDieback:
                    namedQuery1.SetString("classDesc", "Health Class");
                    break;
                  default:
                    throw new Exception(classifiers.ToString() + " need to be added in setupClassifiers.");
                }
                namedQuery1.SetInt32("classFlag", 0);
                namedQuery1.ExecuteUpdate();
                if (dictionary2.Count > 0)
                {
                  IQuery namedQuery2 = session.GetNamedQuery("selectStatisticaEstimatelTablesByClassifierId");
                  namedQuery2.SetInt32("classifierId", (int) classifiers);
                  foreach ((int, string) tuple in (IEnumerable<(int, string)>) namedQuery2.SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, string)>()).List<(int, string)>())
                  {
                    if (!dictionary1.ContainsKey(tuple.Item1))
                      dictionary1.Add(tuple.Item1, tuple.Item2);
                  }
                }
              }
            }
            if (dictionary1.Count > 0)
            {
              SASIUtilProvider sasUtilProvider = mInput.GetSASQuerySupplier(session).GetSASUtilProvider();
              foreach (int key in dictionary1.Keys)
              {
                try
                {
                  sasUtilProvider.GetSQLQueryDropTable(dictionary1[key]).ExecuteUpdate();
                }
                catch
                {
                }
                session.GetNamedQuery("deletePartitionDefinitionsTable").SetInt32("statisticalTableId", key).ExecuteUpdate();
                session.GetNamedQuery("deleteTableOfStatisticalEstimates").SetInt32("statisticalTableId", key).ExecuteUpdate();
              }
            }
            transaction.Commit();
          }
          catch (Exception ex)
          {
            transaction.Rollback();
            throw;
          }
        }
      }
    }

    public void setupEstimateType(InputSession mInput)
    {
      using (ISession session = mInput.CreateSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
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
            foreach ((int, string) tuple in (IEnumerable<(int, string)>) session.GetNamedQuery("selectEstimationTypeTable").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, string)>()).List<(int, string)>())
              dictionary2.Add(tuple.Item1, tuple.Item2);
            foreach (EstimateTypeEnum estimateTypeEnum in Enum.GetValues(typeof (EstimateTypeEnum)))
            {
              if (dictionary2.ContainsKey((int) estimateTypeEnum))
              {
                if (dictionary1[(int) estimateTypeEnum].ToLower() != dictionary2[(int) estimateTypeEnum].ToLower())
                  session.GetNamedQuery("updateEstimationTypeTable").SetString("quantityEstimated", dictionary1[(int) estimateTypeEnum]).SetInt32("estimateTypeId", (int) estimateTypeEnum).ExecuteUpdate();
              }
              else
                session.GetNamedQuery("insertEstimationTypeTable").SetInt32("estimateTypeId", (int) estimateTypeEnum).SetString("quantityEstimated", dictionary1[(int) estimateTypeEnum]).SetInt32("estimatedTypeFlag", 0).ExecuteUpdate();
            }
            transaction.Commit();
          }
          catch (Exception ex)
          {
            transaction.Rollback();
            throw;
          }
        }
      }
    }

    public void setupEquationType(InputSession mInput)
    {
      using (ISession session = mInput.CreateSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          try
          {
            Dictionary<int, string> dictionary1 = new Dictionary<int, string>();
            Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
            dictionary1.Add(1, "None");
            dictionary2.Add(1, "None");
            dictionary1.Add(2, "Percent");
            dictionary2.Add(2, "Percent");
            dictionary1.Add(5, "Sample mean");
            dictionary2.Add(5, "Sample mean");
            dictionary1.Add(7, "Sample Covariance");
            dictionary2.Add(7, "Sample Covariance");
            dictionary1.Add(8, "Stratum sample mean estim");
            dictionary2.Add(8, "Stratum sample mean estim");
            dictionary1.Add(10, "Per area stratum mean est");
            dictionary2.Add(10, "Per area stratum mean estimator");
            dictionary1.Add(12, "Percent stratum mean est");
            dictionary2.Add(12, "Percent stratum sample mean estimator");
            dictionary1.Add(14, "Total stratum estimator");
            dictionary2.Add(14, "Total stratum estimator");
            dictionary1.Add(16, "Ratio stratum estimator");
            dictionary2.Add(16, "Ratio stratum estimator");
            dictionary1.Add(18, "Strata sample mean estima");
            dictionary2.Add(18, "Strata sample mean estimator");
            dictionary1.Add(20, "Per area strata sample es");
            dictionary2.Add(20, "Per area strata sample mean estimator");
            dictionary1.Add(22, "Percent strata mean estim");
            dictionary2.Add(22, "Percent strata sample mean estimator");
            dictionary1.Add(24, "Total strata estimator");
            dictionary2.Add(24, "Total strata estimator");
            dictionary1.Add(26, "Ratio strata estimator");
            dictionary2.Add(26, "Ratio strata estimator");
            dictionary1.Add(50, "Shannon-Wiener diversity");
            dictionary2.Add(50, "Shannon-Wiener diversity");
            dictionary1.Add(51, "Menhinick's Diversity Ind");
            dictionary2.Add(51, "Menhinick's Diversity Ind");
            dictionary1.Add(52, "Simpson's Diversity index");
            dictionary2.Add(52, "Simpson's Diversity index");
            dictionary1.Add(53, "Sahnnon-Wiener Evenness I");
            dictionary2.Add(53, "Sahnnon-Wiener Evenness I");
            dictionary1.Add(54, "Sanders' Rarefraction Tec");
            dictionary2.Add(54, "Sanders' Rarefraction Tec");
            dictionary1.Add(55, "SpeciesRichness");
            dictionary2.Add(55, "SpeciesRichness");
            Dictionary<int, string> dictionary3 = new Dictionary<int, string>();
            Dictionary<int, string> dictionary4 = new Dictionary<int, string>();
            foreach ((int, string, string) tuple in (IEnumerable<(int, string, string)>) session.GetNamedQuery("selectEquationTypeTable").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, string, string)>()).List<(int, string, string)>())
            {
              dictionary3.Add(tuple.Item1, tuple.Item2);
              dictionary4.Add(tuple.Item1, tuple.Item3);
            }
            foreach (EquationTypes equationTypes in Enum.GetValues(typeof (EquationTypes)))
            {
              if (dictionary3.ContainsKey((int) equationTypes))
              {
                if (dictionary1[(int) equationTypes].ToLower() != dictionary3[(int) equationTypes].ToLower() || dictionary2[(int) equationTypes].ToLower() != dictionary4[(int) equationTypes].ToLower())
                  session.GetNamedQuery("updateEquationTypeTable").SetString("equationTypeName", dictionary1[(int) equationTypes]).SetString("equationTypeDescription", dictionary2[(int) equationTypes]).SetInt32("equationTypeId", (int) equationTypes).ExecuteUpdate();
              }
              else
                session.GetNamedQuery("insertEquationTypeTable").SetInt32("equationTypeId", (int) equationTypes).SetString("equationTypeName", dictionary1[(int) equationTypes]).SetString("equationTypeDescription", dictionary2[(int) equationTypes]).SetInt32("equationTypeFlag", 0).ExecuteUpdate();
            }
            transaction.Commit();
          }
          catch (Exception ex)
          {
            transaction.Rollback();
            throw;
          }
        }
      }
    }

    public void setupUnits(InputSession mInput)
    {
      using (ISession session = mInput.CreateSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          try
          {
            Dictionary<int, string> dictionary1 = new Dictionary<int, string>();
            Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
            dictionary1.Add(1, "None");
            dictionary2.Add(1, "None");
            dictionary1.Add(2, "No.");
            dictionary2.Add(2, "Count");
            dictionary1.Add(3, "%");
            dictionary2.Add(3, "Percent");
            dictionary1.Add(4, "(none)");
            dictionary2.Add(4, "Boolean");
            dictionary1.Add(5, "in");
            dictionary2.Add(5, "Inches");
            dictionary1.Add(6, "ft");
            dictionary2.Add(6, "Feet");
            dictionary1.Add(7, "mi");
            dictionary2.Add(7, "Miles");
            dictionary1.Add(8, "in2");
            dictionary2.Add(8, "Square inch");
            dictionary1.Add(9, "ft2");
            dictionary2.Add(9, "Square feet");
            dictionary1.Add(10, "mi2");
            dictionary2.Add(10, "Square miles");
            dictionary1.Add(11, "cm");
            dictionary2.Add(11, "Centimeters");
            dictionary1.Add(12, "m");
            dictionary2.Add(12, "Meters");
            dictionary1.Add(13, "km");
            dictionary2.Add(13, "Kilometer");
            dictionary1.Add(14, "cm2");
            dictionary2.Add(14, "Square centimeter");
            dictionary1.Add(15, "m2");
            dictionary2.Add(15, "Square meter");
            dictionary1.Add(16, "km2");
            dictionary2.Add(16, "Square kilometer");
            dictionary1.Add(17, "oz");
            dictionary2.Add(17, "Ounces");
            dictionary1.Add(18, "lbs");
            dictionary2.Add(18, "Pounds");
            dictionary1.Add(19, "ton");
            dictionary2.Add(19, "Ton");
            dictionary1.Add(20, "g");
            dictionary2.Add(20, "Grams");
            dictionary1.Add(21, "kg");
            dictionary2.Add(21, "Kilograms");
            dictionary1.Add(22, "mt");
            dictionary2.Add(22, "Metric Tons");
            dictionary1.Add(23, "a");
            dictionary2.Add(23, "Acre");
            dictionary1.Add(24, "ha");
            dictionary2.Add(24, "Hectare");
            dictionary1.Add(25, "Hr");
            dictionary2.Add(25, "Hour");
            dictionary1.Add(26, "Dy");
            dictionary2.Add(26, "Day");
            dictionary1.Add(27, "mth");
            dictionary2.Add(27, "Month");
            dictionary1.Add(28, "Yr");
            dictionary2.Add(28, "Year");
            dictionary1.Add(29, "$");
            dictionary2.Add(29, "Monetary unit");
            dictionary1.Add(30, "mwh");
            dictionary2.Add(30, "Megawatt-hours");
            dictionary1.Add(31, "Mbtu");
            dictionary2.Add(31, "Million British Thermal Units");
            dictionary1.Add(32, "gr");
            dictionary2.Add(32, "Growing Period");
            dictionary1.Add(33, "in3");
            dictionary2.Add(33, "Cubic inch");
            dictionary1.Add(34, "ft3");
            dictionary2.Add(34, "Cubic feet");
            dictionary1.Add(35, "mi3");
            dictionary2.Add(35, "Cubic miles");
            dictionary1.Add(36, "cm3");
            dictionary2.Add(36, "Cubic Centimeter");
            dictionary1.Add(37, "m3");
            dictionary2.Add(37, "Cubic Meter");
            dictionary1.Add(38, "km3");
            dictionary2.Add(38, "Cubic Kilometer");
            dictionary1.Add(39, "F");
            dictionary2.Add(39, "Fahrenheit");
            Dictionary<int, string> dictionary3 = new Dictionary<int, string>();
            Dictionary<int, string> dictionary4 = new Dictionary<int, string>();
            foreach ((int, string, string) tuple in (IEnumerable<(int, string, string)>) session.GetNamedQuery("selectUnitsTable").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, string, string)>()).List<(int, string, string)>())
            {
              dictionary3.Add(tuple.Item1, tuple.Item3);
              dictionary4.Add(tuple.Item1, tuple.Item2);
            }
            foreach (Units units in Enum.GetValues(typeof (Units)))
            {
              if (dictionary3.ContainsKey((int) units))
              {
                if (dictionary1[(int) units].ToLower() != dictionary3[(int) units].ToLower() || dictionary2[(int) units].ToLower() != dictionary4[(int) units].ToLower())
                  session.GetNamedQuery("updateUnitsTable").SetString("unitsDescription", dictionary2[(int) units]).SetString("unitsAbbreviation", dictionary1[(int) units]).SetInt32("unitsId", (int) units).ExecuteUpdate();
              }
              else
                session.GetNamedQuery("insertUnitsTable").SetInt32("unitsId", (int) units).SetString("unitsDescription", dictionary2[(int) units]).SetString("unitsAbbreviation", dictionary1[(int) units]).SetInt32("unitsFlag", 0).ExecuteUpdate();
            }
            transaction.Commit();
          }
          catch (Exception ex)
          {
            transaction.Rollback();
            throw;
          }
        }
      }
    }
  }
}
