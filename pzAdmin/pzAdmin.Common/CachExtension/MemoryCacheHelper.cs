using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Common.CachExtension
{
      /// <summary>
      /// 用ASP.NET的IMemoryCache实现的内存缓存
      /// </summary>
      public class MemoryCacheHelper : IMemoryCacheHelper
      {
            private readonly IMemoryCache memoryCache;
            public MemoryCacheHelper(IMemoryCache memoryCache)
            {
                  this.memoryCache = memoryCache;
            }


            //验证TResult是否是允许的类型
            private static void ValidateValueType<TResult>()
            {
                  //因为IEnumerable、IQueryable等有延迟执行的问题，造成麻烦，因此禁止用这些类型
                  Type typeResult = typeof(TResult);
                  if (typeResult.IsGenericType)//如果是IEnumerable<String>这样的泛型类型，则把String这样的具体类型信息去掉，再比较
                  {
                        typeResult = typeResult.GetGenericTypeDefinition();
                  }
                  //注意用相等比较，不要用IsAssignableTo
                  if (typeResult == typeof(IEnumerable<>) || typeResult == typeof(IEnumerable)
                      || typeResult == typeof(IAsyncEnumerable<TResult>)
                      || typeResult == typeof(IQueryable<TResult>) || typeResult == typeof(IQueryable))
                  {
                        throw new InvalidOperationException($"TResult of {typeResult} is not allowed, please use List<T> or T[] instead.");
                  }
            }

            //随机生成一个时间间隔，防止缓存雪崩
            private static void InitCacheEntry(ICacheEntry entry, int baseExpireSeconds)
            {
                  double d1 = (double)baseExpireSeconds;
                  double d2 = d1 * 2;
                  double sec = d1 + Random.Shared.NextDouble() * (d2 - d1);
                  TimeSpan expiration = TimeSpan.FromSeconds(sec);
                  entry.AbsoluteExpirationRelativeToNow = expiration;
            }

            public TResult? GetOrCreate<TResult>(string cacheKey, Func<ICacheEntry, TResult?> valueFactory, int baseExpireSeconds = 60)
            {
                  ValidateValueType<TResult>();
                  //因为IMemoryCache保存的是一个CacheEntry，所以null值也认为是合法的，因此返回null不会有“缓存穿透”的问题
                  //不调用系统内置的CacheExtensions.GetOrCreate，而是直接用GetOrCreate的代码，这样免得包装一次委托
                  if (!memoryCache.TryGetValue(cacheKey, out TResult result))
                  {
                        using ICacheEntry entry = memoryCache.CreateEntry(cacheKey);
                        InitCacheEntry(entry, baseExpireSeconds);
                        result = valueFactory(entry)!;
                        entry.Value = result;
                  }
                  return result;
            }

            public async Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey, Func<ICacheEntry, Task<TResult?>> valueFactory, int baseExpireSeconds = 60)
            {
                  ValidateValueType<TResult>();
                  if (!memoryCache.TryGetValue(cacheKey, out TResult result))
                  {
                        using ICacheEntry entry = memoryCache.CreateEntry(cacheKey);
                        InitCacheEntry(entry, baseExpireSeconds);
                        result = (await valueFactory(entry))!;
                        entry.Value = result;
                  }
                  return result;
            }

            public void Remove(string cacheKey)
            {
                  memoryCache.Remove(cacheKey);
            }
            /// <summary>
            /// 尝试获取缓存值
            /// </summary>
            /// <typeparam name="TResult"></typeparam>
            /// <param name="cacheKey"></param>
            /// <param name="result"></param>
            /// <returns></returns>
            public bool TryGetValue<TResult>(string cacheKey, out TResult? result)
            {
                  ValidateValueType<TResult>();
                  return memoryCache.TryGetValue(cacheKey, out result);
            }
            /// <summary>
            /// 设置缓存值
            /// </summary>
            /// <typeparam name="TResult"></typeparam>      
            public void Set<TResult>(string cacheKey, TResult value, int expireSeconds = 60)
            {
                  ValidateValueType<TResult>();
                  using ICacheEntry entry = memoryCache.CreateEntry(cacheKey);
                  InitCacheEntry(entry, expireSeconds);
                  entry.Value = value;
            }
      }
}
