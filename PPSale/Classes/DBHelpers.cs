namespace PPSale.Classes
{
    using System;
    using Models.Conexion;
    using Models.Other;

    public class DBHelpers
    {
        public static Response SaveChage(ConexionContext db)
        {
            //Response es un modelo que tiene dos campos el succeded y Message
            var response = new Response();

            try
            {
                db.SaveChanges();

                response.Succeded = true;
                return response;
            }
            catch (Exception Ex)
            {
                if (Ex.InnerException != null &&
                    Ex.InnerException.InnerException != null &&
                    Ex.InnerException.InnerException.Message.Contains("_Index"))
                {

                    response.Succeded = false;
                    response.Message = "El Registro ya existe!";

                    return response;
                }
                else if (Ex.InnerException != null &&
                    Ex.InnerException.InnerException != null &&
                    Ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {

                    response.Succeded = false;
                    response.Message = "Te Recuerdo que no puedes borrar!. Por que Existe en otras tablas y esta siendo usado.";


                    return response;
                }
                else
                {

                    response.Succeded = false;
                    response.Message = Ex.Message;

                    return response;
                }
            }

        }
    }
}