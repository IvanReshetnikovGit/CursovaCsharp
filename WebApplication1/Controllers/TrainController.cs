using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting;
namespace WebApplication1.Controllers
{
    public class TrainController : Controller
    {
        private TrainSchedule _trainSchedule;

        public TrainController(TrainSchedule trainSchedule)
        {
            _trainSchedule = trainSchedule;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_trainSchedule);
        }
        public IActionResult Return()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddTrain(string destination, int peron,int hour,int minute,int second)
        {
            try
            {
                Time time= new(hour,minute,second);
                TrainInfo trainInfo = new(destination,time,peron);
                for (int i = 0; i < _trainSchedule.TrainAmount; i++)
                {
                    if (_trainSchedule.Schedule[i].GetDepartureTime() == trainInfo.GetDepartureTime()
                    && _trainSchedule.Schedule[i].GetPeron() == trainInfo.GetPeron()
                    || !_trainSchedule.Schedule[i].GetDepartureTime().IsValid(trainInfo.GetDepartureTime()) 
                    &&  _trainSchedule.Schedule[i].GetPeron() == trainInfo.GetPeron())
                    {
                        throw new ArgumentOutOfRangeException(paramName: nameof(trainInfo),message:"Time difference between two trains on the sam peron must be more than 30  minutes!");
                    }
                }
                _trainSchedule.Schedule.Push(trainInfo);
                _trainSchedule.TrainAmount++;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Time), ex.Message);
                return View("Index",_trainSchedule);
            }
            return RedirectToAction("Index");
        }
        public IActionResult FindTrainByDepartureTime(int hour,int minute,int second)
        {
            List<TrainInfo> res = new();
            try
            {
                Time t = new(hour,minute,second);
                res = _trainSchedule.FindTrainByDepartureTime(t);
                if (res != null)
                {
                    ViewBag.res = res;
                    ViewBag.massive = true;
                    return View("SearchResult");
                }
                return View("Index",_trainSchedule);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);
                return View("Index",_trainSchedule);
            }
        }   
        public IActionResult ClosestTrain(string destination)
        {
            TrainInfo res = new();
            try
            {
                res = _trainSchedule.ClosestTrain(destination.ToUpper());
                if (res != null)
                {
                    ViewBag.res = res;
                    ViewBag.massive = false;
                    return View("SearchResult");
                }
                return View("Index",_trainSchedule);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);
                return View("Index",_trainSchedule);
            }
        }
        public IActionResult ToEdit()
        {
            return View("Edit",_trainSchedule);
        }
        public IActionResult Edit(string[] destinations, int[] hours, int[] minutes, int[] seconds,int[] perons)
        {
            for (int i = 0; i < _trainSchedule.TrainAmount; i++)
            {
                _trainSchedule.Schedule[i].SetDestination(destinations[i]);
                Time t = new(hours[i],minutes[i],seconds[i]);
                _trainSchedule.Schedule[i].SetDepartureTime(t);
                _trainSchedule.Schedule[i].SetPeron(perons[i]);
            }
            return View("Index",_trainSchedule);
        }
        public IActionResult WriteFile()
        {
            string json = JsonSerializer.Serialize(_trainSchedule.ToString());
            string filePath = Path.Combine("schedule.json");
            System.IO.File.WriteAllText(filePath, json);

            return View("Index",_trainSchedule);
        }
        public IActionResult ReadFile()
        {
            try 
            {
                string filePath = "schedule.json"; // Путь к файлу в папке проекта

                if (!System.IO.File.Exists(filePath))
                    throw new Exception("File doesnt exist");
                
                string fileContent = System.IO.File.ReadAllText(filePath);
                
                string[] lines = fileContent.Split("\\n", StringSplitOptions.RemoveEmptyEntries)
                                        .Select(line => line.Replace("\n", ""))
                                        .ToArray();
                int stop = 1;
                while(_trainSchedule.Schedule.Size() > 0)
                    _trainSchedule.Schedule.Pop();
                
                _trainSchedule.TrainAmount = int.Parse(lines[0].Replace("\"", "").Trim());
                for (int i = 0; i < _trainSchedule.TrainAmount; i++)
                {
                    TrainInfo t = new();
                    t.SetDestination(lines[stop]);
                    Time t1 = new();
                    t1.SetHour(int.Parse(lines[stop+1]));
                    t1.SetMinute(int.Parse(lines[stop+2]));
                    t1.SetSecond(int.Parse(lines[stop+3]));
                    t.SetDepartureTime(t1);
                    t.SetPeron(int.Parse(lines[stop+4]));
                    _trainSchedule.AddTrainInfos(t);
                    stop += 5;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Time), ex.Message);
            }
            return View("Index", _trainSchedule);
        }
    }
}
