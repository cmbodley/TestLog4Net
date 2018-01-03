using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;


namespace TestLog4Net.Helpers
{
    public class ActionLoggingAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        protected log4net.ILog Log = log4net.LogManager.GetLogger(typeof(Controller));

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                log4net.Config.XmlConfigurator.Configure();
                log4net.LogicalThreadContext.Properties["NetworkId"] = HttpContext.Current.User.Identity.Name;
                log4net.LogicalThreadContext.Properties["Action"] = filterContext.RouteData.Values["Action"];
                log4net.LogicalThreadContext.Properties["Controller"] = filterContext.RouteData.Values["Controller"];
                Log.Error("Exception Has Happend", filterContext.Exception);
            }
            base.OnActionExecuted(filterContext);
        }


        [ValidateInput(false)]
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            log4net.Config.XmlConfigurator.Configure();
            log4net.LogicalThreadContext.Properties["NetworkId"] = HttpContext.Current.User.Identity.Name;
            log4net.LogicalThreadContext.Properties["Action"] = filterContext.RouteData.Values["Action"];
            log4net.LogicalThreadContext.Properties["Controller"] = filterContext.RouteData.Values["Controller"];

            var actionParams = filterContext.ActionParameters;
            var fullparams = filterContext.RequestContext.HttpContext.Request.Params;

            log4net.LogicalThreadContext.Properties["Params"] =
                JsonConvert.SerializeObject(new { action = actionParams, full = fullparams });
            Log.Info("User Action");
        }
    }
}