using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoFrame_Basic.Dependency;

namespace DemoFrame_Models.Entitys
{
    [Table(name: "Demo_Model")]
    public class DemoModel : EntityBase
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]//不自动增长   使用IdWorker生成
        public long Id { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 证件类型：1 身份证，2 军官证，3 护照
        /// </summary>
        [DefaultValue(1)]
        public int IdentityCardType { get; set; }
    }
}
