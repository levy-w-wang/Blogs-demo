using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoFrame_CoreMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoFrame_CoreMvc.Filters
{
    /// <summary>
    /// 用户权限验证
    /// </summary>
    public class UserAuthorize : ActionFilterAttribute
    {
        public UserAuthorize()
        {
        }
        public List<long> Popedoms { get; set; }//驼峰命名   首字母大写


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="popedom">权限组</param>
        public UserAuthorize(params long[] popedom)
        {
            Popedoms = popedom.ToList();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //整个类是否设置了都可以访问
            if (filterContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true)
                    .Any(a => a.GetType() == typeof(AllowAnonymousAttribute));
                if (isDefined)
                {
                    return;
                }
            }

            var user = UserCache.GetCurrentUser();//获取当前用户
            if (user != null && Popedoms?.Contains(user.PopedomId) == true)
            {
                return;
            }

            if (user == null)
            {
                filterContext.Result = new CustomHttpStatusCodeResult(200, 401, "授权已失效请重新登录");
            }
            else
            {
                filterContext.Result = new CustomHttpStatusCodeResult(200, 402, "您无权进行该操作，请联系管理员！");
            }

        }
    }
}
