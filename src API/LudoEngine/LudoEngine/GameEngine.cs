using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine
{
    public class GameEngine : IGameEngine
    {
        private IGameHelper _gameHelper;


        public GameEngine(IGameHelper r)
        {
            _gameHelper = r;
        }

        public List<Game> LoadGames(string v)
        {
            return _gameHelper.LoadGames(v);
        }

        public bool PushGame(string name, string key, string gameName)
        {
            if(_gameHelper.PushGame(name, key, gameName))
            {
                return true;
            }
            return false;
        }

        public bool PushGame(string name,string gameName)
        {
            if(_gameHelper.PushGame(name, gameName))
            {
                return true;
            }
            return false;
        }

        public void RemoveGame(string desc)
        {
            _gameHelper.RemoveGame(desc);
        }



    }
}
