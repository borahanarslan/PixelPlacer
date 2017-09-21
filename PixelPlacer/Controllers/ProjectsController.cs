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
            var user = await GetCurrentUserAsync();
            ProjectListViewModel list = new ProjectListViewModel(_context, user);
            return View(list);
        }

        // GET: Projects/Play/5
        public async Task<IActionResult> Play(int id)
        {
            var user = await GetCurrentUserAsync();
            ProjectPlayViewModel model = new ProjectPlayViewModel(_context, user, id);         
            return View(model);
        }

        // Controller to display all videos on Create New Project Option
        // Used on NewProjectDisplay.cshtml in Projects View
        public async Task<IActionResult> NewProjectDisplay()
        {
            var user = await GetCurrentUserAsync();
            VideoTypesViewModel model = new VideoTypesViewModel(_context, user);
            return View(model);
        }


        //GET: Projects/AddBackGroundVideo
        // Displays video that meet contraints to display a background video
        // Accepts the VideoId of a single selected video
        [HttpGet]
        public async Task<IActionResult> AddBackGroundVideo()
        {
            var user = await GetCurrentUserAsync();
            CreateNewProjectViewModel model = new CreateNewProjectViewModel(_context, user);
            return View(model);
        }

        //GET: Projects/AddOverLayVideo
        // Displays videos that meet contraints to display a Green Screen video
        // Accepts the VideoId of a List of selected videos up to 3
        [HttpGet]
        public async Task<IActionResult> AddOverLayVideo()
        {
            var user = await GetCurrentUserAsync();
            CreateNewProjectViewModel model = new CreateNewProjectViewModel(_context, user);
            return View(model);
        }

        // POST: Projects/AddVideos 
        // Accepts argument which is a VideoID
        // Once Video is added a new Project is created and each video is added 
        // to the ProjectVideo JT
        // if an open Project exists(no title has been added on the Project Table)
        // then the DB will update current existing Project and JT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBackGroundVideoToDB(int id)
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
            else
            {
                ProjectVideos pv = new ProjectVideos() { VideoId = video.VideoId, ProjectId = currentProject.ProjectId, User = user, BackGround = true };
                _context.ProjectVideos.Add(pv);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("NewProjectDisplay", "Projects");
        }

        // POST: Projects/AddVideos 
        // Accepts argument which is a VideoID
        // Once Video is added a new Project is created and each video is added 
        // to the ProjectVideo JT
        // if an open Project exists(no title has been added on the Project Table)
        // then the DB will update current existing Project and JT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOverLayVideoToDB(int id)
        {
            var user = await GetCurrentUserAsync();
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var video = await _context.Video.SingleOrDefaultAsync(v => v.VideoId == id);

            var currentProject = await _context.Project
                .SingleOrDefaultAsync(m => m.User == user && m.Title == null);

            if (currentProject == null)
            {
                ModelState.Remove("Project.Title");
                Project project = new Project() { User = user };
                _context.Project.Add(project);
                ProjectVideos projectVideos = new ProjectVideos() { VideoId = video.VideoId, ProjectId = project.ProjectId, User = user, BackGround = false};
                _context.ProjectVideos.Add(projectVideos);
                await _context.SaveChangesAsync();
                return RedirectToAction("NewProjectDisplay", "Projects");
            }
            else {
                ProjectVideos pv = new ProjectVideos() { VideoId = video.VideoId, ProjectId = currentProject.ProjectId , User = user, BackGround = false };
                _context.ProjectVideos.Add(pv);
                await _context.SaveChangesAsync();              
            }

            return RedirectToAction("NewProjectDisplay", "Projects");
        }

        // Ajax post request from javascript site.js file
        // Addes Title to a Project and saves Coordinates for Videos 
        // in ProjectVideo Table
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
                    projects.Width = item.Width;
                    projects.Height = item.Height;
                    projects.Rotation = item.Rotation;
                    _context.ProjectVideos.Update(projects);

                }
                await _context.SaveChangesAsync();
            }
            return Ok(new { response = "Go baby go" });
        }


        // GET: Projects/Edit/5
        // Pass in EditProjectViewModel, accessed from Edit View for Projects
        // Method accepts the ProjectId as an argument
        // View Model returns a list of videos associated with that ProjectId from the ProjectVideos Table
        public async Task<IActionResult> Edit(int id)
        {
            var user = await GetCurrentUserAsync();
            EditProjectViewModel model = new EditProjectViewModel(_context, user, id);
        
            var project = await _context.Project.SingleOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // Method called in NewProjectDisplay.cshtml and accepts the ProjectVideoId
        // Allows user to remove videos that have been selected and added to an open Project
        // There will only be one item with that id, if it exists remove from DB 
        // and redirect back to DisplayNewProject View
        public async Task<IActionResult> RemoveProjectVideo(int id)
        {
            List<ProjectVideos> numOfVidOnProject = new List<ProjectVideos>();
            if (id == 0)
            {
                return NotFound();
            }
            var projects = _context.ProjectVideos.SingleOrDefault(p => p.ProjectVideosId == id);
            var projectID = projects.ProjectId;
            numOfVidOnProject = (from pv in _context.ProjectVideos
                                 where pv.ProjectId == projectID
                                 select pv).ToList();

            if (numOfVidOnProject.Count > 1)
            {
                _context.ProjectVideos.Remove(projects);
                await _context.SaveChangesAsync();
                return RedirectToAction("NewProjectDisplay", "Projects");
            } else if (numOfVidOnProject.Count == 1) {
                var project = _context.Project.SingleOrDefault(v => v.ProjectId == projectID);
                _context.ProjectVideos.Remove(projects);
                _context.Project.Remove(project);
                await _context.SaveChangesAsync();
                return RedirectToAction("NewProjectDisplay", "Projects");
            }
            return RedirectToAction("NewProjectDisplay", "Projects");
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Title")] Project project)
        //{
        //    if (id != project.ProjectId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(project);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProjectExists(project.ProjectId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return View(project);
        //}

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
