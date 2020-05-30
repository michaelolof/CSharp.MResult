using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Michaelolof.Monads.Result
{
  public struct Partitions<V,E>
  {
    // readonly internal IEnumerable<Result<V,E>> results;
    readonly internal IEnumerable<V> val;
    readonly internal IEnumerable<E> err;

    public Partitions(IEnumerable<Result<V,E>> results) {
      this.val = results.Where(r => r.IsOk).Select(r => r.GetValueAndErr().Item1);
      this.err = results.Where(r => r.IsErr).Select(r => r.GetValueAndErr().Item2);
    }

    public Partitions(IEnumerable<V> val, IEnumerable<E> err) {
      this.val = val;
      this.err = err;
    }

    public Partitions((IEnumerable<V>, IEnumerable<E>) valAndErr) {
      var (var, err) = valAndErr;
      this.val = var;
      this.err = err;
    }

    public static implicit operator Partitions<V,E>((IEnumerable<V>,IEnumerable<E>) valAndErr) => new Partitions<V,E>(valAndErr);
    public static implicit operator Partitions<V,E>((IList<V>,IList<E>) valAndErr) => new Partitions<V,E>(valAndErr);
    public static implicit operator Partitions<V,E>(Result<V,E> result) => result.ToPartitions<V,E>();
    public static implicit operator Partitions<V,E>(Result<IList<V>,E> result) => result.ToPartitions<V,E>();
    public static implicit operator Partitions<V,E>(Result<IEnumerable<V>,E> result) => result.ToPartitions<V,E>();
    public static implicit operator Partitions<V,E>(Result<V[],E> result) => result.ToPartitions<V,E>();
    public static implicit operator Partitions<V,E>(List<Result<V,E>> result) => result.ToPartitions<V,E>();

  }

}

