using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Basic;
using DemoFrame_CoreMvc.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoFrame_Controls
{
    public class TokenTestController : BaseUserController
    {
        [HttpGet]
        [Route("testtoken")]
        [AllowAnonymous]//允许所有人访问
        public ActionResult TestToken()
        {
            var token = TokenManager.GenerateToken("测试token的生成");
            Response.Headers["token"] = token;
            Response.Headers["Access-Control-Expose-Headers"] = "token";//一定要添加这一句  不然前端是取不到token字段的值的！更别提存store了。
            return Succeed(token);
        }

        //[HttpPost]
        //[Route("validtoken")]
        //public ActionResult ValidToken([FromHeader]string token)
        //{
        //    var str = TokenManager.ValidateToken(token, out DateTime date);
        //    if (!string.IsNullOrEmpty(str) || date > DateTime.Now)
        //    {
        //        //当token过期时间小于五小时，更新token并重新返回新的token
        //        if (date.AddHours(-5) > DateTime.Now) return Succeed($"Token字符串:{str},过期时间：{date}");
        //        var nToken = TokenManager.GenerateToken(str);
        //        Response.Headers["token"] = nToken;
        //        token = nToken;
        //        Response.Headers["Access-Control-Expose-Headers"] = "token";
        //    }
        //    else
        //    {
        //        return Fail(101, "未取得授权信息");
        //    }
        //    return Succeed($"Token字符串:{str},过期时间：{DateTime.Now.AddHours(3 * 24)}");
        //}
        [HttpPost]
        [Route("validtoken")]
        public ActionResult ValidToken()
        {
            //业务处理  token已在基类中验证
            return Succeed("成功");
        }
    }
}
