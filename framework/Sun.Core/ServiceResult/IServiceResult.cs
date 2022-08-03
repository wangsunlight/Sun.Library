using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sun.Core.ServiceResult
{
    /// <summary>
    /// 服务接口统一响应
    /// </summary>
    public interface IServiceResult
    {
        /// <summary>
        /// 请求id
        /// </summary>
        string requestId { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonIgnore]
        bool successful { get; }
        /// <summary>
        /// 状态码
        /// </summary>
        int code { get; set; }


        /// <summary>
        /// 消息
        /// </summary>
        string msg { get; set; }

        /// <summary>
        /// 服务器时间戳
        /// </summary>

        long timestamp { get; }

        /// <summary>
        /// 服务来源
        /// </summary>
        string source { get; }
    }
    /// <summary>
    /// 服务接口统一响应
    /// </summary>
    public interface IServiceResult<TData> : IServiceResult
    {
        /// <summary>
        /// 返回值
        /// </summary>
        TData data { get; set; }
    }
}
