using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SocialMedia.Api.Application.Queries;
using SocialMedia.Api.Interfaces;
using SocialMedia.Api.Services;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.BackgroundTasks;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Options;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Services;
using System;
using System.IO;

namespace SocialMedia.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        //habilitar el return y no usar el void habilita el encadenamiento de metodos
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SocialMediaContext>(o => o.UseSqlServer(configuration.GetConnectionString("SocialMedia")));
            return services;
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            //configuracion por defecto de las paginas por si el usuario no ingresa paginacion
            services.Configure<PaginationOptions>(options => configuration.GetSection("Pagination").Bind(options));
            //Configuracion de las contraseñas y los hashing
            services.Configure<PasswordOptions>(options => configuration.GetSection("PasswordOptions").Bind(options));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //Servicios
            //services.AddTransient<IPostQueries, PostQueries>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            //services.AddTransient<IPostRepository, PostMongoRepository>(); para cambiar el acceso a BD y hacerlo mas escalable
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            //Agregar el servicio de infraestructura de password
            services.AddSingleton<IPasswordService, PasswordService>();
            //Agrega el servicio de Redis Cache
            services.AddSingleton<ICacheService, RedisCacheService>();
            services.AddHostedService<RedisSubcriber>();
            services.AddSingleton<IUriService>(provider =>
            {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>(); // queremos obtener la instancia del objeto HTTP
                var request = accesor.HttpContext.Request; //obtener el request que nos genera
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent()); // define la URL Base 
                return new UriService(absoluteUri); // Incorpora la URL base al UriService
            });
            return services;
        }

        public static IServiceCollection AddJwtrAuthentication(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, string xmlFileName)
        {
            //Swagger
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media API", Version = "v1" });
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                o.IncludeXmlComments(xmlPath);
            });
            return services;
        }
    }
}
