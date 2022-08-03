
namespace Sun.Core.Helper
{
    /// <summary>
    /// 动态生产有规律的ID
    /// </summary>
    public static class SnowflakeHelper
    {
        ///// <summary>
        ///// 获取雪花分布式id
        ///// </summary>
        ///// <returns></returns>
        //public static long GetId()
        //{
        //    IDistributedSnowflakeId builder = ServiceLocator.Current.Create<IDistributedSnowflakeId>();
        //    if (builder == null)
        //    {
        //        throw new SnowflakeBuilderException("IDistributedSnowflakeId未注册");
        //    }
        //    return builder.GetId();
        //}

        ///// <summary>
        ///// 批量获取雪花分布式id
        ///// </summary>
        ///// <returns></returns>
        //public static long[] GetId(int count)
        //{
        //    IDistributedSnowflakeId builder = ServiceLocator.Current.Create<IDistributedSnowflakeId>();
        //    if (builder == null)
        //    {
        //        throw new SnowflakeBuilderException("IDistributedSnowflakeId未注册");
        //    }
        //    return builder.GetId(count);
        //}


        ///// <summary>
        ///// 本地雪花算法生成id
        ///// </summary>
        ///// <returns></returns>
        //public static long GetLocalId()
        //{
        //    return ServiceLocator.Current.Create<IdWorker>().NextId();
        //}

        ///// <summary>
        ///// 本地雪花漂移算法15位
        ///// </summary>
        ///// <returns></returns>
        //public static long GetLocalDriftId()
        //{
        //    return YitIdHelper.NextId();
        //}
    }
}