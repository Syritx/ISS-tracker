using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace ISStracker {

    class Program {

        static HttpClient client = new HttpClient();
        static IWebDriver driver;

        static async Task Main() {

            driver = new FirefoxDriver();

            while (true) {

                string data = await GetISSPosition();
                JsonDocument doc = JsonDocument.Parse(data);
                JsonElement root = doc.RootElement.GetProperty("iss_position");
                JsonElement lat = root.GetProperty("latitude"), lon = root.GetProperty("longitude");

                float lat_f = float.Parse(lat.ToString());
                float lon_f = float.Parse(lon.ToString());

                string url = "https://www.google.com/maps/@"+lat_f+","+lon_f+",11z";
                Console.WriteLine(url);
                driver.Url = url;

                Thread.Sleep(3000);
            }
        }


        static async Task<string> GetISSPosition() {

            HttpResponseMessage responseMessage = await client.GetAsync("http://api.open-notify.org/iss-now.json");
            string r = await responseMessage.Content.ReadAsStringAsync();
            return r;
        }
    }
}
