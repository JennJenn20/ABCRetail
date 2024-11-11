using ABCRetailWebApplication.Models;
using ABCRetailWebApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailWebApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly AzureTableService _tableService;
        private readonly AzureBlobService _blobService;

        public ProductController(AzureTableService tableService, AzureBlobService blobService)
        {
            _tableService = tableService;
            _blobService = blobService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _tableService.GetAllEntities<Product>();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    var imageUrl = await UploadImageToBlobStorage(imageFile);
                    product.ImageUrl = imageUrl;
                }

                await _tableService.AddEntity(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var product = await _tableService.GetEntity<Product>(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    var imageUrl = await UploadImageToBlobStorage(imageFile);
                    product.ImageUrl = imageUrl;
                }

                await _tableService.UpdateEntity(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _tableService.GetEntity<Product>(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _tableService.DeleteEntity<Product>(id);
            return RedirectToAction("Index");
        }

        private async Task<string> UploadImageToBlobStorage(IFormFile imageFile)
        {
            var blobName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            await _blobService.UploadBlob(blobName, imageFile);
            return _blobService.GetBlobUri(blobName).ToString();
        }
    }
}

