using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

namespace Michaelolof.Monads.Result
{

  [AsyncMethodBuilder(typeof(ResultTaskMethodBuilder<>))]
  public class ResultTask<T>
  {

    #region Internal Fields
    internal ResultType type;
    internal object val;
    internal Exception err;
    #endregion


    #region Constructor
    private ResultTask(object val) {
      this.type = ResultType.Val;
      this.val = val;
      this.err = null;
    }

    private ResultTask(Exception err) {
      this.type = ResultType.Err;
      this.err = err;
      this.val = null;
    }
    #endregion


    #region Static Factories
    public static ResultTask<(V, E)> Ok<V,E>(V val) where E : Exception => ok<V,E>(() => Task.FromResult(val));

    public static ResultTask<(V, E)> Ok<V,E>(Task<V> val) where E : Exception => ok<V,E>(() => val);

    private static ResultTask<(V,E)> ok<V,E>(Func<Task<V>> val) where E : Exception {
      var result = new ResultTask<(V,E)>( val );
      return result;
    }


    public static ResultTask<(V,E)> Err<V,E>(E err) where E : Exception {
      var result = new ResultTask<(V,E)>( err );
      return result;
    }
    #endregion


    #region Internal Methods
    internal Func<Task<V>> dFunc<V>() => val as Func<Task<V>>;
    #endregion

  }

}