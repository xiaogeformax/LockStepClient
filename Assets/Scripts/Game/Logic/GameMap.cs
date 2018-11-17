using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
   public class GameMap
    {
        public int m_curFrameCount = 1;
        public int m_curRoleId = 0;
        public List<GameObj> m_gameObjList = new List<GameObj>();
        public GameObj m_curObj;


        private NetManager _netManager;

        public NetManager netManager
        {
            get
            {
                return _netManager;
            }
        }

        public void InitNet(string ip, int tcpPort, int udpPort)
        {
            _netManager = new NetManager();
            netManager.SetIpInfo(ip, tcpPort, udpPort);
            netManager.InitClient();
        }

        public void InputCmd(Cmd cmd, string param)
        {
            KeyData keyData = new KeyData(cmd, param, m_curRoleId);
            _netManager.AddKeyPack(keyData);
        }

        public void DoCmd(Cmd cmd, string param, int roleId)
        {
            switch (cmd)
            {
                case Cmd.UseSkill:
                    for (int i = 0; i < m_gameObjList.Count; ++i)
                    {
                        if (m_gameObjList[i].mCharData.m_roleId == roleId)
                        {
                           (m_gameObjList[i] as Player).DoSkill(int.Parse(param));
                        }
                    }
                    break;
                case Cmd.Move:
                    break;
                case Cmd.Turn:
                    break;
                default:
                    Debug.LogError("无效命令");
                    break;
            }
        }

        public void DoCmd(KeyData keyData)
        {
            Debug.LogError("执行关键帧 " + keyData.ToString());
            DoCmd(keyData.m_cmd, keyData.m_data, keyData.m_roleId);
        }

        public void Init()
        {
            m_gameObjList = new List<GameObj>();
        }

        public void Update()
        {
            for (int i = 0; i < m_gameObjList.Count; ++i)
            {
                m_gameObjList[i].Update();
            }

            if (_netManager != null)
            {
                _netManager.Update();
            }
        }
    }
}
