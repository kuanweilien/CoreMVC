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


        #region---Index---
        // GET: PhotoModels
        public async Task<IActionResult> Index()
        {
            if(_context.PhotoModel != null)
            {
                return View(await _context.PhotoModel.ToListAsync());
            }
            else
            {
                return Problem("Entity set 'MariaDBContext.PhotoModel'  is null.");
            }
        }
        #endregion

        #region---Detail---
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


            return View(photoModel);
        }
        #endregion

        #region---Create---
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
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ImageName,Image,FileName,FilePath")] PhotoModel photoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(await SetFile(photoModel));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(photoModel);
        }
        #endregion

        #region---Edit---
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
            return View(photoModel);
        }

        // POST: PhotoModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ImageName,Image,ImageBase64,FileName,FilePath")] PhotoModel photoModel)
        {
            if (id != photoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(photoModel.ImageBase64))
                    {
                        photoModel.Image = Convert.FromBase64String(photoModel.ImageBase64);
                    }
                    _context.Update(await SetFile(photoModel));
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
        #endregion

        #region---Delete---
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
                if (!string.IsNullOrEmpty(photoModel.FilePath))
                {
                    System.IO.File.Delete(photoModel.FilePath);
                }
                _context.PhotoModel.Remove(photoModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // POST: PhotoModels/Index/cb1;cb3;cb5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteList(string checkList)
        {
            if (_context.PhotoModel == null)
            {
                return Problem("Entity set 'MariaDBContext.PhotoModel'  is null.");
            }
            if (!string.IsNullOrEmpty(checkList))
            {
                List<string> ids = checkList.Replace("cb","").Split(';').ToList();
                foreach (string id in ids)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        var photoModel = await _context.PhotoModel.FindAsync(Convert.ToInt32(id));
                        if (photoModel != null)
                        {
                            if (!string.IsNullOrEmpty(photoModel.FilePath))
                            {
                                System.IO.File.Delete(photoModel.FilePath);
                            }
                            _context.PhotoModel.Remove(photoModel);
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        private bool PhotoModelExists(int id)
        {
          return (_context.PhotoModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #region---Function---
        private async Task<PhotoModel> SetFile(PhotoModel photoModel)
        {
            foreach (var file in Request.Form.Files)
            {
                switch(file.Name )
                {
                    case "inputImage":
                        photoModel.ImageName = file.FileName;
                        MemoryStream ms = new MemoryStream();
                        file.CopyTo(ms);
                        photoModel.Image = ms.ToArray();
                        ms.Close();
                        ms.Dispose();
                        break;
                    case "inputFile":
                        photoModel.FileName = Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FileUploads", photoModel.FileName);
                        if (!string.IsNullOrEmpty(photoModel.FilePath))
                        {
                            if (photoModel.FilePath != filePath)
                            {
                                System.IO.File.Delete(photoModel.FilePath);
                            }
                        }
                        photoModel.FilePath = filePath;
                        using (var fileStream = new FileStream(photoModel.FilePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        break;

                }
            }
            return  photoModel;
        }
            
        #endregion
    }
}
