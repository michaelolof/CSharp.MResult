using System;
using System.Threading.Tasks;
using Xunit;


namespace Michaelolof.Monads.Result.Tests
{

  public class TaskResult_Then__Tests
  {

    [Fact]
    public void Should_Propagate_if_Current_Result_is_Fufilled_and_Ok_and_Handler_Result_is_Fufilled_and_Ok()
    {
      const string val = "Wonderful";
      var currentResult = Task.FromResult( 30 ).ToResult();

      // Arrange
      var resultOne = currentResult.Then( Task.FromResult( val ).ToResult() );
      var resultTwo = currentResult.Then( async () => await Task.FromResult( val ).ToResult() );
      var resultThree = currentResult.Then( async name => await Task.FromResult( val ).ToResult() );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr().Result;
      var (valTwo, errTwo) = resultTwo.GetValueAndErr().Result;
      var (valThree, errThree) = resultThree.GetValueAndErr().Result;

      // Assert
      Assert.True( valOne == "Wonderful" && errOne == null, "Aseertion One" );
      Assert.True( valTwo == "Wonderful" && errTwo == null, "Aseertion One" );
      Assert.True( valThree == "Wonderful" && errThree == null, "Aseertion One" );

    }

    [Fact]
    public void Should_Not_Propagate_if_Current_Result_is_Fufilled_and_Ok_and_Handler_is_Unfufilled()
    {
      var currentResult = Task.FromResult( 30 ).ToResult();
      var unfufilledResult = Task.FromException<string>( new Exception("Nothing") ).ToResult();
      
      // Arrange
      var resultOne = currentResult.Then( unfufilledResult );
      var resultTwo = currentResult.Then( async () => await unfufilledResult );
      var resultThree = currentResult.Then( async name => await unfufilledResult );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr().Result;
      var (valTwo, errTwo) = resultTwo.GetValueAndErr().Result;
      var (valThree, errThree) = resultThree.GetValueAndErr().Result;

      // Assert
      Assert.True(valOne == null && errOne is Exception && errOne.Message == "Nothing");
      Assert.True(valTwo == null && errTwo is Exception && errTwo.Message == "Nothing");
      Assert.True(valThree == null && errThree is Exception && errThree.Message == "Nothing");

    }

    [Fact]
    public void Should_NOT_Propagate_if_Current_Result_is_Fufilled_and_Ok_and_Handler_is_Fufilled_and_Err()
    {

      var currentResult = Task.FromResult( 30 ).ToResult();
      var fufilledErr = Task.FromResult( new Exception("Nothing").ToErr<string, Exception>() );

      // Arrange
      var resultOne = currentResult.Then( fufilledErr );
      var resultTwo = currentResult.Then( async () => await fufilledErr );
      var resultThree = currentResult.Then( async name => await fufilledErr );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr().Result;
      var (valTwo, errTwo) = resultTwo.GetValueAndErr().Result;
      var (valThree, errThree) = resultThree.GetValueAndErr().Result;

      // Assert
      Assert.True(valOne == null && errOne is Exception && errOne.Message == "Nothing");
      Assert.True(valTwo == null && errTwo is Exception && errTwo.Message == "Nothing");
      Assert.True(valThree == null && errThree is Exception && errThree.Message == "Nothing");

    }

    [Fact]
    public void Should_Not_Propagate_if_Current_Result_is_Fufilled_and_Err()
    {

      var currentResult = Task.FromResult( new Exception("Nothing").ToErr<int,Exception>() );
      var fufilledHandler = Task.FromResult("Wonderful".ToOk());

      // Arrange
      var resultOne = currentResult.Then( fufilledHandler );
      var resultTwo = currentResult.Then( async () => await fufilledHandler );
      var resultThree = currentResult.Then( async name => await fufilledHandler );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr().Result;
      var (valTwo, errTwo) = resultTwo.GetValueAndErr().Result;
      var (valThree, errThree) = resultThree.GetValueAndErr().Result;      

      // Assert
      Assert.True(valOne == null && errOne is Exception && errOne.Message == "Nothing");
      Assert.True(valTwo == null && errTwo is Exception && errTwo.Message == "Nothing");
      Assert.True(valThree == null && errThree is Exception && errThree.Message == "Nothing");

    }

  }

}