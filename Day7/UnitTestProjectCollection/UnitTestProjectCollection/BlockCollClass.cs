using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace UnitTestProjectCollection
{
  public class BlockCollClass
  {
    protected ConcurrentBag<int> bc;

    private void producer()
    {
      for (int i = 0; i < 100; i++)
      {
        bc.Add(i * i);
        Debug.WriteLine("Create " + i * i);
      }
    }

    private void consumer()
    {
        int res = 0;
      while (bc.TryTake(out res))
      {
        Debug.WriteLine("Take: " + res);
      }
    }

    public void Start()
    {
      bc = new ConcurrentBag<int>();

      Task Pr = new Task(producer);
      Task Cn = new Task(consumer);

      Pr.Start();
      Cn.Start();

      try
      {
        Task.WaitAll(Cn, Pr);
      }
      finally
      {
        Cn.Dispose();
        Pr.Dispose();
      }
    }
  }
}
