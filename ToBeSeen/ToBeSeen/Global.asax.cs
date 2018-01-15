using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Windsor.Installer;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using ToBeSeen.Plumbing;

namespace ToBeSeen
{
    public class MvcApplication : System.Web.HttpApplication
    {
		// see https://github.com/castleproject/Windsor/blob/master/docs/mvc-tutorial-part-4-putting-it-all-together.md

		private static IWindsorContainer _container;

		protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

			BootstrapContainer();
        }

		private static void BootstrapContainer()
		{
			_container = new WindsorContainer()
				.Install(FromAssembly.This());
			var controllerFactory = new WindsorControllerFactory(_container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
		}

		protected void Application_End()
		{
			_container.Dispose();
		}
	}
}
