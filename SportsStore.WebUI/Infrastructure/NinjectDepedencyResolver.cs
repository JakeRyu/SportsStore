﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Web.Mvc;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Concrete;
using System.Configuration;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Infrastructure.Concrete;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectDepedencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDepedencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        private void AddBindings()
        {
            // put bindings here
            //Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //mock.Setup(m => m.Products).Returns(new List<Product>
            //{
            //    new Product {Name = "Football", Price = 25},
            //    new Product {Name = "Surf board", Price = 179},
            //    new Product {Name = "Running shoes", Price = 95}
            //});

            //kernel.Bind<IProductRepository>().ToConstant(mock.Object);

            kernel.Bind<IProductRepository>().To<EFProductRepository>();

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);

            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}