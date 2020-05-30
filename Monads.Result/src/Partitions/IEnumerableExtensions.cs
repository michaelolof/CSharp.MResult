using System.Linq;
using System.Collections.Generic;


namespace Michaelolof.Monads.Result
{

  public static class IEnumerableExtensions
  {

    public static void AddRange<T>(this IEnumerable<T> list, IEnumerable<T> range)
    {
      foreach(var item in range) list.Append( item );
    }

    public static bool ContainsAValue<T>(this IEnumerable<T> list) 
    {
      var index = 0;
      foreach(var item in list) {
        index = index + 1;
        break;
      }
      return index > 0;
    }

    public static bool IsEmpty<T>(this IEnumerable<T> list) => list.ContainsAValue() == false;


  }


}