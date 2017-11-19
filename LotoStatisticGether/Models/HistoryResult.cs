using System;

namespace LotoStatisticGether.Models
{
    [Serializable]
    public class HistoryResult
    {
        public HistoryResult() { }

        public int[] Lot { get; set; }
        
        public DateTime Date { get; set; }
    }
}
