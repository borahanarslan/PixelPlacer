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
using PixelPlacer.Models.ViewModels;

namespace PixelPlacer.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _currentUser { get; set; }

        public ProjectsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // This task retrieves the currently authenticated user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Project.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .SingleOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        public IActionResult NewProjectDisplay()
        {
            VideoTypesViewModel model = new VideoTypesViewModel(_context);
            return View(model);
        }

       
        //GET: Projects/Create
        [HttpGet]
        public async Task<IActionResult> RetrieveVideo(int id)
        {
            var user = await GetCurrentUserAsync();
            CreateNewProjectViewModel model = new CreateNewProjectViewModel(_context, user, id);
            return View(model);
        }

        // POST: Projects/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RetrieveVideo(CreateNewProjectViewModel model, int videoId)
        {
            ModelState.Remove("Project.Title");
            var user = await GetCurrentUserAsync();
            var video = await _context.Video.SingleOrDefaultAsync(v => v.VideoId == videoId);

            var currentProject = await _context.Project
                .SingleOrDefaultAsync(m => m.User == user && m.Title == null);

            if (currentProject == null)
            {
                Project project = new Project() { User = user };
                _context.Project.Add(project);
                ProjectVideos projectVideos = new ProjectVideos() { VideoId = video.VideoId, ProjectId = project.ProjectId };
                _context.ProjectVideos.Add(projectVideos);
                await _context.SaveChangesAsync();
                return View(model);
            }
            else {
                ProjectVideos pv = new ProjectVideos() { VideoId = video.VideoId, ProjectId = currentProject.ProjectId };
                _context.ProjectVideos.Add(pv);
                await _context.SaveChangesAsync();              
            }
            return View(model);
        }

             




        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.SingleOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Title")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
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
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .SingleOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.SingleOrDefaultAsync(m => m.ProjectId == id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }
    }
}
