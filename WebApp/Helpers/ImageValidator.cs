namespace WebApp.Helpers
{
    public class ImageValidator
    {
        private static readonly string[] AllowedExtensions =
         {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp",
            ".gif",
            ".svg",
            ".avif"
        };
        private const long MaxSize = 512 * 1024; // 512 KB

        // 🔥 Основной метод валидации
        public static bool IsValidImage(IFormFile file, out string error)
        {
            error = string.Empty;

            if (file == null || file.Length == 0)
            {
                error = "Файл отсутствует.";
                return false;
            }

            // 1. Размер
            if (file.Length > MaxSize)
            {
                error = "Размер файла превышает 512 KB.";
                return false;
            }

            // 2. Расширение
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedExtensions.Contains(extension))
            {
                error = "Недопустимое расширение файла.";
                return false;
            }

            // 3. MIME
            if (!file.ContentType.StartsWith("image/"))
            {
                error = "Недопустимый MIME-тип файла.";
                return false;
            }

            // 4. Сигнатуры (magic numbers)
            using (var binaryReader = new BinaryReader(file.OpenReadStream()))
            {
                byte[] header = binaryReader.ReadBytes(8);

                bool isJpg = header.Take(3).SequenceEqual(new byte[] { 0xFF, 0xD8, 0xFF });
                bool isPng = header.Take(4).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47 });

                if (!isJpg && !isPng)
                {
                    error = "Файл не является корректным изображением.";
                    return false;
                }
            }

            return true;
        }
    }
}
