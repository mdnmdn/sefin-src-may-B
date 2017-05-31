using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.Importer.Common
{
    
    [AttributeUsage(AttributeTargets.Class)]
    public class JobDescriptionAttribute: Attribute
    {
        public JobDescriptionAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; set; }

        public string Notes { get; set; }
    }
}
