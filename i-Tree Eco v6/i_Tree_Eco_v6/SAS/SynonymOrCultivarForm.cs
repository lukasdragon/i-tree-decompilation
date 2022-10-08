// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.SAS.SynonymOrCultivarForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.NHibernate;
using Eco.Domain.v6;
using Eco.Util.Views;
using i_Tree_Eco_v6.Events;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.SAS
{
  public class SynonymOrCultivarForm : Form
  {
    private ProgramSession _ps;
    private ISession _project_s;
    private Year _currYear;
    private Series _currSeries;
    private Project _currProject;
    private string msg = "The US Forest Service has determined that the {0}{1}{2} listed below are {3}{4}{5} of other species. To streamline the i-Tree species database, they are no longer available but can be replaced with the suggested species shown in the “Replace by” column below. \n    - Click the Replace All button to accept ALL these changes on ALL tabs shown (e.g. Trees, Shrubs, Forecast). \n    - Click Replace Selected button to replace selected rows, one at a time. Make sure you do this on all tabs shown. \n    - Double Click the row to select another species to replace. \n    - Click Cancel to CLOSE this form and manually change EACH incidence of these species names under the main i-Tree Eco Data tab. ";
    private string msg2 = "Note: completely invalid species names and/or codes cannot be replaced with this form. These must be replaced manually under the Data tab.";
    private Dictionary<string, string> _treeSynonymCultivar;
    private Dictionary<string, string> _shrubSynonymCultivar;
    private Dictionary<string, string> _mortalityTreeSynonymCultivar;
    private bool _includeIncompletePlots;
    private IContainer components;
    private TabControl tabControl1;
    private TabPage tabPageTrees;
    private TabPage tabPageShrubs;
    private Panel panel2;
    private Button btnCancel;
    private Button btnReplace;
    private Panel panel1;
    private TabPage tabPageForecastMortalities;
    private Button btnReplaceSelected;
    private Label labelMessage;
    private Label label1;
    private DataGridView dataGridViewTrees;
    private DataGridViewTextBoxColumn GridTreeGuid;
    private DataGridViewTextBoxColumn GridTreePlotId;
    private DataGridViewTextBoxColumn GridTreeTreeId;
    private DataGridViewTextBoxColumn GridTreeSpecies;
    private DataGridViewTextBoxColumn GridTreeReason;
    private DataGridViewTextBoxColumn GridTreeReplaceBy;
    private DataGridView dataGridViewShrubs;
    private DataGridViewTextBoxColumn GridShrubGuid;
    private DataGridViewTextBoxColumn GridShrubPlotId;
    private DataGridViewTextBoxColumn GridShrubShrubId;
    private DataGridViewTextBoxColumn GridShrubSpecies;
    private DataGridViewTextBoxColumn GridShrubReason;
    private DataGridViewTextBoxColumn GridShrubReplaceBy;
    private Label label2;
    private DataGridView dataGridViewMortalities;
    private DataGridViewTextBoxColumn GridMortalityGuid;
    private DataGridViewTextBoxColumn GridMortalityForecast;
    private DataGridViewTextBoxColumn GridMortalityType;
    private DataGridViewTextBoxColumn GridMortalitySpecies;
    private DataGridViewTextBoxColumn GridMortalityReason;
    private DataGridViewTextBoxColumn GridMortalityReplaceBy;
    private Label label3;
    private Panel panel3;
    private Label labelMessage2;

    public SynonymOrCultivarForm() => this.InitializeComponent();

    public void InitializeForm(
      ProgramSession m_ps,
      bool includeIncompletePlots,
      Dictionary<string, string> treeSynonymCultivar,
      Dictionary<string, string> shrubSynonymCultivar,
      Dictionary<string, string> mortalityTreeSynonymCultivar)
    {
      this._ps = m_ps;
      this._project_s = this._ps.InputSession.CreateSession();
      this._currYear = this._project_s.Get<Year>((object) this._ps.InputSession.YearKey);
      this._currSeries = this._currYear.Series;
      this._currProject = this._currSeries.Project;
      this._treeSynonymCultivar = treeSynonymCultivar;
      this._shrubSynonymCultivar = shrubSynonymCultivar;
      this._mortalityTreeSynonymCultivar = mortalityTreeSynonymCultivar;
      this._includeIncompletePlots = includeIncompletePlots;
      using (ISession session = m_ps.LocSp.OpenSession())
      {
        Dictionary<string, string> dictionary1 = session.QueryOver<Species>().Fetch<Species, Species>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.ReplacedBy.Code)).Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (sp => sp.Code != default (string) && sp.ReplacedBy != default (object))).Cacheable().List().ToDictionary<Species, string, string>((Func<Species, string>) (sp => sp.Code), (Func<Species, string>) (sp => sp.ReplacedBy.Code));
        session.QueryOver<Species>().Fetch<Species, Species>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.Parent.Code)).Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (sp => sp.Code != default (string) && (int) sp.Rank == 8)).Cacheable().List().ToDictionary<Species, string, string>((Func<Species, string>) (sp => sp.Code), (Func<Species, string>) (sp => sp.Parent.Code));
        IList<(Guid, int, int, string)> valueTupleList1 = (IList<(Guid, int, int, string)>) null;
        IList<(Guid, int, int, string)> valueTupleList2 = (IList<(Guid, int, int, string)>) null;
        bool flag1 = false;
        bool flag2 = false;
        Plot plotType = (Plot) null;
        Tree treeType = (Tree) null;
        Shrub shrubType = (Shrub) null;
        if (this._currSeries.SampleType == SampleType.Inventory)
        {
          if (this._treeSynonymCultivar.Count > 0)
            valueTupleList1 = this._project_s.QueryOver<Tree>((System.Linq.Expressions.Expression<Func<Tree>>) (() => treeType)).Where((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Species.IsIn(this._treeSynonymCultivar.Keys))).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => plotType)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this._currYear)).OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plotType.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) treeType.Id)).Asc.Select((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) treeType.Guid)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plotType.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) treeType.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => treeType.Species))).TransformUsing((IResultTransformer) new TupleResultTransformer<(Guid, int, int, string)>()).List<(Guid, int, int, string)>();
        }
        else
        {
          List<bool> trueFalse = new List<bool>();
          trueFalse.Add(true);
          if (this._includeIncompletePlots)
            trueFalse.Add(false);
          valueTupleList1 = (IList<(Guid, int, int, string)>) null;
          if (this._treeSynonymCultivar.Count > 0)
            valueTupleList1 = this._project_s.QueryOver<Tree>((System.Linq.Expressions.Expression<Func<Tree>>) (() => treeType)).Where((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Species.IsIn(this._treeSynonymCultivar.Keys))).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => plotType)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this._currYear && p.IsComplete.IsIn(trueFalse))).OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plotType.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) treeType.Id)).Asc.Select((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) treeType.Guid)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plotType.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) treeType.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => treeType.Species))).TransformUsing((IResultTransformer) new TupleResultTransformer<(Guid, int, int, string)>()).List<(Guid, int, int, string)>();
          valueTupleList2 = (IList<(Guid, int, int, string)>) null;
          if (this._shrubSynonymCultivar.Count > 0)
            valueTupleList2 = this._project_s.QueryOver<Shrub>((System.Linq.Expressions.Expression<Func<Shrub>>) (() => shrubType)).Where((System.Linq.Expressions.Expression<Func<Shrub, bool>>) (sh => sh.Species.IsIn(this._shrubSynonymCultivar.Keys))).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Shrub, Plot>>) (t => t.Plot), (System.Linq.Expressions.Expression<Func<Plot>>) (() => plotType)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this._currYear && p.IsComplete.IsIn(trueFalse))).OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plotType.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) shrubType.Id)).Asc.Select((IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) shrubType.Guid)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plotType.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) shrubType.Id)), (IProjection) Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => shrubType.Species))).TransformUsing((IResultTransformer) new TupleResultTransformer<(Guid, int, int, string)>()).List<(Guid, int, int, string)>();
        }
        IList<Mortality> mortalityList = this._project_s.QueryOver<Mortality>().Where((System.Linq.Expressions.Expression<Func<Mortality, bool>>) (m => m.Type == "Genus" && m.Value.IsIn(this._mortalityTreeSynonymCultivar.Keys))).JoinQueryOver<Forecast>((System.Linq.Expressions.Expression<Func<Mortality, Forecast>>) (m => m.Forecast)).Where((System.Linq.Expressions.Expression<Func<Forecast, bool>>) (f => f.Year == this._currYear)).List<Mortality>();
        if (mortalityList != null && mortalityList.Count == 0)
          mortalityList = (IList<Mortality>) null;
        if (valueTupleList1 != null || valueTupleList2 != null || mortalityList != null)
        {
          Dictionary<string, SpeciesView> dictionary2 = session.QueryOver<Species>().Fetch<Species, Species>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.Names), (System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.LeafType), (System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.Parent), (System.Linq.Expressions.Expression<Func<Species, object>>) (sp => (object) sp.PercentLeafType)).Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (sp => sp.Code != default (string) && (sp.ReplacedBy != default (object) || (int) sp.Rank == 8))).JoinQueryOver<LeafType>((System.Linq.Expressions.Expression<Func<Species, LeafType>>) (sp => sp.LeafType)).TransformUsing(Transformers.DistinctRootEntity).Cacheable().List().ToDictionary<Species, string, SpeciesView>((Func<Species, string>) (sp => sp.Code), (Func<Species, SpeciesView>) (sp => new SpeciesView(sp)));
          if (valueTupleList1 != null)
          {
            DataGridView dataGridViewTrees = this.dataGridViewTrees;
            for (int index = 0; index < valueTupleList1.Count; ++index)
            {
              DataGridViewRow row = dataGridViewTrees.Rows[dataGridViewTrees.Rows.Add()];
              row.Cells["GridTreeGuid"].Value = (object) valueTupleList1[index].Item1;
              row.Cells["GridTreePlotId"].Value = (object) valueTupleList1[index].Item2;
              row.Cells["GridTreeTreeId"].Value = (object) valueTupleList1[index].Item3;
              row.Cells["GridTreeSpecies"].Value = (object) (valueTupleList1[index].Item4 + " - " + dictionary2[valueTupleList1[index].Item4].CommonScientificName);
              if (dictionary1.ContainsKey(valueTupleList1[index].Item4))
              {
                row.Cells["GridTreeReason"].Value = (object) "Synonym";
                flag1 = true;
              }
              else
              {
                row.Cells["GridTreeReason"].Value = (object) "Cultivar";
                flag2 = true;
              }
              string key = treeSynonymCultivar[valueTupleList1[index].Item4];
              row.Cells["GridTreeReplaceBy"].Value = (object) (key + " - " + m_ps.Species[key].CommonScientificName);
              row.Cells["GridTreeReplaceBy"].ToolTipText = "Double Click to select another species";
            }
            dataGridViewTrees.Columns["GridTreeGuid"].Visible = false;
          }
          if (valueTupleList2 != null)
          {
            DataGridView dataGridViewShrubs = this.dataGridViewShrubs;
            for (int index = 0; index < valueTupleList2.Count; ++index)
            {
              DataGridViewRow row = dataGridViewShrubs.Rows[dataGridViewShrubs.Rows.Add()];
              row.Cells["GridShrubGuid"].Value = (object) valueTupleList2[index].Item1;
              row.Cells["GridShrubPlotId"].Value = (object) valueTupleList2[index].Item2;
              row.Cells["GridShrubShrubId"].Value = (object) valueTupleList2[index].Item3;
              row.Cells["GridShrubSpecies"].Value = (object) (valueTupleList2[index].Item4 + " - " + dictionary2[valueTupleList2[index].Item4].CommonScientificName);
              if (dictionary1.ContainsKey(valueTupleList2[index].Item4))
              {
                row.Cells["GridShrubReason"].Value = (object) "Synonym";
                flag1 = true;
              }
              else
              {
                row.Cells["GridShrubReason"].Value = (object) "Cultivar";
                flag2 = true;
              }
              string key = shrubSynonymCultivar[valueTupleList2[index].Item4];
              row.Cells["GridShrubReplaceBy"].Value = (object) (key + " - " + m_ps.Species[key].CommonScientificName);
              row.Cells["GridShrubReplaceBy"].ToolTipText = "Double Click to select another species";
            }
            dataGridViewShrubs.Columns["GridShrubGuid"].Visible = false;
          }
          if (mortalityList != null)
          {
            DataGridView gridViewMortalities = this.dataGridViewMortalities;
            for (int index = 0; index < mortalityList.Count; ++index)
            {
              DataGridViewRow row = gridViewMortalities.Rows[gridViewMortalities.Rows.Add()];
              row.Cells["GridMortalityGuid"].Value = (object) mortalityList[index].Guid;
              row.Cells["GridMortalityForecast"].Value = (object) mortalityList[index].Forecast.Title;
              row.Cells["GridMortalityType"].Value = (object) mortalityList[index].Type;
              row.Cells["GridMortalitySpecies"].Value = (object) (mortalityList[index].Value + " - " + dictionary2[mortalityList[index].Value].CommonScientificName);
              if (dictionary1.ContainsKey(mortalityList[index].Value))
              {
                row.Cells["GridMortalityReason"].Value = (object) "Synonym";
                flag1 = true;
              }
              else
              {
                row.Cells["GridMortalityReason"].Value = (object) "Cultivar";
                flag2 = true;
              }
              string key = mortalityTreeSynonymCultivar[mortalityList[index].Value];
              row.Cells["GridMortalityReplaceBy"].Value = (object) (key + " - " + m_ps.Species[key].CommonScientificName);
              row.Cells["GridMortalityReplaceBy"].ToolTipText = "Double Click to select another genus";
            }
            gridViewMortalities.Columns["GridMortalityGuid"].Visible = false;
          }
          if (this._currSeries.SampleType == SampleType.Inventory)
            this.dataGridViewTrees.Columns["GridTreePlotId"].Visible = false;
          if (valueTupleList1 == null)
            this.tabControl1.TabPages.RemoveByKey("tabPageTrees");
          if (valueTupleList2 == null)
            this.tabControl1.TabPages.RemoveByKey("tabPageShrubs");
          if (mortalityList == null)
            this.tabControl1.TabPages.RemoveByKey("tabPageForecastMortalities");
        }
        string str1 = "";
        string str2 = "";
        string str3 = "";
        string str4 = "";
        string str5 = "";
        string str6 = "";
        if (valueTupleList1 != null)
          str1 = "trees";
        if (valueTupleList2 != null)
          str3 = "shrubs";
        if (valueTupleList1 != null && valueTupleList2 != null)
          str2 = " and ";
        if (flag1)
          str4 = "synonyms";
        if (flag2)
          str6 = "cultivars";
        if (flag1 & flag2)
          str5 = " or ";
        this.labelMessage.Text = string.Format(this.msg, (object) str1, (object) str2, (object) str3, (object) str4, (object) str5, (object) str6);
        this.labelMessage2.Text = this.msg2;
      }
    }

    private void btnReplace_Click(object sender, EventArgs e)
    {
      string str = "";
      if (this.tabControl1.TabPages.Count > 1)
        str = " in every tab";
      if (MessageBox.Show((IWin32Window) this, string.Format("Perform ALL these changes{0}?", (object) str), "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
        return;
      this.btnReplace.Enabled = false;
      this.btnReplaceSelected.Enabled = false;
      if (this._treeSynonymCultivar.Count > 0)
      {
        IList<Tree> treeList;
        if (this._currSeries.SampleType == SampleType.Inventory)
        {
          treeList = this._project_s.QueryOver<Tree>().Where((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Species.IsIn(this._treeSynonymCultivar.Keys))).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this._currYear)).List<Tree>();
        }
        else
        {
          List<bool> trueFalse = new List<bool>();
          trueFalse.Add(true);
          if (this._includeIncompletePlots)
            trueFalse.Add(false);
          treeList = this._project_s.QueryOver<Tree>().Where((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Species.IsIn(this._treeSynonymCultivar.Keys))).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this._currYear && p.IsComplete.IsIn(trueFalse))).List<Tree>();
        }
        using (ITransaction transaction = this._project_s.BeginTransaction())
        {
          foreach (Tree tree in (IEnumerable<Tree>) treeList)
          {
            tree.Species = this._treeSynonymCultivar[tree.Species];
            this._project_s.SaveOrUpdate((object) tree);
          }
          transaction.Commit();
        }
        foreach (Tree entity in (IEnumerable<Tree>) treeList)
          EventPublisher.Publish<EntityUpdated<Tree>>(new EntityUpdated<Tree>(entity), (Control) this);
        if (!this._currYear.Changed)
        {
          using (ITransaction transaction = this._project_s.BeginTransaction())
          {
            this._currYear.Changed = true;
            this._project_s.SaveOrUpdate((object) this._currYear);
            transaction.Commit();
          }
          EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(this._currYear), (Control) this);
        }
      }
      if (this._shrubSynonymCultivar.Count > 0)
      {
        List<bool> trueFalse = new List<bool>();
        trueFalse.Add(true);
        if (this._includeIncompletePlots)
          trueFalse.Add(false);
        IList<Shrub> shrubList = this._project_s.QueryOver<Shrub>().Where((System.Linq.Expressions.Expression<Func<Shrub, bool>>) (sh => sh.Species.IsIn(this._shrubSynonymCultivar.Keys))).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Shrub, Plot>>) (t => t.Plot)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year == this._currYear && p.IsComplete.IsIn(trueFalse))).List<Shrub>();
        using (ITransaction transaction = this._project_s.BeginTransaction())
        {
          foreach (Shrub shrub in (IEnumerable<Shrub>) shrubList)
          {
            shrub.Species = this._shrubSynonymCultivar[shrub.Species];
            this._project_s.SaveOrUpdate((object) shrub);
          }
          transaction.Commit();
        }
        foreach (Shrub entity in (IEnumerable<Shrub>) shrubList)
          EventPublisher.Publish<EntityUpdated<Shrub>>(new EntityUpdated<Shrub>(entity), (Control) this);
        if (!this._currYear.Changed)
        {
          using (ITransaction transaction = this._project_s.BeginTransaction())
          {
            this._currYear.Changed = true;
            this._project_s.SaveOrUpdate((object) this._currYear);
            transaction.Commit();
          }
          EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(this._currYear), (Control) this);
        }
      }
      if (this._mortalityTreeSynonymCultivar.Count > 0)
      {
        Dictionary<Guid, Forecast> dictionary = new Dictionary<Guid, Forecast>();
        IList<Mortality> mortalityList = this._project_s.QueryOver<Mortality>().Where((System.Linq.Expressions.Expression<Func<Mortality, bool>>) (m => m.Type == "Genus" && m.Value.IsIn(this._mortalityTreeSynonymCultivar.Keys))).JoinQueryOver<Forecast>((System.Linq.Expressions.Expression<Func<Mortality, Forecast>>) (m => m.Forecast)).Where((System.Linq.Expressions.Expression<Func<Forecast, bool>>) (f => f.Year == this._currYear)).List<Mortality>();
        using (ITransaction transaction = this._project_s.BeginTransaction())
        {
          foreach (Mortality mortality in (IEnumerable<Mortality>) mortalityList)
          {
            mortality.Value = this._mortalityTreeSynonymCultivar[mortality.Value];
            this._project_s.SaveOrUpdate((object) mortality);
            if (!mortality.Forecast.Changed && !dictionary.ContainsKey(mortality.Forecast.Guid))
            {
              mortality.Forecast.Changed = true;
              this._project_s.SaveOrUpdate((object) mortality.Forecast);
              dictionary.Add(mortality.Forecast.Guid, mortality.Forecast);
            }
          }
          transaction.Commit();
        }
        foreach (Mortality entity in (IEnumerable<Mortality>) mortalityList)
          EventPublisher.Publish<EntityUpdated<Mortality>>(new EntityUpdated<Mortality>(entity), (Control) this);
        if (dictionary.Count > 0)
        {
          foreach (KeyValuePair<Guid, Forecast> keyValuePair in dictionary)
            EventPublisher.Publish<EntityUpdated<Forecast>>(new EntityUpdated<Forecast>(keyValuePair.Value), (Control) this);
        }
      }
      this.Close();
    }

    private void btnReplaceSelected_Click(object sender, EventArgs e)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SynonymOrCultivarForm.\u003C\u003Ec__DisplayClass14_0 cDisplayClass140 = new SynonymOrCultivarForm.\u003C\u003Ec__DisplayClass14_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass140.aGrid = (DataGridView) null;
      string str;
      if (this.tabControl1.SelectedTab.Name == "tabPageTrees")
      {
        // ISSUE: reference to a compiler-generated field
        cDisplayClass140.aGrid = this.dataGridViewTrees;
        str = "trees";
      }
      else if (this.tabControl1.SelectedTab.Name == "tabPageShrubs")
      {
        // ISSUE: reference to a compiler-generated field
        cDisplayClass140.aGrid = this.dataGridViewShrubs;
        str = "shrubs";
      }
      else
      {
        if (!(this.tabControl1.SelectedTab.Name == "tabPageForecastMortalities"))
          return;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass140.aGrid = this.dataGridViewMortalities;
        str = "mortalities";
      }
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass140.aGrid.SelectedRows.Count == 0)
      {
        int num1 = (int) MessageBox.Show((IWin32Window) this, "No selected " + str);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (MessageBox.Show((IWin32Window) this, string.Format("Perform changes on {0} selected {1}?", (object) cDisplayClass140.aGrid.SelectedRows.Count, (object) str), "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
          return;
        this.btnReplace.Enabled = false;
        this.btnReplaceSelected.Enabled = false;
        if (this.tabControl1.SelectedTab.Name == "tabPageTrees")
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClass140.aGrid = this.dataGridViewTrees;
          List<int> intList = new List<int>();
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < cDisplayClass140.aGrid.SelectedRows.Count; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            intList.Add(cDisplayClass140.aGrid.SelectedRows[index].Index);
          }
          intList.Sort();
          using (ITransaction transaction = this._project_s.BeginTransaction())
          {
            for (int index = intList.Count - 1; index >= 0; --index)
            {
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              SynonymOrCultivarForm.\u003C\u003Ec__DisplayClass14_1 cDisplayClass141 = new SynonymOrCultivarForm.\u003C\u003Ec__DisplayClass14_1();
              // ISSUE: reference to a compiler-generated field
              cDisplayClass141.CS\u0024\u003C\u003E8__locals1 = cDisplayClass140;
              // ISSUE: reference to a compiler-generated field
              cDisplayClass141.thisIndex = intList[index];
              ParameterExpression parameterExpression;
              // ISSUE: method reference
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: method reference
              // ISSUE: method reference
              Tree tree = this._project_s.QueryOver<Tree>().Where(System.Linq.Expressions.Expression.Lambda<Func<Tree, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(t.Guid, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.New((ConstructorInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.\u002Ector)), (IEnumerable<System.Linq.Expressions.Expression>) new System.Linq.Expressions.Expression[1]
              {
                (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(cDisplayClass141.CS\u0024\u003C\u003E8__locals1.aGrid.Rows[cDisplayClass141.thisIndex].Cells["GridTreeGuid"].Value, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (object.ToString)), Array.Empty<System.Linq.Expressions.Expression>())
              }), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression)).SingleOrDefault<Tree>();
              tree.Species = this._treeSynonymCultivar[tree.Species];
              this._project_s.SaveOrUpdate((object) tree);
            }
            transaction.Commit();
          }
          for (int index1 = intList.Count - 1; index1 >= 0; --index1)
          {
            int index2 = intList[index1];
            // ISSUE: reference to a compiler-generated field
            EventPublisher.Publish<EntityUpdated<Tree>>(new EntityUpdated<Tree>(new Guid(cDisplayClass140.aGrid.Rows[index2].Cells["GridTreeGuid"].Value.ToString())), (Control) this);
            // ISSUE: reference to a compiler-generated field
            cDisplayClass140.aGrid.Rows.RemoveAt(index2);
          }
          if (!this._currYear.Changed)
          {
            using (ITransaction transaction = this._project_s.BeginTransaction())
            {
              this._currYear.Changed = true;
              this._project_s.SaveOrUpdate((object) this._currYear);
              transaction.Commit();
            }
            EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(this._currYear), (Control) this);
          }
          int num2 = (int) MessageBox.Show((IWin32Window) this, intList.Count.ToString() + " trees are replaced. ");
          // ISSUE: reference to a compiler-generated field
          if (cDisplayClass140.aGrid.Rows.Count == 0)
            this.tabControl1.TabPages.RemoveByKey("tabPageTrees");
        }
        else if (this.tabControl1.SelectedTab.Name == "tabPageShrubs")
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClass140.aGrid = this.dataGridViewShrubs;
          List<int> intList = new List<int>();
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < cDisplayClass140.aGrid.SelectedRows.Count; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            intList.Add(cDisplayClass140.aGrid.SelectedRows[index].Index);
          }
          intList.Sort();
          using (ITransaction transaction = this._project_s.BeginTransaction())
          {
            for (int index = intList.Count - 1; index >= 0; --index)
            {
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              SynonymOrCultivarForm.\u003C\u003Ec__DisplayClass14_2 cDisplayClass142 = new SynonymOrCultivarForm.\u003C\u003Ec__DisplayClass14_2();
              // ISSUE: reference to a compiler-generated field
              cDisplayClass142.CS\u0024\u003C\u003E8__locals2 = cDisplayClass140;
              // ISSUE: reference to a compiler-generated field
              cDisplayClass142.thisIndex = intList[index];
              ParameterExpression parameterExpression;
              // ISSUE: method reference
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: method reference
              // ISSUE: method reference
              Shrub shrub = this._project_s.QueryOver<Shrub>().Where(System.Linq.Expressions.Expression.Lambda<Func<Shrub, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(s.Guid, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.New((ConstructorInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.\u002Ector)), (IEnumerable<System.Linq.Expressions.Expression>) new System.Linq.Expressions.Expression[1]
              {
                (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(cDisplayClass142.CS\u0024\u003C\u003E8__locals2.aGrid.Rows[cDisplayClass142.thisIndex].Cells["GridShrubGuid"].Value, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (object.ToString)), Array.Empty<System.Linq.Expressions.Expression>())
              }), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression)).SingleOrDefault<Shrub>();
              shrub.Species = this._shrubSynonymCultivar[shrub.Species];
              this._project_s.SaveOrUpdate((object) shrub);
            }
            transaction.Commit();
          }
          for (int index3 = intList.Count - 1; index3 >= 0; --index3)
          {
            int index4 = intList[index3];
            // ISSUE: reference to a compiler-generated field
            EventPublisher.Publish<EntityUpdated<Shrub>>(new EntityUpdated<Shrub>(new Guid(cDisplayClass140.aGrid.Rows[index4].Cells["GridShrubGuid"].Value.ToString())), (Control) this);
            // ISSUE: reference to a compiler-generated field
            cDisplayClass140.aGrid.Rows.RemoveAt(index4);
          }
          if (!this._currYear.Changed)
          {
            using (ITransaction transaction = this._project_s.BeginTransaction())
            {
              this._currYear.Changed = true;
              this._project_s.SaveOrUpdate((object) this._currYear);
              transaction.Commit();
            }
            EventPublisher.Publish<EntityUpdated<Year>>(new EntityUpdated<Year>(this._currYear), (Control) this);
          }
          int num3 = (int) MessageBox.Show((IWin32Window) this, intList.Count.ToString() + " shrubs are replaced. ");
          // ISSUE: reference to a compiler-generated field
          if (cDisplayClass140.aGrid.Rows.Count == 0)
            this.tabControl1.TabPages.RemoveByKey("tabPageShrubs");
        }
        else if (this.tabControl1.SelectedTab.Name == "tabPageForecastMortalities")
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClass140.aGrid = this.dataGridViewMortalities;
          List<int> intList = new List<int>();
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < cDisplayClass140.aGrid.SelectedRows.Count; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            intList.Add(cDisplayClass140.aGrid.SelectedRows[index].Index);
          }
          intList.Sort();
          Dictionary<Guid, Forecast> dictionary = new Dictionary<Guid, Forecast>();
          using (ITransaction transaction = this._project_s.BeginTransaction())
          {
            for (int index = intList.Count - 1; index >= 0; --index)
            {
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              SynonymOrCultivarForm.\u003C\u003Ec__DisplayClass14_3 cDisplayClass143 = new SynonymOrCultivarForm.\u003C\u003Ec__DisplayClass14_3();
              // ISSUE: reference to a compiler-generated field
              cDisplayClass143.CS\u0024\u003C\u003E8__locals3 = cDisplayClass140;
              // ISSUE: reference to a compiler-generated field
              cDisplayClass143.thisIndex = intList[index];
              ParameterExpression parameterExpression;
              // ISSUE: method reference
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: method reference
              // ISSUE: method reference
              Mortality mortality = this._project_s.QueryOver<Mortality>().Where(System.Linq.Expressions.Expression.Lambda<Func<Mortality, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(m.Guid, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.New((ConstructorInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.\u002Ector)), (IEnumerable<System.Linq.Expressions.Expression>) new System.Linq.Expressions.Expression[1]
              {
                (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(cDisplayClass143.CS\u0024\u003C\u003E8__locals3.aGrid.Rows[cDisplayClass143.thisIndex].Cells["GridMortalityGuid"].Value, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (object.ToString)), Array.Empty<System.Linq.Expressions.Expression>())
              }), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression)).SingleOrDefault<Mortality>();
              mortality.Value = this._mortalityTreeSynonymCultivar[mortality.Value];
              this._project_s.SaveOrUpdate((object) mortality);
              if (!mortality.Forecast.Changed && !dictionary.ContainsKey(mortality.Forecast.Guid))
              {
                mortality.Forecast.Changed = true;
                this._project_s.SaveOrUpdate((object) mortality.Forecast);
                dictionary.Add(mortality.Forecast.Guid, mortality.Forecast);
              }
            }
            transaction.Commit();
          }
          for (int index5 = intList.Count - 1; index5 >= 0; --index5)
          {
            int index6 = intList[index5];
            // ISSUE: reference to a compiler-generated field
            EventPublisher.Publish<EntityUpdated<Mortality>>(new EntityUpdated<Mortality>(new Guid(cDisplayClass140.aGrid.Rows[index6].Cells["GridMortalityGuid"].Value.ToString())), (Control) this);
            // ISSUE: reference to a compiler-generated field
            cDisplayClass140.aGrid.Rows.RemoveAt(index6);
          }
          if (dictionary.Count > 0)
          {
            foreach (KeyValuePair<Guid, Forecast> keyValuePair in dictionary)
              EventPublisher.Publish<EntityUpdated<Forecast>>(new EntityUpdated<Forecast>(keyValuePair.Value), (Control) this);
          }
          int num4 = (int) MessageBox.Show((IWin32Window) this, intList.Count.ToString() + " mortalities are replaced. ");
          // ISSUE: reference to a compiler-generated field
          if (cDisplayClass140.aGrid.Rows.Count == 0)
            this.tabControl1.TabPages.RemoveByKey("tabPageForecastMortalities");
        }
        this.btnReplace.Enabled = true;
        this.btnReplaceSelected.Enabled = true;
        if (this.tabControl1.TabPages.Count != 0)
          return;
        this.Close();
      }
    }

    private void GridView_DoubleClick(object sender, EventArgs e)
    {
      DataGridView dataGridView = (DataGridView) sender;
      string columnName1 = "GridTreeSpecies";
      string columnName2 = "GridTreeReplaceBy";
      Dictionary<string, string> dictionary = this._treeSynonymCultivar;
      bool GenusOnly = false;
      if (dataGridView == this.dataGridViewShrubs)
      {
        columnName1 = "GridShrubSpecies";
        columnName2 = "GridShrubReplaceBy";
        dictionary = this._shrubSynonymCultivar;
        GenusOnly = false;
      }
      else if (dataGridView == this.dataGridViewMortalities)
      {
        columnName1 = "GridMortalitySpecies";
        columnName2 = "GridMortalityReplaceBy";
        dictionary = this._mortalityTreeSynonymCultivar;
        GenusOnly = true;
      }
      SelectSpecies selectSpecies = new SelectSpecies();
      selectSpecies.InitializeForm(this._ps, dataGridView.CurrentRow.Cells[columnName1].Value.ToString(), dataGridView.CurrentRow.Cells[columnName2].Value.ToString(), GenusOnly);
      int num1 = (int) selectSpecies.ShowDialog((IWin32Window) this);
      string selectedSpeciesCode = selectSpecies.SelectedSpeciesCode;
      selectSpecies.Close();
      selectSpecies.Dispose();
      if (string.IsNullOrEmpty(selectedSpeciesCode))
        return;
      string str = dataGridView.CurrentRow.Cells[columnName1].Value.ToString();
      int length = str.IndexOf(" - ");
      string key = str.Substring(0, length).Trim();
      dictionary[key] = selectedSpeciesCode;
      int num2 = 0;
      for (int index = 0; index < dataGridView.Rows.Count; ++index)
      {
        if (str.Equals(dataGridView.Rows[index].Cells[columnName1].Value.ToString(), StringComparison.OrdinalIgnoreCase))
        {
          dataGridView.Rows[index].Cells[columnName2].Value = (object) (selectedSpeciesCode + " - " + this._ps.Species[selectedSpeciesCode].CommonScientificName);
          ++num2;
        }
      }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void SynonymOrCultivarForm_Resize(object sender, EventArgs e) => this.labelMessage.MaximumSize = new Size(this.ClientSize.Width, 0);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SynonymOrCultivarForm));
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      this.tabControl1 = new TabControl();
      this.tabPageTrees = new TabPage();
      this.dataGridViewTrees = new DataGridView();
      this.GridTreeGuid = new DataGridViewTextBoxColumn();
      this.GridTreePlotId = new DataGridViewTextBoxColumn();
      this.GridTreeTreeId = new DataGridViewTextBoxColumn();
      this.GridTreeSpecies = new DataGridViewTextBoxColumn();
      this.GridTreeReason = new DataGridViewTextBoxColumn();
      this.GridTreeReplaceBy = new DataGridViewTextBoxColumn();
      this.label1 = new Label();
      this.tabPageShrubs = new TabPage();
      this.dataGridViewShrubs = new DataGridView();
      this.GridShrubGuid = new DataGridViewTextBoxColumn();
      this.GridShrubPlotId = new DataGridViewTextBoxColumn();
      this.GridShrubShrubId = new DataGridViewTextBoxColumn();
      this.GridShrubSpecies = new DataGridViewTextBoxColumn();
      this.GridShrubReason = new DataGridViewTextBoxColumn();
      this.GridShrubReplaceBy = new DataGridViewTextBoxColumn();
      this.label2 = new Label();
      this.tabPageForecastMortalities = new TabPage();
      this.dataGridViewMortalities = new DataGridView();
      this.GridMortalityGuid = new DataGridViewTextBoxColumn();
      this.GridMortalityForecast = new DataGridViewTextBoxColumn();
      this.GridMortalityType = new DataGridViewTextBoxColumn();
      this.GridMortalitySpecies = new DataGridViewTextBoxColumn();
      this.GridMortalityReason = new DataGridViewTextBoxColumn();
      this.GridMortalityReplaceBy = new DataGridViewTextBoxColumn();
      this.label3 = new Label();
      this.panel2 = new Panel();
      this.btnReplaceSelected = new Button();
      this.btnCancel = new Button();
      this.btnReplace = new Button();
      this.panel1 = new Panel();
      this.labelMessage = new Label();
      this.panel3 = new Panel();
      this.labelMessage2 = new Label();
      this.tabControl1.SuspendLayout();
      this.tabPageTrees.SuspendLayout();
      ((ISupportInitialize) this.dataGridViewTrees).BeginInit();
      this.tabPageShrubs.SuspendLayout();
      ((ISupportInitialize) this.dataGridViewShrubs).BeginInit();
      this.tabPageForecastMortalities.SuspendLayout();
      ((ISupportInitialize) this.dataGridViewMortalities).BeginInit();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.tabControl1.Controls.Add((Control) this.tabPageTrees);
      this.tabControl1.Controls.Add((Control) this.tabPageShrubs);
      this.tabControl1.Controls.Add((Control) this.tabPageForecastMortalities);
      componentResourceManager.ApplyResources((object) this.tabControl1, "tabControl1");
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabPageTrees.Controls.Add((Control) this.dataGridViewTrees);
      this.tabPageTrees.Controls.Add((Control) this.label1);
      componentResourceManager.ApplyResources((object) this.tabPageTrees, "tabPageTrees");
      this.tabPageTrees.Name = "tabPageTrees";
      this.tabPageTrees.UseVisualStyleBackColor = true;
      this.dataGridViewTrees.AllowUserToAddRows = false;
      this.dataGridViewTrees.AllowUserToDeleteRows = false;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.dataGridViewTrees.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.dataGridViewTrees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewTrees.Columns.AddRange((DataGridViewColumn) this.GridTreeGuid, (DataGridViewColumn) this.GridTreePlotId, (DataGridViewColumn) this.GridTreeTreeId, (DataGridViewColumn) this.GridTreeSpecies, (DataGridViewColumn) this.GridTreeReason, (DataGridViewColumn) this.GridTreeReplaceBy);
      componentResourceManager.ApplyResources((object) this.dataGridViewTrees, "dataGridViewTrees");
      this.dataGridViewTrees.Name = "dataGridViewTrees";
      this.dataGridViewTrees.ReadOnly = true;
      this.dataGridViewTrees.RowTemplate.Height = 24;
      this.dataGridViewTrees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      componentResourceManager.ApplyResources((object) this.GridTreeGuid, "GridTreeGuid");
      this.GridTreeGuid.Name = "GridTreeGuid";
      this.GridTreeGuid.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridTreePlotId, "GridTreePlotId");
      this.GridTreePlotId.Name = "GridTreePlotId";
      this.GridTreePlotId.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridTreeTreeId, "GridTreeTreeId");
      this.GridTreeTreeId.Name = "GridTreeTreeId";
      this.GridTreeTreeId.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridTreeSpecies, "GridTreeSpecies");
      this.GridTreeSpecies.Name = "GridTreeSpecies";
      this.GridTreeSpecies.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridTreeReason, "GridTreeReason");
      this.GridTreeReason.Name = "GridTreeReason";
      this.GridTreeReason.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridTreeReplaceBy, "GridTreeReplaceBy");
      this.GridTreeReplaceBy.Name = "GridTreeReplaceBy";
      this.GridTreeReplaceBy.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      this.tabPageShrubs.Controls.Add((Control) this.dataGridViewShrubs);
      this.tabPageShrubs.Controls.Add((Control) this.label2);
      componentResourceManager.ApplyResources((object) this.tabPageShrubs, "tabPageShrubs");
      this.tabPageShrubs.Name = "tabPageShrubs";
      this.tabPageShrubs.UseVisualStyleBackColor = true;
      this.dataGridViewShrubs.AllowUserToAddRows = false;
      this.dataGridViewShrubs.AllowUserToDeleteRows = false;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Control;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.WindowText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.True;
      this.dataGridViewShrubs.ColumnHeadersDefaultCellStyle = gridViewCellStyle2;
      this.dataGridViewShrubs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewShrubs.Columns.AddRange((DataGridViewColumn) this.GridShrubGuid, (DataGridViewColumn) this.GridShrubPlotId, (DataGridViewColumn) this.GridShrubShrubId, (DataGridViewColumn) this.GridShrubSpecies, (DataGridViewColumn) this.GridShrubReason, (DataGridViewColumn) this.GridShrubReplaceBy);
      componentResourceManager.ApplyResources((object) this.dataGridViewShrubs, "dataGridViewShrubs");
      this.dataGridViewShrubs.Name = "dataGridViewShrubs";
      this.dataGridViewShrubs.ReadOnly = true;
      this.dataGridViewShrubs.RowTemplate.Height = 24;
      this.dataGridViewShrubs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      componentResourceManager.ApplyResources((object) this.GridShrubGuid, "GridShrubGuid");
      this.GridShrubGuid.Name = "GridShrubGuid";
      this.GridShrubGuid.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridShrubPlotId, "GridShrubPlotId");
      this.GridShrubPlotId.Name = "GridShrubPlotId";
      this.GridShrubPlotId.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridShrubShrubId, "GridShrubShrubId");
      this.GridShrubShrubId.Name = "GridShrubShrubId";
      this.GridShrubShrubId.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridShrubSpecies, "GridShrubSpecies");
      this.GridShrubSpecies.Name = "GridShrubSpecies";
      this.GridShrubSpecies.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridShrubReason, "GridShrubReason");
      this.GridShrubReason.Name = "GridShrubReason";
      this.GridShrubReason.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridShrubReplaceBy, "GridShrubReplaceBy");
      this.GridShrubReplaceBy.Name = "GridShrubReplaceBy";
      this.GridShrubReplaceBy.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      this.tabPageForecastMortalities.Controls.Add((Control) this.dataGridViewMortalities);
      this.tabPageForecastMortalities.Controls.Add((Control) this.label3);
      componentResourceManager.ApplyResources((object) this.tabPageForecastMortalities, "tabPageForecastMortalities");
      this.tabPageForecastMortalities.Name = "tabPageForecastMortalities";
      this.tabPageForecastMortalities.UseVisualStyleBackColor = true;
      this.dataGridViewMortalities.AllowUserToAddRows = false;
      this.dataGridViewMortalities.AllowUserToDeleteRows = false;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = SystemColors.Control;
      gridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.ForeColor = SystemColors.WindowText;
      gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.dataGridViewMortalities.ColumnHeadersDefaultCellStyle = gridViewCellStyle3;
      this.dataGridViewMortalities.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewMortalities.Columns.AddRange((DataGridViewColumn) this.GridMortalityGuid, (DataGridViewColumn) this.GridMortalityForecast, (DataGridViewColumn) this.GridMortalityType, (DataGridViewColumn) this.GridMortalitySpecies, (DataGridViewColumn) this.GridMortalityReason, (DataGridViewColumn) this.GridMortalityReplaceBy);
      componentResourceManager.ApplyResources((object) this.dataGridViewMortalities, "dataGridViewMortalities");
      this.dataGridViewMortalities.Name = "dataGridViewMortalities";
      this.dataGridViewMortalities.ReadOnly = true;
      this.dataGridViewMortalities.RowTemplate.Height = 24;
      this.dataGridViewMortalities.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      componentResourceManager.ApplyResources((object) this.GridMortalityGuid, "GridMortalityGuid");
      this.GridMortalityGuid.Name = "GridMortalityGuid";
      this.GridMortalityGuid.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridMortalityForecast, "GridMortalityForecast");
      this.GridMortalityForecast.Name = "GridMortalityForecast";
      this.GridMortalityForecast.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridMortalityType, "GridMortalityType");
      this.GridMortalityType.Name = "GridMortalityType";
      this.GridMortalityType.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridMortalitySpecies, "GridMortalitySpecies");
      this.GridMortalitySpecies.Name = "GridMortalitySpecies";
      this.GridMortalitySpecies.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridMortalityReason, "GridMortalityReason");
      this.GridMortalityReason.Name = "GridMortalityReason";
      this.GridMortalityReason.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.GridMortalityReplaceBy, "GridMortalityReplaceBy");
      this.GridMortalityReplaceBy.Name = "GridMortalityReplaceBy";
      this.GridMortalityReplaceBy.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      this.panel2.Controls.Add((Control) this.btnReplaceSelected);
      this.panel2.Controls.Add((Control) this.btnCancel);
      this.panel2.Controls.Add((Control) this.btnReplace);
      componentResourceManager.ApplyResources((object) this.panel2, "panel2");
      this.panel2.Name = "panel2";
      componentResourceManager.ApplyResources((object) this.btnReplaceSelected, "btnReplaceSelected");
      this.btnReplaceSelected.Name = "btnReplaceSelected";
      this.btnReplaceSelected.UseVisualStyleBackColor = true;
      this.btnReplaceSelected.Click += new EventHandler(this.btnReplaceSelected_Click);
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      componentResourceManager.ApplyResources((object) this.btnReplace, "btnReplace");
      this.btnReplace.Name = "btnReplace";
      this.btnReplace.UseVisualStyleBackColor = true;
      this.btnReplace.Click += new EventHandler(this.btnReplace_Click);
      this.panel1.Controls.Add((Control) this.panel3);
      this.panel1.Controls.Add((Control) this.panel2);
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.Name = "panel1";
      componentResourceManager.ApplyResources((object) this.labelMessage, "labelMessage");
      this.labelMessage.Name = "labelMessage";
      this.panel3.Controls.Add((Control) this.labelMessage2);
      componentResourceManager.ApplyResources((object) this.panel3, "panel3");
      this.panel3.Name = "panel3";
      componentResourceManager.ApplyResources((object) this.labelMessage2, "labelMessage2");
      this.labelMessage2.Name = "labelMessage2";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.labelMessage);
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (SynonymOrCultivarForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Load += new EventHandler(this.SynonymOrCultivarForm_Resize);
      this.Resize += new EventHandler(this.SynonymOrCultivarForm_Resize);
      this.tabControl1.ResumeLayout(false);
      this.tabPageTrees.ResumeLayout(false);
      this.tabPageTrees.PerformLayout();
      ((ISupportInitialize) this.dataGridViewTrees).EndInit();
      this.tabPageShrubs.ResumeLayout(false);
      this.tabPageShrubs.PerformLayout();
      ((ISupportInitialize) this.dataGridViewShrubs).EndInit();
      this.tabPageForecastMortalities.ResumeLayout(false);
      this.tabPageForecastMortalities.PerformLayout();
      ((ISupportInitialize) this.dataGridViewMortalities).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
