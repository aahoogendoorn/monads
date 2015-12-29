using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monads.Test
{
    public class EmployeeRepository
    {
        public Employee Create(string name){

            return new Employee { Name = name };
        }
    }
}
