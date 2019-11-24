using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoFrame_CoreMvc.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DemoFramework_MainWeb.Swagger
{
    /// <summary>
    /// 自定义添加参数
    /// </summary>
    public class SwaggerParameter : IOperationFilter
    {
        /// <summary>
        /// 实现 Apply 方法
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<IParameter>();
            var attrs = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;
            var t = typeof(BaseUserController);
            //先判断是否是继承用户验证类
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor && context.MethodInfo.DeclaringType?.IsSubclassOf(t) == true)
            {
                //再验证是否允许匿名访问
                var actionAttributes = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
                bool isAnonymous = actionAttributes.Any(a => a is AllowAnonymousAttribute);
                // 需要验证的方法添加
                if (!isAnonymous)
                {
                    //query 参数字段值对应放在url中
                    //header 参数值对应放在header param中
                    //body 参数对应放到请求体中
                    //path 参数应该对应放到请求路径 具体貌似没用
                    //formData 参数对应放到请求表单中
                    // 参考 https://github.com/OAI/OpenAPI-Specification/blob/master/versions/2.0.md#parameter-object
                    operation.Parameters.Add(new NonBodyParameter()
                    {
                        Name = "sid",
                        In = "header", //query header body path formData
                        Type = "string",
                        Required = true,//是否必选
                        Description = "登录返回的sid"
                    });
                    operation.Parameters.Add(new NonBodyParameter()
                    {
                        Name = "token",
                        In = "header", //query header body path formData
                        Type = "string",
                        Required = true,//是否必选
                        Description = "登录返回的token"
                    });
                }
            }
        }
    }
}
