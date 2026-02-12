using Kendo.Mvc.UI;
using KendoMvcDemo.Core.Models.Course;

namespace KendoMvcDemo.ViewModels
{
    public class CourseScheduleViewModel : ISchedulerEvent
    {
        private DateTime start;
        private DateTime end;

        public int Id { get; set; }

        public int CourseId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsAllDay { get; set; }

        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value.ToUniversalTime();
            }
        }

        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value.ToUniversalTime();
            }
        }


        public string StartTimezone { get; set; }

        public string EndTimezone { get; set; }

        public string RecurrenceRule { get; set; }

        public string RecurrenceException { get; set; }

        public int? RecurrenceId { get; set; }

        public IEnumerable<CourseDto> Courses { get; set; } = [];
    }
}
