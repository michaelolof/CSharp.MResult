using Xunit;
using System;

namespace Michaelolof.Monads.Result.Tests
{

  public class ResultExtensions_Merge__Tests
  {

    [Fact]
    public void Should_Return_Value_If_Result_Is_Ok()
    {
      var result = Result<string,string>.Ok("Test");

      var value = result.Merge();

      Assert.True( value == "Test" );
    }

    [Fact]
    public void Should_Return_Err_If_Result_Is_Err()
    {
      var result = Result<Tokens, Tokens>.Err( new Tokens("err-token") );

      var value = result.Merge();

      Assert.True( value is Tokens  );
      Assert.True( value.Type == "err-token" );
    }


    class Tokens {
      public string Type {get;}

      public Tokens(string type)
      {
        Type = type;
      }
    }

  }


}