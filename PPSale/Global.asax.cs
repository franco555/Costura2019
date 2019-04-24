using PPSale.Classes;
using PPSale.Migrations;
using PPSale.Models.Conexion;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PPSale
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ConexionContext, Configuration>());

            CheckRolesAndSuperUser();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void CheckRolesAndSuperUser()
        {
            UsersHelper.CheckRole("Admin",true);
            UsersHelper.CheckRole("Usuario", true);
            UsersHelper.CheckRole("Proveedor", true);
            UsersHelper.CheckRole("Cliente", true);

            UsersHelper.CheckSuperUser();
            AdminHelpers.IngresarDatosAdmin();
        }
    }
}
