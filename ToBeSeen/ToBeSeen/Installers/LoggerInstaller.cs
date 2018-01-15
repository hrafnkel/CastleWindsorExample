using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Abp.Castle.Logging.Log4Net;
using log4net.Config;
[assembly: XmlConfigurator(Watch = true)]

namespace ToBeSeen.Installers
{
	// see:
	// https://github.com/castleproject/Windsor/blob/master/docs/mvc-tutorial-part-5-adding-logging-support.md
	// https://github.com/castleproject/Windsor/blob/master/docs/logging-facility.md
	// https://aspnetboilerplate.com/Pages/Documents/Logging#abpcastlelog4net-package

	// Note CW's log4net is broken

	public class LoggerInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			// container.AddFacility<LoggingFacility>(f => f.UseLog4Net()); <<< broken

			container.AddFacility<LoggingFacility>(f => f.UseAbpLog4Net().WithConfig("log4net.config"));
		}
	}


}