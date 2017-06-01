using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleConsole
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

    }

    public class PersonDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

    }

    public class AnagrDto :Person { }
}
