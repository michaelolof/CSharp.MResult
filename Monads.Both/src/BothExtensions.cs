using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Michaelolof.Monads.Result
{

  public static class BothListExtensions
  {

    #region Converters
    public static BothList<V,E> ToBothList<V,E>(this Result<IList<V>,E>[] listOfResults) 
    {
      var allVals = new List<V>();
      var allErss = new List<E>();

      
      foreach(var result in listOfResults) {
        var (val, err) = result.GetValueAndErr();
        if( err != null ) allErss.Add( err );
        else allVals.AddRange( val );
      } 

      return (allVals, allErss);
    }

    public static async Task<BothList<V,E>> ToBothList<V,E>(this Task<Result<IList<V>,E>[]> listOfResults) => (await listOfResults).ToBothList();
  
    #endregion


    #region Other Methods
    public static bool HasErr<V,E>(this BothList<V,E> both) => Has(both.err);
    public static bool HasVal<V,E>(this BothList<V,E> both) => Has(both.val);
    public static bool AllErr<V,E>(this BothList<V,E> both) => both.HasErr() && both.HasVal() == false;
    public static bool AllOk<V,E>(this BothList<V,E> both) => both.HasVal() && both.HasErr() == false;

    public static ICollection<V> GetVal<V,E>(this BothList<V,E> both) => both.val;
    public static ICollection<E> GetErr<V,E>(this BothList<V,E> both) => both.err;
    public static (ICollection<V>,ICollection<E>) GetValAndErr<V,E>(this BothList<V,E> both) => (both.GetVal(), both.GetErr());
    #endregion


    #region OnOk Overloads
    public static BothList<TV,E> OnOk<V,E,TV>(this BothList<V,E> both, TV value ) 
    {
      var vals = both.GetVal().Select( v => value);
      return new Result.BothList<TV,E>(vals.ToList(), both.GetErr());
    }

    public static BothList<TV,E> OnOk<V,E,TV>(this BothList<V,E> both, Func<TV> handler ) => both.OnOk( handler() );

    public static BothList<TV,E> OnOk<V,E,TV>(this BothList<V,E> both, Func<V,TV> handler ) 
    {
      var vals = both.GetVal().Select( handler );
      return new Result.BothList<TV,E>(vals.ToList(), both.GetErr());
    }
    #endregion


    #region Then Overloads
    public static BothList<TV,E> Then<V,E,TV,VE>(this BothList<V,E> both, BothList<TV,VE> monad ) where VE : E
    {
      var vals = both.GetVal().SelectMany( v => monad.GetVal() );
      var errs = both.GetErr().ToList();
      var monadErrs = monad.GetErr() as ICollection<E>; 
      errs.AddRange( monadErrs );
      return new Result.BothList<TV,E>(vals.ToList(), errs );
    }

    public static BothList<TV,E> Then<V,E,TV,VE>(this BothList<V,E> both, Func<BothList<TV,VE>> handler) where VE: E => both.Then( handler() );

    public static BothList<TV,E> Then<V,E,TV,VE>(this BothList<V,E> both, Func<V,BothList<TV,VE>> handler) where VE : E
    {
      var vals = both.GetVal().SelectMany( v => handler(v).GetVal() );
      var errs = both.GetErr().ToList();
      var monadErrs = both.val.SelectMany( v => handler( v ).GetErr() as ICollection<E> );
      errs.AddRange( monadErrs );
      return new Result.BothList<TV,E>(vals.ToList(), errs);
    }
    #endregion


    #region Catch Overloads
    public static BothList<V,TE> Catch<V,E,EV,TE>(this BothList<V,E> both, BothList<EV,TE> monad ) where V : EV 
    {
      var errs = both.GetErr().SelectMany( v => monad.GetErr() );
      var vals = both.GetVal().ToList();
      var monadVals = monad.GetVal() as ICollection<V>;
      vals.AddRange( monadVals );
      return new Result.BothList<V,TE>(vals, errs.ToList());
    }

    public static BothList<V,TE> Catch<V,E,EV,TE>(this BothList<V,E> both, Func<BothList<EV,TE>> handler ) where V : EV => both.Catch( handler() );

    public static BothList<V,TE> Catch<V,E,EV,TE>(this BothList<V,E> both, Func<E,BothList<EV,TE>> handler ) where V : EV
    {
      var errs = both.GetErr().SelectMany( e => handler(e).GetErr() );
      var vals = both.GetVal().ToList();
      var monadVals = both.err.SelectMany( e => handler(e).GetVal() as ICollection<V> );
      vals.AddRange( monadVals );
      return new Result.BothList<V,TE>(vals, errs.ToList());
    }
    
    #endregion


    public static ICollection<T> Merge<T>(this BothList<T,T> both) {
      var val = both.GetVal();
      val.ToList().AddRange( both.GetErr() );
      return val;
    }

    private static bool Has<T>(ICollection<T> stuff) => stuff.ToList().Count > 0;

  }


} 

