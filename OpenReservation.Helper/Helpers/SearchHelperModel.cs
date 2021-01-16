namespace OpenReservation.Helpers
{
    public class SearchHelperModel
    {
        private int _pageNumber = 1, _pageSize = 10;

        /// <summary>
        /// 当前页码索引
        /// </summary>
        public int PageIndex
        {
            get => _pageNumber;
            set
            {
                if (value > 0)
                {
                    _pageNumber = value;
                }
            }
        }

        /// <summary>
        /// 页码容量，每页数据量
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value > 0)
                {
                    _pageSize = value;
                }
            }
        }

        public string SearchItem0 { get; set; }

        public string SearchItem1 { get; set; }

        public string SearchItem2 { get; set; }
    }
}
