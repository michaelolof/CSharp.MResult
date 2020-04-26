using System;
using Xunit;

namespace Michaelolof.MResult.Tests
{

  public class Result_OnErr__Tests
  {

    [Fact]
    public void Passing_Test_When_Using_A_Value_Argument()
    {
      const string errMsg = "Not Found In DB";
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Err(new Exception(errMsg));
      Func<Result<Tokens,Exception>> GetValue = () => Result<Tokens,Exception>.Err( new Exception("Secondary Exception") );


      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnErr( GetValue );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val == null );
      Assert.True( err.Message == "Secondary Exception" );
    }

    [Fact]
    public void Failing_Test_When_Using_A_Value_Argument()
    {
      // Arrange
      const string value = "one two three";
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Ok(new Tokens(value));
      Func<Result<Tokens,Exception>> GetValue = () => Result<Tokens,Exception>.Err( new Exception("Secondary Exception") );

      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnErr( GetValue );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val is Tokens );
      Assert.True( val.Value == value );
      Assert.True( err == null );
    }

    [Fact]
    public void Passing_Test_When_Using_A_Parameterless_Callback_Argument()
    {
      // Arrange
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Err(new Exception("Not found in DB"));
      Func<Result<Tokens,Exception>> GetValue = () => Result<Tokens,Exception>.Err( new Exception("Secondary Exception") );

      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnErr( e => GetValue() );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val == null );
      Assert.True( err is Exception );
      Assert.True( err.Message == "Secondary Exception" );
    }

    [Fact]
    public void Failing_Test_When_Using_A_Parameterless_Callback_Argument()
    {
      // Arrange
      const string value = "one two three";
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Ok(new Tokens(value));
      Func<Result<Tokens,Exception>> GetValue = () => Result<Tokens,Exception>.Err( new Exception("Secondary Exception") );
      
      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnErr(() => GetValue() );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val is Tokens );
      Assert.True( val.Value == value );
      Assert.True( err == null );      
    }

    [Fact]
    public void Passing_Test_When_Using_A_Callback_With_One_Argument()
    {
      // Arrange
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Err(new Exception("Nothing"));
      Func<string,Result<Tokens,NotSupportedException>> GetValue = (t) => Result<Tokens,NotSupportedException>.Err( new NotSupportedException("Secondary Exception") );
      
      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnErr( e => GetValue(e.Message) );
      
      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val == null);
      Assert.True( err.Message == "Secondary Exception" );

    }

    [Fact]
    public void Failing_Test_When_Using_A_Callback_With_One_Argument()
    {
      // Arrange
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Ok(new Tokens());
      Func<string, Result<Tokens,NotSupportedException>> GetValue = (t) => Result<Tokens,NotSupportedException>.Err( new NotSupportedException("Secondary Exception") );
      
      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnErr( ex => GetValue(ex.Message) );
      
      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val is Tokens );
      Assert.True( err == null );
    }

    [Fact]
    public void Passing_Test_When_Returning_A_Result_Value()
    {
      // Arrange
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Err(new Exception("Nothing"));
      Func<string, Result<Tokens, Exception>> StoreTokensToDB = (tks) => Result<Tokens,Exception>.Ok( new Tokens() );

      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnErr( () => StoreTokensToDB("") );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val is Tokens );
      Assert.True( err == null );
    }

    [Fact]
    public void Failing_Test_When_Returning_A_Result_Value()
    {
      // Arrange
      Func<int, Result<Tokens, Exception>> ReadTokensFromDB = (id) => Result<Tokens,Exception>.Err( new Exception("Nothing") );
      Func<string, Result<Tokens, NotSupportedException>> StoreTokensToDB = (tks) => 
        Result<Tokens,NotSupportedException>.Err( new NotSupportedException( "Secondary Exception") );


      // Act
      var tokens = ReadTokensFromDB( 20 )
        .OnErr( e => StoreTokensToDB(e.Message) );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val == null );
      Assert.True( err is NotSupportedException );
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