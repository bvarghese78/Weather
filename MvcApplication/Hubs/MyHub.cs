using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Globalization;
using MvcApplication.Controllers;
using MvcApplication.Models;

namespace MvcApplication.Hubs
{
    public class MyHub : Hub
    {
        public MyHub()
        {
            // Create a Long running task to do an infinite loop which will keep sending the server time
            // to the clients every 3 seconds.
            var taskTimer = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    // OKC
                    string timeNow = DateTime.Now.ToLongTimeString() + " CDT";
                    string timeNowDate = DateTime.Now.ToString("D");

                    // UTC
                    string timeUtc = DateTime.UtcNow.ToLongTimeString() + " UTC";
                    string timeUtcDate = DateTime.UtcNow.ToString("D");

                    // BOM
                    DateTime BOM = FindTimeFromTimeZone("India Standard Time");
                    string timeIST = BOM.ToLongTimeString() + " IST";
                    string timeISTDate = BOM.ToString("D");

                    // NYC
                    DateTime NYC = FindTimeFromTimeZone("Eastern Standard Time");
                    string timeEST = NYC.ToLongTimeString() + " EDT";
                    string timeESTDate = NYC.ToString("D");

                    // LON
                    DateTime LON = FindTimeFromTimeZone("GMT Standard Time");
                    string timeGMT = LON.ToLongTimeString() + " BST";
                    string timeGMTDate = LON.ToString("D");

                    // DXB
                    DateTime DXB = FindTimeFromTimeZone("Arabian Standard Time");
                    string timeAST = DXB.ToLongTimeString() + " AST";
                    string timeASTDate = DXB.ToString("D");

                    //Sending the server time to all the connected clients on the client method SendServerTime()
                    Clients.All.SendServerTime(timeNow);
                    Clients.All.SendServerTimeUtc(timeUtc);
                    Clients.All.SendServerTimeIST(timeIST);
                    Clients.All.SendServerTimeEST(timeEST);
                    Clients.All.SendServerTimeGMT(timeGMT);
                    Clients.All.SendServerTimeAST(timeAST);

                    Clients.All.SendLocalDate(timeNowDate);
                    Clients.All.SendUtcDate(timeUtcDate);
                    Clients.All.SendISTDate(timeISTDate);
                    Clients.All.SendESTDate(timeESTDate);
                    Clients.All.SendGMTDate(timeGMTDate);
                    Clients.All.SendASTDate(timeASTDate);
                    
                    await Task.Delay(1000);
                }
            }, TaskCreationOptions.LongRunning);

            //var taskWeather = Task.Factory.StartNew(async ()=>
            //{
            //    HomeController hc = new HomeController();
            //    double lat;
            //    double lon;
            //    string address = "3941 NW 122nd Street, Oklahoma City, OK";
            //    hc.GetGeocode(address, out lat, out lon);

            //    while (true)
            //    {
            //        WeatherModel weather = hc.GetLocalForecast((float)lat, (float)lon);
            //        hc.BuildWeatherDisplay(weather);

            //        Clients.All.SendWeather(weather);
            //        await Task.Delay(60000);
            //    }
                
            //}, TaskCreationOptions.LongRunning);
        }

        private static DateTime FindTimeFromTimeZone(string timeZoneName)
        {
            var ist = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
            var newDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, ist);
            return newDate;
        }
    }
}