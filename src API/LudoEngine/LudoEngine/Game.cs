using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine
{

   public class Game
    {

        
        public string NameOfGameInstance { get; set; }
        public Guid Id { get; set; }
        public int CurrentTurn { get; set; }
        public int MaxTurn { get; set; }
        public List<Player> Players { get; set; }
        public int LatestRoll { get; set; } = 0;

        public Utils GameStatus { get; private set; }
        public string PrivateKey { get;private set; }
        public string Victor { get;private set; }
        
        

        public Game(string privateKey, string playerName, string gameName)
        {
            PrivateKey = privateKey;
            this.Id = Guid.NewGuid();
            GameStatus = Utils.Lobby;
            NameOfGameInstance = gameName;
            Players = new List<Player>();
            AddPlayer(playerName);
        }

        public Game(string playerName, string gameName)
        {
            this.Id = Guid.NewGuid();
            GameStatus = Utils.Lobby;
            NameOfGameInstance = gameName;
            Players = new List<Player>();
            AddPlayer(playerName);
        }

        public bool StartGame()
        {
            if (GameStatus == Utils.Lobby &&
                (Players.Count <= 4 && Players.Count >= 2))
            {
                GameStatus = Utils.Running;
                CurrentTurn = 1;
                return true;
            }
            else
                return false;
            
        }
        
        public void AddPlayer(string name)
        {
            foreach(Player p in Players)
            {
                if(name == p.Name)
                {
                    return;
                }
            }

            if(Players.Count != 4)
            Players.Add(new Player(Players.Count+1, DecidePlayerColor(Players.Count+1), name));
        }

        public Player hasPlayerWon()
        {
            Player tempVictor = Players.Where(x => x.playerHasWon() == true).FirstOrDefault();
            if (tempVictor == null)
                return null;
            else
            {
                Victor = tempVictor.Name;
                return tempVictor;
            }
                

        }

        private Color DecidePlayerColor(int PlayerCount)
        {
            switch(PlayerCount)
            {
                case 1: return Color.Yellow;
                case 2: return Color.Blue;
                case 3: return Color.Red;
                case 4: return Color.Green;
                default: return Color.Yellow;
            }
        }

        public void PlayerMovePiece(string pieceId)
        {
            Players.Where(x => x.turnOrder == CurrentTurn).
                First().movePiece(int.Parse(pieceId), LatestRoll);

           if(hasPlayerWon() != null)
            {
                GameStatus = Utils.Won;
            }

            PassTurn();
        }

        public void RemovePlayer(string name)
        {
            if(Players.Count != 2)
            Players.Remove(
                Players.Where(x => x.Name == name).First()
                );
        }

        public void PassTurn()
        {
            if (CurrentTurn == Players.Count)
                CurrentTurn = 1;
            else
                CurrentTurn++;
        }

        

        

        

       
        
        
    }
}
