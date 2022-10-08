// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.BasicReport
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Views;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace i_Tree_Eco_v6.Reports
{
  [DesignerCategory("Code")]
  public class BasicReport : ReportBase
  {
    protected const int AuLocId = 21;
    protected const int CaLocId = 45;
    protected const int UsLocId = 219;
    protected const int UkLocId = 218;
    protected const int MxLocId = 138;
    protected Year curYear;
    protected Guid YearGuid;
    protected Series series;
    protected Project project;
    protected bool pollutionIsAvailable;
    protected ISession curInputISession;
    protected ISession locSpSession;
    protected Location loc;
    protected string ReportTitleRegular;
    protected List<Location> locAndParents;
    private Location _continent;
    private Location _nation;
    private Location _state;
    private Location _county;
    private Location _city;
    private LocationSpecies.Domain.Currency _currency;
    public LineDef tableBorderLineGray = new LineDef((Unit) "1pt", Color.Gray);

    public Location continent => this._continent;

    public Location nation => this._nation;

    public Location state => this._state;

    public Location county => this._county;

    public Location city => this._city;

    public string locationName => this.curYear.Series.Project.Name;

    public string ProjectLocation
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (this.locAndParents.Count > 0)
        {
          for (int index = 0; index < this.locAndParents.Count; ++index)
          {
            stringBuilder.Append(this.locAndParents[index].Name);
            if (index < this.locAndParents.Count - 1)
              stringBuilder.Append(", ");
          }
        }
        else
          stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.Unavailable);
        return stringBuilder.ToString();
      }
    }

    public string CurrencySymbol => this._currency != null ? this._currency.Symbol : string.Empty;

    public string CurrencyName => this._currency != null ? this._currency.Name : string.Empty;

    public string CurrencyAbbreviation => this._currency != null ? this._currency.Abbreviation : string.Empty;

    public string ProjectConvertFeetMeter(object val)
    {
      if (ReportBase.IsNull(val))
        return string.Empty;
      return this.curYear.Unit == YearUnit.English ? EstimateUtil.ConvertToMetric((double) val, Units.Feet, !ReportBase.EnglishUnits).ToString("N1") : EstimateUtil.ConvertToEnglish((double) val, Units.Meters, ReportBase.EnglishUnits).ToString("N1");
    }

    public string GetSpecies(string code)
    {
      SpeciesView speciesView;
      if (!ReportBase.m_ps.Species.TryGetValue(code, out speciesView))
        return code.ToUpper();
      return !ReportBase.ScientificName ? speciesView.CommonName : speciesView.ScientificName;
    }

    public BasicReport()
    {
      this.curInputISession = ReportBase.m_ps.InputSession.CreateSession();
      this.YearGuid = ReportBase.m_ps.InputSession.YearKey.Value;
      this.curYear = this.curInputISession.Get<Year>((object) ReportBase.m_ps.InputSession.YearKey);
      this.series = this.curYear.Series;
      this.project = this.series.Project;
      this.SetLocations();
      ReportBase.plotInventory = this.series.SampleType != SampleType.Inventory;
      this.pollutionIsAvailable = this.curYear.YearLocationData.FirstOrDefault<YearLocationData>().PollutionYear != (short) -1;
    }

    public override void InitDocument(C1PrintDocument C1doc)
    {
      this.SetLayout(C1doc);
      this.SetAlignment(C1doc);
      this.SetFont(C1doc);
      this.TagSettings(C1doc);
    }

    private void SetLocations()
    {
      this.locSpSession = ReportBase.m_ps.LocSp.OpenSession();
      this.loc = this.locSpSession.Get<Location>((object) this.project.LocationId);
      this.locAndParents = new List<Location>();
      LocationRelation locationRelation;
      for (Location location = this.loc; location != null; location = locationRelation.Parent)
      {
        NHibernateUtil.Initialize((object) location.GrowthPeriod);
        locationRelation = this.locSpSession.CreateCriteria<LocationRelation>().Add((ICriterion) Restrictions.Eq("Location", (object) location)).Add((ICriterion) Restrictions.IsNotNull("Code")).SetCacheable(true).UniqueResult<LocationRelation>();
        if (this._currency == null && location.Currency != null)
          this._currency = location.Currency;
        if (locationRelation != null)
        {
          if (locationRelation.Level == (short) 5)
            this._city = location;
          else if (locationRelation.Level == (short) 4)
            this._county = location;
          else if (locationRelation.Level == (short) 3)
            this._state = location;
          else if (locationRelation.Level == (short) 2)
          {
            this._nation = location;
          }
          else
          {
            if (locationRelation.Level == (short) 1)
            {
              this._continent = location;
              break;
            }
            break;
          }
          this.locAndParents.Add(location);
        }
        else
          break;
      }
      if (this._nation == null)
        this._nation = this._continent;
      if (this._state == null)
        this._state = this._nation;
      if (this._county == null)
        this._county = this._state;
      if (this._city != null)
        return;
      this._city = this._county;
    }

    public override void RenderHeader(C1PrintDocument C1doc)
    {
      RenderArea renderArea = new RenderArea();
      renderArea.Style.TextAlignHorz = AlignHorzEnum.Left;
      C1doc.PageLayout.PageHeader = (RenderObject) renderArea;
      RenderImage ro1 = new RenderImage((Image) i_Tree_Eco_v6.Properties.Resources.EcoLogo);
      ro1.Style.ImageAlign.KeepAspectRatio = true;
      ro1.Style.ImageAlign.StretchHorz = true;
      ro1.Style.ImageAlign.StretchVert = true;
      ro1.Name = "ri";
      ro1.Width = (Unit) "9%";
      ro1.Style.FlowAlign = FlowAlignEnum.Far;
      renderArea.Children.Add((RenderObject) ro1);
      RenderLine ro2 = new RenderLine();
      ro2.X = (Unit) "0";
      ro2.Y = (Unit) "ri.top + ri.height / 2 - height / 2";
      ro2.Name = "line";
      ro2.Width = (Unit) "parent.width - ri.width";
      ro2.Style.ShapeLine = new LineDef(Color.FromArgb(85, 133, 191));
      renderArea.Children.Add((RenderObject) ro2);
      RenderParagraph ro3 = new RenderParagraph();
      ro3.Width = (Unit) "parent.width - ri.width";
      renderArea.Children.Add((RenderObject) ro3);
      ro3.Style.TextColor = Color.FromArgb(85, 133, 191);
      ro3.Y = (Unit) "prev.top - self.height";
      ro3.X = (Unit) "0";
      ParagraphText po1 = new ParagraphText(this.ReportTitle);
      ro3.Content.Add((ParagraphObject) po1);
      po1.Style.FontBold = true;
      po1.Style.FontSize = 16f;
      if (!string.IsNullOrEmpty(this.ReportTitleRegular))
      {
        ParagraphText po2 = new ParagraphText(string.Format(" - {0}", (object) this.ReportTitleRegular));
        ro3.Content.Add((ParagraphObject) po2);
        po2.Style.FontSize = 14f;
      }
      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("{0}: ", (object) i_Tree_Eco_v6.Resources.Strings.Location);
      sb.Append(this.ProjectLocation);
      this.SetHeaderText(ref sb);
      RenderText ro4 = new RenderText(sb.ToString());
      ro4.Style.TextColor = Color.FromArgb(85, 133, 191);
      ro4.Style.FontSize = 10f;
      ro4.X = (Unit) "0";
      ro4.Y = (Unit) "line.top + line.height";
      renderArea.Children.Add((RenderObject) ro4);
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
    }

    protected virtual string ReportMessage() => string.Empty;

    protected bool CheckNations() => this._nation != null && NationFeatures.IsUSorUSlikeNation(this.nation.Id);

    protected bool ProjectIsUsingTropicalEquations() => this.curYear.Series.Project.Locations.Where<Eco.Domain.v6.ProjectLocation>((Func<Eco.Domain.v6.ProjectLocation, bool>) (p => p.LocationId == this.curYear.Series.Project.LocationId)).First<Eco.Domain.v6.ProjectLocation>().UseTropical;

    protected override void Note(C1PrintDocument C1doc)
    {
      RenderTable ro = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) ro);
      ro.Style.FontSize = 12f;
      ro.Style.Spacing.Top = (Unit) "1ls";
      ro.Width = (Unit) "100%";
      ro.Cols[0].Width = (Unit) "100%";
      ro.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      ro.Cells[0, 0].Text = this.ReportMessage();
    }

    protected override void DisplayStandardMessage(C1PrintDocument C1doc, string message)
    {
      C1doc.ClipPage = true;
      RenderTable ro = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) ro);
      ro.Style.FontSize = 12f;
      ro.Width = (Unit) "100%";
      ro.Cols[0].Width = (Unit) "100%";
      ro.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Center;
      ro.Cells[0, 0].Text = message;
    }

    public override void RenderFooter(C1PrintDocument C1doc)
    {
      RenderTable renderTable = new RenderTable();
      renderTable.Style.FontSize = 10f;
      renderTable.Cells[0, 2].Text = string.Format("{2} {0}PageNo{1}", (object) C1doc.TagOpenParen, (object) C1doc.TagCloseParen, (object) i_Tree_Eco_v6.Resources.Strings.Page);
      renderTable.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Style.GridLines.All = LineDef.Empty;
      C1doc.PageLayout.PageFooter = (RenderObject) renderTable;
    }

    protected virtual void SetLayout(C1PrintDocument C1doc)
    {
      C1doc.PageLayout.PageSettings.Landscape = true;
      C1doc.PageLayout.PageSettings.BottomMargin = (Unit) ".5in";
      C1doc.PageLayout.PageSettings.RightMargin = (Unit) ".5in";
      C1doc.PageLayout.PageSettings.TopMargin = (Unit) ".5in";
      C1doc.PageLayout.PageSettings.LeftMargin = (Unit) ".5in";
    }

    public virtual void SetAlignment(C1PrintDocument C1doc)
    {
      C1doc.Style.FlowAlign = FlowAlignEnum.Center;
      C1doc.Style.TextAlignHorz = AlignHorzEnum.Center;
      C1doc.Style.FlowAlignChildren = FlowAlignEnum.Center;
    }

    public virtual void SetFont(C1PrintDocument C1doc) => C1doc.Style.Font = new Font("Calibri", 12f);

    protected virtual void TagSettings(C1PrintDocument C1doc)
    {
      C1doc.TagOpenParen = "@@[";
      C1doc.TagCloseParen = "@@]";
    }

    public virtual void SetHeaderText(ref StringBuilder sb)
    {
      sb.Append(Environment.NewLine);
      sb.Append(string.Format("{0}, ", (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtReportHeader, (object) v6Strings.Project_SingularName, (object) this.project.Name)));
      sb.Append(string.Format("{0}, ", (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtReportHeader, (object) v6Strings.Series_SingularName, (object) this.series.Id)));
      sb.Append(string.Format("{0}{1}", (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtReportHeader, (object) v6Strings.Year_SingularName, (object) this.curYear.Id), (object) Environment.NewLine));
      sb.Append(string.Format("{0}", (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtReportHeader, (object) i_Tree_Eco_v6.Resources.Strings.Generated, (object) DateTime.Now.ToShortDateString())));
    }
  }
}
