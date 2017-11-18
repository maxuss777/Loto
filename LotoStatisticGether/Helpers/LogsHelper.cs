using System.Configuration;
using System.IO;

namespace Loto
{
    public static class LogsHelper
    {
        private static string _resultFilePath;
        static LogsHelper()
        {
            _resultFilePath = ConfigurationManager.AppSettings["resultFile"];
        }

        public static void Log(string info)
        {
            var file = new FileInfo(_resultFilePath);
            file.Directory.Create();
            File.WriteAllText(file.FullName, info);
        }
    }
}