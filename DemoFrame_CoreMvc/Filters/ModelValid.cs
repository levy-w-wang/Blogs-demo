using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Basic.Exceptions;
using DemoFrame_CoreMvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoFrame_CoreMvc.Filters
{
    /// <inheritdoc />
    /// <summary>
    /// 模型数据验证
    /// </summary>
    public class ModelValid : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //判断模型是否验证通过
            if (filterContext.ModelState.ErrorCount == 0 && filterContext.ModelState.IsValid)
                return;
            var errMsg = new StringBuilder();
            foreach (var modelStateKey in filterContext.ModelState.Keys)
            {
                var value = filterContext.ModelState[modelStateKey];
                foreach (var error in value.Errors)
                {
                    if (!string.IsNullOrEmpty(error.ErrorMessage))
                    {
                        errMsg.Append(error.ErrorMessage + ",");
                    }
                }
            }

            if (errMsg.Length > 0)
            {
                errMsg.Remove(errMsg.Length - 1, 1);
            }
            if (filterContext.Controller is BaseController controller)
                filterContext.Result = controller.Fail(1005, $"请求数据验证失败,{errMsg}");//1005为自定义的错误Code
            else
                throw new CustomSystemException("默认Controller都要继承BaseController，以实现全局模型的验证、错误提醒",999);//999为自定义的错误Code
        }
    }
}
