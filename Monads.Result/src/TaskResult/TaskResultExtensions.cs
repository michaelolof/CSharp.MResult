using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;


namespace Michaelolof.Monads.Result
{
  public static class TaskResultExtensions
  {

    #region Converters
    public static async Task<Result<IEnumerable<V>,E>> ToResult<V,E>(this Task<Partitions<V,E>> partitions) where E : Exception
    {
      try {
        var awaited = await partitions;
        if( awaited.HasErr() ) return awaited.GetErr().First();
        else return Result<IEnumerable<V>,E>.Ok( awaited.GetVal() );
      }
      catch(Exception ex) {
        return ex as E;
      }
    }
    #endregion


    #region Other Methods
    public static async Task<bool> IsOk<V,E>(this Task<Result<V,E>> result)
    {
      try {
        return (await result).IsOk;
      }
      catch {
        return false;
      }
    }

    public static async Task<bool> IsErr<V,E>(this Task<Result<V,E>> result)
    {
      try {
        return (await result).IsErr;
      }
      catch {
        return true;
      }
    }

    public static async Task<(V, E)> GetValueAndErr<V,E>(this Task<Result<V,E>> result) where E : Exception 
    { 
      try {
        return (await result).GetValueAndErr();
      }
      catch (Exception ex) {
        return (default(V), ex as E);
      }
    }

    public static async Task<V> GetValueOrDefault<V,E>(this Task<Result<V,E>> result, V defaultVal) where E : Exception 
    { 
      try {
        return (await result).GetValueOrDefault(defaultVal);
      }
      catch {
        return defaultVal;
      }
    }

    public static async Task<E> GetErrOrDefault<V,E>(this Task<Result<V,E>> result, E defaultErr) where E : Exception 
    { 
      try {
        return (await result).GetErrOrDefault(defaultErr);
      }
      catch {
        return defaultErr;
      }
    }
    #endregion
  
    
    #region OnOk Method Overloads
    public static async Task<Result<TV,E>> OnOk<V,E,TV>(this Task<Result<V,E>> result, TV value) where E : Exception 
    {
      try {
        return (await result).OnOk( value ); 
      }
      catch (Exception ex) {
        return ex as E;
      }
    }

    public static async Task<Result<TV,E>> OnOk<V,E,TV>(this Task<Result<V,E>> result, Func<TV> handler) where E : Exception 
    {
      try {
        return (await result).OnOk( handler ); 
      }
      catch (Exception ex) {
        return ex as E;
      }
    }

    public static async Task<Result<TV,E>> OnOk<V,E,TV>(this Task<Result<V,E>> result, Func<V, TV> handler ) where E : Exception
    {
      try {
        return (await result).OnOk( handler );
      }
      catch (Exception ex) {
        return ex as E;
      }
    }
    #endregion


    #region Then Method Overloads
    public async static Task<Result<TV,E>> Then<V,E,TV,VE>(this Task<Result<V,E>> result, Result<TV,VE> value) where VE : E where E : Exception
    {
      try {
        return (await result).Then( value );
      }
      catch (Exception ex)
      {
        return ex as E;
      }
    }

    public async static Task<Result<TV,E>> Then<V,E,TV,VE>(this Task<Result<V,E>> result, Func<Result<TV,VE>> handler) where VE : E where E : Exception
    {
      try {
        return (await result).Then( handler );
      }
      catch(Exception ex) {
        return ex as E;
      }
    }

    public async static Task<Result<TV,E>> Then<V,E,TV,VE>(this Task<Result<V,E>> result, Func<V, Result<TV,VE>> handler) where VE : E where E : Exception
    {
      try {
        return (await result).Then( handler );
      }
      catch(Exception ex) {
        return ex as E;
      }
    }

    public static async Task<Result<TV,E>> Then<V,E,TV,VE>(this Task<Result<V,E>> result, Func<V,Task<Result<TV,VE>>> handler) where VE : E where E : Exception
    {
     try {
        var awaitedResult = (await result);
        var (val, err) = awaitedResult.GetValueAndErr();
        if( awaitedResult.IsErr ) return err;
        return await result.Then( await handler( val ) );
      }
      catch(Exception ex) {
        return ex as E;
      }
    }

    public static async Task<Result<TV,E>> Then<V,E,TV,VE>(this Task<Result<V,E>> result, Func<Task<Result<TV,VE>>> handler) where VE : E where E : Exception
    {
      try {
        var awaitedResult = (await result);
        var (val, err) = awaitedResult.GetValueAndErr();
        if( awaitedResult.IsErr ) return err;
        return await result.Then( await handler() );
      }
      catch(Exception ex) {
        return ex as E;
      }
    }

    public static async Task<Result<TV,E>> Then<V,E,TV,VE>(this Task<Result<V,E>> result, Task<Result<TV,VE>> handler) where VE : E where E : Exception
    {
      try {
        var awaitedResult = (await result);
        var (val, err) = awaitedResult.GetValueAndErr();
        if( awaitedResult.IsErr ) return err;
        return await result.Then( await handler );
      }
      catch(Exception ex) {
        return ex as E;
      }
    }
    #endregion


    #region OnErr Method Overloads
    public static async Task<Result<V, TE>> OnErr<V,E,TE>(this Task<Result<V,E>> result, TE err) where TE : Exception
    {
      try {
        return (await result).OnErr( err );
      }
      catch (Exception ex)
      {
        return ex as TE;
      }
    }

    public static async Task<Result<V,TE>> OnErr<V,E,TE>(this Task<Result<V,E>> result, Func<TE> handler) where TE : Exception
    {
      try {
        return (await result).OnErr( handler );
      }
      catch (Exception ex)
      {
        return ex as TE;
      }
    }

    public static async Task<Result<V,TE>> OnErr<V,E,TE>(this Task<Result<V,E>> result, Func<E,TE> handler) where TE : Exception
    {
      try {
        return (await result).OnErr( handler );
      }
      catch (Exception ex)
      {
        return ex as TE;
      }
    }
    #endregion


    #region Catch Method Overloads
    public async static Task<Result<V,TE>> Catch<V,E,TE,EV>(this Task<Result<V,E>> result, Result<EV,TE> value) where EV : V where TE : Exception
    {
      try {
        return (await result).Catch( value );
      }
      catch (Exception ex)
      {
        return ex as TE;
      }
    }

    public async static Task<Result<V,TE>> Catch<V,E,TE,EV>(this Task<Result<V,E>> result, Func<Result<EV,TE>> handler) where EV : V where TE : Exception
    {
      try {
        return (await result).Catch( handler );
      }
      catch(Exception ex) {
        return ex as TE;
      }
    }

    public async static Task<Result<V,TE>> Catch<V,E,TE,EV>(this Task<Result<V,E>> result, Func<E, Result<EV,TE>> handler) where EV : V where TE : Exception
    {
      try {
        return (await result).Catch( handler );
      }
      catch(Exception ex) {
        return ex as TE;
      }
    }

    public async static Task<Result<V,TE>> Catch<V,E,EV,TE>(this Task<Result<V,E>> result, Task<Result<EV,TE>> alterative) where TE : Exception where EV : V
    {
      try {
        var awaitedResult = await result;
        var (val, err) = awaitedResult.GetValueAndErr();
        if( awaitedResult.IsOk ) return val;
        var awaited = await alterative;
        var (aval, aerr) = awaited.GetValueAndErr();
        if( awaited.IsOk ) return aval;
        else return aerr;
      }
      catch(Exception ex) {
        return (ex as TE)!;
      }
    }

    public static Task<Result<V,TE>> Catch<V,E,EV,TE>(this Task<Result<V,E>> result, Func<Task<Result<EV,TE>>> handler) where TE : Exception where EV : V
      => result.Catch( handler() );


    public async static Task<Result<V,TE>> Catch<V,E,EV,TE>(this Task<Result<V,E>> result, Func<E,Task<Result<EV,TE>>> handler) where TE : Exception where EV : V
    {
      try {
        var awaitedResult = await result;
        var (val, err) = awaitedResult.GetValueAndErr();
        if( awaitedResult.IsOk ) return val;
        var awaited = await handler( err );
        var (aval, aerr) = awaited.GetValueAndErr();
        if( awaited.IsOk ) return aval;
        else return aerr;
      }
      catch(Exception ex) {
        return (ex as TE)!;
      }
    }
    #endregion


    #region Match Method Overloads
    public static async Task<Result<TV,TE>> Match<V,E,TV,TE>(this Task<Result<V,E>> result, TV onOk, TE onErr) where TE : Exception 
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch(Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE>(this Task<Result<V,E>> result, TV onOk, Func<E, TE> onErr) where TE : Exception
    {
      try {
        return (await result).Match( onOk, onErr );
      } 
      catch (Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE>(this Task<Result<V,E>> result, Func<V, TV> onOk, TE onErr) where TE : Exception
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE>(this Task<Result<V,E>> result, Func<V,TV> onOk, Func<E, TE> onErr) where TE : Exception
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE,VE>(this Task<Result<V,E>> result, Result<TV,VE> onOk, TE onErr) where TE : Exception, VE
    { 
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE,EV>(this Task<Result<V,E>> result, TV onOk, Result<EV, TE> onErr) where TE : Exception where TV : class, EV
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE,VE>(this Task<Result<V,E>> result, Result<TV,VE> onOk, Func<E, TE> onErr) where TE :Exception, VE
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE,EV>(this Task<Result<V,E>> result, Func<V,TV> onOk, Result<EV, TE> onErr) where TV : class, EV where TE : Exception
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE,VE,EV>(this Task<Result<V,E>> result, Result<TV, VE> onOk, Result<EV,TE> onErr ) where TV : class, EV where TE : Exception, VE
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE,VE>(this Task<Result<V,E>> result, Func<V,Result<TV,VE>> onOk, TE onErr) where TE : Exception, VE
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex)
      {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE,EV>(this Task<Result<V,E>> result, TV onOk, Func<E, Result<EV, TE>> onErr) where TV : class, EV where TE : Exception
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE,VE>(this Task<Result<V,E>> result, Func<V,Result<TV,VE>> onOk, Func<E, TE> onErr) where TE : Exception, VE
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE,EV>(this Task<Result<V,E>> result, Func<V, TV> onOk, Func<E, Result<EV, TE>> onErr) where TV : class, EV where TE : Exception
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE,VE,EV>(this Task<Result<V,E>> result, Func<V,Result<TV,VE>> onOk, Result<EV,TE> onErr ) where TV : class, EV where TE : Exception, VE
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE,VE,EV>(this Task<Result<V,E>> result, Result<TV,VE> onOk, Func<E, Result<EV,TE>> onErr) where TV : class, EV where TE : Exception, VE
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex) {
        return ex as TE;
      }
    }

    public static async Task<Result<TV,TE>> Match<V,E,TV,TE,VE,EV>(this Task<Result<V,E>> result, Func<V, Result<TV,VE>> onOk, Func<E, Result<EV,TE>> onErr) where TV : class, EV where TE : Exception, VE
    {
      try {
        return (await result).Match( onOk, onErr );
      }
      catch (Exception ex) {
        return ex as TE;
      }
    }
    #endregion
  

    #region Awaiter Implementation
    public static TaskAwaiter<Exception> GetAwaiter<V,E>(this Task<Result<V,E>> result) where E : Exception => 
      throw new NotImplementedException("You can't await this type");
      // result.GetContinued().GetAwaiter();

    public static ConfiguredTaskAwaitable<Result<V,E>> ConfigureAwait<V,E>(this Task<Result<V,E>> result, bool continueOnCapturedContext) where E : Exception => 
      result.GetContinued().ConfigureAwait(continueOnCapturedContext);
      
    public static Task<Result<V,E>> GetContinued<V,E>(this Task<Result<V,E>> result) where E : Exception
    {
      return result.ContinueWith( t => {

        if( t.IsCanceled ) return new TaskCanceledException(t) as E;

        if (t.IsFaulted) return t.Exception as E;

        return t.Result;

      });
    }
    #endregion

  }

}
