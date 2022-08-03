using AspectCore.DynamicProxy;
using Sun.Core.DataEncryption.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun.DynamicProxy.Attributes
{
    /// <summary>
    /// 使用缓存拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UseCacheInterceptorAttribute : AbstractInterceptorAttribute
    {
        public UseCacheInterceptorAttribute(
               string key,
               string paramsAppend = "",
               int cacheTime = 20,
               bool autoSet = true,
               bool useMemoryCache = false,
               bool md5Key = false)
        {
            this.Key = key;
            this.CacheTime = cacheTime;
            this.AutoSet = autoSet;
            this.ParamsAppend = paramsAppend;
            this.UseMemoryCache = useMemoryCache;
            this.Md5Key = md5Key;
        }
        /// <summary>
        /// 当缓存需要用到方法的参数作为动态值时
        /// 填写方法的参数名称,大小写要保持一致,多个用逗号隔开
        /// 如果是一个实体参数,需要用到实体的属性,则使用  实体参数名称:实体属性名称
        /// </summary>
        public string ParamsAppend { get; set; }
        /// <summary>
        /// 缓存的key名称
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 缓存失效期 单位是分钟, 0表示永不过期
        /// </summary>
        public int CacheTime { get; set; }
        /// <summary>
        /// 是否自动写入缓存值
        /// </summary>
        public bool AutoSet { get; set; }
        /// <summary>
        /// 是否使用memorycache
        /// </summary>
        public bool UseMemoryCache { get; set; }
        /// <summary>
        /// 是否对长key进行md5加密  目的：为了取短key
        /// </summary>
        public bool Md5Key { get; set; }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                Console.WriteLine("Before service call");

                if (!string.IsNullOrEmpty(ParamsAppend))
                {
                    var keys = context.Parameters.ToList();
                    if (keys.Count > 0)
                    {
                        ParamsAppend.Split(',').ToList().ForEach(item =>
                        {
                            if (keys.Contains(item))
                            {
                                var getVal = context.Parameters.FirstOrDefault(p => p.ToString() == item);
                                if (getVal != null)
                                {
                                    //if (getVal.GetType().IsDefined(typeof(CacheKeyToIntAttribute), true))
                                    //{
                                    //    Key += $"{Convert.ToInt32(getVal)}_";
                                    //}
                                    //else
                                    //{
                                    Key += $"{getVal.ToString()}_";
                                    //}
                                }
                            }
                        });
                    }
                }

                if (Md5Key)
                {
                    Key = $"MD5Key:{Key.ToMD5Encrypt()}";
                }

                await next(context);
            }
            catch (Exception)
            {
                Console.WriteLine("Service threw an exception!");
                throw;
            }
            finally
            {
                Console.WriteLine("After service call");
            }
        }
    }
}
