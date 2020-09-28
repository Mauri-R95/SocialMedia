using SocialMedia.Api.Application.Queries;
using SocialMedia.Api.Interfaces;
using SocialMedia.Api.Services;

namespace SocialMedia.Api.Infrastructure.AutofacModules
{
    public class ApplicationModule // : Autofac.Module
    {
        //builder para autofac
        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;
        }

        //protected override void Load(/*ContainerBuilder builder*/)
        //{
        //    builder.Register(c => new PostQueries(QueriesConnectionString))
        //        .As<IPostQueries>()
        //        .InstancePerLifetimeScope();

        //    builder.RegisterType<PostService>()
        //        .As<IPostService>()
        //        .InstancePerLifetimeScope();
        //    //base.Load(builder);
        //}
    }
}
