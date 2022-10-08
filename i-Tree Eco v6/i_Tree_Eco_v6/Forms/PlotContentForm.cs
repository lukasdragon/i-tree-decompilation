// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.PlotContentForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Domain.v6;
using Eco.Util.Views;
using LocationSpecies.Domain;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class PlotContentForm : DockContent
  {
    private Plot m_plot;
    private ISession m_session;
    private ProgramSession m_ps;
    private IDictionary<short, string> m_dPcts;
    private IDictionary<string, ReferenceObjectType> m_dRefObjs;
    private bool m_tpPlotInit;
    private bool m_tpTreesInit;
    private bool m_tpShrubsInit;
    private IContainer components;
    private TabControl tcPlotContent;
    private TabPage tpPlot;
    private TabPage tpTrees;
    private TabPage tpShrubs;
    private RichTextLabel richTextLabel2;
    private DataGridView dgShrubs;
    private RichTextLabel richTextLabel3;
    private TableLayoutPanel tableLayoutPanel2;
    private Panel panel1;
    private DataGridView dgLandUses;
    private Label lblLandUse;
    private Panel panel2;
    private DataGridView dgGroundCovers;
    private Label lblGroundCover;
    private Panel panel3;
    private Label lblRefObjects;
    private DataGridView dgRefObjects;
    private DBTableLayoutPanel tableLayoutPanel1;
    private RichTextLabel richTextLabel1;
    private NumericTextBox txtId;
    private NumericTextBox txtSize;
    private NullableDateTimePicker dtDate;
    private TextBox txtCrew;
    private NumericTextBox txtLatitude;
    private NumericTextBox txtLongitude;
    private TextBox txtAddress;
    private ComboBox cboStrata;
    private ComboBox cboPctTree;
    private ComboBox cboPctShrub;
    private ComboBox cboPctPlantable;
    private TextBox txtPhotoId;
    private TextBox txtContact;
    private TextBox txtNotes;
    private DataGridView dgTrees;
    private BackgroundWorker bgWorker;
    private NumericTextBox ntbPctMeasured;
    private DataGridViewComboBoxColumn gcGroundCover;
    private DataGridViewTextBoxColumn gcPctPlot;
    private DataGridViewComboBoxColumn luLandUse;
    private DataGridViewTextBoxColumn luPctPlot;
    private DataGridViewComboBoxColumn roType;
    private DataGridViewTextBoxColumn roDistance;
    private DataGridViewTextBoxColumn roDirection;
    private DataGridViewTextBoxColumn roDBH;
    private DataGridViewTextBoxColumn roNotes;
    private DataGridViewTextBoxColumn shId;
    private DataGridViewComboBoxColumn shSpecies;
    private DataGridViewTextBoxColumn shHeight;
    private DataGridViewTextBoxColumn shPctShrubArea;
    private DataGridViewTextBoxColumn shPctMissing;
    private DataGridViewTextBoxColumn shComments;

    public PlotContentForm()
    {
      this.InitializeComponent();
      this.m_dPcts = (IDictionary<short, string>) new SortedDictionary<short, string>()
      {
        {
          (short) -1,
          i_Tree_Eco_v6.Resources.Strings.EntryNotEntered
        },
        {
          (short) 0,
          "0%"
        },
        {
          (short) 3,
          "1% - 5%"
        },
        {
          (short) 8,
          "5% - 10%"
        },
        {
          (short) 13,
          "10% - 15%"
        },
        {
          (short) 18,
          "15% - 20%"
        },
        {
          (short) 23,
          "20% - 25%"
        },
        {
          (short) 28,
          "25% - 30%"
        },
        {
          (short) 33,
          "30% - 35%"
        },
        {
          (short) 38,
          "35% - 40%"
        },
        {
          (short) 43,
          "40% - 45%"
        },
        {
          (short) 48,
          "45% - 50%"
        },
        {
          (short) 53,
          "50% - 55%"
        },
        {
          (short) 58,
          "55% - 60%"
        },
        {
          (short) 63,
          "60% - 65%"
        },
        {
          (short) 68,
          "65% - 70%"
        },
        {
          (short) 73,
          "70% - 75%"
        },
        {
          (short) 78,
          "75% - 80%"
        },
        {
          (short) 83,
          "80% - 85%"
        },
        {
          (short) 88,
          "85% - 90%"
        },
        {
          (short) 93,
          "90% - 95%"
        },
        {
          (short) 98,
          "95% - 99%"
        },
        {
          (short) 100,
          "100%"
        }
      };
      this.m_dRefObjs = (IDictionary<string, ReferenceObjectType>) new SortedDictionary<string, ReferenceObjectType>();
      foreach (ReferenceObjectType referenceObjectType in Enum.GetValues(typeof (ReferenceObjectType)))
        this.m_dRefObjs.Add(EnumHelper.GetDescription<ReferenceObjectType>(referenceObjectType), referenceObjectType);
      this.roType.DisplayMember = "Key";
      this.roType.ValueMember = "Value";
      this.roType.ValueType = typeof (ReferenceObjectType);
      this.roType.DataPropertyName = "Object";
      this.roType.DataSource = (object) new BindingSource((object) this.m_dRefObjs, (string) null);
      this.m_ps = Program.Session;
      this.shSpecies.BindTo<Species>((Expression<Func<Species, object>>) (sp => sp.CommonName), (Expression<Func<Species, object>>) (sp => sp.Code), (object) this.m_ps.Species.Values.ToList<SpeciesView>());
      this.m_ps.InputSessionChanged += new EventHandler(this.ps_InputSessionChanged);
    }

    private void ps_InputSessionChanged(object sender, EventArgs e)
    {
      if (this.m_session != null)
      {
        this.m_session.Close();
        this.m_session.Dispose();
      }
      this.m_session = this.m_ps.InputSession.CreateSession();
      this.m_ps.InputSession.YearChanged += new EventHandler(this.InputSession_YearChanged);
    }

    private void InputSession_YearChanged(object sender, EventArgs e)
    {
      using (this.m_session.BeginTransaction())
      {
        Year year = this.m_session.Get<Year>((object) this.m_ps.InputSession.YearKey);
        this.cboStrata.DataSource = (object) null;
        this.cboStrata.DisplayMember = "Description";
        this.cboStrata.DataSource = (object) year.Strata.ToList<Strata>();
        this.cboPctPlantable.DataSource = (object) null;
        this.cboPctPlantable.DisplayMember = "Value";
        this.cboPctPlantable.ValueMember = "Key";
        this.cboPctPlantable.DataSource = (object) new BindingSource((object) this.m_dPcts, (string) null);
        this.cboPctShrub.DataSource = (object) null;
        this.cboPctShrub.DisplayMember = "Value";
        this.cboPctShrub.ValueMember = "Key";
        this.cboPctShrub.DataSource = (object) new BindingSource((object) this.m_dPcts, (string) null);
        this.cboPctTree.DataSource = (object) null;
        this.cboPctTree.DisplayMember = "Value";
        this.cboPctTree.ValueMember = "Key";
        this.cboPctTree.DataSource = (object) new BindingSource((object) this.m_dPcts, (string) null);
        this.luLandUse.DataSource = (object) null;
        this.luLandUse.ValueType = typeof (LandUse);
        this.luLandUse.DataPropertyName = "FieldLandUse";
        this.luLandUse.DisplayMember = "Description";
        this.luLandUse.ValueMember = "Self";
        this.luLandUse.DataSource = (object) year.LandUses.OrderBy<LandUse, string>((Func<LandUse, string>) (flu => flu.Description)).ToList<LandUse>();
        this.luPctPlot.ValueType = typeof (short);
        this.luPctPlot.DataPropertyName = "PercentOfPlot";
        this.gcGroundCover.DataSource = (object) null;
        this.gcGroundCover.ValueType = typeof (GroundCover);
        this.gcGroundCover.DataPropertyName = "GroundCover";
        this.gcGroundCover.DisplayMember = "Description";
        this.gcGroundCover.ValueMember = "Self";
        this.gcGroundCover.DataSource = (object) year.GroundCovers.OrderBy<GroundCover, string>((Func<GroundCover, string>) (gc => gc.Description)).ToList<GroundCover>();
        this.gcPctPlot.ValueType = typeof (int);
        this.gcPctPlot.DataPropertyName = "PercentCovered";
        this.roDistance.ValueType = typeof (float);
        this.roDistance.DataPropertyName = "Distance";
        this.roDirection.ValueType = typeof (int);
        this.roDirection.DataPropertyName = "Direction";
        this.roDBH.ValueType = typeof (float);
        this.roDBH.DataPropertyName = "DBH";
        this.roNotes.DataPropertyName = "Notes";
        this.shId.ValueType = typeof (int);
        this.shId.DataPropertyName = "Id";
        this.shSpecies.ValueType = typeof (string);
        this.shSpecies.DataPropertyName = "Species";
        this.shHeight.ValueType = typeof (float);
        this.shHeight.DataPropertyName = "Height";
        this.shPctShrubArea.ValueType = typeof (int);
        this.shPctShrubArea.DataPropertyName = "PercentOfShrubArea";
        this.shPctMissing.ValueType = typeof (int);
        this.shPctMissing.DataPropertyName = "PercentMissing";
        this.shComments.DataPropertyName = "Comments";
      }
    }

    public void Refresh(Guid plotKey)
    {
      if (this.bgWorker.IsBusy)
        return;
      this.bgWorker.RunWorkerAsync((object) plotKey);
    }

    private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
    {
    }

    private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      Guid? nullable = e.Argument as Guid?;
      if (!nullable.HasValue)
        return;
      lock (this.m_session)
      {
        using (this.m_session.BeginTransaction())
        {
          Plot plot = this.m_session.Get<Plot>((object) nullable.Value);
          NHibernateUtil.Initialize((object) plot.PlotGroundCovers);
          e.Result = (object) plot;
        }
      }
    }

    private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.m_plot = e.Result as Plot;
      this.m_tpPlotInit = false;
      this.m_tpShrubsInit = false;
      this.m_tpTreesInit = false;
      this.OnPlotContentTabChange();
    }

    private void tcPlotContent_SelectedIndexChanged(object sender, EventArgs e) => this.OnPlotContentTabChange();

    private void OnPlotContentTabChange()
    {
      TabPage selectedTab = this.tcPlotContent.SelectedTab;
      if (selectedTab.Equals((object) this.tpPlot))
        this.InitPlotTab();
      else if (selectedTab.Equals((object) this.tpTrees))
      {
        this.InitTreesTab();
      }
      else
      {
        if (!selectedTab.Equals((object) this.tpShrubs))
          return;
        this.InitShrubsTab();
      }
    }

    private void InitPlotTab()
    {
      if (this.m_tpPlotInit)
        return;
      lock (this.m_session)
      {
        using (this.m_session.BeginTransaction())
        {
          this.txtId.DataBindings.Clear();
          this.txtId.DataBindings.Add("Text", (object) this.m_plot, "Id");
          this.txtSize.DataBindings.Clear();
          this.txtSize.DataBindings.Add("Text", (object) this.m_plot, "Size");
          this.ntbPctMeasured.DataBindings.Clear();
          this.ntbPctMeasured.DataBindings.Add("Text", (object) this.m_plot, "PercentMeasured");
          this.dtDate.DataBindings.Clear();
          this.dtDate.DataBindings.Add("Value", (object) this.m_plot, "Date", true);
          this.txtCrew.DataBindings.Clear();
          this.txtCrew.DataBindings.Add("Text", (object) this.m_plot, "Crew");
          this.txtLatitude.DataBindings.Clear();
          this.txtLatitude.DataBindings.Add("Text", (object) this.m_plot, "Latitude");
          this.txtLongitude.DataBindings.Clear();
          this.txtLongitude.DataBindings.Add("Text", (object) this.m_plot, "Longitude");
          this.txtAddress.DataBindings.Clear();
          this.txtAddress.DataBindings.Add("Text", (object) this.m_plot, "Address");
          this.cboStrata.DataBindings.Clear();
          this.cboStrata.DataBindings.Add("SelectedItem", (object) this.m_plot, "Strata", true);
          this.cboPctTree.DataBindings.Clear();
          this.cboPctTree.DataBindings.Add("SelectedValue", (object) this.m_plot, "PercentTreeCover");
          this.cboPctShrub.DataBindings.Clear();
          this.cboPctShrub.DataBindings.Add("SelectedValue", (object) this.m_plot, "PercentShrubCover");
          this.cboPctPlantable.DataBindings.Clear();
          this.cboPctPlantable.DataBindings.Add("SelectedValue", (object) this.m_plot, "PercentPlantable");
          this.txtPhotoId.DataBindings.Clear();
          this.txtPhotoId.DataBindings.Add("Text", (object) this.m_plot, "Photo");
          this.txtContact.DataBindings.Clear();
          this.txtContact.DataBindings.Add("Text", (object) this.m_plot, "ContactInfo");
          this.txtNotes.DataBindings.Clear();
          this.txtNotes.DataBindings.Add("Text", (object) this.m_plot, "Comments");
          this.dgLandUses.AutoGenerateColumns = false;
          this.dgLandUses.DataSource = (object) new BindingList<PlotLandUse>((IList<PlotLandUse>) this.m_plot.PlotLandUses.ToList<PlotLandUse>());
          this.dgGroundCovers.AutoGenerateColumns = false;
          this.dgGroundCovers.DataSource = (object) new BindingList<PlotGroundCover>((IList<PlotGroundCover>) this.m_plot.PlotGroundCovers.ToList<PlotGroundCover>());
          this.dgRefObjects.AutoGenerateColumns = false;
          this.dgRefObjects.DataSource = (object) new BindingList<ReferenceObject>((IList<ReferenceObject>) this.m_plot.ReferenceObjects.ToList<ReferenceObject>());
        }
      }
      this.m_tpPlotInit = true;
    }

    private void InitTreesTab()
    {
      if (this.m_tpTreesInit)
        return;
      this.m_tpTreesInit = true;
    }

    private void InitShrubsTab()
    {
      if (this.m_tpShrubsInit)
        return;
      lock (this.m_session)
      {
        using (this.m_session.BeginTransaction())
        {
          this.dgShrubs.AutoGenerateColumns = false;
          this.dgShrubs.DataSource = (object) new BindingList<Shrub>((IList<Shrub>) this.m_plot.Shrubs.OrderBy<Shrub, int>((Func<Shrub, int>) (s => s.Id)).ToList<Shrub>());
        }
      }
      this.m_tpShrubsInit = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PlotContentForm));
      DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
      this.tcPlotContent = new TabControl();
      this.tpPlot = new TabPage();
      this.tableLayoutPanel2 = new TableLayoutPanel();
      this.panel1 = new Panel();
      this.dgLandUses = new DataGridView();
      this.luLandUse = new DataGridViewComboBoxColumn();
      this.luPctPlot = new DataGridViewTextBoxColumn();
      this.lblLandUse = new Label();
      this.panel2 = new Panel();
      this.dgGroundCovers = new DataGridView();
      this.gcGroundCover = new DataGridViewComboBoxColumn();
      this.gcPctPlot = new DataGridViewTextBoxColumn();
      this.lblGroundCover = new Label();
      this.panel3 = new Panel();
      this.lblRefObjects = new Label();
      this.dgRefObjects = new DataGridView();
      this.roType = new DataGridViewComboBoxColumn();
      this.roDistance = new DataGridViewTextBoxColumn();
      this.roDirection = new DataGridViewTextBoxColumn();
      this.roDBH = new DataGridViewTextBoxColumn();
      this.roNotes = new DataGridViewTextBoxColumn();
      this.tableLayoutPanel1 = new DBTableLayoutPanel(this.components);
      this.ntbPctMeasured = new NumericTextBox();
      this.txtSize = new NumericTextBox();
      this.dtDate = new NullableDateTimePicker();
      this.txtCrew = new TextBox();
      this.txtId = new NumericTextBox();
      this.txtLatitude = new NumericTextBox();
      this.txtLongitude = new NumericTextBox();
      this.txtAddress = new TextBox();
      this.cboStrata = new ComboBox();
      this.cboPctTree = new ComboBox();
      this.cboPctShrub = new ComboBox();
      this.cboPctPlantable = new ComboBox();
      this.txtPhotoId = new TextBox();
      this.txtContact = new TextBox();
      this.txtNotes = new TextBox();
      this.richTextLabel1 = new RichTextLabel();
      this.tpTrees = new TabPage();
      this.dgTrees = new DataGridView();
      this.richTextLabel2 = new RichTextLabel();
      this.tpShrubs = new TabPage();
      this.dgShrubs = new DataGridView();
      this.shId = new DataGridViewTextBoxColumn();
      this.shSpecies = new DataGridViewComboBoxColumn();
      this.shHeight = new DataGridViewTextBoxColumn();
      this.shPctShrubArea = new DataGridViewTextBoxColumn();
      this.shPctMissing = new DataGridViewTextBoxColumn();
      this.shComments = new DataGridViewTextBoxColumn();
      this.richTextLabel3 = new RichTextLabel();
      this.bgWorker = new BackgroundWorker();
      Label label1 = new Label();
      Label label2 = new Label();
      Label label3 = new Label();
      Label label4 = new Label();
      Label label5 = new Label();
      Label label6 = new Label();
      Label label7 = new Label();
      Label label8 = new Label();
      Label label9 = new Label();
      Label label10 = new Label();
      Label label11 = new Label();
      Label label12 = new Label();
      Label label13 = new Label();
      Label label14 = new Label();
      Label label15 = new Label();
      Label label16 = new Label();
      this.tcPlotContent.SuspendLayout();
      this.tpPlot.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.dgLandUses).BeginInit();
      this.panel2.SuspendLayout();
      ((ISupportInitialize) this.dgGroundCovers).BeginInit();
      this.panel3.SuspendLayout();
      ((ISupportInitialize) this.dgRefObjects).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.tpTrees.SuspendLayout();
      ((ISupportInitialize) this.dgTrees).BeginInit();
      this.tpShrubs.SuspendLayout();
      ((ISupportInitialize) this.dgShrubs).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) label1, "lblPlot");
      label1.Name = "lblPlot";
      componentResourceManager.ApplyResources((object) label2, "lblId");
      label2.Name = "lblId";
      componentResourceManager.ApplyResources((object) label3, "lblSize");
      label3.Name = "lblSize";
      componentResourceManager.ApplyResources((object) label4, "lblPctMeasured");
      label4.Name = "lblPctMeasured";
      componentResourceManager.ApplyResources((object) label5, "lblDate");
      label5.Name = "lblDate";
      componentResourceManager.ApplyResources((object) label6, "lblCrew");
      label6.Name = "lblCrew";
      componentResourceManager.ApplyResources((object) label7, "lblLatitude");
      label7.Name = "lblLatitude";
      componentResourceManager.ApplyResources((object) label8, "lblLongitude");
      label8.Name = "lblLongitude";
      componentResourceManager.ApplyResources((object) label9, "lblAddress");
      label9.Name = "lblAddress";
      componentResourceManager.ApplyResources((object) label10, "lblStrata");
      label10.Name = "lblStrata";
      componentResourceManager.ApplyResources((object) label11, "lblPctTree");
      label11.Name = "lblPctTree";
      componentResourceManager.ApplyResources((object) label12, "lblPctShrub");
      label12.Name = "lblPctShrub";
      componentResourceManager.ApplyResources((object) label13, "lblPctPlantable");
      label13.Name = "lblPctPlantable";
      componentResourceManager.ApplyResources((object) label14, "lblPhotoId");
      label14.Name = "lblPhotoId";
      componentResourceManager.ApplyResources((object) label15, "lblContact");
      label15.Name = "lblContact";
      componentResourceManager.ApplyResources((object) label16, "lblNotes");
      label16.Name = "lblNotes";
      this.tcPlotContent.Controls.Add((Control) this.tpPlot);
      this.tcPlotContent.Controls.Add((Control) this.tpTrees);
      this.tcPlotContent.Controls.Add((Control) this.tpShrubs);
      componentResourceManager.ApplyResources((object) this.tcPlotContent, "tcPlotContent");
      this.tcPlotContent.Name = "tcPlotContent";
      this.tcPlotContent.SelectedIndex = 0;
      this.tcPlotContent.SizeMode = TabSizeMode.FillToRight;
      this.tcPlotContent.SelectedIndexChanged += new EventHandler(this.tcPlotContent_SelectedIndexChanged);
      this.tpPlot.Controls.Add((Control) this.tableLayoutPanel2);
      this.tpPlot.Controls.Add((Control) this.tableLayoutPanel1);
      this.tpPlot.Controls.Add((Control) this.richTextLabel1);
      componentResourceManager.ApplyResources((object) this.tpPlot, "tpPlot");
      this.tpPlot.Name = "tpPlot";
      this.tpPlot.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel2, "tableLayoutPanel2");
      this.tableLayoutPanel2.Controls.Add((Control) this.panel1, 0, 0);
      this.tableLayoutPanel2.Controls.Add((Control) this.panel2, 0, 1);
      this.tableLayoutPanel2.Controls.Add((Control) this.panel3, 0, 2);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.panel1.Controls.Add((Control) this.dgLandUses);
      this.panel1.Controls.Add((Control) this.lblLandUse);
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.Name = "panel1";
      componentResourceManager.ApplyResources((object) this.dgLandUses, "dgLandUses");
      this.dgLandUses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      gridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle.BackColor = Color.FromArgb((int) byte.MaxValue, 128, 128);
      gridViewCellStyle.Font = new Font("Calibri", 9.75f);
      gridViewCellStyle.ForeColor = SystemColors.WindowText;
      gridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle.WrapMode = DataGridViewTriState.True;
      this.dgLandUses.ColumnHeadersDefaultCellStyle = gridViewCellStyle;
      this.dgLandUses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgLandUses.Columns.AddRange((DataGridViewColumn) this.luLandUse, (DataGridViewColumn) this.luPctPlot);
      this.dgLandUses.EditMode = DataGridViewEditMode.EditOnEnter;
      this.dgLandUses.Name = "dgLandUses";
      componentResourceManager.ApplyResources((object) this.luLandUse, "luLandUse");
      this.luLandUse.Name = "luLandUse";
      componentResourceManager.ApplyResources((object) this.luPctPlot, "luPctPlot");
      this.luPctPlot.Name = "luPctPlot";
      componentResourceManager.ApplyResources((object) this.lblLandUse, "lblLandUse");
      this.lblLandUse.Name = "lblLandUse";
      this.panel2.Controls.Add((Control) this.dgGroundCovers);
      this.panel2.Controls.Add((Control) this.lblGroundCover);
      componentResourceManager.ApplyResources((object) this.panel2, "panel2");
      this.panel2.Name = "panel2";
      componentResourceManager.ApplyResources((object) this.dgGroundCovers, "dgGroundCovers");
      this.dgGroundCovers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.dgGroundCovers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgGroundCovers.Columns.AddRange((DataGridViewColumn) this.gcGroundCover, (DataGridViewColumn) this.gcPctPlot);
      this.dgGroundCovers.EditMode = DataGridViewEditMode.EditOnEnter;
      this.dgGroundCovers.Name = "dgGroundCovers";
      componentResourceManager.ApplyResources((object) this.gcGroundCover, "gcGroundCover");
      this.gcGroundCover.Name = "gcGroundCover";
      componentResourceManager.ApplyResources((object) this.gcPctPlot, "gcPctPlot");
      this.gcPctPlot.MaxInputLength = 3;
      this.gcPctPlot.Name = "gcPctPlot";
      componentResourceManager.ApplyResources((object) this.lblGroundCover, "lblGroundCover");
      this.lblGroundCover.Name = "lblGroundCover";
      this.panel3.Controls.Add((Control) this.lblRefObjects);
      this.panel3.Controls.Add((Control) this.dgRefObjects);
      componentResourceManager.ApplyResources((object) this.panel3, "panel3");
      this.panel3.Name = "panel3";
      componentResourceManager.ApplyResources((object) this.lblRefObjects, "lblRefObjects");
      this.lblRefObjects.Name = "lblRefObjects";
      componentResourceManager.ApplyResources((object) this.dgRefObjects, "dgRefObjects");
      this.dgRefObjects.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgRefObjects.Columns.AddRange((DataGridViewColumn) this.roType, (DataGridViewColumn) this.roDistance, (DataGridViewColumn) this.roDirection, (DataGridViewColumn) this.roDBH, (DataGridViewColumn) this.roNotes);
      this.dgRefObjects.EditMode = DataGridViewEditMode.EditOnEnter;
      this.dgRefObjects.Name = "dgRefObjects";
      componentResourceManager.ApplyResources((object) this.roType, "roType");
      this.roType.Name = "roType";
      componentResourceManager.ApplyResources((object) this.roDistance, "roDistance");
      this.roDistance.Name = "roDistance";
      componentResourceManager.ApplyResources((object) this.roDirection, "roDirection");
      this.roDirection.Name = "roDirection";
      componentResourceManager.ApplyResources((object) this.roDBH, "roDBH");
      this.roDBH.Name = "roDBH";
      componentResourceManager.ApplyResources((object) this.roNotes, "roNotes");
      this.roNotes.Name = "roNotes";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.ntbPctMeasured, 1, 3);
      this.tableLayoutPanel1.Controls.Add((Control) label1, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) label2, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) label3, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) label4, 0, 3);
      this.tableLayoutPanel1.Controls.Add((Control) label5, 0, 4);
      this.tableLayoutPanel1.Controls.Add((Control) label6, 0, 5);
      this.tableLayoutPanel1.Controls.Add((Control) label7, 0, 6);
      this.tableLayoutPanel1.Controls.Add((Control) label8, 0, 7);
      this.tableLayoutPanel1.Controls.Add((Control) label9, 0, 8);
      this.tableLayoutPanel1.Controls.Add((Control) label10, 0, 9);
      this.tableLayoutPanel1.Controls.Add((Control) label11, 0, 10);
      this.tableLayoutPanel1.Controls.Add((Control) label12, 0, 11);
      this.tableLayoutPanel1.Controls.Add((Control) label13, 0, 12);
      this.tableLayoutPanel1.Controls.Add((Control) label14, 0, 13);
      this.tableLayoutPanel1.Controls.Add((Control) label15, 0, 14);
      this.tableLayoutPanel1.Controls.Add((Control) label16, 0, 15);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtSize, 1, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.dtDate, 1, 4);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtCrew, 1, 5);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtId, 1, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtLatitude, 1, 6);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtLongitude, 1, 7);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtAddress, 1, 8);
      this.tableLayoutPanel1.Controls.Add((Control) this.cboStrata, 1, 9);
      this.tableLayoutPanel1.Controls.Add((Control) this.cboPctTree, 1, 10);
      this.tableLayoutPanel1.Controls.Add((Control) this.cboPctShrub, 1, 11);
      this.tableLayoutPanel1.Controls.Add((Control) this.cboPctPlantable, 1, 12);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtPhotoId, 1, 13);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtContact, 1, 14);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtNotes, 0, 16);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.CellPaint += new TableLayoutCellPaintEventHandler(this.tableLayoutPanel1_CellPaint);
      this.ntbPctMeasured.DecimalPlaces = 2;
      componentResourceManager.ApplyResources((object) this.ntbPctMeasured, "ntbPctMeasured");
      this.ntbPctMeasured.Format = "#;-#";
      this.ntbPctMeasured.HasDecimal = false;
      this.ntbPctMeasured.Name = "ntbPctMeasured";
      this.ntbPctMeasured.Signed = true;
      componentResourceManager.ApplyResources((object) this.txtSize, "txtSize");
      this.txtSize.DecimalPlaces = 2;
      this.txtSize.Format = "#;-#";
      this.txtSize.HasDecimal = false;
      this.txtSize.Name = "txtSize";
      this.txtSize.Signed = true;
      componentResourceManager.ApplyResources((object) this.dtDate, "dtDate");
      this.dtDate.Name = "dtDate";
      this.dtDate.Value = new DateTime?(new DateTime(2014, 8, 18, 2, 12, 10, 229));
      componentResourceManager.ApplyResources((object) this.txtCrew, "txtCrew");
      this.txtCrew.Name = "txtCrew";
      componentResourceManager.ApplyResources((object) this.txtId, "txtId");
      this.txtId.DecimalPlaces = 2;
      this.txtId.Format = "#;-#";
      this.txtId.HasDecimal = false;
      this.txtId.Name = "txtId";
      this.txtId.Signed = true;
      this.txtLatitude.DecimalPlaces = 2;
      componentResourceManager.ApplyResources((object) this.txtLatitude, "txtLatitude");
      this.txtLatitude.Format = "#;-#";
      this.txtLatitude.HasDecimal = false;
      this.txtLatitude.Name = "txtLatitude";
      this.txtLatitude.Signed = true;
      this.txtLongitude.DecimalPlaces = 2;
      componentResourceManager.ApplyResources((object) this.txtLongitude, "txtLongitude");
      this.txtLongitude.Format = "#;-#";
      this.txtLongitude.HasDecimal = false;
      this.txtLongitude.Name = "txtLongitude";
      this.txtLongitude.Signed = true;
      componentResourceManager.ApplyResources((object) this.txtAddress, "txtAddress");
      this.txtAddress.Name = "txtAddress";
      componentResourceManager.ApplyResources((object) this.cboStrata, "cboStrata");
      this.cboStrata.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboStrata.FormattingEnabled = true;
      this.cboStrata.Name = "cboStrata";
      componentResourceManager.ApplyResources((object) this.cboPctTree, "cboPctTree");
      this.cboPctTree.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPctTree.FormattingEnabled = true;
      this.cboPctTree.Name = "cboPctTree";
      componentResourceManager.ApplyResources((object) this.cboPctShrub, "cboPctShrub");
      this.cboPctShrub.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPctShrub.FormattingEnabled = true;
      this.cboPctShrub.Name = "cboPctShrub";
      componentResourceManager.ApplyResources((object) this.cboPctPlantable, "cboPctPlantable");
      this.cboPctPlantable.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPctPlantable.FormattingEnabled = true;
      this.cboPctPlantable.Name = "cboPctPlantable";
      componentResourceManager.ApplyResources((object) this.txtPhotoId, "txtPhotoId");
      this.txtPhotoId.Name = "txtPhotoId";
      componentResourceManager.ApplyResources((object) this.txtContact, "txtContact");
      this.txtContact.Name = "txtContact";
      this.tableLayoutPanel1.SetColumnSpan((Control) this.txtNotes, 2);
      componentResourceManager.ApplyResources((object) this.txtNotes, "txtNotes");
      this.txtNotes.Name = "txtNotes";
      componentResourceManager.ApplyResources((object) this.richTextLabel1, "richTextLabel1");
      this.richTextLabel1.Name = "richTextLabel1";
      this.richTextLabel1.TabStop = false;
      this.tpTrees.Controls.Add((Control) this.dgTrees);
      this.tpTrees.Controls.Add((Control) this.richTextLabel2);
      componentResourceManager.ApplyResources((object) this.tpTrees, "tpTrees");
      this.tpTrees.Name = "tpTrees";
      this.tpTrees.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.dgTrees, "dgTrees");
      this.dgTrees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgTrees.Name = "dgTrees";
      componentResourceManager.ApplyResources((object) this.richTextLabel2, "richTextLabel2");
      this.richTextLabel2.Name = "richTextLabel2";
      this.richTextLabel2.TabStop = false;
      this.tpShrubs.Controls.Add((Control) this.dgShrubs);
      this.tpShrubs.Controls.Add((Control) this.richTextLabel3);
      componentResourceManager.ApplyResources((object) this.tpShrubs, "tpShrubs");
      this.tpShrubs.Name = "tpShrubs";
      this.tpShrubs.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.dgShrubs, "dgShrubs");
      this.dgShrubs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgShrubs.Columns.AddRange((DataGridViewColumn) this.shId, (DataGridViewColumn) this.shSpecies, (DataGridViewColumn) this.shHeight, (DataGridViewColumn) this.shPctShrubArea, (DataGridViewColumn) this.shPctMissing, (DataGridViewColumn) this.shComments);
      this.dgShrubs.Name = "dgShrubs";
      componentResourceManager.ApplyResources((object) this.shId, "shId");
      this.shId.Name = "shId";
      componentResourceManager.ApplyResources((object) this.shSpecies, "shSpecies");
      this.shSpecies.Name = "shSpecies";
      componentResourceManager.ApplyResources((object) this.shHeight, "shHeight");
      this.shHeight.Name = "shHeight";
      componentResourceManager.ApplyResources((object) this.shPctShrubArea, "shPctShrubArea");
      this.shPctShrubArea.Name = "shPctShrubArea";
      componentResourceManager.ApplyResources((object) this.shPctMissing, "shPctMissing");
      this.shPctMissing.Name = "shPctMissing";
      componentResourceManager.ApplyResources((object) this.shComments, "shComments");
      this.shComments.Name = "shComments";
      componentResourceManager.ApplyResources((object) this.richTextLabel3, "richTextLabel3");
      this.richTextLabel3.Name = "richTextLabel3";
      this.richTextLabel3.TabStop = false;
      this.bgWorker.DoWork += new DoWorkEventHandler(this.bgWorker_DoWork);
      this.bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
      this.AllowEndUserDocking = false;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tcPlotContent);
      this.DockAreas = DockAreas.Float | DockAreas.Document;
      this.HideOnClose = true;
      this.Name = nameof (PlotContentForm);
      this.ShowHint = DockState.Document;
      this.tcPlotContent.ResumeLayout(false);
      this.tpPlot.ResumeLayout(false);
      this.tableLayoutPanel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.dgLandUses).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      ((ISupportInitialize) this.dgGroundCovers).EndInit();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      ((ISupportInitialize) this.dgRefObjects).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.tpTrees.ResumeLayout(false);
      ((ISupportInitialize) this.dgTrees).EndInit();
      this.tpShrubs.ResumeLayout(false);
      ((ISupportInitialize) this.dgShrubs).EndInit();
      this.ResumeLayout(false);
    }
  }
}
