// Decompiled with JetBrains decompiler
// Type: Eco.Util.RetryExecutionHandler
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.Controls;
using Spring.Retry.Retry;
using Spring.Retry.Retry.Backoff;
using Spring.Retry.Retry.Policy;
using Spring.Retry.Retry.Support;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Eco.Util
{
  public class RetryExecutionHandler : IExecutionHandler
  {
    private static RetryExecutionHandler _instance;
    private readonly RetryTemplate _retry;

    private RetryExecutionHandler() => this._retry = new RetryTemplate()
    {
      BackOffPolicy = (IBackOffPolicy) new ExponentialBackOffPolicy(),
      RetryPolicy = (IRetryPolicy) new SimpleRetryPolicy()
      {
        MaxAttempts = 5
      }
    };

    void IExecutionHandler.Execute(System.Action action)
    {
      while (true)
      {
        try
        {
          this._retry.Execute<object>((Func<IRetryContext, object>) (context => this.DoExecute(context, action)));
          break;
        }
        catch
        {
          RetryExecutionHandler.RetryOrExit();
        }
      }
    }

    T IExecutionHandler.Execute<T>(Func<T> func)
    {
      while (true)
      {
        try
        {
          return this._retry.Execute<T>((Func<IRetryContext, T>) (context => this.DoExecute<T>(context, func)));
        }
        catch
        {
          RetryExecutionHandler.RetryOrExit();
        }
      }
    }

    private static void RetryOrExit()
    {
      if (RTFMessageBox.Show(Strings.InternetConnectionIssue, Application.ProductName, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Retry)
        return;
      Environment.Exit(1);
    }

    private object DoExecute(IRetryContext context, System.Action action)
    {
      action();
      return (object) null;
    }

    private T DoExecute<T>(IRetryContext context, Func<T> func) => func();

    public static T Execute<T>(Func<T> func) => RetryExecutionHandler.Instance.Execute<T>(func);

    public static void Execute(System.Action action) => RetryExecutionHandler.Instance.Execute(action);

    public static IExecutionHandler Instance
    {
      get
      {
        if (RetryExecutionHandler._instance == null)
        {
          RetryExecutionHandler executionHandler = new RetryExecutionHandler();
          Interlocked.CompareExchange<RetryExecutionHandler>(ref RetryExecutionHandler._instance, executionHandler, (RetryExecutionHandler) null);
        }
        return (IExecutionHandler) RetryExecutionHandler._instance;
      }
    }
  }
}
