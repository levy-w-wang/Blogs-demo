using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DemoFrame_Basic.Extensions
{
    /// <summary>
    /// 静态帮助类
    /// </summary>
    public static class ObjStrExtension
    {
        /// <summary>
        /// 返回Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象  例:T.ToJson()</param>
        /// <param name="isNullValueHandling">是否忽略Null字段，最终字符串中是否包含Null字段</param>
        /// <param name="indented">是否展示为具有Json格式的字符串</param>
        /// <param name="isLowCase">是否忽略大小写</param>
        /// <param name="dateTimeFormat">时间转换格式</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(this object obj, bool isNullValueHandling = false, bool indented = false, bool isLowCase = false, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            var options = new JsonSerializerSettings();

            if (indented)
                options.Formatting = Formatting.Indented;
            if (isLowCase)
            {
                options.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            if (isNullValueHandling)
                options.NullValueHandling = NullValueHandling.Ignore;
            options.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.Converters = new List<JsonConverter> { new IsoDateTimeConverter { DateTimeFormat = dateTimeFormat } };
            return obj.ToJson(options);
        }
        /// <summary>
        /// Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象  例:T.ToJson(settings)</param>
        /// <param name="settings">Json序列化设置</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(this object obj, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }
        /// <summary>
        /// 返回相关设定格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format">格式 例：yyyy-MM-dd HH:mm:ss</param>
        /// <returns>设定格式字符串</returns>
        public static string ToDateTimeString(this object obj, string format)
        {
            DateTime.TryParse(obj?.ToString(), out var dateTime);
            return dateTime.ToString(format);
        }
        /// <summary>
        /// 得到字符串的Byte
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Byte</returns>
        public static byte[] GetBytes(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return default(byte[]);
            return Encoding.UTF8.GetBytes(str);
        }

        public static bool ToBool(this string str, bool defaultValue = false)
        {
            bool.TryParse(str, out defaultValue);
            return defaultValue;
        }

        public static short ToShort(this string str, short defaultValue = 0)
        {
            short.TryParse(str, out defaultValue);
            return defaultValue;
        }

        public static int ToInt(this string str, int defaultValue = 0)
        {
            int.TryParse(str, out defaultValue);
            return defaultValue;
        }

        public static long ToLong(this string str, long defaultValue = 0)
        {
            long.TryParse(str, out defaultValue);
            return defaultValue;
        }

        public static double ToDouble(this string str, double defaultValue = 0)
        {
            double.TryParse(str, out defaultValue);
            return defaultValue;
        }

        public static TEnum ToEnum<TEnum>(this string str, bool ignoreCase = true, TEnum defaultValue = default(TEnum)) where TEnum : struct
        {
            Enum.TryParse(str, ignoreCase, out defaultValue);
            return defaultValue;
        }

        public static T ToNetType<T>(this string str, bool isIgnoreNull = true, bool isIgnoreEx = false)
        {
            var setting = new JsonSerializerSettings
            {
                NullValueHandling = isIgnoreNull ? NullValueHandling.Ignore : NullValueHandling.Include
            };
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return default(T);
                }
                else if ("\"\"" == str)
                {
                    return default(T);
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(str, setting);
                }
            }
            catch (Exception)
            {
                if (!isIgnoreEx)
                    throw;
                return default(T);
            }
        }

        public static T ToNetType<T>(this string str, JsonSerializerSettings settings)
        {
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return default(T);
                }
                else if ("\"\"" == str)
                {
                    return default(T);
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(str, settings);
                }
            }
            catch (Exception)
            {

                return default(T);
            }
        }

        /// <summary>
        /// 比较是否相等，忽略大小写
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string a, string b)
        {
            if (a != null)
                return a.Equals(b, StringComparison.CurrentCultureIgnoreCase);
            return b == null;
        }
    }
}
