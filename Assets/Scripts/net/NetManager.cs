using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using net;


namespace Game
{
    public class NetManager
    {
        private string _ip = "127.0.0.1";
        private int _tcpPort = 1255;
        private int _udpPort = 1337;


        AsycUdpClient m_client;

        private List<string> m_keyPack = new List<string>();

        public void AddKeyPack(KeyData data)
        {
            m_keyPack.Add(data.ToString());
        }

        public void ClearKeyPack()
        {
            m_keyPack.Clear();
        }

        public void SetIpInfo(string ip = "127.0.0.1", int tcpPort = 1255, int udpPort = 1337)
        {
            _ip = ip;
            _tcpPort = tcpPort;
            _udpPort = udpPort;
        }

        public void InitClient()
        {
            m_client = new AsycUdpClient();
            m_client.OnConnect += OnConnect;
            m_client.OnDisconnect += OnDisconnect;
            m_client.OnMessage += OnMessage;
        }

        public void Update()
        {
            m_client.Update();
        }
        //Todo 同步位置和关键帧


        /// <summary>
        /// 客户端准备
        /// </summary>
        public void Ready()
        {
            MessageBuffer msgBuf = new MessageBuffer();
           /* msgBuf.WriteInt(cProto.READY);
            msgBuf.WriteInt(SceneManager.instance.viewMap.LogicMap.curRoleId);*/
            Send(msgBuf);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        private void Send(string msg)
        {
            MessageBuffer msgBuf = new MessageBuffer();
            msgBuf.WriteString(msg);
           if (m_client.Connected)
            {
                m_client.Send(msgBuf);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="cproto"></param>
        private void Send(int cproto)
        {
            MessageBuffer msgBuf = new MessageBuffer();
            msgBuf.WriteInt(cproto);
            if (m_client.Connected)
            {
                m_client.Send(msgBuf);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgBuf"></param>
        private void Send(MessageBuffer msgBuf)
        {
            if (m_client.Connected)
            {
                m_client.Send(msgBuf);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="cproto"></param>
        /// <param name="msg"></param>
        private void Send(int cproto, string msg)
        {
            MessageBuffer msgBuf = new MessageBuffer();
            msgBuf.WriteInt(cproto);
            msgBuf.WriteString(msg);
            if (m_client.Connected)
            {
                m_client.Send(msgBuf);
            }
        }

        public void OnConnect()
        {
            Debug.Log("Connected to server!");
        }

        public void OnDisconnect()
        {
            Debug.Log("Disconnected from server!");
        }

        public void OnMessage(MessageBuffer msg) {

            int cproto = msg.ReadInt();
            Debug.Log(cproto);
            switch (cproto)
            {

            }
        }
    }

}


