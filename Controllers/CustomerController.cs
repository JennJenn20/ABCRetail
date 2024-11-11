using ABCRetailWebApplication.Models;
using ABCRetailWebApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailWebApplication.Controllers
{
    public class CustomerController : Controller
    {
            private readonly AzureTableService _tableService;

            public CustomerController(AzureTableService tableService)
            {
                _tableService = tableService;
            }

            [HttpGet]
            public async Task<IActionResult> Index()
            {
                var customers = await _tableService.GetAllEntities<Customer>();
                return View(customers);
            }

            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(Customer customer)
            {
                if (ModelState.IsValid)
                {
                    await _tableService.AddEntity(customer);
                    return RedirectToAction("Index");
                }
                return View(customer);
            }

            [HttpGet]
            public async Task<IActionResult> Edit(string id)
            {
                var customer = await _tableService.GetEntity<Customer>(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(Customer customer)
            {
                if (ModelState.IsValid)
                {
                    await _tableService.UpdateEntity(customer);
                    return RedirectToAction("Index");
                }
                return View(customer);
            }

            [HttpGet]
            public async Task<IActionResult> Delete(string id)
            {
                var customer = await _tableService.GetEntity<Customer>(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }

            [HttpPost, ActionName("Delete")]
            public async Task<IActionResult> DeleteConfirmed(string id)
            {
                await _tableService.DeleteEntity<Customer>(id);
                return RedirectToAction("Index");
            }
        }
}
