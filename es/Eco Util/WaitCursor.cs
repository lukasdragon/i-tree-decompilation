// Decompiled with JetBrains decompiler
// Type: Eco.Util.WaitCursor
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.Controls.Extensions;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Eco.Util
{
  public class WaitCursor
  {
    private int _count;
    private AutoResetEvent _showEvent;
    private AutoResetEvent _hideEvent;
    private Form _form;

    public WaitCursor(Form form)
    {
      this._form = form;
      this._showEvent = new AutoResetEvent(true);
      this._hideEvent = new AutoResetEvent(false);
    }

    public void Show()
    {
      if (Interlocked.Increment(ref this._count) != 1)
        return;
      this._showEvent.WaitOne();
      this.SetWaitCursor(true);
      this._hideEvent.Set();
    }

    public void Hide()
    {
      if (Interlocked.Decrement(ref this._count) != 0)
        return;
      this._hideEvent.WaitOne();
      this.SetWaitCursor(false);
      this.FixCursor(this._form, (Control) this._form);
      this._showEvent.Set();
    }

    private void SetWaitCursor(bool show)
    {
      if (this._form.IsHandleCreated)
      {
        if (this._form.InvokeRequired)
        {
          try
          {
            this._form.Invoke((Delegate) (() => this.SetWaitCursor(show)));
            return;
          }
          catch (ObjectDisposedException ex)
          {
            return;
          }
        }
      }
      this._form.ShowWaitCursor(show);
    }

    private void FixCursor(Form f, Control c)
    {
      try
      {
        if (this._form.IsDisposed || !this._form.IsHandleCreated)
          return;
        if (this._form.InvokeRequired)
          this._form.Invoke((Delegate) (() => this.FixCursor(f, c)));
        else if (c is DataGridView)
        {
          c.Cursor = f.Cursor;
        }
        else
        {
          Control.ControlCollection controls = c.Controls;
          for (int index = 0; index < controls.Count; ++index)
            this.FixCursor(f, controls[index]);
        }
      }
      catch (Exception ex)
      {
      }
    }
  }
}
