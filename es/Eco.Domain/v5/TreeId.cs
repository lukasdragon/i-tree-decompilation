// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.TreeId
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v5
{
  public class TreeId : SubPlotId
  {
    private int m_tree;

    public TreeId()
    {
    }

    public TreeId(string location, string series, short year, int plot, int subplot, int tree)
      : base(location, series, year, plot, subplot)
    {
      this.m_tree = tree;
    }

    public virtual int Tree => this.m_tree;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      bool flag = base.Equals(obj);
      if (flag)
      {
        if (!(obj is TreeId treeId))
          return false;
        flag = this.Tree == treeId.Tree;
      }
      return flag;
    }

    public override int GetHashCode()
    {
      int num = 41;
      return ((((num * this.Location.GetHashCode() * num + this.Series.GetHashCode()) * num + this.Year.GetHashCode()) * num + this.Plot.GetHashCode()) * num + this.SubPlot.GetHashCode()) * num + this.Tree.GetHashCode();
    }
  }
}
