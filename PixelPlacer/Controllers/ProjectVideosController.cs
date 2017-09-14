using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PixelPlacer.Data;
using PixelPlacer.Models;
using Microsoft.AspNetCore.Identity;

namespace PixelPlacer.Controllers
{
    public class ProjectVideosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _currentUser { get; set; }

        public ProjectVideosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // This task retrieves the currently authenticated user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: ProjectVideos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProjectVideos.Include(p => p.Video);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectVideos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectVideos = await _context.ProjectVideos
                .Include(p => p.Video)
                .SingleOrDefaultAsync(m => m.ProjectVideosId == id);
            if (projectVideos == null)
            {
                return NotFound();
            }

            return View(projectVideos);
        }

        // GET: ProjectVideos/Create
        public IActionResult Create()
        {
            ViewData["VideoId"] = new SelectList(_context.Set<Video>(), "VideoId", "UserId");
            return View();
        }

        // POST: ProjectVideos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectVideosId,SavedProjectId,VideoId,XPositition,YPosition")] ProjectVideos projectVideos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectVideos);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["VideoId"] = new SelectList(_context.Set<Video>(), "VideoId", "UserId", projectVideos.VideoId);
            return View(projectVideos);
        }



        // GET: ProjectVideos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectVideos = await _context.ProjectVideos.SingleOrDefaultAsync(m => m.ProjectVideosId == id);
            if (projectVideos == null)
            {
                return NotFound();
            }
            ViewData["VideoId"] = new SelectList(_context.Set<Video>(), "VideoId", "UserId", projectVideos.VideoId);
            return View(projectVideos);
        }

        // POST: ProjectVideos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectVideosId,SavedProjectId,VideoId,XPositition,YPosition")] ProjectVideos projectVideos)
        {
            if (id != projectVideos.ProjectVideosId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectVideos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectVideosExists(projectVideos.ProjectVideosId))
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
            ViewData["VideoId"] = new SelectList(_context.Set<Video>(), "VideoId", "UserId", projectVideos.VideoId);
            return View(projectVideos);
        }

        // GET: ProjectVideos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectVideos = await _context.ProjectVideos
                .Include(p => p.Video)
                .SingleOrDefaultAsync(m => m.ProjectVideosId == id);
            if (projectVideos == null)
            {
                return NotFound();
            }

            return View(projectVideos);
        }

        // POST: ProjectVideos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectVideos = await _context.ProjectVideos.SingleOrDefaultAsync(m => m.ProjectVideosId == id);
            _context.ProjectVideos.Remove(projectVideos);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProjectVideosExists(int id)
        {
            return _context.ProjectVideos.Any(e => e.ProjectVideosId == id);
        }
    }
}
