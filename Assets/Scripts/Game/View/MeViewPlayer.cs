using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class MeViewPlayer:ViewPlayer
    {
        public override void Create(CharData charData, ViewMap viewMap)
        {
            _viewMap = viewMap;
            m_gameObj = new MePlayer();
            m_gameObj.Init(charData, viewMap.LogicMap);
            //            gameGo = new GameObject();
            m_gameGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            m_gameGo.name = charData.m_name;
            m_gameGo.AddComponent<PlayerMoveController>();
            m_gameTrans = m_gameGo.transform;
        }
    }
}
