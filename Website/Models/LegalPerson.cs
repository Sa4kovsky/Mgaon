using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models
{
    public class LegalPerson : Person
    {
        public string NameLegal { get; set; }
    }
}
