using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sun.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun.Core.ServiceResult
{
    /// <summary>
    /// 服务接口统一响应
    /// </summary>
    /// <typeparam name="TData"></typeparam>

    public class ServiceResult<TData> : IServiceResult<TData>
    {
        /// <summary>
        /// 请求id
        /// </summary>

        public string requestId { get; set; } = ApiContext.RequestId;

        /// <summary>
        /// 处理是否成功
        /// </summary>
        [JsonIgnore]
        public bool successful
        {
            get
            {
                return code == StatusCodes.Status200OK;
            }
        }
        /// <summary>
        /// 状态码
        /// </summary>

        public int code { get; set; } = StatusCodes.Status200OK;

        /// <summary>
        /// 数据
        /// </summary>
        public TData data { get; set; }

        /// <summary>
        /// 消息
        /// </summary>

        public string msg { get; set; } = "";

        /// <summary>
        /// 服务器时间戳
        /// </summary>

        public long timestamp => DateTimeOffset.Now.ToUnixTimeMilliseconds();

        /// <summary>
        /// 服务来源
        /// </summary>
        public string source => Environment.MachineName;


    }


    /// <summary>
    /// 服务接口统一响应
    /// </summary>
    public class ServiceResult : ServiceResult<object>
    {

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="output">响应数据</param>
        /// <returns></returns>
        public static ServiceResult<TData> Success<TData>(TData output)
        {
            return new ServiceResult<TData> { data = output };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="code">错误状态码</param>
        /// <param name="msg">错误消息</param>
        /// <returns></returns>
        public static ServiceResult<TData> Faild<TData>(int code, string msg)
        {
            return new ServiceResult<TData> { code = code, msg = msg };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="code">错误状态码</param>
        /// <param name="msg">错误消息</param>
        /// <returns></returns>
        public static ServiceResult<TData> Faild<TData>(Enum code)
        {
            return new ServiceResult<TData> { code = code.Value(), msg = code.Description() };
        }
        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="code">错误状态码</param>
        /// <param name="msg">错误消息</param>
        /// <returns></returns>
        public static ServiceResult<TData> Faild<TData>(Enum code, string msg)
        {
            return new ServiceResult<TData> { code = code.Value(), msg = msg };
        }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <returns></returns>
        public static ServiceResult Success()
        {
            return new ServiceResult();
        }
        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="code">错误状态码</param>
        /// <param name="msg">错误消息</param>
        /// <returns></returns>
        public static ServiceResult Faild(int code, string msg)
        {
            return new ServiceResult { code = code, msg = msg };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="code">错误状态码</param>
        /// <param name="msg">错误消息</param>
        /// <returns></returns>
        public static ServiceResult Faild(Enum code)
        {
            return new ServiceResult { code = code.Value(), msg = code.Description() };
        }
        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="code">错误状态码</param>
        /// <param name="msg">错误消息</param>
        /// <returns></returns>
        public static ServiceResult Faild(Enum code, string msg)
        {
            return new ServiceResult { code = code.Value(), msg = msg };
        }
    }
}
