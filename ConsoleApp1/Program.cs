using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Net;

namespace ConsoleApp1
{
    public class Coord
    {
        public float lon { set; get; }
        public float lat { set; get; }
    }


    public class Weather
    {
        public int id { set; get; }
        public string tag { set; get; }
        public string description { set; get; }

    }

    public class MainWeather
    {
        public float temp { set; get; }
        public float feels_like { set; get; }
        public float temp_min { set; get; }
        public float temp_max { set; get; }
        public int pressure { set; get; }
        public int humidity { set; get; }
    }

    public class Wind
    {
        public float speed { set; get; }
        public float deg { set; get; }
    }

    public class CityWeather
    {
        public string name { set; get; }
        public int timeZone { set; get; }
        public int id { set; get; }
        public Weather[] weather { set; get; }
        public Coord coord { set; get; }
        public MainWeather mainn { set; get; }
        public Wind wind { set; get; }

        public string GetWeather()
        {
            using (WebClient webClient = new WebClient())
            {
                string call = string.Format("https://api.openweathermap.org/data/2.5/weather?q=" + name + "&appid=9e78fe82c3c763872a651f2affb0f808");                
                var json = webClient.DownloadString(call);
                return json;
            }
        }
    }

    public class WeatherBase : DbContext
    {
        public virtual DbSet<CityWeather> CitiesWeather { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            CityWeather cityWeather = new CityWeather();
            cityWeather.name = "Wroclaw";            
            Console.WriteLine(cityWeather.GetWeather());
            
            CityWeather distinctedWeatherInfo = JsonConvert.DeserializeObject<CityWeather>(cityWeather.GetWeather());
            //MACIEJ NIE DZIALAAAAA;-;-;-;-;-;-;-;
            //EDIT, POWINNO JUZ DZIALAC AAAAAAAAAA, TERAZ POTRZEBUJEMY TYLKO MIEC AKTYWOWANY KLUCZ API <3<3

            Console.WriteLine("City: " + distinctedWeatherInfo.name);
            Console.WriteLine("Latitude: " + distinctedWeatherInfo.coord.lat);
            //Console.WriteLine(distinctedWeatherInfo.weather);
            Console.WriteLine("ID: " + distinctedWeatherInfo.weather[0].id);
            Console.WriteLine("Temperature :" + distinctedWeatherInfo.mainn.temp);
            Console.WriteLine("Feels like :" + distinctedWeatherInfo.mainn.feels_like);
            Console.Read();
        }
    }
}
