using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzureRedisWebApplication.Models
{
    public class Contact
    {
        public Guid Id { get; set; }

        [DisplayName("Contact Name")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Country { get; set; }
    }
}