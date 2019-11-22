using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DemoFrame_Models.CusEntitys
{
    public class TestValidModel
    {
        [Required(ErrorMessage = "请输入名字"),
         RegularExpression("^(?!_)(?!.*?_$)[a-zA-Z0-9_]{4,12}$", ErrorMessage = "登录名不符合规则，请输入4-12位不包含特殊字符的数据")]
        public string Name { get; set; }

        [Range(22,35,ErrorMessage = "年龄段应在22-35之间")]
        public int Age { get; set; }

        [ClassicTestEqual("test")]
        public string CusValid { get; set; }

        [StringLength(3,ErrorMessage = "字符串长度请控制在3个以内")]
        public string Role { get; set; }

        [Required(ErrorMessage = "请输入邮件")]
        [EmailAddress(ErrorMessage = "邮件格式不正确")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请输入电话"),
         RegularExpression("^1[3|4|5|6|7|8|9][0-9]{9}$", ErrorMessage = "电话号码格式不正确")]
        public long? Phone { get; set; }
//        public Nullable<long> Phone { get; set; }
    }
    /// <summary>
    /// 验证值是否等于内置值
    /// </summary>
    public class ClassicTestEqualAttribute : ValidationAttribute
    {
        private string _bulitIn;
        private string _cusValid;

        public ClassicTestEqualAttribute(string bulitIn)
        {
            _bulitIn = bulitIn;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var movie = (TestValidModel)validationContext.ObjectInstance;
            _cusValid = movie.CusValid;
            var cusValid = (string)value;//两种方式获取该字段值 - 也可以获取其它字段值

            if (_bulitIn != cusValid)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        public string CusValid => _cusValid;

        public string GetErrorMessage()
        {
            return $"测试模型中的{_bulitIn}不等于内置值";
        }
    }
}
