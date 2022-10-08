// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.PlotTreesForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using DaveyTree.Controls.Extensions;
using DaveyTree.Core;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Views;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Events;
using i_Tree_Eco_v6.Interfaces;
using IPED.Domain;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class PlotTreesForm : ContentForm, IPlotContent, IActionable, IExportable
  {
    private ProgramSession m_ps;
    private ISession m_session;
    private Plot m_boundPlot;
    private Plot m_selectedPlot;
    private Year m_year;
    private DataGridViewManager m_dgManager;
    private TaskManager m_taskManager;
    private WaitCursor m_wc;
    private readonly object m_syncobj;
    private int m_dgHorizPos;
    private bool m_refresh;
    private IPEDData m_iped;
    private IList<Street> m_streets;
    private BindingList<SpeciesView> m_species;
    private IDictionary<double, string> m_dbhs;
    private DataBindingList<FlatTreeView> m_trees;
    private IList<PlotLandUse> m_plotLandUses;
    private IContainer components;
    private DataGridView dgTrees;
    private DataGridViewNumericTextBoxColumn dcId;
    private DataGridViewTextBoxColumn dcUserId;
    private DataGridViewNullableDateTimeColumn dcSurveyDate;
    private DataGridViewComboBoxColumn dcStatus;
    private DataGridViewNumericTextBoxColumn dcDistance;
    private DataGridViewNumericTextBoxColumn dcDirection;
    private DataGridViewFilteredComboBoxColumn dcSpecies;
    private DataGridViewComboBoxColumn dcLandUse;
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
    private DataGridViewComboBoxColumn dcPctImpervious;
    private DataGridViewComboBoxColumn dcPctShrub;
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
    private DataGridViewComboBoxColumn dcStreet;
    private DataGridViewTextBoxColumn dcAddress;
    private DataGridViewComboBoxColumn dcLocSite;
    private DataGridViewNumericTextBoxColumn dcLocNo;
    private DataGridViewTextBoxColumn dcLatitude;
    private DataGridViewTextBoxColumn dcLongitude;
    private DataGridViewCheckBoxColumn dcNoteThisTree;
    private DataGridViewTextBoxColumn dcComments;

    public PlotTreesForm()
    {
      this.InitializeComponent();
      this.dcId.DefaultCellStyle.DataSourceNullValue = (object) 0;
      this.dcDirection.DefaultCellStyle.DataSourceNullValue = (object) -1;
      this.dcDistance.DefaultCellStyle.DataSourceNullValue = (object) -1f;
      this.dcCrownPercentMissing.DefaultCellStyle.DataSourceNullValue = (object) PctMidRange.PRINV;
      this.dcPctImpervious.DefaultCellStyle.DataSourceNullValue = (object) PctMidRange.PRINV;
      this.dcPctShrub.DefaultCellStyle.DataSourceNullValue = (object) PctMidRange.PRINV;
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
      this.m_ps = Program.Session;
      this.m_dgManager = new DataGridViewManager(this.dgTrees);
      this.m_wc = new WaitCursor((Form) this);
      this.m_taskManager = new TaskManager(this.m_wc);
      this.m_syncobj = new object();
      this.dgTrees.DoubleBuffered(true);
      this.dgTrees.AutoGenerateColumns = false;
      this.dgTrees.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgTrees.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dgTrees.CellEndEdit += new DataGridViewCellEventHandler(this.dgTrees_CellEndEdit);
      EventPublisher.Register<EntityUpdated<Tree>>(new EventHandler<EntityUpdated<Tree>>(this.Tree_Updated));
      EventPublisher.Register<EntityUpdated<Year>>(new EventHandler<EntityUpdated<Year>>(this.Year_Updated));
      EventPublisher.Register<EntityCreated<PlotLandUse>>(new EventHandler<EntityCreated<PlotLandUse>>(this.PlotLandUse_Created));
      EventPublisher.Register<EntityUpdated<PlotLandUse>>(new EventHandler<EntityUpdated<PlotLandUse>>(this.PlotLandUse_Updated));
      EventPublisher.Register<EntityDeleted<PlotLandUse>>(new EventHandler<EntityDeleted<PlotLandUse>>(this.PlotLandUse_Deleted));
      this.Init();
    }

    private void LoadData()
    {
      ISession session = this.m_ps.InputSession.CreateSession();
      using (session.BeginTransaction())
      {
        Year year = session.Get<Year>((object) this.m_ps.InputSession.YearKey);
        NHibernateUtil.Initialize((object) year.Series);
        if (!year.DBHActual)
          NHibernateUtil.Initialize((object) year.DBHs);
        if (year.RecordCrownCondition)
          NHibernateUtil.Initialize((object) year.Conditions);
        if (year.RecordSiteType)
          NHibernateUtil.Initialize((object) year.SiteTypes);
        if (year.RecordMaintRec)
          NHibernateUtil.Initialize((object) year.MaintRecs);
        if (year.RecordMaintTask)
          NHibernateUtil.Initialize((object) year.MaintTasks);
        if (year.RecordSidewalk)
          NHibernateUtil.Initialize((object) year.SidewalkDamages);
        if (year.RecordWireConflict)
          NHibernateUtil.Initialize((object) year.WireConflicts);
        if (year.RecordOtherOne)
          NHibernateUtil.Initialize((object) year.OtherOnes);
        if (year.RecordOtherTwo)
          NHibernateUtil.Initialize((object) year.OtherTwos);
        if (year.RecordOtherThree)
          NHibernateUtil.Initialize((object) year.OtherThree);
        if (year.RecordIPED)
          this.m_iped = this.m_ps.IPEDData;
        if (year.RecordTreeStreet)
          this.m_streets = session.CreateCriteria<Street>().CreateAlias("ProjectLocation", "pl").CreateAlias("pl.Project", "p").CreateAlias("p.Series", "s").CreateAlias("s.Years", "y").Add((ICriterion) Restrictions.Eq("y.Guid", (object) this.m_year.Guid)).AddOrder(Order.Asc(TypeHelper.NameOf<Street>((System.Linq.Expressions.Expression<Func<Street, object>>) (st => st.Name)))).List<Street>();
        if (year.RecordLocSite)
          NHibernateUtil.Initialize((object) year.LocSites);
        lock (this.m_syncobj)
        {
          this.m_dbhs = (IDictionary<double, string>) year.DBHs.ToDictionary<DBH, double, string>((Func<DBH, double>) (d => Convert.ToDouble(d.Id)), (Func<DBH, string>) (d => d.Description));
          this.m_session = session;
          this.m_year = year;
        }
      }
      List<SpeciesView> list = this.m_ps.Species.Values.ToList<SpeciesView>();
      list.Sort((IComparer<SpeciesView>) new PropertyComparer<SpeciesView>((Func<SpeciesView, object>) (sp => (object) sp.CommonName)));
      this.m_species = (BindingList<SpeciesView>) new ExtendedBindingList<SpeciesView>((IList<SpeciesView>) list);
    }

    private void InitGrid()
    {
      Year year = this.m_year;
      if (year == null)
        return;
      BindingSource dataSource1 = new BindingSource((object) EnumHelper.ConvertToDictionary<PctMidRange>(), (string) null);
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
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcUserId);
      this.dcSpecies.BindTo<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.CommonScientificName), (System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.Code), (object) this.m_species);
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
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcStatus);
      if (year.RecordPlotCenter)
      {
        this.dcDistance.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) v6Strings.Tree_Distance, (object) str2);
      }
      else
      {
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDistance);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDirection);
      }
      if (!year.RecordLanduse)
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcLandUse);
      if (year.DBHActual)
      {
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDBH1c);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDBH2c);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDBH3c);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDBH4c);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDBH5c);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDBH6c);
        this.dcDBH1.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 1), (object) str1);
        this.dcDBH2.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 2), (object) str1);
        this.dcDBH3.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 3), (object) str1);
        this.dcDBH4.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 4), (object) str1);
        this.dcDBH5.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 5), (object) str1);
        this.dcDBH6.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 6), (object) str1);
      }
      else
      {
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDBH1);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDBH2);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDBH3);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDBH4);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDBH5);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcDBH6);
        this.dcDBH1c.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 1), (object) str1);
        this.dcDBH2c.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 2), (object) str1);
        this.dcDBH3c.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 3), (object) str1);
        this.dcDBH4c.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 4), (object) str1);
        this.dcDBH5c.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 5), (object) str1);
        this.dcDBH6c.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 6), (object) str1);
        BindingSource dataSource2 = new BindingSource((object) this.m_dbhs, (string) null);
        this.dcDBH1c.BindTo("Value", "Key", (object) dataSource2);
        this.dcDBH2c.BindTo("Value", "Key", (object) dataSource2);
        this.dcDBH3c.BindTo("Value", "Key", (object) dataSource2);
        this.dcDBH4c.BindTo("Value", "Key", (object) dataSource2);
        this.dcDBH5c.BindTo("Value", "Key", (object) dataSource2);
        this.dcDBH6c.BindTo("Value", "Key", (object) dataSource2);
      }
      this.dcDBH1Height.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 1), (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      this.dcDBH2Height.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 2), (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      this.dcDBH3Height.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 3), (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      this.dcDBH4Height.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 4), (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      this.dcDBH5Height.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 5), (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      this.dcDBH6Height.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) i_Tree_Eco_v6.Resources.Strings.DBH, (object) 6), (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      if (year.RecordCrownCondition)
      {
        if (year.DisplayCondition)
        {
          this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcCrownDieback);
          this.dcCrownCondition.BindTo<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Description), (System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Self), (object) year.Conditions.ToList<Condition>());
        }
        else
        {
          this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcCrownCondition);
          this.dcCrownDieback.BindTo<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.DiebackDesc), (System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Self), (object) year.Conditions.ToList<Condition>());
        }
      }
      else
      {
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcCrownCondition);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcCrownDieback);
      }
      if (year.RecordHeight)
        this.dcTreeHeight.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtTotal, (object) i_Tree_Eco_v6.Resources.Strings.Height), (object) str2);
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcTreeHeight);
      if (year.RecordCrownSize)
      {
        this.dcCrownPercentMissing.BindTo("Value", "Key", (object) dataSource1);
        this.dcCrownTopHeight.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) v6Strings.Crown_SingularName, (object) v6Strings.Crown_TopHeight), (object) str2);
        this.dcCrownBaseHeight.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) v6Strings.Crown_SingularName, (object) v6Strings.Crown_BaseHeight), (object) str2);
        this.dcCrownWidthNS.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) v6Strings.Crown_SingularName, (object) v6Strings.Crown_WidthNS), (object) str2);
        this.dcCrownWidthEW.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) v6Strings.Crown_SingularName, (object) v6Strings.Crown_WidthEW), (object) str2);
      }
      else
      {
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcCrownPercentMissing);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcCrownWidthEW);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcCrownWidthNS);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcCrownBaseHeight);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcCrownTopHeight);
      }
      if (year.RecordHydro)
      {
        this.dcPctImpervious.BindTo("Value", "Key", (object) dataSource1);
        this.dcPctShrub.BindTo("Value", "Key", (object) dataSource1);
      }
      else
      {
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcPctImpervious);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcPctShrub);
      }
      if (year.RecordCLE)
        this.dcCrownLightExposure.BindTo("Value", "Key", (object) new BindingSource((object) EnumHelper.ConvertToDictionary<CrownLightExposure>((CrownLightExposure[]) null, (IComparer<CrownLightExposure>) new AttributePropertyComparer<CrownLightExposure, DescriptionAttribute>("Description")), (string) null));
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcCrownLightExposure);
      if (year.RecordEnergy)
      {
        this.dcBldg1Distance.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.Building_SingularName, (object) 1), (object) v6Strings.Building_Distance), (object) str2);
        this.dcBldg2Distance.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.Building_SingularName, (object) 2), (object) v6Strings.Building_Distance), (object) str2);
        this.dcBldg3Distance.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.Building_SingularName, (object) 3), (object) v6Strings.Building_Distance), (object) str2);
        this.dcBldg4Distance.HeaderText = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldUnit, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtSubfield, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtConcat, (object) v6Strings.Building_SingularName, (object) 4), (object) v6Strings.Building_Distance), (object) str2);
      }
      else
      {
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcBldg1Direction);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcBldg1Distance);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcBldg2Direction);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcBldg2Distance);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcBldg3Direction);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcBldg3Distance);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcBldg4Direction);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcBldg4Distance);
      }
      if (year.RecordSiteType)
        this.dcSiteType.BindTo<SiteType>((System.Linq.Expressions.Expression<Func<SiteType, object>>) (st => st.Description), (System.Linq.Expressions.Expression<Func<SiteType, object>>) (st => st.Self), (object) year.SiteTypes.ToList<SiteType>());
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcSiteType);
      if (!year.RecordStreetTree)
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcStTree);
      if (year.RecordMaintRec)
        this.BindLookupColumn<MaintRec>(this.dcMaintRec, (IList<MaintRec>) year.MaintRecs.ToList<MaintRec>());
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcMaintRec);
      if (year.RecordMaintTask)
        this.BindLookupColumn<MaintTask>(this.dcMaintTask, (IList<MaintTask>) year.MaintTasks.ToList<MaintTask>());
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcMaintTask);
      if (year.RecordSidewalk)
        this.BindLookupColumn<Sidewalk>(this.dcSidewalk, (IList<Sidewalk>) year.SidewalkDamages.ToList<Sidewalk>());
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcSidewalk);
      if (year.RecordWireConflict)
        this.BindLookupColumn<WireConflict>(this.dcWireConflict, (IList<WireConflict>) year.WireConflicts.ToList<WireConflict>());
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcWireConflict);
      if (year.RecordOtherOne)
      {
        if (!string.IsNullOrEmpty(year.OtherOne))
          this.dcFieldOne.HeaderText = year.OtherOne;
        this.BindLookupColumn<OtherOne>(this.dcFieldOne, (IList<OtherOne>) year.OtherOnes.ToList<OtherOne>());
      }
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcFieldOne);
      if (year.RecordOtherTwo)
      {
        if (!string.IsNullOrEmpty(year.OtherTwo))
          this.dcFieldTwo.HeaderText = year.OtherTwo;
        this.BindLookupColumn<OtherTwo>(this.dcFieldTwo, (IList<OtherTwo>) year.OtherTwos.ToList<OtherTwo>());
      }
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcFieldTwo);
      if (year.RecordOtherThree)
      {
        if (!string.IsNullOrEmpty(year.OtherThree))
          this.dcFieldThree.HeaderText = year.OtherThree;
        this.BindLookupColumn<OtherThree>(this.dcFieldThree, (IList<OtherThree>) year.OtherThrees.ToList<OtherThree>());
      }
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcFieldThree);
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
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDBBAbnGrowth);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDBBDiseaseSigns);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDBBInsectPres);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDBBInsectSigns);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDBBNotes);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDBBProbLoc);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDFTAbnFoli);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDFTChewFoli);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDFTDiscFoli);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDFTFoliAffect);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDFTInsectSigns);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDFTNotes);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDTSDieback);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDTSEnvStress);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDTSEpiSprout);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDTSHumStress);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDTSNotes);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDTSWiltFoli);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcIPEDPest);
      }
      if (!year.MgmtStyle.HasValue || (year.MgmtStyle.Value & MgmtStyleEnum.RecordPublicPrivate) == MgmtStyleEnum.DefaultPublic)
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcCityManaged);
      if (year.RecordTreeStreet)
        this.dcStreet.BindTo<Street>((System.Linq.Expressions.Expression<Func<Street, object>>) (st => st.Name), (System.Linq.Expressions.Expression<Func<Street, object>>) (st => st.Self), (object) this.m_streets);
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcStreet);
      if (!year.RecordTreeAddress)
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcAddress);
      if (year.RecordLocSite)
        this.BindLookupColumn<LocSite>(this.dcLocSite, (IList<LocSite>) year.LocSites.ToList<LocSite>());
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcLocSite);
      if (!year.RecordLocNo)
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcLocNo);
      if (!year.RecordTreeGPS)
      {
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcLatitude);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcLongitude);
      }
      if (year.RecordNoteTree)
        return;
      this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcNoteThisTree);
    }

    private void BindLookupColumn<T>(DataGridViewComboBoxColumn cbcLookup, IList<T> dataSource) where T : Eco.Domain.v6.Lookup => cbcLookup.BindTo<T>((System.Linq.Expressions.Expression<Func<T, object>>) (lu => lu.Description), (System.Linq.Expressions.Expression<Func<T, object>>) (lu => lu.Self), (object) dataSource);

    private void BindIPEDLookupColumn<T>(DataGridViewComboBoxColumn cbcLookup, IList<T> dataSource) where T : IPED.Domain.Lookup => cbcLookup.BindTo<T>((System.Linq.Expressions.Expression<Func<T, object>>) (lu => lu.Description), (System.Linq.Expressions.Expression<Func<T, object>>) (lu => (object) lu.Code), (object) dataSource);

    private void PlotLandUse_Deleted(object sender, EntityDeleted<PlotLandUse> e)
    {
      BindingSource dataSource1 = this.dcLandUse.DataSource as BindingSource;
      if (this.m_session == null)
        return;
      using (ITransaction transaction = this.m_session.BeginTransaction())
      {
        if (dataSource1 != null)
        {
          if (dataSource1.DataSource is IDictionary<PlotLandUse, string> dataSource2)
          {
            PlotLandUse key1 = (PlotLandUse) null;
            foreach (PlotLandUse key2 in (IEnumerable<PlotLandUse>) dataSource2.Keys)
            {
              if (key2.Guid == e.Guid)
              {
                key1 = key2;
                break;
              }
            }
            if (key1 != null)
              dataSource2.Remove(key1);
          }
          this.dcLandUse.DataSource = dataSource2.Count <= 0 ? (object) null : (object) new BindingSource((object) dataSource2, (string) null);
        }
        transaction.Commit();
      }
    }

    private void PlotLandUse_Updated(object sender, EntityUpdated<PlotLandUse> e)
    {
      BindingSource dataSource1 = this.dcLandUse.DataSource as BindingSource;
      if (this.m_session == null || this.m_plotLandUses == null || dataSource1 == null)
        return;
      IDictionary<PlotLandUse, string> dataSource2 = dataSource1.DataSource as IDictionary<PlotLandUse, string>;
      foreach (PlotLandUse plotLandUse in (IEnumerable<PlotLandUse>) this.m_plotLandUses)
      {
        if (plotLandUse.Guid == e.Guid)
        {
          lock (this.m_syncobj)
          {
            using (this.m_session.BeginTransaction())
            {
              this.m_session.Refresh((object) plotLandUse);
              this.m_session.Refresh((object) plotLandUse.LandUse);
            }
          }
          dataSource2[plotLandUse] = plotLandUse.LandUse.Description;
          break;
        }
      }
    }

    private void PlotLandUse_Created(object sender, EntityCreated<PlotLandUse> e)
    {
      BindingSource dataSource1 = this.dcLandUse.DataSource as BindingSource;
      if (this.m_session == null || this.m_plotLandUses == null || dataSource1 == null)
        return;
      IDictionary<PlotLandUse, string> dataSource2 = dataSource1.DataSource as IDictionary<PlotLandUse, string>;
      lock (this.m_syncobj)
      {
        using (this.m_session.BeginTransaction())
        {
          PlotLandUse key = this.m_session.QueryOver<PlotLandUse>().Where((System.Linq.Expressions.Expression<Func<PlotLandUse, bool>>) (p => p.Plot == this.m_boundPlot)).Where((System.Linq.Expressions.Expression<Func<PlotLandUse, bool>>) (p => p.Guid == e.Guid)).Fetch<PlotLandUse, PlotLandUse>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<PlotLandUse, object>>) (p => p.LandUse)).SingleOrDefault();
          if (key == null)
            return;
          this.m_plotLandUses.Add(key);
          dataSource2[key] = key.LandUse.Description;
        }
      }
    }

    private void Tree_Updated(object sender, EntityUpdated<Tree> e)
    {
      if (this.m_session == null)
        return;
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() =>
      {
        lock (this.m_syncobj)
        {
          using (ITransaction transaction = this.m_session.BeginTransaction())
          {
            Tree proxy = this.m_session.Load<Tree>((object) e.Guid);
            if (NHibernateUtil.IsInitialized((object) proxy))
              this.m_session.Refresh((object) proxy, LockMode.None);
            transaction.Commit();
          }
        }
      }), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler));
    }

    private void Year_Updated(object sender, EntityUpdated<Year> e)
    {
      if (this.m_year == null || !(this.m_year.Guid == e.Guid))
        return;
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.GetYear(e.Guid)), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.OnRequestRefresh();
      }), scheduler));
    }

    private void GetYear(Guid g)
    {
      lock (this.m_syncobj)
      {
        if (this.m_session == null || this.m_year == null || !(this.m_year.Guid == g))
          return;
        this.m_session.Refresh((object) this.m_year);
      }
    }

    private void Init()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.LoadData()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.InitGrid();
      }), scheduler));
    }

    private void LoadAndBindPlot()
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.m_taskManager.Add(Task.Factory.StartNew((System.Action) (() => this.LoadSelectedPlot()), CancellationToken.None, TaskCreationOptions.None, this.m_ps.Scheduler).ContinueWith((System.Action<Task>) (t =>
      {
        if (this.IsDisposed)
          return;
        this.InitBoundPlot();
      }), scheduler));
    }

    private void LoadSelectedPlot()
    {
      lock (this.m_syncobj)
      {
        Plot p = this.m_selectedPlot;
        if (p == null || p.PercentTreeCover <= PctMidRange.PR0)
        {
          this.m_boundPlot = p;
          this.m_trees = (DataBindingList<FlatTreeView>) null;
          this.m_plotLandUses = (IList<PlotLandUse>) null;
        }
        else
        {
          if (this.m_session == null || this.m_year == null)
            return;
          using (this.m_session.BeginTransaction())
          {
            if (this.m_year.RecordLanduse)
              this.m_plotLandUses = this.m_session.QueryOver<PlotLandUse>().Where((System.Linq.Expressions.Expression<Func<PlotLandUse, bool>>) (plu => plu.Plot == p)).Fetch<PlotLandUse, PlotLandUse>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<PlotLandUse, object>>) (plu => plu.LandUse)).List();
            else
              this.m_plotLandUses = (IList<PlotLandUse>) null;
            IList<Tree> treeList = this.m_session.QueryOver<Tree>().Where((System.Linq.Expressions.Expression<Func<Tree, bool>>) (tr => tr.Plot == p)).OrderBy((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => (object) tr.Id)).Asc.List();
            this.m_trees = new DataBindingList<FlatTreeView>();
            foreach (Tree tree in (IEnumerable<Tree>) treeList)
            {
              NHibernateUtil.Initialize((object) tree.Stems);
              NHibernateUtil.Initialize((object) tree.Buildings);
              this.m_trees.Add(new FlatTreeView(this.m_session, tree));
            }
            this.m_boundPlot = p;
          }
        }
      }
    }

    private void InitBoundPlot()
    {
      lock (this.m_syncobj)
      {
        if (this.m_selectedPlot != this.m_boundPlot)
          return;
        if (this.m_trees != null)
        {
          if (this.dgTrees.DataSource != this.m_trees)
          {
            this.m_trees.Sortable = true;
            this.m_trees.AddComparer<TreeStatus>((IComparer) new AttributePropertyComparer<TreeStatus, DescriptionAttribute>("Description"));
            this.m_trees.AddComparer<CrownLightExposure>((IComparer) new AttributePropertyComparer<CrownLightExposure, DescriptionAttribute>("Description"));
            this.m_trees.AddComparer<PlotLandUse>((IComparer) new ChildComparer<PlotLandUse, LandUse>((Func<PlotLandUse, LandUse>) (plu => plu.LandUse), (IComparer<LandUse>) new PropertyComparer<LandUse>((Func<LandUse, object>) (lu => (object) lu.Description))));
            this.m_trees.AddComparer<Condition>((IComparer) new PropertyComparer<Condition>((Func<Condition, object>) (c => (object) c.PctDieback)));
            this.m_trees.AddComparer<Street>((IComparer) new PropertyComparer<Street>((Func<Street, object>) (s => (object) s.Name)));
            this.m_trees.AddComparer<Eco.Domain.v6.Lookup>((IComparer) new PropertyComparer<Eco.Domain.v6.Lookup>((Func<Eco.Domain.v6.Lookup, object>) (lu => (object) lu.Description)));
            if (this.m_plotLandUses != null)
            {
              IDictionary<PlotLandUse, string> dataSource = (IDictionary<PlotLandUse, string>) new Dictionary<PlotLandUse, string>();
              foreach (PlotLandUse plotLandUse in (IEnumerable<PlotLandUse>) this.m_plotLandUses)
                dataSource.Add(plotLandUse, plotLandUse.LandUse.Description);
              this.dgTrees.DataSource = (object) null;
              if (dataSource.Count > 0)
                this.dcLandUse.BindTo("Value", "Key", (object) new BindingSource((object) dataSource, (string) null));
              else
                this.dcLandUse.DataSource = (object) null;
            }
            this.dgTrees.DataSource = (object) this.m_trees;
            this.dgTrees.ResizeColumns(DataGridViewAutoSizeColumnMode.AllCells);
            this.m_trees.AddingNew += new AddingNewEventHandler(this.Trees_AddingNew);
            this.m_trees.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.Trees_BeforeRemove);
            this.m_trees.ListChanged += new ListChangedEventHandler(this.Trees_ListChanged);
          }
        }
        else
          this.dgTrees.DataSource = (object) null;
        this.m_refresh = false;
        this.OnRequestRefresh();
      }
    }

    public void OnPlotSelectionChanged(object sender, PlotEventArgs e)
    {
      lock (this.m_syncobj)
      {
        this.m_selectedPlot = e.Plot;
        if (this.Visible)
          this.LoadAndBindPlot();
        else
          this.m_refresh = true;
      }
    }

    private void Trees_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemAdded)
        return;
      this.OnRequestRefresh();
    }

    private void Trees_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      if (!(sender is DataBindingList<FlatTreeView> dataBindingList) || e.NewIndex >= dataBindingList.Count)
        return;
      FlatTreeView flatTreeView = dataBindingList[e.NewIndex];
      if (flatTreeView.Tree.IsTransient)
        return;
      lock (this.m_syncobj)
      {
        using (ITransaction transaction = this.m_session.BeginTransaction())
        {
          this.m_session.Delete((object) flatTreeView.Tree);
          transaction.Commit();
        }
      }
    }

    private void Trees_AddingNew(object sender, AddingNewEventArgs e)
    {
      DataBindingList<FlatTreeView> trees = sender as DataBindingList<FlatTreeView>;
      ISession session;
      Year year;
      lock (this.m_syncobj)
      {
        session = this.m_session;
        year = this.m_year;
      }
      int num = this.NextTreeId((IList<FlatTreeView>) trees);
      MgmtStyleEnum? mgmtStyle = year.MgmtStyle;
      short? nullable = mgmtStyle.HasValue ? new short?((short) mgmtStyle.GetValueOrDefault()) : new short?();
      if (nullable.HasValue)
        nullable = new short?((short) ((int) nullable.Value & 1));
      Condition condition1 = (Condition) null;
      if (this.dcCrownCondition.DataSource is BindingList<Condition> dataSource)
      {
        foreach (Condition condition2 in (Collection<Condition>) dataSource)
        {
          if (condition2.PctDieback + 1.0 <= double.Epsilon)
          {
            condition1 = condition2;
            break;
          }
        }
      }
      FlatTreeView flatTreeView = new FlatTreeView(session, new Tree()
      {
        Id = num,
        Plot = this.m_boundPlot,
        SurveyDate = this.m_boundPlot.Date,
        CityManaged = nullable,
        StreetTree = this.m_year.RecordStreetTree && this.m_year.DefaultStreetTree,
        Crown = new Crown() { Condition = condition1 }
      });
      e.NewObject = (object) flatTreeView;
    }

    private int NextTreeId(IList<FlatTreeView> trees)
    {
      int num = 1;
      foreach (FlatTreeView tree in (IEnumerable<FlatTreeView>) trees)
      {
        if (tree.Tree.Id >= num)
          num = tree.Tree.Id + 1;
      }
      return num;
    }

    private void dgTrees_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<FlatTreeView> dataSource = this.dgTrees.DataSource as DataBindingList<FlatTreeView>;
      if (this.dgTrees.CurrentRow != null && !this.dgTrees.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      FlatTreeView flatTreeView1 = dataSource[e.RowIndex];
      DataGridViewRow row = this.dgTrees.Rows[e.RowIndex];
      DataGridViewCell dataGridViewCell1 = (DataGridViewCell) null;
      string text = (string) null;
      foreach (FlatTreeView flatTreeView2 in (Collection<FlatTreeView>) dataSource)
      {
        if (flatTreeView2 != flatTreeView1 && flatTreeView2.Id == flatTreeView1.Id)
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
      if (text == null)
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show((IWin32Window) this, text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void dgTrees_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dgTrees.ReadOnly || this.m_boundPlot == null)
        return;
      DataBindingList<FlatTreeView> dataSource = this.dgTrees.DataSource as DataBindingList<FlatTreeView>;
      if (this.dgTrees.CurrentRow != null && this.dgTrees.CurrentRow.IsNewRow)
      {
        this.DeleteSelectedRows();
      }
      else
      {
        if (dataSource != null && e.RowIndex < dataSource.Count)
        {
          FlatTreeView flatTreeView = dataSource[e.RowIndex];
          if (flatTreeView.Tree.IsTransient || this.m_session.IsDirty())
          {
            lock (this.m_syncobj)
            {
              using (ITransaction transaction = this.m_session.BeginTransaction())
              {
                this.m_session.SaveOrUpdate((object) flatTreeView.Tree);
                transaction.Commit();
              }
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgTrees_CellEndEdit(object sender, DataGridViewCellEventArgs e) => this.OnRequestRefresh();

    private void dgTrees_SelectionChanged(object sender, EventArgs e)
    {
      if (this.m_boundPlot == null)
        return;
      this.OnRequestRefresh();
    }

    private void DeleteSelectedRows()
    {
      if (!(this.dgTrees.DataSource is DataBindingList<FlatTreeView> dataSource))
        return;
      CurrencyManager currencyManager = this.dgTrees.BindingContext[(object) dataSource] as CurrencyManager;
      bool flag = false;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgTrees.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
        {
          dataSource.RemoveAt(selectedRow.Index);
          flag = true;
        }
      }
      if (!(currencyManager.Position != -1 & flag))
        return;
      this.dgTrees.Rows[currencyManager.Position].Selected = true;
    }

    private void PlotTreesForm_VisibleChanged(object sender, EventArgs e)
    {
      bool flag = false;
      lock (this.m_syncobj)
      {
        flag = this.m_selectedPlot == null ? this.m_boundPlot != null && this.Visible : !this.m_selectedPlot.Equals((object) this.m_boundPlot) && this.Visible;
        flag |= this.m_refresh;
      }
      if (!flag)
        return;
      this.LoadAndBindPlot();
    }

    private void dgTrees_Scroll(object sender, ScrollEventArgs e)
    {
      if (e.ScrollOrientation != ScrollOrientation.HorizontalScroll)
        return;
      this.m_dgHorizPos = e.NewValue;
    }

    private void dgTrees_Sorted(object sender, EventArgs e) => this.dgTrees.HorizontalScrollingOffset = this.m_dgHorizPos;

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
      int num = this.dgTrees.Columns.IndexOf(dbh_col);
      if (column != num || !(this.dgTrees.DataSource is DataBindingList<FlatTreeView> dataSource) || row >= dataSource.Count)
        return;
      FlatTreeView component = dataSource[row];
      PropertyDescriptorCollection properties = TypeDescriptor.GetProperties((object) component);
      PropertyDescriptor propertyDescriptor1 = properties.Find(dbh_col.DataPropertyName, true);
      PropertyDescriptor propertyDescriptor2 = properties.Find(dbh_height_col.DataPropertyName, true);
      if (propertyDescriptor1 == null || propertyDescriptor2 == null)
        return;
      object obj1 = propertyDescriptor1.GetValue((object) component);
      object obj2 = propertyDescriptor2.GetValue((object) component);
      // ISSUE: variable of a boxed type
      __Boxed<double> local = (ValueType) -1.0;
      if (!obj1.Equals((object) local) || obj2.Equals((object) -1.0))
        return;
      propertyDescriptor2.SetValue((object) component, (object) -1.0);
    }

    private void dgTrees_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      this.UpdateDBHHeights(e.ColumnIndex, e.RowIndex);
      if (e.ColumnIndex != this.dcCrownCondition.DisplayIndex && e.ColumnIndex != this.dcCrownDieback.DisplayIndex)
        return;
      this.dgTrees.Refresh();
    }

    public void ContentActivated()
    {
      this.dgTrees.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgTrees.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      this.dgTrees.DefaultCellStyle = Program.ActiveGridDefaultCellStyle;
    }

    public void ContentDeactivated()
    {
      this.dgTrees.ColumnHeadersDefaultCellStyle = Program.InActiveGridColumnHeaderStyle;
      this.dgTrees.RowHeadersDefaultCellStyle = Program.InActiveGridRowHeaderStyle;
      this.dgTrees.DefaultCellStyle = Program.InActiveGridDefaultCellStyle;
    }

    protected override void OnRequestRefresh()
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new System.Action(((ContentForm) this).OnRequestRefresh));
      }
      else
      {
        DataBindingList<FlatTreeView> dataSource = this.dgTrees.DataSource as DataBindingList<FlatTreeView>;
        bool flag = this.m_year != null && this.m_year.Changed && dataSource != null && (!this.m_year.RecordLanduse || this.dcLandUse.DataSource != null);
        this.dgTrees.AllowUserToAddRows = flag;
        this.dgTrees.AllowUserToDeleteRows = flag;
        this.dgTrees.ReadOnly = !flag;
        this.Enabled = dataSource != null;
        base.OnRequestRefresh();
      }
    }

    public bool CanPerformAction(UserActions action)
    {
      bool flag1 = this.m_year != null && this.m_year.Changed && this.dgTrees.DataSource != null;
      bool flag2 = flag1 && this.dgTrees.AllowUserToAddRows;
      bool flag3 = this.dgTrees.SelectedRows.Count > 0;
      bool flag4 = this.dgTrees.CurrentRow != null && this.dgTrees.IsCurrentRowDirty;
      bool flag5 = this.dgTrees.CurrentRow != null && this.dgTrees.CurrentRow.IsNewRow;
      bool flag6 = false;
      switch (action)
      {
        case UserActions.New:
          flag6 = flag2 && !flag5 && !flag4;
          break;
        case UserActions.Copy:
          flag6 = flag2 & flag3 && !flag5 && !flag4;
          break;
        case UserActions.Undo:
          flag6 = flag1 && this.m_dgManager.CanUndo;
          break;
        case UserActions.Redo:
          flag6 = flag1 && this.m_dgManager.CanRedo;
          break;
        case UserActions.Delete:
          flag6 = flag1 & flag3 && !flag5 | flag4;
          break;
      }
      return flag6;
    }

    public void PerformAction(UserActions action)
    {
      if (!this.CanPerformAction(action))
        return;
      switch (action)
      {
        case UserActions.New:
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgTrees.SelectedRows)
            selectedRow.Selected = false;
          this.dgTrees.Rows[this.dgTrees.NewRowIndex].Selected = true;
          this.dgTrees.FirstDisplayedScrollingRowIndex = this.dgTrees.NewRowIndex - this.dgTrees.DisplayedRowCount(false) + 1;
          this.dgTrees.CurrentCell = this.dgTrees.Rows[this.dgTrees.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          DataGridViewRow selectedRow1 = this.dgTrees.SelectedRows[0];
          if (!(this.dgTrees.DataSource is DataBindingList<FlatTreeView> dataSource) || selectedRow1.Index >= dataSource.Count)
            break;
          FlatTreeView flatTreeView = dataSource[selectedRow1.Index].Clone() as FlatTreeView;
          flatTreeView.Id = this.NextTreeId((IList<FlatTreeView>) dataSource);
          dataSource.Add(flatTreeView);
          this.dgTrees.BindingContext[(object) dataSource].Position = dataSource.Count - 1;
          break;
        case UserActions.Undo:
          this.m_dgManager.Undo();
          break;
        case UserActions.Redo:
          this.m_dgManager.Redo();
          break;
        case UserActions.Delete:
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
        this.dgTrees.ExportToCSV(file);
    }

    public bool CanExport(ExportFormat format)
    {
      switch (format)
      {
        case ExportFormat.CSV:
          return this.dgTrees.DataSource != null;
        case ExportFormat.KML:
          return this.dgTrees.DataSource != null && this.m_year != null && this.m_year.RecordTreeGPS;
        default:
          return false;
      }
    }

    private void ExportKml(string fn)
    {
      if (!(this.dgTrees.DataSource is DataBindingList<FlatTreeView> dataSource))
        return;
      Kml root = new Kml();
      Document document1 = new Document();
      foreach (FlatTreeView flatTreeView in (Collection<FlatTreeView>) dataSource)
      {
        double? nullable = flatTreeView.Latitude;
        if (nullable.HasValue)
        {
          nullable = flatTreeView.Latitude;
          double num1 = -90.0;
          if (!(nullable.GetValueOrDefault() < num1 & nullable.HasValue))
          {
            nullable = flatTreeView.Latitude;
            double num2 = 90.0;
            if (!(nullable.GetValueOrDefault() > num2 & nullable.HasValue))
            {
              nullable = flatTreeView.Longitude;
              if (nullable.HasValue)
              {
                nullable = flatTreeView.Longitude;
                double num3 = -180.0;
                if (!(nullable.GetValueOrDefault() < num3 & nullable.HasValue))
                {
                  nullable = flatTreeView.Longitude;
                  double num4 = 180.0;
                  if (!(nullable.GetValueOrDefault() > num4 & nullable.HasValue))
                  {
                    Document document2 = document1;
                    Placemark placemark = new Placemark();
                    int id = flatTreeView.Id;
                    placemark.Id = id.ToString();
                    id = flatTreeView.Id;
                    placemark.Name = id.ToString();
                    placemark.Description = new Description()
                    {
                      Text = flatTreeView.Strata.Description
                    };
                    SharpKml.Dom.Point point = new SharpKml.Dom.Point();
                    nullable = flatTreeView.Latitude;
                    double latitude = nullable.Value;
                    nullable = flatTreeView.Longitude;
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

    private void dgTrees_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgTrees.NewRowIndex || !(this.dgTrees.DataSource is DataBindingList<FlatTreeView> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgTrees.Columns[e.ColumnIndex];
      FlatTreeView flatTreeView = dataSource[e.RowIndex];
      if (column == this.dcId)
      {
        if (flatTreeView.Id > 0)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcStatus)
      {
        if (Enum.IsDefined(typeof (TreeStatus), (object) flatTreeView.Status))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcDistance)
      {
        if ((double) Math.Abs(flatTreeView.Distance + 1f) <= 1.4012984643248171E-45)
          return;
        if ((double) flatTreeView.Distance < 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) flatTreeView.Distance <= 100.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 100);
        }
      }
      else if (column == this.dcDirection)
      {
        if (flatTreeView.Direction < -1)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatTreeView.Direction <= 360)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 360);
        }
      }
      else if (column == this.dcSpecies)
      {
        if (string.IsNullOrEmpty(flatTreeView.Species))
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
        }
        else
        {
          DataGridViewCell cell = this.dgTrees.Rows[e.RowIndex].Cells[e.ColumnIndex];
          if (cell.FormattedValue != null && !cell.FormattedValue.Equals((object) string.Empty))
            return;
          SpeciesView speciesView;
          if (this.m_ps.InvalidSpecies.TryGetValue(flatTreeView.Species, out speciesView))
            e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrSpeciesInvalid, (object) speciesView.CommonScientificName);
          else
            e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrSpeciesCode, (object) flatTreeView.Species);
        }
      }
      else if (column == this.dcLandUse)
      {
        if (flatTreeView.PlotLandUse != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcDBH1)
      {
        if (Math.Abs(flatTreeView.DBH1 + 1.0) > double.Epsilon)
        {
          if (flatTreeView.DBH1 >= 0.5)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0.5);
        }
        else if (Math.Abs(flatTreeView.DBH1Height + 1.0) > double.Epsilon)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcDBH1Height.HeaderText, (object) column.HeaderText);
        }
        else
        {
          if (Math.Abs(flatTreeView.DBH2 + 1.0) > double.Epsilon || Math.Abs(flatTreeView.DBH3 + 1.0) > double.Epsilon || Math.Abs(flatTreeView.DBH4 + 1.0) > double.Epsilon || Math.Abs(flatTreeView.DBH5 + 1.0) > double.Epsilon || Math.Abs(flatTreeView.DBH6 + 1.0) > double.Epsilon)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
        }
      }
      else if (column == this.dcDBH1c)
      {
        if (Math.Abs(flatTreeView.DBH1 + 1.0) <= double.Epsilon || this.m_dbhs.ContainsKey(flatTreeView.DBH1))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrInvalidOption, (object) flatTreeView.DBH1.ToString("0.#"));
      }
      else if (column == this.dcDBH1Height)
      {
        if (Math.Abs(flatTreeView.DBH1Height + 1.0) <= double.Epsilon)
          return;
        if (flatTreeView.DBH1Height <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatTreeView.DBH1Height <= 6.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 6);
        }
      }
      else if (column == this.dcDBH2)
      {
        if (Math.Abs(flatTreeView.DBH2 + 1.0) > double.Epsilon)
        {
          if (flatTreeView.DBH2 >= 0.5)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0.5);
        }
        else
        {
          if (Math.Abs(flatTreeView.DBH2Height + 1.0) <= double.Epsilon)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcDBH2Height.HeaderText, (object) column.HeaderText);
        }
      }
      else if (column == this.dcDBH2c)
      {
        if (Math.Abs(flatTreeView.DBH2 + 1.0) <= double.Epsilon || this.m_dbhs.ContainsKey(flatTreeView.DBH2))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrInvalidOption, (object) flatTreeView.DBH2.ToString("0.#"));
      }
      else if (column == this.dcDBH2Height)
      {
        if (Math.Abs(flatTreeView.DBH2Height + 1.0) <= double.Epsilon)
          return;
        if (flatTreeView.DBH2Height <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatTreeView.DBH2Height <= 6.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 6);
        }
      }
      else if (column == this.dcDBH3)
      {
        if (Math.Abs(flatTreeView.DBH3 + 1.0) > double.Epsilon)
        {
          if (flatTreeView.DBH3 >= 0.5)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0.5);
        }
        else
        {
          if (Math.Abs(flatTreeView.DBH3Height + 1.0) <= double.Epsilon)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcDBH3Height.HeaderText, (object) column.HeaderText);
        }
      }
      else if (column == this.dcDBH3c)
      {
        if (Math.Abs(flatTreeView.DBH3 + 1.0) <= double.Epsilon || this.m_dbhs.ContainsKey(flatTreeView.DBH3))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrInvalidOption, (object) flatTreeView.DBH3.ToString("0.#"));
      }
      else if (column == this.dcDBH3Height)
      {
        if (Math.Abs(flatTreeView.DBH3Height + 1.0) <= double.Epsilon)
          return;
        if (flatTreeView.DBH3Height <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatTreeView.DBH3Height <= 6.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 6);
        }
      }
      else if (column == this.dcDBH4)
      {
        if (Math.Abs(flatTreeView.DBH4 + 1.0) > double.Epsilon)
        {
          if (flatTreeView.DBH4 >= 0.5)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0.5);
        }
        else
        {
          if (Math.Abs(flatTreeView.DBH4Height + 1.0) <= double.Epsilon)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcDBH4Height.HeaderText, (object) column.HeaderText);
        }
      }
      else if (column == this.dcDBH4c)
      {
        if (Math.Abs(flatTreeView.DBH4 + 1.0) <= double.Epsilon || this.m_dbhs.ContainsKey(flatTreeView.DBH4))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrInvalidOption, (object) flatTreeView.DBH4.ToString("0.#"));
      }
      else if (column == this.dcDBH4Height)
      {
        if (Math.Abs(flatTreeView.DBH4Height + 1.0) <= double.Epsilon)
          return;
        if (flatTreeView.DBH4Height <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatTreeView.DBH4Height <= 6.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 6);
        }
      }
      else if (column == this.dcDBH5)
      {
        if (Math.Abs(flatTreeView.DBH5 + 1.0) > double.Epsilon)
        {
          if (flatTreeView.DBH5 >= 0.5)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0.5);
        }
        else
        {
          if (Math.Abs(flatTreeView.DBH5Height + 1.0) <= double.Epsilon)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcDBH5Height.HeaderText, (object) column.HeaderText);
        }
      }
      else if (column == this.dcDBH5c)
      {
        if (Math.Abs(flatTreeView.DBH5 + 1.0) <= double.Epsilon || this.m_dbhs.ContainsKey(flatTreeView.DBH5))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrInvalidOption, (object) flatTreeView.DBH5.ToString("0.#"));
      }
      else if (column == this.dcDBH5Height)
      {
        if (Math.Abs(flatTreeView.DBH5Height + 1.0) <= double.Epsilon)
          return;
        if (flatTreeView.DBH5Height <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatTreeView.DBH5Height <= 6.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 6);
        }
      }
      else if (column == this.dcDBH6)
      {
        if (Math.Abs(flatTreeView.DBH6 + 1.0) > double.Epsilon)
        {
          if (flatTreeView.DBH6 >= 0.5)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0.5);
        }
        else
        {
          if (Math.Abs(flatTreeView.DBH6Height + 1.0) <= double.Epsilon)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcDBH6Height.HeaderText, (object) column.HeaderText);
        }
      }
      else if (column == this.dcDBH6c)
      {
        if (Math.Abs(flatTreeView.DBH6 + 1.0) <= double.Epsilon || this.m_dbhs.ContainsKey(flatTreeView.DBH6))
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrInvalidOption, (object) flatTreeView.DBH6.ToString("0.#"));
      }
      else if (column == this.dcDBH6Height)
      {
        if (Math.Abs(flatTreeView.DBH6Height + 1.0) <= double.Epsilon)
          return;
        if (flatTreeView.DBH6Height <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatTreeView.DBH6Height <= 6.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 6);
        }
      }
      else if (column == this.dcTreeHeight)
      {
        if ((double) flatTreeView.Height < 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) flatTreeView.Height <= 450.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 450);
        }
      }
      else if (column == this.dcCrownCondition)
      {
        if (flatTreeView.CrownCondition != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcCrownDieback)
      {
        if (flatTreeView.CrownCondition != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcCrownTopHeight)
      {
        if (flatTreeView.CrownCondition == null || flatTreeView.IsDead)
          return;
        if ((double) Math.Abs(flatTreeView.CrownTopHeight + 1f) < 1.4012984643248171E-45)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
        else if ((double) flatTreeView.CrownTopHeight <= 0.0)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0.ToString("D"));
        else if (this.m_year.RecordHeight && (double) flatTreeView.Height > 0.0 && (double) flatTreeView.Height <= 450.0 && (double) flatTreeView.CrownTopHeight > (double) flatTreeView.Height)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) flatTreeView.Height.ToString("F2"));
        }
        else
        {
          if ((double) flatTreeView.CrownTopHeight <= 450.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 450.ToString("D"));
        }
      }
      else if (column == this.dcCrownBaseHeight)
      {
        if (flatTreeView.CrownCondition == null || flatTreeView.IsDead)
          return;
        if ((double) Math.Abs(flatTreeView.CrownBaseHeight + 1f) < 1.4012984643248171E-45)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
        else if ((double) flatTreeView.CrownBaseHeight < 0.0)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        else if ((double) flatTreeView.CrownTopHeight > 0.0 && (double) flatTreeView.CrownTopHeight <= 450.0 && (double) flatTreeView.CrownBaseHeight > (double) flatTreeView.CrownTopHeight)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) flatTreeView.CrownTopHeight.ToString("F2"));
        else if (this.m_year.RecordHeight && (double) flatTreeView.Height > 0.0 && (double) flatTreeView.Height <= 450.0 && (double) flatTreeView.CrownBaseHeight > (double) flatTreeView.Height)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) flatTreeView.Height.ToString("F2"));
        }
        else
        {
          if ((double) flatTreeView.CrownBaseHeight <= 450.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 450);
        }
      }
      else if (column == this.dcCrownWidthNS)
      {
        if (flatTreeView.CrownCondition == null || flatTreeView.IsDead)
          return;
        if ((double) Math.Abs(flatTreeView.CrownWidthNS + 1f) < 1.4012984643248171E-45)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
        else if ((double) flatTreeView.CrownWidthNS <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) flatTreeView.CrownWidthNS <= 300.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 300);
        }
      }
      else if (column == this.dcCrownWidthEW)
      {
        if (flatTreeView.CrownCondition == null || flatTreeView.IsDead)
          return;
        if ((double) Math.Abs(flatTreeView.CrownWidthEW + 1f) < 1.4012984643248171E-45)
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
        else if ((double) flatTreeView.CrownWidthEW <= 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) flatTreeView.CrownWidthEW <= 300.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 300);
        }
      }
      else if (column == this.dcBldg1Distance)
      {
        float num = this.m_year.Unit == YearUnit.English ? 60f : 18.29f;
        if ((double) Math.Abs(flatTreeView.B1Distance + 1f) < 1.4012984643248171E-45)
        {
          if (flatTreeView.B1Direction == (short) -1)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcBldg1Direction.HeaderText, (object) column.HeaderText);
        }
        else if ((double) flatTreeView.B1Distance < 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) flatTreeView.B1Distance <= (double) num)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) num);
        }
      }
      else if (column == this.dcBldg1Direction)
      {
        if (flatTreeView.B1Direction == (short) -1)
        {
          if ((double) Math.Abs(flatTreeView.B1Distance + 1f) <= 1.4012984643248171E-45)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcBldg1Distance.HeaderText, (object) column.HeaderText);
        }
        else if (flatTreeView.B1Direction < (short) 0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatTreeView.B1Direction <= (short) 360)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 360);
        }
      }
      else if (column == this.dcBldg2Distance)
      {
        float num = this.m_year.Unit == YearUnit.English ? 60f : 18.29f;
        if ((double) Math.Abs(flatTreeView.B2Distance + 1f) < 1.4012984643248171E-45)
        {
          if (flatTreeView.B2Direction == (short) -1)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcBldg2Direction.HeaderText, (object) column.HeaderText);
        }
        else if ((double) flatTreeView.B2Distance < 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) flatTreeView.B2Distance <= (double) num)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) num);
        }
      }
      else if (column == this.dcBldg2Direction)
      {
        if (flatTreeView.B2Direction == (short) -1)
        {
          if ((double) Math.Abs(flatTreeView.B2Distance + 1f) <= 1.4012984643248171E-45)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcBldg2Distance.HeaderText, (object) column.HeaderText);
        }
        else if (flatTreeView.B2Direction < (short) 0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatTreeView.B2Direction <= (short) 360)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 360);
        }
      }
      else if (column == this.dcBldg3Distance)
      {
        float num = this.m_year.Unit == YearUnit.English ? 60f : 18.29f;
        if ((double) Math.Abs(flatTreeView.B3Distance + 1f) < 1.4012984643248171E-45)
        {
          if (flatTreeView.B3Direction == (short) -1)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcBldg3Direction.HeaderText, (object) column.HeaderText);
        }
        else if ((double) flatTreeView.B3Distance < 0.0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if ((double) flatTreeView.B3Distance <= (double) num)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) num);
        }
      }
      else if (column == this.dcBldg3Direction)
      {
        if (flatTreeView.B3Direction == (short) -1)
        {
          if ((double) Math.Abs(flatTreeView.B3Distance + 1f) <= 1.4012984643248171E-45)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrDependentField, (object) this.dcBldg3Distance.HeaderText, (object) column.HeaderText);
        }
        else if (flatTreeView.B3Direction < (short) 0)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) column.HeaderText, (object) 0);
        }
        else
        {
          if (flatTreeView.B3Direction <= (short) 360)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 360);
        }
      }
      else
      {
        if (column != this.dcIPEDPest)
          return;
        if (flatTreeView.IPEDPest == 0 && (flatTreeView.IPEDTSDieback != 0 || flatTreeView.IPEDTSEpiSprout != 0 || flatTreeView.IPEDTSWiltFoli != 0 || flatTreeView.IPEDTSEnvStress != 0 || flatTreeView.IPEDTSHumStress != 0 || !string.IsNullOrEmpty(flatTreeView.IPEDTSNotes) || flatTreeView.IPEDBBAbnGrowth != 0 || flatTreeView.IPEDBBDiseaseSigns != 0 || flatTreeView.IPEDBBInsectPres != 0 || flatTreeView.IPEDBBInsectSigns != 0 || flatTreeView.IPEDBBProbLoc != 0 || !string.IsNullOrEmpty(flatTreeView.IPEDBBNotes) || flatTreeView.IPEDFTChewFoli != 0 || flatTreeView.IPEDFTDiscFoli != 0 || flatTreeView.IPEDFTAbnFoli != 0 || flatTreeView.IPEDFTInsectSigns != 0 || flatTreeView.IPEDFTFoliAffect != 0 || !string.IsNullOrEmpty(flatTreeView.IPEDFTNotes)))
        {
          e.ErrorText = i_Tree_Eco_v6.Resources.Strings.ErrIPEDPest;
        }
        else
        {
          if (flatTreeView.IPEDPest == 0 || flatTreeView.IPEDTSDieback != 0 || flatTreeView.IPEDTSEnvStress != 0 || flatTreeView.IPEDTSEpiSprout != 0 || flatTreeView.IPEDTSHumStress != 0 || flatTreeView.IPEDTSWiltFoli != 0 || !string.IsNullOrEmpty(flatTreeView.IPEDTSNotes) || flatTreeView.IPEDBBAbnGrowth != 0 || flatTreeView.IPEDBBDiseaseSigns != 0 || flatTreeView.IPEDBBInsectPres != 0 || flatTreeView.IPEDBBInsectSigns != 0 || flatTreeView.IPEDBBProbLoc != 0 || !string.IsNullOrEmpty(flatTreeView.IPEDBBNotes) || flatTreeView.IPEDFTAbnFoli != 0 || flatTreeView.IPEDFTChewFoli != 0 || flatTreeView.IPEDFTDiscFoli != 0 || flatTreeView.IPEDFTFoliAffect != 0 || flatTreeView.IPEDFTInsectSigns != 0 || !string.IsNullOrEmpty(flatTreeView.IPEDFTNotes))
            return;
          e.ErrorText = i_Tree_Eco_v6.Resources.Strings.ErrIPEDSymptoms;
        }
      }
    }

    private void dgTrees_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    private void dgTrees_EditingControlShowing(
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

    private void dgTrees_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
    {
      DataGridViewCell currentCell = this.dgTrees.CurrentCell;
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PlotTreesForm));
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
      DataGridViewCellStyle gridViewCellStyle31 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle32 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle33 = new DataGridViewCellStyle();
      this.dgTrees = new DataGridView();
      this.dcId = new DataGridViewNumericTextBoxColumn();
      this.dcUserId = new DataGridViewTextBoxColumn();
      this.dcSurveyDate = new DataGridViewNullableDateTimeColumn();
      this.dcStatus = new DataGridViewComboBoxColumn();
      this.dcDistance = new DataGridViewNumericTextBoxColumn();
      this.dcDirection = new DataGridViewNumericTextBoxColumn();
      this.dcSpecies = new DataGridViewFilteredComboBoxColumn();
      this.dcLandUse = new DataGridViewComboBoxColumn();
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
      this.dcPctImpervious = new DataGridViewComboBoxColumn();
      this.dcPctShrub = new DataGridViewComboBoxColumn();
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
      this.dcStreet = new DataGridViewComboBoxColumn();
      this.dcAddress = new DataGridViewTextBoxColumn();
      this.dcLocSite = new DataGridViewComboBoxColumn();
      this.dcLocNo = new DataGridViewNumericTextBoxColumn();
      this.dcLatitude = new DataGridViewTextBoxColumn();
      this.dcLongitude = new DataGridViewTextBoxColumn();
      this.dcNoteThisTree = new DataGridViewCheckBoxColumn();
      this.dcComments = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dgTrees).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgTrees.AllowUserToAddRows = false;
      this.dgTrees.AllowUserToDeleteRows = false;
      this.dgTrees.AllowUserToResizeRows = false;
      this.dgTrees.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dgTrees.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dgTrees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgTrees.Columns.AddRange((DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcUserId, (DataGridViewColumn) this.dcSurveyDate, (DataGridViewColumn) this.dcStatus, (DataGridViewColumn) this.dcDistance, (DataGridViewColumn) this.dcDirection, (DataGridViewColumn) this.dcSpecies, (DataGridViewColumn) this.dcLandUse, (DataGridViewColumn) this.dcDBH1, (DataGridViewColumn) this.dcDBH1c, (DataGridViewColumn) this.dcDBH1Height, (DataGridViewColumn) this.dcDBH1Measured, (DataGridViewColumn) this.dcDBH2, (DataGridViewColumn) this.dcDBH2c, (DataGridViewColumn) this.dcDBH2Height, (DataGridViewColumn) this.dcDBH2Measured, (DataGridViewColumn) this.dcDBH3, (DataGridViewColumn) this.dcDBH3c, (DataGridViewColumn) this.dcDBH3Height, (DataGridViewColumn) this.dcDBH3Measured, (DataGridViewColumn) this.dcDBH4, (DataGridViewColumn) this.dcDBH4c, (DataGridViewColumn) this.dcDBH4Height, (DataGridViewColumn) this.dcDBH4Measured, (DataGridViewColumn) this.dcDBH5, (DataGridViewColumn) this.dcDBH5c, (DataGridViewColumn) this.dcDBH5Height, (DataGridViewColumn) this.dcDBH5Measured, (DataGridViewColumn) this.dcDBH6, (DataGridViewColumn) this.dcDBH6c, (DataGridViewColumn) this.dcDBH6Height, (DataGridViewColumn) this.dcDBH6Measured, (DataGridViewColumn) this.dcCrownCondition, (DataGridViewColumn) this.dcCrownDieback, (DataGridViewColumn) this.dcTreeHeight, (DataGridViewColumn) this.dcCrownTopHeight, (DataGridViewColumn) this.dcCrownBaseHeight, (DataGridViewColumn) this.dcCrownWidthNS, (DataGridViewColumn) this.dcCrownWidthEW, (DataGridViewColumn) this.dcCrownPercentMissing, (DataGridViewColumn) this.dcPctImpervious, (DataGridViewColumn) this.dcPctShrub, (DataGridViewColumn) this.dcCrownLightExposure, (DataGridViewColumn) this.dcBldg1Direction, (DataGridViewColumn) this.dcBldg1Distance, (DataGridViewColumn) this.dcBldg2Direction, (DataGridViewColumn) this.dcBldg2Distance, (DataGridViewColumn) this.dcBldg3Direction, (DataGridViewColumn) this.dcBldg3Distance, (DataGridViewColumn) this.dcBldg4Direction, (DataGridViewColumn) this.dcBldg4Distance, (DataGridViewColumn) this.dcSiteType, (DataGridViewColumn) this.dcStTree, (DataGridViewColumn) this.dcMaintRec, (DataGridViewColumn) this.dcMaintTask, (DataGridViewColumn) this.dcSidewalk, (DataGridViewColumn) this.dcWireConflict, (DataGridViewColumn) this.dcFieldOne, (DataGridViewColumn) this.dcFieldTwo, (DataGridViewColumn) this.dcFieldThree, (DataGridViewColumn) this.dcIPEDTSDieback, (DataGridViewColumn) this.dcIPEDTSEpiSprout, (DataGridViewColumn) this.dcIPEDTSWiltFoli, (DataGridViewColumn) this.dcIPEDTSEnvStress, (DataGridViewColumn) this.dcIPEDTSHumStress, (DataGridViewColumn) this.dcIPEDTSNotes, (DataGridViewColumn) this.dcIPEDFTChewFoli, (DataGridViewColumn) this.dcIPEDFTDiscFoli, (DataGridViewColumn) this.dcIPEDFTAbnFoli, (DataGridViewColumn) this.dcIPEDFTInsectSigns, (DataGridViewColumn) this.dcIPEDFTFoliAffect, (DataGridViewColumn) this.dcIPEDFTNotes, (DataGridViewColumn) this.dcIPEDBBInsectSigns, (DataGridViewColumn) this.dcIPEDBBInsectPres, (DataGridViewColumn) this.dcIPEDBBDiseaseSigns, (DataGridViewColumn) this.dcIPEDBBProbLoc, (DataGridViewColumn) this.dcIPEDBBAbnGrowth, (DataGridViewColumn) this.dcIPEDBBNotes, (DataGridViewColumn) this.dcIPEDPest, (DataGridViewColumn) this.dcCityManaged, (DataGridViewColumn) this.dcStreet, (DataGridViewColumn) this.dcAddress, (DataGridViewColumn) this.dcLocSite, (DataGridViewColumn) this.dcLocNo, (DataGridViewColumn) this.dcLatitude, (DataGridViewColumn) this.dcLongitude, (DataGridViewColumn) this.dcNoteThisTree, (DataGridViewColumn) this.dcComments);
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.dgTrees.DefaultCellStyle = gridViewCellStyle2;
      componentResourceManager.ApplyResources((object) this.dgTrees, "dgTrees");
      this.dgTrees.EnableHeadersVisualStyles = false;
      this.dgTrees.MultiSelect = false;
      this.dgTrees.Name = "dgTrees";
      this.dgTrees.ReadOnly = true;
      this.dgTrees.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgTrees.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgTrees.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgTrees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgTrees.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgTrees_CellErrorTextNeeded);
      this.dgTrees.CellParsing += new DataGridViewCellParsingEventHandler(this.dgTrees_CellParsing);
      this.dgTrees.CellValueChanged += new DataGridViewCellEventHandler(this.dgTrees_CellValueChanged);
      this.dgTrees.DataError += new DataGridViewDataErrorEventHandler(this.dgTrees_DataError);
      this.dgTrees.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgTrees_EditingControlShowing);
      this.dgTrees.RowValidated += new DataGridViewCellEventHandler(this.dgTrees_RowValidated);
      this.dgTrees.RowValidating += new DataGridViewCellCancelEventHandler(this.dgTrees_RowValidating);
      this.dgTrees.Scroll += new ScrollEventHandler(this.dgTrees_Scroll);
      this.dgTrees.SelectionChanged += new EventHandler(this.dgTrees_SelectionChanged);
      this.dgTrees.Sorted += new EventHandler(this.dgTrees_Sorted);
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
      this.dcDistance.DataPropertyName = "Distance";
      this.dcDistance.DecimalPlaces = 2;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle3.Format = "N2";
      this.dcDistance.DefaultCellStyle = gridViewCellStyle3;
      this.dcDistance.Format = (string) null;
      this.dcDistance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDistance, "dcDistance");
      this.dcDistance.Name = "dcDistance";
      this.dcDistance.ReadOnly = true;
      this.dcDistance.Resizable = DataGridViewTriState.True;
      this.dcDistance.Signed = false;
      this.dcDirection.DataPropertyName = "Direction";
      this.dcDirection.DecimalPlaces = 0;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcDirection.DefaultCellStyle = gridViewCellStyle4;
      this.dcDirection.Format = (string) null;
      this.dcDirection.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcDirection, "dcDirection");
      this.dcDirection.Name = "dcDirection";
      this.dcDirection.ReadOnly = true;
      this.dcDirection.Resizable = DataGridViewTriState.True;
      this.dcDirection.Signed = false;
      this.dcSpecies.DataPropertyName = "Species";
      this.dcSpecies.DataSource = (object) null;
      this.dcSpecies.DisplayMember = (string) null;
      componentResourceManager.ApplyResources((object) this.dcSpecies, "dcSpecies");
      this.dcSpecies.Name = "dcSpecies";
      this.dcSpecies.ReadOnly = true;
      this.dcSpecies.Resizable = DataGridViewTriState.True;
      this.dcSpecies.ValueMember = (string) null;
      this.dcLandUse.DataPropertyName = "PlotLandUse";
      this.dcLandUse.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcLandUse, "dcLandUse");
      this.dcLandUse.Name = "dcLandUse";
      this.dcLandUse.ReadOnly = true;
      this.dcLandUse.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH1.DataPropertyName = "DBH1";
      this.dcDBH1.DecimalPlaces = 1;
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle5.Format = "N1";
      this.dcDBH1.DefaultCellStyle = gridViewCellStyle5;
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
      gridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle6.Format = "N2";
      this.dcDBH1Height.DefaultCellStyle = gridViewCellStyle6;
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
      gridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle7.Format = "N1";
      this.dcDBH2.DefaultCellStyle = gridViewCellStyle7;
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
      gridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle8.Format = "N2";
      this.dcDBH2Height.DefaultCellStyle = gridViewCellStyle8;
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
      gridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle9.Format = "N1";
      this.dcDBH3.DefaultCellStyle = gridViewCellStyle9;
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
      gridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle10.Format = "N2";
      this.dcDBH3Height.DefaultCellStyle = gridViewCellStyle10;
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
      gridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle11.Format = "N1";
      this.dcDBH4.DefaultCellStyle = gridViewCellStyle11;
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
      gridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle12.Format = "N2";
      this.dcDBH4Height.DefaultCellStyle = gridViewCellStyle12;
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
      gridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle13.Format = "N1";
      this.dcDBH5.DefaultCellStyle = gridViewCellStyle13;
      this.dcDBH5.Format = "#;#";
      this.dcDBH5.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDBH5, "dcDBH5");
      this.dcDBH5.Name = "dcDBH5";
      this.dcDBH5.ReadOnly = true;
      this.dcDBH5.Signed = false;
      this.dcDBH5c.DataPropertyName = "DBH 5";
      this.dcDBH5c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcDBH5c, "dcDBH5c");
      this.dcDBH5c.Name = "dcDBH5c";
      this.dcDBH5c.ReadOnly = true;
      this.dcDBH5c.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcDBH5Height.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcDBH5Height.DataPropertyName = "DBH5Height";
      this.dcDBH5Height.DecimalPlaces = 2;
      gridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle14.Format = "N2";
      this.dcDBH5Height.DefaultCellStyle = gridViewCellStyle14;
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
      gridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle15.Format = "N1";
      this.dcDBH6.DefaultCellStyle = gridViewCellStyle15;
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
      gridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle16.Format = "N2";
      this.dcDBH6Height.DefaultCellStyle = gridViewCellStyle16;
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
      gridViewCellStyle17.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle17.Format = "N2";
      this.dcTreeHeight.DefaultCellStyle = gridViewCellStyle17;
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
      gridViewCellStyle18.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle18.Format = "N2";
      this.dcCrownTopHeight.DefaultCellStyle = gridViewCellStyle18;
      this.dcCrownTopHeight.Format = "#;#";
      this.dcCrownTopHeight.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownTopHeight, "dcCrownTopHeight");
      this.dcCrownTopHeight.Name = "dcCrownTopHeight";
      this.dcCrownTopHeight.ReadOnly = true;
      this.dcCrownTopHeight.Signed = false;
      this.dcCrownBaseHeight.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownBaseHeight.DataPropertyName = "CrownBaseHeight";
      this.dcCrownBaseHeight.DecimalPlaces = 2;
      gridViewCellStyle19.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle19.Format = "N2";
      this.dcCrownBaseHeight.DefaultCellStyle = gridViewCellStyle19;
      this.dcCrownBaseHeight.Format = "0.#;0.#";
      this.dcCrownBaseHeight.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownBaseHeight, "dcCrownBaseHeight");
      this.dcCrownBaseHeight.Name = "dcCrownBaseHeight";
      this.dcCrownBaseHeight.ReadOnly = true;
      this.dcCrownBaseHeight.Signed = false;
      this.dcCrownWidthNS.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownWidthNS.DataPropertyName = "CrownWidthNS";
      this.dcCrownWidthNS.DecimalPlaces = 1;
      gridViewCellStyle20.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle20.Format = "N1";
      this.dcCrownWidthNS.DefaultCellStyle = gridViewCellStyle20;
      this.dcCrownWidthNS.Format = "#;#";
      this.dcCrownWidthNS.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownWidthNS, "dcCrownWidthNS");
      this.dcCrownWidthNS.Name = "dcCrownWidthNS";
      this.dcCrownWidthNS.ReadOnly = true;
      this.dcCrownWidthNS.Signed = false;
      this.dcCrownWidthEW.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownWidthEW.DataPropertyName = "CrownWidthEW";
      this.dcCrownWidthEW.DecimalPlaces = 1;
      gridViewCellStyle21.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle21.Format = "N1";
      this.dcCrownWidthEW.DefaultCellStyle = gridViewCellStyle21;
      this.dcCrownWidthEW.Format = "#;#";
      this.dcCrownWidthEW.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownWidthEW, "dcCrownWidthEW");
      this.dcCrownWidthEW.Name = "dcCrownWidthEW";
      this.dcCrownWidthEW.ReadOnly = true;
      this.dcCrownWidthEW.Signed = false;
      this.dcCrownPercentMissing.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownPercentMissing.DataPropertyName = "CrownPercentMissing";
      gridViewCellStyle22.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcCrownPercentMissing.DefaultCellStyle = gridViewCellStyle22;
      this.dcCrownPercentMissing.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcCrownPercentMissing, "dcCrownPercentMissing");
      this.dcCrownPercentMissing.Name = "dcCrownPercentMissing";
      this.dcCrownPercentMissing.ReadOnly = true;
      this.dcCrownPercentMissing.Resizable = DataGridViewTriState.True;
      this.dcCrownPercentMissing.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctImpervious.DataPropertyName = "PercentImpervious";
      gridViewCellStyle23.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcPctImpervious.DefaultCellStyle = gridViewCellStyle23;
      this.dcPctImpervious.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcPctImpervious, "dcPctImpervious");
      this.dcPctImpervious.Name = "dcPctImpervious";
      this.dcPctImpervious.ReadOnly = true;
      this.dcPctImpervious.Resizable = DataGridViewTriState.True;
      this.dcPctImpervious.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctShrub.DataPropertyName = "PercentShrub";
      gridViewCellStyle24.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcPctShrub.DefaultCellStyle = gridViewCellStyle24;
      this.dcPctShrub.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcPctShrub, "dcPctShrub");
      this.dcPctShrub.Name = "dcPctShrub";
      this.dcPctShrub.ReadOnly = true;
      this.dcPctShrub.Resizable = DataGridViewTriState.True;
      this.dcPctShrub.SortMode = DataGridViewColumnSortMode.Automatic;
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
      gridViewCellStyle25.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle25.Format = "N0";
      this.dcBldg1Direction.DefaultCellStyle = gridViewCellStyle25;
      this.dcBldg1Direction.Format = "#;#";
      this.dcBldg1Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg1Direction, "dcBldg1Direction");
      this.dcBldg1Direction.Name = "dcBldg1Direction";
      this.dcBldg1Direction.ReadOnly = true;
      this.dcBldg1Direction.Signed = false;
      this.dcBldg1Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg1Distance.DataPropertyName = "B1Distance";
      this.dcBldg1Distance.DecimalPlaces = 2;
      gridViewCellStyle26.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle26.Format = "N2";
      this.dcBldg1Distance.DefaultCellStyle = gridViewCellStyle26;
      this.dcBldg1Distance.Format = "#;#";
      this.dcBldg1Distance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcBldg1Distance, "dcBldg1Distance");
      this.dcBldg1Distance.Name = "dcBldg1Distance";
      this.dcBldg1Distance.ReadOnly = true;
      this.dcBldg1Distance.Signed = false;
      this.dcBldg2Direction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg2Direction.DataPropertyName = "B2Direction";
      this.dcBldg2Direction.DecimalPlaces = 0;
      gridViewCellStyle27.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle27.Format = "N0";
      this.dcBldg2Direction.DefaultCellStyle = gridViewCellStyle27;
      this.dcBldg2Direction.Format = "#;#";
      this.dcBldg2Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg2Direction, "dcBldg2Direction");
      this.dcBldg2Direction.Name = "dcBldg2Direction";
      this.dcBldg2Direction.ReadOnly = true;
      this.dcBldg2Direction.Signed = false;
      this.dcBldg2Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg2Distance.DataPropertyName = "B2Distance";
      this.dcBldg2Distance.DecimalPlaces = 2;
      gridViewCellStyle28.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle28.Format = "N2";
      this.dcBldg2Distance.DefaultCellStyle = gridViewCellStyle28;
      this.dcBldg2Distance.Format = "#;#";
      this.dcBldg2Distance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcBldg2Distance, "dcBldg2Distance");
      this.dcBldg2Distance.Name = "dcBldg2Distance";
      this.dcBldg2Distance.ReadOnly = true;
      this.dcBldg2Distance.Signed = false;
      this.dcBldg3Direction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg3Direction.DataPropertyName = "B3Direction";
      this.dcBldg3Direction.DecimalPlaces = 0;
      gridViewCellStyle29.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle29.Format = "N0";
      this.dcBldg3Direction.DefaultCellStyle = gridViewCellStyle29;
      this.dcBldg3Direction.Format = "#;#";
      this.dcBldg3Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg3Direction, "dcBldg3Direction");
      this.dcBldg3Direction.Name = "dcBldg3Direction";
      this.dcBldg3Direction.ReadOnly = true;
      this.dcBldg3Direction.Signed = false;
      this.dcBldg3Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg3Distance.DataPropertyName = "B3Distance";
      this.dcBldg3Distance.DecimalPlaces = 2;
      gridViewCellStyle30.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle30.Format = "N2";
      this.dcBldg3Distance.DefaultCellStyle = gridViewCellStyle30;
      this.dcBldg3Distance.Format = "#;#";
      this.dcBldg3Distance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcBldg3Distance, "dcBldg3Distance");
      this.dcBldg3Distance.Name = "dcBldg3Distance";
      this.dcBldg3Distance.ReadOnly = true;
      this.dcBldg3Distance.Signed = false;
      this.dcBldg4Direction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg4Direction.DataPropertyName = "B4Direction";
      this.dcBldg4Direction.DecimalPlaces = 0;
      gridViewCellStyle31.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle31.Format = "N0";
      this.dcBldg4Direction.DefaultCellStyle = gridViewCellStyle31;
      this.dcBldg4Direction.Format = "#;#";
      this.dcBldg4Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg4Direction, "dcBldg4Direction");
      this.dcBldg4Direction.Name = "dcBldg4Direction";
      this.dcBldg4Direction.ReadOnly = true;
      this.dcBldg4Direction.Signed = false;
      this.dcBldg4Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg4Distance.DataPropertyName = "B4Distance";
      this.dcBldg4Distance.DecimalPlaces = 2;
      gridViewCellStyle32.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle32.Format = "N2";
      this.dcBldg4Distance.DefaultCellStyle = gridViewCellStyle32;
      this.dcBldg4Distance.Format = "#;#";
      this.dcBldg4Distance.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg4Distance, "dcBldg4Distance");
      this.dcBldg4Distance.Name = "dcBldg4Distance";
      this.dcBldg4Distance.ReadOnly = true;
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
      this.dcStreet.DataPropertyName = "Street";
      componentResourceManager.ApplyResources((object) this.dcStreet, "dcStreet");
      this.dcStreet.Name = "dcStreet";
      this.dcStreet.ReadOnly = true;
      this.dcStreet.Resizable = DataGridViewTriState.True;
      this.dcStreet.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcAddress.DataPropertyName = "Address";
      componentResourceManager.ApplyResources((object) this.dcAddress, "dcAddress");
      this.dcAddress.MaxInputLength = (int) byte.MaxValue;
      this.dcAddress.Name = "dcAddress";
      this.dcAddress.ReadOnly = true;
      this.dcLocSite.DataPropertyName = "LocSite";
      componentResourceManager.ApplyResources((object) this.dcLocSite, "dcLocSite");
      this.dcLocSite.Name = "dcLocSite";
      this.dcLocSite.ReadOnly = true;
      this.dcLocSite.Resizable = DataGridViewTriState.True;
      this.dcLocSite.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcLocNo.DataPropertyName = "LocNo";
      this.dcLocNo.DecimalPlaces = 0;
      gridViewCellStyle33.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle33.Format = "N0";
      this.dcLocNo.DefaultCellStyle = gridViewCellStyle33;
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
      this.dcNoteThisTree.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcNoteThisTree.DataPropertyName = "NoteThisTree";
      componentResourceManager.ApplyResources((object) this.dcNoteThisTree, "dcNoteThisTree");
      this.dcNoteThisTree.Name = "dcNoteThisTree";
      this.dcNoteThisTree.ReadOnly = true;
      this.dcNoteThisTree.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcComments.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcComments.DataPropertyName = "Comments";
      componentResourceManager.ApplyResources((object) this.dcComments, "dcComments");
      this.dcComments.MaxInputLength = (int) byte.MaxValue;
      this.dcComments.Name = "dcComments";
      this.dcComments.ReadOnly = true;
      this.dcComments.Resizable = DataGridViewTriState.True;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CloseButtonVisible = false;
      this.Controls.Add((Control) this.dgTrees);
      this.DockAreas = DockAreas.DockBottom;
      this.Name = nameof (PlotTreesForm);
      this.VisibleChanged += new EventHandler(this.PlotTreesForm_VisibleChanged);
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.dgTrees, 0);
      ((ISupportInitialize) this.dgTrees).EndInit();
      this.ResumeLayout(false);
    }
  }
}
