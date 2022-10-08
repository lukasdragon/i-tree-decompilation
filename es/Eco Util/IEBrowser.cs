// Decompiled with JetBrains decompiler
// Type: Eco.Util.IEBrowser
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Microsoft.Win32;
using System;

namespace Eco.Util
{
  public static class IEBrowser
  {
    public static Version Version
    {
      get
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer"))
        {
          object obj = registryKey.GetValue("svcVersion") ?? registryKey.GetValue(nameof (Version));
          if (obj != null)
            return new Version(obj.ToString());
        }
        return (Version) null;
      }
    }
  }
}
