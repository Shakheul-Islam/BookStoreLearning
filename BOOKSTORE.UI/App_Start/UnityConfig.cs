using BOOKSTORE.INFRASTRUCTURE.Interfaces;
using BOOKSTORE.INFRASTRUCTURE.Services.BookServices;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace BOOKSTORE.UI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IBookService, BookService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}