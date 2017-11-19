using LottoStatisticsAnalyzer.Domain;
using System;
using System.Collections.Generic;

namespace LottoStatisticsAnalyzer
{
    public class Lot
    {
        public Lot(DateTime date, List<Drop> drops)
        {
            Date = date;
            Drops = drops;
        }
        public DateTime Date { get; private set; }
        public List<Drop> Drops { get; private set; }
    }
}