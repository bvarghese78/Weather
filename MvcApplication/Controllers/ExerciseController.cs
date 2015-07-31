using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace MvcApplication.Controllers
{
    public class ExerciseController : Controller
    {
        ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
        //
        // GET: /Exercise/

        public ActionResult Index()
        {
            Task resultTask = ConsumeResult(queue);

            List<Task> _tasks = new List<Task>();
            _tasks.Add(loop());

            return View();
        }

        private async Task loop()
        {
            
            for (int i = 0; i < 1000000000; i++)
            {
                queue.Enqueue(i);
            }
            await Task.Delay(50);
        }

        private async Task ConsumeResult(ConcurrentQueue<int> localQueue)
        {
            int entry;
            while (true)
            {
                while (localQueue.TryDequeue(out entry))
                    HandleResult(entry);
                await Task.Delay(10);
            }
        }

        private void HandleResult(int entry)
        {
            ViewBag.Task = entry;
        }
    }
}
