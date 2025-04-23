namespace ProyectoU2025.Models.ViewModels
{
    public class ScheduleViewModel
    {
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public string Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Room { get; set; }
        public string Block { get; set; }
        public string Building { get; set; }
        public string Campus { get; set; }
        public DateTime? ChangeStartDate { get; set; }
        public DateTime? ChangeEndDate { get; set; }
        public string ChangeReason { get; set; }
    }


}
