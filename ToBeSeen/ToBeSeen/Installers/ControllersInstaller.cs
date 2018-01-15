using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace ToBeSeen.Installers
{
	public class ControllersInstaller : IWindsorInstaller
	{
		// see https://github.com/castleproject/Windsor/blob/master/docs/mvc-tutorial-part-3-writing-your-first-installer.md
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Classes.FromThisAssembly()
				.BasedOn<IController>()
				.LifestyleTransient());
		}
	}
}