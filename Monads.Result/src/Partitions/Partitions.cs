using System.Collections.Generic;

namespace Michaelolof.Monads.Result
{
  public struct Partitions<V,E>
  {
    readonly internal IList<V> val;
    readonly internal IList<E> err;

    public readonly int Count => val.Count + err.Count;

    public Partitions(IList<V> val, IList<E> err) {
      this.val = val;
      this.err = err;
    }

    public Partitions((IList<V>, IList<E>) valAndErr) {
      var (var, err) = valAndErr;
      this.val = var;
      this.err = err;
    }

    public static implicit operator Partitions<V,E>((IList<V>,IList<E>) valAndErr) => new Partitions<V,E>(valAndErr);

    public static implicit operator Partitions<V,E>(Result<V,E> result) => result.ToPartitions<V,E>();
    public static implicit operator Partitions<V,E>(Result<IList<V>,E> result) => result.ToPartitions<V,E>();
    public static implicit operator Partitions<V,E>(Result<IEnumerable<V>,E> result) => result.ToPartitions<V,E>();
    public static implicit operator Partitions<V,E>(Result<V[],E> result) => result.ToPartitions<V,E>();
    public static implicit operator Partitions<V,E>(List<Result<V,E>> result) => result.ToPartitions<V,E>();

  }

}

