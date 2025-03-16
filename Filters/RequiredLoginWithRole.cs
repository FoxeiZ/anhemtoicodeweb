using Microsoft.Ajax.Utilities;
using System;
using System.Web;
using System.Web.Mvc;

namespace anhemtoicodeweb.Filters
{
    public class RequireLoginWithRole : RequiredLogin
    {

        /// <summary>
        /// The role required to access the action.
        /// </summary>
        private readonly Enums.Role[] _requiredRoles;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireLoginWithRole"/> class.
        /// </summary>
        /// <param name="requiredRole">The role required to access the action.</param>
        public RequireLoginWithRole(Enums.Role requiredRole, bool excludeReturnUrl = false, string customReturnUrl = null)
            : base(excludeReturnUrl, customReturnUrl)
        {
            _requiredRoles = new Enums.Role[] { requiredRole };
        }

        public RequireLoginWithRole(params Enums.Role[] requiredRole)
            : base(false, null)
        {
            _requiredRoles = requiredRole;
        }

        /// <summary>
        /// Checks if the current user has the required role.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>True if the user has the required role; otherwise, false.</returns>
        protected virtual bool UserHasRequiredRole(HttpContextBase httpContext)
        {
            // Get the user's role from the session
            var userRole = httpContext.Session["UserRole"].IfNotNull(x => (Enums.Role)x, Enums.Role.Empty);

            // Check if the user's role matches the required role
            return userRole.Equals(Enums.Role.Admin) || Array.IndexOf(_requiredRoles, userRole) >= 0;
        }

        protected override bool ShouldRedirectToLogin(ActionExecutingContext filterContext)
        {
            return base.ShouldRedirectToLogin(filterContext) || !UserHasRequiredRole(filterContext.HttpContext);
        }
    }

    /// <summary>
    /// Action filter that enforces user authentication with admin role.
    /// Redirects to the login page if the user is not logged in or is not an admin.
    /// </summary>
    public class RequireAdminRole : RequireLoginWithRole
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequireAdminRole"/> class.
        /// </summary>
        /// <param name="excludeReturnUrl">When true, no return URL will be appended to the login redirect.</param>
        /// <param name="customReturnUrl">A custom return path to use after login (optional).</param>
        public RequireAdminRole(bool excludeReturnUrl = false, string customReturnUrl = null)
            : base(Enums.Role.Admin, excludeReturnUrl, customReturnUrl)
        {
        }
    }
}