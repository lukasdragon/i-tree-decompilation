// Decompiled with JetBrains decompiler
// Type: TreeEnergy.IndividualTreeException
// Assembly: TreeEnergy, Version=1.1.6718.0, Culture=neutral, PublicKeyToken=null
// MVID: 999CE94F-70B0-42EB-8D64-3ADB949CAFD0
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\TreeEnergy.dll

using System;

namespace TreeEnergy
{
  public class IndividualTreeException : TreeEnergyException
  {
    public IndividualTreeException()
    {
    }

    public IndividualTreeException(string message)
      : base(message)
    {
    }

    public IndividualTreeException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
