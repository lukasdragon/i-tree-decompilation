// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PestSignSymptomDetailsBySpeciesBindings
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Util;
using EcoIpedReportGenerator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  internal class PestSignSymptomDetailsBySpeciesBindings
  {
    public List<DataBinding> Bindings()
    {
      List<DataBinding> dataBindingList = new List<DataBinding>();
      if (!staticData.IsSample)
      {
        DataTable dataTable = staticData.DsData.Tables["SpeciesPresent"].AsEnumerable().OrderBy<DataRow, string>((Func<DataRow, string>) (r => !staticData.UseScientificName ? r.Field<string>("ClassValueName") : r.Field<string>("ClassValueName1"))).CopyToDataTable<DataRow>();
        DataRow row = dataTable.NewRow();
        row["SpCode"] = (object) string.Empty;
        row["ClassValueName"] = (object) i_Tree_Eco_v6.Resources.Strings.AllSpecies;
        row["ClassValueName1"] = (object) i_Tree_Eco_v6.Resources.Strings.AllSpecies;
        dataTable.Rows.InsertAt(row, 0);
        DataBinding dataBinding = new DataBinding(0)
        {
          Description = i_Tree_Eco_v6.Resources.Strings.Species,
          DisplayMember = staticData.UseScientificName ? "ClassValueName1" : "ClassValueName",
          ValueMember = "SpCode",
          DataSource = (object) dataTable
        };
        dataBindingList.Add(dataBinding);
      }
      else
      {
        List<short> treePests = Enumerable.OfType<DataRow>(staticData.DsData.Tables["Species_PestPest"].Rows).Select<DataRow, short>((Func<DataRow, short>) (r => r.Field<short>("Species"))).ToList<short>();
        IOrderedEnumerable<Tuple<short, string, string>> orderedEnumerable = staticData.estUtil.ClassValues[Classifiers.Species].Where<KeyValuePair<short, Tuple<string, string>>>((Func<KeyValuePair<short, Tuple<string, string>>, bool>) (r => treePests.Contains(r.Key))).Select<KeyValuePair<short, Tuple<string, string>>, Tuple<short, string, string>>((Func<KeyValuePair<short, Tuple<string, string>>, Tuple<short, string, string>>) (r => new Tuple<short, string, string>(r.Key, r.Value.Item1, r.Value.Item2))).OrderBy<Tuple<short, string, string>, string>((Func<Tuple<short, string, string>, string>) (r => !staticData.UseScientificName ? r.Item2 : r.Item3));
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("ClassValueOrder");
        dataTable.Columns.Add("ClassValueName");
        dataTable.Columns.Add("ClassValueName1");
        DataRow row1 = dataTable.NewRow();
        row1["ClassValueOrder"] = (object) -1;
        row1["ClassValueName"] = (object) i_Tree_Eco_v6.Resources.Strings.AllSpecies;
        row1["ClassValueName1"] = (object) i_Tree_Eco_v6.Resources.Strings.AllSpecies;
        dataTable.Rows.Add(row1);
        foreach (Tuple<short, string, string> tuple in (IEnumerable<Tuple<short, string, string>>) orderedEnumerable)
        {
          short num1;
          string str1;
          string str2;
          tuple.Deconstruct<short, string, string>(out num1, out str1, out str2);
          short num2 = num1;
          string str3 = str1;
          string str4 = str2;
          DataRow row2 = dataTable.NewRow();
          row2["ClassValueOrder"] = (object) num2;
          row2["ClassValueName"] = (object) str3;
          row2["ClassValueName1"] = (object) str4;
          dataTable.Rows.Add(row2);
        }
        DataBinding dataBinding = new DataBinding(0)
        {
          Description = i_Tree_Eco_v6.Resources.Strings.Species,
          DisplayMember = staticData.UseScientificName ? "ClassValueName1" : "ClassValueName",
          ValueMember = "ClassValueOrder",
          DataSource = (object) dataTable
        };
        dataBindingList.Add(dataBinding);
      }
      return dataBindingList;
    }
  }
}
