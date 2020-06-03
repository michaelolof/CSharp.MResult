using System;
using System.Collections.Generic;
using System.Linq;

namespace Michaelolof.Monads.Result
{

  public static class PartitionsExtensions
  {

    #region Converters
    internal static Partitions<V,E> ToPartitions<V,E,EnumV>(this IEnumerable<Result<EnumV,E>> results) where EnumV : IEnumerable<V> 
    {
      var allVals = new List<V>();
      var allErss = new List<E>();

      foreach(var result in results) {
        var (val, err) = result.GetValueAndErr();
        if( err != null ) allErss.Add( err );
        else allVals.AddRange( val );
      } 

      return (allVals, allErss);
    }

    public static Partitions<V,E> ToPartitions<V,E>(this Result<V,E> result)
    {
      var allVals = new List<V>();
      var allErss = new List<E>();

      var (val, err) = result.GetValueAndErr();
      if( err.Exists() ) allErss.Add( err );
      else allVals.Add( val );

      return (allVals, allErss);
    }

    public static Partitions<V,E> ToPartitions<V,E>(this IEnumerable<Result<IEnumerable<V>,E>> results)
      => results.ToPartitions<V,E,IEnumerable<V>>();      
    public static Partitions<V,E> ToPartitions<V,E>(this IEnumerable<Result<V[],E>> results) 
      => results.ToPartitions<V,E,V[]>();
    public static Partitions<V,E> ToPartitions<V,E>(this IEnumerable<Result<IList<V>,E>> results) 
      => results.ToPartitions<V,E,IList<V>>();
    public static Partitions<V,E> ToPartitions<V,E>(this IEnumerable<Result<IQueryable<V>,E>> results) 
      => results.ToPartitions<V,E,IQueryable<V>>();

    public static Partitions<V,E> ToPartitions<V,E>(this IEnumerable<Result<V,E>> results)
    {
      var allVals = new List<V>();
      var allErss = new List<E>();
      
      foreach(var result in results) {
        var (val, err) = result.GetValueAndErr();
        if( err != null ) allErss.Add( err );
        else allVals.Add( val );
      } 

      return (allVals, allErss);
    }

    internal static Partitions<V,E> ToPartitions<V,E,EnumV>(this Result<EnumV,E> result) where EnumV : IEnumerable<V>
    {
      var allVals = new List<V>();
      var allErss = new List<E>();
      
      var (val, err) = result.GetValueAndErr();
      if( err != null ) allErss.Add( err );
      else allVals.AddRange( val );

      return (allVals, allErss);
    }

    public static Partitions<V,E> ToPartitions<V,E>(this Result<IEnumerable<V>,E> result) => result.ToPartitions<V,E,IEnumerable<V>>();
    public static Partitions<V,E> ToPartitions<V,E>(this Result<V[],E> result) => result.ToPartitions<V,E,V[]>();
    public static Partitions<V,E> ToPartitions<V,E>(this Result<IList<V>,E> result) => result.ToPartitions<V,E,IList<V>>();
    public static Partitions<V,E> ToPartitions<V,E>(this Result<IQueryable<V>,E> result) => result.ToPartitions<V,E,IQueryable<V>>();
    

    internal static Partitions<V,E> ToPartitions<V,E,EnumV,EnumE>(this Result<EnumV,EnumE> result) where EnumV : IEnumerable<V> where EnumE : IEnumerable<E>
    {
      var allVals = new List<V>();
      var allErss = new List<E>();
      
      var (val, err) = result.GetValueAndErr();
      allErss.AddRange( err );
      allVals.AddRange( val );

      return (allVals, allErss);
    }

    public static Partitions<V,E> ToPartitions<V,E>(this Result<IEnumerable<V>,IEnumerable<E>> result) 
      => result.ToPartitions<V,E,IEnumerable<V>,IEnumerable<E>>();
    public static Partitions<V,E> ToPartitions<V,E>(this Result<V[],E[]> result) 
      => result.ToPartitions<V,E,V[],E[]>();
    public static Partitions<V,E> ToPartitions<V,E>(this Result<IList<V>,IList<E>> result) 
      => result.ToPartitions<V,E,IList<V>,IList<E>>();
    public static Partitions<V,E> ToPartitions<V,E>(this Result<IQueryable<V>,IQueryable<E>> result) 
      => result.ToPartitions<V,E,IQueryable<V>,IQueryable<E>>();  
    #endregion


    #region Other Methods
    public static bool HasErr<V,E>(this Partitions<V,E> partitions) => Has(partitions.err);
    public static bool HasVal<V,E>(this Partitions<V,E> partitions) => Has(partitions.val);
    public static bool AllErr<V,E>(this Partitions<V,E> partitions) => partitions.HasErr() && partitions.HasVal() == false;
    public static bool AllOk<V,E>(this Partitions<V,E> partitions) => partitions.HasVal() && partitions.HasErr() == false;

    public static IEnumerable<V> GetVal<V,E>(this Partitions<V,E> partitions) => partitions.val;
    public static IEnumerable<E> GetErr<V,E>(this Partitions<V,E> partitions) => partitions.err;
    public static (IEnumerable<V>,IEnumerable<E>) GetValAndErr<V,E>(this Partitions<V,E> partitions) => (partitions.GetVal(), partitions.GetErr());
    #endregion


    #region OnOk Overloads
    public static Partitions<TV,E> OnOk<V,E,TV>(this Partitions<V,E> partition, TV value ) 
    {
      var vals = partition.GetVal().Select( v => value);
      return (vals, partition.GetErr());
    }

    public static Partitions<TV,E> OnOk<V,E,TV>(this Partitions<V,E> partition, Func<TV> handler ) 
    { 
      var vals = partition.GetVal().Select( v => handler());
      return (vals, partition.GetErr());
    }

    public static Partitions<TV,E> OnOk<V,E,TV>(this Partitions<V,E> partition, Func<V,TV> handler ) 
    {
      var vals = partition.GetVal().Select( handler );
      return (vals, partition.GetErr());
    }
    #endregion


    #region Then Overloads
    public static Partitions<TV,E> Then<V,E,TV,VE>(this Partitions<V,E> partitions, Partitions<TV,VE> monad ) where VE : E
    {
      var vals = partitions.GetVal().SelectMany( v => monad.GetVal() );
      var errs = partitions.GetErr();
      var monadErrs = monad.GetErr() as ICollection<E>; 
      errs.AddRange( monadErrs );
      return new Result.Partitions<TV,E>(vals, errs );
    }

    public static Partitions<TV,E> Then<V,E,TV,VE>(this Partitions<V,E> partitions, Func<Partitions<TV,VE>> handler) where VE: E => partitions.Then( handler() );

    public static Partitions<TV,E> Then<V,E,TV,VE>(this Partitions<V,E> partitions, Func<V,Partitions<TV,VE>> handler) where VE : E
    {
      var vals = partitions.GetVal().SelectMany( v => handler(v).GetVal() );
      var errs = partitions.GetErr();
      var monadErrs = partitions.val.SelectMany( v => handler( v ).GetErr() as ICollection<E> );
      errs.AddRange( monadErrs );
      return new Result.Partitions<TV,E>(vals, errs);
    }

    public static Partitions<TV,E> Then<V,E,TV,VE>(this Partitions<V,E> partitions, Result<TV,VE> result ) where VE : E
    {
      var monad = result.ToPartitions();
      var vals = partitions.GetVal().SelectMany( v => monad.GetVal() );
      var errs = partitions.GetErr();
      var monadErrs = monad.GetErr() as ICollection<E>; 
      errs.AddRange( monadErrs );
      return new Result.Partitions<TV,E>(vals, errs );
    }

    public static Partitions<TV,E> Then<V,E,TV,VE>(this Partitions<V,E> partitions, Func<Result<TV,VE>> handler) where VE: E => partitions.Then( handler() );

    public static Partitions<TV,E> Then<V,E,TV,VE>(this Partitions<V,E> partitions, Func<V,Result<TV,VE>> handler) where VE : E
    {
      var vals = partitions.GetVal().SelectMany( v => handler(v).ToPartitions().GetVal() );
      var errs = partitions.GetErr();
      var monadErrs = partitions.val.SelectMany( v => handler( v ).ToPartitions().GetErr() as ICollection<E> );
      errs.AddRange( monadErrs );
      return new Result.Partitions<TV,E>(vals, errs);
    }
    #endregion


    #region OnErr Overloads
    public static Partitions<V,TE> OnErr<V,E,TE>(this Partitions<V,E> partition, TE err) 
    {
      var errs = partition.GetErr().Select( e => err );
      return (partition.GetVal(), errs);
    }

    public static Partitions<V,TE> OnErr<V,E,TE>(this Partitions<V,E> partition, Func<TE> handler ) => partition.OnErr( handler() );

    public static Partitions<V,TE> OnErr<V,E,TE>(this Partitions<V,E> partition, Func<E,TE> handler ) 
    {
      var errs = partition.GetErr().Select( handler );
      return new Result.Partitions<V,TE>( partition.GetVal(), errs );
    }
    #endregion


    #region OnErrs
    public static Partitions<V,TE> OnErrs<V,E,EV,TE>(this Partitions<V,E> partitions, Func<IEnumerable<E>,Partitions<EV,TE>> handler) where TE : E where EV : V
    {
      var vals = partitions.GetVal();
      var errs = partitions.GetErr();

      var handledPartitions = handler( errs );
      var handledVals = handledPartitions.GetVal();
      var handledErrs = handledPartitions.GetErr();

      vals.AddRange( handledVals as IEnumerable<V> );

      return (vals, handledErrs);
    }
    #endregion


    #region Catch Overloads
    public static Partitions<V,TE> Catch<V,E,EV,TE>(this Partitions<V,E> partitions, Partitions<EV,TE> monad ) where V : EV 
    {
      var errs = partitions.GetErr().SelectMany( v => monad.GetErr() );
      var vals = partitions.GetVal();
      var monadVals = monad.GetVal() as ICollection<V>;
      vals.AddRange( monadVals );
      return new Result.Partitions<V,TE>(vals, errs);
    }

    public static Partitions<V,TE> Catch<V,E,EV,TE>(this Partitions<V,E> partitions, Func<Partitions<EV,TE>> handler ) where V : EV => partitions.Catch( handler() );

    public static Partitions<V,TE> Catch<V,E,EV,TE>(this Partitions<V,E> partitions, Func<E,Partitions<EV,TE>> handler ) where V : EV
    {
      var errs = partitions.GetErr().SelectMany( e => handler(e).GetErr() );
      var vals = partitions.GetVal();
      var monadVals = partitions.err.SelectMany( e => handler(e).GetVal() as ICollection<V> );
      vals.AddRange( monadVals );
      return new Result.Partitions<V,TE>(vals, errs);
    }

    public static Partitions<V,TE> Catch<V,E,EV,TE>(this Partitions<V,E> partitions, Result<EV,TE> result ) where V : EV 
    {
      var resultPartitions = result.ToPartitions();
      var errs = partitions.GetErr().SelectMany( v => resultPartitions.GetErr() );
      var vals = partitions.GetVal();
      var monadVals = resultPartitions.GetVal() as ICollection<V>;
      vals.AddRange( monadVals );
      return new Result.Partitions<V,TE>(vals, errs);
    }

    public static Partitions<V,TE> Catch<V,E,EV,TE>(this Partitions<V,E> partitions, Func<Result<EV,TE>> handler ) where V : EV => partitions.Catch( handler() );

    public static Partitions<V,TE> Catch<V,E,EV,TE>(this Partitions<V,E> partitions, Func<E,Result<EV,TE>> handler ) where V : EV
    {
      var errs = partitions.GetErr().SelectMany( e => handler(e).ToPartitions().GetErr() );
      var vals = partitions.GetVal();
      var monadVals = partitions.err.SelectMany( e => handler(e).ToPartitions().GetVal() as ICollection<V> );
      vals.AddRange( monadVals );
      return new Result.Partitions<V,TE>(vals, errs);
    }
    #endregion
   
    public static IEnumerable<T> Merge<T>(this Partitions<T,T> partitions) {
      var val = partitions.GetVal();
      val.AddRange( partitions.GetErr() );
      return val;
    }

    private static bool Has<T>(IEnumerable<T> stuff) => stuff.ContainsAValue();

  }


} 

