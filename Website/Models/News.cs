using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Website.Models
{
    public class News
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "Новость")]
        [DataType(DataType.MultilineText)]
        public string Title { get; set; } // Название и текст новости
        [Display(Name = "Картинка")]
        [DataType(DataType.MultilineText)]
        public string Images { get; set; } // Картинка 
        [Display(Name = "Организация")]
        [DataType(DataType.MultilineText)]
        public string Organization { get; set; } // Организация выполняющая действия 
        [Display(Name = "Должность")]
        [DataType(DataType.MultilineText)]
        public string Position { get; set; } // Должность ответственного
        [Display(Name = "Фио")]
        [DataType(DataType.MultilineText)]
        public string Name { get; set; } // Фио ответственого
        [Display(Name = " Дата начало проведения")]
        public DateTime DateStart { get; set; } // Дата начало проведения
        [Display(Name = " Дата конца проведения ")]
        public DateTime DateFinish { get; set; } // Дата конца проведения
        public string Language { get; set; } 
    }
}
