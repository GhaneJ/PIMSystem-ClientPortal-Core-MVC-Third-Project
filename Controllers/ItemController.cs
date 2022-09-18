using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PIM_Dashboard.Data;
using PIM_Dashboard.Models;
using PIM_Dashboard.ViewModels;

namespace PIM_Dashboard.Controllers
{
    public class ItemController : Controller
    {
        private readonly PIMDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        string baseURL = "https://localhost:7149/api/";

        public ItemController(PIMDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Item
        public async Task<IActionResult> Index(string itemName)
        {
            var items = from i in _context.Items
                        select i;

            if (!string.IsNullOrEmpty(itemName))
            {
                items = items.Where(s => s.ItemName.Contains(itemName));
            }
            return View(await items.OrderByDescending(s => s.ItemCreated).ToListAsync());
        }

        // GET: Item/Details/5
        public async Task<IActionResult> Details(string itemName)
        {
            if (itemName == null || _context.Items == null)
            {
                return NotFound();
            }

            var clickedItem = GetSelectedItemInfoAsync(itemName);

            var itemObject = await _context.Items
                .FirstOrDefaultAsync(m => m.ItemName == itemName);
            if (clickedItem != null)
            {
                //itemObject.ItemRetailPrice = clickedItem.ItemRetailPrice;
            }
            
            if (itemObject == null)
            {
                return NotFound();
            }

            return View(itemObject);
        }
        

        // GET: Item/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Item/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item)
        {

            bool DoesItemNameExist = _context.Items.Any
         (x => x.ItemName == item.ItemName && x.ItemId != item.ItemId);
            if (DoesItemNameExist == true)
            {
                ModelState.AddModelError("ItemName", "ItemName already exists");
            }

            ItemViewModel model = new ItemViewModel();
            if (ModelState.IsValid)
            {
                if (item.ResourceImageFile != null)
                {
                    model.ItemName = item.ItemName;
                    item.ItemCreated = DateTime.Now;
                    model.ItemPackageType = item.ItemPackageType;
                    model.ItemRetailPrice = item.ItemRetailPrice;

                    //Save image to wwwroot/Images
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(item.ResourceImageFile.FileName);
                    string extension = Path.GetExtension(item.ResourceImageFile.FileName);
                    item.ResourceFileName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await item.ResourceImageFile.CopyToAsync(fileStream);
                    }
                }

                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult Edit(string itemName, decimal itemPrice)
        {
            if (itemName == null)
            {
                return NotFound();
            }
            ItemViewModel model = new ItemViewModel();
            var item = _context.Items.Where(x => x.ItemName.Contains(itemName)).FirstOrDefault();
            //var item = await _context.Items.FindAsync(itemName);
            var clickedItem = GetSelectedItemInfoAsync(itemName);
            if (item.ItemRetailPrice == 0)
            {
                //item.ItemRetailPrice = clickedItem.ItemRetailPrice;
            }

            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Item/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string itemName, Item item)
        {
            if (itemName != item.ItemName)
            {
                return NotFound();
            }

            ItemViewModel model = new ItemViewModel();

            model.ItemName = item.ItemName;
            item.ItemCreated = DateTime.Now;
            model.ResourceFileName = item.ResourceFileName;
            model.ResourceImageTitle = item.ResourceImageTitle;
            model.ItemRetailPrice = item.ItemRetailPrice;
            model.ResourceImageFile = item.ResourceImageFile;

            if (ModelState.IsValid)
            {
                _context.Entry(item).State = EntityState.Modified;
                try
                {
                    if (item.ResourceImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(item.ResourceImageFile.FileName);
                        string extension = Path.GetExtension(item.ResourceImageFile.FileName);
                        item.ResourceFileName = fileName = fileName + DateTime.Now.ToString("yymmssffff") + extension;
                        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await item.ResourceImageFile.CopyToAsync(fileStream);
                        }
                    }

                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Item/Delete/5
        public async Task<IActionResult> Delete(string itemName)
        {
            if (itemName == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.ItemName == itemName);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string itemName)
        {
            //var itemModel = await _context.Items.FindAsync(itemName);
            var itemModel = _context.Items.Where(x => x.ItemName.Contains(itemName)).FirstOrDefault();

            //delete image from wwwroot/image
            try
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", itemModel.ResourceFileName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
                //delete the record
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            finally
            {
                _context.Items.Remove(itemModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(string itemName)
        {
            return _context.Items.Any(e => e.ItemName == itemName);
        }

        [HttpPost]
        public JsonResult IsItemNameAvailable(string itemName, int? id)
        {
            var validateName = _context.Items.FirstOrDefault
                                (x => x.ItemName == itemName && x.ItemId != id);
            if (validateName != null)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        public async Task<ItemViewModel> GetSelectedItemInfoAsync(string itemName)
        {
            IList<ItemViewModel> item = new List<ItemViewModel>();
            HttpResponseMessage response = new HttpResponseMessage();
            ItemViewModel clickedItem = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                try
                {
                    response = await client.GetAsync("Item");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }                
                ViewData.Model = item;
            }

            return clickedItem;
        }
    }
}
