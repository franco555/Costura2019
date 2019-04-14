using System.Web;
using System.Web.Optimization;

namespace PPSale
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/fa-jquery-confirm.js",
                      "~/Scripts/fa-jquery.dataTables.min.js",
                      "~/Scripts/fa-dataTables.bootstrap4.min.js",
                      "~/Scripts/fa-bootstrap-datetimepicker.js",
                      "~/Scripts/fa-FunctionBase.js",
                      "~/Scripts/fa-fileUpload.js",
                      "~/Scripts/fa-NavBar.js",
                      "~/Scripts/fa-ECommerce.js")); 

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/site.css",
                      "~/Content/fa-jquery-confirm.css",
                      "~/Content/fa-dataTable.bootstrap4.min.css",
                      "~/Content/fa-bootstrap-datetimepicker.css",
                      "~/Content/fa-fileUpload.css",
                      "~/Content/fa-ECommerceMenu.css",
                      "~/Content/fa-Estilos.css",
                      "~/Content/fa-NavBar.css",
                      "~/Content/fa-FormularioEntradaSalida.css"));
        }
    }
}
