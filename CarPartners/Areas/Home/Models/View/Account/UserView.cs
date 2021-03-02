using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarPartners.Areas.Home.Models.View.Account
{
    public class UserView
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Uzupełnij login użytkownika")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Uzupełnij E-mail użytkownika")]
        public string Email { get; set; }
    }
}
