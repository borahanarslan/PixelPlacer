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
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

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

                IFormFile videoFile = upload.UserVideo;

                //create unique identifying using Guid
                // The use Property of Path and GetExtension method to get the FileName extension
                // combine these two so videos uploaded cannot have duplocate naming
                string guid = Guid.NewGuid().ToString();
                string newFileName = guid + Path.GetExtension(videoFile.FileName);
                var videoPath = _environment.WebRootPath + $@"\video\{newFileName}";

                // create custom thumbnail, installed FFMPEG video utility library
                // set the path to the environment root folder 
                string thumbName = guid + ".jpg";
                string thumbPath = _environment.WebRootPath + $@"\video\thumbs\{thumbName}";

                using (var fileStream = new FileStream(videoPath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(fileStream);
                    upload.Video.VideoFilePath = "..\\..\\video\\" + newFileName;
                }

                // arguments to pass to FFMPEG Process
                // -i = telling ffmpeg program which video file to use as an input during it's Process, key value pairs
                // -vframes = 1 frame to use to make an image, key value pair
                // -ss how many seconds into video to capture image, key value pair
                // -filter: v = use video filter that is built in to scale size proportionally
                // scale = 280 is the width and -1 tells it to keep it's height proportional to width, key value pair
                string ffmpegArgs = "-i " + videoPath + " -vframes 1 -ss 00:00:05 -filter:v scale=\"280:-1\" " + thumbPath;

                // start ffmpeg.exe file by creating a new instance which is called a Process
                Process thumb = new Process();
                // use instance of StartInfo class from Process class
                thumb.StartInfo.FileName = _environment.WebRootPath + "\\ffmpeg.exe";
                // declare what arguments it will use
                thumb.StartInfo.Arguments = ffmpegArgs;
                // make sure that Process does not continue if Application stops running
                thumb.StartInfo.UseShellExecute = false;

                // start process, make it wait until process is complete , then dispose of the process instance to cleam up memory              
                try
                {
                    thumb.Start();
                    thumb.WaitForExit();
                    thumb.Dispose();
                }
                catch(Exception err)
                {
                    // make sure error is displayed in debug output console, may not display if using Console.Writeline
                    Debug.WriteLine("thumb error message : " + err.Message);
                }                

                upload.Video.User = user;
                upload.Video.Thumbnail = "..\\..\\video\\thumbs\\" + thumbName;
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
