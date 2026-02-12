using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KendoMvcDemo.Core.Entities;
using KendoMvcDemo.Core.Models.Course;
using KendoMvcDemo.Infrastructure.Database;
using KendoMvcDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KendoMvcDemo.Controllers
{
    public class CourseScheduleController : Controller
    {
        private readonly KaustDbContext db;

        public CourseScheduleController(KaustDbContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            CourseSchedulesViewModel viewModel = new()
            {
                Courses = await GetCoursesAsync()
            };

            return View(viewModel);
        }

        public async Task<JsonResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<CourseDto> courses = await GetCoursesAsync();

            List<CourseScheduleViewModel> schedules = await db.CourseSchedules
                .Select(x => new CourseScheduleViewModel
                {
                    Id = x.Id,
                    CourseId = x.CourseId,
                    Title = x.Title,
                    Description = x.Description,
                    Start = DateTime.SpecifyKind(x.Start, DateTimeKind.Utc),
                    End = DateTime.SpecifyKind(x.End, DateTimeKind.Utc),
                    IsAllDay = x.IsAllDay,
                    StartTimezone = x.StartTimezone,
                    EndTimezone = x.EndTimezone,
                    RecurrenceException = x.RecurrenceException,
                    RecurrenceId = x.RecurrenceId,
                    RecurrenceRule = x.RecurrenceRule,
                    Courses = courses
                }).ToListAsync();

            return Json(schedules.ToDataSourceResult(request));
        }

        public async Task<JsonResult> Create([DataSourceRequest] DataSourceRequest request, CourseScheduleViewModel viewModel)
        {
            if (viewModel.CourseId == 0)
            {
                ModelState.AddModelError(nameof(viewModel.CourseId), "Please select a course");
            }

            if (ModelState.IsValid)
            {
                CourseSchedule courseSchedule = new()
                {
                    CourseId = viewModel.CourseId,
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Start = viewModel.Start.ToUniversalTime(),
                    End = viewModel.End.ToUniversalTime(),
                    IsAllDay = viewModel.IsAllDay,
                    StartTimezone = viewModel.StartTimezone,
                    EndTimezone = viewModel.EndTimezone,
                    RecurrenceException = viewModel.RecurrenceException,
                    RecurrenceId = viewModel.RecurrenceId,
                    RecurrenceRule = viewModel.RecurrenceRule
                };

                db.CourseSchedules.Add(courseSchedule);

                await db.SaveChangesAsync();

                viewModel.Id = courseSchedule.Id;
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        public async Task<JsonResult> Update([DataSourceRequest] DataSourceRequest request, CourseScheduleViewModel viewModel)
        {
            if (viewModel.CourseId == 0)
            {
                ModelState.AddModelError(nameof(viewModel.CourseId), "Please select a course");
            }

            if (ModelState.IsValid)
            {
                CourseSchedule courseSchedule = await db.CourseSchedules
                    .Where(x => x.Id == viewModel.Id)
                    .FirstOrDefaultAsync();

                courseSchedule.CourseId = viewModel.CourseId;
                courseSchedule.Title = viewModel.Title;
                courseSchedule.Description = viewModel.Description;
                courseSchedule.Start = viewModel.Start;
                courseSchedule.End = viewModel.End;
                courseSchedule.IsAllDay = viewModel.IsAllDay;
                courseSchedule.StartTimezone = viewModel.StartTimezone;
                courseSchedule.EndTimezone = viewModel.EndTimezone;
                courseSchedule.RecurrenceException = viewModel.RecurrenceException;
                courseSchedule.RecurrenceId = viewModel.RecurrenceId;
                courseSchedule.RecurrenceRule = viewModel.RecurrenceRule;

                db.CourseSchedules.Update(courseSchedule);

                await db.SaveChangesAsync();
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        public async Task<JsonResult> Delete([DataSourceRequest] DataSourceRequest request, CourseScheduleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                CourseSchedule courseSchedule = await db.CourseSchedules
                    .Where(x => x.Id == viewModel.Id)
                    .FirstOrDefaultAsync();

                db.CourseSchedules.Remove(courseSchedule);

                await db.SaveChangesAsync();
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        private async Task<IEnumerable<CourseDto>> GetCoursesAsync()
        {
            List<CourseDto> courses = await db.Courses
                .Select(x => new CourseDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Start = x.Start,
                    End = x.End
                }).ToListAsync();

            return courses;
        }
    }
}
