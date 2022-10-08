// Decompiled with JetBrains decompiler
// Type: UFOREWrapper.ParameterWrapper
// Assembly: UFOREWrapper, Version=1.0.5750.15158, Culture=neutral, PublicKeyToken=null
// MVID: A0E9D70D-C711-40C2-8444-67229D4F47C1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFOREWrapper.dll

using \u003CCppImplementationDetails\u003E;
using msclr.interop;
using System;
using System.Runtime.InteropServices;

namespace UFOREWrapper
{
  public class ParameterWrapper : IDisposable
  {
    private unsafe CParameterCalculatorObj* myParameterCalculator;

    public unsafe ParameterWrapper()
    {
      CParameterCalculatorObj* pThis = (CParameterCalculatorObj*) \u003CModule\u003E.@new(1760U);
      CParameterCalculatorObj* cparameterCalculatorObjPtr;
      // ISSUE: fault handler
      try
      {
        if ((IntPtr) pThis != IntPtr.Zero)
        {
          \u003CModule\u003E.CUforeProjectObj\u002E\u007Bctor\u007D((CUforeProjectObj*) pThis);
          // ISSUE: fault handler
          try
          {
            \u003CModule\u003E.CUforeErrorLogObj\u002E\u007Bctor\u007D((CUforeErrorLogObj*) ((IntPtr) pThis + 1752));
          }
          __fault
          {
            // ISSUE: method pointer
            // ISSUE: cast to a function pointer type
            \u003CModule\u003E.___CxxCallUnwindDtor((__FnPtr<void (void*)>) __methodptr(CUforeProjectObj\u002E\u007Bdtor\u007D), (void*) pThis);
          }
          cparameterCalculatorObjPtr = pThis;
        }
        else
          cparameterCalculatorObjPtr = (CParameterCalculatorObj*) 0;
      }
      __fault
      {
        \u003CModule\u003E.delete((void*) pThis);
      }
      this.myParameterCalculator = cparameterCalculatorObjPtr;
    }

    private unsafe void \u007EParameterWrapper()
    {
      CParameterCalculatorObj* parameterCalculator = this.myParameterCalculator;
      if ((IntPtr) parameterCalculator == IntPtr.Zero)
        return;
      // ISSUE: fault handler
      try
      {
        \u003CModule\u003E.CUforeErrorLogObj\u002E\u007Bdtor\u007D((CUforeErrorLogObj*) ((IntPtr) parameterCalculator + 1752));
      }
      __fault
      {
        // ISSUE: method pointer
        // ISSUE: cast to a function pointer type
        \u003CModule\u003E.___CxxCallUnwindDtor((__FnPtr<void (void*)>) __methodptr(CUforeProjectObj\u002E\u007Bdtor\u007D), (void*) parameterCalculator);
      }
      \u003CModule\u003E.CUforeProjectObj\u002E\u007Bdtor\u007D((CUforeProjectObj*) parameterCalculator);
      \u003CModule\u003E.delete((void*) parameterCalculator);
    }

    [return: MarshalAs(UnmanagedType.U1)]
    public unsafe bool runParameterCalculator(
      string errorLogFileName,
      string speciesDatabaseName,
      string locationDatabaseName,
      string inputDatabaseName,
      string inventoryDatabaseName,
      string configDefFileName,
      string location,
      string series,
      int year)
    {
      marshal_context marshalContext1 = new marshal_context();
      marshal_context marshalContext2;
      bool flag;
      // ISSUE: fault handler
      try
      {
        marshalContext2 = marshalContext1;
        \u0024ArrayType\u0024\u0024\u0024BY0HNA\u0040_W arrayTypeBy0HnaW;
        flag = \u003CModule\u003E.CParameterCalculatorObj\u002ErunParameterCalculator(this.myParameterCalculator, marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(errorLogFileName), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(speciesDatabaseName), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(locationDatabaseName), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(inputDatabaseName), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(inventoryDatabaseName), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(configDefFileName), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(location), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(series), (short) year, (char*) &arrayTypeBy0HnaW, 2000);
      }
      __fault
      {
        marshalContext2.Dispose();
      }
      marshalContext2.Dispose();
      return flag;
    }

    protected virtual unsafe void Dispose([MarshalAs(UnmanagedType.U1)] bool _param1)
    {
      if (_param1)
      {
        CParameterCalculatorObj* parameterCalculator = this.myParameterCalculator;
        if ((IntPtr) parameterCalculator == IntPtr.Zero)
          return;
        // ISSUE: fault handler
        try
        {
          \u003CModule\u003E.CUforeErrorLogObj\u002E\u007Bdtor\u007D((CUforeErrorLogObj*) ((IntPtr) parameterCalculator + 1752));
        }
        __fault
        {
          // ISSUE: method pointer
          // ISSUE: cast to a function pointer type
          \u003CModule\u003E.___CxxCallUnwindDtor((__FnPtr<void (void*)>) __methodptr(CUforeProjectObj\u002E\u007Bdtor\u007D), (void*) parameterCalculator);
        }
        \u003CModule\u003E.CUforeProjectObj\u002E\u007Bdtor\u007D((CUforeProjectObj*) parameterCalculator);
        \u003CModule\u003E.delete((void*) parameterCalculator);
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
  }
}
