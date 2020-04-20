using System;
using System.Collections.Generic;

namespace LotoStatisticGether.Models
{
    [Serializable]
    public class HistoryResult
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public IReadOnlyList<int> Balls { get; set; }
    }
}
