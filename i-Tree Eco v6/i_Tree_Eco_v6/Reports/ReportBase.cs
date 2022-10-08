// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ReportBase
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using CsvHelper;
using DaveyTree.Core;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Views;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Forms;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace i_Tree_Eco_v6.Reports
{
  public abstract class ReportBase
  {
    public const string roundDouble0 = "N0";
    public const string roundDouble1 = "N1";
    public const string roundDouble2 = "N2";
    public const string roundDouble3 = "N3";
    public const string roundDouble5 = "N5";
    public const string roundDoubleWZero0 = "{0:N0}";
    public const string roundDoubleWZero1 = "{0:N1}";
    public const string roundDoubleWZero2 = "{0:N2}";
    public const string roundDoubleWZero3 = "{0:N3}";
    public const string valueDecimals2 = "N2";
    public const string totalsDecimals0 = "N0";
    protected static bool csvExport;
    protected static bool plotInventory;
    public static bool energyEffectsAvailable;
    protected static ProgramSession m_ps = ProgramSession.GetInstance();

    public string HelpTopic { get; protected set; }

    public bool hasConvertableUnits { get; protected set; }

    public bool hasSpecies { get; protected set; }

    public bool hasCoordinates { get; protected set; }

    public bool hasComments { get; protected set; }

    public bool hasUID { get; protected set; }

    public bool hasZeros { get; protected set; }

    public bool exportReady { get; protected set; }

    public static bool EnglishUnits => ReportBase.m_ps.UseEnglishUnits;

    public static bool ScientificName => ReportBase.m_ps.SpeciesDisplayName == SpeciesDisplayEnum.ScientificName;

    public ReportBase() => this.ReportTitle = string.Empty;

    protected string SquareMeterUnits()
    {
      if (ReportBase.csvExport)
        return string.Format("{0}^2", (object) ReportBase.MUnits());
      return !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitSquareMetersAbbr : i_Tree_Eco_v6.Resources.Strings.UnitSquareFeetAbbr;
    }

    protected static string CubicMeterUnits()
    {
      if (ReportBase.csvExport)
        return string.Format("{0}^3", (object) ReportBase.MUnits());
      return !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitCubicMetersAbbr : i_Tree_Eco_v6.Resources.Strings.UnitCubicFeetAbbr;
    }

    protected static string CubicMeterUnitsV() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitCubicMeters : i_Tree_Eco_v6.Resources.Strings.UnitCubicFeet;

    protected static string HaUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitHectaresAbbr : i_Tree_Eco_v6.Resources.Strings.UnitAcresAbbr;

    protected static string HectarUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitHectare : i_Tree_Eco_v6.Resources.Strings.UnitAcre;

    protected static string HectaresUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitHectares : i_Tree_Eco_v6.Resources.Strings.UnitAcres;

    protected static string HectareUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitHectare : i_Tree_Eco_v6.Resources.Strings.UnitAcre;

    protected static string CmUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitCentimetersAbbr : i_Tree_Eco_v6.Resources.Strings.UnitInchesAbbr;

    protected static string CentimeterUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitCentimeters : i_Tree_Eco_v6.Resources.Strings.UnitInches;

    protected static string MUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitMetersAbbr : i_Tree_Eco_v6.Resources.Strings.UnitFeetAbbr;

    protected static string KMUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitKilometersAbbr : i_Tree_Eco_v6.Resources.Strings.UnitMilesAbbr;

    protected static string KgUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitKilogramsAbbr : i_Tree_Eco_v6.Resources.Strings.UnitPoundsAbbr;

    protected static string KilogramUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitKilogram : i_Tree_Eco_v6.Resources.Strings.UnitPound;

    protected static string KilogramsUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitKilograms : i_Tree_Eco_v6.Resources.Strings.UnitPounds;

    protected static string TonneUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitTonne : i_Tree_Eco_v6.Resources.Strings.UnitTon;

    protected static string TonnesUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitTonnes : i_Tree_Eco_v6.Resources.Strings.UnitTons;

    protected static string GUnits() => !ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.UnitGramsAbbr : i_Tree_Eco_v6.Resources.Strings.UnitOuncesAbbr;

    public string ReportTitle { get; set; }

    public abstract void InitDocument(C1PrintDocument C1doc);

    public abstract void RenderHeader(C1PrintDocument C1doc);

    public abstract void RenderBody(C1PrintDocument C1doc, Graphics g);

    protected virtual void WriteCSV(CsvWriter csv)
    {
    }

    protected virtual void GetDBData()
    {
    }

    protected virtual void getOrdinals()
    {
    }

    protected virtual void Note(C1PrintDocument C1doc)
    {
    }

    protected virtual void DisplayStandardMessage(C1PrintDocument C1doc, string message)
    {
    }

    public virtual void ExportCSVandRenderBody(
      C1PrintDocument C1doc,
      Graphics g,
      string csvFileName)
    {
      ReportBase.csvExport = true;
      bool dataReady = this.ExportCSV(csvFileName);
      ReportBase.csvExport = false;
      this.WriteDataToFile(C1doc, csvFileName, dataReady);
    }

    public virtual void ExportKMLandRenderBody(
      C1PrintDocument C1doc,
      Graphics g,
      string csvFileName,
      ExportFormat format = ExportFormat.CSV)
    {
      ReportBase.csvExport = true;
      bool dataReady = this.ExportKML(csvFileName);
      ReportBase.csvExport = false;
      this.WriteDataToFile(C1doc, csvFileName, dataReady);
    }

    private void WriteDataToFile(C1PrintDocument C1doc, string csvFileName, bool dataReady)
    {
      if (dataReady)
      {
        Process.Start(csvFileName);
        ReportUtil.DisplayReportLocationMessage(C1doc, csvFileName);
        this.Note(C1doc);
      }
      else
        this.DisplayStandardMessage(C1doc, i_Tree_Eco_v6.Resources.Strings.MsgNoData);
    }

    public abstract void RenderFooter(C1PrintDocument C1doc);

    public virtual object GetData() => (object) null;

    public virtual string CustomMesssage() => string.Empty;

    public virtual List<ColumnFormat> ColumnsFormat(DataTable data) => (List<ColumnFormat>) null;

    public virtual OrderedDictionary ColumnsFormat() => (OrderedDictionary) null;

    public virtual bool ExportCSV(string csvFileName)
    {
      object data = this.GetData();
      if (data == null)
        return false;
      if (data is DataTable)
      {
        List<ColumnFormat> columnsFormat = this.ColumnsFormat((DataTable) data);
        this.ExportToCSV(csvFileName, columnsFormat, (DataTable) data);
      }
      return true;
    }

    public virtual bool ExportKML(string fileName)
    {
      if (!(this.GetData() is DataTable data))
        return false;
      List<ColumnFormat> columnsFormat = this.ColumnsFormat(data);
      Kml root = new Kml();
      Document document = new Document();
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        object x = row["xCoordinate"];
        object y = row["yCoordinate"];
        if (ValidationHelper.ValidateCoordinates(x, y))
        {
          Placemark placemark = new Placemark();
          placemark.Geometry = (Geometry) new SharpKml.Dom.Point()
          {
            Coordinate = new Vector(Convert.ToDouble(y), Convert.ToDouble(x), 0.0)
          };
          placemark.ExtendedData = new ExtendedData();
          Placemark placeMark = placemark;
          this.AddDataToKMLPlacemarkExtendedData(placeMark, columnsFormat, row);
          document.AddFeature((Feature) placeMark);
        }
      }
      root.Feature = (Feature) document;
      KmlFile.Create((Element) root, true).Save((Stream) new FileStream(fileName, FileMode.Create));
      return true;
    }

    public void AddDataToKMLPlacemarkExtendedData(
      Placemark placeMark,
      List<ColumnFormat> columnsFormat,
      DataRow row)
    {
      foreach (ColumnFormat columnFormat in columnsFormat)
        placeMark.ExtendedData.AddData(new SharpKml.Dom.Data()
        {
          Name = columnFormat.HeaderText,
          Value = columnFormat.Format(row[columnFormat.ColName]).ToString()
        });
    }

    public static bool IsNull(object val) => val == null || Convert.IsDBNull(val);

    public static string FormatInt(object val) => ReportBase.IsNull(val) ? string.Empty : val.ToString();

    public static string FormatDoubleSquaremeter(object val) => ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish((double) val, Units.Squaremeter, ReportBase.EnglishUnits).ToString("N1");

    public static string FormatDoubleSquaremeter3(object val) => ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish((double) val, Units.Squaremeter, ReportBase.EnglishUnits).ToString("N3");

    public static string FormatDoubleCentimeters(object val) => ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish((double) val, Units.Centimeters, ReportBase.EnglishUnits).ToString("N1");

    public static string FormatDoubleMeters(object val) => ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish((double) val, Units.Meters, ReportBase.EnglishUnits).ToString("N1");

    public static string FormatDoubleKiloMeters(object val) => ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish((double) val, Units.Kilometer, ReportBase.EnglishUnits).ToString("N1");

    public static string FormatDoubleValue0(object val) => ReportBase.IsNull(val) ? string.Empty : ((double) val).ToString("N0");

    public static string FormatDoubleValue1(object val) => ReportBase.IsNull(val) ? string.Empty : ((double) val).ToString("N1");

    public static string FormatDoubleValue2(object val) => ReportBase.IsNull(val) ? string.Empty : ((double) val).ToString("N2");

    public static string FormatDoubleValueZero2(object val) => ReportBase.IsNull(val) ? string.Empty : string.Format("{0:N2}", (object) (double) val);

    public static string FormatDoubleValue3(object val) => ReportBase.IsNull(val) ? string.Empty : ((double) val).ToString("N3");

    public static string FormatEnergyEffectsSingle(object val)
    {
      if (!ReportBase.energyEffectsAvailable)
        return i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
      return ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish((double) val, Units.Kilograms, ReportBase.EnglishUnits).ToString("N1");
    }

    public static string FormatEnergyEffectsValue(object val)
    {
      if (!ReportBase.energyEffectsAvailable)
        return i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
      return ReportBase.IsNull(val) ? string.Empty : ((double) val).ToString("N2");
    }

    public static double FormatDoubleVolumeCubicMeter(object val) => EstimateUtil.ConvertToEnglish((double) val, Units.CubicMeter, ReportBase.EnglishUnits);

    public static string FormatDoubleVolumeCubicMeter1(object val) => ReportBase.IsNull(val) ? string.Empty : ReportBase.FormatDoubleVolumeCubicMeter(val).ToString("N1");

    public static string FormatDoubleVolumeCubicMeterZero2(object val) => ReportBase.IsNull(val) ? string.Empty : string.Format("{0:N2}", (object) ReportBase.FormatDoubleVolumeCubicMeter(val));

    public static string FormatTonne1(object val) => ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish((double) val, Units.MetricTons, ReportBase.EnglishUnits).ToString("N1");

    public static string FormatTonneSE1(object val) => ReportBase.IsNull(val) ? string.Empty : string.Format("±{0}", (object) EstimateUtil.ConvertToEnglish((double) val, Units.MetricTons, ReportBase.EnglishUnits).ToString("N1"));

    public static string FormatDoubleWeight(object val) => ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish((double) val, Units.Kilograms, ReportBase.EnglishUnits).ToString("N1");

    public static string FormatDoubleWeight0(object val) => ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish((double) val, Units.Kilograms, ReportBase.EnglishUnits).ToString("N0");

    public static string FormatDoubleEnglishWeight0(object val) => ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish(EstimateUtil.ConvertToMetric((double) val, Units.Pounds, true), Units.Kilograms, ReportBase.EnglishUnits).ToString("N0");

    public static string FormatDoubleWeight3(object val) => ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish((double) val, Units.Kilograms, ReportBase.EnglishUnits).ToString("N3");

    public static string FormatDoubleEnglishWeight3(object val) => ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish(EstimateUtil.ConvertToMetric((double) val, Units.Pounds, true), Units.Kilograms, ReportBase.EnglishUnits).ToString("N3");

    public static string FormatDoubleWeightGrams(object val) => ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish((double) val, Units.Grams, ReportBase.EnglishUnits).ToString("N1");

    public static string FormatDoubleEnglishWeightScaleDownToGrams(object val) => ReportBase.IsNull(val) ? string.Empty : EstimateUtil.ConvertToEnglish(EstimateUtil.ConvertToMetric((double) val, Units.Pounds, true) * 1000.0, Units.Grams, ReportBase.EnglishUnits).ToString("N1");

    public static string FormatCount(object val) => ReportBase.IsNull(val) ? string.Empty : string.Format("{0:N0}", val);

    public static string FormatSE(object val) => ReportBase.IsNull(val) ? string.Empty : string.Format("(±{0})", (object) string.Format("{0:N0}", val));

    public static string FormatStr(object val) => ReportBase.IsNull(val) ? string.Empty : val.ToString();

    public static string FormatBool(object val)
    {
      if (ReportBase.IsNull(val))
        return string.Empty;
      return (bool) val ? i_Tree_Eco_v6.Resources.Strings.Yes : i_Tree_Eco_v6.Resources.Strings.No;
    }

    public static string FormatDBBool(object val)
    {
      if (ReportBase.IsNull(val))
        return string.Empty;
      switch (Convert.ToInt16(val, (IFormatProvider) CultureInfo.InvariantCulture))
      {
        case 0:
          return i_Tree_Eco_v6.Resources.Strings.No;
        case 1:
          return i_Tree_Eco_v6.Resources.Strings.Yes;
        default:
          return string.Empty;
      }
    }

    public static string FormatTreeStatus(object val) => ReportBase.IsNull(val) ? string.Empty : EnumHelper.GetDescription<TreeStatus>((TreeStatus) Convert.ToChar(val));

    public static string FormatDate(object val) => ReportBase.IsNull(val) ? string.Empty : ((DateTime) val).ToString("d");

    public static string FormatSpecies(object val)
    {
      try
      {
        SpeciesView specy = ProgramSession.GetInstance().Species[val.ToString()];
        return ReportBase.ScientificName ? specy.ScientificName : specy.CommonName;
      }
      catch (Exception ex)
      {
        return val.ToString();
      }
    }

    public void ExportToCSV(string csvFileName, List<ColumnFormat> columnsFormat, DataTable data)
    {
      using (TextWriter writer = (TextWriter) new StreamWriter(csvFileName))
      {
        CsvWriter csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
        foreach (ColumnFormat columnFormat in columnsFormat)
          csvWriter.WriteField<string>(columnFormat.HeaderText);
        csvWriter.NextRecord();
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
        {
          foreach (ColumnFormat columnFormat in columnsFormat)
            csvWriter.WriteField(columnFormat.Format(row[columnFormat.ColName]));
          csvWriter.NextRecord();
        }
        csvWriter.NextRecord();
      }
    }
  }
}
