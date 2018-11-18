using Game;
using net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class Main : MonoBehaviour
{

    public string m_ip = "127.0.0.1";
    public int m_tcpPort = 1255;
    public int m_udpPort = 1337;

    public Button btn_connect;
    public Button btn_ready;
    public Button btn_skill;
    public int m_skillId = 1001;

    AsycUdpClient m_client;

    void Awake()
    {
        InvokeRepeating("Tick", 1, 0.02f);
    }

    void Tick()
    {
        TimerHeap.Tick();
        FrameTimerHeap.Tick();
    }

    void Start()
    {
        SceneManager.instance.InitGame(m_ip, m_tcpPort, m_udpPort);
        btn_connect.onClick.RemoveAllListeners();
        btn_connect.onClick.AddListener(SceneManager.instance.viewMap.LogicMap.netManager.Connect);

        btn_ready.onClick.RemoveAllListeners();
        btn_ready.onClick.AddListener(SceneManager.instance.viewMap.LogicMap.netManager.Ready);

        btn_skill.onClick.RemoveAllListeners();
        btn_skill.onClick.AddListener(ClientDoSkill);
    }

    // Update is called once per frame
    void Update()
    {
        SceneManager.instance.Update();
    }

    public void ClientDoSkill()
    {
        SceneManager.instance.viewMap.LogicMap.InputCmd(Cmd.UseSkill, m_skillId.ToString());
    }
    void OnDisable()
    {
        SceneManager.instance.viewMap.LogicMap.netManager.Disconnect();
    }
}
