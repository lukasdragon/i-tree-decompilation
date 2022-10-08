// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.FieldLandUse
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;
using NHibernate;

namespace Eco.Domain.v5
{
  public class FieldLandUse
  {
    private int? m_hash;
    private FieldLandUseId m_compositeId;
    private SubPlot m_subPlot;

    public FieldLandUse()
    {
    }

    public FieldLandUse(FieldLandUseId id) => this.m_compositeId = id;

    public virtual FieldLandUseId CompositeId => this.m_compositeId;

    public virtual SubPlot SubPlot => this.m_subPlot;

    public virtual char Id => this.IsTransient ? char.MinValue : this.m_compositeId.FieldLandUse;

    public virtual short PercentOfSubPlot { get; set; }

    public virtual bool IsTransient => this.m_compositeId == null;

    public virtual FieldLandUseDTO GetDTO() => new FieldLandUseDTO()
    {
      Id = this.Id,
      PercentOfSubPlot = this.PercentOfSubPlot
    };

    public override bool Equals(object obj)
    {
      if (!(obj is FieldLandUse fieldLandUse) || this.IsTransient ^ fieldLandUse.IsTransient)
        return false;
      return this.IsTransient && fieldLandUse.IsTransient ? this == fieldLandUse : this.CompositeId.Equals((object) fieldLandUse.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }

    public static void Delete(SubPlotId subplot, FieldLandUseDTO landuse, ISession s)
    {
      FieldLandUseId id = new FieldLandUseId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, landuse.Id);
      FieldLandUse fieldLandUse = s.Get<FieldLandUse>((object) id);
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Delete((object) fieldLandUse);
        transaction.Commit();
      }
    }

    public static void Create(SubPlotId subplot, FieldLandUseDTO landuse, ISession s)
    {
      FieldLandUse fieldLandUse = new FieldLandUse(new FieldLandUseId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, landuse.Id));
      fieldLandUse.PercentOfSubPlot = landuse.PercentOfSubPlot;
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Save((object) fieldLandUse);
        transaction.Commit();
      }
    }

    public static void Update(SubPlotId subplot, FieldLandUseDTO landuse, ISession s)
    {
      FieldLandUseId id = new FieldLandUseId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, landuse.Id);
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Get<FieldLandUse>((object) id).PercentOfSubPlot = landuse.PercentOfSubPlot;
        transaction.Commit();
      }
    }
  }
}
