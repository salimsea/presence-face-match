using Ispm.Core.Helpers;

namespace Pfm.Api.Helpers
{
    public static class FileHelper
    {
        public static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        public static void SaveFile(string fullFileName, IFormFile fileUpload)
        {
            string path = Path.GetDirectoryName(fullFileName);
            if (File.Exists(fullFileName))
                File.Delete(fullFileName);
            using (var fileStream = new FileStream(fullFileName, FileMode.Create))
            {
                fileUpload.CopyTo(fileStream);
            }
        }
        public static void DeleteFile(string fullFileName)
        {
            if (File.Exists(fullFileName))
                File.Delete(fullFileName);
            string fileThumbnail = $"thumb_{Path.GetFileName(fullFileName)}";
            if (File.Exists($"{Path.GetDirectoryName(fullFileName)}/{fileThumbnail}"))
                File.Delete($"{Path.GetDirectoryName(fullFileName)}/{fileThumbnail}");
        }
        public static void DeleteFiles(string path, string fileContaints)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] Files = d.GetFiles(fileContaints); //Getting Text files
            foreach (FileInfo file in Files)
            {
                FileHelper.DeleteFile(file.FullName);
            }
        }

        static string[] imageExtensions = { ".JPG", ".JPEG", ".PNG" };

        public static bool IsImage(string fullPathFileName)
        {
            return -1 != Array.IndexOf(imageExtensions, Path.GetExtension(fullPathFileName).ToUpperInvariant());
        }

        public static string SaveImage(string path, string filename, IFormFile file, bool generateThumbain)
        {
            try
            {
                string fullFileName = Path.Combine(path, filename);
                if (File.Exists(fullFileName))
                    File.Delete(fullFileName);
                using (var fileStream = new FileStream(fullFileName, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ErrorHelper.GetErrorMessage("SaveImage", ex);
            }
        }
        public static string DeleteImage(string path, string filename)
        {
            try
            {
                string fullFileName = Path.Combine(path, filename);
                if (File.Exists(fullFileName))
                    File.Delete(fullFileName);
                var fileThumbnail = Path.Combine(path, $"thumb_{filename}");
                if (File.Exists(fileThumbnail))
                    File.Delete(fileThumbnail);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ErrorHelper.GetErrorMessage("DeleteImage", ex);
            }
        }

        static string[] videoExtensions =
            {".WEBM", ".MPG",".MP2",".MPEG",".MPE",".MPV", ".OGG", ".MP4",".M4P",".M4V", ".AVI", ".WMV",".MOV",".QT",".FLV",".SWF"};

        public static bool IsVideo(string fullPathFileName)
        {
            return -1 != Array.IndexOf(videoExtensions, Path.GetExtension(fullPathFileName).ToUpperInvariant());
        }
        public static string SaveFile(int type, string pathFile, string fileName, bool generateThumnail, IFormFile fileUpload)
        {
            string err = string.Empty;
            try
            {
                if (type == 1) //image
                {
                    if (!FileHelper.IsImage($"{pathFile}/{fileUpload.FileName}")) { err = "file must be in image format"; goto GotoReturn; }
                    if (string.IsNullOrEmpty(Path.GetExtension(fileName)))
                        fileName = $"{fileName}{Path.GetExtension(fileUpload.FileName)}";
                    FileHelper.SaveImage(pathFile, fileName, fileUpload, generateThumnail);
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                goto GotoReturn;
            }

        GotoReturn:
            return err;
        }
    }
}