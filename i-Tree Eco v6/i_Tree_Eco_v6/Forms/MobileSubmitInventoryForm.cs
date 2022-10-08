// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.MobileSubmitInventoryForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using DaveyTree.NHibernate;
using DaveyTree.NHibernate.Extensions;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Views;
using i_Tree_Eco_v6.Events;
using IPED.Domain;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class MobileSubmitInventoryForm : MobileForm
  {
    private TaskManager m_taskManager;
    private ProgramSession m_ps;
    private ISession m_session;
    private BindingList<SpeciesView> m_dsSpecies;
    private ISet<object> m_selPlots;
    private Year m_year;
    private IPEDData m_iped;
    private IList<Street> m_dsStreets;
    private PagedList<Plot> m_plots;
    private DataBindingList<MobileLogEntry> m_dsLogEntries;
    private FlatPlotView m_fpv;
    private PropertyDescriptorCollection m_fpvProperties;
    private IDictionary<double, string> m_dbhs;
    private IContainer components;
    private TableLayoutPanel tableLayoutPanel1;
    private TableLayoutPanel panel1;
    private Label lblSelTrees;
    internal CheckBox chkAllTrees;
    private Label label1;
    private Button btnResetPassword;
    private Button btnSubmit;
    internal Label lblEmail2;
    internal TextBox txtEmail2;
    private Label Label3;
    private Label lblPassword2;
    private Label lblPassword1;
    private TextBox txtPassword2;
    private TextBox txtPassword1;
    internal TextBox txtEmail1;
    private Label lblEmail1;
    private DataGridView dgPlots;
    private TableLayoutPanel tableLayoutPanel2;
    private Label lblResetPassword;
    private Label label2;
    private DataGridView dgSubmitLog;
    private DataGridViewTextBoxColumn dcMobileKey;
    private DataGridViewNullableDateTimeColumn dcDateTime;
    private DataGridViewTextBoxColumn dcDescription;
    private DataGridViewCheckBoxColumn dcSelect;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewTextBoxColumn dcUserId;
    private DataGridViewComboBoxColumn dcStrata;
    private DataGridViewTextBoxColumn dcCrew;
    private DataGridViewNullableDateTimeColumn dcSurveyDate;
    private DataGridViewComboBoxColumn dcStatus;
    private DataGridViewFilteredComboBoxColumn dcSpecies;
    private DataGridViewComboBoxColumn dcStreet;
    private DataGridViewTextBoxColumn dcAddress;
    private DataGridViewComboBoxColumn dcLandUse;
    private DataGridViewTextBoxColumn dcPhoto;
    private DataGridViewNumericTextBoxColumn dcDBH1;
    private DataGridViewComboBoxColumn dcDBH1c;
    private DataGridViewNumericTextBoxColumn dcDBH1Height;
    private DataGridViewCheckBoxColumn dcDBH1Measured;
    private DataGridViewNumericTextBoxColumn dcDBH2;
    private DataGridViewComboBoxColumn dcDBH2c;
    private DataGridViewNumericTextBoxColumn dcDBH2Height;
    private DataGridViewCheckBoxColumn dcDBH2Measured;
    private DataGridViewNumericTextBoxColumn dcDBH3;
    private DataGridViewComboBoxColumn dcDBH3c;
    private DataGridViewNumericTextBoxColumn dcDBH3Height;
    private DataGridViewCheckBoxColumn dcDBH3Measured;
    private DataGridViewNumericTextBoxColumn dcDBH4;
    private DataGridViewComboBoxColumn dcDBH4c;
    private DataGridViewNumericTextBoxColumn dcDBH4Height;
    private DataGridViewCheckBoxColumn dcDBH4Measured;
    private DataGridViewNumericTextBoxColumn dcDBH5;
    private DataGridViewComboBoxColumn dcDBH5c;
    private DataGridViewNumericTextBoxColumn dcDBH5Height;
    private DataGridViewCheckBoxColumn dcDBH5Measured;
    private DataGridViewNumericTextBoxColumn dcDBH6;
    private DataGridViewComboBoxColumn dcDBH6c;
    private DataGridViewNumericTextBoxColumn dcDBH6Height;
    private DataGridViewCheckBoxColumn dcDBH6Measured;
    private DataGridViewComboBoxColumn dcCrownCondition;
    private DataGridViewComboBoxColumn dcCrownDieback;
    private DataGridViewNumericTextBoxColumn dcTreeHeight;
    private DataGridViewNumericTextBoxColumn dcCrownTopHeight;
    private DataGridViewNumericTextBoxColumn dcCrownBaseHeight;
    private DataGridViewNumericTextBoxColumn dcCrownWidthNS;
    private DataGridViewNumericTextBoxColumn dcCrownWidthEW;
    private DataGridViewComboBoxColumn dcCrownPercentMissing;
    private DataGridViewComboBoxColumn dcCrownLightExposure;
    private DataGridViewNumericTextBoxColumn dcBldg1Direction;
    private DataGridViewNumericTextBoxColumn dcBldg1Distance;
    private DataGridViewNumericTextBoxColumn dcBldg2Direction;
    private DataGridViewNumericTextBoxColumn dcBldg2Distance;
    private DataGridViewNumericTextBoxColumn dcBldg3Direction;
    private DataGridViewNumericTextBoxColumn dcBldg3Distance;
    private DataGridViewComboBoxColumn dcSiteType;
    private DataGridViewCheckBoxColumn dcStTree;
    private DataGridViewComboBoxColumn dcMaintRec;
    private DataGridViewComboBoxColumn dcMaintTask;
    private DataGridViewComboBoxColumn dcSidewalk;
    private DataGridViewComboBoxColumn dcWireConflict;
    private DataGridViewComboBoxColumn dcFieldOne;
    private DataGridViewComboBoxColumn dcFieldTwo;
    private DataGridViewComboBoxColumn dcFieldThree;
    private DataGridViewComboBoxColumn dcIPEDTSDieback;
    private DataGridViewComboBoxColumn dcIPEDTSEpiSprout;
    private DataGridViewComboBoxColumn dcIPEDTSWiltFoli;
    private DataGridViewComboBoxColumn dcIPEDTSEnvStress;
    private DataGridViewComboBoxColumn dcIPEDTSHumStress;
    private DataGridViewTextBoxColumn dcIPEDTSNotes;
    private DataGridViewComboBoxColumn dcIPEDFTChewFoli;
    private DataGridViewComboBoxColumn dcIPEDFTDiscFoli;
    private DataGridViewComboBoxColumn dcIPEDFTAbnFoli;
    private DataGridViewComboBoxColumn dcIPEDFTInsectSigns;
    private DataGridViewComboBoxColumn dcIPEDFTFoliAffect;
    private DataGridViewTextBoxColumn dcIPEDFTNotes;
    private DataGridViewComboBoxColumn dcIPEDBBInsectSigns;
    private DataGridViewComboBoxColumn dcIPEDBBInsectPres;
    private DataGridViewComboBoxColumn dcIPEDBBDiseaseSigns;
    private DataGridViewComboBoxColumn dcIPEDBBProbLoc;
    private DataGridViewComboBoxColumn dcIPEDBBAbnGrowth;
    private DataGridViewTextBoxColumn dcIPEDBBNotes;
    private DataGridViewComboBoxColumn dcIPEDPest;
    private DataGridViewCheckBoxColumn dcCityManaged;
    private DataGridViewComboBoxColumn dcLocSite;
    private DataGridViewNumericTextBoxColumn dcLocNo;
    private DataGridViewTextBoxColumn dcLatitude;
    private DataGridViewTextBoxColumn dcLongitude;
    private DataGridViewCheckBoxColumn dcNoteTree;
    private DataGridViewTextBoxColumn dcComments;

    public MobileSubmitInventoryForm()
    {
      this.m_taskManager = new TaskManager(new WaitCursor((Form) this));
      this.m_ps = Program.Session;
      this.m_selPlots = (ISet<object>) new HashSet<object>();
      this.InitializeComponent();
      this.dgPlots.DoubleBuffered(true);
      this.dgPlots.AutoGenerateColumns = false;
      this.dgPlots.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgPlots.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgSubmitLog.DoubleBuffered(true);
      this.dgSubmitLog.AutoGenerateColumns = false;
      this.dgSubmitLog.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgSubmitLog.RowHeadersDefaultCellStyle = Program.InActiveGridDefaultCellStyle;
      this.m_fpvProperties = TypeDescriptor.GetProperties(typeof (FlatPlotView));
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t => this.Init()), scheduler));
    }

    private void LoadData()
    {
      if (this.m_ps.InputSession == null || !this.m_ps.InputSession.YearKey.HasValue)
        return;
      this.m_session = this.m_ps.InputSession.CreateSession();
      using (this.m_session.BeginTransaction())
      {
        this.m_year = this.m_session.Get<Year>((object) this.m_ps.InputSession.YearKey);
        if (this.m_year.RecordIPED)
          this.m_iped = this.m_ps.IPEDData;
        if (!this.m_year.DBHActual)
          NHibernateUtil.Initialize((object) this.m_year.DBHs);
        NHibernateUtil.Initialize((object) this.m_year.Conditions);
        if (this.m_year.RecordStrata)
          NHibernateUtil.Initialize((object) this.m_year.Strata);
        if (this.m_year.RecordLanduse)
          NHibernateUtil.Initialize((object) this.m_year.LandUses);
        if (this.m_year.RecordTreeStreet)
          this.m_dsStreets = this.m_session.CreateCriteria<Street>().CreateAlias("ProjectLocation", "pl").CreateAlias("pl.Project", "p").CreateAlias("p.Series", "s").CreateAlias("s.Years", "y").Add((ICriterion) Restrictions.Eq("y.Guid", (object) this.m_year.Guid)).AddOrder(Order.Asc("Name")).List<Street>();
        if (this.m_year.RecordLocSite)
          NHibernateUtil.Initialize((object) this.m_year.LocSites);
        using (TypeHelper<Plot> typeHelper = new TypeHelper<Plot>())
          this.m_plots = this.m_session.CreateCriteria<Plot>().Add((ICriterion) Restrictions.Eq(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Year)), (object) this.m_year)).Fetch(SelectMode.Fetch, typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Trees))).Fetch(SelectMode.Fetch, typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.PlotLandUses))).AddOrder(Order.Asc(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)))).PagedList<Plot>(100);
        using (TypeHelper<MobileLogEntry> typeHelper = new TypeHelper<MobileLogEntry>())
        {
          this.m_dsLogEntries = new DataBindingList<MobileLogEntry>(this.m_session.CreateCriteria<MobileLogEntry>().Add((ICriterion) Restrictions.Eq(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<MobileLogEntry, object>>) (e => e.Year)), (object) this.m_year)).Add((ICriterion) Restrictions.Eq(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<MobileLogEntry, object>>) (e => (object) e.Submitted)), (object) true)).AddOrder(Order.Desc(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<MobileLogEntry, object>>) (e => (object) e.DateTime)))).List<MobileLogEntry>());
          this.m_dsLogEntries.Sortable = true;
        }
        this.m_dbhs = (IDictionary<double, string>) this.m_year.DBHs.ToDictionary<DBH, double, string>((Func<DBH, double>) (d => Convert.ToDouble(d.Id)), (Func<DBH, string>) (d => d.Description));
      }
      this.m_dsSpecies = new BindingList<SpeciesView>((IList<SpeciesView>) this.m_ps.Species.Values.ToList<SpeciesView>());
    }

    private IList<T> GetIPEDLookups<T>(IStatelessSession s) where T : class => s.CreateCriteria<T>().Add((ICriterion) Restrictions.IsNotNull("Sequence")).AddOrder(Order.Asc("Sequence")).List<T>();

    private void Init()
    {
      if (this.IsDisposed)
        return;
      this.chkAllTrees.Visible = this.m_plots.Count <= 100;
      this.dcSpecies.BindTo<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.CommonScientificName), (System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.Code), (object) this.m_dsSpecies);
      Year year = this.m_year;
      this.txtEmail1.Text = year.MobileEmail;
      this.txtEmail2.Text = year.MobileEmail;
      this.btnResetPassword.Visible = !string.IsNullOrEmpty(year.MobileKey);
      this.lblResetPassword.Visible = this.btnResetPassword.Visible;
      this.txtPassword2.Visible = string.IsNullOrEmpty(year.MobileKey);
      this.lblPassword2.Visible = this.txtPassword2.Visible;
      if (!year.RecordStrata)
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcStrata);
      else
        this.dcStrata.BindTo<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Description), (System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Self), (object) year.Strata.ToList<Strata>());
      string str1;
      string str2;
      if (year.Unit == YearUnit.English)
      {
        str1 = i_Tree_Eco_v6.Resources.Strings.UnitInchesAbbr;
        str2 = i_Tree_Eco_v6.Resources.Strings.UnitFeetAbbr;
      }
      else
      {
        str1 = i_Tree_Eco_v6.Resources.Strings.UnitCentimetersAbbr;
        str2 = i_Tree_Eco_v6.Resources.Strings.UnitMetersAbbr;
      }
      if (!year.RecordTreeUserId)
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcUserId);
      if (year.DBHActual)
      {
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcDBH1c);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcDBH2c);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcDBH3c);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcDBH4c);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcDBH5c);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcDBH6c);
        this.dcDBH1.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 1), (object) str1);
        this.dcDBH2.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 2), (object) str1);
        this.dcDBH3.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 3), (object) str1);
        this.dcDBH4.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 4), (object) str1);
        this.dcDBH5.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 5), (object) str1);
        this.dcDBH6.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 6), (object) str1);
      }
      else
      {
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcDBH1);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcDBH2);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcDBH3);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcDBH4);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcDBH5);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcDBH6);
        this.dcDBH1c.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 1), (object) str1);
        this.dcDBH2c.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 2), (object) str1);
        this.dcDBH3c.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 3), (object) str1);
        this.dcDBH4c.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 4), (object) str1);
        this.dcDBH5c.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 5), (object) str1);
        this.dcDBH6c.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 6), (object) str1);
        BindingSource dataSource = new BindingSource((object) this.m_dbhs, (string) null);
        this.dcDBH1c.BindTo("Value", "Key", (object) dataSource);
        this.dcDBH2c.BindTo("Value", "Key", (object) dataSource);
        this.dcDBH3c.BindTo("Value", "Key", (object) dataSource);
        this.dcDBH4c.BindTo("Value", "Key", (object) dataSource);
        this.dcDBH5c.BindTo("Value", "Key", (object) dataSource);
        this.dcDBH6c.BindTo("Value", "Key", (object) dataSource);
      }
      this.dcDBH1Height.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 1), (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      this.dcDBH2Height.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 2), (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      this.dcDBH3Height.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 3), (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      this.dcDBH4Height.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 4), (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      this.dcDBH5Height.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 5), (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      this.dcDBH6Height.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 6), (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      this.dcTreeHeight.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtTotal, (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      this.dcCrownTopHeight.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) v6Strings.Crown_SingularName, (object) v6Strings.Crown_TopHeight), (object) str2);
      this.dcCrownBaseHeight.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) v6Strings.Crown_SingularName, (object) v6Strings.Crown_BaseHeight), (object) str2);
      this.dcCrownWidthNS.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) v6Strings.Crown_SingularName, (object) v6Strings.Crown_WidthNS), (object) str2);
      this.dcCrownWidthEW.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) v6Strings.Crown_SingularName, (object) v6Strings.Crown_WidthEW), (object) str2);
      this.dcBldg1Distance.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.Building_SingularName, (object) 1), (object) v6Strings.Building_Distance), (object) str2);
      this.dcBldg2Distance.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.Building_SingularName, (object) 2), (object) v6Strings.Building_Distance), (object) str2);
      this.dcBldg3Distance.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.Building_SingularName, (object) 3), (object) v6Strings.Building_Distance), (object) str2);
      if (!year.RecordEnergy)
      {
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg1Direction);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg1Distance);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg2Direction);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg2Distance);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg3Direction);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg3Distance);
      }
      if (!year.RecordLanduse)
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcLandUse);
      else
        this.dcLandUse.BindTo<LandUse>((System.Linq.Expressions.Expression<Func<LandUse, object>>) (lu => lu.Description), (System.Linq.Expressions.Expression<Func<LandUse, object>>) (lu => lu.Self), (object) year.LandUses.ToList<LandUse>());
      MgmtStyleEnum? mgmtStyle = year.MgmtStyle;
      if (mgmtStyle.HasValue)
      {
        mgmtStyle = year.MgmtStyle;
        if ((mgmtStyle.Value & MgmtStyleEnum.RecordPublicPrivate) != MgmtStyleEnum.DefaultPublic)
          goto label_20;
      }
      this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcCityManaged);
label_20:
      if (!year.RecordStreetTree)
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcStTree);
      if (!year.RecordSiteType)
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcSiteType);
      if (year.RecordTreeStatus)
        this.dcStatus.BindTo("Value", "Key", (object) new BindingSource((object) EnumHelper.ConvertToDictionary<TreeStatus>((TreeStatus[]) null, (IComparer<TreeStatus>) new AttributePropertyComparer<TreeStatus, DescriptionAttribute>("Description")), (string) null));
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcStatus);
      if (!year.RecordTreeGPS)
      {
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcLatitude);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcLongitude);
      }
      if (year.RecordIPED)
      {
        this.dcIPEDPest.BindTo<Pest>((System.Linq.Expressions.Expression<Func<Pest, object>>) (p => p.CommonName), (System.Linq.Expressions.Expression<Func<Pest, object>>) (p => (object) p.Id), (object) this.m_iped.Pests);
        this.BindIPEDLookupColumn<BBAbnGrowth>(this.dcIPEDBBAbnGrowth, this.m_iped.BBAbnGrowth);
        this.BindIPEDLookupColumn<BBDiseaseSigns>(this.dcIPEDBBDiseaseSigns, this.m_iped.BBDiseaseSigns);
        this.BindIPEDLookupColumn<BBInsectPres>(this.dcIPEDBBInsectPres, this.m_iped.BBInsectPres);
        this.BindIPEDLookupColumn<BBInsectSigns>(this.dcIPEDBBInsectSigns, this.m_iped.BBInsectSigns);
        this.BindIPEDLookupColumn<BBProbLoc>(this.dcIPEDBBProbLoc, this.m_iped.BBProbLoc);
        this.BindIPEDLookupColumn<FTAbnFoli>(this.dcIPEDFTAbnFoli, this.m_iped.FTAbnFoli);
        this.BindIPEDLookupColumn<FTChewFoli>(this.dcIPEDFTChewFoli, this.m_iped.FTChewFoli);
        this.BindIPEDLookupColumn<FTDiscFoli>(this.dcIPEDFTDiscFoli, this.m_iped.FTDiscFoli);
        this.BindIPEDLookupColumn<FTFoliAffect>(this.dcIPEDFTFoliAffect, this.m_iped.FTFoliAffect);
        this.BindIPEDLookupColumn<FTInsectSigns>(this.dcIPEDFTInsectSigns, this.m_iped.FTInsectSigns);
        this.BindIPEDLookupColumn<TSDieback>(this.dcIPEDTSDieback, this.m_iped.TSDieback);
        this.BindIPEDLookupColumn<TSEnvStress>(this.dcIPEDTSEnvStress, this.m_iped.TSEnvStress);
        this.BindIPEDLookupColumn<TSEpiSprout>(this.dcIPEDTSEpiSprout, this.m_iped.TSEpiSprout);
        this.BindIPEDLookupColumn<TSHumStress>(this.dcIPEDTSHumStress, this.m_iped.TSHumStress);
        this.BindIPEDLookupColumn<TSWiltFoli>(this.dcIPEDTSWiltFoli, this.m_iped.TSWiltFoli);
      }
      else
      {
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDBBAbnGrowth);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDBBDiseaseSigns);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDBBInsectPres);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDBBInsectSigns);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDBBNotes);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDBBProbLoc);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDFTAbnFoli);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDFTChewFoli);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDFTDiscFoli);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDFTFoliAffect);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDFTInsectSigns);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDFTNotes);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDTSDieback);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDTSEnvStress);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDTSEpiSprout);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDTSHumStress);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDTSNotes);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDTSWiltFoli);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcIPEDPest);
      }
      if (year.RecordTreeStreet)
        this.dcStreet.BindTo<Street>((System.Linq.Expressions.Expression<Func<Street, object>>) (s => s.Name), (System.Linq.Expressions.Expression<Func<Street, object>>) (s => s.Self), (object) this.m_dsStreets);
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcStreet);
      if (!year.RecordTreeAddress)
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcAddress);
      if (!year.RecordHeight)
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcTreeHeight);
      if (year.RecordCrownCondition)
      {
        if (year.DisplayCondition)
        {
          this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcCrownDieback);
          this.dcCrownCondition.BindTo<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Description), (System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Self), (object) year.Conditions.ToList<Condition>());
        }
        else
        {
          this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcCrownCondition);
          this.dcCrownDieback.BindTo<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.DiebackDesc), (System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Self), (object) year.Conditions.ToList<Condition>());
        }
      }
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcCrownCondition);
      if (year.RecordCrownSize)
      {
        this.dcCrownPercentMissing.BindTo("Value", "Key", (object) new BindingSource((object) EnumHelper.ConvertToDictionary<PctMidRange>(new PctMidRange[1]
        {
          PctMidRange.PRINV
        }), (string) null));
      }
      else
      {
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcCrownPercentMissing);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcCrownWidthEW);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcCrownWidthNS);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcCrownBaseHeight);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcCrownTopHeight);
      }
      if (year.RecordCLE)
        this.dcCrownLightExposure.BindTo("Value", "Key", (object) new BindingSource((object) EnumHelper.ConvertToDictionary<CrownLightExposure>((CrownLightExposure[]) null, (IComparer<CrownLightExposure>) new AttributePropertyComparer<CrownLightExposure, DescriptionAttribute>("Description")), (string) null));
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcCrownLightExposure);
      if (year.RecordLocSite)
        this.BindLookupColumn<LocSite>(this.dcLocSite, (IList<LocSite>) year.LocSites.ToList<LocSite>());
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcLocSite);
      if (year.RecordMaintRec)
        this.BindLookupColumn<MaintRec>(this.dcMaintRec, (IList<MaintRec>) year.MaintRecs.ToList<MaintRec>());
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcMaintRec);
      if (year.RecordMaintTask)
        this.BindLookupColumn<MaintTask>(this.dcMaintTask, (IList<MaintTask>) year.MaintTasks.ToList<MaintTask>());
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcMaintTask);
      if (year.RecordSidewalk)
        this.BindLookupColumn<Sidewalk>(this.dcSidewalk, (IList<Sidewalk>) year.SidewalkDamages.ToList<Sidewalk>());
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcSidewalk);
      if (year.RecordWireConflict)
        this.BindLookupColumn<WireConflict>(this.dcWireConflict, (IList<WireConflict>) year.WireConflicts.ToList<WireConflict>());
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcWireConflict);
      if (year.RecordOtherOne)
      {
        if (!string.IsNullOrEmpty(year.OtherOne))
          this.dcFieldOne.HeaderText = year.OtherOne;
        this.BindLookupColumn<OtherOne>(this.dcFieldOne, (IList<OtherOne>) year.OtherOnes.ToList<OtherOne>());
      }
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcFieldOne);
      if (year.RecordOtherTwo)
      {
        if (!string.IsNullOrEmpty(year.OtherTwo))
          this.dcFieldTwo.HeaderText = year.OtherTwo;
        this.BindLookupColumn<OtherTwo>(this.dcFieldTwo, (IList<OtherTwo>) year.OtherTwos.ToList<OtherTwo>());
      }
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcFieldTwo);
      if (year.RecordOtherThree)
      {
        if (!string.IsNullOrEmpty(year.OtherThree))
          this.dcFieldThree.HeaderText = year.OtherThree;
        this.BindLookupColumn<OtherThree>(this.dcFieldThree, (IList<OtherThree>) year.OtherThrees.ToList<OtherThree>());
      }
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcFieldThree);
      this.dgPlots.RowCount = this.m_plots.Count;
      this.dgPlots.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
      this.chkAllTrees.Visible = this.m_plots.Count <= 1000;
      this.dgSubmitLog.DataSource = (object) this.m_dsLogEntries;
      this.dgSubmitLog.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
      this.DisplaySelectedTrees();
      this.tableLayoutPanel1.Enabled = true;
    }

    private void BindLookupColumn<T>(DataGridViewComboBoxColumn col, IList<T> dataSource) where T : Eco.Domain.v6.Lookup => col.BindTo<T>((System.Linq.Expressions.Expression<Func<T, object>>) (lu => lu.Description), (System.Linq.Expressions.Expression<Func<T, object>>) (lu => lu.Self), (object) dataSource);

    private void BindIPEDLookupColumn<T>(DataGridViewComboBoxColumn col, IList<T> dataSource) where T : IPED.Domain.Lookup => col.BindTo<T>((System.Linq.Expressions.Expression<Func<T, object>>) (lu => lu.Description), (System.Linq.Expressions.Expression<Func<T, object>>) (lu => (object) lu.Code), (object) dataSource);

    private void DisplaySelectedTrees()
    {
      if (this.m_plots == null)
        return;
      this.lblSelTrees.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSelected, (object) this.m_selPlots.Count, (object) (this.m_plots.Count > 1000 ? 1000 : this.m_plots.Count), (object) v6Strings.Tree_PluralName);
    }

    private void dgPlots_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.m_plots == null || e.RowIndex >= this.m_plots.Count)
        return;
      Plot plot = this.m_plots[e.RowIndex];
      FlatPlotView fpv = this.m_fpv;
      if (fpv == null || fpv.Plot == null || !fpv.Plot.Equals((object) plot))
        this.m_fpv = new FlatPlotView(this.m_session, plot);
      DataGridViewColumn column = this.dgPlots.Columns[e.ColumnIndex];
      if (column == this.dcSelect)
      {
        e.Value = (object) this.m_selPlots.Contains((object) plot.Guid);
      }
      else
      {
        PropertyDescriptor propertyDescriptor = this.m_fpvProperties.Find(column.DataPropertyName, true);
        e.Value = propertyDescriptor.GetValue((object) fpv);
      }
    }

    private void dgPlots_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (this.m_plots == null || e.ColumnIndex < 0 || e.RowIndex < 0 || e.RowIndex > this.m_plots.Count || this.dgPlots.Columns[e.ColumnIndex] != this.dcSelect)
        return;
      object id = this.m_plots.Ids[e.RowIndex];
      if (!this.m_selPlots.Contains(id))
      {
        if (this.m_selPlots.Count < 1000)
        {
          this.m_selPlots.Add(id);
        }
        else
        {
          int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrMaxSelected, (object) v6Strings.Tree_PluralName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      else
        this.m_selPlots.Remove(id);
      this.DisplaySelectedTrees();
      if (this.m_selPlots.Count == this.m_plots.Count)
        this.chkAllTrees.CheckState = CheckState.Checked;
      else if (this.chkAllTrees.CheckState == CheckState.Checked)
      {
        this.chkAllTrees.CheckState = CheckState.Indeterminate;
      }
      else
      {
        if (this.m_selPlots.Count != 0)
          return;
        this.chkAllTrees.CheckState = CheckState.Unchecked;
      }
    }

    private void chkAllTrees_CheckedChanged(object sender, EventArgs e)
    {
      this.m_selPlots = !this.chkAllTrees.Checked || this.m_plots == null ? (ISet<object>) new HashSet<object>() : (ISet<object>) new HashSet<object>((IEnumerable<object>) this.m_plots.Ids);
      this.DisplaySelectedTrees();
      this.dgPlots.Refresh();
    }

    private bool IsValid()
    {
      int num = this.txtEmail1.Text.Length > 0 ? 1 : 0;
      bool flag1 = this.txtEmail2.Text.Length > 0;
      if (num != 0)
      {
        bool flag2 = this.EmailValid(this.txtEmail1.Text);
        this.ep.SetError((Control) this.txtEmail1, !flag2, i_Tree_Eco_v6.Resources.Strings.ErrInvalidEmail);
        this.ep.SetError((Control) this.txtEmail2, !flag1, i_Tree_Eco_v6.Resources.Strings.ErrConfirmEmail);
        if (flag2 & flag1)
        {
          bool flag3 = this.txtEmail1.Text.Equals(this.txtEmail2.Text, StringComparison.CurrentCultureIgnoreCase);
          this.ep.SetError((Control) this.txtEmail1, !flag3, i_Tree_Eco_v6.Resources.Strings.ErrEmailNotMatch);
          this.ep.SetError((Control) this.txtEmail2, !flag3, i_Tree_Eco_v6.Resources.Strings.ErrEmailNotMatch);
        }
      }
      else
        this.ep.SetError((Control) this.txtEmail1, true, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) i_Tree_Eco_v6.Resources.Strings.EmailAddress));
      bool flag4 = this.txtPassword1.Text.Length > 0;
      if (string.IsNullOrEmpty(this.m_year.MobileKey))
      {
        bool flag5 = this.txtPassword2.Text.Length > 0;
        if (flag4 & flag5)
        {
          bool flag6 = this.txtPassword1.Text.Equals(this.txtPassword2.Text, StringComparison.CurrentCulture);
          this.ep.SetError((Control) this.txtPassword1, !flag6, i_Tree_Eco_v6.Resources.Strings.ErrPasswordNotMatch);
          this.ep.SetError((Control) this.txtPassword2, !flag6, i_Tree_Eco_v6.Resources.Strings.ErrPasswordNotMatch);
        }
        else
        {
          this.ep.SetError((Control) this.txtPassword1, !flag4, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) i_Tree_Eco_v6.Resources.Strings.Password));
          this.ep.SetError((Control) this.txtPassword2, flag4 && !flag5, i_Tree_Eco_v6.Resources.Strings.ErrConfirmPassword);
        }
      }
      else
        this.ep.SetError((Control) this.txtPassword1, !flag4, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) i_Tree_Eco_v6.Resources.Strings.Password));
      return !this.HasErrors(this.ep);
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
      if (!this.IsValid())
        return;
      this.tableLayoutPanel1.Enabled = false;
      this.m_year.MobileEmail = this.txtEmail1.Text;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      IList<Plot> selPlots;
      using (this.m_session.BeginTransaction())
        selPlots = this.m_session.CreateCriteria<Plot>().Add((ICriterion) Restrictions.In((IProjection) Projections.Id(), this.m_selPlots.ToArray<object>())).List<Plot>();
      this.m_taskManager.Add(Task.Factory.StartNew<int>((Func<int>) (() => this.UploadData(this.m_session, this.m_year, this.txtPassword1.Text, (IEnumerable<Plot>) selPlots)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<int>>) (_ =>
      {
        if (_.IsFaulted)
        {
          StringBuilder stringBuilder = new StringBuilder(_.Exception.InnerException.Message);
          stringBuilder.Append(Environment.NewLine);
          stringBuilder.Append(Environment.NewLine);
          stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.InternetErrorHelpMessage);
          int num = (int) MessageBox.Show((IWin32Window) this, stringBuilder.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.tableLayoutPanel1.Enabled = true;
        }
        else
        {
          switch (_.Result)
          {
            case 0:
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                this.m_session.SaveOrUpdate((object) this.m_year);
                StringBuilder stringBuilder = new StringBuilder();
                int num = 0;
                foreach (Plot plot in (IEnumerable<Plot>) selPlots)
                {
                  stringBuilder.Append(plot.Id);
                  if (++num < selPlots.Count)
                    stringBuilder.Append(", ");
                }
                this.m_session.SaveOrUpdate((object) new MobileLogEntry()
                {
                  Year = this.m_year,
                  Description = stringBuilder.ToString(),
                  MobileKey = this.m_year.MobileKey,
                  Submitted = true,
                  DateTime = DateTime.Now
                });
                transaction.Commit();
              }
              EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(this.m_year), (Control) this);
              this.Close();
              break;
            case 1:
              int num1 = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrInvalidPassword, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              break;
            default:
              int num2 = (int) MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.ErrUnexpectedRetry, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
              break;
          }
          this.tableLayoutPanel1.Enabled = true;
        }
      }), scheduler));
    }

    private void btnResetPassword_Click(object sender, EventArgs e)
    {
      this.tableLayoutPanel1.Enabled = false;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew<string>((Func<string>) (() => this.RequestPwdReset(this.m_session, this.m_year)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task<string>>) (t =>
      {
        string result = t.Result;
        if (string.IsNullOrEmpty(result))
        {
          using (Form form = (Form) new MobileResetPasswordForm())
          {
            int num = (int) form.ShowDialog((IWin32Window) this);
          }
        }
        else
        {
          StringBuilder stringBuilder = new StringBuilder(result);
          stringBuilder.Append(Environment.NewLine);
          stringBuilder.Append(Environment.NewLine);
          stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.InternetErrorHelpMessage);
          int num = (int) MessageBox.Show((IWin32Window) this, stringBuilder.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        this.tableLayoutPanel1.Enabled = true;
      }), scheduler));
    }

    private void dgPlots_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgPlots_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgPlots.NewRowIndex || this.m_plots == null || e.RowIndex >= this.m_plots.Count)
        return;
      DataGridViewColumn column = this.dgPlots.Columns[e.ColumnIndex];
      Plot plot = this.m_plots[e.RowIndex];
      FlatPlotView flatPlotView = this.m_fpv;
      if (flatPlotView == null || flatPlotView.Plot == null || flatPlotView.Plot.Id != plot.Id)
        flatPlotView = new FlatPlotView(this.m_session, plot);
      if (column == this.dcId)
      {
        if (flatPlotView.Id > 0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcStrata)
      {
        if (flatPlotView.Strata != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcStatus)
      {
        if (Enum.IsDefined(typeof (TreeStatus), (object) flatPlotView.Status))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcSpecies)
      {
        if (string.IsNullOrEmpty(flatPlotView.Species))
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
        }
        else
        {
          DataGridViewCell cell = this.dgPlots.Rows[e.RowIndex].Cells[e.ColumnIndex];
          if (cell.FormattedValue != null && !cell.FormattedValue.Equals((object) string.Empty))
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrSpeciesCode, (object) flatPlotView.Species);
        }
      }
      else if (column == this.dcLandUse)
      {
        if (flatPlotView.LandUse != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcDBH1)
      {
        if (Math.Abs(flatPlotView.DBH1 + 1.0) > double.Epsilon)
        {
          if (flatPlotView.DBH1 >= 0.5)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0.5);
        }
        else if (Math.Abs(flatPlotView.DBH1Height + 1.0) > double.Epsilon)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcDBH1Height.HeaderText, (object) column.HeaderText);
        }
        else
        {
          if (Math.Abs(flatPlotView.DBH2 + 1.0) > double.Epsilon || Math.Abs(flatPlotView.DBH3 + 1.0) > double.Epsilon || Math.Abs(flatPlotView.DBH4 + 1.0) > double.Epsilon || Math.Abs(flatPlotView.DBH5 + 1.0) > double.Epsilon || Math.Abs(flatPlotView.DBH6 + 1.0) > double.Epsilon)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
        }
      }
      else if (column == this.dcDBH1c)
      {
        if (Math.Abs(flatPlotView.DBH1 + 1.0) <= double.Epsilon || this.m_dbhs.ContainsKey(flatPlotView.DBH1))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrInvalidOption, (object) flatPlotView.DBH1.ToString("0.#"));
      }
      else if (column == this.dcDBH1Height)
      {
        if (Math.Abs(flatPlotView.DBH1Height + 1.0) <= double.Epsilon)
          return;
        if (flatPlotView.DBH1Height <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatPlotView.DBH1Height <= 6.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 6);
        }
      }
      else if (column == this.dcDBH2)
      {
        if (Math.Abs(flatPlotView.DBH2 + 1.0) > double.Epsilon)
        {
          if (flatPlotView.DBH2 >= 0.5)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0.5);
        }
        else
        {
          if (Math.Abs(flatPlotView.DBH2Height + 1.0) <= double.Epsilon)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcDBH2Height.HeaderText, (object) column.HeaderText);
        }
      }
      else if (column == this.dcDBH2c)
      {
        if (Math.Abs(flatPlotView.DBH2 + 1.0) <= double.Epsilon || this.m_dbhs.ContainsKey(flatPlotView.DBH2))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrInvalidOption, (object) flatPlotView.DBH2.ToString("0.#"));
      }
      else if (column == this.dcDBH2Height)
      {
        if (Math.Abs(flatPlotView.DBH2Height + 1.0) <= double.Epsilon)
          return;
        if (flatPlotView.DBH2Height <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatPlotView.DBH2Height <= 6.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 6);
        }
      }
      else if (column == this.dcDBH3)
      {
        if (Math.Abs(flatPlotView.DBH3 + 1.0) > double.Epsilon)
        {
          if (flatPlotView.DBH3 >= 0.5)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0.5);
        }
        else
        {
          if (Math.Abs(flatPlotView.DBH3Height + 1.0) <= double.Epsilon)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcDBH3Height.HeaderText, (object) column.HeaderText);
        }
      }
      else if (column == this.dcDBH3c)
      {
        if (Math.Abs(flatPlotView.DBH3 + 1.0) <= double.Epsilon || this.m_dbhs.ContainsKey(flatPlotView.DBH3))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrInvalidOption, (object) flatPlotView.DBH3.ToString("0.#"));
      }
      else if (column == this.dcDBH3Height)
      {
        if (Math.Abs(flatPlotView.DBH3Height + 1.0) <= double.Epsilon)
          return;
        if (flatPlotView.DBH3Height <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatPlotView.DBH3Height <= 6.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 6);
        }
      }
      else if (column == this.dcDBH4)
      {
        if (Math.Abs(flatPlotView.DBH4 + 1.0) > double.Epsilon)
        {
          if (flatPlotView.DBH4 >= 0.5)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0.5);
        }
        else
        {
          if (Math.Abs(flatPlotView.DBH4Height + 1.0) <= double.Epsilon)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcDBH4Height.HeaderText, (object) column.HeaderText);
        }
      }
      else if (column == this.dcDBH4c)
      {
        if (Math.Abs(flatPlotView.DBH4 + 1.0) <= double.Epsilon || this.m_dbhs.ContainsKey(flatPlotView.DBH4))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrInvalidOption, (object) flatPlotView.DBH4.ToString("0.#"));
      }
      else if (column == this.dcDBH4Height)
      {
        if (Math.Abs(flatPlotView.DBH4Height + 1.0) <= double.Epsilon)
          return;
        if (flatPlotView.DBH4Height <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatPlotView.DBH4Height <= 6.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 6);
        }
      }
      else if (column == this.dcDBH5)
      {
        if (Math.Abs(flatPlotView.DBH5 + 1.0) > double.Epsilon)
        {
          if (flatPlotView.DBH5 >= 0.5)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0.5);
        }
        else
        {
          if (Math.Abs(flatPlotView.DBH5Height + 1.0) <= double.Epsilon)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcDBH5Height.HeaderText, (object) column.HeaderText);
        }
      }
      else if (column == this.dcDBH5c)
      {
        if (Math.Abs(flatPlotView.DBH5 + 1.0) <= double.Epsilon || this.m_dbhs.ContainsKey(flatPlotView.DBH5))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrInvalidOption, (object) flatPlotView.DBH5.ToString("0.#"));
      }
      else if (column == this.dcDBH5Height)
      {
        if (Math.Abs(flatPlotView.DBH5Height + 1.0) <= double.Epsilon)
          return;
        if (flatPlotView.DBH5Height <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatPlotView.DBH5Height <= 6.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 6);
        }
      }
      else if (column == this.dcDBH6)
      {
        if (Math.Abs(flatPlotView.DBH6 + 1.0) > double.Epsilon)
        {
          if (flatPlotView.DBH6 >= 0.5)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0.5);
        }
        else
        {
          if (Math.Abs(flatPlotView.DBH6Height + 1.0) <= double.Epsilon)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcDBH6Height.HeaderText, (object) column.HeaderText);
        }
      }
      else if (column == this.dcDBH6c)
      {
        if (Math.Abs(flatPlotView.DBH6 + 1.0) <= double.Epsilon || this.m_dbhs.ContainsKey(flatPlotView.DBH6))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrInvalidOption, (object) flatPlotView.DBH6.ToString("0.#"));
      }
      else if (column == this.dcDBH6Height)
      {
        if (Math.Abs(flatPlotView.DBH6Height + 1.0) <= double.Epsilon)
          return;
        if (flatPlotView.DBH6Height <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatPlotView.DBH6Height <= 6.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 6);
        }
      }
      else if (column == this.dcTreeHeight)
      {
        if ((double) flatPlotView.Height < 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) flatPlotView.Height <= 450.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 450);
        }
      }
      else if (column == this.dcCrownCondition)
      {
        if (flatPlotView.CrownCondition != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcCrownDieback)
      {
        if (flatPlotView.CrownCondition != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcCrownTopHeight)
      {
        if (flatPlotView.CrownCondition == null || flatPlotView.IsDead)
          return;
        if ((double) flatPlotView.CrownTopHeight + 1.0 < 1.4012984643248171E-45)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
        else if ((double) flatPlotView.CrownTopHeight <= 0.0)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        else if (this.m_year.RecordHeight && (double) flatPlotView.Height > 0.0 && (double) flatPlotView.Height <= 450.0 && (double) flatPlotView.CrownTopHeight > (double) flatPlotView.Height)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) flatPlotView.Height.ToString("F2"));
        }
        else
        {
          if ((double) flatPlotView.CrownTopHeight <= 450.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 450);
        }
      }
      else if (column == this.dcCrownBaseHeight)
      {
        if (flatPlotView.CrownCondition == null || flatPlotView.IsDead)
          return;
        if ((double) flatPlotView.CrownBaseHeight + 1.0 < 1.4012984643248171E-45)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
        else if ((double) flatPlotView.CrownBaseHeight < 0.0)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        else if ((double) flatPlotView.CrownTopHeight > 0.0 && (double) flatPlotView.CrownTopHeight <= 450.0 && (double) flatPlotView.CrownBaseHeight > (double) flatPlotView.CrownTopHeight)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) flatPlotView.CrownTopHeight.ToString("F2"));
        else if (this.m_year.RecordHeight && (double) flatPlotView.Height > 0.0 && (double) flatPlotView.Height <= 450.0 && (double) flatPlotView.CrownBaseHeight > (double) flatPlotView.Height)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) flatPlotView.Height.ToString("F2"));
        }
        else
        {
          if ((double) flatPlotView.CrownBaseHeight <= 450.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 450);
        }
      }
      else if (column == this.dcCrownWidthNS)
      {
        if (flatPlotView.CrownCondition == null || flatPlotView.IsDead)
          return;
        if ((double) flatPlotView.CrownWidthNS + 1.0 < 1.4012984643248171E-45)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
        else if ((double) flatPlotView.CrownWidthNS <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) flatPlotView.CrownWidthNS <= 300.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 300);
        }
      }
      else if (column == this.dcCrownWidthEW)
      {
        if (flatPlotView.CrownCondition == null || flatPlotView.IsDead)
          return;
        if ((double) flatPlotView.CrownWidthEW + 1.0 < 1.4012984643248171E-45)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
        else if ((double) flatPlotView.CrownWidthEW <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) flatPlotView.CrownWidthEW <= 300.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 300);
        }
      }
      else if (column == this.dcCrownPercentMissing)
      {
        if (flatPlotView.CrownCondition == null || flatPlotView.IsDead || flatPlotView.CrownPercentMissing != PctMidRange.PRINV)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcBldg1Distance)
      {
        float num = this.m_year.Unit == YearUnit.English ? 60f : 18.29f;
        if ((double) Math.Abs(flatPlotView.B1Distance + 1f) < 1.4012984643248171E-45)
        {
          if (flatPlotView.B1Direction == (short) -1)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcBldg1Direction.HeaderText, (object) column.HeaderText);
        }
        else if ((double) flatPlotView.B1Distance < 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) flatPlotView.B1Distance <= (double) num)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) num);
        }
      }
      else if (column == this.dcBldg1Direction)
      {
        if (flatPlotView.B1Direction == (short) -1)
        {
          if ((double) Math.Abs(flatPlotView.B1Distance + 1f) <= 1.4012984643248171E-45)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcBldg1Distance.HeaderText, (object) column.HeaderText);
        }
        else if (flatPlotView.B1Direction < (short) 0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatPlotView.B1Direction <= (short) 360)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 360);
        }
      }
      else if (column == this.dcBldg2Distance)
      {
        float num = this.m_year.Unit == YearUnit.English ? 60f : 18.29f;
        if ((double) Math.Abs(flatPlotView.B2Distance + 1f) < 1.4012984643248171E-45)
        {
          if (flatPlotView.B2Direction == (short) -1)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcBldg2Direction.HeaderText, (object) column.HeaderText);
        }
        else if ((double) flatPlotView.B2Distance < 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) flatPlotView.B2Distance <= (double) num)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) num);
        }
      }
      else if (column == this.dcBldg2Direction)
      {
        if (flatPlotView.B2Direction == (short) -1)
        {
          if ((double) Math.Abs(flatPlotView.B2Distance + 1f) <= 1.4012984643248171E-45)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcBldg2Distance.HeaderText, (object) column.HeaderText);
        }
        else if (flatPlotView.B2Direction < (short) 0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatPlotView.B2Direction <= (short) 360)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 360);
        }
      }
      else if (column == this.dcBldg3Distance)
      {
        float num = this.m_year.Unit == YearUnit.English ? 60f : 18.29f;
        if ((double) Math.Abs(flatPlotView.B3Distance + 1f) < 1.4012984643248171E-45)
        {
          if (flatPlotView.B3Direction == (short) -1)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcBldg3Direction.HeaderText, (object) column.HeaderText);
        }
        else if ((double) flatPlotView.B3Distance < 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) flatPlotView.B3Distance <= (double) num)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) num);
        }
      }
      else if (column == this.dcBldg3Direction)
      {
        if (flatPlotView.B3Direction == (short) -1)
        {
          if ((double) Math.Abs(flatPlotView.B3Distance + 1f) <= 1.4012984643248171E-45)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcBldg3Distance.HeaderText, (object) column.HeaderText);
        }
        else if (flatPlotView.B3Direction < (short) 0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatPlotView.B3Direction <= (short) 360)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 360);
        }
      }
      else
      {
        if (column != this.dcIPEDPest)
          return;
        if (flatPlotView.IPEDPest == 0 && (flatPlotView.IPEDTSDieback != 0 || flatPlotView.IPEDTSEpiSprout != 0 || flatPlotView.IPEDTSWiltFoli != 0 || flatPlotView.IPEDTSEnvStress != 0 || flatPlotView.IPEDTSHumStress != 0 || !string.IsNullOrEmpty(flatPlotView.IPEDTSNotes) || flatPlotView.IPEDBBAbnGrowth != 0 || flatPlotView.IPEDBBDiseaseSigns != 0 || flatPlotView.IPEDBBInsectPres != 0 || flatPlotView.IPEDBBInsectSigns != 0 || flatPlotView.IPEDBBProbLoc != 0 || !string.IsNullOrEmpty(flatPlotView.IPEDBBNotes) || flatPlotView.IPEDFTChewFoli != 0 || flatPlotView.IPEDFTDiscFoli != 0 || flatPlotView.IPEDFTAbnFoli != 0 || flatPlotView.IPEDFTInsectSigns != 0 || flatPlotView.IPEDFTFoliAffect != 0 || !string.IsNullOrEmpty(flatPlotView.IPEDFTNotes)))
        {
          e.ErrorText = i_Tree_Eco_v6.Resources.Strings.ErrIPEDPest;
        }
        else
        {
          if (flatPlotView.IPEDPest == 0 || flatPlotView.IPEDTSDieback != 0 || flatPlotView.IPEDTSEnvStress != 0 || flatPlotView.IPEDTSEpiSprout != 0 || flatPlotView.IPEDTSHumStress != 0 || flatPlotView.IPEDTSWiltFoli != 0 || !string.IsNullOrEmpty(flatPlotView.IPEDTSNotes) || flatPlotView.IPEDBBAbnGrowth != 0 || flatPlotView.IPEDBBDiseaseSigns != 0 || flatPlotView.IPEDBBInsectPres != 0 || flatPlotView.IPEDBBInsectSigns != 0 || flatPlotView.IPEDBBProbLoc != 0 || !string.IsNullOrEmpty(flatPlotView.IPEDBBNotes) || flatPlotView.IPEDFTAbnFoli != 0 || flatPlotView.IPEDFTChewFoli != 0 || flatPlotView.IPEDFTDiscFoli != 0 || flatPlotView.IPEDFTFoliAffect != 0 || flatPlotView.IPEDFTInsectSigns != 0 || !string.IsNullOrEmpty(flatPlotView.IPEDFTNotes))
            return;
          e.ErrorText = i_Tree_Eco_v6.Resources.Strings.ErrIPEDSymptoms;
        }
      }
    }

    private void panel1_Paint(object sender, PaintEventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MobileSubmitInventoryForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle7 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle8 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle9 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle10 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle11 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle12 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle13 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle14 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle15 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle16 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle17 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle18 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle19 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle20 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle21 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle22 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle23 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle24 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle25 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle26 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle27 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle28 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle29 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle30 = new DataGridViewCellStyle();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.panel1 = new TableLayoutPanel();
      this.dgPlots = new DataGridView();
      this.dcSelect = new DataGridViewCheckBoxColumn();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcUserId = new DataGridViewTextBoxColumn();
      this.dcStrata = new DataGridViewComboBoxColumn();
      this.dcCrew = new DataGridViewTextBoxColumn();
      this.dcSurveyDate = new DataGridViewNullableDateTimeColumn();
      this.dcStatus = new DataGridViewComboBoxColumn();
      this.dcSpecies = new DataGridViewFilteredComboBoxColumn();
      this.dcStreet = new DataGridViewComboBoxColumn();
      this.dcAddress = new DataGridViewTextBoxColumn();
      this.dcLandUse = new DataGridViewComboBoxColumn();
      this.dcPhoto = new DataGridViewTextBoxColumn();
      this.dcDBH1 = new DataGridViewNumericTextBoxColumn();
      this.dcDBH1c = new DataGridViewComboBoxColumn();
      this.dcDBH1Height = new DataGridViewNumericTextBoxColumn();
      this.dcDBH1Measured = new DataGridViewCheckBoxColumn();
      this.dcDBH2 = new DataGridViewNumericTextBoxColumn();
      this.dcDBH2c = new DataGridViewComboBoxColumn();
      this.dcDBH2Height = new DataGridViewNumericTextBoxColumn();
      this.dcDBH2Measured = new DataGridViewCheckBoxColumn();
      this.dcDBH3 = new DataGridViewNumericTextBoxColumn();
      this.dcDBH3c = new DataGridViewComboBoxColumn();
      this.dcDBH3Height = new DataGridViewNumericTextBoxColumn();
      this.dcDBH3Measured = new DataGridViewCheckBoxColumn();
      this.dcDBH4 = new DataGridViewNumericTextBoxColumn();
      this.dcDBH4c = new DataGridViewComboBoxColumn();
      this.dcDBH4Height = new DataGridViewNumericTextBoxColumn();
      this.dcDBH4Measured = new DataGridViewCheckBoxColumn();
      this.dcDBH5 = new DataGridViewNumericTextBoxColumn();
      this.dcDBH5c = new DataGridViewComboBoxColumn();
      this.dcDBH5Height = new DataGridViewNumericTextBoxColumn();
      this.dcDBH5Measured = new DataGridViewCheckBoxColumn();
      this.dcDBH6 = new DataGridViewNumericTextBoxColumn();
      this.dcDBH6c = new DataGridViewComboBoxColumn();
      this.dcDBH6Height = new DataGridViewNumericTextBoxColumn();
      this.dcDBH6Measured = new DataGridViewCheckBoxColumn();
      this.dcCrownCondition = new DataGridViewComboBoxColumn();
      this.dcCrownDieback = new DataGridViewComboBoxColumn();
      this.dcTreeHeight = new DataGridViewNumericTextBoxColumn();
      this.dcCrownTopHeight = new DataGridViewNumericTextBoxColumn();
      this.dcCrownBaseHeight = new DataGridViewNumericTextBoxColumn();
      this.dcCrownWidthNS = new DataGridViewNumericTextBoxColumn();
      this.dcCrownWidthEW = new DataGridViewNumericTextBoxColumn();
      this.dcCrownPercentMissing = new DataGridViewComboBoxColumn();
      this.dcCrownLightExposure = new DataGridViewComboBoxColumn();
      this.dcBldg1Direction = new DataGridViewNumericTextBoxColumn();
      this.dcBldg1Distance = new DataGridViewNumericTextBoxColumn();
      this.dcBldg2Direction = new DataGridViewNumericTextBoxColumn();
      this.dcBldg2Distance = new DataGridViewNumericTextBoxColumn();
      this.dcBldg3Direction = new DataGridViewNumericTextBoxColumn();
      this.dcBldg3Distance = new DataGridViewNumericTextBoxColumn();
      this.dcSiteType = new DataGridViewComboBoxColumn();
      this.dcStTree = new DataGridViewCheckBoxColumn();
      this.dcMaintRec = new DataGridViewComboBoxColumn();
      this.dcMaintTask = new DataGridViewComboBoxColumn();
      this.dcSidewalk = new DataGridViewComboBoxColumn();
      this.dcWireConflict = new DataGridViewComboBoxColumn();
      this.dcFieldOne = new DataGridViewComboBoxColumn();
      this.dcFieldTwo = new DataGridViewComboBoxColumn();
      this.dcFieldThree = new DataGridViewComboBoxColumn();
      this.dcIPEDTSDieback = new DataGridViewComboBoxColumn();
      this.dcIPEDTSEpiSprout = new DataGridViewComboBoxColumn();
      this.dcIPEDTSWiltFoli = new DataGridViewComboBoxColumn();
      this.dcIPEDTSEnvStress = new DataGridViewComboBoxColumn();
      this.dcIPEDTSHumStress = new DataGridViewComboBoxColumn();
      this.dcIPEDTSNotes = new DataGridViewTextBoxColumn();
      this.dcIPEDFTChewFoli = new DataGridViewComboBoxColumn();
      this.dcIPEDFTDiscFoli = new DataGridViewComboBoxColumn();
      this.dcIPEDFTAbnFoli = new DataGridViewComboBoxColumn();
      this.dcIPEDFTInsectSigns = new DataGridViewComboBoxColumn();
      this.dcIPEDFTFoliAffect = new DataGridViewComboBoxColumn();
      this.dcIPEDFTNotes = new DataGridViewTextBoxColumn();
      this.dcIPEDBBInsectSigns = new DataGridViewComboBoxColumn();
      this.dcIPEDBBInsectPres = new DataGridViewComboBoxColumn();
      this.dcIPEDBBDiseaseSigns = new DataGridViewComboBoxColumn();
      this.dcIPEDBBProbLoc = new DataGridViewComboBoxColumn();
      this.dcIPEDBBAbnGrowth = new DataGridViewComboBoxColumn();
      this.dcIPEDBBNotes = new DataGridViewTextBoxColumn();
      this.dcIPEDPest = new DataGridViewComboBoxColumn();
      this.dcCityManaged = new DataGridViewCheckBoxColumn();
      this.dcLocSite = new DataGridViewComboBoxColumn();
      this.dcLocNo = new DataGridViewNumericTextBoxColumn();
      this.dcLatitude = new DataGridViewTextBoxColumn();
      this.dcLongitude = new DataGridViewTextBoxColumn();
      this.dcNoteTree = new DataGridViewCheckBoxColumn();
      this.dcComments = new DataGridViewTextBoxColumn();
      this.lblSelTrees = new Label();
      this.chkAllTrees = new CheckBox();
      this.tableLayoutPanel2 = new TableLayoutPanel();
      this.lblEmail1 = new Label();
      this.txtEmail1 = new TextBox();
      this.txtEmail2 = new TextBox();
      this.txtPassword2 = new TextBox();
      this.lblPassword2 = new Label();
      this.lblEmail2 = new Label();
      this.lblPassword1 = new Label();
      this.txtPassword1 = new TextBox();
      this.Label3 = new Label();
      this.btnResetPassword = new Button();
      this.btnSubmit = new Button();
      this.lblResetPassword = new Label();
      this.label2 = new Label();
      this.dgSubmitLog = new DataGridView();
      this.dcMobileKey = new DataGridViewTextBoxColumn();
      this.dcDateTime = new DataGridViewNullableDateTimeColumn();
      this.dcDescription = new DataGridViewTextBoxColumn();
      this.label1 = new Label();
      ((ISupportInitialize) this.ep).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.dgPlots).BeginInit();
      this.tableLayoutPanel2.SuspendLayout();
      ((ISupportInitialize) this.dgSubmitLog).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.panel1, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel2, 1, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.label2, 1, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.dgSubmitLog, 1, 2);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.Controls.Add((Control) this.dgPlots, 0, 1);
      this.panel1.Controls.Add((Control) this.lblSelTrees, 1, 0);
      this.panel1.Controls.Add((Control) this.chkAllTrees, 0, 0);
      this.panel1.Name = "panel1";
      this.tableLayoutPanel1.SetRowSpan((Control) this.panel1, 3);
      this.panel1.Paint += new PaintEventHandler(this.panel1_Paint);
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
      this.dgPlots.Columns.AddRange((DataGridViewColumn) this.dcSelect, (DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcUserId, (DataGridViewColumn) this.dcStrata, (DataGridViewColumn) this.dcCrew, (DataGridViewColumn) this.dcSurveyDate, (DataGridViewColumn) this.dcStatus, (DataGridViewColumn) this.dcSpecies, (DataGridViewColumn) this.dcStreet, (DataGridViewColumn) this.dcAddress, (DataGridViewColumn) this.dcLandUse, (DataGridViewColumn) this.dcPhoto, (DataGridViewColumn) this.dcDBH1, (DataGridViewColumn) this.dcDBH1c, (DataGridViewColumn) this.dcDBH1Height, (DataGridViewColumn) this.dcDBH1Measured, (DataGridViewColumn) this.dcDBH2, (DataGridViewColumn) this.dcDBH2c, (DataGridViewColumn) this.dcDBH2Height, (DataGridViewColumn) this.dcDBH2Measured, (DataGridViewColumn) this.dcDBH3, (DataGridViewColumn) this.dcDBH3c, (DataGridViewColumn) this.dcDBH3Height, (DataGridViewColumn) this.dcDBH3Measured, (DataGridViewColumn) this.dcDBH4, (DataGridViewColumn) this.dcDBH4c, (DataGridViewColumn) this.dcDBH4Height, (DataGridViewColumn) this.dcDBH4Measured, (DataGridViewColumn) this.dcDBH5, (DataGridViewColumn) this.dcDBH5c, (DataGridViewColumn) this.dcDBH5Height, (DataGridViewColumn) this.dcDBH5Measured, (DataGridViewColumn) this.dcDBH6, (DataGridViewColumn) this.dcDBH6c, (DataGridViewColumn) this.dcDBH6Height, (DataGridViewColumn) this.dcDBH6Measured, (DataGridViewColumn) this.dcCrownCondition, (DataGridViewColumn) this.dcCrownDieback, (DataGridViewColumn) this.dcTreeHeight, (DataGridViewColumn) this.dcCrownTopHeight, (DataGridViewColumn) this.dcCrownBaseHeight, (DataGridViewColumn) this.dcCrownWidthNS, (DataGridViewColumn) this.dcCrownWidthEW, (DataGridViewColumn) this.dcCrownPercentMissing, (DataGridViewColumn) this.dcCrownLightExposure, (DataGridViewColumn) this.dcBldg1Direction, (DataGridViewColumn) this.dcBldg1Distance, (DataGridViewColumn) this.dcBldg2Direction, (DataGridViewColumn) this.dcBldg2Distance, (DataGridViewColumn) this.dcBldg3Direction, (DataGridViewColumn) this.dcBldg3Distance, (DataGridViewColumn) this.dcSiteType, (DataGridViewColumn) this.dcStTree, (DataGridViewColumn) this.dcMaintRec, (DataGridViewColumn) this.dcMaintTask, (DataGridViewColumn) this.dcSidewalk, (DataGridViewColumn) this.dcWireConflict, (DataGridViewColumn) this.dcFieldOne, (DataGridViewColumn) this.dcFieldTwo, (DataGridViewColumn) this.dcFieldThree, (DataGridViewColumn) this.dcIPEDTSDieback, (DataGridViewColumn) this.dcIPEDTSEpiSprout, (DataGridViewColumn) this.dcIPEDTSWiltFoli, (DataGridViewColumn) this.dcIPEDTSEnvStress, (DataGridViewColumn) this.dcIPEDTSHumStress, (DataGridViewColumn) this.dcIPEDTSNotes, (DataGridViewColumn) this.dcIPEDFTChewFoli, (DataGridViewColumn) this.dcIPEDFTDiscFoli, (DataGridViewColumn) this.dcIPEDFTAbnFoli, (DataGridViewColumn) this.dcIPEDFTInsectSigns, (DataGridViewColumn) this.dcIPEDFTFoliAffect, (DataGridViewColumn) this.dcIPEDFTNotes, (DataGridViewColumn) this.dcIPEDBBInsectSigns, (DataGridViewColumn) this.dcIPEDBBInsectPres, (DataGridViewColumn) this.dcIPEDBBDiseaseSigns, (DataGridViewColumn) this.dcIPEDBBProbLoc, (DataGridViewColumn) this.dcIPEDBBAbnGrowth, (DataGridViewColumn) this.dcIPEDBBNotes, (DataGridViewColumn) this.dcIPEDPest, (DataGridViewColumn) this.dcCityManaged, (DataGridViewColumn) this.dcLocSite, (DataGridViewColumn) this.dcLocNo, (DataGridViewColumn) this.dcLatitude, (DataGridViewColumn) this.dcLongitude, (DataGridViewColumn) this.dcNoteTree, (DataGridViewColumn) this.dcComments);
      this.panel1.SetColumnSpan((Control) this.dgPlots, 2);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgPlots.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgPlots, "dgPlots");
      this.dgPlots.EditMode = DataGridViewEditMode.EditProgrammatically;
      this.dgPlots.EnableHeadersVisualStyles = false;
      this.dgPlots.MultiSelect = false;
      this.dgPlots.Name = "dgPlots";
      this.dgPlots.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgPlots.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgPlots.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgPlots.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgPlots.VirtualMode = true;
      this.dgPlots.CellClick += new DataGridViewCellEventHandler(this.dgPlots_CellClick);
      this.dgPlots.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgPlots_CellErrorTextNeeded);
      this.dgPlots.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dgPlots_CellValueNeeded);
      this.dgPlots.DataError += new DataGridViewDataErrorEventHandler(this.dgPlots_DataError);
      this.dcSelect.Frozen = true;
      componentResourceManager.ApplyResources((object) this.dcSelect, "dcSelect");
      this.dcSelect.Name = "dcSelect";
      this.dcId.DataPropertyName = "Id";
      this.dcId.DecimalPlaces = 0;
      this.dcId.Format = (string) null;
      this.dcId.Frozen = true;
      this.dcId.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcId, "dcId");
      this.dcId.Name = "dcId";
      this.dcId.Resizable = DataGridViewTriState.True;
      this.dcId.Signed = false;
      this.dcUserId.DataPropertyName = "UserId";
      this.dcUserId.Frozen = true;
      componentResourceManager.ApplyResources((object) this.dcUserId, "dcUserId");
      this.dcUserId.Name = "dcUserId";
      this.dcStrata.DataPropertyName = "Strata";
      this.dcStrata.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcStrata, "dcStrata");
      this.dcStrata.Name = "dcStrata";
      this.dcStrata.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcCrew.DataPropertyName = "Crew";
      componentResourceManager.ApplyResources((object) this.dcCrew, "dcCrew");
      this.dcCrew.Name = "dcCrew";
      this.dcSurveyDate.CustomFormat = (string) null;
      this.dcSurveyDate.DataPropertyName = "SurveyDate";
      this.dcSurveyDate.DateFormat = DateTimePickerFormat.Short;
      componentResourceManager.ApplyResources((object) this.dcSurveyDate, "dcSurveyDate");
      this.dcSurveyDate.Name = "dcSurveyDate";
      this.dcStatus.DataPropertyName = "Status";
      this.dcStatus.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcStatus, "dcStatus");
      this.dcStatus.Name = "dcStatus";
      this.dcStatus.Resizable = DataGridViewTriState.True;
      this.dcStatus.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcSpecies.DataPropertyName = "Species";
      this.dcSpecies.DataSource = (object) null;
      this.dcSpecies.DisplayMember = (string) null;
      componentResourceManager.ApplyResources((object) this.dcSpecies, "dcSpecies");
      this.dcSpecies.Name = "dcSpecies";
      this.dcSpecies.Resizable = DataGridViewTriState.True;
      this.dcSpecies.ValueMember = (string) null;
      this.dcStreet.DataPropertyName = "Street";
      componentResourceManager.ApplyResources((object) this.dcStreet, "dcStreet");
      this.dcStreet.Name = "dcStreet";
      this.dcStreet.Resizable = DataGridViewTriState.True;
      this.dcStreet.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcAddress.DataPropertyName = "Address";
      componentResourceManager.ApplyResources((object) this.dcAddress, "dcAddress");
      this.dcAddress.Name = "dcAddress";
      this.dcLandUse.DataPropertyName = "LandUse";
      this.dcLandUse.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcLandUse, "dcLandUse");
      this.dcLandUse.Name = "dcLandUse";
      this.dcLandUse.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPhoto.DataPropertyName = "Photo";
      componentResourceManager.ApplyResources((object) this.dcPhoto, "dcPhoto");
      this.dcPhoto.MaxInputLength = 100;
      this.dcPhoto.Name = "dcPhoto";
      this.dcDBH1.DataPropertyName = "DBH1";
      this.dcDBH1.DecimalPlaces = 1;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle3.Format = "N1";
      this.dcDBH1.DefaultCellStyle = gridViewCellStyle3;
      this.dcDBH1.Format = "#;#";
      this.dcDBH1.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH1, "dcDBH1");
      this.dcDBH1.Name = "dcDBH1";
      this.dcDBH1.Resizable = DataGridViewTriState.True;
      this.dcDBH1.Signed = false;
      this.dcDBH1c.DataPropertyName = "DBH1";
      this.dcDBH1c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcDBH1c, "dcDBH1c");
      this.dcDBH1c.Name = "dcDBH1c";
      this.dcDBH1c.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH1Height.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH1Height.DataPropertyName = "DBH1Height";
      this.dcDBH1Height.DecimalPlaces = 2;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle4.Format = "N2";
      this.dcDBH1Height.DefaultCellStyle = gridViewCellStyle4;
      this.dcDBH1Height.Format = "#;#";
      this.dcDBH1Height.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH1Height, "dcDBH1Height");
      this.dcDBH1Height.Name = "dcDBH1Height";
      this.dcDBH1Height.Resizable = DataGridViewTriState.True;
      this.dcDBH1Height.Signed = false;
      this.dcDBH1Measured.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH1Measured.DataPropertyName = "DBH1Measured";
      componentResourceManager.ApplyResources((object) this.dcDBH1Measured, "dcDBH1Measured");
      this.dcDBH1Measured.Name = "dcDBH1Measured";
      this.dcDBH1Measured.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH2.DataPropertyName = "DBH2";
      this.dcDBH2.DecimalPlaces = 1;
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle5.Format = "N1";
      this.dcDBH2.DefaultCellStyle = gridViewCellStyle5;
      this.dcDBH2.Format = "#;#";
      this.dcDBH2.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH2, "dcDBH2");
      this.dcDBH2.Name = "dcDBH2";
      this.dcDBH2.Resizable = DataGridViewTriState.True;
      this.dcDBH2.Signed = false;
      this.dcDBH2c.DataPropertyName = "DBH2";
      this.dcDBH2c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcDBH2c, "dcDBH2c");
      this.dcDBH2c.Name = "dcDBH2c";
      this.dcDBH2c.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH2Height.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH2Height.DataPropertyName = "DBH2Height";
      this.dcDBH2Height.DecimalPlaces = 2;
      gridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle6.Format = "N2";
      this.dcDBH2Height.DefaultCellStyle = gridViewCellStyle6;
      this.dcDBH2Height.Format = "#;#";
      this.dcDBH2Height.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH2Height, "dcDBH2Height");
      this.dcDBH2Height.Name = "dcDBH2Height";
      this.dcDBH2Height.Resizable = DataGridViewTriState.True;
      this.dcDBH2Height.Signed = false;
      this.dcDBH2Measured.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH2Measured.DataPropertyName = "DBH2Measured";
      componentResourceManager.ApplyResources((object) this.dcDBH2Measured, "dcDBH2Measured");
      this.dcDBH2Measured.Name = "dcDBH2Measured";
      this.dcDBH2Measured.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH3.DataPropertyName = "DBH3";
      this.dcDBH3.DecimalPlaces = 1;
      gridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle7.Format = "N1";
      this.dcDBH3.DefaultCellStyle = gridViewCellStyle7;
      this.dcDBH3.Format = "#;#";
      this.dcDBH3.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH3, "dcDBH3");
      this.dcDBH3.Name = "dcDBH3";
      this.dcDBH3.Resizable = DataGridViewTriState.True;
      this.dcDBH3.Signed = false;
      this.dcDBH3c.DataPropertyName = "DBH3";
      this.dcDBH3c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcDBH3c, "dcDBH3c");
      this.dcDBH3c.Name = "dcDBH3c";
      this.dcDBH3c.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH3Height.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH3Height.DataPropertyName = "DBH3Height";
      this.dcDBH3Height.DecimalPlaces = 2;
      gridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle8.Format = "N2";
      this.dcDBH3Height.DefaultCellStyle = gridViewCellStyle8;
      this.dcDBH3Height.Format = "#;#";
      this.dcDBH3Height.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH3Height, "dcDBH3Height");
      this.dcDBH3Height.Name = "dcDBH3Height";
      this.dcDBH3Height.Resizable = DataGridViewTriState.True;
      this.dcDBH3Height.Signed = false;
      this.dcDBH3Measured.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH3Measured.DataPropertyName = "DBH3Measured";
      componentResourceManager.ApplyResources((object) this.dcDBH3Measured, "dcDBH3Measured");
      this.dcDBH3Measured.Name = "dcDBH3Measured";
      this.dcDBH3Measured.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH4.DataPropertyName = "DBH4";
      this.dcDBH4.DecimalPlaces = 1;
      gridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle9.Format = "N1";
      this.dcDBH4.DefaultCellStyle = gridViewCellStyle9;
      this.dcDBH4.Format = "#;#";
      this.dcDBH4.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH4, "dcDBH4");
      this.dcDBH4.Name = "dcDBH4";
      this.dcDBH4.Signed = false;
      this.dcDBH4c.DataPropertyName = "DBH4";
      this.dcDBH4c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcDBH4c, "dcDBH4c");
      this.dcDBH4c.Name = "dcDBH4c";
      this.dcDBH4c.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH4Height.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH4Height.DataPropertyName = "DBH4Height";
      this.dcDBH4Height.DecimalPlaces = 2;
      gridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle10.Format = "N2";
      this.dcDBH4Height.DefaultCellStyle = gridViewCellStyle10;
      this.dcDBH4Height.Format = "#;#";
      this.dcDBH4Height.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH4Height, "dcDBH4Height");
      this.dcDBH4Height.Name = "dcDBH4Height";
      this.dcDBH4Height.Signed = false;
      this.dcDBH4Measured.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH4Measured.DataPropertyName = "DBH4Measured";
      componentResourceManager.ApplyResources((object) this.dcDBH4Measured, "dcDBH4Measured");
      this.dcDBH4Measured.Name = "dcDBH4Measured";
      this.dcDBH4Measured.Resizable = DataGridViewTriState.True;
      this.dcDBH4Measured.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH5.DataPropertyName = "DBH5";
      this.dcDBH5.DecimalPlaces = 1;
      gridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle11.Format = "N1";
      this.dcDBH5.DefaultCellStyle = gridViewCellStyle11;
      this.dcDBH5.Format = "#;#";
      this.dcDBH5.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH5, "dcDBH5");
      this.dcDBH5.Name = "dcDBH5";
      this.dcDBH5.Signed = false;
      this.dcDBH5c.DataPropertyName = "DBH5";
      this.dcDBH5c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcDBH5c, "dcDBH5c");
      this.dcDBH5c.Name = "dcDBH5c";
      this.dcDBH5c.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH5Height.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH5Height.DataPropertyName = "DBH5Height";
      this.dcDBH5Height.DecimalPlaces = 2;
      gridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle12.Format = "N2";
      this.dcDBH5Height.DefaultCellStyle = gridViewCellStyle12;
      this.dcDBH5Height.Format = "#;#";
      this.dcDBH5Height.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH5Height, "dcDBH5Height");
      this.dcDBH5Height.Name = "dcDBH5Height";
      this.dcDBH5Height.Signed = false;
      this.dcDBH5Measured.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH5Measured.DataPropertyName = "DBH5Measured";
      componentResourceManager.ApplyResources((object) this.dcDBH5Measured, "dcDBH5Measured");
      this.dcDBH5Measured.Name = "dcDBH5Measured";
      this.dcDBH5Measured.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH6.DataPropertyName = "DBH6";
      this.dcDBH6.DecimalPlaces = 1;
      gridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle13.Format = "N1";
      this.dcDBH6.DefaultCellStyle = gridViewCellStyle13;
      this.dcDBH6.Format = "#;#";
      this.dcDBH6.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH6, "dcDBH6");
      this.dcDBH6.Name = "dcDBH6";
      this.dcDBH6.Signed = false;
      this.dcDBH6c.DataPropertyName = "DBH6";
      this.dcDBH6c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcDBH6c, "dcDBH6c");
      this.dcDBH6c.Name = "dcDBH6c";
      this.dcDBH6c.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH6Height.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH6Height.DataPropertyName = "DBH6Height";
      this.dcDBH6Height.DecimalPlaces = 2;
      gridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle14.Format = "N2";
      this.dcDBH6Height.DefaultCellStyle = gridViewCellStyle14;
      this.dcDBH6Height.Format = "#;#";
      this.dcDBH6Height.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH6Height, "dcDBH6Height");
      this.dcDBH6Height.Name = "dcDBH6Height";
      this.dcDBH6Height.Signed = false;
      this.dcDBH6Measured.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH6Measured.DataPropertyName = "DBH6Measured";
      componentResourceManager.ApplyResources((object) this.dcDBH6Measured, "dcDBH6Measured");
      this.dcDBH6Measured.Name = "dcDBH6Measured";
      this.dcDBH6Measured.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcCrownCondition.DataPropertyName = "CrownCondition";
      this.dcCrownCondition.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcCrownCondition, "dcCrownCondition");
      this.dcCrownCondition.Name = "dcCrownCondition";
      this.dcCrownCondition.Resizable = DataGridViewTriState.True;
      this.dcCrownCondition.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcCrownDieback.DataPropertyName = "CrownCondition";
      this.dcCrownDieback.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcCrownDieback, "dcCrownDieback");
      this.dcCrownDieback.Name = "dcCrownDieback";
      this.dcCrownDieback.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcTreeHeight.DataPropertyName = "Height";
      this.dcTreeHeight.DecimalPlaces = 2;
      gridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle15.Format = "N2";
      this.dcTreeHeight.DefaultCellStyle = gridViewCellStyle15;
      this.dcTreeHeight.Format = (string) null;
      this.dcTreeHeight.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcTreeHeight, "dcTreeHeight");
      this.dcTreeHeight.Name = "dcTreeHeight";
      this.dcTreeHeight.Resizable = DataGridViewTriState.True;
      this.dcTreeHeight.Signed = false;
      this.dcCrownTopHeight.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownTopHeight.DataPropertyName = "CrownTopHeight";
      this.dcCrownTopHeight.DecimalPlaces = 2;
      gridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle16.Format = "N2";
      this.dcCrownTopHeight.DefaultCellStyle = gridViewCellStyle16;
      this.dcCrownTopHeight.Format = "#;#";
      this.dcCrownTopHeight.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownTopHeight, "dcCrownTopHeight");
      this.dcCrownTopHeight.Name = "dcCrownTopHeight";
      this.dcCrownTopHeight.Signed = false;
      this.dcCrownBaseHeight.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownBaseHeight.DataPropertyName = "CrownBaseHeight";
      this.dcCrownBaseHeight.DecimalPlaces = 2;
      gridViewCellStyle17.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle17.Format = "N2";
      this.dcCrownBaseHeight.DefaultCellStyle = gridViewCellStyle17;
      this.dcCrownBaseHeight.Format = "#;#";
      this.dcCrownBaseHeight.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownBaseHeight, "dcCrownBaseHeight");
      this.dcCrownBaseHeight.Name = "dcCrownBaseHeight";
      this.dcCrownBaseHeight.Signed = false;
      this.dcCrownWidthNS.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownWidthNS.DataPropertyName = "CrownWidthNS";
      this.dcCrownWidthNS.DecimalPlaces = 1;
      gridViewCellStyle18.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle18.Format = "N1";
      this.dcCrownWidthNS.DefaultCellStyle = gridViewCellStyle18;
      this.dcCrownWidthNS.Format = "#;#";
      this.dcCrownWidthNS.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownWidthNS, "dcCrownWidthNS");
      this.dcCrownWidthNS.Name = "dcCrownWidthNS";
      this.dcCrownWidthNS.Signed = false;
      this.dcCrownWidthEW.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownWidthEW.DataPropertyName = "CrownWidthEW";
      this.dcCrownWidthEW.DecimalPlaces = 1;
      gridViewCellStyle19.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle19.Format = "N1";
      this.dcCrownWidthEW.DefaultCellStyle = gridViewCellStyle19;
      this.dcCrownWidthEW.Format = "#;#";
      this.dcCrownWidthEW.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownWidthEW, "dcCrownWidthEW");
      this.dcCrownWidthEW.Name = "dcCrownWidthEW";
      this.dcCrownWidthEW.Signed = false;
      this.dcCrownPercentMissing.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownPercentMissing.DataPropertyName = "CrownPercentMissing";
      gridViewCellStyle20.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcCrownPercentMissing.DefaultCellStyle = gridViewCellStyle20;
      this.dcCrownPercentMissing.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcCrownPercentMissing, "dcCrownPercentMissing");
      this.dcCrownPercentMissing.Name = "dcCrownPercentMissing";
      this.dcCrownPercentMissing.Resizable = DataGridViewTriState.True;
      this.dcCrownPercentMissing.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcCrownLightExposure.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownLightExposure.DataPropertyName = "CrownLightExposure";
      this.dcCrownLightExposure.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcCrownLightExposure, "dcCrownLightExposure");
      this.dcCrownLightExposure.Name = "dcCrownLightExposure";
      this.dcCrownLightExposure.Resizable = DataGridViewTriState.True;
      this.dcCrownLightExposure.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcBldg1Direction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg1Direction.DataPropertyName = "B1Direction";
      this.dcBldg1Direction.DecimalPlaces = 0;
      gridViewCellStyle21.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle21.Format = "N0";
      this.dcBldg1Direction.DefaultCellStyle = gridViewCellStyle21;
      this.dcBldg1Direction.Format = "#;#";
      this.dcBldg1Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg1Direction, "dcBldg1Direction");
      this.dcBldg1Direction.Name = "dcBldg1Direction";
      this.dcBldg1Direction.Signed = false;
      this.dcBldg1Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg1Distance.DataPropertyName = "B1Distance";
      this.dcBldg1Distance.DecimalPlaces = 2;
      gridViewCellStyle22.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle22.Format = "N2";
      this.dcBldg1Distance.DefaultCellStyle = gridViewCellStyle22;
      this.dcBldg1Distance.Format = "#;#";
      this.dcBldg1Distance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcBldg1Distance, "dcBldg1Distance");
      this.dcBldg1Distance.Name = "dcBldg1Distance";
      this.dcBldg1Distance.Signed = false;
      this.dcBldg2Direction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg2Direction.DataPropertyName = "B2Direction";
      this.dcBldg2Direction.DecimalPlaces = 0;
      gridViewCellStyle23.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle23.Format = "N0";
      this.dcBldg2Direction.DefaultCellStyle = gridViewCellStyle23;
      this.dcBldg2Direction.Format = "#;#";
      this.dcBldg2Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg2Direction, "dcBldg2Direction");
      this.dcBldg2Direction.Name = "dcBldg2Direction";
      this.dcBldg2Direction.Signed = false;
      this.dcBldg2Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg2Distance.DataPropertyName = "B2Distance";
      this.dcBldg2Distance.DecimalPlaces = 2;
      gridViewCellStyle24.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle24.Format = "N2";
      this.dcBldg2Distance.DefaultCellStyle = gridViewCellStyle24;
      this.dcBldg2Distance.Format = "#;#";
      this.dcBldg2Distance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcBldg2Distance, "dcBldg2Distance");
      this.dcBldg2Distance.Name = "dcBldg2Distance";
      this.dcBldg2Distance.Signed = false;
      this.dcBldg3Direction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg3Direction.DataPropertyName = "B3Direction";
      this.dcBldg3Direction.DecimalPlaces = 0;
      gridViewCellStyle25.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle25.Format = "N0";
      this.dcBldg3Direction.DefaultCellStyle = gridViewCellStyle25;
      this.dcBldg3Direction.Format = "#;#";
      this.dcBldg3Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg3Direction, "dcBldg3Direction");
      this.dcBldg3Direction.Name = "dcBldg3Direction";
      this.dcBldg3Direction.Signed = false;
      this.dcBldg3Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg3Distance.DataPropertyName = "B3Distance";
      this.dcBldg3Distance.DecimalPlaces = 2;
      gridViewCellStyle26.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle26.Format = "N2";
      this.dcBldg3Distance.DefaultCellStyle = gridViewCellStyle26;
      this.dcBldg3Distance.Format = "#;#";
      this.dcBldg3Distance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcBldg3Distance, "dcBldg3Distance");
      this.dcBldg3Distance.Name = "dcBldg3Distance";
      this.dcBldg3Distance.Signed = false;
      this.dcSiteType.DataPropertyName = "SiteType";
      this.dcSiteType.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcSiteType, "dcSiteType");
      this.dcSiteType.Name = "dcSiteType";
      this.dcSiteType.Resizable = DataGridViewTriState.True;
      this.dcSiteType.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcStTree.DataPropertyName = "StreetTree";
      componentResourceManager.ApplyResources((object) this.dcStTree, "dcStTree");
      this.dcStTree.Name = "dcStTree";
      this.dcStTree.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcMaintRec.DataPropertyName = "MaintRec";
      this.dcMaintRec.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcMaintRec, "dcMaintRec");
      this.dcMaintRec.Name = "dcMaintRec";
      this.dcMaintRec.Resizable = DataGridViewTriState.True;
      this.dcMaintRec.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcMaintTask.DataPropertyName = "MaintTask";
      this.dcMaintTask.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcMaintTask, "dcMaintTask");
      this.dcMaintTask.Name = "dcMaintTask";
      this.dcMaintTask.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcSidewalk.DataPropertyName = "SidewalkDamage";
      this.dcSidewalk.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcSidewalk, "dcSidewalk");
      this.dcSidewalk.Name = "dcSidewalk";
      this.dcWireConflict.DataPropertyName = "WireConflict";
      this.dcWireConflict.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcWireConflict, "dcWireConflict");
      this.dcWireConflict.Name = "dcWireConflict";
      this.dcFieldOne.DataPropertyName = "OtherOne";
      this.dcFieldOne.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcFieldOne, "dcFieldOne");
      this.dcFieldOne.Name = "dcFieldOne";
      this.dcFieldTwo.DataPropertyName = "OtherTwo";
      this.dcFieldTwo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcFieldTwo, "dcFieldTwo");
      this.dcFieldTwo.Name = "dcFieldTwo";
      this.dcFieldThree.DataPropertyName = "OtherThree";
      this.dcFieldThree.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcFieldThree, "dcFieldThree");
      this.dcFieldThree.Name = "dcFieldThree";
      this.dcIPEDTSDieback.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDTSDieback.DataPropertyName = "IPEDTSDieback";
      this.dcIPEDTSDieback.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDTSDieback, "dcIPEDTSDieback");
      this.dcIPEDTSDieback.Name = "dcIPEDTSDieback";
      this.dcIPEDTSDieback.Resizable = DataGridViewTriState.True;
      this.dcIPEDTSDieback.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDTSEpiSprout.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDTSEpiSprout.DataPropertyName = "IPEDTSEpiSprout";
      this.dcIPEDTSEpiSprout.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDTSEpiSprout, "dcIPEDTSEpiSprout");
      this.dcIPEDTSEpiSprout.Name = "dcIPEDTSEpiSprout";
      this.dcIPEDTSEpiSprout.Resizable = DataGridViewTriState.True;
      this.dcIPEDTSEpiSprout.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDTSWiltFoli.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDTSWiltFoli.DataPropertyName = "IPEDTSWiltFoli";
      this.dcIPEDTSWiltFoli.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDTSWiltFoli, "dcIPEDTSWiltFoli");
      this.dcIPEDTSWiltFoli.Name = "dcIPEDTSWiltFoli";
      this.dcIPEDTSWiltFoli.Resizable = DataGridViewTriState.True;
      this.dcIPEDTSWiltFoli.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDTSEnvStress.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDTSEnvStress.DataPropertyName = "IPEDTSEnvStress";
      this.dcIPEDTSEnvStress.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDTSEnvStress, "dcIPEDTSEnvStress");
      this.dcIPEDTSEnvStress.Name = "dcIPEDTSEnvStress";
      this.dcIPEDTSEnvStress.Resizable = DataGridViewTriState.True;
      this.dcIPEDTSEnvStress.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDTSHumStress.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDTSHumStress.DataPropertyName = "IPEDTSHumStress";
      this.dcIPEDTSHumStress.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDTSHumStress, "dcIPEDTSHumStress");
      this.dcIPEDTSHumStress.Name = "dcIPEDTSHumStress";
      this.dcIPEDTSHumStress.Resizable = DataGridViewTriState.True;
      this.dcIPEDTSHumStress.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDTSNotes.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDTSNotes.DataPropertyName = "IPEDTSNotes";
      componentResourceManager.ApplyResources((object) this.dcIPEDTSNotes, "dcIPEDTSNotes");
      this.dcIPEDTSNotes.Name = "dcIPEDTSNotes";
      this.dcIPEDFTChewFoli.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDFTChewFoli.DataPropertyName = "IPEDFTChewFoli";
      this.dcIPEDFTChewFoli.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDFTChewFoli, "dcIPEDFTChewFoli");
      this.dcIPEDFTChewFoli.Name = "dcIPEDFTChewFoli";
      this.dcIPEDFTChewFoli.Resizable = DataGridViewTriState.True;
      this.dcIPEDFTChewFoli.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDFTDiscFoli.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDFTDiscFoli.DataPropertyName = "IPEDFTDiscFoli";
      this.dcIPEDFTDiscFoli.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDFTDiscFoli, "dcIPEDFTDiscFoli");
      this.dcIPEDFTDiscFoli.Name = "dcIPEDFTDiscFoli";
      this.dcIPEDFTDiscFoli.Resizable = DataGridViewTriState.True;
      this.dcIPEDFTDiscFoli.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDFTAbnFoli.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDFTAbnFoli.DataPropertyName = "IPEDFTAbnFoli";
      this.dcIPEDFTAbnFoli.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDFTAbnFoli, "dcIPEDFTAbnFoli");
      this.dcIPEDFTAbnFoli.Name = "dcIPEDFTAbnFoli";
      this.dcIPEDFTAbnFoli.Resizable = DataGridViewTriState.True;
      this.dcIPEDFTAbnFoli.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDFTInsectSigns.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDFTInsectSigns.DataPropertyName = "IPEDFTInsectSigns";
      this.dcIPEDFTInsectSigns.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDFTInsectSigns, "dcIPEDFTInsectSigns");
      this.dcIPEDFTInsectSigns.Name = "dcIPEDFTInsectSigns";
      this.dcIPEDFTInsectSigns.Resizable = DataGridViewTriState.True;
      this.dcIPEDFTInsectSigns.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDFTFoliAffect.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDFTFoliAffect.DataPropertyName = "IPEDFTFoliAffect";
      this.dcIPEDFTFoliAffect.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDFTFoliAffect, "dcIPEDFTFoliAffect");
      this.dcIPEDFTFoliAffect.Name = "dcIPEDFTFoliAffect";
      this.dcIPEDFTFoliAffect.Resizable = DataGridViewTriState.True;
      this.dcIPEDFTFoliAffect.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDFTNotes.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDFTNotes.DataPropertyName = "IPEDFTNotes";
      componentResourceManager.ApplyResources((object) this.dcIPEDFTNotes, "dcIPEDFTNotes");
      this.dcIPEDFTNotes.Name = "dcIPEDFTNotes";
      this.dcIPEDBBInsectSigns.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDBBInsectSigns.DataPropertyName = "IPEDBBInsectSigns";
      this.dcIPEDBBInsectSigns.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDBBInsectSigns, "dcIPEDBBInsectSigns");
      this.dcIPEDBBInsectSigns.Name = "dcIPEDBBInsectSigns";
      this.dcIPEDBBInsectSigns.Resizable = DataGridViewTriState.True;
      this.dcIPEDBBInsectSigns.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDBBInsectPres.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDBBInsectPres.DataPropertyName = "IPEDBBInsectPres";
      this.dcIPEDBBInsectPres.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDBBInsectPres, "dcIPEDBBInsectPres");
      this.dcIPEDBBInsectPres.Name = "dcIPEDBBInsectPres";
      this.dcIPEDBBInsectPres.Resizable = DataGridViewTriState.True;
      this.dcIPEDBBInsectPres.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDBBDiseaseSigns.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDBBDiseaseSigns.DataPropertyName = "IPEDBBDiseaseSigns";
      this.dcIPEDBBDiseaseSigns.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDBBDiseaseSigns, "dcIPEDBBDiseaseSigns");
      this.dcIPEDBBDiseaseSigns.Name = "dcIPEDBBDiseaseSigns";
      this.dcIPEDBBDiseaseSigns.Resizable = DataGridViewTriState.True;
      this.dcIPEDBBDiseaseSigns.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDBBProbLoc.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDBBProbLoc.DataPropertyName = "IPEDBBProbLoc";
      this.dcIPEDBBProbLoc.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDBBProbLoc, "dcIPEDBBProbLoc");
      this.dcIPEDBBProbLoc.Name = "dcIPEDBBProbLoc";
      this.dcIPEDBBProbLoc.Resizable = DataGridViewTriState.True;
      this.dcIPEDBBProbLoc.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDBBAbnGrowth.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDBBAbnGrowth.DataPropertyName = "IPEDBBAbnGrowth";
      this.dcIPEDBBAbnGrowth.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDBBAbnGrowth, "dcIPEDBBAbnGrowth");
      this.dcIPEDBBAbnGrowth.Name = "dcIPEDBBAbnGrowth";
      this.dcIPEDBBAbnGrowth.Resizable = DataGridViewTriState.True;
      this.dcIPEDBBAbnGrowth.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDBBNotes.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDBBNotes.DataPropertyName = "IPEDBBNotes";
      componentResourceManager.ApplyResources((object) this.dcIPEDBBNotes, "dcIPEDBBNotes");
      this.dcIPEDBBNotes.Name = "dcIPEDBBNotes";
      this.dcIPEDBBNotes.Resizable = DataGridViewTriState.True;
      this.dcIPEDPest.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDPest.DataPropertyName = "IPEDPest";
      this.dcIPEDPest.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDPest, "dcIPEDPest");
      this.dcIPEDPest.Name = "dcIPEDPest";
      this.dcIPEDPest.Resizable = DataGridViewTriState.True;
      this.dcIPEDPest.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcCityManaged.DataPropertyName = "CityManaged";
      this.dcCityManaged.FalseValue = (object) "0";
      componentResourceManager.ApplyResources((object) this.dcCityManaged, "dcCityManaged");
      this.dcCityManaged.Name = "dcCityManaged";
      this.dcCityManaged.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcCityManaged.TrueValue = (object) "1";
      this.dcLocSite.DataPropertyName = "LocSite";
      componentResourceManager.ApplyResources((object) this.dcLocSite, "dcLocSite");
      this.dcLocSite.Name = "dcLocSite";
      this.dcLocSite.Resizable = DataGridViewTriState.True;
      this.dcLocSite.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcLocNo.DataPropertyName = "LocNo";
      this.dcLocNo.DecimalPlaces = 0;
      gridViewCellStyle27.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle27.Format = "N0";
      this.dcLocNo.DefaultCellStyle = gridViewCellStyle27;
      this.dcLocNo.Format = (string) null;
      this.dcLocNo.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcLocNo, "dcLocNo");
      this.dcLocNo.Name = "dcLocNo";
      this.dcLocNo.Resizable = DataGridViewTriState.True;
      this.dcLocNo.Signed = false;
      this.dcLatitude.DataPropertyName = "Latitude";
      componentResourceManager.ApplyResources((object) this.dcLatitude, "dcLatitude");
      this.dcLatitude.Name = "dcLatitude";
      this.dcLongitude.DataPropertyName = "Longitude";
      componentResourceManager.ApplyResources((object) this.dcLongitude, "dcLongitude");
      this.dcLongitude.Name = "dcLongitude";
      this.dcNoteTree.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcNoteTree.DataPropertyName = "NoteThisTree";
      componentResourceManager.ApplyResources((object) this.dcNoteTree, "dcNoteTree");
      this.dcNoteTree.Name = "dcNoteTree";
      this.dcNoteTree.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcComments.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcComments.DataPropertyName = "Comments";
      componentResourceManager.ApplyResources((object) this.dcComments, "dcComments");
      this.dcComments.Name = "dcComments";
      this.dcComments.Resizable = DataGridViewTriState.True;
      this.dcComments.SortMode = DataGridViewColumnSortMode.NotSortable;
      componentResourceManager.ApplyResources((object) this.lblSelTrees, "lblSelTrees");
      this.lblSelTrees.Name = "lblSelTrees";
      componentResourceManager.ApplyResources((object) this.chkAllTrees, "chkAllTrees");
      this.chkAllTrees.Name = "chkAllTrees";
      this.chkAllTrees.UseVisualStyleBackColor = true;
      this.chkAllTrees.CheckedChanged += new EventHandler(this.chkAllTrees_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel2, "tableLayoutPanel2");
      this.tableLayoutPanel2.Controls.Add((Control) this.lblEmail1, 0, 0);
      this.tableLayoutPanel2.Controls.Add((Control) this.txtEmail1, 1, 0);
      this.tableLayoutPanel2.Controls.Add((Control) this.txtEmail2, 1, 1);
      this.tableLayoutPanel2.Controls.Add((Control) this.txtPassword2, 1, 3);
      this.tableLayoutPanel2.Controls.Add((Control) this.lblPassword2, 0, 3);
      this.tableLayoutPanel2.Controls.Add((Control) this.lblEmail2, 0, 1);
      this.tableLayoutPanel2.Controls.Add((Control) this.lblPassword1, 0, 2);
      this.tableLayoutPanel2.Controls.Add((Control) this.txtPassword1, 1, 2);
      this.tableLayoutPanel2.Controls.Add((Control) this.Label3, 0, 5);
      this.tableLayoutPanel2.Controls.Add((Control) this.btnResetPassword, 2, 4);
      this.tableLayoutPanel2.Controls.Add((Control) this.btnSubmit, 2, 6);
      this.tableLayoutPanel2.Controls.Add((Control) this.lblResetPassword, 0, 4);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      componentResourceManager.ApplyResources((object) this.lblEmail1, "lblEmail1");
      this.lblEmail1.Name = "lblEmail1";
      componentResourceManager.ApplyResources((object) this.txtEmail1, "txtEmail1");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.txtEmail1, 2);
      this.txtEmail1.Name = "txtEmail1";
      componentResourceManager.ApplyResources((object) this.txtEmail2, "txtEmail2");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.txtEmail2, 2);
      this.txtEmail2.Name = "txtEmail2";
      componentResourceManager.ApplyResources((object) this.txtPassword2, "txtPassword2");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.txtPassword2, 2);
      this.txtPassword2.Name = "txtPassword2";
      componentResourceManager.ApplyResources((object) this.lblPassword2, "lblPassword2");
      this.lblPassword2.Name = "lblPassword2";
      componentResourceManager.ApplyResources((object) this.lblEmail2, "lblEmail2");
      this.lblEmail2.Name = "lblEmail2";
      componentResourceManager.ApplyResources((object) this.lblPassword1, "lblPassword1");
      this.lblPassword1.Name = "lblPassword1";
      componentResourceManager.ApplyResources((object) this.txtPassword1, "txtPassword1");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.txtPassword1, 2);
      this.txtPassword1.Name = "txtPassword1";
      componentResourceManager.ApplyResources((object) this.Label3, "Label3");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.Label3, 3);
      this.Label3.ForeColor = Color.Blue;
      this.Label3.Name = "Label3";
      componentResourceManager.ApplyResources((object) this.btnResetPassword, "btnResetPassword");
      this.btnResetPassword.Name = "btnResetPassword";
      this.btnResetPassword.UseVisualStyleBackColor = true;
      this.btnResetPassword.Click += new EventHandler(this.btnResetPassword_Click);
      componentResourceManager.ApplyResources((object) this.btnSubmit, "btnSubmit");
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.UseVisualStyleBackColor = true;
      this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
      componentResourceManager.ApplyResources((object) this.lblResetPassword, "lblResetPassword");
      this.tableLayoutPanel2.SetColumnSpan((Control) this.lblResetPassword, 2);
      this.lblResetPassword.Name = "lblResetPassword";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      this.dgSubmitLog.AllowUserToAddRows = false;
      this.dgSubmitLog.AllowUserToDeleteRows = false;
      this.dgSubmitLog.AllowUserToResizeRows = false;
      this.dgSubmitLog.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle28.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle28.BackColor = SystemColors.Control;
      gridViewCellStyle28.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle28.ForeColor = SystemColors.WindowText;
      gridViewCellStyle28.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle28.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle28.WrapMode = DataGridViewTriState.True;
      this.dgSubmitLog.ColumnHeadersDefaultCellStyle = gridViewCellStyle28;
      this.dgSubmitLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgSubmitLog.Columns.AddRange((DataGridViewColumn) this.dcMobileKey, (DataGridViewColumn) this.dcDateTime, (DataGridViewColumn) this.dcDescription);
      gridViewCellStyle29.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle29.BackColor = SystemColors.Window;
      gridViewCellStyle29.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle29.ForeColor = SystemColors.ControlText;
      gridViewCellStyle29.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle29.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle29.WrapMode = DataGridViewTriState.False;
      this.dgSubmitLog.DefaultCellStyle = gridViewCellStyle29;
      componentResourceManager.ApplyResources((object) this.dgSubmitLog, "dgSubmitLog");
      this.dgSubmitLog.EditMode = DataGridViewEditMode.EditProgrammatically;
      this.dgSubmitLog.EnableHeadersVisualStyles = false;
      this.dgSubmitLog.MultiSelect = false;
      this.dgSubmitLog.Name = "dgSubmitLog";
      this.dgSubmitLog.ReadOnly = true;
      this.dgSubmitLog.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle30.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle30.BackColor = SystemColors.Control;
      gridViewCellStyle30.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle30.ForeColor = SystemColors.WindowText;
      gridViewCellStyle30.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle30.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle30.WrapMode = DataGridViewTriState.True;
      this.dgSubmitLog.RowHeadersDefaultCellStyle = gridViewCellStyle30;
      this.dgSubmitLog.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgSubmitLog.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgSubmitLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgSubmitLog.VirtualMode = true;
      this.dcMobileKey.DataPropertyName = "MobileKey";
      componentResourceManager.ApplyResources((object) this.dcMobileKey, "dcMobileKey");
      this.dcMobileKey.Name = "dcMobileKey";
      this.dcMobileKey.ReadOnly = true;
      this.dcDateTime.CustomFormat = (string) null;
      this.dcDateTime.DataPropertyName = "DateTime";
      componentResourceManager.ApplyResources((object) this.dcDateTime, "dcDateTime");
      this.dcDateTime.Name = "dcDateTime";
      this.dcDateTime.ReadOnly = true;
      this.dcDateTime.Resizable = DataGridViewTriState.True;
      this.dcDescription.DataPropertyName = "Description";
      componentResourceManager.ApplyResources((object) this.dcDescription, "dcDescription");
      this.dcDescription.Name = "dcDescription";
      this.dcDescription.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (MobileSubmitInventoryForm);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.label1, 0);
      this.Controls.SetChildIndex((Control) this.tableLayoutPanel1, 0);
      ((ISupportInitialize) this.ep).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.dgPlots).EndInit();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      ((ISupportInitialize) this.dgSubmitLog).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
