using System.Text;
using DemoFrame_Basic.Extensions;
using DemoFrame_CoreMvc.Filters;
using DemoFrame_CoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoFrame_CoreMvc.Controllers
{
    /// <summary>
    /// 基础控制器类，所有控制器都继承该类
    /// </summary>
    [ModelValid]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// 从 Request.Body 中获取数据并JSON序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetJsonParams<T>()
        {
            if (Request.ContentLength != null)
            {
                var bytes = new byte[(int)Request.ContentLength];
                Request.Body.Read(bytes, 0, bytes.Length);
                var json = Encoding.UTF8.GetString(bytes);
                return json.ToNetType<T>();
            }

            return default(T);
        }

        /// <summary>
        /// 返回Json数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected ActionResult MyJson(BaseResponseResult data)
        {
            return new CustomJsonResult
            {
                Data = data,
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };
        }

        /// <summary>
        /// 返回成功
        /// Json格式
        /// </summary>
        /// <returns></returns>
        protected ActionResult Succeed()
        {
            return Succeed(true);
        }

        /// <summary>
        /// 返回成功
        /// Json格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected ActionResult Succeed(object data)
        {
            return MyJson(new ResponseResult().Succeed(data));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult Fail(int code, string content = "", string desc = null)
        {
            return MyJson(new ResponseResult().Fail(code, content + " " + desc, "")
           );
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
