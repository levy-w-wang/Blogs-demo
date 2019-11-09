namespace DemoFrame_CoreMvc.Models
{
    /// <inheritdoc />
    /// <summary>
    /// 响应返回体
    /// </summary>
    public class ResponseResult : ResponseResult<object>
    {
    }
    /// <summary>
    /// 响应返回体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseResult<T> : BaseResponseResult
    {
        public T Data { get; set; }

        public ResponseResult<T> Fail(int code, string msg, T data)
        {
            Code = code;
            Message = msg;
            Data = data;
            return this;
        }

        public ResponseResult<T> Succeed(T data, int code = 200, string msg = "successful")
        {
            Code = code;
            Message = msg;
            Data = data;
            return this;
        }
    }
    public class BaseResponseResult
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public bool Success => Code == 200;//自定义成功状态码为200
    }
}
