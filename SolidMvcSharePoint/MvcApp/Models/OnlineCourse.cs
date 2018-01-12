using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidMvcSharePoint.Models
{
    public class OnlineCourse : CourseTemplate
    {
        public Uri LaunchUrl { get; set; }

        public override string Description => "Online course not available";
    };
}