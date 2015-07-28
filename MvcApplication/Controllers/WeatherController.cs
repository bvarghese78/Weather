using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForecastIO;
using MvcApplication.Models;
using System.Threading.Tasks;
using GoogleMaps.LocationServices;

namespace MvcApplication.Controllers
{
    public class WeatherController : Controller
    {
        //
        // GET: /Weather/
        public ActionResult Index()
        {
            double lat;
            double lon;
            string address = "Times Square, Manhattan, NY, 10036";
            GetGeocode(address, out lat, out lon);

            WeatherModel weather = GetForecast((float)lat, (float)lon);
            BuildCurrentView(weather);
            BuildDailyView(weather);
                
            return View();
        }

        [HttpPost]
        public ActionResult Index(WeatherModel model)
        {
            string address = model.Address;
            double lat;
            double lon;

            GetGeocode(address, out lat, out lon);

            WeatherModel weather = GetForecast((float)lat, (float)lon);
            BuildCurrentView(weather);
            BuildDailyView(weather);

            return View(model);
        }
        public WeatherModel GetForecast(float lat, float lon)
        {
            var request = new ForecastIORequest("d1d02d9b39d4125af3216ea665368a5c", lat, lon, Unit.us);
            var response = request.Get();

            WeatherModel weather = new WeatherModel(response);
            return weather;
        }

        public void BuildCurrentView(WeatherModel weather)
        {
            // finds the wind direction.
            var windDirection = (weather.currentWeather.windBearing * 2) <= 360 ? weather.currentWeather.windBearing * 2 : (weather.currentWeather.windBearing * 2) - 360;
            string cardinalDirection = FindCardinalDirection(weather.currentWeather.windBearing);

            ViewBag.ApparentTemperature = Convert.ToInt32(weather.currentWeather.apparentTemperature) + "° F";
            ViewBag.Temperature = Convert.ToInt32(weather.currentWeather.temperature) + "° F";
            ViewBag.CardinalDirection = weather.currentWeather.windSpeed + " mph winds from the " + cardinalDirection;
            ViewBag.CloudCover = weather.currentWeather.cloudCover + "%";
            ViewBag.DewPoint = Convert.ToInt32(weather.currentWeather.dewPoint) + "° F";
            ViewBag.Humidity = weather.currentWeather.humidity + "%";
            //ViewBag.Icon = weather.currentWeather.icon;
            //ViewBag.NearestStormBearing = weather.currentWeather.nearestStormBearing;
            //ViewBag.NearestStormDistance = weather.currentWeather.nearestStormDistance + " mi";
            //ViewBag.Ozone = weather.currentWeather.ozone;
            //ViewBag.PrecipIntensity = weather.currentWeather.precipIntensity + " in/hr";
            //ViewBag.PrecipProbablity = weather.currentWeather.precipProbablity + " %";
            ViewBag.Pressure = weather.currentWeather.pressure + " mb";
            ViewBag.Summary = weather.currentWeather.summary;
            ViewBag.Time = weather.currentWeather.time;
            ViewBag.Visibility = weather.currentWeather.visibility + " mi";
            ViewBag.WindBearing = windDirection + "°. ";
            ViewBag.WindSpeed = weather.currentWeather.windSpeed + " mph";
        }

        public void BuildDailyView(WeatherModel weather)
        {
            var windDirection = (weather.day0.windBearing * 2) <= 360 ? weather.day0.windBearing * 2 : (weather.day0.windBearing * 2) - 360;
            string cardinalDirection = FindCardinalDirection(weather.day0.windBearing);

            // Day 0
            ViewBag.Day0ApparentTemperatureMax = Convert.ToInt32(weather.day0.apparentTemperatureMax) + "° F";
            ViewBag.Day0TemperatureMax = Convert.ToInt32(weather.day0.temperatureMax) + "° F";
            ViewBag.Day0CardinalDirection = weather.day0.windSpeed + " mph winds from the " + cardinalDirection;
            ViewBag.Day0CloudCover = weather.day0.cloudCover + "%";
            ViewBag.Day0DewPoint = Convert.ToInt32(weather.day0.dewPoint) + "° F";
            ViewBag.Day0Humidity = weather.day0.humidity + "%";
            //ViewBag.Day0Icon = weather.day0.icon;
            //ViewBag.Day0MoonPhase = weather.day0.moonPhase;
            //ViewBag.Day0Ozone = weather.day0.ozone;
            //ViewBag.Day0PrecipIntensity = weather.day0.precipIntensity + " in/hr";
            //ViewBag.Day0PrecipProbablity = weather.day0.precipProbablity + " %";
            ViewBag.Day0Pressure = weather.day0.pressure + " mb";
            ViewBag.Day0Sunrise = weather.day0.SunriseTime;
            ViewBag.Day0Sunset = weather.day0.SunsetTime;
            ViewBag.Day0TemperatureMaxTime = weather.day0.temperatureMaxTime;
            ViewBag.Day0TemperatureMin = Convert.ToInt32(weather.day0.temperatureMin) + "° F";
            ViewBag.Day0TemperatureMinTime = weather.day0.temperatureMinTime;
            ViewBag.Day0Summary = weather.day0.summary;
            ViewBag.Day0Time = weather.day0.time.ToString("MM/dd/yyyy");
            ViewBag.Day0Visibility = weather.day0.visibility + " mi";
            ViewBag.Day0WindBearing = windDirection + "°. ";
            ViewBag.Day0WindSpeed = weather.day0.windSpeed + " mph";

            // Day 1
            windDirection = (weather.day1.windBearing * 2) <= 360 ? weather.day1.windBearing * 2 : (weather.day1.windBearing * 2) - 360;
            cardinalDirection = FindCardinalDirection(weather.day1.windBearing);

            ViewBag.Day1ApparentTemperatureMax = Convert.ToInt32(weather.day1.apparentTemperatureMax) + "° F";
            ViewBag.Day1TemperatureMax = Convert.ToInt32(weather.day1.temperatureMax) + "° F";
            ViewBag.Day1CardinalDirection = weather.day1.windSpeed + " mph winds from the " + cardinalDirection;
            ViewBag.Day1CloudCover = weather.day1.cloudCover + "%";
            ViewBag.Day1DewPoint = Convert.ToInt32(weather.day1.dewPoint) + "° F";
            ViewBag.Day1Humidity = weather.day1.humidity + "%";
            //ViewBag.Day1Icon = weather.day1.icon;
            //ViewBag.Day1MoonPhase = weather.day1.moonPhase;
            //ViewBag.Day1Ozone = weather.day1.ozone;
            //ViewBag.Day1PrecipIntensity = weather.day1.precipIntensity + " in/hr";
            //ViewBag.Day1PrecipProbablity = weather.day1.precipProbablity + " %";
            ViewBag.Day1Pressure = weather.day1.pressure + " mb";
            ViewBag.Day1Sunrise = weather.day1.SunriseTime;
            ViewBag.Day1Sunset = weather.day1.SunsetTime;
            ViewBag.Day1TemperatureMaxTime = weather.day1.temperatureMaxTime;
            ViewBag.Day1TemperatureMin = Convert.ToInt32(weather.day1.temperatureMin) + "° F";
            ViewBag.Day1TemperatureMinTime = weather.day1.temperatureMinTime;
            ViewBag.Day1Summary = weather.day1.summary;
            ViewBag.Day1Time = weather.day1.time.ToString("MM/dd/yyyy");
            ViewBag.Day1Visibility = weather.day1.visibility + " mi";
            ViewBag.Day1WindBearing = windDirection + "°. ";
            ViewBag.Day1WindSpeed = weather.day1.windSpeed + " mph";

            // Day 2
            windDirection = (weather.day2.windBearing * 2) <= 360 ? weather.day2.windBearing * 2 : (weather.day2.windBearing * 2) - 360;
            cardinalDirection = FindCardinalDirection(weather.day2.windBearing);

            ViewBag.Day2ApparentTemperatureMax = Convert.ToInt32(weather.day2.apparentTemperatureMax) + "° F";
            ViewBag.Day2TemperatureMax = Convert.ToInt32(weather.day2.temperatureMax) + "° F";
            ViewBag.Day2CardinalDirection = weather.day2.windSpeed + " mph winds from the " + cardinalDirection;
            ViewBag.Day2CloudCover = weather.day2.cloudCover + "%";
            ViewBag.Day2DewPoint = Convert.ToInt32(weather.day2.dewPoint) + "° F";
            ViewBag.Day2Humidity = weather.day2.humidity + "%";
            //ViewBag.Day2Icon = weather.day2.icon;
            //ViewBag.Day2MoonPhase = weather.day2.moonPhase;
            //ViewBag.Day2Ozone = weather.day2.ozone;
            //ViewBag.Day2PrecipIntensity = weather.day2.precipIntensity + " in/hr";
            //ViewBag.Day2PrecipProbablity = weather.day2.precipProbablity + " %";
            ViewBag.Day2Pressure = weather.day2.pressure + " mb";
            ViewBag.Day2Sunrise = weather.day2.SunriseTime;
            ViewBag.Day2Sunset = weather.day2.SunsetTime;
            ViewBag.Day2TemperatureMaxTime = weather.day2.temperatureMaxTime;
            ViewBag.Day2TemperatureMin = Convert.ToInt32(weather.day2.temperatureMin) + "° F";
            ViewBag.Day2TemperatureMinTime = weather.day2.temperatureMinTime;
            ViewBag.Day2Summary = weather.day2.summary;
            ViewBag.Day2Time = weather.day2.time.ToString("MM/dd/yyyy");
            ViewBag.Day2Visibility = weather.day2.visibility + " mi";
            ViewBag.Day2WindBearing = windDirection + "°. ";
            ViewBag.Day2WindSpeed = weather.day2.windSpeed + " mph";
        }
        private static string FindCardinalDirection(int windBearing)
        {
            string cardinalDirection;
            if (windBearing == 0 || windBearing == 360)
                cardinalDirection = "N";
            else if (windBearing > 0 && windBearing < 45)
                cardinalDirection = "NNE";
            else if (windBearing == 45)
                cardinalDirection = "NE";
            else if (windBearing > 45 && windBearing < 90)
                cardinalDirection = "ENE";
            else if (windBearing == 90)
                cardinalDirection = "E";
            else if (windBearing > 90 && windBearing < 135)
                cardinalDirection = "ESE";
            else if (windBearing == 135)
                cardinalDirection = "SE";
            else if (windBearing > 135 && windBearing < 180)
                cardinalDirection = "SSE";
            else if (windBearing == 180)
                cardinalDirection = "S";
            else if (windBearing > 180 && windBearing < 225)
                cardinalDirection = "SSW";
            else if (windBearing == 225)
                cardinalDirection = "SW";
            else if (windBearing > 225 && windBearing < 270)
                cardinalDirection = "WSW";
            else if (windBearing == 270)
                cardinalDirection = "W";
            else if (windBearing > 270 && windBearing < 315)
                cardinalDirection = "WNW";
            else if (windBearing == 315)
                cardinalDirection = "NW";
            else if (windBearing > 315 && windBearing < 360)
                cardinalDirection = "NNW";
            else
                cardinalDirection = "";

            return cardinalDirection;
        }

        public void GetGeocode(string address, out double lat, out double lon)
        {
            var googleLocation = new GoogleLocationService();
            var latlon = googleLocation.GetLatLongFromAddress(address);
            lat = latlon.Latitude;
            lon = latlon.Longitude;
        }
    }
}
