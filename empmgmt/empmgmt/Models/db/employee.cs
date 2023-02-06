using System.ComponentModel.DataAnnotations;

namespace empmgmt.Models.db
{
    public class employee
    {
        [Key]
        public int emp_id { get; set; }

        public string emp_name { get; set; }

        public DateTime date_of_birth { get; set; }

        public DateTime joining_date { get; set; }

        public decimal salary { get; set; }

        public string designation { get; set; }

        public string email_id { get; set; }

    }
}
