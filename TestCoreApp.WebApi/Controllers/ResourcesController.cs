using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TestCoreApp.DataAccessLayer;
using TestCoreApp.Entities;
using TestCoreApp.WebApi.Filters;
using TestCoreApp.Entities.ServiceTypes;
using System.Threading.Tasks;

namespace TestCoreApp.WebApi.Controllers.V1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v-{version:apiVersion}/[controller]")]
    public class ResourcesController : Controller
    {
        #region Fields

        private IResourceRepository resourceRepository;

        #endregion

        #region Contructor(s)

        public ResourcesController(IResourceRepository resourceRepository)
        {
            this.resourceRepository = resourceRepository;
        }

        #endregion

        #region Methods


        [HttpGet]
        public IEnumerable<Resource> Get([FromQuery(Name = "filter")] string filter, [FromQuery(Name = "sort")] string order, [FromQuery(Name = "orderBy")] string orderBy)
        {
            Result<List<Resource>> result;

            if (orderBy != null && order != null)
            {
                Enum.TryParse<OrderType>(order, out var orderType);

                result = filter != null
                    ? resourceRepository.GetResources(filter, orderType, orderBy)
                    : resourceRepository.GetResources(orderType, orderBy);

                return result.ResultObject;
            }

            result = filter != null ? resourceRepository.GetResources(filter) : resourceRepository.GetResources();

            return result.ResultObject;
        }

        [HttpGet("{id}")]
        [EtagFilter(200)]
        public Resource Get(int id)
        {
            var result = resourceRepository.GetResourceById(id);

            return result.ResultObject;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Resource resource)
        {
            if (resource == null)
            {
                ModelState.AddModelError("", "Resource is required");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (resource.CustomProperty3.Month == 5)
            {
                if (resource.CustomProperty5 == null)
                {
                    ModelState.AddModelError("CustomProperty5", "Is required if CustomProperty3 equals May 2018");
                    return BadRequest(ModelState);
                }
            }

            var result = resourceRepository.AddNewResource(resource);

            if (result.IsSuccess) return Ok(result.ResultObject);
            
    
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("Add error", error.Message);
            }

            return BadRequest(ModelState);
        }

        // TODO: Find a better way to validate model after applying changes but before sending them to Data Base.
        [HttpPatch("{id}", Name = "PatchResource")]
        public async Task<IActionResult> Patch(int id, [FromBody] List<PatchDto> patchDtos)
        {
            var resourceResult = resourceRepository.GetResourceById(id);

            if (!resourceResult.IsSuccess) return BadRequest();

            var resourceToValidateResult = resourceRepository.TryPatchResource(resourceResult.ResultObject, patchDtos);

            if (!resourceToValidateResult.IsSuccess) return BadRequest();

            if (!TryValidateModel(resourceToValidateResult.ResultObject)) return BadRequest(ModelState);

            if (resourceToValidateResult.ResultObject.CustomProperty3.Month == 5)
            {
                if (resourceToValidateResult.ResultObject.CustomProperty5 == null)
                {
                    ModelState.AddModelError("CustomProperty5", "Is required if CustomProperty3 equals May 2018");
                    return BadRequest(ModelState);
                }
            }

            await resourceRepository.ApplyPatchAsync(resourceResult.ResultObject.Id, patchDtos);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = resourceRepository.DeleteResourceById(id);

            if (result.IsSuccess)
            {
                return Ok();
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("Delete error" , error.Message);
            }
            return BadRequest(ModelState);
        } 

        #endregion
    }
}

namespace TestCoreApp.WebApi.Controllers.V2
{
    [Produces("application/json")]
    [ApiVersion("2.0")]
    [Route("api/v-{version:apiVersion}/[controller]")]
    public class ResourcesController : Controller
    {
        #region Fields

        private IResourceRepository resourceRepository;

        #endregion

        #region Contructor(s)

        public ResourcesController(IResourceRepository resourceRepository)
        {
            this.resourceRepository = resourceRepository;
        }

        #endregion

        #region Methods

        // GET api/values
        [HttpGet]
        [EtagFilter(200)]
        public IEnumerable<Resource> Get()
        {
            return new[]
            {
                new Resource()
                {
                    Name = "Resource 1"
                },
                new Resource()
                {
                    Name = "Resource 2"
                }
            };
        } 

        #endregion

    }
}