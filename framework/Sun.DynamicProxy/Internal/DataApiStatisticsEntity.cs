using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun.DynamicProxy.Internal
{
    public class DataApiStatisticsEntity
    {
        /// <summary>
        /// 方法名
        /// </summary>
        public string Function { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// http类型,get  post
        /// </summary>
        public string HttpType { get; set; }
        /// <summary>
        /// http状态码
        /// </summary>
        public int HttpState { get; set; }
        /// <summary>
        /// 时间, 单位ms
        /// </summary>
        public long Time { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; }
        /// <summary>
        /// 消息,可以放错误消息或者方便日志查看的记录消息
        /// </summary>
        public string Message { get; set; }
    }
}
