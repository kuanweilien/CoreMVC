using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreMVC.Data;
using CoreMVC.Models;
using System.IO;
namespace CoreMVC.Controllers
{
    public class PhotoModelsController : Controller
    {
        private readonly MariaDBContext _context;

        public PhotoModelsController(MariaDBContext context)
        {
            _context = context;
        }

        // GET: PhotoModels
        public async Task<IActionResult> Index()
        {
            if(_context.PhotoModel != null)
            {
                List<PhotoModel> photos = await _context.PhotoModel.ToListAsync();
                Dictionary<int,string> imgUrls = new Dictionary<int, string>();
                foreach(PhotoModel photo in photos)
                {
                    string imageBase64Data = Convert.ToBase64String(photo.Image);
                    string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                    imgUrls.Add(photo.Id, imageDataURL);
                }
                ViewData["imgUrls"] = imgUrls;
                return View(await _context.PhotoModel.ToListAsync());
            }
            else
            {
                return Problem("Entity set 'MariaDBContext.PhotoModel'  is null.");
            }
                
                          
                          
        }
        // GET: PhotoModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PhotoModel == null)
            {
                return NotFound();
            }

            var photoModel = await _context.PhotoModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photoModel == null)
            {
                return NotFound();
            }

            LoadPhoto(photoModel.Image);

            return View(photoModel);
        }

        // GET: PhotoModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PhotoModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ImageName,Image")] PhotoModel photoModel)
        {
            if (ModelState.IsValid)
            {
                SetPhoto(photoModel);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(photoModel);
        }

        // GET: PhotoModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PhotoModel == null)
            {
                return NotFound();
            }

            var photoModel = await _context.PhotoModel.FindAsync(id);
            if (photoModel == null)
            {
                return NotFound();
            }
            LoadPhoto(photoModel.Image);
            return View(photoModel);
        }

        // POST: PhotoModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Image")] PhotoModel photoModel)
        {
            if (id != photoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SetPhoto(photoModel);
                    _context.Update(photoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoModelExists(photoModel.Id))
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
            return View(photoModel);
        }

        // GET: PhotoModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PhotoModel == null)
            {
                return NotFound();
            }

            var photoModel = await _context.PhotoModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photoModel == null)
            {
                return NotFound();
            }
            LoadPhoto(photoModel.Image);

            return View(photoModel);
        }

        // POST: PhotoModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PhotoModel == null)
            {
                return Problem("Entity set 'MariaDBContext.PhotoModel'  is null.");
            }
            var photoModel = await _context.PhotoModel.FindAsync(id);
            if (photoModel != null)
            {
                _context.PhotoModel.Remove(photoModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoModelExists(int id)
        {
          return (_context.PhotoModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private void SetPhoto(PhotoModel photoModel)
        {
            foreach (var file in Request.Form.Files)
            {
                PhotoModel photo = new PhotoModel();
                photo.Title = photoModel.Title;
                photo.Description = photoModel.Description;
                photo.ImageName = file.FileName;
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                photo.Image = ms.ToArray();
                ms.Close();
                ms.Dispose();

                _context.Add(photo);
            }
        }
        private void LoadPhoto(byte[] image)
        {
            string imageBase64Data = Convert.ToBase64String(image);
            ViewData["imgUrl"] = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
        }
    }
}
