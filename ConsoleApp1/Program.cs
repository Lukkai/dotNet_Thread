using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Net;
using System.Threading;

namespace ConsoleApp1
{
    public class Coord
    {
        public float lon { set; get; }
        public float lat { set; get; }
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
        public Coord coord { set; get; }
        public MainWeather main { set; get; }
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

        public void ShowWeather()
        {
            Console.WriteLine($"Cities: {name}");
            Console.WriteLine("Latitude: " + coord.lat);
            Console.WriteLine("Temperature :" + main.temp);
            Console.WriteLine("Feels like :" + main.feels_like);
            Console.WriteLine($"Wind speed: {wind.speed}, wind degree direction: {wind.deg}");
        }
    }

    public class WeatherBase : DbContext
    {
        public virtual DbSet<CityWeather> CitiesWeather { get; set; }

        public CityWeather AddDataToBase(string cityName)
        {
            var cityWeather = new CityWeather();
            cityWeather.name = cityName;
            cityWeather = JsonConvert.DeserializeObject<CityWeather>(cityWeather.GetWeather());
            return cityWeather;
        }

        public void ShowDataBaseContent()
        {
            var citiesWeather = (from a in this.CitiesWeather select a).ToList<CityWeather>();
            foreach (var cw in citiesWeather)
            {
                cw.ShowWeather();
                Console.WriteLine("");
            }
        }

        public void ClearBase()
        {
            CitiesWeather.RemoveRange(CitiesWeather);
            this.SaveChanges();
        }

        public void RemoveLast()
        {
            var cs = CitiesWeather.First<CityWeather>();
            CitiesWeather.Remove(cs);
            this.SaveChanges();
        }

        public void FindByTemp(int searched_temp)
        {
            //var query = (from city in this.CitiesWeather
            //             where city.main.temp == searched_temp
            //             select city).ToList<CityWeather>();
            var foundcities = this.CitiesWeather.Where(city => city.main.temp >= searched_temp).ToList<CityWeather>();
            foreach (var cw in foundcities)
            {
                cw.ShowWeather();
                Console.WriteLine("");
            }
        }

        public void FindByWindSpeed(int searched_wind_speed)
        {
            var foundcities = this.CitiesWeather.Where(city => city.wind.speed >= searched_wind_speed).ToList<CityWeather>();
            foreach (var cw in foundcities)
            {
                cw.ShowWeather();
                Console.WriteLine("");
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {

            /*
             * 
    do 
    {
    line = input.ReadLine();

    if (line == null)
    {
    return;
    }

    if (line == String.Empty)
    {
    continue;
    }

    // Here you process the non-empty line

    } while (true);
            */

            var cityWeatherBase = new WeatherBase();
            //int key;
            //string line;

            // cityWeatherBase.CitiesWeather.Add(cityWeatherBase.AddDataToBase("Olawa"));
            // cityWeatherBase.SaveChanges();
            // cityWeatherBase.ShowDataBaseContent();


            
            do
            {
                Console.Clear();
                Console.WriteLine("1. Add new city");
                Console.WriteLine("2. Clear whole database");
                Console.WriteLine("3. Show whole content");
                Console.WriteLine("4. Remove last added element");
                Console.WriteLine("5. Find elements by temperature");
                Console.WriteLine("6. Find elements by wind speed");
                Console.WriteLine("7. Menu");
                Console.WriteLine("8. Exit");
                Console.WriteLine("Chose one option");

            } while (MainMenu(cityWeatherBase));
            //while (MainMenu((int)Convert.ToDecimal(Console.ReadLine()), cityWeatherBase));
            //{
            //    Console.Clear();
            //    MainMenu(7, cityWeatherBase);
            //}
        }
        static bool MainMenu(WeatherBase cityWeatherBase)
        {
            
            switch (Console.ReadLine())
            {
                case "0":
                    return true;
                case "1":
                    Console.WriteLine("Insert name of the city:");
                    cityWeatherBase.CitiesWeather.Add(cityWeatherBase.AddDataToBase(Console.ReadLine()));
                    cityWeatherBase.SaveChanges();
                    return true;
                case "2":
                    cityWeatherBase.ClearBase();
                    Console.WriteLine("Base Cleared");
                    Console.ReadLine();
                    return true;
                case "3":
                    cityWeatherBase.ShowDataBaseContent();
                    Console.ReadLine();
                    return true;
                case "7":
                    Console.Clear();
                    Console.WriteLine("1. Add new city");
                    Console.WriteLine("2. Clear whole database");
                    Console.WriteLine("3. Show whole content");
                    Console.WriteLine("4. Remove last added element");
                    Console.WriteLine("5. Find elements by temperature");
                    Console.WriteLine("6. Find elements by wind speed");
                    Console.WriteLine("7. Menu");
                    Console.WriteLine("8. Exit");
                    Console.WriteLine("Chose one option");
                    return true;

                case "4":
                    cityWeatherBase.RemoveLast();
                    Console.WriteLine("Element removed");
                    Console.ReadLine();
                    return true;

                case "5":
                    Console.WriteLine("Insert the temperature: ");
                    cityWeatherBase.FindByTemp(Convert.ToInt32(Console.ReadLine()));
                    
                    Console.WriteLine("Press any key to return");
                    Console.ReadLine();
                    return true;

                case "6":
                    Console.WriteLine("Insert the wind speed: ");
                    cityWeatherBase.FindByWindSpeed(Convert.ToInt32(Console.ReadLine()));
                    Console.WriteLine("Press any key to return");
                    Console.ReadLine();
                    return true;

                case "8":
                    return false;

                default:
                    Console.WriteLine("Error: option not found");
                    Console.WriteLine("\nPress any key to return");
                    Console.ReadLine();
                    return true;
            }


        }

    }
}



//cityWeatherBase.CitiesWeather.RemoveRange(cityWeatherBase.CitiesWeather); //truncates whole table
//cityWeatherBase.SaveChanges();