using OpenQA.Selenium.Chrome;


namespace Loto
{
    public static class Init
    {
        public static DriverAdaptor InitiateDriver()
        {
            var options = new ChromeOptions();
            options.AddArguments(new[]
            {
                "start-maximized",
                "--disable-save-password-bubble"
            });
            DriverAdaptor.Configurate(new ChromeDriver(options));

            return DriverAdaptor.Instance;
        }
    }
}
