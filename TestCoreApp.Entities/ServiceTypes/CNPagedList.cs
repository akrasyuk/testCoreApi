using System.Linq;

namespace TestCoreApp.Entities.ServiceTypes
{
    /// <summary>
    /// A class that handles pagination of an IQueryable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CnPagedList<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">The full list of items you would like to paginate</param>
        /// <param name="page">(optional) The current page number</param>
        /// <param name="pageSize">(optional) The size of the page</param>
        public CnPagedList(IQueryable<T> list, int? page = null, int? pageSize = null)
        {
            _list = list;
            _page = page;
            _pageSize = pageSize;
        }

        private IQueryable<T> _list;

        /// <summary>
        /// The paginated result
        /// </summary>
        public IQueryable<T> Items => _list?.Skip((Page - 1) * PageSize).Take(PageSize);

        private int? _page;
        /// <summary>
        ///  The current page.
        /// </summary>
        public int Page => _page ?? 1;

        private int? _pageSize;
        /// <summary>
        /// The size of the page.
        /// </summary>
        public int PageSize
        {
            get
            {
                if (!_pageSize.HasValue)
                {
                    return _list?.Count() ?? 0;
                }
                else
                {
                    return _pageSize.Value;
                }
            }
        }

        /// <summary>
        /// The total number of items in the original list of items.
        /// </summary>
        public int TotalItemCount => _list?.Count() ?? 0;
    }
}
