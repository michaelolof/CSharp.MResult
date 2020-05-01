using System;
using System.Threading.Tasks;
using Xunit;


namespace Michaelolof.Monads.Result.Tests
{

  public class ResultTask_IsErr__Test
  {

    [Fact]
    public void Should_Return_True_When_ResultTask_Is_Err()
    {
      var result = new Exception("Nothing").ToErrTask<int, Exception>();

      var isErr = result.IsErr();

      Assert.True( isErr );      
    }

    [Fact]
    public void Should_Return_False_For_Simple_Value_When_ResultTask_Is_Ok()
    {
      var simpleValue = "Welback";
      var result = simpleValue.ToOkTask<string, Exception>();

      var isErr = result.IsErr();

      Assert.True( isErr == false );

    }

    [Fact]
    public void Should_Return_False_For_Fuffilled_Task_Value_When_Result_Is_Ok()
    {
      var fufilledTask = Task.FromResult( 30 );
      var result = fufilledTask.ToOkTask<int, Exception>();

      var isErr = result.IsErr();

      Assert.True( isErr == false );

    }

    [Fact]
    public void Should_Return_True_For_Unfufilled_Task_Value_When_Result_Is_Ok()
    {
      var exceptionTask = Task.FromException<int>( new Exception("Nothing") );
      var result = exceptionTask.ToOkTask<int, Exception>();

      var isErr = result.IsErr();

      Assert.True( isErr );
    }


  }

}