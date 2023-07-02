using System;

namespace detskiysad__.Entities
{
    public class ChildForPrint
    {
        public string FullName { get; set; }
        public int Attendance { get; set; }
        public DateTime? Date { get; set; }

        public ChildForPrint(Сhildren child, DateTime? date)
        {
            FullName = child.FullName;
            if (child.Attendance == 1)
            {
                child.Attendance = 12;
            }
            Attendance = child.Attendance;
            this.Date = date == null ? DateTime.Today : date;
        }
    }
}
