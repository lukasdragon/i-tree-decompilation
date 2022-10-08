// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ProjectMetadata
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Forms;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UFORE.Deposition;

namespace i_Tree_Eco_v6.Reports
{
  internal class ProjectMetadata : DatabaseReport
  {
    private ProgramSession m_ProgramSession = ProgramSession.GetInstance();

    public ProjectMetadata()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleMetadataReportFor;
      this.hasCoordinates = true;
    }

    private string getBenefitsStr(double price, double pricePer, int year, string round = "N2") => string.Format(i_Tree_Eco_v6.Resources.Strings.FmtEcoDefaultValue, (object) price.ToString(round), (object) pricePer.ToString(round), (object) year);

    private string getPollutantValueStr(double pollutant, string round = "N2") => (ReportBase.EnglishUnits ? pollutant * 0.907185 : pollutant).ToString(round);

    private string getPollutantStr(string pollutant, string currencyString) => string.Format("{0} {1}/{2}", (object) pollutant, (object) currencyString, (object) ReportBase.TonneUnits());

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      C1doc.Stacking = StackingRulesEnum.BlockTopToBottom;
      Style styleList1 = C1doc.Style.Children.Add();
      styleList1.Spacing.Left = (Unit) "1.2cm";
      styleList1.Spacing.Bottom = (Unit) "0ls";
      Style styleBullet = C1doc.Style.Children.Add();
      styleBullet.Spacing.Left = (Unit) ".6cm";
      styleBullet.Spacing.Bottom = (Unit) "0ls";
      Style styleList2 = C1doc.Style.Children.Add();
      styleList2.Spacing.Left = (Unit) "1.8cm";
      styleList2.Spacing.Bottom = (Unit) "0ls";
      Guid yearGuid = this.YearGuid;
      using (ISession session1 = ReportBase.m_ps.InputSession.CreateSession())
      {
        Year year1 = session1.Get<Year>((object) this.YearGuid);
        Project project = this.curYear.Series.Project;
        string currencyString = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtCurrency, (object) this.CurrencySymbol, (object) this.CurrencyAbbreviation);
        IList<YearResult> yearResultList = session1.CreateCriteria<YearResult>().Add((ICriterion) Restrictions.Eq("Year", (object) year1)).AddOrder(Order.Desc("DateTime")).List<YearResult>();
        RenderText ro1 = new RenderText(string.Format("i-Tree Eco v{0}", (object) new Version(Application.ProductVersion).ToString(3)));
        ro1.Style.Spacing.Top = (Unit) "1ls";
        C1doc.Body.Children.Add((RenderObject) ro1);
        ro1.Style.FontBold = true;
        FileInfo fileInfo = new FileInfo(ReportBase.m_ps.InputSession.InputDb);
        C1doc.Body.Children.Add((RenderObject) new RenderText()
        {
          Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.ProjectCreated, (object) fileInfo.CreationTime.ToShortDateString())
        });
        C1doc.Body.Children.Add((RenderObject) new RenderText()
        {
          Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.ProjectLocation, (object) fileInfo.FullName)
        });
        RenderParagraph ro2 = new RenderParagraph();
        C1doc.Body.Children.Add((RenderObject) ro2);
        ro2.Style.Spacing.Bottom = (Unit) "1ls";
        ParagraphText po;
        if (this.curYear.Changed)
        {
          po = new ParagraphText(i_Tree_Eco_v6.Resources.Strings.ModelHasNOTBeenRun);
          po.Style.TextColor = Color.Red;
        }
        else
        {
          po = new ParagraphText(i_Tree_Eco_v6.Resources.Strings.ModelHASBeenRun);
          po.Style.TextColor = Color.Green;
        }
        ro2.Content.Add((ParagraphObject) po);
        RenderText ro3 = new RenderText(string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.ProjectType, this.curYear.Series.IsSample ? (object) i_Tree_Eco_v6.Resources.Strings.PlotBased : (object) i_Tree_Eco_v6.Resources.Strings.CompleteInventory));
        C1doc.Body.Children.Add((RenderObject) ro3);
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) v6Strings.Project_SingularName, (object) this.curYear.Series.Project.Name));
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) v6Strings.Series_SingularName, (object) this.curYear.Series.Id));
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) v6Strings.Year_SingularName, (object) this.curYear.Id));
        int num1 = session1.CreateCriteria<Tree>().CreateAlias("Plot", "p").CreateAlias("p.Year", "y").Add((ICriterion) Restrictions.Eq("y.Guid", (object) this.YearGuid)).SetProjection(Projections.RowCount()).UniqueResult<int>();
        if (this.curYear.Series.IsSample)
        {
          int num2 = session1.CreateCriteria<Plot>().CreateAlias("Year", "y").Add((ICriterion) Restrictions.Eq("y.Guid", (object) this.YearGuid)).SetProjection(Projections.RowCount()).UniqueResult<int>();
          int num3 = session1.CreateCriteria<Plot>().CreateAlias("Year", "y").Add((ICriterion) Restrictions.Eq("y.Guid", (object) this.YearGuid)).Add((ICriterion) Restrictions.Eq("IsComplete", (object) true)).SetProjection(Projections.RowCount()).UniqueResult<int>();
          this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.PlotsOfPlotsCompleted, (object) num3, (object) num2));
          this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.TotalNumberOfTrees, (object) num1.ToString("N0")));
        }
        else
          this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.TotalNumberOfTrees, (object) num1.ToString("N0")));
        RenderText ro4 = new RenderText(string.Format((IFormatProvider) CultureInfo.InvariantCulture, i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.ProjectInfo, (object) string.Empty));
        ro4.Style.Spacing.Top = (Unit) "1ls";
        C1doc.Body.Children.Add((RenderObject) ro4);
        List<Tuple<string, string>> projectLocationLayers = this.ProjectLocationLayers;
        if (projectLocationLayers.Count > 0)
        {
          this.RenderListItem(C1doc, styleBullet, styleList1, string.Format((IFormatProvider) CultureInfo.InvariantCulture, i_Tree_Eco_v6.Resources.Strings.LocationOfAProject, (object) string.Empty));
          for (int index = projectLocationLayers.Count - 1; index >= 0; --index)
          {
            string text = string.Format((IFormatProvider) CultureInfo.InvariantCulture, i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) projectLocationLayers[index].Item1, (object) projectLocationLayers[index].Item2);
            RenderArea ro5 = this.RenderIndentedItem(styleList2, text);
            C1doc.Body.Children.Add((RenderObject) ro5);
          }
        }
        else
          this.RenderListItem(C1doc, styleBullet, styleList1, string.Format((IFormatProvider) CultureInfo.InvariantCulture, i_Tree_Eco_v6.Resources.Strings.LocationOfAProject, (object) i_Tree_Eco_v6.Resources.Strings.Unavailable));
        GrowthPeriod growthPeriod = (GrowthPeriod) null;
        foreach (Location locAndParent in this.locAndParents)
        {
          if (locAndParent.GrowthPeriod != null)
          {
            growthPeriod = locAndParent.GrowthPeriod;
            break;
          }
        }
        if (growthPeriod != null)
        {
          this.RenderListItem(C1doc, styleBullet, styleList1, string.Format((IFormatProvider) CultureInfo.InvariantCulture, i_Tree_Eco_v6.Resources.Strings.LeafOnDaysOfALocation, (object) growthPeriod.LeafOnDays));
          this.RenderListItem(C1doc, styleBullet, styleList1, string.Format((IFormatProvider) CultureInfo.InvariantCulture, i_Tree_Eco_v6.Resources.Strings.LeafOffDaysOfALocation, (object) growthPeriod.LeafOffDays));
          this.RenderListItem(C1doc, styleBullet, styleList1, string.Format((IFormatProvider) CultureInfo.InvariantCulture, i_Tree_Eco_v6.Resources.Strings.FrostFreeDaysOfALocation, (object) growthPeriod.FrostFreeDays));
        }
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format((IFormatProvider) CultureInfo.InvariantCulture, i_Tree_Eco_v6.Resources.Strings.LongitudeOfALocation, (object) this.loc.Longitude));
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format((IFormatProvider) CultureInfo.InvariantCulture, i_Tree_Eco_v6.Resources.Strings.LatitudeOfALocation, (object) this.loc.Latitude));
        string str1 = ReportBase.FormatDoubleMeters((object) this.loc.Elevation);
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format((IFormatProvider) CultureInfo.InvariantCulture, i_Tree_Eco_v6.Resources.Strings.ElevationOfALocation, string.IsNullOrEmpty(str1) ? (object) i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr : (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, i_Tree_Eco_v6.Resources.Strings.FormatUnit, (object) ReportBase.FormatDoubleMeters((object) this.loc.Elevation), (object) ReportBase.MUnits())));
        if (this.curYear.RecordStrata)
        {
          double num4 = (double) this.curYear.Strata.Sum<Strata>((Func<Strata, float>) (s => s.Size));
          if (ReportBase.EnglishUnits && this.curYear.Unit == YearUnit.Metric)
            num4 *= 2.47105;
          if (!ReportBase.EnglishUnits && this.curYear.Unit == YearUnit.English)
            num4 /= 2.47105;
          this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.Area, (object) string.Format("{0} {1}", (object) num4.ToString("n2"), (object) ReportBase.HectaresUnits())));
        }
        else
          this.RenderListItem(C1doc, styleBullet, styleList1, i_Tree_Eco_v6.Resources.Strings.ProjectAreaNotConfigured);
        if (this.curYear.YearLocationData.Count > 0)
        {
          this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.Population, (object) (this.curYear.YearLocationData.FirstOrDefault<YearLocationData>()?.Population.ToString("#,#") ?? i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr)));
          this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.StudyAreaIsTreatedAsUrban, this.curYear.YearLocationData.First<YearLocationData>().ProjectLocation.IsUrban ? (object) i_Tree_Eco_v6.Resources.Strings.Yes : (object) i_Tree_Eco_v6.Resources.Strings.No));
          if (this.locAndParents[0].TropicalClimate.HasValue)
            this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.TropicalClimateEquationsUsed, this.ProjectIsUsingTropicalEquations() ? (object) i_Tree_Eco_v6.Resources.Strings.Yes : (object) i_Tree_Eco_v6.Resources.Strings.No));
        }
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.Units, this.curYear.Unit == YearUnit.English ? (object) v6Strings.YearUnit_English : (object) v6Strings.YearUnit_Metric));
        if (this.curYear.YearLocationData.Count > 0)
        {
          RenderText ro6 = new RenderText(i_Tree_Eco_v6.Resources.Strings.PollutionDetails);
          ro6.Style.Spacing.Top = (Unit) "1ls";
          C1doc.Body.Children.Add((RenderObject) ro6);
          this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) v6Strings.Year_SingularName, (object) this.curYear.YearLocationData.First<YearLocationData>().PollutionYear));
          RenderArea ro7 = new RenderArea();
          RenderText ro8 = new RenderText();
          RenderTable renderTable = new RenderTable();
          ro7.Children.Add((RenderObject) ro8);
          ro7.Children.Add((RenderObject) renderTable);
          C1doc.Body.Children.Add((RenderObject) ro7);
          renderTable.Style.Font = new Font("Calibri", 9f);
          renderTable.Style.TextAlignHorz = AlignHorzEnum.Left;
          renderTable.BreakAfter = BreakEnum.None;
          renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
          renderTable.Width = Unit.Auto;
          renderTable.SplitHorzBehavior = SplitBehaviorEnum.Never;
          int count = 1;
          renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
          int num5 = ReportUtil.FormatRenderTableHeader(renderTable);
          Style style = ReportUtil.AddAlternateStyle(renderTable);
          DataTable data = (DataTable) this.GetData();
          if (data == null)
          {
            this.hasCoordinates = false;
            this.DisplayStandardMessage(C1doc, i_Tree_Eco_v6.Resources.Strings.MsgNoData);
            return;
          }
          List<ColumnFormat> columnFormatList = this.ColumnsFormat(data);
          foreach (ColumnFormat columnFormat in columnFormatList)
            renderTable.Cells[0, columnFormat.ColNum].Text = columnFormat.HeaderText;
          int num6 = count;
          foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
          {
            foreach (ColumnFormat columnFormat in columnFormatList)
              renderTable.Cells[num6, columnFormat.ColNum].Text = columnFormat.Format(row[columnFormat.ColName]);
            if ((num6 - num5) % 2 == 0)
              renderTable.Rows[num6].Style.Parent = style;
            ++num6;
          }
        }
        string USAF = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        string WBAN = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        string notApplicableAbbr = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        string str2 = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        string str3 = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        YearLocationData yld = this.curYear.YearLocationData.FirstOrDefault<YearLocationData>();
        if (yld != null)
        {
          notApplicableAbbr = yld.WeatherYear.ToString();
          Match match = new Regex("^([0-9]{6})-([0-9]{5})$").Match(yld.WeatherStationId);
          if (match.Success)
          {
            USAF = match.Groups[1].Value;
            WBAN = match.Groups[2].Value;
            try
            {
              (string, bool)? nullable1 = RetryExecutionHandler.Execute<(string, bool)?>((Func<(string, bool)?>) (() =>
              {
                using (ISession session2 = ReportBase.m_ps.LocSp.OpenSession())
                {
                  using (ITransaction transaction = session2.BeginTransaction())
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    WeatherDetail weatherDetail = session2.QueryOver<WeatherDetail>().Fetch<WeatherDetail, WeatherDetail>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<WeatherDetail, object>>) (wd => wd.WeatherStation)).Where((System.Linq.Expressions.Expression<Func<WeatherDetail, bool>>) (wd => wd.Year == (int) this.yld.WeatherYear)).JoinQueryOver<WeatherStation>((System.Linq.Expressions.Expression<Func<WeatherDetail, WeatherStation>>) (wd => wd.WeatherStation)).Where((System.Linq.Expressions.Expression<Func<WeatherStation, bool>>) (ws => ws.USAF == this.USAF && ws.WBAN == this.WBAN)).Cacheable().SingleOrDefault();
                    transaction.Commit();
                    (string, bool)? nullable2 = new (string, bool)?();
                    if (weatherDetail != null)
                      nullable2 = new (string, bool)?((weatherDetail.WeatherStation.Name ?? i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr, weatherDetail.WeatherSubmissions.Count > 0));
                    return nullable2;
                  }
                }
              }));
              if (nullable1.HasValue)
              {
                (string, bool) tuple = nullable1.Value;
                str2 = tuple.Item1;
                str3 = tuple.Item2 ? i_Tree_Eco_v6.Resources.Strings.NCDCAndUserSubmitted : i_Tree_Eco_v6.Resources.Strings.NCDCOnly;
              }
            }
            catch
            {
            }
          }
        }
        RenderText ro9 = new RenderText(string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.WeatherStationDetails, (object) string.Empty));
        ro9.Style.Spacing.Top = (Unit) "1ls";
        C1doc.Body.Children.Add((RenderObject) ro9);
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) v6Strings.Year_SingularName, (object) notApplicableAbbr));
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.USAF, (object) USAF));
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.WBAN, (object) WBAN));
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.Name, (object) str2));
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.DataSource, (object) str3));
        RenderText ro10 = new RenderText(i_Tree_Eco_v6.Resources.Strings.NoteSeeAvoidedRunoff);
        C1doc.Body.Children.Add((RenderObject) ro10);
        if (this.curYear.Electricity == null || this.curYear.Gas != null || this.curYear.Carbon != null || this.curYear.H2O != null || this.curYear.CO != null || this.curYear.NO2 != null || this.curYear.O3 != null || this.curYear.SO2 != null || this.curYear.PM25 != null)
        {
          RenderText ro11 = new RenderText(string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.BenefitPrices, (object) string.Empty));
          ro11.Style.Spacing.Top = (Unit) "1ls";
          C1doc.Body.Children.Add((RenderObject) ro11);
          int num7 = !this.CheckNations() ? 1 : 0;
          double pricePer1 = -1.0;
          double pricePer2 = -1.0;
          double num8 = -1.0;
          double num9 = -1.0;
          int year2 = -1;
          int year3 = -1;
          int num10 = -1;
          int year4 = -1;
          LocationCost locationCost = this.LocationService.GetLocationCost(project.LocationId);
          if (locationCost != null)
          {
            pricePer1 = locationCost.Electricity;
            year2 = (int) locationCost.Year;
            pricePer2 = locationCost.Fuels / 10.002387672;
            year3 = (int) locationCost.Year;
          }
          LocationEnvironmentalValue environmentalValue = this.LocationService.GetEnvironmentalValue(project.LocationId);
          if (environmentalValue != null)
          {
            num8 = (double) environmentalValue.Carbon;
            num10 = environmentalValue.CarbonYear;
            num9 = environmentalValue.RainfallInterception * 264.172;
            year4 = environmentalValue.InterceptionYear;
          }
          string str4 = string.Format(i_Tree_Eco_v6.Resources.Strings.ElectricityValuePerKWh, (object) currencyString);
          if (this.curYear.Electricity != null)
          {
            if (year2 == -1)
              this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) str4, (object) this.curYear.Electricity.Price.ToString("N2")));
            else
              this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) str4, (object) this.getBenefitsStr(this.curYear.Electricity.Price, pricePer1, year2)));
          }
          else
            this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) str4, (object) this.getBenefitsStr(this.customizedElectricityDollarsPerKwh, pricePer1, year2)));
          if (this.curYear.Gas != null)
          {
            string str5 = string.Format(i_Tree_Eco_v6.Resources.Strings.FuelsValuePerTherm, (object) currencyString);
            if (year3 == -1)
              this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) str5, (object) this.curYear.Gas.Price.ToString("N2")));
            else
              this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) str5, (object) this.getBenefitsStr(this.curYear.Gas.Price, pricePer2, year3)));
          }
          if (this.curYear.Carbon != null)
          {
            string str6 = string.Format(i_Tree_Eco_v6.Resources.Strings.CarbonValuePerTonne, (object) currencyString, (object) ReportBase.TonneUnits());
            string str7 = (ReportBase.EnglishUnits ? this.curYear.Carbon.Price * 0.907185 : this.curYear.Carbon.Price).ToString("N2");
            if (num10 == -1)
              this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) str6, (object) str7));
            else
              this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.CarbonValuePerTonne, (object) currencyString, (object) ReportBase.TonneUnits()), (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtEcoDefaultValue, (object) str7, (object) (ReportBase.EnglishUnits ? num8 * 0.907185 : num8).ToString("N2"), (object) num10)));
          }
          if (this.curYear.H2O != null)
          {
            string str8 = string.Format(i_Tree_Eco_v6.Resources.Strings.AvoidedRunoffPerCubicMeter, (object) currencyString, ReportBase.EnglishUnits ? (object) i_Tree_Eco_v6.Resources.Strings.UnitGallon : (object) i_Tree_Eco_v6.Resources.Strings.UnitCubicMetersAbbr);
            double price = ReportBase.EnglishUnits ? this.curYear.H2O.Price / 264.172 : this.curYear.H2O.Price;
            double pricePer3 = ReportBase.EnglishUnits ? num9 / 264.172 : num9;
            string str9 = ReportBase.EnglishUnits ? "N4" : "N3";
            if (year4 == -1)
              this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) str8, (object) price.ToString(str9)));
            else
              this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) str8, (object) this.getBenefitsStr(price, pricePer3, year4, str9)));
          }
          if (num7 == 0)
          {
            if (this.curYear.CO != null)
              this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) this.getPollutantStr(i_Tree_Eco_v6.Resources.Strings.CarbonMonoxideFormula, currencyString), (object) this.getPollutantValueStr(this.curYear.CO.Price)));
            if (this.curYear.O3 != null)
              this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) this.getPollutantStr(i_Tree_Eco_v6.Resources.Strings.OzoneFurmula, currencyString), (object) this.getPollutantValueStr(this.curYear.O3.Price)));
            if (this.curYear.NO2 != null)
              this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) this.getPollutantStr(i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideFurmula, currencyString), (object) this.getPollutantValueStr(this.curYear.NO2.Price)));
            if (this.curYear.SO2 != null)
              this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) this.getPollutantStr(i_Tree_Eco_v6.Resources.Strings.SulfurDioxideFurmula, currencyString), (object) this.getPollutantValueStr(this.curYear.SO2.Price)));
            if (this.curYear.PM25 != null)
              this.RenderListItem(C1doc, styleBullet, styleList1, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) this.getPollutantStr(i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25Furmula, currencyString), (object) this.getPollutantValueStr(this.curYear.PM25.Price)));
          }
        }
        RenderText ro12 = new RenderText(string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.Models, (object) string.Empty));
        ro12.Style.Spacing.Top = (Unit) "1ls";
        C1doc.Body.Children.Add((RenderObject) ro12);
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format("Forecast v{0}", (object) new Version(Application.ProductVersion).ToString(3)));
        Version version = typeof (UFORE_D).Assembly.GetName().Version;
        this.RenderListItem(C1doc, styleBullet, styleList1, string.Format("UFORE-D v{0}.{1}.{2}", (object) version.Major, (object) version.Minor, (object) version.Revision));
        if (yearResultList.Count > 0)
        {
          RenderText ro13 = new RenderText(string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.ProcessingHistory, (object) string.Empty));
          ro13.Style.Spacing.Top = (Unit) "1ls";
          C1doc.Body.Children.Add((RenderObject) ro13);
          RenderTable renderTable = new RenderTable();
          C1doc.Body.Children.Add((RenderObject) renderTable);
          renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
          renderTable.Width = Unit.Auto;
          renderTable.Style.FontSize = 10f;
          renderTable.Style.Spacing.Bottom = (Unit) "1ls";
          renderTable.RowGroups[0, 1].Header = TableHeaderEnum.Page;
          renderTable.RowGroups[0, 1].Style.FontSize = 11f;
          renderTable.Cells[0, 0].Text = string.Format("{0}/{1}", (object) i_Tree_Eco_v6.Resources.Strings.Date, (object) i_Tree_Eco_v6.Resources.Strings.Time);
          renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.FileName;
          renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.Retrieved;
          int row = 1;
          foreach (YearResult yearResult in (IEnumerable<YearResult>) yearResultList)
          {
            if (yearResult.DateTime.HasValue)
              renderTable.Cells[row, 0].Text = yearResult.DateTime.Value.ToString("f");
            renderTable.Cells[row, 1].Text = yearResult.Data;
            renderTable.Cells[row, 2].Text = !yearResult.Completed ? i_Tree_Eco_v6.Resources.Strings.No : i_Tree_Eco_v6.Resources.Strings.Yes;
            ++row;
          }
          ReportUtil.FormatRenderTable(renderTable);
          renderTable.Style.TextAlignHorz = AlignHorzEnum.Left;
        }
        RenderText ro14 = new RenderText(i_Tree_Eco_v6.Resources.Strings.ReportAvailability);
        ro14.Style.FontBold = true;
        ro14.Style.FontSize = 16f;
        ro14.Style.FontUnderline = true;
        C1doc.Body.Children.Add((RenderObject) ro14);
        TreeView tvReportTree = new ShowAvailableReports(this.nation, this.curYear, false, false).tvReportTree;
        LineDef lineDef = LineDef.Default;
        RenderTable renderTable1 = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) renderTable1);
        renderTable1.Style.FontSize = 10f;
        renderTable1.Cols[0].Width = (Unit) "4%";
        renderTable1.Cols[1].Width = (Unit) "4%";
        renderTable1.Cols[2].Width = (Unit) "4%";
        renderTable1.Cols[3].Width = (Unit) "88%";
        int num11 = -1;
        foreach (TreeNode node1 in tvReportTree.Nodes)
        {
          ++num11;
          renderTable1.Cells[num11, node1.Level].Text = node1.Text;
          renderTable1.Cells[num11, node1.Level].Style.TextColor = node1.ForeColor == Color.Red ? Color.Red : Color.Black;
          renderTable1.Cells[num11, node1.Level].SpanCols = 4;
          renderTable1.Rows[num11].Style.Borders.Top = lineDef;
          renderTable1.Rows[num11].Style.Borders.Bottom = lineDef;
          if (node1.Nodes.Count > 0)
          {
            foreach (TreeNode node2 in node1.Nodes)
            {
              ++num11;
              renderTable1.Cells[num11, node2.Level].Text = node2.Text;
              renderTable1.Cells[num11, node2.Level].Style.TextColor = node2.ForeColor == Color.Red ? Color.Red : Color.Black;
              renderTable1.Cells[num11, node2.Level].SpanCols = 3;
              renderTable1.Rows[num11].Style.Borders.Top = lineDef;
              renderTable1.Rows[num11].Style.Borders.Bottom = lineDef;
              if (node2.Nodes.Count > 0)
              {
                foreach (TreeNode node3 in node2.Nodes)
                {
                  ++num11;
                  renderTable1.Cells[num11, node3.Level].Text = node3.Text;
                  renderTable1.Cells[num11, node3.Level].Style.TextColor = node3.ForeColor == Color.Red ? Color.Red : Color.Black;
                  renderTable1.Cells[num11, node3.Level].SpanCols = 2;
                  renderTable1.Rows[num11].Style.Borders.Top = lineDef;
                  renderTable1.Rows[num11].Style.Borders.Bottom = lineDef;
                  if (node3.Nodes.Count > 0)
                  {
                    foreach (TreeNode node4 in node3.Nodes)
                    {
                      ++num11;
                      renderTable1.Cells[num11, node4.Level].Text = node4.Text;
                      renderTable1.Cells[num11, node4.Level].Style.TextColor = node4.ForeColor == Color.Red ? Color.Red : Color.Black;
                      renderTable1.Rows[num11].Style.Borders.Top = lineDef;
                      renderTable1.Rows[num11].Style.Borders.Bottom = lineDef;
                    }
                  }
                }
              }
            }
          }
        }
        ReportUtil.FormatRenderTable(renderTable1);
        renderTable1.Style.TextAlignHorz = AlignHorzEnum.Left;
      }
    }

    public override object GetData() => (object) new PollutionStations(this.m_ProgramSession).GetData(this.curYear.YearLocationData.First<YearLocationData>().PollutionYear, this.project.LocationId);

    public override List<ColumnFormat> ColumnsFormat(DataTable data) => new PollutionStations(this.m_ProgramSession).ColumnsFormat(data);

    public List<Tuple<string, string>> ProjectLocationLayers
    {
      get
      {
        List<Tuple<string, string>> projectLocationLayers = new List<Tuple<string, string>>();
        if (this.locAndParents.Count > 0)
        {
          for (int index = 0; index < this.locAndParents.Count; ++index)
            projectLocationLayers.Add(new Tuple<string, string>(this.locAndParents[index].Type.ToString(), this.locAndParents[index].Name));
        }
        return projectLocationLayers;
      }
    }

    public override void SetAlignment(C1PrintDocument C1doc) => C1doc.Style.FlowAlignChildren = FlowAlignEnum.Default;

    private void RenderListItem(
      C1PrintDocument C1doc,
      Style styleBullet,
      Style styleList,
      string text)
    {
      RenderArea ro1 = this.RenderIndentedItem(styleList, text);
      RenderText ro2 = new RenderText("•", styleBullet);
      ro2.Name = "bullet";
      ro1.Children.Insert(0, (RenderObject) ro2);
      ro1.Children[1].Y = (Unit) "bullet.Top";
      C1doc.Body.Children.Add((RenderObject) ro1);
    }

    private RenderArea RenderIndentedItem(Style styleList, string text)
    {
      RenderArea renderArea = new RenderArea();
      renderArea.Style.Spacing.Top = (Unit) "0cm";
      renderArea.Style.Spacing.Bottom = (Unit) "0cm";
      renderArea.Children.Add((RenderObject) new RenderText(text, styleList));
      return renderArea;
    }

    private void RenderSubListItem(
      C1PrintDocument C1doc,
      Style styleBullet,
      Style styleList,
      string text)
    {
      RenderArea ro1 = this.RenderIndentedItem(styleList, text);
      RenderText ro2 = new RenderText("◦", styleBullet);
      ro2.Name = "bullet";
      ro1.Children.Insert(0, (RenderObject) ro2);
      ro1.Children[1].Y = (Unit) "bullet.Top";
      C1doc.Body.Children.Add((RenderObject) ro1);
    }

    protected override void SetLayout(C1PrintDocument C1doc)
    {
      base.SetLayout(C1doc);
      C1doc.PageLayout.PageSettings.Landscape = false;
    }
  }
}
