using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LudoApp.Models
{
    public class PieceModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Position { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Color { get; set; }

    }
}
