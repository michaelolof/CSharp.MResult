using System;
using Xunit;


namespace Michaelolof.Result.Tests
{

  public class Resut_IsErr__Tests
  {

    [Fact]
    public void IsErr__Passing__Tests()
    {
      // Arrange
      var strRslt = Result<string, Exception>.Err( new Exception("This is not ok"));
      var intRslt = Result<int, string>.Err( "This is an error" );
      var classRslt = Result<MyClass, int>.Err( 20 );
      var structRslt = Result<MyStruct, MyStruct>.Err( new MyStruct() );

      // Act
      var strIsErr = strRslt.IsErr;
      var intIsErr = intRslt.IsErr;
      var classIsErr = classRslt.IsErr;
      var structIsOk = structRslt.IsErr;

      // Assert
      Assert.True( strIsErr );
      Assert.True( intIsErr );
      Assert.True( classIsErr );
      Assert.True( structIsOk );
    }

    [Fact]
    public void IsErr__Failing__Tests()
    {
      // Arrange
      var strRslt = Result<string, Exception>.Ok("This is Ok");
      var intRslt = Result<int, Exception>.Ok( 20 );
      var classRslt = Result<MyClass, Exception>.Ok( new MyClass() );
      var structRslt = Result<MyStruct, Exception>.Ok( new MyStruct() );

      // Act
      var strIsErr = strRslt.IsErr;
      var intIsErr = intRslt.IsErr;
      var classIsErr = classRslt.IsErr;
      var structIsErr = structRslt.IsErr;

      // Assert
      Assert.False( strIsErr );
      Assert.False( intIsErr );
      Assert.False( classIsErr );
      Assert.False( structIsErr );
    }


    class MyClass {};
    struct MyStruct {};


  }

}