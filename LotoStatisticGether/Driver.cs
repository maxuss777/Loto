using System;
using System.Collections.ObjectModel;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Loto
{
    /// <summary>
    ///     WebDriver Adaptor by D. Yankova
    /// </summary>
    public class DriverAdaptor
    {
        /// <summary>
        ///     Default time to wait using in WebDriverWait instance
        /// </summary>
        private readonly TimeSpan _defaultTimeToWait = TimeSpan.FromSeconds(10);

        /// <summary>
        ///     IWebDriver
        /// </summary>
        private IWebDriver _webDriver;

        /// <summary>
        ///     Singleton
        /// </summary>
        private static readonly DriverAdaptor Driver = new DriverAdaptor();

        /// <summary>
        ///     Return DriverAdaptor Instance
        /// </summary>
        public static DriverAdaptor Instance
        {
            get
            {
                return Driver;
            }
        }

        /// <summary>
        ///     Return new WebDriverWait instance
        /// </summary>
        public WebDriverWait Wait
        {
            get
            {
                return new WebDriverWait(_webDriver, _defaultTimeToWait);
            }
        }

        /// <summary>
        ///     Return new Actions instance
        /// </summary>
        public Actions Actions
        {
            get
            {
                return new Actions(_webDriver);
            }
        }

        /// <summary>
        ///     Return  IAlert interface
        /// </summary>
        private IAlert Alert
        {
            get
            {
                return _webDriver.SwitchTo()
                                .Alert();
            }
        }

        /// <summary>
        ///     Returns IWebDriver as IJavaScriptExecutor
        /// </summary>
        public IJavaScriptExecutor JavaScriptExecutor
        {
            get
            {
                return _webDriver as IJavaScriptExecutor;
            }
        }

        /// <summary>
        ///     Configurate Driver Adaptor
        /// </summary>
        /// <param name="webDriver">IWebdriver type object</param>
        public static void Configurate(IWebDriver webDriver)
        {
            Instance._webDriver = webDriver;
        }

        /// <summary>
        ///     Returns IWebDriver as ITakesScreenshot
        /// </summary>
        public ITakesScreenshot ScreenshotTaker
        {
            get
            {
                return _webDriver as ITakesScreenshot;
            }
        }

        /// <summary>
        ///     Get page url
        /// </summary>
        public string PageUrl
        {
            get
            {
                return _webDriver.Url;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether or not finded element is disabled
        /// </summary>
        /// <param name="by">By by</param>
        /// <returns>True if element is disabled,False if element is enabled </returns>
        public bool IsElementDisabled(By by)
        {
            try
            {
                var element = Instance.FindElementWithWait(by);

                if (element != null)
                {
                    return element.Enabled == false;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return false;
        }

        /// <summary>
        ///     Returns cookie by name
        /// </summary>
        /// <param name="cookieName">string name of cookie</param>
        /// <returns>Cookie value</returns>
        public Cookie GetCookieNamed(string cookieName)
        {
            return _webDriver.Manage()
                            .Cookies.GetCookieNamed(cookieName);
        }

        /// <summary>
        ///     Return True if all cookies count equals 0, False if cookies count not equals 0
        /// </summary>
        public bool IsCookiesEmpty
        {
            get
            {
                return _webDriver.Manage()
                                .Cookies.AllCookies.Count == 0;
            }
        }

        /// <summary>
        ///     Find element
        /// </summary>
        /// <param name="by">By by</param>
        /// <returns>IWebElement</returns>
        public IWebElement FindElement(By by)
        {
            IWebElement element = null;
            try
            {
                element = _webDriver.FindElement(by);
            }
            catch (Exception exception)
            {
                Console.Out.WriteLine("FindElement exception {0}", exception.Message);
            }

            return element;
        }

        /// <summary>
        ///     Find element with wait
        /// </summary>
        /// <param name="by">By by</param>
        /// <returns>IWebElement</returns>
        public IWebElement FindElementWithWait(By by)
        {
            IWebElement element = null;
            try
            {
                for (var i = 1; i < 4; i++)
                {
                    element = Driver.Wait.Until(
                        dr => dr.FindElement(by));

                    if (element != null)
                    {
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.Out.WriteLine("FindElementWithWait exception {0}", exception.Message);
            }

            return element;
        }

        /// <summary>
        ///     Find non-visible element
        /// </summary>
        /// <param name="by">By by</param>
        /// <returns>IWebElement</returns>
        public IWebElement FindNonVisibleElement(By by)
        {
            IWebElement element = null;
            try
            {
                for (var i = 1; i < 4; i++)
                {
                    var exist = Driver.Wait.Until(
                        dr =>
                        {
                            element = dr.FindElement(by);
                            return element.Displayed && element.Enabled;
                        });

                    if (exist)
                    {
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return element;
        }

        /// <summary>
        ///     Click on element
        /// </summary>
        /// <param name="by">By by</param>
        public void ClickOnElement(By by)
        {
            try
            {
                var element = FindElementWithWait(by);

                if (element != null)
                {
                    element.Click();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        ///     Click on non-visible elemet
        /// </summary>
        /// <param name="by">By by</param>
        public void ClickOnNonVisibleElement(By by)
        {
            try
            {
                var element = Instance.FindNonVisibleElement(by);

                if (element != null)
                {
                    element.Click();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        ///     Click on potentialy stale element
        /// </summary>
        /// <param name="by">By</param>
        public void ClickOnPotentialyStaleElement(By by)
        {
            try
            {
                Instance.FindElementWithWait(by)
                        .Click();
            }
            catch (StaleElementReferenceException)
            {
                Console.Out.WriteLine("Catch StaleElementReferenceException");
                ClickOnPotentialyStaleElement(by);
            }
        }

        /// <summary>
        ///     Navigate to url by string
        /// </summary>
        /// <param name="url">string url</param>
        public void GoToUrl(string url)
        {
            try
            {
                _webDriver.Navigate()
                         .GoToUrl(url);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        ///     Quit webdriver
        /// </summary>
        public void Quit()
        {
            try
            {
                _webDriver.Quit();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        ///     Send string text keys to web element
        /// </summary>
        /// <param name="by">By by</param>
        /// <param name="text">string text</param>
        public void SendKeysToElement(By by, string text)
        {
            try
            {
                var element = Instance.FindElementWithWait(by);

                if (element == null)
                {
                    return;
                }
                element.Clear();
                element.SendKeys(text);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        ///     Refresh page
        /// </summary>
        public void RefreshPage()
        {
            try
            {
                _webDriver.Navigate()
                         .Refresh();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        ///     Clear element
        /// </summary>
        /// <param name="by">By</param>
        public void ClearElement(By by)
        {
            try
            {
                var element = Instance.FindElementWithWait(by);

                if (element != null)
                {
                    element.Clear();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        ///     Clear element using Backspace keyboard key
        /// </summary>
        /// <param name="by">By</param>
        public void ClearElementWithBackspaceKey(By by)
        {
            try
            {
                var element = Instance.FindElementWithWait(by);

                if (element == null)
                {
                    return;
                }
                do
                {
                    Instance.Actions.MoveToElement(element)
                            .SendKeys(Keys.Backspace)
                            .Build()
                            .Perform();
                }
                while (element.GetAttribute("value") != String.Empty);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        ///     Delete all cookies for domain
        /// </summary>
        /// <param name="domain">String domain name</param>
        public void DeleteCookiesForDomain(string domain)
        {
            try
            {
                Instance.GoToUrl(domain);

                if (Instance.IsCookiesEmpty)
                {
                    return;
                }

                _webDriver.Manage()
                         .Cookies.DeleteAllCookies();

                Instance.RefreshPage();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        ///     Send string text keys to web element using Actions class
        /// </summary>
        /// <param name="by">By by</param>
        /// <param name="text">string text</param>
        public void SendKeysToElementUsingActions(By by, string text)
        {
            var charArray = text.ToCharArray();
            try
            {
                var element = Instance.FindElementWithWait(by);
                if (element != null)
                {
                    Instance.Actions.Click(element)
                            .Perform();
                }

                foreach (var i in charArray)
                {
                    Instance.Actions.SendKeys(i.ToString(CultureInfo.InvariantCulture))
                            .Perform();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        ///     Wait until an element is no longer attached to the DOM
        /// </summary>
        /// <param name="by">By by</param>
        public void WaitForStalenessOfElement(By by)
        {
            Instance.Wait.Until(
                dr =>
                {
                    try
                    {
                        if (dr.FindElement(by)
                              .Enabled)
                        {
                            return false;
                        }
                    }
                    catch (StaleElementReferenceException)
                    {
                        return true;
                    }
                    catch (NoSuchElementException)
                    {
                        return true;
                    }

                    return false;
                });
        }

        /// <summary>
        ///     Find elements
        /// </summary>
        /// <param name="by">By by</param>
        /// <returns>ReadOnlyCollection of IWebElements</returns>
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            ReadOnlyCollection<IWebElement> elements = null;
            try
            {
                for (var i = 1; i < 4; i++)
                {
                    elements = Driver.Wait.Until(
                        dr => dr.FindElements(by));

                    if (elements != null)
                    {
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return elements;
        }

        /// <summary>
        ///     Accepts a alert
        /// </summary>
        public void AcceptAlert()
        {
            try
            {
                Alert.Accept();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        ///     Returns Text property or Value of found element
        /// </summary>
        /// <param name="by">By by</param>
        /// <returns>Value of Text or Value attribute of element</returns>
        public String TextOfElement(By by)
        {
            string text = null;

            var element = Instance.FindNonVisibleElement(by);

            try
            {
                if (element != null)
                {
                    if (element.GetCssValue("display") != "none")
                    {
                        text = element.Text;

                        if (text.Trim() == String.Empty)
                        {
                            text = element.GetAttribute("value");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return text;
        }

        /// <summary>
        ///     Find select element
        /// </summary>
        /// <param name="by">By by</param>
        /// <returns>returns SelectElement intance</returns>
        public SelectElement FindSelectElement(By by)
        {
            try
            {
                var element = Instance.FindElementWithWait(by);

                if (element != null)
                {
                    return new SelectElement(element);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return null;
        }

        /// <summary>
        ///     Switch the focus to the  window with given name
        /// </summary>
        /// <param name="windowName">string window name </param>
        public void SwitchToWindow(string windowName)
        {
            try
            {
                _webDriver.SwitchTo()
                         .Window(windowName);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        ///     Gets the window handels of browser window
        /// </summary>
        public ReadOnlyCollection<string> WindowHandles
        {
            get
            {
                return _webDriver.WindowHandles;
            }
        }

        /// <summary>
        ///     Wait until number of handles windows equals expected number
        /// </summary>
        /// <param name="numberOfWindows">int expected number of handles windows </param>
        public void WaitForNumberOfWindowsEquals(int numberOfWindows)
        {
            try
            {
                Instance.Wait.Until(
                    dr => dr.WindowHandles.Count == numberOfWindows);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}