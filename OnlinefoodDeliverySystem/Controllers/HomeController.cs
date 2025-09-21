using Microsoft.AspNetCore.Mvc;
using OnlinefoodDeliverySystem.Models;
using System.Linq;
using System.Xml.Linq;

namespace PracticeWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodOrderingDbContext _context;

        public HomeController(FoodOrderingDbContext context)
        {
            _context = context;
        }

        // Show all records
        public IActionResult Index()
        {
          
            return View();
        }
        // Show all orders (use Orders.cshtml instead of Index.cshtml)
        public IActionResult Orders()
        {
            var dataList = _context.FoodOrders.ToList();
            return View("Orders", dataList);  // explicitly point to Orders.cshtml
        }

        // Create page (GET)
        public IActionResult CreateOrder()
        {
            // Generate the next ID here so it shows in the form
            var newData = new FoodData
            {
                orderId = GenerateNextId()
            };

            // ✅ Create a list of food items with prices
            ViewBag.FoodItems = new List<(string Name, int Price)>
    {
        ("🍔 Burger", 120),
        ("🍕 Pizza", 250),
        ("🍝 Pasta", 180),
        ("🥗 Salad", 100),
        ("🥤 Soft Drink", 50)
    };

            return View(newData);
        }
        
        // Create new record (POST)
        [HttpPost]
        public IActionResult CreateOrder(FoodData data)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(data.orderId))
                    data.orderId = GenerateNextId();

                _context.FoodOrders.Add(data);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(data);
        }



        // ======================
        // DELETE ORDER
        // ======================

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var order = _context.FoodOrders.FirstOrDefault(o => o.orderId == id);
            if (order == null) return NotFound();

            // Delete and save changes
            _context.FoodOrders.Remove(order);
            _context.SaveChanges();

            return RedirectToAction(nameof(Orders)); // refresh table
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var order = _context.FoodOrders.FirstOrDefault(o => o.orderId == id);
            if (order == null) return NotFound();

            return View(order); // shows Edit.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(FoodData updatedOrder)
        {
            if (ModelState.IsValid)
            {
                _context.FoodOrders.Update(updatedOrder);
                _context.SaveChanges();
                return RedirectToAction(nameof(Orders)); // back to list
            }
            return View(updatedOrder);
        }


        // ✅ Function to generate next ID like BF00-1, BF00-2...
        private string GenerateNextId()
        {
            // Get the last ID in the database (if any)
            var lastRecord = _context.FoodOrders
                .OrderByDescending(x => x.orderId)
                .FirstOrDefault();

            int nextNumber = 1;

            if (lastRecord != null && !string.IsNullOrEmpty(lastRecord.orderId))
            {
                // lastRecord.Id is like "BF00-5"
                var parts = lastRecord.orderId.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[1], out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"BF00-{nextNumber}";
        }
    }
}
