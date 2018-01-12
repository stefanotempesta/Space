using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidMvcSharePoint.Models
{
    public class Content
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string Name { get; set; }

        public ContentMimeType ContentType { get; set; }
    }

    public enum ContentMimeType
    {
        Undefined,
        Audio,
        Video,
        Document,
        Simulation,
        URL
    }
}
