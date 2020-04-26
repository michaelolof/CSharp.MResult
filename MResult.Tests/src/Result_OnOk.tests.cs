using System;
using Xunit;

namespace Michaelolof.MResult.Tests
{

  public class Result_OnOk__Tests
  {

    [Fact]
    public void Passing_Test_When_Using_A_Value_Argument()
    {
      // Arrange
      const string value = "one two three";
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Ok(new Tokens(value));

      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnOk( "transformed-ok" );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val == "transformed-ok" );
      Assert.True( val != value );
      Assert.True( err == null );
    }

    [Fact]
    public void Failing_Test_When_Using_A_Value_Argument()
    {
      const string errMsg = "Not Found In DB";
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Err(new Exception(errMsg));

      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnOk( "not-using" );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val == null );
      Assert.True( err is Exception );
      Assert.True( err.Message == errMsg );
    }

    [Fact]
    public void Passing_Test_When_Using_A_Parameterless_Callback_Argument()
    {
      // Arrange
      const string value = "one two three";
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Ok(new Tokens(value));
      Func<string> GetValue = () => "transformed-ok";

      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnOk(() => GetValue() );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val == "transformed-ok" );
      Assert.True( val != value );
      Assert.True( err == null );      
    }

    [Fact]
    public void Failing_Test_When_Using_A_Parameterless_Callback_Argument()
    {
      // Arrange
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Err(new Exception("Not found in DB"));
      Func<string> GetValue = () => "transformed-ok";

      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnOk(() => GetValue() );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val == null );
      Assert.True( err is Exception );
      Assert.True( err.Message == "Not found in DB" );
    }

    [Fact]
    public void Passing_Test_When_Using_A_Callback_With_One_Argument()
    {
      // Arrange
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Ok(new Tokens());
      Func<Tokens, string> GetValue = (t) => "transformed-ok";

      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnOk( toks => GetValue(toks) );
      
      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val == "transformed-ok");
      Assert.True( err == null );
    }

    [Fact]
    public void Failing_Test_When_Using_A_Callback_With_One_Argument()
    {
      // Arrange
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Err(new Exception("Nothing"));
      Func<Tokens, string> GetValue = (t) => "transformed-ok";

      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnOk( toks => GetValue(toks) );
      
      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val == null);
      Assert.True( err.Message == "Nothing" );

    }

    [Fact]
    public void Passing_Test_When_Returning_A_Result_Value()
    {
      // Arrange
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Ok( new Tokens() );
      Func<Tokens, Result<int, Exception>> StoreTokensToDB = (tks) => Result<int,Exception>.Ok( 25 );

      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnOk( toks => StoreTokensToDB( toks) );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val == 25 );
      Assert.True( err == null );
    }

    [Fact]
    public void Failing_Test_When_Returning_A_Result_Value()
    {
      // Arrange
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Err(new Exception("Nothing"));
      Func<Tokens, Result<int, Exception>> StoreTokensToDB = (tks) => Result<int,Exception>.Ok( 25 );

      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnOk( toks => StoreTokensToDB( toks) );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val == 0 );
      Assert.True( err is Exception );
    }

    [Fact]
    public void Failing_Test_When_Returning_A_Result_Value_2()
    {
      // Arrange
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Ok( new Tokens() );
      Func<Tokens, Result<int, Exception>> StoreTokensToDB = (tks) => Result<int,Exception>.Err( new Exception("Could Not Store") );

      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnOk( toks => StoreTokensToDB( toks) );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val == 0 );
      Assert.True( err is Exception );
      Assert.True( err.Message == "Could Not Store" );
    }


    class Tokens {
      public string Value;

      public Tokens()
      {
        Value = "tokens";
      }

      public Tokens(string value) {
        Value = value;
      }
      public override string ToString() => "Tokens"; 
    };

  }


}