namespace PPSale.Classes
{
    using Models.Other;
    using System.IO;
    using System.Web;

    public class FilesHelpers
    {
        public static Response UploadPhoto(HttpPostedFileBase file, string folder, string NewName)
        {
            var response = new Response();

            if (file == null || string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(NewName))
            {
                response.Succeded = false;
                response.Message = $"Nombre de Folder: {folder} - Nombre de Archivo: {NewName}";
                response.Objet = file;

                return response;
            }

            try
            {
                string ruta = string.Empty;

                if (file != null)
                {
                    ruta = Path.Combine(HttpContext.Current.Server.MapPath(folder), NewName);
                    file.SaveAs(ruta);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                else
                {
                    response.Succeded = false;
                    response.Message = $"El archivo esta vacío: {file}";
                }

                

                return response;
            }
            catch
            {

                return false;
            }


        }
    }
}