// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.Stem
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;
using NHibernate;

namespace Eco.Domain.v5
{
  public class Stem
  {
    private int? m_hash;
    private StemId m_compositeId;
    private Tree m_tree;

    public Stem()
    {
    }

    public Stem(StemId id) => this.m_compositeId = id;

    public virtual StemId CompositeId => this.m_compositeId;

    public virtual Tree Tree => this.m_tree;

    public virtual int Id => this.IsTransient ? 0 : this.m_compositeId.Stem;

    public virtual float Diameter { get; set; }

    public virtual float DiameterHeight { get; set; }

    public virtual bool Measured { get; set; }

    public virtual bool IsTransient => this.m_compositeId == null;

    public virtual StemDTO GetDTO() => new StemDTO()
    {
      Id = this.Id,
      Diameter = this.Diameter,
      DiameterHeight = this.DiameterHeight,
      Measured = this.Measured
    };

    public override bool Equals(object obj)
    {
      if (!(obj is Stem stem) || this.IsTransient ^ stem.IsTransient)
        return false;
      return this.IsTransient && stem.IsTransient ? this == stem : this.CompositeId.Equals((object) stem.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }

    public static void Create(TreeId tree, StemDTO stem, ISession s)
    {
      Stem stem1 = new Stem(new StemId(tree.Location, tree.Series, tree.Year, tree.Plot, tree.SubPlot, tree.Tree, stem.Id));
      stem1.Diameter = stem.Diameter;
      stem1.DiameterHeight = stem.DiameterHeight;
      stem1.Measured = stem.Measured;
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Save((object) stem1);
        transaction.Commit();
      }
    }

    public static void Delete(TreeId tree, StemDTO stem, ISession s)
    {
      Stem stem1 = new Stem(new StemId(tree.Location, tree.Series, tree.Year, tree.Plot, tree.SubPlot, tree.Tree, stem.Id));
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Delete((object) stem1);
        transaction.Commit();
      }
    }

    public static void Update(TreeId tree, StemDTO stem, ISession s)
    {
      StemId id = new StemId(tree.Location, tree.Series, tree.Year, tree.Plot, tree.SubPlot, tree.Tree, stem.Id);
      Stem stem1 = s.Get<Stem>((object) id);
      using (ITransaction transaction = s.BeginTransaction())
      {
        stem1.Diameter = stem.Diameter;
        stem1.DiameterHeight = stem.DiameterHeight;
        stem1.Measured = stem.Measured;
        transaction.Commit();
      }
    }
  }
}
