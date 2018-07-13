using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using TestCoreApp.WebApi.Helpers;

namespace TestCoreApp.WebApi.Filters
{
    public class EtagFilter : Attribute, IActionFilter
    {
        private readonly int[] _statusCodes;

        public EtagFilter(params int[] statusCodes)
        {
            _statusCodes = statusCodes;
            if (statusCodes.Length == 0) _statusCodes = new[] { 200 };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Request.Method == "GET")
            {
                if (_statusCodes.Contains(context.HttpContext.Response.StatusCode))
                {
                    var content = JsonConvert.SerializeObject(context.Result);

                    var etag = EtagGenerator.GetETag(context.HttpContext.Request.Path.ToString(), Encoding.UTF8.GetBytes(content));

                    if (context.HttpContext.Request.Headers.Keys.Contains("If-None-Match") && context.HttpContext.Request.Headers["If-None-Match"].ToString() == etag)
                    {
                        context.Result = new StatusCodeResult(304);
                    }
                    context.HttpContext.Response.Headers.Add("ETag", new[] { etag });
                }
            }
        }
    }
}
