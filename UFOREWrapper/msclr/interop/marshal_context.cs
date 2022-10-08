// Decompiled with JetBrains decompiler
// Type: msclr.interop.marshal_context
// Assembly: UFOREWrapper, Version=1.0.5750.15158, Culture=neutral, PublicKeyToken=null
// MVID: A0E9D70D-C711-40C2-8444-67229D4F47C1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFOREWrapper.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace msclr.interop
{
  internal class marshal_context : IDisposable
  {
    internal readonly LinkedList<object> _clean_up_list = new LinkedList<object>();

    public unsafe char* marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(
      string _from)
    {
      return marshal_context.internal_marshaler\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u002C1\u003E.marshal_as(&_from, this._clean_up_list);
    }

    private void \u007Emarshal_context()
    {
      LinkedList<object>.Enumerator enumerator = this._clean_up_list.GetEnumerator();
      if (!enumerator.MoveNext())
        return;
      do
      {
        if (enumerator.Current is IDisposable current)
          current.Dispose();
      }
      while (enumerator.MoveNext());
    }

    protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool _param1)
    {
      if (_param1)
      {
        this.\u007Emarshal_context();
      }
      else
      {
        // ISSUE: explicit finalizer call
        this.Finalize();
      }
    }

    public virtual void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public class internal_marshaler\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u002C1\u003E
    {
      public static unsafe char* marshal_as(string* _from, LinkedList<object> _clean_up_list)
      {
        char* chPtr;
        context_node\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E constSystemString = new context_node\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E(&chPtr, (string) *(object*) _from);
        _clean_up_list.AddLast((object) constSystemString);
        return chPtr;
      }
    }
  }
}
