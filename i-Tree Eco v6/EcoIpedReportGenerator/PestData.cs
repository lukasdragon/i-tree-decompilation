// Decompiled with JetBrains decompiler
// Type: EcoIpedReportGenerator.PestData
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.Win.C1FlexGrid;
using Eco.Domain.Properties;
using Eco.Util;
using i_Tree_Eco_v6;
using IPED.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EcoIpedReportGenerator
{
  public class PestData : COutput
  {
    private readonly ProgramSession ps = ProgramSession.GetInstance();
    private readonly DataTable m_dtInventory;
    private readonly DataTable m_dtPests;
    public Dictionary<int, string> DctBBAbnGrowth;
    public Dictionary<int, string> DctBBDiseaseSigns;
    public Dictionary<int, string> DctBBInsectPres;
    public Dictionary<int, string> DctBBInsectSigns;
    public Dictionary<int, string> DctBBProbLoc;
    public Dictionary<int, string> DctFTAbnFoli;
    public Dictionary<int, string> DctFTChewFoli;
    public Dictionary<int, string> DctFTDiscFoli;
    public Dictionary<int, string> DctFTFoliAffect;
    public Dictionary<int, string> DctFTInsectSigns;
    public Dictionary<int, string> DctTSDieback;
    public Dictionary<int, string> DctTSEnvStress;
    public Dictionary<int, string> DctTSEpiSprout;
    public Dictionary<int, string> DctTSHumStress;
    public Dictionary<int, string> DctTSWiltFoli;

    public PestData()
    {
      this.m_dtInventory = staticData.DsData.Tables["Trees"];
      this.m_dtPests = staticData.DsData.Tables["Pests"];
      this.DctBBAbnGrowth = this.GetLookups<BBAbnGrowth>(this.ps.IPEDData.BBAbnGrowth, (Func<BBAbnGrowth, bool>) (lu => lu.Code > 0));
      this.DctBBDiseaseSigns = this.GetLookups<BBDiseaseSigns>(this.ps.IPEDData.BBDiseaseSigns, (Func<BBDiseaseSigns, bool>) (lu => lu.Code > 0));
      this.DctBBProbLoc = this.GetLookups<BBProbLoc>(this.ps.IPEDData.BBProbLoc, (Func<BBProbLoc, bool>) (lu => lu.Code > 0));
      this.DctBBInsectPres = this.GetLookups<BBInsectPres>(this.ps.IPEDData.BBInsectPres, (Func<BBInsectPres, bool>) (lu => lu.Code > 0));
      this.DctBBInsectSigns = this.GetLookups<BBInsectSigns>(this.ps.IPEDData.BBInsectSigns, (Func<BBInsectSigns, bool>) (lu => lu.Code > 0));
      this.DctFTAbnFoli = this.GetLookups<FTAbnFoli>(this.ps.IPEDData.FTAbnFoli, (Func<FTAbnFoli, bool>) (lu => lu.Code > 0));
      this.DctFTChewFoli = this.GetLookups<FTChewFoli>(this.ps.IPEDData.FTChewFoli, (Func<FTChewFoli, bool>) (lu => lu.Code > 0));
      this.DctFTDiscFoli = this.GetLookups<FTDiscFoli>(this.ps.IPEDData.FTDiscFoli, (Func<FTDiscFoli, bool>) (lu => lu.Code > 0));
      this.DctFTFoliAffect = this.GetLookups<FTFoliAffect>(this.ps.IPEDData.FTFoliAffect, (Func<FTFoliAffect, bool>) (lu => lu.Code > 0));
      this.DctFTInsectSigns = this.GetLookups<FTInsectSigns>(this.ps.IPEDData.FTInsectSigns, (Func<FTInsectSigns, bool>) (lu => lu.Code > 0));
      this.DctTSDieback = this.GetLookups<TSDieback>(this.ps.IPEDData.TSDieback, (Func<TSDieback, bool>) (lu => lu.Code > 0));
      this.DctTSEnvStress = this.GetLookups<TSEnvStress>(this.ps.IPEDData.TSEnvStress, (Func<TSEnvStress, bool>) (lu => lu.Code > 0));
      this.DctTSEpiSprout = this.GetLookups<TSEpiSprout>(this.ps.IPEDData.TSEpiSprout, (Func<TSEpiSprout, bool>) (lu => lu.Code == 1));
      this.DctTSHumStress = this.GetLookups<TSHumStress>(this.ps.IPEDData.TSHumStress, (Func<TSHumStress, bool>) (lu => lu.Code > 0));
      this.DctTSWiltFoli = this.GetLookups<TSWiltFoli>(this.ps.IPEDData.TSWiltFoli, (Func<TSWiltFoli, bool>) (lu => lu.Code > 0));
    }

    private Dictionary<int, string> GetLookups<T>(IList<T> lookupList, Func<T, bool> cond) where T : Lookup => Enumerable.ToDictionary<T, int, string>(Enumerable.Where<T>(lookupList, cond), (Func<T, int>) (d => d.Code), (Func<T, string>) (d => d.Description));

    public Dictionary<string, List<int>> GetSeletedSymptoms(DataTable dt)
    {
      Dictionary<string, List<int>> seletedSymptoms = new Dictionary<string, List<int>>();
      foreach (DataRow row in (InternalDataCollectionBase) dt.Rows)
      {
        if (row["Select"].Equals((object) true))
        {
          List<int> intList = (List<int>) null;
          if (!seletedSymptoms.TryGetValue(row["LookupType"].ToString(), out intList))
          {
            intList = new List<int>();
            seletedSymptoms[row["LookupType"].ToString()] = intList;
          }
          intList.Add(Convert.ToInt32(row["LookupCode"]));
        }
      }
      return seletedSymptoms;
    }

    public DataSet GetAllSymptoms()
    {
      DataSet allSymptoms = new DataSet("Symptoms");
      DataTable dt = allSymptoms.Tables.Add("Data");
      dt.Columns.AddRange(new DataColumn[6]
      {
        new DataColumn("Category", typeof (string)),
        new DataColumn("SubCategory", typeof (string)),
        new DataColumn("Select", typeof (bool)),
        new DataColumn("Description", typeof (string)),
        new DataColumn("LookupType", typeof (string)),
        new DataColumn("LookupCode", typeof (string))
      });
      dt.ExtendedProperties.Add((object) "GroupBy", (object) new string[2]
      {
        "Category",
        "SubCategory"
      });
      dt.ExtendedProperties.Add((object) "ShowColumns", (object) new string[4]
      {
        "Category",
        "SubCategory",
        "Select",
        "Description"
      });
      dt.ExtendedProperties.Add((object) "EditableColumns", (object) new string[1]
      {
        "Select"
      });
      dt.ExtendedProperties.Add((object) "DefaultLevel", (object) 2);
      dt.ExtendedProperties.Add((object) "TreeStyleFlag", (object) TreeStyleFlags.Complete);
      this.AddSymptoms("Tree Stress", "Dieback", this.DctTSDieback, dt, "TSDieback");
      this.AddSymptoms("Tree Stress", "Epicormic Sprouts", this.DctTSEpiSprout, dt, "TSEpiSprout");
      this.AddSymptoms("Tree Stress", "Wilted Foliage", this.DctTSWiltFoli, dt, "TSWiltFoli");
      this.AddSymptoms("Tree Stress", "Environmental Stress", this.DctTSEnvStress, dt, "TSEnvStress");
      this.AddSymptoms("Tree Stress", "Human-caused Stress", this.DctTSHumStress, dt, "TSHumStress");
      this.AddSymptoms("Foliage / Twigs", "Defoliation", this.DctFTChewFoli, dt, "FTChewFoli");
      this.AddSymptoms("Foliage / Twigs", "Discolored Foliage", this.DctFTDiscFoli, dt, "FTDiscFoli");
      this.AddSymptoms("Foliage / Twigs", "Abnormal Foliage", this.DctFTAbnFoli, dt, "FTAbnFoli");
      this.AddSymptoms("Foliage / Twigs", "Insect Signs", this.DctFTInsectSigns, dt, "FTInsectSigns");
      this.AddSymptoms("Foliage / Twigs", "% Foliage Affected", this.DctFTFoliAffect, dt, "FTFoliAffect");
      this.AddSymptoms("Branches / Bole", "Insect Signs", this.DctBBInsectSigns, dt, "BBInsectSigns");
      this.AddSymptoms("Branches / Bole", "Insect Presence", this.DctBBInsectPres, dt, "BBInsectPres");
      this.AddSymptoms("Branches / Bole", "Disease Signs", this.DctBBDiseaseSigns, dt, "BBDiseaseSigns");
      this.AddSymptoms("Branches / Bole", "Problem Location", this.DctBBProbLoc, dt, "BBProbLoc");
      this.AddSymptoms("Branches / Bole", "Loose Bark", this.DctBBAbnGrowth, dt, "BBAbnGrowth");
      return allSymptoms;
    }

    private void AddSymptoms(
      string Category,
      string SubCategory,
      Dictionary<int, string> dv,
      DataTable dt,
      string lucidType)
    {
      foreach (KeyValuePair<int, string> keyValuePair in dv)
      {
        DataRow row = dt.NewRow();
        row[nameof (Category)] = (object) Category;
        row[nameof (SubCategory)] = (object) SubCategory;
        row["LookupCode"] = (object) keyValuePair.Key;
        row["Description"] = (object) keyValuePair.Value;
        row["LookupType"] = (object) lucidType;
        row["Select"] = (object) false;
        dt.Rows.Add(row);
      }
    }

    public Dictionary<string, DataSet> GetRecordsForPest(int PestId)
    {
      Dictionary<string, List<int>> symptomsForPest = this.GetSymptomsForPest(PestId);
      DataSet symptomsDs = this.GetSymptomsDS(symptomsForPest);
      List<string> hostsForPest = this.GetHostsForPest(PestId);
      DataSet dataSet1 = this.RecordsForSymptoms(symptomsForPest, hostsForPest, true);
      DataSet dataSet2 = this.RecordsForSymptoms(symptomsForPest, hostsForPest, false);
      return new Dictionary<string, DataSet>()
      {
        [symptomsDs.DataSetName] = symptomsDs,
        ["HostRecords"] = dataSet1,
        ["OtherRecords"] = dataSet2
      };
    }

    private DataSet GetSymptomsDS(Dictionary<string, List<int>> dSymptoms)
    {
      DataSet symptomsDs = new DataSet("Symptoms");
      DataTable dataTable = symptomsDs.Tables.Add("Data");
      dataTable.Columns.AddRange(new DataColumn[3]
      {
        new DataColumn("Category", typeof (string)),
        new DataColumn("SubCategory", typeof (string)),
        new DataColumn("Value", typeof (string))
      });
      dataTable.ExtendedProperties.Add((object) "GroupBy", (object) new string[2]
      {
        "Category",
        "SubCategory"
      });
      dataTable.ExtendedProperties.Add((object) "ShowColumns", (object) new string[3]
      {
        "Category",
        "SubCategory",
        "Value"
      });
      dataTable.ExtendedProperties.Add((object) "DefaultLevel", (object) 2);
      dataTable.ExtendedProperties.Add((object) "TreeStyleFlag", (object) TreeStyleFlags.Complete);
      foreach (KeyValuePair<string, List<int>> dSymptom in dSymptoms)
      {
        string str1 = dSymptom.Key.Substring(0, 2);
        string str2 = dSymptom.Key.Substring(2);
        string str3;
        Dictionary<int, string> dictionary;
        if (!(str1 == "TS"))
        {
          if (!(str1 == "FT"))
          {
            if (str1 == "BB")
            {
              str3 = "Branches / Bole";
              if (!(str2 == "InsectSigns"))
              {
                if (!(str2 == "InsectPres"))
                {
                  if (!(str2 == "DiseaseSigns"))
                  {
                    if (!(str2 == "ProbLoc"))
                    {
                      if (str2 == "AbnGrowth")
                      {
                        str2 = "Loose Bark";
                        dictionary = this.DctBBAbnGrowth;
                      }
                      else
                        continue;
                    }
                    else
                    {
                      str2 = "Problem Location";
                      dictionary = this.DctBBProbLoc;
                    }
                  }
                  else
                  {
                    str2 = "Disease Signs";
                    dictionary = this.DctBBDiseaseSigns;
                  }
                }
                else
                {
                  str2 = "Insect Presence";
                  dictionary = this.DctBBInsectPres;
                }
              }
              else
              {
                str2 = "Insect Signs";
                dictionary = this.DctBBInsectSigns;
              }
            }
            else
              continue;
          }
          else
          {
            str3 = "Foliage / Twigs";
            if (!(str2 == "ChewFoli"))
            {
              if (!(str2 == "DiscFoli"))
              {
                if (!(str2 == "AbnFoli"))
                {
                  if (!(str2 == "InsectSigns"))
                  {
                    if (str2 == "FoliAffect")
                    {
                      str2 = "% Foliage Affected";
                      dictionary = this.DctFTFoliAffect;
                    }
                    else
                      continue;
                  }
                  else
                  {
                    str2 = "Insect Signs";
                    dictionary = this.DctFTInsectSigns;
                  }
                }
                else
                {
                  str2 = "Abnormal Foliage";
                  dictionary = this.DctFTAbnFoli;
                }
              }
              else
              {
                str2 = "Discolored Foliage";
                dictionary = this.DctFTDiscFoli;
              }
            }
            else
            {
              str2 = "Defoliation";
              dictionary = this.DctFTChewFoli;
            }
          }
        }
        else
        {
          str3 = "Tree Stress";
          if (!(str2 == "Dieback"))
          {
            if (!(str2 == "EpiSprout"))
            {
              if (!(str2 == "WiltFoli"))
              {
                if (!(str2 == "EnvStress"))
                {
                  if (str2 == "HumStress")
                  {
                    str2 = "Human-caused Stress";
                    dictionary = this.DctTSHumStress;
                  }
                  else
                    continue;
                }
                else
                {
                  str2 = "Environmental Stress";
                  dictionary = this.DctTSEnvStress;
                }
              }
              else
              {
                str2 = "Wilted Foliage";
                dictionary = this.DctTSWiltFoli;
              }
            }
            else
            {
              str2 = "Epicormic Sprouts";
              dictionary = this.DctTSEpiSprout;
            }
          }
          else
            dictionary = this.DctTSDieback;
        }
        foreach (int key in dSymptom.Value)
        {
          if (dictionary.ContainsKey(key))
          {
            DataRow row = dataTable.NewRow();
            row["Category"] = (object) str3;
            row["SubCategory"] = (object) str2;
            row["Value"] = (object) dictionary[key];
            dataTable.Rows.Add(row);
          }
        }
      }
      return symptomsDs;
    }

    public DataSet RecordsForSymptoms(
      Dictionary<string, List<int>> dSymptoms,
      List<string> lHosts,
      bool bMatchHost)
    {
      Dictionary<string, SortedDictionary<int, SortedDictionary<int, List<IPEDInfo>>>> dictionary = new Dictionary<string, SortedDictionary<int, SortedDictionary<int, List<IPEDInfo>>>>();
      int num1 = 0;
      foreach (DataRow row in (InternalDataCollectionBase) this.m_dtInventory.Rows)
      {
        string genusFromSppCode = staticData.GetGenusFromSppCode(row["SpCode"].ToString());
        if (bMatchHost && lHosts.Contains(genusFromSppCode) || !bMatchHost && !lHosts.Contains(genusFromSppCode))
        {
          IPEDInfo PestInfo = new IPEDInfo();
          this.StoreHitOrMiss(PestInfo, "Tree Stress", "TSDieback", dSymptoms, Convert.ToInt32(row["PestTSDieback"]), this.DctTSDieback);
          this.StoreHitOrMiss(PestInfo, "Tree Stress", "TSEpiSprout", dSymptoms, Convert.ToInt32(row["PestTSEpiSprout"]), this.DctTSEpiSprout);
          this.StoreHitOrMiss(PestInfo, "Tree Stress", "TSWiltFoli", dSymptoms, Convert.ToInt32(row["PestTSWiltFoli"]), this.DctTSWiltFoli);
          this.StoreHitOrMiss(PestInfo, "Tree Stress", "TSEnvStress", dSymptoms, Convert.ToInt32(row["PestTSEnvStress"]), this.DctTSEnvStress);
          this.StoreHitOrMiss(PestInfo, "Tree Stress", "TSHumStress", dSymptoms, Convert.ToInt32(row["PestTSHumStress"]), this.DctTSHumStress);
          this.StoreHitOrMiss(PestInfo, "Foliage/Twigs", "FTChewFoli", dSymptoms, Convert.ToInt32(row["PestFTChewFoli"]), this.DctFTChewFoli);
          this.StoreHitOrMiss(PestInfo, "Foliage/Twigs", "FTDiscFoli", dSymptoms, Convert.ToInt32(row["PestFTDiscFoli"]), this.DctFTDiscFoli);
          this.StoreHitOrMiss(PestInfo, "Foliage/Twigs", "FTAbnFoli", dSymptoms, Convert.ToInt32(row["PestFTAbnFoli"]), this.DctFTAbnFoli);
          this.StoreHitOrMiss(PestInfo, "Foliage/Twigs", "FTInsectSigns", dSymptoms, Convert.ToInt32(row["PestFTInsectSigns"]), this.DctFTInsectSigns);
          this.StoreHitOrMiss(PestInfo, "Foliage/Twigs", "FTFoliAffect", dSymptoms, Convert.ToInt32(row["PestFTFoliAffect"]), this.DctFTFoliAffect);
          this.StoreHitOrMiss(PestInfo, "Branches/Bole", "BBInsectSigns", dSymptoms, Convert.ToInt32(row["PestBBInsectSigns"]), this.DctBBInsectSigns);
          this.StoreHitOrMiss(PestInfo, "Branches/Bole", "BBInsectPres", dSymptoms, Convert.ToInt32(row["PestBBInsectPres"]), this.DctBBInsectPres);
          this.StoreHitOrMiss(PestInfo, "Branches/Bole", "BBDiseaseSigns", dSymptoms, Convert.ToInt32(row["PestBBDiseaseSigns"]), this.DctBBDiseaseSigns);
          this.StoreHitOrMiss(PestInfo, "Branches/Bole", "BBProbLoc", dSymptoms, Convert.ToInt32(row["PestBBProbLoc"]), this.DctBBProbLoc);
          this.StoreHitOrMiss(PestInfo, "Branches/Bole", "BBAbnGrowth", dSymptoms, Convert.ToInt32(row["PestBBAbnGrowth"]), this.DctBBAbnGrowth);
          int key1 = 0;
          int key2 = 0;
          foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair in PestInfo.Hit)
            key1 += keyValuePair.Value.Count;
          foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair in PestInfo.Miss)
            key2 += keyValuePair.Value.Count;
          if (key1 > 0 && key2 < 4)
          {
            SortedDictionary<int, SortedDictionary<int, List<IPEDInfo>>> sortedDictionary1 = (SortedDictionary<int, SortedDictionary<int, List<IPEDInfo>>>) null;
            if (!dictionary.TryGetValue(row["SpCode"].ToString(), out sortedDictionary1))
            {
              sortedDictionary1 = new SortedDictionary<int, SortedDictionary<int, List<IPEDInfo>>>();
              dictionary[row["SpCode"].ToString()] = sortedDictionary1;
            }
            SortedDictionary<int, List<IPEDInfo>> sortedDictionary2 = (SortedDictionary<int, List<IPEDInfo>>) null;
            if (!sortedDictionary1.TryGetValue(key1, out sortedDictionary2))
            {
              sortedDictionary2 = new SortedDictionary<int, List<IPEDInfo>>();
              sortedDictionary1[key1] = sortedDictionary2;
            }
            List<IPEDInfo> ipedInfoList = (List<IPEDInfo>) null;
            if (!sortedDictionary2.TryGetValue(key2, out ipedInfoList))
            {
              ipedInfoList = new List<IPEDInfo>();
              sortedDictionary2[key2] = ipedInfoList;
            }
            PestInfo.dr = row;
            ipedInfoList.Add(PestInfo);
          }
        }
        ++num1;
      }
      DataSet dataSet = new DataSet("Records");
      DataTable dataTable1 = dataSet.Tables.Add("Data");
      dataTable1.Columns.AddRange(new DataColumn[11]
      {
        new DataColumn("Species", typeof (string)),
        new DataColumn("Category", typeof (string)),
        new DataColumn("SubCategory", typeof (string)),
        new DataColumn("PlotID", typeof (int)),
        new DataColumn("TreeId", typeof (int)),
        new DataColumn("Strata", typeof (string)),
        new DataColumn("Field Landuse", typeof (string)),
        new DataColumn("Street Tree", typeof (string)),
        new DataColumn("Address", typeof (string)),
        new DataColumn("Hit", typeof (int)),
        new DataColumn("Miss", typeof (int))
      });
      dataTable1.ExtendedProperties.Add((object) "HideColumns", (object) new string[2]
      {
        "Hit",
        "Miss"
      });
      dataTable1.ExtendedProperties.Add((object) "GroupBy", (object) new string[3]
      {
        "Species",
        "Category",
        "SubCategory"
      });
      dataTable1.ExtendedProperties.Add((object) "DefaultLevel", (object) 0);
      DataTable dataTable2 = dataTable1;
      DataTable dataTable3 = dataSet.Tables.Add("TreeSymptoms");
      dataTable3.Columns.AddRange(new DataColumn[6]
      {
        new DataColumn("PlotId", typeof (int)),
        new DataColumn("TreeId", typeof (int)),
        new DataColumn("Category", typeof (string)),
        new DataColumn("SubCategory", typeof (string)),
        new DataColumn("Sign/Symptom", typeof (string)),
        new DataColumn("Value", typeof (string))
      });
      dataTable3.ExtendedProperties.Add((object) "HideColumns", (object) new string[1]
      {
        "TreeId"
      });
      dataTable3.ExtendedProperties.Add((object) "GroupBy", (object) new string[2]
      {
        "Category",
        "SubCategory"
      });
      dataTable3.ExtendedProperties.Add((object) "DefaultLevel", (object) 2);
      DataTable dataTable4 = dataTable3;
      dataSet.Relations.Add(new DataRelation("RecordSymptoms", new DataColumn[2]
      {
        dataTable2.Columns["PlotId"],
        dataTable2.Columns["TreeId"]
      }, new DataColumn[2]
      {
        dataTable4.Columns["PlotId"],
        dataTable4.Columns["TreeId"]
      }, true));
      dataSet.DefaultViewManager.DataViewSettings["Data"].Sort = "Species ASC, Hit DESC, Miss ASC, TreeId ASC";
      foreach (KeyValuePair<string, SortedDictionary<int, SortedDictionary<int, List<IPEDInfo>>>> keyValuePair1 in dictionary)
      {
        int num2 = 0;
        foreach (KeyValuePair<int, SortedDictionary<int, List<IPEDInfo>>> keyValuePair2 in keyValuePair1.Value)
        {
          foreach (KeyValuePair<int, List<IPEDInfo>> keyValuePair3 in keyValuePair2.Value)
            num2 += keyValuePair3.Value.Count;
        }
        foreach (KeyValuePair<int, SortedDictionary<int, List<IPEDInfo>>> keyValuePair4 in keyValuePair1.Value)
        {
          int num3 = 0;
          foreach (KeyValuePair<int, List<IPEDInfo>> keyValuePair5 in keyValuePair4.Value)
            num3 += keyValuePair5.Value.Count;
          foreach (KeyValuePair<int, List<IPEDInfo>> keyValuePair6 in keyValuePair4.Value)
          {
            foreach (IPEDInfo ipedInfo in keyValuePair6.Value)
            {
              DataRow dr = ipedInfo.dr;
              DataRow row1 = dataTable2.NewRow();
              row1["Hit"] = (object) keyValuePair4.Key;
              row1["Miss"] = (object) keyValuePair6.Key;
              row1["Category"] = (object) string.Format("{2} of the {4} Trees Exactly Match {0} of {1} Sign/Symptom Categories [{3:0.00%} of All Trees]", (object) keyValuePair4.Key, (object) dSymptoms.Count, (object) num3, (object) (num3 / num1), (object) num2);
              if (keyValuePair6.Key > 0)
                row1["SubCategory"] = (object) string.Format("{1} of the {3} Trees Have {0} Additional Signs/Symptoms [{2:0.00%} of All Trees]", (object) keyValuePair6.Key, (object) keyValuePair6.Value.Count, (object) (keyValuePair6.Value.Count / num1), (object) num3);
              else
                row1["SubCategory"] = (object) string.Format("{0} of the {2} Trees Have No Additional Signs/Symptoms [{1:0.00%} of All Trees]", new object[3]
                {
                  (object) keyValuePair6.Value.Count,
                  (object) (keyValuePair6.Value.Count / num1),
                  (object) num3
                });
              row1["PlotId"] = dr["PlotId"];
              row1["TreeId"] = dr["TreeId"];
              row1["Strata"] = dr["MapLanduseDescription"];
              row1["Field Landuse"] = dr["FieldLandUseDescription"];
              row1["Street Tree"] = (bool) dr["TreeSite"] ? (object) "Yes" : (object) "No";
              row1["Address"] = dr["Address"];
              row1["Species"] = (object) string.Format("{0} [{1} Trees, {2:0.00%} of All Trees]", (object) this.GetSpeciesName(dr["SpCode"].ToString()), (object) num2, (object) (num2 / num1));
              dataTable2.Rows.Add(row1);
              foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair7 in ipedInfo.Hit)
              {
                foreach (KeyValuePair<string, int> keyValuePair8 in keyValuePair7.Value)
                {
                  DataRow row2 = dataTable4.NewRow();
                  row2["PlotId"] = dr["PlotId"];
                  row2["TreeId"] = dr["TreeId"];
                  row2["Category"] = (object) "Matching Signs/Symptoms";
                  row2["SubCategory"] = (object) keyValuePair7.Key;
                  row2["Sign/Symptom"] = (object) staticData.GetIPEDFieldName(keyValuePair8.Key);
                  row2["Value"] = (object) this.GetIPEDDescription(keyValuePair8.Key, keyValuePair8.Value);
                  dataTable4.Rows.Add(row2);
                }
              }
              foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair9 in ipedInfo.Miss)
              {
                foreach (KeyValuePair<string, int> keyValuePair10 in keyValuePair9.Value)
                {
                  DataRow row3 = dataTable4.NewRow();
                  row3["PlotId"] = dr["PlotId"];
                  row3["TreeId"] = dr["TreeId"];
                  row3["Category"] = (object) "Additional Signs/Symptoms";
                  row3["SubCategory"] = (object) keyValuePair9.Key;
                  row3["Sign/Symptom"] = (object) staticData.GetIPEDFieldName(keyValuePair10.Key);
                  row3["Value"] = (object) this.GetIPEDDescription(keyValuePair10.Key, keyValuePair10.Value);
                  dataTable4.Rows.Add(row3);
                }
              }
            }
          }
        }
      }
      return dataSet;
    }

    public DataSet GetRecordsForSymptoms(Dictionary<string, List<int>> dSymptoms)
    {
      SortedDictionary<string, List<IPEDInfo>> sortedDictionary = new SortedDictionary<string, List<IPEDInfo>>();
      int num1 = 0;
      int num2 = 0;
      HitMiss hmInfo = new HitMiss();
      foreach (DataRow row in (InternalDataCollectionBase) this.m_dtInventory.Rows)
      {
        IPEDInfo PestInfo = new IPEDInfo();
        this.StoreHitOrMiss(PestInfo, "Tree Stress", "TSDieback", dSymptoms, Convert.ToInt32(row["PestTSDieback"]), this.DctTSDieback);
        this.StoreHitOrMiss(PestInfo, "Tree Stress", "TSEpiSprout", dSymptoms, Convert.ToInt32(row["PestTSEpiSprout"]), this.DctTSEpiSprout);
        this.StoreHitOrMiss(PestInfo, "Tree Stress", "TSWiltFoli", dSymptoms, Convert.ToInt32(row["PestTSWiltFoli"]), this.DctTSWiltFoli);
        this.StoreHitOrMiss(PestInfo, "Tree Stress", "TSEnvStress", dSymptoms, Convert.ToInt32(row["PestTSEnvStress"]), this.DctTSEnvStress);
        this.StoreHitOrMiss(PestInfo, "Tree Stress", "TSHumStress", dSymptoms, Convert.ToInt32(row["PestTSHumStress"]), this.DctTSHumStress);
        this.StoreHitOrMiss(PestInfo, "Foliage/Twigs", "FTChewFoli", dSymptoms, Convert.ToInt32(row["PestFTChewFoli"]), this.DctFTChewFoli);
        this.StoreHitOrMiss(PestInfo, "Foliage/Twigs", "FTDiscFoli", dSymptoms, Convert.ToInt32(row["PestFTDiscFoli"]), this.DctFTDiscFoli);
        this.StoreHitOrMiss(PestInfo, "Foliage/Twigs", "FTAbnFoli", dSymptoms, Convert.ToInt32(row["PestFTAbnFoli"]), this.DctFTAbnFoli);
        this.StoreHitOrMiss(PestInfo, "Foliage/Twigs", "FTInsectSigns", dSymptoms, Convert.ToInt32(row["PestFTInsectSigns"]), this.DctFTInsectSigns);
        this.StoreHitOrMiss(PestInfo, "Foliage/Twigs", "FTFoliAffect", dSymptoms, Convert.ToInt32(row["PestFTFoliAffect"]), this.DctFTFoliAffect);
        this.StoreHitOrMiss(PestInfo, "Branches/Bole", "BBInsectSigns", dSymptoms, Convert.ToInt32(row["PestBBInsectSigns"]), this.DctBBInsectSigns);
        this.StoreHitOrMiss(PestInfo, "Branches/Bole", "BBInsectPres", dSymptoms, Convert.ToInt32(row["PestBBInsectPres"]), this.DctBBInsectPres);
        this.StoreHitOrMiss(PestInfo, "Branches/Bole", "BBDiseaseSigns", dSymptoms, Convert.ToInt32(row["PestBBDiseaseSigns"]), this.DctBBDiseaseSigns);
        this.StoreHitOrMiss(PestInfo, "Branches/Bole", "BBProbLoc", dSymptoms, Convert.ToInt32(row["PestBBProbLoc"]), this.DctBBProbLoc);
        this.StoreHitOrMiss(PestInfo, "Branches/Bole", "BBAbnGrowth", dSymptoms, Convert.ToInt32(row["PestBBAbnGrowth"]), this.DctBBAbnGrowth);
        int num3 = 0;
        foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair in PestInfo.Hit)
          num3 += keyValuePair.Value.Count;
        if (num3 == dSymptoms.Count && num3 > 0)
        {
          ++num2;
          this.IncHitOrMiss(hmInfo, "Tree Stress", "TSDieback", dSymptoms, Convert.ToInt32(row["PestTSDieback"]), this.DctTSDieback);
          this.IncHitOrMiss(hmInfo, "Tree Stress", "TSEpiSprout", dSymptoms, Convert.ToInt32(row["PestTSEpiSprout"]), this.DctTSEpiSprout);
          this.IncHitOrMiss(hmInfo, "Tree Stress", "TSWiltFoli", dSymptoms, Convert.ToInt32(row["PestTSWiltFoli"]), this.DctTSWiltFoli);
          this.IncHitOrMiss(hmInfo, "Tree Stress", "TSEnvStress", dSymptoms, Convert.ToInt32(row["PestTSEnvStress"]), this.DctTSEnvStress);
          this.IncHitOrMiss(hmInfo, "Tree Stress", "TSHumStress", dSymptoms, Convert.ToInt32(row["PestTSHumStress"]), this.DctTSHumStress);
          this.IncHitOrMiss(hmInfo, "Foliage/Twigs", "FTChewFoli", dSymptoms, Convert.ToInt32(row["PestFTChewFoli"]), this.DctFTChewFoli);
          this.IncHitOrMiss(hmInfo, "Foliage/Twigs", "FTDiscFoli", dSymptoms, Convert.ToInt32(row["PestFTDiscFoli"]), this.DctFTDiscFoli);
          this.IncHitOrMiss(hmInfo, "Foliage/Twigs", "FTAbnFoli", dSymptoms, Convert.ToInt32(row["PestFTAbnFoli"]), this.DctFTAbnFoli);
          this.IncHitOrMiss(hmInfo, "Foliage/Twigs", "FTInsectSigns", dSymptoms, Convert.ToInt32(row["PestFTInsectSigns"]), this.DctFTInsectSigns);
          this.IncHitOrMiss(hmInfo, "Foliage/Twigs", "FTFoliAffect", dSymptoms, Convert.ToInt32(row["PestFTFoliAffect"]), this.DctFTFoliAffect);
          this.IncHitOrMiss(hmInfo, "Branches/Bole", "BBInsectSigns", dSymptoms, Convert.ToInt32(row["PestBBInsectSigns"]), this.DctBBInsectSigns);
          this.IncHitOrMiss(hmInfo, "Branches/Bole", "BBInsectPres", dSymptoms, Convert.ToInt32(row["PestBBInsectPres"]), this.DctBBInsectPres);
          this.IncHitOrMiss(hmInfo, "Branches/Bole", "BBDiseaseSigns", dSymptoms, Convert.ToInt32(row["PestBBDiseaseSigns"]), this.DctBBDiseaseSigns);
          this.IncHitOrMiss(hmInfo, "Branches/Bole", "BBProbLoc", dSymptoms, Convert.ToInt32(row["PestBBProbLoc"]), this.DctBBProbLoc);
          this.IncHitOrMiss(hmInfo, "Branches/Bole", "BBAbnGrowth", dSymptoms, Convert.ToInt32(row["PestBBAbnGrowth"]), this.DctBBAbnGrowth);
          int num4 = 0;
          foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair in PestInfo.Miss)
            num4 += keyValuePair.Value.Count;
          List<IPEDInfo> ipedInfoList = (List<IPEDInfo>) null;
          if (!sortedDictionary.TryGetValue(row["SpCode"].ToString(), out ipedInfoList))
          {
            ipedInfoList = new List<IPEDInfo>();
            sortedDictionary[row["SpCode"].ToString()] = ipedInfoList;
          }
          PestInfo.dr = row;
          ipedInfoList.Add(PestInfo);
        }
        ++num1;
      }
      DataSet recordsForSymptoms = new DataSet("Records");
      DataTable dataTable1 = recordsForSymptoms.Tables.Add("Data");
      dataTable1.Columns.AddRange(new DataColumn[8]
      {
        new DataColumn("Category", typeof (string)),
        new DataColumn("Species", typeof (string)),
        new DataColumn("PlotId", typeof (int)),
        new DataColumn("TreeId", typeof (int)),
        new DataColumn("Strata", typeof (string)),
        new DataColumn("Field Landuse", typeof (string)),
        new DataColumn("Street Tree", typeof (string)),
        new DataColumn("Address", typeof (string))
      });
      dataTable1.ExtendedProperties.Add((object) "GroupBy", (object) new string[2]
      {
        "Category",
        "Species"
      });
      dataTable1.ExtendedProperties.Add((object) "DefaultLevel", (object) 1);
      DataTable dataTable2 = dataTable1;
      DataTable dataTable3 = recordsForSymptoms.Tables.Add("TreeSymptoms");
      dataTable3.Columns.AddRange(new DataColumn[6]
      {
        new DataColumn("PlotId", typeof (int)),
        new DataColumn("TreeId", typeof (int)),
        new DataColumn("Category", typeof (string)),
        new DataColumn("SubCategory", typeof (string)),
        new DataColumn("Sign/Symptom", typeof (string)),
        new DataColumn("Value", typeof (string))
      });
      dataTable3.ExtendedProperties.Add((object) "HideColumns", (object) new string[2]
      {
        "PlotId",
        "TreeId"
      });
      dataTable3.ExtendedProperties.Add((object) "GroupBy", (object) new string[2]
      {
        "Category",
        "SubCategory"
      });
      dataTable3.ExtendedProperties.Add((object) "DefaultLevel", (object) 2);
      DataTable dataTable4 = dataTable3;
      DataTable dataTable5 = recordsForSymptoms.Tables.Add("SymptomStats");
      dataTable5.Columns.AddRange(new DataColumn[6]
      {
        new DataColumn("Category", typeof (string)),
        new DataColumn("SubCategory", typeof (string)),
        new DataColumn("Sign/Symptom", typeof (string)),
        new DataColumn("Count", typeof (string)),
        new DataColumn("RawCount", typeof (int)),
        new DataColumn("RawSS", typeof (int))
      });
      dataTable5.ExtendedProperties.Add((object) "HideColumns", (object) new string[2]
      {
        "RawCount",
        "RawSS"
      });
      dataTable5.ExtendedProperties.Add((object) "GroupBy", (object) new string[2]
      {
        "Category",
        "SubCategory"
      });
      dataTable5.ExtendedProperties.Add((object) "DefaultLevel", (object) 2);
      DataTable dataTable6 = dataTable5;
      recordsForSymptoms.Relations.Add(new DataRelation("RecordSymptoms", new DataColumn[2]
      {
        dataTable2.Columns["PlotID"],
        dataTable2.Columns["TreeId"]
      }, new DataColumn[2]
      {
        dataTable4.Columns["PlotID"],
        dataTable4.Columns["TreeId"]
      }, true));
      recordsForSymptoms.DefaultViewManager.DataViewSettings["Data"].Sort = "Species ASC";
      recordsForSymptoms.DefaultViewManager.DataViewSettings["SymptomStats"].Sort = "Category DESC, SubCategory ASC, RawCount DESC, RawSS ASC";
      foreach (KeyValuePair<string, List<IPEDInfo>> keyValuePair1 in sortedDictionary)
      {
        foreach (IPEDInfo ipedInfo in keyValuePair1.Value)
        {
          DataRow dr = ipedInfo.dr;
          DataRow row1 = dataTable2.NewRow();
          row1["Category"] = (object) string.Format("{0} Trees Match the Selected Sign/Symptom Categories [{1:0.00%} of All Trees]", new object[2]
          {
            (object) num2,
            (object) (num2 / num1)
          });
          row1["Species"] = (object) string.Format("{0} [{1} Trees, {2:0.00%} of All Trees]", (object) this.GetSpeciesName(dr["SpCode"].ToString()), (object) keyValuePair1.Value.Count, (object) (keyValuePair1.Value.Count / num1));
          row1["PlotId"] = dr["PlotID"];
          row1["TreeId"] = dr["TreeId"];
          row1["Strata"] = dr["MapLanduseDescription"];
          row1["Field Landuse"] = dr["FieldLandUseDescription"];
          row1["Address"] = dr["Address"];
          row1["Street Tree"] = (bool) dr["TreeSite"] ? (object) "Yes" : (object) "No";
          dataTable2.Rows.Add(row1);
          foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair2 in ipedInfo.Hit)
          {
            foreach (KeyValuePair<string, int> keyValuePair3 in keyValuePair2.Value)
            {
              DataRow row2 = dataTable4.NewRow();
              row2["PlotId"] = dr["PlotId"];
              row2["TreeId"] = dr["TreeId"];
              row2["Category"] = (object) "Matching Signs/Symptoms";
              row2["SubCategory"] = (object) keyValuePair2.Key;
              row2["Sign/Symptom"] = (object) staticData.GetIPEDFieldName(keyValuePair3.Key);
              row2["Value"] = (object) this.GetIPEDDescription(keyValuePair3.Key, keyValuePair3.Value);
              dataTable4.Rows.Add(row2);
            }
          }
          foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair4 in ipedInfo.Miss)
          {
            foreach (KeyValuePair<string, int> keyValuePair5 in keyValuePair4.Value)
            {
              DataRow row3 = dataTable4.NewRow();
              row3["PlotId"] = dr["PlotId"];
              row3["TreeId"] = dr["TreeId"];
              row3["Category"] = (object) "Additional Signs/Symptoms";
              row3["SubCategory"] = (object) keyValuePair4.Key;
              row3["Sign/Symptom"] = (object) staticData.GetIPEDFieldName(keyValuePair5.Key);
              row3["Value"] = (object) this.GetIPEDDescription(keyValuePair5.Key, keyValuePair5.Value);
              dataTable4.Rows.Add(row3);
            }
          }
        }
      }
      foreach (KeyValuePair<string, Dictionary<string, Dictionary<int, int>>> keyValuePair6 in hmInfo.Miss)
      {
        foreach (KeyValuePair<string, Dictionary<int, int>> keyValuePair7 in keyValuePair6.Value)
        {
          foreach (KeyValuePair<int, int> keyValuePair8 in keyValuePair7.Value)
          {
            DataRow row = dataTable6.NewRow();
            row["Category"] = (object) keyValuePair6.Key;
            row["SubCategory"] = (object) staticData.GetIPEDFieldName(keyValuePair7.Key);
            row["Sign/Symptom"] = (object) this.GetIPEDDescription(keyValuePair7.Key, keyValuePair8.Key);
            row["Count"] = (object) keyValuePair8.Value.ToString();
            row["RawCount"] = (object) keyValuePair8.Value;
            row["RawSS"] = (object) keyValuePair8.Key;
            dataTable6.Rows.Add(row);
          }
        }
      }
      return recordsForSymptoms;
    }

    private string GetIPEDDescription(string sField, int sValue)
    {
      Dictionary<int, string> pestLc = this.GetPestLC(sField);
      return !pestLc.ContainsKey(sValue) ? sValue.ToString() : pestLc[sValue];
    }

    private void IncHitOrMiss(
      HitMiss hmInfo,
      string Category,
      string SubCategory,
      Dictionary<string, List<int>> dSymptoms,
      int value,
      Dictionary<int, string> dvLookup)
    {
      List<int> intList = (List<int>) null;
      Dictionary<string, Dictionary<int, int>> dictionary1 = (Dictionary<string, Dictionary<int, int>>) null;
      Dictionary<int, int> dictionary2 = (Dictionary<int, int>) null;
      int num1 = 0;
      if (dSymptoms.TryGetValue(SubCategory, out intList))
      {
        if (!intList.Contains(value))
          return;
        if (!hmInfo.Hit.TryGetValue(Category, out dictionary1))
        {
          dictionary1 = new Dictionary<string, Dictionary<int, int>>();
          hmInfo.Hit[Category] = dictionary1;
        }
        if (!dictionary1.TryGetValue(SubCategory, out dictionary2))
        {
          dictionary2 = new Dictionary<int, int>();
          dictionary1[SubCategory] = dictionary2;
        }
        dictionary2.TryGetValue(value, out num1);
        int num2 = num1 + 1;
        dictionary2[value] = num2;
      }
      else
      {
        if (!dvLookup.ContainsKey(value))
          return;
        if (!hmInfo.Miss.TryGetValue(Category, out dictionary1))
        {
          dictionary1 = new Dictionary<string, Dictionary<int, int>>();
          hmInfo.Miss[Category] = dictionary1;
        }
        if (!dictionary1.TryGetValue(SubCategory, out dictionary2))
        {
          dictionary2 = new Dictionary<int, int>();
          dictionary1[SubCategory] = dictionary2;
        }
        dictionary2.TryGetValue(value, out num1);
        int num3 = num1 + 1;
        dictionary2[value] = num3;
      }
    }

    private Dictionary<int, string> GetPestLC(string sField)
    {
      Dictionary<int, string> pestLc = (Dictionary<int, string>) null;
      switch (sField.ToLower())
      {
        case "bbabngrowth":
          pestLc = this.DctBBAbnGrowth;
          break;
        case "bbdiseasesigns":
          pestLc = this.DctBBDiseaseSigns;
          break;
        case "bbinsectpres":
          pestLc = this.DctBBInsectPres;
          break;
        case "bbinsectsigns":
          pestLc = this.DctBBInsectSigns;
          break;
        case "bbprobloc":
          pestLc = this.DctBBProbLoc;
          break;
        case "ftabnfoli":
          pestLc = this.DctFTAbnFoli;
          break;
        case "ftchewfoli":
          pestLc = this.DctFTChewFoli;
          break;
        case "ftdiscfoli":
          pestLc = this.DctFTDiscFoli;
          break;
        case "ftfoliaffect":
          pestLc = this.DctFTFoliAffect;
          break;
        case "ftinsectsigns":
          pestLc = this.DctFTInsectSigns;
          break;
        case "tsdieback":
          pestLc = this.DctTSDieback;
          break;
        case "tsenvstress":
          pestLc = this.DctTSEnvStress;
          break;
        case "tsepisprout":
          pestLc = this.DctTSEpiSprout;
          break;
        case "tshumstress":
          pestLc = this.DctTSHumStress;
          break;
        case "tswiltfoli":
          pestLc = this.DctTSWiltFoli;
          break;
      }
      return pestLc;
    }

    private void StoreHitOrMiss(
      IPEDInfo PestInfo,
      string Category,
      string SubCategory,
      Dictionary<string, List<int>> dSymptoms,
      int value,
      Dictionary<int, string> dvLookup)
    {
      List<int> intList;
      if (dSymptoms.TryGetValue(SubCategory, out intList))
      {
        if (!intList.Contains(value))
          return;
        Dictionary<string, int> dictionary;
        if (!PestInfo.Hit.TryGetValue(Category, out dictionary))
        {
          dictionary = new Dictionary<string, int>();
          PestInfo.Hit[Category] = dictionary;
        }
        dictionary[SubCategory] = value;
      }
      else
      {
        if (!dvLookup.ContainsKey(value))
          return;
        Dictionary<string, int> dictionary;
        if (!PestInfo.Miss.TryGetValue(Category, out dictionary))
        {
          dictionary = new Dictionary<string, int>();
          PestInfo.Miss[Category] = dictionary;
        }
        dictionary[SubCategory] = value;
      }
    }

    public Dictionary<string, List<int>> GetSymptomsForPest(int PestId)
    {
      Dictionary<string, List<int>> symptomsForPest = new Dictionary<string, List<int>>();
      DataTable table = staticData.DsData.Tables["Lucid"];
      DataRow dataRow = table.Rows.Find((object) PestId);
      if (dataRow != null)
      {
        foreach (DataColumn column in (InternalDataCollectionBase) table.Columns)
        {
          bool flag = !dataRow[column.ColumnName].Equals((object) 0);
          string[] strArray = column.ColumnName.Split("_".ToCharArray());
          if (strArray.Length == 2 & flag)
          {
            string key = strArray[0];
            int result = 0;
            if (int.TryParse(strArray[1], out result))
            {
              List<int> intList = (List<int>) null;
              if (!symptomsForPest.TryGetValue(key, out intList))
              {
                intList = new List<int>();
                symptomsForPest.Add(key, intList);
              }
              intList.Add(result);
            }
          }
        }
      }
      return symptomsForPest;
    }

    public List<string> GetHostsForPest(int PestId)
    {
      List<string> hostsForPest = new List<string>();
      DataTable table = staticData.DsData.Tables["Lucid"];
      DataRow dataRow = table.Rows.Find((object) PestId);
      if (dataRow != null)
      {
        foreach (DataColumn column in (InternalDataCollectionBase) table.Columns)
        {
          bool flag = !dataRow[column.ColumnName].Equals((object) 0);
          string[] strArray = column.ColumnName.Split("_".ToCharArray());
          if (strArray.Length == 2 & flag)
          {
            int result = 0;
            if (!int.TryParse(strArray[1], out result))
              hostsForPest.Add(strArray[1].ToUpper());
          }
        }
      }
      return hostsForPest;
    }

    public string GetPestName(int PestId)
    {
      DataRow dataRow = this.m_dtPests.Rows.Find((object) PestId);
      return PestId == -1 ? "-Unknown-" : (dataRow != null ? (!staticData.UseScientificName ? (string) dataRow["CommonName"] : (string) dataRow["ScientificName"]) : PestId.ToString());
    }

    public Dictionary<string, PestStats> CreateSSSummaryTotalDictionary() => new Dictionary<string, PestStats>()
    {
      ["Tree Stress"] = new PestStats(),
      ["Foliage/Twigs"] = new PestStats(),
      ["Branches/Bole"] = new PestStats()
    };

    public Dictionary<string, List<DataRow>> GetAffectedSpeciesDictionaryList()
    {
      Dictionary<string, List<DataRow>> speciesDictionaryList = new Dictionary<string, List<DataRow>>();
      foreach (DataRowView dataRowView in new DataView(staticData.DsData.Tables["Trees"], "PestPest <> 0", string.Empty, DataViewRowState.CurrentRows))
      {
        if (!speciesDictionaryList.ContainsKey((string) dataRowView["SpCode"]))
          speciesDictionaryList[(string) dataRowView["SpCode"]] = new List<DataRow>();
        speciesDictionaryList[(string) dataRowView["SpCode"]].Add(dataRowView.Row);
      }
      return speciesDictionaryList;
    }

    private DataView GetAffectedTreeListSample(string CurMapLanduse)
    {
      List<DataRow> dataRowList = new List<DataRow>();
      DataTable table = staticData.DsData.Tables["Trees"];
      DataView affectedTreeListSample = string.IsNullOrEmpty(CurMapLanduse) || CurMapLanduse == i_Tree_Eco_v6.Resources.Strings.AllStrata ? new DataView(table, "PestPest <> 0", string.Empty, DataViewRowState.CurrentRows) : new DataView(table, "PestPest <> 0 AND MapLandUseDescription = '" + CurMapLanduse + "'", string.Empty, DataViewRowState.CurrentRows);
      affectedTreeListSample.Sort = "PestPest ASC";
      return affectedTreeListSample;
    }

    public Dictionary<string, List<DataRow>> GetAllSpeciesDictionaryList()
    {
      Dictionary<string, List<DataRow>> speciesDictionaryList = new Dictionary<string, List<DataRow>>();
      DataTable table = staticData.DsData.Tables["Trees"];
      foreach (DataRow row in (InternalDataCollectionBase) new DataView(table, string.Empty, string.Empty, DataViewRowState.CurrentRows).ToTable(true, "SpCode").Rows)
      {
        foreach (DataRowView dataRowView in new DataView(table, "SpCode='" + row["SpCode"]?.ToString() + "'", "PlotID, TreeID", DataViewRowState.CurrentRows))
        {
          if (!speciesDictionaryList.ContainsKey((string) row["SpCode"]))
            speciesDictionaryList[(string) row["SpCode"]] = new List<DataRow>();
          speciesDictionaryList[(string) row["SpCode"]].Add(dataRowView.Row);
        }
      }
      return speciesDictionaryList;
    }

    public int GetAllTreeCount() => new DataView(staticData.DsData.Tables["Trees"], string.Empty, string.Empty, DataViewRowState.CurrentRows).Count;

    public int GetPestAffectedTreeCount() => new DataView(staticData.DsData.Tables["Trees"], "PestPest <> 0", string.Empty, DataViewRowState.CurrentRows).Count;

    public DataSet BuildPrimaryPestDetailsForLandusesSample(string CurMapLanduse)
    {
      DataSet dataSet = new DataSet();
      DataTable table1 = staticData.DsData.Tables["Trees"];
      DataTable table2 = new DataTable("Trees");
      table2.Columns.Add(i_Tree_Eco_v6.Resources.Strings.PestGrouping, typeof (string));
      table2.Columns.Add(i_Tree_Eco_v6.Resources.Strings.PlotID, typeof (int));
      table2.Columns.Add(i_Tree_Eco_v6.Resources.Strings.TreeID, typeof (int));
      table2.Columns.Add(i_Tree_Eco_v6.Resources.Strings.Species, typeof (string));
      table2.Columns.Add(v6Strings.Strata_SingularName, typeof (string));
      table2.Columns.Add(i_Tree_Eco_v6.Resources.Strings.Address, typeof (string));
      dataSet.Tables.Add(table2);
      DataView affectedTreeListSample = this.GetAffectedTreeListSample(CurMapLanduse);
      affectedTreeListSample.Sort = "PestPest, PlotId, TreeId";
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      foreach (DataRowView dataRowView in affectedTreeListSample)
      {
        if ((string) dataRowView["MapLanduseDescription"] == CurMapLanduse || CurMapLanduse == i_Tree_Eco_v6.Resources.Strings.AllStrata)
        {
          if (dictionary.ContainsKey(this.GetPestName((int) dataRowView["PestPest"])))
            dictionary[this.GetPestName((int) dataRowView["PestPest"])]++;
          else
            dictionary.Add(this.GetPestName((int) dataRowView["PestPest"]), 1);
        }
      }
      foreach (DataRowView dataRowView in affectedTreeListSample)
      {
        if (CurMapLanduse == i_Tree_Eco_v6.Resources.Strings.AllStrata || (string) dataRowView["MapLandUseDescription"] == CurMapLanduse)
        {
          DataRow row = table2.NewRow();
          int num = dictionary[this.GetPestName((int) dataRowView["PestPest"])];
          row[i_Tree_Eco_v6.Resources.Strings.PestGrouping] = (object) string.Format("{0} - [{1} {2}]", (object) this.GetPestName((int) dataRowView["PestPest"]), (object) num.ToString(), num > 1 ? (object) v6Strings.Tree_PluralName : (object) v6Strings.Tree_SingularName);
          row[i_Tree_Eco_v6.Resources.Strings.PlotID] = dataRowView["PlotID"];
          row[i_Tree_Eco_v6.Resources.Strings.TreeID] = dataRowView["TreeID"];
          row[i_Tree_Eco_v6.Resources.Strings.Species] = (object) this.GetSpeciesName((string) dataRowView["SpCode"]);
          row[v6Strings.Strata_SingularName] = dataRowView["MapLandUseDescription"];
          row[i_Tree_Eco_v6.Resources.Strings.Address] = dataRowView["Address"];
          table2.Rows.Add(row);
        }
      }
      table2.DefaultView.Sort = i_Tree_Eco_v6.Resources.Strings.PestGrouping;
      table2.ExtendedProperties.Add((object) "GroupBy", (object) new string[1]
      {
        i_Tree_Eco_v6.Resources.Strings.PestGrouping
      });
      return dataSet;
    }

    public Dictionary<string, Dictionary<string, PestStats>> CreateSSSummaryDetailDictionary()
    {
      Dictionary<string, Dictionary<string, PestStats>> detailDictionary = new Dictionary<string, Dictionary<string, PestStats>>();
      detailDictionary["Tree Stress"] = new Dictionary<string, PestStats>();
      detailDictionary["Tree Stress"]["Dieback"] = new PestStats();
      detailDictionary["Tree Stress"]["Epicormic Sprouts"] = new PestStats();
      detailDictionary["Tree Stress"]["Wilted Foliage"] = new PestStats();
      detailDictionary["Tree Stress"]["Environmental Stress"] = new PestStats();
      detailDictionary["Tree Stress"]["Human caused Stress"] = new PestStats();
      if (!staticData.IsSample)
        detailDictionary["Tree Stress"]["Notes Present"] = new PestStats();
      detailDictionary["Foliage/Twigs"] = new Dictionary<string, PestStats>();
      detailDictionary["Foliage/Twigs"]["Defoliation"] = new PestStats();
      detailDictionary["Foliage/Twigs"]["Discolored Foliage"] = new PestStats();
      detailDictionary["Foliage/Twigs"]["Abnormal Foliage"] = new PestStats();
      detailDictionary["Foliage/Twigs"]["Insect Signs"] = new PestStats();
      detailDictionary["Foliage/Twigs"]["% Foliage Affected"] = new PestStats();
      if (!staticData.IsSample)
        detailDictionary["Foliage/Twigs"]["Notes Present"] = new PestStats();
      detailDictionary["Branches/Bole"] = new Dictionary<string, PestStats>();
      detailDictionary["Branches/Bole"]["Insect Signs"] = new PestStats();
      detailDictionary["Branches/Bole"]["Insect Presence"] = new PestStats();
      detailDictionary["Branches/Bole"]["Disease Signs"] = new PestStats();
      detailDictionary["Branches/Bole"]["Problem Location"] = new PestStats();
      detailDictionary["Branches/Bole"]["Loose Bark"] = new PestStats();
      if (!staticData.IsSample)
        detailDictionary["Branches/Bole"]["Notes Present"] = new PestStats();
      return detailDictionary;
    }

    public Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>> CreateSSDCDetailsDictionary()
    {
      Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>> detailsDictionary = new Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>();
      detailsDictionary["Tree Stress"] = new Dictionary<string, Dictionary<string, PestStats>>();
      detailsDictionary["Tree Stress"]["Dieback"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctTSDieback.Values)
        detailsDictionary["Tree Stress"]["Dieback"][key] = new PestStats();
      detailsDictionary["Tree Stress"]["Epicormic Sprouts"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctTSEpiSprout.Values)
        detailsDictionary["Tree Stress"]["Epicormic Sprouts"][key] = new PestStats();
      detailsDictionary["Tree Stress"]["Wilted Foliage"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctTSWiltFoli.Values)
        detailsDictionary["Tree Stress"]["Wilted Foliage"][key] = new PestStats();
      detailsDictionary["Tree Stress"]["Environmental Stress"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctTSEnvStress.Values)
        detailsDictionary["Tree Stress"]["Environmental Stress"][key] = new PestStats();
      detailsDictionary["Tree Stress"]["Human caused Stress"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctTSHumStress.Values)
        detailsDictionary["Tree Stress"]["Human caused Stress"][key] = new PestStats();
      if (!staticData.IsSample)
      {
        detailsDictionary["Tree Stress"]["Notes Present"] = new Dictionary<string, PestStats>();
        detailsDictionary["Tree Stress"]["Notes Present"][string.Empty] = new PestStats();
      }
      detailsDictionary["Foliage/Twigs"] = new Dictionary<string, Dictionary<string, PestStats>>();
      detailsDictionary["Foliage/Twigs"]["Defoliation"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctFTChewFoli.Values)
        detailsDictionary["Foliage/Twigs"]["Defoliation"][key] = new PestStats();
      detailsDictionary["Foliage/Twigs"]["Discolored Foliage"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctFTDiscFoli.Values)
        detailsDictionary["Foliage/Twigs"]["Discolored Foliage"][key] = new PestStats();
      detailsDictionary["Foliage/Twigs"]["Abnormal Foliage"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctFTAbnFoli.Values)
        detailsDictionary["Foliage/Twigs"]["Abnormal Foliage"][key] = new PestStats();
      detailsDictionary["Foliage/Twigs"]["Insect Signs"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctFTInsectSigns.Values)
        detailsDictionary["Foliage/Twigs"]["Insect Signs"][key] = new PestStats();
      detailsDictionary["Foliage/Twigs"]["% Foliage Affected"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctFTFoliAffect.Values)
        detailsDictionary["Foliage/Twigs"]["% Foliage Affected"][key] = new PestStats();
      if (!staticData.IsSample)
      {
        detailsDictionary["Foliage/Twigs"]["Notes Present"] = new Dictionary<string, PestStats>();
        detailsDictionary["Foliage/Twigs"]["Notes Present"][string.Empty] = new PestStats();
      }
      detailsDictionary["Branches/Bole"] = new Dictionary<string, Dictionary<string, PestStats>>();
      detailsDictionary["Branches/Bole"]["Insect Signs"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctBBInsectSigns.Values)
        detailsDictionary["Branches/Bole"]["Insect Signs"][key] = new PestStats();
      detailsDictionary["Branches/Bole"]["Insect Presence"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctBBInsectPres.Values)
        detailsDictionary["Branches/Bole"]["Insect Presence"][key] = new PestStats();
      detailsDictionary["Branches/Bole"]["Disease Signs"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctBBDiseaseSigns.Values)
        detailsDictionary["Branches/Bole"]["Disease Signs"][key] = new PestStats();
      detailsDictionary["Branches/Bole"]["Problem Location"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctBBProbLoc.Values)
        detailsDictionary["Branches/Bole"]["Problem Location"][key] = new PestStats();
      detailsDictionary["Branches/Bole"]["Loose Bark"] = new Dictionary<string, PestStats>();
      foreach (string key in this.DctBBAbnGrowth.Values)
        detailsDictionary["Branches/Bole"]["Loose Bark"][key] = new PestStats();
      if (!staticData.IsSample)
      {
        detailsDictionary["Branches/Bole"]["Notes Present"] = new Dictionary<string, PestStats>();
        detailsDictionary["Branches/Bole"]["Notes Present"][string.Empty] = new PestStats();
      }
      return detailsDictionary;
    }

    public double GetEstTotalAffected() => (double) staticData.DsData.Tables["Species_PestPest"].Compute("SUM(EstimateValue)", "PestPest=" + staticData.PestPestAffectedClassValueOrder.ToString());

    public double GetEstTotalSpecies(short SpeciesClassValueOrder)
    {
      try
      {
        return (double) staticData.DsData.Tables["Species_PestPest"].Compute("SUM(EstimateValue)", string.Format("Species={0} AND (PestPest={1} OR PestPest={2})", new object[3]
        {
          (object) SpeciesClassValueOrder,
          (object) staticData.PestPestAffectedClassValueOrder,
          (object) staticData.PestPestNoneClassValueOrder
        }));
      }
      catch
      {
        return 0.0;
      }
    }

    public double GetEstTotalLanduse(int LanduseClassValueOrder) => (double) staticData.DsData.Tables["Landuse_PestPest"].Compute("SUM(EstimateValue)", string.Format("Strata={0} AND (PestPest={1} OR PestPest={2})", new object[3]
    {
      (object) LanduseClassValueOrder,
      (object) staticData.PestPestAffectedClassValueOrder,
      (object) staticData.PestPestNoneClassValueOrder
    }));

    public PestStats GetEstSpeciesAffectedStats(int SpeciesClassValueOrder)
    {
      DataTable table = staticData.DsData.Tables["Species_PestPest"];
      PestStats speciesAffectedStats = new PestStats();
      string filterExpression = "Species=" + SpeciesClassValueOrder.ToString() + " AND PestPest=" + staticData.PestPestAffectedClassValueOrder.ToString();
      DataRow[] dataRowArray = table.Select(filterExpression);
      if (dataRowArray.Length == 1)
      {
        speciesAffectedStats.PopEst = (double) dataRowArray[0]["EstimateValue"];
        speciesAffectedStats.StdErr = (double) dataRowArray[0]["EstimateStandardError"];
      }
      return speciesAffectedStats;
    }

    public PestStats GetEstLanduseAffectedStats(int LanduseClassValueOrder)
    {
      DataTable table = staticData.DsData.Tables["Landuse_PestPest"];
      PestStats landuseAffectedStats = new PestStats();
      string filterExpression = "Strata=" + LanduseClassValueOrder.ToString() + " AND PestPest=" + staticData.PestPestAffectedClassValueOrder.ToString();
      DataRow[] dataRowArray = table.Select(filterExpression);
      if (dataRowArray.Length == 1)
      {
        landuseAffectedStats.PopEst = (double) dataRowArray[0]["EstimateValue"];
        landuseAffectedStats.StdErr = (double) dataRowArray[0]["EstimateStandardError"];
      }
      return landuseAffectedStats;
    }

    public double GetEstAllTreeCount() => (double) staticData.DsData.Tables["Species_PestPest"].Compute("SUM(EstimateValue)", string.Format("PestPest={0} OR PestPest={1}", new object[2]
    {
      (object) staticData.PestPestAffectedClassValueOrder,
      (object) staticData.PestPestNoneClassValueOrder
    }));

    public Dictionary<int, Dictionary<string, Dictionary<string, PestStats>>> GetEstSpeciesSignAndSymptomDetailsSummaryDictionary()
    {
      Dictionary<int, Dictionary<string, Dictionary<string, PestStats>>> summaryDictionary = new Dictionary<int, Dictionary<string, Dictionary<string, PestStats>>>();
      foreach (DataTable table in (InternalDataCollectionBase) staticData.DsData.Tables)
      {
        string[] strArray = table.TableName.Split('_');
        if (strArray.Length == 2 && strArray[0] == "Species" && !string.IsNullOrEmpty(staticData.GetIPEDSignSymptomTypeLocationForField(strArray[1])))
        {
          string str = strArray[1];
          foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
          {
            int int32 = Convert.ToInt32(row["Species"]);
            Classifiers estIpedClassifier = staticData.GetEstIPEDClassifier(str);
            if (!summaryDictionary.ContainsKey(int32))
              summaryDictionary[int32] = this.CreateSSSummaryDetailDictionary();
            if (Convert.ToInt32(row[str]) != staticData.dctClassifierNoneValues[estIpedClassifier])
              summaryDictionary[int32][staticData.GetIPEDSignSymptomTypeLocationForField(str)][staticData.GetIPEDFieldName(str)].PopEst += (double) row["EstimateValue"];
          }
        }
      }
      return summaryDictionary;
    }

    public Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>> GetEstSpeciesSignAndSymptomDetailsCompleteDictionary()
    {
      Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>> completeDictionary = new Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>>();
      double estTotalAffected = this.GetEstTotalAffected();
      double estAllTreeCount = this.GetEstAllTreeCount();
      foreach (DataTable table in (InternalDataCollectionBase) staticData.DsData.Tables)
      {
        string[] strArray = table.TableName.Split('_');
        if (strArray.Length == 2 && strArray[0] == "Species" && !string.IsNullOrEmpty(staticData.GetIPEDSignSymptomTypeLocationForField(strArray[1])))
        {
          string str = strArray[1];
          foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
          {
            double estTotalSpecies = this.GetEstTotalSpecies(Convert.ToInt16(row["Species"]));
            int int32 = Convert.ToInt32(row["Species"]);
            Classifiers estIpedClassifier = staticData.GetEstIPEDClassifier(str);
            if (!completeDictionary.ContainsKey(int32))
              completeDictionary[int32] = this.CreateSSDCDetailsDictionary();
            if ((int) Convert.ToInt16(row[str]) != staticData.dctClassifierNoneValues[estIpedClassifier])
            {
              completeDictionary[int32][staticData.GetIPEDSignSymptomTypeLocationForField(str)][staticData.GetIPEDFieldName(str)][staticData.GetClassValueName(estIpedClassifier, Convert.ToInt16(row[str]))].PopEst += (double) row["EstimateValue"];
              completeDictionary[int32][staticData.GetIPEDSignSymptomTypeLocationForField(str)][staticData.GetIPEDFieldName(str)][staticData.GetClassValueName(estIpedClassifier, Convert.ToInt16(row[str]))].StdErr = (double) row["EstimateStandardError"];
              completeDictionary[int32][staticData.GetIPEDSignSymptomTypeLocationForField(str)][staticData.GetIPEDFieldName(str)][staticData.GetClassValueName(estIpedClassifier, Convert.ToInt16(row[str]))].Pct = (double) row["EstimateValue"] / estAllTreeCount;
              completeDictionary[int32][staticData.GetIPEDSignSymptomTypeLocationForField(str)][staticData.GetIPEDFieldName(str)][staticData.GetClassValueName(estIpedClassifier, Convert.ToInt16(row[str]))].PctAffected = (double) row["EstimateValue"] / estTotalAffected;
              completeDictionary[int32][staticData.GetIPEDSignSymptomTypeLocationForField(str)][staticData.GetIPEDFieldName(str)][staticData.GetClassValueName(estIpedClassifier, Convert.ToInt16(row[str]))].PctTree = (double) row["EstimateValue"] / estTotalSpecies;
            }
          }
        }
      }
      return completeDictionary;
    }

    public Dictionary<int, Dictionary<string, Dictionary<string, PestStats>>> GetEstLanduseSignAndSymptomDetailsSummaryDictionary()
    {
      Dictionary<int, Dictionary<string, Dictionary<string, PestStats>>> summaryDictionary = new Dictionary<int, Dictionary<string, Dictionary<string, PestStats>>>();
      foreach (DataTable table in (InternalDataCollectionBase) staticData.DsData.Tables)
      {
        string[] strArray = table.TableName.Split('_');
        if (strArray.Length == 2 && strArray[0] == "Landuse" && !string.IsNullOrEmpty(staticData.GetIPEDSignSymptomTypeLocationForField(strArray[1])))
        {
          string str = strArray[1];
          foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
          {
            int int32 = Convert.ToInt32(row["Strata"]);
            Classifiers estIpedClassifier = staticData.GetEstIPEDClassifier(str);
            if (!summaryDictionary.ContainsKey(int32))
              summaryDictionary[int32] = this.CreateSSSummaryDetailDictionary();
            if (Convert.ToInt32(row[str]) != staticData.dctClassifierNoneValues[estIpedClassifier])
              summaryDictionary[int32][staticData.GetIPEDSignSymptomTypeLocationForField(str)][staticData.GetIPEDFieldName(str)].PopEst += (double) row["EstimateValue"];
          }
        }
      }
      return summaryDictionary;
    }

    public Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>> GetEstLanduseSignAndSymptomDetailsCompleteDictionary()
    {
      Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>> completeDictionary = new Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>>();
      double estTotalAffected = this.GetEstTotalAffected();
      double estAllTreeCount = this.GetEstAllTreeCount();
      foreach (DataTable table in (InternalDataCollectionBase) staticData.DsData.Tables)
      {
        string[] strArray = table.TableName.Split('_');
        if (strArray.Length == 2 && strArray[0] == "Landuse" && !string.IsNullOrEmpty(staticData.GetIPEDSignSymptomTypeLocationForField(strArray[1])))
        {
          string str = strArray[1];
          foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
          {
            int int32 = Convert.ToInt32(row["Strata"]);
            double estTotalLanduse = this.GetEstTotalLanduse(int32);
            Classifiers estIpedClassifier = staticData.GetEstIPEDClassifier(str);
            if (!completeDictionary.ContainsKey(int32))
              completeDictionary[int32] = this.CreateSSDCDetailsDictionary();
            if (Convert.ToInt32(row[str]) != staticData.dctClassifierNoneValues[estIpedClassifier])
            {
              completeDictionary[int32][staticData.GetIPEDSignSymptomTypeLocationForField(str)][staticData.GetIPEDFieldName(str)][staticData.GetClassValueName(estIpedClassifier, Convert.ToInt16(row[str]))].PopEst += (double) row["EstimateValue"];
              completeDictionary[int32][staticData.GetIPEDSignSymptomTypeLocationForField(str)][staticData.GetIPEDFieldName(str)][staticData.GetClassValueName(estIpedClassifier, Convert.ToInt16(row[str]))].StdErr = (double) row["EstimateStandardError"];
              completeDictionary[int32][staticData.GetIPEDSignSymptomTypeLocationForField(str)][staticData.GetIPEDFieldName(str)][staticData.GetClassValueName(estIpedClassifier, Convert.ToInt16(row[str]))].Pct = (double) row["EstimateValue"] / estAllTreeCount;
              completeDictionary[int32][staticData.GetIPEDSignSymptomTypeLocationForField(str)][staticData.GetIPEDFieldName(str)][staticData.GetClassValueName(estIpedClassifier, Convert.ToInt16(row[str]))].PctAffected = (double) row["EstimateValue"] / estTotalAffected;
              completeDictionary[int32][staticData.GetIPEDSignSymptomTypeLocationForField(str)][staticData.GetIPEDFieldName(str)][staticData.GetClassValueName(estIpedClassifier, Convert.ToInt16(row[str]))].PctTree = (double) row["EstimateValue"] / estTotalLanduse;
            }
          }
        }
      }
      return completeDictionary;
    }
  }
}
