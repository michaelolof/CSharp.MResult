using System;
using System.Threading.Tasks;

namespace Michaelolof.MResult
{
  public static class ResultExtensions
  {
    /// <summary>Converts a Type V to an ok/truthy Result of V or E</summary>
    public static Result<V, E> ToOk<V, E>(this V val) => Result<V, E>.Ok( val );

    /// <summary>Converts a Type V to an ok/truthy Result of V or Exception</summary>
    public static Result<V, Exception> ToOk<V>(this V val) => Result<V, Exception>.Ok( val );

    /// <summary>Converts a Type E to an err/falsy Result of V or E</summary>
    public static Result<V, E> ToErr<V, E>(this E err) => Result<V, E>.Err( err );

    /// <summary>Converts a Type E to an err/falsy Result of V or Exception</summary>
    public static Result<V, Exception> ToErr<V>(this Exception err) => Result<V, Exception>.Err( err );

    /// <summary>Returns either the left or right side of a Result in that order.</summary>
    public static T Merge<T>(this Result<T, T> result) {
      var (var, err) = result.GetValueAndErr();
      if( result.IsOk ) return var;
      else return err;
    }

    /// <summary>Convets a Task of T to a Task of Result of T or Exception</summary>
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

