namespace ZipFileExplorer
{
    public class FileHelper
    {
        public static bool IsPlainTextFile(Stream stream, bool disposeAfterUsed = true)
        {
            var reader = new StreamReader(stream);

            var buffer = new char[4096];
            var bytesRead = reader.Read(buffer, 0, buffer.Length);

            bool isPlianText = true;

            for (int i = 0; i < bytesRead; i++)
            {
                if (buffer[i] == '\0')
                {
                    return false;
                }

                if (char.IsControl(buffer[i]) &&
                    buffer[i] != '\r' &&
                    buffer[i] != '\n' &&
                    buffer[i] != '\t')
                {
                    isPlianText = false;

                    break;
                }
            }

            if (disposeAfterUsed)
            {
                reader.Close();
                reader.Dispose();
                stream.Dispose();
            }
            else
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            return isPlianText;
        }

        public static readonly Dictionary<string, string> MimeMappings = new Dictionary<string, string>()
        {
            {"png", "image/png" },
            {"jpg", "image/jpeg"},
            {"jpeg", "image/jpeg"},
            {"gif", "image/gif"},
            {"svg", "image/svg+xml"},
            {"bmp", "image/bmp"},
            {"tiff", "image/tiff"},
            {"tif", "image/tiff"},
            {"emf", "image/x-emf"},
            {"wmf", "image/x-wmf"},
            {"webp", "image/webp"},
            {"mp4", "video/mp4"},
            {"m4v", "video/mp4"},
            {"webm", "video/webm"},
            {"avi", "video/x-msvideo"},
            {"mp3", "audio/mpeg"},
            {"wav", "audio/wav"},
            {"m4a", "audio/mp4"},
            {"ogg", "audio/ogg"},
        };

        public static string GetMimeType(string type, string fileType)
        {
            if (MimeMappings.ContainsKey(fileType))
            {
                return MimeMappings[fileType];
            }
            else
            {
                switch (type)
                {
                    case "image":
                        return "image/png";
                        break;
                    case "video":
                        return "video/mp4";
                        break;
                    case "audio":
                        return "audio/mpeg";
                        break;
                }
            }

            return string.Empty;
        }

        public static bool IsXmlFile(string extension)
        {
            string[] xmlExtensions = new string[] { ".xml", ".rels", ".sln", ".slnx", ".csproj", ".resx" };

            return xmlExtensions.Contains(extension);
        }

        public static bool IsImageFile(string extension)
        {
            string[] imageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".emf", ".wdp", ".tiff" };
            
            return imageExtensions.Contains(extension);
        }

        public static bool IsAudioFile(string extension)
        {
            string[] audioExtensions = new string[] { ".mp3", ".wav", ".ogg", ".flac", ".aac" };
            
            return audioExtensions.Contains(extension);
        }

        public static bool IsVideoFile(string extension)
        {
            string[] videoExtensions = new string[] { ".mp4", ".avi", ".wmv", ".flv", ".mkv", ".mov", ".mpg", ".vob" };
            
            return videoExtensions.Contains(extension);
        }
      

        public static string GetBase64StringFromByteArray(byte[] bytes, string type, string fileType)
        {
            if (bytes != null)
            {
                string str = Convert.ToBase64String(bytes);

                string mimeType = GetMimeType(type, fileType);

                return $"data:{mimeType};base64,{str}";
            }

            return null;
        }
    }
}
