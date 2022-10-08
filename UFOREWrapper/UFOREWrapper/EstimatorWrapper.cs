// Decompiled with JetBrains decompiler
// Type: UFOREWrapper.EstimatorWrapper
// Assembly: UFOREWrapper, Version=1.0.5750.15158, Culture=neutral, PublicKeyToken=null
// MVID: A0E9D70D-C711-40C2-8444-67229D4F47C1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\UFOREWrapper.dll

using \u003CCppImplementationDetails\u003E;
using msclr.interop;
using System;
using System.Runtime.InteropServices;

namespace UFOREWrapper
{
  public class EstimatorWrapper : IDisposable
  {
    private unsafe OverallStatisticEstimatorObj* myOverallStatisticEstimatorObj;

    public unsafe EstimatorWrapper()
    {
      OverallStatisticEstimatorObj* statisticEstimatorObjPtr1 = (OverallStatisticEstimatorObj*) \u003CModule\u003E.@new(1U);
      OverallStatisticEstimatorObj* statisticEstimatorObjPtr2;
      if ((IntPtr) statisticEstimatorObjPtr1 != IntPtr.Zero)
      {
        // ISSUE: initblk instruction
        __memset((IntPtr) statisticEstimatorObjPtr1, 0, 1);
        statisticEstimatorObjPtr2 = statisticEstimatorObjPtr1;
      }
      else
        statisticEstimatorObjPtr2 = (OverallStatisticEstimatorObj*) 0;
      this.myOverallStatisticEstimatorObj = statisticEstimatorObjPtr2;
    }

    private unsafe void \u007EEstimatorWrapper() => \u003CModule\u003E.delete((void*) this.myOverallStatisticEstimatorObj);

    [return: MarshalAs(UnmanagedType.U1)]
    public unsafe bool runStatisticEstimator(
      string errorLogFileName,
      string speciesDatabaseName,
      string locationDatabaseName,
      string pestDBName,
      string inputDatabaseName,
      string inventoryDatabaseName,
      string estimateDBFileName,
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
        flag = \u003CModule\u003E.OverallStatisticEstimatorObj\u002ErunStatisticEstimator(this.myOverallStatisticEstimatorObj, marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(errorLogFileName), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(speciesDatabaseName), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(locationDatabaseName), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(pestDBName), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(inputDatabaseName), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(inventoryDatabaseName), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(estimateDBFileName), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(location), marshalContext2.marshal_as\u003Cwchar_t\u0020const\u0020\u002A\u002CSystem\u003A\u003AString\u003E(series), (short) year, (char*) &arrayTypeBy0HnaW, 2000);
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
        \u003CModule\u003E.delete((void*) this.myOverallStatisticEstimatorObj);
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
