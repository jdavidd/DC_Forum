using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forum_v1.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }
        [Required]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int CategoryID{ get; set; }
        public Category Category { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}