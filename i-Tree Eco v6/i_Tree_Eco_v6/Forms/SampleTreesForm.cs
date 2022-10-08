// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.SampleTreesForm
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
  public class SampleTreesForm : DataContentForm, IActionable, IExportable
  {
    private DataGridViewManager m_dgManager;
    private int m_dgHorizPos;
    private IPEDData m_iped;
    private IList<Street> m_streets;
    private BindingList<SpeciesView> m_species;
    private List<FlatTreeView> m_trees;
    private IList<Plot> m_plots;
    private Dictionary<PlotLandUse, string> m_PlotLandUses;
    private IDictionary<double, string> m_dbhs;
    private IContainer components;
    private DataGridView dgTrees;
    private TableLayoutPanel pnlEditHelp;
    private DataGridViewComboBoxColumn dcPlot;
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
    private DataGridViewCheckBoxColumn dcNoteTree;
    private DataGridViewTextBoxColumn dcComments;

    public SampleTreesForm()
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
      this.m_dgManager = new DataGridViewManager(this.dgTrees);
      this.dgTrees.DoubleBuffered(true);
      this.dgTrees.AutoGenerateColumns = false;
      this.dgTrees.ColumnHeadersDefaultCellStyle = Program.ActiveGridColumnHeaderStyle;
      this.dgTrees.RowHeadersDefaultCellStyle = Program.ActiveGridRowHeaderStyle;
      EventPublisher.Register<EntityUpdated<Tree>>(new EventHandler<EntityUpdated<Tree>>(this.Tree_Updated));
    }

    protected override void InitializeYear(Year y)
    {
      base.InitializeYear(y);
      NHibernateUtil.Initialize((object) y.Series);
      NHibernateUtil.Initialize((object) y.Plots);
      if (!y.DBHActual)
        NHibernateUtil.Initialize((object) y.DBHs);
      if (y.RecordCrownCondition)
        NHibernateUtil.Initialize((object) y.Conditions);
      if (y.RecordSiteType)
        NHibernateUtil.Initialize((object) y.SiteTypes);
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
        NHibernateUtil.Initialize((object) y.OtherThree);
      if (y.RecordIPED)
        this.m_iped = Program.Session.IPEDData;
      if (!y.RecordLocSite)
        return;
      NHibernateUtil.Initialize((object) y.LocSites);
    }

    protected override void LoadData()
    {
      base.LoadData();
      lock (this.Session)
      {
        using (this.Session.BeginTransaction())
        {
          Year year = this.Year;
          if (year.RecordTreeStreet)
            this.m_streets = this.Session.CreateCriteria<Street>().CreateAlias("ProjectLocation", "pl").CreateAlias("pl.Project", "p").CreateAlias("p.Series", "s").CreateAlias("s.Years", "y").Add((ICriterion) Restrictions.Eq("y.Guid", (object) year.Guid)).AddOrder(Order.Asc(TypeHelper.NameOf<Street>((System.Linq.Expressions.Expression<Func<Street, object>>) (st => st.Name)))).List<Street>();
          if (year.RecordLanduse)
          {
            using (TypeHelper<PlotLandUse> typeHelper = new TypeHelper<PlotLandUse>())
            {
              IList<PlotLandUse> plotLandUseList = this.Session.CreateCriteria<PlotLandUse>().CreateAlias(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<PlotLandUse, object>>) (plu => plu.Plot)), "p").CreateAlias(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<PlotLandUse, object>>) (plu => plu.LandUse)), "lu").Add((ICriterion) Restrictions.Eq("p.Year", (object) year)).Fetch(SelectMode.Fetch, typeHelper.NameOf((System.Linq.Expressions.Expression<Func<PlotLandUse, object>>) (plu => plu.LandUse))).AddOrder(Order.Asc("lu.Description")).List<PlotLandUse>();
              this.m_PlotLandUses = new Dictionary<PlotLandUse, string>();
              foreach (PlotLandUse key in (IEnumerable<PlotLandUse>) plotLandUseList)
                this.m_PlotLandUses[key] = key.LandUse.Description;
            }
          }
          using (TypeHelper<Stem> typeHelper = new TypeHelper<Stem>())
            this.Session.CreateCriteria<Stem>().CreateAlias(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Stem, object>>) (st => st.Tree)), "t").CreateAlias("t.Plot", "p").Add((ICriterion) Restrictions.Eq("p.Year", (object) year)).List<Stem>();
          using (TypeHelper<Building> typeHelper = new TypeHelper<Building>())
            this.Session.CreateCriteria<Building>().CreateAlias(typeHelper.NameOf((System.Linq.Expressions.Expression<Func<Building, object>>) (b => b.Tree)), "t").CreateAlias("t.Plot", "p").Add((ICriterion) Restrictions.Eq("p.Year", (object) year)).List<Building>();
          this.m_plots = this.Session.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this.Year)).OrderBy((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)).Asc.JoinQueryOver<Tree>((System.Linq.Expressions.Expression<Func<Plot, IEnumerable<Tree>>>) (p => p.Trees)).OrderBy((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => (object) tr.Id)).Asc.TransformUsing(Transformers.DistinctRootEntity).List();
          this.m_trees = new List<FlatTreeView>();
          foreach (Plot plot in (IEnumerable<Plot>) this.m_plots)
          {
            int count = plot.Shrubs.Count;
            foreach (Tree tree in (IEnumerable<Tree>) plot.Trees)
            {
              NHibernateUtil.Initialize((object) tree.Stems);
              NHibernateUtil.Initialize((object) tree.Buildings);
              this.m_trees.Add(new FlatTreeView(this.Session, tree));
            }
          }
          this.m_dbhs = (IDictionary<double, string>) year.DBHs.ToDictionary<DBH, double, string>((Func<DBH, double>) (d => Convert.ToDouble(d.Id)), (Func<DBH, string>) (d => d.Description));
        }
      }
      List<SpeciesView> list = Program.Session.Species.Values.ToList<SpeciesView>();
      list.Sort((IComparer<SpeciesView>) new PropertyComparer<SpeciesView>((Func<SpeciesView, object>) (sp => (object) sp.CommonName)));
      this.m_species = (BindingList<SpeciesView>) new ExtendedBindingList<SpeciesView>((IList<SpeciesView>) list);
    }

    protected override void OnDataLoaded()
    {
      base.OnDataLoaded();
      this.InitGrid();
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
      this.dcPlot.BindTo<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id), (System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Self), (object) this.m_plots);
      if (!year.RecordTreeUserId)
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcUserId);
      using (TypeHelper<SpeciesView> typeHelper = new TypeHelper<SpeciesView>())
      {
        this.dcSpecies.DisplayMember = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.CommonScientificName));
        this.dcSpecies.ValueMember = typeHelper.NameOf((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (sv => sv.Code));
        this.dcSpecies.DataSource = (object) this.m_species;
      }
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
      if (year.RecordLanduse)
        this.dcLandUse.BindTo<KeyValuePair<PlotLandUse, string>>((System.Linq.Expressions.Expression<Func<KeyValuePair<PlotLandUse, string>, object>>) (de => de.Value), (System.Linq.Expressions.Expression<Func<KeyValuePair<PlotLandUse, string>, object>>) (de => de.Key), (object) this.m_PlotLandUses.ToList<KeyValuePair<PlotLandUse, string>>());
      else
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
        this.dcCrownPercentMissing.BindTo("Value", "Key", (object) new BindingSource((object) EnumHelper.ConvertToDictionary<PctMidRange>(new PctMidRange[1]
        {
          PctMidRange.PRINV
        }), (string) null));
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
        BindingSource dataSource = new BindingSource((object) EnumHelper.ConvertToDictionary<PctMidRange>(), (string) null);
        this.dcPctImpervious.BindTo("Value", "Key", (object) dataSource);
        this.dcPctShrub.BindTo("Value", "Key", (object) dataSource);
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
        this.dcMaintRec.BindTo<MaintRec>((System.Linq.Expressions.Expression<Func<MaintRec, object>>) (mr => mr.Description), (System.Linq.Expressions.Expression<Func<MaintRec, object>>) (mr => mr.Self), (object) year.MaintRecs.ToList<MaintRec>());
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcMaintRec);
      if (year.RecordMaintTask)
        this.dcMaintTask.BindTo<MaintTask>((System.Linq.Expressions.Expression<Func<MaintTask, object>>) (mt => mt.Description), (System.Linq.Expressions.Expression<Func<MaintTask, object>>) (mt => mt.Self), (object) year.MaintTasks.ToList<MaintTask>());
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcMaintTask);
      if (year.RecordSidewalk)
        this.dcSidewalk.BindTo<Sidewalk>((System.Linq.Expressions.Expression<Func<Sidewalk, object>>) (sw => sw.Description), (System.Linq.Expressions.Expression<Func<Sidewalk, object>>) (sw => sw.Self), (object) year.SidewalkDamages.ToList<Sidewalk>());
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcSidewalk);
      if (year.RecordWireConflict)
        this.dcWireConflict.BindTo<WireConflict>((System.Linq.Expressions.Expression<Func<WireConflict, object>>) (wc => wc.Description), (System.Linq.Expressions.Expression<Func<WireConflict, object>>) (wc => wc.Self), (object) year.WireConflicts.ToList<WireConflict>());
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcWireConflict);
      if (year.RecordOtherOne)
      {
        if (!string.IsNullOrEmpty(year.OtherOne))
          this.dcFieldOne.HeaderText = year.OtherOne;
        this.dcFieldOne.BindTo<OtherOne>((System.Linq.Expressions.Expression<Func<OtherOne, object>>) (f1 => f1.Description), (System.Linq.Expressions.Expression<Func<OtherOne, object>>) (f1 => f1.Self), (object) year.OtherOnes.ToList<OtherOne>());
      }
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcFieldOne);
      if (year.RecordOtherTwo)
      {
        if (!string.IsNullOrEmpty(year.OtherTwo))
          this.dcFieldTwo.HeaderText = year.OtherTwo;
        this.dcFieldTwo.BindTo<OtherTwo>((System.Linq.Expressions.Expression<Func<OtherTwo, object>>) (f2 => f2.Description), (System.Linq.Expressions.Expression<Func<OtherTwo, object>>) (f2 => f2.Self), (object) year.OtherTwos.ToList<OtherTwo>());
      }
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcFieldTwo);
      if (year.RecordOtherThree)
      {
        if (!string.IsNullOrEmpty(year.OtherThree))
          this.dcFieldThree.HeaderText = year.OtherThree;
        this.dcFieldThree.BindTo<OtherThree>((System.Linq.Expressions.Expression<Func<OtherThree, object>>) (f3 => f3.Description), (System.Linq.Expressions.Expression<Func<OtherThree, object>>) (f3 => f3.Self), (object) year.OtherThrees.ToList<OtherThree>());
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
        this.dcLocSite.BindTo<LocSite>((System.Linq.Expressions.Expression<Func<LocSite, object>>) (ls => ls.Description), (System.Linq.Expressions.Expression<Func<LocSite, object>>) (ls => ls.Self), (object) year.LocSites.ToList<LocSite>());
      else
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcLocSite);
      if (!year.RecordLocNo)
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcLocNo);
      if (!year.RecordTreeGPS)
      {
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcLatitude);
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcLongitude);
      }
      if (!year.RecordNoteTree)
        this.dgTrees.Columns.Remove((DataGridViewColumn) this.dcNoteTree);
      this.pnlEditHelp.Visible = this.Year.Changed;
      DataBindingList<FlatTreeView> dataBindingList = new DataBindingList<FlatTreeView>((IList<FlatTreeView>) this.m_trees);
      dataBindingList.Sortable = true;
      dataBindingList.AddComparer<TreeStatus>((IComparer) new AttributePropertyComparer<TreeStatus, DescriptionAttribute>("Description"));
      dataBindingList.AddComparer<CrownLightExposure>((IComparer) new AttributePropertyComparer<CrownLightExposure, DescriptionAttribute>("Description"));
      dataBindingList.AddComparer<Plot>((IComparer) new PropertyComparer<Plot>((Func<Plot, object>) (p => (object) p.Id)));
      dataBindingList.AddComparer<Strata>((IComparer) new PropertyComparer<Strata>((Func<Strata, object>) (st => (object) st.Description)));
      dataBindingList.AddComparer<PlotLandUse>((IComparer) new ChildComparer<PlotLandUse, LandUse>((Func<PlotLandUse, LandUse>) (plu => plu.LandUse), (IComparer<LandUse>) new PropertyComparer<LandUse>((Func<LandUse, object>) (lu => (object) lu.Description))));
      dataBindingList.AddComparer<Condition>((IComparer) new PropertyComparer<Condition>((Func<Condition, object>) (c => (object) c.PctDieback)));
      dataBindingList.AddComparer<Street>((IComparer) new PropertyComparer<Street>((Func<Street, object>) (st => (object) st.Name)));
      dataBindingList.AddComparer<Eco.Domain.v6.Lookup>((IComparer) new PropertyComparer<Eco.Domain.v6.Lookup>((Func<Eco.Domain.v6.Lookup, object>) (lu => (object) lu.Description)));
      this.dgTrees.DataSource = (object) dataBindingList;
      this.dgTrees.ResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells);
      dataBindingList.AddingNew += new AddingNewEventHandler(this.Trees_AddingNew);
      dataBindingList.BeforeRemove += new EventHandler<ListChangedEventArgs>(this.Trees_BeforeRemove);
      dataBindingList.ListChanged += new ListChangedEventHandler(this.Trees_ListChanged);
      this.OnRequestRefresh();
    }

    private void Tree_Updated(object sender, EntityUpdated<Tree> e)
    {
      if (this.Session == null)
        return;
      this.TaskManager.Add(Task.Factory.StartNew((System.Action) (() =>
      {
        lock (this.Session)
        {
          using (ITransaction transaction = this.Session.BeginTransaction())
          {
            Tree proxy = this.Session.Load<Tree>((object) e.Guid);
            if (NHibernateUtil.IsInitialized((object) proxy))
              this.Session.Refresh((object) proxy, LockMode.None);
            transaction.Commit();
          }
        }
      }), CancellationToken.None, TaskCreationOptions.None, Program.Session.Scheduler));
    }

    private void BindIPEDLookupColumn<T>(DataGridViewComboBoxColumn col, IList<T> dataSource) where T : IPED.Domain.Lookup => col.BindTo<T>((System.Linq.Expressions.Expression<Func<T, object>>) (lu => lu.Description), (System.Linq.Expressions.Expression<Func<T, object>>) (lu => (object) lu.Code), (object) dataSource);

    protected override void OnRequestRefresh()
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new System.Action(((ContentForm) this).OnRequestRefresh));
      }
      else
      {
        Year year = this.Year;
        bool flag = year != null && year.Changed && this.dgTrees.DataSource != null && year.Plots.Count > 0;
        this.dgTrees.ReadOnly = !flag;
        this.dgTrees.AllowUserToAddRows = flag;
        this.dgTrees.AllowUserToDeleteRows = flag;
        base.OnRequestRefresh();
      }
    }

    private void Trees_ListChanged(object sender, ListChangedEventArgs e) => this.OnRequestRefresh();

    private void Trees_BeforeRemove(object sender, ListChangedEventArgs e)
    {
      DataBindingList<FlatTreeView> dataBindingList = sender as DataBindingList<FlatTreeView>;
      if (e.NewIndex >= dataBindingList.Count)
        return;
      FlatTreeView flatTreeView = dataBindingList[e.NewIndex];
      if (flatTreeView.Tree.IsTransient)
        return;
      lock (this.Session)
      {
        using (ITransaction transaction = this.Session.BeginTransaction())
        {
          flatTreeView.Plot.Trees.Remove(flatTreeView.Tree);
          this.Session.Delete((object) flatTreeView.Tree);
          transaction.Commit();
        }
      }
    }

    private void Trees_AddingNew(object sender, AddingNewEventArgs e)
    {
      MgmtStyleEnum? mgmtStyle = this.Year.MgmtStyle;
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
      FlatTreeView flatTreeView = new FlatTreeView(this.Session, new Tree()
      {
        CityManaged = nullable,
        StreetTree = this.Year.RecordStreetTree && this.Year.DefaultStreetTree,
        Crown = new Crown() { Condition = condition1 }
      });
      e.NewObject = (object) flatTreeView;
    }

    public bool CanPerformAction(UserActions action)
    {
      IList<Plot> dataSource = this.dcPlot.DataSource as IList<Plot>;
      bool flag1 = this.Year != null && this.Year.Changed && this.dgTrees.DataSource != null && dataSource != null && dataSource.Count > 0;
      bool flag2 = flag1 && this.dgTrees.AllowUserToAddRows;
      bool flag3 = this.dgTrees.SelectedRows.Count > 0;
      bool flag4 = this.dgTrees.CurrentRow != null && this.dgTrees.IsCurrentRowDirty;
      bool flag5 = this.dgTrees.CurrentRow != null && this.dgTrees.CurrentRow.IsNewRow;
      switch (action)
      {
        case UserActions.New:
          return flag2 && !flag5 && !flag4;
        case UserActions.Copy:
          return false;
        case UserActions.Undo:
          return flag1 && this.m_dgManager.CanUndo;
        case UserActions.Redo:
          return flag1 && this.m_dgManager.CanRedo;
        case UserActions.Delete:
          return flag1 & flag3 && !flag5 | flag4;
        case UserActions.ImportData:
          return flag2 && !flag5 && !flag4;
        default:
          return false;
      }
    }

    public void PerformAction(UserActions action)
    {
      switch (action)
      {
        case UserActions.New:
          if (!this.dgTrees.AllowUserToAddRows)
            break;
          foreach (DataGridViewBand selectedRow in (BaseCollection) this.dgTrees.SelectedRows)
            selectedRow.Selected = false;
          this.dgTrees.Rows[this.dgTrees.NewRowIndex].Selected = true;
          this.dgTrees.FirstDisplayedScrollingRowIndex = this.dgTrees.NewRowIndex - this.dgTrees.DisplayedRowCount(false) + 1;
          this.dgTrees.CurrentCell = this.dgTrees.Rows[this.dgTrees.NewRowIndex].Cells[0];
          break;
        case UserActions.Copy:
          if (this.dgTrees.SelectedRows.Count <= 0)
            break;
          DataGridViewRow selectedRow1 = this.dgTrees.SelectedRows[0];
          if (!(this.dgTrees.DataSource is DataBindingList<FlatTreeView> dataSource) || selectedRow1.Index >= dataSource.Count)
            break;
          FlatTreeView flatTreeView = dataSource[selectedRow1.Index].Clone() as FlatTreeView;
          int num = 1;
          foreach (Tree tree in (IEnumerable<Tree>) flatTreeView.Tree.Plot.Trees)
          {
            if (tree.Id >= num)
              num = tree.Id + 1;
          }
          flatTreeView.Id = num;
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
          if (this.dgTrees.SelectedRows.Count <= 0)
            break;
          this.DeleteSelectedRows();
          break;
      }
    }

    private void dgTrees_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataBindingList<FlatTreeView> dataSource = this.dgTrees.DataSource as DataBindingList<FlatTreeView>;
      if (this.dgTrees.CurrentRow != null && !this.dgTrees.IsCurrentRowDirty || dataSource == null || e.RowIndex >= dataSource.Count)
        return;
      FlatTreeView flatTreeView = dataSource[e.RowIndex];
      DataGridViewRow row = this.dgTrees.Rows[e.RowIndex];
      DataGridViewCell dataGridViewCell1 = (DataGridViewCell) null;
      string text = (string) null;
      if (flatTreeView.Plot != null)
      {
        foreach (Tree tree in (IEnumerable<Tree>) flatTreeView.Plot.Trees)
        {
          if (!tree.Equals((object) flatTreeView.Tree) && tree.Id == flatTreeView.Id)
          {
            text = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldUnique, (object) this.dcId.HeaderText);
            dataGridViewCell1 = row.Cells[this.dcId.DisplayIndex];
            break;
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

    private void dgTrees_RowValidated(object sender, DataGridViewCellEventArgs e)
    {
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
          if (flatTreeView.Tree.IsTransient || this.Session.IsDirty())
          {
            lock (this.Session)
            {
              using (ITransaction transaction = this.Session.BeginTransaction())
              {
                this.Session.SaveOrUpdate((object) flatTreeView.Tree);
                transaction.Commit();
              }
            }
          }
        }
        this.OnRequestRefresh();
      }
    }

    private void dgTrees_CurrentCellDirtyStateChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void dgTrees_SelectionChanged(object sender, EventArgs e) => this.OnRequestRefresh();

    private void DeleteSelectedRows()
    {
      if (!(this.dgTrees.DataSource is DataBindingList<FlatTreeView> dataSource))
        return;
      CurrencyManager currencyManager = this.dgTrees.BindingContext[(object) dataSource] as CurrencyManager;
      foreach (DataGridViewRow selectedRow in (BaseCollection) this.dgTrees.SelectedRows)
      {
        if (selectedRow.Index < dataSource.Count)
          dataSource.RemoveAt(selectedRow.Index);
      }
      if (currencyManager.Position == -1)
        return;
      this.dgTrees.Rows[currencyManager.Position].Selected = true;
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

    private void UpdatePlotInfo(int column, int row)
    {
      if (!(this.dgTrees.DataSource is DataBindingList<FlatTreeView> dataSource) || this.dgTrees.Columns.IndexOf((DataGridViewColumn) this.dcPlot) != column || row >= dataSource.Count)
        return;
      FlatTreeView flatTreeView = dataSource[row];
      if (flatTreeView.PlotLandUse != null && !flatTreeView.PlotLandUse.Plot.Equals((object) flatTreeView.Plot))
      {
        LandUse landUse = flatTreeView.PlotLandUse.LandUse;
        PlotLandUse plotLandUse1 = (PlotLandUse) null;
        foreach (PlotLandUse plotLandUse2 in (IEnumerable<PlotLandUse>) flatTreeView.Plot.PlotLandUses)
        {
          if (plotLandUse2.LandUse.Equals((object) landUse))
          {
            plotLandUse1 = plotLandUse2;
            break;
          }
        }
        flatTreeView.PlotLandUse = plotLandUse1;
      }
      if (flatTreeView.Plot == null)
        return;
      if (!flatTreeView.SurveyDate.HasValue && flatTreeView.Plot.Date.HasValue)
        flatTreeView.SurveyDate = flatTreeView.Plot.Date;
      if (flatTreeView.Id != 0)
        return;
      int num = 1;
      foreach (Tree tree in (IEnumerable<Tree>) flatTreeView.Plot.Trees)
      {
        if (tree.Id >= num)
          num = tree.Id + 1;
      }
      flatTreeView.Id = num;
    }

    private void dgTrees_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      this.UpdateDBHHeights(e.ColumnIndex, e.RowIndex);
      this.UpdatePlotInfo(e.ColumnIndex, e.RowIndex);
      if (e.ColumnIndex != this.dcCrownCondition.DisplayIndex && e.ColumnIndex != this.dcCrownDieback.DisplayIndex)
        return;
      this.dgTrees.Refresh();
    }

    private void dgTrees_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      if (this.dgTrees.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn != this.dcPlot || e.Value == null || !(this.dgTrees.DataSource is DataBindingList<FlatTreeView> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      Plot plot = dataSource[e.RowIndex].Plot;
      if (plot == null)
        return;
      string str = string.Format("{0} {1}: {2} {3}", (object) v6Strings.Plot_SingularName, (object) plot.Id, (object) plot.Trees.Count, plot.Trees.Count > 1 ? (object) v6Strings.Tree_PluralName : (object) v6Strings.Tree_SingularName);
      if (this.Year.RecordShrub)
        str += string.Format("; {0} {1}", (object) plot.Shrubs.Count, plot.Shrubs.Count > 1 ? (object) v6Strings.Shrub_PluralName : (object) v6Strings.Shrub_SingularName);
      this.dgTrees.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = str;
    }

    private void dgTrees_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

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
          return this.dgTrees.DataSource != null && this.Year != null && this.Year.RecordTreeGPS;
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

    private void dgTrees_EditingControlShowing(
      object sender,
      DataGridViewEditingControlShowingEventArgs e)
    {
      DataGridViewComboBoxCell currentCell = this.dgTrees.CurrentCell as DataGridViewComboBoxCell;
      DataBindingList<FlatTreeView> dataSource = this.dgTrees.DataSource as DataBindingList<FlatTreeView>;
      if (currentCell != null && dataSource != null && this.dcLandUse.Equals((object) this.dgTrees.Columns[currentCell.ColumnIndex]))
      {
        FlatTreeView flatTreeView = dataSource[currentCell.RowIndex];
        if (flatTreeView.Plot == null)
        {
          currentCell.DataSource = (object) null;
        }
        else
        {
          List<KeyValuePair<PlotLandUse, string>> keyValuePairList = new List<KeyValuePair<PlotLandUse, string>>();
          foreach (KeyValuePair<PlotLandUse, string> plotLandUse in this.m_PlotLandUses)
          {
            if (plotLandUse.Key.Plot.Equals((object) flatTreeView.Plot))
              keyValuePairList.Add(plotLandUse);
          }
          currentCell.DisplayMember = "Value";
          currentCell.ValueMember = "Key";
          currentCell.DataSource = (object) keyValuePairList;
          e.CellStyle.BackColor = this.dgTrees.DefaultCellStyle.BackColor;
        }
      }
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

    private void dgTrees_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      DataGridViewComboBoxCell cell = this.dgTrees.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
      DataGridViewColumn column = this.dgTrees.Columns[e.ColumnIndex];
      if (cell == null || !this.dcLandUse.Equals((object) column))
        return;
      cell.DataSource = this.dcLandUse.DataSource;
    }

    private void dgTrees_CellErrorTextNeeded(
      object sender,
      DataGridViewCellErrorTextNeededEventArgs e)
    {
      if (e.RowIndex == this.dgTrees.NewRowIndex || !(this.dgTrees.DataSource is DataBindingList<FlatTreeView> dataSource) || e.RowIndex >= dataSource.Count)
        return;
      DataGridViewColumn column = this.dgTrees.Columns[e.ColumnIndex];
      FlatTreeView flatTreeView = dataSource[e.RowIndex];
      if (column == this.dcPlot)
      {
        if (flatTreeView.Plot != null)
          return;
        e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldRequired, (object) column.HeaderText);
      }
      else if (column == this.dcId)
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
          if (Program.Session.InvalidSpecies.TryGetValue(flatTreeView.Species, out speciesView))
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
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) column.HeaderText, (object) 0);
        else if (this.Year.RecordHeight && (double) flatTreeView.Height > 0.0 && (double) flatTreeView.Height <= 450.0 && (double) flatTreeView.CrownTopHeight > (double) flatTreeView.Height)
        {
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) flatTreeView.Height.ToString("F2"));
        }
        else
        {
          if ((double) flatTreeView.CrownTopHeight <= 450.0)
            return;
          e.ErrorText = string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) column.HeaderText, (object) 450);
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
        else if (this.Year.RecordHeight && (double) flatTreeView.Height > 0.0 && (double) flatTreeView.Height <= 450.0 && (double) flatTreeView.CrownBaseHeight > (double) flatTreeView.Height)
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
        float num = this.Year.Unit == YearUnit.English ? 60f : 18.29f;
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
        float num = this.Year.Unit == YearUnit.English ? 60f : 18.29f;
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
        float num = this.Year.Unit == YearUnit.English ? 60f : 18.29f;
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

    public Eco.Util.ImportSpec ImportSpec()
    {
      Eco.Util.ImportSpec<FlatTreeView> importSpec1 = new Eco.Util.ImportSpec<FlatTreeView>();
      importSpec1.RecordCount = this.m_trees.Count;
      Eco.Util.ImportSpec importSpec2 = (Eco.Util.ImportSpec) importSpec1;
      List<FieldSpec> fieldsSpecs = importSpec2.FieldsSpecs;
      fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.Plot), this.dcPlot.HeaderText, true).AddFormat((FieldFormat) new FieldFormat<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id))).SetData(this.dcPlot.DataSource, TypeHelper.NameOf<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Self))));
      if (this.Year.RecordTreeUserId)
        fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.UserId), this.dcUserId.HeaderText, false));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.SurveyDate), this.dcSurveyDate.HeaderText, false));
      if (this.Year.RecordTreeStatus)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.Status), this.dcStatus.HeaderText, false).AddFormat(new FieldFormat(typeof (TreeStatus), (string) null, "Value")).SetData(this.dcStatus.DataSource, "Key"));
      if (this.Year.RecordPlotCenter)
      {
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.Distance), this.dcDistance.HeaderText, false).SetNullValue((object) -1f));
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.Direction), this.dcDirection.HeaderText, false).SetNullValue((object) -1));
      }
      fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.Species), this.dcSpecies.HeaderText, true).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (s => s.CommonName))).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (s => s.ScientificName))).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (s => s.CommonScientificName))).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (s => s.ScientificCommonName))).AddFormat((FieldFormat) new FieldFormat<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (s => s.Code))).SetData(this.dcSpecies.DataSource, TypeHelper.NameOf<SpeciesView>((System.Linq.Expressions.Expression<Func<SpeciesView, object>>) (s => s.Code))));
      FieldConstraint constraint1 = new FieldConstraint((AConstraint) new OrConstraint((AConstraint) new EqConstraint((object) -1.0), (AConstraint) new GtEqConstraint((object) 0.5)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) "{0}", (object) 0.5));
      FieldConstraint constraint2 = new FieldConstraint((AConstraint) new OrConstraint((AConstraint) new EqConstraint((object) -1.0), (AConstraint) new GtConstraint((object) 0.0)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) "{0}", (object) 0.0));
      FieldConstraint constraint3 = new FieldConstraint((AConstraint) new LtEqConstraint((object) 6.0), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) "{0}", (object) 6.0));
      FieldConstraint constraint4 = new FieldConstraint((AConstraint) new OrConstraint((AConstraint) new EqConstraint((object) -1.0), (AConstraint) new GtEqConstraint((object) 0.0)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) "{0}", (object) 0.0f));
      FieldConstraint constraint5 = new FieldConstraint((AConstraint) new LtEqConstraint((object) 450.0), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) "{0}", (object) 450f));
      if (this.Year.DBHActual)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH1), this.dcDBH1.HeaderText, true).SetNullValue((object) -1.0).AddConstraint(constraint1));
      else
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH1), this.dcDBH1c.HeaderText, true).SetNullValue((object) -1.0).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Id))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => d.Description))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Value))).SetData((object) this.Year.DBHs.ToList<DBH>(), TypeHelper.NameOf<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.DBHId))));
      fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH1Height), this.dcDBH1Height.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint2).AddConstraint(constraint3));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH1Measured), this.dcDBH1Measured.HeaderText, false));
      if (this.Year.DBHActual)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH2), this.dcDBH2.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint1));
      else
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH2), this.dcDBH2c.HeaderText, false).SetNullValue((object) -1.0).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Id))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => d.Description))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Value))).SetData((object) this.Year.DBHs.ToList<DBH>(), TypeHelper.NameOf<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.DBHId))));
      fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH2Height), this.dcDBH2Height.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint2).AddConstraint(constraint3));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH2Measured), this.dcDBH2Measured.HeaderText, false));
      if (this.Year.DBHActual)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH3), this.dcDBH3.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint1));
      else
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH3), this.dcDBH3c.HeaderText, false).SetNullValue((object) -1.0).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Id))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => d.Description))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Value))).SetData((object) this.Year.DBHs.ToList<DBH>(), TypeHelper.NameOf<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.DBHId))));
      fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH3Height), this.dcDBH3Height.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint2).AddConstraint(constraint3));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH3Measured), this.dcDBH3Measured.HeaderText, false));
      if (this.Year.DBHActual)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH4), this.dcDBH4.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint1));
      else
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH4), this.dcDBH4c.HeaderText, false).SetNullValue((object) -1.0).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Id))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => d.Description))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Value))).SetData((object) this.Year.DBHs.ToList<DBH>(), TypeHelper.NameOf<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.DBHId))));
      fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH4Height), this.dcDBH4Height.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint2).AddConstraint(constraint3));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH4Measured), this.dcDBH4Measured.HeaderText, false));
      if (this.Year.DBHActual)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH5), this.dcDBH5.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint1));
      else
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH5), this.dcDBH5c.HeaderText, false).SetNullValue((object) -1.0).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Id))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => d.Description))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Value))).SetData((object) this.Year.DBHs.ToList<DBH>(), TypeHelper.NameOf<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.DBHId))));
      fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH5Height), this.dcDBH5Height.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint2).AddConstraint(constraint3));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH5Measured), this.dcDBH5Measured.HeaderText, false));
      if (this.Year.DBHActual)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH6), this.dcDBH6.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint1));
      else
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH6), this.dcDBH6c.HeaderText, false).SetNullValue((object) -1.0).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Id))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => d.Description))).AddFormat((FieldFormat) new FieldFormat<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.Value))).SetData((object) this.Year.DBHs.ToList<DBH>(), TypeHelper.NameOf<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => (object) d.DBHId))));
      fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH6Height), this.dcDBH6Height.HeaderText, false).SetNullValue((object) -1.0).AddConstraint(constraint2).AddConstraint(constraint3));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.DBH6Measured), this.dcDBH6Measured.HeaderText, false));
      if (this.Year.RecordCrownCondition)
      {
        if (this.Year.DisplayCondition)
          fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.CrownCondition), this.dcCrownCondition.HeaderText, false).AddFormat((FieldFormat) new FieldFormat<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => (object) c.Id))).AddFormat((FieldFormat) new FieldFormat<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Description))).AddFormat((FieldFormat) new FieldFormat<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => (object) c.Percent))).SetData((object) this.Year.Conditions.ToList<Condition>(), TypeHelper.NameOf<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Self))));
        else
          fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.CrownCondition), this.dcCrownDieback.HeaderText, false).AddFormat((FieldFormat) new FieldFormat<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => (object) c.Id))).AddFormat((FieldFormat) new FieldFormat<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.DiebackDesc))).AddFormat((FieldFormat) new FieldFormat<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => (object) c.PctDieback))).SetData((object) this.Year.Conditions.ToList<Condition>(), TypeHelper.NameOf<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => c.Self))));
      }
      if (this.Year.RecordHeight)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.Height), this.dcTreeHeight.HeaderText, false).AddConstraint(constraint4).AddConstraint(constraint5));
      if (this.Year.RecordCrownSize)
      {
        FieldConstraint constraint6 = new FieldConstraint((AConstraint) new OrConstraint((AConstraint) new EqConstraint((object) -1f), (AConstraint) new GtEqConstraint((object) 0.0f)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) "{0}", (object) 0.0f));
        FieldConstraint constraint7 = new FieldConstraint((AConstraint) new LtEqConstraint((object) 300f), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) "{0}", (object) 300f));
        fieldsSpecs.AddRange((IEnumerable<FieldSpec>) new FieldSpec[5]
        {
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.CrownTopHeight), this.dcCrownTopHeight.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint4).AddConstraint(constraint5),
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.CrownBaseHeight), this.dcCrownBaseHeight.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint4).AddConstraint(constraint5),
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.CrownWidthNS), this.dcCrownWidthNS.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint6).AddConstraint(constraint7),
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.CrownWidthEW), this.dcCrownWidthEW.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint6).AddConstraint(constraint7),
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.CrownPercentMissing), this.dcCrownPercentMissing.HeaderText, false).SetNullValue((object) PctMidRange.PRINV)
        });
      }
      if (this.Year.RecordHydro)
        fieldsSpecs.AddRange((IEnumerable<FieldSpec>) new FieldSpec[2]
        {
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.PercentImpervious), this.dcPctImpervious.HeaderText, false).SetNullValue((object) PctMidRange.PRINV),
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.PercentShrub), this.dcPctShrub.HeaderText, false).SetNullValue((object) PctMidRange.PRINV)
        });
      if (this.Year.RecordCLE)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.CrownLightExposure), this.dcCrownLightExposure.HeaderText, false).SetNullValue((object) CrownLightExposure.NotEntered));
      if (this.Year.RecordEnergy)
      {
        FieldConstraint constraint8 = new FieldConstraint((AConstraint) new OrConstraint((AConstraint) new EqConstraint((object) -1f), (AConstraint) new GtEqConstraint((object) 0.0f)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGtEq, (object) "{0}", (object) 0.0f));
        FieldConstraint constraint9 = new FieldConstraint((AConstraint) new LtEqConstraint((object) 360f), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldLtEq, (object) "{0}", (object) 360f));
        FieldConstraint constraint10 = new FieldConstraint((AConstraint) new OrConstraint((AConstraint) new EqConstraint((object) -1f), (AConstraint) new GtConstraint((object) 0.0f)), string.Format(i_Tree_Eco_v6.Resources.Strings.ErrFieldGt, (object) "{0}", (object) 0.0f));
        fieldsSpecs.AddRange((IEnumerable<FieldSpec>) new FieldSpec[8]
        {
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.B1Direction), this.dcBldg1Direction.HeaderText, false).SetNullValue((object) (short) -1).AddConstraint(constraint8).AddConstraint(constraint9),
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.B1Distance), this.dcBldg1Distance.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint10),
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.B2Direction), this.dcBldg2Direction.HeaderText, false).SetNullValue((object) (short) -1).AddConstraint(constraint8).AddConstraint(constraint9),
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.B2Distance), this.dcBldg2Distance.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint10),
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.B3Direction), this.dcBldg3Direction.HeaderText, false).SetNullValue((object) (short) -1).AddConstraint(constraint8).AddConstraint(constraint9),
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.B3Distance), this.dcBldg3Distance.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint10),
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.B4Direction), this.dcBldg4Direction.HeaderText, false).SetNullValue((object) (short) -1).AddConstraint(constraint8).AddConstraint(constraint9),
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.B4Distance), this.dcBldg4Distance.HeaderText, false).SetNullValue((object) -1f).AddConstraint(constraint10)
        });
      }
      if (this.Year.RecordSiteType)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.SiteType), this.dcSiteType.HeaderText, false).AddFormat((FieldFormat) new FieldFormat<SiteType>((System.Linq.Expressions.Expression<Func<SiteType, object>>) (st => (object) st.Id))).AddFormat((FieldFormat) new FieldFormat<SiteType>((System.Linq.Expressions.Expression<Func<SiteType, object>>) (st => st.Description))).SetData(this.dcSiteType.DataSource, TypeHelper.NameOf<SiteType>((System.Linq.Expressions.Expression<Func<SiteType, object>>) (st => st.Self))));
      if (this.Year.RecordStreetTree)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.StreetTree), this.dcStTree.HeaderText, false).SetDefaultValue((object) this.Year.DefaultStreetTree));
      if (this.Year.RecordMaintRec)
        fieldsSpecs.Add(this.CreateLookupField<FlatTreeView, MaintRec>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.MaintRec), this.dcMaintRec));
      if (this.Year.RecordMaintTask)
        fieldsSpecs.Add(this.CreateLookupField<FlatTreeView, MaintTask>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.MaintTask), this.dcMaintTask));
      if (this.Year.RecordSidewalk)
        fieldsSpecs.Add(this.CreateLookupField<FlatTreeView, Sidewalk>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.SidewalkDamage), this.dcSidewalk));
      if (this.Year.RecordWireConflict)
        fieldsSpecs.Add(this.CreateLookupField<FlatTreeView, WireConflict>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.WireConflict), this.dcWireConflict));
      if (this.Year.RecordOtherOne)
        fieldsSpecs.Add(this.CreateLookupField<FlatTreeView, OtherOne>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.OtherOne), this.dcFieldOne));
      if (this.Year.RecordOtherTwo)
        fieldsSpecs.Add(this.CreateLookupField<FlatTreeView, OtherTwo>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.OtherTwo), this.dcFieldTwo));
      if (this.Year.RecordOtherThree)
        fieldsSpecs.Add(this.CreateLookupField<FlatTreeView, OtherThree>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.OtherThree), this.dcFieldThree));
      if (this.Year.RecordIPED)
        fieldsSpecs.AddRange((IEnumerable<FieldSpec>) new FieldSpec[19]
        {
          this.CreateIPEDField<FlatTreeView, TSDieback>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDTSDieback), this.dcIPEDTSDieback),
          this.CreateIPEDField<FlatTreeView, TSEpiSprout>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDTSEpiSprout), this.dcIPEDTSEpiSprout),
          this.CreateIPEDField<FlatTreeView, TSWiltFoli>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDTSWiltFoli), this.dcIPEDTSWiltFoli),
          this.CreateIPEDField<FlatTreeView, TSEnvStress>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDTSEnvStress), this.dcIPEDTSEnvStress),
          this.CreateIPEDField<FlatTreeView, TSHumStress>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDTSHumStress), this.dcIPEDTSHumStress),
          (FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.IPEDTSNotes), this.dcIPEDTSNotes.HeaderText, false),
          this.CreateIPEDField<FlatTreeView, FTChewFoli>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDFTChewFoli), this.dcIPEDFTChewFoli),
          this.CreateIPEDField<FlatTreeView, FTDiscFoli>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDFTDiscFoli), this.dcIPEDFTDiscFoli),
          this.CreateIPEDField<FlatTreeView, FTAbnFoli>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDFTAbnFoli), this.dcIPEDFTAbnFoli),
          this.CreateIPEDField<FlatTreeView, FTInsectSigns>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDFTInsectSigns), this.dcIPEDFTInsectSigns),
          this.CreateIPEDField<FlatTreeView, FTFoliAffect>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDFTFoliAffect), this.dcIPEDFTFoliAffect),
          (FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.IPEDFTNotes), this.dcIPEDFTNotes.HeaderText, false),
          this.CreateIPEDField<FlatTreeView, BBInsectSigns>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDBBInsectSigns), this.dcIPEDBBInsectSigns),
          this.CreateIPEDField<FlatTreeView, BBInsectPres>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDBBInsectPres), this.dcIPEDBBInsectPres),
          this.CreateIPEDField<FlatTreeView, BBDiseaseSigns>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDBBDiseaseSigns), this.dcIPEDBBDiseaseSigns),
          this.CreateIPEDField<FlatTreeView, BBProbLoc>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDBBProbLoc), this.dcIPEDBBProbLoc),
          this.CreateIPEDField<FlatTreeView, BBAbnGrowth>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDBBAbnGrowth), this.dcIPEDBBAbnGrowth),
          (FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.IPEDBBNotes), this.dcIPEDBBNotes.HeaderText, false),
          new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.IPEDPest), this.dcIPEDPest.HeaderText, false).AddFormat((FieldFormat) new FieldFormat<Pest>((System.Linq.Expressions.Expression<Func<Pest, object>>) (p => p.ScientificName))).AddFormat((FieldFormat) new FieldFormat<Pest>((System.Linq.Expressions.Expression<Func<Pest, object>>) (p => p.CommonName))).AddFormat((FieldFormat) new FieldFormat<Pest>((System.Linq.Expressions.Expression<Func<Pest, object>>) (p => (object) p.Id))).SetData(this.dcIPEDPest.DataSource, TypeHelper.NameOf<Pest>((System.Linq.Expressions.Expression<Func<Pest, object>>) (p => (object) p.Id)))
        });
      if (this.Year.MgmtStyle.HasValue && (this.Year.MgmtStyle.Value & MgmtStyleEnum.RecordPublicPrivate) != MgmtStyleEnum.DefaultPublic)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.CityManaged), this.dcCityManaged.HeaderText, false).SetDefaultValue((object) ((this.Year.MgmtStyle.Value & MgmtStyleEnum.DefaultPrivate) == MgmtStyleEnum.DefaultPublic)));
      if (this.Year.RecordTreeStreet)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.Street), this.dcStreet.HeaderText, false).AddFormat((FieldFormat) new FieldFormat<Street>((System.Linq.Expressions.Expression<Func<Street, object>>) (st => st.Name))).SetData(this.dcStreet.DataSource, TypeHelper.NameOf<Street>((System.Linq.Expressions.Expression<Func<Street, object>>) (st => st.Self))));
      if (this.Year.RecordTreeAddress)
        fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.Address), this.dcAddress.HeaderText, false));
      if (this.Year.RecordLocSite)
        fieldsSpecs.Add(new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.LocSite), this.dcLocSite.HeaderText, false).AddFormat((FieldFormat) new FieldFormat<LocSite>((System.Linq.Expressions.Expression<Func<LocSite, object>>) (ls => (object) ls.Id))).AddFormat((FieldFormat) new FieldFormat<LocSite>((System.Linq.Expressions.Expression<Func<LocSite, object>>) (ls => ls.Description))).SetData(this.dcLocSite.DataSource, TypeHelper.NameOf<LocSite>((System.Linq.Expressions.Expression<Func<LocSite, object>>) (ls => ls.Self))));
      if (this.Year.RecordLocNo)
        fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.LocNo), this.dcLocNo.HeaderText, false));
      if (this.Year.RecordTreeGPS)
        fieldsSpecs.AddRange((IEnumerable<FieldSpec>) new FieldSpec[2]
        {
          (FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.Latitude), this.dcLatitude.HeaderText, false),
          (FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.Longitude), this.dcLongitude.HeaderText, false)
        });
      if (this.Year.RecordNoteTree)
        fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => (object) ftv.NoteThisTree), this.dcNoteTree.HeaderText, false));
      fieldsSpecs.Add((FieldSpec) new FieldSpec<FlatTreeView>((System.Linq.Expressions.Expression<Func<FlatTreeView, object>>) (ftv => ftv.Comments), this.dcComments.HeaderText, false));
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
      return Task.Factory.StartNew<IList<FlatTreeView>>((Func<IList<FlatTreeView>>) (() => this.DoImport(data, progress, ct)), ct, TaskCreationOptions.None, Program.Session.Scheduler).ContinueWith((System.Action<Task<IList<FlatTreeView>>>) (t =>
      {
        if (this.Year == null || t.IsCanceled || t.IsFaulted)
          return;
        IList<FlatTreeView> result = t.Result;
        DataBindingList<FlatTreeView> dataSource = this.dgTrees.DataSource as DataBindingList<FlatTreeView>;
        foreach (FlatTreeView flatTreeView in (IEnumerable<FlatTreeView>) result)
          dataSource.Add(flatTreeView);
      }), scheduler);
    }

    private IList<FlatTreeView> DoImport(
      IList data,
      IProgress<ProgressEventArgs> progress,
      CancellationToken ct)
    {
      IList<FlatTreeView> flatTreeViewList = (IList<FlatTreeView>) new List<FlatTreeView>();
      Dictionary<Plot, int> dictionary = new Dictionary<Plot, int>();
      ISet<Condition> conditions = this.Year.Conditions;
      Condition condition1 = (Condition) null;
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
      lock (this.Session)
      {
        int count = data.Count;
        int num1 = Math.Max(Math.Min(count / 100, 1000), 1);
        int num2 = 0;
        ITransaction transaction1 = this.Session.BeginTransaction();
        IList<Guid> guidList = (IList<Guid>) new List<Guid>();
        try
        {
          foreach (object obj in (IEnumerable) data)
          {
            ct.ThrowIfCancellationRequested();
            if (obj is FlatTreeView flatTreeView)
            {
              flatTreeView.Session = this.Session;
              int num3;
              if (dictionary.TryGetValue(flatTreeView.Plot, out num3))
                ++num3;
              else
                num3 = this.NextId(flatTreeView.Plot);
              dictionary[flatTreeView.Plot] = num3;
              flatTreeView.Id = num3;
              if (flatTreeView.CrownCondition == null)
                flatTreeView.CrownCondition = condition1;
              flatTreeView.Plot.Trees.Add(flatTreeView.Tree);
              this.Session.Persist((object) flatTreeView.Tree);
              flatTreeViewList.Add(flatTreeView);
              guidList.Add(flatTreeView.Tree.Guid);
              ++num2;
              if (num2 % num1 == 0)
              {
                transaction1.Commit();
                transaction1.Dispose();
                this.Session.Flush();
                this.Session.Clear();
                transaction1 = this.Session.BeginTransaction();
                progress.Report(new ProgressEventArgs(i_Tree_Eco_v6.Resources.Strings.MsgImporting, count / num1, num2 / num1));
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
          for (int index = 0; index < num2 / num1 * num1; ++index)
          {
            this.Session.Delete((object) this.Session.Load<Tree>((object) guidList[index]));
            if (index % num1 == 0)
            {
              transaction2.Commit();
              transaction2.Dispose();
              this.Session.Flush();
              this.Session.Clear();
              transaction2 = this.Session.BeginTransaction();
              progress.Report(new ProgressEventArgs(i_Tree_Eco_v6.Resources.Strings.MsgCanceling, count / num1, (num2 - 1) / num1));
            }
          }
          transaction2.Commit();
          transaction2.Dispose();
        }
      }
      return flatTreeViewList;
    }

    private int NextId(Plot p) => (p.Trees.Any<Tree>() ? p.Trees.Max<Tree>((Func<Tree, int>) (tr => tr.Id)) : 0) + 1;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SampleTreesForm));
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
      this.dgTrees = new DataGridView();
      this.dcPlot = new DataGridViewComboBoxColumn();
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
      this.dcNoteTree = new DataGridViewCheckBoxColumn();
      this.dcComments = new DataGridViewTextBoxColumn();
      this.pnlEditHelp = new TableLayoutPanel();
      ((ISupportInitialize) this.dgTrees).BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.lblBreadcrumb, "lblBreadcrumb");
      this.dgTrees.AllowUserToAddRows = false;
      this.dgTrees.AllowUserToDeleteRows = false;
      this.dgTrees.AllowUserToResizeRows = false;
      this.dgTrees.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgTrees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgTrees.Columns.AddRange((DataGridViewColumn) this.dcPlot, (DataGridViewColumn) this.dcId, (DataGridViewColumn) this.dcUserId, (DataGridViewColumn) this.dcSurveyDate, (DataGridViewColumn) this.dcStatus, (DataGridViewColumn) this.dcDistance, (DataGridViewColumn) this.dcDirection, (DataGridViewColumn) this.dcSpecies, (DataGridViewColumn) this.dcLandUse, (DataGridViewColumn) this.dcDBH1, (DataGridViewColumn) this.dcDBH1c, (DataGridViewColumn) this.dcDBH1Height, (DataGridViewColumn) this.dcDBH1Measured, (DataGridViewColumn) this.dcDBH2, (DataGridViewColumn) this.dcDBH2c, (DataGridViewColumn) this.dcDBH2Height, (DataGridViewColumn) this.dcDBH2Measured, (DataGridViewColumn) this.dcDBH3, (DataGridViewColumn) this.dcDBH3c, (DataGridViewColumn) this.dcDBH3Height, (DataGridViewColumn) this.dcDBH3Measured, (DataGridViewColumn) this.dcDBH4, (DataGridViewColumn) this.dcDBH4c, (DataGridViewColumn) this.dcDBH4Height, (DataGridViewColumn) this.dcDBH4Measured, (DataGridViewColumn) this.dcDBH5, (DataGridViewColumn) this.dcDBH5c, (DataGridViewColumn) this.dcDBH5Height, (DataGridViewColumn) this.dcDBH5Measured, (DataGridViewColumn) this.dcDBH6, (DataGridViewColumn) this.dcDBH6c, (DataGridViewColumn) this.dcDBH6Height, (DataGridViewColumn) this.dcDBH6Measured, (DataGridViewColumn) this.dcCrownCondition, (DataGridViewColumn) this.dcCrownDieback, (DataGridViewColumn) this.dcTreeHeight, (DataGridViewColumn) this.dcCrownTopHeight, (DataGridViewColumn) this.dcCrownBaseHeight, (DataGridViewColumn) this.dcCrownWidthNS, (DataGridViewColumn) this.dcCrownWidthEW, (DataGridViewColumn) this.dcCrownPercentMissing, (DataGridViewColumn) this.dcPctImpervious, (DataGridViewColumn) this.dcPctShrub, (DataGridViewColumn) this.dcCrownLightExposure, (DataGridViewColumn) this.dcBldg1Direction, (DataGridViewColumn) this.dcBldg1Distance, (DataGridViewColumn) this.dcBldg2Direction, (DataGridViewColumn) this.dcBldg2Distance, (DataGridViewColumn) this.dcBldg3Direction, (DataGridViewColumn) this.dcBldg3Distance, (DataGridViewColumn) this.dcBldg4Direction, (DataGridViewColumn) this.dcBldg4Distance, (DataGridViewColumn) this.dcSiteType, (DataGridViewColumn) this.dcStTree, (DataGridViewColumn) this.dcMaintRec, (DataGridViewColumn) this.dcMaintTask, (DataGridViewColumn) this.dcSidewalk, (DataGridViewColumn) this.dcWireConflict, (DataGridViewColumn) this.dcFieldOne, (DataGridViewColumn) this.dcFieldTwo, (DataGridViewColumn) this.dcFieldThree, (DataGridViewColumn) this.dcIPEDTSDieback, (DataGridViewColumn) this.dcIPEDTSEpiSprout, (DataGridViewColumn) this.dcIPEDTSWiltFoli, (DataGridViewColumn) this.dcIPEDTSEnvStress, (DataGridViewColumn) this.dcIPEDTSHumStress, (DataGridViewColumn) this.dcIPEDTSNotes, (DataGridViewColumn) this.dcIPEDFTChewFoli, (DataGridViewColumn) this.dcIPEDFTDiscFoli, (DataGridViewColumn) this.dcIPEDFTAbnFoli, (DataGridViewColumn) this.dcIPEDFTInsectSigns, (DataGridViewColumn) this.dcIPEDFTFoliAffect, (DataGridViewColumn) this.dcIPEDFTNotes, (DataGridViewColumn) this.dcIPEDBBInsectSigns, (DataGridViewColumn) this.dcIPEDBBInsectPres, (DataGridViewColumn) this.dcIPEDBBDiseaseSigns, (DataGridViewColumn) this.dcIPEDBBProbLoc, (DataGridViewColumn) this.dcIPEDBBAbnGrowth, (DataGridViewColumn) this.dcIPEDBBNotes, (DataGridViewColumn) this.dcIPEDPest, (DataGridViewColumn) this.dcCityManaged, (DataGridViewColumn) this.dcStreet, (DataGridViewColumn) this.dcAddress, (DataGridViewColumn) this.dcLocSite, (DataGridViewColumn) this.dcLocNo, (DataGridViewColumn) this.dcLatitude, (DataGridViewColumn) this.dcLongitude, (DataGridViewColumn) this.dcNoteTree, (DataGridViewColumn) this.dcComments);
      componentResourceManager.ApplyResources((object) this.dgTrees, "dgTrees");
      this.dgTrees.EnableHeadersVisualStyles = false;
      this.dgTrees.Name = "dgTrees";
      this.dgTrees.ReadOnly = true;
      this.dgTrees.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      this.dgTrees.RowTemplate.DefaultCellStyle.BackColor = Color.White;
      this.dgTrees.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dgTrees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dgTrees.CellEndEdit += new DataGridViewCellEventHandler(this.dgTrees_CellEndEdit);
      this.dgTrees.CellErrorTextNeeded += new DataGridViewCellErrorTextNeededEventHandler(this.dgTrees_CellErrorTextNeeded);
      this.dgTrees.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgTrees_CellFormatting);
      this.dgTrees.CellParsing += new DataGridViewCellParsingEventHandler(this.dgTrees_CellParsing);
      this.dgTrees.CellValueChanged += new DataGridViewCellEventHandler(this.dgTrees_CellValueChanged);
      this.dgTrees.CurrentCellDirtyStateChanged += new EventHandler(this.dgTrees_CurrentCellDirtyStateChanged);
      this.dgTrees.DataError += new DataGridViewDataErrorEventHandler(this.dgTrees_DataError);
      this.dgTrees.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dgTrees_EditingControlShowing);
      this.dgTrees.RowValidated += new DataGridViewCellEventHandler(this.dgTrees_RowValidated);
      this.dgTrees.RowValidating += new DataGridViewCellCancelEventHandler(this.dgTrees_RowValidating);
      this.dgTrees.Scroll += new ScrollEventHandler(this.dgTrees_Scroll);
      this.dgTrees.SelectionChanged += new EventHandler(this.dgTrees_SelectionChanged);
      this.dgTrees.Sorted += new EventHandler(this.dgTrees_Sorted);
      this.dcPlot.DataPropertyName = "Plot";
      this.dcPlot.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.dcPlot.Frozen = true;
      componentResourceManager.ApplyResources((object) this.dcPlot, "dcPlot");
      this.dcPlot.Name = "dcPlot";
      this.dcPlot.ReadOnly = true;
      this.dcPlot.SortMode = DataGridViewColumnSortMode.Automatic;
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
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle1.Format = "N2";
      this.dcDistance.DefaultCellStyle = gridViewCellStyle1;
      this.dcDistance.Format = (string) null;
      this.dcDistance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcDistance, "dcDistance");
      this.dcDistance.Name = "dcDistance";
      this.dcDistance.ReadOnly = true;
      this.dcDistance.Resizable = DataGridViewTriState.True;
      this.dcDistance.Signed = false;
      this.dcDirection.DataPropertyName = "Direction";
      this.dcDirection.DecimalPlaces = 0;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcDirection.DefaultCellStyle = gridViewCellStyle2;
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
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle3.Format = "N1";
      this.dcDBH1.DefaultCellStyle = gridViewCellStyle3;
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
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle4.Format = "N2";
      this.dcDBH1Height.DefaultCellStyle = gridViewCellStyle4;
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
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle5.Format = "N1";
      this.dcDBH2.DefaultCellStyle = gridViewCellStyle5;
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
      gridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle6.Format = "N2";
      this.dcDBH2Height.DefaultCellStyle = gridViewCellStyle6;
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
      gridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle7.Format = "N1";
      this.dcDBH3.DefaultCellStyle = gridViewCellStyle7;
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
      gridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle8.Format = "N2";
      this.dcDBH3Height.DefaultCellStyle = gridViewCellStyle8;
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
      gridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle9.Format = "N1";
      this.dcDBH4.DefaultCellStyle = gridViewCellStyle9;
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
      gridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle10.Format = "N2";
      this.dcDBH4Height.DefaultCellStyle = gridViewCellStyle10;
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
      gridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle11.Format = "N1";
      this.dcDBH5.DefaultCellStyle = gridViewCellStyle11;
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
      gridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle12.Format = "N2";
      this.dcDBH5Height.DefaultCellStyle = gridViewCellStyle12;
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
      gridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle13.Format = "N1";
      this.dcDBH6.DefaultCellStyle = gridViewCellStyle13;
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
      gridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle14.Format = "N2";
      this.dcDBH6Height.DefaultCellStyle = gridViewCellStyle14;
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
      gridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle15.Format = "N2";
      this.dcTreeHeight.DefaultCellStyle = gridViewCellStyle15;
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
      gridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle16.Format = "N2";
      this.dcCrownTopHeight.DefaultCellStyle = gridViewCellStyle16;
      this.dcCrownTopHeight.Format = "#;#";
      this.dcCrownTopHeight.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownTopHeight, "dcCrownTopHeight");
      this.dcCrownTopHeight.Name = "dcCrownTopHeight";
      this.dcCrownTopHeight.ReadOnly = true;
      this.dcCrownTopHeight.Signed = false;
      this.dcCrownBaseHeight.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownBaseHeight.DataPropertyName = "CrownBaseHeight";
      this.dcCrownBaseHeight.DecimalPlaces = 2;
      gridViewCellStyle17.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle17.Format = "N2";
      this.dcCrownBaseHeight.DefaultCellStyle = gridViewCellStyle17;
      this.dcCrownBaseHeight.Format = "0.#;0.#";
      this.dcCrownBaseHeight.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcCrownBaseHeight, "dcCrownBaseHeight");
      this.dcCrownBaseHeight.Name = "dcCrownBaseHeight";
      this.dcCrownBaseHeight.ReadOnly = true;
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
      this.dcCrownWidthNS.ReadOnly = true;
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
      this.dcCrownWidthEW.ReadOnly = true;
      this.dcCrownWidthEW.Signed = false;
      this.dcCrownPercentMissing.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcCrownPercentMissing.DataPropertyName = "CrownPercentMissing";
      gridViewCellStyle20.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcCrownPercentMissing.DefaultCellStyle = gridViewCellStyle20;
      this.dcCrownPercentMissing.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcCrownPercentMissing, "dcCrownPercentMissing");
      this.dcCrownPercentMissing.Name = "dcCrownPercentMissing";
      this.dcCrownPercentMissing.ReadOnly = true;
      this.dcCrownPercentMissing.Resizable = DataGridViewTriState.True;
      this.dcCrownPercentMissing.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctImpervious.DataPropertyName = "PercentImpervious";
      gridViewCellStyle21.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcPctImpervious.DefaultCellStyle = gridViewCellStyle21;
      this.dcPctImpervious.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      componentResourceManager.ApplyResources((object) this.dcPctImpervious, "dcPctImpervious");
      this.dcPctImpervious.Name = "dcPctImpervious";
      this.dcPctImpervious.ReadOnly = true;
      this.dcPctImpervious.Resizable = DataGridViewTriState.True;
      this.dcPctImpervious.SortMode = DataGridViewColumnSortMode.Automatic;
      this.dcPctShrub.DataPropertyName = "PercentShrub";
      gridViewCellStyle22.Alignment = DataGridViewContentAlignment.MiddleRight;
      this.dcPctShrub.DefaultCellStyle = gridViewCellStyle22;
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
      gridViewCellStyle23.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle23.Format = "N0";
      this.dcBldg1Direction.DefaultCellStyle = gridViewCellStyle23;
      this.dcBldg1Direction.Format = "#;#";
      this.dcBldg1Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg1Direction, "dcBldg1Direction");
      this.dcBldg1Direction.Name = "dcBldg1Direction";
      this.dcBldg1Direction.ReadOnly = true;
      this.dcBldg1Direction.Signed = false;
      this.dcBldg1Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg1Distance.DataPropertyName = "B1Distance";
      this.dcBldg1Distance.DecimalPlaces = 2;
      gridViewCellStyle24.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle24.Format = "N2";
      this.dcBldg1Distance.DefaultCellStyle = gridViewCellStyle24;
      this.dcBldg1Distance.Format = "#;#";
      this.dcBldg1Distance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcBldg1Distance, "dcBldg1Distance");
      this.dcBldg1Distance.Name = "dcBldg1Distance";
      this.dcBldg1Distance.ReadOnly = true;
      this.dcBldg1Distance.Signed = false;
      this.dcBldg2Direction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg2Direction.DataPropertyName = "B2Direction";
      this.dcBldg2Direction.DecimalPlaces = 0;
      gridViewCellStyle25.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle25.Format = "N0";
      this.dcBldg2Direction.DefaultCellStyle = gridViewCellStyle25;
      this.dcBldg2Direction.Format = "#;#";
      this.dcBldg2Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg2Direction, "dcBldg2Direction");
      this.dcBldg2Direction.Name = "dcBldg2Direction";
      this.dcBldg2Direction.ReadOnly = true;
      this.dcBldg2Direction.Signed = false;
      this.dcBldg2Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg2Distance.DataPropertyName = "B2Distance";
      this.dcBldg2Distance.DecimalPlaces = 2;
      gridViewCellStyle26.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle26.Format = "N2";
      this.dcBldg2Distance.DefaultCellStyle = gridViewCellStyle26;
      this.dcBldg2Distance.Format = "#;#";
      this.dcBldg2Distance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcBldg2Distance, "dcBldg2Distance");
      this.dcBldg2Distance.Name = "dcBldg2Distance";
      this.dcBldg2Distance.ReadOnly = true;
      this.dcBldg2Distance.Signed = false;
      this.dcBldg3Direction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg3Direction.DataPropertyName = "B3Direction";
      this.dcBldg3Direction.DecimalPlaces = 0;
      gridViewCellStyle27.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle27.Format = "N0";
      this.dcBldg3Direction.DefaultCellStyle = gridViewCellStyle27;
      this.dcBldg3Direction.Format = "#;#";
      this.dcBldg3Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg3Direction, "dcBldg3Direction");
      this.dcBldg3Direction.Name = "dcBldg3Direction";
      this.dcBldg3Direction.ReadOnly = true;
      this.dcBldg3Direction.Signed = false;
      this.dcBldg3Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg3Distance.DataPropertyName = "B3Distance";
      this.dcBldg3Distance.DecimalPlaces = 2;
      gridViewCellStyle28.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle28.Format = "N2";
      this.dcBldg3Distance.DefaultCellStyle = gridViewCellStyle28;
      this.dcBldg3Distance.Format = "#;#";
      this.dcBldg3Distance.HasDecimal = true;
      componentResourceManager.ApplyResources((object) this.dcBldg3Distance, "dcBldg3Distance");
      this.dcBldg3Distance.Name = "dcBldg3Distance";
      this.dcBldg3Distance.ReadOnly = true;
      this.dcBldg3Distance.Signed = false;
      this.dcBldg4Direction.DataPropertyName = "B4Direction";
      this.dcBldg4Direction.DecimalPlaces = 0;
      gridViewCellStyle29.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle29.Format = "N0";
      this.dcBldg4Direction.DefaultCellStyle = gridViewCellStyle29;
      this.dcBldg4Direction.Format = "#;#";
      this.dcBldg4Direction.HasDecimal = false;
      componentResourceManager.ApplyResources((object) this.dcBldg4Direction, "dcBldg4Direction");
      this.dcBldg4Direction.Name = "dcBldg4Direction";
      this.dcBldg4Direction.ReadOnly = true;
      this.dcBldg4Direction.Signed = false;
      this.dcBldg4Distance.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.dcBldg4Distance.DataPropertyName = "B4Distance";
      this.dcBldg4Distance.DecimalPlaces = 2;
      gridViewCellStyle30.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle30.Format = "N2";
      this.dcBldg4Distance.DefaultCellStyle = gridViewCellStyle30;
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
      gridViewCellStyle31.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle31.Format = "N0";
      this.dcLocNo.DefaultCellStyle = gridViewCellStyle31;
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
      componentResourceManager.ApplyResources((object) this.pnlEditHelp, "pnlEditHelp");
      this.pnlEditHelp.Name = "pnlEditHelp";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.dgTrees);
      this.Controls.Add((Control) this.pnlEditHelp);
      this.DockAreas = DockAreas.Document;
      this.Name = nameof (SampleTreesForm);
      this.ShowHint = DockState.Document;
      this.Controls.SetChildIndex((Control) this.lblBreadcrumb, 0);
      this.Controls.SetChildIndex((Control) this.pnlEditHelp, 0);
      this.Controls.SetChildIndex((Control) this.dgTrees, 0);
      ((ISupportInitialize) this.dgTrees).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
