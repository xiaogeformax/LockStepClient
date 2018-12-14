using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class JoystickController : MonoBehaviour
    {
        public static JoystickController instance;
        private Vector3 m_origin;
        private Transform m_selfTransform;

        private float m_distance, m_moveMaxDistance = 80, m_activationDistance = 1;

        //偏移矢量
        private Vector2 m_deltaPos;

        //移动方向
        public Vector3 m_movePosNorm { get; private set; }

        void Awake()
        {
            instance = this;

            #region 添加操作响应函数
            //1.拖拽开始响应函数
            EventTriggrListener.GetEventTriggerListener(gameObject).OnDragDelegate = OnDragDelegate;

            //2.拖拽结束响应函数
            EventTriggrListener.GetEventTriggerListener(gameObject).OnDragEndDelegate = OnDragEndDelegate;

            //3.触摸按钮响应函数
            EventTriggrListener.GetEventTriggerListener(gameObject).OnPointDownDelegate = OnPointerDownDelegate;
            #endregion

        }

        /// <summary>
        /// 拖拽
        /// </summary>
        /// <param name="go"></param>
        /// <param name="delta"></param>
        private void OnDragDelegate(GameObject go, Vector2 delta)
        {
            //设置偏移矢量
            m_deltaPos = delta;

           
            //设置移动位置
            m_selfTransform.localPosition += new Vector3(m_deltaPos.x, m_deltaPos.y, 0);
        }

        /// <summary>
        /// 点击响应函数
        /// </summary>
        /// <param name="go">点击的对象</param>
        private void OnPointerDownDelegate(GameObject go)
        {
            //TODO: 
            //响应玩家控制类的对应函数
            if (PlayerMoveController.moveStart != null)
                PlayerMoveController.moveStart();
        }

        /// <summary>
        /// 拖拽结束响应函数
        /// </summary>
        /// <param name="go"></param>
        private void OnDragEndDelegate(GameObject go)
        {
            //回归原点
            m_selfTransform.localPosition = m_origin;

            //响应玩家控制类的对应函数
          /*  if (PlayerMoveController.moveEnd != null)
                PlayerMoveController.moveEnd();*/
        }


        void Start()
        {
            //初始化
            m_origin = transform.localPosition;
            m_selfTransform = this.transform;
        }

        void Update()
        {
            //计算距离，用来判定当前位置和原点的距离，用来做最大距离的限制判断值
            m_distance = Vector3.Distance(m_selfTransform.localPosition, m_origin);

            //限制拖拽的最大移动距离
            if (m_distance >= m_moveMaxDistance)
            {
                //计算在圆上的一个点<公式：（目标点-原点）*半径/原点到目标点的距离了>
                Vector3 point = m_origin + (m_selfTransform.localPosition - m_origin) * m_moveMaxDistance / m_distance;

                //设置当前位置为圆上一点
                m_selfTransform.localPosition = point;
            }


            //判定玩家是否激活摇杆，即滑动距离超过预设值就代表移动
            if (Vector3.Distance(m_selfTransform.localPosition, m_origin) > m_activationDistance)
            {
                //获取移动的方向<移除长度>
                m_movePosNorm = (m_selfTransform.localPosition - m_origin).normalized;
                //设置方向
                m_movePosNorm = new Vector3(m_movePosNorm.x, 0, m_movePosNorm.y);
            }
            else
            {
                //无移动，任一向
                m_movePosNorm = Vector3.zero;
            }
        }
    }
}
