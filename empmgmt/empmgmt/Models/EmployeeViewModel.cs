namespace empmgmt.Models
{
    public class EmployeeViewModel
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
