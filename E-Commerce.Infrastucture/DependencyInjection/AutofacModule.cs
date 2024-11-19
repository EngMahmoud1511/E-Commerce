using Autofac;
using CloudinaryDotNet;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Interfaces;
using E_Commerce.Infrastucture.Configuration;
using E_Commerce.Infrastucture.ExternalServices.CloudinaryService;
using E_Commerce.Infrastucture.ExternalServices.Context;
using E_Commerce.Infrastucture.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastucture.DependencyInjection
{
    public class AutofacModule :Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx =>
            {
                var configuration = ctx.Resolve<IConfiguration>();
                return configuration;
            }).As<IConfiguration>().SingleInstance();

            // Register DbContext with SQL Server
            builder.Register(ctx =>
            {
                var configuration = ctx.Resolve<IConfiguration>();
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("connect"));
                return new ApplicationDbContext(optionsBuilder.Options);
            }).As<ApplicationDbContext>().InstancePerLifetimeScope();

            // Register Cloudinary service
            builder.Register(ctx =>
            {
                var configuration = ctx.Resolve<IConfiguration>();
                var cloudinarySettings = new CloudinarySettings
                {
                    CloudName = configuration["CloudinarySettings:CloudName"],
                    ApiKey = configuration["CloudinarySettings:ApiKey"],
                    ApiSecret = configuration["CloudinarySettings:ApiSecret"]
                };

                var account = new Account(
                    cloudinarySettings.CloudName,
                    cloudinarySettings.ApiKey,
                    cloudinarySettings.ApiSecret
                );

                return new Cloudinary(account);
            }).As<Cloudinary>().SingleInstance();

            // Register other services and repositories
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<CloudinaryService>().As<ICloudinaryService>().InstancePerLifetimeScope();
        }
    }
}
