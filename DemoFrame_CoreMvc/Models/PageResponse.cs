using System;
using System.Collections.Generic;
using System.Text;

namespace DemoFrame_CoreMvc.Models
{
    /// <summary>
    /// 分页模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResponse<T>
    {
        private long _recordTotal;

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageTotal { get; set; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long RecordTotal
        {
            get => _recordTotal;
            set
            {
                _recordTotal = value;
                if (PageSize <= 0) return;
                PageTotal = (int)Math.Ceiling(RecordTotal / (double)PageSize);
            }
        }

        public List<T> Data { get; set; }

        public PageResponse()
        {
            Data = new List<T>();
        }

        public PageResponse(List<T> data, int pageIndex, int pageTotal)
        {
            Data = data;
            PageIndex = pageIndex;
            PageTotal = pageTotal;
        }
    }
}
