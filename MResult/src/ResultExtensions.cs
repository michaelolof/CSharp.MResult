using System;
using System.Threading.Tasks;

namespace Michaelolof.MResult
{
  public static class ResultExtensions
  {
    public static Result<V, E> ToOk<V, E>(this V val) => Result<V, E>.Ok( val );
    public static Result<V, E> ToErr<V, E>(this E err) => Result<V, E>.Err( err );

    public static T Merge<T>(this Result<T, T> result) {
      var (var, err) = result.GetValueAndErr();
      if( result.IsOk ) return var;
      else return err;
    }

    public static T MergeResult<T>(this (T, T) valAndErr ) => valAndErr.Item1 != null ? valAndErr.Item1 : valAndErr.Item2;

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

