using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DemoFrame_Basic;
using DemoFrame_CoreMvc.Controllers;
using DemoFrame_Models.CusEntitys;
using Microsoft.AspNetCore.Mvc;

namespace DemoFrame_Controls
{
    public class TestController : BaseController
    {
        [HttpGet]
        [Route("test")]
        public ActionResult Test()
        {
            return Succeed("测试");
        }

        [AcceptVerbs("Get", "Post")]//允许get 和post方法
        [Route("validphone")]
        public ActionResult TestValid([Required(ErrorMessage = "请输入电话号码")]
            [RegularExpression(@"^1[3|4|5|6|7|8|9][0-9]{9}$", ErrorMessage = "不是一个有效的手机号")] string phone)
        {
            //phone = "123";
            //TryValidateModel(phone);//单个字段该方法无效  应使用类

            if (!ModelState.IsValid)
            {
                var errMsg = new StringBuilder();
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var value = ModelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        errMsg.Append(error.ErrorMessage + ",");
                    }
                }

                errMsg.Remove(errMsg.Length - 1, 1);
                return Fail(1, errMsg.ToString());
            }
            return Succeed("测试");
        }

        [HttpPost]
        [Route("testvalidmodel")]
        public ActionResult ValidModel([FromBody]TestValidModel validModel)
        {
            //            if (!ModelState.IsValid)
            //            {
            //                return Fail(1005, "模型验证失败");
            //            }
            validModel.Age = 18;
            TryValidateModel(validModel);
            var b = ModelState.IsValid;
            return Succeed("测试");
        }
    }
}
