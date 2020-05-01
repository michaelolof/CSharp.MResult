using Xunit;
using System;
using System.Threading.Tasks;

namespace Michaelolof.Monads.Result.Tests
{

  public class ResultExtensions_ToResult__Tests
  {

    [Fact]
    public void Should_Convert_A_Fufilled_Task_To_Task_Of_Ok_Result()
    {
      var fufilledTask = Task.FromResult( 30 );

      var resultingTask = fufilledTask.ToResult();
      var result = resultingTask.Result;
      var (val, err) = result.GetValueAndErr();

      Assert.True( resultingTask is Task<Result<int, Exception>> );
      Assert.True( val == 30 );
      Assert.True( err == null );
    }

    [Fact]
    public void Should_Convert_A_Failed_Task_To_Task_Of_Err_Result()
    {
      var failedTask = Task.FromException<int>( new Exception("Nothing") );

      var resultingTask = failedTask.ToResult();
      var result = resultingTask.Result;
      var (val, err) = result.GetValueAndErr();

      Assert.True( resultingTask is Task<Result<int, Exception>> );
      Assert.True( val == 0 );
      Assert.True( err is Exception );
      Assert.True( err.Message == "Nothing" );
    }

  }

}