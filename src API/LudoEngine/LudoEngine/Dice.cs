using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine
{
    public class Dice
    {
        private Random _random = new Random(Guid.NewGuid().GetHashCode());

        public int Roll()
        {
            return _random.Next(1, 7);
        }
    }
}
