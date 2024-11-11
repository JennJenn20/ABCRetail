using Microsoft.AspNetCore.Mvc;
using ABCRetailWebApplication.Models;
using ABCRetailWebApplication.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ABCRetailWebApplication.Controllers
{
    public class ContractController : Controller
    {
        private readonly AzureQueueService _queueService;
        private readonly AzureFileService _fileService;
        private static List<Contract> contracts = new List<Contract>(); // Simulating a database

        public ContractController(AzureQueueService queueService, AzureFileService fileService)
        {
            _queueService = queueService;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View(contracts); // Returning the list of contracts
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Contract contract, IFormFile document)
        {
            if (ModelState.IsValid)
            {
                if (document != null && document.Length > 0)
                {
                    // Upload the file to Azure Files and set the file path
                    string fileName = Path.GetFileName(document.FileName);
                    await _fileService.UploadFile(fileName, document);
                    contract.FilePath = fileName; // Assigning uploaded file name to FilePath
                }

                contracts.Add(contract); // Simulating saving to a database

                // Send a message to the Azure Queue
                await _queueService.SendMessage(contract);
                return RedirectToAction(nameof(Index));
            }
            return View(contract);
        }

        public IActionResult Edit(int id)
        {
            var contract = contracts.Find(c => c.Id == id);
            if (contract == null)
            {
                return NotFound();
            }
            return View(contract);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Contract contract, IFormFile document)
        {
            if (ModelState.IsValid)
            {
                var existingContract = contracts.Find(c => c.Id == contract.Id);
                if (existingContract != null)
                {
                    existingContract.Title = contract.Title;

                    if (document != null && document.Length > 0)
                    {
                        // Upload the new file to Azure Files and set the file path
                        string fileName = Path.GetFileName(document.FileName);
                        await _fileService.UploadFile(fileName, document);
                        existingContract.FilePath = fileName; // Updating FilePath
                    }

                    // Send a message to the Azure Queue
                    await _queueService.SendMessage(existingContract);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(contract);
        }

        public IActionResult Delete(int id)
        {
            var contract = contracts.Find(c => c.Id == id);
            if (contract != null)
            {
                contracts.Remove(contract); // Simulating deletion from a database
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
