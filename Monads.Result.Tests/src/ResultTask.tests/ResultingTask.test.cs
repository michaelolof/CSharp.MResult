using System;
using Xunit;
using System.Threading.Tasks;

namespace Michaelolof.Monads.Result.Tests
{

  public class ResultTasks__Tests
  {

    [Fact]
    public async void Any_Test()
    {
      var result = 20.ToOkTask<int, Exception>();

      var dd = result.OnOk( v => {
        return v.ToString() + "Left";
      });

      var (var, err) = await hope( 30 ).GetValueAndErr();

      // result.OnOk<int, string, Exception>( v => {
      //   var d = v;
      //   return 20;
      // });


      Console.WriteLine("Done");
    }

    public async Task<Result<string, MyException>> hope( int n )
    {
      await Task.Delay( 3000 );
      if( n == 20 ) return "Okay";
      else return new MyException();
    }


  }

  public class MyException : Exception {}

}