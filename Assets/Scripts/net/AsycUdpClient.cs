using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace net{

    public class AsycUdpClient
    {
        public static class DebugInfo
        {
            public static bool Data = false;
        }

        static readonly byte m_pingByte = byte.MaxValue; //255


        Stopwatch m_pingWatch; //一个时间间隔的运行时间

        bool Pinging
        {
            get
            {
                return m_pingWatch != null;
            }
        }

        static int m_upByteBuffer, m_downByteBuffer;
        static int m_upByteTotal, m_downByteTotal;

        public static int UpBytes
        {
            get
            {
                int n = m_upByteBuffer;
                m_upByteBuffer = 0;
                return n;
            }
        }

        public static int DownBytes
        {
            get
            {
                int n = m_downByteBuffer;
                m_downByteBuffer = 0;
                return n;
            }
        }

        public static int UpBytesTotal
        {
            get
            {
                return m_upByteTotal;
            }
        }

        public static int DownBytesTotal
        {
            get
            {
                return m_downByteTotal;
            }
        }

        public delegate void ConnectHandle();
        public delegate void MessageHandle(MessageBuffer msg);
        public delegate void DisconnectHandle();
        public delegate void ExceptionHandle(Exception e);
        public delegate void PingHandle(int m);
        public delegate void DebugHandle(string msg);

        public event ConnectHandle OnConnect;
        public event DisconnectHandle OnDisconnect;
        public event MessageHandle OnMessage;
        public event ExceptionHandle OnException;
        public event PingHandle OnPing;
        public event DebugHandle OnDebug;

        int m_myID;

        List<string> m_debugMessageList = new List<string>();

        void Debug(string s) { m_debugMessageList.Add(s); }
        List<MessageBuffer> m_inMessages = new List<MessageBuffer>(), m_outMessages = new List<MessageBuffer>();


        IPEndPoint m_tcpAdress, m_udpAdress;
        TcpClient m_tcpSocket;
        UdpClient m_udpSocket;

        Thread m_receiveThread, m_sendThread, m_aliveThread;

        public bool Connected
        {
            get
            {
                if (m_tcpSocket == null) return false;
                return m_tcpSocket.Connected;
            }
        }

        public AsycUdpClient()
        {
        }

        public void Connect(string ip, int tcpPort, int udpPort)
        {
            if (Connected) return;

            m_tcpAdress = new IPEndPoint(IPAddress.Parse(ip), tcpPort);
            m_udpAdress = new IPEndPoint(IPAddress.Parse(ip), udpPort);

            Thread connect = new Thread(ConnectThread);
            connect.Start();
        }

        public void Update()
        {
            while (m_inMessages.Count > 0)
            {
                while (m_inMessages[0] == null) ;

                OnMessage(m_inMessages[0]);
                m_inMessages.RemoveAt(0);
            }

            string[] debug = m_debugMessageList.ToArray();
            foreach (string s in debug) if (OnDebug != null) OnDebug(s);
            m_debugMessageList.Clear();
        }

        void ConnectThread()
        {
            try
            {
                m_tcpSocket = new TcpClient();
                m_udpSocket = new UdpClient();

                m_tcpSocket.Connect(m_tcpAdress);
                m_udpSocket.Connect(m_udpAdress);

                //Read accepted ID
                byte[] buff = new byte[4];
                NetworkStream stream = m_tcpSocket.GetStream();

                for (int i = 0; i < buff.Length; i++)
                    buff[i] = (byte)stream.ReadByte();

                //Send it back
                m_myID = BitConverter.ToInt32(buff, 0);
                m_udpSocket.Send(buff, 4);

                m_receiveThread = new Thread(ReceiveThread);
                m_sendThread = new Thread(SendThread);
                m_aliveThread = new Thread(AliveThread);

                m_receiveThread.Start();
                m_sendThread.Start();
                m_aliveThread.Start();

                OnConnect();
            }
            catch(Exception e)
            {
                CatchException(e);
                Disconnect();
            }
        }

        void CatchException(Exception e)
        {
            if (OnException != null) OnException(e);
        }


        void ReceiveThread()
        {
            while (this.Connected)
            {
                try
                {
                    IPEndPoint ip = m_udpAdress;
                    byte[] data = m_udpSocket.Receive(ref ip);

                    ReceiveData(data);
                }
                catch (Exception e)
                {
                    CatchException(e);
                }
            }
        }

        void SendThread()
        {
            while (this.Connected)
            {
                for (int i = 0; i < m_outMessages.Count; i++)
                {
                    while (m_outMessages[i] == null) ;

                    if (DebugInfo.Data) Debug("Sent " + m_outMessages[i].Size);

                    m_udpSocket.Send(m_outMessages[i].Array, m_outMessages[i].Size);
                    m_upByteBuffer += m_outMessages[i].Size;
                    m_upByteTotal += m_outMessages[i].Size;
                }

                m_outMessages.Clear();

                Thread.Sleep(1);
            }
        }

        void AliveThread()
        {
            while (Connected)
            {
                try
                {
                    m_tcpSocket.GetStream().Write(new byte[] { 1 }, 0, 1);
                }
                catch (Exception e)
                {
                    CatchException(e);
                }

                Thread.Sleep(1000);
            }

            Disconnect();
        }

        public void Disconnect()
        {
            if (m_tcpSocket == null || m_udpSocket == null) return;

            if (m_tcpSocket.Connected) m_tcpSocket.GetStream().Close();
            m_tcpSocket.Close();
            m_udpSocket.Close();

            m_tcpSocket = null;
            m_udpSocket = null;

            OnDisconnect();
            m_receiveThread.Abort();
            m_sendThread.Abort();
            m_aliveThread.Abort();
        }

        public void Send(MessageBuffer msg)
        {
            m_outMessages.Add(msg);
        }

        void ReceiveData(byte[] data)
        {
            if (DebugInfo.Data) Debug("Received " + data.Length);

            if (data.Length == 1 && data[0] == m_pingByte)
            {
                if (Pinging)
                {
                    if (OnPing!=null)
                    {
                        OnPing(m_pingWatch.Elapsed.Milliseconds);
                    }
                    m_pingWatch = null;
                }
                else
                {
                    m_udpSocket.Send(data, data.Length);
                }
                return;
            }
            m_inMessages.Add(new MessageBuffer(data));

            m_downByteBuffer += data.Length;
            m_downByteTotal += data.Length;

        }

        public void Ping()
        {
            if (!Pinging)
            {
                m_pingWatch = Stopwatch.StartNew();
                m_udpSocket.Send(new byte[] { m_pingByte }, 1);
            }
        }

        public static long Ping(IPEndPoint ip) {
            UdpClient client = new UdpClient();
            client.Connect(ip);

            Stopwatch watch = Stopwatch.StartNew();

            client.Send(new byte[] { m_pingByte }, 1);
            var data = client.Receive(ref ip);

            long millis = watch.Elapsed.Milliseconds;
            watch.Stop();

            client.Close();

            return millis;

        }




    }

  

}
