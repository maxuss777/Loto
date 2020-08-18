using HtmlAgilityPack;
using Loto;
using LotoStatisticGether.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LotoStatisticGether.v2
{
    public class Program
    {
        static void Main()
        {
            var sid = "ojfi8200ej2arhmi8l49k35vp1";
            var token = "VHpKfmNPNk1iSFlCY34wVzk5aGt3YVRvRlM4Q0llXzhBSGY6aR62_V2MEzP3dcUncqZe7YOqIX2WSh7Nhk2iuw==";

            var client = new RestClient("https://igra.msl.ua/megalote/uk/slide-aside");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            request.AddParameter("registrationPopup", "close", ParameterType.Cookie);
            request.AddParameter("is18", "true", ParameterType.Cookie);
            request.AddParameter("tutorialMillionaire", "seen", ParameterType.Cookie);
            request.AddParameter("lotteryCode", "000", ParameterType.Cookie);
            request.AddParameter("gravitecOptInBlocked", "true", ParameterType.Cookie);

            request.AddParameter("sid", sid, ParameterType.Cookie);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            int start = 1316;
            int end = 1979;

            List<HistoryResult> result = new List<HistoryResult>();
            for (int i = start; i <= end; i++)
            {
                request.AddOrUpdateParameter("application/x-www-form-urlencoded; charset=UTF-8", $"YII_CSRF_TOKEN={token}&drawID={i}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string source = WebUtility.HtmlDecode(response.Content);
                HtmlDocument html = new HtmlDocument();
                html.LoadHtml(source);

                HtmlNode date = html
                    .DocumentNode
                    .Descendants()
                    .Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("results_slider"))
                    .First();

                List<int> balls = html.DocumentNode.Descendants()
                    .Where(x => x.Name == "span" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("ball-number"))
                    .Select(x => int.Parse(x.InnerHtml)).ToList();

                result.Add(new HistoryResult { Id = i, Date = GetDate(date), Balls = balls });

                Console.Clear();
                Console.WriteLine(result.Count);
            }

            new HistoryHelper().Log(result);
        }

        private static string GetDate(HtmlNode date)
        {
            var webValue = date.InnerText.Replace('\n', ' ').Trim();
            var d = webValue.Substring(3).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var monthName = d[1].Trim();
            var day = d[0].Trim();
            var year = d[2];

            var monthInt = ((int)Enum.Parse(typeof(Months), monthName)).ToString();

            return $"{year}-{monthInt}-{day}";
        }
    }
}