using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class ViewObj
    {
        public GameObj m_gameObj;
        public GameObject m_gameGo;
        protected ViewMap _viewMap;
        protected Transform m_gameTrans;


        public virtual void Create(CharData charData, ViewMap viewMap)
        {
            // _viewMap = viewMap;
            m_gameObj = new GameObj();
            m_gameObj.Init(charData, viewMap.LogicMap);
            m_gameGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            m_gameGo.name = charData.m_name;
            m_gameTrans = m_gameGo.transform;

        }

        public int Id
        {
            get
            {
                if (m_gameObj == null)
                {
                    return -1;
                }
                return m_gameObj.mCharData.m_roleId;
            }
        }

        public Vector3 Pos
        {
            get
            {
                if (m_gameTrans == null)
                {
                    return Vector3.zero;
                }
                return m_gameTrans.position;
            }
            set
            {
                if (m_gameTrans != null)
                {
                    m_gameTrans.position = value;
                }
            }
        }

        public Vector3 EulerAngles
        {
            get
            {
                if (m_gameTrans == null)
                {
                    return Vector3.zero;
                }
                return m_gameTrans.localEulerAngles;
            }
            set
            {
                m_gameTrans.localRotation = Quaternion.Euler(value);
            }
        }


        public virtual void Update()
        {
            if (m_gameObj == null)
            {
                return;
            }
            m_gameObj.Update();
        }
    }
}
