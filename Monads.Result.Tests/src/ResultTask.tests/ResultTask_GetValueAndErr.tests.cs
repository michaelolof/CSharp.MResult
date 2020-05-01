using System;
using Xunit;
using System.Threading.Tasks;

namespace Michaelolof.Monads.Result.Tests
{

  public class ResultTask_GetValueAndErr__Tests
  {


    [Fact]
    public void Should_Return_a_Task__Val_and_Err_with_Err_as_default_when_Result_is_Ok_and_Task_is_Fufilled()
    {
      var fufilledTask = Task.FromResult( 30 );
      var result = fufilledTask.ToOkTask<int, Exception>();

      var (val, err) = result.GetValueAndErr().Result;

      Assert.True( val == 30 );
      Assert.True( err == null );

      Console.WriteLine("Done");
    }


    [Fact]
    public void Should_Return_a_Task_Val_and_Err_with_Val_as_default_when_Result_is_Ok_and_Task_is_Unfufilled()
    {
      var unfufilledTask = Task.FromException<int>( new Exception("Nothing") );
      var result = unfufilledTask.ToOkTask<int, Exception>();

      var(val, err) = result.GetValueAndErr().Result;

      Assert.True( err is Exception );
      Assert.True( err.Message == "Nothing" );
      Assert.True( val == 0 );
    }


    [Fact]
    public void Should_Return_a_Task_Val_and_Err_with_Val_as_default_when_Result_is_Err()
    {
      var result = new Exception("Nothing").ToErrTask<int, Exception>();
     
      var (val, err) = result.GetValueAndErr().Result;

      Assert.True( val == 0 );
      Assert.True( err is Exception );
      Assert.True( err.Message == "Nothing" );
    }


    [Fact]
    public void Should_Return_a_Task_Val_and_Err_with_Err_as_default_when_Result_is_Ok_and_Val_is_Basic()
    {
      var basicValue = 30;
      var result = basicValue.ToOkTask<int, Exception>();

      var (val, err) = result.GetValueAndErr().Result;

      Assert.True( val == 30 );
      Assert.True( err == null );
    }


  }


}