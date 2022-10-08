// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.CoverType
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;

namespace Eco.Domain.v5
{
  public class CoverType
  {
    private int? m_hash;
    private CoverTypeId m_compositeId;
    private Project m_project;

    public CoverType()
    {
    }

    public CoverType(CoverTypeId id) => this.m_compositeId = id;

    public virtual CoverTypeId CompositeId => this.m_compositeId;

    public virtual Project Project => this.m_project;

    public virtual int Id => this.IsTransient ? 0 : this.m_compositeId.Cover;

    public virtual string Description { get; set; }

    public virtual bool IsTransient => this.m_compositeId == null;

    public override bool Equals(object obj)
    {
      if (!(obj is CoverType coverType) || this.IsTransient ^ coverType.IsTransient)
        return false;
      return this.IsTransient && coverType.IsTransient ? this == coverType : this.CompositeId.Equals((object) coverType.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }

    public virtual CoverTypeDTO GetDTO() => new CoverTypeDTO()
    {
      Id = this.Id,
      Description = this.Description
    };
  }
}
