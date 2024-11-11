using Microsoft.AspNetCore.Mvc;
using ABCRetailWebApplication.Models;
using ABCRetailWebApplication.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABCRetailWebApplication.Controllers
{
    public class OrderController : Controller
    {
        private readonly AzureQueueService _queueService;
        private static List<Order> orders = new List<Order>(); // Simulating a database

        public OrderController(AzureQueueService queueService)
        {
            _queueService = queueService;
        }

        public IActionResult Index()
        {
            return View(orders); // Returning the list of orders
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                orders.Add(order); // Simulating saving to a database

                // Send a message to the Azure Queue
                await _queueService.SendMessage(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        public IActionResult Edit(int id)
        {
            var order = orders.Find(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                var existingOrder = orders.Find(o => o.Id == order.Id);
                if (existingOrder != null)
                {
                    existingOrder.ProductId = order.ProductId; // Updating fields as necessary
                    existingOrder.Quantity = order.Quantity;
                   

                    // Send a message to the Azure Queue
                    await _queueService.SendMessage(existingOrder);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(order);
        }

        public IActionResult Delete(int id)
        {
            var order = orders.Find(o => o.Id == id);
            if (order != null)
            {
                orders.Remove(order); // Simulating deletion from a database
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
