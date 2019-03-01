using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Models
{
    public class ExerciseActivationModel
    {
        public string ActiveExercise { get; set; }

        public IEnumerable<SelectListItem> Exercises { get; set; }
    }
}