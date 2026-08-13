namespace ZipFileExplorer.Helper
{
    public class CultureInfoHelper
    {
        public static bool IsZhCN()
        {
            var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;

            if (cultureInfo.Name == "zh-CN")
            {
                return true;
            }

            return false;
        }
    }
}
