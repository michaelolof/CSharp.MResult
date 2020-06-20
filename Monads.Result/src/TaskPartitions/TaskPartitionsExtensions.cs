using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Michaelolof.Monads.Result
{

  public static class TaskPartitionsExtensions
  {

    #region Converters
    public static Partitions<V,E> ToPartitions<V,E>(this E err) where E : Exception => (new List<V>(), new List<E>{err});
    public static Partitions<V,E> ToPartitions<V,E>(this Exception err) where E : Exception => (new List<V>(), new List<E>{err as E});
    
    private static async Task<Partitions<V,E>> ToPartitions<V,E,EnumV>(this Task<IList<Result<EnumV,E>>> results) where EnumV : IList<V> where E : Exception
    {
      try {
        return (await results).ToPartitions<V,E,EnumV>();
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }

    }

    public static async Task<Partitions<V,E>> ToPartitions<V,E>(this Task<IList<Result<IEnumerable<V>,E>>> results) where E : Exception
    {
      try {
        return (await results).ToPartitions<V,E,IEnumerable<V>>();      
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }

    }

    public static async Task<Partitions<V,E>> ToPartitions<V,E>(this Task<IList<Result<V[],E>>> results) where E : Exception
    {
      try {
        return (await results).ToPartitions<V,E,V[]>();
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();  
      }
    }

    public static async Task<Partitions<V,E>> ToPartitions<V,E>(this Task<IList<Result<IList<V>,E>>> results) where E : Exception
    {
      try {
        return (await results).ToPartitions<V,E,IList<V>>();
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }
    }

    public static async Task<Partitions<V,E>> ToPartitions<V,E>(this Task<IList<Result<IQueryable<V>,E>>> results) where E : Exception
    {
      try {
        return (await results).ToPartitions<V,E,IQueryable<V>>();
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }
    } 
   
    public static async Task<Partitions<V,E>> ToPartitions<V,E>(this Task<IList<Result<V,E>>> results) where E : Exception
    {
      try {
        return (await results).ToPartitions();
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }
    }

    private static async Task<Partitions<V,E>> ToPartitions<V,E,EnumV>(this Task<Result<EnumV,E>> result) where EnumV : IEnumerable<V> where E : Exception
    {
      try {
        return (await result).ToPartitions<V,E,EnumV>();
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }
    }

    public static async Task<Partitions<V,E>> ToPartitions<V,E>(this Task<Result<IEnumerable<V>,E>> result) where E : Exception
    {
      try {
        return (await result).ToPartitions<V,E,IEnumerable<V>>();
      }
      catch (Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }
    }

    public static async Task<Partitions<V,E>> ToPartitions<V,E>(this Task<Result<V[],E>> result) where E : Exception
    {
      try {
        return (await result).ToPartitions<V,E,V[]>();
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }
    }

    public static async Task<Partitions<V,E>> ToPartitions<V,E>(this Task<Result<IList<V>,E>> result) where E : Exception
    {
      try {
        return (await result).ToPartitions<V,E,IList<V>>();
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }
    } 

    public static async Task<Partitions<V,E>> ToPartitions<V,E>(this Task<Result<IQueryable<V>,E>> result) where E : Exception
    {
      try {
        return (await result).ToPartitions<V,E,IQueryable<V>>();
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }
    }
    

    private static async Task<Partitions<V,E>> ToPartitions<V,E,EnumV,EnumE>(this Task<Result<EnumV,EnumE>> result) where EnumV : IEnumerable<V> where EnumE : IEnumerable<E> where E : Exception
    {
      try {
        return (await result).ToPartitions<V,E,EnumV,EnumE>();
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }
    }

    public static async Task<Partitions<V,E>> ToPartitions<V,E>(this Task<Result<IEnumerable<V>,IEnumerable<E>>> result) where E : Exception
    {
      try {
        return (await result).ToPartitions<V,E,IEnumerable<V>,IEnumerable<E>>();
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }
    }

    public static async Task<Partitions<V,E>> ToPartitions<V,E>(this Task<Result<V[],E[]>> result) where E : Exception
    {
      try {
        return (await result).ToPartitions<V,E,V[],E[]>();
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }
    } 
    public static async Task<Partitions<V,E>> ToPartitions<V,E>(this Task<Result<IList<V>,IList<E>>> result) where E : Exception
    {
      try {
        return (await result).ToPartitions<V,E,IList<V>,IList<E>>();
      }
      catch (Exception ex) {
        return ex.ToPartitions<V,E>();
      }
    }
    public static async Task<Partitions<V,E>> ToPartitions<V,E>(this Task<Result<IQueryable<V>,IQueryable<E>>> result) where E : Exception
    {
      try {
        return (await result).ToPartitions<V,E,IQueryable<V>,IQueryable<E>>();  
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,E>();
      }
    }
    #endregion


    #region Other Methods
    public static async Task<bool> HasErr<V,E>(this Task<Partitions<V,E>> partition)
    {
      try {
        return (await partition).HasErr();
      }
      catch {
        return true;
      }
    } 
    public static async Task<bool> HasVal<V,E>(this Task<Partitions<V,E>> partition)
    {
      try {
        return (await partition).HasVal();
      }
      catch{
        return false;
      }
    } 
    public static async Task<bool> AllErr<V,E>(this Task<Partitions<V,E>> partition)
    {
      try {
        return (await partition).HasErr() && (await partition).HasVal() == false;
      }
      catch {
        return true;
      }
    } 
    public static async Task<bool> AllOk<V,E>(this Task<Partitions<V,E>> partition) 
    {
      try {
        return (await partition).HasVal() && (await partition).HasErr() == false;
      }
      catch {
        return false;
      }
    } 
    public static async Task<IEnumerable<V>> GetVal<V,E>(this Task<Partitions<V,E>> partitions) {
      try {
        return (await partitions).val;
      }
      catch {
        return new List<V>();
      }

    }
    public static async Task<IEnumerable<E>> GetErr<V,E>(this Task<Partitions<V,E>> partitions) where E : Exception
    {
      try {
        return (await partitions).err;
      }
      catch(Exception ex)
      {
        return new List<E>(){ ex as E };
      }
    }
    public static async Task<(IEnumerable<V>,IEnumerable<E>)> GetValAndErr<V,E>(this Task<Partitions<V,E>> partitions) where E : Exception
    {
      try {
        return ((await partitions).GetVal(), (await partitions).GetErr());
      }
      catch (Exception ex)
      {
        return (new List<V>(), new List<E>(){ex as E});
      }
    }
    #endregion


    #region OnOk Overloads
    public static async Task<Partitions<TV,E>> OnOk<V,E,TV>(this Task<Partitions<V,E>> partition, TV value ) where E : Exception
    {
      try {
        return (await partition).OnOk( value );
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<TV,E>();
      }
    }

    public static Task<Partitions<TV,E>> OnOk<V,E,TV>(this Task<Partitions<V,E>> partition, Func<TV> handler ) where E : Exception  
      => partition.OnOk( handler() ) ;

    public static async Task<Partitions<TV,E>> OnOk<V,E,TV>(this Task<Partitions<V,E>> partition, Func<V,TV> handler ) where E : Exception 
    {
      try {
        return (await partition).OnOk( handler );
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<TV,E>();
      }
    }

    public static async Task<Partitions<TV,E>> OnOk<V,E,TV>(this Task<Partitions<V,E>> partition, Task<TV> value ) where E : Exception
    {
      try {
        return await partition.OnOk( await value );
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<TV,E>();
      }
    }

    public static Task<Partitions<TV,E>> OnOk<V,E,TV>(this Task<Partitions<V,E>> partition, Func<Task<TV>> handler ) where E : Exception  
      => partition.OnOk( handler() ) ;

    public static async Task<Partitions<TV,E>> OnOk<V,E,TV>(this Task<Partitions<V,E>> partition, Func<V,Task<TV>> handler ) where E : Exception 
    {
      try {
        var vals = await Task.WhenAll( (await partition.GetVal()).Select( handler ) );
        return (vals.ToList(), await partition.GetErr());
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<TV,E>();
      }
    }
    #endregion


    #region Then Overloads
    public static async Task<Partitions<TV,E>> Then<V,E,TV,VE>(this Task<Partitions<V,E>> partitions, Partitions<TV,VE> monad ) where VE : E where E : Exception
    {
      try {
        var vals = (await partitions).GetVal().SelectMany( v => monad.GetVal() );
        var errs = (await partitions).GetErr().ToList();
        var monadErrs = monad.GetErr() as ICollection<E>; 
        errs.AddRange( monadErrs );
        return (vals.ToList(), errs );
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<TV,E>();
      }
    }

    public static Task<Partitions<TV,E>> Then<V,E,TV,VE>(this Task<Partitions<V,E>> partitions, Func<Partitions<TV,VE>> handler) where VE: E where E : Exception => partitions.Then( handler() );

    public static async Task<Partitions<TV,E>> Then<V,E,TV,VE>(this Task<Partitions<V,E>> partitions, Func<V,Partitions<TV,VE>> handler) where VE : E where E : Exception
    {
      try {
        var vals = (await partitions).GetVal().SelectMany( v => handler(v).GetVal() );
        var errs = (await partitions).GetErr().ToList();
        var monadErrs = (await partitions).val.SelectMany( v => handler( v ).GetErr() as ICollection<E> );
        errs.AddRange( monadErrs );
        return (vals.ToList(), errs);
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<TV,E>();
      }
    }

    public static async Task<Partitions<TV,E>> Then<V,E,TV,VE>(this Task<Partitions<V,E>> partitions, Result<TV,VE> result ) where VE : E where E : Exception
    {
      try {
        var monad = result.ToPartitions();
        var vals = (await partitions).GetVal().SelectMany( v => monad.GetVal() );
        var errs = (await partitions).GetErr().ToList();
        var monadErrs = monad.GetErr() as ICollection<E>; 
        errs.AddRange( monadErrs );
        return (vals.ToList(), errs );
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<TV,E>();
      }
    }

    public static Task<Partitions<TV,E>> Then<V,E,TV,VE>(this Task<Partitions<V,E>> partitions, Func<Result<TV,VE>> handler) where VE: E where E : Exception => partitions.Then( handler() );

    public static async Task<Partitions<TV,E>> Then<V,E,TV,VE>(this Task<Partitions<V,E>> partitions, Func<V,Result<TV,VE>> handler) where VE : E where E : Exception
    {
      var vals = (await partitions).GetVal().SelectMany( v => handler(v).ToPartitions().GetVal() );
      var errs = (await partitions).GetErr().ToList();
      var monadErrs = (await partitions).val.SelectMany( v => handler( v ).ToPartitions().GetErr() as ICollection<E> );
      errs.AddRange( monadErrs );
      return (vals.ToList(), errs);
    }

    public static async Task<Partitions<TV,E>> Then<V,E,TV,VE>(this Task<Partitions<V,E>> partitions, Task<Partitions<TV,VE>> monad ) where VE : E where E : Exception
    {
      try {
        var monadVals = await monad.GetVal();
        var vals = (await partitions).GetVal().SelectMany( v => monadVals );
        var errs = (await partitions).GetErr().ToList();
        var monadErrs = monad.GetErr() as ICollection<E>; 
        errs.AddRange( monadErrs );
        return (vals.ToList(), errs );
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<TV,E>();
      }
    }

    public static Task<Partitions<TV,E>> Then<V,E,TV,VE>(this Task<Partitions<V,E>> partitions, Func<Task<Partitions<TV,VE>>> handler) where VE: E where E : Exception => partitions.Then( handler() );

    public static async Task<Partitions<TV,E>> Then<V,E,TV,VE>(this Task<Partitions<V,E>> partitions, Func<V,Task<Partitions<TV,VE>>> handler) where VE : E where E : Exception
    {
      try {
        var monadVals = await Task.WhenAll((await partitions).GetVal().Select( v => handler(v).GetVal() ));
        var vals = monadVals.SelectMany( v => v );
        var errs = (await partitions).GetErr().ToList();
        var monadErrs = (await partitions).val.SelectMany( v => handler( v ).GetErr() as ICollection<E> );
        errs.AddRange( monadErrs );
        return (vals.ToList(), errs);
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<TV,E>();
      }
    }

    public static async Task<Partitions<TV,E>> Then<V,E,TV,VE>(this Task<Partitions<V,E>> partitions, Task<Result<TV,VE>> result ) where VE : E where E : Exception
    {
      try {
        var monad = (await result).ToPartitions();
        var vals = (await partitions).GetVal().SelectMany( v => monad.GetVal() );
        var errs = (await partitions).GetErr().ToList();
        var monadErrs = monad.GetErr() as ICollection<E>; 
        errs.AddRange( monadErrs );
        return (vals.ToList(), errs );
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<TV,E>();
      }
    }

    public static Task<Partitions<TV,E>> Then<V,E,TV,VE>(this Task<Partitions<V,E>> partitions, Func<Task<Result<TV,VE>>> handler) where VE: E where E : Exception => partitions.Then( handler() );

    public static async Task<Partitions<TV,E>> Then<V,E,TV,VE>(this Task<Partitions<V,E>> partitions, Func<V,Task<Result<TV,VE>>> handler) where VE : E where E : Exception
    {
      var monadVals = await Task.WhenAll((await partitions).GetVal().Select( async v => (await handler(v)).ToPartitions().GetVal() ));
      var vals = monadVals.SelectMany( v => v );
      var errs = (await partitions).GetErr().ToList();
      var monadErrsOfErrs = await Task.WhenAll((await partitions).val.Select( async v => (await handler( v )).ToPartitions().GetErr() ));
      var monadErrs = monadErrsOfErrs.SelectMany( e => e );
      errs.AddRange( monadErrs );
      return (vals.ToList(), errs);
    }
    #endregion


    #region OnOks Overloads
    public static async Task<Partitions<TV,E>> OnOks<V,E,TV>(this Task<Partitions<V,E>> partitions, IList<TV> values) where E : Exception
    {
      try {
        var awaitedP = await partitions;
        var errs = awaitedP.GetErr();
        return (values, errs);
      }
      catch(Exception ex) {
        return ex.ToPartitions<TV,E>();
      }      
    }
    public static Task<Partitions<TV,E>> OnOks<V,E,TV>(this Task<Partitions<V,E>> partitions, Func<IList<TV>> handler) where E : Exception
      => partitions.OnOks( handler() );
    

    public static async Task<Partitions<TV,E>> OnOks<V,E,TV>(this Task<Partitions<V,E>> partitions, Func<IEnumerable<V>, IList<TV>> handler) where E : Exception
    {
      try {
        var awaitedP = await partitions;
        var pVals = awaitedP.GetVal();
        var tVals = handler( pVals );
        return (tVals, awaitedP.GetErr());
      }
      catch(Exception ex) {
        return ex.ToPartitions<TV,E>();
      }      
    }

    public static async Task<Partitions<V,E>> OnOks<V,E>(this Task<Partitions<V,E>> partitions, Action<IEnumerable<V>> handler ) where E : Exception
    {
      try {
        var awaitedP = await partitions;
        var pVals = awaitedP.GetVal();
        handler( pVals );
        return awaitedP;
      }
      catch(Exception ex) {
        return ex.ToPartitions<V,E>();
      }
    }
    #endregion


    #region OnErr Overloads
    public static async Task<Partitions<V,TE>> OnErr<V,E,TE>(this Task<Partitions<V,E>> partition, TE err) where TE : Exception
    {
      try {
        return (await partition).OnErr( err );
      }
      catch(Exception ex) {
        return ex.ToPartitions<V,TE>();
      }
    }

    public static Task<Partitions<V,TE>> OnErr<V,E,TE>(this Task<Partitions<V,E>> partition, Func<TE> handler) where TE : Exception => partition.OnErr( handler() );

    public static async Task<Partitions<V,TE>> OnErr<V,E,TE>(this Task<Partitions<V,E>> partition, Func<E,TE> handler )  where TE : Exception
    {
      try {
        var awaitedPartitions = await partition;
        var errs = awaitedPartitions.GetErr().Select( handler ).ToList();
        return (awaitedPartitions.GetVal(), errs );
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,TE>();
      }
    }

    public static async Task<Partitions<V,TE>> OnErr<V,E,TE>(this Task<Partitions<V,E>> partition, Task<TE> err) where TE : Exception
    {
      try {
        return (await partition).OnErr( await err );
      }
      catch(Exception ex) {
        return ex.ToPartitions<V,TE>();
      }
    }

    public static Task<Partitions<V,TE>> OnErr<V,E,TE>(this Task<Partitions<V,E>> partition, Func<Task<TE>> handler) where TE : Exception => partition.OnErr( handler() );

    public static async Task<Partitions<V,TE>> OnErr<V,E,TE>(this Task<Partitions<V,E>> partition, Func<E,Task<TE>> handler )  where TE : Exception
    {
      try {
        var awaitedPartitions = await partition;
        var errs = await Task.WhenAll( awaitedPartitions.GetErr().Select( handler ).ToList() );
        return new Result.Partitions<V,TE>( awaitedPartitions.GetVal(), errs );
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,TE>();
      }
    }    
    #endregion
    public static async Task<Partitions<V,TE>> OnErrs<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Func<IEnumerable<E>,Partitions<EV,TE>> handler) where E : Exception where TE : E where EV : V
    {
      try {
        return (await partitions).OnErrs( handler );
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,TE>();
      }
    }

    public static async Task<Partitions<V,TE>> OnErrs<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Func<IEnumerable<E>,Task<Partitions<EV,TE>>> handler) where E : Exception where TE : E where EV : V
    {
      try {
        var ap = await partitions;
        var vals = ap.GetVal();
        var errs = ap.GetErr();

        var handledParts = await handler( errs );
        var handledVals = handledParts.GetVal();
        var handledErrs = handledParts.GetErr();

        vals.AddRange( handledVals as IEnumerable<V> );
        return (vals, handledErrs); 
      }
      catch(Exception ex) 
      {
        return ex.ToPartitions<V,TE>();  
      }
    }

    public static async Task<Partitions<V,TE>> OnErrs<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Func<Task<IEnumerable<E>>,Task<Partitions<EV,TE>>> handler) where E : Exception where TE : E where EV : V
    {
      try {
        var errs = partitions.GetErr();

        var handledParts = await handler( errs );
        var handledVals = handledParts.GetVal();
        var handledErrs = handledParts.GetErr();

        var vals = await partitions.GetVal();

        vals.AddRange( handledVals as IEnumerable<V> );
        return (vals, handledErrs); 
      }
      catch(Exception ex) 
      {
        return ex.ToPartitions<V,TE>();  
      }
    }


    #region OnErrs Overloads


    #endregion


    #region Catch Overloads
    public static async Task<Partitions<V,TE>> Catch<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Partitions<EV,TE> monad ) where V : EV where TE : Exception 
    {
      try {
        var awaitedPartitions = await partitions;
        var errs = awaitedPartitions.GetErr().SelectMany( v => monad.GetErr() );
        var vals = awaitedPartitions.GetVal().ToList();
        var monadVals = monad.GetVal() as ICollection<V>;
        vals.AddRange( monadVals );
        return (vals, errs.ToList());
      }
      catch(Exception ex) {
        return ex.ToPartitions<V,TE>();
      }
    }

    public static Task<Partitions<V,TE>> Catch<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Func<Partitions<EV,TE>> handler ) where V : EV where TE : Exception => partitions.Catch( handler() );

    public static async Task<Partitions<V,TE>> Catch<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Func<E,Partitions<EV,TE>> handler ) where V : EV where TE : Exception
    {
      try {
        var awaitedPartitions = await partitions;
        var errs = awaitedPartitions.GetErr().SelectMany( e => handler(e).GetErr() );
        var vals = awaitedPartitions.GetVal().ToList();
        var monadVals = awaitedPartitions.err.SelectMany( e => handler(e).GetVal() as ICollection<V> );
        vals.AddRange( monadVals );
        return (vals, errs.ToList());
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,TE>();
      }
    }

    public static async Task<Partitions<V,TE>> Catch<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Result<EV,TE> result ) where V : EV where TE : Exception
    {
      try {
        var awaitedPartitions = (await partitions);
        var resultPartitions = result.ToPartitions();
        var errs = awaitedPartitions.GetErr().SelectMany( v => resultPartitions.GetErr() );
        var vals = awaitedPartitions.GetVal().ToList();
        var monadVals = resultPartitions.GetVal() as ICollection<V>;
        vals.AddRange( monadVals );
        return (vals, errs.ToList());
      }
      catch(Exception ex) {
        return ex.ToPartitions<V,TE>();
      }
    }

    public static Task<Partitions<V,TE>> Catch<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Func<Result<EV,TE>> handler ) where V : EV where TE : Exception => partitions.Catch( handler() );

    public static async Task<Partitions<V,TE>> Catch<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Func<E,Result<EV,TE>> handler ) where V : EV where TE : Exception
    {
      try {
        var awaitedPartitions = await partitions;
        var errs = awaitedPartitions.GetErr().SelectMany( e => handler(e).ToPartitions().GetErr() );
        var vals = awaitedPartitions.GetVal().ToList();
        var monadVals = awaitedPartitions.err.SelectMany( e => handler(e).ToPartitions().GetVal() as ICollection<V> );
        vals.AddRange( monadVals );
        return new Result.Partitions<V,TE>(vals, errs.ToList());
      }
      catch(Exception ex) {
        return ex.ToPartitions<V,TE>();
      }
    }

    public static async Task<Partitions<V,TE>> Catch<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Task<Partitions<EV,TE>> monad ) where V : EV where TE : Exception 
    {
      try {
        var awaitedPartitions = await partitions;
        var errsOfErrs = await Task.WhenAll( awaitedPartitions.GetErr().Select( v => monad.GetErr() ) );
        var errs = errsOfErrs.SelectMany( e => e );
        var vals = awaitedPartitions.GetVal().ToList();
        var monadVals = monad.GetVal() as ICollection<V>;
        vals.AddRange( monadVals );
        return (vals, errs.ToList());
      }
      catch(Exception ex) {
        return ex.ToPartitions<V,TE>();
      }
    }

    public static Task<Partitions<V,TE>> Catch<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Func<Task<Partitions<EV,TE>>> handler ) where V : EV where TE : Exception => partitions.Catch( handler() );

    public static async Task<Partitions<V,TE>> Catch<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Func<E,Task<Partitions<EV,TE>>> handler ) where V : EV where TE : Exception
    {
      try {
        var awaitedPartitions = await partitions;
        var errsOfErrs = await Task.WhenAll( awaitedPartitions.GetErr().Select( e => handler(e).GetErr() ) );
        var errs = errsOfErrs.SelectMany( e => e );
        var vals = awaitedPartitions.GetVal().ToList();
        var monadVals = awaitedPartitions.err.SelectMany( e => handler(e).GetVal() as ICollection<V> );
        vals.AddRange( monadVals );
        return (vals, errs.ToList());
      }
      catch(Exception ex)
      {
        return ex.ToPartitions<V,TE>();
      }
    }

    public static async Task<Partitions<V,TE>> Catch<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Task<Result<EV,TE>> result ) where V : EV where TE : Exception
    {
      try {
        var awaitedPartitions = (await partitions);
        var resultPartitions = (await result).ToPartitions();
        var errs = awaitedPartitions.GetErr().SelectMany( v => resultPartitions.GetErr() );
        var vals = awaitedPartitions.GetVal().ToList();
        var monadVals = resultPartitions.GetVal() as ICollection<V>;
        vals.AddRange( monadVals );
        return (vals, errs.ToList());
      }
      catch(Exception ex) {
        return ex.ToPartitions<V,TE>();
      }
    }

    public static Task<Partitions<V,TE>> Catch<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Func<Task<Result<EV,TE>>> handler ) where V : EV where TE : Exception => partitions.Catch( handler() );

    public static async Task<Partitions<V,TE>> Catch<V,E,EV,TE>(this Task<Partitions<V,E>> partitions, Func<E,Task<Result<EV,TE>>> handler ) where V : EV where TE : Exception
    {
      try {
        var awaitedPartitions = await partitions;
        var errsOfErrs = await Task.WhenAll( awaitedPartitions.GetErr().Select( async e => (await handler(e)).ToPartitions().GetErr() ) );
        var errs = errsOfErrs.SelectMany( e => e );
        var vals = awaitedPartitions.GetVal().ToList();
        var monadValsOfVals = await Task.WhenAll( awaitedPartitions.err.Select( async e => (await handler(e)).ToPartitions().GetVal() as ICollection<V> ));
        var monadVals = monadValsOfVals.SelectMany( v => v );
        vals.AddRange( monadVals );
        return (vals, errs.ToList());
      }
      catch(Exception ex) {
        return ex.ToPartitions<V,TE>();
      }
    }
    #endregion


  }


}