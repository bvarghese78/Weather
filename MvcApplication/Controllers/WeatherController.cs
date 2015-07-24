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
            string address = "3941 NW 122nd Street, Oklahoma City, OK 73120";
            GetGeocode(address, out lat, out lon);

            WeatherModel weather = GetForecast((float)lat, (float)lon);
            BuildView(weather);
            return View();
        }

        public WeatherModel GetForecast(float lat, float lon)
        {
            var request = new ForecastIORequest("d1d02d9b39d4125af3216ea665368a5c", lat, lon, Unit.us);
            var response = request.Get();

            WeatherModel weather = new WeatherModel(response);
            return weather;
        }

        public void BuildView(WeatherModel weather)
        {
            ViewBag.ApparentTemperature = Convert.ToInt32(weather.currentWeather.apparentTemperature) + "° F";
            ViewBag.Temperature = Convert.ToInt32(weather.currentWeather.temperature) + "° F";
            ViewBag.CloudCover = weather.currentWeather.cloudCover + "%";
            ViewBag.DewPoint = Convert.ToInt32(weather.currentWeather.dewPoint) + "° F";
            ViewBag.Humidity = weather.currentWeather.humidity + "%";
            //ViewBag.Icon = weather.currentWeather.icon;
            //ViewBag.NearestStormBearing = weather.currentWeather.nearestStormBearing;
            //ViewBag.NearestStormDistance = weather.currentWeather.nearestStormDistance + " mi";
            //ViewBag.Ozone = weather.currentWeather.ozone;
            //ViewBag.PrecipIntensity = weather.currentWeather.precipIntensity + " in/hr";
            ViewBag.PrecipProbablity = weather.currentWeather.precipProbablity + " %";
            ViewBag.Pressure = weather.currentWeather.pressure + " mb";
            ViewBag.Summary = weather.currentWeather.summary;
            ViewBag.Time = weather.currentWeather.time;
            ViewBag.Visibility = weather.currentWeather.visibility + " mi";
            ViewBag.WindBearing = weather.currentWeather.windBearing + "°";
            ViewBag.WindSpeed = weather.currentWeather.windSpeed + " mph";
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
