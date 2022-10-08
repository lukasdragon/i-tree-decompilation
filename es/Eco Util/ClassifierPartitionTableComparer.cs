// Decompiled with JetBrains decompiler
// Type: Eco.Util.ClassifierPartitionTableComparer
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Eco.Util
{
  public class ClassifierPartitionTableComparer : 
    IEqualityComparer<Tuple<EstimateDataTypes, List<Classifiers>>>
  {
    public bool Equals(
      Tuple<EstimateDataTypes, List<Classifiers>> x,
      Tuple<EstimateDataTypes, List<Classifiers>> y)
    {
      return x.Item1 == y.Item1 && x.Item2.SequenceEqual<Classifiers>((IEnumerable<Classifiers>) y.Item2);
    }

    public int GetHashCode(Tuple<EstimateDataTypes, List<Classifiers>> obj)
    {
      int hashCode = obj.Item1.GetHashCode();
      foreach (int num in obj.Item2)
        hashCode += num.GetHashCode();
      return hashCode;
    }
  }
}
