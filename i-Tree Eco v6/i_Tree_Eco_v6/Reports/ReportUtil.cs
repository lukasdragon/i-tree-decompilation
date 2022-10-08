// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ReportUtil
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using C1.Win.C1Chart;
using DaveyTree.Core;
using Eco.Domain.v6;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public static class ReportUtil
  {
    public static Dictionary<string, string> _PluralUnits = new Dictionary<string, string>()
    {
      {
        i_Tree_Eco_v6.Resources.Strings.DS_Millionth,
        i_Tree_Eco_v6.Resources.Strings.DS_Millionths
      },
      {
        i_Tree_Eco_v6.Resources.Strings.DS_Thousandth,
        i_Tree_Eco_v6.Resources.Strings.DS_Thousandths
      },
      {
        i_Tree_Eco_v6.Resources.Strings.DS_Thousand,
        i_Tree_Eco_v6.Resources.Strings.DS_Thousands
      },
      {
        i_Tree_Eco_v6.Resources.Strings.DS_Million,
        i_Tree_Eco_v6.Resources.Strings.DS_Millions
      },
      {
        i_Tree_Eco_v6.Resources.Strings.DS_Billion,
        i_Tree_Eco_v6.Resources.Strings.DS_Billions
      },
      {
        i_Tree_Eco_v6.Resources.Strings.DS_Trillion,
        i_Tree_Eco_v6.Resources.Strings.DS_Trillions
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitCentimeter,
        i_Tree_Eco_v6.Resources.Strings.UnitCentimeters
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitInch,
        i_Tree_Eco_v6.Resources.Strings.UnitInches
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitFoot,
        i_Tree_Eco_v6.Resources.Strings.UnitFeet
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitMeter,
        i_Tree_Eco_v6.Resources.Strings.UnitMeters
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitKilometer,
        i_Tree_Eco_v6.Resources.Strings.UnitKilometers
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitMile,
        i_Tree_Eco_v6.Resources.Strings.UnitMiles
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitSquareMeter,
        i_Tree_Eco_v6.Resources.Strings.UnitSquareMeters
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitSquareFoot,
        i_Tree_Eco_v6.Resources.Strings.UnitSquareFeet
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitSquareKilometer,
        i_Tree_Eco_v6.Resources.Strings.UnitSquareKilometers
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitSquareMile,
        i_Tree_Eco_v6.Resources.Strings.UnitSquareMiles
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitHectare,
        i_Tree_Eco_v6.Resources.Strings.UnitHectares
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitAcre,
        i_Tree_Eco_v6.Resources.Strings.UnitAcres
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitGram,
        i_Tree_Eco_v6.Resources.Strings.UnitGrams
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitOunce,
        i_Tree_Eco_v6.Resources.Strings.UnitOunces
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitKilogram,
        i_Tree_Eco_v6.Resources.Strings.UnitKilograms
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitPound,
        i_Tree_Eco_v6.Resources.Strings.UnitPounds
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitTonne,
        i_Tree_Eco_v6.Resources.Strings.UnitTonnes
      },
      {
        i_Tree_Eco_v6.Resources.Strings.UnitTon,
        i_Tree_Eco_v6.Resources.Strings.UnitTons
      }
    };

    public static string PluralizeLast(string s)
    {
      string str1 = s;
      string str2 = s.Substring(s.LastIndexOf(" ") + 1);
      string newValue;
      if (ReportUtil._PluralUnits.TryGetValue(str2, out newValue))
        str1 = s.Replace(str2, newValue);
      return str1;
    }

    public static void DisplayReportLocationMessage(C1PrintDocument c1Doc, string csvFileName)
    {
      RenderTable ro = new RenderTable();
      c1Doc.Body.Children.Add((RenderObject) ro);
      ro.Style.FontSize = 12f;
      ro.Width = (Unit) "100%";
      ro.Cols[0].Width = (Unit) "100%";
      ro.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Center;
      ro.Cells[0, 0].Text = "CSV file is generated: " + csvFileName;
    }

    public static LineDef GetTableLine() => new LineDef((Unit) "1pt", Color.Gray);

    public static string GetIsAreString(bool multi)
    {
      string str = multi ? i_Tree_Eco_v6.Resources.Strings.Are : i_Tree_Eco_v6.Resources.Strings.Is;
      return !(str != string.Empty) ? string.Empty : string.Format("{0} ", (object) str);
    }

    public static string FormatHeaderUnitsStr(string s) => string.Format(i_Tree_Eco_v6.Resources.Strings.FmtUnit, (object) s);

    public static string FormatInlineHeaderUnitsStr(string name, string unit) => string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) name, (object) unit);

    public static string FormatInlineCSVHeaderStr(string[] s) => string.Join(" ", s);

    public static string FormatInlineCSVHeaderWUnitsStr(string h, string u) => string.Format(i_Tree_Eco_v6.Resources.Strings.FmtCSVFieldUnit, (object) h, (object) u);

    public static string GetValuePerYrStr(string val) => ReportUtil.GetValuePerValueStr(val, i_Tree_Eco_v6.Resources.Strings.YearAbbr);

    public static string GetValuePerHrStr(string val) => ReportUtil.GetValuePerValueStr(val, i_Tree_Eco_v6.Resources.Strings.UnitHourAbbr);

    public static string GetValuePerValueStr(string val1, string val2) => string.Format("{0}/{1}", (object) val1, (object) val2);

    public static string GetValueByValue(string val1, string val2) => string.Format(i_Tree_Eco_v6.Resources.Strings.ValueByValue, (object) val1, (object) val2);

    public static string GetFormattedValuePerYrStr(string val) => ReportUtil.FormatHeaderUnitsStr(ReportUtil.GetValuePerYrStr(val));

    public static C1.C1Preview.Style SetDefaultReportFormatting(
      C1PrintDocument doc,
      RenderTable rTable,
      Year curYear,
      int headerRows)
    {
      doc.Style.Font = new Font("Calibri", 9f);
      doc.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Justify;
      rTable.CellStyle.Padding.Left = (Unit) "1mm";
      rTable.CellStyle.Padding.Right = (Unit) "1mm";
      rTable.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
      rTable.RowGroups[0, headerRows].Header = TableHeaderEnum.Page;
      TableVectorGroup pageHeader = rTable.RowGroups.GetPageHeader();
      if (pageHeader != null)
      {
        int count = pageHeader.Count;
        for (int index = 0; index < count; ++index)
          rTable.Rows[index].Style.FontBold = true;
        rTable.Rows[count - 1].Style.GridLines.Bottom = LineDef.Default;
        pageHeader.Style.TextAlignVert = C1.C1Preview.AlignVertEnum.Bottom;
      }
      return ReportUtil.AddAlternateStyle(rTable);
    }

    public static void SetPieChartStyle(C1.Win.C1Chart.C1Chart chart)
    {
      string familyName = "Calibri";
      chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Pie;
      foreach (ChartDataSeries series in (CollectionBase) chart.ChartGroups.Group0.ChartData.SeriesList)
        series.LineStyle.Color = ReportUtil.GetColor(chart.ChartGroups.Group0.ChartData.SeriesList.IndexOf(series));
      chart.Style.Font = new Font(familyName, chart.Style.Font.Size);
      chart.ChartArea.AxisX.Font = new Font(familyName, 11f);
      chart.Legend.Style.Font = new Font(familyName, 12f);
      chart.Footer.Style.Font = new Font(familyName, 11f, FontStyle.Bold);
      chart.Footer.Visible = true;
      chart.BackColor = Color.White;
      chart.UseAntiAliasedGraphics = true;
      chart.ChartArea.PlotArea.UseAntiAlias = true;
      chart.ChartGroups.Group0.Use3D = true;
      chart.ChartArea.PlotArea.View3D.Depth = 3;
      chart.ChartArea.PlotArea.View3D.Elevation = 70;
      chart.ChartLabels.AutoArrangement.Method = AutoLabelArrangementMethodEnum.Decimation;
      chart.ChartLabels.AutoArrangement.Options = AutoLabelArrangementOptions.Default;
      chart.ChartLabels.AutoArrangement.MaxIterations = 100;
    }

    public static void SetBarChartStyle(C1.Win.C1Chart.C1Chart chart)
    {
      string familyName = "Calibri";
      chart.Footer.Visible = true;
      chart.ChartArea.AxisX.Font = new Font(familyName, 11f);
      chart.Font = new Font(familyName, 11f);
      chart.Footer.Style.Font = new Font(familyName, 11f, FontStyle.Bold);
      chart.BackColor = Color.White;
      chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      chart.ChartArea.AxisY.NoAnnoOverlap = true;
      chart.ChartArea.AxisY.Rotation = RotationEnum.Rotate270;
      chart.ChartArea.AxisX.AnnoMethod = AnnotationMethodEnum.ValueLabels;
      chart.ChartArea.AxisX.AnnotationRotation = -45;
      chart.ChartArea.AxisX.TickMinor = TickMarksEnum.None;
      chart.ChartArea.AxisY.TickMinor = TickMarksEnum.None;
    }

    public static void SetXYPlotChartStyle(C1.Win.C1Chart.C1Chart chart)
    {
      string familyName = "Calibri";
      chart.Footer.Visible = true;
      chart.ChartArea.AxisX.Font = new Font(familyName, 11f);
      chart.Font = new Font(familyName, 11f);
      chart.Footer.Style.Font = new Font(familyName, 11f, FontStyle.Bold);
      chart.BackColor = Color.White;
      chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.XYPlot;
      chart.ChartGroups.Group1.ChartType = Chart2DTypeEnum.Bar;
      chart.ChartArea.AxisY.Rotation = RotationEnum.Rotate270;
      chart.ChartArea.AxisY2.Rotation = RotationEnum.Rotate270;
      chart.ChartArea.AxisY2.Visible = true;
      chart.ChartArea.AxisY.NoAnnoOverlap = true;
      chart.ChartArea.AxisX.AnnoMethod = AnnotationMethodEnum.ValueLabels;
      chart.ChartArea.AxisX.TickMinor = TickMarksEnum.None;
      chart.ChartArea.AxisY.TickMinor = TickMarksEnum.None;
      chart.ChartArea.AxisY2.TickMinor = TickMarksEnum.None;
      chart.ChartArea.AxisX.AnnotationRotation = -45;
    }

    public static void AddChartTriangleMarker(ChartDataSeries cdsAmount)
    {
      cdsAmount.LineStyle.Pattern = LinePatternEnum.None;
      cdsAmount.SymbolStyle.Shape = SymbolShapeEnum.Tri;
      cdsAmount.SymbolStyle.Size = 10;
      cdsAmount.SymbolStyle.Color = Color.Black;
    }

    public static void AddWrittenReportTableHeaderFormat(RenderTable rt)
    {
      TableVectorGroup pageHeader = rt.RowGroups.GetPageHeader();
      if (pageHeader == null)
        return;
      pageHeader.Style.BackColor = Color.FromArgb(120, 114, 144, 201);
      pageHeader.Style.FontItalic = true;
      pageHeader.Style.TextAlignVert = C1.C1Preview.AlignVertEnum.Bottom;
      pageHeader.Style.GridLines.Bottom = LineDef.Default;
    }

    public static double RoundToSignificantFigures(double num, int n)
    {
      if (num == 0.0)
        return 0.0;
      double num1 = Math.Ceiling(Math.Log10(num < 0.0 ? -num : num));
      double num2 = Math.Pow(10.0, (double) (n - (int) num1));
      return (double) Convert.ToInt64(Math.Round(num * num2, 0)) / num2;
    }

    public static C1.C1Preview.Style AddAlternateStyle(RenderTable rt)
    {
      C1.C1Preview.Style style = rt.Style.Children.Add();
      style.BackColor = Color.FromArgb(217, 217, 217);
      return style;
    }

    public static void FormatRenderTable(RenderTable rt)
    {
      rt.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
      int num = ReportUtil.FormatRenderTableHeader(rt);
      C1.C1Preview.Style style = ReportUtil.AddAlternateStyle(rt);
      for (int index = num; index < rt.Rows.Count; ++index)
      {
        if ((index - num) % 2 == 0)
          rt.Rows[index].Style.Parent = style;
      }
    }

    public static int FormatRenderTableHeader(RenderTable rt)
    {
      TableVectorGroup pageHeader = rt.RowGroups.GetPageHeader();
      int num = 1;
      if (pageHeader != null)
      {
        num = pageHeader.Count;
        pageHeader.Style.TextAlignVert = C1.C1Preview.AlignVertEnum.Bottom;
        pageHeader.Style.FontBold = true;
        pageHeader.Style.GridLines.Bottom = LineDef.Default;
      }
      rt.CellStyle.Padding.Left = (Unit) "1mm";
      rt.CellStyle.Padding.Right = (Unit) "1mm";
      return num;
    }

    public static int AddTableNote(RenderTable rTable, int row, float fontSize)
    {
      ++row;
      rTable.Cells[row, 0].SpanCols = rTable.Cols.Count;
      rTable.Rows[row].Style.FontSize = fontSize;
      rTable.Cells[row, 0].Style.BackColor = Color.White;
      rTable.Cells[row, 0].Text = i_Tree_Eco_v6.Resources.Strings.NoteSusceptibilityOfTreesToPestsByStratum;
      return row;
    }

    public static int AddEmptyRow(RenderTable rTable, int row)
    {
      ++row;
      rTable.Cells[row, 0].Text = " ";
      return row;
    }

    public static void FormatRenderTableWrittenReport(RenderTable rt)
    {
      rt.Rows[0].Style.GridLines.Top = LineDef.Default;
      rt.Cols[0].Style.GridLines.Left = LineDef.Default;
      rt.Cols[rt.Cols.Count - 1].Style.GridLines.Right = LineDef.Default;
      rt.Rows[rt.Rows.Count - 1].Style.GridLines.Bottom = LineDef.Default;
      rt.CellStyle.Padding.Left = (Unit) "1mm";
      rt.CellStyle.Padding.Right = (Unit) "1mm";
      rt.Rows[0].Style.TextAlignVert = C1.C1Preview.AlignVertEnum.Bottom;
    }

    public static RenderImage CreateChartRenderObject(
      C1.Win.C1Chart.C1Chart ChartIn,
      Graphics FormGraphics,
      C1PrintDocument C1PrintDocumentIn,
      double FractionOfPageWidth,
      double FractionOfPageHeight)
    {
      if (FractionOfPageWidth == 0.0 || FractionOfPageHeight == 0.0 || ChartIn == null)
        return new RenderImage();
      C1.Win.C1Chart.C1Chart c1Chart = new C1.Win.C1Chart.C1Chart();
      c1Chart.LoadChartAndImagesFromString(ChartIn.SaveChartAndImagesToString(false));
      Unit unit1 = new Unit(C1PrintDocumentIn.PageLayout.PageSettings.Width.ConvertUnit(UnitTypeEnum.Inch) - (C1PrintDocumentIn.PageLayout.PageSettings.LeftMargin.ConvertUnit(UnitTypeEnum.Inch) + C1PrintDocumentIn.PageLayout.PageSettings.RightMargin.ConvertUnit(UnitTypeEnum.Inch)), UnitTypeEnum.Inch);
      Unit unit2 = new Unit(C1PrintDocumentIn.PageLayout.PageSettings.Height.ConvertUnit(UnitTypeEnum.Inch) - (C1PrintDocumentIn.PageLayout.PageSettings.TopMargin.ConvertUnit(UnitTypeEnum.Inch) + C1PrintDocumentIn.PageLayout.PageSettings.BottomMargin.ConvertUnit(UnitTypeEnum.Inch)), UnitTypeEnum.Inch);
      double num1 = unit1.ConvertUnit(C1PrintDocumentIn.CreationDpi, UnitTypeEnum.Pixel, FormGraphics.DpiX);
      double num2 = unit2.ConvertUnit(C1PrintDocumentIn.CreationDpi, UnitTypeEnum.Pixel, FormGraphics.DpiY);
      c1Chart.Height = (int) (num2 * FractionOfPageHeight);
      c1Chart.Width = (int) (num1 * FractionOfPageWidth);
      RenderImage chartRenderObject = new RenderImage();
      chartRenderObject.Image = c1Chart.GetImage(ImageFormat.Tiff);
      chartRenderObject.Width = Unit.Auto;
      return chartRenderObject;
    }

    public static void SetChartOptions(C1.Win.C1Chart.C1Chart chart, bool UseXAxisValueLabels)
    {
      chart.BackColor = Color.White;
      chart.Style.Font = new Font("Calibri", 14f);
      if (UseXAxisValueLabels)
      {
        chart.ChartArea.AxisX.AnnoMethod = AnnotationMethodEnum.ValueLabels;
        chart.ChartArea.AxisX.AnnotationRotation = -45;
        chart.ChartArea.AxisX.TickMinor = TickMarksEnum.None;
      }
      else
      {
        chart.ChartArea.AxisX.AnnoFormat = FormatEnum.NumericManual;
        chart.ChartArea.AxisX.AnnoFormatString = "#,#.###";
      }
      chart.ChartArea.AxisY.AnnoFormat = FormatEnum.NumericManual;
      chart.ChartArea.AxisY.AnnoFormatString = "#,#.###";
      chart.ChartArea.AxisY2.AnnoFormat = FormatEnum.NumericManual;
      chart.ChartArea.AxisY2.AnnoFormatString = "#,#.###";
      chart.ChartArea.Margins.Top = chart.ChartArea.Margins.Right = chart.ChartArea.Margins.Left = chart.ChartArea.Margins.Bottom = 0;
    }

    private static Color LightenColor(double alpha, int r, int g, int b)
    {
      r = Convert.ToInt32((1.0 - alpha) * (double) r + alpha * (double) byte.MaxValue);
      g = Convert.ToInt32((1.0 - alpha) * (double) g + alpha * (double) byte.MaxValue);
      b = Convert.ToInt32((1.0 - alpha) * (double) b + alpha * (double) byte.MaxValue);
      return Color.FromArgb(r, g, b);
    }

    public static Color GetColor(int index)
    {
      int num1 = 3;
      int num2 = 11;
      double alpha = (double) (index / num2) / (double) num1;
      switch (index % num2)
      {
        case 0:
          return ReportUtil.LightenColor(alpha, 0, 93, 55);
        case 1:
          return ReportUtil.LightenColor(alpha, 158, 206, 108);
        case 2:
          return ReportUtil.LightenColor(alpha, 130, 82, 39);
        case 3:
          return ReportUtil.LightenColor(alpha, 114, 144, 201);
        case 4:
          return ReportUtil.LightenColor(alpha, 133, 135, 137);
        case 5:
          return ReportUtil.LightenColor(alpha, 43, 150, 119);
        case 6:
          return ReportUtil.LightenColor(alpha, 50, 105, 117);
        case 7:
          return ReportUtil.LightenColor(alpha, 50, 71, 89);
        case 8:
          return ReportUtil.LightenColor(alpha, 115, 125, 47);
        case 9:
          return ReportUtil.LightenColor(alpha, 206, 102, 58);
        case 10:
          return ReportUtil.LightenColor(alpha, 226, 65, 65);
        default:
          return Color.Black;
      }
    }

    public static string UpperCaseFirstLetterEachWord(string input)
    {
      bool flag = input.Contains<char>('/');
      string[] strArray = input.Split(new char[2]
      {
        ' ',
        '/'
      }, StringSplitOptions.RemoveEmptyEntries);
      for (int index = 0; index <= strArray.GetUpperBound(0); ++index)
      {
        string str1 = strArray[index].Substring(0, 1);
        string str2 = string.Empty;
        if (strArray[index].Length > 1)
          str2 = strArray[index].Substring(1).ToLower();
        strArray[index] = str1.ToUpperInvariant() + str2;
      }
      return flag ? string.Join("/", strArray) : string.Join(" ", strArray);
    }

    public static string UpperCaseFirstLetter(string input)
    {
      switch (input)
      {
        case "":
        case null:
          return input;
        default:
          return input.Length == 1 ? input.ToUpper() : input.Substring(0, 1).ToUpper() + input.Substring(1);
      }
    }

    public static T ConvertFromDBVal<T>(object obj) => obj == DBNull.Value ? default (T) : TypeHelper.Convert<T>(obj);

    public static string GetNumericWordWithMagnitude(double d, int sigDigits)
    {
      string empty = string.Empty;
      if (d / 1000000000000.0 > 1.0)
        return string.Format(i_Tree_Eco_v6.Resources.Strings.UnitRoundTrillion, (object) ReportUtil.RoundToSignificantFigures(d / 1000000000000.0, sigDigits));
      if (d / 1000000000.0 > 1.0)
        return string.Format(i_Tree_Eco_v6.Resources.Strings.UnitRoundBillion, (object) ReportUtil.RoundToSignificantFigures(d / 1000000000.0, sigDigits));
      if (d / 1000000.0 > 1.0)
        return string.Format(i_Tree_Eco_v6.Resources.Strings.UnitRoundMillion, (object) ReportUtil.RoundToSignificantFigures(d / 1000000.0, sigDigits));
      return d / 1000.0 > 1.0 ? string.Format(i_Tree_Eco_v6.Resources.Strings.UnitRoundThousand, (object) ReportUtil.RoundToSignificantFigures(d / 1000.0, sigDigits)) : ReportUtil.RoundToSignificantFigures(d, sigDigits).ToString();
    }

    public static string AddSpace(string s) => string.Format("{0} ", (object) s);

    public static string NumberToWords(int number)
    {
      if (number == 0)
        return i_Tree_Eco_v6.Resources.Strings.Zero;
      if (number < 0)
        return string.Format(i_Tree_Eco_v6.Resources.Strings.MinusValue, (object) ReportUtil.NumberToWords(Math.Abs(number)));
      string words = "";
      if (number / 1000000 > 0)
      {
        words += ReportUtil.AddSpace(string.Format(i_Tree_Eco_v6.Resources.Strings.NumberToWordsMillion, (object) ReportUtil.NumberToWords(number / 1000000)));
        number %= 1000000;
      }
      if (number / 1000 > 0)
      {
        words += ReportUtil.AddSpace(string.Format(i_Tree_Eco_v6.Resources.Strings.NumberToWordsThousand, (object) ReportUtil.NumberToWords(number / 1000)));
        number %= 1000;
      }
      if (number / 100 > 0)
      {
        words += ReportUtil.AddSpace(string.Format(i_Tree_Eco_v6.Resources.Strings.NumberToWordsHundred, (object) ReportUtil.NumberToWords(number / 100)));
        number %= 100;
      }
      if (number > 0)
      {
        if (words != string.Empty)
          words += ReportUtil.AddSpace(i_Tree_Eco_v6.Resources.Strings.And);
        string[] strArray1 = new string[20]
        {
          i_Tree_Eco_v6.Resources.Strings.Zero,
          i_Tree_Eco_v6.Resources.Strings.One,
          i_Tree_Eco_v6.Resources.Strings.Two,
          i_Tree_Eco_v6.Resources.Strings.Three,
          i_Tree_Eco_v6.Resources.Strings.Four,
          i_Tree_Eco_v6.Resources.Strings.Five,
          i_Tree_Eco_v6.Resources.Strings.Six,
          i_Tree_Eco_v6.Resources.Strings.Seven,
          i_Tree_Eco_v6.Resources.Strings.Eight,
          i_Tree_Eco_v6.Resources.Strings.Nine,
          i_Tree_Eco_v6.Resources.Strings.Ten,
          i_Tree_Eco_v6.Resources.Strings.Eleven,
          i_Tree_Eco_v6.Resources.Strings.Twelve,
          i_Tree_Eco_v6.Resources.Strings.Thirteen,
          i_Tree_Eco_v6.Resources.Strings.Fourteen,
          i_Tree_Eco_v6.Resources.Strings.Fifteen,
          i_Tree_Eco_v6.Resources.Strings.Sixteen,
          i_Tree_Eco_v6.Resources.Strings.Seventeen,
          i_Tree_Eco_v6.Resources.Strings.Eighteen,
          i_Tree_Eco_v6.Resources.Strings.Nineteen
        };
        string[] strArray2 = new string[10]
        {
          i_Tree_Eco_v6.Resources.Strings.Zero,
          i_Tree_Eco_v6.Resources.Strings.Ten,
          i_Tree_Eco_v6.Resources.Strings.Twenty,
          i_Tree_Eco_v6.Resources.Strings.Thirty,
          i_Tree_Eco_v6.Resources.Strings.Forty,
          i_Tree_Eco_v6.Resources.Strings.Fifty,
          i_Tree_Eco_v6.Resources.Strings.Sixty,
          i_Tree_Eco_v6.Resources.Strings.Seventy,
          i_Tree_Eco_v6.Resources.Strings.Eighty,
          i_Tree_Eco_v6.Resources.Strings.Ninety
        };
        if (number < 20)
        {
          words += strArray1[number];
        }
        else
        {
          words += strArray2[number / 10];
          if (number % 10 > 0)
            words += string.Format("- {0}", (object) strArray1[number % 10]);
        }
      }
      return words;
    }
  }
}
