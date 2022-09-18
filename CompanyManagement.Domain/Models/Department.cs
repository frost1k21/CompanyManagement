using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Domain.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Department? HeadDepartment { get; set; }
        public List<Employee> Employees { get; set; } = new();
    }
}
