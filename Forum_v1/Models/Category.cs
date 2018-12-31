using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Forum_v1.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }

    }

}