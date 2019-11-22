using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Basic.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DemoFrame_CoreMvc.Models
{
    /// <summary>
    /// 返回带有HTTP状态码的json结果
    /// </summary>
    public class CustomHttpStatusCodeResult : ActionResult
    {

        public int StatusCode { get; }

        public string Data { get; }

        public CustomHttpStatusCodeResult(int httpStatusCode, int msgCode, string content = "", object data = null)
        {
            StatusCode = httpStatusCode;
            Data = new ResponseResult().Fail(msgCode, content ?? "", data ?? "").ToJson(true, isLowCase: true);
        }

        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            context.HttpContext.Response.StatusCode = StatusCode;
            if (string.IsNullOrEmpty(Data))
                return;
            context.HttpContext.Response.ContentType = "application/json";
            var bytes = Encoding.UTF8.GetBytes(Data);

            context.HttpContext.Response.Body.Write(bytes, 0, bytes.Length);
        }
    }
}
