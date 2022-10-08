// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ModelProcessingNotes
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using NHibernate.Transform;
using System;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  internal class ModelProcessingNotes : DatabaseReport
  {
    public ModelProcessingNotes() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleModelProcessingNotes;

    public override object GetData() => (object) this.curInputISession.GetNamedQuery(nameof (ModelProcessingNotes)).SetParameter<Guid>("y", this.YearGuid).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      C1doc.ClipPage = true;
      DataTableReader dataReader = ((DataTable) this.GetData()).CreateDataReader();
      int ordinal = dataReader.GetOrdinal("ParameterCalculatorNote");
      dataReader.GetOrdinal("EstimatorNote");
      if (!dataReader.Read())
        return;
      string str = ReportUtil.ConvertFromDBVal<string>(dataReader[ordinal]);
      if (!string.IsNullOrEmpty(str))
      {
        RenderText ro1 = new RenderText(str.Replace("\f", ""));
        ro1.Style.Font = new Font("Courier New", 8f);
        ro1.BreakAfter = BreakEnum.None;
        ro1.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
        ro1.Width = Unit.Auto;
        C1doc.Body.Children.Add((RenderObject) ro1);
        RenderTable ro2 = new RenderTable();
        Style style = ro2.Style.Children.Add();
        style.BackColor = Color.FromArgb(217, 217, 217);
        ro2.Cells[0, 0].SpanCols = 3;
        ro2.Cells[0, 0].Text = "Key Words and Interpretation of Notes";
        ro2.Rows[0].Style.FontBold = true;
        ro2.Rows[0].Style.FontSize = 16f;
        ro2.Cells[0, 0].Style.TextAlignHorz = AlignHorzEnum.Center;
        int num1 = 0 + 1;
        ro2.Rows[num1].Style.FontBold = true;
        ro2.Rows[num1].Style.FontSize = 12f;
        ro2.Cells[num1, 0].Text = "Error Message";
        ro2.Cells[num1, 1].Text = "Description";
        ro2.Cells[num1, 2].Text = "To Correct";
        ro2.Cols[0].CellStyle.Padding.Right = (Unit) "1mm";
        ro2.Cols[1].CellStyle.Padding.Right = (Unit) "1mm";
        int num2 = num1 + 1;
        ro2.Cells[num2, 0].Text = "Error: Code 'species code' - has a leaf area to biomass entry but no corresponding group entry - leaf area to biomass entry deleted.";
        ro2.Cells[num2, 1].Text = "The species code is not assigned to a tree biomass group in the species database.";
        ro2.Cells[num2, 2].Text = "Contact i-Tree staff for more information.";
        if (num2 % 2 == 0)
          ro2.Rows[num2].Style.Parent = style;
        int num3 = num2 + 1;
        ro2.Cells[num3, 0].Text = "Error: Code 'Species code' - has a tree value entry but no corresponding group entry - tree value entry deleted.";
        ro2.Cells[num3, 1].Text = "The species code is missing information in the species database.";
        ro2.Cells[num3, 2].Text = "Contact i-Tree staff for more information.";
        if (num3 % 2 == 0)
          ro2.Rows[num3].Style.Parent = style;
        int num4 = num3 + 1;
        ro2.Cells[num4, 0].Text = "Error: Field plot 'number' has no corresponding map land use data - plot deleted.";
        ro2.Cells[num4, 1].Text = "The plot does not have a strata entry associated with it.";
        ro2.Cells[num4, 2].Text = "Edit plot and assign a strata.";
        if (num4 % 2 == 0)
          ro2.Rows[num4].Style.Parent = style;
        int num5 = num4 + 1;
        ro2.Cells[num5, 0].Text = "Error: No ufore to energy region mapping for 'city name'";
        ro2.Cells[num5, 1].Text = "There is no climate zone entry for the city in the location database.";
        ro2.Cells[num5, 2].Text = "Contact i-Tree staff for more information.";
        if (num5 % 2 == 0)
          ro2.Rows[num5].Style.Parent = style;
        int num6 = num5 + 1;
        ro2.Cells[num6, 0].Text = "Warning: 'species code' - code missing from species list - renamed other.";
        ro2.Cells[num6, 1].Text = "The species list used by the model is missing the species code.  These trees have been renamed 'Other' and are equivalent to unknown species. Default, generic species values are used to calculate benefit estimates.";
        ro2.Cells[num6, 2].Text = "Check the species code list available at www.itreetools.org under Resources. Corrections can be made using the data entry form under Data > Plots or Data > Trees. If the species is not in the species code list, please contact i-Tree support.";
        if (num6 % 2 == 0)
          ro2.Rows[num6].Style.Parent = style;
        int num7 = num6 + 1;
        ro2.Cells[num7, 0].Text = "Notice: Tree species 'species code,' number 'number' has an illegal CLE value - leaf area factor set to 1.";
        ro2.Cells[num7, 1].Text = "Tree crown light exposure is missing or not in the accepted range of 0-5. Crown light exposure is used to determine a leaf area factor.";
        ro2.Cells[num7, 2].Text = "CLE value can be changed in the Trees table of the input database you created at the beginning of the project.";
        if (num7 % 2 == 0)
          ro2.Rows[num7].Style.Parent = style;
        int num8 = num7 + 1;
        ro2.Cells[num8, 0].Text = "Notice: No base values for state - values not calculated.";
        ro2.Cells[num8, 1].Text = "The base value to calculate compensatory value is missing for the state. Projects outside the U.S. will receive this message.";
        ro2.Cells[num8, 2].Text = "International projects will be manually processed. U.S. projects receiving this message should contact i-Tree support staff.";
        if (num8 % 2 == 0)
          ro2.Rows[num8].Style.Parent = style;
        ro2.BreakBefore = BreakEnum.Page;
        C1doc.Body.Children.Add((RenderObject) ro2);
      }
      else
      {
        RenderText ro = new RenderText(i_Tree_Eco_v6.Resources.Strings.MsgNoErrors);
        C1doc.Body.Children.Add((RenderObject) ro);
      }
    }

    public override void SetAlignment(C1PrintDocument C1doc) => C1doc.Style.FlowAlignChildren = FlowAlignEnum.Center;
  }
}
