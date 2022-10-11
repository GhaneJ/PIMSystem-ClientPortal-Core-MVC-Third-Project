﻿using System;
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

        //ValueTask<T> is a value type, not a reference type and because of this
        //it will have less memory and provides better performance as compared to Task<T>.

        // GET: Item
        public async Task<IActionResult> Index(string itemName)
        {
            var items = from i in _context.Items
                        select i;

            if (!string.IsNullOrEmpty(itemName))
            {
                items = items.Where(s => s.ItemName.Contains(itemName));
            }
            List<Item> listOfItems = await items.OrderByDescending(s => s.ItemCreated).ToListAsync();
            return View(listOfItems);
        }

        // GET: Item/Details/5
        public async Task<IActionResult> Details(Item item)
        {
            ItemViewModel clickedItem = new ItemViewModel();
            //Item item = new Item();
            Task<string> apiResponse = null;
            if (item.ItemName == null || _context.Items == null)
            {
                return NotFound();
            }

            Item itemObject = await _context.Items.Where(m => m.ItemName == item.ItemName)
                .FirstOrDefaultAsync();

            //if (itemObject.ItemStatus == "Enrichment Complete")
            //{
            //    //If the client item is enriched, get the price from server API

            //if (itemObject.ItemRetailPrice == null)
            //    {
            //        apiResponse = GetSelectedItemInfo(itemName);
            //        if (!string.IsNullOrEmpty(apiResponse.Result))
            //        {
            //            try
            //            {
            //                if (apiResponse.Result.Contains(itemName))
            //                {
            //                    clickedItem = DeserializeAPIResponseToEntity(apiResponse.Result, itemName);
            //                    if (clickedItem.ItemName != null)
            //                    {
            //                        itemObject.ItemRetailPrice = clickedItem.ItemRetailPrice;

            //                        await Edit(itemName, itemObject.ItemRetailPrice, itemObject);

            //                        if (itemObject == null)
            //                        {
            //                            return NotFound();
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    // Report that the item does not exist in the Server database
            //                }
            //            }
            //            catch (NullReferenceException e)
            //            {
            //                Console.WriteLine("\nException Caught!");
            //                Console.WriteLine("Message :{0} ", e.Message);
            //            }
            //        }
            //    }
            //}
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
                model.ItemName = item.ItemName;
                item.ItemCreated = DateTime.Now;
                model.ItemServiceInterval = item.ItemServiceInterval;
                model.ItemEngineType = item.ItemEngineType;
                model.ItemStatus = item.ItemStatus;
                model.ItemForceSend = item.ItemForceSend;
                model.ItemPackageQuantity = item.ItemPackageQuantity;
                model.ItemRetailPrice = item.ItemRetailPrice;
                model.ItemBrandColor = item.ItemBrandColor;
                model.ItemBaseColor = item.ItemBaseColor;
                model.ItemNutritionalFacts = item.ItemNutritionalFacts;
                model.ItemCategory = item.ItemCategory;
                model.ItemPackageType = item.ItemPackageType;
                model.ItemSize = item.ItemSize;

                if (item.ResourceImageFile != null)
                {
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
                TempData["Success"] = "The item was created";
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult Edit(string itemName)
        {
            if (itemName == null)
            {
                return NotFound();
            }
            ItemViewModel model = new ItemViewModel();
            var item = _context.Items.Where(x => x.ItemName.Contains(itemName)).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }

            //var apiResponse = GetSelectedItemInfo(itemName);
            //if (apiResponse.Result != null)
            //{
            //    if (apiResponse.Result.Contains(itemName))
            //    {
            //        ItemViewModel clickedItem = DeserializeAPIResponseToEntity(apiResponse.Result, itemName);
            //        if (item.ItemRetailPrice == null)
            //        {
            //            item.ItemRetailPrice = clickedItem.ItemRetailPrice;
            //        }
            //    }
            //}
            return View(item);
        }

        // POST: Item/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string itemName, double? itemRetailPrice, Item item)
        {
            if (itemName != item.ItemName)
            {
                return NotFound();
            }

            ItemViewModel model = new ItemViewModel();

            model.ItemName = item.ItemName;
            model.ItemCreated = item.ItemCreated;
            model.ResourceFileName = item.ResourceFileName;
            model.ResourceImageTitle = item.ResourceImageTitle;
            model.ItemRetailPrice = itemRetailPrice;
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
                    TempData["Success"] = "The item was updated";
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
            var itemModel = await _context.Items.Where(x => x.ItemName.Contains(itemName)).FirstOrDefaultAsync();
            

            //delete image from wwwroot/image
            try
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", itemModel.ResourceFileName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            
                //delete the record
                _context.Items.Remove(itemModel);
                await _context.SaveChangesAsync();
            TempData["Success"] = "The item was deleted";

            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(string itemName)
        {
            return _context.Items.Any(e => e.ItemName == itemName);
        }

        public async Task<string> GetSelectedItemInfo(string itemName)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string responseBody = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    response = await client.GetAsync("Item");
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }                
                //ViewData.Model = item;
            }
            return responseBody;
        }

        public ItemViewModel DeserializeAPIResponseToEntity(string response, string itemName)
        {
            List<ItemViewModel> items = new List<ItemViewModel>();
            ItemViewModel clickedItem = null;
            if (response != null)
            {
                items = JsonConvert.DeserializeObject<List<ItemViewModel>>(response);
                clickedItem = items.Where(x => x.ItemName == itemName).FirstOrDefault();
            }            
            return clickedItem;
        }

        //public IActionResult CreateFoodTruckitemPartial()
        //{
        //    List<Item> items = new List<Item>();
        //    return PartialView("_CreateFoodTruckView",  items);
        //}
    }
}
