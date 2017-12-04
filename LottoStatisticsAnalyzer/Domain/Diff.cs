using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoStatisticsAnalyzer.Domain
{
    public class Diff
    {

        public Diff(DateTime date, double percents)
        {
            Date = date;
            Percents = percents;
        }
        public DateTime Date { get; private set; }
        public double Percents { get; private set; }
    }
}
