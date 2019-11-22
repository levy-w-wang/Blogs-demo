using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DemoFrame_Biz;
using DemoFrame_CoreMvc.Controllers;
using DemoFrame_Models.CusEntitys;
using DemoFrame_Models.Entitys;
using Microsoft.AspNetCore.Mvc;

namespace DemoFrame_Controls
{
    [Route("api/[controller]")]
    public class DemoModelController : BaseController
    {
        [Route("testadd"), HttpPost]
        public async Task<ActionResult> AddDemoModel()
        {
            var models = new List<DemoModel>();
            for (int i = 0; i < 100; i++)
            {
                var testModel = new DemoModel()
                {
                    CustomerName = i +"-Levy" + DateTime.Now.ToString("HH:mm:ss"),
                    IdentityCardType = 1
                };
                models.Add(testModel);
            }
            for (int i = 0; i < 100; i++)
            {
                var testModel = new DemoModel()
                {
                    CustomerName = i + "-zzzz" + DateTime.Now.ToString("HH:mm:ss"),
                    IdentityCardType = 2
                };
                models.Add(testModel);
            }

            var res = await Task.FromResult(DemoModelBiz.Instance.AddDemoModel(models));
            return Succeed(res);
        }

        [Route("demolist"), HttpPost]
        public async Task<ActionResult> DemoModelList([FromBody] QueryDemoDto queryDemo)
        {
            var res = await Task.FromResult(DemoModelBiz.Instance.DemoModelList(queryDemo));
            return Succeed(res);
        }
    }
}
