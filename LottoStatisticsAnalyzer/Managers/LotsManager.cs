using System.Collections.Generic;
using Loto;
using LottoStatisticsAnalyzer.Domain;
using System.Linq;
using LotoStatisticGether.Models;
using System;

namespace LottoStatisticsAnalyzer.Managers
{
    public class LotsManager
    {
        private HistoryHelper _historyHelper;

        public LotsManager()
        {
            _historyHelper = new HistoryHelper();
        }

        public List<Lot> GetAllLots(int index)
        {
            var historyResults = _historyHelper
                .GetHistoryResults()
                .OrderByDescending(hist => hist.Date)
                .ToList();

            var result = new List<Lot>();
            var length = historyResults.Count;
            for (int i = 0; i < length; i++)
            {
                double diffInPersents = 0;
                if (length - i > 3)
                {
                    diffInPersents = GetDifference(index, historyResults[i], historyResults[i + 1], historyResults[i + 2]);
                }

                result.Add(new Lot(
                    historyResults[i].Date,
                    historyResults[i].Balls
                    .Select(drop => new Drop { Value = drop, Diff = diffInPersents })
                    .ToList()));
            }
            return result;
        }

        private double GetDifference(int position, HistoryResult current, HistoryResult previous, HistoryResult last)
        {
            var currentValue = current.Balls[position];
            var previousValue = previous.Balls[position];
            var lastValue = last.Balls[position];

            double curentMinusPrevious = Math.Abs(currentValue - previousValue);
            double previousMinusLast = Math.Abs(previousValue - lastValue);

            if (curentMinusPrevious == 0 || previousMinusLast == 0)
            {
                return 0;
            }

            var result = curentMinusPrevious / previousMinusLast;

            return curentMinusPrevious;
        }
    }
}