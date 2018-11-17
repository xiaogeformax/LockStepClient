using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//完工
namespace Game
{
    public class KeyData
    {

        public Cmd m_cmd;
        public string m_data;
        public int m_roleId;

        //构造函数
        public KeyData(Cmd cmd, string data, int roleId)
        {
            this.m_cmd = cmd;
            this.m_data = data;
            this.m_roleId = roleId;
        }

        public KeyData(string dataStr)
        {
            string[] str = dataStr.Split('#');
            this.m_cmd = (Cmd)int.Parse(str[0]);
            this.m_data = str[1];
            this.m_roleId = int.Parse(str[2]);
        }

        public override string ToString()
        {
            int iCmd = (int)m_cmd;
            string str = iCmd.ToString() + "#" + m_data + "#" + m_roleId.ToString();
            return str;
        }
    }

}


public enum Cmd
{
    UseSkill,
    Move,
    Turn
}