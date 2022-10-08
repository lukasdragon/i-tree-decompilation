// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.LifeSpanHandler
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using CefSharp;
using System.Diagnostics;

namespace i_Tree_Eco_v6.Forms
{
  public class LifeSpanHandler : ILifeSpanHandler
  {
    public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser) => false;

    public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
    {
    }

    public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
    {
    }

    public bool OnBeforePopup(
      IWebBrowser chromiumWebBrowser,
      IBrowser browser,
      IFrame frame,
      string targetUrl,
      string targetFrameName,
      WindowOpenDisposition targetDisposition,
      bool userGesture,
      IPopupFeatures popupFeatures,
      IWindowInfo windowInfo,
      IBrowserSettings browserSettings,
      ref bool noJavascriptAccess,
      out IWebBrowser newBrowser)
    {
      Process.Start(targetUrl);
      newBrowser = (IWebBrowser) null;
      return true;
    }
  }
}
