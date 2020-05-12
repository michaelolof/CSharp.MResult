using System;
using System.Threading.Tasks;

namespace Michaelolof.Monads.Result
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

    public async static Task<Result<V,E>> Flip<V,E>(this Result<Task<V>,E> result) where E : Exception
    {
      var (taskVal, err) = result.GetValueAndErr();
      if( err != null ) return err;
      try { 
        return await taskVal;
      } 
      catch(Exception ex) {
        return ex as E;
      }
    }

    public static bool Exists<T>(this T value) => value != null;


    #region OnOk Overloads
    public async static Task<Result<TV,E>> OnOk<V,E,TV>(this Result<Task<V>,E> result, TV value) where E : Exception
    {
      if( result.IsErr ) return result.err;     
      try {
        var val = await result.val;
        return value;
      } 
      catch (Exception ex) {
        return ex as E;
      }
    }

    public async static Task<Result<TV,E>> OnOk<V,E,TV>(this Result<Task<V>,E> result, Func<TV> handler) where E : Exception
    {
      if( result.IsErr ) return result.err;
      try {
        var val = await result.val;
        return handler();
      } 
      catch (Exception ex) {
        return ex as E;
      }     
    }

    public async static Task<Result<TV,E>> OnOk<V,E,TV>(this Result<Task<V>,E> result, Func<V,TV> handler) where E : Exception
    {
      if( result.IsErr ) return result.err;
      try {
        var val = await result.val;
        return handler( val );
      }
      catch(Exception ex) {
        return ex as E;
      }
    }
    #endregion


    #region Then Overloads
    public async static Task<Result<TV,E>> Then<V,E,TV,VE>(this Result<Task<V>,E> result, Result<TV,VE> value) where E : Exception where VE : E
    {
      if( result.IsErr ) return result.err;
      try {
        var val = await result.val;
        if( value.IsErr ) return value.err;
        else return value.val;
      }
      catch(Exception ex) {
        return ex as E;
      }
    }

    public async static Task<Result<TV,E>> Then<V,E,TV,VE>(this Result<Task<V>,E> result, Func<Result<TV,VE>> handler) where E : Exception where VE : E
    {
      if( result.IsErr ) return result.err;
      try {
        var val = await result.val;
        var value = handler();
        if( value.IsErr ) return value.err;
        else return value.val;
      }
      catch(Exception ex) {
        return ex as E;
      }
    }

    public async static Task<Result<TV,E>> Then<V,E,TV,VE>(this Result<Task<V>,E> result, Func<V,Result<TV,VE>> handler) where E : Exception where VE : E
    {
      if( result.IsErr ) return result.err;
      try {
        var val = await result.val;
        var value = handler(val);
        if( value.IsErr ) return value.err;
        else return value.val;
      }
      catch(Exception ex) {
        return ex as E;
      }
    }
    #endregion

  }

}

