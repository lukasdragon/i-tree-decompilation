// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ImportDataForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using CsvHelper;
using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ImportDataForm : Form
  {
    private ImportSource m_importSource;
    private ImportSpec m_importSpec;
    private bool m_bEditFieldSource;
    private bool m_bEditFieldType;
    private string m_curSrcColumn;
    private RecordsetView m_rvImport;
    private Dictionary<string, SortedSet<object>> m_dImportColumnValues;
    private FieldMapList m_mapList;
    private Dictionary<Type, Dictionary<string, Func<object, object>>> m_propCache;
    private TaskManager m_taskManager;
    private DataAnalyzer m_dataAnalyzer;
    private ProgramSession m_ps;
    private Dictionary<bool, string> m_dBoolean;
    private IContainer components;
    private Wizard wizImport;
    private WizardPage wpDataSource;
    private TableLayoutPanel tableLayoutPanel1;
    private Label lblSource;
    private Button btnBrowse;
    private Label lblFileName;
    private DataGridView dgSourceData;
    private WizardPage wpMatchFields;
    private ErrorProvider ep;
    private TableLayoutPanel tableLayoutPanel2;
    private DataGridView dgUserData;
    private ComboBox cboFieldType;
    private Label lblFieldType;
    private ComboBox cboEcoField;
    private Label lblEcoField;
    private WrappingCheckBox chkNeedMapValue;
    private Label label4;
    private TextBox txtSourceColumn;
    private Label label1;
    private TableLayoutPanel tableLayoutPanel3;
    private Label lblSelectFile;
    private WizardPage wpMatchValues;
    private SplitContainer splitContainer1;
    private Label label2;
    private Label lblSourceData;
    private DataGridView dgSourceFields;
    private DataGridView dgMapValues;
    private WizardPage wpProcessData;
    private Label lblReview;
    private TableLayoutPanel tableLayoutPanel4;
    private TableLayoutPanel tableLayoutPanel5;
    private TableLayoutPanel tableLayoutPanel6;
    private Label lblRecordsAnalyzed;
    private Label lblRecordsNotImported;
    private Label lblRecordsToImport;
    private Label lblRecordsToImportHdr;
    private Label lblNumRecordsAnalyzed;
    private Label lblNumRecordsToImport;
    private Label lblNumRecordsNotImported;
    private DataGridView dgImportedData;
    private Label lblFinish;
    private TextBox txtFileName;
    private ComboBox cboSource;
    private CheckBox chkHasHeaders;
    private WizardPage wpIntro;
    private RichTextLabel rtlIntro;
    private RichTextLabel richTextLabel1;
    private Label label3;
    private Label label5;
    private Label lblNumExistingRecords;
    private TabControl tcRecords;
    private TabPage tpImported;
    private TabPage tpRejected;
    private DataGridView dgRejectedData;
    private Button btnSaveRejects;
    private DataGridViewTextBoxColumn dcRejectedRow;
    private DataGridViewTextBoxColumn dcRejectedReason;
    private DataGridViewTextBoxColumn dcUserValue;
    private DataGridViewTextBoxColumn dcEcoValueAlt;
    private DataGridViewComboBoxColumn dcEcoValue;
    private DataGridViewFilteredComboBoxColumn dcEcoValueSearch;

    public ImportDataForm(ImportSpec spec)
    {
      this.m_importSpec = spec;
      this.m_propCache = new Dictionary<Type, Dictionary<string, Func<object, object>>>();
      this.m_dBoolean = new Dictionary<bool, string>()
      {
        {
          true,
          i_Tree_Eco_v6.Resources.Strings.Yes
        },
        {
          false,
          i_Tree_Eco_v6.Resources.Strings.No
        }
      };
      this.InitializeComponent();
      this.Disposed += new EventHandler(this.ImportDataForm_Disposed);
      this.dgSourceData.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgSourceData.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgSourceData.DoubleBuffered(true);
      this.dgUserData.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgUserData.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgUserData.DoubleBuffered(true);
      this.dgSourceFields.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgSourceFields.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgSourceFields.DoubleBuffered(true);
      this.dgMapValues.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgMapValues.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgMapValues.AutoGenerateColumns = false;
      this.dgMapValues.DoubleBuffered(true);
      this.dgImportedData.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgImportedData.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgImportedData.AutoGenerateColumns = false;
      this.dgImportedData.DoubleBuffered(true);
      this.dgRejectedData.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgRejectedData.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgRejectedData.AutoGenerateColumns = false;
      this.dgRejectedData.DoubleBuffered(true);
      this.m_taskManager = new TaskManager(new WaitCursor((Form) this));
      this.m_ps = ProgramSession.GetInstance();
    }

    private void ImportDataForm_Disposed(object sender, EventArgs e)
    {
      if (this.m_rvImport != null)
      {
        this.dgSourceData.DataSource = (object) null;
        this.m_rvImport.Dispose();
        this.m_rvImport = (RecordsetView) null;
      }
      if (this.m_importSource == null)
        return;
      this.m_importSource.Dispose();
      this.m_importSource = (ImportSource) null;
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Filter = string.Join("|", new string[4]
        {
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAllFormats, (object) string.Join(";", new string[3]
          {
            string.Join(";", Eco.Util.Properties.Settings.Default.ExtCSV),
            string.Join(";", Eco.Util.Properties.Settings.Default.ExtExcel),
            string.Join(";", Eco.Util.Properties.Settings.Default.ExtAccess)
          })),
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterCSV, (object) string.Join(";", Eco.Util.Properties.Settings.Default.ExtCSV)),
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterExcel, (object) string.Join(";", Eco.Util.Properties.Settings.Default.ExtExcel)),
          string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFilter, (object) i_Tree_Eco_v6.Resources.Strings.FilterAccess, (object) string.Join(";", Eco.Util.Properties.Settings.Default.ExtAccess))
        });
        openFileDialog.ShowHelp = false;
        if (openFileDialog.ShowDialog() != DialogResult.OK)
          return;
        this.txtFileName.Text = openFileDialog.FileName;
        this.m_importSource = new ImportSource(openFileDialog.FileName);
        this.InitSubDataSource();
      }
    }

    public IList ImportedData => this.m_dataAnalyzer.ImportedData;

    private void InitSubDataSource()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew<List<DataSource>>((Func<List<DataSource>>) (() => this.m_importSource.DataSources()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<List<DataSource>>>) (t =>
      {
        this.cboSource.DataSource = (object) null;
        this.dgSourceData.DataSource = (object) null;
        this.m_rvImport = (RecordsetView) null;
        if (t.IsFaulted)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFileOpen, (object) Path.GetFileName(this.m_importSource.FileName)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          List<DataSource> result = t.Result;
          switch (this.m_importSource.FileFormat)
          {
            case FileFormat.CSV:
              this.cboSource.Visible = false;
              this.lblSource.Visible = false;
              this.chkHasHeaders.Visible = true;
              this.chkHasHeaders.Checked = this.m_importSource.HasHeader;
              this.LoadData();
              break;
            case FileFormat.Excel:
              this.lblSource.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtLabel, (object) i_Tree_Eco_v6.Resources.Strings.DataSourceExcel);
              this.cboSource.DataSource = (object) result;
              this.lblSource.Visible = true;
              this.cboSource.Visible = true;
              this.chkHasHeaders.Visible = true;
              this.chkHasHeaders.Checked = this.m_importSource.HasHeader;
              break;
            case FileFormat.Access:
              this.lblSource.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtLabel, (object) i_Tree_Eco_v6.Resources.Strings.DataSourceAccess);
              this.cboSource.DataSource = (object) result;
              this.lblSource.Visible = true;
              this.cboSource.Visible = true;
              this.chkHasHeaders.Visible = false;
              break;
          }
          this.dgSourceData.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
        }
      }), scheduler));
    }

    private void wizImport_Cancel(object sender, EventArgs e)
    {
      if (MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.MsgConfirmCancel, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void chkHasHeaders_CheckedChanged(object sender, EventArgs e)
    {
      if (this.m_importSource == null)
        return;
      this.m_importSource.HasHeader = this.chkHasHeaders.Checked;
      this.LoadData();
    }

    private void cboSource_SelectedIndexChanged(object sender, EventArgs e) => this.LoadData();

    private void LoadData()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      if (this.m_rvImport != null)
      {
        this.dgSourceData.DataSource = (object) null;
        this.m_rvImport.Dispose();
        this.m_rvImport = (RecordsetView) null;
      }
      DataSource ds = (DataSource) null;
      if (this.cboSource.DataSource != null)
        ds = this.cboSource.SelectedItem as DataSource;
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.InitDataSource(ds)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (t.IsFaulted)
        {
          string text = t.Exception.InnerException.Message;
          if (this.m_importSource.FileFormat == FileFormat.CSV && text.Contains("''"))
            text = text.Replace("''", string.Format("'{0}'", (object) this.m_importSource.FileName));
          int num = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          switch (this.m_importSource.FileFormat)
          {
            case FileFormat.CSV:
              this.txtFileName.Text = string.Empty;
              this.m_importSource = (ImportSource) null;
              break;
            case FileFormat.Excel:
            case FileFormat.Access:
              this.cboSource.SelectedIndex = -1;
              break;
          }
        }
        else
        {
          this.dgSourceData.DataSource = (object) this.m_rvImport;
          foreach (DataGridViewColumn column in (BaseCollection) this.dgSourceData.Columns)
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
          this.dgSourceData.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
          this.m_mapList = new FieldMapList(this.m_importSpec.FieldsSpecs);
        }
      }), scheduler));
    }

    private void InitDataSource(DataSource ds)
    {
      switch (this.m_importSource.FileFormat)
      {
        case FileFormat.CSV:
          this.m_rvImport = this.m_importSource.GetData(Path.GetFileName(this.m_importSource.FileName));
          break;
        case FileFormat.Excel:
        case FileFormat.Access:
          if (ds == null)
            break;
          this.m_rvImport = this.m_importSource.GetData(ds.Name);
          break;
      }
    }

    private void chkRowHeaders_CheckedChanged(object sender, EventArgs e) => this.InitSubDataSource();

    private void txtFileName_Validating(object sender, CancelEventArgs e)
    {
      string text = this.txtFileName.Text;
      bool hasError = string.IsNullOrEmpty(text) || !File.Exists(text);
      this.ep.SetError((Control) this.btnBrowse, hasError, i_Tree_Eco_v6.Resources.Strings.ErrImportFileRequired);
      if (!hasError)
      {
        hasError |= this.m_rvImport == null;
        this.ep.SetError((Control) this.btnBrowse, hasError, i_Tree_Eco_v6.Resources.Strings.ErrFileDataSource);
      }
      e.Cancel = hasError;
    }

    private void cboSource_Validating(object sender, CancelEventArgs e)
    {
      bool hasError = this.cboSource.DataSource != null && this.cboSource.SelectedItem == null;
      this.ep.SetError((Control) this.cboSource, hasError, i_Tree_Eco_v6.Resources.Strings.ErrOptionRequired);
      e.Cancel = hasError;
    }

    private void wizImport_NextPage(object sender, PageEventArgs e)
    {
      WizardPage currentPage = this.wizImport.CurrentPage;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      WizardPage wpDataSource = this.wpDataSource;
      if (currentPage == wpDataSource)
        this.wpDataSource_Validating(sender, (CancelEventArgs) e);
      if (e.Cancel)
        return;
      if (e.NextPage == this.wpMatchFields)
      {
        this.dgUserData.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
        this.dgUserData.DataSource = this.dgSourceData.DataSource;
        this.dgUserData.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
        for (int index = 0; index < this.dgUserData.ColumnCount; ++index)
        {
          DataGridViewColumn column = this.dgUserData.Columns[index];
          column.Name = column.HeaderText;
          column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        this.dgUserData.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      }
      if (e.NextPage == this.wpMatchValues)
      {
        if (this.m_mapList.GetMatchedFieldsToMap().Count > 0)
        {
          Progress<ProgressEventArgs> progress = new Progress<ProgressEventArgs>();
          CancellationTokenSource cts = new CancellationTokenSource();
          ProcessingForm processingForm = new ProcessingForm();
          Task<DataTable> task = Task.Factory.StartNew<DataTable>((Func<DataTable>) (() =>
          {
            this.BuildFieldMaps((IProgress<ProgressEventArgs>) progress, cts.Token);
            return this.LoadMatchedFieldsToMap();
          }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler);
          Task<DataTable> t = task;
          Progress<ProgressEventArgs> progress2 = progress;
          CancellationTokenSource token = cts;
          if (processingForm.ShowDialog((IWin32Window) this, (Task) t, progress2, token) == DialogResult.OK)
          {
            this.dgSourceFields.DataSource = (object) task.Result;
            this.dgSourceFields.Columns["UserField"].HeaderText = i_Tree_Eco_v6.Resources.Strings.YourField;
            this.dgSourceFields.Columns["EcoField"].HeaderText = i_Tree_Eco_v6.Resources.Strings.EcoField;
            this.dgSourceFields.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
          }
          else
            e.Cancel = true;
        }
        else
          e.NextPage = this.wpProcessData;
      }
      if (e.NextPage != this.wpProcessData)
        return;
      ProcessingForm processingForm1 = new ProcessingForm();
      Progress<ProgressEventArgs> progress1 = new Progress<ProgressEventArgs>();
      CancellationTokenSource cts1 = new CancellationTokenSource();
      Task task1 = Task.Factory.StartNew((System.Action) (() =>
      {
        if (this.m_mapList.GetMatchedFieldsToMap().Count == 0)
          this.BuildFieldMaps((IProgress<ProgressEventArgs>) progress1, cts1.Token);
        this.ProcessData(progress1, cts1.Token);
      }), cts1.Token, TaskCreationOptions.None, this.m_ps.Scheduler);
      Task t1 = task1;
      Progress<ProgressEventArgs> progress3 = progress1;
      CancellationTokenSource token1 = cts1;
      DialogResult dialogResult = processingForm1.ShowDialog((IWin32Window) this, t1, progress3, token1);
      this.m_taskManager.Add(task1.ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed || t.IsCanceled || t.IsFaulted)
          return;
        int count1 = this.m_rvImport.Count;
        int count2 = this.m_dataAnalyzer.RejectedData.Count;
        int count3 = this.m_dataAnalyzer.ImportedData.Count;
        this.lblNumExistingRecords.Text = this.m_importSpec.RecordCount.ToString("N0");
        this.lblNumRecordsAnalyzed.Text = count1.ToString("N0");
        this.lblNumRecordsNotImported.Text = count2.ToString("N0");
        this.lblNumRecordsToImport.Text = count3.ToString("N0");
        this.dgRejectedData.DataSource = (object) null;
        this.dgImportedData.DataSource = (object) null;
        this.dgImportedData.Columns.Clear();
        foreach (FieldMap matchedField in this.m_mapList.GetMatchedFields())
        {
          FieldFormat activeFormat = matchedField.ActiveFormat;
          FieldSpec fieldSpec = matchedField.FieldSpec;
          if (matchedField.Data != null)
            this.dgImportedData.Columns.Add((DataGridViewColumn) new DataGridViewComboBoxColumn()
            {
              DataPropertyName = fieldSpec.Field,
              DisplayMember = activeFormat.Property,
              ValueMember = matchedField.ValueMember,
              DataSource = matchedField.Data,
              HeaderText = fieldSpec.Description,
              DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing,
              DefaultCellStyle = {
                DataSourceNullValue = fieldSpec.NullValue
              }
            });
          else if (activeFormat.FormatType.IsEnum)
            this.dgImportedData.Columns.Add((DataGridViewColumn) new DataGridViewComboBoxColumn()
            {
              DataPropertyName = matchedField.FieldSpec.Field,
              DisplayMember = "Value",
              ValueMember = "Key",
              DataSource = (object) new BindingSource((object) EnumHelper.ConvertToDictionary(activeFormat.FormatType), (string) null),
              HeaderText = fieldSpec.Description,
              DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing,
              DefaultCellStyle = {
                DataSourceNullValue = fieldSpec.NullValue
              }
            });
          else if (activeFormat.FormatType == typeof (bool) || activeFormat.FormatType == typeof (bool?))
          {
            this.dgImportedData.Columns.Add((DataGridViewColumn) new DataGridViewCheckBoxColumn()
            {
              DataPropertyName = fieldSpec.Field,
              HeaderText = fieldSpec.Description,
              SortMode = DataGridViewColumnSortMode.Automatic,
              DefaultCellStyle = {
                DataSourceNullValue = fieldSpec.NullValue
              }
            });
          }
          else
          {
            DataGridViewTextBoxColumn viewTextBoxColumn = new DataGridViewTextBoxColumn();
            viewTextBoxColumn.DataPropertyName = fieldSpec.Field;
            viewTextBoxColumn.HeaderText = fieldSpec.Description;
            viewTextBoxColumn.DefaultCellStyle.DataSourceNullValue = fieldSpec.NullValue;
            if (activeFormat.FormatType != typeof (string))
              viewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dgImportedData.Columns.Add((DataGridViewColumn) viewTextBoxColumn);
          }
        }
        this.dgImportedData.DataSource = (object) this.m_dataAnalyzer.ImportedData;
        this.dgImportedData.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
        this.dgRejectedData.DataSource = (object) this.m_dataAnalyzer.RejectedData;
        this.dgRejectedData.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
      }), scheduler));
      e.Cancel = dialogResult != DialogResult.OK;
    }

    private void ProcessData(Progress<ProgressEventArgs> progress, CancellationToken ct)
    {
      this.m_dataAnalyzer = new DataAnalyzer(this.m_rvImport, this.m_importSpec, this.m_mapList.List);
      this.m_dataAnalyzer.Analyze((IProgress<ProgressEventArgs>) progress, ct);
    }

    private DataTable LoadMatchedFieldsToMap()
    {
      List<FieldMap> matchedFieldsToMap = this.m_mapList.GetMatchedFieldsToMap();
      if (matchedFieldsToMap.Count == 0)
        return (DataTable) null;
      DataTable map = new DataTable();
      map.Columns.Add("UserField", typeof (string));
      map.Columns.Add("EcoField", typeof (string));
      map.Columns.Add("Status", typeof (string));
      for (int index = 0; index < matchedFieldsToMap.Count; ++index)
      {
        FieldMap fieldMap = matchedFieldsToMap[index];
        DataRow row = map.NewRow();
        row["UserField"] = (object) fieldMap.SourceColumn;
        row["EcoField"] = (object) fieldMap.FieldSpec.Description;
        row["Status"] = (object) string.Format(i_Tree_Eco_v6.Resources.Strings.MsgMappingStatus, (object) fieldMap.Map.Count<KeyValuePair<object, object>>((Func<KeyValuePair<object, object>, bool>) (o => o.Value != null)), (object) this.m_dImportColumnValues[fieldMap.SourceColumn].Count);
        map.Rows.Add(row);
      }
      return map;
    }

    private void BuildFieldMaps(IProgress<ProgressEventArgs> progress, CancellationToken ct)
    {
      List<FieldMap> matchedFields = this.m_mapList.GetMatchedFields();
      this.m_dImportColumnValues = new Dictionary<string, SortedSet<object>>();
      for (int index = 0; index < matchedFields.Count; ++index)
        matchedFields[index].Map.Clear();
      int num1 = 0;
      int num2 = Math.Max(this.m_rvImport.Count / 100, 1);
      foreach (RecordsetRow recordsetRow in this.m_rvImport)
      {
        for (int index = 0; index < matchedFields.Count; ++index)
        {
          FieldMap fieldMap = matchedFields[index];
          object obj1 = recordsetRow[fieldMap.SourceColumn];
          if (!fieldMap.Map.ContainsKey(obj1) && obj1 != null)
          {
            FieldFormat activeFormat = fieldMap.ActiveFormat;
            PropertyDescriptor propertyDescriptor = this.m_importSpec.Properties.Find(fieldMap.FieldSpec.Field, true);
            object obj2 = (object) null;
            if (propertyDescriptor != null)
            {
              Type propertyType = propertyDescriptor.PropertyType;
              string str = obj1.ToString();
              if (!string.IsNullOrEmpty(str))
              {
                if (fieldMap.Data != null && activeFormat.Property != null)
                {
                  obj2 = this.TrimmedCaseInsensitveMatchFor(fieldMap.Data as IEnumerable, activeFormat.Property, obj1);
                  if (obj2 == null && fieldMap.NeedsMapped)
                    obj2 = this.BestMatchFor(fieldMap.Data as IEnumerable, activeFormat.Property, obj1);
                }
                else if (propertyType.IsEnum)
                {
                  if (!EnumHelper.TryParse(propertyType, str, out obj2) && fieldMap.NeedsMapped)
                  {
                    object obj3 = this.BestMatchFor((IEnumerable) EnumHelper.ConvertToDictionary(propertyType), "Value", obj1);
                    if (obj3 != null)
                      obj2 = (object) ((KeyValuePair<Enum, string>) obj3).Key;
                  }
                }
                else if (propertyType == typeof (bool) || propertyType == typeof (bool?))
                {
                  if (!TypeHelper.TryConvert(obj1, propertyType, out obj2))
                  {
                    object obj4 = this.PrefixMatchFor((IEnumerable) this.m_dBoolean, "Value", obj1);
                    if (obj4 != null)
                      obj2 = (object) ((KeyValuePair<bool, string>) obj4).Key;
                  }
                }
                else
                  TypeHelper.TryConvert(obj1, propertyType, out obj2);
              }
            }
            fieldMap.Map[obj1] = obj2;
          }
        }
        ct.ThrowIfCancellationRequested();
        if (num1 % num2 == 0)
          progress.Report(new ProgressEventArgs(i_Tree_Eco_v6.Resources.Strings.MsgMappingValues, this.m_rvImport.Count / num2, num1 / num2));
        ++num1;
      }
      for (int index = 0; index < matchedFields.Count; ++index)
      {
        FieldMap fieldMap = matchedFields[index];
        if (fieldMap.NeedsMapped)
          this.m_dImportColumnValues[fieldMap.SourceColumn] = new SortedSet<object>((IEnumerable<object>) fieldMap.Map.Keys, (IComparer<object>) new ObjectComparer());
      }
    }

    private void wizImport_Finished(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void wpDataSource_Validating(object sender, CancelEventArgs e) => e.Cancel = !this.wpDataSource.ValidateChildren();

    private void wpMatchFields_Validating(object sender, CancelEventArgs e)
    {
      if (this.cboFieldType.Visible && this.cboFieldType.SelectedIndex == -1)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrFieldType, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.cboFieldType.Focus();
        e.Cancel = true;
      }
      else
      {
        foreach (FieldMap fieldMap in this.m_mapList.List)
        {
          if (string.IsNullOrEmpty(fieldMap.SourceColumn) && fieldMap.FieldSpec.Required)
          {
            int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrEcoFieldRequired, (object) fieldMap.FieldSpec.Description), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            e.Cancel = true;
            break;
          }
        }
      }
    }

    private void dgUserData_SelectionChanged(object sender, EventArgs e)
    {
      if (this.dgUserData.SelectedColumns.Count != 1)
        return;
      DataGridViewColumn selectedColumn = this.dgUserData.SelectedColumns[0];
      if (selectedColumn.Name == null)
        return;
      string name = selectedColumn.Name;
      this.m_bEditFieldSource = false;
      this.txtSourceColumn.Text = name;
      FieldMap fieldMapFromSource = this.m_mapList.GetFieldMapFromSource(name);
      this.cboEcoField.DisplayMember = "Description";
      this.cboEcoField.DataSource = (object) this.m_mapList.GetUnMatchedFields(name);
      this.cboEcoField.SelectedItem = fieldMapFromSource == null ? (object) FieldMap.Empty : (object) fieldMapFromSource;
      this.m_bEditFieldSource = true;
    }

    private void cboEcoField_SelectedValueChanged(object sender, EventArgs e)
    {
      FieldMap selectedItem = (FieldMap) this.cboEcoField.SelectedItem;
      DataGridViewColumn column = this.dgUserData.Columns[this.txtSourceColumn.Text];
      if (this.m_bEditFieldSource)
      {
        FieldMap fieldMapFromSource = this.m_mapList.GetFieldMapFromSource(this.txtSourceColumn.Text);
        if (fieldMapFromSource != null && selectedItem != fieldMapFromSource)
        {
          fieldMapFromSource.SourceColumn = (string) null;
          fieldMapFromSource.ActiveFormat = (FieldFormat) null;
          fieldMapFromSource.NeedsMapped = false;
        }
      }
      if (selectedItem == null || selectedItem == FieldMap.Empty)
      {
        this.m_bEditFieldType = false;
        this.lblFieldType.Visible = false;
        this.cboFieldType.Visible = false;
        this.cboFieldType.DataSource = (object) null;
        this.m_bEditFieldType = true;
        if (this.m_bEditFieldSource)
          this.SetColumnType(column, selectedItem);
        this.chkNeedMapValue.Visible = false;
      }
      else
      {
        selectedItem.SourceColumn = this.txtSourceColumn.Text;
        FieldFormat activeFormat = selectedItem.ActiveFormat;
        this.m_bEditFieldType = false;
        if (selectedItem.FieldSpec.Formats.Count > 1)
        {
          this.lblFieldType.Visible = true;
          this.cboFieldType.Visible = true;
          this.cboFieldType.DisplayMember = "Name";
          this.cboFieldType.DataSource = (object) selectedItem.FieldSpec.Formats;
          this.cboFieldType.SelectedItem = (object) activeFormat;
          this.chkNeedMapValue.Visible = true;
        }
        else
        {
          this.lblFieldType.Visible = false;
          this.cboFieldType.Visible = false;
          this.cboFieldType.DataSource = (object) null;
          this.chkNeedMapValue.Visible = true;
        }
        this.chkNeedMapValue.Checked = selectedItem.NeedsMapped;
        this.m_bEditFieldType = true;
        this.SetColumnType(column, selectedItem);
      }
    }

    private void cboFieldType_SelectedValueChanged(object sender, EventArgs e)
    {
      FieldMap selectedItem1 = (FieldMap) this.cboEcoField.SelectedItem;
      FieldFormat selectedItem2 = (FieldFormat) this.cboFieldType.SelectedItem;
      if (selectedItem1 == null || !this.m_bEditFieldType)
        return;
      DataGridViewColumn column = this.dgUserData.Columns[this.txtSourceColumn.Text];
      selectedItem1.Map.Clear();
      selectedItem1.ActiveFormat = selectedItem2;
      this.SetColumnType(column, selectedItem1);
    }

    private void dgUserData_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      if (this.cboEcoField.SelectedIndex <= 0 || !this.cboFieldType.Visible || this.cboFieldType.SelectedIndex != -1)
        return;
      int num = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrFieldType, Application.ProductName, MessageBoxButtons.OK);
      this.cboFieldType.Focus();
      e.Cancel = true;
    }

    private void SetColumnType(DataGridViewColumn c, FieldMap fm)
    {
      FieldFormat activeFormat = fm.ActiveFormat;
      c.ValueType = activeFormat == null || !(activeFormat.FormatType != (Type) null) ? typeof (string) : (activeFormat.FormatType.IsEnum || fm.NeedsMapped ? typeof (string) : activeFormat.FormatType);
      this.dgUserData.Refresh();
    }

    private void dgUserData_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      e.ErrorText = string.Empty;
      FieldMap fieldMapFromSource = this.m_mapList.GetFieldMapFromSource(this.dgUserData.Columns[e.ColumnIndex].Name);
      if (fieldMapFromSource == null)
        return;
      FieldFormat activeFormat = fieldMapFromSource.ActiveFormat;
      if (activeFormat == null || fieldMapFromSource.NeedsMapped || !(activeFormat.FormatType != (Type) null))
        return;
      DataGridViewCell dataGridViewCell = this.dgUserData[e.ColumnIndex, e.RowIndex];
      Type formatType = activeFormat.FormatType;
      object val = dataGridViewCell.Value;
      if (!string.IsNullOrEmpty(val.ToString()))
      {
        if (fieldMapFromSource.Data != null && activeFormat.Property != null)
        {
          if (this.DataContains(fieldMapFromSource.Data as IList, activeFormat.Property, val))
            return;
          e.ErrorText = i_Tree_Eco_v6.Resources.Strings.ErrUnmappedValue;
        }
        else if (formatType.IsEnum)
        {
          if (EnumHelper.EnumContains(formatType, val.ToString()))
            return;
          e.ErrorText = i_Tree_Eco_v6.Resources.Strings.ErrUnmappedValue;
        }
        else
        {
          object dest;
          if (!TypeHelper.TryConvert(val, formatType, out dest))
          {
            if (formatType == typeof (bool) || formatType == typeof (bool?))
            {
              if (this.PrefixMatchFor((IEnumerable) this.m_dBoolean, "Value", val) != null)
                return;
              e.ErrorText = ImportDataForm.GetTypeError(formatType);
            }
            else
              e.ErrorText = ImportDataForm.GetTypeError(formatType);
          }
          else
          {
            if (fieldMapFromSource.FieldSpec.Constraints.Count <= 0)
              return;
            foreach (FieldConstraint constraint in fieldMapFromSource.FieldSpec.Constraints)
            {
              if (!constraint.IsValid(dest))
              {
                e.ErrorText = constraint.FormatError((object) i_Tree_Eco_v6.Resources.Strings.Value);
                break;
              }
            }
          }
        }
      }
      else
      {
        if (!fieldMapFromSource.FieldSpec.Required)
          return;
        e.ErrorText = i_Tree_Eco_v6.Resources.Strings.ErrNoValue;
      }
    }

    private static string GetTypeError(Type t)
    {
      if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof (Nullable<>)))
        t = t.GetGenericArguments()[0];
      string str = t.Name;
      switch (Type.GetTypeCode(t))
      {
        case TypeCode.Int16:
        case TypeCode.Int32:
        case TypeCode.Int64:
          str = i_Tree_Eco_v6.Resources.Strings.TypeInteger;
          break;
        case TypeCode.UInt16:
        case TypeCode.UInt32:
        case TypeCode.UInt64:
          str = i_Tree_Eco_v6.Resources.Strings.TypePositiveInteger;
          break;
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          str = i_Tree_Eco_v6.Resources.Strings.TypeDecimal;
          break;
        case TypeCode.DateTime:
          str = i_Tree_Eco_v6.Resources.Strings.TypeDateTime;
          break;
        case TypeCode.String:
          str = i_Tree_Eco_v6.Resources.Strings.TypeString;
          break;
      }
      return string.Format(i_Tree_Eco_v6.Resources.Strings.ErrConvertValue, (object) str);
    }

    private bool DataContains(IList data, string prop, object val) => this.TrimmedCaseInsensitveMatchFor((IEnumerable) data, prop, val) != null;

    private void chkNeedMapValue_CheckedChanged(object sender, EventArgs e)
    {
      FieldMap fieldMapFromSource = this.m_mapList.GetFieldMapFromSource(this.txtSourceColumn.Text);
      if (fieldMapFromSource == null || !this.m_bEditFieldSource)
        return;
      fieldMapFromSource.NeedsMapped = this.chkNeedMapValue.Checked;
      this.SetColumnType(this.dgUserData.Columns[this.txtSourceColumn.Text], fieldMapFromSource);
    }

    private void dgSourceFields_SelectionChanged(object sender, EventArgs e)
    {
      DataGridViewRow currentRow = this.dgSourceFields.CurrentRow;
      DataTable dataSource = this.dgSourceFields.DataSource as DataTable;
      if (currentRow != null && dataSource != null)
      {
        this.m_curSrcColumn = (string) null;
        string str = dataSource.DefaultView[currentRow.Index]["UserField"].ToString();
        SortedSet<object> importColumnValue = this.m_dImportColumnValues[str];
        FieldMap fieldMapFromSource = this.m_mapList.GetFieldMapFromSource(str);
        if (fieldMapFromSource != null)
        {
          FieldFormat activeFormat = fieldMapFromSource.ActiveFormat;
          Type formatType = activeFormat.FormatType;
          this.dgMapValues.DataSource = (object) null;
          this.dgMapValues.Columns.Clear();
          this.dgMapValues.Columns.Add((DataGridViewColumn) this.dcUserValue);
          IList list = (IList) null;
          if (fieldMapFromSource.Data != null && activeFormat.Property != null)
            list = (IList) new BindingSource((object) this.DataToDictionary(fieldMapFromSource.Data as IList, activeFormat.Property), (string) null);
          else if (formatType.IsEnum)
            list = (IList) new BindingSource((object) EnumHelper.ConvertToDictionary(formatType), (string) null);
          else if (formatType == typeof (bool) || formatType == typeof (bool?))
            list = (IList) new BindingSource((object) this.m_dBoolean, (string) null);
          if (list != null)
          {
            if (list.Count < 100)
            {
              this.dcEcoValue.DisplayMember = "Value";
              this.dcEcoValue.ValueMember = "Key";
              this.dcEcoValue.DataSource = (object) list;
              this.dgMapValues.Columns.Add((DataGridViewColumn) this.dcEcoValue);
            }
            else
            {
              this.dcEcoValueSearch.DisplayMember = "Value";
              this.dcEcoValueSearch.ValueMember = "Key";
              this.dcEcoValueSearch.DataSource = (object) list;
              this.dgMapValues.Columns.Add((DataGridViewColumn) this.dcEcoValueSearch);
            }
          }
          else
          {
            this.dcEcoValue.DataSource = (object) null;
            this.dgMapValues.Columns.Add((DataGridViewColumn) this.dcEcoValueAlt);
          }
          this.dgMapValues.RowCount = importColumnValue.Count;
        }
        this.m_curSrcColumn = str;
        this.dgMapValues.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
      }
      else
      {
        this.dgMapValues.RowCount = 0;
        this.m_curSrcColumn = (string) null;
      }
    }

    private IDictionary<object, string> DataToDictionary(IList data, string prop)
    {
      IDictionary<object, string> dictionary = (IDictionary<object, string>) new SortedDictionary<object, string>((IComparer<object>) new PropertyComparer(prop));
      if (data != null)
      {
        foreach (object obj1 in (IEnumerable) data)
        {
          object obj2 = (object) null;
          string str = (string) null;
          Func<object, object> propertyDelegate = this.GetPropertyDelegate(obj1, prop);
          if (propertyDelegate != null)
            obj2 = propertyDelegate(obj1);
          if (obj2 != null)
            str = obj2.ToString();
          dictionary[obj1] = str;
        }
      }
      return dictionary;
    }

    private Func<object, object> GetPropertyDelegate(object o, string prop)
    {
      if (o == null)
        return (Func<object, object>) null;
      Type type = o.GetType();
      Dictionary<string, Func<object, object>> dictionary;
      if (!this.m_propCache.TryGetValue(type, out dictionary))
      {
        dictionary = new Dictionary<string, Func<object, object>>();
        this.m_propCache.Add(type, dictionary);
      }
      Func<object, object> propertyGetter;
      if (!dictionary.TryGetValue(prop, out propertyGetter))
      {
        propertyGetter = TypeHelper.GetPropertyGetter(type, prop);
        dictionary[prop] = propertyGetter;
      }
      return propertyGetter;
    }

    private object TrimmedCaseInsensitveMatchFor(IEnumerable data, string prop, object val)
    {
      object obj1 = (object) null;
      if (data != null && prop != null && val != null)
      {
        string strB = val.ToString().Trim();
        foreach (object o in data)
        {
          Func<object, object> propertyDelegate = this.GetPropertyDelegate(o, prop);
          if (propertyDelegate != null)
          {
            object obj2 = propertyDelegate(o);
            if (obj2 != null && string.Compare(obj2.ToString().Trim(), strB, true) == 0)
              obj1 = o;
          }
        }
      }
      return obj1;
    }

    private object BestMatchFor(IEnumerable data, string prop, object val)
    {
      if (val == null || data == null || prop == null)
        return (object) null;
      string str1 = val.ToString();
      char[] chArray = new char[4]{ ' ', '\t', '\n', '\r' };
      string str2 = Regex.Replace(str1.Trim().ToLower(), "[ \t\n\r]", "");
      object obj1 = (object) null;
      int num1 = -1;
      int num2 = 0;
      foreach (object o in data)
      {
        Func<object, object> propertyDelegate = this.GetPropertyDelegate(o, prop);
        if (propertyDelegate != null)
        {
          object obj2 = propertyDelegate(o);
          if (obj2 != null)
          {
            string str3 = string.Join("", obj2.ToString().Trim().ToLower().Split(chArray));
            if (string.Compare(str3, str2, true) == 0)
            {
              obj1 = o;
              break;
            }
            int maxDistance = (Math.Min(str3.Length, str2.Length) + 1) / 2;
            int num3 = this.LevnshteinDistance(str2, str3, maxDistance, true);
            if (num1 == -1 && num3 <= maxDistance || num3 < num1)
            {
              num2 = str3.Length;
              num1 = num3;
              obj1 = o;
            }
            else if (num3 == num1 && str3.Length < num2)
            {
              num2 = str3.Length;
              obj1 = o;
            }
          }
        }
      }
      return obj1;
    }

    private object PrefixMatchFor(IEnumerable data, string prop, object val)
    {
      object obj1 = (object) null;
      int num = -1;
      string lower1 = val.ToString().Trim().ToLower();
      foreach (object o in data)
      {
        Func<object, object> propertyDelegate = this.GetPropertyDelegate(o, prop);
        if (propertyDelegate != null)
        {
          object obj2 = propertyDelegate(o);
          if (obj2 != null)
          {
            string lower2 = obj2.ToString().Trim().ToLower();
            if (lower2.Length >= lower1.Length && lower2.StartsWith(lower1) && (num == -1 || lower2.Length - lower1.Length < num))
            {
              obj1 = o;
              num = lower2.Length - lower1.Length;
            }
          }
        }
      }
      return obj1;
    }

    private IEnumerable<string> PermutationsOf(string s, char[] separators)
    {
      string[] words = s.ToLower().Split(separators);
      int n = words.Length;
      int[] p = new int[n];
      int i = 1;
      while (i < n)
      {
        if (p[i] < i)
        {
          int index = i % 2 == 0 ? 0 : p[i];
          string str = words[i];
          words[i] = words[index];
          words[index] = str;
          yield return string.Join("", words);
          ++p[i];
          i = 1;
        }
        else
        {
          p[i] = 0;
          ++i;
        }
      }
    }

    private int LevnshteinDistance(string s1, string s2, int maxDistance = -1, bool ignoreCase = false)
    {
      if (s1.Length == 0)
        return s2.Length;
      if (s2.Length == 0)
        return s1.Length;
      if (maxDistance < 0)
        maxDistance = Math.Min(s2.Length, s1.Length);
      int[] numArray1 = new int[s2.Length + 1];
      int[] numArray2 = new int[s2.Length + 1];
      for (int index = 1; index < s2.Length + 1; ++index)
        numArray1[index] = index;
      IComparer<char> comparer = ignoreCase ? (IComparer<char>) Comparer<char>.Default : (IComparer<char>) new CaseInsensitiveCharComparer(CultureInfo.InvariantCulture);
      for (int index1 = 1; index1 < s1.Length + 1; ++index1)
      {
        int val1_1 = index1;
        numArray2[0] = index1;
        for (int index2 = 1; index2 < s2.Length + 1; ++index2)
        {
          int val1_2 = numArray1[index2 - 1];
          if (comparer.Compare(s1[index1 - 1], s2[index2 - 1]) != 0)
            ++val1_2;
          numArray2[index2] = Math.Min(val1_2, Math.Min(numArray1[index2] + 1, numArray2[index2 - 1] + 1));
          val1_1 = Math.Min(val1_1, numArray2[index2]);
        }
        if (val1_1 > maxDistance)
          return val1_1;
        int[] numArray3 = numArray1;
        numArray1 = numArray2;
        numArray2 = numArray3;
      }
      return numArray1[s2.Length];
    }

    private void dgMapValues_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.dgSourceData.DataSource == null || this.m_curSrcColumn == null)
        return;
      DataGridViewColumn column = this.dgMapValues.Columns[e.ColumnIndex];
      object key = this.m_dImportColumnValues[this.m_curSrcColumn].ToList<object>()[e.RowIndex];
      if (column == this.dcUserValue)
      {
        e.Value = key;
      }
      else
      {
        if (column != this.dcEcoValue && column != this.dcEcoValueAlt && column != this.dcEcoValueSearch)
          return;
        FieldMap fieldMapFromSource = this.m_mapList.GetFieldMapFromSource(this.m_curSrcColumn);
        if (fieldMapFromSource == null)
          return;
        object obj = (object) null;
        if (!fieldMapFromSource.Map.TryGetValue(key, out obj))
          return;
        e.Value = obj;
      }
    }

    private void dgMapValues_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (this.dgSourceFields.DataSource == null || this.m_curSrcColumn == null)
        return;
      DataGridViewColumn column = this.dgMapValues.Columns[e.ColumnIndex];
      FieldMap fieldMapFromSource = this.m_mapList.GetFieldMapFromSource(this.m_curSrcColumn);
      DataGridViewTextBoxColumn dcEcoValueAlt = this.dcEcoValueAlt;
      if (column != dcEcoValueAlt || fieldMapFromSource == null)
        return;
      DataGridViewCell dgMapValue = this.dgMapValues[e.ColumnIndex, e.RowIndex];
      object dest = dgMapValue.Value;
      if (dest != null)
      {
        Type formatType = fieldMapFromSource.ActiveFormat.FormatType;
        if (!TypeHelper.TryConvert(dgMapValue.Value, formatType, out dest))
        {
          e.ErrorText = ImportDataForm.GetTypeError(formatType);
        }
        else
        {
          if (fieldMapFromSource.FieldSpec.Constraints.Count <= 0)
            return;
          foreach (FieldConstraint constraint in fieldMapFromSource.FieldSpec.Constraints)
          {
            if (!constraint.IsValid(dest))
            {
              e.ErrorText = constraint.FormatError((object) i_Tree_Eco_v6.Resources.Strings.Value);
              break;
            }
          }
        }
      }
      else
      {
        if (!fieldMapFromSource.FieldSpec.Required)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) fieldMapFromSource.FieldSpec.Field);
      }
    }

    private void dgMapValues_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.m_curSrcColumn == null)
        return;
      DataGridViewColumn column = this.dgMapValues.Columns[e.ColumnIndex];
      FieldMap fieldMapFromSource = this.m_mapList.GetFieldMapFromSource(this.m_curSrcColumn);
      List<object> list = this.m_dImportColumnValues[this.m_curSrcColumn].ToList<object>();
      object key = list[e.RowIndex];
      if (column != this.dcEcoValue && column != this.dcEcoValueAlt && column != this.dcEcoValueSearch)
        return;
      fieldMapFromSource.Map[key] = e.Value;
      (this.dgSourceFields.DataSource as DataTable).Rows[this.dgSourceFields.CurrentRow.Index]["Status"] = (object) string.Format(i_Tree_Eco_v6.Resources.Strings.MsgMappingStatus, (object) fieldMapFromSource.Map.Count<KeyValuePair<object, object>>((Func<KeyValuePair<object, object>, bool>) (o => o.Value != null)), (object) list.Count);
    }

    private void dgMapValues_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.m_curSrcColumn == null)
        return;
      DataGridViewColumn column = this.dgMapValues.Columns[e.ColumnIndex];
      FieldMap fieldMapFromSource = this.m_mapList.GetFieldMapFromSource(this.m_curSrcColumn);
      if (fieldMapFromSource == null)
        return;
      object key = this.m_dImportColumnValues[this.m_curSrcColumn].ToList<object>()[e.RowIndex];
      object component = (object) null;
      if (fieldMapFromSource.Map.TryGetValue(key, out component) && component != null)
      {
        if (fieldMapFromSource.ActiveFormat.Property != null)
        {
          component.GetType();
          PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component).Find(fieldMapFromSource.ActiveFormat.Property, true);
          if (propertyDescriptor != null)
            component = propertyDescriptor.GetValue(component);
        }
        component = (object) component.ToString().ToLower();
      }
      DataGridViewRow row = this.dgMapValues.Rows[e.RowIndex];
      DataGridViewCell cell = row.Cells[e.ColumnIndex];
      if (row.Selected)
        cell.Style.ForeColor = Color.Black;
      else if (key.ToString().ToLower().Equals(component))
        cell.Style.ForeColor = Color.Green;
      else if (component != null)
        cell.Style.ForeColor = Color.DarkOrange;
      else
        cell.Style.ForeColor = Color.Black;
    }

    private void dgSourceData_SelectionChanged(object sender, EventArgs e) => this.dgSourceData.ClearSelection();

    private void dgMapValues_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgMapValues_EditingControlShowing(
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

    private void rtlIntro_LinkClicked(object sender, LinkClickedEventArgs e)
    {
      try
      {
        Process.Start(e.LinkText);
      }
      catch (Win32Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ExMsgOpenLink, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void dgProcessedData_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void btnSaveRejects_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.AddExtension = true;
      saveFileDialog.DefaultExt = ".csv";
      saveFileDialog.Filter = "Comma Separated File (*.csv)|*.csv|All Files (*.*)|*.*";
      saveFileDialog.CheckPathExists = true;
      saveFileDialog.OverwritePrompt = true;
      saveFileDialog.ShowHelp = false;
      saveFileDialog.CreatePrompt = false;
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.ReadWrite))
      {
        using (TextWriter writer = (TextWriter) new StreamWriter((Stream) fileStream))
        {
          CsvWriter csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
          csvWriter.WriteField("Row #");
          csvWriter.WriteField("Reason");
          csvWriter.NextRecord();
          foreach (KeyValuePair<int, string> keyValuePair in (IEnumerable) this.m_dataAnalyzer.RejectedData)
          {
            csvWriter.WriteField<int>(keyValuePair.Key);
            csvWriter.WriteField(keyValuePair.Value);
            csvWriter.NextRecord();
          }
        }
      }
    }

    private void wizImport_PreviousPage(object sender, PageEventArgs e)
    {
      if (sender == this.wpMatchFields)
      {
        this.dgUserData.DataSource = (object) null;
      }
      else
      {
        if (sender != this.wpMatchValues)
          return;
        this.m_curSrcColumn = (string) null;
        this.dgSourceFields.DataSource = (object) null;
        this.dgMapValues.RowCount = 0;
      }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ImportDataForm));
      this.wizImport = new Wizard();
      this.wpIntro = new WizardPage();
      this.rtlIntro = new RichTextLabel();
      this.wpDataSource = new WizardPage();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.dgSourceData = new DataGridView();
      this.lblSource = new Label();
      this.txtFileName = new TextBox();
      this.lblFileName = new Label();
      this.btnBrowse = new Button();
      this.lblSelectFile = new Label();
      this.cboSource = new ComboBox();
      this.chkHasHeaders = new CheckBox();
      this.wpMatchFields = new WizardPage();
      this.tableLayoutPanel3 = new TableLayoutPanel();
      this.tableLayoutPanel2 = new TableLayoutPanel();
      this.chkNeedMapValue = new WrappingCheckBox();
      this.cboFieldType = new ComboBox();
      this.lblFieldType = new Label();
      this.cboEcoField = new ComboBox();
      this.lblEcoField = new Label();
      this.label4 = new Label();
      this.txtSourceColumn = new TextBox();
      this.richTextLabel1 = new RichTextLabel();
      this.label1 = new Label();
      this.dgUserData = new DataGridView();
      this.wpMatchValues = new WizardPage();
      this.splitContainer1 = new SplitContainer();
      this.tableLayoutPanel4 = new TableLayoutPanel();
      this.label2 = new Label();
      this.dgSourceFields = new DataGridView();
      this.tableLayoutPanel5 = new TableLayoutPanel();
      this.lblSourceData = new Label();
      this.dgMapValues = new DataGridView();
      this.dcUserValue = new DataGridViewTextBoxColumn();
      this.dcEcoValueAlt = new DataGridViewTextBoxColumn();
      this.dcEcoValue = new DataGridViewComboBoxColumn();
      this.dcEcoValueSearch = new DataGridViewFilteredComboBoxColumn();
      this.wpProcessData = new WizardPage();
      this.tableLayoutPanel6 = new TableLayoutPanel();
      this.lblReview = new Label();
      this.lblRecordsAnalyzed = new Label();
      this.lblRecordsNotImported = new Label();
      this.lblRecordsToImport = new Label();
      this.lblRecordsToImportHdr = new Label();
      this.lblNumRecordsAnalyzed = new Label();
      this.lblNumRecordsNotImported = new Label();
      this.lblNumRecordsToImport = new Label();
      this.lblFinish = new Label();
      this.label3 = new Label();
      this.label5 = new Label();
      this.lblNumExistingRecords = new Label();
      this.tcRecords = new TabControl();
      this.tpImported = new TabPage();
      this.dgImportedData = new DataGridView();
      this.tpRejected = new TabPage();
      this.btnSaveRejects = new Button();
      this.dgRejectedData = new DataGridView();
      this.dcRejectedRow = new DataGridViewTextBoxColumn();
      this.dcRejectedReason = new DataGridViewTextBoxColumn();
      this.ep = new ErrorProvider(this.components);
      this.wpIntro.SuspendLayout();
      this.wpDataSource.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.dgSourceData).BeginInit();
      this.wpMatchFields.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      ((ISupportInitialize) this.dgUserData).BeginInit();
      this.wpMatchValues.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.tableLayoutPanel4.SuspendLayout();
      ((ISupportInitialize) this.dgSourceFields).BeginInit();
      this.tableLayoutPanel5.SuspendLayout();
      ((ISupportInitialize) this.dgMapValues).BeginInit();
      this.wpProcessData.SuspendLayout();
      this.tableLayoutPanel6.SuspendLayout();
      this.tcRecords.SuspendLayout();
      this.tpImported.SuspendLayout();
      ((ISupportInitialize) this.dgImportedData).BeginInit();
      this.tpRejected.SuspendLayout();
      ((ISupportInitialize) this.dgRejectedData).BeginInit();
      ((ISupportInitialize) this.ep).BeginInit();
      this.SuspendLayout();
      this.wizImport.BackColor = Color.White;
      componentResourceManager.ApplyResources((object) this.wizImport, "wizImport");
      this.wizImport.Name = "wizImport";
      this.wizImport.Pages.Add(this.wpIntro);
      this.wizImport.Pages.Add(this.wpDataSource);
      this.wizImport.Pages.Add(this.wpMatchFields);
      this.wizImport.Pages.Add(this.wpMatchValues);
      this.wizImport.Pages.Add(this.wpProcessData);
      this.wizImport.Sidebar.BackColor = Color.Transparent;
      this.wizImport.Sidebar.BackgroundImage = (Image) i_Tree_Eco_v6.Properties.Resources.sidebar;
      componentResourceManager.ApplyResources((object) this.wizImport.Sidebar, "wizImport.Sidebar");
      this.wizImport.Sidebar.Name = "Sidebar";
      this.wizImport.SidebarWidth = 150;
      this.wizImport.NextPage += new Wizard.PageEventHandler(this.wizImport_NextPage);
      this.wizImport.PreviousPage += new Wizard.PageEventHandler(this.wizImport_PreviousPage);
      this.wizImport.Finished += new EventHandler(this.wizImport_Finished);
      this.wizImport.Cancel += new EventHandler(this.wizImport_Cancel);
      this.wpIntro.Controls.Add((Control) this.rtlIntro);
      this.wpIntro.Name = "wpIntro";
      componentResourceManager.ApplyResources((object) this.wpIntro, "wpIntro");
      componentResourceManager.ApplyResources((object) this.rtlIntro, "rtlIntro");
      this.rtlIntro.Name = "rtlIntro";
      this.rtlIntro.TabStop = false;
      this.rtlIntro.LinkClicked += new LinkClickedEventHandler(this.rtlIntro_LinkClicked);
      this.wpDataSource.Controls.Add((Control) this.tableLayoutPanel1);
      this.wpDataSource.Name = "wpDataSource";
      componentResourceManager.ApplyResources((object) this.wpDataSource, "wpDataSource");
      this.wpDataSource.Validating += new CancelEventHandler(this.wpDataSource_Validating);
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.dgSourceData, 0, 4);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblSource, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtFileName, 1, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblFileName, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.btnBrowse, 2, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblSelectFile, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.cboSource, 1, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.chkHasHeaders, 1, 3);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.dgSourceData.AllowUserToAddRows = false;
      this.dgSourceData.AllowUserToDeleteRows = false;
      this.dgSourceData.AllowUserToResizeRows = false;
      this.dgSourceData.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgSourceData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.dgSourceData, 4);
      componentResourceManager.ApplyResources((object) this.dgSourceData, "dgSourceData");
      this.dgSourceData.EnableHeadersVisualStyles = false;
      this.dgSourceData.MultiSelect = false;
      this.dgSourceData.Name = "dgSourceData";
      this.dgSourceData.ReadOnly = true;
      this.dgSourceData.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgSourceData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgSourceData.SelectionChanged += new EventHandler(this.dgSourceData_SelectionChanged);
      componentResourceManager.ApplyResources((object) this.lblSource, "lblSource");
      this.lblSource.Name = "lblSource";
      componentResourceManager.ApplyResources((object) this.txtFileName, "txtFileName");
      this.txtFileName.Name = "txtFileName";
      this.txtFileName.Validating += new CancelEventHandler(this.txtFileName_Validating);
      componentResourceManager.ApplyResources((object) this.lblFileName, "lblFileName");
      this.lblFileName.Name = "lblFileName";
      componentResourceManager.ApplyResources((object) this.btnBrowse, "btnBrowse");
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.UseVisualStyleBackColor = true;
      this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
      componentResourceManager.ApplyResources((object) this.lblSelectFile, "lblSelectFile");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblSelectFile, 4);
      this.lblSelectFile.Name = "lblSelectFile";
      componentResourceManager.ApplyResources((object) this.cboSource, "cboSource");
      this.cboSource.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSource.FormattingEnabled = true;
      this.cboSource.Name = "cboSource";
      this.cboSource.SelectedIndexChanged += new EventHandler(this.cboSource_SelectedIndexChanged);
      this.cboSource.Validating += new CancelEventHandler(this.cboSource_Validating);
      componentResourceManager.ApplyResources((object) this.chkHasHeaders, "chkHasHeaders");
      this.chkHasHeaders.Name = "chkHasHeaders";
      this.chkHasHeaders.UseVisualStyleBackColor = true;
      this.chkHasHeaders.CheckedChanged += new EventHandler(this.chkHasHeaders_CheckedChanged);
      this.wpMatchFields.Controls.Add((Control) this.tableLayoutPanel3);
      this.wpMatchFields.Name = "wpMatchFields";
      componentResourceManager.ApplyResources((object) this.wpMatchFields, "wpMatchFields");
      this.wpMatchFields.Validating += new CancelEventHandler(this.wpMatchFields_Validating);
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel3, "tableLayoutPanel3");
      this.tableLayoutPanel3.Controls.Add((Control) this.tableLayoutPanel2, 0, 2);
      this.tableLayoutPanel3.Controls.Add((Control) this.label1, 0, 0);
      this.tableLayoutPanel3.Controls.Add((Control) this.dgUserData, 0, 1);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel2, "tableLayoutPanel2");
      this.tableLayoutPanel2.CausesValidation = false;
      this.tableLayoutPanel2.Controls.Add((Control) this.chkNeedMapValue, 0, 2);
      this.tableLayoutPanel2.Controls.Add((Control) this.cboFieldType, 3, 2);
      this.tableLayoutPanel2.Controls.Add((Control) this.lblFieldType, 2, 2);
      this.tableLayoutPanel2.Controls.Add((Control) this.cboEcoField, 3, 1);
      this.tableLayoutPanel2.Controls.Add((Control) this.lblEcoField, 2, 1);
      this.tableLayoutPanel2.Controls.Add((Control) this.label4, 0, 1);
      this.tableLayoutPanel2.Controls.Add((Control) this.txtSourceColumn, 1, 1);
      this.tableLayoutPanel2.Controls.Add((Control) this.richTextLabel1, 0, 0);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.chkNeedMapValue, "chkNeedMapValue");
      this.chkNeedMapValue.CausesValidation = false;
      this.tableLayoutPanel2.SetColumnSpan((Control) this.chkNeedMapValue, 2);
      this.chkNeedMapValue.Name = "chkNeedMapValue";
      this.chkNeedMapValue.Tag = (object) "";
      this.chkNeedMapValue.UseVisualStyleBackColor = true;
      this.chkNeedMapValue.CheckedChanged += new EventHandler(this.chkNeedMapValue_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.cboFieldType, "cboFieldType");
      this.cboFieldType.CausesValidation = false;
      this.cboFieldType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldType.FormattingEnabled = true;
      this.cboFieldType.Name = "cboFieldType";
      this.cboFieldType.Tag = (object) "";
      this.cboFieldType.SelectedValueChanged += new EventHandler(this.cboFieldType_SelectedValueChanged);
      componentResourceManager.ApplyResources((object) this.lblFieldType, "lblFieldType");
      this.lblFieldType.Name = "lblFieldType";
      this.lblFieldType.Tag = (object) "UFORHICCrossWalker.Help.Identify.FieldType.rtf";
      componentResourceManager.ApplyResources((object) this.cboEcoField, "cboEcoField");
      this.cboEcoField.CausesValidation = false;
      this.cboEcoField.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboEcoField.FormattingEnabled = true;
      this.cboEcoField.Name = "cboEcoField";
      this.cboEcoField.Tag = (object) "";
      this.cboEcoField.SelectedValueChanged += new EventHandler(this.cboEcoField_SelectedValueChanged);
      componentResourceManager.ApplyResources((object) this.lblEcoField, "lblEcoField");
      this.lblEcoField.Name = "lblEcoField";
      this.lblEcoField.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      this.label4.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.txtSourceColumn, "txtSourceColumn");
      this.txtSourceColumn.CausesValidation = false;
      this.txtSourceColumn.Name = "txtSourceColumn";
      this.txtSourceColumn.Tag = (object) "";
      componentResourceManager.ApplyResources((object) this.richTextLabel1, "richTextLabel1");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.richTextLabel1, 4);
      this.richTextLabel1.Name = "richTextLabel1";
      this.richTextLabel1.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      this.dgUserData.AllowUserToAddRows = false;
      this.dgUserData.AllowUserToDeleteRows = false;
      this.dgUserData.AllowUserToResizeRows = false;
      this.dgUserData.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgUserData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      componentResourceManager.ApplyResources((object) this.dgUserData, "dgUserData");
      this.dgUserData.EnableHeadersVisualStyles = false;
      this.dgUserData.MultiSelect = false;
      this.dgUserData.Name = "dgUserData";
      this.dgUserData.ReadOnly = true;
      this.dgUserData.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgUserData.RowHeadersVisible = false;
      this.dgUserData.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgUserData_CellErrorTextNeeded);
      this.dgUserData.CellValidating += new DataGridViewCellValidatingEventHandler(this.dgUserData_CellValidating);
      this.dgUserData.SelectionChanged += new EventHandler(this.dgUserData_SelectionChanged);
      this.wpMatchValues.Controls.Add((Control) this.splitContainer1);
      this.wpMatchValues.Name = "wpMatchValues";
      componentResourceManager.ApplyResources((object) this.wpMatchValues, "wpMatchValues");
      componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.tableLayoutPanel4);
      this.splitContainer1.Panel2.Controls.Add((Control) this.tableLayoutPanel5);
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel4, "tableLayoutPanel4");
      this.tableLayoutPanel4.Controls.Add((Control) this.label2, 0, 0);
      this.tableLayoutPanel4.Controls.Add((Control) this.dgSourceFields, 0, 1);
      this.tableLayoutPanel4.Name = "tableLayoutPanel4";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      this.dgSourceFields.AllowUserToAddRows = false;
      this.dgSourceFields.AllowUserToDeleteRows = false;
      this.dgSourceFields.AllowUserToResizeRows = false;
      this.dgSourceFields.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgSourceFields.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      componentResourceManager.ApplyResources((object) this.dgSourceFields, "dgSourceFields");
      this.dgSourceFields.EnableHeadersVisualStyles = false;
      this.dgSourceFields.MultiSelect = false;
      this.dgSourceFields.Name = "dgSourceFields";
      this.dgSourceFields.ReadOnly = true;
      this.dgSourceFields.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgSourceFields.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgSourceFields.SelectionChanged += new EventHandler(this.dgSourceFields_SelectionChanged);
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel5, "tableLayoutPanel5");
      this.tableLayoutPanel5.Controls.Add((Control) this.lblSourceData, 0, 0);
      this.tableLayoutPanel5.Controls.Add((Control) this.dgMapValues, 0, 1);
      this.tableLayoutPanel5.Name = "tableLayoutPanel5";
      componentResourceManager.ApplyResources((object) this.lblSourceData, "lblSourceData");
      this.lblSourceData.Name = "lblSourceData";
      this.dgMapValues.AllowUserToAddRows = false;
      this.dgMapValues.AllowUserToDeleteRows = false;
      this.dgMapValues.AllowUserToResizeRows = false;
      this.dgMapValues.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgMapValues.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgMapValues.Columns.AddRange((DataGridViewColumn) this.dcUserValue, (DataGridViewColumn) this.dcEcoValueAlt, (DataGridViewColumn) this.dcEcoValue, (DataGridViewColumn) this.dcEcoValueSearch);
      componentResourceManager.ApplyResources((object) this.dgMapValues, "dgMapValues");
      this.dgMapValues.EditMode = DataGridViewEditMode.EditOnEnter;
      this.dgMapValues.EnableHeadersVisualStyles = false;
      this.dgMapValues.MultiSelect = false;
      this.dgMapValues.Name = "dgMapValues";
      this.dgMapValues.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgMapValues.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgMapValues.VirtualMode = true;
      this.dgMapValues.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgMapValues_CellErrorTextNeeded);
      this.dgMapValues.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgMapValues_CellFormatting);
      this.dgMapValues.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dgMapValues_CellValueNeeded);
      this.dgMapValues.CellValuePushed += new DataGridViewCellValueEventHandler(this.dgMapValues_CellValuePushed);
      this.dgMapValues.DataError += new DataGridViewDataErrorEventHandler(this.dgMapValues_DataError);
      this.dgMapValues.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgMapValues_EditingControlShowing);
      this.dcUserValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.dcUserValue.DataPropertyName = "Key";
      componentResourceManager.ApplyResources((object) this.dcUserValue, "dcUserValue");
      this.dcUserValue.Name = "dcUserValue";
      this.dcUserValue.ReadOnly = true;
      this.dcEcoValueAlt.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dcEcoValueAlt.DataPropertyName = "Value";
      componentResourceManager.ApplyResources((object) this.dcEcoValueAlt, "dcEcoValueAlt");
      this.dcEcoValueAlt.Name = "dcEcoValueAlt";
      this.dcEcoValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dcEcoValue.DataPropertyName = "Value";
      this.dcEcoValue.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcEcoValue, "dcEcoValue");
      this.dcEcoValue.Name = "dcEcoValue";
      this.dcEcoValue.Resizable = DataGridViewTriState.True;
      this.dcEcoValueSearch.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dcEcoValueSearch.DataPropertyName = "Value";
      this.dcEcoValueSearch.DataSource = (object) null;
      this.dcEcoValueSearch.DisplayMember = (string) null;
      componentResourceManager.ApplyResources((object) this.dcEcoValueSearch, "dcEcoValueSearch");
      this.dcEcoValueSearch.Name = "dcEcoValueSearch";
      this.dcEcoValueSearch.ValueMember = (string) null;
      this.wpProcessData.Controls.Add((Control) this.tableLayoutPanel6);
      this.wpProcessData.Name = "wpProcessData";
      componentResourceManager.ApplyResources((object) this.wpProcessData, "wpProcessData");
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel6, "tableLayoutPanel6");
      this.tableLayoutPanel6.Controls.Add((Control) this.lblReview, 0, 3);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblRecordsAnalyzed, 1, 4);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblRecordsNotImported, 1, 6);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblRecordsToImport, 1, 7);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblRecordsToImportHdr, 0, 8);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblNumRecordsAnalyzed, 2, 4);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblNumRecordsNotImported, 2, 6);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblNumRecordsToImport, 2, 7);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblFinish, 1, 10);
      this.tableLayoutPanel6.Controls.Add((Control) this.label3, 0, 0);
      this.tableLayoutPanel6.Controls.Add((Control) this.label5, 1, 1);
      this.tableLayoutPanel6.Controls.Add((Control) this.lblNumExistingRecords, 2, 1);
      this.tableLayoutPanel6.Controls.Add((Control) this.tcRecords, 1, 9);
      this.tableLayoutPanel6.Name = "tableLayoutPanel6";
      componentResourceManager.ApplyResources((object) this.lblReview, "lblReview");
      this.tableLayoutPanel6.SetColumnSpan((Control) this.lblReview, 4);
      this.lblReview.Name = "lblReview";
      componentResourceManager.ApplyResources((object) this.lblRecordsAnalyzed, "lblRecordsAnalyzed");
      this.lblRecordsAnalyzed.Name = "lblRecordsAnalyzed";
      componentResourceManager.ApplyResources((object) this.lblRecordsNotImported, "lblRecordsNotImported");
      this.lblRecordsNotImported.Name = "lblRecordsNotImported";
      componentResourceManager.ApplyResources((object) this.lblRecordsToImport, "lblRecordsToImport");
      this.lblRecordsToImport.Name = "lblRecordsToImport";
      componentResourceManager.ApplyResources((object) this.lblRecordsToImportHdr, "lblRecordsToImportHdr");
      this.tableLayoutPanel6.SetColumnSpan((Control) this.lblRecordsToImportHdr, 3);
      this.lblRecordsToImportHdr.Name = "lblRecordsToImportHdr";
      componentResourceManager.ApplyResources((object) this.lblNumRecordsAnalyzed, "lblNumRecordsAnalyzed");
      this.lblNumRecordsAnalyzed.Name = "lblNumRecordsAnalyzed";
      componentResourceManager.ApplyResources((object) this.lblNumRecordsNotImported, "lblNumRecordsNotImported");
      this.lblNumRecordsNotImported.Name = "lblNumRecordsNotImported";
      componentResourceManager.ApplyResources((object) this.lblNumRecordsToImport, "lblNumRecordsToImport");
      this.lblNumRecordsToImport.Name = "lblNumRecordsToImport";
      componentResourceManager.ApplyResources((object) this.lblFinish, "lblFinish");
      this.tableLayoutPanel6.SetColumnSpan((Control) this.lblFinish, 3);
      this.lblFinish.Name = "lblFinish";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.tableLayoutPanel6.SetColumnSpan((Control) this.label3, 4);
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.lblNumExistingRecords, "lblNumExistingRecords");
      this.lblNumExistingRecords.Name = "lblNumExistingRecords";
      this.tableLayoutPanel6.SetColumnSpan((Control) this.tcRecords, 3);
      this.tcRecords.Controls.Add((Control) this.tpImported);
      this.tcRecords.Controls.Add((Control) this.tpRejected);
      componentResourceManager.ApplyResources((object) this.tcRecords, "tcRecords");
      this.tcRecords.Name = "tcRecords";
      this.tcRecords.SelectedIndex = 0;
      this.tpImported.Controls.Add((Control) this.dgImportedData);
      componentResourceManager.ApplyResources((object) this.tpImported, "tpImported");
      this.tpImported.Name = "tpImported";
      this.tpImported.UseVisualStyleBackColor = true;
      this.dgImportedData.AllowUserToAddRows = false;
      this.dgImportedData.AllowUserToDeleteRows = false;
      this.dgImportedData.AllowUserToResizeRows = false;
      this.dgImportedData.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgImportedData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      componentResourceManager.ApplyResources((object) this.dgImportedData, "dgImportedData");
      this.dgImportedData.EnableHeadersVisualStyles = false;
      this.dgImportedData.MultiSelect = false;
      this.dgImportedData.Name = "dgImportedData";
      this.dgImportedData.ReadOnly = true;
      this.dgImportedData.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgImportedData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgImportedData.DataError += new DataGridViewDataErrorEventHandler(this.dgProcessedData_DataError);
      this.tpRejected.Controls.Add((Control) this.btnSaveRejects);
      this.tpRejected.Controls.Add((Control) this.dgRejectedData);
      componentResourceManager.ApplyResources((object) this.tpRejected, "tpRejected");
      this.tpRejected.Name = "tpRejected";
      this.tpRejected.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.btnSaveRejects, "btnSaveRejects");
      this.btnSaveRejects.Name = "btnSaveRejects";
      this.btnSaveRejects.UseVisualStyleBackColor = true;
      this.btnSaveRejects.Click += new EventHandler(this.btnSaveRejects_Click);
      this.dgRejectedData.AllowUserToAddRows = false;
      this.dgRejectedData.AllowUserToDeleteRows = false;
      this.dgRejectedData.AllowUserToResizeRows = false;
      componentResourceManager.ApplyResources((object) this.dgRejectedData, "dgRejectedData");
      this.dgRejectedData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
      this.dgRejectedData.Columns.AddRange((DataGridViewColumn) this.dcRejectedRow, (DataGridViewColumn) this.dcRejectedReason);
      this.dgRejectedData.EnableHeadersVisualStyles = false;
      this.dgRejectedData.MultiSelect = false;
      this.dgRejectedData.Name = "dgRejectedData";
      this.dgRejectedData.ReadOnly = true;
      this.dgRejectedData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dcRejectedRow.DataPropertyName = "Key";
      componentResourceManager.ApplyResources((object) this.dcRejectedRow, "dcRejectedRow");
      this.dcRejectedRow.Name = "dcRejectedRow";
      this.dcRejectedRow.ReadOnly = true;
      this.dcRejectedReason.DataPropertyName = "Value";
      componentResourceManager.ApplyResources((object) this.dcRejectedReason, "dcRejectedReason");
      this.dcRejectedReason.Name = "dcRejectedReason";
      this.dcRejectedReason.ReadOnly = true;
      this.ep.ContainerControl = (ContainerControl) this;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.wizImport);
      this.Name = nameof (ImportDataForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.wpIntro.ResumeLayout(false);
      this.wpDataSource.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((ISupportInitialize) this.dgSourceData).EndInit();
      this.wpMatchFields.ResumeLayout(false);
      this.tableLayoutPanel3.ResumeLayout(false);
      this.tableLayoutPanel3.PerformLayout();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      ((ISupportInitialize) this.dgUserData).EndInit();
      this.wpMatchValues.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.tableLayoutPanel4.ResumeLayout(false);
      this.tableLayoutPanel4.PerformLayout();
      ((ISupportInitialize) this.dgSourceFields).EndInit();
      this.tableLayoutPanel5.ResumeLayout(false);
      this.tableLayoutPanel5.PerformLayout();
      ((ISupportInitialize) this.dgMapValues).EndInit();
      this.wpProcessData.ResumeLayout(false);
      this.tableLayoutPanel6.ResumeLayout(false);
      this.tableLayoutPanel6.PerformLayout();
      this.tcRecords.ResumeLayout(false);
      this.tpImported.ResumeLayout(false);
      ((ISupportInitialize) this.dgImportedData).EndInit();
      this.tpRejected.ResumeLayout(false);
      ((ISupportInitialize) this.dgRejectedData).EndInit();
      ((ISupportInitialize) this.ep).EndInit();
      this.ResumeLayout(false);
    }
  }
}
