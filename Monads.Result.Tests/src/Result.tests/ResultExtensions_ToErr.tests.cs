using Xunit;
using System;

namespace Michaelolof.Monads.Result.Tests
{

  public class ResultExtensions_ToErr__Tests
  {

    [Fact]
    public void Should_Create_An_Err_Result_From_Any_Value()
    {
      var result = 20.ToErr<string, int>();

      var (val, err) = result.GetValueAndErr();

      Assert.True( result is Result<string, int> );
      Assert.True( val == null );
      Assert.True( err == 20 );
    }

    [Fact]
    public void Should_Create_An_Err_Result_From_Any_Object()
    {
      var result = new Exception("Nothing").ToErr<int>();

      var (val, err) = result.GetValueAndErr();

      Assert.True( result is Result<int, Exception> );
      Assert.True( val == 0 );
      Assert.True( err is Exception );
      Assert.True( err.Message == "Nothing" );
    }

  }


}