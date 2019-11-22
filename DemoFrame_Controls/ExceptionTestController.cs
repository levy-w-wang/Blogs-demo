using DemoFrame_CoreMvc.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Basic;
using Microsoft.AspNetCore.Mvc;

namespace DemoFrame_Controls
{
    [Route("api/[controller]")]
    public class ExceptionTestController : BaseController
    {
        [HttpGet]
        [Route("dividezero")]
        public ActionResult TestDivideZero()
        {
            LogHelper.Logger.Debug("测试除0异常");
            var zero = 0;
            var test = 10 / zero;
            return Succeed();
        }
    }
}
