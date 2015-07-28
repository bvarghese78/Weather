using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForecastIO;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication.Models
{
    public class WeatherModel
    {
        public class Current
        {
            public float apparentTemperature;   // Feels Like weather in farenheit
            public int cloudCover;    // Cloud Cover values in percentage. (0 - Clear Sky, 40 - Scattered Clouds, 75 - Broken Cloud Cover, 100 - Overcast Sky) 
            public float dewPoint;  // Dew Point in farenheit
            public int humidity;    // Humidity in percentage
            public string icon; // Icon name (clear-day, clear-night, rain, snow, sleet, wind, fog, cloudy, partly-cloudy-day, partly-cloudy-night)
            public int nearestStormBearing; // Nearest storm bearing in degrees
            public int nearestStormDistance;    // Nearest storm distance in miles
            public float ozone; // Columnar density of total atmospheric ozone at the given time in Dobson units
            public float precipIntensity;   // Precipitation intensity in inches of liquid water per hour. (0 in/hr - No Precipitation, 0.002 in/hr - very light precipitation, 0.017 in/hr - light precipitation, 0.1 in/hr - moderate precipitation, 0.4 in/hr - heavy precipiation
            public int precipProbablity;    // Precipitation probability in percentage
            public float pressure;  // Sea level air pressure in millibars
            public string summary;  // Text summary for current temperature
            public float temperature; // Current Temperature in farenheit
            public DateTime time;   // UNIX Time
            public float visibility;    // Average visibility in miles
            public int windBearing; // Wind direction in degrees
            public int windSpeed;   // Wind speed in mph

            public Current(ForecastIO.ForecastIOResponse forecast)
            {
                this.apparentTemperature = forecast.currently.apparentTemperature;
                this.cloudCover = Convert.ToInt32(forecast.currently.cloudCover * 100);
                this.dewPoint = forecast.currently.dewPoint;
                this.humidity = Convert.ToInt32(forecast.currently.humidity * 100);
                this.icon = forecast.currently.icon;
                this.nearestStormBearing = Convert.ToInt32(forecast.currently.nearestStormBearing);
                this.nearestStormDistance = Convert.ToInt32(forecast.currently.nearestStormDistance);
                this.ozone = forecast.currently.ozone;
                this.precipIntensity = forecast.currently.precipIntensity;
                this.precipProbablity = Convert.ToInt32(forecast.currently.precipProbability);
                this.pressure = forecast.currently.pressure;
                this.summary = forecast.currently.summary;
                this.temperature = forecast.currently.temperature;
                this.visibility = forecast.currently.visibility;
                this.windBearing = Convert.ToInt32(forecast.currently.windBearing);
                this.windSpeed = Convert.ToInt32(forecast.currently.windSpeed);

                DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                this.time = UnixTime.AddSeconds(forecast.currently.time).ToLocalTime();
            }
        }

        public class Daily
        {
            public float apparentTemperatureMax;   // Maximum temperature for the day
            public float apparentTemperatureMin;   // Minimum temperature for the day
            public float apparentTemperatureMaxTime;   // Maximum temperature for the day (at the time at which it occurs)
            public float apparentTemperatureMinTime;   // Minimum temperature for the day (at the time at which it occurs)
            public int cloudCover;    // Cloud Cover values in percentage. (0 - Clear Sky, 40 - Scattered Clouds, 75 - Broken Cloud Cover, 100 - Overcast Sky) 
            public float dewPoint;  // Dew Point in farenheit
            public int humidity;    // Humidity in percentage
            public string icon; // Icon name (clear-day, clear-night, rain, snow, sleet, wind, fog, cloudy, partly-cloudy-day, partly-cloudy-night)
            public float moonPhase; // Represents the fractional part of the lunation number of the given day (0 - new moon, 0.25 - first quarter moon, 0.5 - full moon, 0.75 - last quarter moon)
            public float ozone; // Columnar density of total atmospheric ozone at the given time in Dobson units
            public float precipIntensity;   // Precipitation intensity in inches of liquid water per hour. (0 in/hr - No Precipitation, 0.002 in/hr - very light precipitation, 0.017 in/hr - light precipitation, 0.1 in/hr - moderate precipitation, 0.4 in/hr - heavy precipiation
            public float precipIntensityMax; // Maximum expected instensity in inches/hour
            public int precipProbablity;    // Precipitation probability in percentage
            public float pressure;  // Sea level air pressure in millibars
            public string summary;  // Text summary for current temperature
            public DateTime SunriseTime;    // Sunrise Time in UNIX time
            public DateTime SunsetTime;     // Sunset Time in UNIX time
            public float temperatureMax; // Maximum temperature for the day
            public DateTime temperatureMaxTime; // Time of day when maximum temperature reaches
            public float temperatureMin; // Minimum temperature for the day
            public DateTime temperatureMinTime; // Time of day when minimum temperature reaches
            public DateTime time;   // UNIX Time
            public float visibility;    // Average visibility in miles
            public int windBearing; // Wind direction in degrees
            public int windSpeed;   // Wind speed in mph

            public Daily(ForecastIO.ForecastIOResponse forecast, int index)
            {
                this.apparentTemperatureMax = forecast.daily.data[index].apparentTemperatureMax;
                this.apparentTemperatureMin = forecast.daily.data[index].apparentTemperatureMin;
                this.apparentTemperatureMaxTime = forecast.daily.data[index].apparentTemperatureMaxTime;
                this.apparentTemperatureMinTime = forecast.daily.data[index].apparentTemperatureMinTime;
                this.cloudCover = Convert.ToInt32(forecast.daily.data[index].cloudCover * 100);
                this.dewPoint = forecast.daily.data[index].dewPoint;
                this.humidity = Convert.ToInt32(forecast.daily.data[index].humidity * 100);
                this.icon = forecast.daily.data[index].icon;
                this.moonPhase = forecast.daily.data[index].moonPhase;
                this.ozone = forecast.daily.data[index].ozone;
                this.precipIntensity = forecast.daily.data[index].precipIntensity;
                this.precipIntensityMax = forecast.daily.data[index].precipIntensityMax;
                this.precipProbablity = Convert.ToInt32(forecast.daily.data[index].precipProbability);
                this.pressure = forecast.daily.data[index].pressure;
                this.summary = forecast.daily.data[index].summary;
                this.temperatureMax = forecast.daily.data[index].temperatureMax;
                this.temperatureMin = forecast.daily.data[index].temperatureMin;
                this.visibility = forecast.daily.data[index].visibility;
                this.windBearing = Convert.ToInt32(forecast.daily.data[index].windBearing);
                this.windSpeed = Convert.ToInt32(forecast.daily.data[index].windSpeed);

                DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                this.time = UnixTime.AddSeconds(forecast.daily.data[index].time).ToLocalTime();
                this.SunriseTime = UnixTime.AddSeconds(forecast.daily.data[index].sunriseTime).ToLocalTime();
                this.SunsetTime = UnixTime.AddSeconds(forecast.daily.data[index].sunsetTime).ToLocalTime();
                this.temperatureMaxTime = UnixTime.AddSeconds(forecast.daily.data[index].temperatureMaxTime).ToLocalTime();
                this.temperatureMinTime = UnixTime.AddSeconds(forecast.daily.data[index].temperatureMinTime).ToLocalTime();
            }
        }

        public Current currentWeather;
        public Daily day0;
        public Daily day1;
        public Daily day2;

        public WeatherModel()
        {

        }

        public WeatherModel(ForecastIO.ForecastIOResponse forecast)
        {
            this.currentWeather = new Current(forecast);
            this.day0 = new Daily(forecast, 0);
            this.day1 = new Daily(forecast, 1);
            this.day2 = new Daily(forecast, 2);
        }

        [Display(Name = "Address")]
        public string Address { get; set; }
    }
}
