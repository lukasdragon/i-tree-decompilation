// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.ReferenceObject
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;
using NHibernate;

namespace Eco.Domain.v5
{
  public class ReferenceObject
  {
    private int? m_hash;
    private ReferenceObjectId m_compositeId;
    private SubPlot m_subPlot;

    public ReferenceObject()
    {
    }

    public ReferenceObject(ReferenceObjectId id) => this.m_compositeId = id;

    public virtual ReferenceObjectId CompositeId => this.m_compositeId;

    public virtual SubPlot SubPlot => this.m_subPlot;

    public virtual int Id => this.IsTransient ? 0 : this.m_compositeId.Direction;

    public virtual int Direction => this.m_compositeId.Direction;

    public virtual float Distance { get; set; }

    public virtual int Object { get; set; }

    public virtual float DBH { get; set; }

    public virtual string Notes { get; set; }

    public virtual bool IsTransient => this.m_compositeId == null;

    public virtual ReferenceObjectDTO GetDTO() => new ReferenceObjectDTO()
    {
      Id = this.Id,
      DBH = this.DBH,
      Distance = this.Distance,
      Notes = this.Notes,
      Object = this.Object
    };

    public override bool Equals(object obj)
    {
      if (!(obj is ReferenceObject referenceObject) || this.IsTransient ^ referenceObject.IsTransient)
        return false;
      return this.IsTransient && referenceObject.IsTransient ? this == referenceObject : this.CompositeId.Equals((object) referenceObject.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }

    public static void Delete(SubPlotId subplot, ReferenceObjectDTO refobj, ISession s)
    {
      ReferenceObjectId id = new ReferenceObjectId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, refobj.Id);
      ReferenceObject referenceObject = s.Get<ReferenceObject>((object) id);
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Delete((object) referenceObject);
        transaction.Commit();
      }
    }

    public static void Create(SubPlotId subplot, ReferenceObjectDTO refobj, ISession s)
    {
      ReferenceObject referenceObject = new ReferenceObject(new ReferenceObjectId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, refobj.Id));
      referenceObject.DBH = refobj.DBH;
      referenceObject.Distance = refobj.Distance;
      referenceObject.Notes = refobj.Notes;
      referenceObject.Object = refobj.Object;
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Save((object) referenceObject);
        transaction.Commit();
      }
    }

    public static void Update(SubPlotId subplot, ReferenceObjectDTO refobj, ISession s)
    {
      ReferenceObjectId id = new ReferenceObjectId(subplot.Location, subplot.Series, subplot.Year, subplot.Plot, subplot.SubPlot, refobj.Id);
      ReferenceObject referenceObject = s.Get<ReferenceObject>((object) id);
      using (ITransaction transaction = s.BeginTransaction())
      {
        referenceObject.DBH = refobj.DBH;
        referenceObject.Distance = refobj.Distance;
        referenceObject.Notes = refobj.Notes;
        referenceObject.Object = refobj.Object;
        transaction.Commit();
      }
    }
  }
}
