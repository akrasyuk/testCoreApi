using Microsoft.AspNetCore.Mvc;
using System;
using TestCoreApp.DataAccessLayer;
using TestCoreApp.Entities;
using TestCoreApp.Entities.ServiceTypes;
using TestCoreApp.WebApi.Filters;

namespace TestCoreApp.WebApi.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v-{version:apiVersion}/[controller]")]
    public class ResourcesPaginatedController: Controller
    {
        #region Fields

        private IResourceRepository resourceRepository;

        #endregion

        #region Contructor(s)

        public ResourcesPaginatedController(IResourceRepository resourceRepository)
        {
            this.resourceRepository = resourceRepository;
        }

        #endregion

        #region Methods

        [HttpGet]
        [EtagFilter(200)]
        public ResourcesPage Get([FromQuery(Name = "itemsPerPage")] int itemsPerPage, [FromQuery(Name = "page")] int page, [FromQuery(Name = "filter")] string filter, [FromQuery(Name = "sort")] string order, [FromQuery(Name = "orderBy")] string orderBy)
        {
            ResourcesPage result;

            if (orderBy != null && order != null)
            {
                Enum.TryParse<OrderType>(order, out var orderType);

                result = filter != null
                    ? resourceRepository.GetResourcesPaginated(itemsPerPage, page, filter, orderType, orderBy).ResultObject
                    : resourceRepository.GetResourcesPaginated(itemsPerPage, page, orderType, orderBy).ResultObject;

                return result;
            }

            result = filter != null ? resourceRepository.GetResourcesPaginated(itemsPerPage, page, filter).ResultObject : resourceRepository
                .GetResourcesPaginated(itemsPerPage, page).ResultObject;

            return result;
        }

        #endregion
    }
}
