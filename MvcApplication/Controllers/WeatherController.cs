using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForecastIO;
using MvcApplication.Models;
using System.Threading.Tasks;
using GoogleMaps.LocationServices;
using RestSharp;
using System.Diagnostics;

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
            Helpers.GoogleTimeZone UTCOffSet = GetLocalDateTime(lat, lon, weather.currentWeather.time);
            DateTime localTime = weather.currentWeather.time.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset);
            BuildCurrentView(weather, localTime);
            BuildDailyView(weather, UTCOffSet);

            ViewBag.CurrentCity = "New York, New York";
            return View();
        }

        [HttpPost]
        public ActionResult Index(WeatherModel model)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string address = model.Address;
            double lat;
            double lon;

            if (address != null || address.Length != 0)
                GetGeocode(address, out lat, out lon);
            else
                return View(model);

            WeatherModel weather = GetForecast((float)lat, (float)lon);
            Helpers.GoogleTimeZone UTCOffset = GetLocalDateTime(lat, lon, weather.currentWeather.time);
            DateTime localTime = weather.currentWeather.time.AddSeconds(UTCOffset.rawOffset + UTCOffset.dstOffset);
            BuildCurrentView(weather, localTime);
            BuildDailyView(weather, UTCOffset);
            stopwatch.Stop();
            ViewBag.StopWatch = "Elapsed Time: " + stopwatch.ElapsedMilliseconds;

            ViewBag.CurrentCity = address + " (" + string.Format("{0:00.00000}, {1:00.00000}", lat, lon) + ")";
            return View(model);
        }

        // Get Forecast information for a location using "Forecast.io" API
        // We are using Forecast.io.45 wrapper library to contact Forecast.io API
        public WeatherModel GetForecast(float lat, float lon)
        {
            // ForecastIORequest(API key, lat, lon, measurement unit)
            var request = new ForecastIORequest("d1d02d9b39d4125af3216ea665368a5c", lat, lon, Unit.us);
            var response = request.Get();

            WeatherModel weather = new WeatherModel(response);
            return weather;
        }

        //private static WeatherModel GetForecastRestSharp(float lat, float lon)
        //{
        //    var latlon = lat + "," + lon;
        //    var client = new RestClient("https://api.forecast.io/forecast/d1d02d9b39d4125af3216ea665368a5c/");
        //    var request = new RestRequest(latlon);
        //    var response = client.Execute(request);
            
        //    var weather = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherModel>(response.Content);
        //    return weather;
        //}

        // Get UTC Offset and TimeZone info for a location using Google's TimeZone API
        // We are using RestSharp library to contact Google TimeZone API.
        public Helpers.GoogleTimeZone GetLocalDateTime(double latitude, double longitude, DateTime utcDate)
        {
            DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = utcDate - unix;
            double unixSeconds = Math.Floor(diff.TotalSeconds);

            var client = new RestClient("https://maps.googleapis.com");
            var request = new RestRequest("maps/api/timezone/json", Method.GET);
            request.AddParameter("location", latitude + "," + longitude);
            request.AddParameter("timestamp", unixSeconds);
            var response = client.Execute(request);

            Helpers.GoogleTimeZone offset = Newtonsoft.Json.JsonConvert.DeserializeObject<Helpers.GoogleTimeZone>(response.Content);
            return offset;
        }

        public void BuildCurrentView(WeatherModel weather, DateTime localTime)
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
            ViewBag.Icon = weather.currentWeather.icon;
            //ViewBag.NearestStormBearing = weather.currentWeather.nearestStormBearing;
            //ViewBag.NearestStormDistance = weather.currentWeather.nearestStormDistance + " mi";
            //ViewBag.Ozone = weather.currentWeather.ozone;
            //ViewBag.PrecipIntensity = weather.currentWeather.precipIntensity + " in/hr";
            //ViewBag.PrecipProbablity = weather.currentWeather.precipProbablity + " %";
            ViewBag.Pressure = weather.currentWeather.pressure + " mb";
            ViewBag.Summary = weather.currentWeather.summary;
            ViewBag.Time = localTime;
            ViewBag.Visibility = weather.currentWeather.visibility + " mi";
            ViewBag.WindBearing = windDirection + "°. ";
            ViewBag.WindSpeed = weather.currentWeather.windSpeed + " mph";

            
        }

        public void BuildDailyView(WeatherModel weather, Helpers.GoogleTimeZone UTCOffSet)
        {
            var windDirection = (weather.day0.windBearing * 2) <= 360 ? weather.day0.windBearing * 2 : (weather.day0.windBearing * 2) - 360;
            string cardinalDirection = FindCardinalDirection(weather.day0.windBearing);

            // Day 0
            ViewBag.Day0ApparentTemperatureMax = Convert.ToInt32(weather.day0.apparentTemperatureMax) + "° F";
            ViewBag.Day0TemperatureMax = Convert.ToInt32(weather.day0.temperatureMax) + "° F";
            ViewBag.Day0CardinalDirection = weather.day0.windSpeed + " mph winds from the " + cardinalDirection;
            ViewBag.Day0CloudCover = weather.day0.cloudCover + "%";
            ViewBag.Day0DayOfWeek = (weather.day0.time.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset)).DayOfWeek;
            ViewBag.Day0DewPoint = Convert.ToInt32(weather.day0.dewPoint) + "° F";
            ViewBag.Day0Humidity = weather.day0.humidity + "%";
            //ViewBag.Day0Icon = weather.day0.icon;
            //ViewBag.Day0MoonPhase = weather.day0.moonPhase;
            //ViewBag.Day0Ozone = weather.day0.ozone;
            //ViewBag.Day0PrecipIntensity = weather.day0.precipIntensity + " in/hr";
            //ViewBag.Day0PrecipProbablity = weather.day0.precipProbablity + " %";
            ViewBag.Day0Pressure = weather.day0.pressure + " mb";
            ViewBag.Day0Sunrise = weather.day0.SunriseTime.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset);
            ViewBag.Day0Sunset = weather.day0.SunsetTime.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset);
            ViewBag.Day0TemperatureMaxTime = weather.day0.temperatureMaxTime.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset);
            ViewBag.Day0TemperatureMin = Convert.ToInt32(weather.day0.temperatureMin) + "° F";
            ViewBag.Day0TemperatureMinTime = weather.day0.temperatureMinTime.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset);
            ViewBag.Day0Summary = weather.day0.summary;
            ViewBag.Day0Time = (weather.day0.time.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset)).ToString("MM/dd/yyyy");
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
            ViewBag.Day1DayOfWeek = (weather.day1.time.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset)).DayOfWeek;
            ViewBag.Day1DewPoint = Convert.ToInt32(weather.day1.dewPoint) + "° F";
            ViewBag.Day1Humidity = weather.day1.humidity + "%";
            //ViewBag.Day1Icon = weather.day1.icon;
            //ViewBag.Day1MoonPhase = weather.day1.moonPhase;
            //ViewBag.Day1Ozone = weather.day1.ozone;
            //ViewBag.Day1PrecipIntensity = weather.day1.precipIntensity + " in/hr";
            //ViewBag.Day1PrecipProbablity = weather.day1.precipProbablity + " %";
            ViewBag.Day1Pressure = weather.day1.pressure + " mb";
            ViewBag.Day1Sunrise = weather.day1.SunriseTime.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset);
            ViewBag.Day1Sunset = weather.day1.SunsetTime.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset);
            ViewBag.Day1TemperatureMaxTime = weather.day1.temperatureMaxTime.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset);
            ViewBag.Day1TemperatureMin = Convert.ToInt32(weather.day1.temperatureMin) + "° F";
            ViewBag.Day1TemperatureMinTime = weather.day1.temperatureMinTime.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset);
            ViewBag.Day1Summary = weather.day1.summary;
            ViewBag.Day1Time = (weather.day1.time.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset)).ToString("MM/dd/yyyy");
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
            ViewBag.Day2DayOfWeek = (weather.day2.time.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset)).DayOfWeek;
            ViewBag.Day2DewPoint = Convert.ToInt32(weather.day2.dewPoint) + "° F";
            ViewBag.Day2Humidity = weather.day2.humidity + "%";
            //ViewBag.Day2Icon = weather.day2.icon;
            //ViewBag.Day2MoonPhase = weather.day2.moonPhase;
            //ViewBag.Day2Ozone = weather.day2.ozone;
            //ViewBag.Day2PrecipIntensity = weather.day2.precipIntensity + " in/hr";
            //ViewBag.Day2PrecipProbablity = weather.day2.precipProbablity + " %";
            ViewBag.Day2Pressure = weather.day2.pressure + " mb";
            ViewBag.Day2Summary = weather.day2.summary;
            ViewBag.Day2Sunrise = weather.day2.SunriseTime.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset);
            ViewBag.Day2Sunset = weather.day2.SunsetTime.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset);
            ViewBag.Day2TemperatureMaxTime = weather.day2.temperatureMaxTime.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset);
            ViewBag.Day2TemperatureMin = Convert.ToInt32(weather.day2.temperatureMin) + "° F";
            ViewBag.Day2TemperatureMinTime = weather.day2.temperatureMinTime.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset);
            ViewBag.Day2Time = (weather.day2.time.AddSeconds(UTCOffSet.rawOffset + UTCOffSet.dstOffset)).ToString("MM/dd/yyyy");
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

        // Gets latitude and longitude for a given address using Google's Location Services API
        public void GetGeocode(string address, out double lat, out double lon)
        {
            var googleLocation = new GoogleLocationService();
            var latlon = googleLocation.GetLatLongFromAddress(address);
            lat = latlon.Latitude;
            lon = latlon.Longitude;
        }
    }
}
