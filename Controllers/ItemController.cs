using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PIMSystemITEMCRUD.Models;
using PIMSystemITEMCRUD.ViewModels;

namespace PIMSystemITEMCRUD.Controllers
{
    public class ItemController : Controller
    {
        private readonly ItemDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        string baseURL = "https://localhost:7149/api/";

        public ItemController(ItemDbContext context, IWebHostEnvironment hostenvironment)
        {
            _context = context;
            _hostEnvironment = hostenvironment;
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
            return View(await items.ToListAsync());
        }
        
        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Item/Details/5
        public async Task<IActionResult> Details(string itemName)
        {
            if (itemName == null || _context.Items == null)
            {
                return NotFound();
            }

            var clickedItem = GetSelectedItemInfo(itemName);

            // Code for getting item price from the API

            //IList<ItemViewModel> item = new List<ItemViewModel>();
            //HttpResponseMessage getData = new HttpResponseMessage();
            //ItemViewModel clickedItem = null;
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(baseURL);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    try
            //    {
            //        getData = await client.GetAsync("Item");
            //    }
            //    catch (Exception e)
            //    {
            //        throw new Exception($"{e.Message}, The API server is not running!" );
            //    }
                

            //    if (getData.IsSuccessStatusCode)
            //    {
            //        string results = getData.Content.ReadAsStringAsync().Result;
            //        item = JsonConvert.DeserializeObject<List<ItemViewModel>>(results);
            //        clickedItem = item.Where(x => x.ItemName == itemName).FirstOrDefault();
            //    }
            //    else
            //    {
            //        Console.WriteLine("Error calling Web API");
            //    }
            //    ViewData.Model = item;
            //}

            // End of code

            

            var itemObject = await _context.Items
                .FirstOrDefaultAsync(m => m.ItemName == itemName);
            itemObject.ItemPrice = clickedItem.ItemPrice;
            if (itemObject == null)
            {
                return NotFound();
            }

            return View(itemObject);
        }

        public ItemViewModel GetSelectedItemInfo(string itemName)
        {
            IList<ItemViewModel> item = new List<ItemViewModel>();
            HttpResponseMessage getData = new HttpResponseMessage();
            ItemViewModel clickedItem = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    getData = client.GetAsync("Item").GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    throw new Exception($"{e.Message}, The API server is not running!");
                }


                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    item = JsonConvert.DeserializeObject<List<ItemViewModel>>(results);
                    clickedItem = item.Where(x => x.ItemName == itemName).FirstOrDefault();
                }
                else
                {
                    Console.WriteLine("Error calling Web API");
                }
                ViewData.Model = item;
            }

            return clickedItem;
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
            ItemViewModel model = new ItemViewModel();
            if (ModelState.IsValid)
            {
                if (item.ItemImageFile != null)
                {
                    model.ItemName = item.ItemName;
                    item.ItemCreated = DateTime.Now;
                    model.ItemPackageType = item.ItemPackageType;
                    model.ItemPrice = item.ItemPrice;
                    
                    //Save image to wwwroot/Images
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(item.ItemImageFile.FileName);
                    string extension = Path.GetExtension(item.ItemImageFile.FileName);
                    item.ItemImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await item.ItemImageFile.CopyToAsync(fileStream);
                    }
                }

                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string itemName, decimal itemPrice)
        {
            if (itemName == null)
            {
                return NotFound();
            }
            ItemViewModel model = new ItemViewModel();
            var item = await _context.Items.FindAsync(itemName);
            var clickedItem = GetSelectedItemInfo(itemName);
            if (item.ItemPrice == 0)
            {
                item.ItemPrice = clickedItem.ItemPrice;
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
            model.ItemImageName = item.ItemImageName;
            model.ItemImageTitle = item.ItemImageTitle;
            model.ItemPrice = item.ItemPrice;
            model.ItemImageFile = item.ItemImageFile;

            if (ModelState.IsValid)
            {
                try
                {
                    if (item.ItemImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(item.ItemImageFile.FileName);
                        string extension = Path.GetExtension(item.ItemImageFile.FileName);
                        item.ItemImageName = fileName = fileName + DateTime.Now.ToString("yymmssffff") + extension;
                        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await item.ItemImageFile.CopyToAsync(fileStream);
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
            var itemModel = await _context.Items.FindAsync(itemName);

            //delete image from wwwroot/image
            try
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", itemModel.ItemImageName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
                //delete the record
                _context.Items.Remove(itemModel);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(string itemName)
        {
            return _context.Items.Any(e => e.ItemName == itemName);
        }
    }
}
