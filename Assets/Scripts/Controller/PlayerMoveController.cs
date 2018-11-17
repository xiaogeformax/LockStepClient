using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public delegate void MoveDelegate();
    public class PlayerMoveController : MonoBehaviour
    {
        public static MoveDelegate moveStart;
        public static MoveDelegate moveEnd;

        public static PlayerMoveController instance;

        private JoystickController m_guiJoystackController;
        private Transform m_selfTransform;


        [SerializeField]
        private bool m_turnBase = false;
        private float m_angle;
        [SerializeField]
        private float m_moveSpeed = 5;

        void Awake()
        {
            instance = this;
            m_selfTransform = this.transform;

            moveStart = OnMoveStart;
            moveEnd = OnMoveEnd;
            m_guiJoystackController = JoystickController.instance;
        }

        void Update()
        {
            if (m_guiJoystackController == null)
            {
                return;
            }

            if (m_turnBase)
            {
                Vector3 move = m_guiJoystackController.m_movePosNorm * Time.deltaTime * m_moveSpeed;
                m_selfTransform.localPosition += move;
                //从JoytackController移动方向 算出自身的角度
                m_angle = Mathf.Atan2(m_guiJoystackController.m_movePosNorm.x,
                    m_guiJoystackController.m_movePosNorm.z) * Mathf.Rad2Deg;
                m_selfTransform.localRotation = Quaternion.Euler(Vector3.up * m_angle);
            }
        }

        private void OnMoveEnd()
        {
            m_turnBase = false;
        }

        private void OnMoveStart()
        {
            m_turnBase = true;
        }
    }
}
