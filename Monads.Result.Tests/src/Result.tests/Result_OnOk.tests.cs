using Xunit;
using System;

namespace Michaelolof.Monads.Result.Tests
{

  public class Result_OnOk__Tests
  {

    [Fact]
    public void Should_Propagate_Value_If_Current_Result_Is_OK()
    {
      // Base
      var currentResult = Result<int,Exception>.Ok( 20 );
      
      // Arrange (Test all supported overloads)
      var resultOne = currentResult.OnOk( "Test" );
      var resultTwo = currentResult.OnOk( () => "Test" );
      var resultThree = currentResult.OnOk( b => b + 5 );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr();
      var (valTwo, errTwo) = resultTwo.GetValueAndErr();
      var (valThree, errThree) = resultThree.GetValueAndErr();

      // Assert
      Assert.True( valOne == "Test" && errOne == null );
      Assert.True( valTwo == "Test" && errTwo == null );
      Assert.True( valThree == 25 && errThree == null );

    }

    [Fact]
    public void Should_Not_Propagate_Value_If_Current_Result_Is_Err()
    {

      // Base Result
      var currentResult = Result<int, Exception>.Err( new Exception("Nothing") );

      // Arrange (Test all supported overloads)
      var resultOne = currentResult.OnOk( "Test" );
      var resultTwo = currentResult.OnOk( () => "Test" );
      var resultThree = currentResult.OnOk( b => b + 5 );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr();
      var (valTwo, errTwo) = resultTwo.GetValueAndErr();
      var (valThree, errThree) = resultThree.GetValueAndErr();

      // Assert
      Assert.True( valOne == null && errOne is Exception && errOne.Message == "Nothing" );
      Assert.True( valTwo == null && errTwo is Exception && errTwo.Message == "Nothing" );
      Assert.True( valThree == 0 && errThree is Exception && errThree.Message == "Nothing" );
    }

  }

}