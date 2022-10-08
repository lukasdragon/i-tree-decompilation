// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.Resources.TabPageSplashTextRes
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace i_Tree_Eco_v6.Forms.Resources
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class TabPageSplashTextRes
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal TabPageSplashTextRes()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (TabPageSplashTextRes.resourceMan == null)
          TabPageSplashTextRes.resourceMan = new ResourceManager("i_Tree_Eco_v6.Forms.Resources.TabPageSplashTextRes", typeof (TabPageSplashTextRes).Assembly);
        return TabPageSplashTextRes.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => TabPageSplashTextRes.resourceCulture;
      set => TabPageSplashTextRes.resourceCulture = value;
    }

    internal static string LaunchSplash => TabPageSplashTextRes.ResourceManager.GetString(nameof (LaunchSplash), TabPageSplashTextRes.resourceCulture);

    internal static string rtData => TabPageSplashTextRes.ResourceManager.GetString(nameof (rtData), TabPageSplashTextRes.resourceCulture);

    internal static string rtForecast => TabPageSplashTextRes.ResourceManager.GetString(nameof (rtForecast), TabPageSplashTextRes.resourceCulture);

    internal static string rtProject => TabPageSplashTextRes.ResourceManager.GetString(nameof (rtProject), TabPageSplashTextRes.resourceCulture);

    internal static string rtProject_full => TabPageSplashTextRes.ResourceManager.GetString(nameof (rtProject_full), TabPageSplashTextRes.resourceCulture);

    internal static string rtReports => TabPageSplashTextRes.ResourceManager.GetString(nameof (rtReports), TabPageSplashTextRes.resourceCulture);

    internal static string rtSupport => TabPageSplashTextRes.ResourceManager.GetString(nameof (rtSupport), TabPageSplashTextRes.resourceCulture);

    internal static string rtView => TabPageSplashTextRes.ResourceManager.GetString(nameof (rtView), TabPageSplashTextRes.resourceCulture);
  }
}
