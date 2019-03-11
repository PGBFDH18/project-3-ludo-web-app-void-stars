using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LudoApp.Models
{
    public class PlayerModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Player Name not alloew to exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        public string PlayerColor { get; set; }

        [Required]
        public List<PieceModel> Pieces { get; set; }

        [Required]
        public int TurnOrder { get; set; }

    }
}
