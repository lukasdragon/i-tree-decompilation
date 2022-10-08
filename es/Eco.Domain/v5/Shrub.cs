// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.Shrub
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;
using NHibernate;

namespace Eco.Domain.v5
{
  public class Shrub
  {
    private int? m_hash;
    private ShrubId m_compositeId;
    private SubPlot m_subPlot;

    public Shrub()
    {
    }

    public Shrub(ShrubId id) => this.m_compositeId = id;

    public virtual ShrubId CompositeId => this.m_compositeId;

    public virtual SubPlot SubPlot => this.m_subPlot;

    public virtual int Id => this.IsTransient ? 0 : this.m_compositeId.Shrub;

    public virtual int PercentOfShrubArea { get; set; }

    public virtual string Species { get; set; }

    public virtual float Height { get; set; }

    public virtual int PercentMissing { get; set; }

    public virtual string Comments { get; set; }

    public virtual bool IsTransient => this.m_compositeId == null;

    public virtual ShrubDTO GetDTO() => new ShrubDTO()
    {
      Id = this.Id,
      Comments = this.Comments,
      Height = this.Height,
      PercentMissing = this.PercentMissing,
      PercentOfShrubArea = this.PercentOfShrubArea,
      Species = this.Species
    };

    public override bool Equals(object obj)
    {
      if (!(obj is Shrub shrub) || this.IsTransient ^ shrub.IsTransient)
        return false;
      return this.IsTransient && shrub.IsTransient ? this == shrub : this.CompositeId.Equals((object) shrub.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }

    public static void Delete(SubPlotId subplot, ShrubDTO shrub, ISession s)
    {
      ShrubId id = new ShrubId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, shrub.Id);
      Shrub shrub1 = s.Get<Shrub>((object) id);
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Delete((object) shrub1);
        transaction.Commit();
      }
    }

    public static void Create(SubPlotId subplot, ShrubDTO shrub, ISession s)
    {
      Shrub shrub1 = new Shrub(new ShrubId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, shrub.Id));
      shrub1.Comments = shrub.Comments;
      shrub1.Height = shrub.Height;
      shrub1.PercentMissing = shrub.PercentMissing;
      shrub1.PercentOfShrubArea = shrub.PercentOfShrubArea;
      shrub1.Species = shrub.Species;
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Save((object) shrub1);
        transaction.Commit();
      }
    }

    public static void Update(SubPlotId subplot, ShrubDTO shrub, ISession s)
    {
      ShrubId id = new ShrubId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, shrub.Id);
      Shrub shrub1 = s.Get<Shrub>((object) id);
      using (ITransaction transaction = s.BeginTransaction())
      {
        shrub1.Comments = shrub.Comments;
        shrub1.Height = shrub.Height;
        shrub1.PercentMissing = shrub.PercentMissing;
        shrub1.PercentOfShrubArea = shrub.PercentOfShrubArea;
        shrub1.Species = shrub.Species;
        transaction.Commit();
      }
    }
  }
}
