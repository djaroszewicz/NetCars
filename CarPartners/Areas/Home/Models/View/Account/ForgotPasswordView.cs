using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCars.Areas.Home.Models.View.Account
{
    public class ForgotPasswordView
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
