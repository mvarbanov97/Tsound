using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TSound.Data.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }
    }
}
