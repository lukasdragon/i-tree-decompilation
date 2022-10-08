// Decompiled with JetBrains decompiler
// Type: UFORE.EngineProgressArg
// Assembly: UFORE, Version=1.1.16.0, Culture=neutral, PublicKeyToken=null
// MVID: 58461BF3-EED8-45BF-BAF2-1F014CDA7667
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFORE.dll

using System;

namespace UFORE
{
  public class EngineProgressArg : EventArgs
  {
    public int Percent;
    public int CurrentStep;
    public int TotalSteps;
    public string Description;
  }
}
