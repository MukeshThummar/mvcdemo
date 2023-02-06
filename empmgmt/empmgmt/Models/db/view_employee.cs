using Microsoft.EntityFrameworkCore;

namespace empmgmt.Models.db
{
    [Keyless]
    public class view_employee
    {
        
        public int emp_id { get; set; }

        public string emp_name { get; set; }

        public DateTime date_of_birth { get; set; }

        public DateTime joining_date { get; set; }

        public decimal salary { get; set; }

        public string designation { get; set; }

        public string email_id { get; set; }
        public string skills { get; set; }
    }
}
