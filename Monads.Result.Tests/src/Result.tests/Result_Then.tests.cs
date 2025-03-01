using Xunit;
using System;

namespace Michaelolof.Monads.Result.Tests
{

  public class Result_Then__Tests
  {

    [Fact]
    public void Should_Propagate_Result_If_Current_Result_Is_Ok()
    {

      // Current Result
      var currentResult = Result<int,Exception>.Ok( 30 );

      // Arrange
      var resultOne = currentResult.Then( 25.ToOk() );
      var resultTwo = currentResult.Then(() => Result<string,NotSupportedException>.Ok( "Two" ));
      var resultThree = currentResult.Then(() => Result<string,NotSupportedException>.Err( new NotSupportedException("Something") ));
      var resultFour = currentResult.Then( v => (v + 1).ToOk() );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr();
      var (valTwo, errTwo) = resultTwo.GetValueAndErr();
      var (valThree, errThree) = resultThree.GetValueAndErr();
      var (valFour, errFour) = resultFour.GetValueAndErr();

      // Assert
      Assert.True( valOne == 25 && errOne == null, "First Overload" );
      Assert.True( valTwo == "Two" && errTwo == null, "Second Overload" );
      Assert.True( valThree == null && errThree is Exception && errThree.Message == "Something", "Third Overload" );
      Assert.True( valFour == 31 && errFour == null, "Fourth Overload" );

    }

    [Fact]
    public void Should_Not_Propagate_Result_If_Current_Result_Is_Err()
    {

      // Current Result
      var currentResult = "Nothing".ToErr<int, string>();

      // Arrange
      var resultOne = currentResult.Then( "Another".ToErr<int, string>() );
      var resultTwo = currentResult.Then(() => Result<string,string>.Ok( "Two" ));
      var resultThree = currentResult.Then(() => Result<string,string>.Err( "Something" ));

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr();
      var (valTwo, errTwo) = resultTwo.GetValueAndErr();
      var (valThree, errThree) = resultThree.GetValueAndErr();

      // Assert
      Assert.True( valOne == 0 && errOne == "Nothing", "First Overload" );
      Assert.True( valTwo == null && errTwo == "Nothing", "Second Overload" );
      Assert.True( valThree == null && errThree == "Nothing", "Third Overload" );

    }

  }

}