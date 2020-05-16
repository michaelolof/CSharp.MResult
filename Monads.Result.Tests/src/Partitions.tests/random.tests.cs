using Xunit;
using System;
using System.Collections.Generic;

namespace Michaelolof.Monads.Result.Tests
{

  public class Random_Partition__Tests
  {


    [Fact]
    public void TestMe()
    {
      var nums = new int[]{ 30 };
      var resultArr = nums.ToOk<int[],Exception>();

      var partitions = resultArr.ToPartitions();

      var tp = partitions.OnOk( n => n + 1 );

      var vals = tp.GetVal();
      var errs = tp.GetErr();

      Console.WriteLine("Done");
    }


  }


}