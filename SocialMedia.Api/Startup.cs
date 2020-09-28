using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Api.Application.Queries;
using SocialMedia.Api.Extensions;
using SocialMedia.Api.Mappings;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Filters;
using StackExchange.Redis;
using System;
using System.Reflection;
using System.Text;

namespace SocialMedia.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(cfg => { cfg.AddProfile<AutomapperProfile>();});
            services.AddTransient<IPostQueries, PostQueries>();
            services.AddControllers(o => 
            {
                o.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore; //para ignorar los nulos de los responses
            })
            .ConfigureApiBehaviorOptions(o => 
            {
                //o.SuppressModelStateInvalidFilter = true;                 
            });
            //services.AddOptions(Configuration).AddDbContexts(Configuration); // Encadenamiento de metodos que igual es factible
            //se ve mas ordenado asi
            services.AddOptions(Configuration);
            services.AddDbContexts(Configuration);
            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(Configuration.GetValue<string>("RedisConnection")));
            services.AddServices();
            //services.AddScoped<IPostQueries>(_ => new PostQueries(sql));
            services.AddTransient<IPostQueries, PostQueries>();
            var sqlConnectionConfiguration = new SqlConfiguration(Configuration.GetConnectionString("SocialMedia"));
            services.AddSingleton(sqlConnectionConfiguration);
            services.AddSwagger($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            //Agregar autentication JWT
            services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Authentication:Issuer"],
                        ValidAudience = Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                    };
                });
            //Agregar un filtro de forma global
            services.AddMvc(o =>
            {
                //test no se utiliza
                o.Filters.Add<ValidationFilter>();
            }).AddFluentValidation(o =>
            {
                //Registramos los Validator de Fluent Validator
                o.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });
            
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //configure autofac 
            //var container = new ContainerBuilder();
            //container.Populate(services);
            //container.RegisterModule(new ApplicationModule(Configuration["SocialMedia"]));
            //return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Media API V1");
                o.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            //autenticacion y despues la autorizacion, se define quien soy y despues a que tengo permiso
            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
