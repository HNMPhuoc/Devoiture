namespace Devoiture.Helpers
{
    public class MyUtil
    {
        public static string UploadHinh(string folder, IFormFile Hinh)
        {
            try
            {
                var fileName = Hinh.FileName;
                var fullpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folder, fileName);
                if (File.Exists(fullpath))
                {
                    // Thêm chuỗi ngẫu nhiên vào tên file để đảm bảo tính duy nhất
                    var fileExtension = Path.GetExtension(fileName);
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    var uniqueFileName = $"{fileNameWithoutExtension}_{Guid.NewGuid()}{fileExtension}";
                    fullpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folder, uniqueFileName);
                    fileName = uniqueFileName;
                }
                using (var myfile = new FileStream(fullpath, FileMode.CreateNew))
                {
                    Hinh.CopyTo(myfile);
                }
                return fileName;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}