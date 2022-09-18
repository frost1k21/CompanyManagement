using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Domain.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public Position Position { get; set; }
        public Department Department { get; set; }
    }
}
