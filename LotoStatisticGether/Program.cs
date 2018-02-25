using System.Collections.Generic;
using OpenQA.Selenium;
using LotoStatisticGether.Models;
using ServiceStack;
using System;
using System.Linq;

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
                DriverAdaptor.Instance.GoToUrl("http://msl.ua/uk/megalot/archive");
                //DriverAdaptor.Instance.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//button[@class='yes']"))).Click();
                GetSattistic();
            }
            finally
            {
                _historyHelper.Log(_results);
            }
        }

        private static void GetSattistic()
        {
            bool isEnough = false;
            for (; ; )
            {
                for (var j = 2; j < 12; j++)
                {
                    var pathToLot = $"body > section > div > div > div:nth-child({j}) > div.span10 > div:nth-child(2)";
                    var pathToDate = $"body > section > div > div > div:nth-child({j}) > div.span2";
                    IWebElement webElementLot = null;
                    IWebElement webElementDate = null;
                    try
                    {
                        webElementLot = DriverAdaptor.Instance.FindElementWithWait(By.CssSelector(pathToLot));
                        webElementDate = DriverAdaptor.Instance.FindElementWithWait(By.CssSelector(pathToDate));

                        var lot = webElementLot
                            .Text
                            .Replace(" + мегакулька: ", ",")
                            .Replace("Результати ", string.Empty)
                            .Split(',')
                            .Select<string, int>(i=>Int32.Parse(i))
                            .ToArray();
                        var date = GetDate(webElementDate.Text);

                        _results.Add(new HistoryResult { Lot = lot, Date = date });
                    }
                    catch (Exception exc)
                    {
                        Console.Out.Write(exc.Message);
                        isEnough = true;
                        break;
                    }
                }
                if (isEnough)
                {
                    break;
                }

                DriverAdaptor.Instance.FindElement(By.CssSelector("#yw0 > li.next > a")).Click();
            }
        }

        private static DateTime GetDate(string date)
        {
            var d = date.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var monthName = d[1].Trim();
            var day = d[0];
            var year = d[2];

            var monthInt = ((int)Enum.Parse(typeof(Months), monthName)).ToString();

            return DateTime.Parse($"{year}-{monthInt}-{day}");
        }
    }
}