using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class MePlayer : Player
    {
        public override void Init(CharData charData, GameMap gameMap)
        {
            base.Init(charData, gameMap);
            _gameMap.m_curObj = this;
        }

    }
}
