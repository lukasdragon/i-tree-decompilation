// Decompiled with JetBrains decompiler
// Type: TreeEnergy.TreeEnergyException
// Assembly: TreeEnergy, Version=1.1.6718.0, Culture=neutral, PublicKeyToken=null
// MVID: 999CE94F-70B0-42EB-8D64-3ADB949CAFD0
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\TreeEnergy.dll

using System;

namespace TreeEnergy
{
  public class TreeEnergyException : Exception
  {
    public TreeEnergyException()
    {
    }

    public TreeEnergyException(string message)
      : base(message)
    {
    }

    public TreeEnergyException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
