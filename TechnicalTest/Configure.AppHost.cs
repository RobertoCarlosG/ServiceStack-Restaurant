using Funq;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using TechnicalTest.ServiceInterface;

[assembly: HostingStartup(typeof(TechnicalTest.AppHost))]

namespace TechnicalTest;

public class AppHost : AppHostBase, IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services => {
            // Configure ASP.NET Core IOC Dependencies
        });

    public AppHost() : base("TechnicalTest", typeof(MyServices).Assembly) {}

    public override void Configure(Container container)
    {
        // enable server-side rendering, see: https://sharpscript.net/docs/sharp-pages
        OrmLiteConnectionFactory dbFactory = new OrmLiteConnectionFactory(
           "Server=DESKTOP-HAQL8HN\\SQLEXPRESS;Database=Prueba;User Id=testing;Password=QWERTY1234;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True;",
           SqlServerDialect.Provider);

        container.Register<IDbConnectionFactory>(dbFactory);

        Plugins.Add(new SharpPagesFeature
        {
            EnableSpaFallback = true
        });

        SetConfig(new HostConfig
        {
            AddRedirectParamsToQueryString = true,
        });
    }
}
