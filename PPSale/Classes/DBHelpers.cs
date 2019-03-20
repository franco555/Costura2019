using PPSale.Models.Conexion;
using PPSale.Models.Other;
using System;

namespace PPSale.Classes
{
    public class DBHelpers
    {
        public static Response SaveChage(ConexionContext db)
        {
            //Response es un modelo que tiene dos campos el succeded y Message
            var response = String.Empty;
            try
            {
                db.SaveChanges();

                var responses = new Response { Succeded = true,};
                return responses;
            }
            catch (Exception Ex)
            {
                if (Ex.InnerException != null &&
                    Ex.InnerException.InnerException != null &&
                    Ex.InnerException.InnerException.Message.Contains("_Index"))
                {

                    var responses = new Response
                    {
                        Succeded = false,
                        Message = "El Registro ya existe!",
                    };

                    return responses;
                }
                else if (Ex.InnerException != null &&
                    Ex.InnerException.InnerException != null &&
                    Ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    var responses = new Response
                    {
                        Succeded = false,
                        Message = "Te Recuerdo que no puedes borrar!. Por que Existe en otras tablas y esta siendo usado.",
                    };

                    return responses;
                }
                else
                {
                    var responses = new Response
                    {
                        Succeded = false,
                        Message = Ex.Message,
                    };

                    return responses;
                }
            }

        }
    }
}