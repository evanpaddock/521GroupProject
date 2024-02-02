using Microsoft.AspNetCore.Mvc;
using IronParadise.Models;
using IronParadise;
using Microsoft.AspNetCore.Authorization;

namespace IronParadise.Controllers
{
    [Authorize(Roles = "Administrator, Manager")]
    public class ReportsController : Controller
    {
        private readonly GetReports _getReports;

        public ReportsController(GetReports getReports)
        {
            _getReports = getReports;
        }

        public IActionResult MostTypeOfUsers()
        {
            var report = _getReports.GetMostTypeOfUsers();
            return View(report);
        }

        public IActionResult AverageTimePerSession()
        {
            var report = _getReports.GetAverageTimePerSession();
            return View(report);
        }

        public IActionResult MostCommonGoal()
        {
            var report = _getReports.GetMostCommonGoal();
            return View(report);
        }

        public IActionResult NumberOfUsers()
        {
            var report = _getReports.GetNumberOfUsers();
            return View(report);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
