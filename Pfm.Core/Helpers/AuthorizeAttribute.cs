using System;
using Pfm.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Pfm.Core.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (TbUser)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new JsonResult(new
                {
                    IsSuccess = false,
                    ReturnMessage = "Unauthorized",
                    Data = string.Empty
                })
                { StatusCode = StatusCodes.Status401Unauthorized };

            }
        }
    }
}

