
using System;

namespace Michaelolof.Monads.Result
{

  ///<summary>
  /// Result Either Monad used to hold a truthy or falsy result state of an operation.
  ///</summary>  
  /// <typeparam name="V">Type of Value/Left field</typeparam>
  /// <typeparam name="E">Type of Err/Right field</typeparam>
  public interface IResult<V, E>
  {

    /// <summary>Determines if the Result is Ok or exists in a truthy state</summary>
    bool IsOk {get;}
    
    /// <summary>Determines if the Result is Err or exists in a falsy state</summary>
    bool IsErr {get;}


    #region Match Method Overloads
    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE>(TV onOk, TE onErr);

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE>(TV onOk, Func<E, TE> onErr); 

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE>(Func<V, TV> onOk, TE onErr);

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE>(Func<V, TV> onOk, Func<E, TE> onErr);

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE, VE>(IResult<TV,VE> onOk, TE onErr) where TE : class, VE;

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE, EV>(TV onOk, IResult<EV, TE> onErr) where TV : class, EV;

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE, VE>(IResult<TV,VE> onOk, Func<E, TE> onErr) where TE : class, VE;

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE, EV>(Func<V,TV> onOk, IResult<EV, TE> onErr) where TV : class, EV;

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE, VE, EV>(IResult<TV, VE> onOk, IResult<EV,TE> onErr ) where TV : class, EV where TE : class, VE;

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE, VE>(Func<V, IResult<TV,VE>> onOk, TE onErr) where TE : class, VE;

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE, EV>(TV onOk, Func<E, IResult<EV, TE>> onErr) where TV : class, EV;

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE, VE>(Func<V, IResult<TV,VE>> onOk, Func<E, TE> onErr) where TE : class, VE;

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE, EV>(Func<V, TV> onOk, Func<E, IResult<EV, TE>> onErr) where TV : class, EV;

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE, VE, EV>(Func<V, IResult<TV,VE>> onOk, IResult<EV,TE> onErr ) where TV : class, EV where TE : class, VE;

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE, VE, EV>(IResult<TV,VE> onOk, Func<E, IResult<EV,TE>> onErr) where TV : class, EV where TE : class, VE;

    /// <summary>Safely act on a Result by handling what to do when this result is ok/truthy and err/falsy</summary>
    IResult<TV, TE> Match<TV, TE, VE, EV>(Func<V, IResult<TV,VE>> onOk, Func<E, IResult<EV,TE>> onErr) where TV : class, EV where TE : class, VE;
    #endregion


    #region OnOk Method Overloads
    /// <summary>Gets Excecuted when the Result is ok and Propagates the transformed Result. Here you can safely access the value of the monad and transform/return it.</summary>
    IResult<TV, E> OnOk<TV>(TV value);

    /// <summary>Gets Excecuted when the Result is ok and Propagates the transformed Result. Here you can safely access the value of the monad and transform/return it.</summary>   
    IResult<TV, E> OnOk<TV>(Func<TV> handler);

    /// <summary>Gets Excecuted when the Result is ok and Propagates the transformed Result. Here you can safely access the value of the monad and transform/return it.</summary>
    IResult<TV, E> OnOk<TV>(Func<V, TV> handler);
    
    /// <summary>Gets Excecuted when the Result is ok and Propagates the transformed Result. Here you can safely access the value of the monad and transform/return it.</summary>
    IResult<V, E> OnOk(Action<V> handler);
    #endregion
    

    #region Then Method Overloads
    /// <summary>Gets Excecuted when the Result is ok and Propagates a flattened transformed Result. Here you can safely access the value of the monad and transform/return it. This is useful when you a chaning multiple methods that return Results</summary>
    IResult<TV, E> Then<TV, VE>(IResult<TV, VE> result) where VE : E;

    /// <summary>Gets Excecuted when the Result is ok and Propagates a flattened transformed Result. Here you can safely access the value of the monad and transform/return it. This is useful when you a chaning multiple methods that return Results</summary>
    IResult<TV, E> Then<TV, VE>(Func<IResult<TV, VE>> handler ) where VE : E ;

    /// <summary>Gets Excecuted when the Result is ok and Propagates a flattened transformed Result. Here you can safely access the value of the monad and transform/return it. This is useful when you a chaning multiple methods that return Results</summary>
    IResult<TV, E> Then<TV, VE>(Func<V, IResult<TV, VE>> handler ) where VE : E;

    /// <summary>Gets Excecuted when the Result is ok and Propagates a flattened transformed Result. Here you can safely access the value of the monad and transform/return it. This is useful when you a chaning multiple methods that return Results</summary>
    IResult<V, E> Then(Action<V> handler);
    #endregion


    #region OnErr Method Overloads
    /// <summary>Gets Executed when the Result is Err/falsy and Propagates the transformed Result. Here you can safely access the err/right of your Result and transform/return it.</summary>
    IResult<V, TE> OnErr<TE>(TE err);

    /// <summary>Gets Executed when the Result is Err/falsy and Propagates the transformed Result. Here you can safely access the err/right of your Result and transform/return it.</summary>
    IResult<V, TE> OnErr<TE>(Func<TE> handler);

    /// <summary>Gets Executed when the Result is Err/falsy and Propagates the transformed Result. Here you can safely access the err/right of your Result and transform/return it.</summary>
    IResult<V, TE> OnErr<TE>(Func<E, TE> handler);

    /// <summary>Gets Executed when the Result is Err/falsy and Propagates the transformed Result. Here you can safely access the err/right of your Result and transform/return it.</summary>
    IResult<V, E> OnErr(Action<E> handler);
    #endregion


    #region Catch Method Overloads
    /// <summary>Gets Excecuted when the Result is err and Propagates a flattened transformed Result. Here you can safely access the err/right of the monad and transform/return it. This is useful when you a retrying with multiple methods that return Results</summary>
    IResult<V, TE> Catch<TE, EV>(IResult<EV,TE> result) where EV : V;

    /// <summary>Gets Excecuted when the Result is err and Propagates a flattened transformed Result. Here you can safely access the err/right of the monad and transform/return it. This is useful when you a retrying with multiple methods that return Results</summary>
    IResult<V, TE> Catch<TE, EV>(Func<IResult<EV, TE>> handler ) where EV : V;

    /// <summary>Gets Excecuted when the Result is err and Propagates a flattened transformed Result. Here you can safely access the err/right of the monad and transform/return it. This is useful when you a retrying with multiple methods that return Results</summary>
    IResult<V, TE> Catch<TE, EV>(Func<E, IResult<EV, TE>> handler ) where EV : V;

    /// <summary>Gets Excecuted when the Result is err and Propagates a flattened transformed Result. Here you can safely access the err/right of the monad and transform/return it. This is useful when you a retrying with multiple methods that return Results</summary>
    IResult<V, E> Catch(Action<E> handler);
    #endregion


    #region Other Methods
    /// <summary>If the Result is truthy, it returns the value else returns the default value</summary>
    V GetValueOrDefault(V defaultVal);

    /// <summary>If the Result is falsy, it returns the err else returns the default err</summary>
    E GetErrOrDefault(E defaultErr);

    /// <summary>Returns the value and err of the result in a tuple. CAUTION as both value or error may exist in thier default states.</summary>
    (V, E) GetValueAndErr();
    #endregion



  }

}

