using System;
using Xunit;
using System.Threading.Tasks;

namespace Michaelolof.Monads.Result.Tests
{

  public class ResultTask_IsOk__Tests
  {

    [Fact]
    public void Should_Return_True_For_A_Simple_Value_When_Result_Is_Ok()
    {
      var simpleValue = 30;
      var result = simpleValue.ToOkTask<int, Exception>();

      var isOk = result.IsOk();

      Assert.True( isOk );
    }

    [Fact]
    public void Should_Return_True_For_A_Fufilled_Task_When_Result_Is_Ok()
    {
      var fufilledTask = Task.FromResult( "Okay" );
      var result = fufilledTask.ToOkTask<string, Exception>();

      var isOk = result.IsOk();

      Assert.True( isOk );
    }

    [Fact]
    public void Should_Return_False_For_An_Unfufillied_Task_When_Result_Is_Ok()
    {
      var exceptionTask = Task.FromException<int>(new Exception("Nothing"));
      // var cancelledTask = Task.FromCanceled<int>( new  );
      
      var exceptionResult = exceptionTask.ToOkTask<int, Exception>();
      // var cancelledResult = ResultTask<int, Exception>.OK( cancelledTask );

      var exceptionIsOk = exceptionResult.IsOk();
      // var cancelledIsOk = cancelledResult.IsOk;

      Assert.True( exceptionIsOk == false );
      // Assert.True( cancelledIsOk == false );
    }

    [Fact]
    public void Should_Return_False_When_Result_Is_Err()
    {
      var result = new Exception("Nothing").ToErrTask<int, Exception>();

      var isOk = result.IsOk();

      Assert.True( isOk == false );
    }



  }


}