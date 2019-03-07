using LudoApp.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace LudoApp.Models
{
    public class GameModel
    {
        
        [Required]
        [StringLength(50)]
        public string GameName { get; set; }

        [Required]
        [CheckGameAttribute]
        public string GameStatus { get; set; }

        [Required]
        [ListRange(2,4)]
        public List<PlayerModel> Players { get; set; }

        public int CurrentTurn { get; set; }

        //not using id, we identify ID by name, restricting identical GameModel.GameName To exists
        public Guid Id { get; set; }
    }
}
