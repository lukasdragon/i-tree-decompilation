// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ForecastSummary
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.Core;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Views;
using i_Tree_Eco_v6.Forms.Resources;
using LocationSpecies.Domain;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;

namespace i_Tree_Eco_v6.Reports
{
  public class ForecastSummary : ForecastReport
  {
    public ForecastSummary()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleForecastConfigurationSummary;
      this.SetConvertRatio(Conversions.InToCm);
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.addBasicTable(C1doc);
      if (this._forecast.Mortalities.Count > 3)
        this.addMortalityTable(C1doc);
      if (this._forecast.Replanting.Count > 0)
        this.addReplantingTable(C1doc);
      if (this._forecast.PestEvents.Count > 0)
        this.addPestsTable(C1doc);
      if (this._forecast.WeatherEvents.Count <= 0)
        return;
      this.getWeatherTable(C1doc);
    }

    private void addBasicTable(C1PrintDocument report)
    {
      IList<double> doubleList = this.baseMortalityRates();
      string str1 = doubleList[0].ToString("N1") + "%";
      string str2 = doubleList[1].ToString("N1") + "%";
      string str3 = doubleList[2].ToString("N1") + "%";
      string str4 = this._forecast.NumYears.ToString();
      string str5 = this._forecast.FrostFreeDays.ToString();
      RenderTable renderTable = new RenderTable();
      report.Body.Children.Add((RenderObject) renderTable);
      renderTable.Cells[0, 0].Text = ForecastRes.BasicParametersStr;
      renderTable.Rows[0].Style.FontBold = true;
      renderTable.RowGroups[0, 1].Header = TableHeaderEnum.Page;
      renderTable.Width = (Unit) "55%";
      renderTable.Cols[0].Width = (Unit) "40%";
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Cells[1, 0].Text = ForecastRes.ForecastYearsMessage;
      renderTable.Cells[2, 0].Text = ForecastRes.FrostFreeDaysMessage;
      renderTable.Cells[3, 0].Text = ForecastRes.HealthyMessage;
      renderTable.Cells[4, 0].Text = ForecastRes.SickMessage;
      renderTable.Cells[5, 0].Text = ForecastRes.DyingMessage;
      renderTable.Cells[1, 1].Text = str4;
      renderTable.Cells[2, 1].Text = str5;
      renderTable.Cells[3, 1].Text = str1;
      renderTable.Cells[4, 1].Text = str2;
      renderTable.Cells[5, 1].Text = str3;
      ReportUtil.FormatRenderTable(renderTable);
    }

    private void addMortalityTable(C1PrintDocument report)
    {
      RenderTable renderTable = new RenderTable();
      report.Body.Children.Add((RenderObject) renderTable);
      renderTable.Cells[0, 0].Text = ForecastRes.CustomMortalitiesStr;
      renderTable.Cells[0, 0].SpanCols = 4;
      renderTable.Cells[0, 0].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.RowGroups[0, 2].Style.FontBold = true;
      renderTable.RowGroups[0, 2].Header = TableHeaderEnum.Page;
      renderTable.Cells[1, 0].Text = ForecastRes.TypeStr;
      renderTable.Cells[1, 1].Text = ForecastRes.ValueStr;
      renderTable.Cells[1, 2].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.AnnualRate, "%");
      renderTable.Cells[1, 3].Text = i_Tree_Eco_v6.Resources.Strings.PercentType;
      IList<Mortality> mortalities = this.getMortalities();
      Func<Mortality, string> func = (Func<Mortality, string>) (m => !m.IsPercentStarting ? string.Format("% {0}", (object) i_Tree_Eco_v6.Resources.Strings.StartingPopulation) : string.Format("% {}", (object) i_Tree_Eco_v6.Resources.Strings.Annual));
      for (short index = 0; (int) index < mortalities.Count; ++index)
      {
        renderTable.Cells[(int) index + 2, 0].Text = mortalities[(int) index].Type;
        renderTable.Cells[(int) index + 2, 1].Text = mortalities[(int) index].Type == ForecastRes.GenusStr ? this.getCommonGenusName(mortalities[(int) index].Value) : mortalities[(int) index].Value;
        renderTable.Cells[(int) index + 2, 2].Text = mortalities[(int) index].Percent.ToString("N1");
        renderTable.Cells[(int) index + 2, 3].Text = func(mortalities[(int) index]);
      }
      renderTable.Width = (Unit) "70%";
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Cols[3].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.BreakAfter = BreakEnum.Line;
      ReportUtil.FormatRenderTable(renderTable);
    }

    private void addReplantingTable(C1PrintDocument report)
    {
      RenderTable renderTable = new RenderTable();
      report.Body.Children.Add((RenderObject) renderTable);
      renderTable.Cells[0, 0].Text = ForecastRes.ReplantTreesStr;
      renderTable.Cells[0, 0].SpanCols = 5;
      renderTable.Cells[0, 0].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.RowGroups[0, 2].Style.FontBold = true;
      renderTable.UserCellGroups.Add(new UserCellGroup(new Rectangle(1, 0, 1, 2))
      {
        Style = {
          TextAlignHorz = AlignHorzEnum.Center
        }
      });
      renderTable.RowGroups[0, 2].Header = TableHeaderEnum.Page;
      renderTable.Cells[1, 0].Text = ForecastRes.StratumStr;
      renderTable.Cells[1, 1].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.DBH, ReportBase.CentimeterUnits());
      renderTable.Cells[1, 2].Text = i_Tree_Eco_v6.Resources.Strings.Number;
      renderTable.Cells[1, 3].Text = i_Tree_Eco_v6.Resources.Strings.StartYear;
      renderTable.Cells[1, 4].Text = i_Tree_Eco_v6.Resources.Strings.DurationInYears;
      IList<Replanting> replanting = this.getReplanting();
      for (short index = 0; (int) index < replanting.Count; ++index)
      {
        renderTable.Cells[(int) index + 2, 0].Text = replanting[(int) index].StratumDesc;
        renderTable.Cells[(int) index + 2, 1].Text = (replanting[(int) index].DBH * this.convertRatio).ToString("N1");
        renderTable.Cells[(int) index + 2, 2].Text = replanting[(int) index].Number.ToString("N0");
        renderTable.Cells[(int) index + 2, 3].Text = replanting[(int) index].StartYear.ToString("N0");
        renderTable.Cells[(int) index + 2, 4].Text = replanting[(int) index].Duration.ToString("N0");
      }
      renderTable.Width = (Unit) "70%";
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Cols[3].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Cols[4].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.BreakAfter = BreakEnum.Line;
      ReportUtil.FormatRenderTable(renderTable);
    }

    private void addPestsTable(C1PrintDocument report)
    {
      RenderTable renderTable = new RenderTable();
      report.Body.Children.Add((RenderObject) renderTable);
      renderTable.Cells[0, 0].Text = ForecastRes.PestStr + " " + ForecastRes.ExtremeEventsStr;
      renderTable.Cells[0, 0].SpanCols = 5;
      renderTable.Cells[0, 0].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.RowGroups[0, 2].Style.FontBold = true;
      renderTable.UserCellGroups.Add(new UserCellGroup(new Rectangle(1, 0, 1, 2))
      {
        Style = {
          TextAlignHorz = AlignHorzEnum.Center
        }
      });
      renderTable.RowGroups[0, 2].Header = TableHeaderEnum.Page;
      renderTable.Cells[1, 0].Text = ForecastRes.ValueStr;
      renderTable.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.StartYear;
      renderTable.Cells[1, 2].Text = i_Tree_Eco_v6.Resources.Strings.DurationInYears;
      renderTable.Cells[1, 3].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.AnnualMortalityRate, "%");
      renderTable.Cells[1, 4].Text = i_Tree_Eco_v6.Resources.Strings.PlantPestHostsQuestion;
      IList<ForecastPestEvent> pestEvents = this.getPestEvents();
      Func<ForecastPestEvent, string> func = (Func<ForecastPestEvent, string>) (ee => !ee.PlantPestHosts ? i_Tree_Eco_v6.Resources.Strings.No : i_Tree_Eco_v6.Resources.Strings.Yes);
      for (short index = 0; (int) index < pestEvents.Count; ++index)
      {
        renderTable.Cells[(int) index + 2, 0].Text = this.getCommonPestName(pestEvents[(int) index].PestId);
        renderTable.Cells[(int) index + 2, 1].Text = pestEvents[(int) index].StartYear.ToString("N0");
        renderTable.Cells[(int) index + 2, 2].Text = pestEvents[(int) index].Duration.ToString("N0");
        renderTable.Cells[(int) index + 2, 3].Text = pestEvents[(int) index].MortalityPercent.ToString("N1");
        renderTable.Cells[(int) index + 2, 4].Text = func(pestEvents[(int) index]);
      }
      renderTable.Width = (Unit) "70%";
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Cols[3].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Cols[4].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.BreakAfter = BreakEnum.Line;
      ReportUtil.FormatRenderTable(renderTable);
    }

    private void getWeatherTable(C1PrintDocument report)
    {
      RenderTable renderTable = new RenderTable();
      report.Body.Children.Add((RenderObject) renderTable);
      renderTable.Cells[0, 0].Text = ForecastRes.WeatherStr + " " + ForecastRes.ExtremeEventsStr;
      renderTable.Cells[0, 0].SpanCols = 3;
      renderTable.Cells[0, 0].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.RowGroups[0, 2].Style.FontBold = true;
      renderTable.UserCellGroups.Add(new UserCellGroup(new Rectangle(1, 0, 1, 2))
      {
        Style = {
          TextAlignHorz = AlignHorzEnum.Center
        }
      });
      renderTable.RowGroups[0, 2].Header = TableHeaderEnum.Page;
      renderTable.Cells[1, 0].Text = ForecastRes.ValueStr;
      renderTable.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.OccurrsInYear;
      renderTable.Cells[1, 2].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.MortalityRate, "%");
      IList<ForecastWeatherEvent> weatherEvents = this.getWeatherEvents();
      for (short index = 0; (int) index < weatherEvents.Count; ++index)
      {
        renderTable.Cells[(int) index + 2, 0].Text = EnumHelper.GetDescription<WeatherEvent>(weatherEvents[(int) index].WeatherEvent);
        renderTable.Cells[(int) index + 2, 1].Text = weatherEvents[(int) index].StartYear.ToString("N0");
        renderTable.Cells[(int) index + 2, 2].Text = weatherEvents[(int) index].MortalityPercent.ToString("N1");
      }
      renderTable.Width = (Unit) "70%";
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.BreakAfter = BreakEnum.Line;
      ReportUtil.FormatRenderTable(renderTable);
    }

    private IList<double> baseMortalityRates()
    {
      Guid? forecastKey = ReportBase.m_ps.InputSession.ForecastKey;
      return this.curInputISession.QueryOver<Mortality>().Where((Expression<Func<Mortality, bool>>) (m => (Guid?) m.Forecast.Guid == ReportBase.m_ps.InputSession.ForecastKey && m.Type == ForecastRes.BaseStr)).OrderBy((Expression<Func<Mortality, object>>) (m => (object) m.Percent)).Asc.Select((Expression<Func<Mortality, object>>) (m => (object) m.Percent)).List<double>();
    }

    private IList<Mortality> getMortalities() => this.curInputISession.QueryOver<Mortality>().Where((Expression<Func<Mortality, bool>>) (m => (Guid?) m.Forecast.Guid == ReportBase.m_ps.InputSession.ForecastKey && m.Type != ForecastRes.BaseStr)).OrderBy((Expression<Func<Mortality, object>>) (m => m.Type)).Asc.ThenBy((Expression<Func<Mortality, object>>) (m => m.Value)).Asc.List();

    private IList<Replanting> getReplanting() => this.curInputISession.QueryOver<Replanting>().Where((Expression<Func<Replanting, bool>>) (r => (Guid?) r.Forecast.Guid == ReportBase.m_ps.InputSession.ForecastKey)).OrderBy((Expression<Func<Replanting, object>>) (r => (object) r.DBH)).Asc.ThenBy((Expression<Func<Replanting, object>>) (r => (object) r.DBH)).Asc.ThenBy((Expression<Func<Replanting, object>>) (r => (object) r.StartYear)).Asc.List();

    private IList<ForecastPestEvent> getPestEvents() => this.curInputISession.QueryOver<ForecastPestEvent>().Where((Expression<Func<ForecastPestEvent, bool>>) (p => (Guid?) p.Forecast.Guid == ReportBase.m_ps.InputSession.ForecastKey)).OrderBy((Expression<Func<ForecastPestEvent, object>>) (p => (object) p.StartYear)).Asc.ThenBy((Expression<Func<ForecastPestEvent, object>>) (p => (object) p.PestId)).Asc.List();

    private IList<ForecastWeatherEvent> getWeatherEvents() => this.curInputISession.QueryOver<ForecastWeatherEvent>().Where((Expression<Func<ForecastWeatherEvent, bool>>) (w => (Guid?) w.Forecast.Guid == ReportBase.m_ps.InputSession.ForecastKey)).OrderBy((Expression<Func<ForecastWeatherEvent, object>>) (w => (object) w.StartYear)).Asc.ThenBy((Expression<Func<ForecastWeatherEvent, object>>) (w => (object) w.WeatherEvent)).Asc.List();

    private string getCommonGenusName(string spCode)
    {
      SpeciesView speciesView;
      return ReportBase.m_ps.Species.TryGetValue(spCode, out speciesView) ? speciesView.CommonName : (string) null;
    }

    private string getCommonPestName(int pest_id) => RetryExecutionHandler.Execute<string>((Func<string>) (() =>
    {
      using (ISession session = Program.Session.LocSp.OpenSession())
      {
        // ISSUE: reference to a compiler-generated field
        return session.QueryOver<Pest>().Where((Expression<Func<Pest, bool>>) (p => p.Id == this.pest_id)).Select((Expression<Func<Pest, object>>) (p => p.CommonName)).SingleOrDefault<string>();
      }
    }));
  }
}
