using System;

namespace DemoFrame_Basic.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// 自定义系统错误异常
    /// </summary>
    public class CustomSystemException : Exception
    {
        /// <summary>
        /// HTTP状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public object[] Args { get; set; }

        public CustomSystemException(string message, int code, params object[] args) : base(message)
        {
            Code = code;
            Args = args;
        }
    }
}
