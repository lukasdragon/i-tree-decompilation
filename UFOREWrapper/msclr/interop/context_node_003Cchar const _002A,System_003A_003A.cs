// Decompiled with JetBrains decompiler
// Type: msclr.interop.context_node<char const *,System::String ^>
// Assembly: UFOREWrapper, Version=1.0.5750.15158, Culture=neutral, PublicKeyToken=null
// MVID: A0E9D70D-C711-40C2-8444-67229D4F47C1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFOREWrapper.dll

using msclr.interop.details;
using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace msclr.interop
{
  internal class context_node\u003Cchar\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E : 
    context_node_base,
    IDisposable
  {
    private unsafe sbyte* _ptr;

    public unsafe context_node\u003Cchar\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E(
      sbyte** _to_object,
      string _from_object)
    {
      this._ptr = (sbyte*) 0;
      char_buffer\u003Cchar\u003E charBufferChar;
      if (_from_object == (string) null)
      {
        *(int*) _to_object = 0;
      }
      else
      {
        uint ansiStringSize = \u003CModule\u003E.msclr\u002Einterop\u002Edetails\u002EGetAnsiStringSize(_from_object);
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(int&) ref charBufferChar = (int) \u003CModule\u003E.new\u005B\u005D(ansiStringSize);
        // ISSUE: fault handler
        try
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          if (^(int&) ref charBufferChar == 0)
            throw new InsufficientMemoryException();
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          \u003CModule\u003E.msclr\u002Einterop\u002Edetails\u002EWriteAnsiString((sbyte*) ^(int&) ref charBufferChar, ansiStringSize, _from_object);
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          sbyte* numPtr = (sbyte*) ^(int&) ref charBufferChar;
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          ^(int&) ref charBufferChar = 0;
          this._ptr = numPtr;
          *(int*) _to_object = (int) numPtr;
        }
        __fault
        {
          // ISSUE: method pointer
          // ISSUE: cast to a function pointer type
          \u003CModule\u003E.___CxxCallUnwindDtor((__FnPtr<void (void*)>) __methodptr(msclr\u002Einterop\u002Edetails\u002Echar_buffer\u003Cchar\u003E\u002E\u007Bdtor\u007D), (void*) &charBufferChar);
        }
        \u003CModule\u003E.delete\u005B\u005D((void*) 0);
      }
      // ISSUE: fault handler
      try
      {
      }
      __fault
      {
        // ISSUE: method pointer
        // ISSUE: cast to a function pointer type
        \u003CModule\u003E.___CxxCallUnwindDtor((__FnPtr<void (void*)>) __methodptr(msclr\u002Einterop\u002Edetails\u002Echar_buffer\u003Cchar\u003E\u002E\u007Bdtor\u007D), (void*) &charBufferChar);
      }
    }

    private unsafe void \u007Econtext_node\u003Cchar\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E() => \u003CModule\u003E.delete\u005B\u005D((void*) this._ptr);

    private unsafe void \u0021context_node\u003Cchar\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E() => \u003CModule\u003E.delete\u005B\u005D((void*) this._ptr);

    [HandleProcessCorruptedStateExceptions]
    protected virtual unsafe void Dispose([MarshalAs(UnmanagedType.U1)] bool _param1)
    {
      if (_param1)
      {
        \u003CModule\u003E.delete\u005B\u005D((void*) this._ptr);
      }
      else
      {
        try
        {
          this.\u0021context_node\u003Cchar\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E();
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

    ~context_node\u003Cchar\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u0020\u005E\u003E() => this.Dispose(false);
  }
}
