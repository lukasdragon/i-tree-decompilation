// Decompiled with JetBrains decompiler
// Type: Eco.Util.Tasks.ITask`1
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System.Threading.Tasks;

namespace Eco.Util.Tasks
{
  internal interface ITask<T>
  {
    Task<T> DoWork();
  }
}
