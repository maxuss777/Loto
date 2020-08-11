using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LotoStatisticGether.v2
{
    public class Program
    {
        private static readonly HttpClient _client = new HttpClient();

        static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://igra.msl.ua//megalote/uk/slide-aside");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "megalote/uk/slide-aside");
            request.Content = new StringContent("YII_CSRF_TOKEN=NXpaWWJaaFdvWVIzTER6VTB4OEE0OXYwYnVoTHRGOEjVgpptoSFSsCuOt5V64Pqw3hzflGDfg1sN76A6MN-V6w%3D%3D&drawID=1975",
                                                Encoding.UTF8,
                                                "application/x-www-form-urlencoded");

            
            request.Headers.Host = "igra.msl.ua";
            request.Headers.Add("Cookie", "_gcl_au=1.1.1561254622.1595959461; _ga=GA1.2.1571770125.1595959461; registrationPopup=close; is18=true; GN_USER_ID_KEY=c88e6455-854d-40f4-bca9-92debbdba6d0; loyalUser=null; tutorialMillionaire=seen; sid=15p5bdg3ken05jsd8b3g8f31m4; lotteryCode=000; _gid=GA1.2.1945938776.1597168178; GN_SESSION_ID_KEY=b3c192b3-3634-4499-a6c0-e17fbb4d87d1; gravitecOptInBlocked=true");

            HttpResponseMessage response = await client.SendAsync(request);
        }
    }
}
