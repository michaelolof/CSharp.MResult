using System;
using Xunit;

namespace Michaelolof.MResult.Tests
{

  public class Result_GetValueAndErr__Tests
  {

    [Fact]
    public void Should_Return_A_Tuple_Of_Value_And_Null_When_Result_Is_Ok()
    {
      var result = Result<int, Exception>.Ok( 20 );

      var (val, err) = result.GetValueAndErr();

      Assert.True( val == 20 );
      Assert.True( err == null );
    }

    [Fact]
    public void Should_Return_A_Tuple_Of_Zero_And_Err_When_Result_Is_Err_And_Value_Is_An_Integer()
    {
      var result = Result<int, Exception>.Err( new Exception("Nothing") );

      var (val, err) = result.GetValueAndErr();

      Assert.True( val == 0 );
      Assert.True( err is Exception );
      Assert.True( err.Message == "Nothing" );
    }

    [Fact]
    public void Should_Return_A_Tuple_Of_Null_And_Exception_When_Result_Is_Err_And_Value_Is_A_Reference()
    {
      var result = Result<Tokens,Exception>.Err( new Exception("Nothing") );

      var (val, err) = result.GetValueAndErr();

      Assert.True( val == null );
      Assert.True( err is Exception );
      Assert.True( err.Message == "Nothing" );
    }

    [Fact]
    public void Should_Return_A_Tuple_Of_Empty_Struct_And_Exception_When_Result_Is_Err_And_Value_Is_Struct()
    {
      var result = Result<Struct, Exception>.Err( new Exception("Nothing") );

      var (val, err) = result.GetValueAndErr();

      Assert.True( val.Name == null );
      Assert.True( err is Exception );
      Assert.True( err.Message == "Nothing" );
    }

    public class Tokens {}

    public struct Struct {
      public string Name {get;}
    }


  }

}