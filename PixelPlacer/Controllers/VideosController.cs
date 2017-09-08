using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PixelPlacer.Data;
using PixelPlacer.Models;
using Microsoft.AspNetCore.Authorization;
using PixelPlacer.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.IO;

namespace PixelPlacer.Controllers
{
    public class VideosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _environment;
        private ApplicationUser _currentUser { get; set; }

        public VideosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }


        // This task retrieves the currently authenticated user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Videos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Video.ToListAsync());
        }

        // GET: Videos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Video
                .SingleOrDefaultAsync(m => m.VideoId == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Upload()
        {

            UploadVideoViewModel upload = new UploadVideoViewModel(_context)
            {
                Video = new Video()
            };
            var user = await GetCurrentUserAsync();
            return View(upload);
        }
        

        // Post Uploaded Video
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(UploadVideoViewModel upload)
        {
            ModelState.Remove("Video.User");
            ModelState.Remove("Video.VideoFilePath");

            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                upload.Video.VideoTypeId = upload.Video.VideoTypeId;
                foreach (var file in upload.UserVideo)
                {
                    var filename = ContentDispositionHeaderValue.Parse
                        (file.ContentDisposition).FileName.Trim('"');
                    filename = _environment.WebRootPath + $@"\video\{file.FileName.Split('\\').Last()}";
                    using (var fileStream = new FileStream(filename, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        upload.Video.VideoFilePath = $@"\video\{file.FileName.Split('\\').Last()}";
                    }
                }

                upload.Video.User = user;
                _context.Add(upload.Video);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View("Index", "Home");
        }
        

        // GET: Videos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Video.SingleOrDefaultAsync(m => m.VideoId == id);
            if (video == null)
            {
                return NotFound();
            }
            return View(video);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VideoId,VideoTitle,VideoFilePath,Thumbnail,VideoTypeId")] Video video)
        {
            if (id != video.VideoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(video);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoExists(video.VideoId))
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
            return View(video);
        }

        // GET: Videos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Video
                .SingleOrDefaultAsync(m => m.VideoId == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var video = await _context.Video.SingleOrDefaultAsync(m => m.VideoId == id);
            _context.Video.Remove(video);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool VideoExists(int id)
        {
            return _context.Video.Any(e => e.VideoId == id);
        }
    }
}
