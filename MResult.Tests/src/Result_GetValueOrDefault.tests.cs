using System;
using Xunit;

namespace Michaelolof.Result.Tests
{

  public class Result_GetValueOrDefault__Tests
  {

    [Fact]
    public void Should_Return_Value_When_Result_Is_Ok()
    {
      // Arrange
      const int value = 20;
      const int defaultVal = 5;
      var result = Result<int, Exception>.Ok( value );

      // Act
      var val = result.GetValueOrDefault( defaultVal );

      // Assert
      Assert.True( val == value );
      Assert.True( val != defaultVal );
    }


    [Fact]
    public void Should_Return_Default_Value_When_Result_Is_Err()
    {
      // Arrange
      const int value = 20;
      const int defaultVal = 5;
      var result = Result<int, Exception>.Err( new Exception("Nothing") );

      // Act
      var val = result.GetValueOrDefault( defaultVal );

      // Assert
      Assert.True( val == defaultVal );
      Assert.True( val != value );
    }

  }


}