using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KendoMvcDemo.Core.Entities;
using KendoMvcDemo.Core.Models.Teacher;
using KendoMvcDemo.Infrastructure.Database;
using KendoMvcDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KendoMvcDemo.Controllers
{
    public class CoursesController : Controller
    {
        private readonly KaustDbContext db;

        public CoursesController(KaustDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Teachers = await db.Teachers
                .Select(x => new TeacherDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            CreateCourseViewModel viewModel = new()
            {
                Start = DateOnly.FromDateTime(DateTime.Today),
                Teachers = await db.Teachers
                    .Select(x => new TeacherDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCourseViewModel viewModel)
        {
            viewModel.Teachers = await db.Teachers
                .Select(t => new TeacherDto
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToListAsync();

            // File validation
            int maxImageFileSizeInBytes = 1048576;
            int minImageFileSizeInBytes = 51200;

            if (viewModel.ImageFile == null || viewModel.ImageFile.Length < minImageFileSizeInBytes || viewModel.ImageFile.Length > maxImageFileSizeInBytes)
            {
                return View(viewModel);
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Custom validation
            if (await db.Courses.AnyAsync(c => c.Code == viewModel.Code))
            {
                ModelState.AddModelError(nameof(viewModel.Code), "A course with the same code already exists.");
            }

            if (await db.Courses.AnyAsync(c => c.Name == viewModel.Name))
            {
                ModelState.AddModelError(nameof(viewModel.Name), "A course with the same name already exists.");
            }

            if (viewModel.End != null && viewModel.End < viewModel.Start)
            {
                ModelState.AddModelError(nameof(viewModel.End), "End date cannot be before start date.");
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                Course course = new()
                {
                    Code = viewModel.Code,
                    Name = viewModel.Name,
                    Section = viewModel.Section,
                    Start = viewModel.Start.ToDateTime(),
                    End = viewModel.End?.ToDateTime()
                };

                foreach (int teacherId in viewModel.SelectedTeacherIds)
                {
                    course.Teachers.Add(new CourseTeacher
                    {
                        Course = course,
                        TeacherId = teacherId
                    });
                }

                db.Courses.Add(course);

                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> GetCourses([DataSourceRequest] DataSourceRequest request)
        {
            // We can work with IQueryable here since the ToDataSourceResultAsync extension method will execute the query against the database.
            // This allows us to only select the necessary fields and avoid loading unnecessary data into memory.
            IQueryable<CourseViewModel> courses = db.Courses
                .Select(x => new CourseViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Section = x.Section,
                    Start = x.Start,
                    End = x.End,
                    SelectedTeacherIds = x.Teachers.Select(t => t.TeacherId).ToList(),
                    Teachers = x.Teachers.Select(t => new TeacherDto
                    {
                        Id = t.Teacher.Id,
                        Name = t.Teacher.Name
                    })
                });

            DataSourceResult result = await courses.ToDataSourceResultAsync(request);

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCourse([DataSourceRequest] DataSourceRequest request, CreateCourseViewModel viewModel)
        {
            List<TeacherDto> teachers = await db.Teachers
                .Where(t => viewModel.SelectedTeacherIds.Contains(t.Id))
                .Select(t => new TeacherDto
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToListAsync();

            if (ModelState.IsValid)
            {
                // Custom validation
                if (await db.Courses.AnyAsync(c => c.Code == viewModel.Code))
                {
                    ModelState.AddModelError(nameof(viewModel.Code), "A course with the same code already exists.");
                }

                if (await db.Courses.AnyAsync(c => c.Name == viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "A course with the same name already exists.");
                }

                if (viewModel.End != null && viewModel.End < viewModel.Start)
                {
                    ModelState.AddModelError(nameof(viewModel.End), "End date cannot be before start date.");
                }

                if (ModelState.IsValid)
                {
                    Course course = new()
                    {
                        Code = viewModel.Code,
                        Name = viewModel.Name,
                        Section = viewModel.Section,
                        Start = viewModel.Start.ToDateTime(),
                        End = viewModel.End?.ToDateTime()
                    };

                    foreach (int teacherId in viewModel.SelectedTeacherIds)
                    {
                        course.Teachers.Add(new CourseTeacher
                        {
                            Course = course,
                            TeacherId = teacherId
                        });
                    }

                    db.Courses.Add(course);

                    await db.SaveChangesAsync();

                    viewModel.Id = course.Id;
                }
            }

            // Return the inserted course. The Grid needs the generated course Id. Also, return any validation errors through the ModelState.
            return Json(new[] { viewModel.ConvertToCourseViewModel(teachers) }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCourse([DataSourceRequest] DataSourceRequest request, UpdateCourseViewModel viewModel)
        {
            List<TeacherDto> teachers = await db.Teachers
                .Where(t => viewModel.SelectedTeacherIds.Contains(t.Id))
                .Select(t => new TeacherDto
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToListAsync();

            if (ModelState.IsValid)
            {
                // Custom validation
                if (await db.Courses.AnyAsync(c => c.Code == viewModel.Code && c.Id != viewModel.Id))
                {
                    ModelState.AddModelError(nameof(viewModel.Code), "A course with the same code already exists.");
                    return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
                }

                if (await db.Courses.AnyAsync(c => c.Name == viewModel.Name && c.Id != viewModel.Id))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), "A course with the same name already exists.");
                    return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
                }

                if (viewModel.End != null && viewModel.End < viewModel.Start)
                {
                    ModelState.AddModelError(nameof(viewModel.End), "End date cannot be before start date.");
                }

                if (ModelState.IsValid)
                {
                    Course course = await db.Courses
                        .Include(c => c.Teachers)
                            .ThenInclude(ct => ct.Teacher)
                        .Where(c => c.Id == viewModel.Id)
                        .FirstOrDefaultAsync();

                    course.Code = viewModel.Code;
                    course.Name = viewModel.Name;
                    course.Section = viewModel.Section;
                    course.Start = viewModel.Start.ToDateTime();
                    course.End = viewModel.End?.ToDateTime();

                    course.Teachers.Clear();

                    foreach (int teacherId in viewModel.SelectedTeacherIds)
                    {
                        course.Teachers.Add(new CourseTeacher
                        {
                            CourseId = course.Id,
                            TeacherId = teacherId
                        });
                    }

                    db.Courses.Update(course);

                    await db.SaveChangesAsync();
                }
            }

            return Json(new[] { viewModel.ConvertToCourseViewModel(teachers) }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCourse([DataSourceRequest] DataSourceRequest request, DeleteCourseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Course course = await db.Courses
                    .Where(c => c.Id == viewModel.Id)
                    .FirstOrDefaultAsync();

                db.Courses.Remove(course);

                await db.SaveChangesAsync();
            }

            // Return the removed course. Also, return any validation errors through the ModelState.
            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }
    }
}
