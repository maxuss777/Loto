using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Loto
{
    public class Program
    {
        private static List<string> _result = new List<string>();
        static void Main()
        {
            Init.InitiateDriver();
            DriverAdaptor.Instance.GoToUrl("http://msl.ua/uk/megalot/archive");
            //DriverAdaptor.Instance.Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//button[@class='yes']"))).Click();
            GetSattistic();
            Loger.Write(_result.ToArray());
        }

        private static void GetSattistic()
        {
            bool isEnough = false;

            for (;;)
            {
                for (var j = 1; j < 11; j++)
                {
                    var row = string.Format(".//div[@class='ml-content']/div[{0}]/div[@class='span10']/div[1]", j);
                    var value = DriverAdaptor.Instance.FindElementWithWait(By.XPath(row));
                    if (value == null)
                    {
                        isEnough = true;
                        break;
                    }
                    _result.Add(value.Text.Replace(" + мегакулька: ", ",").Replace("Результати ", string.Empty));
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