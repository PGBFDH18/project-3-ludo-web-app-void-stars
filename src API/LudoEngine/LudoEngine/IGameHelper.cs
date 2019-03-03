using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine
{
    public interface IGameHelper
    {
        

        List<Game> LoadGames(string v);

        bool PushGame(string name, string key, string gameName);
        bool PushGame(string name, string gameName);
        

        void RemoveGame(string desc);

    }
}
