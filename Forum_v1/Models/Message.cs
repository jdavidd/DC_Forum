using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forum_v1.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required(ErrorMessage = "Mesajul nu poate fi gol")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int SubjectID { get; set; }
        public Subject Subject { get; set; }
    }
}