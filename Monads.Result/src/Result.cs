using System;

namespace Michaelolof.Monads.Result
{

  enum ResultType
  {
    Val,
    Err
  }

  ///<summary>
  /// Result Either Monad used to hold a truthy or falsy result state of an operation.
  ///</summary>  
  /// <typeparam name="V">Type of Value/Left field</typeparam>
  /// <typeparam name="E">Type of Err/Right field</typeparam>
  public class Result<V, E>
  {

    #region Private Fields
    V val = default!;
    E err = default!;
    ResultType type = ResultType.Err;
    #endregion


    #region Private Constructors      
    private Result(V val) 
    {
      this.type = ResultType.Val;
      this.val = val;
    }
    private Result(E err)
    {
      this.type = ResultType.Err;
      this.err = err;
    }
    #endregion


    #region Static Factories
    /// <summary>Creates an Ok/Truthy Result by assigning the argument to the Left Side (V) of the Result Monad</summary>
    public static Result<V, E> Ok(V val) => new Result<V, E>(val);

    /// <summary>Creates an Err/Falsy Result by assigning the argument to the Right Side (E) of the Result Monad</summary>
    public static Result<V, E> Err(E err) => new Result<V, E>(err);
    #endregion


    #region Properties
    /// <summary>Determines if the Result is Ok or exists in a truthy state</summary>
    public bool IsOk => this.type == ResultType.Val;
    
    /// <summary>Determines if the Result is Err or exists in a falsy state</summary>
    public bool IsErr => this.type == ResultType.Err;
    #endregion


    #region Match Method Overloads
    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE>(TV onOk, TE onErr) => IsOk ? Result<TV,TE>.Ok( onOk ) : Result<TV,TE>.Err( onErr );

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE>(TV onOk, Func<E, TE> onErr) 
    {
      if( IsOk ) return Result<TV, TE>.Ok( onOk );
      else return Result<TV, TE>.Err( onErr( err ) );
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE>(Func<V, TV> onOk, TE onErr)
    {
      if( IsOk ) return Result<TV, TE>.Ok( onOk( val ) );
      else return Result<TV, TE>.Err( onErr );
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE>(Func<V, TV> onOk, Func<E, TE> onErr)
    {
      if( IsOk ) return new Result<TV, TE>( onOk(val) );
      else return new Result<TV, TE>( onErr(err) );
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE, VE>(Result<TV,VE> onOk, TE onErr) where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, E>( onOk );
      else return Result<TV,TE>.Err( onErr );
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE, EV>(TV onOk, Result<EV, TE> onErr) where TV : class, EV
    {
      if( IsOk ) return Result<TV,TE>.Ok( onOk );
      else return flattenErrResult<TV, TE, E, EV>( onErr );
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE, VE>(Result<TV,VE> onOk, Func<E, TE> onErr) where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, E>( onOk );
      else return Result<TV, TE>.Err( onErr( err ) );
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE, EV>(Func<V,TV> onOk, Result<EV, TE> onErr) where TV : class, EV
    {
      if( IsOk ) return Result<TV, TE>.Ok( onOk( val ) );
      else return flattenErrResult<TV, TE, E, EV>( onErr );
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE, VE, EV>(Result<TV, VE> onOk, Result<EV,TE> onErr ) where TV : class, EV where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, EV>( onOk );
      else return flattenErrResult<TV, TE, VE, EV>( onErr );
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE, VE>(Func<V, Result<TV,VE>> onOk, TE onErr) where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, TV>( onOk );
      else return Result<TV, TE>.Err( onErr );    
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE, EV>(TV onOk, Func<E, Result<EV, TE>> onErr) where TV : class, EV
    {
      if( IsOk ) return Result<TV, TE>.Ok( onOk );
      else return flattenErrResult<TV, TE, TE, EV>( onErr );
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE, VE>(Func<V, Result<TV,VE>> onOk, Func<E, TE> onErr) where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, TV>( onOk );
      else return Result<TV, TE>.Err( onErr(this.err) );
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE, EV>(Func<V, TV> onOk, Func<E, Result<EV, TE>> onErr) where TV : class, EV
    {
      if( IsOk ) return Result<TV, TE>.Ok( onOk( this.val ) );
      else return flattenErrResult<TV, TE, TE, EV>( onErr );
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE, VE, EV>(Func<V, Result<TV,VE>> onOk, Result<EV,TE> onErr ) where TV : class, EV where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, EV>( onOk( val ) );
      else return flattenErrResult<TV, TE, VE, EV>( onErr );
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE, VE, EV>(Result<TV,VE> onOk, Func<E, Result<EV,TE>> onErr) where TV : class, EV where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, EV>( onOk );
      else return flattenErrResult<TV, TE, VE, EV>( onErr( err ) );    
    }

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    public Result<TV, TE> Match<TV, TE, VE, EV>(Func<V, Result<TV,VE>> onOk, Func<E, Result<EV,TE>> onErr) where TV : class, EV where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, EV>( onOk( val ) );
      else return flattenErrResult<TV, TE, VE, EV>( onErr( err ) );
    }
    #endregion


    #region OnOk Method Overloads
    /// <summary>Gets Excecuted when the Result is ok and Propagates the transformed Result. Here you can safely access the value of the monad and transform/return it.</summary>
    public Result<TV, E> OnOk<TV>(TV value) { 
      if( IsErr ) return Result<TV, E>.Err( err );
      else return Result<TV, E>.Ok( value );
    }

    /// <summary>Gets Excecuted when the Result is ok and Propagates the transformed Result. Here you can safely access the value of the monad and transform/return it.</summary>   
    public Result<TV, E> OnOk<TV>(Func<TV> handler) { 
      if( IsErr ) return Result<TV, E>.Err( err );
      else return OnOk( handler() );
    }

    /// <summary>Gets Excecuted when the Result is ok and Propagates the transformed Result. Here you can safely access the value of the monad and transform/return it.</summary>
    public Result<TV, E> OnOk<TV>(Func<V, TV> handler) { 
      if( IsErr ) return Result<TV, E>.Err( err );
      else return OnOk( handler( val ) );
    }
    
    /// <summary>Gets Excecuted when the Result is ok and Propagates the transformed Result. Here you can safely access the value of the monad and transform/return it.</summary>
    public Result<V, E> OnOk(Action<V> handler)
    { 
      if( IsErr ) return this;
      else {
        handler( val );
        return this;
      }
    }
    #endregion
    

    #region Then Method Overloads
    /// <summary>Gets Excecuted when the Result is ok and Propagates a flattened transformed Result. Here you can safely access the value of the monad and transform/return it. This is useful when you a chaning multiple methods that return Results</summary>
    public Result<TV, E> Then<TV, VE>(Result<TV, VE> result) where VE : E 
    {
      if( IsErr ) return Result<TV, E>.Err( err );
      
      if( result.IsErr ) return Result<TV, E>.Err( result.err );
    
      else return Result<TV, E>.Ok( result.val );            
    }

    /// <summary>Gets Excecuted when the Result is ok and Propagates a flattened transformed Result. Here you can safely access the value of the monad and transform/return it. This is useful when you a chaning multiple methods that return Results</summary>
    public Result<TV, E> Then<TV, VE>(Func<Result<TV, VE>> handler ) where VE : E 
    {
      if( IsErr ) return Result<TV, E>.Err( err );
      else return Then( handler() );
    }

    /// <summary>Gets Excecuted when the Result is ok and Propagates a flattened transformed Result. Here you can safely access the value of the monad and transform/return it. This is useful when you a chaning multiple methods that return Results</summary>
    public Result<TV, E> Then<TV, VE>(Func<V, Result<TV, VE>> handler ) where VE : E 
    {
      if( IsErr ) return Result<TV, E>.Err( err );
      else return Then( handler( val ) );
    }

    /// <summary>Gets Excecuted when the Result is ok and Propagates a flattened transformed Result. Here you can safely access the value of the monad and transform/return it. This is useful when you a chaning multiple methods that return Results</summary>
    public Result<V, E> Then(Action<V> handler)
    {
      if( IsErr ) return this;
      else {
        handler(val);
        return this;
      }
    }
    #endregion


    #region OnErr Method Overloads
    /// <summary>Gets Executed when the Result is Err/falsy and Propagates the transformed Result. Here you can safely access the err/right of your Result and transform/return it.</summary>
    public Result<V, TE> OnErr<TE>(TE err) 
    {
      if( IsOk ) return Result<V, TE>.Ok( val );
      else return Result<V, TE>.Err( err );
    }

    /// <summary>Gets Executed when the Result is Err/falsy and Propagates the transformed Result. Here you can safely access the err/right of your Result and transform/return it.</summary>
    public Result<V, TE> OnErr<TE>(Func<TE> handler)
    {
      if( IsOk ) return Result<V, TE>.Ok( val );
      else return Result<V, TE>.Err( handler() );
    }

    /// <summary>Gets Executed when the Result is Err/falsy and Propagates the transformed Result. Here you can safely access the err/right of your Result and transform/return it.</summary>
    public Result<V, TE> OnErr<TE>(Func<E, TE> handler) 
    { 
      if( IsOk ) return Result<V, TE>.Ok( val );
      else return Result<V, TE>.Err( handler( err ) );
    }

    /// <summary>Gets Executed when the Result is Err/falsy and Propagates the transformed Result. Here you can safely access the err/right of your Result and transform/return it.</summary>
    public Result<V, E> OnErr(Action<E> handler)
    {
      if( IsOk ) return this;
      else { 
        handler( err );
        return this;
      }
    }
    #endregion


    #region Catch Method Overloads
    /// <summary>Gets Excecuted when the Result is err and Propagates a flattened transformed Result. Here you can safely access the err/right of the monad and transform/return it. This is useful when you a retrying with multiple methods that return Results</summary>
    public Result<V, TE> Catch<TE, EV>(Result<EV,TE> result) where EV : V
    {
      if( IsOk ) return Result<V, TE>.Ok( val );
      else if( result.IsOk ) return Result<V, TE>.Ok( result.val );
      else return Result<V, TE>.Err( result.err );
    }

    /// <summary>Gets Excecuted when the Result is err and Propagates a flattened transformed Result. Here you can safely access the err/right of the monad and transform/return it. This is useful when you a retrying with multiple methods that return Results</summary>
    public Result<V, TE> Catch<TE, EV>(Func<Result<EV, TE>> handler ) where EV : V
    {
      if( IsOk ) return Result<V, TE>.Ok( val );
      var result = handler();
      if( result.IsOk ) return Result<V, TE>.Ok( result.val );
      else return Result<V, TE>.Err( result.err );
    }

    /// <summary>Gets Excecuted when the Result is err and Propagates a flattened transformed Result. Here you can safely access the err/right of the monad and transform/return it. This is useful when you a retrying with multiple methods that return Results</summary>
    public Result<V, TE> Catch<TE, EV>(Func<E, Result<EV, TE>> handler ) where EV : V
    {
      if( IsOk ) return Result<V, TE>.Ok( val );
      var result = handler(err);
      if( result.IsOk ) return Result<V, TE>.Ok( result.val );
      else return Result<V, TE>.Err( result.err );
    }

    /// <summary>Gets Excecuted when the Result is err and Propagates a flattened transformed Result. Here you can safely access the err/right of the monad and transform/return it. This is useful when you a retrying with multiple methods that return Results</summary>
    public Result<V, E> Catch(Action<E> handler)
    {
      if( IsOk ) return this;
      else {
        handler(err);
        return this;
      }
    }
    #endregion


    #region Other Methods
    /// <summary>If the Result is truthy, it returns the value else returns the default value</summary>
    public V GetValueOrDefault(V defaultVal) => IsOk ? val : defaultVal;

    /// <summary>If the Result is falsy, it returns the err else returns the default err</summary>
    public E GetErrOrDefault(E defaultErr) => IsErr ? err : defaultErr;

    /// <summary>Returns the value and err of the result in a tuple. CAUTION as both value or error may exist in thier default states.</summary>
    public (V, E) GetValueAndErr() => (val, err);
    #endregion


    #region Operators
    public static implicit operator Result<V, E>(V val) => new Result<V, E>( val );

    public static implicit operator Result<V, E>(E err) => new Result<V, E>( err );    
    #endregion


    #region Private Methods
    private Result<TV, TE> flattenOkResult<TV, TE, VE, EV>(Func<V, Result<TV,VE>> onOk) where TE : class, VE
    {
      return flattenOkResult<TV, TE, VE, EV>( onOk( val ) );
    }

    private Result<TV, TE> flattenOkResult<TV, TE, VE, EV>(Result<TV, VE> onOk) where TE : class, VE
    {
      if( onOk.IsErr ) return Result<TV, TE>.Err( (onOk.err as TE)! );
      else return Result<TV, TE>.Ok( onOk.val );
    }

    private Result<TV, TE> flattenErrResult<TV, TE, VE, EV>(Func<E, Result<EV, TE>> onErr) where TV : class, EV 
    {
      return flattenErrResult<TV, TE, VE, EV>( onErr( err ) );
    }

    private Result<TV, TE> flattenErrResult<TV, TE, VE, EV>(Result<EV, TE> onErr) where TV : class, EV 
    {
      if( onErr.IsOk ) return Result<TV, TE>.Ok( (onErr.val as TV)! );
      else return Result<TV, TE>.Err( onErr.err );
    }
    #endregion

  }

}

