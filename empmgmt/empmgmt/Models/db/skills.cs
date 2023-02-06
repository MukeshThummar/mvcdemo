using System.ComponentModel.DataAnnotations;

namespace empmgmt.Models.db
{
    public class skills
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }
}
