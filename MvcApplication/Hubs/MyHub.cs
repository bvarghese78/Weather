using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Globalization;

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
                    string timeNow = DateTime.Now.ToLongTimeString() + " CDT";
                    string timeNowDate = calcDateTime(DateTime.Now);
                    string timeUtc = DateTime.UtcNow.ToLongTimeString() + " UTC";
                    string timeUtcDate = calcDateTime(DateTime.UtcNow);
                    //Sending the server time to all the connected clients on the client method SendServerTime()
                    Clients.All.SendServerTime(timeNow);
                    Clients.All.SendServerTimeUtc(timeUtc);

                    Clients.All.SendLocalDate(timeNowDate);
                    Clients.All.SendUtcDate(timeUtcDate);
                    
                    await Task.Delay(1000);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private string calcDateTime(DateTime date)
        {
            string ret;
            string time = date.ToLongTimeString();
            string tempDate = date.ToString("D");
            string timeZone = TimeZoneInfo.Local.DisplayName;

            TimeSpan v = new TimeSpan(5, 30, 0);
            DateTime k = DateTime.UtcNow;
            DateTime d = new DateTime(k.Year, k.Month, k.Day, k.Hour, k.Minute, k.Second);

            var temp = new DateTime(2015, 9, 15);


            DateTimeOffset d1 = new DateTimeOffset(d, v);


            var ist = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var newDate = TimeZoneInfo.ConvertTime(d1, ist);
            
            ret = tempDate;

            return ret;
        }
    }
}