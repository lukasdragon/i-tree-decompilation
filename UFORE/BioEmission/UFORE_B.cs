// Decompiled with JetBrains decompiler
// Type: UFORE.BioEmission.UFORE_B
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading;
using UFORE.Deposition;
using UFORE.LAI;
using UFORE.Location;

namespace UFORE.BioEmission
{
  public class UFORE_B
  {
    private string LocDB;
    private string NationIDSelected;
    private string PriPartIDSelected;
    private string SecPartIDSelected;
    private string TerPartIDSelected;
    private string SpeciesDB;
    private string AceDB;
    private string LeafBiomassTable;
    private string LanduseCoverTable;
    private string LaiDB;
    private string DryDepDB;
    private string BioEmisDB;
    private DryDeposition.VEG_TYPE VegType;
    private bool Inventory;
    private string FinBioEmisDB;
    private CancellationToken uforeCancellationToken;
    private IProgress<EngineProgressArg> uforeProgress;
    private EngineProgressArg uforeProgressArg;

    public UFORE_B(
      string sLocDB,
      string sNationID,
      string sPriPartID,
      string sSecPartID,
      string sTerPartID,
      string sSpDB,
      string sAceDB,
      string sLeafBiomassTbl,
      string sLanduseTbl,
      string sLaiDB,
      string sDDB,
      string sBioEmisDB,
      DryDeposition.VEG_TYPE sVegType,
      bool bInv,
      string sFinalDB,
      CancellationToken passinCancellationToken,
      IProgress<EngineProgressArg> passinProgress,
      EngineProgressArg passProgressArg)
      : this(sLocDB, sNationID, sPriPartID, sSecPartID, sTerPartID, sSpDB, sAceDB, sLeafBiomassTbl, sLanduseTbl, sLaiDB, sDDB, sBioEmisDB, sVegType, bInv, sFinalDB)
    {
      this.uforeCancellationToken = passinCancellationToken;
      this.uforeProgress = passinProgress;
      this.uforeProgressArg = passProgressArg;
    }

    public void reportProgress(int percent)
    {
      if (this.uforeProgress == null)
        return;
      if (this.uforeCancellationToken.IsCancellationRequested)
        throw new Exception("User cancelled");
      if (percent < this.uforeProgressArg.Percent + 1)
        return;
      this.uforeProgressArg.Percent = percent <= 100 ? (percent >= 0 ? percent : 0) : 100;
      this.uforeProgress.Report(this.uforeProgressArg);
    }

    public UFORE_B(
      string sLocDB,
      string sNationID,
      string sPriPartID,
      string sSecPartID,
      string sTerPartID,
      string sSpDB,
      string sAceDB,
      string sLeafBiomassTbl,
      string sLanduseTbl,
      string sLaiDB,
      string sDDB,
      string sBioEmisDB,
      DryDeposition.VEG_TYPE sVegType,
      bool bInv,
      string sFinalDB)
    {
      this.LocDB = sLocDB;
      this.NationIDSelected = sNationID;
      this.PriPartIDSelected = sPriPartID;
      this.SecPartIDSelected = sSecPartID;
      this.TerPartIDSelected = sTerPartID;
      this.SpeciesDB = sSpDB;
      this.AceDB = sAceDB;
      this.LeafBiomassTable = sLeafBiomassTbl;
      this.LanduseCoverTable = sLanduseTbl;
      this.LaiDB = sLaiDB;
      this.DryDepDB = sDDB;
      this.BioEmisDB = sBioEmisDB;
      this.VegType = sVegType;
      this.Inventory = bInv;
      this.FinBioEmisDB = sFinalDB;
    }

    private void SmoothBaseEmissionDictionary(
      OleDbConnection locSppConn,
      List<LeafBiomass> lbList,
      Dictionary<string, BaseEmission> beDict)
    {
      using (OleDbCommand oleDbCommand1 = new OleDbCommand())
      {
        using (OleDbCommand oleDbCommand2 = new OleDbCommand())
        {
          oleDbCommand1.Connection = locSppConn;
          oleDbCommand1.CommandText = "select SpeciesId, Code, ParentID, SpeciesTypeId from _Species WHERE Code=@code";
          oleDbCommand1.Parameters.Add("@code", OleDbType.VarWChar);
          oleDbCommand2.Connection = locSppConn;
          oleDbCommand2.CommandText = "select SpeciesId, Code, ParentID, SpeciesTypeId from _Species WHERE speciesId=@speciesid";
          oleDbCommand2.Parameters.Add("@speciesid", OleDbType.Integer);
          using (List<LeafBiomass>.Enumerator enumerator = lbList.GetEnumerator())
          {
label_15:
            while (enumerator.MoveNext())
            {
              LeafBiomass current = enumerator.Current;
              if (!beDict.ContainsKey(current.Genera))
              {
                oleDbCommand1.Parameters["@code"].Value = !(current.Genera.ToUpper() == "OTHER") ? (object) current.Genera : throw new Exception("Your project contains a species which is no longer in our database. This usually indicates that you are running an outdated version of i-Tree Eco. Please update and then resubmit your project.");
                OleDbDataReader oleDbDataReader = oleDbCommand1.ExecuteReader();
                if (!oleDbDataReader.Read())
                {
                  oleDbDataReader.Close();
                  throw new Exception("Genera code '" + current.Genera + "' does not exist. Debugging the project. The user is not notified yet.");
                }
                do
                {
                  oleDbCommand2.Parameters["@speciesid"].Value = (object) oleDbDataReader.GetInt32(oleDbDataReader.GetOrdinal("ParentId"));
                  oleDbDataReader.Close();
                  oleDbDataReader = oleDbCommand2.ExecuteReader();
                  if (oleDbDataReader.Read())
                  {
                    if (oleDbDataReader["Code"] != DBNull.Value && beDict.ContainsKey(oleDbDataReader.GetString(oleDbDataReader.GetOrdinal("Code"))))
                    {
                      beDict.Add(current.Genera, beDict[oleDbDataReader.GetString(oleDbDataReader.GetOrdinal("Code"))]);
                      oleDbDataReader.Close();
                      goto label_15;
                    }
                  }
                  else
                    goto label_14;
                }
                while (oleDbDataReader.GetInt32(oleDbDataReader.GetOrdinal("SpeciesTypeId")) != 1);
                oleDbDataReader.Close();
                throw new Exception("No bio emission found for '" + current.Genera + "' and its parents.");
label_14:
                throw new Exception("Genera '" + current.Genera + "' does not have a class as top parent.");
              }
            }
          }
        }
      }
    }

    public void RunUFORE_B()
    {
      List<string> landuse = new List<string>();
      Dictionary<string, Landuse> luDict = new Dictionary<string, Landuse>();
      City city = new City();
      LocationDataForB locationDataForB = new LocationDataForB();
      List<LeafBiomass> lbList = new List<LeafBiomass>();
      Dictionary<string, BaseEmission> beDict = new Dictionary<string, BaseEmission>();
      List<string> genera = new List<string>();
      Dictionary<string, LeafBiomass> genDict = new Dictionary<string, LeafBiomass>();
      List<DryDeposition> dryDepositionList = new List<DryDeposition>();
      List<LeafAreaIndex> laiList = new List<LeafAreaIndex>();
      this.reportProgress(0);
      locationDataForB.NationID = this.NationIDSelected;
      locationDataForB.PriPartID = this.PriPartIDSelected;
      locationDataForB.SecPartID = this.SecPartIDSelected;
      locationDataForB.TerPartID = this.TerPartIDSelected;
      using (OleDbConnection cnAceDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.AceDB))
      {
        cnAceDB.Open();
        Landuse.ReadLanduseData(cnAceDB, this.LanduseCoverTable, ref landuse, ref luDict);
        city.ReadCityData(cnAceDB, this.LanduseCoverTable);
        LeafBiomass.ReadLeafBiomassData(cnAceDB, this.LeafBiomassTable, this.VegType, ref lbList);
        LeafBiomass.ReadGeneraLeafBiomass(cnAceDB, this.LeafBiomassTable, this.VegType, ref genera, ref genDict);
      }
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.SpeciesDB))
      {
        oleDbConnection.Open();
        BaseEmission.ReadBaseEmission(oleDbConnection, ref beDict);
        this.SmoothBaseEmissionDictionary(oleDbConnection, lbList, beDict);
      }
      using (OleDbConnection cnDryDepDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.DryDepDB))
      {
        cnDryDepDB.Open();
        DryDeposition.ReadLightCorrFct(cnDryDepDB, "NO2", ref dryDepositionList);
        DateTime timeStamp1 = dryDepositionList[0].TimeStamp;
        DateTime timeStamp2 = dryDepositionList[dryDepositionList.Count - 1].TimeStamp;
        double maxLai;
        using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.LaiDB))
        {
          oleDbConnection.Open();
          maxLai = LeafAreaIndex.ReadMaxLAI(oleDbConnection, timeStamp1, timeStamp2);
          LeafAreaIndex.ReadLAIPartialRecords(oleDbConnection, ref laiList, timeStamp1, timeStamp2);
        }
        AccessFunc.CreateDB(this.BioEmisDB);
        this.reportProgress(2);
        using (OleDbConnection cnBioEDB = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.BioEmisDB))
        {
          cnBioEDB.Open();
          CorrectionFactor.ProcessCorrectionFactor(ref landuse, ref luDict, ref dryDepositionList, maxLai, ref laiList, this.VegType, this.Inventory, cnBioEDB, this, 2, 96);
          this.reportProgress(96);
          using (OleDbCommand oleDbCommand = new OleDbCommand())
          {
            oleDbCommand.Connection = cnBioEDB;
            if (this.VegType == DryDeposition.VEG_TYPE.TREE)
            {
              oleDbCommand.CommandText = "SELECT LBIOMASS.LANDUSE, LBIOMASS.CODE, LBIOMASS.GENUS,true AS TREE, true AS DECIDUOUS, LBIOMASS.LVE_TYPE,LBIOMASS.FORM_IND, LBIOMASS.STOT_LB AS TotalLeafBiomass INTO LBIOMASS  FROM [" + this.AceDB + "].LBIOMASS  WHERE ucase(LBIOMASS.FORM_IND) = 'TREE'";
              oleDbCommand.ExecuteNonQuery();
            }
            else
            {
              oleDbCommand.CommandText = "SELECT LBIOMASS.LANDUSE, LBIOMASS.CODE, LBIOMASS.GENUS,true AS TREE, true AS DECIDUOUS, LBIOMASS.LVE_TYPE,LBIOMASS.FORM_IND, LBIOMASS.STOT_LB AS TotalLeafBiomass INTO LBIOMASS  FROM [" + this.AceDB + "].LBIOMASS  WHERE ucase(LBIOMASS.FORM_IND) = 'SHRUB'";
              oleDbCommand.ExecuteNonQuery();
            }
            oleDbCommand.CommandText = "UPDATE LBIOMASS SET TREE = True WHERE ucase(FORM_IND)='TREE'";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "UPDATE LBIOMASS SET TREE = False WHERE ucase(FORM_IND)<>'TREE'";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "UPDATE LBIOMASS SET DECIDUOUS = True WHERE ucase(LVE_TYPE) = 'DECIDUOUS'";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "UPDATE LBIOMASS SET DECIDUOUS = False WHERE ucase(LVE_TYPE) <> 'DECIDUOUS'";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "CREATE INDEX [LanduseInd] ON LBIOMASS ([LANDUSE])";
            oleDbCommand.ExecuteNonQuery();
            oleDbCommand.CommandText = "CREATE INDEX [CodeGenera] ON LBIOMASS ([CODE],[LANDUSE])";
            oleDbCommand.ExecuteNonQuery();
          }
          Emission.ProcessBioEmission(ref lbList, ref beDict, cnBioEDB);
          this.reportProgress(98);
          Summary.CalculateSummaryForLessMdbStorage(cnBioEDB);
        }
      }
      this.CreateFinalDB();
      this.reportProgress(100);
    }

    private void CreateFinalDB()
    {
      AccessFunc.CreateDB(this.FinBioEmisDB);
      using (OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data source= " + this.FinBioEmisDB))
      {
        OleDbCommand oleDbCommand = new OleDbCommand();
        try
        {
          oleDbConnection.Open();
          oleDbCommand.Connection = oleDbConnection;
          oleDbCommand.CommandText = "SELECT Genera,[Isoprene emitted],[Monoterpene emitted],[Other VOCs emitted] INTO YearlySummaryByGenera FROM [" + this.BioEmisDB + "].[02_YearlySummaryByGenera];";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "SELECT Landuse, LanduseDesc,[Isoprene emitted],[Monoterpene emitted],[Other VOCs emitted] INTO YearlySummaryByLanduse FROM [" + this.BioEmisDB + "].[01_YearlySummaryByLanduse];";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "SELECT TimeStamp,Genera,[Isoprene emitted],[Monoterpene emitted] INTO HourlySummaryByGenera FROM [" + this.BioEmisDB + "].[09_HourlySummaryByGenera];";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "SELECT TimeStamp,Code, Genera,[Isoprene emitted],[Monoterpene emitted] INTO HourlySummaryBySpecies FROM [" + this.BioEmisDB + "].[11_HourlySummaryBySpecies];";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "SELECT TimeStamp,Landuse, LanduseDesc,[Isoprene emitted],[Monoterpene emitted] INTO HourlySummaryByLanduse FROM [" + this.BioEmisDB + "].[08_HourlySummaryByLanduse];";
          oleDbCommand.ExecuteNonQuery();
          oleDbCommand.CommandText = "SELECT TimeStamp,[Isoprene emitted],[Monoterpene emitted] INTO HourlyDomainSummary FROM [" + this.BioEmisDB + "].[10_HourlySummaryDomain];";
          oleDbCommand.ExecuteNonQuery();
          AccessFunc.CopyTable(this.BioEmisDB, "YearlyCorrectionFactor", this.FinBioEmisDB, "YearlyCorrectionFactor");
        }
        catch (Exception ex)
        {
          throw;
        }
        finally
        {
          oleDbConnection.Close();
        }
      }
    }
  }
}
