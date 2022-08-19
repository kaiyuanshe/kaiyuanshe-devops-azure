using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kaiyuanshe.DevOps.Extensions
{
    internal static class AsyncEnumeratorExtension
    {
        public static async Task ForEach<T>(this IAsyncEnumerable<T> source, Action<T> action, int? limit = null, CancellationToken cancellationToken = default)
        {
            IAsyncEnumerator<T> enumerator = source.GetAsyncEnumerator();
            int count = 0;
            try
            {
                while (await enumerator.MoveNextAsync() && !cancellationToken.IsCancellationRequested)
                {
                    action(enumerator.Current);
                    if (limit.HasValue)
                    {
                        if (Interlocked.Increment(ref count) >= limit.Value)
                        {
                            break;
                        }
                    }
                }
            }
            finally
            {
                await enumerator.DisposeAsync();
            }
        }

        public static async Task ForEach<T>(this IAsyncEnumerable<T> source, Func<T, Task> func, int? limit = null, CancellationToken cancellationToken = default)
        {
            IAsyncEnumerator<T> enumerator = source.GetAsyncEnumerator();
            int count = 0;
            try
            {
                while (await enumerator.MoveNextAsync() && !cancellationToken.IsCancellationRequested)
                {
                    await func(enumerator.Current);
                    if (limit.HasValue)
                    {
                        if (Interlocked.Increment(ref count) >= limit.Value)
                        {
                            break;
                        }
                    }
                }
            }
            finally
            {
                await enumerator.DisposeAsync();
            }
        }
    }
}
