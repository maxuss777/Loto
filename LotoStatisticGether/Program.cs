using System.Collections.Generic;
using OpenQA.Selenium;
using LotoStatisticGether.Models;
using ServiceStack;
using System;

namespace Loto
{
    public class Program
    {
        private static List<Result> _results = new List<Result>();

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
                LogsHelper.Log(_results.ToJson());
            }
        }

        private static void GetSattistic()
        {
            bool isEnough = false;
            for (; ; )
            {
                for (var j = 1; j < 11; j++)
                {
                    var pathToLot = string.Format(".//div[@class='ml-content']/div[{0}]/div[@class='span10']/div[1]", j);
                    var pathToDate = "body > section > div > div > div:nth-child(2) > div.span2";
                    IWebElement webElementLot = null;
                    IWebElement webElementDate = null;
                    try
                    {
                        webElementLot = DriverAdaptor.Instance.FindElementWithWait(By.XPath(pathToLot));
                        webElementDate = DriverAdaptor.Instance.FindElementWithWait(By.CssSelector(pathToDate));

                        var lot = webElementLot
                            .Text
                            .Replace(" + мегакулька: ", ",")
                            .Replace("Результати ", string.Empty)
                            .Split(',');
                        var dateTime = webElementDate.Text.Replace(" + мегакулька: ", ",").Replace("Результати ", string.Empty);

                        _results.Add(new Result(lot, dateTime));
                    }
                    catch (Exception)
                    {
                        isEnough = true;
                        break;
                    }
                }
                if (isEnough)
                {
                    break;
                }

                DriverAdaptor.Instance.FindElement(By.XPath(".//li[@class='next']")).Click();
            }
        }
    }
}