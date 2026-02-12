using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KendoMvcDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KendoMvcDemo.Controllers
{
    public class GanttChartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public virtual JsonResult ReadTasks([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetAllTasks().ToDataSourceResult(request));
        }

        public virtual JsonResult DestroyTask([DataSourceRequest] DataSourceRequest request, GanttTaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                var newTasks = GetAllTasks().Where(t => t.Id != task.Id);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult CreateTask([DataSourceRequest] DataSourceRequest request, GanttTaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                task.Id = GetAllTasks().Last().Id + 1;
                var newTasks = GetAllTasks().ToList();
                newTasks.Add(task);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult UpdateTask([DataSourceRequest] DataSourceRequest request, GanttTaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                var newTasks = GetAllTasks().Where(t => t.Id != task.Id).ToList();
                newTasks.Add(task);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        private IEnumerable<GanttTaskViewModel> GetAllTasks()
        {
            List<GanttTaskViewModel> ganttTasks = new List<GanttTaskViewModel>
            {
                new GanttTaskViewModel
                {
                    Id = 7,
                    Title = "Software validation, research and implementation",
                    ParentId = null,
                    OrderId = 0,
                    Start = new DateTime(2025, 6, 1, 3, 0, 0),
                    End = new DateTime(2025, 6, 18, 3, 0, 0),
                    PlannedStart = new DateTime(2025, 6, 1, 3, 0, 0),
                    PlannedEnd = new DateTime(2025, 6, 12, 3, 0, 0),
                    PercentComplete = 0.43M,
                    Summary = true,
                    Expanded = true
                },

                new GanttTaskViewModel
                {
                    Id = 18,
                    Title = "Project Kickoff",
                    ParentId = 7,
                    OrderId = 0,
                    Start = new DateTime(2025, 6, 1, 3, 0, 0),
                    End = new DateTime(2025, 6, 1, 3, 0, 0),
                    PlannedStart = new DateTime(2025, 6, 1, 3, 0, 0),
                    PlannedEnd = new DateTime(2025, 6, 1, 3, 0, 0),
                    PercentComplete = 0.23M,
                    Summary = false,
                    Expanded = true
                },
                new GanttTaskViewModel
                {
                    Id = 13,
                    Title = "Implementation",
                    ParentId = 7,
                    OrderId = 1,
                    Start = new DateTime(2025, 6, 3, 3, 0, 0),
                    End = new DateTime(2025, 6, 17, 3, 0, 0),
                    PlannedStart = new DateTime(2025, 6, 2, 3, 0, 0),
                    PlannedEnd = new DateTime(2025, 6, 17, 3, 0, 0),
                    PercentComplete = 0.77M,
                    Summary = true,
                    Expanded = true
                },
                new GanttTaskViewModel
                {
                    Id = 100,
                    Title = "Test",
                    ParentId = 13,
                    OrderId = 0,
                    Start = new DateTime(2025, 6, 4, 3, 0, 0),
                    End = new DateTime(2025, 6, 5, 1, 0, 0),
                    PlannedStart = new DateTime(2025, 6, 4, 3, 0, 0),
                    PlannedEnd = new DateTime(2025, 6, 5, 1, 0, 0),
                    PercentComplete = 0.3M,
                    Summary = false,
                    Expanded = true
                },
                new GanttTaskViewModel
                {
                    Id = 24,
                    Title = "Prototype",
                    ParentId = 13,
                    OrderId = 0,
                    Start = new DateTime(2025, 6, 3, 3, 0, 0),
                    End = new DateTime(2025, 6, 5, 3, 0, 0),
                    PlannedStart = new DateTime(2025, 6, 3, 3, 0, 0),
                    PlannedEnd = new DateTime(2025, 6, 6, 3, 0, 0),
                    PercentComplete = 0.77M,
                    Summary = false,
                    Expanded = true
                },
                new GanttTaskViewModel
                {
                    Id = 26,
                    Title = "Architecture",
                    ParentId = 13,
                    OrderId = 1,
                    Start = new DateTime(2025, 6, 5, 3, 0, 0),
                    End = new DateTime(2025, 6, 7, 3, 0, 0),
                    PlannedStart = new DateTime(2025, 6, 4, 3, 0, 0),
                    PlannedEnd = new DateTime(2025, 6, 6, 3, 0, 0),
                    PercentComplete = 0.82M,
                    Summary = false,
                    Expanded = true
                },
                new GanttTaskViewModel
                {
                    Id = 27,
                    Title = "Data Layer",
                    ParentId = 13,
                    OrderId = 2,
                    Start = new DateTime(2025, 6, 3, 3, 0, 0),
                    End = new DateTime(2025, 6, 8, 3, 0, 0),
                    PlannedStart = new DateTime(2025, 6, 2, 3, 0, 0),
                    PlannedEnd = new DateTime(2025, 6, 8, 3, 0, 0),
                    PercentComplete = 1.00M,
                    Summary = false,
                    Expanded = true
                },
                new GanttTaskViewModel
                {
                    Id = 28,
                    Title = "Unit Tests",
                    ParentId = 13,
                    OrderId = 4,
                    Start = new DateTime(2025, 6, 9, 3, 0, 0),
                    End = new DateTime(2025, 6, 11, 3, 0, 0),
                    PlannedStart = new DateTime(2025, 6, 8, 3, 0, 0),
                    PlannedEnd = new DateTime(2025, 6, 10, 3, 0, 0),
                    PercentComplete = 0.68M,
                    Summary = false,
                    Expanded = true
                },
                new GanttTaskViewModel
                {
                    Id = 29,
                    Title = "UI and Interaction",
                    ParentId = 13,
                    OrderId = 5,
                    Start = new DateTime(2025, 6, 5, 3, 0, 0),
                    End = new DateTime(2025, 6, 9, 3, 0, 0),
                    PlannedStart = new DateTime(2025, 6, 4, 3, 0, 0),
                    PlannedEnd = new DateTime(2025, 6, 7, 3, 0, 0),
                    PercentComplete = 0.60M,
                    Summary = false,
                    Expanded = true
                },
            };

            return ganttTasks;
        }

        public virtual JsonResult ReadDependencies([DataSourceRequest] DataSourceRequest request)
        {
            List<GanttDependencyViewModel> dependencies = new List<GanttDependencyViewModel>()
            {
                new GanttDependencyViewModel() { DependencyID = 1, PredecessorID = 24 , SuccessorID = 26, Type = DependencyType.FinishStart },
                new GanttDependencyViewModel() { DependencyID = 2, PredecessorID = 26 , SuccessorID = 27, Type = DependencyType.FinishStart },

            };

            var result = dependencies.Select(o =>
                new
                {
                    DependencyID = o.DependencyID,
                    PredecessorID = o.PredecessorID,
                    SuccessorID = o.SuccessorID,
                    Type = (int)o.Type
                }).ToList();
            return Json(result.ToDataSourceResult(request));
        }

        public virtual JsonResult CreateDependency([DataSourceRequest] DataSourceRequest request, GanttDependencyViewModel task)
        {
            if (ModelState.IsValid)
            {
                // ... 
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult UpdateDependency([DataSourceRequest] DataSourceRequest request, GanttDependencyViewModel task)
        {
            if (ModelState.IsValid)
            {
                // ...
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult DestroyDependency([DataSourceRequest] DataSourceRequest request, GanttDependencyViewModel task)
        {
            if (ModelState.IsValid)
            {
                // ...
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }
    }
}
