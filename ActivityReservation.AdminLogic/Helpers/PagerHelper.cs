using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ActivityReservation.Helpers
{
    /// <summary>
    /// PagerHelper 分页帮助类
    /// </summary>
    public static class PagerHelper
    {
        /// <summary>
        /// Pager V1.0
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, PagerModel pager)
        {
            StringBuilder sbHtmlText = new StringBuilder();
            sbHtmlText.Append("<div style=\"text-align:center\"><nav><ul  class=\"pagination\">");
            if (pager.PageIndex <= 1)
            {
                sbHtmlText.Append("<li class=\"disabled\"><a href=\"javascript:void(0)\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span></a></li>");
            }
            else
            {
                sbHtmlText.AppendFormat("<li><a href=\"javascript:loadData(1)\" aria-label=\"1\"><span aria-hidden=\"true\">&laquo;</span></a></li>", pager.PageIndex - 1);
                sbHtmlText.AppendFormat("<li><a href=\"javascript:loadData({0})\">{0}</a></li>", pager.PageIndex - 1);
            }
            sbHtmlText.AppendFormat("<li class=\"active\"><a href=\"javascript:void(0)\">{0}<span class=\"sr-only\">(current)</span></a></li>", pager.PageIndex);
            if (pager.PageIndex >= pager.PageCount)
            {
                sbHtmlText.Append("<li class=\"disabled\"><a href=\"javascript:void(0)\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span></a></li>");
            }
            else
            {
                sbHtmlText.AppendFormat("<li><a href=\"javascript:loadData({0})\">{0}</a></li>", pager.PageIndex + 1);
                sbHtmlText.AppendFormat("<li><a href=\"javascript:loadData({0})\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span></a></li>", pager.PageIndex + 1);
            }
            sbHtmlText.Append("</ul></nav>");
            sbHtmlText.AppendFormat("<div><span>每页有<strong>{0}</strong>条数据，一共有<strong>{1}</strong>页，总计<strong>{2}</strong>条数据</span></div></div>", pager.PageSize, pager.PageCount, pager.TotalCount);            
            return MvcHtmlString.Create(sbHtmlText.ToString());
        }

        /// <summary>
        /// Pager V2.0
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pager"></param>
        /// <param name="onPageChange"></param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, PagerModel pager, Func<int, string> onPageChange)
        {
            StringBuilder sbHtmlText = new StringBuilder();
            sbHtmlText.Append("<div style=\"text-align:center\"><nav><ul  class=\"pagination\">");
            if (pager.PageIndex <= 1)
            {
                sbHtmlText.Append("<li class=\"disabled\"><a href=\"javascript:void(0)\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span></a></li>");
            }
            else
            {
                sbHtmlText.AppendFormat("<li><a href=\"{0}\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span></a></li>", onPageChange(pager.PageIndex - 1));
                sbHtmlText.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", onPageChange(pager.PageIndex - 1), pager.PageIndex - 1);
            }
            sbHtmlText.AppendFormat("<li class=\"active\"><a href=\"javascript:void(0)\">{0}<span class=\"sr-only\">(current)</span></a></li>", pager.PageIndex);
            if (pager.PageIndex >= pager.PageCount)
            {
                sbHtmlText.Append("<li class=\"disabled\"><a href=\"javascript:void(0)\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span></a></li>");
            }
            else
            {
                sbHtmlText.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", onPageChange(pager.PageIndex + 1), pager.PageIndex + 1);
                sbHtmlText.AppendFormat("<li><a href=\"{0}\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span></a></li>", onPageChange(pager.PageIndex + 1));
            }
            sbHtmlText.Append("</ul></nav>");
            sbHtmlText.AppendFormat("<div><span>每页有<strong>{0}</strong>条数据，一共有<strong>{1}</strong>页，总计<strong>{2}</strong>条数据</span></div></div>", pager.PageSize, pager.PageCount, pager.TotalCount);
            return MvcHtmlString.Create(sbHtmlText.ToString());
        }

        public static MvcHtmlString Pager(this HtmlHelper helper, PagerModel pager, Func<int, string> onPageChange, string pagerViewName, PagingDisplayMode displayMode = PagingDisplayMode.Always)
        {
            pager.OnPageChange = onPageChange;
            pager.PagingDisplayMode = displayMode;
            return MvcHtmlString.Create(helper.Partial(pagerViewName, pager).ToHtmlString());
        }
    }

    /// <summary>
    /// PagerModel 分页数据模型
    /// </summary>
    public class PagerModel: IPagerModel
    {
        public PagingDisplayMode PagingDisplayMode { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public int TotalCount { get; set; }

        public PagerModel(int pageIndex, int pageSize, int totalCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            PageCount = Convert.ToInt32(Math.Ceiling(TotalCount * 1.0 / PageSize));
        } 

        public bool IsFirstPage { get { return PageIndex <= 1; } }

        public bool IsLastPage { get { return PageIndex >= PageCount; } }

        public bool HasPreviousPage { get { return PageIndex > 1; } }

        public bool HasNextPage { get { return PageIndex < PageCount; } }

        public int FirstItem { get {  return (PageIndex - 1) * PageSize + 1; } }

        public int LastItem
        {
            get
            {
                if (IsLastPage)
                {
                    return FirstItem + (TotalCount -1)%PageSize;
                }
                else
                {
                    return PageIndex * PageSize;
                }
            }
        }

        public Func<int, string> OnPageChange { get; set; }

    }
    public interface IPagerModel
    {
        PagingDisplayMode PagingDisplayMode { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }

        int PageCount { get; set; }

        int TotalCount { get; set; }

        int FirstItem { get; }

        int LastItem { get; }

        bool IsFirstPage { get;}

        bool IsLastPage { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }

        Func<int, string> OnPageChange { get; set; }
    }
    public enum PagingDisplayMode
    {
        Always = 0,
        IfNeeded =1        
    }
}