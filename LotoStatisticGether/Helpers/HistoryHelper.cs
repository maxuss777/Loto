using LotoStatisticGether.Models;
using System.IO;
using ServiceStack.Text;
using System.Collections.Generic;
using ServiceStack;
using System;

namespace Loto
{
    public class HistoryHelper
    {
        private string _resultFilePath1;
        private string _resultFilePath2;
        public HistoryHelper()
        {
            var date = DateTime.Now.ToString("yyyy_MM_dd");
            _resultFilePath1 = System.AppDomain.CurrentDomain.RelativeSearchPath + $"\\Results\\lots_{date}.txt";
            _resultFilePath2 = System.AppDomain.CurrentDomain.RelativeSearchPath + $"\\Results\\lotsAsArray_{date}.txt";
        }

        public void Log(List<HistoryResult> result)
        {
            var file1 = new FileInfo(_resultFilePath1);
            file1.Directory.Create();
            File.WriteAllText(file1.FullName, result.ToJson());

            var file2 = new FileInfo(_resultFilePath2);
            file2.Directory.Create();
            using (var writer = new StreamWriter(file2.FullName))
            {
                foreach(var res in result)
                {
                    writer.WriteLine(res.Balls.ToJson());
                }
            }
        }

        public List<HistoryResult> GetHistoryResults()
        {
            var history = File.ReadAllText(_resultFilePath1);

            return JsonSerializer.DeserializeFromString<List<HistoryResult>>(history);
        }
    }
}