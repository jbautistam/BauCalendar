using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace RudiGrobler.Calendar.Common
{
    public class Appointments : ObservableCollection<Appointment>
    {
        public Appointments()
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 30, 0);
            Add(new Appointment() { Subject = "Dummy Appointment", StartTime = date, EndTime = date+TimeSpan.FromHours(1) });
        }
    }
}
