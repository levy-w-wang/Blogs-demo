using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using DemoFrame_Basic.Dependency;

namespace DemoFrame_Models.Entitys
{
    [Table(name: "Test_Model")]
    public class TestModel : IEntityBase
    {
        [Key]
        public long Id { get; set; }

        public string BlogName { get; set; }

        [DefaultValue(0)]
        public int BlogUseDay { get; set; }

        [DefaultValue(0)]
        public int BlogPhone { get; set; }
    }
}
