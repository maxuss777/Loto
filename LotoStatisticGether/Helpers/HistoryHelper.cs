using LotoStatisticGether.Models;
using System.IO;
using ServiceStack.Text;
using System.Collections.Generic;
using ServiceStack;
using System;
using System.Linq;

namespace Loto
{
    public class HistoryHelper
    {
        public void Log(List<HistoryResult> result)
        {
            string resultFolderPath = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\Result";
            if (!System.IO.Directory.Exists(resultFolderPath))
            {
                System.IO.Directory.CreateDirectory(resultFolderPath);
            }

            var file = new FileInfo(resultFolderPath + "\\" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt");
            using (var writer = new StreamWriter(file.FullName))
            {
                writer.WriteLine("[");
                foreach (var res in result)
                {
                    writer.WriteLine(res.ToJson()+",");
                }
                writer.WriteLine("]");
            }
        }

        public List<HistoryResult> GetHistoryResults()
        {
            var report = System.IO.Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory.ParentDirectory() + "\\Result").First();
            var history = File.ReadAllText(report);

            return JsonSerializer.DeserializeFromString<List<HistoryResult>>(history).Where(e => e != null).ToList();
        }
    }
}