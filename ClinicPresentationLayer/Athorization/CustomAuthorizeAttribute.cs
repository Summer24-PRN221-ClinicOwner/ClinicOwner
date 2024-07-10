using BusinessObjects.Entities;
using ClinicPresentationLayer.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
namespace ClinicPresentationLayer.Athorization
{
    public class CustomAuthorizeAttribute : Attribute, IPageFilter
    {
        private readonly int[] _roles;

        public CustomAuthorizeAttribute(params int[] roles)
        {
            _roles = roles;
        }
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            var user = context.HttpContext.Session.GetObject<User>("UserAccount");

            if (user == null)
            {
                context.Result = new RedirectToPageResult("/Login");
                return;
            }

            if (IsAuthorized(user.Role.Value) == false)
            {
                context.Result = new RedirectToPageResult("/Unauthorized");
                return;
            }
        }

            public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var user = context.HttpContext.Session.GetObject<User>("UserAccount");

            if (user == null)
            {
                context.Result = new RedirectToPageResult("/Login");
                return;
            }

            if (IsAuthorized(user.Role.Value) == false)
            {
                context.Result = new RedirectToPageResult("/Unauthorized");
                return;
            }
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            var user = context.HttpContext.Session.GetObject<User>("UserAccount");

            if (user == null)
            {
                context.HttpContext.Response.Redirect("/Login");
                return;
            }

            if (IsAuthorized(user.Role.Value) == false)
            {
                context.HttpContext.Response.Redirect("/Unauthorized");
                return;
            }
        }
        public bool IsAuthorized(int userRole)
        {
            return _roles.Contains(userRole);
        }
    }
}
