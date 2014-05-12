using System;
using System.Threading.Tasks;

namespace JPush.Extensions
{
    public static class TaskExtensions
    {
        public static Task<TDestination> MapTask<TDestination, TSource>(this Task<TSource> source, Func<TSource, TDestination> mapper)
        {
            var tcs = new TaskCompletionSource<TDestination>();
            source
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        tcs.TrySetException(task.Exception ?? new Exception());
                    }
                    else if (task.IsCanceled)
                    {
                        tcs.TrySetCanceled();
                    }
                    else
                    {
                        tcs.TrySetResult(mapper.Invoke(source.Result));
                    }
                });

            return tcs.Task;
        }
    }
}