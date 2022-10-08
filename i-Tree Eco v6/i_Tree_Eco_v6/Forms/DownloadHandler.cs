// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.DownloadHandler
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using CefSharp;
using System;

namespace i_Tree_Eco_v6.Forms
{
  public class DownloadHandler : IDownloadHandler
  {
    public EventHandler<DownloadItem> BeforeDownload;
    public EventHandler<DownloadItem> DownloadUpdated;

    public void OnBeforeDownload(
      IWebBrowser chromiumWebBrowser,
      IBrowser browser,
      DownloadItem downloadItem,
      IBeforeDownloadCallback callback)
    {
      EventHandler<DownloadItem> beforeDownload = this.BeforeDownload;
      if (beforeDownload != null)
        beforeDownload((object) this, downloadItem);
      if (callback.IsDisposed)
        return;
      using (callback)
        callback.Continue(downloadItem.SuggestedFileName, true);
    }

    public void OnDownloadUpdated(
      IWebBrowser chromiumWebBrowser,
      IBrowser browser,
      DownloadItem downloadItem,
      IDownloadItemCallback callback)
    {
      EventHandler<DownloadItem> downloadUpdated = this.DownloadUpdated;
      if (downloadUpdated == null)
        return;
      downloadUpdated((object) this, downloadItem);
    }
  }
}
