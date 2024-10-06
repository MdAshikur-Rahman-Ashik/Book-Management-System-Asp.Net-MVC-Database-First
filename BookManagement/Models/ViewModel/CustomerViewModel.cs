using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookManagement.Models.ViewModel
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {
            this.BookList = new List<int>();
        }
        public int CustomerId { get; set; }
        [Required,Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Required,Display(Name = "Birth Date"),DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        [ValidDate]
        public System.DateTime BirthDate { get; set; }
        public bool IsRegular { get; set; }
        public string Picture { get; set; }
        public HttpPostedFileBase PicturePath { get; set; }
        public List<int> BookList { get; set; }
    }
}