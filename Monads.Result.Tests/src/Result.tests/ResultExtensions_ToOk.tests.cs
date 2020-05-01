using System;
using Xunit;

namespace Michaelolof.Monads.Result.Tests
{

  public class ResultExtensions_ToOk__Tests
  {

    [Fact]
    public void Should_Create_An_Ok_Result_From_Any_Value()
    {
      var result = 20.ToOk<int, Exception>();

      var (val, err) = result.GetValueAndErr();

      Assert.True( val == 20 );
      Assert.True( err == null );
    }

    [Fact]
    public void Should_Create_An_Ok_Result_From_Any_Object()
    {

      var result = new Tokens().ToOk();

      var (val, err) = result.GetValueAndErr();

      Assert.True( val is Tokens );
      Assert.True( err is null );

    }

    class Tokens {};
  }

}