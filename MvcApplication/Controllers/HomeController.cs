using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication.Models;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using ForecastIO;
using GoogleMaps.LocationServices;

namespace MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            //double lat;
            //double lon;
            //string address = "3941 NW 122nd Street, Oklahoma City, OK";
            //GetGeocode(address, out lat, out lon);
            //WeatherModel weather = GetLocalForecast((float)lat, (float)lon);

            //BuildWeatherDisplay(weather);

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            var lists = new List<HomeModel>{
                new HomeModel(){ Date = new DateTime(2015, 08, 10), Weight = 232.8 },
                new HomeModel(){ Date = new DateTime(2015, 08, 11), Weight = 232.6 },
                new HomeModel(){ Date = new DateTime(2015, 08, 12), Weight = 233 },
                new HomeModel(){ Date = new DateTime(2015, 08, 13), Weight = 232.8 },
                new HomeModel(){ Date = new DateTime(2015, 08, 14), Weight = 233.4 },
                new HomeModel(){ Date = new DateTime(2015, 08, 15), Weight = 233 },
                new HomeModel(){ Date = new DateTime(2015, 08, 16), Weight = 233 },
                new HomeModel(){ Date = new DateTime(2015, 08, 17), Weight = 232.8 },
                new HomeModel(){ Date = new DateTime(2015, 08, 18), Weight = 231.2 },
                new HomeModel(){ Date = new DateTime(2015, 08, 19), Weight = 231.8 },
                new HomeModel(){ Date = new DateTime(2015, 08, 20), Weight = 230.8 },
                new HomeModel(){ Date = new DateTime(2015, 08, 21), Weight = 231.8 },
                new HomeModel(){ Date = new DateTime(2015, 08, 22), Weight = 231 },
                new HomeModel(){ Date = new DateTime(2015, 08, 23), Weight = 230.2 },
                new HomeModel(){ Date = new DateTime(2015, 08, 24), Weight = 232.2 },
                new HomeModel(){ Date = new DateTime(2015, 08, 25), Weight = 232 },
                new HomeModel(){ Date = new DateTime(2015, 08, 26), Weight = 231.6 },
                new HomeModel(){ Date = new DateTime(2015, 08, 27), Weight = 230 },
                new HomeModel(){ Date = new DateTime(2015, 08, 28), Weight = 230 },
                new HomeModel(){ Date = new DateTime(2015, 08, 29), Weight = 231.4 },
                new HomeModel(){ Date = new DateTime(2015, 08, 30), Weight = 231.8 },
                new HomeModel(){ Date = new DateTime(2015, 08, 31), Weight = 230.6 },
                new HomeModel(){ Date = new DateTime(2015, 09, 01), Weight = 230.4 },
                new HomeModel(){ Date = new DateTime(2015, 09, 02), Weight = 229.2 },
                new HomeModel(){ Date = new DateTime(2015, 09, 03), Weight = 229.8 },
                new HomeModel(){ Date = new DateTime(2015, 09, 04), Weight = 230 },
                new HomeModel(){ Date = new DateTime(2015, 09, 05), Weight = 229.4 },
                new HomeModel(){ Date = new DateTime(2015, 09, 06), Weight = 230.2 },
                new HomeModel(){ Date = new DateTime(2015, 09, 07), Weight = 229.4 },
                new HomeModel(){ Date = new DateTime(2015, 09, 08), Weight = 228.8 }
            };

            var xDataDate = lists.Select(x => x.Date).ToArray();
            string[] xDate = xDataDate.Select(x => x.ToString("yyyy/MM/dd")).ToArray();
            var yDataWeight = lists.Select(x => new object[] { x.Weight }).ToArray();

            var chart = new Highcharts("charts");
            chart.InitChart(new Chart { DefaultSeriesType = ChartTypes.Line });
            chart.SetTitle(new Title { Text = "Weight for last month" });
            chart.SetSubtitle(new Subtitle { Text = "August 8 - September 8" });

            chart.SetXAxis(new XAxis { Categories = xDate });
            chart.SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Weight in lbs" } })
                .SetTooltip(new Tooltip
                {
                    Enabled = true,
                    Formatter = @"function() { return '<b>' + this.series.name + '</b><br/>' + this.x +': ' + this.y; }"
                })
                .SetPlotOptions(new PlotOptions
                {
                    Line = new PlotOptionsLine
                    {
                        DataLabels = new PlotOptionsLineDataLabels
                        {
                            Enabled = true
                        },
                        EnableMouseTracking = false
                    }
                })

                .SetSeries(new[] {
                    new Series{ Name = "Date", Data = new Data(yDataWeight)},

                });

            return View(chart);
        }

        public WeatherModel GetLocalForecast(float lat, float lon)
        {
            var request = new ForecastIORequest("d1d02d9b39d4125af3216ea665368a5c", lat, lon, Unit.us);
            var response = request.Get();

            WeatherModel weather = new WeatherModel(response);
            return weather;
        }

        public void GetGeocode(string address, out double lat, out double lon)
        {
            var googleLocation = new GoogleLocationService();
            var latlon = googleLocation.GetLatLongFromAddress(address);
            lat = latlon.Latitude;
            lon = latlon.Longitude;
        }

        public void BuildWeatherDisplay(WeatherModel weather)
        {
            ViewBag.Temp = Convert.ToInt32(weather.currentWeather.temperature) + "°F / " + ConvertToCelsius(weather.currentWeather.temperature) + "°C";
            ViewBag.Max = Convert.ToInt32(weather.day0.apparentTemperatureMax) + "°F / " + ConvertToCelsius(weather.day0.apparentTemperatureMax) + "°C at " + Convert.ToDateTime(weather.day0.temperatureMaxTime).ToLongTimeString();
            ViewBag.Min = Convert.ToInt32(weather.day0.apparentTemperatureMin) + "°F / " + ConvertToCelsius(weather.day0.apparentTemperatureMin) + "°C at " + Convert.ToDateTime(weather.day0.temperatureMinTime).ToLongTimeString();
            ViewBag.Summary = weather.currentWeather.summary;
            ViewBag.Sunrise = weather.day0.SunriseTime.ToLongTimeString();
            ViewBag.Sunset = weather.day0.SunsetTime.ToLongTimeString();
            ViewBag.ClassName = iconClassName(weather.currentWeather.icon);
        }

        private int ConvertToCelsius(float farenheit)
        {
            double celsius = (farenheit - 32) / 1.8;
            return Convert.ToInt32(celsius);
        }

        private string iconClassName(string iconName)
        {
            string className = "";
            switch (iconName)
            {
                case "clear-day":
                    className = "wi wi-day-sunny";
                    break;
                case "clear-night":
                    className = "wi wi-night-clear";
                    break;
                case "rain":
                    className = "wi wi-rain";
                    break;
                case "snow":
                    className = "wi wi-snow";
                    break;
                case "sleet":
                    className = "wi wi-sleet";
                    break;
                case "wind":
                    className = "wi wi-strong-wind";
                    break;
                case "fog":
                    className = "wi wi-fog";
                    break;
                case "partly-cloudy-day":
                    className = "wi wi-day-cloudy";
                    break;
                case "partly-cloudy-night":
                    className = "wi wi-night-cloudy";
                    break;
                case "hail":
                    className = "wi wi-hail";
                    break;
                case "thunderstorm":
                    className = "wi wi-thunderstorm";
                    break;
                case "tornado":
                    className = "wi wi-tornado";
                    break;
            }

            return className;
        }
    }
}
