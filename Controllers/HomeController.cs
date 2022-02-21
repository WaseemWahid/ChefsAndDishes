using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChefsAndDishes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ChefsAndDishes.Controllers
{
    public class HomeController : Controller
    {

        private ChefContext db;
        public HomeController(ChefContext context)
        {
            db = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<Chef> AllChefs = db.chefs
            .Include(Chef => Chef.dishes)
            .ToList();
            ViewBag.allchefs = AllChefs;
            return View("Index");
        }

        [HttpGet("dishes")]
        public IActionResult Dishes()
        {
            List<Dish> AllDishes = db.dishes
            .Include(Dish => Dish.Creator)
            .ToList();
            ViewBag.alldishes = AllDishes;
            return View("Dishes");
        }

        [HttpGet("new")]
        public IActionResult Chef()
        {
            return View("AddChef");
        }

        [HttpPost("addchef")]
        public IActionResult AddChef(Chef chef)
        {
            if(ModelState.IsValid)
            {
                if(chef.Birthday >= DateTime.Today)
                {
                    ModelState.AddModelError("Birthday", "Must be from the past!");
                    return View("AddChef");
                }
                Chef newChef = new Chef
                {
                    FirstName = chef.FirstName,
                    LastName = chef.LastName,
                    Birthday = chef.Birthday
                };
                db.Add(newChef);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else 
            {
                return View("AddChef");
            }
        }

        [HttpGet("dishes/new")]
        public IActionResult AddDishView()
        {
            List<Chef> AllChefs = db.chefs.ToList();
            ViewBag.allchefs = AllChefs;
            return View("AddDish");
        }

        [HttpPost("/dishes/addnew")]
        public IActionResult AddDish(Dish dish)
        {
            if(ModelState.IsValid)
            {
                db.Add(dish);
                db.SaveChanges();
                return RedirectToAction("AddDishView");
            }
            else 
            {
                List<Chef> AllChefs = db.chefs.ToList();
                ViewBag.allchefs = AllChefs;
                return View("AddDish", dish);
            }
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
