using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TestCoreApp.DataAccessLayer.EF;
using TestCoreApp.Entities;
using TestCoreApp.Entities.ServiceTypes;

namespace TestCoreApp.DataAccessLayer
{
    /// <summary>
    /// Resources Api.
    /// </summary>
    public class ResourcesRepository : IResourceRepository
    {
        /// <summary>
        /// Getting all resources.
        /// </summary>
        /// <returns>List of Resources</returns>
        public Result<List<Resource>> GetResources()
        {
            using(var resourceContext = new ResourceContext())
            {
                try
                {
                    return new Result<List<Resource>>(resourceContext.Resources.ToList());
                }
                catch (Exception e)
                {
                    return new Result<List<Resource>>(e.Message);
                }
            }
        }

        /// <summary>
        /// Getting all resources by filter.
        /// </summary>
        /// <param name="filter">Resource name</param>
        /// <returns>List of Resources filtered by resource name</returns>
        public Result<List<Resource>> GetResources(object filter)
        {
            if (filter == null) throw new ArgumentNullException();

            using (var resourceContext = new ResourceContext())
            {
                try
                {
                    return new Result<List<Resource>>(resourceContext.Resources.Where(r => r.Name.Contains(filter.ToString())).ToList());
                }
                catch (Exception e)
                {
                    return new Result<List<Resource>>(e.Message);
                }
            }
        }

        /// <summary>
        /// Getting all resources ordered normal or invert.
        /// </summary>
        /// <param name="order">Order type</param>
        /// <param name="orderBy">order by</param>
        /// <returns>Ordered list of Resources</returns>
        public Result<List<Resource>> GetResources(OrderType order, string orderBy)
        {
            using (var resourceContext = new ResourceContext())
            {
                try
                {
                    var prop = TypeDescriptor.GetProperties(typeof(Resource)).Find(orderBy, true);

                    switch (order)
                    {
                        case OrderType.Normal:
                            return new Result<List<Resource>>(resourceContext.Resources.OrderBy(x => prop.GetValue(x)).ToList());
                        case OrderType.Invert:
                            return new Result<List<Resource>>(resourceContext.Resources.OrderByDescending(x => prop.GetValue(x)).ToList());
                        default:
                            return new Result<List<Resource>>(resourceContext.Resources.OrderBy(x => prop.GetValue(x)).ToList());
                    }
                }
                catch (Exception e)
                {
                    return new Result<List<Resource>>(e.Message);
                }
            }
        }

        /// <summary>
        /// Getting resources by filter and ordered (normal or invert).
        /// </summary>
        /// <param name="filter">Resource name</param>
        /// <param name="order">Order type</param>
        /// <param name="orderBy">order by</param>
        /// <returns>Ordered list of Resources filtered by resource name</returns>
        public Result<List<Resource>> GetResources(object filter, OrderType order, string orderBy)
        {
            if (filter == null) throw new ArgumentNullException();

            using (var resourceContext = new ResourceContext())
            {
                try
                {
                    var prop = TypeDescriptor.GetProperties(typeof(Resource)).Find(orderBy, true);

                    switch (order)
                    {
                        case OrderType.Normal:
                            return new Result<List<Resource>>(resourceContext.Resources.Where(r => r.Name.Contains(filter.ToString())).OrderBy(x => prop.GetValue(x)).ToList());
                        case OrderType.Invert:
                            return new Result<List<Resource>>(resourceContext.Resources.Where(r => r.Name.Contains(filter.ToString())).OrderByDescending(x => prop.GetValue(x)).ToList());
                        default:
                            return new Result<List<Resource>>(resourceContext.Resources.Where(r => r.Name.Contains(filter.ToString())).OrderBy(x => prop.GetValue(x)).ToList());
                    }
                }
                catch (Exception e)
                {
                    return new Result<List<Resource>>(e.Message);
                }
            }
        }

        /// <summary>
        /// Getting resource by Id.
        /// </summary>
        /// <param name="id">Resource Id</param>
        /// <returns>Resource object</returns>
        public Result<Resource> GetResourceById(int id)
        {
            using (var resourceContext = new ResourceContext())
            {
                try
                {
                    var result = resourceContext.Resources.FirstOrDefault(r => r.Id == id);
                    return result != null ? new Result<Resource>(result) : new Result<Resource>("Resource not found.");
                }
                catch (Exception e)
                {
                    return new Result<Resource>(e.Message);
                }
            }
        }
        
        /// <summary>
        /// Adding new resource.
        /// </summary>
        /// <param name="resource">New object</param>
        /// <returns>Added Resource object</returns>
        public Result<Resource> AddNewResource(Resource resource)
        {
            if (resource == null) throw new ArgumentNullException();

            using (var resourceContext = new ResourceContext())
            {
                try
                {
                    resourceContext.Resources.Add(resource);
                    resourceContext.SaveChanges();
                    return new Result<Resource>(resource);
                }
                catch (Exception e)
                {
                    return new Result<Resource>(e.Message);
                }
            }
        }

        /// <summary>
        /// Deleting resource by Id.
        /// </summary>
        /// <param name="id">Resource Id</param>
        /// <returns>true</returns>
        public Result DeleteResourceById(int id)
        {
            using(var resourceContext = new ResourceContext())
            {
                try
                {
                    var resourceToRemove = resourceContext.Resources.FirstOrDefault(r => r.Id == id);

                    if (resourceToRemove == null) return new Result("No resource to delete.");

                    resourceContext.Resources.Remove(resourceToRemove);
                    resourceContext.SaveChanges();
                    return new Result(true);
                }
                catch (Exception e)
                {
                    return new Result(e.Message);
                }
            }
        }
        
        /// <summary>
        /// Trying to patch resource.
        /// </summary>
        /// <param name="entity">Resource object</param>
        /// <param name="patchDtos">List of PATCHes</param>
        /// <returns>Patched Resource object</returns>
        public Result<Resource> TryPatchResource(Resource entity, List<PatchDto> patchDtos)
        {
            if (entity == null || patchDtos == null) throw new ArgumentNullException();
            
            try
            {
                foreach (var patchDto in patchDtos)
                {
                    var prop = entity.GetType().GetProperty(patchDto.PropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(entity, Convert.ChangeType(patchDto.PropertyValue, prop.PropertyType));
                    }
                }
                return new Result<Resource>(entity);
            }
            catch (Exception ex)
            {
                return new Result<Resource>(ex.Message);
            }
        }

        /// <summary>
        /// Async applying patch.
        /// </summary>
        /// <param name="resourceId">id of resource</param>
        /// <param name="patchDtos">List of PATCHes</param>
        /// <returns>Modified entity</returns>
        public async Task ApplyPatchAsync(int resourceId, List<PatchDto> patchDtos)
        {
            if (patchDtos == null) throw new ArgumentNullException();

            using(var resourceContext = new ResourceContext())
            {
                var entity = resourceContext.Resources.FirstOrDefault(x => x.Id == resourceId);

                if (entity == null) return;

                foreach (var patchDto in patchDtos)
                {
                    var prop = entity.GetType().GetProperty(patchDto.PropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(entity, Convert.ChangeType(patchDto.PropertyValue, prop.PropertyType));
                    }
                }
                var dbEntityEntry = resourceContext.Entry(entity);
                dbEntityEntry.State = EntityState.Modified;
                await resourceContext.SaveChangesAsync();
            }  
        }

        public Result<ResourcesPage> GetResourcesPaginated(int itemsPerPage, int page)
        {
            try
            {
                using (var context = new ResourceContext())
                {
                    var querry = context.Resources.AsQueryable();

                    var pagedList = new CnPagedList<Resource>(querry, page, itemsPerPage);

                    var result = new ResourcesPage(pagedList.Items.ToList(), page, pagedList.TotalItemCount, itemsPerPage);

                    return new Result<ResourcesPage>(result);
                }
            }
            catch (Exception ex)
            {
                return new Result<ResourcesPage>(ex.Message);
            }
        }

        /// <summary>
        /// Gets Paginated resource page
        /// </summary>
        /// <param name="itemsPerPage">items in one page</param>
        /// <param name="page">page number</param>
        /// <param name="filter">name filter</param>
        /// <returns>Page</returns>
        public Result<ResourcesPage> GetResourcesPaginated(int itemsPerPage, int page, object filter)
        {
            try
            {
                using (var context = new ResourceContext())
                {
                    var querry = context.Resources.Where(r => r.Name.Contains(filter.ToString())).AsQueryable();

                    var pagedList = new CnPagedList<Resource>(querry, page, itemsPerPage);

                    var result = new ResourcesPage(pagedList.Items.ToList(), page, pagedList.TotalItemCount, itemsPerPage);

                    return new Result<ResourcesPage>(result);
                }
            }
            catch (Exception ex)
            {
                return new Result<ResourcesPage>(ex.Message);
            }
        }

        /// <summary>
        /// Gets Paginated resource page
        /// </summary>
        /// <param name="itemsPerPage">items in one page</param>
        /// <param name="page">page number</param>
        /// <param name="order">order</param>
        /// <param name="orderBy">order property</param>
        /// <returns>Page</returns>
        public Result<ResourcesPage> GetResourcesPaginated(int itemsPerPage, int page, OrderType order, string orderBy)
        {
            try
            {
                var prop = TypeDescriptor.GetProperties(typeof(Resource)).Find(orderBy, true);

                using (var context = new ResourceContext())
                {
                    IQueryable<Resource> querry;
                    switch (order)
                    {
                        case OrderType.Normal:
                            querry = context.Resources.OrderBy(x => prop.GetValue(x)).AsQueryable();
                            break;
                        case OrderType.Invert:
                            querry = context.Resources.OrderByDescending(x => prop.GetValue(x)).AsQueryable();
                            break;
                        default:
                            querry = context.Resources.OrderBy(x => prop.GetValue(x)).AsQueryable();
                            break;
                    }

                    var pagedList = new CnPagedList<Resource>(querry, page, itemsPerPage);

                    var result = new ResourcesPage(pagedList.Items.ToList(), page, pagedList.TotalItemCount, itemsPerPage);

                    return new Result<ResourcesPage>(result);
                }
            }
            catch (Exception ex)
            {
                return new Result<ResourcesPage>(ex.Message);
            }
        }

        /// <summary>
        /// Gets Paginated resource page
        /// </summary>
        /// <param name="itemsPerPage">items in one page</param>
        /// <param name="page">page number</param>
        /// <param name="order">order</param>
        /// <param name="filter">name filter</param>
        /// <param name="orderBy">order property</param>
        /// <returns>Page</returns>
        public Result<ResourcesPage> GetResourcesPaginated(int itemsPerPage, int page, object filter, OrderType order, string orderBy)
        {
            try
            {
                var prop = TypeDescriptor.GetProperties(typeof(Resource)).Find(orderBy, true);

                using (var context = new ResourceContext())
                {
                    IQueryable<Resource> querry;
                    switch (order)
                    {
                        case OrderType.Normal:
                            querry = context.Resources.Where(x => x.Name.Contains(filter.ToString())).OrderBy(x => prop.GetValue(x)).AsQueryable();
                            break;
                        case OrderType.Invert:
                            querry = context.Resources.Where(x => x.Name.Contains(filter.ToString())).OrderByDescending(x => prop.GetValue(x)).AsQueryable();
                            break;
                        default:
                            querry = context.Resources.Where(x => x.Name.Contains(filter.ToString())).OrderBy(x => prop.GetValue(x)).AsQueryable();
                            break;
                    }

                    var pagedList = new CnPagedList<Resource>(querry, page, itemsPerPage);

                    var result = new ResourcesPage(pagedList.Items.ToList(), page, pagedList.TotalItemCount, itemsPerPage);

                    return new Result<ResourcesPage>(result);
                }
            }
            catch (Exception ex)
            {
                return new Result<ResourcesPage>(ex.Message);
            }
        }
    }
}
