using System.Collections.Generic;

namespace Michaelolof.Monads.Result
{
  public struct BothList<V,E>
  {
    internal ICollection<V> val;
    internal ICollection<E> err;

    public BothList(ICollection<V> val, ICollection<E> err) {
      this.val = val;
      this.err = err;
    }

    public BothList((ICollection<V>, ICollection<E>) valAndErr) {
      var (var, err) = valAndErr;
      this.val = var;
      this.err = err;
    }

    public static implicit operator BothList<V,E>((ICollection<V>,ICollection<E>) valAndErr) => new BothList<V,E>(valAndErr);

  }

}