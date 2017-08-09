using CKSource.CKFinder.Connector.Config;
using Microsoft.Owin;
using Owin;
using CKSource.CKFinder.Connector.Core.Builders;
using CKSource.CKFinder.Connector.Core.Logs;
using CKSource.CKFinder.Connector.Host.Owin;
using CKSource.CKFinder.Connector.Logs.NLog;
using CKSource.CKFinder.Connector.KeyValue.EntityFramework;
using CKSource.FileSystem.Local;

[assembly: OwinStartupAttribute(typeof(Blog.Startup))]
namespace Blog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            FileSystemFactory.RegisterFileSystem<LocalStorage>();

            app.Map("/ckfinder/connector", SetupConnector);
        }
        private static void SetupConnector(IAppBuilder app)
        {
            /*
             * Create a connector instance using ConnectorBuilder. The call to the LoadConfig() method
             * will configure the connector using CKFinder configuration options defined in Web.config.
             */
            var connectorFactory = new OwinConnectorFactory();
            var connectorBuilder = new ConnectorBuilder();
            /*
             * Create an instance of authenticator implemented in the previous step.
             */
            var customAuthenticator = new CustomCKFinderAuthenticator();
            connectorBuilder
                /*
                 * Provide the global configuration.
                 *
                 * If you installed CKSource.CKFinder.Connector.Config you should load the static configuration
                 * from XML:
                 * connectorBuilder.LoadConfig();
                 */
                .LoadConfig()
                .SetAuthenticator(customAuthenticator)
                .SetRequestConfiguration(
                    (request, config) =>
                    {
                        /*
                         * Configure settings per request.
                         *
                         * The minimal configuration has to include at least one backend, one resource type
                         * and one ACL rule.
                         *
                         * For example:
                         * config.LoadConfig()
                         * config.AddBackend("default", new LocalStorage(@"C:\files"));
                         * config.AddResourceType("images", builder => builder.SetBackend("default", "images"));
                         * config.AddAclRule(new AclRule(
                         *     new StringMatcher("*"),
                         *     new StringMatcher("*"),
                         *     new StringMatcher("*"),
                         *     new Dictionary<Permission, PermissionType> { { Permission.All, PermissionType.Allow } }));
                         *
                         * If you installed CKSource.CKFinder.Connector.Config, you should load the static configuration
                         * from XML:
                         */ config.LoadConfig();
                         /*
                         * If you installed CKSource.CKFinder.Connector.KeyValue.EntityFramework, you may enable caching:
                         */ config.SetKeyValueStoreProvider(
                            new EntityFrameworkKeyValueStoreProvider("DefaultConnection"));
                        
                    }
                );
            /*
             * Build the connector middleware.
             */
            var connector = connectorBuilder
                .Build(connectorFactory);
            /*
             * Add the CKFinder connector middleware to the web application pipeline.
             */
            app.UseConnector(connector);
        }
    }
}
