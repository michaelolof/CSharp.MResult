using System;

namespace Michaelolof.MResult
{

  enum ResultType
  {
    Val,
    Err
  }

  public class Result<V, E>
  {

    V val = default!;
    E err = default!;
    ResultType type = ResultType.Err;

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

    public static Result<V, E> Ok(V val) => new Result<V, E>(val);
    public static Result<V, E> Err(E err) => new Result<V, E>(err);


    public bool IsOk => this.type == ResultType.Val;
    public bool IsErr => this.type == ResultType.Err;


    public Result<TV, TE> Transform<TV, TE>( TV val, TE err ) => IsOk ? new Result<TV, TE>(val) : new Result<TV, TE>(err);


    #region Match Method Overloads
    public Result<TV, TE> Match<TV, TE>(Func<V, TV> onOk, Func<E, TE> onErr)
    {
      if( IsOk ) return new Result<TV, TE>( onOk(val) );
      else return new Result<TV, TE>( onErr(err) );
    }

    public Result<TV, TE> Match<TV, TE, VE, EV>(Func<V, Result<TV,VE>> onOk, Func<E, Result<EV,TE>> onErr) where TV : class, EV where TE : class, VE
    {
      if( this.IsOk ) return this.flattenOkResult<TV, TE, VE, EV>( onOk );
      else return this.flattenErrResult<TV, TE, VE, EV>( onErr );
    }

    public Result<TV, TE> Match<TV, TE, VE>(Func<V, Result<TV,VE>> onOk, Func<E, TE> onErr) where TE : class, VE
    {
      if( IsErr ) return Result<TV, TE>.Err( onErr(this.err) );
      else return flattenOkResult<TV, TE, VE, TV>( onOk );
    }

    public Result<TV, TE> Match<TV, TE, EV>(Func<V, TV> onOk, Func<E, Result<EV, TE>> onErr) where TV : class, EV
    {
      if( IsOk ) return Result<TV, TE>.Ok( onOk( this.val ) );
      else return flattenErrResult<TV, TE, TE, EV>( onErr );
    }
    #endregion


    #region OnOk Method Overloads
    public Result<TV, E> OnOk<TV>(TV value) => Result<TV, E>.Ok( value );
    public Result<TV, E> OnOk<TV>(Func<TV> handler) => Result<TV, E>.Ok( handler() );
    public Result<TV, E> OnOk<TV>(Func<V, TV> handler) => Result<TV, E>.Ok( handler( this.val ) );
    
    public Result<TV, E> OnOk<TV, VE>(Result<TV, VE> result) where VE : E {
      if( result.IsErr ) return Result<TV, E>.Err( result.err );
      else return Result<TV, E>.Ok( result.val );            
    }

    public Result<TV, E> OnOk<TV, VE>(Func<Result<TV, VE>> handler ) where VE : E => OnOk( handler() );

    public Result<TV, E> OnOk<TV, VE>(Func<V, Result<TV, VE>> handler ) where VE : E => OnOk( handler( this.val ) );
    #endregion


    #region OnErr Method Overloads
    public Result<V, TE> OnErr<TE>(Func<E, TE> handler) => Result<V, TE>.Err( handler( this.err ) );

    public Result<V, TE> OnErr<TE>(Result<V,TE> result) {
      if( result.IsOk ) return Result<V, TE>.Ok( result.val );
      else return Result<V, TE>.Err( result.err );
    }

    public Result<V, TE> OnErr<TE>(Func<Result<V, TE>> handler ) => OnErr( handler() );

    public Result<V, TE>  OnErr<TE>(Func<E, Result<V, TE>> handler ) => OnErr( handler( err ) );
    #endregion

    public V GetValueOrNull() => IsOk ? val : default!;
    public V GetValueOrDefault(V defaultVal) => IsOk ? val : defaultVal;

    public E GetErrOrNull() => IsErr ? err : default!;
    public E GetErrOrDefault(E defaultErr) => IsErr ? err : defaultErr;

    private Result<TV, TE> flattenOkResult<TV, TE, VE, EV>(Func<V, Result<TV,VE>> onOk) where TV : EV where TE : class, VE
    {
      var flattenedResult = onOk( this.val );
      if( flattenedResult.IsErr ) return Result<TV, TE>.Err( flattenedResult.err as TE );
      else return Result<TV, TE>.Ok( flattenedResult.val );
    }

    private Result<TV, TE> flattenErrResult<TV, TE, VE, EV>(Func<E, Result<EV, TE>> onErr) where TV : class, EV where TE : VE 
    {
      var flattenedResult = onErr( this.err );
      if( flattenedResult.IsOk ) return Result<TV, TE>.Ok( flattenedResult.val as TV );
      else return Result<TV, TE>.Err( flattenedResult.err );
    }

  }


}

