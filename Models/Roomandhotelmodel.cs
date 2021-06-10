using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotelmanagementsystem.Models
{
    public class Roomandhotelmodel
    {
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Count must be a natural number")]
        //[Range(10, 10, ErrorMessage = "Mobile number not more than or less than 10 digits.")]

        [Required(ErrorMessage = "Customer Mobile Number is required")]
        public string customermobilenum { get; set; }

        [Required(ErrorMessage = "Customer Name is required")]
        public string customername { get; set; }

        [Required(ErrorMessage = "Checkin date is required")]
        public string checkindate { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        public string checkoutdate { get; set; }
        public string checkroomandhall { get; set; }
        public string availablerooms { get; set; }
        public string availablehall { get; set; }
        public string roomandhalltype { get; set; }
        public string discount { get; set; }
        public string SGSTandCGST { get; set; }
        public string Total { get; set; }
        public string radiohotel { get; set; }
        public string radiohall { get; set; }
        public string Finalamount { get; set; }
        public string Availableroomandhall { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }

    }
}