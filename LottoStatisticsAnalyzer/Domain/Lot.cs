using LottoStatisticsAnalyzer.Domain;
using System;
using System.Collections.Generic;

namespace LottoStatisticsAnalyzer
{
    public class Lot
    {
        public List<Drop> Drops { get; set; }

        public DateTime Date { get; set; }
    }
}