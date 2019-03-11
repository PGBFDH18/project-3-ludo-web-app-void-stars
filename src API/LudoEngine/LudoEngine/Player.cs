using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine
{   
    public class Player
    {
        public string Name { get; private set; }
        public Color PlayerColor { get; private set; }
        public int turnOrder { get; private set; }
        public List<Piece> Pieces { get; set; }

        public Player(int turnOrder, Color Color, string name)
        {
            this.PlayerColor = Color;
            this.turnOrder = turnOrder;
            this.Name = name;
            Pieces = new List<Piece>();

            InitPieces();
        }

        public void movePiece(int id, int rollResult)
        {
            Pieces.Where(x => x.ID == id).First().movePiece(rollResult);
        }

        private void InitPieces()
        {
            for(int i = 0; i < 4; i++)
            {
                Pieces.Add(new Piece(i, PlayerColor));
            }
        }

        public bool playerHasWon()
        {
            return Pieces.Where(x => x.State == PieceState.Goal)
                .Count() == 4 ? true : false; 
        }
        
    }
}
