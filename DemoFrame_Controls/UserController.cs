using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Basic;
using DemoFrame_Basic.Extensions;
using DemoFrame_CoreMvc;
using DemoFrame_CoreMvc.Controllers;
using DemoFrame_CoreMvc.Filters;
using DemoFrame_Models.CusEntitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoFrame_Controls
{
    [Route("api/[controller]")]
    public class UserController : BaseUserController
    {
        [HttpPost, Route("login"),AllowAnonymous]//AllowAnonymous加上这一句 为任何用户可访问，在基础控制器中有设置
        public ActionResult Login()
        {
            //调用登录方法进行登录 在业务层处理  将登录成功的用户信息缓存 以及生成token
            //此处示例 为了方便就直接在控制器中编写
            var token = TokenManager.GenerateToken("测试token的生成");
            Response.Headers["token"] = token;
            //Response.Headers["Access-Control-Expose-Headers"] = "token"; //需要同时设置 不然后者会冲突掉前者
            var user = new UserDto()
            {
                UserName = "Levy",
                Email = "levy_w_wang@qq.com",
                Age = 23,
                PopedomId = 666
            };
            var sessionId = Guid.NewGuid().ToString("N");
            DemoWeb.HttpContext.Response.Headers["sid"] = sessionId;
            UserCache.Set(sessionId,user.ToJson());
            DemoWeb.HttpContext.Response.Headers["Access-Control-Expose-Headers"] = "token,sid";//前后端分离 跨域的情况下  加上这句 前端才能拿到 sid 字段对应值 多个用英文逗号分隔
            return Succeed(user);
        }

        /// <summary>
        /// 用户权限为888 或者为 999 的才能访问
        /// </summary>
        /// <returns></returns>
        [HttpPost,Route("popedom"),UserAuthorize(888,999)]
        public ActionResult PopedomTest()
        {
            return Succeed("成功访问");
        }
    }
}
