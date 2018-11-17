using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class CharData
    {
        public int m_roleId;
        public string m_name;

        public CharData(string str)
        {
            string[] playStr = str.Split('#');
            m_roleId = int.Parse(playStr[0]);
            m_name = playStr[1];
        }

        public override string ToString()
        {
            string str = m_roleId + "#" + m_name;
            return str;
        }
    }
}
