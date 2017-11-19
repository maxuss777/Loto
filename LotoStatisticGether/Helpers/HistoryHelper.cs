using LotoStatisticGether.Models;
using System.Configuration;
using System.IO;
using ServiceStack.Text;
using System.Collections.Generic;

namespace Loto
{
    public class HistoryHelper
    {
        private string _resultFilePath;
        public HistoryHelper()
        {
            _resultFilePath = @"C:\source\repos\Loto\LotoStatisticGether\Results\lots.txt";
        }

        public void Log(string info)
        {
            var file = new FileInfo(_resultFilePath);
            file.Directory.Create();
            File.WriteAllText(file.FullName, info);
        }

        public List<HistoryResult> GetHistoryResults()
        {
            var history = File.ReadAllText(_resultFilePath);
            return JsonSerializer.DeserializeFromString<List<HistoryResult>>(history);
        }
    }
}