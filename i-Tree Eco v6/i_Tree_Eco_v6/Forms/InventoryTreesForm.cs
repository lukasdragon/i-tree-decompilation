// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.InventoryTreesForm
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
using Eco.Util.Constraints;
using Eco.Util.Views;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Interfaces;
using IPED.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class InventoryTreesForm : DataContentForm, IActionable, IExportable, IImport
  {
    private IPEDData m_iped;
    private PagedList<Plot> m_plots;
    private BindingList<SpeciesView> m_species;
    private DataGridViewManager m_dgManager;
    private int m_dgHorizPos;
    private bool m_rowDirty;
    private FlatPlotView m_fpv;
    private bool m_inDelete;
    private IDictionary<double, string> m_dbhs;
    private Dictionary<string, (Func<object, object> getValue, System.Action<object, object> setValue)> m_fpvProperties;
    private IContainer components;
    private DataGridView dgPlots;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewTextBoxColumn dcUserId;
    private DataGridViewComboBoxColumn dcStrata;
    private DataGridViewTextBoxColumn dcCrew;
    private DataGridViewNullableDateTimeColumn dcSurveyDate;
    private DataGridViewComboBoxColumn dcStatus;
    private DataGridViewFilteredComboBoxColumn dcSpecies;
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
    private DataGridViewNumericTextBoxColumn dcBldg4Direction;
    private DataGridViewNumericTextBoxColumn dcBldg4Distance;
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

    public InventoryTreesForm()
    {
      this.InitializeComponent();
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.dcCrownPercentMissing.DefaultCellStyle.DataSourceNullValue = (object) PctMidRange.PRINV;
      this.dcCrownLightExposure.DefaultCellStyle.DataSourceNullValue = (object) CrownLightExposure.NotEntered;
      this.dcTreeHeight.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcCrownBaseHeight.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcCrownTopHeight.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcCrownWidthNS.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcCrownWidthEW.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcDBH1.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH1c.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH1Height.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH2.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH2c.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH2Height.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH3.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH3c.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH3Height.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH4.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH4c.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH4Height.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH5.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH5c.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH5Height.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH6.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH6c.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcDBH6Height.DefaultCellStyle.DataSourceNullValue = (object) -1.0;
      this.dcBldg1Direction.DefaultCellStyle.DataSourceNullValue = (object) (short) -1;
      this.dcBldg1Distance.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcBldg2Direction.DefaultCellStyle.DataSourceNullValue = (object) (short) -1;
      this.dcBldg2Distance.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcBldg3Direction.DefaultCellStyle.DataSourceNullValue = (object) (short) -1;
      this.dcBldg3Distance.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcBldg4Direction.DefaultCellStyle.DataSourceNullValue = (object) (short) -1;
      this.dcBldg4Distance.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcCityManaged.IndeterminateValue = (object) null;
      this.m_dgManager = new DataGridViewManager(this.dgPlots);
      this.m_fpvProperties = new Dictionary<string, (Func<object, object>, System.Action<object, object>)>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (PropertyInfo property in typeof (FlatPlotView).GetProperties())
      {
        Func<object, object> propertyGetter = TypeHelper.GetPropertyGetter(property);
        System.Action<object, object> propertySetter = TypeHelper.GetPropertySetter(property);
        this.m_fpvProperties[property.Name] = (propertyGetter, propertySetter);
      }
      this.dgPlots.DoubleBuffered(true);
      this.dgPlots.AutoGenerateColumns = false;
      this.dgPlots.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgPlots.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      EventPublisher.Register<EntityUpdated<Tree>>(new EventHandler<EntityUpdated<Tree>>(this.Tree_Updated));
    }

    private void Tree_Updated(object sender, EntityUpdated<Tree> e)
    {
      if (this.Session == null)
        return;
      this.TaskManager.Add(Task.Factory.StartNew<Plot>((Func<Plot>) (() =>
      {
        Plot plot = (Plot) null;
        lock (this.Session)
        {
          using (ITransaction transaction = this.Session.BeginTransaction())
          {
            Tree proxy = this.Session.Load<Tree>((object) e.Guid);
            if (NHibernateUtil.IsInitialized((object) proxy))
              this.Session.Refresh((object) proxy, LockMode.None);
            plot = proxy.Plot;
            transaction.Commit();
          }
        }
        return plot;
      }), CancellationToken.None, TaskCreationOptions.None, Program.Session.Scheduler).ContinueWith((System.Action<Task<Plot>>) (t =>
      {
        if (this.IsDisposed | t.IsFaulted)
          return;
        int rowIndex = this.m_plots.IndexOf(t.Result);
        if (rowIndex < 0)
          return;
        this.dgPlots.InvalidateRow(rowIndex);
      }), TaskScheduler.FromCurrentSynchronizationContext()));
    }

    private void BindLookupColumn<T>(DataGridViewComboBoxColumn col, IList<T> dataSource) where T : Eco.Domain.v6.Lookup => col.BindTo<T>((System.Linq.Expressions.Expression<Func<T, object>>) (lu => lu.Description), (System.Linq.Expressions.Expression<Func<T, object>>) (lu => lu.Self), (object) dataSource);

    private void BindIPEDLookupColumn<T>(DataGridViewComboBoxColumn col, IList<T> dataSource) where T : IPED.Domain.Lookup => col.BindTo<T>((System.Linq.Expressions.Expression<Func<T, object>>) (lu => lu.Description), (System.Linq.Expressions.Expression<Func<T, object>>) (lu => (object) lu.Code), (object) dataSource);

    protected override void InitializeYear(Year y)
    {
      base.InitializeYear(y);
      NHibernateUtil.Initialize((object) y.Series);
      NHibernateUtil.Initialize((object) y.Strata);
      NHibernateUtil.Initialize((object) y.Conditions);
      if (!y.DBHActual)
        NHibernateUtil.Initialize((object) y.DBHs);
      if (y.RecordIPED)
        this.m_iped = Program.Session.IPEDData;
      if (y.RecordLanduse)
        NHibernateUtil.Initialize((object) y.LandUses);
      if (y.RecordSiteType)
        NHibernateUtil.Initialize((object) y.SiteTypes);
      if (y.RecordLocSite)
        NHibernateUtil.Initialize((object) y.LocSites);
      if (y.RecordMaintRec)
        NHibernateUtil.Initialize((object) y.MaintRecs);
      if (y.RecordMaintTask)
        NHibernateUtil.Initialize((object) y.MaintTasks);
      if (y.RecordSidewalk)
        NHibernateUtil.Initialize((object) y.SidewalkDamages);
      if (y.RecordWireConflict)
        NHibernateUtil.Initialize((object) y.WireConflicts);
      if (y.RecordOtherOne)
        NHibernateUtil.Initialize((object) y.OtherOnes);
      if (y.RecordOtherTwo)
        NHibernateUtil.Initialize((object) y.OtherTwos);
      if (y.RecordOtherThree)
        NHibernateUtil.Initialize((object) y.OtherThrees);
      this.m_dbhs = (IDictionary<double, string>) y.DBHs.ToDictionary<DBH, double, string>((Func<DBH, double>) (d => Convert.ToDouble(d.Id)), (Func<DBH, string>) (d => d.Description));
    }

    protected override void LoadData()
    {
      base.LoadData();
      List<SpeciesView> list = Program.Session.Species.Values.ToList<SpeciesView>();
      list.Sort((IComparer<SpeciesView>) new PropertyComparer<SpeciesView>((Func<SpeciesView, object>) (sp => (object) sp.CommonName)));
      this.m_species = (BindingList<SpeciesView>) new ExtendedBindingList<SpeciesView>((IList<SpeciesView>) list);
      lock (this.Session)
      {
        using (this.Session.BeginTransaction())
        {
          using (TypeHelper<Plot> typeHelper = new TypeHelper<Plot>())
            this.m_plots = this.Session.CreateCriteria<Plot>().Add((ICriterion) Restrictions.Eq(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Year)), (object) this.Year)).Fetch(SelectMode.Fetch, typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Trees))).Fetch(SelectMode.Fetch, typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.PlotLandUses))).AddOrder(Order.Asc(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)))).SetResultTransformer((IResultTransformer) new DistinctRootEntityResultTransformer()).PagedList<Plot>(100);
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
      Year year = this.Year;
      if (year == null)
        return;
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
      this.dcSpecies.BindTo<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.CommonScientificName), (System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.Code), (object) this.m_species);
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
      if (year.RecordStrata)
        this.dcStrata.BindTo<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Description), (System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Self), (object) year.Strata.ToList<Strata>());
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcStrata);
      if (year.RecordLanduse)
        this.dcLandUse.BindTo<LandUse>((System.Linq.Expressions.Expression<Func<LandUse, object>>) (lu => lu.Description), (System.Linq.Expressions.Expression<Func<LandUse, object>>) (lu => lu.Self), (object) year.LandUses.ToList<LandUse>());
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcLandUse);
      if (year.RecordEnergy)
      {
        this.dcBldg1Distance.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.Building_SingularName, (object) 1), (object) v6Strings.Building_Distance), (object) str2);
        this.dcBldg2Distance.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.Building_SingularName, (object) 2), (object) v6Strings.Building_Distance), (object) str2);
        this.dcBldg3Distance.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.Building_SingularName, (object) 3), (object) v6Strings.Building_Distance), (object) str2);
        this.dcBldg4Distance.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.Building_SingularName, (object) 4), (object) v6Strings.Building_Distance), (object) str2);
      }
      else
      {
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg1Direction);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg1Distance);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg2Direction);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg2Distance);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg3Direction);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg3Distance);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg4Direction);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcBldg4Distance);
      }
      if (!year.MgmtStyle.HasValue || (year.MgmtStyle.Value & MgmtStyleEnum.RecordPublicPrivate) == MgmtStyleEnum.DefaultPublic)
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcCityManaged);
      if (!year.RecordStreetTree)
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcStTree);
      if (year.RecordSiteType)
        this.dcSiteType.BindTo<SiteType>((System.Linq.Expressions.Expression<Func<SiteType, object>>) (st => st.Description), (System.Linq.Expressions.Expression<Func<SiteType, object>>) (st => st.Self), (object) year.SiteTypes.ToList<SiteType>());
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcSiteType);
      if (year.RecordTreeStatus)
      {
        TreeStatus[] exclude = (TreeStatus[]) null;
        if (year.IsInitialMeasurement)
          exclude = new TreeStatus[6]
          {
            TreeStatus.HazardRemoved,
            TreeStatus.HealthyRemoved,
            TreeStatus.LandUseChangeRemoved,
            TreeStatus.NoChange,
            TreeStatus.UnknownRemoved,
            TreeStatus.InitialSample
          };
        this.dcStatus.BindTo("Value", "Key", (object) new BindingSource((object) EnumHelper.ConvertToDictionary<TreeStatus>(exclude, (IComparer<TreeStatus>) new AttributePropertyComparer<TreeStatus, DescriptionAttribute>("Description")), (string) null));
      }
      else
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcStatus);
      if (!year.RecordTreeGPS)
      {
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcLatitude);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcLongitude);
      }
      if (!year.RecordTreeAddress)
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcAddress);
      if (year.RecordHeight)
        this.dcTreeHeight.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtTotal, (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      else
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
      {
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcCrownCondition);
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcCrownDieback);
      }
      if (year.RecordCrownSize)
      {
        this.dcCrownPercentMissing.BindTo("Value", "Key", (object) new BindingSource((object) EnumHelper.ConvertToDictionary<PctMidRange>(new PctMidRange[1]
        {
          PctMidRange.PRINV
        }), (string) null));
        this.dcCrownWidthEW.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) v6Strings.Crown_SingularName, (object) v6Strings.Crown_WidthEW), (object) str2);
        this.dcCrownWidthNS.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) v6Strings.Crown_SingularName, (object) v6Strings.Crown_WidthNS), (object) str2);
        this.dcCrownBaseHeight.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) v6Strings.Crown_SingularName, (object) v6Strings.Crown_BaseHeight), (object) str2);
        this.dcCrownTopHeight.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) v6Strings.Crown_SingularName, (object) v6Strings.Crown_TopHeight), (object) str2);
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
      if (!year.RecordLocNo)
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcLocNo);
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
      if (!year.RecordNoteTree)
        this.dgPlots.Columns.Remove((DataGridViewColumn) this.dcNoteTree);
      this.dgPlots.RowCount = this.m_plots.Count;
      this.dgPlots.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
    }

    protected override void OnRequestRefresh()
    {
      Year year = this.Year;
      bool flag1 = year != null && year.Changed && this.m_plots != null;
      bool flag2 = flag1 && this.m_plots.Count > 0;
      this.dgPlots.ReadOnly = !flag1;
      this.dgPlots.AllowUserToAddRows = flag1;
      this.dgPlots.AllowUserToDeleteRows = flag2;
      base.OnRequestRefresh();
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

    public bool CanPerformAction(UserActions action)
    {
      Year year = this.Year;
      bool flag1 = year != null && year.Changed && this.m_plots != null;
      bool flag2 = this.dgPlots.SelectedRows.Count > 0;
      bool flag3 = this.dgPlots.CurrentRow != null && this.dgPlots.CurrentRow.IsNewRow;
      switch (action)
      {
        case UserActions.New:
          return flag1 && !flag3;
        case UserActions.Copy:
          return false;
        case UserActions.Undo:
        case UserActions.Redo:
          return false;
        case UserActions.Delete:
          return flag1 & flag2;
        case UserActions.ImportData:
          return flag1;
        default:
          return false;
      }
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
          if (this.m_plots == null || selectedRow1.Index >= this.m_plots.Count)
            break;
          int num = this.NextPlotId();
          lock (this.Session)
          {
            Plot plot = this.m_plots[selectedRow1.Index].Clone() as Plot;
            plot.Id = num;
            this.m_plots.Add(plot);
            break;
          }
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

    private void dgPlots_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      if (this.dgPlots.CurrentRow != null && !this.dgPlots.IsCurrentRowDirty || this.m_plots == null || e.RowIndex >= this.m_plots.Count)
        return;
      Plot p = this.m_fpv?.Plot;
      DataGridViewRow row = this.dgPlots.Rows[e.RowIndex];
      DataGridViewCell dataGridViewCell1 = (DataGridViewCell) null;
      string text = (string) null;
      if (p != null)
      {
        using (ISession session = Program.Session.InputSession.CreateSession())
        {
          using (ITransaction transaction = session.BeginTransaction())
          {
            Plot plot = session.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (pl => pl.Year == this.Year && pl.Id == p.Id)).SingleOrDefault();
            if (plot != null && !plot.Equals((object) p))
            {
              text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcId.HeaderText);
              dataGridViewCell1 = row.Cells[this.dcId.DisplayIndex];
            }
            transaction.Commit();
          }
        }
      }
      if (text == null)
      {
        DataGridViewCell dataGridViewCell2 = row.ErrorCell();
        if (dataGridViewCell2 != null)
          text = dataGridViewCell2.ErrorText;
      }
      if (text == null)
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private Plot GetPlot(int index)
    {
      lock (this.Session)
        return this.m_plots[index];
    }

    private void dgPlots_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgPlots.CurrentRow != null && this.dgPlots.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (this.dgPlots.CurrentRow != null && !this.dgPlots.IsCurrentRowDirty)
          return;
        if (this.m_plots != null && e.RowIndex < this.m_plots.Count)
        {
          Plot plot = this.m_fpv?.Plot;
          if (plot != null)
          {
            lock (this.Session)
            {
              using (ITransaction transaction = this.Session.BeginTransaction())
              {
                if (!this.Session.Contains((object) plot))
                {
                  this.Session.Merge<Plot>(plot);
                }
                else
                {
                  this.Session.SaveOrUpdate((object) plot);
                  foreach (Tree tree in (IEnumerable<Tree>) plot.Trees)
                  {
                    this.Session.SaveOrUpdate((object) tree);
                    foreach (object stem in (IEnumerable<Stem>) tree.Stems)
                      this.Session.SaveOrUpdate(stem);
                  }
                }
                transaction.Commit();
              }
              this.Session.Flush();
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgPlots_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      this.m_rowDirty = true;
      this.OnRequestRefresh();
    }

    private void dgPlots_SelectionChanged(object sender, EventArgs e)
    {
      if (this.m_inDelete)
        return;
      this.OnRequestRefresh();
    }

    private void DeleteSelectedRows()
    {
      if (this.dgPlots.SelectedRows.Count == 0)
        return;
      this.DeleteRows(Enumerable.Cast<DataGridViewRow>(this.dgPlots.SelectedRows).ToArray<DataGridViewRow>(), true);
    }

    private void DeleteRows(DataGridViewRow[] rows, bool removeRow)
    {
      if (this.m_plots == null || this.m_inDelete || rows.Length == 0)
        return;
      List<object> ids = new List<object>();
      this.m_inDelete = true;
      foreach (DataGridViewRow row in rows)
      {
        int index = row.Index;
        if (index != -1)
        {
          if (index < this.m_plots.Count)
          {
            lock (this.Session)
            {
              ids.Add(this.m_plots.Ids[index]);
              this.m_plots.RemoveAt(index);
            }
          }
          if (removeRow && index != this.dgPlots.NewRowIndex)
            this.dgPlots.Rows.Remove(row);
        }
      }
      if (this.dgPlots.CurrentRow != null && this.dgPlots.CurrentRow.IsNewRow && this.dgPlots.CurrentRow.Selected)
        this.dgPlots.CurrentCell = (DataGridViewCell) null;
      this.m_inDelete = false;
      this.TaskManager.Add(Task.Factory.StartNew((System.Action) (() =>
      {
        int index1 = 0;
        do
        {
          List<object> values = new List<object>();
          for (int index2 = 0; index1 < ids.Count && index2 < 1000; ++index1)
          {
            values.Add(ids[index1]);
            ++index2;
          }
          if (values.Count > 0)
          {
            lock (this.Session)
            {
              using (ITransaction transaction = this.Session.BeginTransaction())
              {
                foreach (Plot plot in (IEnumerable) this.Session.CreateCriteria<Plot>().Add((ICriterion) Restrictions.In((IProjection) Projections.Id(), (ICollection) values)).List())
                  this.Session.Delete((object) plot);
                transaction.Commit();
              }
            }
          }
        }
        while (index1 < ids.Count);
      }), CancellationToken.None, TaskCreationOptions.None, Program.Session.Scheduler));
    }

    private void dgPlots_Scroll(object sender, ScrollEventArgs e)
    {
      if (e.ScrollOrientation != ScrollOrientation.HorizontalScroll)
        return;
      this.m_dgHorizPos = e.NewValue;
    }

    private void dgPlots_Sorted(object sender, EventArgs e) => this.dgPlots.HorizontalScrollingOffset = this.m_dgHorizPos;

    private void UpdateDBHHeights(int column, int row)
    {
      this.SetDefaultHeight((DataGridViewColumn) this.dcDBH1, (DataGridViewColumn) this.dcDBH1Height, column, row);
      this.SetDefaultHeight((DataGridViewColumn) this.dcDBH2, (DataGridViewColumn) this.dcDBH2Height, column, row);
      this.SetDefaultHeight((DataGridViewColumn) this.dcDBH3, (DataGridViewColumn) this.dcDBH3Height, column, row);
      this.SetDefaultHeight((DataGridViewColumn) this.dcDBH4, (DataGridViewColumn) this.dcDBH4Height, column, row);
      this.SetDefaultHeight((DataGridViewColumn) this.dcDBH5, (DataGridViewColumn) this.dcDBH5Height, column, row);
      this.SetDefaultHeight((DataGridViewColumn) this.dcDBH6, (DataGridViewColumn) this.dcDBH6Height, column, row);
    }

    private void SetDefaultHeight(
      DataGridViewColumn dbh_col,
      DataGridViewColumn dbh_height_col,
      int column,
      int row)
    {
      int num = this.dgPlots.Columns.IndexOf(dbh_col);
      if (column != num || this.m_plots == null || row >= this.m_plots.Count)
        return;
      Plot plot = this.GetPlot(row);
      lock (this.Session)
      {
        FlatPlotView flatPlotView = new FlatPlotView(this.Session, plot);
        (Func<object, object> getValue, System.Action<object, object> setValue) fpvProperty1 = this.m_fpvProperties[dbh_col.DataPropertyName];
        (Func<object, object> getValue, System.Action<object, object> setValue) fpvProperty2 = this.m_fpvProperties[dbh_height_col.DataPropertyName];
        object obj1 = fpvProperty1.getValue((object) flatPlotView);
        object obj2 = fpvProperty2.getValue((object) flatPlotView);
        // ISSUE: variable of a boxed type
        __Boxed<double> local = (ValueType) -1.0;
        if (!obj1.Equals((object) local) || obj2.Equals((object) -1.0))
          return;
        fpvProperty2.setValue((object) flatPlotView, (object) -1.0);
      }
    }

    private void dgPlots_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex >= 0)
        this.UpdateDBHHeights(e.ColumnIndex, e.RowIndex);
      if (e.ColumnIndex != this.dcCrownCondition.DisplayIndex && e.ColumnIndex != this.dcCrownDieback.DisplayIndex)
        return;
      this.dgPlots.Refresh();
    }

    private void dgPlots_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    public void Export(ExportFormat format, string file)
    {
      if (format != ExportFormat.CSV)
      {
        if (format != ExportFormat.KML)
          return;
        this.ExportKml(file);
      }
      else
        this.dgPlots.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format)
    {
      switch (format)
      {
        case ExportFormat.CSV:
          return this.m_plots != null;
        case ExportFormat.KML:
          return this.m_plots != null && this.Year != null && this.Year.RecordTreeGPS;
        default:
          return false;
      }
    }

    private void ExportKml(string fn)
    {
      if (this.m_plots == null)
        return;
      Kml root = new Kml();
      Document document1 = new Document();
      lock (this.Session)
      {
        foreach (Plot plot in this.m_plots)
        {
          FlatPlotView flatPlotView = new FlatPlotView(this.Session, plot);
          double? nullable = flatPlotView.Latitude;
          if (nullable.HasValue)
          {
            nullable = flatPlotView.Longitude;
            if (nullable.HasValue)
            {
              nullable = flatPlotView.Longitude;
              // ISSUE: variable of a boxed type
              __Boxed<double> x = (ValueType) nullable.Value;
              nullable = flatPlotView.Latitude;
              // ISSUE: variable of a boxed type
              __Boxed<double> y = (ValueType) nullable.Value;
              if (ValidationHelper.ValidateCoordinates((object) x, (object) y))
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
                nullable = flatPlotView.Latitude;
                double latitude = nullable.Value;
                nullable = flatPlotView.Longitude;
                double longitude = nullable.Value;
                point.Coordinate = new Vector(latitude, longitude, 0.0);
                placemark.Geometry = (Geometry) point;
                document2.AddFeature((Feature) placemark);
              }
            }
          }
        }
      }
      root.Feature = (Feature) document1;
      KmlFile.Create((Element) root, true).Save((Stream) new FileStream(fn, FileMode.Create));
    }

    private void dgPlots_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgPlots.NewRowIndex || this.m_plots == null || e.RowIndex >= this.m_plots.Count)
        return;
      DataGridViewColumn column = this.dgPlots.Columns[e.ColumnIndex];
      Plot plot = this.GetPlot(e.RowIndex);
      FlatPlotView flatPlotView = this.m_fpv;
      if (!plot.Equals((object) flatPlotView?.Plot))
        flatPlotView = new FlatPlotView(this.Session, plot);
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
          SpeciesView speciesView;
          if (Program.Session.InvalidSpecies.TryGetValue(flatPlotView.Species, out speciesView))
            e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrSpeciesInvalid, (object) speciesView.CommonScientificName);
          else
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
        else if (this.Year.RecordHeight && (double) flatPlotView.Height > 0.0 && (double) flatPlotView.Height <= 450.0 && (double) flatPlotView.CrownTopHeight > (double) flatPlotView.Height)
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
        else if (this.Year.RecordHeight && (double) flatPlotView.Height > 0.0 && (double) flatPlotView.Height <= 450.0 && (double) flatPlotView.CrownBaseHeight > (double) flatPlotView.Height)
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
        float num = this.Year.Unit == YearUnit.English ? 60f : 18.29f;
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
        float num = this.Year.Unit == YearUnit.English ? 60f : 18.29f;
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
        float num = this.Year.Unit == YearUnit.English ? 60f : 18.29f;
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

    public Eco.Util.ImportSpec ImportSpec()
    {
      Eco.Util.ImportSpec<FlatPlotView> importSpec1 = new Eco.Util.ImportSpec<FlatPlotView>();
      importSpec1.RecordCount = this.m_plots.Count;
      Eco.Util.ImportSpec importSpec2 = (Eco.Util.ImportSpec) importSpec1;
      List<FieldSpec> fieldsSpecs = importSpec2.FieldsSpecs;
      if (this.Year.RecordTreeUserId)
        fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.UserId), this.dcUserId.HeaderText, false));
      if (this.Year.RecordStrata)
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.Strata), this.dcStrata.HeaderText, false).AddFormat((FieldFormat) new FieldFormat<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Description))).AddFormat((FieldFormat) new FieldFormat<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Abbreviation))).AddFormat((FieldFormat) new FieldFormat<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => (object) s.Id))).SetData(this.dcStrata.DataSource, TypeHelper.NameOf<Strata>((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => s.Self))));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.Crew), this.dcCrew.HeaderText, false));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.SurveyDate), this.dcSurveyDate.HeaderText, false));
      if (this.Year.RecordTreeStatus)
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.Status), this.dcStatus.HeaderText, false).AddFormat(new FieldFormat(typeof (TreeStatus), (string) null, "Value")).SetData(this.dcStatus.DataSource, "Key"));
      fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.Species), this.dcSpecies.HeaderText, true).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (s => s.CommonName))).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (s => s.ScientificName))).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (s => s.CommonScientificName))).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (s => s.ScientificCommonName))).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (s => s.Code))).SetData(this.dcSpecies.DataSource, TypeHelper.NameOf<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (s => s.Code))));
      if (this.Year.RecordTreeAddress)
        fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.Address), this.dcAddress.HeaderText, false));
      if (this.Year.RecordLanduse)
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.LandUse), this.dcLandUse.HeaderText, false).AddFormat((FieldFormat) new FieldFormat<LandUse>((System.Linq.Expressions.Expression<Func<LandUse, object>>) (l => l.Description))).AddFormat((FieldFormat) new FieldFormat<LandUse>((System.Linq.Expressions.Expression<Func<LandUse, object>>) (l => (object) l.Id))).SetData(this.dcLandUse.DataSource, TypeHelper.NameOf<LandUse>((System.Linq.Expressions.Expression<Func<LandUse, object>>) (l => l.Self))));
      FieldConstraint constraint1 = new FieldConstraint((AConstraint) new OrConstraint((AConstraint) new EqConstraint((object) -1.0), (AConstraint) new GtEqConstraint((object) 0.5)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) "{0}", (object) 0.5));
      FieldConstraint constraint2 = new FieldConstraint((AConstraint) new OrConstraint((AConstraint) new EqConstraint((object) -1.0), (AConstraint) new GtConstraint((object) 0.0)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) "{0}", (object) 0.0));
      FieldConstraint constraint3 = new FieldConstraint((AConstraint) new LtEqConstraint((object) 6.0), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) "{0}", (object) 6.0));
      FieldConstraint constraint4 = new FieldConstraint((AConstraint) new OrConstraint((AConstraint) new EqConstraint((object) -1.0), (AConstraint) new GtEqConstraint((object) 0.0)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) "{0}", (object) 0.0f));
      FieldConstraint constraint5 = new FieldConstraint((AConstraint) new LtEqConstraint((object) 450.0), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) "{0}", (object) 450f));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.Photo), this.dcPhoto.HeaderText, false));
      if (this.Year.DBHActual)
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH1), this.dcDBH1.HeaderText, true).SetNullValue((object) -1.0).AddConstraint(constraint1));
      else
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH1), this.dcDBH1c.HeaderText, true).SetNullValue((object) -1.0).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Id))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => d.Description))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Value))).SetData((object) this.Year.DBHs.ToList<DBH>(), TypeHelper.NameOf<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.DBHId))));
      fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH1Height), this.dcDBH1Height.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint2).AddConstraint(constraint3));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH1Measured), this.dcDBH1Measured.HeaderText, false));
      if (this.Year.DBHActual)
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH2), this.dcDBH2.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint1));
      else
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH2), this.dcDBH2c.HeaderText, false).SetNullValue((object) -1.0).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Id))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => d.Description))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Value))).SetData((object) this.Year.DBHs.ToList<DBH>(), TypeHelper.NameOf<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.DBHId))));
      fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH2Height), this.dcDBH2Height.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint2).AddConstraint(constraint3));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH2Measured), this.dcDBH2Measured.HeaderText, false));
      if (this.Year.DBHActual)
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH3), this.dcDBH3.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint1));
      else
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH3), this.dcDBH3c.HeaderText, false).SetNullValue((object) -1.0).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Id))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => d.Description))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Value))).SetData((object) this.Year.DBHs.ToList<DBH>(), TypeHelper.NameOf<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.DBHId))));
      fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH3Height), this.dcDBH3Height.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint2).AddConstraint(constraint3));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH3Measured), this.dcDBH3Measured.HeaderText, false));
      if (this.Year.DBHActual)
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH4), this.dcDBH4.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint1));
      else
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH4), this.dcDBH4c.HeaderText, false).SetNullValue((object) -1.0).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Id))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => d.Description))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Value))).SetData((object) this.Year.DBHs.ToList<DBH>(), TypeHelper.NameOf<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.DBHId))));
      fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH4Height), this.dcDBH4Height.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint2).AddConstraint(constraint3));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH4Measured), this.dcDBH4Measured.HeaderText, false));
      if (this.Year.DBHActual)
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH5), this.dcDBH5.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint1));
      else
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH5), this.dcDBH5c.HeaderText, false).SetNullValue((object) -1.0).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Id))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => d.Description))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Value))).SetData((object) this.Year.DBHs.ToList<DBH>(), TypeHelper.NameOf<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.DBHId))));
      fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH5Height), this.dcDBH5Height.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint2).AddConstraint(constraint3));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH5Measured), this.dcDBH5Measured.HeaderText, false));
      if (this.Year.DBHActual)
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH6), this.dcDBH6.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint1));
      else
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH6), this.dcDBH6c.HeaderText, false).SetNullValue((object) -1.0).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Id))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => d.Description))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Value))).SetData((object) this.Year.DBHs.ToList<DBH>(), TypeHelper.NameOf<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.DBHId))));
      fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH6Height), this.dcDBH6Height.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint2).AddConstraint(constraint3));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.DBH6Measured), this.dcDBH6Measured.HeaderText, false));
      if (this.Year.RecordCrownCondition)
      {
        if (this.Year.DisplayCondition)
          fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.CrownCondition), this.dcCrownCondition.HeaderText, false).AddFormat((FieldFormat) new FieldFormat<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => (object) c.Id))).AddFormat((FieldFormat) new FieldFormat<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Description))).AddFormat((FieldFormat) new FieldFormat<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => (object) c.Percent))).SetData((object) this.Year.Conditions.ToList<Condition>(), TypeHelper.NameOf<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Self))));
        else
          fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.CrownCondition), this.dcCrownDieback.HeaderText, false).AddFormat((FieldFormat) new FieldFormat<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => (object) c.Id))).AddFormat((FieldFormat) new FieldFormat<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.DiebackDesc))).AddFormat((FieldFormat) new FieldFormat<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => (object) c.PctDieback))).SetData((object) this.Year.Conditions.ToList<Condition>(), TypeHelper.NameOf<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Self))));
      }
      if (this.Year.RecordHeight)
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.Height), this.dcTreeHeight.HeaderText, false).AddConstraint(constraint4).AddConstraint(constraint5));
      if (this.Year.RecordCrownSize)
      {
        FieldConstraint constraint6 = new FieldConstraint((AConstraint) new OrConstraint((AConstraint) new EqConstraint((object) -1f), (AConstraint) new GtEqConstraint((object) 0.0f)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) "{0}", (object) 0.0f));
        FieldConstraint constraint7 = new FieldConstraint((AConstraint) new LtEqConstraint((object) 300f), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) "{0}", (object) 300f));
        fieldsSpecs.AddRange((IEnumerable<FieldSpec>) new FieldSpec[5]
        {
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.CrownTopHeight), this.dcCrownTopHeight.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint4).AddConstraint(constraint5),
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.CrownBaseHeight), this.dcCrownBaseHeight.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint4).AddConstraint(constraint5),
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.CrownWidthNS), this.dcCrownWidthNS.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint6).AddConstraint(constraint7),
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.CrownWidthEW), this.dcCrownWidthEW.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint6).AddConstraint(constraint7),
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.CrownPercentMissing), this.dcCrownPercentMissing.HeaderText, false).SetNullValue((object) PctMidRange.PRINV)
        });
      }
      if (this.Year.RecordCLE)
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.CrownLightExposure), this.dcCrownLightExposure.HeaderText, false).SetNullValue((object) CrownLightExposure.NotEntered));
      if (this.Year.RecordEnergy)
      {
        FieldConstraint constraint8 = new FieldConstraint((AConstraint) new OrConstraint((AConstraint) new EqConstraint((object) -1f), (AConstraint) new GtEqConstraint((object) 0.0f)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) "{0}", (object) 0.0f));
        FieldConstraint constraint9 = new FieldConstraint((AConstraint) new LtEqConstraint((object) 360f), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) "{0}", (object) 360f));
        FieldConstraint constraint10 = new FieldConstraint((AConstraint) new OrConstraint((AConstraint) new EqConstraint((object) -1f), (AConstraint) new GtConstraint((object) 0.0f)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) "{0}", (object) 0.0f));
        fieldsSpecs.AddRange((IEnumerable<FieldSpec>) new FieldSpec[8]
        {
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.B1Direction), this.dcBldg1Direction.HeaderText, false).SetNullValue((object) (short) -1).AddConstraint(constraint8).AddConstraint(constraint9),
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.B1Distance), this.dcBldg1Distance.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint10),
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.B2Direction), this.dcBldg2Direction.HeaderText, false).SetNullValue((object) (short) -1).AddConstraint(constraint8).AddConstraint(constraint9),
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.B2Distance), this.dcBldg2Distance.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint10),
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.B3Direction), this.dcBldg3Direction.HeaderText, false).SetNullValue((object) (short) -1).AddConstraint(constraint8).AddConstraint(constraint9),
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.B3Distance), this.dcBldg3Distance.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint10),
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.B4Direction), this.dcBldg4Direction.HeaderText, false).SetNullValue((object) (short) -1).AddConstraint(constraint8).AddConstraint(constraint9),
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.B4Distance), this.dcBldg4Distance.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint10)
        });
      }
      if (this.Year.RecordStreetTree)
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.StreetTree), this.dcStTree.HeaderText, false).SetDefaultValue((object) this.Year.DefaultStreetTree));
      if (this.Year.RecordMaintRec)
        fieldsSpecs.Add(this.CreateLookupField<FlatPlotView, MaintRec>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.MaintRec), this.dcMaintRec));
      if (this.Year.RecordMaintTask)
        fieldsSpecs.Add(this.CreateLookupField<FlatPlotView, MaintTask>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.MaintTask), this.dcMaintTask));
      if (this.Year.RecordSidewalk)
        fieldsSpecs.Add(this.CreateLookupField<FlatPlotView, Sidewalk>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.SidewalkDamage), this.dcSidewalk));
      if (this.Year.RecordWireConflict)
        fieldsSpecs.Add(this.CreateLookupField<FlatPlotView, WireConflict>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.WireConflict), this.dcWireConflict));
      if (this.Year.RecordOtherOne)
        fieldsSpecs.Add(this.CreateLookupField<FlatPlotView, OtherOne>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.OtherOne), this.dcFieldOne));
      if (this.Year.RecordOtherTwo)
        fieldsSpecs.Add(this.CreateLookupField<FlatPlotView, OtherTwo>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.OtherTwo), this.dcFieldTwo));
      if (this.Year.RecordOtherThree)
        fieldsSpecs.Add(this.CreateLookupField<FlatPlotView, OtherThree>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.OtherThree), this.dcFieldThree));
      if (this.Year.RecordIPED)
        fieldsSpecs.AddRange((IEnumerable<FieldSpec>) new FieldSpec[19]
        {
          this.CreateIPEDField<FlatPlotView, TSDieback>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDTSDieback), this.dcIPEDTSDieback),
          this.CreateIPEDField<FlatPlotView, TSEpiSprout>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDTSEpiSprout), this.dcIPEDTSEpiSprout),
          this.CreateIPEDField<FlatPlotView, TSWiltFoli>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDTSWiltFoli), this.dcIPEDTSWiltFoli),
          this.CreateIPEDField<FlatPlotView, TSEnvStress>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDTSEnvStress), this.dcIPEDTSEnvStress),
          this.CreateIPEDField<FlatPlotView, TSHumStress>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDTSHumStress), this.dcIPEDTSHumStress),
          (FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.IPEDTSNotes), this.dcIPEDTSNotes.HeaderText, false),
          this.CreateIPEDField<FlatPlotView, FTChewFoli>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDFTChewFoli), this.dcIPEDFTChewFoli),
          this.CreateIPEDField<FlatPlotView, FTDiscFoli>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDFTDiscFoli), this.dcIPEDFTDiscFoli),
          this.CreateIPEDField<FlatPlotView, FTAbnFoli>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDFTAbnFoli), this.dcIPEDFTAbnFoli),
          this.CreateIPEDField<FlatPlotView, FTInsectSigns>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDFTInsectSigns), this.dcIPEDFTInsectSigns),
          this.CreateIPEDField<FlatPlotView, FTFoliAffect>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDFTFoliAffect), this.dcIPEDFTFoliAffect),
          (FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.IPEDFTNotes), this.dcIPEDFTNotes.HeaderText, false),
          this.CreateIPEDField<FlatPlotView, BBInsectSigns>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDBBInsectSigns), this.dcIPEDBBInsectSigns),
          this.CreateIPEDField<FlatPlotView, BBInsectPres>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDBBInsectPres), this.dcIPEDBBInsectPres),
          this.CreateIPEDField<FlatPlotView, BBDiseaseSigns>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDBBDiseaseSigns), this.dcIPEDBBDiseaseSigns),
          this.CreateIPEDField<FlatPlotView, BBProbLoc>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDBBProbLoc), this.dcIPEDBBProbLoc),
          this.CreateIPEDField<FlatPlotView, BBAbnGrowth>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDBBAbnGrowth), this.dcIPEDBBAbnGrowth),
          (FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.IPEDBBNotes), this.dcIPEDBBNotes.HeaderText, false),
          new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.IPEDPest), this.dcIPEDPest.HeaderText, false).AddFormat((FieldFormat) new FieldFormat<Pest>((System.Linq.Expressions.Expression<Func<Pest, object>>) (p => p.ScientificName))).AddFormat((FieldFormat) new FieldFormat<Pest>((System.Linq.Expressions.Expression<Func<Pest, object>>) (p => p.CommonName))).AddFormat((FieldFormat) new FieldFormat<Pest>((System.Linq.Expressions.Expression<Func<Pest, object>>) (p => (object) p.Id))).SetData(this.dcIPEDPest.DataSource, TypeHelper.NameOf<Pest>((System.Linq.Expressions.Expression<Func<Pest, object>>) (p => (object) p.Id)))
        });
      if (this.Year.MgmtStyle.HasValue && (this.Year.MgmtStyle.Value & MgmtStyleEnum.RecordPublicPrivate) != MgmtStyleEnum.DefaultPublic)
        fieldsSpecs.Add(new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.CityManaged), this.dcCityManaged.HeaderText, false).SetDefaultValue((object) ((this.Year.MgmtStyle.Value & MgmtStyleEnum.DefaultPrivate) == MgmtStyleEnum.DefaultPublic)));
      if (this.Year.RecordTreeGPS)
        fieldsSpecs.AddRange((IEnumerable<FieldSpec>) new FieldSpec[2]
        {
          (FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.Latitude), this.dcLatitude.HeaderText, false),
          (FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => (object) fpv.Longitude), this.dcLongitude.HeaderText, false)
        });
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatPlotView>((System.Linq.Expressions.Expression<Func<FlatPlotView, object>>) (fpv => fpv.Comments), this.dcComments.HeaderText, false));
      return importSpec2;
    }

    private FieldSpec CreateLookupField<T, V>(
      System.Linq.Expressions.Expression<Func<T, object>> exp,
      DataGridViewComboBoxColumn col)
      where V : Eco.Domain.v6.Lookup<V>
    {
      return new FieldSpec<T>(exp, col.HeaderText, false).AddFormat((FieldFormat) new FieldFormat<V>((System.Linq.Expressions.Expression<Func<V, object>>) (l => l.Description))).AddFormat((FieldFormat) new FieldFormat<V>((System.Linq.Expressions.Expression<Func<V, object>>) (l => (object) l.Id))).SetData(col.DataSource, TypeHelper.NameOf<V>((System.Linq.Expressions.Expression<Func<V, object>>) (l => l.Self)));
    }

    private FieldSpec CreateIPEDField<T, V>(
      System.Linq.Expressions.Expression<Func<T, object>> exp,
      DataGridViewComboBoxColumn col)
      where V : IPED.Domain.Lookup
    {
      return new FieldSpec<T>(exp, col.HeaderText, false).AddFormat((FieldFormat) new FieldFormat<V>((System.Linq.Expressions.Expression<Func<V, object>>) (l => l.Description))).AddFormat((FieldFormat) new FieldFormat<V>((System.Linq.Expressions.Expression<Func<V, object>>) (l => (object) l.Code))).SetData(col.DataSource, TypeHelper.NameOf<V>((System.Linq.Expressions.Expression<Func<V, object>>) (l => (object) l.Code)));
    }

    public Task ImportData(
      IList data,
      IProgress<ProgressEventArgs> progress,
      CancellationToken ct)
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      return Task.Factory.StartNew((System.Action) (() => this.DoImport(data, progress, ct)), ct, TaskCreationOptions.None, Program.Session.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.Year == null || t.IsCanceled || t.IsFaulted)
          return;
        lock (this.Session)
        {
          using (TypeHelper<Plot> typeHelper = new TypeHelper<Plot>())
          {
            PagedList<Plot> pagedList = this.Session.CreateCriteria<Plot>().Add((ICriterion) Restrictions.Eq(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Year)), (object) this.Year)).Fetch(SelectMode.Fetch, typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Trees))).Fetch(SelectMode.Fetch, typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.PlotLandUses))).AddOrder(Order.Asc(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)))).SetResultTransformer((IResultTransformer) new DistinctRootEntityResultTransformer()).PagedList<Plot>(100);
            this.m_plots = pagedList;
            this.dgPlots.RowCount = pagedList.Count;
            this.dgPlots.RowCount = this.m_plots.Count + 1;
          }
        }
      }), scheduler);
    }

    private void DoImport(IList data, IProgress<ProgressEventArgs> progress, CancellationToken ct)
    {
      Condition condition1 = (Condition) null;
      ISet<Condition> conditions = this.Year.Conditions;
      if (conditions != null)
      {
        foreach (Condition condition2 in (IEnumerable<Condition>) conditions)
        {
          if (condition2.PctDieback + 1.0 <= double.Epsilon)
          {
            condition1 = condition2;
            break;
          }
        }
      }
      Strata strata = this.Year.Strata.FirstOrDefault<Strata>();
      int num1 = this.NextPlotId();
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
            if (obj is FlatPlotView flatPlotView)
            {
              if (flatPlotView.CrownCondition == null)
                flatPlotView.CrownCondition = condition1;
              if (flatPlotView.Strata == null)
                flatPlotView.Strata = strata;
              flatPlotView.Plot.Year = this.Year;
              if (flatPlotView.Id == 0 || flatPlotView.Id < num1)
                flatPlotView.Id = num1++;
              else
                num1 = flatPlotView.Id + 1;
              flatPlotView.Session = this.Session;
              this.Session.SaveOrUpdate((object) flatPlotView.Plot);
              guidList.Add(flatPlotView.Plot.Guid);
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
        catch (Exception ex)
        {
          switch (ex)
          {
            case OperationCanceledException _:
            case HibernateException _:
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
                  progress.Report(new ProgressEventArgs(i_Tree_Eco_v6.Resources.Strings.MsgCanceling, count / num2, (num3 - index) / num2));
                }
              }
              transaction2.Commit();
              transaction2.Dispose();
              break;
          }
          throw;
        }
      }
      ct.ThrowIfCancellationRequested();
    }

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

    private void dgPlots_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.m_plots == null || e.RowIndex >= this.m_plots.Count)
        return;
      Plot plot = this.GetPlot(e.RowIndex);
      FlatPlotView flatPlotView = this.m_fpv;
      if (!plot.Equals((object) flatPlotView?.Plot))
        flatPlotView = new FlatPlotView(this.Session, plot);
      (Func<object, object> getValue, System.Action<object, object> setValue) fpvProperty = this.m_fpvProperties[this.dgPlots.Columns[e.ColumnIndex].DataPropertyName];
      e.Value = fpvProperty.getValue((object) flatPlotView);
    }

    private void dgPlots_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
      if (this.m_plots == null || e.RowIndex >= this.m_plots.Count)
        return;
      Plot plot = this.GetPlot(e.RowIndex);
      FlatPlotView flatPlotView = this.m_fpv;
      if (!plot.Equals((object) flatPlotView?.Plot))
        flatPlotView = new FlatPlotView(this.Session, plot);
      (Func<object, object> getValue, System.Action<object, object> setValue) fpvProperty = this.m_fpvProperties[this.dgPlots.Columns[e.ColumnIndex].DataPropertyName];
      if (fpvProperty.setValue == null)
        return;
      fpvProperty.setValue((object) flatPlotView, e.Value);
    }

    private void dgPlots_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
    {
      if (this.m_inDelete)
        return;
      MgmtStyleEnum? mgmtStyle = this.Year.MgmtStyle;
      short? nullable = mgmtStyle.HasValue ? new short?((short) mgmtStyle.GetValueOrDefault()) : new short?();
      if (nullable.HasValue)
        nullable = new short?((short) ((int) nullable.Value & 1));
      Condition condition1 = (Condition) null;
      ISet<Condition> conditions = this.Year.Conditions;
      if (conditions != null)
      {
        foreach (Condition condition2 in (IEnumerable<Condition>) conditions)
        {
          if (condition2.PctDieback + 1.0 <= double.Epsilon)
          {
            condition1 = condition2;
            break;
          }
        }
      }
      Strata strata = (Strata) null;
      if (!this.Year.RecordStrata)
        strata = this.Year.Strata.FirstOrDefault<Strata>();
      Plot plot = new Plot()
      {
        Id = this.NextPlotId(),
        Date = new DateTime?(DateTime.Today),
        Year = this.Year,
        Strata = strata,
        IsComplete = true
      };
      FlatPlotView flatPlotView = new FlatPlotView(this.Session, plot);
      Tree tree = flatPlotView.Tree;
      tree.CityManaged = nullable;
      tree.StreetTree = this.Year.RecordStreetTree && this.Year.DefaultStreetTree;
      tree.Crown.Condition = condition1;
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          this.Session.SaveOrUpdate((object) plot);
          transaction.Commit();
        }
      }
      this.m_plots.Add(flatPlotView.Plot);
    }

    private void dgPlots_RowDirtyStateNeeded(object sender, QuestionEventArgs e) => e.Response = this.m_rowDirty;

    private void dgPlots_RowEnter(object sender, DataGridViewCellEventArgs e)
    {
      if (this.m_plots == null || e.RowIndex >= this.m_plots.Count)
        return;
      Plot plot = this.GetPlot(e.RowIndex);
      if (plot.Equals((object) this.m_fpv?.Plot))
        return;
      this.m_rowDirty = false;
      this.m_fpv = new FlatPlotView(this.Session, plot);
    }

    private void dgPlots_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) => this.DeleteRows(new DataGridViewRow[1]
    {
      e.Row
    }, false);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (InventoryTreesForm));
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
      this.dgPlots = new DataGridView();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcUserId = new DataGridViewTextBoxColumn();
      this.dcStrata = new DataGridViewComboBoxColumn();
      this.dcCrew = new DataGridViewTextBoxColumn();
      this.dcSurveyDate = new DataGridViewNullableDateTimeColumn();
      this.dcStatus = new DataGridViewComboBoxColumn();
      this.dcSpecies = new DataGridViewFilteredComboBoxColumn();
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
      this.dcBldg4Direction = new DataGridViewNumericTextBoxColumn();
      this.dcBldg4Distance = new DataGridViewNumericTextBoxColumn();
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
      ((ISupportInitialize) this.dgPlots).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgPlots.AllowUserToAddRows = false;
      this.dgPlots.AllowUserToDeleteRows = false;
      this.dgPlots.AllowUserToResizeRows = false;
      this.dgPlots.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgPlots.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgPlots.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcUserId, (DataGridViewColumn) this.dcStrata, (DataGridViewColumn) this.dcCrew, (DataGridViewColumn) this.dcSurveyDate, (DataGridViewColumn) this.dcStatus, (DataGridViewColumn) this.dcSpecies, (DataGridViewColumn) this.dcAddress, (DataGridViewColumn) this.dcLandUse, (DataGridViewColumn) this.dcPhoto, (DataGridViewColumn) this.dcDBH1, (DataGridViewColumn) this.dcDBH1c, (DataGridViewColumn) this.dcDBH1Height, (DataGridViewColumn) this.dcDBH1Measured, (DataGridViewColumn) this.dcDBH2, (DataGridViewColumn) this.dcDBH2c, (DataGridViewColumn) this.dcDBH2Height, (DataGridViewColumn) this.dcDBH2Measured, (DataGridViewColumn) this.dcDBH3, (DataGridViewColumn) this.dcDBH3c, (DataGridViewColumn) this.dcDBH3Height, (DataGridViewColumn) this.dcDBH3Measured, (DataGridViewColumn) this.dcDBH4, (DataGridViewColumn) this.dcDBH4c, (DataGridViewColumn) this.dcDBH4Height, (DataGridViewColumn) this.dcDBH4Measured, (DataGridViewColumn) this.dcDBH5, (DataGridViewColumn) this.dcDBH5c, (DataGridViewColumn) this.dcDBH5Height, (DataGridViewColumn) this.dcDBH5Measured, (DataGridViewColumn) this.dcDBH6, (DataGridViewColumn) this.dcDBH6c, (DataGridViewColumn) this.dcDBH6Height, (DataGridViewColumn) this.dcDBH6Measured, (DataGridViewColumn) this.dcCrownCondition, (DataGridViewColumn) this.dcCrownDieback, (DataGridViewColumn) this.dcTreeHeight, (DataGridViewColumn) this.dcCrownTopHeight, (DataGridViewColumn) this.dcCrownBaseHeight, (DataGridViewColumn) this.dcCrownWidthNS, (DataGridViewColumn) this.dcCrownWidthEW, (DataGridViewColumn) this.dcCrownPercentMissing, (DataGridViewColumn) this.dcCrownLightExposure, (DataGridViewColumn) this.dcBldg1Direction, (DataGridViewColumn) this.dcBldg1Distance, (DataGridViewColumn) this.dcBldg2Direction, (DataGridViewColumn) this.dcBldg2Distance, (DataGridViewColumn) this.dcBldg3Direction, (DataGridViewColumn) this.dcBldg3Distance, (DataGridViewColumn) this.dcBldg4Direction, (DataGridViewColumn) this.dcBldg4Distance, (DataGridViewColumn) this.dcSiteType, (DataGridViewColumn) this.dcStTree, (DataGridViewColumn) this.dcMaintRec, (DataGridViewColumn) this.dcMaintTask, (DataGridViewColumn) this.dcSidewalk, (DataGridViewColumn) this.dcWireConflict, (DataGridViewColumn) this.dcFieldOne, (DataGridViewColumn) this.dcFieldTwo, (DataGridViewColumn) this.dcFieldThree, (DataGridViewColumn) this.dcIPEDTSDieback, (DataGridViewColumn) this.dcIPEDTSEpiSprout, (DataGridViewColumn) this.dcIPEDTSWiltFoli, (DataGridViewColumn) this.dcIPEDTSEnvStress, (DataGridViewColumn) this.dcIPEDTSHumStress, (DataGridViewColumn) this.dcIPEDTSNotes, (DataGridViewColumn) this.dcIPEDFTChewFoli, (DataGridViewColumn) this.dcIPEDFTDiscFoli, (DataGridViewColumn) this.dcIPEDFTAbnFoli, (DataGridViewColumn) this.dcIPEDFTInsectSigns, (DataGridViewColumn) this.dcIPEDFTFoliAffect, (DataGridViewColumn) this.dcIPEDFTNotes, (DataGridViewColumn) this.dcIPEDBBInsectSigns, (DataGridViewColumn) this.dcIPEDBBInsectPres, (DataGridViewColumn) this.dcIPEDBBDiseaseSigns, (DataGridViewColumn) this.dcIPEDBBProbLoc, (DataGridViewColumn) this.dcIPEDBBAbnGrowth, (DataGridViewColumn) this.dcIPEDBBNotes, (DataGridViewColumn) this.dcIPEDPest, (DataGridViewColumn) this.dcCityManaged, (DataGridViewColumn) this.dcLocSite, (DataGridViewColumn) this.dcLocNo, (DataGridViewColumn) this.dcLatitude, (DataGridViewColumn) this.dcLongitude, (DataGridViewColumn) this.dcNoteTree, (DataGridViewColumn) this.dcComments);
      componentResourceManager.ApplyResources((object) this.dgPlots, "dgPlots");
      this.dgPlots.EnableHeadersVisualStyles = false;
      this.dgPlots.Name = "dgPlots";
      this.dgPlots.ReadOnly = true;
      this.dgPlots.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgPlots.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgPlots.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgPlots.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgPlots.VirtualMode = true;
      this.dgPlots.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgPlots_CellErrorTextNeeded);
      this.dgPlots.CellParsing += new DataGridViewCellParsingEventHandler(this.dgPlots_CellParsing);
      this.dgPlots.CellValueChanged += new DataGridViewCellEventHandler(this.dgPlots_CellValueChanged);
      this.dgPlots.CellValueNeeded += new DataGridViewCellValueEventHandler(this.dgPlots_CellValueNeeded);
      this.dgPlots.CellValuePushed += new DataGridViewCellValueEventHandler(this.dgPlots_CellValuePushed);
      this.dgPlots.CurrentCellDirtyStateChanged += new EventHandler(this.dgPlots_CurrentCellDirtyStateChanged);
      this.dgPlots.DataError += new DataGridViewDataErrorEventHandler(this.dgPlots_DataError);
      this.dgPlots.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgPlots_EditingControlShowing);
      this.dgPlots.NewRowNeeded += new DataGridViewRowEventHandler(this.dgPlots_NewRowNeeded);
      this.dgPlots.RowDirtyStateNeeded += new QuestionEventHandler(this.dgPlots_RowDirtyStateNeeded);
      this.dgPlots.RowEnter += new DataGridViewCellEventHandler(this.dgPlots_RowEnter);
      this.dgPlots.RowValidated += new DataGridViewCellEventHandler(this.dgPlots_RowValidated);
      this.dgPlots.RowValidating += new DataGridViewCellCancelEventHandler(this.dgPlots_RowValidating);
      this.dgPlots.Scroll += new ScrollEventHandler(this.dgPlots_Scroll);
      this.dgPlots.SelectionChanged += new EventHandler(this.dgPlots_SelectionChanged);
      this.dgPlots.Sorted += new EventHandler(this.dgPlots_Sorted);
      this.dgPlots.UserDeletingRow += new DataGridViewRowCancelEventHandler(this.dgPlots_UserDeletingRow);
      this.dcId.DataPropertyName = "Id";
      this.dcId.DecimalPlaces = 0;
      this.dcId.Format = (string) null;
      this.dcId.Frozen = true;
      this.dcId.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcId, "dcId");
      this.dcId.Name = "dcId";
      this.dcId.ReadOnly = true;
      this.dcId.Resizable = DataGridViewTriState.True;
      this.dcId.Signed = false;
      this.dcUserId.DataPropertyName = "UserId";
      this.dcUserId.Frozen = true;
      componentResourceManager.ApplyResources((object) this.dcUserId, "dcUserId");
      this.dcUserId.Name = "dcUserId";
      this.dcUserId.ReadOnly = true;
      this.dcStrata.DataPropertyName = "Strata";
      this.dcStrata.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcStrata, "dcStrata");
      this.dcStrata.Name = "dcStrata";
      this.dcStrata.ReadOnly = true;
      this.dcStrata.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcCrew.DataPropertyName = "Crew";
      componentResourceManager.ApplyResources((object) this.dcCrew, "dcCrew");
      this.dcCrew.Name = "dcCrew";
      this.dcCrew.ReadOnly = true;
      this.dcSurveyDate.CustomFormat = (string) null;
      this.dcSurveyDate.DataPropertyName = "SurveyDate";
      this.dcSurveyDate.DateFormat = DateTimePickerFormat.Short;
      componentResourceManager.ApplyResources((object) this.dcSurveyDate, "dcSurveyDate");
      this.dcSurveyDate.Name = "dcSurveyDate";
      this.dcSurveyDate.ReadOnly = true;
      this.dcStatus.DataPropertyName = "Status";
      this.dcStatus.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcStatus, "dcStatus");
      this.dcStatus.Name = "dcStatus";
      this.dcStatus.ReadOnly = true;
      this.dcStatus.Resizable = DataGridViewTriState.True;
      this.dcStatus.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcSpecies.DataPropertyName = "Species";
      this.dcSpecies.DataSource = (object) null;
      this.dcSpecies.DisplayMember = (string) null;
      componentResourceManager.ApplyResources((object) this.dcSpecies, "dcSpecies");
      this.dcSpecies.Name = "dcSpecies";
      this.dcSpecies.ReadOnly = true;
      this.dcSpecies.Resizable = DataGridViewTriState.True;
      this.dcSpecies.ValueMember = (string) null;
      this.dcAddress.DataPropertyName = "Address";
      componentResourceManager.ApplyResources((object) this.dcAddress, "dcAddress");
      this.dcAddress.MaxInputLength = (int) byte.MaxValue;
      this.dcAddress.Name = "dcAddress";
      this.dcAddress.ReadOnly = true;
      this.dcLandUse.DataPropertyName = "LandUse";
      this.dcLandUse.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcLandUse, "dcLandUse");
      this.dcLandUse.Name = "dcLandUse";
      this.dcLandUse.ReadOnly = true;
      this.dcLandUse.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPhoto.DataPropertyName = "Photo";
      componentResourceManager.ApplyResources((object) this.dcPhoto, "dcPhoto");
      this.dcPhoto.MaxInputLength = 100;
      this.dcPhoto.Name = "dcPhoto";
      this.dcPhoto.ReadOnly = true;
      this.dcDBH1.DataPropertyName = "DBH1";
      this.dcDBH1.DecimalPlaces = 1;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle1.Format = "N1";
      this.dcDBH1.DefaultCellStyle = gridViewCellStyle1;
      this.dcDBH1.Format = "#;#";
      this.dcDBH1.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH1, "dcDBH1");
      this.dcDBH1.Name = "dcDBH1";
      this.dcDBH1.ReadOnly = true;
      this.dcDBH1.Resizable = DataGridViewTriState.True;
      this.dcDBH1.Signed = false;
      this.dcDBH1c.DataPropertyName = "DBH1";
      this.dcDBH1c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcDBH1c, "dcDBH1c");
      this.dcDBH1c.Name = "dcDBH1c";
      this.dcDBH1c.ReadOnly = true;
      this.dcDBH1c.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH1Height.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH1Height.DataPropertyName = "DBH1Height";
      this.dcDBH1Height.DecimalPlaces = 2;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle2.Format = "N2";
      this.dcDBH1Height.DefaultCellStyle = gridViewCellStyle2;
      this.dcDBH1Height.Format = "#;#";
      this.dcDBH1Height.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH1Height, "dcDBH1Height");
      this.dcDBH1Height.Name = "dcDBH1Height";
      this.dcDBH1Height.ReadOnly = true;
      this.dcDBH1Height.Resizable = DataGridViewTriState.True;
      this.dcDBH1Height.Signed = false;
      this.dcDBH1Measured.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH1Measured.DataPropertyName = "DBH1Measured";
      componentResourceManager.ApplyResources((object) this.dcDBH1Measured, "dcDBH1Measured");
      this.dcDBH1Measured.Name = "dcDBH1Measured";
      this.dcDBH1Measured.ReadOnly = true;
      this.dcDBH1Measured.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH2.DataPropertyName = "DBH2";
      this.dcDBH2.DecimalPlaces = 1;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle3.Format = "N1";
      this.dcDBH2.DefaultCellStyle = gridViewCellStyle3;
      this.dcDBH2.Format = "#;#";
      this.dcDBH2.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH2, "dcDBH2");
      this.dcDBH2.Name = "dcDBH2";
      this.dcDBH2.ReadOnly = true;
      this.dcDBH2.Resizable = DataGridViewTriState.True;
      this.dcDBH2.Signed = false;
      this.dcDBH2c.DataPropertyName = "DBH2";
      this.dcDBH2c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcDBH2c, "dcDBH2c");
      this.dcDBH2c.Name = "dcDBH2c";
      this.dcDBH2c.ReadOnly = true;
      this.dcDBH2c.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH2Height.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH2Height.DataPropertyName = "DBH2Height";
      this.dcDBH2Height.DecimalPlaces = 2;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle4.Format = "N2";
      this.dcDBH2Height.DefaultCellStyle = gridViewCellStyle4;
      this.dcDBH2Height.Format = "#;#";
      this.dcDBH2Height.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH2Height, "dcDBH2Height");
      this.dcDBH2Height.Name = "dcDBH2Height";
      this.dcDBH2Height.ReadOnly = true;
      this.dcDBH2Height.Resizable = DataGridViewTriState.True;
      this.dcDBH2Height.Signed = false;
      this.dcDBH2Measured.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH2Measured.DataPropertyName = "DBH2Measured";
      componentResourceManager.ApplyResources((object) this.dcDBH2Measured, "dcDBH2Measured");
      this.dcDBH2Measured.Name = "dcDBH2Measured";
      this.dcDBH2Measured.ReadOnly = true;
      this.dcDBH2Measured.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH3.DataPropertyName = "DBH3";
      this.dcDBH3.DecimalPlaces = 1;
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle5.Format = "N1";
      this.dcDBH3.DefaultCellStyle = gridViewCellStyle5;
      this.dcDBH3.Format = "#;#";
      this.dcDBH3.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH3, "dcDBH3");
      this.dcDBH3.Name = "dcDBH3";
      this.dcDBH3.ReadOnly = true;
      this.dcDBH3.Resizable = DataGridViewTriState.True;
      this.dcDBH3.Signed = false;
      this.dcDBH3c.DataPropertyName = "DBH3";
      this.dcDBH3c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcDBH3c, "dcDBH3c");
      this.dcDBH3c.Name = "dcDBH3c";
      this.dcDBH3c.ReadOnly = true;
      this.dcDBH3c.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH3Height.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH3Height.DataPropertyName = "DBH3Height";
      this.dcDBH3Height.DecimalPlaces = 2;
      gridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle6.Format = "N2";
      this.dcDBH3Height.DefaultCellStyle = gridViewCellStyle6;
      this.dcDBH3Height.Format = "#;#";
      this.dcDBH3Height.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH3Height, "dcDBH3Height");
      this.dcDBH3Height.Name = "dcDBH3Height";
      this.dcDBH3Height.ReadOnly = true;
      this.dcDBH3Height.Resizable = DataGridViewTriState.True;
      this.dcDBH3Height.Signed = false;
      this.dcDBH3Measured.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH3Measured.DataPropertyName = "DBH3Measured";
      componentResourceManager.ApplyResources((object) this.dcDBH3Measured, "dcDBH3Measured");
      this.dcDBH3Measured.Name = "dcDBH3Measured";
      this.dcDBH3Measured.ReadOnly = true;
      this.dcDBH3Measured.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH4.DataPropertyName = "DBH4";
      this.dcDBH4.DecimalPlaces = 1;
      gridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle7.Format = "N1";
      this.dcDBH4.DefaultCellStyle = gridViewCellStyle7;
      this.dcDBH4.Format = "#;#";
      this.dcDBH4.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH4, "dcDBH4");
      this.dcDBH4.Name = "dcDBH4";
      this.dcDBH4.ReadOnly = true;
      this.dcDBH4.Signed = false;
      this.dcDBH4c.DataPropertyName = "DBH4";
      this.dcDBH4c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcDBH4c, "dcDBH4c");
      this.dcDBH4c.Name = "dcDBH4c";
      this.dcDBH4c.ReadOnly = true;
      this.dcDBH4c.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH4Height.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH4Height.DataPropertyName = "DBH4Height";
      this.dcDBH4Height.DecimalPlaces = 2;
      gridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle8.Format = "N2";
      this.dcDBH4Height.DefaultCellStyle = gridViewCellStyle8;
      this.dcDBH4Height.Format = "#;#";
      this.dcDBH4Height.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH4Height, "dcDBH4Height");
      this.dcDBH4Height.Name = "dcDBH4Height";
      this.dcDBH4Height.ReadOnly = true;
      this.dcDBH4Height.Signed = false;
      this.dcDBH4Measured.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH4Measured.DataPropertyName = "DBH4Measured";
      componentResourceManager.ApplyResources((object) this.dcDBH4Measured, "dcDBH4Measured");
      this.dcDBH4Measured.Name = "dcDBH4Measured";
      this.dcDBH4Measured.ReadOnly = true;
      this.dcDBH4Measured.Resizable = DataGridViewTriState.True;
      this.dcDBH4Measured.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH5.DataPropertyName = "DBH5";
      this.dcDBH5.DecimalPlaces = 1;
      gridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle9.Format = "N1";
      this.dcDBH5.DefaultCellStyle = gridViewCellStyle9;
      this.dcDBH5.Format = "#;#";
      this.dcDBH5.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH5, "dcDBH5");
      this.dcDBH5.Name = "dcDBH5";
      this.dcDBH5.ReadOnly = true;
      this.dcDBH5.Signed = false;
      this.dcDBH5c.DataPropertyName = "DBH5";
      this.dcDBH5c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcDBH5c, "dcDBH5c");
      this.dcDBH5c.Name = "dcDBH5c";
      this.dcDBH5c.ReadOnly = true;
      this.dcDBH5c.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH5Height.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH5Height.DataPropertyName = "DBH5Height";
      this.dcDBH5Height.DecimalPlaces = 2;
      gridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle10.Format = "N2";
      this.dcDBH5Height.DefaultCellStyle = gridViewCellStyle10;
      this.dcDBH5Height.Format = "#;#";
      this.dcDBH5Height.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH5Height, "dcDBH5Height");
      this.dcDBH5Height.Name = "dcDBH5Height";
      this.dcDBH5Height.ReadOnly = true;
      this.dcDBH5Height.Signed = false;
      this.dcDBH5Measured.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH5Measured.DataPropertyName = "DBH5Measured";
      componentResourceManager.ApplyResources((object) this.dcDBH5Measured, "dcDBH5Measured");
      this.dcDBH5Measured.Name = "dcDBH5Measured";
      this.dcDBH5Measured.ReadOnly = true;
      this.dcDBH5Measured.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH6.DataPropertyName = "DBH6";
      this.dcDBH6.DecimalPlaces = 1;
      gridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle11.Format = "N1";
      this.dcDBH6.DefaultCellStyle = gridViewCellStyle11;
      this.dcDBH6.Format = "#;#";
      this.dcDBH6.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH6, "dcDBH6");
      this.dcDBH6.Name = "dcDBH6";
      this.dcDBH6.ReadOnly = true;
      this.dcDBH6.Signed = false;
      this.dcDBH6c.DataPropertyName = "DBH6";
      this.dcDBH6c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcDBH6c, "dcDBH6c");
      this.dcDBH6c.Name = "dcDBH6c";
      this.dcDBH6c.ReadOnly = true;
      this.dcDBH6c.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH6Height.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH6Height.DataPropertyName = "DBH6Height";
      this.dcDBH6Height.DecimalPlaces = 2;
      gridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle12.Format = "N2";
      this.dcDBH6Height.DefaultCellStyle = gridViewCellStyle12;
      this.dcDBH6Height.Format = "#;#";
      this.dcDBH6Height.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH6Height, "dcDBH6Height");
      this.dcDBH6Height.Name = "dcDBH6Height";
      this.dcDBH6Height.ReadOnly = true;
      this.dcDBH6Height.Signed = false;
      this.dcDBH6Measured.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH6Measured.DataPropertyName = "DBH6Measured";
      componentResourceManager.ApplyResources((object) this.dcDBH6Measured, "dcDBH6Measured");
      this.dcDBH6Measured.Name = "dcDBH6Measured";
      this.dcDBH6Measured.ReadOnly = true;
      this.dcDBH6Measured.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcCrownCondition.DataPropertyName = "CrownCondition";
      this.dcCrownCondition.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcCrownCondition, "dcCrownCondition");
      this.dcCrownCondition.Name = "dcCrownCondition";
      this.dcCrownCondition.ReadOnly = true;
      this.dcCrownCondition.Resizable = DataGridViewTriState.True;
      this.dcCrownCondition.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcCrownDieback.DataPropertyName = "CrownCondition";
      this.dcCrownDieback.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcCrownDieback, "dcCrownDieback");
      this.dcCrownDieback.Name = "dcCrownDieback";
      this.dcCrownDieback.ReadOnly = true;
      this.dcCrownDieback.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcTreeHeight.DataPropertyName = "Height";
      this.dcTreeHeight.DecimalPlaces = 2;
      gridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle13.Format = "N2";
      this.dcTreeHeight.DefaultCellStyle = gridViewCellStyle13;
      this.dcTreeHeight.Format = (string) null;
      this.dcTreeHeight.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcTreeHeight, "dcTreeHeight");
      this.dcTreeHeight.Name = "dcTreeHeight";
      this.dcTreeHeight.ReadOnly = true;
      this.dcTreeHeight.Resizable = DataGridViewTriState.True;
      this.dcTreeHeight.Signed = false;
      this.dcCrownTopHeight.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownTopHeight.DataPropertyName = "CrownTopHeight";
      this.dcCrownTopHeight.DecimalPlaces = 2;
      gridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle14.Format = "N2";
      this.dcCrownTopHeight.DefaultCellStyle = gridViewCellStyle14;
      this.dcCrownTopHeight.Format = "#;#";
      this.dcCrownTopHeight.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownTopHeight, "dcCrownTopHeight");
      this.dcCrownTopHeight.Name = "dcCrownTopHeight";
      this.dcCrownTopHeight.ReadOnly = true;
      this.dcCrownTopHeight.Signed = false;
      this.dcCrownBaseHeight.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownBaseHeight.DataPropertyName = "CrownBaseHeight";
      this.dcCrownBaseHeight.DecimalPlaces = 2;
      gridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle15.Format = "N2";
      this.dcCrownBaseHeight.DefaultCellStyle = gridViewCellStyle15;
      this.dcCrownBaseHeight.Format = "0.#;0.#";
      this.dcCrownBaseHeight.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownBaseHeight, "dcCrownBaseHeight");
      this.dcCrownBaseHeight.Name = "dcCrownBaseHeight";
      this.dcCrownBaseHeight.ReadOnly = true;
      this.dcCrownBaseHeight.Signed = false;
      this.dcCrownWidthNS.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownWidthNS.DataPropertyName = "CrownWidthNS";
      this.dcCrownWidthNS.DecimalPlaces = 1;
      gridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle16.Format = "N1";
      this.dcCrownWidthNS.DefaultCellStyle = gridViewCellStyle16;
      this.dcCrownWidthNS.Format = "#;#";
      this.dcCrownWidthNS.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownWidthNS, "dcCrownWidthNS");
      this.dcCrownWidthNS.Name = "dcCrownWidthNS";
      this.dcCrownWidthNS.ReadOnly = true;
      this.dcCrownWidthNS.Signed = false;
      this.dcCrownWidthEW.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownWidthEW.DataPropertyName = "CrownWidthEW";
      this.dcCrownWidthEW.DecimalPlaces = 1;
      gridViewCellStyle17.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle17.Format = "N1";
      this.dcCrownWidthEW.DefaultCellStyle = gridViewCellStyle17;
      this.dcCrownWidthEW.Format = "#;#";
      this.dcCrownWidthEW.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownWidthEW, "dcCrownWidthEW");
      this.dcCrownWidthEW.Name = "dcCrownWidthEW";
      this.dcCrownWidthEW.ReadOnly = true;
      this.dcCrownWidthEW.Signed = false;
      this.dcCrownPercentMissing.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownPercentMissing.DataPropertyName = "CrownPercentMissing";
      gridViewCellStyle18.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcCrownPercentMissing.DefaultCellStyle = gridViewCellStyle18;
      this.dcCrownPercentMissing.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcCrownPercentMissing, "dcCrownPercentMissing");
      this.dcCrownPercentMissing.Name = "dcCrownPercentMissing";
      this.dcCrownPercentMissing.ReadOnly = true;
      this.dcCrownPercentMissing.Resizable = DataGridViewTriState.True;
      this.dcCrownPercentMissing.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcCrownLightExposure.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownLightExposure.DataPropertyName = "CrownLightExposure";
      this.dcCrownLightExposure.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcCrownLightExposure, "dcCrownLightExposure");
      this.dcCrownLightExposure.Name = "dcCrownLightExposure";
      this.dcCrownLightExposure.ReadOnly = true;
      this.dcCrownLightExposure.Resizable = DataGridViewTriState.True;
      this.dcCrownLightExposure.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcBldg1Direction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg1Direction.DataPropertyName = "B1Direction";
      this.dcBldg1Direction.DecimalPlaces = 0;
      gridViewCellStyle19.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle19.Format = "N0";
      this.dcBldg1Direction.DefaultCellStyle = gridViewCellStyle19;
      this.dcBldg1Direction.Format = "#;#";
      this.dcBldg1Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg1Direction, "dcBldg1Direction");
      this.dcBldg1Direction.Name = "dcBldg1Direction";
      this.dcBldg1Direction.ReadOnly = true;
      this.dcBldg1Direction.Signed = false;
      this.dcBldg1Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg1Distance.DataPropertyName = "B1Distance";
      this.dcBldg1Distance.DecimalPlaces = 2;
      gridViewCellStyle20.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle20.Format = "N2";
      this.dcBldg1Distance.DefaultCellStyle = gridViewCellStyle20;
      this.dcBldg1Distance.Format = "#;#";
      this.dcBldg1Distance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcBldg1Distance, "dcBldg1Distance");
      this.dcBldg1Distance.Name = "dcBldg1Distance";
      this.dcBldg1Distance.ReadOnly = true;
      this.dcBldg1Distance.Signed = false;
      this.dcBldg2Direction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg2Direction.DataPropertyName = "B2Direction";
      this.dcBldg2Direction.DecimalPlaces = 0;
      gridViewCellStyle21.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle21.Format = "N0";
      this.dcBldg2Direction.DefaultCellStyle = gridViewCellStyle21;
      this.dcBldg2Direction.Format = "#;#";
      this.dcBldg2Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg2Direction, "dcBldg2Direction");
      this.dcBldg2Direction.Name = "dcBldg2Direction";
      this.dcBldg2Direction.ReadOnly = true;
      this.dcBldg2Direction.Signed = false;
      this.dcBldg2Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg2Distance.DataPropertyName = "B2Distance";
      this.dcBldg2Distance.DecimalPlaces = 2;
      gridViewCellStyle22.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle22.Format = "N2";
      this.dcBldg2Distance.DefaultCellStyle = gridViewCellStyle22;
      this.dcBldg2Distance.Format = "#;#";
      this.dcBldg2Distance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcBldg2Distance, "dcBldg2Distance");
      this.dcBldg2Distance.Name = "dcBldg2Distance";
      this.dcBldg2Distance.ReadOnly = true;
      this.dcBldg2Distance.Signed = false;
      this.dcBldg3Direction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg3Direction.DataPropertyName = "B3Direction";
      this.dcBldg3Direction.DecimalPlaces = 0;
      gridViewCellStyle23.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle23.Format = "N0";
      this.dcBldg3Direction.DefaultCellStyle = gridViewCellStyle23;
      this.dcBldg3Direction.Format = "#;#";
      this.dcBldg3Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg3Direction, "dcBldg3Direction");
      this.dcBldg3Direction.Name = "dcBldg3Direction";
      this.dcBldg3Direction.ReadOnly = true;
      this.dcBldg3Direction.Signed = false;
      this.dcBldg3Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg3Distance.DataPropertyName = "B3Distance";
      this.dcBldg3Distance.DecimalPlaces = 2;
      gridViewCellStyle24.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle24.Format = "N2";
      this.dcBldg3Distance.DefaultCellStyle = gridViewCellStyle24;
      this.dcBldg3Distance.Format = "#;#";
      this.dcBldg3Distance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcBldg3Distance, "dcBldg3Distance");
      this.dcBldg3Distance.Name = "dcBldg3Distance";
      this.dcBldg3Distance.ReadOnly = true;
      this.dcBldg3Distance.Signed = false;
      this.dcBldg4Direction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg4Direction.DataPropertyName = "B4Direction";
      this.dcBldg4Direction.DecimalPlaces = 0;
      gridViewCellStyle25.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle25.Format = "N0";
      this.dcBldg4Direction.DefaultCellStyle = gridViewCellStyle25;
      this.dcBldg4Direction.Format = "#;#";
      this.dcBldg4Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg4Direction, "dcBldg4Direction");
      this.dcBldg4Direction.Name = "dcBldg4Direction";
      this.dcBldg4Direction.ReadOnly = true;
      this.dcBldg4Direction.Signed = false;
      this.dcBldg4Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg4Distance.DataPropertyName = "B4Distance";
      this.dcBldg4Distance.DecimalPlaces = 2;
      gridViewCellStyle26.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle26.Format = "N2";
      this.dcBldg4Distance.DefaultCellStyle = gridViewCellStyle26;
      this.dcBldg4Distance.Format = "#;#";
      this.dcBldg4Distance.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg4Distance, "dcBldg4Distance");
      this.dcBldg4Distance.Name = "dcBldg4Distance";
      this.dcBldg4Distance.ReadOnly = true;
      this.dcBldg4Distance.Resizable = DataGridViewTriState.True;
      this.dcBldg4Distance.Signed = false;
      this.dcSiteType.DataPropertyName = "SiteType";
      this.dcSiteType.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcSiteType, "dcSiteType");
      this.dcSiteType.Name = "dcSiteType";
      this.dcSiteType.ReadOnly = true;
      this.dcSiteType.Resizable = DataGridViewTriState.True;
      this.dcSiteType.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcStTree.DataPropertyName = "StreetTree";
      componentResourceManager.ApplyResources((object) this.dcStTree, "dcStTree");
      this.dcStTree.Name = "dcStTree";
      this.dcStTree.ReadOnly = true;
      this.dcStTree.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcMaintRec.DataPropertyName = "MaintRec";
      this.dcMaintRec.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcMaintRec, "dcMaintRec");
      this.dcMaintRec.Name = "dcMaintRec";
      this.dcMaintRec.ReadOnly = true;
      this.dcMaintRec.Resizable = DataGridViewTriState.True;
      this.dcMaintRec.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcMaintTask.DataPropertyName = "MaintTask";
      this.dcMaintTask.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcMaintTask, "dcMaintTask");
      this.dcMaintTask.Name = "dcMaintTask";
      this.dcMaintTask.ReadOnly = true;
      this.dcMaintTask.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcSidewalk.DataPropertyName = "SidewalkDamage";
      this.dcSidewalk.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcSidewalk, "dcSidewalk");
      this.dcSidewalk.Name = "dcSidewalk";
      this.dcSidewalk.ReadOnly = true;
      this.dcSidewalk.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcWireConflict.DataPropertyName = "WireConflict";
      this.dcWireConflict.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcWireConflict, "dcWireConflict");
      this.dcWireConflict.Name = "dcWireConflict";
      this.dcWireConflict.ReadOnly = true;
      this.dcWireConflict.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcFieldOne.DataPropertyName = "OtherOne";
      this.dcFieldOne.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcFieldOne, "dcFieldOne");
      this.dcFieldOne.Name = "dcFieldOne";
      this.dcFieldOne.ReadOnly = true;
      this.dcFieldOne.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcFieldTwo.DataPropertyName = "OtherTwo";
      this.dcFieldTwo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcFieldTwo, "dcFieldTwo");
      this.dcFieldTwo.Name = "dcFieldTwo";
      this.dcFieldTwo.ReadOnly = true;
      this.dcFieldTwo.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcFieldThree.DataPropertyName = "OtherThree";
      this.dcFieldThree.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcFieldThree, "dcFieldThree");
      this.dcFieldThree.Name = "dcFieldThree";
      this.dcFieldThree.ReadOnly = true;
      this.dcFieldThree.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDTSDieback.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDTSDieback.DataPropertyName = "IPEDTSDieback";
      this.dcIPEDTSDieback.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDTSDieback, "dcIPEDTSDieback");
      this.dcIPEDTSDieback.Name = "dcIPEDTSDieback";
      this.dcIPEDTSDieback.ReadOnly = true;
      this.dcIPEDTSDieback.Resizable = DataGridViewTriState.True;
      this.dcIPEDTSDieback.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDTSEpiSprout.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDTSEpiSprout.DataPropertyName = "IPEDTSEpiSprout";
      this.dcIPEDTSEpiSprout.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDTSEpiSprout, "dcIPEDTSEpiSprout");
      this.dcIPEDTSEpiSprout.Name = "dcIPEDTSEpiSprout";
      this.dcIPEDTSEpiSprout.ReadOnly = true;
      this.dcIPEDTSEpiSprout.Resizable = DataGridViewTriState.True;
      this.dcIPEDTSEpiSprout.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDTSWiltFoli.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDTSWiltFoli.DataPropertyName = "IPEDTSWiltFoli";
      this.dcIPEDTSWiltFoli.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDTSWiltFoli, "dcIPEDTSWiltFoli");
      this.dcIPEDTSWiltFoli.Name = "dcIPEDTSWiltFoli";
      this.dcIPEDTSWiltFoli.ReadOnly = true;
      this.dcIPEDTSWiltFoli.Resizable = DataGridViewTriState.True;
      this.dcIPEDTSWiltFoli.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDTSEnvStress.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDTSEnvStress.DataPropertyName = "IPEDTSEnvStress";
      this.dcIPEDTSEnvStress.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDTSEnvStress, "dcIPEDTSEnvStress");
      this.dcIPEDTSEnvStress.Name = "dcIPEDTSEnvStress";
      this.dcIPEDTSEnvStress.ReadOnly = true;
      this.dcIPEDTSEnvStress.Resizable = DataGridViewTriState.True;
      this.dcIPEDTSEnvStress.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDTSHumStress.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDTSHumStress.DataPropertyName = "IPEDTSHumStress";
      this.dcIPEDTSHumStress.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDTSHumStress, "dcIPEDTSHumStress");
      this.dcIPEDTSHumStress.Name = "dcIPEDTSHumStress";
      this.dcIPEDTSHumStress.ReadOnly = true;
      this.dcIPEDTSHumStress.Resizable = DataGridViewTriState.True;
      this.dcIPEDTSHumStress.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDTSNotes.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDTSNotes.DataPropertyName = "IPEDTSNotes";
      componentResourceManager.ApplyResources((object) this.dcIPEDTSNotes, "dcIPEDTSNotes");
      this.dcIPEDTSNotes.MaxInputLength = (int) byte.MaxValue;
      this.dcIPEDTSNotes.Name = "dcIPEDTSNotes";
      this.dcIPEDTSNotes.ReadOnly = true;
      this.dcIPEDFTChewFoli.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDFTChewFoli.DataPropertyName = "IPEDFTChewFoli";
      this.dcIPEDFTChewFoli.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDFTChewFoli, "dcIPEDFTChewFoli");
      this.dcIPEDFTChewFoli.Name = "dcIPEDFTChewFoli";
      this.dcIPEDFTChewFoli.ReadOnly = true;
      this.dcIPEDFTChewFoli.Resizable = DataGridViewTriState.True;
      this.dcIPEDFTChewFoli.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDFTDiscFoli.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDFTDiscFoli.DataPropertyName = "IPEDFTDiscFoli";
      this.dcIPEDFTDiscFoli.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDFTDiscFoli, "dcIPEDFTDiscFoli");
      this.dcIPEDFTDiscFoli.Name = "dcIPEDFTDiscFoli";
      this.dcIPEDFTDiscFoli.ReadOnly = true;
      this.dcIPEDFTDiscFoli.Resizable = DataGridViewTriState.True;
      this.dcIPEDFTDiscFoli.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDFTAbnFoli.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDFTAbnFoli.DataPropertyName = "IPEDFTAbnFoli";
      this.dcIPEDFTAbnFoli.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDFTAbnFoli, "dcIPEDFTAbnFoli");
      this.dcIPEDFTAbnFoli.Name = "dcIPEDFTAbnFoli";
      this.dcIPEDFTAbnFoli.ReadOnly = true;
      this.dcIPEDFTAbnFoli.Resizable = DataGridViewTriState.True;
      this.dcIPEDFTAbnFoli.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDFTInsectSigns.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDFTInsectSigns.DataPropertyName = "IPEDFTInsectSigns";
      this.dcIPEDFTInsectSigns.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDFTInsectSigns, "dcIPEDFTInsectSigns");
      this.dcIPEDFTInsectSigns.Name = "dcIPEDFTInsectSigns";
      this.dcIPEDFTInsectSigns.ReadOnly = true;
      this.dcIPEDFTInsectSigns.Resizable = DataGridViewTriState.True;
      this.dcIPEDFTInsectSigns.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDFTFoliAffect.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDFTFoliAffect.DataPropertyName = "IPEDFTFoliAffect";
      this.dcIPEDFTFoliAffect.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDFTFoliAffect, "dcIPEDFTFoliAffect");
      this.dcIPEDFTFoliAffect.Name = "dcIPEDFTFoliAffect";
      this.dcIPEDFTFoliAffect.ReadOnly = true;
      this.dcIPEDFTFoliAffect.Resizable = DataGridViewTriState.True;
      this.dcIPEDFTFoliAffect.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDFTNotes.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDFTNotes.DataPropertyName = "IPEDFTNotes";
      componentResourceManager.ApplyResources((object) this.dcIPEDFTNotes, "dcIPEDFTNotes");
      this.dcIPEDFTNotes.MaxInputLength = (int) byte.MaxValue;
      this.dcIPEDFTNotes.Name = "dcIPEDFTNotes";
      this.dcIPEDFTNotes.ReadOnly = true;
      this.dcIPEDBBInsectSigns.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDBBInsectSigns.DataPropertyName = "IPEDBBInsectSigns";
      this.dcIPEDBBInsectSigns.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDBBInsectSigns, "dcIPEDBBInsectSigns");
      this.dcIPEDBBInsectSigns.Name = "dcIPEDBBInsectSigns";
      this.dcIPEDBBInsectSigns.ReadOnly = true;
      this.dcIPEDBBInsectSigns.Resizable = DataGridViewTriState.True;
      this.dcIPEDBBInsectSigns.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDBBInsectPres.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDBBInsectPres.DataPropertyName = "IPEDBBInsectPres";
      this.dcIPEDBBInsectPres.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDBBInsectPres, "dcIPEDBBInsectPres");
      this.dcIPEDBBInsectPres.Name = "dcIPEDBBInsectPres";
      this.dcIPEDBBInsectPres.ReadOnly = true;
      this.dcIPEDBBInsectPres.Resizable = DataGridViewTriState.True;
      this.dcIPEDBBInsectPres.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDBBDiseaseSigns.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDBBDiseaseSigns.DataPropertyName = "IPEDBBDiseaseSigns";
      this.dcIPEDBBDiseaseSigns.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDBBDiseaseSigns, "dcIPEDBBDiseaseSigns");
      this.dcIPEDBBDiseaseSigns.Name = "dcIPEDBBDiseaseSigns";
      this.dcIPEDBBDiseaseSigns.ReadOnly = true;
      this.dcIPEDBBDiseaseSigns.Resizable = DataGridViewTriState.True;
      this.dcIPEDBBDiseaseSigns.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDBBProbLoc.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDBBProbLoc.DataPropertyName = "IPEDBBProbLoc";
      this.dcIPEDBBProbLoc.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDBBProbLoc, "dcIPEDBBProbLoc");
      this.dcIPEDBBProbLoc.Name = "dcIPEDBBProbLoc";
      this.dcIPEDBBProbLoc.ReadOnly = true;
      this.dcIPEDBBProbLoc.Resizable = DataGridViewTriState.True;
      this.dcIPEDBBProbLoc.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDBBAbnGrowth.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDBBAbnGrowth.DataPropertyName = "IPEDBBAbnGrowth";
      this.dcIPEDBBAbnGrowth.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDBBAbnGrowth, "dcIPEDBBAbnGrowth");
      this.dcIPEDBBAbnGrowth.Name = "dcIPEDBBAbnGrowth";
      this.dcIPEDBBAbnGrowth.ReadOnly = true;
      this.dcIPEDBBAbnGrowth.Resizable = DataGridViewTriState.True;
      this.dcIPEDBBAbnGrowth.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcIPEDBBNotes.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDBBNotes.DataPropertyName = "IPEDBBNotes";
      componentResourceManager.ApplyResources((object) this.dcIPEDBBNotes, "dcIPEDBBNotes");
      this.dcIPEDBBNotes.MaxInputLength = (int) byte.MaxValue;
      this.dcIPEDBBNotes.Name = "dcIPEDBBNotes";
      this.dcIPEDBBNotes.ReadOnly = true;
      this.dcIPEDBBNotes.Resizable = DataGridViewTriState.True;
      this.dcIPEDPest.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcIPEDPest.DataPropertyName = "IPEDPest";
      this.dcIPEDPest.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcIPEDPest, "dcIPEDPest");
      this.dcIPEDPest.Name = "dcIPEDPest";
      this.dcIPEDPest.ReadOnly = true;
      this.dcIPEDPest.Resizable = DataGridViewTriState.True;
      this.dcIPEDPest.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcCityManaged.DataPropertyName = "CityManaged";
      componentResourceManager.ApplyResources((object) this.dcCityManaged, "dcCityManaged");
      this.dcCityManaged.Name = "dcCityManaged";
      this.dcCityManaged.ReadOnly = true;
      this.dcCityManaged.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcCityManaged.ThreeState = true;
      this.dcLocSite.DataPropertyName = "LocSite";
      componentResourceManager.ApplyResources((object) this.dcLocSite, "dcLocSite");
      this.dcLocSite.Name = "dcLocSite";
      this.dcLocSite.ReadOnly = true;
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
      this.dcLocNo.ReadOnly = true;
      this.dcLocNo.Resizable = DataGridViewTriState.True;
      this.dcLocNo.Signed = false;
      this.dcLatitude.DataPropertyName = "Latitude";
      componentResourceManager.ApplyResources((object) this.dcLatitude, "dcLatitude");
      this.dcLatitude.Name = "dcLatitude";
      this.dcLatitude.ReadOnly = true;
      this.dcLongitude.DataPropertyName = "Longitude";
      componentResourceManager.ApplyResources((object) this.dcLongitude, "dcLongitude");
      this.dcLongitude.Name = "dcLongitude";
      this.dcLongitude.ReadOnly = true;
      this.dcNoteTree.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcNoteTree.DataPropertyName = "NoteThisTree";
      componentResourceManager.ApplyResources((object) this.dcNoteTree, "dcNoteTree");
      this.dcNoteTree.Name = "dcNoteTree";
      this.dcNoteTree.ReadOnly = true;
      this.dcNoteTree.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcComments.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcComments.DataPropertyName = "Comments";
      componentResourceManager.ApplyResources((object) this.dcComments, "dcComments");
      this.dcComments.MaxInputLength = (int) byte.MaxValue;
      this.dcComments.Name = "dcComments";
      this.dcComments.ReadOnly = true;
      this.dcComments.Resizable = DataGridViewTriState.True;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.dgPlots);
      this.DockAreas = DockAreas.Document;
      this.Name = nameof (InventoryTreesForm);
      this.ShowHint = DockState.Document;
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgPlots, 0);
      ((ISupportInitialize) this.dgPlots).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
