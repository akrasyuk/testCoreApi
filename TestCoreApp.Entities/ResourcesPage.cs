using System;
using System.Collections.Generic;

namespace TestCoreApp.Entities
{
    public class ResourcesPage
    {
        public int Index { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<Resource> Data { get; set; }

        public ResourcesPage(IEnumerable<Resource> resources, int page, int totalItems, int itemsPerPage)
        {
            this.Index = page;
            this.Data = resources;
            this.TotalPages = (int)Math.Ceiling(totalItems / (decimal)itemsPerPage);
        }
    }
}
