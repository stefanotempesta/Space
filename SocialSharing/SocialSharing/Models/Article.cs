using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialSharing.Models
{
    public class Article
    {
        public int Id { get; set; }

        [StringLength(200)]
        [Required, Index(IsUnique = true)]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Abstract { get; set; }
    }
}
