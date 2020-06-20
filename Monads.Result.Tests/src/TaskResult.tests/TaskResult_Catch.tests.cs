using System;
using System.Threading.Tasks;
using Xunit;


namespace Michaelolof.Monads.Result.Tests
{

  public class TaskResult_Catch__Tests
  {

    [Fact]
    public void Should_Not_Propagate_if_Current_Result_is_Fufilled_and_Ok_and_Handler_Result_is_Fufilled_and_Ok()
    {
      const int val = 3000;
      var currentResult = Task.FromResult( 30 ).ToResult();

      // Arrange
      var resultOne = currentResult.Catch( Task.FromResult( val ).ToResult() );
      var resultTwo = currentResult.Catch( async () => { Console.WriteLine("I shouldn't see this"); return await Task.FromResult( val ).ToResult(); });
      var resultThree = currentResult.Catch( async name => await Task.FromResult( val ).ToResult() );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr().Result;
      var (valTwo, errTwo) = resultTwo.GetValueAndErr().Result;
      var (valThree, errThree) = resultThree.GetValueAndErr().Result;

      // Assert
      Assert.True( valOne == 30 && errOne == null, "Aseertion One" );
      Assert.True( valTwo == 30 && errTwo == null, "Aseertion One" );
      Assert.True( valThree == 30 && errThree == null, "Aseertion One" );

    }

    [Fact]
    public void Should_Not_Propagate_if_Current_Result_is_Fufilled_and_Ok_and_Handler_is_Unfufilled()
    {
      var currentResult = Task.FromResult( 30 ).ToResult();
      var unfufilledResult = Task.FromException<int>( new Exception("Nothing") ).ToResult();
      
      // Arrange
      var resultOne = currentResult.Catch( unfufilledResult );
      var resultTwo = currentResult.Catch( async () => { Console.WriteLine("I shouldn't see this"); return await unfufilledResult; });
      var resultThree = currentResult.Catch( async name => await unfufilledResult );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr().Result;
      var (valTwo, errTwo) = resultTwo.GetValueAndErr().Result;
      var (valThree, errThree) = resultThree.GetValueAndErr().Result;

      // Assert
      Assert.True(valOne == 30 && errOne == null);
      Assert.True(valTwo == 30 && errTwo == null);
      Assert.True(valThree == 30 && errThree == null);

    }

    [Fact]
    public void Should_Not_Propagate_if_Current_Result_is_Fufilled_and_Ok_and_Handler_is_Fufilled_and_Err()
    {

      var currentResult = buildCurrentTask( 30 ).ToResult();
      var fufilledErr = Task.FromResult( new Exception("Nothing").ToErr<int, Exception>() );

      // Arrange
      var resultOne = currentResult.Catch( fufilledErr );
      var resultTwo = currentResult.Catch( async () => { Console.WriteLine("This shouldn't run"); return await fufilledErr; });
      var resultThree = currentResult.Catch( async name => await fufilledErr );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr().Result;
      var (valTwo, errTwo) = resultTwo.GetValueAndErr().Result;
      var (valThree, errThree) = resultThree.GetValueAndErr().Result;

      // Assert
      Assert.True(valOne == 30 && errOne == null);
      Assert.True(valTwo == 30 && errTwo == null);
      Assert.True(valThree == 30 && errThree == null);


      //--------------------------------------------------------------------------------
      async Task<int> buildCurrentTask(int n) {
        await Task.Delay( 5_000 );
        return n;
      }

    }

    [Fact]
    public void Should_Propagate_if_Current_Result_is_Fufilled_and_Err()
    {

      var currentResult = Task.FromResult( new Exception("Nothing").ToErr<string,Exception>() );
      var fufilledHandler = Task.FromResult("Wonderful".ToOk());

      // Arrange
      var resultOne = currentResult.Catch( fufilledHandler );
      var resultTwo = currentResult.Catch( async () => await fufilledHandler );
      var resultThree = currentResult.Catch( async name => await fufilledHandler );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr().Result;
      var (valTwo, errTwo) = resultTwo.GetValueAndErr().Result;
      var (valThree, errThree) = resultThree.GetValueAndErr().Result;      

      // Assert
      Assert.True(valOne == "Wonderful" && errOne == null);
      Assert.True(valTwo == "Wonderful" && errTwo == null);
      Assert.True(valThree == "Wonderful" && errThree == null);

    }

  }

}