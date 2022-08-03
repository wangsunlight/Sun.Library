using Microsoft.Extensions.Primitives;
using Sun.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun.Core.ServiceResult
{
    /// <summary>
    /// api请求上下文
    /// </summary>
    public static class ApiContext
    {
        /// <summary>
        /// requestId
        /// </summary>
        public static string RequestId
        {
            get
            {
                WebHelper.HttpContextAccessor?.HttpContext?.Request?.Headers.TryGetValue("requestId", out StringValues requestId);
                return !string.IsNullOrWhiteSpace(requestId) ? (string)requestId : default;
            }
        }
    }
}
