namespace WebApp.Helpers
{
    public class FileService
    {
        public static void SaveImage(IFormFile file, string fullFilePath)
        {
            string? directory = Path.GetDirectoryName(fullFilePath);

            if (directory == null)
                throw new Exception("Invalid path");

            // Создаём папку если нет
            Directory.CreateDirectory(directory);

            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }

        public static void DeleteFile(string folderPath, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            string fullPath = Path.Combine(folderPath, fileName);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
