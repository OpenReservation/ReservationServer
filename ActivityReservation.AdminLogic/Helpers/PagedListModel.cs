using System.Collections.Generic;

namespace ActivityReservation.Helpers
{
    /// <summary>
    /// 数据分页模型
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class PagedListModel<T> where T : class, new()
    {
        public List<T> Data { get; set; }

        public PagerModel Pager { get; set; }
    }
}