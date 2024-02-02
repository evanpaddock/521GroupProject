using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IronParadise.Models;
using IronParadise;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IronParadise.Data;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IronParadise.Controllers
{
    [Authorize]
    public class GPTController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly GPT gpt;

        // Ensure only one constructor exists
        public GPTController(GPT gpt, UserManager<User> userManager, ApplicationDbContext context, ICompositeViewEngine viewEngine)
        {
            this.gpt = gpt; // Assume GPT is a service you've defined
            _userManager = userManager;
            _context = context;
            _viewEngine = viewEngine;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkoutPlan(WorkoutPlanRequest workoutData)
        {
            // Find the user by username
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (_context.FitnessInfo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.FitnessInfo'  is null.");
            }

            var existingFitnessInfoRow = _context.FitnessInfo
            .FirstOrDefault(c => c.UserId == user.Id);

            if (existingFitnessInfoRow != null)
            {
                // Update the property
                existingFitnessInfoRow.HeightInInches = int.Parse(workoutData.height);
                existingFitnessInfoRow.Age = workoutData.age;
                existingFitnessInfoRow.SexAtBirth = workoutData.gender;
                existingFitnessInfoRow.Weight = int.Parse(workoutData.weight);
                existingFitnessInfoRow.FitnessLevel = workoutData.fitnessLevel;
                existingFitnessInfoRow.PreferredDays = workoutData.days;
                existingFitnessInfoRow.SelectedDurationInMinutes = int.Parse(workoutData.timePerSession);
                existingFitnessInfoRow.WorkoutGoal = workoutData.goal;

                // Save changes to the database
               _context.SaveChanges();
            }
            else
            {
                FitnessInfo fitnessInfo = new FitnessInfo()
                {
                    HeightInInches = int.Parse(workoutData.height),
                    Age = workoutData.age,
                    SexAtBirth = workoutData.gender,
                    Weight = int.Parse(workoutData.weight),
                    FitnessLevel = workoutData.fitnessLevel,
                    PreferredDays = workoutData.days,
                    SelectedDurationInMinutes = int.Parse(workoutData.timePerSession),
                    WorkoutGoal = workoutData.goal,
                    UserId = user.Id,
                };
                _context.FitnessInfo.Add(fitnessInfo);
                await _context.SaveChangesAsync();
            }

            try
            {

                if (workoutData == null || string.IsNullOrWhiteSpace(workoutData.days))
                {
                    // Handle the null or empty case, maybe return a view with an error message
                    return View("Error", new ErrorViewModel { Message = "Workout data is missing." });
                }

                var daysFromForm = new HashSet<string>(workoutData.days.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries));
                HashSet<string> daysInPlan;
                List<WorkoutPlan> plans;

                do
                {
                    plans = await gpt.GetDataAsync(workoutData);
                    if (plans == null)
                    {
                        ModelState.AddModelError("", "Error generating workout plans.");
                        return View(workoutData);
                    }

                    daysInPlan = new HashSet<string>(plans.Select(p => p.DayName));
                }
                while (!daysFromForm.SetEquals(daysInPlan));

                var html = await RenderPartialViewToStringAsync("CreateWorkoutPlan", plans);

                // Create a response object that includes both HTML and data
                var response = new
                {
                    html = html,
                    data = plans // Assuming plans is a serializable object
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        private async Task<string> RenderPartialViewToStringAsync(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

                if (!viewResult.Success)
                    return string.Empty;

                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, writer, new HtmlHelperOptions());

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }

}
