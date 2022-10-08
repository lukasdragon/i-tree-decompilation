// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.PlantingSitesForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Interfaces;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class PlantingSitesForm : DataContentForm, IActionable, IExportable
  {
    private DataGridViewManager m_dgManager;
    private IList<Plot> m_plots;
    private IList<PlantingSite> m_sites;
    private IList<PlantingSiteType> m_siteTypes;
    private IList<Street> m_streets;
    private IList<LandUse> m_landuses;
    private IContainer components;
    private DataGridView dgPlantingSites;
    private DataGridViewComboBoxColumn dcPlot;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewComboBoxColumn dcSiteType;
    private DataGridViewComboBoxColumn dcLandUse;
    private DataGridViewComboBoxColumn dcStreet;
    private DataGridViewTextBoxColumn dcAddres;
    private DataGridViewNumericTextBoxColumn dcLatitude;
    private DataGridViewNumericTextBoxColumn dcLongitude;

    public PlantingSitesForm()
    {
      this.InitializeComponent();
      this.m_dgManager = new DataGridViewManager(this.dgPlantingSites);
      this.dgPlantingSites.DoubleBuffered(true);
      this.dgPlantingSites.AutoGenerateColumns = false;
      this.dgPlantingSites.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgPlantingSites.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
    }

    protected override void LoadData()
    {
      lock (this.Session)
      {
        using (this.Session.BeginTransaction())
        {
          this.m_plots = this.Session.CreateCriteria<Plot>().Add((ICriterion) Restrictions.Eq("Year", (object) this.Year)).AddOrder(Order.Asc("Id")).List<Plot>();
          this.m_sites = this.Session.CreateCriteria<PlantingSite>().CreateAlias("Plot", "p").Add((ICriterion) Restrictions.Eq("p.Year", (object) this.Year)).AddOrder(Order.Asc("Id")).List<PlantingSite>();
          this.m_siteTypes = this.Session.QueryOver<PlantingSiteType>().Where((System.Linq.Expressions.Expression<Func<PlantingSiteType, bool>>) (ps => ps.Year == this.Year)).OrderBy((System.Linq.Expressions.Expression<Func<PlantingSiteType, object>>) (ps => ps.Description)).Asc.List();
          this.m_streets = this.Session.CreateCriteria<Street>().CreateAlias("ProjectLocation", "pl").CreateAlias("pl.Project", "p").CreateAlias("p.Series", "s").CreateAlias("s.Year", "y").Add((ICriterion) Restrictions.Eq("y", (object) this.Year)).AddOrder(Order.Asc("Name")).List<Street>();
          this.m_landuses = this.Session.CreateCriteria<LandUse>().Add((ICriterion) Restrictions.Eq("Year", (object) this.Year)).AddOrder(Order.Asc("Description")).List<LandUse>();
        }
      }
    }

    protected override void OnDataLoaded()
    {
      this.InitGrid();
      base.OnDataLoaded();
    }

    private void InitGrid()
    {
      this.dcSiteType.BindTo<PlantingSiteType>((System.Linq.Expressions.Expression<Func<PlantingSiteType, object>>) (st => st.Description), (System.Linq.Expressions.Expression<Func<PlantingSiteType, object>>) (st => st.Self), (object) this.m_siteTypes);
      this.dcPlot.BindTo<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id), (System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Self), (object) this.m_plots);
      this.dcStreet.BindTo<Street>((System.Linq.Expressions.Expression<Func<Street, object>>) (st => st.Name), (System.Linq.Expressions.Expression<Func<Street, object>>) (st => st.Self), (object) this.m_streets);
      this.dcLandUse.BindTo<LandUse>((System.Linq.Expressions.Expression<Func<LandUse, object>>) (lu => lu.Description), (System.Linq.Expressions.Expression<Func<LandUse, object>>) (lu => lu.Self), (object) this.m_landuses);
      DataBindingList<PlantingSite> dataBindingList = new DataBindingList<PlantingSite>(this.m_sites);
      this.dgPlantingSites.DataSource = (object) dataBindingList;
      this.dgPlantingSites.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      dataBindingList.Sortable = true;
      dataBindingList.AddingNew += new AddingNewEventHandler(this.PlantingSites_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.PlantingSites_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.PlantingSites_ListChanged);
      dataBindingList.AddComparer<PlantingSite>((System.Linq.Expressions.Expression<Func<PlantingSite, object>>) (ps => ps.PlantingSiteType), (IComparer) new PropertyComparer<PlantingSiteType>((Func<PlantingSiteType, object>) (pst => (object) pst.Description)));
      dataBindingList.AddComparer<PlantingSite>((System.Linq.Expressions.Expression<Func<PlantingSite, object>>) (ps => ps.PlotLandUse), (IComparer) new ChildComparer<PlotLandUse, LandUse>((Func<PlotLandUse, LandUse>) (plu => plu.LandUse), (IComparer<LandUse>) new PropertyComparer<LandUse>((Func<LandUse, object>) (lu => (object) lu.Description))));
      dataBindingList.AddComparer<PlantingSite>((System.Linq.Expressions.Expression<Func<PlantingSite, object>>) (ps => ps.Street), (IComparer) new PropertyComparer<Street>((Func<Street, object>) (s => (object) s.Name)));
      this.OnRequestRefresh();
    }

    protected override void OnRequestRefresh()
    {
      Year year = this.Year;
      bool flag1 = year != null && year.Changed && this.m_plots.Count > 0 && this.dgPlantingSites.DataSource != null;
      bool flag2 = flag1 && this.m_sites.Count > 0;
      this.dgPlantingSites.ReadOnly = !flag1;
      this.dgPlantingSites.AllowUserToAddRows = flag1;
      this.dgPlantingSites.AllowUserToDeleteRows = flag2;
      base.OnRequestRefresh();
    }

    private void PlantingSites_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void PlantingSites_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<PlantingSite> dataBindingList = sender as DataBindingList<PlantingSite>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      PlantingSite plantingSite = dataBindingList[e.NewIndex];
      if (plantingSite.IsTransient)
        return;
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          this.Session.Delete((object) plantingSite);
          transaction.Commit();
        }
      }
    }

    private void PlantingSites_AddingNew(object sender, AddingNewEventArgs e)
    {
      PlantingSite plantingSite = new PlantingSite();
      e.NewObject = (object) plantingSite;
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.Year != null && this.Year.Changed && this.dgPlantingSites != null;
      bool flag2 = flag1 && this.dgPlantingSites.AllowUserToAddRows;
      bool flag3 = this.dgPlantingSites.SelectedRows.Count > 0;
      bool flag4 = this.dgPlantingSites.CurrentRow != null && this.dgPlantingSites.CurrentRow.IsNewRow;
      bool flag5 = this.dgPlantingSites.CurrentRow != null && this.dgPlantingSites.IsCurrentRowDirty;
      switch (action)
      {
        case UserActions.New:
          return flag2 && !flag4 && !flag5;
        case UserActions.Copy:
          return false;
        case UserActions.Undo:
          return flag1 && this.m_dgManager.CanUndo;
        case UserActions.Redo:
          return flag1 && this.m_dgManager.CanRedo;
        case UserActions.Delete:
          return flag1 & flag3 && !flag4 | flag5;
        default:
          return false;
      }
    }

    public void PerformAction(UserActions action)
    {
      switch (action)
      {
        case UserActions.New:
          if (!this.dgPlantingSites.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgPlantingSites.SelectedRows)
            selectedRow.Selected = false;
          this.dgPlantingSites.Rows[this.dgPlantingSites.NewRowIndex].Selected = true;
          this.dgPlantingSites.FirstDisplayedScrollingRowIndex = this.dgPlantingSites.NewRowIndex - this.dgPlantingSites.DisplayedRowCount(false) + 1;
          this.dgPlantingSites.CurrentCell = this.dgPlantingSites.Rows[this.dgPlantingSites.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          if (this.dgPlantingSites.SelectedRows.Count <= 0)
            break;
          DataGridViewRow selectedRow1 = this.dgPlantingSites.SelectedRows[0];
          if (!(this.dgPlantingSites.DataSource is DataBindingList<PlantingSite> dataSource) || selectedRow1.Index >= dataSource.Count)
            break;
          PlantingSite plantingSite1 = dataSource[selectedRow1.Index].Clone() as PlantingSite;
          int num = 1;
          foreach (PlantingSite plantingSite2 in (IEnumerable<PlantingSite>) plantingSite1.Plot.PlantingSites)
          {
            if (plantingSite2.Id >= num)
              num = plantingSite2.Id + 1;
          }
          plantingSite1.Id = num;
          dataSource.Add(plantingSite1);
          this.dgPlantingSites.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          if (this.dgPlantingSites.SelectedRows.Count <= 0)
            break;
          this.DeleteSelectedRows();
          break;
      }
    }

    private void dgPlantingSites_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<PlantingSite> dataSource = this.dgPlantingSites.DataSource as DataBindingList<PlantingSite>;
      if (this.dgPlantingSites.CurrentRow != null && !this.dgPlantingSites.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      PlantingSite plantingSite1 = dataSource[e.RowIndex];
      string text = (string) null;
      if (plantingSite1.Plot == null)
        text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) this.dcPlot.HeaderText);
      else if (plantingSite1.Id == 0)
        text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) this.dcId.HeaderText);
      else if (plantingSite1.PlantingSiteType == null)
      {
        text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) this.dcSiteType.HeaderText);
      }
      else
      {
        foreach (PlantingSite plantingSite2 in (IEnumerable<PlantingSite>) plantingSite1.Plot.PlantingSites)
        {
          if (plantingSite2 != plantingSite1 && plantingSite2.Id == plantingSite1.Id)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcId.HeaderText);
            break;
          }
        }
      }
      if (text == null)
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgPlantingSites_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      DataBindingList<PlantingSite> dataSource = this.dgPlantingSites.DataSource as DataBindingList<PlantingSite>;
      if (this.dgPlantingSites.CurrentRow != null && this.dgPlantingSites.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (e.RowIndex >= dataSource.Count)
          return;
        PlantingSite plantingSite = dataSource[e.RowIndex];
        lock (this.Session)
        {
          using (ITransaction transaction = this.Session.BeginTransaction())
          {
            this.Session.SaveOrUpdate((object) plantingSite);
            transaction.Commit();
          }
        }
      }
    }

    private void dgPlantingSites_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void dgPlantingSites_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void btnNew_Click(object sender, EventArgs e)
    {
      if (!this.dgPlantingSites.AllowUserToAddRows)
        return;
      foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgPlantingSites.SelectedRows)
        selectedRow.Selected = false;
      this.dgPlantingSites.Rows[this.dgPlantingSites.NewRowIndex].Selected = true;
      this.dgPlantingSites.FirstDisplayedScrollingRowIndex = this.dgPlantingSites.NewRowIndex - this.dgPlantingSites.DisplayedRowCount(false) + 1;
      this.dgPlantingSites.CurrentCell = this.dgPlantingSites.Rows[this.dgPlantingSites.NewRowIndex].Cells[0];
    }

    private void btnCopy_Click(object sender, EventArgs e)
    {
      if (this.dgPlantingSites.SelectedRows.Count <= 0)
        return;
      DataGridViewRow selectedRow = this.dgPlantingSites.SelectedRows[0];
      if (!(this.dgPlantingSites.DataSource is DataBindingList<PlantingSite> dataSource) || selectedRow.Index >= dataSource.Count)
        return;
      PlantingSite plantingSite = dataSource[selectedRow.Index].Clone() as PlantingSite;
      dataSource.Add(plantingSite);
      this.dgPlantingSites.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
    }

    private void DeleteSelectedRows()
    {
      if (!(this.dgPlantingSites.DataSource is DataBindingList<PlantingSite> dataSource))
        return;
      CurrencyManager currencyManager = this.dgPlantingSites.BindingContext[(object) dataSource] as CurrencyManager;
      bool flag = false;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgPlantingSites.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          dataSource.RemoveAt(selectedRow.Index);
          flag = true;
        }
      }
      if (!(currencyManager.Position != -1 & flag))
        return;
      this.dgPlantingSites.Rows[currencyManager.Position].Selected = true;
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
        return;
      this.dgPlantingSites.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format) => format == ExportFormat.CSV && this.dgPlantingSites.DataSource != null;

    private void dgPlantingSites_EditingControlShowing(
      object sender,
      DataGridViewEditingControlShowingEventArgs e)
    {
      if (!(e.Control is ComboBox control))
        return;
      control.KeyDown -= new KeyEventHandler(this.DataGridViewComboBoxCell_KeyDown);
      control.KeyDown += new KeyEventHandler(this.DataGridViewComboBoxCell_KeyDown);
    }

    private void DataGridViewComboBoxCell_KeyDown(object sender, KeyEventArgs e)
    {
      if (!(sender is ComboBox comboBox) || e.KeyCode != Keys.Delete)
        return;
      comboBox.SelectedIndex = -1;
      e.Handled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      this.dgPlantingSites = new DataGridView();
      this.dcPlot = new DataGridViewComboBoxColumn();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcSiteType = new DataGridViewComboBoxColumn();
      this.dcLandUse = new DataGridViewComboBoxColumn();
      this.dcStreet = new DataGridViewComboBoxColumn();
      this.dcAddres = new DataGridViewTextBoxColumn();
      this.dcLatitude = new DataGridViewNumericTextBoxColumn();
      this.dcLongitude = new DataGridViewNumericTextBoxColumn();
      ((ISupportInitialize) this.dgPlantingSites).BeginInit();
      this.SuspendLayout();
      this.lblBreadcrumb.Size = new Size(604, 29);
      this.dgPlantingSites.AllowUserToAddRows = false;
      this.dgPlantingSites.AllowUserToDeleteRows = false;
      this.dgPlantingSites.AllowUserToResizeRows = false;
      this.dgPlantingSites.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgPlantingSites.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgPlantingSites.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgPlantingSites.Columns.AddRange((DataGridViewColumn) this.dcPlot, (DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcSiteType, (DataGridViewColumn) this.dcLandUse, (DataGridViewColumn) this.dcStreet, (DataGridViewColumn) this.dcAddres, (DataGridViewColumn) this.dcLatitude, (DataGridViewColumn) this.dcLongitude);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgPlantingSites.DefaultCellStyle = gridViewCellStyle2;
      this.dgPlantingSites.Dock = DockStyle.Fill;
      this.dgPlantingSites.EnableHeadersVisualStyles = false;
      this.dgPlantingSites.Location = new Point(0, 29);
      this.dgPlantingSites.MultiSelect = false;
      this.dgPlantingSites.Name = "dgPlantingSites";
      this.dgPlantingSites.ReadOnly = true;
      this.dgPlantingSites.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgPlantingSites.RowHeadersWidth = 20;
      this.dgPlantingSites.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
      this.dgPlantingSites.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgPlantingSites.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgPlantingSites.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgPlantingSites.Size = new Size(604, 333);
      this.dgPlantingSites.TabIndex = 2;
      this.dgPlantingSites.CurrentCellDirtyStateChanged += new EventHandler(this.dgPlantingSites_CurrentCellDirtyStateChanged);
      this.dgPlantingSites.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgPlantingSites_EditingControlShowing);
      this.dgPlantingSites.RowValidated += new DataGridViewCellEventHandler(this.dgPlantingSites_RowValidated);
      this.dgPlantingSites.RowValidating += new DataGridViewCellCancelEventHandler(this.dgPlantingSites_RowValidating);
      this.dgPlantingSites.SelectionChanged += new EventHandler(this.dgPlantingSites_SelectionChanged);
      this.dcPlot.DataPropertyName = "Plot";
      this.dcPlot.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcPlot.HeaderText = "Plot";
      this.dcPlot.Name = "dcPlot";
      this.dcPlot.ReadOnly = true;
      this.dcId.DataPropertyName = "Id";
      this.dcId.DecimalPlaces = 0;
      this.dcId.Format = "#;#";
      this.dcId.HasDecimal = false;
      this.dcId.HeaderText = "ID";
      this.dcId.Name = "dcId";
      this.dcId.ReadOnly = true;
      this.dcId.Resizable = DataGridViewTriState.True;
      this.dcId.Signed = false;
      this.dcSiteType.DataPropertyName = "PlantingSiteType";
      this.dcSiteType.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcSiteType.HeaderText = "Site Type";
      this.dcSiteType.Name = "dcSiteType";
      this.dcSiteType.ReadOnly = true;
      this.dcSiteType.Resizable = DataGridViewTriState.True;
      this.dcSiteType.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcLandUse.DataPropertyName = "LandUse";
      this.dcLandUse.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcLandUse.HeaderText = "Land Use";
      this.dcLandUse.Name = "dcLandUse";
      this.dcLandUse.ReadOnly = true;
      this.dcStreet.DataPropertyName = "Street";
      this.dcStreet.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcStreet.HeaderText = "Street";
      this.dcStreet.Name = "dcStreet";
      this.dcStreet.ReadOnly = true;
      this.dcAddres.DataPropertyName = "StreetAddress";
      this.dcAddres.HeaderText = "Address";
      this.dcAddres.MaxInputLength = (int) byte.MaxValue;
      this.dcAddres.Name = "dcAddres";
      this.dcAddres.ReadOnly = true;
      this.dcLatitude.DataPropertyName = "yCoordinate";
      this.dcLatitude.DecimalPlaces = 0;
      this.dcLatitude.Format = "#.#;-#.#";
      this.dcLatitude.HasDecimal = true;
      this.dcLatitude.HeaderText = "Latitude (Y)";
      this.dcLatitude.Name = "dcLatitude";
      this.dcLatitude.ReadOnly = true;
      this.dcLatitude.Resizable = DataGridViewTriState.True;
      this.dcLatitude.Signed = true;
      this.dcLongitude.DataPropertyName = "xCoordinate";
      this.dcLongitude.DecimalPlaces = 0;
      this.dcLongitude.Format = "#.#;-#.#";
      this.dcLongitude.HasDecimal = true;
      this.dcLongitude.HeaderText = "Longitude (X)";
      this.dcLongitude.Name = "dcLongitude";
      this.dcLongitude.ReadOnly = true;
      this.dcLongitude.Resizable = DataGridViewTriState.True;
      this.dcLongitude.Signed = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(604, 362);
      this.Controls.Add((Control) this.dgPlantingSites);
      this.DockAreas = DockAreas.Document;
      this.Name = nameof (PlantingSitesForm);
      this.ShowHint = DockState.Document;
      this.Text = "Planting Sites";
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgPlantingSites, 0);
      ((ISupportInitialize) this.dgPlantingSites).EndInit();
      this.ResumeLayout(false);
    }
  }
}
