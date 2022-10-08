// Decompiled with JetBrains decompiler
// Type: EcoIpedReportGenerator.RgControl
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using C1.C1Preview.Export;
using C1.Win.C1FlexGrid;
using C1.Win.C1Preview;
using i_Tree_Eco_v6;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EcoIpedReportGenerator
{
  public class RgControl : UserControl
  {
    private object curReport;
    private IPEDReports _curReport;
    private bool _useScientificName;
    private ReportGenerator m_rg;
    private ToolStripLabel[] lblBindings = new ToolStripLabel[2];
    private ToolStripComboBox[] cboBindings = new ToolStripComboBox[2];
    private IContainer components;
    private SplitContainer scMain;
    internal Panel pnlAll;
    internal Panel pnlAllRpt;
    internal Label lblAllHdr;
    private Panel panel3;
    private Button cmdPrint;
    private Button cmdExport;
    private C1.Win.C1FlexGrid.C1FlexGrid controlForLicense;
    private C1PrintDocument c1PrintDocument1;
    private C1PrintPreviewControl c1PrintPreviewControl1;
    private ToolStrip mnuReportBindings;

    public bool UseScientificName
    {
      get => this._useScientificName;
      set
      {
        this._useScientificName = value;
        this.GenerateReport(staticData.IsSample, this._useScientificName, (int) this._curReport, staticData.YearGuid, staticData.ProgramSession);
      }
    }

    public RgControl()
    {
      this.InitializeComponent();
      this.OnCreateControl();
      this.InitBindings();
      this.mnuReportBindings.SendToBack();
      this.lblAllHdr.SendToBack();
    }

    private void InitBindings()
    {
      for (int index = 0; index < 2; ++index)
      {
        this.lblBindings[index] = new ToolStripLabel();
        this.lblBindings[index].AutoSize = true;
        this.lblBindings[index].Font = new Font("Calibri", 12f);
        this.lblBindings[index].Visible = true;
        this.lblBindings[index].Name = "lblBinding" + index.ToString();
        this.lblBindings[index].Padding = new Padding(2, 0, 0, 0);
        this.mnuReportBindings.Items.Add((ToolStripItem) this.lblBindings[index]);
        this.cboBindings[index] = new ToolStripComboBox();
        this.cboBindings[index].Font = new Font("Calibri", 12f);
        this.cboBindings[index].ComboBox.Width = 250;
        this.cboBindings[index].Height = 21;
        this.cboBindings[index].Visible = true;
        this.cboBindings[index].Name = "cboBinding" + index.ToString();
        this.cboBindings[index].Padding = new Padding(2, 0, 0, 0);
        this.cboBindings[index].DropDownStyle = ComboBoxStyle.DropDownList;
        this.mnuReportBindings.Items.Add((ToolStripItem) this.cboBindings[index]);
      }
    }

    private void fg_OwnerDrawCell(object sender, OwnerDrawCellEventArgs e)
    {
      C1.Win.C1FlexGrid.C1FlexGrid c1FlexGrid = (C1.Win.C1FlexGrid.C1FlexGrid) sender;
      if (!((string) c1FlexGrid.Cols[e.Col].UserData == "GroupBy") || c1FlexGrid.Rows[e.Row].IsNode)
        return;
      e.Text = string.Empty;
    }

    public void GenerateReport(
      bool IsSample,
      bool UseScientificName,
      int U4Report,
      Guid YearGuid,
      ProgramSession m_ps)
    {
      bool flag = false;
      this.Cursor = Cursors.WaitCursor;
      this.pnlAll.Dock = DockStyle.Fill;
      this.pnlAll.Visible = false;
      this.lblAllHdr.Visible = false;
      this.lblAllHdr.Dock = DockStyle.Top;
      this.pnlAllRpt.Visible = false;
      this.pnlAllRpt.Dock = DockStyle.Fill;
      this.c1PrintPreviewControl1.Visible = false;
      if (staticData.ProgramSession != m_ps)
      {
        staticData.ClearData();
        staticData.IsSample = IsSample;
        staticData.YearGuid = YearGuid;
        staticData.ProgramSession = m_ps;
        staticData.LoadData();
        flag = true;
      }
      if (!staticData.DsData.Tables.Contains("EstMapLanduse"))
        flag = true;
      staticData.UseScientificName = UseScientificName;
      this._curReport = (IPEDReports) U4Report;
      for (int index = 0; index <= this.cboBindings.GetUpperBound(0); ++index)
      {
        this.cboBindings[index].SelectedIndexChanged -= new EventHandler(this.cboBinding_SelectedValueChanged);
        this.cboBindings[index].ComboBox.DataSource = (object) null;
        this.cboBindings[index].ComboBox.DisplayMember = string.Empty;
        this.cboBindings[index].ComboBox.ValueMember = string.Empty;
        this.cboBindings[index].Visible = false;
        this.lblBindings[index].Visible = false;
      }
      switch (this._curReport)
      {
        case IPEDReports.u4IpedPrimaryPestSummaryofTreesforLanduses:
          if (flag && !staticData.LoadEstimateDatabase())
            this.Cursor = Cursors.Default;
          this.ShowPrimaryPestSummary();
          break;
        case IPEDReports.u4IpedPrimaryPestDetailsForLanduses:
          this.ShowPrimaryPestDetails();
          break;
        case IPEDReports.u4IpedPestSignSymptomOverviewbySpecies:
          if (flag && !staticData.LoadEstimateDatabase())
            this.Cursor = Cursors.Default;
          this.ShowPestSignAndSymptomOverviewBySpecies();
          break;
        case IPEDReports.u4IpedPestSignSymptomOverviewbyLanduses:
          if (flag && !staticData.LoadEstimateDatabase())
            this.Cursor = Cursors.Default;
          this.ShowPestSignAndSymptomOverviewLandUse();
          break;
        case IPEDReports.u4IpedPestSignSymptomDetailsSummarybySpecies:
          if (flag && !staticData.LoadEstimateDatabase())
            this.Cursor = Cursors.Default;
          this.ShowPestSignAndSymptomDetailsSummaryBySpecies();
          break;
        case IPEDReports.u4IpedPestSignSymptomDetailsCompletebySpecies:
          if (flag && !staticData.LoadEstimateDatabase())
            this.Cursor = Cursors.Default;
          this.ShowPestSignAndSymptomDetailsCompleteBySpecies();
          break;
        case IPEDReports.u4IpedPestSignSymptomDetailsSummarybyLanduses:
          if (flag && !staticData.LoadEstimateDatabase())
            this.Cursor = Cursors.Default;
          this.ShowPestSignAndSymptomDetailsSummaryByLanduse();
          break;
        case IPEDReports.u4IpedPestSignSymptomDetailsCompletebyLanduse:
          if (flag && !staticData.LoadEstimateDatabase())
            this.Cursor = Cursors.Default;
          this.ShowPestSignAndSymptomDetailsCompleteByLanduse();
          break;
        case IPEDReports.u4IpedPestSignSymptomReviewofTrees:
          this.ShowPestSSReview();
          break;
        case IPEDReports.u4IpedPestReviewofTreesandDetailedPest:
          this.ShowPestReview();
          break;
      }
      this.Cursor = Cursors.Default;
    }

    private void fg_AfterEdit(object sender, RowColEventArgs e)
    {
      C1.Win.C1FlexGrid.C1FlexGrid c1FlexGrid = (C1.Win.C1FlexGrid.C1FlexGrid) sender;
      CurrencyManager currencyManager = (CurrencyManager) this.BindingContext[c1FlexGrid.DataSource, c1FlexGrid.DataMember];
      this.Cursor = Cursors.WaitCursor;
      currencyManager.EndCurrentEdit();
      this.Cursor = Cursors.Default;
    }

    private void fg_AfterDataRefresh(object sender, ListChangedEventArgs e) => this.fg_SetupColumns(sender, EventArgs.Empty);

    private bool ShowPestReview()
    {
      this.BindToReport((ReportGenerator) new PestReviewRG());
      return true;
    }

    private bool ShowPestSSReview()
    {
      this.BindToReport((ReportGenerator) new PestSSReviewRG());
      return true;
    }

    private void BindToReport(ReportGenerator rg)
    {
      this.m_rg = rg;
      this.DisplayBindings(BindingFor.All);
      this.SetReport(this.m_rg.Generate());
    }

    private void DisplayBindings(BindingFor type)
    {
      IBindable rg = this.m_rg as IBindable;
      int index1 = 0;
      if (rg != null)
      {
        this.mnuReportBindings.Visible = false;
        List<DataBinding> dataBindingList = rg.Bindings();
        for (int index2 = 0; index2 <= dataBindingList.Count - 1; ++index2)
        {
          DataBinding dataBinding = dataBindingList[index2];
          if (dataBinding.BindableFor(type))
          {
            this.cboBindings[index1].SelectedIndexChanged -= new EventHandler(this.cboBinding_SelectedValueChanged);
            this.lblBindings[index1].Text = dataBinding.Description + ":";
            this.cboBindings[index1].ComboBox.DataSource = (object) null;
            this.cboBindings[index1].ComboBox.DisplayMember = dataBinding.DisplayMember;
            this.cboBindings[index1].ComboBox.ValueMember = dataBinding.ValueMember;
            this.cboBindings[index1].ComboBox.DataSource = dataBinding.DataSource;
            dataBinding.Value = this.cboBindings[index1].ComboBox.SelectedValue;
            this.cboBindings[index1].SelectedIndexChanged += new EventHandler(this.cboBinding_SelectedValueChanged);
            this.cboBindings[index1].Visible = true;
            this.lblBindings[index1].Visible = true;
            this.mnuReportBindings.Visible = true;
            ++index1;
          }
        }
      }
      for (int index3 = index1; index3 <= this.cboBindings.GetUpperBound(0); ++index3)
      {
        this.cboBindings[index3].SelectedIndexChanged -= new EventHandler(this.cboBinding_SelectedValueChanged);
        this.cboBindings[index3].ComboBox.DataSource = (object) null;
        this.cboBindings[index3].ComboBox.DisplayMember = string.Empty;
        this.cboBindings[index3].ComboBox.ValueMember = string.Empty;
        this.lblBindings[index3].Visible = false;
        this.cboBindings[index3].Visible = false;
      }
      if (index1 != 0)
        return;
      this.mnuReportBindings.Visible = false;
    }

    private void cboBinding_SelectedValueChanged(object sender, EventArgs e)
    {
      IBindable rg = this.m_rg as IBindable;
      if (((ToolStripComboBox) sender).SelectedItem == null || rg == null)
        return;
      for (int index1 = 0; index1 <= this.cboBindings.GetUpperBound(0); ++index1)
      {
        if (sender.Equals((object) this.cboBindings[index1]))
        {
          for (int index2 = 0; index2 <= rg.Bindings().Count - 1; ++index2)
          {
            if (this.cboBindings[index1].ComboBox.DataSource.Equals(rg.Bindings()[index2].DataSource))
            {
              rg.Bindings()[index2].Value = this.cboBindings[index1].ComboBox.SelectedValue;
              this.SetReport(this.m_rg.Generate());
            }
          }
        }
      }
    }

    private bool ShowPrimaryPestSummary()
    {
      this.BindToReport((ReportGenerator) new PrimaryPestSummaryByLanduseRG());
      return true;
    }

    private bool ShowPrimaryPestDetails()
    {
      this.BindToReport((ReportGenerator) new PrimaryPestDetailsForLandusesRG());
      return true;
    }

    private bool ShowPestSignAndSymptomOverviewBySpecies()
    {
      this.BindToReport((ReportGenerator) new PestSignAndSymptomOverviewBySpeciesRG());
      return true;
    }

    private bool ShowPestSignAndSymptomOverviewLandUse()
    {
      this.BindToReport((ReportGenerator) new PestSignSymptomOverviewbyLandusesRG());
      return true;
    }

    private bool ShowPestSignAndSymptomDetailsSummaryBySpecies()
    {
      this.BindToReport((ReportGenerator) new PestSignSymptomDetailsSummarybySpeciesRG());
      return true;
    }

    private bool ShowPestSignAndSymptomDetailsCompleteBySpecies()
    {
      this.BindToReport((ReportGenerator) new PestSignSymptomDetailsCompletebySpeciesRG());
      return true;
    }

    private bool ShowPestSignAndSymptomDetailsCompleteByLanduse()
    {
      this.BindToReport((ReportGenerator) new PestSignSymptomDetailsByStratumRG());
      return true;
    }

    private bool ShowPestSignAndSymptomDetailsSummaryByLanduse()
    {
      this.BindToReport((ReportGenerator) new PestSignSymptomDetailsSummarybyLandusesRG());
      return true;
    }

    private void fg_SetupColumns(object sender, EventArgs e)
    {
      C1.Win.C1FlexGrid.C1FlexGrid c1FlexGrid = sender as C1.Win.C1FlexGrid.C1FlexGrid;
      CurrencyManager currencyManager = (CurrencyManager) this.BindingContext[c1FlexGrid.DataSource, c1FlexGrid.DataMember];
      DataTable dataTable = (DataTable) null;
      if (currencyManager.List is DataView)
        dataTable = ((DataView) currencyManager.List).Table;
      else if (currencyManager.List is BindingSource)
        dataTable = ((DataView) ((BindingSource) currencyManager.List).List).Table;
      if (dataTable.ExtendedProperties.Contains((object) "TreeStyleFlag"))
      {
        c1FlexGrid.Tree.Style = TreeStyleFlags.Lines;
        c1FlexGrid.Tree.Column = c1FlexGrid.Cols.Fixed;
        c1FlexGrid.AllowMerging = AllowMergingEnum.Nodes;
      }
      else
      {
        c1FlexGrid.Tree.Style = TreeStyleFlags.Complete;
        c1FlexGrid.Tree.Column = c1FlexGrid.Cols.Fixed;
        c1FlexGrid.AllowMerging = AllowMergingEnum.Nodes;
      }
      if (dataTable != null && dataTable.ExtendedProperties[(object) "GroupBy"] is string[] extendedProperty1)
      {
        for (int index1 = 0; index1 <= extendedProperty1.Length - 1; ++index1)
        {
          int num = c1FlexGrid.Cols.Fixed + index1;
          for (int index2 = 0; index2 <= c1FlexGrid.Cols.Count - 1; ++index2)
          {
            if (c1FlexGrid.Cols[index2].Name.Equals(extendedProperty1[index1]))
            {
              c1FlexGrid.Cols[index2].UserData = (object) "GroupBy";
              c1FlexGrid.Cols[index2].Move(num);
              c1FlexGrid.Subtotal(AggregateEnum.None, index1, num, c1FlexGrid.Cols.Count - 1, "{0}");
              if (index1 > 0)
              {
                c1FlexGrid.Cols[index1].Visible = false;
                break;
              }
              c1FlexGrid.Cols[index1].Caption = "";
              break;
            }
          }
        }
      }
      c1FlexGrid.AutoSizeCols();
      if (dataTable == null)
        return;
      int int32 = Convert.ToInt32(dataTable.ExtendedProperties[(object) "DefaultLevel"]);
      c1FlexGrid.Tree.Show(int32);
      if (dataTable.ExtendedProperties[(object) "ShowColumns"] is string[] extendedProperty2)
      {
        for (int index3 = 0; index3 < c1FlexGrid.Cols.Count; ++index3)
        {
          Column col = c1FlexGrid.Cols[index3];
          bool flag = false;
          for (int index4 = 0; index4 <= extendedProperty2.Length - 1; ++index4)
          {
            if (extendedProperty2[index4] == col.Name)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            c1FlexGrid.Cols.Remove(col);
            --index3;
          }
        }
      }
      if (dataTable.ExtendedProperties[(object) "HideColumns"] is string[] extendedProperty3)
      {
        for (int index5 = 0; index5 < c1FlexGrid.Cols.Count; ++index5)
        {
          Column col = c1FlexGrid.Cols[index5];
          bool flag = false;
          for (int index6 = 0; index6 <= extendedProperty3.Length - 1; ++index6)
          {
            if (extendedProperty3[index6] == col.Name)
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            c1FlexGrid.Cols.Remove(col);
            --index5;
          }
        }
      }
      if (!c1FlexGrid.AllowEditing || !(dataTable.ExtendedProperties[(object) "EditableColumns"] is string[] extendedProperty4))
        return;
      foreach (Column col in (IEnumerable) c1FlexGrid.Cols)
      {
        bool flag = false;
        for (int index = 0; index <= extendedProperty4.Length - 1; ++index)
        {
          if (extendedProperty4[index] == col.Name)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          col.AllowEditing = false;
      }
    }

    public void SetReport(object rpt)
    {
      bool flag = rpt != null;
      Panel pnlAllRpt = this.pnlAllRpt;
      Label lblAllHdr = this.lblAllHdr;
      this.curReport = (object) null;
      this.cmdExport.Enabled = flag;
      if (!flag)
        return;
      try
      {
        switch (rpt)
        {
          case FlexGridReport _:
            this.pnlAll.Visible = true;
            FlexGridReport flexGridReport = (FlexGridReport) rpt;
            lblAllHdr.Text = flexGridReport.Title;
            while (pnlAllRpt.Controls.Count > 0)
              pnlAllRpt.Controls.RemoveAt(0);
            Control control = (Control) pnlAllRpt;
            for (int index1 = 0; index1 <= flexGridReport.Data.Count - 1; ++index1)
            {
              C1FlexDataTree c1FlexDataTree = new C1FlexDataTree();
              c1FlexDataTree.SetupColumns += new EventHandler(this.fg_SetupColumns);
              c1FlexDataTree.ChildOwnerDrawCell += new OwnerDrawCellEventHandler(this.fg_OwnerDrawCell);
              c1FlexDataTree.OwnerDrawCell += new OwnerDrawCellEventHandler(this.fg_OwnerDrawCell);
              c1FlexDataTree.AfterDataRefresh += new ListChangedEventHandler(this.fg_AfterDataRefresh);
              c1FlexDataTree.Dock = DockStyle.Fill;
              c1FlexDataTree.DrawMode = DrawModeEnum.OwnerDraw;
              c1FlexDataTree.AllowSorting = AllowSortingEnum.None;
              c1FlexDataTree.AllowDragging = AllowDraggingEnum.None;
              c1FlexDataTree.ExtendLastCol = true;
              c1FlexDataTree.AllowEditing = flexGridReport.Data[index1].CanModify;
              c1FlexDataTree.Cols.Fixed = 0;
              if (flexGridReport.Data[index1].CanModify)
                c1FlexDataTree.AfterEdit += new RowColEventHandler(this.fg_AfterEdit);
              for (int index2 = 13; index2 <= 18; ++index2)
              {
                c1FlexDataTree.Styles[index2].BackColor = Color.White;
                c1FlexDataTree.Styles[index2].ForeColor = Color.Black;
                c1FlexDataTree.Styles[index2].Font = new Font(c1FlexDataTree.Styles[index2].Font, FontStyle.Bold);
              }
              c1FlexDataTree.Styles[CellStyleEnum.Highlight].BackColor = Color.White;
              c1FlexDataTree.Styles[CellStyleEnum.Highlight].ForeColor = Color.Black;
              c1FlexDataTree.SelectionMode = SelectionModeEnum.Row;
              c1FlexDataTree.DataMember = flexGridReport.Data[index1].DataMember;
              c1FlexDataTree.DataSource = flexGridReport.Data[index1].DataSource;
              Label label = new Label();
              label.Font = new Font(label.Font, FontStyle.Bold);
              label.Dock = DockStyle.Top;
              label.TextAlign = ContentAlignment.MiddleLeft;
              label.Text = flexGridReport.Data[index1].Header;
              if (index1 != flexGridReport.Data.Count - 1)
              {
                SplitContainer splitContainer = new SplitContainer();
                splitContainer.Dock = DockStyle.Fill;
                splitContainer.Orientation = Orientation.Horizontal;
                splitContainer.Panel1.Controls.Add((Control) c1FlexDataTree);
                splitContainer.Panel1.Controls.Add((Control) label);
                splitContainer.SplitterDistance = 100 / (flexGridReport.Data.Count - index1);
                control.Controls.Add((Control) splitContainer);
                control = (Control) splitContainer.Panel2;
              }
              else
              {
                control.Controls.Add((Control) c1FlexDataTree);
                control.Controls.Add((Control) label);
              }
            }
            if (flexGridReport.ViewStatic)
            {
              C1PrintDocument doc = new C1PrintDocument();
              doc.PageLayout.PageSettings.Landscape = true;
              this.BuildPrintDocument(doc, (Control) this.pnlAll);
              this.c1PrintPreviewControl1.Document = (object) doc;
              this.c1PrintPreviewControl1.PreviewPane.ZoomMode = ZoomModeEnum.PageWidth;
              this.pnlAll.Visible = false;
              this.c1PrintPreviewControl1.Dock = DockStyle.Fill;
              this.c1PrintPreviewControl1.Visible = true;
              this.mnuReportBindings.Parent = (Control) this;
              break;
            }
            this.pnlAll.Visible = true;
            pnlAllRpt.Visible = true;
            this.lblAllHdr.Visible = true;
            this.mnuReportBindings.Parent = (Control) this.pnlAll;
            this.mnuReportBindings.SendToBack();
            this.lblAllHdr.SendToBack();
            break;
          case C1PrintDocument _:
            C1PrintDocument c1PrintDocument = (C1PrintDocument) rpt;
            c1PrintDocument.PageLayout.PageSettings.Landscape = true;
            this.c1PrintPreviewControl1.Document = (object) c1PrintDocument;
            this.c1PrintPreviewControl1.PreviewPane.ZoomMode = ZoomModeEnum.PageWidth;
            this.pnlAll.Visible = false;
            this.c1PrintPreviewControl1.Dock = DockStyle.Fill;
            this.c1PrintPreviewControl1.Visible = true;
            this.mnuReportBindings.Parent = (Control) this;
            break;
        }
        this.curReport = rpt;
      }
      catch (Exception ex)
      {
      }
    }

    private void cmdPrint_Click(object sender, EventArgs e) => this.PrintReport();

    public void PrintReport()
    {
      C1PrintDocument doc = new C1PrintDocument();
      this.BuildPrintDocument(doc, (Control) this.pnlAll);
      doc.Generate();
      PrintDialog printDialog = new PrintDialog();
      printDialog.PrinterSettings.MinimumPage = 1;
      printDialog.PrinterSettings.MaximumPage = doc.Pages.Count;
      printDialog.PrinterSettings.FromPage = 1;
      printDialog.PrinterSettings.ToPage = doc.Pages.Count;
      printDialog.AllowSomePages = true;
      printDialog.UseEXDialog = true;
      if (printDialog.ShowDialog() != DialogResult.OK)
        return;
      doc.Print(printDialog.PrinterSettings, new OutputRange(printDialog.PrinterSettings.FromPage, printDialog.PrinterSettings.ToPage), true);
    }

    private void BuildPrintDocument(C1PrintDocument doc, Control c)
    {
      switch (c)
      {
        case Label _:
          Label label = (Label) c;
          RenderText ro = new RenderText(doc);
          ro.Text = label.Text.Replace("&&", "&");
          ro.Style.Font = label.Font;
          if (label.TextAlign == ContentAlignment.TopLeft || label.TextAlign == ContentAlignment.MiddleLeft || label.TextAlign == ContentAlignment.BottomLeft)
            ro.Style.TextAlignHorz = AlignHorzEnum.Left;
          else if (label.TextAlign == ContentAlignment.TopCenter || label.TextAlign == ContentAlignment.MiddleCenter || label.TextAlign == ContentAlignment.BottomCenter)
            ro.Style.TextAlignHorz = AlignHorzEnum.Center;
          else if (label.TextAlign == ContentAlignment.TopRight || label.TextAlign == ContentAlignment.MiddleRight || label.TextAlign == ContentAlignment.BottomRight)
            ro.Style.TextAlignHorz = AlignHorzEnum.Right;
          doc.Body.Children.Add((RenderObject) ro);
          break;
        case C1FlexDataTree _:
          C1FlexDataTree c1FlexDataTree = (C1FlexDataTree) c;
          doc.Body.Children.Add((RenderObject) c1FlexDataTree.RenderTable(doc));
          break;
        case SplitContainer _:
          SplitContainer splitContainer = (SplitContainer) c;
          this.BuildPrintDocument(doc, (Control) splitContainer.Panel1);
          this.BuildPrintDocument(doc, (Control) splitContainer.Panel2);
          break;
        default:
          if (c.Controls.Count <= 0)
            break;
          for (int index = c.Controls.Count - 1; index >= 0; --index)
            this.BuildPrintDocument(doc, c.Controls[index]);
          break;
      }
    }

    private void cmdExport_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.ShowHelp = false;
      saveFileDialog.CreatePrompt = false;
      saveFileDialog.OverwritePrompt = true;
      saveFileDialog.Title = i_Tree_Eco_v6.Resources.Strings.ExportReport;
      saveFileDialog.Filter = "Adobe Acrobat (*.pdf)|*.pdf|Microsoft Word (*.docx)|*.docx|Rich Text Format (*.rtf)|*.rtf|CSV File (*.csv)|*.csv";
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      C1PrintDocument doc = new C1PrintDocument();
      this.BuildPrintDocument(doc, (Control) this.pnlAll);
      doc.Generate();
      using (FileStream OutFile = new FileStream(saveFileDialog.FileName, FileMode.Create))
      {
        switch (saveFileDialog.FilterIndex)
        {
          case 1:
            doc.Export((Stream) OutFile, (ExportProvider) ExportProviders.PdfExportProvider);
            break;
          case 2:
            doc.Export((Stream) OutFile, (ExportProvider) ExportProviders.DocxExportProvider);
            break;
          case 3:
            doc.Export((Stream) OutFile, (ExportProvider) ExportProviders.RtfExportProvider);
            break;
          case 4:
            this.CreateCSVFromReport(OutFile);
            break;
        }
        OutFile.Close();
      }
    }

    private bool CreateCSVFromReport(FileStream OutFile)
    {
      try
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) OutFile))
        {
          if (this.curReport != null)
          {
            if (this.curReport is FlexGridReport)
            {
              FlexGridReport curReport = (FlexGridReport) this.curReport;
              if (!string.IsNullOrEmpty(curReport.Title))
              {
                streamWriter.WriteLine("\"" + curReport.Title + "\"");
                streamWriter.WriteLine();
              }
              for (int index1 = 0; index1 < curReport.Data.Count; ++index1)
              {
                if (!string.IsNullOrEmpty(curReport.Data[index1].Header))
                  streamWriter.WriteLine("\"" + curReport.Data[index1].Header + " (" + curReport.Data.Count.ToString() + " sections)\"");
                if (curReport.Data[index1].DataSource != null)
                {
                  CurrencyManager currencyManager = (CurrencyManager) this.BindingContext[curReport.Data[index1].DataSource, curReport.Data[index1].DataMember];
                  DataTable dataTable = (DataTable) null;
                  bool flag1 = false;
                  List<int> intList1 = new List<int>();
                  bool flag2 = false;
                  List<int> intList2 = new List<int>();
                  if (currencyManager.List is DataView)
                    dataTable = ((DataView) currencyManager.List).Table;
                  else if (currencyManager.List is BindingSource)
                    dataTable = ((DataView) ((BindingSource) currencyManager.List).List).Table;
                  if (dataTable.ExtendedProperties[(object) "ShowColumns"] is string[] extendedProperty1)
                  {
                    flag1 = true;
                    for (int index2 = 0; index2 < dataTable.Columns.Count; ++index2)
                    {
                      for (int index3 = 0; index3 <= extendedProperty1.GetUpperBound(0); ++index3)
                      {
                        if (dataTable.Columns[index2].ColumnName == extendedProperty1[index3])
                          intList1.Add(index2);
                      }
                    }
                  }
                  if (dataTable.ExtendedProperties[(object) "HideColumns"] is string[] extendedProperty2)
                  {
                    flag2 = true;
                    for (int index4 = 0; index4 < dataTable.Columns.Count; ++index4)
                    {
                      for (int index5 = 0; index5 <= extendedProperty2.GetUpperBound(0); ++index5)
                      {
                        if (dataTable.Columns[index4].ColumnName == extendedProperty2[index5])
                          intList2.Add(index4);
                      }
                    }
                  }
                  for (int index6 = 0; index6 < dataTable.Columns.Count; ++index6)
                  {
                    if (flag1)
                    {
                      if (intList1.Contains(index6))
                        streamWriter.Write((index6 != 0 ? "," : string.Empty) + "\"" + dataTable.Columns[index6].ColumnName + "\"");
                    }
                    else if (flag2)
                    {
                      if (!intList2.Contains(index6))
                        streamWriter.Write((index6 != 0 ? "," : string.Empty) + "\"" + dataTable.Columns[index6].ColumnName + "\"");
                    }
                    else
                      streamWriter.Write((index6 != 0 ? "," : string.Empty) + "\"" + dataTable.Columns[index6].ColumnName + "\"");
                  }
                  streamWriter.Write(Environment.NewLine);
                  for (int index7 = 0; index7 < dataTable.Rows.Count; ++index7)
                  {
                    for (int index8 = 0; index8 < dataTable.Columns.Count; ++index8)
                    {
                      if (flag1)
                      {
                        if (intList1.Contains(index8))
                        {
                          if (dataTable.Columns[index8].IsNumericColumn())
                            streamWriter.Write((index8 != 0 ? "," : string.Empty) + dataTable.Rows[index7][index8].ToString().Replace("\"", "\"\""));
                          else
                            streamWriter.Write((index8 != 0 ? "," : string.Empty) + "\"" + dataTable.Rows[index7][index8].ToString().Replace("\"", "\"\"") + "\"");
                        }
                      }
                      else if (flag2)
                      {
                        if (!intList2.Contains(index8))
                        {
                          if (dataTable.Columns[index8].IsNumericColumn())
                            streamWriter.Write((index8 != 0 ? "," : string.Empty) + dataTable.Rows[index7][index8].ToString().Replace("\"", "\"\""));
                          else
                            streamWriter.Write((index8 != 0 ? "," : string.Empty) + "\"" + dataTable.Rows[index7][index8].ToString().Replace("\"", "\"\"") + "\"");
                        }
                      }
                      else if (dataTable.Columns[index8].IsNumericColumn())
                        streamWriter.Write((index8 != 0 ? "," : string.Empty) + dataTable.Rows[index7][index8].ToString().Replace("\"", "\"\""));
                      else
                        streamWriter.Write((index8 != 0 ? "," : string.Empty) + "\"" + dataTable.Rows[index7][index8].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    streamWriter.Write(Environment.NewLine);
                  }
                  streamWriter.Write(Environment.NewLine);
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RgControl));
      this.scMain = new SplitContainer();
      this.panel3 = new Panel();
      this.cmdPrint = new Button();
      this.cmdExport = new Button();
      this.c1PrintPreviewControl1 = new C1PrintPreviewControl();
      this.pnlAll = new Panel();
      this.pnlAllRpt = new Panel();
      this.controlForLicense = new C1.Win.C1FlexGrid.C1FlexGrid();
      this.mnuReportBindings = new ToolStrip();
      this.lblAllHdr = new Label();
      this.c1PrintDocument1 = new C1PrintDocument();
      this.scMain.BeginInit();
      this.scMain.Panel1.SuspendLayout();
      this.scMain.Panel2.SuspendLayout();
      this.scMain.SuspendLayout();
      this.panel3.SuspendLayout();
      ((ISupportInitialize) this.c1PrintPreviewControl1).BeginInit();
      ((ISupportInitialize) this.c1PrintPreviewControl1.PreviewPane).BeginInit();
      this.c1PrintPreviewControl1.SuspendLayout();
      this.pnlAll.SuspendLayout();
      this.controlForLicense.BeginInit();
      ((ISupportInitialize) this.c1PrintDocument1).BeginInit();
      this.SuspendLayout();
      this.scMain.Dock = DockStyle.Fill;
      this.scMain.FixedPanel = FixedPanel.Panel1;
      this.scMain.IsSplitterFixed = true;
      this.scMain.Location = new Point(0, 0);
      this.scMain.Name = "scMain";
      this.scMain.Panel1.Controls.Add((Control) this.panel3);
      this.scMain.Panel1Collapsed = true;
      this.scMain.Panel2.BackColor = SystemColors.Window;
      this.scMain.Panel2.Controls.Add((Control) this.c1PrintPreviewControl1);
      this.scMain.Panel2.Controls.Add((Control) this.pnlAll);
      this.scMain.Size = new Size(708, 739);
      this.scMain.SplitterDistance = 137;
      this.scMain.TabIndex = 0;
      this.panel3.Controls.Add((Control) this.cmdPrint);
      this.panel3.Controls.Add((Control) this.cmdExport);
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(137, 85);
      this.panel3.TabIndex = 1;
      this.cmdPrint.Font = new Font("Calibri", 10f);
      this.cmdPrint.Location = new Point(24, 48);
      this.cmdPrint.Name = "cmdPrint";
      this.cmdPrint.Size = new Size(75, 23);
      this.cmdPrint.TabIndex = 1;
      this.cmdPrint.Text = "&Print";
      this.cmdPrint.UseVisualStyleBackColor = true;
      this.cmdPrint.Click += new EventHandler(this.cmdPrint_Click);
      this.cmdExport.Font = new Font("Calibri", 10f);
      this.cmdExport.Location = new Point(24, 9);
      this.cmdExport.Name = "cmdExport";
      this.cmdExport.Size = new Size(75, 23);
      this.cmdExport.TabIndex = 0;
      this.cmdExport.Text = "E&xport";
      this.cmdExport.UseVisualStyleBackColor = true;
      this.cmdExport.Click += new EventHandler(this.cmdExport_Click);
      this.c1PrintPreviewControl1.AvailablePreviewActions = C1PreviewActionFlags.Zoom | C1PreviewActionFlags.PageView | C1PreviewActionFlags.Text | C1PreviewActionFlags.FileSave | C1PreviewActionFlags.PageSetup | C1PreviewActionFlags.Print | C1PreviewActionFlags.GoFirst | C1PreviewActionFlags.GoPrev | C1PreviewActionFlags.GoNext | C1PreviewActionFlags.GoLast | C1PreviewActionFlags.GoPage | C1PreviewActionFlags.HistoryNext | C1PreviewActionFlags.HistoryPrev;
      this.c1PrintPreviewControl1.Location = new Point(312, 27);
      this.c1PrintPreviewControl1.Name = "c1PrintPreviewControl1";
      this.c1PrintPreviewControl1.NavigationPanelVisible = false;
      this.c1PrintPreviewControl1.PreviewOutlineView.Dock = DockStyle.Fill;
      this.c1PrintPreviewControl1.PreviewOutlineView.Location = new Point(0, 0);
      this.c1PrintPreviewControl1.PreviewOutlineView.Name = "OutlineView";
      this.c1PrintPreviewControl1.PreviewOutlineView.Size = new Size(165, 427);
      this.c1PrintPreviewControl1.PreviewOutlineView.TabIndex = 0;
      this.c1PrintPreviewControl1.PreviewPane.ExportOptions.Content = new ExporterOptions[18]
      {
        new ExporterOptions("C1dExportProvider", "C1.C1Preview.Export.C1dOptionsForm", false, true, false),
        new ExporterOptions("EmfExportProvider", "C1.C1Preview.Export.EmfOptionsForm", false, true, true),
        new ExporterOptions("TiffExportProvider", "C1.C1Preview.Export.ImagesOptionsForm", false, true, true),
        new ExporterOptions("PngExportProvider", "C1.C1Preview.Export.ImagesOptionsForm", false, true, true),
        new ExporterOptions("JpegExportProvider", "C1.C1Preview.Export.ImagesOptionsForm", false, true, true),
        new ExporterOptions("GifExportProvider", "C1.C1Preview.Export.ImagesOptionsForm", false, true, true),
        new ExporterOptions("BmpExportProvider", "C1.C1Preview.Export.ImagesOptionsForm", false, true, true),
        new ExporterOptions("HtmlExportProvider", "C1.C1Preview.Export.HtmlOptionsForm", false, true, true),
        new ExporterOptions("XpsExportProvider", "C1.C1Preview.Export.DefaultExportOptionsForm", false, true, true),
        new ExporterOptions("C1dxExportProvider", "C1.C1Preview.Export.C1dOptionsForm", false, true, false),
        new ExporterOptions("C1dbExportProvider", "C1.C1Preview.Export.C1dOptionsForm", false, true, false),
        new ExporterOptions("C1mdxExportProvider", "C1.C1Preview.Export.C1mdxOptionsForm", false, true, false),
        new ExporterOptions("ReportHTMLExportProvider", "C1.C1Preview.Export.ReportHtmlOptionsForm", false, true, true),
        new ExporterOptions("ReportRTFExportProvider", "C1.C1Preview.Export.ReportRtfOptionsForm", false, true, true),
        new ExporterOptions("ReportTextExportProvider", "C1.C1Preview.Export.ReportTextOptionsForm", false, true, true),
        new ExporterOptions("ReportMetafileExportProvider", "C1.C1Preview.Export.DefaultExportOptionsForm", false, true, true),
        new ExporterOptions("ReportExcelExportProvider", "C1.C1Preview.Export.DefaultExportOptionsForm", false, true, true),
        new ExporterOptions("ReportOpenXmlExportProvider", "C1.C1Preview.Export.DefaultExportOptionsForm", false, true, true)
      };
      this.c1PrintPreviewControl1.PreviewPane.IntegrateExternalTools = true;
      this.c1PrintPreviewControl1.PreviewPane.TabIndex = 0;
      this.c1PrintPreviewControl1.PreviewTextSearchPanel.Dock = DockStyle.Right;
      this.c1PrintPreviewControl1.PreviewTextSearchPanel.Location = new Point(530, 0);
      this.c1PrintPreviewControl1.PreviewTextSearchPanel.MinimumSize = new Size(200, 240);
      this.c1PrintPreviewControl1.PreviewTextSearchPanel.Name = "PreviewTextSearchPanel";
      this.c1PrintPreviewControl1.PreviewTextSearchPanel.Size = new Size(200, 453);
      this.c1PrintPreviewControl1.PreviewTextSearchPanel.TabIndex = 0;
      this.c1PrintPreviewControl1.PreviewTextSearchPanel.Visible = false;
      this.c1PrintPreviewControl1.PreviewThumbnailView.Dock = DockStyle.Fill;
      this.c1PrintPreviewControl1.PreviewThumbnailView.Location = new Point(0, 0);
      this.c1PrintPreviewControl1.PreviewThumbnailView.Name = "ThumbnailView";
      this.c1PrintPreviewControl1.PreviewThumbnailView.Size = new Size(165, 427);
      this.c1PrintPreviewControl1.PreviewThumbnailView.TabIndex = 0;
      this.c1PrintPreviewControl1.PreviewThumbnailView.UseImageAsThumbnail = false;
      this.c1PrintPreviewControl1.Size = new Size(564, 363);
      this.c1PrintPreviewControl1.TabIndex = 9;
      this.c1PrintPreviewControl1.Text = "c1PrintPreviewControl1";
      this.c1PrintPreviewControl1.ToolBars.File.Open.Image = (Image) componentResourceManager.GetObject("c1PrintPreviewControl1.ToolBars.File.Open.Image");
      this.c1PrintPreviewControl1.ToolBars.File.Open.ImageTransparentColor = Color.Magenta;
      this.c1PrintPreviewControl1.ToolBars.File.Open.Name = "btnFileOpen";
      this.c1PrintPreviewControl1.ToolBars.File.Open.Size = new Size(32, 22);
      this.c1PrintPreviewControl1.ToolBars.File.Open.Tag = (object) "C1PreviewActionEnum.FileOpen";
      this.c1PrintPreviewControl1.ToolBars.File.Open.ToolTipText = "Open File";
      this.c1PrintPreviewControl1.ToolBars.File.Open.Visible = false;
      this.c1PrintPreviewControl1.ToolBars.File.PageSetup.Image = (Image) componentResourceManager.GetObject("c1PrintPreviewControl1.ToolBars.File.PageSetup.Image");
      this.c1PrintPreviewControl1.ToolBars.File.PageSetup.ImageTransparentColor = Color.Magenta;
      this.c1PrintPreviewControl1.ToolBars.File.PageSetup.Name = "btnPageSetup";
      this.c1PrintPreviewControl1.ToolBars.File.PageSetup.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.File.PageSetup.Tag = (object) "C1PreviewActionEnum.PageSetup";
      this.c1PrintPreviewControl1.ToolBars.File.PageSetup.ToolTipText = "Page Setup";
      this.c1PrintPreviewControl1.ToolBars.File.Print.Image = (Image) componentResourceManager.GetObject("c1PrintPreviewControl1.ToolBars.File.Print.Image");
      this.c1PrintPreviewControl1.ToolBars.File.Print.ImageTransparentColor = Color.Magenta;
      this.c1PrintPreviewControl1.ToolBars.File.Print.Name = "btnPrint";
      this.c1PrintPreviewControl1.ToolBars.File.Print.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.File.Print.Tag = (object) "C1PreviewActionEnum.Print";
      this.c1PrintPreviewControl1.ToolBars.File.Print.ToolTipText = "Print";
      this.c1PrintPreviewControl1.ToolBars.File.Reflow.Image = (Image) componentResourceManager.GetObject("c1PrintPreviewControl1.ToolBars.File.Reflow.Image");
      this.c1PrintPreviewControl1.ToolBars.File.Reflow.ImageTransparentColor = Color.Magenta;
      this.c1PrintPreviewControl1.ToolBars.File.Reflow.Name = "btnReflow";
      this.c1PrintPreviewControl1.ToolBars.File.Reflow.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.File.Reflow.Tag = (object) "C1PreviewActionEnum.Reflow";
      this.c1PrintPreviewControl1.ToolBars.File.Reflow.ToolTipText = "Reflow";
      this.c1PrintPreviewControl1.ToolBars.File.Reflow.Visible = false;
      this.c1PrintPreviewControl1.ToolBars.File.Save.Image = (Image) componentResourceManager.GetObject("c1PrintPreviewControl1.ToolBars.File.Save.Image");
      this.c1PrintPreviewControl1.ToolBars.File.Save.ImageTransparentColor = Color.Magenta;
      this.c1PrintPreviewControl1.ToolBars.File.Save.Name = "btnFileSave";
      this.c1PrintPreviewControl1.ToolBars.File.Save.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.File.Save.Tag = (object) "C1PreviewActionEnum.FileSave";
      this.c1PrintPreviewControl1.ToolBars.File.Save.ToolTipText = "Save File";
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoFirst.Image = (Image) componentResourceManager.GetObject("c1PrintPreviewControl1.ToolBars.Navigation.GoFirst.Image");
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoFirst.ImageTransparentColor = Color.Magenta;
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoFirst.Name = "btnGoFirst";
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoFirst.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoFirst.Tag = (object) "C1PreviewActionEnum.GoFirst";
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoFirst.ToolTipText = "Go To First Page";
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoLast.Name = "btnGoLast";
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoLast.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoNext.Name = "btnGoNext";
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoNext.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoPrev.Image = (Image) componentResourceManager.GetObject("c1PrintPreviewControl1.ToolBars.Navigation.GoPrev.Image");
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoPrev.ImageTransparentColor = Color.Magenta;
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoPrev.Name = "btnGoPrev";
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoPrev.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoPrev.Tag = (object) "C1PreviewActionEnum.GoPrev";
      this.c1PrintPreviewControl1.ToolBars.Navigation.GoPrev.ToolTipText = "Go To Previous Page";
      this.c1PrintPreviewControl1.ToolBars.Navigation.HistoryNext.Name = "btnHistoryNext";
      this.c1PrintPreviewControl1.ToolBars.Navigation.HistoryNext.Size = new Size(16, 22);
      this.c1PrintPreviewControl1.ToolBars.Navigation.HistoryPrev.Name = "btnHistoryPrev";
      this.c1PrintPreviewControl1.ToolBars.Navigation.HistoryPrev.Size = new Size(16, 22);
      this.c1PrintPreviewControl1.ToolBars.Navigation.LblOfPages.Name = "lblOfPages";
      this.c1PrintPreviewControl1.ToolBars.Navigation.LblOfPages.Size = new Size(27, 22);
      this.c1PrintPreviewControl1.ToolBars.Navigation.LblOfPages.Tag = (object) "C1PreviewActionEnum.GoPageCount";
      this.c1PrintPreviewControl1.ToolBars.Navigation.LblOfPages.Text = "of 0";
      this.c1PrintPreviewControl1.ToolBars.Navigation.LblPage.Name = "lblPage";
      this.c1PrintPreviewControl1.ToolBars.Navigation.LblPage.Size = new Size(33, 22);
      this.c1PrintPreviewControl1.ToolBars.Navigation.LblPage.Tag = (object) "C1PreviewActionEnum.GoPageLabel";
      this.c1PrintPreviewControl1.ToolBars.Navigation.LblPage.Text = "Page";
      this.c1PrintPreviewControl1.ToolBars.Navigation.ToolTipGoToLastPage = "";
      this.c1PrintPreviewControl1.ToolBars.Navigation.ToolTipGoToNextPage = "";
      this.c1PrintPreviewControl1.ToolBars.Navigation.ToolTipHistoryNext = "";
      this.c1PrintPreviewControl1.ToolBars.Navigation.ToolTipHistoryPrev = "";
      this.c1PrintPreviewControl1.ToolBars.Navigation.ToolTipPageNo = (string) null;
      this.c1PrintPreviewControl1.ToolBars.Page.Continuous.Name = "btnPageContinuous";
      this.c1PrintPreviewControl1.ToolBars.Page.Continuous.Size = new Size(23, 4);
      this.c1PrintPreviewControl1.ToolBars.Page.Facing.Name = "btnPageFacing";
      this.c1PrintPreviewControl1.ToolBars.Page.Facing.Size = new Size(23, 4);
      this.c1PrintPreviewControl1.ToolBars.Page.FacingContinuous.Name = "btnPageFacingContinuous";
      this.c1PrintPreviewControl1.ToolBars.Page.FacingContinuous.Size = new Size(23, 4);
      this.c1PrintPreviewControl1.ToolBars.Page.Single.Image = (Image) componentResourceManager.GetObject("c1PrintPreviewControl1.ToolBars.Page.Single.Image");
      this.c1PrintPreviewControl1.ToolBars.Page.Single.ImageTransparentColor = Color.Magenta;
      this.c1PrintPreviewControl1.ToolBars.Page.Single.Name = "btnPageSingle";
      this.c1PrintPreviewControl1.ToolBars.Page.Single.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.Page.Single.Tag = (object) "C1PreviewActionEnum.PageSingle";
      this.c1PrintPreviewControl1.ToolBars.Page.Single.ToolTipText = "Single Page View";
      this.c1PrintPreviewControl1.ToolBars.Page.ToolTipViewContinuous = "";
      this.c1PrintPreviewControl1.ToolBars.Page.ToolTipViewFacing = "";
      this.c1PrintPreviewControl1.ToolBars.Page.ToolTipViewFacingContinuous = "";
      this.c1PrintPreviewControl1.ToolBars.Text.Find.Name = "btnFind";
      this.c1PrintPreviewControl1.ToolBars.Text.Find.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.Text.Hand.Checked = true;
      this.c1PrintPreviewControl1.ToolBars.Text.Hand.CheckState = CheckState.Checked;
      this.c1PrintPreviewControl1.ToolBars.Text.Hand.Image = (Image) componentResourceManager.GetObject("c1PrintPreviewControl1.ToolBars.Text.Hand.Image");
      this.c1PrintPreviewControl1.ToolBars.Text.Hand.ImageTransparentColor = Color.Magenta;
      this.c1PrintPreviewControl1.ToolBars.Text.Hand.Name = "btnHandTool";
      this.c1PrintPreviewControl1.ToolBars.Text.Hand.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.Text.Hand.Tag = (object) "C1PreviewActionEnum.HandTool";
      this.c1PrintPreviewControl1.ToolBars.Text.Hand.ToolTipText = "Hand Tool";
      this.c1PrintPreviewControl1.ToolBars.Text.SelectText.Name = "btnSelectTextTool";
      this.c1PrintPreviewControl1.ToolBars.Text.SelectText.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.Text.ToolTipFind = "";
      this.c1PrintPreviewControl1.ToolBars.Text.ToolTipToolTextSelect = "";
      this.c1PrintPreviewControl1.ToolBars.Zoom.DropZoomFactor.Name = "dropZoomFactor";
      this.c1PrintPreviewControl1.ToolBars.Zoom.DropZoomFactor.Size = new Size(13, 22);
      this.c1PrintPreviewControl1.ToolBars.Zoom.DropZoomFactor.Tag = (object) "C1PreviewActionEnum.ZoomFactor";
      this.c1PrintPreviewControl1.ToolBars.Zoom.ToolTipToolZoomIn = (string) null;
      this.c1PrintPreviewControl1.ToolBars.Zoom.ToolTipToolZoomOut = (string) null;
      this.c1PrintPreviewControl1.ToolBars.Zoom.ToolTipZoomFactor = (string) null;
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomIn.Image = (Image) componentResourceManager.GetObject("c1PrintPreviewControl1.ToolBars.Zoom.ZoomIn.Image");
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomIn.ImageTransparentColor = Color.Magenta;
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomIn.Name = "btnZoomIn";
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomIn.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomIn.Tag = (object) "C1PreviewActionEnum.ZoomIn";
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomIn.ToolTipText = "Zoom In";
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomInTool.Name = "itemZoomInTool";
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomInTool.Size = new Size(67, 22);
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomOut.Image = (Image) componentResourceManager.GetObject("c1PrintPreviewControl1.ToolBars.Zoom.ZoomOut.Image");
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomOut.ImageTransparentColor = Color.Magenta;
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomOut.Name = "btnZoomOut";
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomOut.Size = new Size(23, 22);
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomOut.Tag = (object) "C1PreviewActionEnum.ZoomOut";
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomOut.ToolTipText = "Zoom Out";
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomOutTool.Name = "itemZoomOutTool";
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomOutTool.Size = new Size(67, 22);
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomTool.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomInTool,
        (ToolStripItem) this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomOutTool
      });
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomTool.Image = (Image) componentResourceManager.GetObject("c1PrintPreviewControl1.ToolBars.Zoom.ZoomTool.Image");
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomTool.ImageTransparentColor = Color.Magenta;
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomTool.Name = "btnZoomTool";
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomTool.Size = new Size(32, 22);
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomTool.Tag = (object) "C1PreviewActionEnum.ZoomInTool";
      this.c1PrintPreviewControl1.ToolBars.Zoom.ZoomTool.ToolTipText = "Zoom In Tool";
      this.c1PrintPreviewControl1.Visible = false;
      this.pnlAll.Controls.Add((Control) this.pnlAllRpt);
      this.pnlAll.Controls.Add((Control) this.controlForLicense);
      this.pnlAll.Controls.Add((Control) this.mnuReportBindings);
      this.pnlAll.Controls.Add((Control) this.lblAllHdr);
      this.pnlAll.Location = new Point(25, 54);
      this.pnlAll.Name = "pnlAll";
      this.pnlAll.Size = new Size(482, 512);
      this.pnlAll.TabIndex = 8;
      this.pnlAll.Visible = false;
      this.pnlAllRpt.BackColor = SystemColors.Window;
      this.pnlAllRpt.Dock = DockStyle.Fill;
      this.pnlAllRpt.Location = new Point(0, 60);
      this.pnlAllRpt.Name = "pnlAllRpt";
      this.pnlAllRpt.Size = new Size(482, 452);
      this.pnlAllRpt.TabIndex = 0;
      this.controlForLicense.ColumnInfo = "10,1,0,0,0,85,Columns:";
      this.controlForLicense.Location = new Point(32, 239);
      this.controlForLicense.Name = "controlForLicense";
      this.controlForLicense.Rows.DefaultSize = 17;
      this.controlForLicense.Size = new Size(81, 66);
      this.controlForLicense.TabIndex = 8;
      this.controlForLicense.Visible = false;
      this.mnuReportBindings.AutoSize = false;
      this.mnuReportBindings.BackColor = SystemColors.Window;
      this.mnuReportBindings.GripStyle = ToolStripGripStyle.Hidden;
      this.mnuReportBindings.Location = new Point(0, 25);
      this.mnuReportBindings.Name = "mnuReportBindings";
      this.mnuReportBindings.Size = new Size(482, 35);
      this.mnuReportBindings.TabIndex = 10;
      this.mnuReportBindings.Text = "toolStrip1";
      this.lblAllHdr.Dock = DockStyle.Top;
      this.lblAllHdr.Font = new Font("Calibri", 15.75f);
      this.lblAllHdr.Location = new Point(0, 0);
      this.lblAllHdr.Name = "lblAllHdr";
      this.lblAllHdr.Size = new Size(482, 25);
      this.lblAllHdr.TabIndex = 0;
      this.lblAllHdr.Text = "HFG";
      this.lblAllHdr.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleMode = AutoScaleMode.None;
      this.AutoSize = true;
      this.Controls.Add((Control) this.scMain);
      this.Name = nameof (RgControl);
      this.Size = new Size(708, 739);
      this.scMain.Panel1.ResumeLayout(false);
      this.scMain.Panel2.ResumeLayout(false);
      this.scMain.EndInit();
      this.scMain.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      ((ISupportInitialize) this.c1PrintPreviewControl1.PreviewPane).EndInit();
      ((ISupportInitialize) this.c1PrintPreviewControl1).EndInit();
      this.c1PrintPreviewControl1.ResumeLayout(false);
      this.c1PrintPreviewControl1.PerformLayout();
      this.pnlAll.ResumeLayout(false);
      this.controlForLicense.EndInit();
      ((ISupportInitialize) this.c1PrintDocument1).EndInit();
      this.ResumeLayout(false);
    }
  }
}
