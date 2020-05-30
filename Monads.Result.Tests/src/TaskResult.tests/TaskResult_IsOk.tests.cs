using System;
using Xunit;
using System.Threading.Tasks;
using System.Linq;


namespace Michaelolof.Monads.Result.Tests
{

  public class TaskResult_IsOk__Tests
  {

    [Fact]
    public void Should_return_True_if_Task_is_fufilled_and_Result_is_Ok()
    {
      var okResult = "Name".ToOk<string, NotSupportedException>();
      var taskOfOkResult = Task.FromResult( okResult );

      var isOk = taskOfOkResult.IsOk().Result;

      Assert.True( isOk == true );
    }


    [Fact]
    public async void Should_return_False_if_Task_is_unfufilled_and_Result_is_Ok()
    {
      var taskOfUnfufilledResult = myTask( -400 );

      var isOk = await taskOfUnfufilledResult.IsOk();

      Assert.True( isOk == false );
      //--------------------------------------------------------------
      async Task<Result<string,Exception>> myTask(int n) {
        
        await Task.Delay( 300 );

        if( n < -5 ) throw new Exception("Something went wrong");

        if( n == 0 ) return new Exception("Another thing Went Wrong");

        else return "Okay";
      } 
    }

    [Fact]
    public async void Should_return_False_if_Task_is_fufilled_and_Result_is_Err()
    {
      var errResult = new Exception().ToErr<string, Exception>();
      var fufilledTask = Task.FromResult( errResult );

      var isOk = await fufilledTask.IsOk();

      Assert.True( isOk == false );
    }

  }


}