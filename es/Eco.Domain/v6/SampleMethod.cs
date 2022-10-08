// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.SampleMethod
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.Attributes;
using Eco.Domain.Properties;

namespace Eco.Domain.v6
{
  public enum SampleMethod
  {
    [LocalizedDescription(typeof (v6Strings), "SampleMethod_None")] None,
    [LocalizedDescription(typeof (v6Strings), "SampleMethod_SimpleRandom")] SimpleRandom,
    [LocalizedDescription(typeof (v6Strings), "SampleMethod_StratifiedRandom")] StratifiedRandom,
    [LocalizedDescription(typeof (v6Strings), "SampleMethod_FixedGrid")] FixedGrid,
    [LocalizedDescription(typeof (v6Strings), "SampleMethod_RandomizedGrid")] RandomizedGrid,
    [LocalizedDescription(typeof (v6Strings), "SampleMethod_Linear")] Linear,
  }
}
