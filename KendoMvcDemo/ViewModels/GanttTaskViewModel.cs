using Kendo.Mvc.UI;

namespace KendoMvcDemo.ViewModels
{
    public class GanttTaskViewModel : IGanttTask
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }

        public string Title { get; set; }

        private DateTime start;
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

        private DateTime end;
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

        private DateTime plannedStart;
        public DateTime PlannedStart
        {
            get
            {
                return plannedStart;
            }
            set
            {
                plannedStart = value.ToUniversalTime();
            }
        }

        private DateTime plannedEnd;
        public DateTime PlannedEnd
        {
            get
            {
                return plannedEnd;
            }
            set
            {
                plannedEnd = value.ToUniversalTime();
            }
        }

        public bool Summary { get; set; }
        public bool Expanded { get; set; }
        public decimal PercentComplete { get; set; }
        public int OrderId { get; set; }
    }

    public class GanttDependencyViewModel : IGanttDependency
    {
        public int DependencyID { get; set; }
        public int PredecessorID { get; set; }
        public int SuccessorID { get; set; }
        public DependencyType Type { get; set; }
    }
}
