using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace prueba_salud.Models
{
    public class productsVm
    {
        public long id { get; set; }

        [Required]
        //[StringLength(100)]
        [Display(Name ="product1")]
        public string product1 { get; set; }

        [Required]
        [Display(Name ="description")]
        public string description { get; set; }

        [Required]        
        //[MaxLength(4, ErrorMessage = "La cantidad maxima de caracteres es 4")]
        [Display(Name ="price")]
        public decimal price { get; set; }

        [Required]
        [Display(Name = "category_id")]
        public long category_id { get; set; }

        //[Required]
        public System.DateTime date_in { get; set; }


        public product product { get; set; }        
        public category category { get; set; }        
        
    }
}