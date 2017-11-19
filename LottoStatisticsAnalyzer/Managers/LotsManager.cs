using System.Collections.Generic;
using Loto;
using LotoStatisticGether.Models;
using System.Linq;

namespace LottoStatisticsAnalyzer.Managers
{
    public class LotsManager
    {
        private HistoryHelper _historyHelper;

        public LotsManager()
        {
            _historyHelper = new HistoryHelper();
        }

        public List<Lot> GetAllLots()
        {
            var historyResults = _historyHelper.GetHistoryResults();

            return historyResults.Select(historyResult =>
            {
                return new Lot(
                    historyResult.Date,
                    historyResult.Lot
                    .Select(drop => new Domain.Drop { Value = drop })
                    .ToList());
            }).ToList();
        }
    }
}
