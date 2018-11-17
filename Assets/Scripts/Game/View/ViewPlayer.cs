using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class ViewPlayer : ViewObj
    {
        public override void Create(CharData charData, ViewMap viewMap)
        {
            _viewMap = viewMap;
            m_gameObj = new Player();
            m_gameObj.Init(charData, viewMap.LogicMap);
            m_gameGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            m_gameGo.name = charData.m_name;
            m_gameTrans = m_gameGo.transform;
        }
    }
}
