using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Day2.Task1.Models
{
    public class Department
    {
        [Key]
       public int DepartmentId { get; set; }
       public string Name { get; set; }
       public string Description { get; set; }

        public ICollection<Doctor>? Doctors { get; set; }
    }
}
