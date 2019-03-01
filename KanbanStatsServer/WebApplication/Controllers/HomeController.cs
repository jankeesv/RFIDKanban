using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new RFIDKanbanEntities())
            {

                Exercises activeExercise = this.HttpContext.Application["exercise"] as Exercises;

                if (activeExercise == null)
                {
                    activeExercise = context.Exercises.Where(e => e.Sequence == 0).FirstOrDefault();
                }

                ViewBag.Title = "RFID Kanban - " + activeExercise.ExerciseName;

                var model = new ExerciseActivationModel();

                model.Exercises = context.Exercises.OrderBy(x => x.Sequence).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.ExerciseName }).ToList();
                model.ActiveExercise = activeExercise.ID.ToString();

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult ActivateExercise(ExerciseActivationModel model)
        {
            using (var context = new RFIDKanbanEntities())
            {
                if (ModelState.IsValid)
                {
                    //Find and set the active exercise in the application
                    Exercises activeExercise = context.Exercises.Where(x => x.ID.ToString().Equals(model.ActiveExercise)).FirstOrDefault();
                    this.HttpContext.Application["exercise"] = activeExercise;

                    //Delete all registrations
                    context.RFIDRegistrations.RemoveRange(context.RFIDRegistrations);
                    context.SaveChanges();

                    ViewBag.Title = "RFID Kanban - " + activeExercise.ExerciseName;

                    return RedirectToAction("Index");
                }

                //Something is wrong. Pass the model again.
                model.Exercises = context.Exercises.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.ExerciseName }).ToList();
                return View("Index", model);
            }
        }
    }
}
