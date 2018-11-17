using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
   public class GameObj
    {

        protected int m_Id = 10000;
        protected Vector3 m_pos = Vector3.zero;
        protected Vector3 m_direction = Vector3.zero;
        protected GameMap _gameMap;
         protected CharData _charData;

        public virtual void Init(CharData charData, GameMap gameMap)
        {
            _charData = charData;
            _gameMap = gameMap;
            m_Id = charData.m_roleId;
        }

        public CharData mCharData
        {
            get
            {
                return _charData;
            }
        }

        public virtual void Update()
        {

        }
    }
}
