using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine
{
    public class GameHelper : IGameHelper
    {
        private string[] QuaryParamter{ get; set; } = { "All"};

        public List<Game> LoadGames(string desc)
        {
            List<Game> _foundGames = new List<Game>();

            if (desc == "All")
            {
                return FakeDataBase.Games;
            }

            foreach (Game game in FakeDataBase.Games)
            {
                
                if (desc == game.NameOfGameInstance &&
                !QuaryParamter.Where(x => x != desc).Equals(Array.Empty<string>()))
                {
                    _foundGames.Add(game); 
                    return _foundGames;
                }
                
                
            }
            
            return _foundGames;
        }

        public void RemoveGame(string desc)
        {
            foreach(Game game in FakeDataBase.Games.ToList())
            {
                if(desc == game.NameOfGameInstance &&
                  !QuaryParamter.Where(x => x == desc).Equals(Array.Empty<string>()))
                {
                    FakeDataBase.Games.Remove(game);
                }
            }
        }

        public bool PushGame(string name, string key, string gameName)
        {
            if (FakeDataBase.Games.Where(x => x.NameOfGameInstance == gameName).Equals(Array.Empty<string>()))
            {
                FakeDataBase.Games.Add(new Game(name, key, gameName));
                return true;
            }

            return false;
              
               
        }

        public bool PushGame(string name, string gameName)
        {
            if (FakeDataBase.Games.Where(x => x.NameOfGameInstance == gameName).ToList().Count == 0)
            {
                FakeDataBase.Games.Add(new Game(name, gameName));
                return true;
            }

            return false;
        }


       
    }
}
