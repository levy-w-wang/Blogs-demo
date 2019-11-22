using System.Collections.Generic;
using DemoFrame_Basic;
using DemoFrame_CoreMvc.Controllers;
using DemoFrame_CoreMvc.Models;
using DemoFrame_Dao;
using DemoFrame_Models.Entitys;
using Microsoft.AspNetCore.Mvc;

namespace DemoFramework_MainWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseController
    {
        private readonly DemoDbContext _context;

        public ValuesController(DemoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("log")]
        public ActionResult TestLog()
        {
            LogHelper.Logger.Trace("测试日志");
            LogHelper.Logger.Debug("测试日志");
            LogHelper.Logger.Info("测试日志");
            LogHelper.Logger.Warn("测试日志");
            LogHelper.Logger.Error("测试日志");
            LogHelper.Logger.Fatal("测试日志");
            return Succeed();
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            
            
            _context.Set<DemoModel>().Add(new DemoModel()
            {
                //Id = 1,
                CustomerName = "levy",
                IdentityCardType = 1,
            });
            var res =  _context.SaveChanges();
            
            return Succeed(new string[] { "value1", res +"" });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<DemoModel> Get(long id)
        {
            
            return _context.Set<DemoModel>().Find(id); ;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
