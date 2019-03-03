using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine
{
   
    

    public class Piece
    {
        
        public int ID { get; set; }
        public int Position { get; set; }
        public PieceState State { get; set; }
        public Color PieceColor { get; private set; }

        private int SpaceFromGoal { get; set; }


        public Piece(int ID, Color color)
        {
            this.ID = ID;
            State = PieceState.Fence;
            PieceColor = color;
            SpaceFromGoal = 57;
            determineFencePos();
            
        }

        private void determineFencePos()
        {
            switch (PieceColor)
            {
                case Color.Red:
                    {
                        Position = -1;
                        break;
                    }
                case Color.Green:
                    {
                        Position = -2;
                        break;
                    }
                case Color.Yellow:
                    {
                        Position = -3;
                        break;
                    }
                case Color.Blue:
                    {
                        Position = -4;
                        break;
                    }
            }
        }

        private void DetermineStartPos()
        {
            switch(PieceColor)
            {
                case Color.Red:
                    {
                        Position = 1;
                        break;
                    }
                case Color.Green:
                    {
                        Position = 14;
                        break;
                    }
                case Color.Yellow:
                    {
                        Position = 27;
                        break;
                    }
                case Color.Blue:
                    {
                        Position = 40;
                        break;
                    }
            }
        }

        public void movePiece(int spaces)
        {
            if (PieceState.Board == State)
            {

                if (Position + spaces > 52)
                {
                    Position += spaces - 52;
                }
                else
                {
                    Position += spaces;
                }

                SpaceFromGoal -= spaces;

                if (SpaceFromGoal <= 0)
                {
                    State = PieceState.Goal;
                }
            }
            else
            {
                if (spaces == 6)
                {
                    State = PieceState.Board;
                    DetermineStartPos();
                    movePiece(6);
                }
                else if (spaces == 1)
                {
                    State = PieceState.Board;
                    DetermineStartPos();
                    movePiece(1);
                }
            }

        }
    }
}
