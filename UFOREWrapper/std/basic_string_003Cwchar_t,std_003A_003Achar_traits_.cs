// Decompiled with JetBrains decompiler
// Type: std.basic_string<wchar_t,std::char_traits<wchar_t>,std::allocator<wchar_t> >
// Assembly: UFOREWrapper, Version=1.0.5750.15158, Culture=neutral, PublicKeyToken=null
// MVID: A0E9D70D-C711-40C2-8444-67229D4F47C1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFOREWrapper.dll

using Microsoft.VisualC;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace std
{
  [UnsafeValueType]
  [DebugInfoInPDB]
  [MiscellaneousBits(64)]
  [NativeCppClass]
  [StructLayout(LayoutKind.Sequential, Size = 28)]
  internal struct basic_string\u003Cwchar_t\u002Cstd\u003A\u003Achar_traits\u003Cwchar_t\u003E\u002Cstd\u003A\u003Aallocator\u003Cwchar_t\u003E\u0020\u003E
  {
    [SpecialName]
    public static unsafe void \u003CMarshalCopy\u003E(
      basic_string\u003Cwchar_t\u002Cstd\u003A\u003Achar_traits\u003Cwchar_t\u003E\u002Cstd\u003A\u003Aallocator\u003Cwchar_t\u003E\u0020\u003E* _param0,
      basic_string\u003Cwchar_t\u002Cstd\u003A\u003Achar_traits\u003Cwchar_t\u003E\u002Cstd\u003A\u003Aallocator\u003Cwchar_t\u003E\u0020\u003E* _param1)
    {
      *(int*) ((IntPtr) _param0 + 20) = 7;
      *(int*) ((IntPtr) _param0 + 16) = 0;
      *(short*) _param0 = (short) 0;
      \u003CModule\u003E.std\u002Ebasic_string\u003Cwchar_t\u002Cstd\u003A\u003Achar_traits\u003Cwchar_t\u003E\u002Cstd\u003A\u003Aallocator\u003Cwchar_t\u003E\u0020\u003E\u002Eassign(_param0, _param1, 0U, uint.MaxValue);
    }

    [SpecialName]
    public static unsafe void \u003CMarshalDestroy\u003E(
      basic_string\u003Cwchar_t\u002Cstd\u003A\u003Achar_traits\u003Cwchar_t\u003E\u002Cstd\u003A\u003Aallocator\u003Cwchar_t\u003E\u0020\u003E* _param0)
    {
      if (8U <= (uint) *(int*) ((IntPtr) _param0 + 20))
        \u003CModule\u003E.delete((void*) *(int*) _param0);
      *(int*) ((IntPtr) _param0 + 20) = 7;
      *(int*) ((IntPtr) _param0 + 16) = 0;
      *(short*) _param0 = (short) 0;
    }
  }
}
