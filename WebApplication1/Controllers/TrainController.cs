using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class TrainController : Controller
    {
        private readonly TrainSchedule _trainSchedule;

        public TrainController(TrainSchedule trainSchedule)
        {
            _trainSchedule = trainSchedule;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_trainSchedule);
        }

        [HttpPost]
        public IActionResult AddTrain(string destination, int peron,int hour,int minute,int second)
        {
            try
            {
                Time time= new(hour,minute,second);
                TrainInfo trainInfo = new(destination,time,peron);
                _trainSchedule.Schedule.Push(trainInfo);
                _trainSchedule.TrainAmount++;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);
                return View("Index", _trainSchedule);
            }
            return RedirectToAction("Index",_trainSchedule);    
        }
    }
}
