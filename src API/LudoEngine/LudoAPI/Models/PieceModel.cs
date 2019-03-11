using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoAPI.Models
{
    public class PieceModel
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public string State { get; set; }
        public string Color { get; set; }

        public PieceModel(LudoEngine.Piece piece)
        {
            Id = piece.ID;
            Position = piece.Position;
            State = piece.State.ToString();
            Color = piece.PieceColor.ToString();
        }
    }
}
