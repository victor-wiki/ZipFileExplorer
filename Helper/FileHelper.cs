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
    }
}
