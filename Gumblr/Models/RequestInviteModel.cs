using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class RequestInviteModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Your email address")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Comments")]
        public string Comments { get; set; }

        [Required]
        [Display(Name = "Your name")]
        public string Name { get; set; }
    }
}