using System;
using Xunit;

namespace Michaelolof.Result.Tests
{
  public class Result_IsOk__Tests
  {
    
    [Fact]
    public void IsOk__Passing__Tests()
    {
      // Arrange
      var strRslt = Result<string, Exception>.Ok("This is Ok");
      var intRslt = Result<int, Exception>.Ok( 20 );
      var classRslt = Result<MyClass, Exception>.Ok( new MyClass() );
      var structRslt = Result<MyStruct, Exception>.Ok( new MyStruct() );

      // Act
      var strIsOk = strRslt.IsOk;
      var intIsOk = intRslt.IsOk;
      var classIsOk = classRslt.IsOk;
      var structIsOk = structRslt.IsOk;

      // Assert
      Assert.True( strIsOk );
      Assert.True( intIsOk );
      Assert.True( classIsOk );
      Assert.True( structIsOk );
    }

    [Fact]
    public void IsOk__Failing__Tests()
    {
      // Arrange
      var strRslt = Result<string, Exception>.Err( new Exception("This is not ok"));
      var intRslt = Result<int, string>.Err( "This is an error" );
      var classRslt = Result<MyClass, int>.Err( 20 );
      var structRslt = Result<MyStruct, MyStruct>.Err( new MyStruct() );

      // Act
      var strIsOk = strRslt.IsOk;
      var intIsOk = intRslt.IsOk;
      var classIsOk = classRslt.IsOk;
      var structIsOk = structRslt.IsOk;

      // Assert
      Assert.False( strIsOk );
      Assert.False( intIsOk );
      Assert.False( classIsOk );
      Assert.False( structIsOk );
    }

    class MyClass {};
    struct MyStruct {};

  }

}
