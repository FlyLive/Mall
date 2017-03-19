using Mall.Web.Back.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Back.Filter
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public string Permission { get; set; }

        public PermissionAuthorizeAttribute(string permission)
        {
            Permission = permission;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            List<PermissionsViewModel> permissions = (List<PermissionsViewModel>)httpContext.Session["Permissions"];
            if (permissions != null)
            {
                permissions.ForEach(p => p.Code.Contains(Permission));
                foreach(var permission in permissions)
                {
                    if (permission.Code.Contains(Permission))
                        return true;
                }
            }
            return base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            UrlHelper url = new UrlHelper(filterContext.RequestContext);
            filterContext.Result = new RedirectResult("/Error");
        }
    }
}