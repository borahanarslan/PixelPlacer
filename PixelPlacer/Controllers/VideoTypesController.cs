using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PixelPlacer.Data;
using PixelPlacer.Models;

namespace PixelPlacer.Controllers
{
    public class VideoTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VideoTypesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: VideoTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.VideoType.ToListAsync());
        }

        // GET: VideoTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoType = await _context.VideoType
                .SingleOrDefaultAsync(m => m.VideoTypeId == id);
            if (videoType == null)
            {
                return NotFound();
            }

            return View(videoType);
        }

        // GET: VideoTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VideoTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoTypeId,Category")] VideoType videoType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(videoType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(videoType);
        }

        // GET: VideoTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoType = await _context.VideoType.SingleOrDefaultAsync(m => m.VideoTypeId == id);
            if (videoType == null)
            {
                return NotFound();
            }
            return View(videoType);
        }

        // POST: VideoTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VideoTypeId,Category")] VideoType videoType)
        {
            if (id != videoType.VideoTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoTypeExists(videoType.VideoTypeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(videoType);
        }

        // GET: VideoTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoType = await _context.VideoType
                .SingleOrDefaultAsync(m => m.VideoTypeId == id);
            if (videoType == null)
            {
                return NotFound();
            }

            return View(videoType);
        }

        // POST: VideoTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var videoType = await _context.VideoType.SingleOrDefaultAsync(m => m.VideoTypeId == id);
            _context.VideoType.Remove(videoType);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool VideoTypeExists(int id)
        {
            return _context.VideoType.Any(e => e.VideoTypeId == id);
        }
    }
}
