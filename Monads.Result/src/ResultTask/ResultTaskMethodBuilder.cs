using System;
using System.Runtime.CompilerServices;

namespace Michaelolof.Monads.Result
{

  class ResultTaskMethodBuilder<T>
  {

    internal T result;
    internal bool GotResult;

    public void SetResult(T result)
    {
      this.result = result;
      GotResult = true;
    }

    public ResultTask<T> Task {get;} 
    public static ResultTaskMethodBuilder<T> Create() => new ResultTaskMethodBuilder<T>();
    public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine => stateMachine.MoveNext();


    public void SetStateMachine(IAsyncStateMachine stateMachine) { }

    public void SetException(Exception exception) { }

    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
    {}

    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
    {
    }

  }

}