using Xunit;
using System;

namespace Michaelolof.Monads.Result.Tests
{

  public class Result_OnErr__Tests
  {

    [Fact]
    public void Should_Propagate_Error_If_Current_Result_Is_Err()
    {

      // Base Result
      var currentResult = Result<int, Exception>.Err( new Exception("Nothing") );

      // Arrange (Test all supported overloads)
      var resultOne = currentResult.OnErr( "Something" );
      var resultTwo = currentResult.OnErr(() => new NotSupportedException());
      var resultThree = currentResult.OnErr(e => new NotSupportedException(e.Message));
      var resultFour = currentResult.OnErr( e => Console.WriteLine(e.Message));

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr();
      var (valTwo, errTwo) = resultTwo.GetValueAndErr();
      var (valThree, errThree) = resultThree.GetValueAndErr();
      var (valFour, errFour) = resultFour.GetValueAndErr();

      // Assert
      Assert.True(errOne == "Something" && valOne == 0, "First Overload" );
      Assert.True(errTwo is NotSupportedException && valTwo == 0, "Second Overload" );
      Assert.True(errThree is NotSupportedException && errThree.Message == "Nothing" && valThree == 0, "Third Overload");
      Assert.True(errFour is Exception && errFour.Message == "Nothing" && valFour == 0, "Fourth Overload" );

    }

    [Fact]
    public void Should_Not_Propagate_Error_If_Current_Result_Is_Ok()
    {

      // Base Result
      var currentResult = Result<string, Exception>.Ok("Current");

      // Arrange (Test all supported overloads)
      var resultOne = currentResult.OnErr( new Exception("One") );
      var resultTwo = currentResult.OnErr(() => "Two");
      var resultThree = currentResult.OnErr(e => new NotSupportedException(e.Message));
      var resultFour = currentResult.OnErr( e => Console.WriteLine(e.Message));

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr();
      var (valTwo, errTwo) = resultTwo.GetValueAndErr();
      var (valThree, errThree) = resultThree.GetValueAndErr();
      var (valFour, errFour) = resultFour.GetValueAndErr();

      // Assert
      Assert.True(errOne == null && valOne == "Current", "First Overload" );
      Assert.True(errTwo == null && valTwo == "Current", "Second Overload" );
      Assert.True(errThree == null && valThree == "Current", "Third Overload");
      Assert.True(errFour == null && valFour == "Current", "Fourth Overload" );

    }


  }

}