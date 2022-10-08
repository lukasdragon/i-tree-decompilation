// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.VersionException
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Resources;
using System;

namespace i_Tree_Eco_v6
{
  public class VersionException : Exception
  {
    public VersionException(string component, Version vRequired, Version vInstalled)
      : this(component, vRequired, vInstalled, (Exception) null)
    {
    }

    public VersionException(
      string component,
      Version vRequired,
      Version vInstalled,
      Exception innerException)
      : base(string.Empty, innerException)
    {
      this.Component = component;
      this.VersionRequired = vRequired;
      this.VersionInstalled = vInstalled;
    }

    public override string Message => this.VersionInstalled == (Version) null ? string.Format(Strings.ExceptionVersionNotFound, (object) this.Component, (object) this.VersionRequired) : string.Format(Strings.ExceptionVersion, (object) this.Component, (object) this.VersionRequired.ToString(), (object) this.VersionInstalled.ToString());

    public string Component { get; private set; }

    public Version VersionRequired { get; private set; }

    public Version VersionInstalled { get; private set; }
  }
}
