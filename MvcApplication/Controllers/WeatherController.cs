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
            string address = "3941 NW 122nd Street Oklahoma City OK 73120";
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
