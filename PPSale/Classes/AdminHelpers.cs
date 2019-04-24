using PPSale.Models.Conexion;
using PPSale.Models.Globals;
using System;
using System.Linq;
using System.Web.WebPages;

namespace PPSale.Classes
{

    public class AdminHelpers:IDisposable
    {
        private static ConexionContext db = new ConexionContext();
        private static FunctionHelpers fn = new FunctionHelpers();

        public static bool IngresarDatosAdmin()
        {
            var ok = 0;//Compania();
            if (ok!=0)
            {
                return true;
            }
            return false;
        }

        

        public void Dispose()
        {
            db.Dispose();
        }
    }
}