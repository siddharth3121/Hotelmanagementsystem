using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotelmanagementsystem.Models
{
    public class Adminmodel
    {
        [Required(ErrorMessage = "Please Enter Your Mobile Number")]
        public string vmobnumber { get; set; }

        [Required(ErrorMessage = "Please Enter Your Password")]
        public string vpassword { get; set; }

        [Required(ErrorMessage ="Please Select User Type")]
        public string vusertype { get; set; }

        [Required(ErrorMessage ="Please Enter Your User Name")]
        public string vusername { get; set; }

        [Required(ErrorMessage = "NEW Password is required")]
        //[Range(10, 10, ErrorMessage = "Mobile number not more than or less than 10 digits.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [DataType(DataType.Password)]
        public string vnewpassword { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]   
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [DataType(DataType.Password)]
        [Compare("vnewpassword", ErrorMessage = "New password and confirm password does not match. Please check again.")]
        public string vnewConfirmPassword { get; set; }

        public int nid { get; set; }

        public string vpassport { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Adhar Card must be a natural number")]
        public string vadharcard { get; set; }

        public string vgender { get; set; }

        public string vnationality { get; set; }
    }
}