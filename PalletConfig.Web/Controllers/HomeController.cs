using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PalletConfig.Web.Models;

namespace PalletConfig.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            PalletConfigViewModel palletConfig = new PalletConfigViewModel();
            palletConfig.EventHandler();

            return View(palletConfig);
        }

        [HttpPost]
        public IActionResult Index(PalletConfigViewModel palletConfig)
        {
            if (ModelState.IsValid)
            {
                palletConfig.EventHandler();
            }
            ModelState.Clear();

            return View(palletConfig);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
