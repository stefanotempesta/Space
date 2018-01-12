using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidMvcSharePoint.Models
{
    public abstract class CourseTemplate
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual IEnumerable<Content> ContentItems { get; set; }
    }
}