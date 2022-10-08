// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.FieldManualGuideForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Forms.Resources;

namespace i_Tree_Eco_v6.Forms
{
  public class FieldManualGuideForm : CustomWebBrowserForm
  {
    protected override string Url => string.Format(UserGuideRes.ViewPdfURL, (object) UserGuideRes.FieldManualURL);
  }
}
