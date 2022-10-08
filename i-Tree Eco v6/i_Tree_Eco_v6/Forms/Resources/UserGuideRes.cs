// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.Resources.UserGuideRes
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
  internal class UserGuideRes
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal UserGuideRes()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (UserGuideRes.resourceMan == null)
          UserGuideRes.resourceMan = new ResourceManager("i_Tree_Eco_v6.Forms.Resources.UserGuideRes", typeof (UserGuideRes).Assembly);
        return UserGuideRes.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => UserGuideRes.resourceCulture;
      set => UserGuideRes.resourceCulture = value;
    }

    internal static string ChangeLogURL => UserGuideRes.ResourceManager.GetString(nameof (ChangeLogURL), UserGuideRes.resourceCulture);

    internal static string CollectingCoordinateDataGuideURL => UserGuideRes.ResourceManager.GetString(nameof (CollectingCoordinateDataGuideURL), UserGuideRes.resourceCulture);

    internal static string DataLimitationsGuideURL => UserGuideRes.ResourceManager.GetString(nameof (DataLimitationsGuideURL), UserGuideRes.resourceCulture);

    internal static string Differences_v5v6GuideURL => UserGuideRes.ResourceManager.GetString(nameof (Differences_v5v6GuideURL), UserGuideRes.resourceCulture);

    internal static string ExampleProjectsGuideURL => UserGuideRes.ResourceManager.GetString(nameof (ExampleProjectsGuideURL), UserGuideRes.resourceCulture);

    internal static string FieldManualURL => UserGuideRes.ResourceManager.GetString(nameof (FieldManualURL), UserGuideRes.resourceCulture);

    internal static string ForecastGuideURL => UserGuideRes.ResourceManager.GetString(nameof (ForecastGuideURL), UserGuideRes.resourceCulture);

    internal static string InstallGuideURL => UserGuideRes.ResourceManager.GetString(nameof (InstallGuideURL), UserGuideRes.resourceCulture);

    internal static string InternationalProjectsGuideURL => UserGuideRes.ResourceManager.GetString(nameof (InternationalProjectsGuideURL), UserGuideRes.resourceCulture);

    internal static string InventoryImporterGuideURL => UserGuideRes.ResourceManager.GetString(nameof (InventoryImporterGuideURL), UserGuideRes.resourceCulture);

    internal static string iTreeMethodsURL => UserGuideRes.ResourceManager.GetString(nameof (iTreeMethodsURL), UserGuideRes.resourceCulture);

    internal static string PostStratifiedSamplesGuideURL => UserGuideRes.ResourceManager.GetString(nameof (PostStratifiedSamplesGuideURL), UserGuideRes.resourceCulture);

    internal static string PrestratifiedSamplesGuideURL => UserGuideRes.ResourceManager.GetString(nameof (PrestratifiedSamplesGuideURL), UserGuideRes.resourceCulture);

    internal static string StratifyingCompleteInventoryGuideURL => UserGuideRes.ResourceManager.GetString(nameof (StratifyingCompleteInventoryGuideURL), UserGuideRes.resourceCulture);

    internal static string UnstratifiedSamplesGuideURL => UserGuideRes.ResourceManager.GetString(nameof (UnstratifiedSamplesGuideURL), UserGuideRes.resourceCulture);

    internal static string UserManualURL => UserGuideRes.ResourceManager.GetString(nameof (UserManualURL), UserGuideRes.resourceCulture);

    internal static string ViewPdfURL => UserGuideRes.ResourceManager.GetString(nameof (ViewPdfURL), UserGuideRes.resourceCulture);
  }
}
