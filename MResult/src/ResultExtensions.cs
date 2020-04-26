using System;
using System.Threading.Tasks;

namespace Michaelolof.MResult
{
  public static class ResultExtensions
  {
    public static Result<V, E> ToOk<V, E>(this V val) => Result<V, E>.Ok( val );
    public static Result<V, Exception> ToOk<V>(this V val) => Result<V, Exception>.Ok( val );
    public static Result<V, E> ToErr<V, E>(this E err) => Result<V, E>.Err( err );
    public static Result<V, Exception> ToErr<V>(this Exception err) => Result<V, Exception>.Err( err );

    public static T Merge<T>(this Result<T, T> result) {
      var (var, err) = result.GetValueAndErr();
      if( result.IsOk ) return var;
      else return err;
    }

    public static async Task<Result<T,Exception>> ToResult<T>(this Task<T> task) {
      try {
       var val = await task;
       return Result<T, Exception>.Ok( val );
      } 
      catch(Exception ex) {
        return Result<T,Exception>.Err( ex );
      }
    }
  }

}

