using System;
using Xunit;

namespace Michaelolof.Monads.Result.Tests
{

  public class Result_Match__Tests
  {

    [Fact]
    public void Match__OnOk__Tests()
    {
      // Arrange
      var certainResult = Result<int, Exception>.Ok( 20 );
      Func<int,string> toString = num => num.ToString();

      // Act
      var matched = certainResult.Match(
        onOk: val => toString( val ),
        onErr: err => { 
          Console.WriteLine( err.Message );
          return -1;
        }
      );
      var value = matched.GetValueOrDefault( "10" );
      var error = matched.GetErrOrDefault( 0 );

      // Assert
      Assert.True( value == "20" );
      Assert.True( error == 0 );

    }

    [Fact]
    public void Match__OnErr__Tests()
    {
      // Arrange
      var uncertainResult = Result<int, Exception>.Err( new Exception() );
      Func<int,string> ToString = n => n.ToString();
      Func<Exception, NotSupportedException> Cast = e => new NotSupportedException(e.Message);

      // Act
      var matched = uncertainResult.Match(
        onOk: ToString,
        onErr: Cast
      );

      var(val, err) = matched.GetValueAndErr();

      // Assert
      Assert.True( val == null );
      Assert.True( err is NotSupportedException );

    }

    [Fact]
    public void Match__OnkOK__Result__Passing__Tests()
    {
      // Arrange
      Func<int, Result<Tokens, Exception>> readFromDB = (id) => Result<Tokens, Exception>.Ok( new Tokens() );
      Func<Tokens, string> normalizeTokens = (t) => t.ToString();
      Func<string, string[]> tokenize = n => n.Split(" ");

      // Act
      var tId = 30;
      var tokens =  readFromDB( tId )
        .OnOk( t => normalizeTokens(t) )
        .OnOk( t => tokenize(t) );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( val != null );
      Assert.True( err == null );
      Assert.True( val is Array );
      Assert.True( val.Length == 5 );
      Assert.True( val[ 0 ] == "one" );
      Assert.True( val[ 1 ] == "two" );
      Assert.True( val[ 2 ] == "three" );
      Assert.True( val[ 3 ] == "four" );
      Assert.True( val[ 4 ] == "five" );
    }

    [Fact]
    public void Match__OnkOK__Result__Failing__Tests()
    {
      // Arrange
      Func<int, Result<Tokens, Exception>> readFromDB = (id) => Result<Tokens, Exception>.Err( new Exception("Tokens Not Found In DB") );
      Func<Tokens, string> normalizeTokens = (t) => t.ToString();
      Func<string, string[]> tokenize = n => n.Split(" ");

      // Act
      var tId = 30;
      var tokens =  readFromDB( tId )
        .OnOk( t => normalizeTokens(t) )
        .OnOk( t => tokenize(t) );

      var (val, err) = tokens.GetValueAndErr();

      // Assert
      Assert.True( err is Exception );
      Assert.True( val == null );

    }



  }

  public class Tokens {
    public override string ToString() => "one two three four five";
  }


}