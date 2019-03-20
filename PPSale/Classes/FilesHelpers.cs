using System.IO;
using System.Web;

namespace PPSale.Classes
{
    public class FilesHelpers
    {
        public static bool UploadPhoto(HttpPostedFileBase file, string folder, string NewName)
        {
            if (file == null || string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(NewName))
            {
                return false;
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
                return true;
            }
            catch
            {
                return false;
            }


        }
    }
}