// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.Resources.TabPageSplashImageRes
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace i_Tree_Eco_v6.Forms.Resources
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class TabPageSplashImageRes
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal TabPageSplashImageRes()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (TabPageSplashImageRes.resourceMan == null)
          TabPageSplashImageRes.resourceMan = new ResourceManager("i_Tree_Eco_v6.Forms.Resources.TabPageSplashImageRes", typeof (TabPageSplashImageRes).Assembly);
        return TabPageSplashImageRes.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => TabPageSplashImageRes.resourceCulture;
      set => TabPageSplashImageRes.resourceCulture = value;
    }

    internal static Bitmap LaunchSplash => (Bitmap) TabPageSplashImageRes.ResourceManager.GetObject(nameof (LaunchSplash), TabPageSplashImageRes.resourceCulture);

    internal static Bitmap rtData => (Bitmap) TabPageSplashImageRes.ResourceManager.GetObject(nameof (rtData), TabPageSplashImageRes.resourceCulture);

    internal static Bitmap rtForecast => (Bitmap) TabPageSplashImageRes.ResourceManager.GetObject(nameof (rtForecast), TabPageSplashImageRes.resourceCulture);

    internal static Bitmap rtProject => (Bitmap) TabPageSplashImageRes.ResourceManager.GetObject(nameof (rtProject), TabPageSplashImageRes.resourceCulture);

    internal static Bitmap rtReports => (Bitmap) TabPageSplashImageRes.ResourceManager.GetObject(nameof (rtReports), TabPageSplashImageRes.resourceCulture);

    internal static Bitmap rtSupport => (Bitmap) TabPageSplashImageRes.ResourceManager.GetObject(nameof (rtSupport), TabPageSplashImageRes.resourceCulture);

    internal static Bitmap rtView => (Bitmap) TabPageSplashImageRes.ResourceManager.GetObject(nameof (rtView), TabPageSplashImageRes.resourceCulture);
  }
}
