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
using PixelPlacer.Classes;
using System.Net;

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

        public async Task<IActionResult> NewProjectDisplay()
        {
            var user = await GetCurrentUserAsync();
            VideoTypesViewModel model = new VideoTypesViewModel(_context, user);
            return View(model);
        }


        //GET: Projects/Create
        [HttpGet]
        public async Task<IActionResult> AddBackGroundVideo(int id)
        {
            var user = await GetCurrentUserAsync();
            CreateNewProjectViewModel model = new CreateNewProjectViewModel(_context, user, id);
            return View(model);
        }

        //GET: Projects/Create
        [HttpGet]
        public async Task<IActionResult> AddOverLayVideo(int id)
        {
            var user = await GetCurrentUserAsync();
            CreateNewProjectViewModel model = new CreateNewProjectViewModel(_context, user, id);
            return View(model);
        }

        // POST: Projects/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVideos(int id)
        {
            var user = await GetCurrentUserAsync();
            var video = await _context.Video.SingleOrDefaultAsync(v => v.VideoId == id);

            var currentProject = await _context.Project
                .SingleOrDefaultAsync(m => m.User == user && m.Title == null);

            if (currentProject == null)
            {
                ModelState.Remove("Project.Title");
                Project project = new Project() { User = user };
                _context.Project.Add(project);
                ProjectVideos projectVideos = new ProjectVideos() { VideoId = video.VideoId, ProjectId = project.ProjectId, User = user, BackGround = true };
                _context.ProjectVideos.Add(projectVideos);
                await _context.SaveChangesAsync();
                return RedirectToAction("NewProjectDisplay", "Projects");
            }
            else {
                ProjectVideos pv = new ProjectVideos() { VideoId = video.VideoId, ProjectId = currentProject.ProjectId , User = user};
                _context.ProjectVideos.Add(pv);
                await _context.SaveChangesAsync();              
            }

            return RedirectToAction("NewProjectDisplay", "Projects");
        }

        [HttpPost]
        public async Task<IActionResult> SaveProject(SaveProjectViewModel project)
        {
            var user = await GetCurrentUserAsync();
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            if (project.Title != null)
            {
                var p = _context.Project.SingleOrDefault(v => v.Title == null && v.User == user);
                p.Title = project.Title;
                _context.Project.Update(p);
                
                foreach (var item in project.ProjectClass)
                {
                    var projects = _context.ProjectVideos.SingleOrDefault(pv => pv.ProjectVideosId == item.ProjectVideosId);
                    projects.XPosition = item.XPosition;
                    projects.YPosition = item.YPosition;
                    _context.ProjectVideos.Update(projects);

                }
                await _context.SaveChangesAsync();
            }
            return Ok(new { response = "Go baby go" });

            //return RedirectToAction("Index", "Home");
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
