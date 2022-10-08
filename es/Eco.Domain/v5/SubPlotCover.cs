// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.SubPlotCover
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;
using NHibernate;

namespace Eco.Domain.v5
{
  public class SubPlotCover
  {
    private int? m_hash;
    private SubPlotCoverId m_compositeId;
    private SubPlot m_subPlot;
    private CoverType m_coverType;

    public SubPlotCover()
    {
    }

    public SubPlotCover(SubPlotCoverId id) => this.m_compositeId = id;

    public virtual SubPlotCoverId CompositeId => this.m_compositeId;

    public virtual SubPlot SubPlot => this.m_subPlot;

    public virtual CoverType CoverType => this.m_coverType;

    public virtual int Id => this.IsTransient ? 0 : this.CompositeId.Cover;

    public virtual int PercentCovered { get; set; }

    public virtual bool IsTransient => this.m_compositeId == null;

    public override bool Equals(object obj)
    {
      if (!(obj is SubPlotCover subPlotCover) || this.IsTransient ^ subPlotCover.IsTransient)
        return false;
      return this.IsTransient && subPlotCover.IsTransient ? this == subPlotCover : this.CompositeId.Equals((object) subPlotCover.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }

    public virtual SubPlotCoverDTO GetDTO() => new SubPlotCoverDTO()
    {
      Id = this.Id,
      PercentCovered = this.PercentCovered
    };

    internal static void Delete(SubPlotId subplot, SubPlotCoverDTO cover, ISession s)
    {
      SubPlotCoverId id = new SubPlotCoverId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, cover.Id);
      SubPlotCover subPlotCover = s.Get<SubPlotCover>((object) id);
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Delete((object) subPlotCover);
        transaction.Commit();
      }
    }

    internal static void Create(SubPlotId subplot, SubPlotCoverDTO cover, ISession s)
    {
      SubPlotCover subPlotCover = new SubPlotCover(new SubPlotCoverId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, cover.Id));
      subPlotCover.PercentCovered = cover.PercentCovered;
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Save((object) subPlotCover);
        transaction.Commit();
      }
    }

    internal static void Update(SubPlotId subplot, SubPlotCoverDTO cover, ISession s)
    {
      SubPlotCoverId id = new SubPlotCoverId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, cover.Id);
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Get<SubPlotCover>((object) id).PercentCovered = cover.PercentCovered;
        transaction.Commit();
      }
    }
  }
}
