using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


namespace Michaelolof.Monads.Result
{
  public static class ResultTaskExtensions
  {

    #region Helper Factories
    public static ResultTask<(V, E)> ToOkTask<V,E>(this V val) where E : Exception => ResultTask<(V, E)>.Ok<V, E>( val );
    public static ResultTask<(V, E)> ToOkTask<V,E>(this Task<V> val) where E : Exception => ResultTask<(V, E)>.Ok<V, E>( val );
    public static ResultTask<(V, E)> ToErrTask<V,E>(this E err) where E : Exception => ResultTask<(V, E)>.Err<V, E>( err );
    public static (V, E) ToErrTask<V,E>(this Task<E> err) where E : Exception => ( default(V), err.GetAwaiter().GetResult() );
    #endregion

    
    private static async Task<V> Val<V,E>(this ResultTask<(V,E)> result) where E : Exception {
      var (val, _) =  await result.GetValueAndErr();
      return val;
    }

    private static async Task<E> Err<V,E>(this ResultTask<(V,E)> result) where E : Exception {
      var (_, err) =  await result.GetValueAndErr();
      return err;
    }


    #region Other Methods
    public static bool IsOk<V,E>(this ResultTask<(V,E)> result) {
      if( result.type == ResultType.Val ) {
        var d = result.dFunc<V>()();
        return d.IsFaulted == false && d.IsCanceled == false;
      }
      else return false;
    }

    public static bool IsErr<V,E>(this ResultTask<(V,E)> result)
    {
      return result.type == ResultType.Err || result.dFunc<V>()().IsFaulted || result.dFunc<V>()().IsCanceled;
    }

    public static async Task<(V, E)> GetValueAndErr<V,E>(this ResultTask<(V, E)> result) where E : Exception
    {
      V value = default(V);
      E error = default(E);

      try {
        value = await result.dFunc<V>()();
      }
      catch(Exception ex)
      {
        error = ex as E;
      }

      if( result.IsOk() ) return (value, error);
      else {
        if( result.err != null ) return (value, result.err as E);
        else return (value, error);
      }

    }

    public static async Task<E> GetErrOrDefault<E>(this ResultTask<(object,E)> result, E defaultErr) where E : Exception
    {
      var (_, err) = await result.GetValueAndErr();
      if( err != null ) return err;
      else return defaultErr;
    }

    public static async Task<V> GetValueOrDefault<V,E>(this ResultTask<(V,E)> result, V defaultVal) where E : Exception
    {
      var (val, err) = await result.GetValueAndErr();
      if( err == null ) return val;
      else return defaultVal;
    }
    #endregion


    #region Awaiter Implementation
    public static TaskAwaiter<(V, Exception)> GetAwaiter<V>(this ResultTask<(V,Exception)> result) => 
      result.GetContinued().GetAwaiter();

    public static ConfiguredTaskAwaitable<(V,Exception)> ConfigureAwait<V>(this ResultTask<(V,Exception)> result, bool continueOnCapturedContext) => 
      result.GetContinued().ConfigureAwait(continueOnCapturedContext);
      
    public static Task<(V, Exception)> GetContinued<V>(this ResultTask<(V,Exception)> result)
    {
      return result.dFunc<V>()().ContinueWith( t => {

        var value = default(V);
        var error = default(Exception);

        if( t.IsCanceled ) return (value, new TaskCanceledException(t));

        if (t.IsFaulted) return (value, t.Exception);

        return (t.Result, error);

      });
    }
    #endregion


    #region OnOk Method Overloads
    
    public static async ResultTask<(TV,E)> OnOk<V,E,TV>(this ResultTask<(V,E)> result, Func<V,TV> handler) where E : Exception
    {
      if( result.IsErr() ) return result.Err().ToErrTask<TV,E>();
      else return ( handler(await result.Val()), default(E) );
    }
    
    
    #endregion
  

  }

  public class MyTask<T> : Task<T>
  {
    public MyTask(Func<T> func) : base(func) {}

  }


}