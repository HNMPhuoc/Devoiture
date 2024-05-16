namespace Devoiture.Helpers
{
    public class MyUtil
    {
        public static string UploadHinh(string folder,IFormFile Hinh1)
        {
            try
            {
                var fullpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","images",folder,Hinh1.FileName);
                using (var myfile = new FileStream(fullpath, FileMode.CreateNew))
                {
                    Hinh1.CopyTo(myfile);
                }
                return Hinh1.FileName;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
