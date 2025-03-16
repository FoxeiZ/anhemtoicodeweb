using Microsoft.Ajax.Utilities;
using System.Web;
using System.Web.Mvc;

namespace anhemtoicodeweb.Filters
{
    /// <summary>
    /// Action filter that enforces user authentication and role-based authorization.
    /// Redirects to the login page if the user is not logged in or doesn't have the required role.
    /// </summary>
    public class RequiredLogin : ActionFilterAttribute
    {
        /// <summary>
        /// Determines if the return URL should be excluded from the redirect.
        /// </summary>
        private readonly bool _excludeReturnUrl;

        /// <summary>
        /// Custom return path to use after login (if specified).
        /// </summary>
        private readonly string _customReturnUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredLogin"/> class.
        /// </summary>
        /// <param name="excludeReturnUrl">When true, no return URL will be appended to the login redirect.</param>
        /// <param name="customReturnUrl">A custom return path to use after login (optional).</param>
        public RequiredLogin(bool excludeReturnUrl = false, string customReturnUrl = null)
        {
            _excludeReturnUrl = excludeReturnUrl;
            _customReturnUrl = customReturnUrl;
        }

        public RequiredLogin()
        {
            _excludeReturnUrl = false;
            _customReturnUrl = null;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool shouldRedirectToLogin = ShouldRedirectToLogin(filterContext);

            if (shouldRedirectToLogin)
            {
                string redirectUrl = BuildRedirectUrl(filterContext);
                filterContext.Result = new RedirectResult(redirectUrl);
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Determines if the current request should be redirected to the login page.
        /// </summary>
        protected virtual bool ShouldRedirectToLogin(ActionExecutingContext filterContext)
        {
            // First check if user is authenticated
            return filterContext.HttpContext.Session["UserId"] == null;
        }

        /// <summary>
        /// Builds the redirect URL for the login page.
        /// </summary>
        private string BuildRedirectUrl(ActionExecutingContext filterContext)
        {
            const string LoginPath = "/Login/Index";

            if (_excludeReturnUrl)
            {
                return LoginPath;
            }

            string returnUrl = DetermineReturnUrl(filterContext);
            return $"{LoginPath}?ReturnUrl={HttpUtility.UrlEncode(returnUrl)}";
        }

        /// <summary>
        /// Determines the return URL to use after login.
        /// </summary>
        private string DetermineReturnUrl(ActionExecutingContext filterContext)
        {
            return _customReturnUrl.IsNullOrWhiteSpace()
                ? filterContext.HttpContext.Request.RawUrl
                : _customReturnUrl;
        }
    }
}