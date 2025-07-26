using Cysharp.Threading.Tasks;
using System;

// https://github.com/Cysharp/UniTask/issues/329
public static class UniTaskHelper
{
    public static Action<T> Action<T>(Func<T, UniTaskVoid> asyncAction)
    {
        return (t1) => asyncAction(t1).Forget();
    }

}