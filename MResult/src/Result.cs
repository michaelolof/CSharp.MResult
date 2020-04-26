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



    #region Match Method Overloads

    public Result<TV, TE> Match<TV, TE>(TV onOk, TE onErr) => IsOk ? Result<TV,TE>.Ok( onOk ) : Result<TV,TE>.Err( onErr );

    public Result<TV, TE> Match<TV, TE>(TV onOk, Func<E, TE> onErr) 
    {
      if( IsOk ) return Result<TV, TE>.Ok( onOk );
      else return Result<TV, TE>.Err( onErr( err ) );
    }

    public Result<TV, TE> Match<TV, TE>(Func<V, TV> onOk, TE onErr)
    {
      if( IsOk ) return Result<TV, TE>.Ok( onOk( val ) );
      else return Result<TV, TE>.Err( onErr );
    }

    public Result<TV, TE> Match<TV, TE>(Func<V, TV> onOk, Func<E, TE> onErr)
    {
      if( IsOk ) return new Result<TV, TE>( onOk(val) );
      else return new Result<TV, TE>( onErr(err) );
    }


    public Result<TV, TE> Match<TV, TE, VE>(Result<TV,VE> onOk, TE onErr) where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, E>( onOk );
      else return Result<TV,TE>.Err( onErr );
    }

    public Result<TV, TE> Match<TV, TE, EV>(TV onOk, Result<EV, TE> onErr) where TV : class, EV
    {
      if( IsOk ) return Result<TV,TE>.Ok( onOk );
      else return flattenErrResult<TV, TE, E, EV>( onErr );
    }

    public Result<TV, TE> Match<TV, TE, VE>(Result<TV,VE> onOk, Func<E, TE> onErr) where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, E>( onOk );
      else return Result<TV, TE>.Err( onErr( err ) );
    }

    public Result<TV, TE> Match<TV, TE, EV>(Func<V,TV> onOk, Result<EV, TE> onErr) where TV : class, EV
    {
      if( IsOk ) return Result<TV, TE>.Ok( onOk( val ) );
      else return flattenErrResult<TV, TE, E, EV>( onErr );
    }

    public Result<TV, TE> Match<TV, TE, VE, EV>(Result<TV, VE> onOk, Result<EV,TE> onErr ) where TV : class, EV where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, EV>( onOk );
      else return flattenErrResult<TV, TE, VE, EV>( onErr );
    }


    public Result<TV, TE> Match<TV, TE, VE>(Func<V, Result<TV,VE>> onOk, TE onErr) where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, TV>( onOk );
      else return Result<TV, TE>.Err( onErr );    
    }

    public Result<TV, TE> Match<TV, TE, EV>(TV onOk, Func<E, Result<EV, TE>> onErr) where TV : class, EV
    {
      if( IsOk ) return Result<TV, TE>.Ok( onOk );
      else return flattenErrResult<TV, TE, TE, EV>( onErr );
    }

    public Result<TV, TE> Match<TV, TE, VE>(Func<V, Result<TV,VE>> onOk, Func<E, TE> onErr) where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, TV>( onOk );
      else return Result<TV, TE>.Err( onErr(this.err) );
    }

    public Result<TV, TE> Match<TV, TE, EV>(Func<V, TV> onOk, Func<E, Result<EV, TE>> onErr) where TV : class, EV
    {
      if( IsOk ) return Result<TV, TE>.Ok( onOk( this.val ) );
      else return flattenErrResult<TV, TE, TE, EV>( onErr );
    }

    public Result<TV, TE> Match<TV, TE, VE, EV>(Func<V, Result<TV,VE>> onOk, Result<EV,TE> onErr ) where TV : class, EV where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, EV>( onOk( val ) );
      else return flattenErrResult<TV, TE, VE, EV>( onErr );
    }

    public Result<TV, TE> Match<TV, TE, VE, EV>(Result<TV,VE> onOk, Func<E, Result<EV,TE>> onErr) where TV : class, EV where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, EV>( onOk );
      else return flattenErrResult<TV, TE, VE, EV>( onErr( err ) );    
    }

    public Result<TV, TE> Match<TV, TE, VE, EV>(Func<V, Result<TV,VE>> onOk, Func<E, Result<EV,TE>> onErr) where TV : class, EV where TE : class, VE
    {
      if( IsOk ) return flattenOkResult<TV, TE, VE, EV>( onOk( val ) );
      else return flattenErrResult<TV, TE, VE, EV>( onErr( err ) );
    }
    #endregion


    #region OnOk Method Overloads
    public Result<TV, E> OnOk<TV>(TV value) { 
      if( IsErr ) return Result<TV, E>.Err( err );
      else return Result<TV, E>.Ok( value );
    }
    
    public Result<TV, E> OnOk<TV>(Func<TV> handler) { 
      if( IsErr ) return Result<TV, E>.Err( err );
      else return OnOk( handler() );
    }

    public Result<TV, E> OnOk<TV>(Func<V, TV> handler) { 
      if( IsErr ) return Result<TV, E>.Err( err );
      else return OnOk( handler( val ) );
    }
    
    public Result<TV, E> OnOk<TV, VE>(Result<TV, VE> result) where VE : E {
      if( IsErr ) return Result<TV, E>.Err( err );
      
      if( result.IsErr ) return Result<TV, E>.Err( result.err );
    
      else return Result<TV, E>.Ok( result.val );            
    }

    public Result<TV, E> OnOk<TV, VE>(Func<Result<TV, VE>> handler ) where VE : E {
      if( IsErr ) return Result<TV, E>.Err( err );
      else return OnOk( handler() );
    }

    public Result<TV, E> OnOk<TV, VE>(Func<V, Result<TV, VE>> handler ) where VE : E {
      if( IsErr ) return Result<TV, E>.Err( err );
      else return OnOk( handler( val ) );
    }
    #endregion


    #region OnErr Method Overloads
    public Result<V, TE> OnErr<TE>(TE err) 
    {
      if( IsOk ) return Result<V, TE>.Ok( val );
      else return Result<V, TE>.Err( err );
    }

    public Result<V, TE> OnErr<TE>(Func<E, TE> handler) 
    { 
      if( IsOk ) return Result<V, TE>.Ok( val );
      else return Result<V, TE>.Err( handler( err ) );
    }

    public Result<V, E> OnErr(Action<E> handler)
    {
      if( IsOk ) return this;
      else { 
        handler( err );
        return this;
      }
    }

    public Result<V, TE> OnErr<TE, EV>(Result<EV,TE> result) where EV : V
    {
      if( IsOk ) return Result<V, TE>.Ok( val );
      else if( result.IsOk ) return Result<V, TE>.Ok( result.val );
      else return Result<V, TE>.Err( result.err );
    }

    public Result<V, TE> OnErr<TE, EV>(Func<Result<EV, TE>> handler ) where EV : V
    {
      if( IsOk ) return Result<V, TE>.Ok( val );
      var result = handler();
      if( result.IsOk ) return Result<V, TE>.Ok( result.val );
      else return Result<V, TE>.Err( result.err );
    }

    public Result<V, TE>  OnErr<TE, EV>(Func<E, Result<EV, TE>> handler ) where EV : V
    {
      if( IsOk ) return Result<V, TE>.Ok( val );
      var result = handler(err);
      if( result.IsOk ) return Result<V, TE>.Ok( result.val );
      else return Result<V, TE>.Err( result.err );
    }
    #endregion


    public V GetValueOrDefault(V defaultVal) => IsOk ? val : defaultVal;

    public E GetErrOrDefault(E defaultErr) => IsErr ? err : defaultErr;

    public (V, E) GetValueAndErr() => (val, err);

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

  }


}

