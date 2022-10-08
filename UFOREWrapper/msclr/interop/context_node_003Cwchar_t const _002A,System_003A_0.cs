// Decompiled with JetBrains decompiler
// Type: msclr.interop.context_node<wchar_t const *,System::String ^>
// Assembly: UFOREWrapper, Version=1.0.5750.15158, Culture=neutral, PublicKeyToken=null
// MVID: A0E9D70D-C711-40C2-8444-67229D4F47C1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFOREWrapper.dll

using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace msclr.interop
{
  internal class context_node\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E : 
    context_node_base,
    IDisposable
  {
    private IntPtr _ip;

    public unsafe context_node\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E(
      char** _to_object,
      string _from_object)
    {
      this._ip = Marshal.StringToHGlobalUni(_from_object);
      *(int*) _to_object = (int) this._ip.ToPointer();
    }

    private void \u007Econtext_node\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E()
    {
      if (!(this._ip != IntPtr.Zero))
        return;
      Marshal.FreeHGlobal(this._ip);
    }

    private void \u0021context_node\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E()
    {
      if (!(this._ip != IntPtr.Zero))
        return;
      Marshal.FreeHGlobal(this._ip);
    }

    [HandleProcessCorruptedStateExceptions]
    protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool _param1)
    {
      if (_param1)
      {
        if (!(this._ip != IntPtr.Zero))
          return;
        Marshal.FreeHGlobal(this._ip);
      }
      else
      {
        try
        {
          this.\u0021context_node\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E();
        }
        finally
        {
          // ISSUE: explicit finalizer call
          base.Finalize();
        }
      }
    }

    public virtual void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    ~context_node\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E() => this.Dispose(false);
  }
}
