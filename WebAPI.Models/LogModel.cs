using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class LogModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Text { get; set; }
    }
}
