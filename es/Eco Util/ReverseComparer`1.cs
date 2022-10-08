// Decompiled with JetBrains decompiler
// Type: Eco.Util.ReverseComparer`1
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System.Collections.Generic;

namespace Eco.Util
{
  public class ReverseComparer<T> : IComparer<T>
  {
    public int Compare(T x, T y) => Comparer<T>.Default.Compare(y, x);
  }
}
