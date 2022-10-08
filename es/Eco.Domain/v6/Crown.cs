// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Crown
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO;
using Eco.Domain.DTO.v6;
using System;

namespace Eco.Domain.v6
{
  public class Crown
  {
    public Crown()
    {
      this.BaseHeight = -1f;
      this.TopHeight = -1f;
      this.WidthNS = -1f;
      this.WidthEW = -1f;
      this.LightExposure = CrownLightExposure.NotEntered;
      this.PercentMissing = PctMidRange.PRINV;
    }

    public virtual float BaseHeight { get; set; }

    public virtual float TopHeight { get; set; }

    public virtual float WidthNS { get; set; }

    public virtual float WidthEW { get; set; }

    public virtual CrownLightExposure LightExposure { get; set; }

    public virtual PctMidRange PercentMissing { get; set; }

    public virtual Condition Condition { get; set; }

    public virtual CrownDTO GetDTO() => new CrownDTO()
    {
      BaseHeight = Util.NullIfDefault(this.BaseHeight, -1f),
      TopHeight = Util.NullIfDefault(this.TopHeight, -1f),
      WidthNS = Util.NullIfDefault(this.WidthNS, -1f),
      WidthEW = Util.NullIfDefault(this.WidthEW, -1f),
      LightExposure = Util.NullIfDefault<int>((int) this.LightExposure, -1),
      PercentMissing = Util.NullIfDefault<int>((int) this.PercentMissing, -1),
      Condition = this.Condition != null ? new Guid?(this.Condition.Guid) : new Guid?()
    };

    public virtual Crown Clone() => Crown.Clone(this, new EntityMap());

    public virtual Crown Clone(bool deep) => Crown.Clone(this, new EntityMap(), deep);

    internal static Crown Clone(Crown c, EntityMap map, bool deep = true)
    {
      Crown crown = new Crown();
      crown.BaseHeight = c.BaseHeight;
      crown.TopHeight = c.TopHeight;
      crown.WidthNS = c.WidthNS;
      crown.WidthEW = c.WidthEW;
      crown.LightExposure = c.LightExposure;
      crown.PercentMissing = c.PercentMissing;
      if (deep)
        crown.Condition = map.GetEntity<Condition>(c.Condition);
      return crown;
    }
  }
}
