using Xunit;
using System;
using System.Threading.Tasks;

namespace Michaelolof.Monads.Result.Tests
{

  public class TaskResult_OnOk__Tests
  {

    [Fact]
    public void Should_Propagate_If_Task_is_fufilled_and_Current_Result_is_Ok()
    {

      var okResult = 20.ToOk();
      var currentFufilledTask = Task.FromResult( okResult );

      // Arrange (Test all supported overloads)
      var resultOne = currentFufilledTask.OnOk( "Test" ).Result;
      var resultTwo = currentFufilledTask.OnOk( () => "Test" ).Result;
      var resultThree = currentFufilledTask.OnOk( b => b + 5 ).Result;

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
    public async void Should_not_Propagate_If_Task_is_unfufilled_and_Current_Result_is_Ok()
    {

      var okResult = 20.ToOk();
        
      var currentUnfufilledTask = unfufillTask( -10 );

      // Arrange (Test all supported overloads)
      var resultOne = await currentUnfufilledTask.OnOk( "Test" );
      var resultTwo = currentUnfufilledTask.OnOk( () => "Test" );
      var resultThree = currentUnfufilledTask.OnOk( b => b + 5 );

      // Act
      var (valOne, errOne) = resultOne.GetValueAndErr();
      var (valTwo, errTwo) = await resultTwo.GetValueAndErr();
      var (valThree, errThree) = await resultThree.GetValueAndErr();

      // Assert
      Assert.True( valOne == null && errOne is Exception && errOne.Message == "Bad Shit" );
      Assert.True( valTwo == null && errTwo is Exception && errTwo.Message == "Bad Shit" );
      Assert.True( valThree == 0 && errThree is Exception && errTwo.Message == "Bad Shit" );

      //----------------------------------------------------------------
      async Task<Result<int, Exception>> unfufillTask(int n) { 
        await Task.Delay(300);
        if( n < 0  ) throw new Exception("Bad Shit"); 
        else return okResult;
      }

    }

  }


}