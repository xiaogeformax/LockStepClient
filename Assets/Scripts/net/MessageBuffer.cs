  using System;
using System.Collections.Generic;

namespace net
{
    public class MessageBuffer
    {
        List<byte> m_byteList = new List<byte>();
        int m_cursor = 0;

        public byte[] Array
        {
            get
            {
                return m_byteList.ToArray();
            }
        }

        public int Size
        {
            get
            {
                return m_byteList.Count;
            }
        }

        public MessageBuffer()
        {
        }

        public MessageBuffer(byte[] data)
        {
            m_byteList.AddRange(data);
        }

        public void MoveCursor(int n)
        {
            m_cursor += n;
        }
        public void Reset()
        {
            m_cursor = 0;
        }

        public byte ReadByte()
        {
            byte ret = m_byteList[m_cursor];
            MoveCursor(1);

            return ret;
        }

        public short ReadShort()
        {
            short ret = BitConverter.ToInt16(m_byteList.ToArray(), m_cursor);
            MoveCursor(2);

            return ret;
        }

        public int ReadInt()
        {
            int ret = BitConverter.ToInt32(m_byteList.ToArray(), m_cursor);
            MoveCursor(4);

            return ret;
        }

        public float ReadFloat()
        {
            float ret = BitConverter.ToSingle(m_byteList.ToArray(), m_cursor);
            MoveCursor(4);

            return ret;
        }

        public double ReadDouble()
        {
            double ret = BitConverter.ToDouble(m_byteList.ToArray(), m_cursor);
            MoveCursor(8);

            return ret;
        }

        public string ReadString()
        {
            int len = ReadInt();

            string s = "";
            for (int i = 0; i < len; i++)
                s += (char)ReadByte();

            return s;
        }

        public void WriteByte(short b) { WriteByte((byte)b); }
        public void WriteByte(int b) { WriteByte((byte)b); }
        public void WriteByte(byte b)
        {
            m_byteList.Add(b);
        }

        public void WriteShort(int s) { WriteShort((short)s); }
        public void WriteShort(short s)
        {
            m_byteList.AddRange(BitConverter.GetBytes(s));
        }

        public void WriteInt(int i)
        {
            m_byteList.AddRange(BitConverter.GetBytes(i));
        }

        public void WriteFloat(float f)
        {
            m_byteList.AddRange(BitConverter.GetBytes(f));
        }

        public void WriteDouble(double d)
        {
            m_byteList.AddRange(BitConverter.GetBytes(d));
        }

        public void WriteString(string s)
        {
            WriteInt(s.Length);
            for (int i = 0; i < s.Length; i++)
                WriteByte((byte)s[i]);
        }
    }
}
