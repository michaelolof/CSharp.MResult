using System;
using Xunit;

namespace Michaelolof.Result.Tests
{


  public class Result_Catch__Tests
  {

    [Fact]
    public void Should_Propagate_Error_If_Current_Result_Is_Err()
    {
      
      // Current Result
      var currentResult = Result<string, Exception>.Err(new Exception("Nothing"));

      // Arrange
      var resultOne = currentResult.Catch( 30.ToErr<string,int>() );
      var resultTwo = currentResult.Catch(() => "Some".ToErr<string,string>() );
      var resultThree = currentResult.Catch( e => new NotSupportedException().ToErr<string, NotSupportedException>() );
      var resultFour = currentResult.Catch( e => "Okay".ToOk<string, NotSupportedException>() );
      var resultFive = currentResult.Catch( e => Console.WriteLine(e.Message) );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr();
      var (valTwo, errTwo) = resultTwo.GetValueAndErr();
      var (valThree, errThree) = resultThree.GetValueAndErr();
      var (valFour, errFour) = resultFour.GetValueAndErr();
      var (valFive, errFive) = resultFive.GetValueAndErr();

      // Assert
      Assert.True(errOne == 30 && valOne == null, "First Overload");
      Assert.True(errTwo == "Some" && valTwo == null, "Second Overload");
      Assert.True(errThree is NotSupportedException && valThree == null, "Third Overload");
      Assert.True(errFour == null && valFour == "Okay", "Fourth Overload");
      Assert.True(errFive is Exception && valFive == null, "Fifth Overload");

    }

    [Fact]
    public void Should_Not_Propagate_Error_If_Current_Result_Is_Ok()
    {

      // Current Result
      var currentResult = Result<string, Exception>.Ok( "Something" );

       // Arrange
      var resultOne = currentResult.Catch( 30.ToErr<string,int>() );
      var resultTwo = currentResult.Catch(() => "Some".ToErr<string,string>() );
      var resultThree = currentResult.Catch( e => new NotSupportedException().ToErr<string, NotSupportedException>() );
      var resultFour = currentResult.Catch( e => "Okay".ToOk<string, NotSupportedException>() );
      var resultFive = currentResult.Catch( e => Console.WriteLine(e.Message) );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr();
      var (valTwo, errTwo) = resultTwo.GetValueAndErr();
      var (valThree, errThree) = resultThree.GetValueAndErr();
      var (valFour, errFour) = resultFour.GetValueAndErr();
      var (valFive, errFive) = resultFive.GetValueAndErr();

      // Assert
      Assert.True(errOne == 0 && valOne == "Something", "First Overload");
      Assert.True(errTwo == null && valTwo == "Something", "Second Overload");
      Assert.True(errThree == null && valThree == "Something", "Third Overload");
      Assert.True(errFour == null && valFour == "Something", "Fourth Overload");
      Assert.True(errFive == null && valFive == "Something", "Fifth Overload");

    }


  }

}