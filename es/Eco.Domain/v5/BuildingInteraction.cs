// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.BuildingInteraction
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;
using NHibernate;

namespace Eco.Domain.v5
{
  public class BuildingInteraction
  {
    private int? m_hash;
    private BuildingInteractionId m_compositeId;
    private Tree m_tree;

    public BuildingInteraction()
    {
    }

    public BuildingInteraction(BuildingInteractionId id) => this.m_compositeId = id;

    public virtual BuildingInteractionId CompositeId => this.m_compositeId;

    public virtual Tree Tree => this.m_tree;

    public virtual int Id => this.IsTransient ? 0 : this.m_compositeId.Building;

    public virtual short Direction { get; set; }

    public virtual float Distance { get; set; }

    public virtual bool IsTransient => this.m_compositeId == null;

    public override bool Equals(object obj)
    {
      if (!(obj is BuildingInteraction buildingInteraction) || this.IsTransient ^ buildingInteraction.IsTransient)
        return false;
      return this.IsTransient && buildingInteraction.IsTransient ? this == buildingInteraction : this.CompositeId.Equals((object) buildingInteraction.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }

    public virtual BuildingInteractionDTO GetDTO() => new BuildingInteractionDTO()
    {
      Id = this.Id,
      Direction = this.Direction,
      Distance = this.Distance,
      State = State.Existing
    };

    public static void Create(TreeId tree, BuildingInteractionDTO building, ISession s)
    {
      BuildingInteraction buildingInteraction = new BuildingInteraction(new BuildingInteractionId(tree.Location, tree.Series, tree.Year, tree.Plot, tree.SubPlot, tree.Tree, building.Id));
      buildingInteraction.Direction = building.Direction;
      buildingInteraction.Distance = building.Distance;
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Save((object) buildingInteraction);
        transaction.Commit();
      }
    }

    public static void Delete(TreeId tree, BuildingInteractionDTO building, ISession s)
    {
      BuildingInteraction buildingInteraction = new BuildingInteraction(new BuildingInteractionId(tree.Location, tree.Series, tree.Year, tree.Plot, tree.SubPlot, tree.Tree, building.Id));
      using (ITransaction transaction = s.BeginTransaction())
      {
        s.Delete((object) buildingInteraction);
        transaction.Commit();
      }
    }

    public static void Update(TreeId tree, BuildingInteractionDTO building, ISession s)
    {
      BuildingInteractionId id = new BuildingInteractionId(tree.Location, tree.Series, tree.Year, tree.Plot, tree.SubPlot, tree.Tree, building.Id);
      BuildingInteraction buildingInteraction = s.Get<BuildingInteraction>((object) id);
      using (ITransaction transaction = s.BeginTransaction())
      {
        buildingInteraction.Direction = building.Direction;
        buildingInteraction.Distance = building.Distance;
        transaction.Commit();
      }
    }
  }
}
