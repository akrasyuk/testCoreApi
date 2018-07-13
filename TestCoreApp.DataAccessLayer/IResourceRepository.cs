using System.Collections.Generic;
using System.Threading.Tasks;
using TestCoreApp.Entities;
using TestCoreApp.Entities.ServiceTypes;

namespace TestCoreApp.DataAccessLayer
{
    public interface IResourceRepository
    {
        Result<List<Resource>> GetResources();

        Result<List<Resource>> GetResources(object filter);

        Result<List<Resource>> GetResources(OrderType order, string orderBy);

        Result<List<Resource>> GetResources(object filter, OrderType order, string orderBy);

        Result<Resource> GetResourceById(int id);

        Result<Resource> AddNewResource(Resource resource);

        Result DeleteResourceById(int id);

        Result<Resource> TryPatchResource(Resource entity, List<PatchDto> patchDtos);

        Task ApplyPatchAsync(int resourceId, List<PatchDto> patchDtos);

        Result<ResourcesPage> GetResourcesPaginated(int itemsPerPage, int page);

        Result<ResourcesPage> GetResourcesPaginated(int itemsPerPage, int page, object filter);

        Result<ResourcesPage> GetResourcesPaginated(int itemsPerPage, int page, OrderType order, string orderBy);

        Result<ResourcesPage> GetResourcesPaginated(int itemsPerPage, int page, object filter, OrderType order, string orderBy);
    }
}
