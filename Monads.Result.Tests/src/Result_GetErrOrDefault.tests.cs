using System;
using Xunit;

namespace Michaelolof.Monads.Result.Tests
{

  public class Result_GetErrOrDefault__Tests
  {

    [Fact]
    public void Should_Return_Err_When_Result_Is_Err()
    {
      // Arrange
      var err = new Exception("Nothing");
      var defaultErr = new Exception("Default");
      var result = Result<int,Exception>.Err( err );
      
      // Act
      var ex = result.GetErrOrDefault( defaultErr );
    
      // Assert
      Assert.True( ex is Exception );
      Assert.True( ex.Message == err.Message );
    }


    [Fact]
    public void Should_Return_Default_Err_When_Result_Is_Ok()
    {
     // Arrange
      var defaultErr = new Exception("Default");
      var result = Result<int,Exception>.Ok( 20 );
      
      // Act
      var ex = result.GetErrOrDefault( defaultErr );
    
      // Assert
      Assert.True( ex is Exception );
      Assert.True( ex.Message == defaultErr.Message );
    }

  }


}