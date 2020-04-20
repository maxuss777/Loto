using System.Collections.Generic;
using OpenQA.Selenium;
using LotoStatisticGether.Models;
using ServiceStack;
using System;
using System.Linq;
using OpenQA.Selenium.Support.UI;

namespace Loto
{
    public class Program
    {
        private static List<HistoryResult> _results = new List<HistoryResult>();
        private static HistoryHelper _historyHelper = new HistoryHelper();

        static void Main()
        {
            try
            {
                Init.InitiateDriver();
                DriverAdaptor.Instance.GoToUrl("https://igra.msl.ua/megalote/uk/archive");
                DriverAdaptor.Instance.Wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("body > div.modal-is18-wrapper.popup.popup-adult.is18wrap > " +
                    "div > div > div.wrapp-button > button.ripple.is18-yes-btn.yep.yes"))).Click();
                ParseHistoryResult();
            }
            finally
            {
                _historyHelper.Log(_results);
            }
        }

        private static void ParseHistoryResult()
        {
            do
            {
                var archiveResultItems = DriverAdaptor.Instance.FindElements(By.ClassName("archive_results-item"));
                for (int i = 0; i < archiveResultItems.Count; i++)
                {
                    var archiveResult = archiveResultItems[i];
                    var archiveResultDetails = archiveResult.FindElement(By.ClassName("archive_result-details"));

                    _results.Add(new HistoryResult { Id = GetId(archiveResultDetails), Date = GetDate(archiveResult), Balls = GetBalls(archiveResultDetails) });
                }

                if (IsNextButtonHidden())
                {
                    _historyHelper.Log(_results);
                    DriverAdaptor.Instance.FindElement(By.CssSelector("#yw0 > li.next > a")).Click();
                }
            }
            while (IsNextButtonHidden());
        }

        private static bool IsNextButtonHidden()
        {
            var nextButton = DriverAdaptor.Instance.FindElement(By.CssSelector("#yw0 > li.next.hidden"));

            return nextButton == null;
        }

        private static int GetId(IWebElement archiveResultDetails)
        {
            var archiveResultBalls = archiveResultDetails.FindElement(By.ClassName("archive_result-number"));
            var innerText = archiveResultBalls.Text;

            var replaced = innerText
                .Replace("Тираж", "")
                .Trim();

            return int.Parse(replaced);
        }

        private static string GetDate(IWebElement archiveResult)
        {
            var webValue = archiveResult.FindElement(By.ClassName("archive_result-date"));
            var d = webValue.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var monthName = d[1].Trim();
            var day = d[0];
            var year = d[2];

            var monthInt = ((int)Enum.Parse(typeof(Months), monthName)).ToString();

            return $"{year}-{monthInt}-{day}";
        }

        private static IReadOnlyList<int> GetBalls(IWebElement archiveResultDetails)
        {
            var archiveResultBalls = archiveResultDetails.FindElement(By.ClassName("archive_result-balls"));
            var innerText = archiveResultBalls.Text;
            var replaced = innerText
                .Replace("Результати\r\n", "")
                .Replace("+ Мегакулька\r\n", "")
                .Replace("\r\n", ",")
                .Trim()
                .Split(',')
                .Select(k => int.Parse(k))
                .ToList()
                .AsReadOnly();

            return replaced;
        }
    }
}