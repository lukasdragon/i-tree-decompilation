// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.PlotsContentForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Constraints;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Interfaces;
using NHibernate;
using NHibernate.Criterion;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class PlotsContentForm : DataContentForm, IActionable, IExportable, IPlotContent
  {
    private readonly DataGridViewManager m_dgManager;
    private readonly System.Windows.Forms.Timer m_timer;
    private ProjectLocation m_projectLocation;
    private int m_dgHorizPos;
    private IList<Plot> m_plots;
    private IContainer components;
    private DataGridView dgPlots;
    private DataGridViewTextBoxColumn dcId;
    private DataGridViewComboBoxColumn dcStrata;
    private DataGridViewTextBoxColumn dcAddress;
    private DataGridViewTextBoxColumn dcLatitude;
    private DataGridViewTextBoxColumn dcLongitude;
    private DataGridViewNullableDateTimeColumn dcDate;
    private DataGridViewTextBoxColumn dcCrew;
    private DataGridViewTextBoxColumn dcContactInfo;
    private DataGridViewNumericTextBoxColumn dcSize;
    private DataGridViewTextBoxColumn dcPhoto;
    private DataGridViewCheckBoxColumn dcStake;
    private DataGridViewComboBoxColumn dcPctTree;
    private DataGridViewComboBoxColumn dcPctShrub;
    private DataGridViewComboBoxColumn dcPctPlantable;
    private DataGridViewNumericTextBoxColumn dcPctMeasured;
    private DataGridViewTextBoxColumn dcComments;
    private DataGridViewCheckBoxColumn dcIsComplete;

    public event EventHandler<PlotEventArgs> PlotSelectionChanged;

    public event EventHandler<CancelEventArgs> PlotValidating;

    public PlotsContentForm()
    {
      this.m_timer = new System.Windows.Forms.Timer()
      {
        Interval = (SystemInformation.KeyboardDelay + 1) * 250 + 100
      };
      this.m_timer.Tick += new EventHandler(this.PlotTimer_OnTick);
      this.InitializeComponent();
      this.m_dgManager = new DataGridViewManager(this.dgPlots);
      this.dgPlots.DoubleBuffered(true);
      this.dgPlots.AutoGenerateColumns = false;
      this.dgPlots.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgPlots.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dcSize.DefaultCellStyle.DataSourceNullValue = (object) -1f;
    }

    protected override void OnRequestRefresh()
    {
      bool flag = this.Year != null && this.Year.Changed && this.dgPlots.DataSource != null;
      this.dgPlots.ReadOnly = !flag;
      this.dgPlots.AllowUserToAddRows = flag;
      this.dgPlots.AllowUserToDeleteRows = flag;
      base.OnRequestRefresh();
    }

    private void Plots_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void Plots_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<Plot> dataBindingList = sender as DataBindingList<Plot>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      Plot entity = dataBindingList[e.NewIndex];
      if (entity.IsTransient)
        return;
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          this.Session.Delete((object) entity);
          transaction.Commit();
        }
      }
      EventPublisher.Publish<EntityDeleted<Plot>>(new EntityDeleted<Plot>(entity), (Control) this);
    }

    private void Plots_AddingNew(object sender, AddingNewEventArgs e)
    {
      int num = this.NextPlotId();
      Plot plot = new Plot()
      {
        Id = num,
        Year = this.Year,
        ProjectLocation = this.m_projectLocation,
        PercentMeasured = 100
      };
      plot.Size = this.Year.Unit != YearUnit.English ? 0.0404686f : 0.1f;
      e.NewObject = (object) plot;
    }

    private int NextPlotId() => this.NextPlotId(1);

    private int NextPlotId(int nextId)
    {
      lock (this.Session)
      {
        using (TypeHelper<Plot> typeHelper = new TypeHelper<Plot>())
        {
          using (this.Session.BeginTransaction())
          {
            try
            {
              return this.Session.CreateCriteria<Plot>().Add((ICriterion) Restrictions.Eq(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Year)), (object) this.Year)).SetProjection((IProjection) Projections.Max(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)))).UniqueResult<int>() + 1;
            }
            catch (HibernateException ex)
            {
              return nextId;
            }
          }
        }
      }
    }

    private void PlotTimer_OnTick(object sender, EventArgs e)
    {
      this.m_timer.Stop();
      DataGridViewRow currentRow = this.dgPlots.CurrentRow;
      if (currentRow == null)
        return;
      this.OnPlotSelectionChanged(currentRow.Index);
    }

    private void dgPlots_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgPlots.NewRowIndex || !(this.dgPlots.DataSource is DataBindingList<Plot> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgPlots.Columns[e.ColumnIndex];
      Plot plot = dataSource[e.RowIndex];
      if (column == this.dcId)
      {
        if (plot.Id > 0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcStrata)
      {
        if (plot.Strata != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcPctTree)
      {
        if (!plot.IsComplete || plot.PercentTreeCover != PctMidRange.PRINV)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else
      {
        if (column != this.dcPctMeasured || plot.PercentMeasured >= 0 && plot.PercentMeasured <= 100)
          return;
        DataGridViewCellErrorTextNeededEventArgs textNeededEventArgs = e;
        string errFieldRange = i_Tree_Eco_v6.Resources.Strings.ErrFieldRange;
        string headerText = column.HeaderText;
        int num = 0;
        string str1 = num.ToString("P0");
        num = 100;
        string str2 = num.ToString("P0");
        string str3 = string.Format(errFieldRange, (object) headerText, (object) str1, (object) str2);
        textNeededEventArgs.ErrorText = str3;
      }
    }

    private void dgPlots_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<Plot> dataSource = this.dgPlots.DataSource as DataBindingList<Plot>;
      if (this.dgPlots.CurrentRow != null && !this.dgPlots.IsCurrentRowDirty)
        return;
      if (dataSource != null && e.RowIndex < dataSource.Count)
      {
        Plot plot1 = dataSource[e.RowIndex];
        DataGridViewRow row = this.dgPlots.Rows[e.RowIndex];
        DataGridViewCell dataGridViewCell1 = (DataGridViewCell) null;
        string text = (string) null;
        foreach (Plot plot2 in (Collection<Plot>) dataSource)
        {
          if (plot2 != plot1 && plot2.Id == plot1.Id)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcId.HeaderText);
            dataGridViewCell1 = row.Cells[this.dcId.DisplayIndex];
            break;
          }
        }
        if (text == null)
        {
          DataGridViewCell dataGridViewCell2 = row.ErrorCell();
          if (dataGridViewCell2 != null)
            text = dataGridViewCell2.ErrorText;
        }
        if (text != null)
        {
          e.Cancel = true;
          int num = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      this.OnPlotValidating((CancelEventArgs) e);
    }

    private void dgPlots_SelectionChanged(object sender, EventArgs e)
    {
      this.m_timer.Stop();
      this.OnRequestRefresh();
      this.m_timer.Start();
    }

    private void dgPlots_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void dgPlots_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      DataBindingList<Plot> dataSource = this.dgPlots.DataSource as DataBindingList<Plot>;
      if (this.dgPlots.CurrentRow != null && this.dgPlots.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          Plot entity = dataSource[e.RowIndex];
          if (entity.IsTransient || this.Session.IsDirty())
          {
            lock (this.Session)
            {
              bool isTransient = entity.IsTransient;
              using (ITransaction transaction = this.Session.BeginTransaction())
              {
                this.Session.SaveOrUpdate((object) entity);
                transaction.Commit();
              }
              if (isTransient)
                EventPublisher.Publish<EntityCreated<Plot>>(new EntityCreated<Plot>(entity), (Control) this);
              else
                EventPublisher.Publish<EntityUpdated<Plot>>(new EntityUpdated<Plot>(entity), (Control) this);
            }
          }
          this.m_timer.Stop();
          this.m_timer.Start();
        }
        this.OnRequestRefresh();
      }
    }

    private void dgPlots_Scroll(object sender, ScrollEventArgs e)
    {
      if (e.ScrollOrientation != ScrollOrientation.HorizontalScroll)
        return;
      this.m_dgHorizPos = e.NewValue;
    }

    private void dgPlots_Sorted(object sender, EventArgs e) => this.dgPlots.HorizontalScrollingOffset = this.m_dgHorizPos;

    private void DeleteSelectedRows()
    {
      if (!(this.dgPlots.DataSource is DataBindingList<Plot> dataSource))
        return;
      CurrencyManager currencyManager = this.dgPlots.BindingContext[(object) dataSource] as CurrencyManager;
      bool flag = false;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgPlots.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          dataSource.RemoveAt(selectedRow.Index);
          flag = true;
        }
      }
      if (!(currencyManager.Position != -1 & flag))
        return;
      this.dgPlots.Rows[currencyManager.Position].Selected = true;
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = (this.Year == null || !this.Year.Changed ? 0 : (this.dgPlots.DataSource != null ? 1 : 0)) != 0 && this.dgPlots.AllowUserToAddRows;
      bool flag2 = this.dgPlots.SelectedRows.Count > 0;
      bool flag3 = this.dgPlots.CurrentRow != null && this.dgPlots.IsCurrentRowDirty;
      bool flag4 = this.dgPlots.CurrentRow != null && this.dgPlots.CurrentRow.IsNewRow;
      bool flag5 = false;
      switch (action)
      {
        case UserActions.New:
          flag5 = flag1 && !flag4 && !flag3;
          break;
        case UserActions.Copy:
          flag5 = flag1 & flag2 && !flag4 && !flag3;
          break;
        case UserActions.Undo:
          flag5 = this.m_dgManager.CanUndo;
          break;
        case UserActions.Redo:
          flag5 = this.m_dgManager.CanRedo;
          break;
        case UserActions.Delete:
          flag5 = flag2 && !flag4 | flag3;
          break;
        case UserActions.ImportData:
          flag5 = flag1 && !flag4 && !flag3;
          break;
      }
      return flag5;
    }

    public void PerformAction(UserActions action)
    {
      switch (action)
      {
        case UserActions.New:
          if (!this.dgPlots.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgPlots.SelectedRows)
            selectedRow.Selected = false;
          this.dgPlots.Rows[this.dgPlots.NewRowIndex].Selected = true;
          this.dgPlots.FirstDisplayedScrollingRowIndex = this.dgPlots.NewRowIndex - this.dgPlots.DisplayedRowCount(false) + 1;
          this.dgPlots.CurrentCell = this.dgPlots.Rows[this.dgPlots.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          if (this.dgPlots.SelectedRows.Count <= 0)
            break;
          DataGridViewRow selectedRow1 = this.dgPlots.SelectedRows[0];
          if (!(this.dgPlots.DataSource is DataBindingList<Plot> dataSource) || selectedRow1.Index >= dataSource.Count)
            break;
          Plot plot1 = dataSource[selectedRow1.Index].Clone() as Plot;
          int num = 1;
          foreach (Plot plot2 in (Collection<Plot>) dataSource)
          {
            if (plot2.Id >= num)
              num = plot2.Id + 1;
          }
          plot1.Id = num;
          dataSource.Add(plot1);
          this.dgPlots.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
          if (this.dgPlots.SelectedRows.Count <= 0)
            break;
          this.DeleteSelectedRows();
          break;
      }
    }

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
      {
        if (format != ExportFormat.KML)
          return;
        this.ExportKml(file);
      }
      else
        this.dgPlots.ExportToCSV(file, (Encoding) new UTF8Encoding(false));
    }

    public bool CanExport(ExportFormat format)
    {
      switch (format)
      {
        case ExportFormat.CSV:
          return this.dgPlots.DataSource != null;
        case ExportFormat.KML:
          return this.dgPlots.DataSource != null && this.Year != null && this.Year.RecordGPS;
        default:
          return false;
      }
    }

    private void ExportKml(string fn)
    {
      if (!(this.dgPlots.DataSource is DataBindingList<Plot> dataSource))
        return;
      Kml root = new Kml();
      Document document1 = new Document();
      foreach (Plot plot in (Collection<Plot>) dataSource)
      {
        double? nullable = plot.Latitude;
        if (nullable.HasValue)
        {
          nullable = plot.Latitude;
          double num1 = -90.0;
          if (!(nullable.GetValueOrDefault() < num1 & nullable.HasValue))
          {
            nullable = plot.Latitude;
            double num2 = 90.0;
            if (!(nullable.GetValueOrDefault() > num2 & nullable.HasValue))
            {
              nullable = plot.Longitude;
              if (nullable.HasValue)
              {
                nullable = plot.Longitude;
                double num3 = -180.0;
                if (!(nullable.GetValueOrDefault() < num3 & nullable.HasValue))
                {
                  nullable = plot.Longitude;
                  double num4 = 180.0;
                  if (!(nullable.GetValueOrDefault() > num4 & nullable.HasValue))
                  {
                    Document document2 = document1;
                    Placemark placemark = new Placemark();
                    int id = plot.Id;
                    placemark.Id = id.ToString();
                    id = plot.Id;
                    placemark.Name = id.ToString();
                    placemark.Description = new Description()
                    {
                      Text = plot.Strata.Description
                    };
                    SharpKml.Dom.Point point = new SharpKml.Dom.Point();
                    nullable = plot.Latitude;
                    double latitude = nullable.Value;
                    nullable = plot.Longitude;
                    double longitude = nullable.Value;
                    point.Coordinate = new Vector(latitude, longitude, 0.0);
                    placemark.Geometry = (Geometry) point;
                    document2.AddFeature((Feature) placemark);
                  }
                }
              }
            }
          }
        }
      }
      root.Feature = (Feature) document1;
      KmlFile.Create((Element) root, true).Save((Stream) new FileStream(fn, FileMode.Create));
    }

    public void ContentActivated()
    {
      this.dgPlots.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgPlots.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgPlots.DefaultCellStyle = Program.ActiveGridDefaultCellStyle;
    }

    public void ContentDeactivated()
    {
      this.dgPlots.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgPlots.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dgPlots.DefaultCellStyle = Program.InActiveGridDefaultCellStyle;
    }

    protected override void OnDataLoaded()
    {
      this.InitGrid();
      base.OnDataLoaded();
    }

    protected override void InitializeYear(Year year)
    {
      base.InitializeYear(year);
      NHibernateUtil.Initialize((object) year.Strata);
    }

    protected override void LoadData()
    {
      base.LoadData();
      lock (this.Session)
      {
        using (this.Session.BeginTransaction())
        {
          ProjectLocation projectLocation = this.Session.QueryOver<ProjectLocation>().Where((System.Linq.Expressions.Expression<Func<ProjectLocation, bool>>) (pl => pl.Project == this.Year.Series.Project)).SingleOrDefault();
          IList<Plot> plotList = this.Session.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.Year)).OrderBy((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)).Asc.List();
          this.m_projectLocation = projectLocation;
          this.m_plots = plotList;
        }
      }
    }

    private void InitGrid()
    {
      IDictionary<PctMidRange, string> dictionary = EnumHelper.ConvertToDictionary<PctMidRange>();
      Year year = this.Year;
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.dcStrata.BindTo<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Description), (System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Self), (object) this.Year.Strata.ToList<Strata>());
      if (!year.RecordPlotAddress)
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcAddress);
      if (!year.RecordGPS)
      {
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcLatitude);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcLongitude);
      }
      this.dcPctTree.BindTo("Value", "Key", (object) new BindingSource((object) dictionary, (string) null));
      this.dcPctTree.DefaultCellStyle.DataSourceNullValue = (object) PctMidRange.PRINV;
      if (!year.RecordPercentShrub)
      {
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcPctShrub);
      }
      else
      {
        this.dcPctShrub.BindTo("Value", "Key", (object) new BindingSource((object) dictionary, (string) null));
        this.dcPctShrub.DefaultCellStyle.DataSourceNullValue = (object) PctMidRange.PRINV;
      }
      if (!year.RecordPlantableSpace)
      {
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcPctPlantable);
      }
      else
      {
        this.dcPctPlantable.BindTo("Value", "Key", (object) new BindingSource((object) dictionary, (string) null));
        this.dcPctPlantable.DefaultCellStyle.DataSourceNullValue = (object) PctMidRange.PRINV;
      }
      string str;
      if (year.Unit == YearUnit.English)
      {
        str = i_Tree_Eco_v6.Resources.Strings.UnitAcresAbbr;
        this.dcSize.DecimalPlaces = 2;
        this.dcSize.DefaultCellStyle.Format = "0.00";
      }
      else
      {
        str = i_Tree_Eco_v6.Resources.Strings.UnitHectaresAbbr;
        this.dcSize.DecimalPlaces = 4;
        this.dcSize.DefaultCellStyle.Format = "0.0000";
      }
      this.dcSize.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) i_Tree_Eco_v6.Resources.Strings.Size, (object) str);
      if (this.m_plots != null)
      {
        DataBindingList<Plot> dataBindingList = new DataBindingList<Plot>(this.m_plots);
        dataBindingList.Sortable = true;
        dataBindingList.AddComparer<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Strata), (IComparer) new PropertyComparer<Strata>((Func<Strata, object>) (s => (object) s.Description)));
        this.dgPlots.DataSource = (object) dataBindingList;
        this.dgPlots.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
        dataBindingList.AddingNew += new AddingNewEventHandler(this.Plots_AddingNew);
        dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.Plots_BeforeRemove);
        dataBindingList.ListChanged += new ListChangedEventHandler(this.Plots_ListChanged);
      }
      this.OnRequestRefresh();
    }

    private void OnPlotSelectionChanged(int row)
    {
      if (this.PlotSelectionChanged == null)
        return;
      if (this.dgPlots.Rows[row].DataBoundItem is Plot plot && plot.IsTransient)
        plot = (Plot) null;
      PlotEventArgs plotEventArgs = new PlotEventArgs()
      {
        Plot = plot
      };
      foreach (Delegate invocation in this.PlotSelectionChanged.GetInvocationList())
      {
        if (invocation.Target is Control target && target.InvokeRequired)
          target.Invoke(invocation, (object) this, (object) plotEventArgs);
        else
          invocation.DynamicInvoke((object) this, (object) plotEventArgs);
      }
    }

    private void OnPlotValidating(CancelEventArgs args)
    {
      if (this.PlotValidating == null)
        return;
      foreach (Delegate invocation in this.PlotValidating.GetInvocationList())
      {
        if (invocation.Target is Control target && target.InvokeRequired)
          target.Invoke(invocation, (object) this, (object) args);
        else
          invocation.DynamicInvoke((object) this, (object) args);
      }
    }

    private void dgPlots_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dgPlots.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn != this.dcId || e.Value == null || !(this.dgPlots.DataSource is DataBindingList<Plot> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      Plot plot = dataSource[e.RowIndex];
      string str = string.Format("{0} {1}: {2} {3}", (object) v6Strings.Plot_SingularName, (object) plot.Id, (object) plot.Trees.Count, plot.Trees.Count > 1 ? (object) v6Strings.Tree_PluralName : (object) v6Strings.Tree_SingularName);
      if (this.Year.RecordShrub)
        str += string.Format("; {0} {1}", (object) plot.Shrubs.Count, plot.Shrubs.Count > 1 ? (object) v6Strings.Shrub_PluralName : (object) v6Strings.Shrub_SingularName);
      this.dgPlots.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = str;
    }

    private void dgPlots_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgPlots_EditingControlShowing(
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

    private void dgPlots_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
    {
      DataGridViewCell currentCell = this.dgPlots.CurrentCell;
      if (currentCell == null)
        return;
      DataGridViewColumn owningColumn = currentCell.OwningColumn;
      if (!(owningColumn is DataGridViewComboBoxColumn))
        return;
      object dataSourceNullValue = owningColumn.DefaultCellStyle.DataSourceNullValue;
      if (!string.Empty.Equals(e.Value) || dataSourceNullValue == null)
        return;
      e.Value = dataSourceNullValue;
      e.ParsingApplied = true;
    }

    public Eco.Util.ImportSpec ImportSpec()
    {
      Year year = this.Year;
      Eco.Util.ImportSpec<Plot> importSpec1 = new Eco.Util.ImportSpec<Plot>();
      importSpec1.RecordCount = this.m_plots.Count;
      Eco.Util.ImportSpec importSpec2 = (Eco.Util.ImportSpec) importSpec1;
      List<FieldSpec> fieldsSpecs = importSpec2.FieldsSpecs;
      fieldsSpecs.Add(new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Strata), this.dcStrata.HeaderText, true).AddFormat((FieldFormat) new FieldFormat<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Description))).AddFormat((FieldFormat) new FieldFormat<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Abbreviation))).AddFormat((FieldFormat) new FieldFormat<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => (object) s.Id))).SetData(this.dcStrata.DataSource, TypeHelper.NameOf<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Self))));
      if (year.RecordPlotAddress)
        fieldsSpecs.Add((FieldSpec) new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Address), this.dcAddress.HeaderText, false));
      if (year.RecordGPS)
      {
        fieldsSpecs.Add((FieldSpec) new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Latitude), this.dcLatitude.HeaderText, false));
        fieldsSpecs.Add((FieldSpec) new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Longitude), this.dcLongitude.HeaderText, false));
      }
      fieldsSpecs.Add((FieldSpec) new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Date), this.dcDate.HeaderText, false));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Crew), this.dcCrew.HeaderText, false));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.ContactInfo), this.dcContactInfo.HeaderText, false));
      fieldsSpecs.Add(new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Size), this.dcSize.HeaderText, false).SetDefaultValue((object) (float) (this.Year.Unit == YearUnit.English ? 0.10000000149011612 : 0.040468599647283554)));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Photo), this.dcPhoto.HeaderText, false));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Stake), this.dcStake.HeaderText, false));
      fieldsSpecs.Add(new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.PercentTreeCover), this.dcPctTree.HeaderText, false).SetNullValue((object) PctMidRange.PRINV));
      if (year.RecordPercentShrub)
        fieldsSpecs.Add(new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.PercentShrubCover), this.dcPctShrub.HeaderText, false).SetNullValue((object) PctMidRange.PRINV));
      if (year.RecordPlantableSpace)
        fieldsSpecs.Add(new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.PercentPlantable), this.dcPctPlantable.HeaderText, false).SetNullValue((object) PctMidRange.PRINV));
      fieldsSpecs.Add(new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.PercentMeasured), this.dcPctMeasured.HeaderText, false).SetDefaultValue((object) 100).AddConstraint(new FieldConstraint((AConstraint) new AndConstraint((AConstraint) new GtEqConstraint((object) 0), (AConstraint) new LtEqConstraint((object) 100)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRange, (object) this.dcPctMeasured.HeaderText, (object) 0.ToString("P0"), (object) 100.ToString("P0")))));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Comments), this.dcComments.HeaderText, false));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.IsComplete), this.dcIsComplete.HeaderText, false));
      return importSpec2;
    }

    public Task ImportData(
      IList data,
      IProgress<ProgressEventArgs> progress,
      CancellationToken ct)
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      return Task.Factory.StartNew<IList<Plot>>((Func<IList<Plot>>) (() => this.DoImport(data, progress, ct)), ct, TaskCreationOptions.None, Program.Session.Scheduler).ContinueWith((System.Action<Task<IList<Plot>>>) (t =>
      {
        if (this.Year == null || t.IsCanceled || t.IsFaulted)
          return;
        IList<Plot> result = t.Result;
        lock (this.Session)
        {
          DataBindingList<Plot> dataSource = this.dgPlots.DataSource as DataBindingList<Plot>;
          foreach (Plot plot in (IEnumerable<Plot>) result)
            dataSource.Add(plot);
        }
      }), scheduler);
    }

    private IList<Plot> DoImport(
      IList data,
      IProgress<ProgressEventArgs> progress,
      CancellationToken ct)
    {
      int num1 = this.NextPlotId();
      IList<Plot> plotList = (IList<Plot>) new List<Plot>();
      lock (this.Session)
      {
        int count = data.Count;
        int num2 = Math.Max(Math.Min(count / 100, 1000), 1);
        int num3 = 0;
        ITransaction transaction1 = this.Session.BeginTransaction();
        IList<Guid> guidList = (IList<Guid>) new List<Guid>();
        try
        {
          foreach (object obj in (IEnumerable) data)
          {
            ct.ThrowIfCancellationRequested();
            if (obj is Plot plot)
            {
              plot.Id = num1++;
              plot.Year = this.Year;
              plot.ProjectLocation = this.m_projectLocation;
              this.Session.Persist((object) plot);
              guidList.Add(plot.Guid);
              plotList.Add(plot);
              ++num3;
              if (num3 % num2 == 0)
              {
                transaction1.Commit();
                transaction1.Dispose();
                this.Session.Flush();
                this.Session.Clear();
                transaction1 = this.Session.BeginTransaction();
                progress.Report(new ProgressEventArgs(i_Tree_Eco_v6.Resources.Strings.MsgImporting, count / num2, num3 / num2));
              }
            }
          }
          transaction1.Commit();
          transaction1.Dispose();
        }
        catch (Exception ex) when (ex is OperationCanceledException || ex is HibernateException)
        {
          transaction1.Rollback();
          transaction1.Dispose();
          ITransaction transaction2 = this.Session.BeginTransaction();
          for (int index = 0; index < num3 / num2 * num2; ++index)
          {
            this.Session.Delete((object) this.Session.Load<Plot>((object) guidList[index]));
            if (index % num2 == 0)
            {
              transaction2.Commit();
              transaction2.Dispose();
              this.Session.Flush();
              transaction2 = this.Session.BeginTransaction();
              progress.Report(new ProgressEventArgs(i_Tree_Eco_v6.Resources.Strings.MsgCanceling, count / num2, (num3 - 1) / num2));
            }
          }
          transaction2.Commit();
          transaction2.Dispose();
        }
        ct.ThrowIfCancellationRequested();
      }
      return plotList;
    }

    protected override bool ShowHelpMsg => false;

    protected override bool ShowReportWarning => false;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PlotsContentForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      this.dgPlots = new DataGridView();
      this.dcId = new DataGridViewTextBoxColumn();
      this.dcStrata = new DataGridViewComboBoxColumn();
      this.dcAddress = new DataGridViewTextBoxColumn();
      this.dcLatitude = new DataGridViewTextBoxColumn();
      this.dcLongitude = new DataGridViewTextBoxColumn();
      this.dcDate = new DataGridViewNullableDateTimeColumn();
      this.dcCrew = new DataGridViewTextBoxColumn();
      this.dcContactInfo = new DataGridViewTextBoxColumn();
      this.dcSize = new DataGridViewNumericTextBoxColumn();
      this.dcPhoto = new DataGridViewTextBoxColumn();
      this.dcStake = new DataGridViewCheckBoxColumn();
      this.dcPctTree = new DataGridViewComboBoxColumn();
      this.dcPctShrub = new DataGridViewComboBoxColumn();
      this.dcPctPlantable = new DataGridViewComboBoxColumn();
      this.dcPctMeasured = new DataGridViewNumericTextBoxColumn();
      this.dcComments = new DataGridViewTextBoxColumn();
      this.dcIsComplete = new DataGridViewCheckBoxColumn();
      ((ISupportInitialize) this.dgPlots).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgPlots.AllowUserToAddRows = false;
      this.dgPlots.AllowUserToDeleteRows = false;
      this.dgPlots.AllowUserToResizeRows = false;
      this.dgPlots.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgPlots.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgPlots.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgPlots.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcStrata, (DataGridViewColumn) this.dcAddress, (DataGridViewColumn) this.dcLatitude, (DataGridViewColumn) this.dcLongitude, (DataGridViewColumn) this.dcDate, (DataGridViewColumn) this.dcCrew, (DataGridViewColumn) this.dcContactInfo, (DataGridViewColumn) this.dcSize, (DataGridViewColumn) this.dcPhoto, (DataGridViewColumn) this.dcStake, (DataGridViewColumn) this.dcPctTree, (DataGridViewColumn) this.dcPctShrub, (DataGridViewColumn) this.dcPctPlantable, (DataGridViewColumn) this.dcPctMeasured, (DataGridViewColumn) this.dcComments, (DataGridViewColumn) this.dcIsComplete);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgPlots.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgPlots, "dgPlots");
      this.dgPlots.EnableHeadersVisualStyles = false;
      this.dgPlots.MultiSelect = false;
      this.dgPlots.Name = "dgPlots";
      this.dgPlots.ReadOnly = true;
      this.dgPlots.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgPlots.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgPlots.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgPlots.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgPlots.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgPlots_CellErrorTextNeeded);
      this.dgPlots.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgPlots_CellFormatting);
      this.dgPlots.CellParsing += new DataGridViewCellParsingEventHandler(this.dgPlots_CellParsing);
      this.dgPlots.CurrentCellDirtyStateChanged += new EventHandler(this.dgPlots_CurrentCellDirtyStateChanged);
      this.dgPlots.DataError += new DataGridViewDataErrorEventHandler(this.dgPlots_DataError);
      this.dgPlots.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgPlots_EditingControlShowing);
      this.dgPlots.RowValidated += new DataGridViewCellEventHandler(this.dgPlots_RowValidated);
      this.dgPlots.RowValidating += new DataGridViewCellCancelEventHandler(this.dgPlots_RowValidating);
      this.dgPlots.Scroll += new ScrollEventHandler(this.dgPlots_Scroll);
      this.dgPlots.SelectionChanged += new EventHandler(this.dgPlots_SelectionChanged);
      this.dgPlots.Sorted += new EventHandler(this.dgPlots_Sorted);
      this.dcId.DataPropertyName = "Id";
      this.dcId.Frozen = true;
      componentResourceManager.ApplyResources((object) this.dcId, "dcId");
      this.dcId.Name = "dcId";
      this.dcId.ReadOnly = true;
      this.dcStrata.DataPropertyName = "Strata";
      this.dcStrata.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcStrata, "dcStrata");
      this.dcStrata.Name = "dcStrata";
      this.dcStrata.ReadOnly = true;
      this.dcStrata.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcAddress.DataPropertyName = "Address";
      componentResourceManager.ApplyResources((object) this.dcAddress, "dcAddress");
      this.dcAddress.MaxInputLength = 100;
      this.dcAddress.Name = "dcAddress";
      this.dcAddress.ReadOnly = true;
      this.dcLatitude.DataPropertyName = "Latitude";
      componentResourceManager.ApplyResources((object) this.dcLatitude, "dcLatitude");
      this.dcLatitude.Name = "dcLatitude";
      this.dcLatitude.ReadOnly = true;
      this.dcLongitude.DataPropertyName = "Longitude";
      componentResourceManager.ApplyResources((object) this.dcLongitude, "dcLongitude");
      this.dcLongitude.Name = "dcLongitude";
      this.dcLongitude.ReadOnly = true;
      this.dcDate.CustomFormat = (string) null;
      this.dcDate.DataPropertyName = "Date";
      this.dcDate.DateFormat = DateTimePickerFormat.Short;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcDate.DefaultCellStyle = gridViewCellStyle3;
      componentResourceManager.ApplyResources((object) this.dcDate, "dcDate");
      this.dcDate.Name = "dcDate";
      this.dcDate.ReadOnly = true;
      this.dcDate.Resizable = DataGridViewTriState.True;
      this.dcCrew.DataPropertyName = "Crew";
      componentResourceManager.ApplyResources((object) this.dcCrew, "dcCrew");
      this.dcCrew.MaxInputLength = 100;
      this.dcCrew.Name = "dcCrew";
      this.dcCrew.ReadOnly = true;
      this.dcContactInfo.DataPropertyName = "ContactInfo";
      componentResourceManager.ApplyResources((object) this.dcContactInfo, "dcContactInfo");
      this.dcContactInfo.MaxInputLength = (int) byte.MaxValue;
      this.dcContactInfo.Name = "dcContactInfo";
      this.dcContactInfo.ReadOnly = true;
      this.dcSize.DataPropertyName = "Size";
      this.dcSize.DecimalPlaces = 1;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle4.Format = "0.0";
      this.dcSize.DefaultCellStyle = gridViewCellStyle4;
      this.dcSize.Format = "#.#";
      this.dcSize.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcSize, "dcSize");
      this.dcSize.Name = "dcSize";
      this.dcSize.ReadOnly = true;
      this.dcSize.Resizable = DataGridViewTriState.True;
      this.dcSize.Signed = false;
      this.dcPhoto.DataPropertyName = "Photo";
      componentResourceManager.ApplyResources((object) this.dcPhoto, "dcPhoto");
      this.dcPhoto.MaxInputLength = 100;
      this.dcPhoto.Name = "dcPhoto";
      this.dcPhoto.ReadOnly = true;
      this.dcStake.DataPropertyName = "Stake";
      componentResourceManager.ApplyResources((object) this.dcStake, "dcStake");
      this.dcStake.Name = "dcStake";
      this.dcStake.ReadOnly = true;
      this.dcStake.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctTree.DataPropertyName = "PercentTreeCover";
      this.dcPctTree.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcPctTree, "dcPctTree");
      this.dcPctTree.Name = "dcPctTree";
      this.dcPctTree.ReadOnly = true;
      this.dcPctTree.Resizable = DataGridViewTriState.True;
      this.dcPctTree.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctShrub.DataPropertyName = "PercentShrubCover";
      this.dcPctShrub.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcPctShrub, "dcPctShrub");
      this.dcPctShrub.Name = "dcPctShrub";
      this.dcPctShrub.ReadOnly = true;
      this.dcPctShrub.Resizable = DataGridViewTriState.True;
      this.dcPctShrub.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctPlantable.DataPropertyName = "PercentPlantable";
      this.dcPctPlantable.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcPctPlantable, "dcPctPlantable");
      this.dcPctPlantable.Name = "dcPctPlantable";
      this.dcPctPlantable.ReadOnly = true;
      this.dcPctPlantable.Resizable = DataGridViewTriState.True;
      this.dcPctPlantable.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctMeasured.DataPropertyName = "PercentMeasured";
      this.dcPctMeasured.DecimalPlaces = 0;
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcPctMeasured.DefaultCellStyle = gridViewCellStyle5;
      this.dcPctMeasured.Format = "#;-#";
      this.dcPctMeasured.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcPctMeasured, "dcPctMeasured");
      this.dcPctMeasured.Name = "dcPctMeasured";
      this.dcPctMeasured.ReadOnly = true;
      this.dcPctMeasured.Resizable = DataGridViewTriState.True;
      this.dcPctMeasured.Signed = true;
      this.dcComments.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcComments.DataPropertyName = "Comments";
      componentResourceManager.ApplyResources((object) this.dcComments, "dcComments");
      this.dcComments.MaxInputLength = 1073741823;
      this.dcComments.Name = "dcComments";
      this.dcComments.ReadOnly = true;
      this.dcIsComplete.DataPropertyName = "IsComplete";
      componentResourceManager.ApplyResources((object) this.dcIsComplete, "dcIsComplete");
      this.dcIsComplete.Name = "dcIsComplete";
      this.dcIsComplete.ReadOnly = true;
      this.dcIsComplete.SortMode = DataGridViewColumnSortMode.Automatic;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.dgPlots);
      this.Name = nameof (PlotsContentForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgPlots, 0);
      ((ISupportInitialize) this.dgPlots).EndInit();
      this.ResumeLayout(false);
    }
  }
}
