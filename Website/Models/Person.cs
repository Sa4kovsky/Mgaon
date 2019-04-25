using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models
{
    public class Person
    {
       
        public string Designation { get; set; }
        [Required(ErrorMessage =  "Укажите полностью ФИО")]
        [RegularExpression(@"^([А-ЯA-Z]|[А-ЯA-Z][\x27а-яa-z]{1,}|[А-ЯA-Z][\x27а-яa-z]{1,}\-([А-ЯA-Z][\x27а-яa-z]{1,}|(оглы)|(кызы)))\040[А-ЯA-Z][\x27а-яa-z]{1,}(\040[А-ЯA-Z][\x27а-яa-z]{1,})?$", ErrorMessage = "Укажите полностью ФИО")]
        public string Name { get; set; }
    
        public string Address { get; set; }
      
        public string Message { get; set; }
   
        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
