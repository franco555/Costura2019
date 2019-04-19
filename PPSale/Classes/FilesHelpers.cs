namespace PPSale.Classes
{
    using Models.Other;
    using System;
    using System.IO;
    using System.Web;

    public class FilesHelpers
    {
        public static Response UploadPhoto(HttpPostedFileBase file, string folder, string NewName, bool IsNew)
        {
            var response = new Response();
            var newFolder = string.Empty;

            try
            {
                string ruta = string.Empty;
                newFolder = $"~/Content/Logos/{folder}";

                if (IsNew)
                {
                    var guid = Guid.NewGuid().ToString();
                    NewName = $"{guid}.jpg";
                }
                else
                {
                    NewName = NewName.Substring(NewName.LastIndexOf('/') + 1);
                }

                if (file != null)
                {
                    ruta = Path.Combine(HttpContext.Current.Server.MapPath(newFolder), NewName);
                    file.SaveAs(ruta);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }

                    response.Succeded = true;
                    response.Message = $"{newFolder}/{NewName}";
                }
                else
                {
                    response.Succeded = false;
                    response.Message = $"El archivo esta vacío: {file}";
                }



                return response;
            }
            catch (Exception ex)
            {
                response.Succeded = false;
                response.Message = ex.Message;

                return response;
            }


        }
    }
}