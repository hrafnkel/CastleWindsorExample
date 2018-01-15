using System;
using System.Linq;
using System.Web.Mvc;
using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.Windsor;
using ToBeSeen.Controllers;
using ToBeSeen.Installers;
using Xunit;

namespace ToBeSeenTests
{
	// Note: Example uses xUnit
	// see https://github.com/castleproject/Windsor/blob/master/docs/mvc-tutorial-part-3a-testing-your-first-installer.md

	public class ControllersInstallerTests
	{
		private IWindsorContainer containerWithControllers;

		public ControllersInstallerTests()
		{
			containerWithControllers = new WindsorContainer()
				.Install(new ControllersInstaller());
		}

		[Fact]
		public void All_controllers_implement_IController()
		{
			var allHandlers = GetAllHandlers(containerWithControllers);
			var controllerHandlers = GetHandlersFor(typeof(IController), containerWithControllers);

			Assert.NotEmpty(allHandlers);
			Assert.Equal(allHandlers, controllerHandlers);
		}

		[Fact]
		public void All_controllers_are_registered()
		{
			var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Is<IController>());
			var registeredControllers = GetImplementationTypesFor(typeof(IController), containerWithControllers);

			Assert.Equal(allControllers, registeredControllers);
		}

		[Fact]
		public void All_and_only_controllers_have_Controllers_suffix()
		{
			var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Name.EndsWith("Controller"));
			var registeredControllers = GetImplementationTypesFor(typeof(IController), containerWithControllers);
			Assert.Equal(allControllers, registeredControllers);
		}

		[Fact]
		public void All_and_only_controllers_live_in_Controllers_namespace()
		{
			var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Namespace.Contains("Controllers"));
			var registeredControllers = GetImplementationTypesFor(typeof(IController), containerWithControllers);
			Assert.Equal(allControllers, registeredControllers);
		}

		[Fact]
		public void All_controllers_are_transient()
		{
			var nonTransientControllers = GetHandlersFor(typeof(IController), containerWithControllers)
				.Where(controller => controller.ComponentModel.LifestyleType != LifestyleType.Transient)
				.ToArray();

			Assert.Empty(nonTransientControllers);
		}

		[Fact]
		public void All_controllers_expose_themselves_as_service()
		{
			var controllersWithWrongName = GetHandlersFor(typeof(IController), containerWithControllers)
				.Where(controller => controller.ComponentModel.Services.Single() != controller.ComponentModel.Implementation)
				.ToArray();

			Assert.Empty(controllersWithWrongName);
		}

		private IHandler[] GetAllHandlers(IWindsorContainer windsorContainer)
		{
			return GetHandlersFor(typeof(object), windsorContainer);
		}

		private IHandler[] GetHandlersFor(Type type, IWindsorContainer windsorContainer)
		{
			return windsorContainer.Kernel.GetAssignableHandlers(type);
		}

		private Type[] GetPublicClassesFromApplicationAssembly(Predicate<Type> where)
		{
			return typeof(HomeController).Assembly.GetExportedTypes()
				.Where(t => t.IsClass)
				.Where(t => t.IsAbstract == false)
				.Where(where.Invoke)
				.OrderBy(t => t.Name)
				.ToArray();
		}
		private Type[] GetImplementationTypesFor(Type type, IWindsorContainer container)
		{
			return GetHandlersFor(type, container)
				.Select(h => h.ComponentModel.Implementation)
				.OrderBy(t => t.Name)
				.ToArray();
		}
	}
}
