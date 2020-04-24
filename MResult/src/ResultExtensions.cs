namespace Michaelolof.MResult
{
  public static class ResultExtensions
  {
    public static Result<V, E> ToOk<V, E>(this V val) => Result<V, E>.Ok( val );
    public static Result<V, E> ToErr<V, E>(this E err) => Result<V, E>.Err( err );
  }

}

