using System;
using System.Text;
using System.Web.Mvc;

namespace ActivityReservation.Helpers
{
    /// <summary>
    /// PagerHelper 分页帮助类
    /// </summary>
    public static class PagerHelper
    {
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
                sbHtmlText.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", onPageChange(pager.PageIndex+1),pager.PageIndex + 1);
                sbHtmlText.AppendFormat("<li><a href=\"{0}\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span></a></li>", onPageChange(pager.PageIndex + 1));
            }
            sbHtmlText.Append("</ul></nav>");
            sbHtmlText.AppendFormat("<div><span>每页有<strong>{0}</strong>条数据，一共有<strong>{1}</strong>页，总计<strong>{2}</strong>条数据</span></div></div>", pager.PageSize, pager.PageCount, pager.TotalCount);
            return MvcHtmlString.Create(sbHtmlText.ToString());
        }

        public static MvcHtmlString Pager(this HtmlHelper helper, PagerModel pager, Func<int, string> onPageChange, string pagerViewName)
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
    }

    /// <summary>
    /// PagerModel 分页数据模型
    /// </summary>
    public class PagerModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; private set; }

        public int TotalCount { get; set; }

        public PagerModel(int totalCount)
        {
            PageIndex = 1;
            PageSize = 10;
            TotalCount = totalCount;
            PageCount = Convert.ToInt32(Math.Ceiling(TotalCount * 1.0 / PageSize));
        }

        public PagerModel(int pageSize, int totalCount)
        {
            PageIndex = 1;
            PageSize = pageSize;
            TotalCount = totalCount;
            PageCount = Convert.ToInt32(Math.Ceiling(TotalCount * 1.0 / PageSize));
        }

        public PagerModel(int pageIndex, int pageSize, int totalCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            PageCount = Convert.ToInt32(Math.Ceiling(TotalCount * 1.0 / PageSize));
        } 
    }
}