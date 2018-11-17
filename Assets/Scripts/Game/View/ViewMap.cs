using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class ViewMap
    {
        private GameMap _logicMap;
        public GameMap LogicMap
        {
            get
            {
                return _logicMap;
            }
        }

        private ViewObj _curViewObj = null;
        public ViewObj CurViewObj
        {
            get
            {
                return _curViewObj;
            }
            set
            {
                _curViewObj = value;
            }
        }


        private List<ViewObj> m_viewOjbList = new List<ViewObj>();

        public void Init()
        {
            _logicMap = new GameMap();
            m_viewOjbList = new List<ViewObj>();
        }
        /// <summary>
        /// 创建一个实体
        /// </summary>
        /// <param name="charData"></param>
        public void CreateViewObj(CharData charData)
        {
            ViewObj obj = new ViewObj();
            obj.Create(charData, this);
            m_viewOjbList.Add(obj);

            _logicMap.m_gameObjList.Add(obj.m_gameObj);

        }

        /// <summary>
        /// 创建自己
        /// </summary>
        /// <param name="charData"></param>
        public void CreateMe(CharData charData)
        {
            MeViewPlayer obj = new MeViewPlayer();
            obj.Create(charData, this);
            m_viewOjbList.Add(obj);
            _logicMap.m_gameObjList.Add(obj.m_gameObj);
            CurViewObj = obj;
        }

        /// <summary>
        /// 创建一个玩家
        /// </summary>
        /// <param name="charData"></param>
        public void CreatePlayer(CharData charData)
        {
            ViewPlayer obj = new ViewPlayer();
            obj.Create(charData, this);
            m_viewOjbList.Add(obj);
            _logicMap.m_gameObjList.Add(obj.m_gameObj);
        }

        /// <summary>
        /// 创建所有玩家
        /// </summary>
        /// <param name="players"></param>
        public void CreateAllPlayer(string players)
        {
            string[] playStr = players.Split(';');
            for (int i = 0; i < playStr.Length; ++i)
            {
                CharData charData = new CharData(playStr[i]);
                if (charData.m_roleId == LogicMap.m_curRoleId)
                {
                    CreateMe(charData);
                }
                else
                {
                    CreatePlayer(charData);
                }
            }
        }

        public void Update()
        {
            _logicMap.Update();
            for (int i = 0; i < m_viewOjbList.Count; ++i)
            {
                m_viewOjbList[i].Update();
            }
        }

        /// <summary>
        /// 同步位置
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="pos"></param>
        public void SyncPos(int roleId, string pos)
        {
            string[] str = pos.Split('#');


            float x = float.Parse(str[0]);
            float y = float.Parse(str[1]);
            float z = float.Parse(str[2]);


            float angleX = float.Parse(str[3]);
            float angleY = float.Parse(str[4]);
            float angleZ = float.Parse(str[5]);

            Vector3 cPos = new Vector3(x, y, z);
            Vector3 cAngle = new Vector3(angleX, angleY, angleZ);

            for (int i = 0; i < m_viewOjbList.Count; i++)
            {
                if (m_viewOjbList[i].m_gameObj.mCharData.m_roleId == roleId)
                {
                    m_viewOjbList[i].Pos = cPos;
                    m_viewOjbList[i].EulerAngles = cAngle;
                    break;
                }
            }

        }
    }
}
